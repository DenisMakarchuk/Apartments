using Apartments.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Email
{
    public interface IEmailMaker
    {
        Task<Result> MakeConfirmEmailMessageAsync(IdentityUser user, string callBackUrl, CancellationToken cancellationToken = default(CancellationToken));

        Task<Result> MakeConfirmPasswordResetMessageAsync(IdentityUser user, string callBackUrl, CancellationToken cancellationToken = default(CancellationToken));

        Task<Result> JustSendEmailAsync(string email, string message, string callBackUrl = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
