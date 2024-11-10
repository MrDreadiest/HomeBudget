using HomeBudget.App.Models;
using HomeBudget.App.Models.Reports;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class ReportTableViewModel : ReportCarouselItemBaseViewModel, IReport
    {

        private Dictionary<string, Dictionary<string, decimal>> _filteredData;

        public ObservableCollection<TransactionRow> Rows { get; set; }
        public ObservableCollection<string> Months { get; set; }

        public ReportType ReportType => ReportType.Table;

        public ReportTableViewModel()
        {
            Title = ReportType.GetDescription();
            Rows = new ObservableCollection<TransactionRow>();
            Months = new ObservableCollection<string>();
        }

        public override void ResetView()
        {
            throw new NotImplementedException();
        }

        public override void ReloadData(List<TransactionCategory> categories, List<Transaction> transactions)
        {
            throw new NotImplementedException();
        }

        public Task DataPresentation(Dictionary<string, Dictionary<string, decimal>> filteredData)
        {
            _filteredData = filteredData;

            Rows.Clear();
            Months.Clear();

            int currentYear = DateTime.Now.Year;



            var months = filteredData.Keys
            .OrderBy(m => DateTime.ParseExact(m, "MMMM yyyy", null))
            .ToList();

            months.ForEach(m =>
            {
                var date = DateTime.ParseExact(m, "MMMM yyyy", null);
                string formattedYear = date.Year == currentYear ? date.ToString("MMM") : date.ToString("MMM yyyy");
                Months.Add(formattedYear);
            });

            var categories = filteredData.Values
                .SelectMany(dict => dict.Keys)
                .Distinct()
                .OrderBy(cat => cat)
                .ToList();

            foreach (var category in categories)
            {
                var row = new TransactionRow
                {
                    Category = category,
                    Values = new List<decimal>()
                };

                foreach (var month in months)
                {
                    if (filteredData[month].TryGetValue(category, out decimal amount))
                    {
                        row.Values.Add(amount);
                    }
                    else
                    {
                        row.Values.Add(0);
                    }
                }
                Rows.Add(row);
            }
            return Task.CompletedTask;
        }

    }

    public class TransactionRow
    {
        public string Category { get; set; }
        public List<decimal> Values { get; set; }
    }


}