﻿using Apartments.Common;
using Apartments.Data.Context;
using Apartments.Data.DataModels;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserService
{
    /// <summary>
    /// Methods of User work with own profile
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ApartmentContext _db;
        private readonly IMapper _mapper;

        public UserService(ApartmentContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Create User profile with identityId
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>        
        [LogAttribute]
        public async Task<Result<UserDTO>> CreateUserProfileAsync(string identityId)
        {
            AddUser newProfile = new AddUser()
            {
                Id = identityId
            };

            var addedUser = _mapper.Map<User>(newProfile);

            _db.Users.Add(addedUser);

            try
            {
                await _db.SaveChangesAsync();

                User userAfterAdding = await _db.Users.Where(_ => _.Id == addedUser.Id)
                    .Select(_ => _)
                    .AsNoTracking().FirstOrDefaultAsync();

                return (Result<UserDTO>)Result<UserDTO>
                    .Ok(_mapper.Map<UserDTO>(userAfterAdding));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return (Result<UserDTO>)Result<UserDTO>
                    .Fail<UserDTO>($"Cannot save model. {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return (Result<UserDTO>)Result<UserDTO>
                    .Fail<UserDTO>($"Cannot save model. {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                return (Result<UserDTO>)Result<UserDTO>
                    .Fail<UserDTO>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get User profile by identityId. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserDTO>> GetUserProfileByIdentityIdAsync(string identityId)
        {
            Guid id = Guid.Parse(identityId);

            try
            {
                var user = await _db.Users.Where(_ => _.Id == id).AsNoTracking().FirstOrDefaultAsync();

                if (user is null)
                {
                    return (Result<UserDTO>)Result<UserDTO>
                        .NoContent<UserDTO>();

                }

                return (Result<UserDTO>)Result<UserDTO>
                    .Ok(_mapper.Map<UserDTO>(user));
            }
            catch (ArgumentNullException ex)
            {
                return (Result<UserDTO>)Result<UserDTO>
                    .Fail<UserDTO>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Delete User by identityId. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> DeleteUserProfileByIdentityIdAsync(string identityId)
        {
            Guid id = Guid.Parse(identityId);

            var user = await _db.Users.IgnoreQueryFilters().FirstOrDefaultAsync(_ => _.Id == id);

            if (user is null)
            {
                return await Task.FromResult(Result.NoContent());
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
