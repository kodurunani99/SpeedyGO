using CustomerServiceRequest.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerServiceRequest.DBContext
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {
        }

        // Table creation 
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
    }
}
