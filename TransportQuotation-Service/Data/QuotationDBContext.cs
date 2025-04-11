using Microsoft.EntityFrameworkCore;
using Quotation_Service.Models;
using System.Collections.Generic;

namespace Quotation_Service.Data
{
    public class QuotationDBContext : DbContext
    {
        public QuotationDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Quotation> Quotations { get; set; }

    }
}
