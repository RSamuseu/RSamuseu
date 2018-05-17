using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AdvertisingAgency.Models;

namespace AdvertisingAgency.Data
{
    public class AccountContext : IdentityDbContext<User>
    {
        public AccountContext(DbContextOptions<AccountContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }
}