using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Common;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Apartments.Web.Identities;
using Microsoft.EntityFrameworkCore.Internal;

namespace Apartments.Web.Controllers.Users
{
    /// <summary>
    /// User work with own profile
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IIdentityUserService _service;

        public UserController(IIdentityUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Put User to the DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        [LogAttribute]
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

            var result = await _service.RegisterAsync(request.Email, request.Password);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("logIn")]
        public async Task<IActionResult> LoginAsync([FromBody]UserLoginRequest request)
        {
            var result = await _service.LoginAsync(request.Email, request.Password);

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
            var result = await _service.DeleteAsync(request.Email, request.Password);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}