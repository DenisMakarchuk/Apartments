using Apartments.Common;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using Apartments.Domain.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// Methods of User work with Orders
    /// </summary>
    public interface IOrderUserService
    {
        /// <summary>
        /// Order formation
        /// </summary>
        /// <param name="order"></param>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<OrderDTO>>
            FormationOrderAsync(AddOrder order, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Add Order to the DataBase
        /// </summary>
        /// <param name="order"></param>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<OrderView>> 
            CreateOrderAsync(AddOrder order, string customerId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get all own Orders by User Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedResponse<OrderView>>> 
            GetAllOrdersByCustomerIdAsync(string customerId, PagedRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get all Orders by Apartment Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedResponse<OrderDTO>>> 
            GetAllOrdersByApartmentIdAsync(PagedRequest<string> request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get Order by Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<OrderView>> 
            GetOrderByIdAsync(string orderId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update Order in DataBase
        /// </summary>
        /// <param name="order"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<OrderView>> 
            UpdateOrderAsync(OrderDTO order, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete own Order by Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> 
            DeleteOrderByIdAsync(string orderId, string customerId, CancellationToken cancellationToken = default(CancellationToken));
    }
}