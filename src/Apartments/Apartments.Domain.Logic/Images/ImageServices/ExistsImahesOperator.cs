using Apartments.Common;
using Apartments.Domain.Logic.Images.ImageInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Apartments.Data.Context;
using System.Linq;

namespace Apartments.Domain.Logic.Images.ImageServices
{
    public class ExistsImahesOperator : IExistsImahesOperator
    {
        private readonly ApartmentContext _db;

        public ExistsImahesOperator(ApartmentContext context)
        {
            _db = context;
        }

        private Result IsExists (string directory, string imageName)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directory);
                if (!dirInfo.Exists)
                {
                    return Result.NotOk("This apartment doesn't have images!");
                }

                var fullPath = Path.Combine(directory, imageName);

                FileInfo fileInfo = new FileInfo(fullPath);
                if (!fileInfo.Exists)
                {
                    return Result.NotOk("Image is not exists!");
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Check directory and get image
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public Result<string> GetImage(string imageName, string apartmentId)
        {
            var directiryPath = $"Resources\\ApartmentImages\\{apartmentId}";
            var fullDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), directiryPath);

            var path = Path.Combine(directiryPath, imageName);

            var checkDirectory = IsExists(fullDirectoryPath, imageName);
            if (!checkDirectory.IsSuccess)
            {
                if (checkDirectory.IsError)
                {
                    return Result<string>.Fail<string>(checkDirectory.Message);
                }

                return Result<string>.NotOk<string>(null, checkDirectory.Message);
            }

            return Result<string>.Ok<string>(path);
        }

        /// <summary>
        /// Check directory and get FileInfo[]
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public Result<List<string>> GetImagesInfo(string apartmentId)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),
                                            $"Resources\\ApartmentImages\\{apartmentId}");

                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    return Result<List<string>>.NotOk<List<string>>(null, "This apartment doesn't have images!");
                }

                var info = dirInfo.GetFiles();

                List<string> result = new List<string>();

                foreach (var item in info)
                {
                    result.Add(item.Name);
                }

                return Result<List<string>>.Ok<List<string>>(result);
            }
            catch (Exception ex)
            {
                return Result<List<string>>.Fail<List<string>>(ex.Message);
            }
        }

        /// <summary>
        /// Remove file from directory by directory and file name
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public Result DeleteImageByName(string imageName, string apartmentId, string ownerId)
        {
            if (_db.Apartments.Where(_ => _.Id.ToString().Equals(apartmentId))
                  .Where(_ => _.OwnerId.ToString().Equals(ownerId))
                  .FirstOrDefault() is null)
            {
                return Result<string>.NotOk<string>(null, "Apartment is not exists or you are not owner");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(),
                                        $"Resources\\ApartmentImages\\{apartmentId}");

            var fullPath = Path.Combine(path, imageName);

            var pathMini = Path.Combine(Directory.GetCurrentDirectory(),
                            $"Resources\\ApartmentImages\\{apartmentId}Mini");

            var fullPathMini = Path.Combine(pathMini, imageName);

            var checkDirectory = IsExists(path, imageName);
            var checkDirectoryMini = IsExists(pathMini, imageName);

            if (!checkDirectory.IsSuccess || !checkDirectoryMini.IsSuccess)
            {
                if (checkDirectory.IsError || checkDirectoryMini.IsError)
                {
                    return Result.Fail(checkDirectory?.Message + checkDirectoryMini?.Message);
                }

                return Result.NotOk(checkDirectory?.Message + checkDirectoryMini?.Message);
            }

            File.Delete(fullPath);
            File.Delete(fullPathMini);

            return Result.Ok();
        }
    }
}
