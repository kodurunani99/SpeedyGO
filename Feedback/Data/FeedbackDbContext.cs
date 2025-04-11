using Feedback.Model;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Data
{
    public class FeedbackDbContext : DbContext
    {
        public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options) : base(options)
        {
        }

        public DbSet<Feedbacks> Feedbacks { get; set; }

    }
}
