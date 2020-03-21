using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// The servuce for the User to work with own Orders
    /// </summary>
    public interface IUserOrderService
    {
        /// <summary>
        /// Create new User Order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<Result<Order>> CreateOrderAsync(AddOrder order);

        /// <summary>
        /// Get all User Orders
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        /// <summary>
        /// Get User Order by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Order> GetOrderByIdAsync(string id);

        /// <summary>
        /// Update the User Order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<Result<Order>> UpdateOrderAsync(Order order);

        /// <summary>
        /// Delete User Order by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteOrderByIdAsync(string id);
    }
}
