using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Common;
using Apartments.Domain.Image;
using Apartments.Domain.Logic;
using Apartments.Domain.Logic.Images.ImageInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Images
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/apartment-image")]
    [ApiController]
    public class ApartmentImageController : ControllerBase
    {
        private readonly IImageWriter _imageWriter;
        private readonly IExistsImahesOperator _imahesOperator;

        public ApartmentImageController(IImageWriter imageWriter, IExistsImahesOperator imahesOperator)
        {
            _imageWriter = imageWriter;
            _imahesOperator = imahesOperator;
        }

        /// <summary>
        /// Check/create directory and write file onto the disk
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload")]
        [RequestSizeLimit(2048000)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> UploadImage([FromQuery]ImageModel file)
        {
            if (file is null || file.Image.Length == 0 || string.IsNullOrEmpty(file.ApartmentId))
            {
                return BadRequest();
            }

            string ownerId = HttpContext.GetUserId();

            try
            {
                var result = await _imageWriter.UploadImageAsync(file.Image, file.ApartmentId, ownerId);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : !result.IsSuccess
                    ? BadRequest(result.Message)
                    : (IActionResult)Ok(result.Data);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Check directory and get image
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("image/{apartmentId}/{imageName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public IActionResult GetImage(string apartmentId, string imageName)
        {
            if (string.IsNullOrEmpty(imageName) || string.IsNullOrEmpty(apartmentId))
            {
                return BadRequest();
            }

            try
            {
                var result = _imahesOperator.GetImage(imageName, apartmentId);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : !result.IsSuccess
                    ? NotFound(result.Message)
                    : (IActionResult)Ok(result.Data);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Check directory and get FileInfo[]
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{apartmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public IActionResult GetImageNamesList(string apartmentId)
        {
            if (string.IsNullOrEmpty(apartmentId))
            {
                return BadRequest();
            }

            try
            {
                var result = _imahesOperator.GetImagesInfo(apartmentId);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : !result.IsSuccess
                    ? NotFound(result.Message)
                    : (IActionResult)Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Remove file from directory by directory and file name
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("image/{apartmentId}/{imageName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public IActionResult Delete(string apartmentId, string imageName)
        {
            if (string.IsNullOrEmpty(imageName) || string.IsNullOrEmpty(apartmentId))
            {
                return BadRequest();
            }

            string ownerId = HttpContext.GetUserId();

            try
            {
                var result = _imahesOperator.DeleteImageByName(imageName, apartmentId, ownerId);

                return result.IsError
                    ? throw new InvalidOperationException(result.Message)
                    : !result.IsSuccess
                    ? NotFound(result.Message)
                    : (IActionResult)NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}