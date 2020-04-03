using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUserAsync([FromBody]AddUser user)
        {
            if (user is null || ModelState.IsValid) // todo: validate user
            {
                return BadRequest(ModelState);
            }

            var result = await _service.CreateUserAsync(user);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetUserByIdAsync(userId);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateUserAsync([FromBody]  UserDTO user)
        {
            if (user is null || ModelState.IsValid) // todo: validate user
            {
                return BadRequest(ModelState);
            }

            var result = await _service.UpdateUserAsync(user);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
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