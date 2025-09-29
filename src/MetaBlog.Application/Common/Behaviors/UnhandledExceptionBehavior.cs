using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MetaBlog.Application.Common.Behaviors
{
    public class UnhandledExceptionBehavior<TRequest,TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            try
            {
                return await next(ct);
            }
            catch(Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                logger.LogError(ex, "Request: Unhandled Exception for Request { Name} { @Request} ", requestName, request);
                throw;
            }
        }
    }
}
