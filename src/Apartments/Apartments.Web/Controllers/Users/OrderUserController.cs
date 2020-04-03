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
    public class OrderUserController : ControllerBase
    {
        private readonly IOrderUserService _service;

        public OrderUserController(IOrderUserService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateOrderAsync([FromBody]AddOrder order)
        {
            if (order is null || ModelState.IsValid) // todo: validate order
            {
                return BadRequest(ModelState);
            }

            var result = await _service.CreateOrderAsync(order);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetAllOrdersByUserIdAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetAllOrdersByUserIdAsync(userId);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("apartment/{apartmentId}")]
        public async Task<IActionResult> GetAllOrdersByApartmentIdAsync(string apartmentId)
        {
            if (!Guid.TryParse(apartmentId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetAllOrdersByApartmentIdAsync(apartmentId);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("order/{orderId}")]
        public async Task<IActionResult> GetOrderByIdAsync(string orderId)
        {
            if (!Guid.TryParse(orderId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetOrderByIdAsync(orderId);

                return result.IsError ? NotFound(result.Message) : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateOrderAsync([FromBody] OrderDTO order)
        {
            if (order is null || ModelState.IsValid) // todo: validate order
            {
                return BadRequest(ModelState);
            }

            var result = await _service.UpdateOrderAsync(order);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        [HttpDelete]
        [Route("{orderId}")]
        public async Task<IActionResult> DeleteOrderByIdAsync(string orderId)
        {
            if (!Guid.TryParse(orderId, out var _))
            {
                return BadRequest();
            }

            var result = await _service.DeleteOrderByIdAsync(orderId);

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}