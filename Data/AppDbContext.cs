using Microsoft.EntityFrameworkCore;
using KovsieCash_WebApp.Models;
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

        public DbSet<Advice> Advice {  get; set; }

        public DbSet<Beneficiary> Beneficiaries { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().ToTable("Account");

            // Configure Adviser relationship
            modelBuilder.Entity<Advice>()
                .HasOne(a => a.Adviser)
                .WithMany(u => u.AdviceGiven)
                .HasForeignKey(a => a.AdviserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Advisee relationship
            modelBuilder.Entity<Advice>()
                .HasOne(a => a.Advisee)
                .WithMany(u => u.AdviceReceived)
                .HasForeignKey(a => a.AdviseeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
            modelBuilder.Entity<Beneficiary>().ToTable("Beneficiaries");
            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<Review>().ToTable("Reviews");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");

        }
    }
}
