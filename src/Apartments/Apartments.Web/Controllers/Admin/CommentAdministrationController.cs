using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Domain.Admin.DTO;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentAdministrationController : ControllerBase
    {
        private readonly ICommentAdministrationService _service;

        public CommentAdministrationController(ICommentAdministrationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetAllCommentsByUserIdAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetAllCommentsByUserIdAsync(userId);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("apartment/{apartmentId}")]
        public async Task<IActionResult> GetAllCommentsByApartmentIdAsync(string apartmentId)
        {
            if (!Guid.TryParse(apartmentId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetAllCommentsByApartmentIdAsync(apartmentId);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("comment/{commentId}")]
        public async Task<IActionResult> GetCommentByIdAsync(string commentId)
        {
            if (!Guid.TryParse(commentId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetCommentByIdAsync(commentId);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateCommentAsync([FromBody]  CommentDTOAdministration comment)
        {
            if (comment is null || ModelState.IsValid) // todo: validate comment
            {
                return BadRequest(ModelState);
            }

            var result = await _service.UpdateCommentAsync(comment);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        [HttpDelete]
        [Route("{commentId}")]
        public async Task<IActionResult> DeleteCommentByIdAsync(string commentId)
        {
            if (!Guid.TryParse(commentId, out var _))
            {
                return BadRequest();
            }

            var result = await _service.DeleteCommentByIdAsync(commentId);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}