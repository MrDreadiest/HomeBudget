using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Extensions;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;

namespace HomeBudget.App.ViewModels.ContentViewModels.UniversalControls
{
    public partial class AddInlistCategoryContentViewModel : ObservableObject, ICollapseContentViewModel
    {
        [RelayCommand]
        public async Task AddCategory()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await CreateCategory();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas wykonywania akcji.", "OK");
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

        [RelayCommand]
        public void ToggleIconSelect()
        {
            IconSelectVM.ToggleVisibility();
        }

        [ObservableProperty]
        private bool _isCollapsed;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private bool _isBusy;

        public IconSelectContentViewModel IconSelectVM { get; }
        public TransactionCategory TemporaryCategory { get; set; }

        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;

        public AddInlistCategoryContentViewModel() : this(App.Services.GetService<IBudgetService>()!, App.Services.GetService<ITransactionCategoryService>()!)
        {
        }

        public AddInlistCategoryContentViewModel(IBudgetService budgetService, ITransactionCategoryService transactionCategoryService)
        {
            //TODO: zasoby
            Title = "Dodaj kategorię";
            IsCollapsed = true;

            _budgetService = budgetService;
            _transactionCategoryService = transactionCategoryService;

            IconSelectVM = new IconSelectContentViewModel();
            IconSelectVM.SelectedIconChanged += IconSelectVM_SelectedIconChanged;
            TemporaryCategory = new TransactionCategory()
            {
                BudgetId = string.Empty,
                Id = string.Empty,
                IconUnicode = string.Empty,
            };
        }

        public void ResetView()
        {
            if (!IconSelectVM.IsPopulated)
            {
                IconSelectVM.ReloadData();
            }

            IconSelectVM.ResetView();

            TemporaryCategory.BudgetId = string.Empty;
            TemporaryCategory.Id = string.Empty;
            TemporaryCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
            TemporaryCategory.Name = string.Empty;
        }

        private async Task CreateCategory()
        {
            if (TemporaryCategory.ToCreateRequest().IsRequestValid())
            {
                bool result = await _transactionCategoryService.CreateTransactionCategoryAsync(_budgetService.CurrentBudget, TemporaryCategory);

                if (result)
                {
                    ResetView();
                }
                else
                {
                    //TODO: zasoby
                    await Shell.Current.DisplayAlert("Ups... Coś poszło nie tak.", "Nie udało się dodać kategorii.", "OK");
                }
            }
        }

        private void IconSelectVM_SelectedIconChanged(object? sender, EventArgs e)
        {
            TemporaryCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
        }
    }
}
