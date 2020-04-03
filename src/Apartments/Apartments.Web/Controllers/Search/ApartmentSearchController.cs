using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Domain.Logic.Search.SearchServiceInterfaces;
using Apartments.Domain.Search.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Search
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentSearchController : ControllerBase
    {
        private readonly IApartmentSearchService _service;

        public ApartmentSearchController(IApartmentSearchService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("apartments")]
        public async Task<IActionResult> GetAllApartmentsAsync([FromBody] SearchParameters search)
        {
            if (search is null)
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetAllApartmentsAsync(search);

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

        [HttpGet]
        [Route("countries")]
        public async Task<IActionResult> GetAllCountriesAsync()
        {
            try
            {
                var result = await _service.GetAllCountriesAsync();

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("country/{countryId}")]
        public async Task<IActionResult> GetCountryByIdAsync(string countryId)
        {
            if (!Guid.TryParse(countryId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetCountryByIdAsync(countryId);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}