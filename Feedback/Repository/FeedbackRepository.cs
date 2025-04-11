using Feedback.Data;
using Feedback.Model;
using Feedback.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System;

namespace Feedback.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly FeedbackDbContext _dbContext;

        public FeedbackRepository(FeedbackDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Map Feedbackdto to Feedbacks and add it to the database
        public async Task<Feedbacks> AddFeedback(Feedbackdto feedbackDto)
        {
            try
            {
                // Map Feedbackdto to Feedbacks
                var feedback = new Feedbacks
                {
                    UserId = feedbackDto.UserId,
                    CustomerName = feedbackDto.CustomerName,
                    Rating = feedbackDto.Rating,
                    Message = feedbackDto.Message,
                    TransporterId = feedbackDto.TransporterId
                };

                await _dbContext.Feedbacks.AddAsync(feedback);
                await _dbContext.SaveChangesAsync();

                return feedback;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("An error occurred while adding feedback.", ex);
            }
        }

        public async Task<IEnumerable<Feedbacks>> GetAllFeedbacks()
        {
            try
            {
                return await _dbContext.Feedbacks.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("An error occurred while retrieving all feedbacks.", ex);
            }
        }

        // Retrieve feedbacks by userId
        public async Task<IEnumerable<Feedbacks>> GetFeedbacksByUserId(string userId)
        {
            try
            {
                return await _dbContext.Feedbacks
                    .Where(f => f.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("An error occurred while retrieving feedbacks by user ID.", ex);
            }
        }

        // Retrieve feedbacks by transporterId
        public async Task<IEnumerable<Feedbacks>> GetFeedbacksByTransporterId(string transporterId)
        {
            try
            {
                return await _dbContext.Feedbacks
                    .Where(f => f.TransporterId == transporterId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("An error occurred while retrieving feedbacks by transporter ID.", ex);
            }
        }
    }
}
