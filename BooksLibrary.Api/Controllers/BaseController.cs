using System.Collections.Generic;
using System.Linq;
using CommonData.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BooksLibrary.Api.Controllers
{
    /// <summary>
    /// Represent the base controller
    /// </summary>
    [SwaggerResponse(204, type: typeof(Error))]
    [SwaggerResponse(400, type: typeof(Error))]
    [SwaggerResponse(500, type: typeof(Error))]
    public class BaseController : ControllerBase
    {
        /// <summary>
        ///     Default page size
        /// </summary>
        public const int DefaultPageSize = 5;

        /// <summary>
        ///     Default page number
        /// </summary>
        public const int DefaultPageNumber = 1;

        /// <summary>
        ///     Base respone answer validation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <returns></returns>
        public IActionResult ResolveResponse<T>(T results) where T : class
        {
            return results == null ? NoContent() : (IActionResult) Ok(results);
        }

        /// <summary>
        ///    Base respone answer validation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <returns></returns>
        public IActionResult ResolveResponse<T>(IEnumerable<T> results) where T : class
        {
            return results == null || !results.Any() ? NoContent() : (IActionResult) Ok(results);
        }
    }
}