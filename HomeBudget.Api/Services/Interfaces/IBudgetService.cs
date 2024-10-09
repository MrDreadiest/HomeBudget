using HomeBudget.Common.EntityDTOs.Budget;

namespace HomeBudget.Api.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<IEnumerable<BudgetGetResponseModel>> GetBudgetsAsync(string userId);
        Task<BudgetGetResponseModel?> GetBudgetByIdsAsync(string userId, string budgetId);
        Task<BudgetCreateResponseModel?> CreateBudgetAsync(string userId, BudgetCreateRequestModel requestModel);
        Task<BudgetUpdateResponseModel?> UpdateBudgetAsync(string userId, string budgetId, BudgetUpdateRequestModel requestModel);
        Task<bool> DeleteBudgetAsync(string userId, string budgetId);
    }
}
