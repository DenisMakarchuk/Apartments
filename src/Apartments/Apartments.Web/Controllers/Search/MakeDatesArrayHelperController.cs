using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartments.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Web.Controllers.Search
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakeDatesArrayHelperController : ControllerBase
    {
        /// <summary>
        /// Get all Apartments by Parameters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("makeDatesArray")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DateTime>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LogAttribute]
        public async Task<IActionResult> GetDatesArray([FromBody] List<DateTime> dates)
        {
            if (dates is null || !dates.Any())
            {
                return (IActionResult)Ok(dates);
            }

            if ((dates.First().Date == dates.Last().Date))
            {
                return (IActionResult)Ok(dates.First().Date);
            }

            List<DateTime> resultDates = new List<DateTime>();


            for (DateTime i = dates.First().Date; i <= dates.Last().Date; i = i.AddDays(1))
            {
                resultDates.Add(i);
            }

            return (IActionResult)Ok(resultDates);
        }
    }
}