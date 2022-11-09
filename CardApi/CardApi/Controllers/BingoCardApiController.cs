using Microsoft.AspNetCore.Mvc;

namespace CardApi.Controllers
{
    ///<summary>
    ///Bingo card API base controller with methods to generate action results.
    ///</summary>
    public class BingoCardApiController : Controller
    {
        /// <summary>
        /// Generates an action result based on a service response with data.
        /// </summary>
        /// <typeparam name="T"> The type of data that is contained in the Actionresult</typeparam>
        /// <param name="serviceresponse"> The response that the serivice gave.</param>
        /// <returns>
        /// The generated Actionresult and status code.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        protected IActionResult GetActionResult<T>(ServiceResponse<T> serviceresponse) where T : class
        {
            if (serviceresponse is null)
            {
                throw new ArgumentNullException(nameof(serviceresponse));
            }

            return serviceresponse.ServiceResultCode switch
            {
                ServiceResultCode.Ok => Ok(serviceresponse.Data),
                ServiceResultCode.NotFound => NotFound(),
                ServiceResultCode.BadRequest => BadRequest(serviceresponse.Message),
                _ => throw new InvalidOperationException("Server Error: Unexpected service response")
            };
        }

        /// <summary>
        ///Generates an action result based on a service response without data.
        /// </summary>
        /// <param name="serviceresponse">The response that the serivice gave.</param>
        /// <returns>
        /// The generated Actionresult and status code.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        protected IActionResult GetActionResult(ServiceResponse serviceresponse)
        {
            if (serviceresponse is null)
            {
                throw new ArgumentNullException(nameof(serviceresponse));
            }

            return serviceresponse.ServiceResultCode switch
            {
                ServiceResultCode.Ok => NoContent(),
                ServiceResultCode.NotFound => NotFound(),
                ServiceResultCode.BadRequest => BadRequest(serviceresponse.Message),
                _ => throw new InvalidOperationException("Server Error: Unexpected service response")
            };
        }
    }
}
