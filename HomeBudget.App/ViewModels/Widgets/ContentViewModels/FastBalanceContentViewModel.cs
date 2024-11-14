using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Factories.Reports;
using HomeBudget.App.Models;
using HomeBudget.App.Models.Reports;
using HomeBudget.App.Models.TransactionCategories;
using HomeBudget.App.Models.WidgetConfigurations;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.Reports;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;
using HomeBudget.App.Views.Widgets;

namespace HomeBudget.App.ViewModels.Widgets
{
    public partial class FastBalanceContentViewModel : WidgetContentViewModelBase, ICollapseContentViewModel
    {
        [RelayCommand]
        public void ToggleIsCollapsed()
        {
            IsCollapsed = !IsCollapsed;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsReportNotVisible))]
        private bool _isReportVisible;

        public bool IsReportNotVisible => !IsReportVisible;

        [ObservableProperty]
        private ReportGraphPieViewModel _reportGraphVM;

        [ObservableProperty]
        private ReportTableViewModel _reportTableVM;

        private FastBalanceWidgetConfiguration _configuration;

        private readonly ReportViewModel _reportViewModel;

        private readonly IConfigurationService _configurationService;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionService _transactionService;
        private readonly ITransactionCategoryService _transactionCategoryService;

        public FastBalanceContentViewModel() : this(
            App.Services.GetService<IBudgetService>()!,
            App.Services.GetService<ITransactionService>()!,
            App.Services.GetService<ITransactionCategoryService>()!,
            App.Services.GetService<IConfigurationService>()!)
        {
        }

        public FastBalanceContentViewModel(IBudgetService budgetService, ITransactionService transactionService, ITransactionCategoryService transactionCategoryService, IConfigurationService configurationService)
        {
            // TODO: Przeniesienie do zasobów
            Title = "Szybki bilans";
            FullViewRoute = nameof(ManageFastBalanceAndroidPageView);

            IsVisible = true;
            IsReportVisible = true;
            IsCollapsed = false;

            _budgetService = budgetService;
            _transactionService = transactionService;
            _transactionCategoryService = transactionCategoryService;
            _configurationService = configurationService;

            _reportViewModel = new ReportViewModel();

            ReportGraphVM = ReportFactoryProvider.GetFactory(ReportType.GraphPie).CreateReport(_reportViewModel) as ReportGraphPieViewModel;
            ReportTableVM = ReportFactoryProvider.GetFactory(ReportType.Table).CreateReport(_reportViewModel) as ReportTableViewModel;
        }

        private async Task ResetView()
        {
            try
            {
                IsBusy = true;

                await Task.Delay(100);

                LoadConfiguration();

                var dateRage = ReportDateFilterType.ThisMonth.GetDateRange();

                List<TransactionCategory> categories = new();
                List<TransactionCategory> categoriesLeft = new();

                List<Transaction> transactions = new();
                List<Transaction> transactionsLeft = new();

                Dictionary<string, Dictionary<string, decimal>> filteredDate = new();

                Enum.TryParse(_configuration.SelectType, out TransactionCategoriesSelectType selectType);

                await _transactionCategoryService.GetAllTransactionCategoriesAsync(_budgetService.CurrentBudget);

                switch (selectType)
                {
                    case TransactionCategoriesSelectType.Own:
                        categories = _budgetService.CurrentBudget.TransactionCategories
                            .Where(c => _configuration.SelectedCategoriesIds.Contains(c.Id))
                            .ToList();
                        break;
                    case TransactionCategoriesSelectType.TopCount:
                        categories = await _transactionCategoryService.GetTopCountTransactionCategoriesInDataRangeAsync(_budgetService.CurrentBudget.Id, _configuration.TopCounter, dateRage.DateFrom, dateRage.DateTo);
                        break;
                    case TransactionCategoriesSelectType.TopAmount:
                        categories = await _transactionCategoryService.GetTopAmountTransactionCategoriesInDataRangeAsync(_budgetService.CurrentBudget.Id, _configuration.TopCounter, dateRage.DateFrom, dateRage.DateTo);
                        break;
                    default:
                        break;
                }

                transactions = await _transactionService.GetTransactionInRangeByCategoriesAsync(
                    _budgetService.CurrentBudget.Id,
                    dateRage.DateFrom,
                    dateRage.DateTo,
                    categories.Select(c => c.Id).ToList());

                if (transactions.Count == 0)
                    IsReportVisible = false;

                if (IsReportVisible)
                {
                    if (_configuration.IsSumOtherAsLastSwitch)
                    {
                        categoriesLeft = _budgetService.CurrentBudget.TransactionCategories
                            .Where(c => !categories.Contains(c))
                            .ToList();
                        transactionsLeft = await _transactionService.GetTransactionInRangeByCategoriesAsync(
                            _budgetService.CurrentBudget.Id,
                            dateRage.DateFrom,
                            dateRage.DateTo,
                            categoriesLeft.Select(c => c.Id).ToList());

                        var localOtherTransactionCategory = new TransactionCategory()
                        {
                            Id = Guid.NewGuid().ToString(),
                            BudgetId = string.Empty,
                            IconUnicode = Icons.Sum,
                            //TODO: Zasoby
                            Name = "Reszta"
                        };
                        var localOtherTransaction = new Transaction()
                        {
                            TransactionCategoryId = localOtherTransactionCategory.Id,
                            BudgetId = string.Empty,
                            Id = string.Empty,
                            CreatorId = string.Empty,
                            TotalAmount = transactionsLeft.Sum(t => t.TotalAmount)
                        };

                        transactions.Add(localOtherTransaction);
                        categories.Add(localOtherTransactionCategory);
                    }

                    _reportViewModel.IsPercentageVisible = _configuration.IsShowPercentageSwitch;
                    _reportViewModel.UpdateData(transactions, categories);
                    _reportViewModel.GenerateCategoryColors();

                    _ = Task.Run(() => ReportTableVM.DataPresentation());
                    _ = Task.Run(() => ReportGraphVM.DataPresentation());
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        internal async Task ReloadData()
        {
            await ResetView();
        }

        public override void LoadConfiguration()
        {
            _configuration = _configurationService.LoadConfiguration<FastBalanceWidgetConfiguration>();

            IsReportVisible = false;

            if (_configuration.SelectedCategoriesIds.Count > 0 && _configuration.SelectType.Equals(TransactionCategoriesSelectType.Own.ToString()))
                IsReportVisible = true;

            if (_configuration.TopCounter >= FastBalanceWidgetConfiguration.TopCounterMin &&
                !_configuration.SelectType.Equals(TransactionCategoriesSelectType.Own.ToString()))
                IsReportVisible = true;

            if (_configuration.IsSumOtherAsLastSwitch)
                IsReportVisible = true;
        }

        public override void SaveConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
