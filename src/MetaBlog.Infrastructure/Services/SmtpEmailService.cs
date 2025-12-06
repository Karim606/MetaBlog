using MetaBlog.Domain.Common.Results;
using MetaBlog.Infrastructure.Common.Interfaces;
using MetaBlog.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MailKit.Net;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

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
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailSettings.FromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = Subject;
            email.Body = new TextPart("plain") { Text = Message };

            try
            {
                using var smtp = new SmtpClient();
                
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP failure sending mail to {email}.", toEmail);
                return Error.Failure();
            }

            return Result.Success;
        }
    }
}
