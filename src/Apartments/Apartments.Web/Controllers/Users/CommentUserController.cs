using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentUserController : ControllerBase
    {
        private readonly ICommentUserService _service;

        public CommentUserController(ICommentUserService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateCommentAsync([FromBody]AddComment comment)
        {
            if (comment is null || ModelState.IsValid) // todo: validate comment
            {
                return BadRequest(ModelState);
            }

            var result = await _service.CreateCommentAsync(comment);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
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
        public async Task<IActionResult> UpdateCommentAsync([FromBody] CommentDTO comment)
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