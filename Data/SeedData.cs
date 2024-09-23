using KovsieCash_WebApp.Models;
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
        private const string adminUser = "Admin";
        private const string adminPassword = "Admin123$";
        private const string adminEmail = "admin@kovsiecash.com";
        private const string consultantUser = "Consultant";
        private const string consultantPassword = "Consultant123$";
        private const string advisorUser = "Advisor";
        private const string advisorPassword = "Advisor123$";
        private const string customerUser = "Customer";
        private const string customerPassword = "Customer123$";

        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            // Create roles
            string[] roles = { "Admin", "Consultant", "Advisor", "Customer" };
            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed users
            await SeedUser(userManager, adminUser, adminEmail, adminPassword, "Admin");
            await SeedUser(userManager, consultantUser, "consultant@kovsiecash.com", consultantPassword, "Consultant");
            await SeedUser(userManager, advisorUser, "advisor@kovsiecash.com", advisorPassword, "Advisor");

            // Seed customers and their accounts
            for (int i = 1; i <= 5; i++)
            {
                string customerUsername = $"Customer{i}";
                string customerEmail = $"customer{i}@kovsiecash.com";
                var customer = await SeedUser(userManager, customerUsername, customerEmail, customerPassword, "Customer");

                // Create an account for each customer
                var account = new Account { AccountNumber = GenerateAccountNumber(), UserId = customer.Id };
                await context.Accounts.AddAsync(account);

                // Add a deposit transaction for each account
                await context.Transactions.AddAsync(new Transaction
                {
                    Amount = 100 + i * 10, // Different deposit amounts
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = account.AccountNumber,
                    DateTime = DateTime.Now
                });
            }

            // Handle one customer with two accounts
            var specialCustomer = await SeedUser(userManager, "CustomerSpecial", "special@kovsiecash.com", customerPassword, "Customer");
            var specialAccount1 = new Account { AccountNumber = GenerateAccountNumber(), UserId = specialCustomer.Id };
            var specialAccount2 = new Account { AccountNumber = GenerateAccountNumber(), UserId = specialCustomer.Id };

            await context.Accounts.AddRangeAsync(specialAccount1, specialAccount2);

            // Add transactions for the customer with two accounts
            await context.Transactions.AddRangeAsync(
                new Transaction
                {
                    Amount = 200,
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = specialAccount1.AccountNumber,
                    DateTime = DateTime.Now
                },
                new Transaction
                {
                    Amount = 150,
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = specialAccount2.AccountNumber,
                    DateTime = DateTime.Now
                },
                new Transaction
                {
                    Amount = -50,
                    TransactionType = TransactionType.Transfer,
                    AccountNumber = specialAccount1.AccountNumber, // From account 1
                    DateTime = DateTime.Now
                },
                new Transaction
                {
                    Amount = 50,
                    TransactionType = TransactionType.Transfer,
                    AccountNumber = specialAccount2.AccountNumber, // To account 2
                    DateTime = DateTime.Now
                }
            );

            await context.SaveChangesAsync();
        }

        private static async Task<IdentityUser> SeedUser(UserManager<IdentityUser> userManager, string userName, string email, string password, string role)
        {
            if (await userManager.FindByNameAsync(userName) == null)
            {
                var user = new IdentityUser { UserName = userName, Email = email };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                    return user;
                }
            }
            return null;
        }

        private static string GenerateAccountNumber()
        {
            // Generate a random account number (e.g., 10-digit number)
            Random random = new Random();
            return random.Next(1000000000, 999999999).ToString();
        }
    }
}
