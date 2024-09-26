using KovsieCash_WebApp.Models;
using KovsieCash_WebApp.Models.KovsieCash_WebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KovsieCash_WebApp.Data
{
    public static class SeedData
    {       
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Accounts.Any())
            {
                context.Accounts.AddRange(
                    new Account { AccountNumber = GenerateAccountNumber(), 
                        UserId=context.ApplicationUsers.FirstOrDefault(u => u.UserName == "Customer").Id, 
                        AccountName="Current" }
                    );;

                context.Accounts.AddRange(
                    new Account
                    {
                        AccountNumber = GenerateAccountNumber(),
                        UserId = context.ApplicationUsers.FirstOrDefault(u => u.UserName == "Customer").Id,
                        AccountName = "Savings"
                    }
                    ); ;
            }

            context.SaveChanges();

            if (!context.Transactions.Any())
            {
                foreach (Account account in context.Accounts.AsQueryable())
                {
                    decimal amount = Decimal.Parse(GenerateAccountNumber());
					account.Balance = amount;
					context.Accounts.Update(account);

					context.Transactions.AddRange(
                        new Transaction
                        {						
							AccountNumber = account.AccountNumber,
                            Reference = "Deposit",
                            DateTime = DateTime.Now,
                            Amount = amount,
                            Type = TransactionType.Deposit,
                            Balance = amount,

                        });
                }
            }

            context.SaveChanges();
        }
        public static async void CreateUsers(IApplicationBuilder app)
        {
            const string adminUser = "Admin";
            const string adminPassword = "Secret123$";
            const string adminEmail = "admin@example.com";
            const string adminRole = "Administrator";

            // Define roles to be seeded
            string[] roles = { "Admin", "Consultant", "Advisor", "Customer" };

            const string customerUser = "customer";
            const string customerPassword = "Secret123$";
            const string customerEmail = "customer@example.com";
            const string customerRole = "customer";

            UserManager<ApplicationUser> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (string role in roles)
            {
                if (await userManager.FindByNameAsync(role) == null)
                {
                    if (await roleManager.FindByNameAsync(role) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }

                    ApplicationUser user = new()
                    {
                        UserName = role,
                        Email = role + "@ufs.ac.za"
                    };

                    IdentityResult result = await userManager.CreateAsync(user, role + "@2024");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }

                }
            }

        }

        private static string GenerateAccountNumber()
        {
            // Generate a random account number (10-digit number)
            Random random = new Random();
            return random.Next(999999999).ToString().PadLeft(10, '0');
        }

    }
}
