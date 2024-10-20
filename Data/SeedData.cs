using KovsieCash_WebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace KovsieCash_WebApp.Data
{
    public static class SeedData
    {      
        public static async void PopulateDatabase(IApplicationBuilder app)
        {
            UserManager<ApplicationUser> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();
           
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            // Check if the Users and Accounts tables are already populated
            if (context.Users.Any() || context.Accounts.Any())
            {
                // If Users or Accounts table has data, exit the method
                return;
            }

            /*---------------------------------------Populating Users-------------------------------------------*/

            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            // Define roles to be seeded
            string[] roles = { "Admin", "Consultant", "Advisor", "Customer" };

            // Create roles if they do not exist
            foreach (string role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Path to the seed data file
            string filePath = Path.Combine(app.ApplicationServices
                .GetRequiredService<IWebHostEnvironment>().ContentRootPath, "wwwroot", "seeddata", "users.txt");

            // Read user data from the text file
            if (File.Exists(filePath))
            {
                var lines = await File.ReadAllLinesAsync(filePath);

                foreach (var line in lines)
                {
                    string[] userData = line.Split(',');
                    if (userData.Length == 4) // Check if the line has all required fields
                    {
                        string username = userData[0].Trim();
                        string email = userData[1].Trim();
                        string password = userData[2].Trim();
                        string role = userData[3].Trim();

                        // Check if user already exists
                        if (await userManager.FindByNameAsync(username) == null)
                        {
                            // Create a new user
                            ApplicationUser user = new()
                            {
                                UserName = username,
                                Email = email
                            };

                            IdentityResult result = await userManager.CreateAsync(user, password);

                            if (result.Succeeded)
                            {
                                // Add user to role
                                if (await roleManager.FindByNameAsync(role) != null)
                                {
                                    await userManager.AddToRoleAsync(user, role);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // Log or handle the case when the file does not exist
                throw new FileNotFoundException("The user seed data file was not found.", filePath);
            }

            /*---------------------------------------------- Populating Accounts -------------------------------------*/

            // Fetch all users from the database
            List<ApplicationUser> users = userManager.Users.ToList();

            foreach (ApplicationUser user in users)
            {
                // Generate a random number to decide how many accounts to create
                Random randomAccounts = new Random();
                int accountCount = randomAccounts.Next(1, 3); // Generates either 1 or 2

                // Create the first account (Current)
                Account currentAccount = new Account
                {
                    AccountNumber = GenerateAccountNumber(),
                    AccountName = "Current",
                    Balance = 0,
                    UserId = user.Id // Set the UserId for the account
                };

                context.Accounts.Add(currentAccount); // Add current account to the context

                // If accountCount is 2, create a second account (Savings)
                if (accountCount == 2)
                {
                    Account savingsAccount = new Account
                    {
                        AccountNumber = GenerateAccountNumber(),
                        AccountName = "Savings",
                        Balance = 0,
                        UserId = user.Id // Set the UserId for the account
                    };

                    context.Accounts.Add(savingsAccount); // Add savings account to the context
                }
            }

            // Save all changes to the database
            await context.SaveChangesAsync();

            /*---------------------------------------------- Populating Transactions -------------------------------------*/

            // Fetch all accounts from the database
            List<Account> accounts = context.Accounts.ToList();
            Random transactionsRandom = new Random();

            foreach (Account account in accounts)
            {
                // Generate a random initial deposit
                double initialDeposit = (double)(transactionsRandom.NextDouble() * (1000 - 100) + 100); // Random deposit between 100 and 1000
                account.Balance += initialDeposit; // Update the account balance with the initial deposit

                // Create a transaction for the initial deposit
                DateTime randomDate = DateTime.Now;

                Transaction initDepositTransaction = new Transaction
                {
                    Reference = "Initial Deposit",
                    DateTime = DateTime.Parse("2024/01/01"),
                    Amount = initialDeposit,
                    Type = TransactionType.Deposit,
                    Balance = account.Balance, // Balance after the deposit
                    AccountNumber = account.AccountNumber // Associate with the correct account
                };

                context.Transactions.Add(initDepositTransaction); // Add the deposit transaction to the context

                // Create a notification for the initial deposit
                var initialDepositNotification = new Notification
                {
                    Type = NotificationType.Deposit,
                    NotificationDescription = $"Deposit {initDepositTransaction.Reference} - {initialDeposit:C} added to account {account.AccountNumber}.",
                    Status = NotificationStatus.Read,
                    UserID = account.UserId,
                    NotificationDateTime = DateTime.Parse("2024/01/01")

                };

                context.Notifications.Add(initialDepositNotification);

                // Generate a random number of transactions (1 to 50)
                int transactionCount = transactionsRandom.Next(1, 51); // Generates a number between 1 and 50
                for (int i = 0; i < transactionCount; i++)
                {
                    DateTime transactionDateTime = GenerateRandomDate();

                    // Decide whether the transaction will be a deposit or withdrawal
                    bool isWithdrawal = transactionsRandom.Next(0, 2) == 0; // 50% chance for withdrawal

                    double amount;
                    if (isWithdrawal)
                    {
                        // For withdrawal, ensure the amount does not exceed the current balance
                        amount = (double)transactionsRandom.NextDouble() * (account.Balance > 100 ? account.Balance / 2 : account.Balance); // Random amount up to half the balance
                        if (amount > account.Balance)
                        {
                            // If for some reason amount exceeds balance, adjust it
                            amount = account.Balance;
                        }
                        account.Balance -= amount; // Deduct from balance

                        // Create a withdrawal transaction
                        Transaction withdrawalTransaction = new Transaction
                        {
                            Reference = $"{ GenerateReference(TransactionType.Withdrawal) }{i + 1}",
                            DateTime = transactionDateTime,
                            Amount = amount,
                            Type = TransactionType.Withdrawal,
                            Balance = account.Balance, // Update the balance after withdrawal
                            AccountNumber = account.AccountNumber // Associate with the correct account
                        };
                        context.Transactions.Add(withdrawalTransaction); // Add the withdrawal transaction to the context

                        // Create a notification for the withdrawal transaction
                        Notification withdrawalNotification = new Notification
                        {
                            Type = NotificationType.Withdrawal,
                            NotificationDescription = $"Withdrawal {withdrawalTransaction.Reference} - {amount:C} made from account {account.AccountNumber}.",
                            Status = NotificationStatus.Read,
                            UserID = account.UserId,
                            NotificationDateTime = transactionDateTime,

                        };
                        context.Notifications.Add(withdrawalNotification);
                    }
                    else
                    {
                        // For deposit, generate a random amount
                        amount = (double)(transactionsRandom.NextDouble() * (1000 - 100) + 100); // Random amount between 100 and 1000
                        account.Balance += amount; // Update balance with the deposit

                        // Create a deposit transaction
                        Transaction depositTransaction = new Transaction
                        {
                            Reference = $"Deposit Transaction {i + 1}",
                            DateTime = GenerateRandomDate(),
                            Amount = amount,
                            Type = TransactionType.Deposit,
                            Balance = account.Balance, // Update the balance after deposit
                            AccountNumber = account.AccountNumber
                            

                        };
                        context.Transactions.Add(depositTransaction); // Add the deposit transaction to the context

                        // Create a notification for the deposit transaction
                        var depositNotification = new Notification
                        {
                            Type = NotificationType.Deposit,
                            NotificationDescription = $"Deposit {depositTransaction.Reference} - {amount:C} made to account {account.AccountNumber}.",
                            Status = NotificationStatus.Read,
                            UserID = account.UserId,
                            NotificationDateTime = transactionDateTime
                        
                        };
                        context.Notifications.Add(depositNotification);
                    }
                }
            }

            // Save all changes to the database
            await context.SaveChangesAsync();

        }

        private static string GenerateAccountNumber()
        {
            // Generate a random account number (10-digit number)
            Random random = new Random();
            return random.Next(999999999).ToString().PadLeft(10, '0');
        }

        public static string GenerateReference(TransactionType transactionType)
        {
            // Get the current date and time in the format "yyyyMMddHHmmss" (year, month, day, hour, minute, second)
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            // Generate a random 4-digit number for extra uniqueness
            Random random = new Random();
            int randomDigits = random.Next(1000, 9999);

            // Determine the abbreviation for the transaction type
            string transactionAbbreviation = transactionType switch
            {
                TransactionType.Transfer => "TRF",
                TransactionType.Deposit => "DEP",
                TransactionType.Withdrawal => "WTH",
                _ => "UNK"
            };

            // Combine the components into a single reference string
            return $"{transactionAbbreviation}-{timestamp}-{randomDigits}";
        }

        public static DateTime GenerateRandomDate()
        {
            // Create a random number generator
            Random randomDate = new Random();

            // Set the start and end dates for 2024
            DateTime startDate = new DateTime(2024, 7, 23);

            // Generate a random number of days to add to the start date
            int range = (DateTime.Today - startDate).Days;
            return startDate.AddDays(randomDate.Next(range));

        }

    }
}
