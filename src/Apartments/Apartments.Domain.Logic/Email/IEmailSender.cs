using Apartments.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Email
{
    public interface IEmailSender
    {
        Task<Result> SendEmailAsync(string email, 
                            string subject, 
                            string message, 
                            CancellationToken cancellationToken = default(CancellationToken));
    }
}
