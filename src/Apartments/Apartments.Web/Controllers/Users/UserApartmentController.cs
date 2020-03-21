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
    /// The controller for the User to work with own Apartments
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserApartmentController : ControllerBase
    {
        private readonly IUserApartmentService _userApartmentService;

        public UserApartmentController(IUserApartmentService userApartmentService)
        {
            _userApartmentService = userApartmentService;
        }

        /// <summary>
        /// Create new User Apartment
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateApartmentAsync([FromBody] AddApartment apartment)
        {
            if (apartment is null || ModelState.IsValid) // todo: validate apartment
            {
                return BadRequest(ModelState);
            }

            var result = await _userApartmentService.CreateApartmentAsync(apartment); //will return Result<Apartment>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Get all User Apartments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllApartmentsAsync()
        {
            try
            {
                var result = await _userApartmentService.GetAllApartmentsAsync();

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get User Apartment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetApartmentByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _userApartmentService.GetApartmentByIdAsync(id);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update the User Apartment
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateApartmentAsync([FromBody] Apartment apartment)
        {
            if (apartment is null || ModelState.IsValid) // todo: validate user
            {
                return BadRequest(ModelState);
            }

            var result = await _userApartmentService.UpdateApartmentAsync(apartment); //will return Result<Apartment>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Delete User Apartment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteApartmentByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            var result = await _userApartmentService.DeleteApartmentByIdAsync(id); //will return Result

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}