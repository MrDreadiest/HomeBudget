
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.UniversalControls
{
    public partial class DropdownTransactionCategoryContentViewModel : ObservableObject
    {
        public event EventHandler? SelectedTransactionCategoryChanged;

        [RelayCommand]
        public async Task Reload()
        {
            if (IsBusy)
                return;
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

        [RelayCommand]
        public async Task ToggleView()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                IsVisible = !IsVisible;

                if (IsVisible)
                {
                    await ReloadData();
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

        [RelayCommand]
        private void SelectionChanged()
        {
            if (SelectionMode == SelectionMode.Multiple)
            {
                SelectedCategories = SelectedObjects.Select(o => o as TransactionCategory).ToList();
                SelectedTransactionCategoryChanged?.Invoke(SelectedCategories, EventArgs.Empty);
            }
        }

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _transactionCategories;

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _filteredTransactionCategories;

        [ObservableProperty]
        private TransactionCategory _selectedCategory;

        public List<TransactionCategory> SelectedCategories;

        [ObservableProperty]
        private ObservableCollection<object> _selectedObjects;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        public bool IsNotBusy => !IsBusy;

        public SelectionMode SelectionMode { get; }

        [ObservableProperty]
        private bool _isAddMethodAvailable = false;

        public AddInlistCategoryContentViewModel AddInlistCategoryVM { get; set; }



        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;

        public DropdownTransactionCategoryContentViewModel() :
            this(App.Services.GetService<IBudgetService>()!, App.Services.GetService<ITransactionCategoryService>()!)
        {

        }

        public DropdownTransactionCategoryContentViewModel(bool isAddMethodAvailable = false) :
    this(App.Services.GetService<IBudgetService>()!, App.Services.GetService<ITransactionCategoryService>()!, SelectionMode.Single, isAddMethodAvailable)
        {

        }

        public DropdownTransactionCategoryContentViewModel(SelectionMode selectionMode, bool isAddMethodAvailable = false) :
            this(App.Services.GetService<IBudgetService>()!, App.Services.GetService<ITransactionCategoryService>()!, selectionMode, isAddMethodAvailable)
        {

        }

        public DropdownTransactionCategoryContentViewModel(IBudgetService budgetService, ITransactionCategoryService transactionCategoryService, SelectionMode selectionMode = SelectionMode.Single, bool isAddMethodAvailable = false)
        {
            _budgetService = budgetService;
            _transactionCategoryService = transactionCategoryService;

            _transactionCategoryService.TransactionCategoryCreated += TransactionCategoryService_TransactionCategoryCreated;

            SelectionMode = selectionMode;
            TransactionCategories = new ObservableCollection<TransactionCategory>();
            FilteredTransactionCategories = new ObservableCollection<TransactionCategory>();
            SelectedCategory = new TransactionCategory() { Id = string.Empty, BudgetId = string.Empty };
            SelectedObjects = new();
            SelectedCategories = new();

            IsAddMethodAvailable = isAddMethodAvailable;
            if (IsAddMethodAvailable)
            {
                AddInlistCategoryVM = new AddInlistCategoryContentViewModel();
            }
        }

        public void SelectTopInRange(int count)
        {
            if (SelectionMode == SelectionMode.Multiple)
            {
                _budgetService.CurrentBudget.TransactionCategories.GetRange(0, count).ForEach(t => { SelectedObjects.Add(t); });
                SelectedTransactionCategoryChanged?.Invoke(SelectedCategories, EventArgs.Empty);
            }
        }

        private void ResetView()
        {
            FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(TransactionCategories);

            SelectedCategory = new TransactionCategory() { Id = string.Empty, BudgetId = string.Empty };
            SelectedCategories.Clear();
            SelectedObjects.Clear();

            AddInlistCategoryVM.ResetView();
            SearchText = string.Empty;
        }

        private async Task ReloadData()
        {
            TransactionCategories.Clear();

            var result = await _transactionCategoryService.GetAllTransactionCategoriesAsync(_budgetService.CurrentBudget);

            if (result)
            {
                _budgetService.CurrentBudget.TransactionCategories.ForEach(c =>
                {
                    TransactionCategories.Add(c);
                });
            }

            ResetView();
        }

        private void FilterTransactionCategories(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(TransactionCategories);
            }
            else
            {
                var filteredItems = TransactionCategories
                .Where(tc => tc.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

                FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(filteredItems);
            }
        }

        partial void OnSearchTextChanging(string? oldValue, string newValue)
        {
            FilterTransactionCategories(newValue);
        }

        partial void OnSelectedCategoryChanged(TransactionCategory? oldValue, TransactionCategory newValue)
        {
            SelectedTransactionCategoryChanged?.Invoke(newValue, EventArgs.Empty);
        }

        private void TransactionCategoryService_TransactionCategoryCreated(object? sender, TransactionCategory e)
        {
            TransactionCategories.Add(e);
            if (string.IsNullOrEmpty(SearchText))
            {
                FilteredTransactionCategories.Add(e);
            }
            else
            {
                FilterTransactionCategories(SearchText);
            }
        }
    }
}
