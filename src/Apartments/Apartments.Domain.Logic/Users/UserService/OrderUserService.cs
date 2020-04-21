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
using System.Threading;
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
            var notFreeDates = await _db.Apartments.AsNoTracking()
                                                   .Where(_ => _.Id == apartmentId)
                                                   .Select(_ => _.Dates
                                                        .Select(_=>_.Date)
                                                        .ToList())
                                                   .FirstOrDefaultAsync();

            foreach (var item in dates)
            {
                if (notFreeDates.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Make OrderView
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [LogAttribute]
        private OrderView MakeViewModel(Order order)
        {
            OrderView view = new OrderView()
            {
                Order = _mapper.Map<OrderDTO>(order),
                Apartment = _mapper.Map<ApartmentDTO>(order.Apartment),
                Address = _mapper.Map<AddressDTO>(order.Apartment?.Address),
                Country = _mapper.Map<CountryDTO>(order.Apartment?.Address?.Country)
            };

            List<DateTime> notFreeDates = new List<DateTime>();

            foreach (var date in (order.Dates as IEnumerable<BusyDate>))
            {
                notFreeDates.Add(date.Date.Value);
            }

            view.Order.Dates = notFreeDates;

            return view;
        }

        private decimal MakeTotalCoast(decimal coastByDay, IEnumerable<DateTime> dates)
        {
            decimal totalCoast = 0m;

            foreach (var item in dates)
            {
                totalCoast += coastByDay;
            }

            return totalCoast;
        }

        private List<BusyDate> MakeListBusyDates(IEnumerable<DateTime> dates, Guid apartmentId)
        {
            List<BusyDate> busyDates = new List<BusyDate>();

            foreach (var item in dates)
            {
                BusyDate date = new BusyDate()
                {
                    ApartmentId = apartmentId,
                    Date = item.Date
                };

                busyDates.Add(date);
            }

            return busyDates;
        }

        /// <summary>
        /// Add Order to the DataBase
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<OrderView>> 
            CreateOrderAsync(AddOrder order, string customerId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var addedOrder = _mapper.Map<Order>(order);
            addedOrder.CustomerId = Guid.Parse(customerId);

            if (!await IsApartmentFree(order.Dates, addedOrder.ApartmentId.Value))
            {
                return (Result<OrderView>)Result<OrderView>
                    .NotOk<OrderView>(null, $"Cannot add order. Dates are not free!");
            };

            List<BusyDate> busyDates = MakeListBusyDates(order.Dates, addedOrder.ApartmentId.Value);

            decimal coastByDay = _db.Apartments.Where(_ => _.Id == addedOrder.ApartmentId.Value)
                                               .FirstOrDefault()
                                               .Price.Value;

            decimal totalCoast = MakeTotalCoast(coastByDay, order.Dates);

            addedOrder.Dates = _mapper.Map<HashSet<BusyDate>>(busyDates);
            addedOrder.TotalCoast = totalCoast;

            _db.Orders.Add(addedOrder);

            try
            {
                await _db.SaveChangesAsync(cancellationToken);

                Order orderAfterAdding = await _db.Orders
                    .Where(_ => _.CustomerId == addedOrder.CustomerId)
                    .Where(_=>_.Dates
                        .Where(_=>_.Date == order.Dates.First())
                        .FirstOrDefault() != null)
                    .Include(_ => _.Dates)
                    .Include(_ => _.Apartment)
                    .Include(_ => _.Apartment.Address)
                    .Include(_ => _.Apartment.Address.Country)
                    .AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                var view = MakeViewModel(orderAfterAdding);

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
        /// Get all own Orders by User Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<OrderView>>> 
            GetAllOrdersByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(customerId);

            try
            {
                var orders = await _db.Orders.Where(_ => _.CustomerId == id)
                    .Include(_=>_.Dates)
                    .Include(_=>_.Apartment)
                    .Include(_ => _.Apartment.Address)
                    .Include(_ => _.Apartment.Address.Country)
                    .AsNoTracking().ToListAsync(cancellationToken);

                List<OrderView> result = new List<OrderView>();

                foreach (var item in orders)
                {
                    var view = MakeViewModel(item);

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
        /// Get all Orders by Apartment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<OrderDTO>>> 
            GetAllOrdersByApartmentIdAsync(string apartmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(apartmentId);

            try
            {
                var orders = await _db.Orders.Where(_ => _.ApartmentId == id)
                    .Include(_ => _.Dates)
                    .AsNoTracking().ToListAsync(cancellationToken);

                var result = _mapper.Map<IEnumerable<OrderDTO>>(orders) as List<OrderDTO>;

                foreach (var item in result)
                {
                    List<DateTime> notFreeDates = new List<DateTime>();

                    var dates = orders.Where(_ => _.Id == Guid.Parse(item.Id))
                        .Select(_=>_.Dates)
                        .FirstOrDefault();

                    foreach (var date in dates)
                    {
                        notFreeDates.Add(date.Date.Value);
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
        /// Get Order by Order Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<OrderView>> 
            GetOrderByIdAsync(string orderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(orderId);

            try
            {
                var order = await _db.Orders.Where(_ => _.Id == id)
                    .Include(_ => _.Dates)
                    .Include(_ => _.Apartment)
                    .Include(_ => _.Apartment.Address)
                    .Include(_ => _.Apartment.Address.Country)
                    .AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (order is null)
                {
                    return (Result<OrderView>)Result<OrderView>
                        .NotOk<OrderView>(null, "Order is not exist");
                }

                var view = MakeViewModel(order);

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
        public async Task<Result<OrderView>> 
            UpdateOrderAsync(OrderDTO order, CancellationToken cancellationToken = default(CancellationToken))
        {
            var updatedOrder = _mapper.Map<Order>(order);
            var oldOrder = _db.Orders.Where(_ => _.Id == updatedOrder.Id).ToList().FirstOrDefault();

            if (oldOrder == null)
            {
                return (Result<OrderView>)Result<OrderView>
                    .NotOk<OrderView>(null, "No order for update!");
            };

            try
            {
                _db.Orders.Remove(oldOrder);
                await _db.SaveChangesAsync(cancellationToken);

                if (!await IsApartmentFree(order.Dates, updatedOrder.ApartmentId.Value))
                {
                    _db.Orders.Add(oldOrder);
                    await _db.SaveChangesAsync(cancellationToken);

                    return (Result<OrderView>)Result<OrderView>
                        .NotOk<OrderView>(null, "Cannot update order. Dates are not free!");
                };

                List<BusyDate> busyDates = MakeListBusyDates(order.Dates, updatedOrder.ApartmentId.Value);

                decimal coastByDay = _db.Apartments.Where(_ => _.Id == updatedOrder.ApartmentId.Value)
                                   .FirstOrDefault()
                                   .Price.Value;

                decimal totalCoast = MakeTotalCoast(coastByDay, order.Dates);

                updatedOrder.Dates = _mapper.Map<HashSet<BusyDate>>(busyDates);
                updatedOrder.TotalCoast = totalCoast;
                updatedOrder.Update = DateTime.Now;

                _db.Orders.Add(updatedOrder);
                await _db.SaveChangesAsync(cancellationToken);

                Order orderAfterUpdating = await _db.Orders
                    .Where(_ => _.Id == updatedOrder.Id)
                    .Include(_ => _.Dates)
                    .Include(_ => _.Apartment)
                    .Include(_ => _.Apartment.Address)
                    .Include(_ => _.Apartment.Address.Country)
                    .AsNoTracking().FirstOrDefaultAsync();

                var view = MakeViewModel(orderAfterUpdating);

                return (Result<OrderView>)Result<OrderView>
                    .Ok(view);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Cannot save model. {ex.InnerException.Message}");
            }
            catch (DbUpdateException ex)
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Cannot save model. {ex.InnerException.Message}");
            }
            catch (ArgumentNullException ex)
            {
                return (Result<OrderView>)Result<OrderView>
                    .Fail<OrderView>($"Source is null. {ex.InnerException.Message}");
            }

        }

        /// <summary>
        /// Delete own Order by Order Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> 
            DeleteOrderByIdAsync(string orderId, string customerId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(orderId);
            Guid cusId = Guid.Parse(customerId);

            var isOrder = await _db.Orders
                .Where(_ => _.Id == id)
                .Where(_ => _.CustomerId == cusId)
                .IgnoreQueryFilters().AnyAsync();

            if (!isOrder)
            {
                return await Task.FromResult(Result.NotOk("Order was not found or you are not customer"));
            }

            _db.Entry(new Order() { Id = id }).State = EntityState.Deleted;

            try
            {
                await _db.SaveChangesAsync(cancellationToken);

                return await Task.FromResult(Result.Ok());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete Order. {ex.Message}"));
            }
            catch (DbUpdateException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete Order. {ex.InnerException.Message}"));
            }
        }
    }
}