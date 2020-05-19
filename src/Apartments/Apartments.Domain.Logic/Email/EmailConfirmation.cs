using Apartments.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Email
{
    public class EmailConfirmation : IEmailConfirmation
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public EmailConfirmation(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<Result> MakeConfirmMessage(IdentityUser user, string callBackUrl, CancellationToken cancellationToken = default(CancellationToken))
        {
            var t = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var token = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(t));

            var emailForSending = user.Email;

            var subject = "Email confirmation";

            var finalUrl = callBackUrl + $"?userId={user.Id}&token={token}";

            var message = $"Please, <a href='{finalUrl}'>CLICK HERE</a> to confirm the email!";

            return await _emailSender.SendEmailAsync(emailForSending, subject, message, cancellationToken);
        }
    }
}
