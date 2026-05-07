using System.Net.Mail;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class EmailSendService : IEmailSendService
    {
        private readonly IConfiguration _configuration;

        public EmailSendService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Response.ProtocolResponse Handle(Request.EmailSend request)
        {
            var smtpHost = _configuration.GetSection("Smtp:Host")?.Value ?? "smtp.example.com";
            var smtpPort = int.Parse(_configuration.GetSection("Smtp:Port")?.Value ?? "587");
            var smtpEmail = _configuration.GetSection("Smtp:Email")?.Value ?? "email@example.com";
            var smtpPassword = _configuration.GetSection("Smtp:Password")?.Value ?? "password";

            try
            {
                using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential(smtpEmail, smtpPassword);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpEmail),
                        Subject = request.Subject,
                        Body = request.Body,
                        IsBodyHtml = false,
                    };
                    mailMessage.To.Add(request.To);

                    smtpClient.Send(mailMessage);
                }

                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = "Email sent successfully",
                };
            }
            catch (Exception ex)
            {
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = $"Error sending email: {ex.Message}",
                };
            }
        }
    }
}