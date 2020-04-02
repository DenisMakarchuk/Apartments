using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdministrationController : ControllerBase
    {
        private readonly IUserAdministrationService _service;

        public UserAdministrationController(IUserAdministrationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
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

        [HttpGet]
        [Route("{id}")]
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

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            var result = await _service.DeleteUserByIdAsync(id);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}