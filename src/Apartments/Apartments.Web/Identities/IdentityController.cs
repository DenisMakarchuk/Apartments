using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Identities
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Register([FromBody]UserRegistrationRequest request)
        {
            var result = await _identityService.RegisterAsync(request.Email, request.Password);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }
    }
}