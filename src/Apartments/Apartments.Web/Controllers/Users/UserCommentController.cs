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
    /// The controller for the User to work with own Comments
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserCommentController : ControllerBase
    {
        private readonly IUserCommentService _userCommentService;

        public UserCommentController(IUserCommentService userCommentService)
        {
            _userCommentService = userCommentService;
        }

        /// <summary>
        /// Create new User Comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateCommentAsync([FromBody] AddComment comment)
        {
            if (comment is null || ModelState.IsValid) // todo: validate Comment
            {
                return BadRequest(ModelState);
            }

            var result = await _userCommentService.CreateCommentAsync(comment); //will return Result<Comment>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Get all User Comments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllCommentsAsync()
        {
            try
            {
                var result = await _userCommentService.GetAllCommentsAsync();

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get User Comment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCommentByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _userCommentService.GetCommentByIdAsync(id);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update the User Comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateCommentAsync([FromBody] Comment comment)
        {
            if (comment is null || ModelState.IsValid) // todo: validate Comment
            {
                return BadRequest(ModelState);
            }

            var result = await _userCommentService.UpdateCommentAsync(comment); //will return Result<Comment>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Delete User Comment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCommentByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            var result = await _userCommentService.DeleteCommentByIdAsync(id); //will return Result

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}