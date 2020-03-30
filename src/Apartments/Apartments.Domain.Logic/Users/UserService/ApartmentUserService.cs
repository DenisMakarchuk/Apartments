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

        /// <summary>
        /// Put Apartment to the DataBase
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        public async Task<Result<ApartmentView>> CreateApartmentAsync(AddApartment apartment)
        {
            var addingApartment = _mapper.Map<Apartment>(apartment);

            _db.Apartments.Add(addingApartment);

            try
            {
                await _db.SaveChangesAsync();

                Apartment addedApartment = await _db.Apartments.Where(_ => _.OwnerId == addingApartment.OwnerId)
                    .Select(_ => _).Include(_ => _.Address.Country).Include(_ => _.Address)
                    .AsNoTracking().FirstOrDefaultAsync();

                ApartmentView view = new ApartmentView()
                {

                    Apartment = _mapper.Map<ApartmentDTO>(addedApartment),

                    Address = _mapper.Map<AddressDTO>(addedApartment.Address),

                    Country = _mapper.Map<CountryDTO>(addedApartment.Address.Country)
                };

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
        /// Get all Apartments by User Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Result<IEnumerable<ApartmentDTO>>> GetAllApartmentByUserIdAsync(string userId)
        {
            Guid ownerId = Guid.Parse(userId);

            try
            {
                var apartments = await _db.Apartments.Where(_ => _.OwnerId == ownerId)
                    .Select(_ => _).Include(_ => _.Address.Country).Include(_ => _.Address)
                    .AsNoTracking().ToListAsync();

                if (!apartments.Any())
                {
                    return (Result<IEnumerable<ApartmentDTO>>)Result<IEnumerable<ApartmentDTO>>
                        .Fail<IEnumerable<ApartmentDTO>>("This User haven't Apartments");
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
        /// Get all Apartment by Apartment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<Result<ApartmentView>> GetApartmentByIdAsync(string apartmentId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update Apartment in DataBase
        /// </summary>
        /// <param name="apartment"></param>
        /// <returns></returns>
        public async Task<Result<ApartmentView>> UpdateApartmentAsync(ApartmentView apartment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete Apartment by Apartment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result> DeleteApartmentByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
