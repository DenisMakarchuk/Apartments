using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Common;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Admin
{
    /// <summary>
    /// Administrator work with Users
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdministrationController : ControllerBase
    {
        private readonly IIdentityUserAdministrationService _service;
        RoleManager<IdentityRole> _roleManager;

        public UserAdministrationController(IIdentityUserAdministrationService service, 
                                                RoleManager<IdentityRole> roleManager)
        {
            _service = service;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Get all Identity Users in role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getusers/{role}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> GetAllUsersInRoleAsync(string role)
        {
            if (role == null)
            {
                return BadRequest("Invalid role");
            }

            try
            {
                var result = await _service.GetAllUsersInRoleAsync(role);

                return result.IsError ? NotFound(result.Message)
                    : result.IsSuccess ? (IActionResult)Ok(result.Data)
                    : NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get IdentityUser with User Profile by IdentityId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var _))
            {
                return BadRequest("Invalid id");
            }

            try
            {
                var result = await _service.GetUserByIdAsync(id);

                return result.IsError ? NotFound(result.Message)
                    : result.Data == null ? NoContent()
                    : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Add User to Admin role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("makeadmit/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> ChangeRoleToAdminAsync(string id)
        {
            if (!Guid.TryParse(id, out var _))
            {
                return BadRequest("Invalid id");
            }

            try
            {
                var result = await _service.AddToAdminAsync(id);

                return result.IsError ? NotFound(result.Message)
                    : result.Data == null ? NoContent()
                    : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Remove User from Admin role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("makeuser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> ChangeRoleToUserAsync(string id)
        {
            if (!Guid.TryParse(id, out var _))
            {
                return BadRequest("Invalid id");
            }

            try
            {
                var result = await _service.AddToUserAsync(id);

                return result.IsError ? NotFound(result.Message)
                    : result.Data == null ? NoContent()
                    : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete IdentityUser & User Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> DeleteUserByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var _))
            {
                return BadRequest("Invalid id");
            }

            try
            {
                var result = await _service.DeleteByIdAsync(id);

                return result.IsError ? NotFound(result.Message)
                    : result.Message == null ? NoContent()
                    : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}