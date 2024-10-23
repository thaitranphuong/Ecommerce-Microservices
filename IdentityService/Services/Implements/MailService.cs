using IdentityService.Models.DTOs;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using System;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace IdentityService.Services.Implements
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task Send(MailDto mailDto)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(mailDto.From));
                email.To.Add(MailboxAddress.Parse(mailDto.To));
                email.Subject = mailDto.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = mailDto.Body };

                using var smtp = new SmtpClient();
                smtp.Connect(_configuration["Mail:HostName"], 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration["Mail:Email"], _configuration["Mail:AppPassword"]);
                smtp.Send(email);
                smtp.Disconnect(true);
                Console.WriteLine("Sended email");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error to send email: " + ex.ToString());
            }

            return Task.CompletedTask;
        }
    }
}
