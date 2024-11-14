﻿using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.Models.Reports;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class ReportTableViewModel : ReportCarouselItemBaseViewModel, IReport
    {
        [ObservableProperty]
        private bool _isPercentageVisible = false;

        public ObservableCollection<CategoryInfo> Categories { get; set; } = new();
        public ObservableCollection<MonthColumn> MonthColumns { get; set; } = new();

        public ReportType ReportType => ReportType.Table;

        private readonly ReportViewModel _reportViewModel;

        public ReportTableViewModel(ReportViewModel reportViewModel)
        {
            Title = ReportType.GetDescription();
            _reportViewModel = reportViewModel;
        }

        private string GenerateLabel(string month)
        {
            int currentYear = DateTime.Now.Year;

            var date = DateTime.ParseExact(month, "MMMM yyyy", null);
            return date.Year == currentYear ? date.ToString("MMM") : date.ToString("MMM yyyy");

        }

        private void PopulateSeries()
        {
            var filteredData = _reportViewModel.FilteredData;
            var months = _reportViewModel.Months;
            var categories = _reportViewModel.Categories;
            var categoryColors = _reportViewModel.CategoryColors;

            IsPercentageVisible = _reportViewModel.IsPercentageVisible;

            Categories.Clear();
            MonthColumns.Clear();

            foreach (var category in categories)
            {
                Categories.Add(new CategoryInfo
                {
                    IconUnicode = category.IconUnicode,
                    Name = category.Name,
                    CategoryColor = categoryColors.ContainsKey(category.Id) ? categoryColors[category.Id] : SKColor.Empty
                });
            }

            foreach (var month in months)
            {
                var column = new MonthColumn
                {
                    MonthLabel = GenerateLabel(month),
                    Values = new List<SumAndPercentage>()
                };

                foreach (var category in categories)
                {
                    if (filteredData[month].TryGetValue(category.Id, out decimal amount))
                    {
                        decimal totalForMonth = filteredData[month].Values.Sum();
                        decimal percentageByCategory = totalForMonth > 0 ? (amount / totalForMonth) * 100 : 0;

                        column.Values.Add(new SumAndPercentage(amount, percentageByCategory));
                    }
                    else
                    {
                        column.Values.Add(new SumAndPercentage(0, 0));
                    }
                }

                MonthColumns.Add(column);
            }
        }

        public Task DataPresentation()
        {
            PopulateSeries();
            return Task.CompletedTask;
        }
    }

    public class CategoryInfo
    {
        public string IconUnicode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public SKColor CategoryColor { get; set; }
    }

    public class MonthColumn
    {
        public string MonthLabel { get; set; } = string.Empty;
        public List<SumAndPercentage> Values { get; set; } = new();
    }

    public class SumAndPercentage
    {
        public decimal Sum { get; }
        public decimal Percentage { get; }

        public SumAndPercentage(decimal sum, decimal percentage)
        {
            Sum = sum;
            Percentage = percentage;
        }
    }
}