using Apartments.Common;
using Apartments.Domain.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    public interface IIdentityUserService
    {
        Task<Result<UserViewModel>> RegisterAsync(string email, string password);

        Task<Result<UserViewModel>> LoginAsync(string email, string password);

        Task<Result> DeleteAsync(string email, string password);
    }
}
