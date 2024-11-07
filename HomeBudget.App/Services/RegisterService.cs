using HomeBudget.App.Services.Common;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.Account;

namespace HomeBudget.App.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IApiClient _apiClient;

        public RegisterService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> RegisterAsync(RegisterRequestModel requestModel)
        {
            var url = $"{ApiEndpoints.BaseAddress}{ApiEndpoints.Register}";
            var result = await _apiClient.PostAsync<RegisterRequestModel>(url, requestModel);
            return result;
        }
    }
}
