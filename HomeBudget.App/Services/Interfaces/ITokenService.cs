﻿using HomeBudget.Common.EntityDTOs.Account;

namespace HomeBudget.App.Services.Interfaces
{
    public interface ITokenService
    {
        Task SaveTokensAsync(LoginResponseModel responseModel);
        Task<string> GetAccessTokenAsync();
        Task<string> GetRefreshTokenAsync();
        Task<int> GetExpiresInAsync();
        Task<string> GetTokenTypeAsync();
        Task RemoveTokensAsync();
    }
}
