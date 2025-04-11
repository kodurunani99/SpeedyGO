using EmailSender.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //Creating table to store email
        public DbSet<OtpRequest> OtpRequests { get; set; }

        public DbSet<User> Users { get; set; }


    }
}
