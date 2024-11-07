using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.Views;
using HomeBudget.Common.EntityDTOs.Account;

namespace HomeBudget.App.ViewModels
{
    public partial class RegisterPageViewModel : BaseViewModel
    {
        [RelayCommand]
        public async Task GoToLogin()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync($"//{nameof(LoginPageAndroidView)}");
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
        public async Task Register()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                if (RegisterModel.Password.Equals(RepeatedPassword))
                {
                    var result = await _registerService.RegisterAsync(RegisterModel);  
                    if (result)
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoginPageAndroidView)}");
                    }
                    else
                    {
                        // TODO: Przenieść do zasobów
                        await Shell.Current.DisplayAlert("Błąd formularza", "Nieudana próba rejestracji.", "Ok");
                    }
                    
                }
                else
                {
                    // TODO: Przenieść do zasobów
                    await Shell.Current.DisplayAlert("Błąd formularza", "Podane hasła nie są jednakowe.", "Ok");
                }

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

        [RelayCommand]
        public async Task ToggleRepeatPasswordVisibility()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                IsRepeatPasswordVisible = !IsRepeatPasswordVisible;
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
        private RegisterRequestModel registerModel;

        [ObservableProperty]
        private string repeatedPassword;

        [ObservableProperty]
        private bool isPasswordVisible;

        [ObservableProperty]
        private bool isRepeatPasswordVisible;

        [ObservableProperty]
        private string passwordVisibleIcon;

        [ObservableProperty]
        private string repeatPasswordVisibleIcon;

        private readonly IRegisterService _registerService;

        public RegisterPageViewModel(IRegisterService registerService)
        {
            _registerService = registerService;
            RegisterModel = new RegisterRequestModel();
            IsPasswordVisible = IsRepeatPasswordVisible = true;
            RepeatedPassword = string.Empty;
            PasswordVisibleIcon = RepeatPasswordVisibleIcon = Icons.Visibility;
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

        partial void OnIsPasswordVisibleChanged(bool value)
        {
            PasswordVisibleIcon = value == true ? Icons.Visibility : Icons.VisibilityOff;
        }

        partial void OnIsRepeatPasswordVisibleChanged(bool value)
        {
            RepeatPasswordVisibleIcon = value == true ? Icons.Visibility : Icons.VisibilityOff;
        }
    }
}