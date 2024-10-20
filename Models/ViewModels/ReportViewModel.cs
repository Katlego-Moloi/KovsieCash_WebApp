using Microsoft.AspNetCore.Mvc.Rendering;

namespace KovsieCash_WebApp.Models.ViewModels
{
    public class ReportViewModel
    {
        // User Information
        public ApplicationUser User { get; set; }

        public IEnumerable<SelectListItem> MonthsAvailable { get; set; }

		// Transactions for this month and last month
		public IEnumerable<Transaction> TransactionsThisMonth { get; set; }
        public IEnumerable<Transaction> TransactionsLastMonth { get; set; }

        public double[] SpendingByMonth { get; set; }

        public double[] SpendingByDay { get; set; }

        // Calculated fields
        public double MoneyInThisMonth => TransactionsThisMonth
            .Where(t => t.Type == TransactionType.Deposit)
            .Sum(t => t.Amount);

        public double MoneyOutThisMonth => TransactionsThisMonth
            .Where(t => t.Type == TransactionType.Withdrawal)
            .Sum(t => t.Amount);

        public double MoneyInLastMonth => TransactionsLastMonth
            .Where(t => t.Type == TransactionType.Deposit)
            .Sum(t => t.Amount);

        public double MoneyOutLastMonth => TransactionsLastMonth
            .Where(t => t.Type == TransactionType.Withdrawal)
            .Sum(t => t.Amount);

        // Variance between Money In and Money Out
        public double VarianceThisMonth => MoneyInThisMonth - MoneyOutThisMonth;
        public double VarianceLastMonth => MoneyInLastMonth - MoneyOutLastMonth;

        // Number of withdrawals made
        public int WithdrawalsThisMonth => TransactionsThisMonth
            .Count(t => t.Type == TransactionType.Withdrawal);

        public int WithdrawalsLastMonth => TransactionsLastMonth
            .Count(t => t.Type == TransactionType.Withdrawal);

        // Average weekly spending (outgoing transactions)
        public double AverageWeeklySpendingThisMonth
        {
            get
            {
                DateTime date = TransactionsThisMonth.OrderBy(t => t.DateTime).FirstOrDefault().DateTime;
                // Get the first day of the month
                DateTime firstDay = new DateTime(date.Year, date.Month, 1);

                // Get the last day of the month
                int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
                DateTime lastDay = new DateTime(date.Year, date.Month, daysInMonth);

                // Calculate the difference in days between the first and last day
                int totalDays = (lastDay - firstDay).Days + 1;

                // Get the day of the week for the first and last day
                DayOfWeek firstDayOfWeek = firstDay.DayOfWeek;
                DayOfWeek lastDayOfWeek = lastDay.DayOfWeek;

                // Calculate how many full weeks fit into the month
                int fullWeeks = totalDays / 7;

                // If the first day is not a Sunday or the last day is not a Saturday, there's an extra partial week
                bool hasPartialWeek = firstDayOfWeek != DayOfWeek.Sunday || lastDayOfWeek != DayOfWeek.Saturday;

                // Return the total number of weeks (full weeks + 1 if there is a partial week)
                int numWeeks = hasPartialWeek ? fullWeeks + 1 : fullWeeks;

                return MoneyOutThisMonth / numWeeks;
            }
        }
    }
}
