using Apartments.Common;
using Apartments.Data.Context;
using Apartments.Domain.Logic.Images.ImageInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Jpeg;

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
                var directiryPathMini = $"Resources\\ApartmentImages\\{apartmentId}Mini";

                var fullDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), directiryPath);
                var fullDirectoryPathMini = Path.Combine(Directory.GetCurrentDirectory(), directiryPathMini);

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

                DirectoryInfo dirInfoMini = new DirectoryInfo(fullDirectoryPathMini);
                if (!dirInfoMini.Exists)
                {
                    dirInfoMini.Create();
                }

                var fullPathMini = Path.Combine(fullDirectoryPathMini, fileName);
                using (Image image = Image.Load(file.OpenReadStream()))
                {
                    if (image.Width < image.Height)
                    {
                        image.Mutate(x => x
                             .Crop(new Rectangle(0, (image.Height - image.Width) / 2, image.Width, image.Width)).Resize(300, 300));
                    }
                    else
                    {
                        image.Mutate(x => x
                             .Crop(new Rectangle((image.Width - image.Height)/2, 0, image.Height, image.Height)).Resize(300, 300));
                    }

                    var bits = new FileStream(fullPathMini, FileMode.Create);
                    image.Save(bits, new JpegEncoder());
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
