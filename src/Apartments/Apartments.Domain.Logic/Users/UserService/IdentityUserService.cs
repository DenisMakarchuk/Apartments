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
    public class IdentityUserService : IIdentityUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        IUserService _service;

        public IdentityUserService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, IUserService service)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;

            _service = service;
        }

        private Result<string> GenerateAuthanticationResult(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (Result<string>)Result<string>.Ok(tokenHandler.WriteToken(token));
        }

        public async Task<Result<UserViewModel>> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return (Result<UserViewModel>)Result<UserViewModel>.Fail<UserViewModel>("User with this Emai already exist");
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createUser = await _userManager.CreateAsync(newUser, password);

            if (!createUser.Succeeded)
            {
                return (Result<UserViewModel>)Result<UserViewModel>
                    .Fail<UserViewModel>(createUser.Errors
                                                .Select(_ => _.Description)
                                                .Join("\n"));
            }

            await _userManager.AddToRoleAsync(newUser, "User");

            var profile = await _service.CreateUserProfileAsync(newUser.Id);

            string token = GenerateAuthanticationResult(newUser).Data;

            if (profile.IsError || string.IsNullOrEmpty(token))
            {
                return (Result<UserViewModel>)Result<UserViewModel>.Fail<UserViewModel>($"{profile.Message}\n" +
                    $"or token is null");
            }

            UserViewModel result = new UserViewModel()
            {
                Profile = profile.Data,
                Token = token
            };

            return (Result<UserViewModel>)Result<UserViewModel>.Ok(result);
        }

        public async Task<Result<UserViewModel>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return (Result<UserViewModel>)Result<UserViewModel>.Fail<UserViewModel>("User with this Emai does not exist");
            }

            var hasUserValidPassvord = await _userManager.CheckPasswordAsync(user, password);

            if (!hasUserValidPassvord)
            {
                return (Result<UserViewModel>)Result<string>.Fail<UserViewModel>("User/password combination is wrong");
            }

            var profile = await _service.GetUserProfileByIdentityIdAsync(user.Id);

            string token = GenerateAuthanticationResult(user).Data;

            if (profile.IsError || string.IsNullOrEmpty(token))
            {
                return (Result<UserViewModel>)Result<UserViewModel>.Fail<UserViewModel>($"{profile.Message}\n" +
                    $"or token is null");
            }

            UserViewModel result = new UserViewModel()
            {
                Profile = profile.Data,
                Token = token
            };

            return (Result<UserViewModel>)Result<UserViewModel>.Ok(result);
        }

        public async Task<Result> DeleteAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return await Task.FromResult(Result.Fail("User with this Emai does not exist"));
            }

            var hasUserValidPassvord = await _userManager.CheckPasswordAsync(user, password);

            if (!hasUserValidPassvord)
            {
                return await Task.FromResult(Result.Fail("User/password combination is wrong"));
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
