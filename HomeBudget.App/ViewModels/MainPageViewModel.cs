using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Extensions;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.Views;
using HomeBudget.Common.EntityDTOs.Account;

namespace HomeBudget.App.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        [RelayCommand]
        public async Task GoToRegister()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync($"//{nameof(RegisterPageAndroidView)}");
            }
            catch
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task Login()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var result = await _authenticationService.LoginAsync(new LoginRequestModel() { Email = Email, Password = Password });
                if (result)
                {
                    if (IsRemembered)
                    {
                        await _appSettingsService.SetRememberedUserEmailAsync(Email);
                        await _appSettingsService.SetRememberedUserPasswordAsync(Password);
                    }

                    if (_userService.CurrentUser.IsAccountSetup)
                    {

                        if (_budgetService.CurrentBudget.IsNullOrEmpty())
                        {
                            await Shell.Current.GoToAsync($"//{nameof(BudgetsPageAndroidView)}");
                        }
                        else
                        {
                            await Shell.Current.GoToAsync($"//{nameof(DashboardPageAndroidView)}");
                        }
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(UserAccountSetupPageAndroidView)}");
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

        [RelayCommand]
        public async Task GoToPasswordReminder()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync($"//{nameof(PasswordReminderPageAndroidView)}");
            }
            catch
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task TogglePasswordVisibility()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                IsPasswordVisible = !IsPasswordVisible;
            }
            catch
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private bool isPasswordVisible;

        [ObservableProperty]
        private string passwordVisibleIcon;

        [ObservableProperty]
        private bool isRemembered;

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IAppSettingsService _appSettingsService;
        private readonly IBudgetService _budgetService;

        public MainPageViewModel(IAuthenticationService authenticationService, IUserService userService, IAppSettingsService appSettingsService, IBudgetService budgetService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _appSettingsService = appSettingsService;
            _budgetService = budgetService;

            IsPasswordVisible = true;
        }

        public async override Task OnAppearingAsync()
        {
            IsVisible = true;

            IsRemembered = await _appSettingsService.GetIsRememberedSwitchOnAsync();

            if (IsRemembered)
            {
                string rememberedPassword = await _appSettingsService.GetRememberedUserPasswordAsync();
                string rememberedEmail = await _appSettingsService.GetRememberedUserEmailAsync();

                if (!string.IsNullOrEmpty(rememberedEmail) && !string.IsNullOrEmpty(rememberedPassword))
                {
                    Password = rememberedPassword;
                    Email = rememberedEmail;
                }
            }
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;
            await Task.CompletedTask;
        }

        partial void OnIsRememberedChanged(bool oldValue, bool newValue)
        {
            Task.Run(async () => { await _appSettingsService.SetIsRememberedSwitchOnAsync(newValue); });
        }

        partial void OnIsPasswordVisibleChanged(bool value)
        {
            PasswordVisibleIcon = value == true ? Icons.Visibility : Icons.VisibilityOff;
        }
    }
}
