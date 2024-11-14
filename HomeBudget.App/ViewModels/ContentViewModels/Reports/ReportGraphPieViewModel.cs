
using HomeBudget.App.Models.Reports;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class ReportGraphPieViewModel : ReportCarouselItemBaseViewModel, IReport
    {
        private readonly ReportViewModel _reportViewModel;

        public ObservableCollection<ISeries> Series { get; set; }
        public ObservableCollection<ObservableValue> ObservableValues { get; set; }

        public ReportType ReportType => ReportType.GraphPie;

        public ReportGraphPieViewModel(ReportViewModel reportViewModel)
        {
            _reportViewModel = reportViewModel;

            Title = ReportType.GetDescription();
            Series = new ObservableCollection<ISeries>();
            ObservableValues = new ObservableCollection<ObservableValue>();
        }

        public Task DataPresentation()
        {
            PopulateSeries();
            return Task.CompletedTask;
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

                Series.Add(new PieSeries<ObservableValue>
                {
                    Values = observableValues,
                    Name = category.Name,
                    MaxRadialColumnWidth = 65,
                    Fill = new SolidColorPaint(categoryColors[category.Id])
                });
            }
        }
    }
}
