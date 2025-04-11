using Feedback.Model;
using Feedback.Model.DTO;

namespace Feedback.Repository
{
    public interface IFeedbackRepository
    {
        Task<Feedbacks> AddFeedback(Feedbackdto feedback);
        Task<IEnumerable<Feedbacks>> GetAllFeedbacks();
        Task<IEnumerable<Feedbacks>> GetFeedbacksByUserId(string userId); // Method to get feedback by userId
        Task<IEnumerable<Feedbacks>> GetFeedbacksByTransporterId(string transporterId); // Method to get feedback by transporterId
    }
}
