using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Apartments.Domain.Logic.Admin.AdminServiceIntersaces;
using Apartments.Domain;

namespace Apartments.Web.Controllers.Admin
{
    /// <summary>
    /// The controller for the administrator to work with SubAdmin accounts
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SubAdminAccountController : ControllerBase
    {
        private readonly ISubAdminAccountService _subAdminAccountService;

        public SubAdminAccountController(ISubAdminAccountService subAdminAccountService)
        {
            _subAdminAccountService = subAdminAccountService;
        }

        #region SubAdminAccount

        /// <summary>
        /// Create new SubAdmin account
        /// </summary>
        /// <param name="subAdmin"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateSubAdminAccountAsync([FromBody] SubAdmin subAdmin)
        {
            if (subAdmin is null || ModelState.IsValid) // todo: validate subAdmin
            {
                return BadRequest(ModelState);
            }

            var result = await _subAdminAccountService.CreateSubAdminAccountAsync(subAdmin); //will return Result<SubAdmin>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Get all SubAdmin accounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllSubAdminAccountsAsync()
        {
            try
            {
                var result = await _subAdminAccountService.GetAllSubAdminAccountsAsync();

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get SubAdmin account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetSubAdminAccountByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _subAdminAccountService.GetSubAdminAccountByIdAsync(id);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update the SubAdmin account
        /// </summary>
        /// <param name="subAdmin"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateSubAdminAccountAsync([FromBody] SubAdmin subAdmin)
        {
            if (subAdmin is null || ModelState.IsValid) // todo: validate subAdmin
            {
                return BadRequest(ModelState);
            }

            var result = await _subAdminAccountService.UpdateSubAdminAccountAsync(subAdmin); //will return Result<SubAdmin>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Delete SubAdmin account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteSubAdminAccountByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            var result = await _subAdminAccountService.DeleteSubAdminAccountByIdAsync(id); //will return Result

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }

        #endregion

        #region SubAdminMessage

        /// <summary>
        /// Get all messages that are in the SubAdmin account by account Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("allMessages/{id}")]
        public async Task<IActionResult> GetAllSubAdminMessagesByAccountIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _subAdminAccountService.GetAllSubAdminMessagesByAccountIdAsync(id);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get message that is in the SubAdmin account by message Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("message/{id}")]
        public async Task<IActionResult> GetSubAdminMessageByMessageIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _subAdminAccountService.GetSubAdminMessageByMessageIdAsync(id);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update SubAdmin message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateSubAdminMessageAsync([FromBody] Message message)
        {
            if (message is null || ModelState.IsValid) // todo: validate message
            {
                return BadRequest(ModelState);
            }

            var result = await _subAdminAccountService.UpdateSubAdminMessageAsync(message); //will return Result<Message>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Delete SubAdmin message by message Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteSubAdminMessageByMessageIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            var result = await _subAdminAccountService.DeleteSubAdminMessageByMessageIdAsync(id); //will return Result

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }

        #endregion
    }
}