﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Common;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Apartments.Domain.Logic;
using System.Threading;
using Apartments.Domain.Users.ViewModels;

namespace Apartments.Web.Controllers.Users
{
    /// <summary>
    /// Work with Orders
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/orders")]
    [ApiController]
    public class OrderUserController : ControllerBase
    {
        private readonly IOrderUserService _service;

        public OrderUserController(IOrderUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add Order to the DataBase
        /// </summary>
        /// <param name="order"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("formation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult>
            FormationOrderAsync([FromBody, CustomizeValidator]AddOrder order, CancellationToken cancellationToken = default(CancellationToken))
        {
            //todo: rewrite, because there is no time to write as it should

            if (order is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.FormationOrderAsync(order, cancellationToken);

                return result.IsError ? throw new InvalidOperationException(result.Message)
                    : result.IsSuccess ? (IActionResult)Ok(result.Data)
                    : BadRequest(result.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Add Order to the DataBase
        /// </summary>
        /// <param name="order"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderView))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult>
            CreateOrderAsync([FromBody, CustomizeValidator]AddOrder order, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (order is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string customerId = HttpContext.GetUserId();

            try
            {
                var result = await _service.CreateOrderAsync(order, customerId, cancellationToken);

                return result.IsError ? throw new InvalidOperationException(result.Message)
                    : result.IsSuccess ? (IActionResult)Ok(result.Data)
                    : BadRequest(result.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get all own Orders by User Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("customer/id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<OrderView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            GetAllOrdersByCustomerIdAsync([FromBody] PagedRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            string customerId = HttpContext.GetUserId();

            try
            {
                var result = await _service.GetAllOrdersByCustomerIdAsync(customerId, request, cancellationToken);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : (IActionResult)Ok(result.Data);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get all Orders by Apartment Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("apartment/id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<OrderDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            GetAllOrdersByApartmentIdAsync([FromBody] PagedRequest<string> request, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (request is null || !Guid.TryParse(request.Data, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetAllOrdersByApartmentIdAsync(request, cancellationToken);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : (IActionResult)Ok(result.Data);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get Order by Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderView))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            GetOrderByIdAsync(string orderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Guid.TryParse(orderId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.GetOrderByIdAsync(orderId, cancellationToken);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : result.IsSuccess
                    ? (IActionResult)Ok(result.Data)
                    : NotFound(result.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update Order in DataBase
        /// </summary>
        /// <param name="order"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            UpdateOrderAsync([FromBody, CustomizeValidator] OrderDTO order, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (order is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string customerId = HttpContext.GetUserId();

            if (!order.CustomerId.Equals(customerId))
            {
                return BadRequest("You are not customer");
            }

            try
            {
                var result = await _service.UpdateOrderAsync(order, cancellationToken);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : result.IsSuccess
                    ? (IActionResult)Ok(result.Data)
                    : BadRequest(result.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete own Order by Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> 
            DeleteOrderByIdAsync(string orderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Guid.TryParse(orderId, out var _))
            {
                return BadRequest();
            }

            string customerId = HttpContext.GetUserId();

            try
            {
                var result = await _service.DeleteOrderByIdAsync(orderId, customerId, cancellationToken);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : result.IsSuccess
                    ? (IActionResult)NoContent()
                    : NotFound(result.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}