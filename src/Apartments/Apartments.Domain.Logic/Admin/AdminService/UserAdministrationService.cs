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

namespace Apartments.Domain.Logic.Admin.AdminService
{
    public class UserAdministrationService : IUserAdministrationService
    {
        private readonly ApartmentContext _db;
        private readonly IMapper _mapper;

        public UserAdministrationService(ApartmentContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTOAdministration>> GetAllUsersAsync()
        {
            var users = await _db.Users.AsNoTracking().ToListAsync().ConfigureAwait(false);

            return _mapper.Map<IEnumerable<UserDTOAdministration>>(users);
        }

        public Task<Result<UserDTOAdministration>> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }        
        
        public Task<Result> DeleteUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
