using Apartments.Common;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// Methods of User work with own Apartments
    /// </summary>
    public interface IApartmentUserService
    {
        /// <summary>
        /// Put Apartment to the DataBase
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        Task<Result<ApartmentView>> CreateApartmentAsync(AddApartment apartment);

        /// <summary>
        /// Get all Apartments by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<ApartmentView>>> GetAllApartmentByUserIdAsync(string userId);

        /// <summary>
        /// Get all Apartment by Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Task<Result<ApartmentView>> GetApartmentByIdAsync(string apartmentId);

        /// <summary>
        /// Update Apartment in DataBase
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        Task<Result<ApartmentView>> UpdateApartmentAsync(ApartmentView apartment);

        /// <summary>
        /// Delete Apartment by Apartment Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteApartmentByIdAsync(string id);
    }
}
