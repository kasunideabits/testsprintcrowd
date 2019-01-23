using Microsoft.AspNetCore.Mvc.Filters;
using SprintCrowd.Backend.Enums;
using SprintCrowd.Backend.Logger;

namespace SprintCrowd.Backend.ExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string logMessage = context.Exception != null ?
                context.Exception.StackTrace 
                : "No exception message provided";
            SLogger.Log($"{logMessage}", LogType.Error);
        }
    }
}