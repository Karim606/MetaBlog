using MediatR;
using MetaBlog.Application.Common.Attributes;
using MetaBlog.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Common.Behaviors
{
    public class PerformanceBehavior<TRequest,TResponse>:IPipelineBehavior<TRequest,TResponse> where TRequest: notnull 
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _user;

        public PerformanceBehavior(ILogger<TRequest> logger,IIdentityService identityService,ICurrentUserService user)
        {
            _timer = new Stopwatch();
            _logger = logger;
            _identityService = identityService;
            _user = user;
        }
        public async Task<TResponse> Handle(TRequest request,RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();
            var elapsedMilliseconds = _timer.ElapsedMilliseconds;
            if (elapsedMilliseconds > 500)
            {
                var safeRequest= SafeRequest(request);
                var requestName = typeof(TRequest).Name;
                var userId = _user.GetId();
                var userName = _user.GetUserName();
                _logger.LogWarning("MetaBlog Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                    requestName, elapsedMilliseconds, userId, userName, safeRequest);
            }
            return response;
        }

        private object SafeRequest(TRequest request)
        {
            if (request == null) return null;

            var props = request.GetType().GetProperties();
            var dict = new Dictionary<string, object>();
            foreach (var prop in props)
            {
                var value = prop.GetValue(request);
                var att = prop.GetCustomAttributes(typeof(SensitiveDataAttribute),true).FirstOrDefault() as SensitiveDataAttribute;

                dict[prop.Name] = att != null ? att.Mask : value;
            }
            return dict;
        }
    }
}
