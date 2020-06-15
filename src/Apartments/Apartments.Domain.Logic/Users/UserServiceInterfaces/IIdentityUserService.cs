using Apartments.Common;
using Apartments.Domain.Users;
using Apartments.Domain.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// Methods of User work with own profile & Identity
    /// </summary>
    public interface IIdentityUserService
    {
        /// <summary>
        /// Create User profile & Identity User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> 
            RegisterAsync(UserRegistrationRequest request,
                          CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Email confirmation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Result> ConfirmEmail(string userId, string token);

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<UserViewModel>> 
            LoginAsync(string name, string password, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete User own profile & Identity User
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> 
            DeleteAsync(string name, string password, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Send email for reset password
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result>
            ForgotPasswordAsync(ForgotPasswordModel request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Reser password and send email about it
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result>
            ResetPasswordAsync(ResetPasswordModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task<Result<UserViewModel>>
            ExchangeRefreshToken(string accessToken, string refrashToken, CancellationToken cancellationToken = default(CancellationToken));
    }
}