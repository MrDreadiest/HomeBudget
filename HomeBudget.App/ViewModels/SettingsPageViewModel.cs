using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.Views;
using HomeBudget.App.Views.Widgets;

namespace HomeBudget.App.ViewModels
{
    public partial class SettingsPageViewModel : BaseViewModel
    {
        [RelayCommand]
        public async Task Logout()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await _authenticationService.LogoutAsync();
                await _settingsService.DeleteIsRememberedSwitchOnAsync();
                await Shell.Current.GoToAsync($"//{nameof(LoginPageAndroidView)}");
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
        public async Task GoToManageFastBalance()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync($"//{nameof(ManageFastBalanceAndroidPageView)}");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }


        private readonly IAppSettingsService _settingsService;
        private readonly IAuthenticationService _authenticationService;

        public SettingsPageViewModel(IAuthenticationService authenticationService, IAppSettingsService settingsService)
        {
            _authenticationService = authenticationService;
            Route = nameof(SettingsPageAndroidView);
            //TODO : zasoby
            Title = "Ustawienia aplikacji";
            IconUnicode = Icons.Settings;
            _settingsService = settingsService;
        }

        public async override Task OnAppearingAsync()
        {
            IsVisible = true;
            await Task.CompletedTask;
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;
            await Task.CompletedTask;
        }
    }
}
