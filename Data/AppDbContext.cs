using Microsoft.EntityFrameworkCore;
using KovsieCash_WebApp.Models;
using KovsieCash_WebApp.Models.KovsieCash_WebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KovsieCash_WebApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets for your models
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Transaction>().ToTable("Transaction");
            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
        }
    }
}
