using Microsoft.EntityFrameworkCore;
using AdAgencyWebAPI.Models; 

namespace AdAgencyWebAPI.Data
{
    public class adAgencyContext:DbContext
    {
        public DbSet<Advedirsments> Advedirsments { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Orders> Orders { get; set; }

        public adAgencyContext(DbContextOptions<adAgencyContext> options)
            : base(options)
        {
        }
    }
}
