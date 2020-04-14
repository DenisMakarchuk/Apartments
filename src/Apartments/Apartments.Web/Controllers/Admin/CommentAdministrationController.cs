using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apartments.Common;
using Apartments.Domain.Admin.DTO;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Admin
{
    /// <summary>
    /// Administrator work with User Comments
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentAdministrationController : ControllerBase
    {
        private readonly ICommentAdministrationService _service;

        public CommentAdministrationController(ICommentAdministrationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all User Comments from the DB by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            GetAllCommentsByUserIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Guid.TryParse(userId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetAllCommentsByUserIdAsync(userId, cancellationToken);

                return result.IsError ? NotFound(result.Message) 
                    : result.IsSuccess ? (IActionResult)Ok(result.Data) 
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
        /// <param name="cancellationToken"></param>
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
                    : result.IsSuccess ? (IActionResult)Ok(result.Data) 
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
        /// <param name="cancellationToken"></param>
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
                    : result.IsSuccess ? (IActionResult)Ok(result.Data)
                    : NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update the comment
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            UpdateCommentAsync([FromBody, CustomizeValidator]  CommentDTOAdministration comment,
                                CancellationToken cancellationToken = default(CancellationToken))
        {
            if (comment is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

            try
            {
                var result = await _service.DeleteCommentByIdAsync(commentId, cancellationToken);

                return result.IsError ? NotFound(result.Message)
                    : result.IsSuccess ? (IActionResult)Ok(result.IsSuccess)
                    : NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}