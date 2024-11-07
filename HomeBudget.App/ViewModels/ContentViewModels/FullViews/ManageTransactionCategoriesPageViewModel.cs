using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Extensions;
using HomeBudget.App.Models;
using HomeBudget.App.Models.TransactionCategories;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using HomeBudget.App.Views;
using HomeBudget.App.Views.ContentViews.FullViews;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.FullViews
{
    public partial class ManageTransactionCategoriesPageViewModel : BaseViewModel
    {
        #region Commands
        [RelayCommand]
        public async Task SelectCategory(TransactionCategory transactionCategory)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                SelectableButtonGroupVM.SelectOption(SelectableButtonGroupVM.Options.First(b => (TransactionCategoryModifyActionType)b.Type == TransactionCategoryModifyActionType.Update));

                TemporaryCategory = new TransactionCategory()
                {
                    BudgetId = transactionCategory.BudgetId,
                    Id = transactionCategory.Id,
                    Name = transactionCategory.Name,
                    IconUnicode = transactionCategory.IconUnicode
                };
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task ToggleIconSelect()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                IconSelectVM.ToggleVisibility();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task ProcessAction()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                if (SelectableButtonGroupVM.SelectedType is TransactionCategoryModifyActionType actionType)
                {
                    switch (actionType)
                    {
                        case TransactionCategoryModifyActionType.Create:
                            await CreateCategory();
                            break;
                        case TransactionCategoryModifyActionType.Update:
                            await UpdateCategory();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas wykonywania akcji.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task RemoveCategory(TransactionCategory transactionCategory)
        {
            if (IsBusy)
                return;

            bool isConfirmed = await Shell.Current.DisplayAlert(
                "Potwierdzenie usunięcia",
                "Czy na pewno chcesz usunąć tę kategorię?",
                "Tak",
                "Nie"
            );

            if (!isConfirmed)
                return;

            try
            {
                IsBusy = true;

                var result = await _transactionCategoryService.DeleteTransactionCategoryAsync(_budgetService.CurrentBudget, transactionCategory);

                if (result)
                {
                    Categories.Remove(transactionCategory);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Błąd", "Nie udało się usunąć kategorii.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas usuwania kategorii.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task Reload()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await ReloadCategories();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task Back()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync($"//{nameof(DashboardPageAndroidView)}", false);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }
        #endregion

        [ObservableProperty]
        private SelectableButtonGroupViewModel _selectableButtonGroupVM;

        [ObservableProperty]
        private IconSelectContentViewModel _iconSelectVM;

        [ObservableProperty]
        private TransactionCategory _temporaryCategory;

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _categories;

        [ObservableProperty]
        private SwipeView _currentOpenSwipeView;

        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;

        public ManageTransactionCategoriesPageViewModel(IBudgetService budgetService, ITransactionCategoryService transactionCategoryService)
        {
            _budgetService = budgetService;
            _transactionCategoryService = transactionCategoryService;

            //TODO: Przeniesienie do zasobów
            Route = nameof(ManageCategoriesAndroidPageView);
            Title = "Zarządzanie kategorią";

            TemporaryCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };
            Categories = new ObservableCollection<TransactionCategory>();

            SelectableButtonGroupVM = new SelectableButtonGroupViewModel(Enum.GetValues(typeof(TransactionCategoryModifyActionType)).Cast<TransactionCategoryModifyActionType>().Select(c => new OptionItem(c.GetDescription(), c)).ToList());
            SelectableButtonGroupVM.SelectedChanged += SelectableButtonGroupVM_SelectedChanged;

            IconSelectVM = new IconSelectContentViewModel();
            IconSelectVM.SelectedIconChanged += IconSelectVM_SelectedIconChanged;
            TemporaryCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
        }

        public async override Task OnAppearingAsync()
        {
            try
            {
                IsBusy = true;
                IsVisible = true;

                await ReloadCategories();
                ResetView();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;
            await Task.CompletedTask;
        }

        public void ToggleSwipeView(SwipeView swipeView)
        {
            if (CurrentOpenSwipeView != null && CurrentOpenSwipeView != swipeView)
            {
                CurrentOpenSwipeView.Close();
            }

            CurrentOpenSwipeView = swipeView;
        }

        private void ResetView()
        {
            if (!IconSelectVM.IsPopulated)
            {
                IconSelectVM.PopulateIconCollection();
            }

            TemporaryCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };
            TemporaryCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
        }

        private async Task ReloadCategories()
        {
            await _transactionCategoryService.GetAllTransactionCategoriesAsync(_budgetService.CurrentBudget);
            Categories = new ObservableCollection<TransactionCategory>(_budgetService.CurrentBudget.TransactionCategories);
        }

        private async Task CreateCategory()
        {
            if (TemporaryCategory.ToCreateRequest().IsRequestValid())
            {
                bool result = await _transactionCategoryService.CreateTransactionCategoryAsync(_budgetService.CurrentBudget, TemporaryCategory);

                if (result)
                {
                    TemporaryCategory = new TransactionCategory()
                    {
                        BudgetId = string.Empty,
                        Id = string.Empty,
                        IconUnicode = IconSelectVM.SelectedIconItem.Unicode,
                    };

                    await ReloadCategories();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Ups... Coś poszło nie tak.", "Nie udało się dodać kategorii.", "OK");
                }
            }
        }

        private async Task UpdateCategory()
        {
            if (!TemporaryCategory.IsNullOrEmpty())
            {
                bool result = await _transactionCategoryService.UpdateTransactionCategoryAsync(_budgetService.CurrentBudget, TemporaryCategory);

                if (result)
                {
                    TemporaryCategory = new TransactionCategory()
                    {
                        BudgetId = string.Empty,
                        Id = string.Empty,
                        IconUnicode = IconSelectVM.SelectedIconItem.Unicode,
                    };

                    await ReloadCategories();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Ups... Coś poszło nie tak.", "Nie udało się zmodyfikować kategorii.", "OK");
                }
            }
        }

        private void IconSelectVM_SelectedIconChanged(object? sender, EventArgs e)
        {
            TemporaryCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
        }

        private void SelectableButtonGroupVM_SelectedChanged(object? sender, EventArgs e)
        {
            if (sender is SelectableButtonGroupViewModel selectableVM)
            {
                if (selectableVM.SelectedType is TransactionCategoryModifyActionType actionType)
                {
                    switch (actionType)
                    {
                        case TransactionCategoryModifyActionType.Create:
                            ResetView();
                            break;
                        case TransactionCategoryModifyActionType.Update:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
