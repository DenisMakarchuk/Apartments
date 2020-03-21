using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Domain;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Users
{
    /// <summary>
    /// The controller for the User to work with own account
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;

        public UserAccountController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        /// <summary>
        /// Create new User account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUserAsync([FromBody] AddUser user)
        {
            if (user is null || ModelState.IsValid) // todo: validate user
            {
                return BadRequest(ModelState);
            }

            var result = await _userAccountService.CreateUserAsync(user); //will return Result<User>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Get User account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _userAccountService.GetUserByIdAsync(id);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update the User account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] User user)
        {
            if (user is null || ModelState.IsValid) // todo: validate user
            {
                return BadRequest(ModelState);
            }

            var result = await _userAccountService.UpdateUserAsync(user); //will return Result<User>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Delete User account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            var result = await _userAccountService.DeleteUserByIdAsync(id); //will return Result

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}