using Apartments.Common;
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

        IUserService _service;

        public IdentityUserService(UserManager<IdentityUser> userManager, 
                                    JwtSettings jwtSettings, 
                                    IUserService service,
                                    RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;

            _service = service;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Genetate token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [LogAttribute]
        private async Task<Result<string>> GenerateAuthanticationResult(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
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
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserViewModel>> RegisterAsync(string email, string password)
        {
            string defaultRole = "User";

            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return (Result<UserViewModel>)Result<UserViewModel>.NotOk<UserViewModel>(null,"User with this Emai already exist");
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            if (!_userManager.Users.Any())
            {
                defaultRole = "Admin";
            }

            var createUser = await _userManager.CreateAsync(newUser, password);

            if (!createUser.Succeeded)
            {
                return (Result<UserViewModel>)Result<UserViewModel>
                    .Fail<UserViewModel>(createUser.Errors
                                                .Select(_ => _.Description)
                                                .Join("\n"));
            }

            await _userManager.AddToRoleAsync(newUser, defaultRole);

            var profile = await _service.CreateUserProfileAsync(newUser.Id);

            var token = await GenerateAuthanticationResult(newUser);

            if (profile.IsError || string.IsNullOrEmpty(token.Data))
            {
                return (Result<UserViewModel>)Result<UserViewModel>.Fail<UserViewModel>($"{profile.Message}\n" +
                    $"or token is null");
            }

            UserViewModel result = new UserViewModel()
            {
                Profile = profile.Data,
                Token = token.Data
            };

            return (Result<UserViewModel>)Result<UserViewModel>.Ok(result);
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserViewModel>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return (Result<UserViewModel>)Result<UserViewModel>.NotOk<UserViewModel>(null, "User with this Emai does not exist");
            }

            var hasUserValidPassvord = await _userManager.CheckPasswordAsync(user, password);

            if (!hasUserValidPassvord)
            {
                return (Result<UserViewModel>)Result<string>.NotOk<UserViewModel>(null, "User/password combination is wrong");
            }

            var profile = await _service.GetUserProfileByIdentityIdAsync(user.Id);

            var token = await GenerateAuthanticationResult(user);

            if (profile.IsError || string.IsNullOrEmpty(token.Data))
            {
                return (Result<UserViewModel>)Result<UserViewModel>.Fail<UserViewModel>($"{profile.Message}\n" +
                    $"or token is null");
            }

            if (!profile.IsSuccess)
            {
                return (Result<UserViewModel>)Result<UserViewModel>.NoContent<UserViewModel>();
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
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> DeleteAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return await Task.FromResult(Result.NoContent());
            }

            var hasUserValidPassvord = await _userManager.CheckPasswordAsync(user, password);

            if (!hasUserValidPassvord)
            {
                return await Task.FromResult(Result.NotOk("User/password combination is wrong"));
            }

            var isProfileDeleted = await _service.DeleteUserProfileByIdentityIdAsync(user.Id);

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
