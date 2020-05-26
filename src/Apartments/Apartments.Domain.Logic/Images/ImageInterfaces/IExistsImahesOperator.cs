using Apartments.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Images.ImageInterfaces
{
    public interface IExistsImahesOperator
    {
        /// <summary>
        /// Check directory and get image
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Result<string> GetImage(string imageName, string apartmentId);

        /// <summary>
        /// Check directory and get FileInfo[]
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Result<List<string>> GetImagesInfo(string apartmentId);

        /// <summary>
        /// Remove file from directory by directory and file name
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Result DeleteImageByName(string imageName, string apartmentId, string ownerId);
    }
}
