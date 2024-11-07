﻿using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Views;

namespace HomeBudget.App.ViewModels.ContentViewModels.FullViews
{
    public partial class ManageCurrentBudgetBalancePageViewModel : BaseViewModel
    {
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

        public ManageCurrentBudgetBalancePageViewModel()
        {
            Title = "Bilans";
        }

        public async override Task OnAppearingAsync()
        {
            try
            {
                IsBusy = true;
                IsVisible = true;


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
    }
}
