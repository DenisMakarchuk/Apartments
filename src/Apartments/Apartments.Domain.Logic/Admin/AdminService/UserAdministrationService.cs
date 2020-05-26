using Apartments.Common;
using Apartments.Data.Context;
using Apartments.Domain.Admin.DTO;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using AutoMapper;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Apartments.Data.DataModels;

namespace Apartments.Domain.Logic.Admin.AdminService
{
    /// <summary>
    /// Methods of Administrator work with User Profiles
    /// </summary>
    public class UserAdministrationService : IUserAdministrationService
    {
        private readonly ApartmentContext _db;
        private readonly IMapper _mapper;

        public UserAdministrationService(ApartmentContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get User profile by Id. Id must be verified to convert to Guid at the web level 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserDTOAdministration>> 
            GetUserProfileByIdentityIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid identityId = Guid.Parse(id);

            try
            {
                var profile = await _db.Users.Where(_ => _.Id == identityId)
                    .AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (profile is null)
                {
                    return (Result<UserDTOAdministration>)Result<UserDTOAdministration>
                        .NotOk<UserDTOAdministration>(null, "Profile is not exist");

                }

                return (Result<UserDTOAdministration>)Result<UserDTOAdministration>
                    .Ok(_mapper.Map<UserDTOAdministration>(profile));
            }
            catch (ArgumentNullException ex)
            {
                return (Result<UserDTOAdministration>)Result<UserDTOAdministration>
                    .Fail<UserDTOAdministration>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Delete User by User Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> 
            DeleteUserProfileByIdentityIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid identityId = Guid.Parse(id);

            var profile = await _db.Users.IgnoreQueryFilters()
                    .Include(_ => _.Apartments).FirstOrDefaultAsync(_ => _.Id == identityId, cancellationToken);


            if (profile is null)
            {
                return await Task.FromResult(Result.NotOk("Profile is not exist"));
            }

            try
            {
                //todo: rewrite, because there is no time to write as it should
                _db.Apartments.RemoveRange(profile.Apartments);
                await _db.SaveChangesAsync(cancellationToken);

                return await Task.FromResult(Result.Ok());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete profile. {ex.Message}"));
            }
            catch (DbUpdateException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete profile. {ex.Message}"));
            }
        }
    }
}