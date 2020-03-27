using Apartments.Common;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    public interface IApartmentUserService
    {
        Task<Result<ApartmentView>> CreateApartmentAsync(AddApartment apartment);
        Task<Result<IEnumerable<ApartmentView>>> GetAllApartmentByUserIdAsync(string userId);
        Task<Result<ApartmentView>> GetApartmentByIdAsync(string apartmentId);
        Task<Result<ApartmentView>> UpdateApartmentAsync(ApartmentView apartment);
        Task<Result> DeleteApartmentByIdAsync(string id);
    }
}
