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
    /// The controller for the GlobalAdmin to work with own account
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalAdminProfileController : ControllerBase
    {
        private readonly IGlobalAdminProfileService _globalAdminProfileService;

        public GlobalAdminProfileController(IGlobalAdminProfileService globalAdminProfileService)
        {
            _globalAdminProfileService = globalAdminProfileService;
        }

        /// <summary>
        /// Get GlobalAdmin account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetGlobalAdminAccountByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _globalAdminProfileService.GetGlobalAdminAccountByIdAsync(id);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update the GlobalAdmin account
        /// </summary>
        /// <param name="globalAdmin"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateGlobalAdminAsync([FromBody] GlobalAdmin globalAdmin)
        {
            if (globalAdmin is null || ModelState.IsValid) // todo: validate SubAdminAccount
            {
                return BadRequest(ModelState);
            }

            var result = await _globalAdminProfileService.UpdateGlobalAdminAsync(globalAdmin); //will return Result<GlobalAdmin>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }
    }
}