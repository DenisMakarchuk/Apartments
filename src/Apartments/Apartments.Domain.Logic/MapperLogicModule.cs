using Apartments.Data.DataModels;
using Apartments.Domain.Admin.DTO;
using Apartments.Domain.Search.DTO;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Logic
{
    public class MapperLogicModule : Profile
    {
        public MapperLogicModule()
        {
            CreateMap<UserDTOAdministration, User>().ReverseMap();

            CreateMap<CommentDTOAdministration, Comment>().ReverseMap();

            CreateMap<AddApartment, Apartment>();
            CreateMap<ApartmentDTO, Apartment>().ReverseMap();

            CreateMap<AddAddress, Address>();
            CreateMap<AddressDTO, Address>().ReverseMap();

            CreateMap<CountryDTO, Country>().ReverseMap();

            CreateMap<CommentDTO, Comment>().ReverseMap();
            CreateMap<AddComment, Comment>();

            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<AddUser, User>().ReverseMap();

            CreateMap<AddOrder, Order>()
            .ForMember(_ => _.Dates, _ => _.Ignore()).ReverseMap();

            CreateMap<OrderDTO, Order>()
            .ForMember(_ => _.Dates, _ => _.Ignore())
            .ReverseMap()
            .ForMember(_ => _.Dates, _ => _.Ignore()).ReverseMap();

            CreateMap<Apartment, ApartmentSearchDTO>().ReverseMap();

            CreateMap<Address, AddressSearchDTO>().ReverseMap();

            CreateMap<Country, CountrySearchDTO>().ReverseMap();
        }
    }
}
