using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

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
        public async Task<IActionResult> RegisterAsync([FromBody]UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest((Result<string>)Result<string>
                            .Fail<string>(ModelState.Values
                                .SelectMany(x => x.Errors
                                    .Select(xx => xx.ErrorMessage))
                                .Join("\n")));
            }

            var result = await _identityService.RegisterAsync(request.Email, request.Password);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        [HttpPost]
        [Route("logIn")]
        public async Task<IActionResult> LoginAsync([FromBody]UserLoginRequest request)
        {
            var result = await _identityService.LoginAsync(request.Email, request.Password);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        [HttpPost]
        [Route("logOut")]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Clear();

            return (IActionResult)Ok(await Task.FromResult(Result.Ok()));
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody]UserLoginRequest request)
        {
            var result = await _identityService.DeleteAsync(request.Email, request.Password);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}