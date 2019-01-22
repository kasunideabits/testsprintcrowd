using Microsoft.AspNetCore.Mvc.Filters;
using SprintCrowdBackEnd.Enums;
using SprintCrowdBackEnd.Logger;

namespace SprintCrowdBackEnd.ExceptionHandler
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