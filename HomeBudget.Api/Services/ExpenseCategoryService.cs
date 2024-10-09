using HomeBudget.Api.Entities;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Api.UnitOfWork.Interfaces;
using HomeBudget.Common.EntityDTOs.ExpenseCategory;

namespace HomeBudget.Api.Services
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpenseCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ExpenseCategoryGetResponseModel>> GetExpenseCategoriesByBudgetIdAsync(string userId, string budgetId)
        {
            if(await HasAccessToBudgetAsync(userId, budgetId))
            {
                var expenseCategories = await _unitOfWork.ExpenseCategories.GetExpenseCategoriesByBudgetIdAsync(budgetId);

                return expenseCategories.Select( e => new ExpenseCategoryGetResponseModel() { Id = e.Id, Name = e.Name});
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<ExpenseCategoryGetResponseModel?> GetExpenseCategoryByIdsAsync(string userId, string budgetId, string categoryId)
        {
            if(await HasAccessToBudgetAsync(userId, budgetId))
            {
                var expenseCategory = await _unitOfWork.ExpenseCategories.GetExpenseCategoryByIdAsync(categoryId);
                
                return expenseCategory == null ? null : new ExpenseCategoryGetResponseModel() { Id = expenseCategory.Id, Name = expenseCategory.Name };
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<ExpenseCategoryCreateResponseModel?> CreateExpenseCategoriesAsync(string userId, string budgetId, ExpenseCategoryCreateRequestModel requestModel)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
                if (requestModel != null && budget != null)
                {
                    if(!await IsBudgetExpenseCategoryExistAsync(budgetId, requestModel.Name))
                    {
                        var expenseCategory = new ExpenseCategory(requestModel.Name, budget);

                        await _unitOfWork.ExpenseCategories.AddAsync(expenseCategory);
                        await _unitOfWork.SaveChangesAsync();
                    
                        return new ExpenseCategoryCreateResponseModel() { Id = expenseCategory.Id, Name = expenseCategory.Name };
                    }
                }
                return null;
            }
            else 
            { 
                throw new UnauthorizedAccessException(); 
            }
        }

        public async Task<ExpenseCategoryUpdateResponseModel?> UpdateExpenseCategoriesAsync(string userId, string budgetId, string categoryId, ExpenseCategoryUpdateRequestModel requestModel)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var expenseCategory = await _unitOfWork.ExpenseCategories.GetExpenseCategoryByIdAsync(categoryId);

                if (requestModel != null && expenseCategory != null)
                {
                    expenseCategory.Name = requestModel.Name;

                    _unitOfWork.ExpenseCategories.Update(expenseCategory);
                    await _unitOfWork.SaveChangesAsync();

                    return new ExpenseCategoryUpdateResponseModel() { Id = expenseCategory.Id, Name = expenseCategory.Name };
                }
                return null;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteExpenseCategory(string userId, string budgetId, string categoryId)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var expenseCategory = await _unitOfWork.ExpenseCategories.GetExpenseCategoryByIdAsync(categoryId);

                if (expenseCategory != null)
                {
                    _unitOfWork.ExpenseCategories.Delete(expenseCategory);
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

        private async Task<bool> IsBudgetExpenseCategoryExistAsync(string budgetId, string name)
        {
            var expenseCategories = await _unitOfWork.ExpenseCategories.GetExpenseCategoriesByBudgetIdAsync(budgetId);
            return expenseCategories.Any(e => e.Name == name);
        }

        private async Task<bool> HasAccessToBudgetAsync(string userId, string budgetId)
        {
            var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
            return budget != null && budget.Users.Any(u => u.Id.Equals(userId));
        }
    }
}
