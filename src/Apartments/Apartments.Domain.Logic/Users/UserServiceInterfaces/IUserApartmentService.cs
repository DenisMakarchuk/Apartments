using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// The servuce for the User to work with own Apartments
    /// </summary>
    public interface IUserApartmentService
    {
        /// <summary>
        /// Create new User Apartment
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        Task<Result<Apartment>> CreateApartmentAsync(AddApartment apartment);

        /// <summary>
        /// Get all User Apartments
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Apartment>> GetAllApartmentsAsync();

        /// <summary>
        /// Get User Apartment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Apartment> GetApartmentByIdAsync(string id);

        /// <summary>
        /// Update the User Apartment
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        Task<Result<Apartment>> UpdateApartmentAsync(Apartment apartment);

        /// <summary>
        /// Delete User Apartment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteApartmentByIdAsync(string id);
    }
}
