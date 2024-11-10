using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Extensions;
using HomeBudget.App.Models;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using HomeBudget.App.Views;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels
{
    public partial class BudgetsPageViewModel : BaseViewModel
    {
        [RelayCommand]
        public async Task ToggleBudgetIconSelect()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                BudgetIconSelectVM.ToggleVisibility();
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
        public async Task AddBudget()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                if (TemporaryBudget.ToCreateRequest().IsCreateRequestValid())
                {
                    if (await _budgetService.CreateBudgetAsync(TemporaryBudget))
                    {
                        await ReloadData();
                    }
                    else
                    {

                    }
                }
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

        [ObservableProperty]
        private IconSelectContentViewModel budgetIconSelectVM;

        [ObservableProperty]
        private SelectableButtonGroupViewModel selectableButtonGroupVM;

        [ObservableProperty]
        private ObservableCollection<SelectBudgetContentViewModel> selectBudgetVMs;

        [ObservableProperty]
        private Budget _temporaryBudget;

        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;

        public BudgetsPageViewModel(IUserService userService, IBudgetService budgetService)
        {
            _userService = userService;
            _budgetService = budgetService;

            Route = nameof(BudgetsPageAndroidView);
            // TODO: Przeniesienie do zasobów
            Title = "Budżety";
            IconUnicode = Icons.Budget;

            SelectableButtonGroupVM = new SelectableButtonGroupViewModel(
                new List<OptionItem> {
                    new OptionItem("Własne",BudgetType.Own),
                    new OptionItem("Współdzielone",BudgetType.Shared),
                });

            SelectBudgetVMs = new ObservableCollection<SelectBudgetContentViewModel>();

            BudgetIconSelectVM = new IconSelectContentViewModel();
            BudgetIconSelectVM.SelectedIconChanged += BudgetIconSelectVM_SelectedIconChanged; ;

            TemporaryBudget = new Budget() { Id = string.Empty, OwnerId = string.Empty };
            TemporaryBudget.IconUnicode = BudgetIconSelectVM.SelectedIconItem.Unicode;
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
            await Task.CompletedTask;
        }

        private async Task ReloadData()
        {
            await _budgetService.GetAllBudgetsAsync();
        }

        private void ResetView()
        {
            SelectBudgetVMs = new ObservableCollection<SelectBudgetContentViewModel>(_budgetService.Budgets.Select(b => new SelectBudgetContentViewModel(_budgetService, b)));

            if (!BudgetIconSelectVM.IsPopulated)
            {
                BudgetIconSelectVM.ReloadData();
            }
        }

        private void BudgetIconSelectVM_SelectedIconChanged(object? sender, EventArgs e)
        {
            if (BudgetIconSelectVM.SelectedIconItem != null)
            {
                TemporaryBudget.IconUnicode = BudgetIconSelectVM.SelectedIconItem.Unicode;
            }
        }

        public enum BudgetType
        {
            Own,
            Shared,
        }
    }
}

