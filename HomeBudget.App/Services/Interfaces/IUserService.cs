﻿using HomeBudget.App.Models;

namespace HomeBudget.App.Services.Interfaces
{
    public interface IUserService
    {
        User CurrentUser { get; }
        event EventHandler CurrentUserChanged;
        Task<bool> GetUserAsync();
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync();
    }
}
