using Apartments.Common;
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
using System.Threading;
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
        /// <param name="identityId"></param>
        /// <param name="nick"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserDTO>> 
            CreateUserProfileAsync(string identityId, string nick, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddUser newProfile = new AddUser()
            {
                Id = identityId,
                NickName = nick
            };

            var addedUser = _mapper.Map<User>(newProfile);

            _db.Users.Add(addedUser);

            try
            {
                await _db.SaveChangesAsync(cancellationToken);

                User userAfterAdding = await _db.Users.Where(_ => _.Id == addedUser.Id)
                    .Select(_ => _)
                    .AsNoTracking().FirstOrDefaultAsync(cancellationToken);

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
        /// <param name="identityId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<UserDTO>> 
            GetUserProfileByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(identityId);

            try
            {
                var user = await _db.Users.Where(_ => _.Id == id).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (user is null)
                {
                    return (Result<UserDTO>)Result<UserDTO>
                        .NotOk<UserDTO>(null, "Profile is not exist");

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
        /// <param name="identityId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> 
            DeleteUserProfileByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(identityId);

            var user = await _db.Users.IgnoreQueryFilters()
                                .Include(_=>_.Apartments).FirstOrDefaultAsync(_ => _.Id == id);

            if (user is null)
            {
                return await Task.FromResult(Result.NotOk("Profile is not exist"));
            }

            try
            {
                //todo: rewrite, because there is no time to write as it should
                _db.Apartments.RemoveRange(user.Apartments);
                await _db.SaveChangesAsync(cancellationToken);

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

        public async Task<Result> AddRefrashTokenAsync(string refrashToken, Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                _db.RefreshTokens.Add(new RefreshToken(refrashToken, DateTime.UtcNow.AddDays(5), id));
                await _db.SaveChangesAsync();

                return await Task.FromResult(Result.Ok());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot save changes. {ex.Message}"));
            }
            catch (DbUpdateException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot save changes. {ex.Message}"));
            }
        }

        public async Task<Result> DeleteRefreshTokenAsync(string refrashToken, Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var user = await _db.Users.Where(_ => _.Id == id).Include(_=>_.RefreshTokens).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (user.RefreshTokens.Any(rt => rt.Token == refrashToken && rt.Active))
                {
                    _db.RefreshTokens.Remove(user.RefreshTokens.First(t => t.Token == refrashToken));
                    await _db.SaveChangesAsync();

                    return await Task.FromResult(Result.Ok());
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot save changes. {ex.Message}"));
            }
            catch (DbUpdateException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot save changes. {ex.Message}"));
            }

            return await Task.FromResult(Result.Fail($"Invalid refrash token"));
        }
    }
}