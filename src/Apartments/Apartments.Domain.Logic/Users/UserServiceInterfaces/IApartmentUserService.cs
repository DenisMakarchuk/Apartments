using Apartments.Common;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using Apartments.Domain.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// Methods of User work with own Apartments
    /// </summary>
    public interface IApartmentUserService
    {
        /// <summary>
        /// Add Apartment to the DataBase
        /// </summary>
        /// <param name="apartment"></param>
        /// <param name="ownerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ApartmentView>> 
            CreateApartmentAsync(AddApartment apartment, string ownerId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get all Apartments by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedResponse<ApartmentView>>> 
            GetAllApartmentByOwnerIdAsync(string userId, PagedRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get Apartment by Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ApartmentView>> 
            GetApartmentByIdAsync(string apartmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update Apartment in DataBase
        /// </summary>
        /// <param name="apartment"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ApartmentView>> 
            UpdateApartmentAsync(ApartmentView apartment, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete Apartment by Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <param name="ownerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> 
            DeleteApartmentByIdAsync(string apartmentId, string ownerId, CancellationToken cancellationToken = default(CancellationToken));
    }
}