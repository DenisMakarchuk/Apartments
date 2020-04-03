using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentUserController : ControllerBase
    {
        private readonly IApartmentUserService _service;

        public ApartmentUserController(IApartmentUserService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateApartmentAsync([FromBody]AddApartment apartment)
        {
            if (apartment is null || ModelState.IsValid) // todo: validate apartment
            {
                return BadRequest(ModelState);
            }

            var result = await _service.CreateApartmentAsync(apartment);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetAllApartmentByUserIdAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetAllApartmentByUserIdAsync(userId);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("apartment/{apartmentId}")]
        public async Task<IActionResult> GetApartmentByIdAsync(string apartmentId)
        {
            if (!Guid.TryParse(apartmentId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetApartmentByIdAsync(apartmentId);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateApartmentAsync([FromBody] ApartmentView apartment)
        {
            if (apartment is null || ModelState.IsValid) // todo: validate comment
            {
                return BadRequest(ModelState);
            }

            var result = await _service.UpdateApartmentAsync(apartment);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        [HttpDelete]
        [Route("{apartmentId}")]
        public async Task<IActionResult> DeleteApartmentByIdAsync(string apartmentId)
        {
            if (!Guid.TryParse(apartmentId, out var _))
            {
                return BadRequest();
            }

            var result = await _service.DeleteApartmentByIdAsync(apartmentId);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }

    }
}