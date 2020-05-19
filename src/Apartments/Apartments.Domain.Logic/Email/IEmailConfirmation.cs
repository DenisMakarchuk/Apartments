using Apartments.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Email
{
    public interface IEmailConfirmation
    {
        Task<Result> MakeConfirmMessage(IdentityUser user, string callBackUrl, CancellationToken cancellationToken = default(CancellationToken));
    }
}
