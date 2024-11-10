using HomeBudget.App.Models;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public static class FilterDataHelper
    {
        public static Dictionary<string, Dictionary<string, decimal>> GroupTransactionsByMonthAndCategory(List<Transaction> transactions, List<TransactionCategory> transactionCategories)
        {
            return transactions
                .GroupBy(t => t.Date.ToString("MMMM yyyy"))
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(t => transactionCategories.Find(c => c.Id.Equals(t.TransactionCategoryId))!.Name)
                          .ToDictionary(
                              c => c.Key,
                              c => c.Sum(t => t.TotalAmount)
                          )
                );
        }
    }
}
