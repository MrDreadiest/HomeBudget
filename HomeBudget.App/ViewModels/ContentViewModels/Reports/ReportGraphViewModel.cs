using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.Models;
using HomeBudget.App.Models.Reports;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class ReportGraphViewModel : ReportCarouselItemBaseViewModel, IReport
    {
        private Dictionary<string, Dictionary<string, decimal>> _filteredData;

        public ObservableCollection<ISeries> Series { get; set; }
        public ObservableCollection<ObservableValue> ObservableValues { get; set; }

        [ObservableProperty]
        private Axis[] _xAxes;

        [ObservableProperty]
        private Axis[] _yAxes;

        public ObservableCollection<TransactionRow> Rows { get; set; }

        public ReportType ReportType => ReportType.Graph;

        public ReportGraphViewModel()
        {
            Title = ReportType.GetDescription();
            Rows = new ObservableCollection<TransactionRow>();
            Series = new ObservableCollection<ISeries>();
            ObservableValues = new ObservableCollection<ObservableValue>();

            YAxes = new Axis[]
{
                new Axis
                {
                    Labeler = Labelers.Currency,
                    MinLimit = 0,
                }
};
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

            Series.Clear();
            Rows.Clear();

            List<string> labels = new List<string>();
            var months = filteredData.Keys.OrderBy(m => DateTime.ParseExact(m, "MMMM yyyy", null)).ToList();
            int currentYear = DateTime.Now.Year;

            if (months.Count > 1)
            {
                months.ForEach(m =>
                {
                    var date = DateTime.ParseExact(m, "MMMM yyyy", null);
                    string formattedYear = date.Year == currentYear ? date.ToString("MMM") : date.ToString("MMM yyyy");
                    labels.Add(formattedYear);
                });
            }
            else
            {
                labels = filteredData.Values
                    .SelectMany(dict => dict.Keys)
                    .Distinct()
                    .OrderBy(cat => cat)
                    .ToList();
            }

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

                var observableValues = new ObservableCollection<ObservableValue>();

                foreach (var month in months)
                {
                    if (filteredData[month].TryGetValue(category, out decimal amount))
                    {
                        row.Values.Add(amount);
                        observableValues.Add(new ObservableValue((double)amount));
                    }
                    else
                    {
                        row.Values.Add(0);
                        observableValues.Add(new ObservableValue(0));
                    }
                }

                Rows.Add(row);

                Series.Add(new ColumnSeries<ObservableValue>
                {
                    Values = observableValues,
                    MaxBarWidth = double.MaxValue,
                    Name = category
                });
            }

            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = months.Count == 1 ? [] : labels,
                    IsVisible = months.Count > 1,
                    LabelsRotation = 0,
                    SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                    SeparatorsAtCenter = false,
                    TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                    TicksAtCenter = true,
                    ForceStepToMin = true,
                    MinStep = 1
                }
            };


            return Task.CompletedTask;
        }


    }
}
