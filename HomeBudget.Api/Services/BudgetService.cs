using HomeBudget.Api.Entities;
using HomeBudget.Api.Extensions;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Api.UnitOfWork.Interfaces;
using HomeBudget.Common.EntityDTOs.Budget;

namespace HomeBudget.Api.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BudgetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BudgetGetResponseModel>> GetBudgetsAsync(string userId)
        {
            var budgets = await _unitOfWork.Budgets.GetBudgetsByUserIdAsync(userId);
            return budgets.Select(b => b.ToGetResponse());
        }

        public async Task<BudgetGetResponseModel?> GetBudgetByIdsAsync(string userId, string budgetId)
        {
            if(await HasAccessToBudgetAsync(userId, budgetId))
            {
                var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
                return budget == null ? null : budget.ToGetResponse();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<BudgetCreateResponseModel?> CreateBudgetAsync(string userId, BudgetCreateRequestModel requestModel)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user != null && requestModel != null) 
            {
                if (!await IsUserBudgetExistAsync(userId, requestModel.Name))
                {
                    var budget = requestModel.ToBudget(user);

                    await _unitOfWork.Budgets.AddAsync(budget);
                    await _unitOfWork.SaveChangesAsync();

                    return budget.ToCreateResponse();
                }
            }
            return null;
        }

        public async Task<BudgetUpdateResponseModel?> UpdateBudgetAsync(string userId, string budgetId, BudgetUpdateRequestModel requestModel)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);

                if (budget != null && requestModel != null) 
                {

                    budget.FromUpdateRequest(requestModel);

                    _unitOfWork.Budgets.Update(budget);
                    await _unitOfWork.SaveChangesAsync();

                    return budget.ToUpdateResponse();
                }
                return null;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteBudgetAsync(string userId, string budgetId)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);

                if (budget != null)
                {
                    _unitOfWork.Budgets.Delete(budget);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            else 
            {
                throw new UnauthorizedAccessException();
            }
        }

        private async Task<bool> IsUserBudgetExistAsync(string userId, string name)
        {
            var budgets = await _unitOfWork.Budgets.GetBudgetsByUserIdAsync(userId);
            return budgets.Any(b => b.Name == name && b.Users.Any(u => u.Id == userId));
        }

        private async Task<bool> HasAccessToBudgetAsync(string userId, string budgetId)
        {
            var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
            return budget != null && budget.Users.Any(u => u.Id.Equals(userId));
        }
    }
}
