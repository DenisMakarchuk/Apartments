using Apartments.Domain;
using Apartments.Domain.Logic.Admin.AdminService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Apartments.Logic.Tests.AdminTests
{
    public class AdminOwnMessageService_Tests
    {
        AdminOwnMessageService _service = new AdminOwnMessageService();

        Message _message = new Message()
        {
            Id = new Guid().ToString(),
            AthorId = new Guid().ToString(),
            DestinationId = new Guid().ToString(),
            Title = "Title",
            Text = "Text"
        };

        [Fact]
        public async Task SendMessageAsyncTest()
        {
            var result = await _service.SendMessageAsync(_message);

        }
    }
}
