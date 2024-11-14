using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.Models.Reports;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class ReportGraphBarViewModel : ReportCarouselItemBaseViewModel, IReport
    {
        private readonly ReportViewModel _reportViewModel;

        public ObservableCollection<ISeries> Series { get; set; }
        public ObservableCollection<ObservableValue> ObservableValues { get; set; }

        [ObservableProperty]
        private Axis[] _xAxes;

        [ObservableProperty]
        private Axis[] _yAxes;

        public ReportType ReportType => ReportType.GraphBar;

        public ReportGraphBarViewModel(ReportViewModel reportViewModel)
        {
            _reportViewModel = reportViewModel;

            Title = ReportType.GetDescription();
            Series = new ObservableCollection<ISeries>();
            ObservableValues = new ObservableCollection<ObservableValue>();

            UpdateYAxes();
        }

        private List<string> GenerateLabels(List<string> months)
        {
            List<string> labels = new List<string>();

            int currentYear = DateTime.Now.Year;

            foreach (var month in months)
            {
                var date = DateTime.ParseExact(month, "MMMM yyyy", null);
                string formattedYear = date.Year == currentYear ? date.ToString("MMM") : date.ToString("MMM yyyy");
                labels.Add(formattedYear);
            }

            return labels;
        }

        private void PopulateSeries()
        {
            var filteredData = _reportViewModel.FilteredData;
            var months = _reportViewModel.Months;
            var categories = _reportViewModel.Categories;
            var categoryColors = _reportViewModel.CategoryColors;

            Series.Clear();

            foreach (var category in categories)
            {
                var observableValues = new ObservableCollection<ObservableValue>();

                foreach (var month in months)
                {
                    if (filteredData[month].TryGetValue(category.Id, out decimal amount))
                    {
                        observableValues.Add(new ObservableValue((double)amount));
                    }
                    else
                    {
                        observableValues.Add(new ObservableValue(0));
                    }
                }

                Series.Add(new ColumnSeries<ObservableValue>
                {
                    Values = observableValues,
                    MaxBarWidth = double.MaxValue,
                    Name = category.Name,
                    Fill = new SolidColorPaint(categoryColors[category.Id])
                });
            }
        }

        private void UpdateYAxes()
        {
            YAxes = new[]
            {
                new Axis
                {
                    Labeler = Labelers.Currency,
                    MinLimit = 0
                }
            };
        }

        private void UpdateXAxes()
        {
            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = _reportViewModel.Months.Count == 1 ? [] : GenerateLabels(_reportViewModel.Months),
                    IsVisible = _reportViewModel.Months.Count > 1,
                    LabelsRotation = 0,
                    SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                    SeparatorsAtCenter = false,
                    TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                    TicksAtCenter = true,
                    ForceStepToMin = true,
                    MinStep = 1
                }
            };
        }

        public Task DataPresentation()
        {
            PopulateSeries();
            UpdateXAxes();
            return Task.CompletedTask;
        }
    }
}
