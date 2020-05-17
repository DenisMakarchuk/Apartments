using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Image
{
    public class ImageModel
    {
        public IFormFile Image { get; set; }

        public string ApartmentId { get; set; }
    }
}
