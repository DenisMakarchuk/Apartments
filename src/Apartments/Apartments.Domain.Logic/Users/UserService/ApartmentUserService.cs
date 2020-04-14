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
    /// Methods of User work with own Apartments
    /// </summary>
    public class ApartmentUserService : IApartmentUserService
    {
        private readonly ApartmentContext _db;
        private readonly IMapper _mapper;

        public ApartmentUserService(ApartmentContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        private ApartmentView MakeApartmentView (Apartment apartment)
        {
            ApartmentView view = new ApartmentView()
            {

                Apartment = _mapper.Map<ApartmentDTO>(apartment),

                Address = _mapper.Map<AddressDTO>(apartment.Address),

                Country = _mapper.Map<CountryDTO>(apartment.Address?.Country)
            };

            return view;
        }

        /// <summary>
        /// Add Apartment to the DataBase
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<ApartmentView>> 
            CreateApartmentAsync(AddApartment apartment, string ownerId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var addedApartment = _mapper.Map<Apartment>(apartment);

            addedApartment.OwnerId = Guid.Parse(ownerId);

            _db.Apartments.Add(addedApartment);

            try
            {
                await _db.SaveChangesAsync(cancellationToken);

                Apartment apartmentAfterAdding = await _db.Apartments.Where(_ => _.Title == addedApartment.Title)
                    .Select(_ => _).Include(_ => _.Address.Country).Include(_ => _.Address)
                    .AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                ApartmentView view = MakeApartmentView(apartmentAfterAdding);

                return (Result<ApartmentView>)Result<ApartmentView>
                    .Ok(view);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return (Result<ApartmentView>)Result<ApartmentView>
                    .Fail<ApartmentView>($"Cannot save model. {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return (Result<ApartmentView>)Result<ApartmentView>
                    .Fail<ApartmentView>($"Cannot save model. {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                return (Result<ApartmentView>)Result<ApartmentView>
                    .Fail<ApartmentView>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get all User Apartments by User Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<ApartmentDTO>>> 
            GetAllApartmentByOwnerIdAsync(string ownerId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(ownerId);

            try
            {
                var apartments = await _db.Apartments.Where(_ => _.OwnerId == id)
                    .Select(_ => _)
                    .AsNoTracking().ToListAsync(cancellationToken);

                if (!apartments.Any())
                {
                    return (Result<IEnumerable<ApartmentDTO>>)Result<IEnumerable<ApartmentDTO>>
                        .NoContent<IEnumerable<ApartmentDTO>>();
                }

                return (Result<IEnumerable<ApartmentDTO>>)Result<IEnumerable<ApartmentDTO>>
                    .Ok(_mapper.Map<IEnumerable<ApartmentDTO>>(apartments));
            }
            catch (ArgumentNullException ex)
            {
                return (Result<IEnumerable<ApartmentDTO>>)Result<IEnumerable<ApartmentDTO>>
                    .Fail<IEnumerable<ApartmentDTO>>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get Apartment by Apartment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<ApartmentView>> 
            GetApartmentByIdAsync(string apartmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(apartmentId);

            try
            {
                var apartment = await _db.Apartments.Where(_ => _.Id == id)
                    .Include(_ => _.Address.Country).Include(_ => _.Address)
                    .AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (apartment is null)
                {
                    return (Result<ApartmentView>)Result<ApartmentView>
                        .NoContent<ApartmentView>();
                }

                ApartmentView view = MakeApartmentView(apartment);

                return (Result<ApartmentView>)Result<ApartmentView>
                    .Ok(view);
            }
            catch (ArgumentNullException ex)
            {
                return (Result<ApartmentView>)Result<ApartmentView>
                    .Fail<ApartmentView>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Update Apartment in DataBase
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<ApartmentView>> 
            UpdateApartmentAsync(ApartmentView apartment, CancellationToken cancellationToken = default(CancellationToken))
        {
            apartment.Address.CountryId = apartment.Country.Id;

            Apartment apartmentForUpdate = _mapper.Map<Apartment>(apartment.Apartment);
            Address addressForUpdate = _mapper.Map<Address>(apartment.Address);

            apartmentForUpdate.Update = DateTime.UtcNow;

            _db.Entry(apartmentForUpdate).Property(c => c.Title).IsModified = true;
            _db.Entry(apartmentForUpdate).Property(c => c.Text).IsModified = true;
            _db.Entry(apartmentForUpdate).Property(c => c.Area).IsModified = true;
            _db.Entry(apartmentForUpdate).Property(c => c.IsOpen).IsModified = true;
            _db.Entry(apartmentForUpdate).Property(c => c.Price).IsModified = true;
            _db.Entry(apartmentForUpdate).Property(c => c.NumberOfRooms).IsModified = true;
            _db.Entry(apartmentForUpdate).Property(c => c.Update).IsModified = true;

            _db.Entry(addressForUpdate).Property(c => c.CountryId).IsModified = true;
            _db.Entry(addressForUpdate).Property(c => c.City).IsModified = true;
            _db.Entry(addressForUpdate).Property(c => c.Street).IsModified = true;
            _db.Entry(addressForUpdate).Property(c => c.Home).IsModified = true;
            _db.Entry(addressForUpdate).Property(c => c.NumberOfApartment).IsModified = true;

            try
            {
                await _db.SaveChangesAsync(cancellationToken);

                return (Result<ApartmentView>)Result<ApartmentView>
                    .Ok(apartment);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return (Result<ApartmentView>)Result<ApartmentView>
                    .Fail<ApartmentView>($"Cannot update model. {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return (Result<ApartmentView>)Result<ApartmentView>
                    .Fail<ApartmentView>($"Cannot update model. {ex.Message}");
            }
        }

        /// <summary>
        /// Delete Apartment by Apartment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> 
            DeleteApartmentByIdAsync(string apartmentId, string ownerId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(apartmentId);
            Guid ownId = Guid.Parse(ownerId);

            var apartment = await _db.Apartments
                .Where(_ => _.Id == id)
                .Where(_=>_.OwnerId == ownId)
                .IgnoreQueryFilters().FirstOrDefaultAsync(cancellationToken);

            if (apartment is null)
            {
                return await Task.FromResult(Result.NotOk("Apartment was not found or you are not owner"));
            }

            try
            {
                _db.Apartments.Remove(apartment);
                await _db.SaveChangesAsync(cancellationToken);

                return await Task.FromResult(Result.Ok());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete Apartment. {ex.InnerException.Message}"));
            }
            catch (DbUpdateException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete Apartment. {ex.InnerException.Message}"));
            }
        }
    }
}