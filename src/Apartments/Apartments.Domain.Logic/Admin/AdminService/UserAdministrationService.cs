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
        /// Get all Users from the database
        /// </summary>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<UserDTOAdministration>>> GetAllUsersAsync()
        {
            var users = await _db.Users.AsNoTracking().ToListAsync();

            if (!users.Any())
            {
                return (Result<IEnumerable<UserDTOAdministration>>)Result<IEnumerable<UserDTOAdministration>>
                    .Fail<IEnumerable<UserDTOAdministration>>("No Users found");
            }

            return (Result<IEnumerable<UserDTOAdministration>>)Result<IEnumerable<UserDTOAdministration>>
                .Ok(_mapper.Map<IEnumerable<UserDTOAdministration>>(users));
        }

        /// <summary>
        /// Get User by User Id. Id must be verified to convert to Guid at the web level 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserDTOAdministration>> GetUserByIdAsync(string id)
        {
            Guid userId = Guid.Parse(id);

            try
            {
                var user = await _db.Users.Where(_ => _.Id == userId).AsNoTracking().FirstOrDefaultAsync();

                if (user is null)
                {
                    return (Result<UserDTOAdministration>)Result<UserDTOAdministration>
                        .Fail<UserDTOAdministration>($"User was not found");

                }

                return (Result<UserDTOAdministration>)Result<UserDTOAdministration>
                    .Ok(_mapper.Map<UserDTOAdministration>(user));
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
        public async Task<Result> DeleteUserByIdAsync(string id)
        {
            Guid userId = Guid.Parse(id);

            var user = await _db.Users.IgnoreQueryFilters().FirstOrDefaultAsync(_=>_.Id == userId);

            if (user is null)
            {
                return await Task.FromResult(Result.Fail("User was not found"));
            }

            try
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

                return await Task.FromResult(Result.Ok());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete User. {ex.Message}"));
            }
            catch (DbUpdateException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete User. {ex.Message}"));
            }
        }
    }
}