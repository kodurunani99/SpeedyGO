using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmailSender.Database;
using EmailSender.Email;
using EmailSender.Models;
using System.Security.Cryptography;
using System.Text;

namespace EmailSender.OtpServices
{
    // The OtpService class implements the IOtpService interface and provides the functionality for generating, validating OTPs, and handling password setting.
    public class OtpService : IOtpService
    {
        private readonly IEmailService _emailSender; // Interface for sending emails
        private readonly AppDbContext _dbContext; // Database context for accessing OTP and user data

        // Constructor to inject dependencies (email sender service and database context)
        public OtpService(IEmailService emailSender, AppDbContext dbContext)
        {
            _emailSender = emailSender;
            _dbContext = dbContext;
        }

        // Method to generate a 6-digit OTP, store it in the database with expiration time, and send it via email
        public async Task<string> GenerateOtpAsync(string email)
        {
            // Generate a 6-digit OTP (random number between 100000 and 999999)
            var otp = new Random().Next(100000, 999999).ToString();

            // Create a new OTP request record to be saved in the database
            var otpRequest = new OtpRequest
            {
                Email = email, // The email associated with the OTP request
                Otp = otp, // The generated OTP
                ExpirationTime = DateTime.UtcNow.AddMinutes(5) // OTP expires in 5 minutes
            };

            // Add the OTP request to the database
            _dbContext.OtpRequests.Add(otpRequest);
            await _dbContext.SaveChangesAsync(); // Save changes asynchronously to the database

            // Send the OTP to the user's email using the SendOtpEmailAsync method
            await SendOtpEmailAsync(email, otp);

            return otp; // Return the generated OTP (it can be used for debugging or validation)
        }

        // Method to send the OTP email using the provided email address and generated OTP
        public async Task SendOtpEmailAsync(string email, string otp)
        {
            var subject = "Your OTP Code"; // Email subject
            var message = $"Your OTP code is: {otp}. It will expire in 5 minutes."; // OTP message body
            await _emailSender.SendEmailAsync(email, subject, message); // Send the email asynchronously

            Console.WriteLine($"Generated OTP for {email}: {otp}"); // Log the OTP for debugging purposes
        }

        // Method to validate the OTP based on the provided email and OTP input
        public bool ValidateOtp(string email, string otp)
        {
            // Retrieve the OTP request record from the database for the given email and OTP
            var otpRequest = _dbContext.OtpRequests
                .FirstOrDefault(x => x.Email == email && x.Otp == otp);

            // If the OTP doesn't exist or has expired, return false
            if (otpRequest == null || otpRequest.ExpirationTime < DateTime.UtcNow)
            {
                return false; // OTP is either invalid or expired
            }

            return true; // OTP is valid and not expired
        }

        // Method to set the user's password after OTP validation
        public async Task<bool> SetPasswordAsync(string email, string password)
        {
            // Find the user in the database by email
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            // If the user is not found, throw an exception
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Hash the new password using SHA256 before saving it to the database
            var hashedPassword = HashPassword(password);

            // Update the user's password in the database
            user.Password = hashedPassword;
            _dbContext.Users.Update(user); // Mark the user entity as modified

            await _dbContext.SaveChangesAsync(); // Save the changes to the database asynchronously

            return true; // Return true indicating the password has been set successfully
        }

        // Utility method to hash the password using the SHA256 algorithm
        // This ensures the password is stored securely in the database
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create()) // Create an instance of SHA256 hashing algorithm
            {
                // Compute the hash of the password (converting it to a byte array)
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                // Convert each byte of the hash into a hexadecimal string
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // Append each byte as a hexadecimal string
                }

                return builder.ToString(); // Return the hashed password as a string
            }
        }
    }
}
