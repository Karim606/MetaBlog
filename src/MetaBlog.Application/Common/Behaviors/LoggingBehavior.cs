using MediatR;
using MetaBlog.Domain.Common.Results.Interface;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Common.Behaviors
{
    internal sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : IResult
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;
            _logger.LogInformation("processing Request {RequestName}.", requestName);

            TResponse response = await next();
            if (response.IsSuccess)
            {
                _logger.LogInformation("Request {RequestName} processed successfully.", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Errors", response.Errors, true))
                {
                    _logger.LogWarning("Request {RequestName} processed with errors.", requestName);
                }

            }
            return response;

        }
    }
}
