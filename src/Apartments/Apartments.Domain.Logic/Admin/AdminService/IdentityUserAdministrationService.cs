using Apartments.Common;
using Apartments.Domain.Admin.DTO;
using Apartments.Domain.Admin.ViewModel;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using Apartments.Domain.Logic.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminService
{
    public class IdentityUserAdministrationService : IIdentityUserAdministrationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        IUserAdministrationService _service;

        public IdentityUserAdministrationService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, IUserAdministrationService service)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;

            _service = service;
        }

        public async Task<Result<IEnumerable<IdentityUserAdministrationDTO>>> GetAllUsersInRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            List<IdentityUserAdministrationDTO> result = new List<IdentityUserAdministrationDTO>();

            foreach (var item in users)
            {
                result.Add(
                    new IdentityUserAdministrationDTO()
                    {
                        IdentityId = item.Id,
                        Email = item.Email
                    });
            }

            if (users == null)
            {
                return (Result<IEnumerable<IdentityUserAdministrationDTO>>)Result<IEnumerable<IdentityUserAdministrationDTO>>
                    .Fail<IEnumerable<IdentityUserAdministrationDTO>>("Not found");
            }

            return (Result<IEnumerable<IdentityUserAdministrationDTO>>)Result<IEnumerable<IdentityUserAdministrationDTO>>
                .Ok(result as IEnumerable<IdentityUserAdministrationDTO>);
        }

        public async Task<Result<UserAdministrationView>> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .Fail<UserAdministrationView>("Not found");
            }

            var profile = await _service.GetUserProfileByIdentityIdAsync(id);

            IdentityUserAdministrationDTO identityUser = new IdentityUserAdministrationDTO()
            {
                IdentityId = user.Id,
                Email = user.Email
            };

            UserAdministrationView view = new UserAdministrationView()
            {
                Profile = profile.Data,
                IdentityUser = identityUser
            };

            return (Result<UserAdministrationView>)Result<UserAdministrationView>
                .Ok(view);
        }

        public async Task<Result<UserAdministrationView>> MakeAdminAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .Fail<UserAdministrationView>("Not found");
            }

            await _userManager.AddToRoleAsync(user,"Admin");

            var profile = await _service.GetUserProfileByIdentityIdAsync(id);

            IdentityUserAdministrationDTO identityUser = new IdentityUserAdministrationDTO()
            {
                IdentityId = user.Id,
                Email = user.Email
            };

            UserAdministrationView view = new UserAdministrationView()
            {
                Profile = profile.Data,
                IdentityUser = identityUser
            };

            return (Result<UserAdministrationView>)Result<UserAdministrationView>
                .Ok(view);
        }

        public async Task<Result<UserAdministrationView>> MakeUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .Fail<UserAdministrationView>("Not found");
            }

            await _userManager.RemoveFromRoleAsync(user, "Admin");

            var profile = await _service.GetUserProfileByIdentityIdAsync(id);

            IdentityUserAdministrationDTO identityUser = new IdentityUserAdministrationDTO()
            {
                IdentityId = user.Id,
                Email = user.Email
            };

            UserAdministrationView view = new UserAdministrationView()
            {
                Profile = profile.Data,
                IdentityUser = identityUser
            };

            return (Result<UserAdministrationView>)Result<UserAdministrationView>
                .Ok(view);
        }

        public async Task<Result> DeleteByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return await Task.FromResult(Result.Fail("Not found"));
            }

            var isProfileDeleted = await _service.DeleteUserProfileByIdentityIdAsync(id);

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return await Task.FromResult(Result.Fail(result.Errors
                                                            .Select(x => x.Description)
                                                            .Join("\n")));
            }

            return await Task.FromResult(Result.Ok(isProfileDeleted.Message));
        }
    }
}
