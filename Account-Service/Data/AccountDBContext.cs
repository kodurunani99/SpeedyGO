using Account_Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Data
{
    public class AccountDBContext : IdentityDbContext<ApplicationUser>
    {
        public AccountDBContext(DbContextOptions<AccountDBContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

    }
}
