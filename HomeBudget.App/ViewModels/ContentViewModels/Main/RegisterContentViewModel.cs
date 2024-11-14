using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.Account;

namespace HomeBudget.App.ViewModels.ContentViewModels.Main
{
    public partial class RegisterContentViewModel : MainCarouselItemViewModelBase
    {
        public event EventHandler<(string email, string password)> OnRegisterContent;

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
                        OnRegisterContent?.Invoke(this, (RegisterModel.Email, RegisterModel.Password));
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
        }

        [RelayCommand]
        public void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        [RelayCommand]
        public void ToggleRepeatPasswordVisibility()
        {

            IsRepeatPasswordVisible = !IsRepeatPasswordVisible;
        }

        [ObservableProperty]
        private RegisterRequestModel _registerModel;

        [ObservableProperty]
        private string _repeatedPassword;

        [ObservableProperty]
        private bool _isPasswordVisible;

        [ObservableProperty]
        private bool _isRepeatPasswordVisible;

        [ObservableProperty]
        private string _passwordVisibleIcon;

        [ObservableProperty]
        private string _repeatPasswordVisibleIcon;

        private readonly IRegisterService _registerService;

        public RegisterContentViewModel(IRegisterService registerService)
        {
            _registerService = registerService;

            RegisterModel = new RegisterRequestModel();
            IsPasswordVisible = IsRepeatPasswordVisible = true;
            RepeatedPassword = string.Empty;
            PasswordVisibleIcon = RepeatPasswordVisibleIcon = Icons.Visibility;
        }

        public RegisterContentViewModel() : this(App.Services.GetService<IRegisterService>()!)
        {

        }

        public async override Task ResetView()
        {
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
