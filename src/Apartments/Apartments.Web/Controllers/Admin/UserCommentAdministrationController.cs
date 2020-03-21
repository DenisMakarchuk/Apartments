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
    /// The controller for the administrator to work with User comments
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserCommentAdministrationController : ControllerBase
    {
        private readonly IUserCommentAdministrationService _userCommentAdministrationService;

        public UserCommentAdministrationController(IUserCommentAdministrationService userCommentAdministrationService)
        {
            _userCommentAdministrationService = userCommentAdministrationService;
        }

        /// <summary>
        /// Post new comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostCommentAsync([FromBody] Comment comment)
        {
            if (comment is null || ModelState.IsValid) // todo: validate comment
            {
                return BadRequest(ModelState);
            }

            var result = await _userCommentAdministrationService.PostCommentAsync(comment); //will return Result<Comment>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Get all comments that are in the User account by account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{ownAccountId}")]
        public async Task<IActionResult> GetAllUserCommentsByAccountIdAsync(string accountId)
        {
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _userCommentAdministrationService.GetAllUserCommentsByAccountIdAsync(accountId);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get comment that is in the User account by comments Id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{messageId}")]
        public async Task<IActionResult> GetUserCommentByCommentIdAsync(string commentId)
        {
            if (string.IsNullOrEmpty(commentId) || !Guid.TryParse(commentId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _userCommentAdministrationService.GetUserCommentByCommentIdAsync(commentId);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update User comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateUserCommentAsync([FromBody] Comment comment)
        {
            if (comment is null || ModelState.IsValid) // todo: validate comment
            {
                return BadRequest(ModelState);
            }

            var result = await _userCommentAdministrationService.UpdateUserCommentAsync(comment); //will return Result<Comment>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Delete User comment by comment Id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{messageId}")]
        public async Task<IActionResult> DeleteUserCommentByCommentIdAsync(string commentId)
        {
            if (string.IsNullOrEmpty(commentId) || !Guid.TryParse(commentId, out var _))
            {
                return BadRequest();
            }

            var result = await _userCommentAdministrationService.DeleteUserCommentByCommentIdAsync(commentId); //will return Result

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}