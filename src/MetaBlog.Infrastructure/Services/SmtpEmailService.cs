using MetaBlog.Domain.Common.Results;
using MetaBlog.Infrastructure.Interfaces;
using MetaBlog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        public SmtpEmailService(IOptions<EmailSettings>emailSettings) { 
            _emailSettings=emailSettings.Value;
        }
        private readonly EmailSettings _emailSettings;
        public Task<Result<Success>> SendAsync(string toEmail,string Subject,string Message)
        {
            throw new NotImplementedException();
        }
    }
}
