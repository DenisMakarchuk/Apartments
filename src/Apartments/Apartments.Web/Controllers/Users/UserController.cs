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
using Microsoft.EntityFrameworkCore.Internal;
using Apartments.Domain.Users.ViewModels;

namespace Apartments.Web.Controllers.Users
{
    /// <summary>
    /// User work with own profile & Identity
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
        /// Add Identity User & Profile to the DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("registration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

            try
            {
                var result = await _service.RegisterAsync(request.Email, request.Password);

                return result.IsError ? BadRequest(result.Message)
                    : result.IsSuccess ? (IActionResult)Ok(result.Data)
                    : BadRequest(result.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("logIn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> LoginAsync([FromBody]UserLoginRequest request)
        {
            try
            {
                var result = await _service.LoginAsync(request.Email, request.Password);

                return result.IsError ? BadRequest(result.Message)
                    : result.Data == null ? NoContent()
                    : result.IsSuccess ? (IActionResult)Ok(result.Data)
                    : BadRequest(result.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("logOut")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [LogAttribute]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Clear();

            return (IActionResult)Ok(await Task.FromResult(Result.Ok()));
        }

        /// <summary>
        /// Delete account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> DeleteAsync([FromBody]UserLoginRequest request)
        {
            try
            {
                var result = await _service.DeleteAsync(request.Email, request.Password);

                return result.IsError ? BadRequest(result.Message) 
                    : !result.IsSuccess ? BadRequest(result.Message)
                    : (IActionResult)Ok(result.IsSuccess);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}