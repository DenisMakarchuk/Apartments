using Apartments.Common;
using Apartments.Data.Context;
using Apartments.Domain.Logic.Images.ImageInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Images.ImageServices
{
    public class ImageWriter : IImageWriter
    {
        private readonly ApartmentContext _db;

        public ImageWriter(ApartmentContext context)
        {
            _db = context;
        }

        public async Task<Result<string>> UploadImageAsync(IFormFile file, string apartmentId, string ownerId)
        {
            if (CheckIfImageFile(file))
            {
                return await WriteFileAsync(file, apartmentId, ownerId);
            }

            return Result<string>.NotOk<string>(null, "Invalid image file");
        }

        /// <summary>
        /// Method to check if file is image file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool CheckIfImageFile(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return WriterHelper.GetImageFormat(fileBytes) != WriterHelper.ImageFormat.unknown;
        }

        /// <summary>
        /// Method to check/create directory and write file onto the disk
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task<Result<string>> WriteFileAsync(IFormFile file, string apartmentId, string ownerId)
        {
            if (_db.Apartments.Where(_=>_.Id.ToString().Equals(apartmentId))
                              .Where(_=>_.OwnerId.ToString().Equals(ownerId))
                              .FirstOrDefault() is null)
            {
                return Result<string>.NotOk<string>(null, "Apartment is not exists or you are not owner");
            }

            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];

                var fileName = Guid.NewGuid().ToString() + extension;
                var directiryPath = $"Resources\\ApartmentImages\\{apartmentId}";
                var fullDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), directiryPath);

                DirectoryInfo dirInfo = new DirectoryInfo(fullDirectoryPath);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                var fullPath = Path.Combine(fullDirectoryPath, fileName);

                using (var bits = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }

                var pathForRote = Path.Combine(directiryPath, fileName);

                return Result<string>.Ok<string>(pathForRote);
            }
            catch (Exception e)
            {
                return Result<string>.Fail<string>(e.Message);
            }
        }
    }
}
