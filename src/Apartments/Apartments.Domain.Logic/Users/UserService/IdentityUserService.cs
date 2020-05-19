using Apartments.Common;
using Apartments.Domain.Logic.Email;
using Apartments.Domain.Logic.Options;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserService
{
    /// <summary>
    /// Methods of User work with own profile & Identity
    /// </summary>
    public class IdentityUserService : IIdentityUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailConfirmation _emailConfirmation;

        IUserService _service;

        public IdentityUserService(UserManager<IdentityUser> userManager, 
                                    JwtSettings jwtSettings, 
                                    IUserService service,
                                    RoleManager<IdentityRole> roleManager,
                                    IEmailConfirmation emailConfirmation)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;

            _service = service;
            _roleManager = roleManager;

            _emailConfirmation = emailConfirmation;
        }

        /// <summary>
        /// Genetate token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [LogAttribute]
        private async Task<Result<string>> GenerateAuthanticationResult(IdentityUser user, string nickName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id),
                new Claim("name", nickName)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null)
                {
                    continue;
                }

                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                    {
                        continue;
                    }

                    claims.Add(roleClaim);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (Result<string>)Result<string>.Ok(tokenHandler.WriteToken(token));
        }

        /// <summary>
        /// Create User profile & Identity User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> 
            RegisterAsync(UserRegistrationRequest request,
                          CancellationToken cancellationToken = default(CancellationToken))
        {
            string defaultRole = "User";

            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            var existingEmail = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return (Result)Result.NotOk("User with this Name already exist");
            }
            else if(existingEmail != null)
            {
                return (Result)Result.NotOk("User with this Email already exist");
            }

            var newUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.UserName
            };

            if (!_userManager.Users.Any())
            {
                defaultRole = "Admin";
            }

            var createUser = await _userManager.CreateAsync(newUser, request.Password);

            if (!createUser.Succeeded)
            {
                return (Result)Result.Fail(createUser.Errors
                                                .Select(_ => _.Description)
                                                .Join("\n"));
            }

            await _userManager.AddToRoleAsync(newUser, defaultRole);

            var profile = await _service.CreateUserProfileAsync(newUser.Id, request.NickName, cancellationToken);

            if (profile.IsError)
            {
                return (Result)Result.Fail($"{profile.Message}");
            }

            return await _emailConfirmation.MakeConfirmMessage(newUser, request.CallBackUrl, cancellationToken);
        }

        /// <summary>
        /// Email confirmation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Result> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.NotOk("User with this id is not exists!");
            }

            var decodedTokenBytes = Convert.FromBase64String(token);
            string decodedTokenString = Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedTokenString);
            if (result.Succeeded)
            {
                return Result.Ok();
            }
            else
            {
                return Result.Fail(result.Errors.Select(x => x.Description)
                                            .Join("\n"));
            }
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserViewModel>> 
            LoginAsync(string name, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await _userManager.FindByNameAsync(name);

            if (user == null)
            {
                return (Result<UserViewModel>)Result<UserViewModel>.NotOk<UserViewModel>(null, "User with this Name does not exist");
            }

            var hasUserValidPassvord = await _userManager.CheckPasswordAsync(user, password);

            if (!hasUserValidPassvord)
            {
                return (Result<UserViewModel>)Result<string>.NotOk<UserViewModel>(null, "User/password combination is wrong");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return (Result<UserViewModel>)Result<string>.NotOk<UserViewModel>(null, "Email is not confirmed!");
            }

            var profile = await _service.GetUserProfileByIdentityIdAsync(user.Id, cancellationToken);

            var token = await GenerateAuthanticationResult(user, profile.Data.NickName);

            if (profile.IsError || string.IsNullOrEmpty(token.Data))
            {
                return (Result<UserViewModel>)Result<UserViewModel>.Fail<UserViewModel>($"{profile.Message}\n" +
                    $"or token is null");
            }

            if (!profile.IsSuccess)
            {
                return (Result<UserViewModel>)Result<UserViewModel>.NotOk<UserViewModel>(null, "Profile is not exist");
            }

            UserViewModel result = new UserViewModel()
            {
                Profile = profile.Data,
                Token = token.Data
            };

            return (Result<UserViewModel>)Result<UserViewModel>.Ok(result);
        }

        /// <summary>
        /// Delete User own profile & Identity User
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> 
            DeleteAsync(string name, 
                        string password,
                        CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await _userManager.FindByNameAsync(name);

            if (user == null)
            {
                return await Task.FromResult(Result.NotOk("User is not exist"));
            }

            var hasUserValidPassvord = await _userManager.CheckPasswordAsync(user, password);

            if (!hasUserValidPassvord)
            {
                return await Task.FromResult(Result.NotOk("User/password combination is wrong"));
            }

            var isProfileDeleted = await _service.DeleteUserProfileByIdentityIdAsync(user.Id, cancellationToken);

            if (isProfileDeleted.IsError)
            {
                return await Task.FromResult(Result.Fail(isProfileDeleted.Message));
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return await Task.FromResult(Result.Fail(result.Errors
                                                            .Select(x => x.Description)
                                                            .Join("\n")));
            }

            return await Task.FromResult(Result.Ok());
        }
    }
}