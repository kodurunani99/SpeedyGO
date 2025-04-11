using System.Net.Mail;
using System.Net;
using EmailSender.Models;
using Microsoft.Extensions.Options;

namespace EmailSender.Email
{
    // Implementation of IEmailService to send emails asynchronously using SMTP
    public class EmailService : IEmailService
    {
        // Private field to hold SMTP settings loaded from configuration
        private readonly SmtpSettings _smtpSettings;

        // Constructor to inject SMTP settings via IOptions
        // IOptions is used to load configuration settings from appsettings.json or other sources
        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value; // Accessing the actual SmtpSettings object
        }

        // Asynchronous method to send an email
        public async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {
            // Initialize the SmtpClient with server and port settings
            // _smtpSettings.Server and _smtpSettings.Port come from configuration
            using (var smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
            {
                // Set the credentials (username and password) for the SMTP server
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);

                // Enable SSL encryption for secure communication with the SMTP server
                smtpClient.EnableSsl = true;

                // Create a new MailMessage instance and configure its properties
                var mailMessage = new MailMessage
                {
                    // Set the "From" email address (sender) using the configured sender details
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    // Set the subject line of the email
                    Subject = subject,
                    // Set the body of the email, including whether it supports HTML content
                    Body = message,
                    IsBodyHtml = true,  // Set to true if you're sending HTML content
                };

                // Add the recipient's email address to the "To" list
                mailMessage.To.Add(recipientEmail);

                // Send the email asynchronously using the SendMailAsync method
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
