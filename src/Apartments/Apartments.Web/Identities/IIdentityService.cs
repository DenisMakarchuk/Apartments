using Apartments.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Web.Identities
{
    public interface IIdentityService
    {
        Task<Result<string>> RegisterAsync(string email, string password);
    }
}
