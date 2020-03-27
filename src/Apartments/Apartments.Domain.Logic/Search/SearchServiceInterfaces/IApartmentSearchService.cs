using Apartments.Common;
using Apartments.Domain.Search.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Search.SearchServiceInterfaces
{
    public interface IApartmentSearchService
    {
        Task<Result<IEnumerable<ApartmentSearchView>>> GetAllApartments();
        Task<Result<ApartmentSearchView>> GetApartmentById(string apartmentId);
    }
}
