namespace EmailSender.OtpServices
{
    // Interface defining OTP service operations
    // This interface ensures that any OTP service implementation provides methods for OTP generation, validation, email sending, and password setting
    public interface IOtpService
    {
        // Asynchronous method to generate a one-time password (OTP)
        // - email: The email address for which the OTP is being generated.
        // Returns a Task<string> which represents the generated OTP.
        Task<string> GenerateOtpAsync(string email);

        // Method to validate the OTP for a specific email
        // - email: The email address to check against.
        // - otp: The OTP to validate.
        // Returns a boolean indicating whether the OTP is valid or not.
        bool ValidateOtp(string email, string otp);

        // Asynchronous method to send the generated OTP to the provided email
        // - email: The recipient's email address to send the OTP to.
        // - otp: The OTP to be sent.
        // This method doesn't return anything (void), it just sends the email asynchronously.
        Task SendOtpEmailAsync(string email, string otp);

        // Asynchronous method to set the password after OTP validation
        // - email: The email address associated with the user.
        // - password: The password to set for the user.
        // Returns a Task<bool> indicating if the password was set successfully.
        Task<bool> SetPasswordAsync(string email, string password);
    }
}
