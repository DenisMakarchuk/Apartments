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
    /// <summary>
    /// Methods of Administrator work with Identity Users & User Profoles
    /// </summary>
    public class IdentityUserAdministrationService : IIdentityUserAdministrationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserAdministrationService _service;

        public IdentityUserAdministrationService(UserManager<IdentityUser> userManager, IUserAdministrationService service)
        {
            _userManager = userManager;
            _service = service;
        }

        /// <summary>
        /// Get all Identity Users in role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<IdentityUserAdministrationDTO>>> GetAllUsersInRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            if (!users.Any())
            {
                return (Result<IEnumerable<IdentityUserAdministrationDTO>>)Result<IEnumerable<IdentityUserAdministrationDTO>>
                    .NoContent<IEnumerable<IdentityUserAdministrationDTO>>();
            }

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

            return (Result<IEnumerable<IdentityUserAdministrationDTO>>)Result<IEnumerable<IdentityUserAdministrationDTO>>
                .Ok(result as IEnumerable<IdentityUserAdministrationDTO>);
        }

        /// <summary>
        /// Get IdentityUser with User Profile by IdentityId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserAdministrationView>> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .NoContent<UserAdministrationView>();
            }

            IdentityUserAdministrationDTO identityUser = new IdentityUserAdministrationDTO()
            {
                IdentityId = user.Id,
                Email = user.Email
            };

            var profile = await _service.GetUserProfileByIdentityIdAsync(id);

            if (profile.IsError)
            {
                UserAdministrationView failView = new UserAdministrationView()
                {
                    Profile = null,
                    IdentityUser = identityUser
                };

                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .NotOk<UserAdministrationView>(failView, profile.Message);
            }

            UserAdministrationView view = new UserAdministrationView()
            {
                Profile = profile.Data,
                IdentityUser = identityUser
            };

            return (Result<UserAdministrationView>)Result<UserAdministrationView>
                .Ok(view);
        }

        /// <summary>
        /// Add User to Admin role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserAdministrationView>> AddToAdminAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .NoContent<UserAdministrationView>();
            }

            await _userManager.AddToRoleAsync(user,"Admin");

            IdentityUserAdministrationDTO identityUser = new IdentityUserAdministrationDTO()
            {
                IdentityId = user.Id,
                Email = user.Email
            };

            var profile = await _service.GetUserProfileByIdentityIdAsync(id);

            if (profile.IsError)
            {
                UserAdministrationView failView = new UserAdministrationView()
                {
                    Profile = null,
                    IdentityUser = identityUser
                };

                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .NotOk<UserAdministrationView>(failView, profile.Message);
            }

            UserAdministrationView view = new UserAdministrationView()
            {
                Profile = profile.Data,
                IdentityUser = identityUser
            };

            return (Result<UserAdministrationView>)Result<UserAdministrationView>
                .Ok(view);
        }

        /// <summary>
        /// Remove User from Admin role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserAdministrationView>> AddToUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .NoContent<UserAdministrationView>();
            }

            await _userManager.RemoveFromRoleAsync(user, "Admin");

            IdentityUserAdministrationDTO identityUser = new IdentityUserAdministrationDTO()
            {
                IdentityId = user.Id,
                Email = user.Email
            };

            var profile = await _service.GetUserProfileByIdentityIdAsync(id);

            if (profile.IsError)
            {
                UserAdministrationView failView = new UserAdministrationView()
                {
                    Profile = null,
                    IdentityUser = identityUser
                };

                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .NotOk<UserAdministrationView>(failView, profile.Message);
            }

            UserAdministrationView view = new UserAdministrationView()
            {
                Profile = profile.Data,
                IdentityUser = identityUser
            };

            return (Result<UserAdministrationView>)Result<UserAdministrationView>
                .Ok(view);
        }

        /// <summary>
        /// Delete IdentityUser & User Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> DeleteByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return await Task.FromResult(Result.NoContent());
            }

            var isProfileDeleted = await _service.DeleteUserProfileByIdentityIdAsync(id);

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return await Task.FromResult(Result.Fail(result.Errors
                                                            .Select(x => x.Description)
                                                            .Join("\n")));
            }

            if (isProfileDeleted.IsSuccess)
            {
                return await Task.FromResult(Result.Ok());
            }

            return await Task.FromResult(Result.NotOk(isProfileDeleted.Message));
        }
    }
}
