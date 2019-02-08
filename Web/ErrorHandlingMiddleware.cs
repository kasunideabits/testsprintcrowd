namespace SprintCrowd.Backend.Web
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using SprintCrowd.Backend.Application;
    using SprintCrowdBackEnd.Application;

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
            Application.ApplicationException applicationException = exception as Application.ApplicationException;
            ResponseObject responseObject = new ResponseObject{
                StatusCode = applicationException.ErrorCode,
                ErrorDescription = exception.Message.ToString()
            };
            response.ContentType = "application/json";
            response.StatusCode = (int)applicationException.ErrorCode;
            await response.WriteAsync(JsonConvert.SerializeObject(responseObject));
        }
    }
}