using HomeBudget.App.Models;
using HomeBudget.App.Models.Helpers;
using SkiaSharp;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public class ReportViewModel
    {
        public Dictionary<string, Dictionary<string, decimal>> FilteredData { get; set; }
        public Dictionary<string, SKColor> CategoryColors { get; set; }
        public List<string> Months { get; set; }
        public List<TransactionCategory> Categories { get; set; }

        public decimal AllTransactionsAmount { get; set; }
        public decimal AvgTransactionsAmount { get; set; }

        public bool IsPercentageVisible = false;

        public ReportViewModel()
        {
            FilteredData = new();
            CategoryColors = new();
            Months = new();
            Categories = new();
        }

        public void UpdateData(List<Transaction> transactions, List<TransactionCategory> categories)
        {
            FilteredData = FilterDataHelper.GroupTransactionsByMonthAndCategory(transactions, categories);
            Categories = categories;

            Months = FilteredData.Keys.OrderBy(m => DateTime.ParseExact(m, "MMMM yyyy", null)).ToList();

            AllTransactionsAmount = transactions.Sum(transaction => transaction.TotalAmount);
            AvgTransactionsAmount = AllTransactionsAmount / Months.Count;
        }

        public void GenerateCategoryColors()
        {
            Random random = new Random();
            float hueStep = 360f / Categories.Count;
            int saturation = 70;
            int value = 90;

            foreach (var category in Categories)
            {
                float hue = (hueStep * Categories.IndexOf(category)) % 360;
                var color = SKColor.FromHsv(hue, saturation, value);
                CategoryColors[category.Id] = color;
            }
        }
    }
}
