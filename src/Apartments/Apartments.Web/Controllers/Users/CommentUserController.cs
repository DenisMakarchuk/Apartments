using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apartments.Common;
using Apartments.Domain.Logic;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Users
{
    /// <summary>
    /// Work with own Comments
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentUserController : ControllerBase
    {
        private readonly ICommentUserService _service;

        public CommentUserController(ICommentUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add Comment to the DB
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            CreateCommentAsync([FromBody, CustomizeValidator]AddComment comment, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (comment is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string authorId = HttpContext.GetUserId();

            try
            {
                var result = await _service.CreateCommentAsync(comment, authorId, cancellationToken);

                return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get all User Comments by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("author")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            GetAllCommentsByAuthorIdAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            string authorId = HttpContext.GetUserId();

            try
            {
                var result = await _service.GetAllCommentsByAuthorIdAsync(authorId, cancellationToken);

                return result.IsError ? NotFound(result.Message)
                    : result.IsSuccess ? (IActionResult)Ok(result)
                    : NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get all Comments by Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("apartment/{apartmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            GetAllCommentsByApartmentIdAsync(string apartmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Guid.TryParse(apartmentId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetAllCommentsByApartmentIdAsync(apartmentId, cancellationToken);

                return result.IsError ? NotFound(result.Message)
                    : result.IsSuccess ? (IActionResult)Ok(result)
                    : NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get Comment by Comment Id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("comment/{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            GetCommentByIdAsync(string commentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Guid.TryParse(commentId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetCommentByIdAsync(commentId, cancellationToken);

                return result.IsError ? NotFound(result.Message)
                    : result.IsSuccess ? (IActionResult)Ok(result)
                    : NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update Comment in DataBase
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            UpdateCommentAsync([FromBody, CustomizeValidator] CommentDTO comment, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (comment is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string authorId = HttpContext.GetUserId();

            if (!comment.AuthorId.Equals(authorId))
            {
                return BadRequest("You are not owner");
            }

            try
            {
                var result = await _service.UpdateCommentAsync(comment, cancellationToken);

                return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete Comment by Comment Id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            DeleteCommentByIdAsync(string commentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Guid.TryParse(commentId, out var _))
            {
                return BadRequest();
            }

            string authorId = HttpContext.GetUserId();

            try
            {
                var result = await _service.DeleteCommentByIdAsync(commentId, authorId, cancellationToken);

                return result.IsError ? NotFound(result.Message)
                    : !result.IsSuccess ? BadRequest(result.Message)
                    : (IActionResult)Ok(result.IsSuccess);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}