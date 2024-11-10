using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Models.Reports;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class FilterReportContentViewModel : ObservableObject, ICollapseContentViewModel
    {
        public event EventHandler<Dictionary<string, Dictionary<string, decimal>>>? FilterCommandRaised;

        [RelayCommand]
        public async Task Filter()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                if (IsValid())
                {
                    var transactions = await _transactionService.GetTransactionInRangeByCategoriesAsync(_budgetService.CurrentBudget.Id, DateFrom, DateTo, FilteredCategories.Select(c => c.Id).ToList());
                    FilterCommandRaised?.Invoke(this, FilterDataHelper.GroupTransactionsByMonthAndCategory(transactions, _budgetService.CurrentBudget.TransactionCategories));
                }
            }
            catch
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public void ToggleIsCollapsed()
        {
            IsCollapsed = !IsCollapsed;
        }
        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _filteredCategories;
        [ObservableProperty]
        private DateTime _dateFrom;
        [ObservableProperty]
        private DateTime _dateTo;

        [ObservableProperty]
        private bool _isDateEntryEnable;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool _isCollapsed;

        [ObservableProperty]
        private string _title = string.Empty;

        public SelectableButtonGroupViewModel FilterDateTypeVM { get; }

        public DropdownTransactionCategoryContentViewModel CategoriesDropDownVM { get; }

        private readonly IBudgetService _budgetService;
        private readonly ITransactionService _transactionService;

        public FilterReportContentViewModel() : this(App.Services.GetService<IBudgetService>()!, App.Services.GetService<ITransactionService>()!)
        {

        }

        public FilterReportContentViewModel(IBudgetService budgetService, ITransactionService transactionService)
        {
            _budgetService = budgetService;
            _transactionService = transactionService;

            IsCollapsed = true;
            Title = String.Concat("Ustawienia filtracji");

            CategoriesDropDownVM = new DropdownTransactionCategoryContentViewModel(SelectionMode.Multiple);
            CategoriesDropDownVM.SelectedTransactionCategoryChanged += CategoriesDropDownVM_SelectedTransactionCategoryChanged;

            FilteredCategories = new ObservableCollection<TransactionCategory>();

            FilterDateTypeVM = new SelectableButtonGroupViewModel(Enum.GetValues(typeof(ReportDateFilterType)).Cast<ReportDateFilterType>().Select(f => new OptionItem(f.GetDescription(), f)).ToList());
            FilterDateTypeVM.SelectedChanged += FilterDateTypeVM_SelectedChanged;

            UpdateDateRange((ReportDateFilterType)FilterDateTypeVM.SelectedType!);
        }

        private void ResetView()
        {
            IsCollapsed = true;

            FilteredCategories.Clear();

            FilterDateTypeVM.SelectWithoutNotify(0);
            UpdateDateRange((ReportDateFilterType)FilterDateTypeVM.SelectedType!);

            CategoriesDropDownVM.SelectTopInRange(3);
            Task.Run(async () => { await Filter(); });
        }

        public async Task ReloadData()
        {
            //await CategoriesDropDownVM.ReloadData();

            ResetView();
        }

        private void UpdateDateRange(ReportDateFilterType dateFilterType)
        {
            var today = DateTime.Today;

            IsDateEntryEnable = dateFilterType == ReportDateFilterType.Own;

            switch (dateFilterType)
            {
                case ReportDateFilterType.ThisMonth:
                    DateFrom = new DateTime(today.Year, today.Month, 1);
                    DateTo = DateFrom.AddMonths(1).AddDays(-1);
                    break;

                case ReportDateFilterType.ThreeMonths:
                    DateFrom = new DateTime(today.Year, today.Month, 1).AddMonths(-2);
                    DateTo = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
                    break;

                case ReportDateFilterType.SixMonths:
                    DateFrom = new DateTime(today.Year, today.Month, 1).AddMonths(-5);
                    DateTo = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
                    break;

                case ReportDateFilterType.TwelveMonths:
                    DateFrom = new DateTime(today.Year, today.Month, 1).AddMonths(-11);
                    DateTo = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
                    break;

                case ReportDateFilterType.Own:
                    DateFrom = today;
                    DateTo = today;
                    break;

                default:
                    break;
            }
        }

        private bool IsValid()
        {
            return true;
        }

        private void FilterDateTypeVM_SelectedChanged(object? sender, EventArgs e)
        {
            if (sender is SelectableButtonGroupViewModel selectableVM)
            {
                if (selectableVM.SelectedType is ReportDateFilterType dateType)
                {
                    UpdateDateRange(dateType);
                }
            }
        }

        private void CategoriesDropDownVM_SelectedTransactionCategoryChanged(object? sender, EventArgs e)
        {
            if (sender is List<TransactionCategory> list)
            {
                FilteredCategories = new ObservableCollection<TransactionCategory>(list);
            }
        }


    }
}
