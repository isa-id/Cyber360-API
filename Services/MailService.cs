using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendPasswordResetCode(string email, string nombre, string codigo)
        {
            var from = _config["MailSettings:From"];
            var smtp = _config["MailSettings:Smtp"];
            var port = int.Parse(_config["MailSettings:Port"]!);
            var user = _config["MailSettings:User"];
            var pass = _config["MailSettings:Password"];

            using var client = new SmtpClient(smtp, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };

            var message = new MailMessage(from!, email)
            {
                Subject = "Código de recuperación - Cyber360",
                Body = $"Hola {nombre},\n\nTu código de recuperación es: {codigo}\nEste código vence en 10 minutos.",
                IsBodyHtml = false
            };

            await client.SendMailAsync(message);
        }
    }
}

