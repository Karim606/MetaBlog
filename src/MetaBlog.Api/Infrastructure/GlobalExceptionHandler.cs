using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MetaBlog.Api.Infrastructure
{
    public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService,ILogger<GlobalExceptionHandler>logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("An unhandled exception has occurred while executing the request: {error}.",exception);
            httpContext.Response.StatusCode = 500;
             var problem= new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Type = exception.GetType().Name,
                    Title = "An application error",
                    Detail = "Contact support if problem persists.",
                }
            };
            return await problemDetailsService.TryWriteAsync(problem);
            
        }
    }
}
