using EmailSender.Models;
using EmailSender.OtpServices;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EmailSender.Controllers
{
    // Route for the EmailController, defining the base URL for the API
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IOtpService _otpService;

        // Constructor for EmailController. It injects the IOtpService dependency for OTP generation and validation.
        public EmailController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        // Endpoint for requesting an OTP (One-Time Password)
        // This endpoint will send an OTP to the user's email

        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] EmailRequest request)
        {
            // Check if the email is provided in the request
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email is required.");
            }

            try
            {
                // Generate and send OTP to the provided email
                await _otpService.GenerateOtpAsync(request.Email);
                return Ok(new { message = "OTP sent successfully." });
            }
            catch (System.Exception ex)
            {
                // Handle any errors that occur during OTP generation
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // This endpoint checks if the OTP entered by the user is valid
        [HttpPost("validate-otp")]
        public IActionResult ValidateOtp([FromBody] OtpValidationRequest request)
        {
            // Check if the model state is valid (i.e., all required fields are provided)
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });
            }

            try
            {
                // Validate the OTP using the provided email and OTP
                var isValid = _otpService.ValidateOtp(request.Email, request.Otp);

                if (isValid)
                {
                    return Ok(new { message = "OTP validated successfully." });
                }
                else
                {
                    return BadRequest(new { message = "OTP is invalid or expired." });
                }
            }
            catch (System.Exception ex)
            {
                // Handle any errors during OTP validation
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // New endpoint for setting the password after OTP validation
        // This endpoint allows the user to set their password once the OTP is validated
        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequest request)
        {
            // Ensure that both email and password are provided in the request
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            try
            {
                // Assuming the OTP is already validated, proceed to set the password
                var isPasswordSet = await _otpService.SetPasswordAsync(request.Email, request.Password);

                if (isPasswordSet)
                {
                    return Ok(new { message = "Password set successfully." });
                }
                else
                {
                    return BadRequest(new { message = "Failed to set the password." });
                }
            }
            catch (System.Exception ex)
            {
                // Handle any errors that occur during password setting
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
