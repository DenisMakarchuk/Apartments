using Apartments.Common;
using Apartments.Domain.User.AddDTO;
using Apartments.Domain.User.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    public interface IApartmentUserService
    {
        Task<Result<ApartmentView>> CreateApartment(AddApartment apartment);
        Task<Result<IEnumerable<ApartmentView>>> GetAllApartmentByUserId(string userId);
        Task<Result<ApartmentView>> GetApartmentById(string apartmentId);
        Task<Result<ApartmentView>> UpdateApartment(ApartmentView apartment);
        Task<Result> DeleteApartmentById(string id);
    }
}
