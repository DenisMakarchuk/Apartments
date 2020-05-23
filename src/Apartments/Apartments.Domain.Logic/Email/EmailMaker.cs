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
    public class EmailMaker : IEmailMaker
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public EmailMaker(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<Result> JustSendEmailAsync(string email, string message, string callBackUrl = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var subject = "Reset Password";
            string finalMessage;

            if (string.IsNullOrEmpty(callBackUrl))
            {
                finalMessage = message;
            }
            else
            {
                finalMessage = $"<a href='{callBackUrl}'>{message}</a>";
            }

            return await _emailSender.SendEmailAsync(email, subject, finalMessage, cancellationToken);
        }

        public async Task<Result> MakeConfirmEmailMessageAsync(IdentityUser user, string callBackUrl, CancellationToken cancellationToken = default(CancellationToken))
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var codedToken = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(token));

            var emailForSending = user.Email;

            var subject = "Email confirmation";

            var finalUrl = callBackUrl + $"?userId={user.Id}&token={codedToken}";

            var message = $"Please, <a href='{finalUrl}'>CLICK HERE</a> to confirm the email!";

            return await _emailSender.SendEmailAsync(emailForSending, subject, message, cancellationToken);
        }

        public async Task<Result> MakeConfirmPasswordResetMessageAsync(IdentityUser user, string callBackUrl, CancellationToken cancellationToken = default)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var codedToken = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(token));

            var emailForSending = user.Email;

            var subject = "Password reset";

            var finalUrl = callBackUrl + $"?userId={user.Id}&token={codedToken}";

            var message = $"Please, <a href='{finalUrl}'>CLICK HERE</a> to reset the password!";

            return await _emailSender.SendEmailAsync(emailForSending, subject, message, cancellationToken);
        }
    }
}
