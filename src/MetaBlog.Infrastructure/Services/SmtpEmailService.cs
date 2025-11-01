using MetaBlog.Domain.Common.Results;
using MetaBlog.Infrastructure.Common.Interfaces;
using MetaBlog.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        public SmtpEmailService(IOptions<EmailSettings>emailSettings,ILogger<SmtpEmailService>logger) { 
            _emailSettings=emailSettings.Value;
            _logger = logger;
        }
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<SmtpEmailService> _logger; 
        public async Task<Result<Success>> SendAsync(string toEmail,string Subject,string Message)
        {
            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, "MetaBlog Support"),
                Subject = Subject,
                Body = Message,
                IsBodyHtml = false
            };

            mailMessage.To.Add(toEmail);
            try
            {

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex) {
                _logger.LogError(ex,"smtp server failure in sending mail to {email}.", toEmail);
                return Error.Failure();
            }

            return Result.Success;
        }
    }
}
