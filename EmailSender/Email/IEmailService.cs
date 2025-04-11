namespace EmailSender.Email
{
    // Interface definition for email service
    // This interface ensures that any email service implementation must provide a method for sending emails asynchronously
    public interface IEmailService
    {
        // Asynchronous method signature for sending an email
        // - recipientEmail: The email address of the recipient.
        // - subject: The subject of the email.
        // - message: The body content of the email.
        // The method returns a Task to signify that it's an asynchronous operation.
        Task SendEmailAsync(string recipientEmail, string subject, string message);
    }
}
