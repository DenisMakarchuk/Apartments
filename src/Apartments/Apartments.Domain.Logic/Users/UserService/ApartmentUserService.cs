using Apartments.Common;
using Apartments.Data.Context;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserService
{
    /// <summary>
    /// Methods of User work with own Apartments
    /// </summary>
    public class ApartmentUserService : IApartmentUserService
    {
        private readonly ApartmentContext _db;
        private readonly IMapper _mapper;

        public ApartmentUserService(ApartmentContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Put Apartment to the DataBase
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        public async Task<Result<ApartmentView>> CreateApartmentAsync(AddApartment apartment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all Apartments by User Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Result<IEnumerable<ApartmentView>>> GetAllApartmentByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all Apartment by Apartment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<Result<ApartmentView>> GetApartmentByIdAsync(string apartmentId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update Apartment in DataBase
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        public async Task<Result<ApartmentView>> UpdateApartmentAsync(ApartmentView apartment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete Apartment by Apartment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result> DeleteApartmentByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
