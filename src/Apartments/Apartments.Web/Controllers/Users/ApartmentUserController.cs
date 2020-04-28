using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Apartments.Common;
using Apartments.Domain.Logic;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.ViewModels;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Users
{
    /// <summary>
    /// Work with own Apartments
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/apartments/")]
    [ApiController]
    public class ApartmentUserController : ControllerBase
    {
        private readonly IApartmentUserService _service;

        public ApartmentUserController(IApartmentUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add Apartment to the DB
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            CreateApartmentAsync([FromBody, CustomizeValidator]AddApartment apartment, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (apartment is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string ownerId = HttpContext.GetUserId();

            try
            {
                var result = await _service.CreateApartmentAsync(apartment, ownerId, cancellationToken);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : (IActionResult)Ok(result.Data);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get all own Apartments by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("owner/id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            GetAllApartmentByOwnerIdAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            string ownerId = HttpContext.GetUserId();

            try
            {
                var result = await _service.GetAllApartmentByOwnerIdAsync(ownerId);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : (IActionResult)Ok(result.Data);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get Apartment by Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{apartmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            GetApartmentByIdAsync(string apartmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Guid.TryParse(apartmentId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetApartmentByIdAsync(apartmentId, cancellationToken);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : result.IsSuccess
                    ? (IActionResult)Ok(result.Data)
                    : NotFound(result.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update own Apartment
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            UpdateApartmentAsync([FromBody, CustomizeValidator] ApartmentView apartment, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (apartment is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string ownerId = HttpContext.GetUserId();

            if (!apartment.Apartment.OwnerId.Equals(ownerId))
            {
                return BadRequest("You are not owner");
            }

            try
            {
                var result = await _service.UpdateApartmentAsync(apartment, cancellationToken);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : (IActionResult)Ok(result.Data);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete Apartment by Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{apartmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            DeleteApartmentByIdAsync(string apartmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Guid.TryParse(apartmentId, out var _))
            {
                return BadRequest("Invalig Id");
            }

            string ownerId = HttpContext.GetUserId();

            try
            {
                var result = await _service.DeleteApartmentByIdAsync(apartmentId, ownerId, cancellationToken);

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