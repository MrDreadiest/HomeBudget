using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget.App.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        [RelayCommand]
        public async Task Register()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await clientService.Register(RegisterModel);
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
                await clientService.Login(LoginModel);
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
        private RegisterModel registerModel;

        [ObservableProperty]
        private LoginModel loginModel;

        [ObservableProperty]
        private string email;
        [ObservableProperty]
        private bool isAuthenticated;

        private readonly ClientService clientService;

        public LoginPageViewModel(ClientService clientService)
        {
            this.clientService = clientService;
            RegisterModel = new RegisterModel();
            LoginModel = new LoginModel();

        }

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }

        public override Task OnDisappearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
