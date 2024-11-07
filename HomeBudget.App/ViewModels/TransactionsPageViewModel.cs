using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels;
using HomeBudget.App.ViewModels.ContentViewModels.Filters;
using HomeBudget.App.Views;

namespace HomeBudget.App.ViewModels
{
    public partial class TransactionsPageViewModel : BaseViewModel
    {
        [RelayCommand]
        public async Task ToggleFilterVisibility(FilterContentViewModelBase selectedFilter)
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                if (_currentFilterVisible != null)
                {
                    if (_currentFilterVisible == selectedFilter)
                    {
                        await _currentFilterVisible.ToggleVisibility();
                        _currentFilterVisible = null;
                    }
                    else
                    {
                        var t1 = _currentFilterVisible.ToggleVisibility();
                        var t2 = selectedFilter.ToggleVisibility();

                        await Task.WhenAll(t1, t2);

                        _currentFilterVisible = selectedFilter;
                    }
                }
                else
                {
                    await selectedFilter.ToggleVisibility();
                    _currentFilterVisible = selectedFilter;
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

        [ObservableProperty]
        private TransactionPriceFilterContentViewModel transactionPriceFilterVM;

        [ObservableProperty]
        private TransactionDateFilterContentViewModel transactionDateFilterVM;

        [ObservableProperty]
        private TransactionTypeFilterContentViewModel transactionTypeFilterVM;

        [ObservableProperty]
        private TransactionCategoryFilterContentViewModel transactionCategoryFilterVM;

        [ObservableProperty]
        private string _searchQuery;

        [ObservableProperty]
        private TransactionListContentViewModel _transactionListVM;

        private FilterContentViewModelBase _currentFilterVisible;

        private List<ITransactionFilter> _transactionFilters;

        private readonly IBudgetService _budgetService;
        private readonly IUserService _userService;
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly ITransactionService _transactionService;

        public TransactionsPageViewModel(IBudgetService budgetService, IUserService userService, ITransactionCategoryService transactionCategoryService, ITransactionService transactionService)
        {
            _budgetService = budgetService;
            _userService = userService;
            _transactionCategoryService = transactionCategoryService;
            _transactionService = transactionService;

            Route = nameof(TransactionsPageAndroidView);
            //TODO: zasoby
            Title = "Historia transakcji";
            IconUnicode = Icons.Transactions;

            InitializeFilters();

            TransactionListVM = new TransactionListContentViewModel(_budgetService, _transactionService);
            TransactionListVM.RefreshViewCall += TransactionListVM_RefreshViewCall;
        }

        private void InitializeFilters()
        {
            TransactionPriceFilterVM = new TransactionPriceFilterContentViewModel();
            TransactionDateFilterVM = new TransactionDateFilterContentViewModel();
            TransactionTypeFilterVM = new TransactionTypeFilterContentViewModel();
            TransactionCategoryFilterVM = new TransactionCategoryFilterContentViewModel();

            _transactionFilters = new List<ITransactionFilter>()
                {
                    TransactionDateFilterVM,
                    TransactionTypeFilterVM,
                    TransactionCategoryFilterVM,
                    TransactionPriceFilterVM
                };

            foreach (var filter in _transactionFilters)
            {
                filter.FilterViewCall += FilterVM_FilterViewCall;
            }
        }

        private async void FilterVM_FilterViewCall(object? sender, EventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    IsBusy = true;
                    var filteredTransaction = TransactionListVM.AllTransactions.AsEnumerable();

                    if (_transactionFilters.Any(f => f.IsNotActive))
                    {
                        TransactionListVM.ReloadTransactions();
                    }

                    foreach (var filter in _transactionFilters)
                    {
                        if (filter.IsActive)
                        {
                            filteredTransaction = filter.Apply(filteredTransaction);
                        }
                    }
                    List<TransactionGroup> transactionGroups = filteredTransaction.ToList();

                    TransactionListVM.FilterTransactions(transactionGroups);
                    TransactionListVM.SearchTransactions();
                });
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void TransactionListVM_RefreshViewCall(object? sender, EventArgs e)
        {
            try
            {
                IsBusy = true;
                await ReloadData();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async override Task OnAppearingAsync()
        {
            try
            {
                IsBusy = true;
                IsVisible = true;

                await Task.Delay(100);

                if (_isInitialized)
                {
                    await ReloadData();
                }
                else
                {
                    _isInitialized = true;
                }

                ResetView();

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
            await HideAllFilters();
        }

        private async Task HideAllFilters()
        {
            foreach (var filter in _transactionFilters)
            {
                if (filter is FilterContentViewModelBase modelBase)
                {
                    if (modelBase.IsVisible)
                    {
                        await modelBase.ToggleVisibility();
                    }
                }
            }
        }

        partial void OnSearchQueryChanged(string? oldValue, string newValue)
        {
            if (TransactionListVM != null)
            {
                TransactionListVM.SearchText = SearchQuery;
                FilterVM_FilterViewCall(this, EventArgs.Empty);
            }
        }

        private void ResetView()
        {
            SearchQuery = string.Empty;
            TransactionListVM.ReloadTransactions();
        }

        private void TurnOffFilters()
        {
            foreach (var filter in _transactionFilters)
            {
                filter.IsActive = false;
            }
        }

        private void Refilter()
        {
            foreach (var filter in _transactionFilters)
            {
                filter.Reply();
            }
        }

        private async Task ReloadData()
        {
            var categoryTask = _transactionCategoryService.GetAllTransactionCategoriesAsync(_budgetService.CurrentBudget);
            var transactionTask = _transactionService.GetAllTransactionAsync(_budgetService.CurrentBudget);
            await Task.WhenAll(categoryTask, transactionTask);
            Refilter();
        }

        internal void PrepareForTransactionView()
        {
            throw new NotImplementedException();
        }
    }
}
