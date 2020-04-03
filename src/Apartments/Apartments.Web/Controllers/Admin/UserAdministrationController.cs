using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Admin
{
    /// <summary>
    /// Administrator work with Users
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdministrationController : ControllerBase
    {
        private readonly IUserAdministrationService _service;

        public UserAdministrationController(IUserAdministrationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all Users from the DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var result = await _service.GetAllUsersAsync();

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetUserByIdAsync(id);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.DeleteUserByIdAsync(id);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result.IsSuccess);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}