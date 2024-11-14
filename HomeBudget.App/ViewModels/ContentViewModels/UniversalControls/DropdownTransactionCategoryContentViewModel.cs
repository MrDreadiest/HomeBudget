
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.UniversalControls
{
    public partial class DropdownTransactionCategoryContentViewModel : ObservableObject
    {
        public event EventHandler<List<TransactionCategory>>? SelectedTransactionCategoryChanged;

        [RelayCommand]
        public async Task Reload()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await ReloadDataAsync();
            }
            catch (Exception ex)
            {
                // TODO: Obsługuje błąd (opcjonalnie logowanie)
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
                    await ReloadDataAsync();
                }
            }
            catch (Exception ex)
            {
                // TODO: Obsługuje błąd (opcjonalnie logowanie)
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
                SelectedCategories = SelectedObjects.OfType<TransactionCategory>().ToList();
                SelectedTransactionCategoryChanged?.Invoke(this, SelectedCategories);
            }
        }

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _transactionCategories = new();

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _filteredTransactionCategories = new();

        [ObservableProperty]
        private TransactionCategory _selectedCategory;

        public List<TransactionCategory> SelectedCategories { get; private set; } = new();

        [ObservableProperty]
        private ObservableCollection<object> _selectedObjects = new();

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private bool _isVisible;

        [ObservableProperty]
        private bool _isEnable = true;

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

            SelectedCategory = new TransactionCategory() { Id = string.Empty, BudgetId = string.Empty };

            SelectionMode = selectionMode;
            IsAddMethodAvailable = isAddMethodAvailable;

            if (IsAddMethodAvailable)
            {
                AddInlistCategoryVM = new AddInlistCategoryContentViewModel();
            }
        }

        public void SelectTopInRange(int count)
        {
            SelectCategories(TransactionCategories.Take(count).Select(c => c.Id));
        }

        public void SelectCategories(IEnumerable<string> ids)
        {
            if (SelectionMode == SelectionMode.Multiple)
            {
                SelectedObjects.Clear();
                foreach (var id in ids)
                {
                    var category = TransactionCategories.FirstOrDefault(c => c.Id == id);
                    if (category != null)
                    {
                        SelectedObjects.Add(category);
                    }
                }
            }
            else if (SelectionMode == SelectionMode.Single)
            {
                var category = TransactionCategories.FirstOrDefault(c => c.Id == ids.First());
                if (category != null)
                {
                    SelectedCategory = category;
                }
            }
        }

        public void ResetView()
        {
            FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(TransactionCategories);

            SelectedCategory = new TransactionCategory() { Id = string.Empty, BudgetId = string.Empty };
            SelectedCategories.Clear();
            SelectedObjects.Clear();

            AddInlistCategoryVM?.ResetView();
            SearchText = string.Empty;
        }

        private async Task ReloadDataAsync()
        {
            var result = await _transactionCategoryService.GetAllTransactionCategoriesAsync(_budgetService.CurrentBudget);

            if (result)
            {
                TransactionCategories.Clear();
                foreach (var category in _budgetService.CurrentBudget.TransactionCategories)
                {
                    TransactionCategories.Add(category);
                }
            }

            FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(TransactionCategories);

            UpdateSelectedObjects();

            AddInlistCategoryVM?.ResetView();
        }

        private void UpdateSelectedObjects()
        {
            if (SelectionMode == SelectionMode.Multiple)
            {
                var selectedCategories = SelectedObjects.OfType<TransactionCategory>().ToList();
                SelectedObjects.Clear();
                foreach (var category in selectedCategories)
                {
                    var selectedCategory = FilteredTransactionCategories.FirstOrDefault(c => c.Id == category.Id);
                    if (selectedCategory != null)
                    {
                        SelectedObjects.Add(selectedCategory);
                    }
                }
            }
            else
            {
                SelectedCategory = FilteredTransactionCategories.FirstOrDefault(c => c.Id == SelectedCategory?.Id) ?? new TransactionCategory { Id = string.Empty, BudgetId = string.Empty };
            }
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
            SelectedTransactionCategoryChanged?.Invoke(newValue, new List<TransactionCategory> { newValue });
        }

        private void TransactionCategoryService_TransactionCategoryCreated(object? sender, TransactionCategory e)
        {
            TransactionCategories.Add(e);
            FilteredTransactionCategories.Add(e);
        }
    }
}
