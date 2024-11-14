using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Extensions;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.Views;
using HomeBudget.Common.EntityDTOs.Account;

namespace HomeBudget.App.ViewModels.ContentViewModels.Main
{
    public partial class LoginContentViewModel : MainCarouselItemViewModelBase
    {
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
                        _ = Task.Run(() => { _appSettingsService.SetRememberedUserEmailAsync(Email); });
                        _ = Task.Run(() => { _appSettingsService.SetRememberedUserPasswordAsync(Password); });
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
        }

        [RelayCommand]
        public void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private bool _isPasswordVisible;

        [ObservableProperty]
        private string _passwordVisibleIcon;

        [ObservableProperty]
        private bool _isRemembered;

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IAppSettingsService _appSettingsService;
        private readonly IBudgetService _budgetService;

        public LoginContentViewModel(IAuthenticationService authenticationService, IUserService userService, IAppSettingsService appSettingsService, IBudgetService budgetService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _appSettingsService = appSettingsService;
            _budgetService = budgetService;

            IsPasswordVisible = true;

        }

        public LoginContentViewModel() : this(
            App.Services.GetService<IAuthenticationService>()!,
            App.Services.GetService<IUserService>()!,
            App.Services.GetService<IAppSettingsService>()!,
            App.Services.GetService<IBudgetService>()!)
        {
        }

        public async override Task ResetView()
        {
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
