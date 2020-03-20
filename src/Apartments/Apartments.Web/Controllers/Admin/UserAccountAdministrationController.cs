using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using Apartments.Domain;

namespace Apartments.Web.Controllers.Admin
{
    /// <summary>
    /// The controller for the administrator to work with User accounts
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountAdministrationController : ControllerBase
    {
        private readonly IUserAccountAdministrationService _userAccountAdministrationService;

        public UserAccountAdministrationController(IUserAccountAdministrationService userAccountAdministrationService)
        {
            _userAccountAdministrationService = userAccountAdministrationService;
        }

        /// <summary>
        /// Create new User account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        {
            if (user is null || ModelState.IsValid) // todo: validate User
            {
                return BadRequest(ModelState);
            }

            var result = await _userAccountAdministrationService.CreateUserAsync(user); //will return Result<User>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Get all User accounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            try
            {
                var result = await _userAccountAdministrationService.GetAllUserAsync();

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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
                var result = await _userAccountAdministrationService.GetUserByIdAsync(id);

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
            if (user is null || ModelState.IsValid) // todo: validate User
            {
                return BadRequest(ModelState);
            }

            var result = await _userAccountAdministrationService.UpdateUserAsync(user); //will return Result<User>

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

            var result = await _userAccountAdministrationService.DeleteUserByIdAsync(id); //will return Result

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}