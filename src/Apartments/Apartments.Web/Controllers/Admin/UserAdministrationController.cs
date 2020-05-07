using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apartments.Common;
using Apartments.Domain.Admin.DTO;
using Apartments.Domain.Admin.ViewModel;
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
    [Route("api/administration/users")]
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
        [Route("roles/{role}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<IdentityUserAdministrationDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            GetAllUsersInRoleAsync(string role)
        {
            if (role == null)
            {
                return BadRequest("Invalid role");
            }

            try
            {
                var result = await _service.GetAllUsersInRoleAsync(role);

                return (IActionResult)Ok(result.Data);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAdministrationView))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            GetUserByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Guid.TryParse(id, out var _))
            {
                return BadRequest("Invalid id");
            }

            try
            {
                var result = await _service.GetUserByIdAsync(id, cancellationToken);

                return result.Data == null ? NotFound(result.Message)
                      : (IActionResult)Ok(result.Data);
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
        [Route("roles/add/admin/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAdministrationView))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            ChangeRoleAsync([FromBody] List<string> riles, string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Guid.TryParse(id, out var _))
            {
                return BadRequest("Invalid id");
            }

            try
            {
                if (riles.Contains("Admin"))
                {
                    var result = await _service.AddToUserAsync(id, cancellationToken);

                    return result.Data == null ? NotFound(result.Message)
                                               : (IActionResult)Ok(result.Data);
                }
                else
                {
                    var result = await _service.AddToAdminAsync(id, cancellationToken);

                    return result.Data == null ? NotFound(result.Message)
                                               : (IActionResult)Ok(result.Data);
                }

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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            DeleteUserByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Guid.TryParse(id, out var _))
            {
                return BadRequest("Invalid id");
            }

            try
            {
                var result = await _service.DeleteByIdAsync(id, cancellationToken);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : result.IsSuccess
                    ? (IActionResult)NoContent()
                    : NotFound(result.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}