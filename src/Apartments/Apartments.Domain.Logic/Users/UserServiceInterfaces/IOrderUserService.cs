using Apartments.Common;
using Apartments.Domain.User.AddDTO;
using Apartments.Domain.User.DTO;
using Apartments.Domain.User.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    public interface IOrderUserService
    {
        Task<Result<OrderView>> CreateOrder(AddOrder order);
        Task<Result<IEnumerable<OrderView>>> GetAllOrdersByUserId(string userId);
        Task<Result<IEnumerable<OrderView>>> GetAllOrdersByApartmentId(string apartmentId);
        Task<Result<OrderView>> GetOrderById(string orderId);
        Task<Result<OrderView>> UpdateOrder(OrderView order);
        Task<Result> DeleteOrderById(string id);
    }
}
