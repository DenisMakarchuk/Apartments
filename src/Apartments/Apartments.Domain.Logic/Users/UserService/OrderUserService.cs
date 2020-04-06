using Apartments.Common;
using Apartments.Data.Context;
using Apartments.Data.DataModels;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using Apartments.Domain.Users.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserService
{
    /// <summary>
    /// Methods of User work with Orders
    /// </summary>
    public class OrderUserService : IOrderUserService
    {
        private readonly ApartmentContext _db;
        private readonly IMapper _mapper;

        public OrderUserService(ApartmentContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        [LogAttribute]
        private async Task<bool> IsApartmentFree(IEnumerable<DateTime> dates, Guid apartmentId)
        {
            foreach (var item in dates)
            {
                var ordered = await _db.Apartments.Where(_ => _.Id == apartmentId)
                    .Where(_ => _.Dates
                        .Where(_ => _.Date.Date != item.Date).Any()).Select(_=>_.Dates).FirstOrDefaultAsync();

                if (ordered !is null || ordered.Any())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Put Order to the DataBase
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<OrderView>> CreateOrderAsync(AddOrder order)
        {
            var addedOrder = _mapper.Map<Order>(order);

            if (await IsApartmentFree(order.Dates, addedOrder.ApartmentId))
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Cannot save model. Dates are not free!");
            };

            List<BusyDate> busyDates = new List<BusyDate>();

            foreach (var item in order.Dates)
            {
                BusyDate date = new BusyDate()
                {
                    ApartmentId = Guid.Parse(order.ApartmentId),
                    Date = item.Date
                };

                busyDates.Add(date);
            }

            addedOrder.Dates = _mapper.Map<HashSet<BusyDate>>(busyDates);

            _db.Orders.Add(addedOrder);

            try
            {
                await _db.SaveChangesAsync();

                Order orderAfterAdding = await _db.Orders.Where(_ => _.CustomerId == addedOrder.CustomerId)
                    .Select(_ => _)
                    .Include(_ => _.Apartment)
                    .Include(_ => _.Apartment.Address)
                    .Include(_ => _.Apartment.Address.Country)
                    .AsNoTracking().FirstOrDefaultAsync();

                OrderView view = new OrderView()
                {

                    Order = _mapper.Map<OrderDTO>(orderAfterAdding),

                    Apartment = _mapper.Map<ApartmentDTO>(orderAfterAdding.Apartment),

                    Address = _mapper.Map<AddressDTO>(orderAfterAdding.Apartment.Address),

                    Country = _mapper.Map<CountryDTO>(orderAfterAdding.Apartment.Address.Country)
                };

                view.Order.Dates = order.Dates;

                return (Result<OrderView>)Result<OrderView>
                    .Ok(view);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Cannot save model. {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Cannot save model. {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get all own Orders by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<OrderView>>> GetAllOrdersByUserIdAsync(string userId)
        {
            Guid customerId = Guid.Parse(userId);

            try
            {
                var orders = await _db.Orders.Where(_ => _.CustomerId == customerId)
                    .Include(_=>_.Dates)
                    .Include(_=>_.Apartment)
                    .Include(_ => _.Apartment.Address)
                    .Include(_ => _.Apartment.Address.Country)
                    .AsNoTracking().ToListAsync();

                if (!orders.Any())
                {
                    return (Result<IEnumerable<OrderView>>)Result<IEnumerable<OrderView>>
                        .Fail<IEnumerable<OrderView>>("This User haven't Orders");
                }

                List<OrderView> result = new List<OrderView>();

                foreach (var item in orders)
                {
                    OrderView view = new OrderView()
                    {
                        Order = _mapper.Map<OrderDTO>(item),

                        Apartment = _mapper.Map<ApartmentDTO>(item.Apartment),

                        Address = _mapper.Map<AddressDTO>(item.Apartment.Address),

                        Country = _mapper.Map<CountryDTO>(item.Apartment.Address.Country)
                    };

                    List<DateTime> notFreeDates = new List<DateTime>();

                    foreach (var date in item.Dates)
                    {
                        notFreeDates.Add(date.Date);
                    }

                    view.Order.Dates = notFreeDates;

                    result.Add(view);
                }

                return (Result<IEnumerable<OrderView>>)Result<IEnumerable<OrderView>>
                    .Ok(_mapper.Map<IEnumerable<OrderView>>(result));
            }
            catch (ArgumentNullException ex)
            {
                return (Result<IEnumerable<OrderView>>)Result<IEnumerable<OrderView>>
                    .Fail<IEnumerable<OrderView>>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get all Orders by Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<OrderDTO>>> GetAllOrdersByApartmentIdAsync(string apartmentId)
        {
            Guid id = Guid.Parse(apartmentId);

            try
            {
                var orders = await _db.Orders.Where(_ => _.ApartmentId == id)
                    .Include(_ => _.Dates)
                    .AsNoTracking().ToListAsync();

                if (!orders.Any())
                {
                    return (Result<IEnumerable<OrderDTO>>)Result<IEnumerable<OrderDTO>>
                        .Fail<IEnumerable<OrderDTO>>("This Apartment haven't Orders");
                }

                var result = _mapper.Map<IEnumerable<OrderDTO>>(orders) as List<OrderDTO>;

                foreach (var item in result)
                {
                    List<DateTime> notFreeDates = new List<DateTime>();

                    var dates = orders.Where(_ => _.Id == Guid.Parse(item.Id))
                        .Select(_=>_.Dates)
                        .FirstOrDefault();

                    foreach (var date in dates)
                    {
                        notFreeDates.Add(date.Date);
                    }

                    item.Dates = notFreeDates;
                }

                return (Result<IEnumerable<OrderDTO>>)Result<IEnumerable<OrderDTO>>
                    .Ok(result as IEnumerable<OrderDTO>);
            }
            catch (ArgumentNullException ex)
            {
                return (Result<IEnumerable<OrderDTO>>)Result<IEnumerable<OrderDTO>>
                    .Fail<IEnumerable<OrderDTO>>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get Order by Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<OrderView>> GetOrderByIdAsync(string orderId)
        {
            Guid id = Guid.Parse(orderId);

            try
            {
                var order = await _db.Orders.Where(_ => _.Id == id)
                    .Include(_ => _.Dates)
                    .Include(_ => _.Apartment)
                    .Include(_ => _.Apartment.Address)
                    .Include(_ => _.Apartment.Address.Country)
                    .AsNoTracking().FirstOrDefaultAsync();

                if (order is null)
                {
                    return (Result<OrderView>)Result<OrderView>
                        .Fail<OrderView>($"Order was not found");
                }

                OrderView view = new OrderView()
                {
                    Order = _mapper.Map<OrderDTO>(order),

                    Apartment = _mapper.Map<ApartmentDTO>(order.Apartment),

                    Address = _mapper.Map<AddressDTO>(order.Apartment.Address),

                    Country = _mapper.Map<CountryDTO>(order.Apartment.Address.Country)
                };

                List<DateTime> notFreeDates = new List<DateTime>();

                foreach (var date in order.Dates)
                {
                    notFreeDates.Add(date.Date);
                }

                view.Order.Dates = notFreeDates;

                return (Result<OrderView>)Result<OrderView>
                    .Ok(_mapper.Map<OrderView>(view));
            }
            catch (ArgumentNullException ex)
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Update Order in DataBase
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<OrderView>> UpdateOrderAsync(OrderDTO order)
        {
            var updatedOrder = _mapper.Map<Order>(order);

            if (await IsApartmentFree(order.Dates, updatedOrder.ApartmentId))
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Cannot save model. Dates are not free!");
            };

            List<BusyDate> busyDates = new List<BusyDate>();

            foreach (var item in order.Dates)
            {
                BusyDate date = new BusyDate()
                {
                    ApartmentId = Guid.Parse(order.ApartmentId),
                    Date = item.Date
                };
            }

            updatedOrder.Dates = _mapper.Map<HashSet<BusyDate>>(busyDates);
            updatedOrder.Update = DateTime.Now;

            _db.Orders.Update(updatedOrder);

            try
            {
                await _db.SaveChangesAsync();

                Order orderAfterUpdating = await _db.Orders.Where(_ => _.CustomerId == updatedOrder.CustomerId)
                    .Select(_ => _)
                    .Include(_ => _.Apartment)
                    .Include(_ => _.Apartment.Address)
                    .Include(_ => _.Apartment.Address.Country)
                    .AsNoTracking().FirstOrDefaultAsync();

                OrderView view = new OrderView()
                {

                    Order = _mapper.Map<OrderDTO>(orderAfterUpdating),

                    Apartment = _mapper.Map<ApartmentDTO>(orderAfterUpdating.Apartment),

                    Address = _mapper.Map<AddressDTO>(orderAfterUpdating.Apartment.Address),

                    Country = _mapper.Map<CountryDTO>(orderAfterUpdating.Apartment.Address.Country)
                };

                view.Order.Dates = order.Dates;

                return (Result<OrderView>)Result<OrderView>
                    .Ok(view);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Cannot save model. {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Cannot save model. {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Source is null. {ex.Message}");
            }

        }

        /// <summary>
        /// Delete own Order by Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> DeleteOrderByIdAsync(string orderId)
        {
            Guid id = Guid.Parse(orderId);

            var order = await _db.Orders.IgnoreQueryFilters().FirstOrDefaultAsync(_ => _.Id == id);

            if (order is null)
            {
                return await Task.FromResult(Result.Fail("Order was not found"));
            }

            try
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();

                return await Task.FromResult(Result.Ok());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete Order. {ex.Message}"));
            }
            catch (DbUpdateException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete Order. {ex.Message}"));
            }
        }
    }
}
