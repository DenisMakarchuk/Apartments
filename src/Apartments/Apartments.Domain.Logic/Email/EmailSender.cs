using Apartments.Domain.Users;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Diagnostics;
using Apartments.Common;

namespace Apartments.Domain.Logic.Email
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; }

        [LogAttribute]
        public async Task<Result> SendEmailAsync(string email, 
                                   string subject, 
                                   string message, 
                                   CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Execute(Options.MailGunKey, subject, message, email, cancellationToken);
        }

        public async Task<Result> Execute(string apiKey, 
                            string subject, 
                            string message, 
                            string email, 
                            CancellationToken cancellationToken = default(CancellationToken))
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", 
                    Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes("api:" + apiKey)));

            var form = new Dictionary<string, string>();
            form["from"] = "hiedmant@gmail.com";
            form["to"] = email;
            form["subject"] = subject;
            form["html"] = message;

            var response = await client.PostAsync(Options.MailGunDomen + "/messages", 
                                                   new FormUrlEncodedContent(form),
                                                   cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Result.Ok();
            }
            else
            {
                return Result.Fail($"{response.StatusCode.ToString()}");
            }
        }
    }
}
