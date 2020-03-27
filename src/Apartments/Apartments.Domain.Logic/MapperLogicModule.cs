using Apartments.Data.DataModels;
using Apartments.Domain.Admin.DTO;
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
            CreateMap<UserDTOAdministration, User>();
        }
    }
}
