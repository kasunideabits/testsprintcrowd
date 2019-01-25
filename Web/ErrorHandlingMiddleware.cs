namespace SprintCrowd.Backend.Web
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    /// <summary>
    /// OWIN middleware used for global error handling.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorHandlingMiddleware" />.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Invokes the next middleware in pipe and
        /// handles any exceptions.
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <returns>A task representing the async invocation.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(Exception ex)
            {
                await this.HandleException(context, ex);
            }

        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            HttpResponse response = context.Response;
            ApplicationException applicationException = exception as ApplicationException;

            int errorCode = -1;
            int statusCode = 500;
            string message = "Unknown error";


            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            
            ErrorResponse error = new ErrorResponse(errorCode, message);
            await response.WriteAsync(JsonConvert.SerializeObject(error));
        }
    }
}