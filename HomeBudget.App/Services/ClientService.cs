using HomeBudget.App.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace HomeBudget.App.Services
{
    public class ClientService
    {
        public static string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5070" : "http://localhost:5070";

        public const string ApiLogin = "/login";
        public const string ApiRegister = "/register";
        public const string ApiForgotPassword = "/forgotPassword";
        public const string ApiResetPassword = "/resetPassword";

        private readonly HttpClient httpClient;

        public ClientService()
        {
            httpClient = new HttpClient();
        }

        public async Task Register(Models.RegisterModel registerModel)
        {
            if (string.IsNullOrEmpty(registerModel?.Email) || string.IsNullOrEmpty(registerModel?.Password))
            {
                return;
            }
            var url = $"{BaseAddress}{ApiRegister}";

            var result = await httpClient.PostAsJsonAsync(url, registerModel);
            if (result.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("Title", "Message", "Cancel");
            }
            await Shell.Current.DisplayAlert("Title", result.ReasonPhrase, "Cancel");
        }

        public async Task Login(Models.LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel?.Email) || string.IsNullOrEmpty(loginModel?.Password))
            {
                return;
            }

            var url = $"{BaseAddress}{ApiLogin}";

            var result = await httpClient.PostAsJsonAsync(url, loginModel);
            var response = await result.Content.ReadFromJsonAsync<LoginResponse>();
            if (response is not null)
            {
                var serializeResponse = JsonSerializer.Serialize(
                    new LoginResponse() 
                    {
                        AccessToken = response.AccessToken,
                        RefreshToken = response.RefreshToken,
                        Email = loginModel.Email,
                    }
                );
                await SecureStorage.Default.SetAsync("Authentication", serializeResponse);
            }
        }

        public async Task Logout()
        {
            SecureStorage.Default.Remove("Authentication");
        }

        public async Task<UserInformation[]> GetAllUsers()
        {
            var serializedLoginResponseAuthentication = await SecureStorage.Default.GetAsync("Authentication");
            if (serializedLoginResponseAuthentication is null)
            {
                return [];
            }
            string token = JsonSerializer.Deserialize<LoginResponse>(serializedLoginResponseAuthentication)!.AccessToken!;

            var url = $"{BaseAddress}/api/User";

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.GetFromJsonAsync<UserInformation[]>(url);
            return result!;
        }
    }
}
