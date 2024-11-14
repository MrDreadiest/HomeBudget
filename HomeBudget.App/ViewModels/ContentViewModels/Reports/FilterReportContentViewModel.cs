using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Models.Reports;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;
using HomeBudget.App.ViewModels.Widgets;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class FilterReportContentViewModel : WidgetContentViewModelBase, ICollapseContentViewModel
    {
        public event EventHandler<(List<Transaction> FilteredTeansactions, List<TransactionCategory> FilteredCategories)> FilterCommandRaised;

        [RelayCommand]
        public async Task Filter()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                var transactions = await _transactionService.GetTransactionInRangeByCategoriesAsync(_budgetService.CurrentBudget.Id, DateFrom, DateTo, FilteredCategories.Select(c => c.Id).ToList());
                FilterCommandRaised?.Invoke(this, (transactions, FilteredCategories.ToList()));
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
            Title = "Ustawienia filtracji";

            CategoriesDropDownVM = new DropdownTransactionCategoryContentViewModel(SelectionMode.Multiple);
            CategoriesDropDownVM.SelectedTransactionCategoryChanged += CategoriesDropDownVM_SelectedTransactionCategoryChanged;

            FilteredCategories = new ObservableCollection<TransactionCategory>();

            FilterDateTypeVM = new SelectableButtonGroupViewModel(Enum.GetValues(typeof(ReportDateFilterType)).Cast<ReportDateFilterType>().Select(f => new OptionItem(f.GetDescription(), f)).ToList());
            FilterDateTypeVM.SelectedChanged += FilterDateTypeVM_SelectedChanged;

            var result = ((ReportDateFilterType)FilterDateTypeVM.SelectedType!).GetDateRange();
            DateFrom = result.DateFrom;
            DateTo = result.DateTo;
        }

        private void ResetView()
        {
            IsCollapsed = true;

            FilteredCategories.Clear();

            FilterDateTypeVM.SelectWithoutNotify(0);

            var result = ((ReportDateFilterType)FilterDateTypeVM.SelectedType!).GetDateRange();
            DateFrom = result.DateFrom;
            DateTo = result.DateTo;

            CategoriesDropDownVM.SelectTopInRange(3);
            Task.Run(async () => { await Filter(); });
        }

        public async Task ReloadData()
        {
            await CategoriesDropDownVM.Reload();

            ResetView();
        }

        private void FilterDateTypeVM_SelectedChanged(object? sender, EventArgs e)
        {
            if (sender is SelectableButtonGroupViewModel selectableVM)
            {
                if (selectableVM.SelectedType is ReportDateFilterType dateType)
                {
                    var result = dateType.GetDateRange();
                    DateFrom = result.DateFrom;
                    DateTo = result.DateTo;
                }
            }
        }

        private void CategoriesDropDownVM_SelectedTransactionCategoryChanged(object? sender, List<TransactionCategory> e)
        {
            FilteredCategories = new ObservableCollection<TransactionCategory>(e);
        }

        public override void LoadConfiguration()
        {
            throw new NotImplementedException();
        }

        public override void SaveConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
