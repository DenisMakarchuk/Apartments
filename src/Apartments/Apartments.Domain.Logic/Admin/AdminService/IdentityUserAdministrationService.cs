using Apartments.Common;
using Apartments.Domain.Admin.DTO;
using Apartments.Domain.Admin.ViewModel;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using Apartments.Domain.Logic.Options;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly IMapper _mapper;

        public IdentityUserAdministrationService(UserManager<IdentityUser> userManager, 
                                                 IUserAdministrationService service,
                                                 IMapper mapper)
        {
            _userManager = userManager;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Identity Users in role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<IdentityUserAdministrationDTO>>> 
            GetAllUsersInRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            return (Result<IEnumerable<IdentityUserAdministrationDTO>>)Result<IEnumerable<IdentityUserAdministrationDTO>>
                .Ok(_mapper.Map<IEnumerable<IdentityUserAdministrationDTO>>(users));
        }

        /// <summary>
        /// Get IdentityUser with User Profile by IdentityId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserAdministrationView>> 
            GetUserByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .NotOk<UserAdministrationView>(null, "User is not exist");
            }

            var identityUser = _mapper.Map<IdentityUserAdministrationDTO>(user);

            var profile = await _service.GetUserProfileByIdentityIdAsync(id, cancellationToken);

            if (profile.IsError || !profile.IsSuccess)
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
        public async Task<Result<UserAdministrationView>> 
            AddToAdminAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .NotOk<UserAdministrationView>(null, "User is not exist");
            }

            await _userManager.AddToRoleAsync(user,"Admin");

            var identityUser = _mapper.Map<IdentityUserAdministrationDTO>(user);

            var profile = await _service.GetUserProfileByIdentityIdAsync(id, cancellationToken);

            if (profile.IsError || !profile.IsSuccess)
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
        public async Task<Result<UserAdministrationView>> 
            AddToUserAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return (Result<UserAdministrationView>)Result<UserAdministrationView>
                    .NotOk<UserAdministrationView>(null, "User is not exist");
            }

            await _userManager.RemoveFromRoleAsync(user, "Admin");

            var identityUser = _mapper.Map<IdentityUserAdministrationDTO>(user);

            var profile = await _service.GetUserProfileByIdentityIdAsync(id, cancellationToken);

            if (profile.IsError || !profile.IsSuccess)
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
        public async Task<Result> 
            DeleteByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return await Task.FromResult(Result.NotOk("User is not exist"));
            }

            var deleteProfileResult = await _service.DeleteUserProfileByIdentityIdAsync(id, cancellationToken);

            if (deleteProfileResult.IsError)
            {
                return await Task.FromResult(Result.Fail(deleteProfileResult.Message));
            }

            var deleteIdentityResult = await _userManager.DeleteAsync(user);

            if (!deleteIdentityResult.Succeeded)
            {
                return await Task.FromResult(Result.Fail(deleteIdentityResult.Errors
                                                            .Select(x => x.Description)
                                                            .Join("\n")));
            }

            if (deleteProfileResult.IsSuccess)
            {
                return await Task.FromResult(Result.Ok());
            }

            return await Task.FromResult(Result.NotOk(deleteProfileResult.Message));
        }
    }
}
