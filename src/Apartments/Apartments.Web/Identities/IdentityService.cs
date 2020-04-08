using Apartments.Common;
using Apartments.Web.Options;
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

namespace Apartments.Web.Identities
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
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

        public async Task<Result<string>> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return (Result<string>)Result<string>.Fail<string>("User with this Emai already exist");
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createUser = await _userManager.CreateAsync(newUser, password);

            if (!createUser.Succeeded)
            {
                return (Result<string>)Result<string>
                    .Fail<string>(createUser.Errors
                                                .Select(_ => _.Description)
                                                .Join("\n"));
            }

            return GenerateAuthanticationResult(newUser);
        }

        public async Task<Result<string>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return (Result<string>)Result<string>.Fail<string>("User with this Emai does not exist");
            }

            var hasUserValidPassvord = await _userManager.CheckPasswordAsync(user, password);

            if (!hasUserValidPassvord)
            {
                return (Result<string>)Result<string>.Fail<string>("User/password combination is wrong");
            }

            return GenerateAuthanticationResult(user);
        }
    }
}
