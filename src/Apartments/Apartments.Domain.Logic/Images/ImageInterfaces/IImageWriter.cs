using Apartments.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Images.ImageInterfaces
{
    public interface IImageWriter
    {
        /// <summary>
        /// Method to check/create directory and write file onto the disk
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        Task<Result<string>> UploadImageAsync(IFormFile file, string apartmentId, string ownerId);
    }
}
