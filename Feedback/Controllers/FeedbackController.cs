using Feedback.Model;
using Feedback.Model.DTO;
using Feedback.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepo;

        public FeedbackController(IFeedbackRepository feedbackRepo)
        {
            _feedbackRepo = feedbackRepo;
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] Feedbackdto feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var addedFeedback = await _feedbackRepo.AddFeedback(feedback);
                return Ok(addedFeedback); // Returns the added feedback data as the response
            }
            catch (System.Exception ex)
            {
                // Log the exception (ex) if necessary
                return StatusCode(500, "Internal server error. Failed to add feedback.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedbacks>>> GetAllFeedbacks()
        {
            try
            {
                var feedbackList = await _feedbackRepo.GetAllFeedbacks();
                return Ok(feedbackList);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error. Failed to fetch feedback.");
            }
        }

        // Get feedbacks by userId
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Feedbacks>>> GetFeedbacksByUserId(string userId)
        {
            try
            {
                var feedbackList = await _feedbackRepo.GetFeedbacksByUserId(userId);
                return Ok(feedbackList);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error. Failed to fetch feedback.");
            }
        }

        // Get feedbacks by transporterId
        [HttpGet("transporter/{transporterId}")]
        public async Task<ActionResult<IEnumerable<Feedbacks>>> GetFeedbacksByTransporterId(string transporterId)
        {
            try
            {
                var feedbackList = await _feedbackRepo.GetFeedbacksByTransporterId(transporterId);
                return Ok(feedbackList);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error. Failed to fetch feedback.");
            }
        }
    }
}
