using Apartments.Common;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using Apartments.Domain.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    public interface IOrderUserService
    {
        Task<Result<OrderView>> CreateOrderAsync(AddOrder order);
        Task<Result<IEnumerable<OrderView>>> GetAllOrdersByUserIdAsync(string userId);
        Task<Result<IEnumerable<OrderView>>> GetAllOrdersByApartmentIdAsync(string apartmentId);
        Task<Result<OrderView>> GetOrderByIdAsync(string orderId);
        Task<Result<OrderView>> UpdateOrderAsync(OrderView order);
        Task<Result> DeleteOrderByIdAsync(string id);
    }
}
