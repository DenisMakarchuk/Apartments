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
    /// Methods of Administrator work with Users
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
        /// Get User by User Id. Id must be verified to convert to Guid at the web level 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserDTOAdministration>> GetUserProfileByIdentityIdAsync(string id)
        {
            try
            {
                var profile = await _db.Users.Where(_ => _.IdentityId == id).AsNoTracking().FirstOrDefaultAsync();

                if (profile is null)
                {
                    return (Result<UserDTOAdministration>)Result<UserDTOAdministration>
                        .Fail<UserDTOAdministration>($"User was not found");

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
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> DeleteUserProfileByIdentityIdAsync(string id)
        {
            var profile = await _db.Users.IgnoreQueryFilters().FirstOrDefaultAsync(_=>_.IdentityId == id);

            if (profile is null)
            {
                return await Task.FromResult(Result.Fail("Not found"));
            }

            try
            {
                _db.Users.Remove(profile);
                await _db.SaveChangesAsync();

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