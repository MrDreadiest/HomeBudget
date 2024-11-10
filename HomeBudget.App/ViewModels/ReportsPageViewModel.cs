using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.Models.Reports;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.Reports;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using HomeBudget.App.Views;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels
{
    public partial class ReportsPageViewModel : BaseViewModel
    {
        public ObservableCollection<ReportCarouselItemBaseViewModel> ReportsCarouselVMs { get; }

        [ObservableProperty]
        private FilterReportContentViewModel _filterVM;

        [ObservableProperty]
        private int _carouselPosition;

        private bool _isInternalCarouselPositionChange;

        public SelectableButtonGroupViewModel SelectableButtonGroupVM { get; }

        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly ITransactionService _transactionService;

        public ReportsPageViewModel(IBudgetService budgetService, ITransactionService transactionService, ITransactionCategoryService transactionCategoryService)
        {
            _budgetService = budgetService;
            _transactionService = transactionService;
            _transactionCategoryService = transactionCategoryService;

            Route = nameof(ReportsPageAndroidView);
            //TODO : Zasoby
            Title = "Raporty";
            IconUnicode = Icons.Graphs;

            FilterVM = new FilterReportContentViewModel();
            FilterVM.FilterCommandRaised += FilterVM_FilterCommandRaised;

            ReportsCarouselVMs = new ObservableCollection<ReportCarouselItemBaseViewModel>()
            {
                new ReportGraphViewModel(),
                new ReportTableViewModel()
            };

            SelectableButtonGroupVM = new SelectableButtonGroupViewModel(
                new List<OptionItem> {
                                new OptionItem(ReportType.Graph.GetDescription(),ReportType.Graph),
                                new OptionItem(ReportType.Table.GetDescription(),ReportType.Table),
                });
            SelectableButtonGroupVM.SelectedChanged += SelectableButtonGroupVM_SelectedChanged;
        }

        private async void FilterVM_FilterCommandRaised(object? sender, Dictionary<string, Dictionary<string, decimal>> filteredData)
        {
            Task[] tasks = [];
            foreach (var item in ReportsCarouselVMs)
            {
                if (item is IReport report)
                {
                    tasks.Append(report.DataPresentation(filteredData));
                }
            }
            await Task.WhenAll(tasks);
        }

        public async override Task OnAppearingAsync()
        {
            try
            {
                IsBusy = true;
                IsVisible = true;

                await Task.Delay(200);

                if (_isInitialized)
                {
                    await ReloadData();
                }
                else
                {
                    _isInitialized = true;
                }

                await ResetView();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;

            if (!FilterVM.IsCollapsed)
            {
                FilterVM.ToggleIsCollapsed();
            }

            await Task.CompletedTask;
        }

        private async Task ResetView()
        {
            await FilterVM.ReloadData();
        }

        private async Task ReloadData()
        {

        }

        partial void OnCarouselPositionChanged(int oldValue, int newValue)
        {
            if (!_isInternalCarouselPositionChange)
            {
                SelectableButtonGroupVM.SelectWithoutNotify(newValue);
            }
        }



        private async void SelectableButtonGroupVM_SelectedChanged(object? sender, EventArgs e)
        {
            if (sender is SelectableButtonGroupViewModel selectableVM)
            {
                if (selectableVM.SelectedType is ReportType reportType)
                {
                    int targetPosition = reportType switch
                    {
                        ReportType.Graph => 0,
                        ReportType.Table => 1,
                        _ => CarouselPosition
                    };

                    if (CarouselPosition != targetPosition)
                    {
                        _isInternalCarouselPositionChange = true;

                        await Task.Run(() =>
                        {
                            CarouselPosition = targetPosition;
                        });

                        await Task.Delay(100);
                        _isInternalCarouselPositionChange = false;
                    }
                }
            }
        }
    }
}
