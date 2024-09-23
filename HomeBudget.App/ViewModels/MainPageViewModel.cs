using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeBudget.App.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
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
                GetUserNameFromSecurityStorage();
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
        public async Task Logout()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await clientService.Logout();
                IsAuthenticated = false;
                UserName = "Guest";
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
        private string userName;
        [ObservableProperty]
        private bool isAuthenticated;

        private readonly ClientService clientService;

        public MainPageViewModel(ClientService clientService)
        {
            this.clientService = clientService;
            RegisterModel = new RegisterModel();
            LoginModel = new LoginModel();
            IsAuthenticated = false;
            GetUserNameFromSecurityStorage();
        }

        private async void GetUserNameFromSecurityStorage()
        {
            if (!string.IsNullOrEmpty(UserName) && UserName! != "Guest")
            {
                IsAuthenticated = true;
                return;
            }
            var serializesLoginResponseInStorage = await SecureStorage.Default.GetAsync("Authentication");
            if(serializesLoginResponseInStorage != null)
            {
                UserName = JsonSerializer.Deserialize<LoginResponse>(serializesLoginResponseInStorage)!.Email!;
                IsAuthenticated = true;
                return ;
            }
            UserName = "Guest";
            IsAuthenticated = false;
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
