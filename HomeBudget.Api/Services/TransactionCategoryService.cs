using HomeBudget.Api.Extensions;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Api.UnitOfWork.Interfaces;
using HomeBudget.Common.EntityDTOs.TransactionCategory;

namespace HomeBudget.Api.Services
{
    public class TransactionCategoryService : ITransactionCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TransactionCategoryGetResponseModel>> GetTransactionCategoriesByBudgetIdAsync(string userId, string budgetId)
        {
            if(await HasAccessToBudgetAsync(userId, budgetId))
            {
                var transactionCategories = await _unitOfWork.TransactionCategories.GetTransactionCategoriesByBudgetIdAsync(budgetId);
                return transactionCategories.Select( e => e.ToGetResponse());
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<TransactionCategoryGetResponseModel?> GetTransactionCategoryByIdsAsync(string userId, string budgetId, string categoryId)
        {
            if(await HasAccessToBudgetAsync(userId, budgetId))
            {
                var transactionCategory = await _unitOfWork.TransactionCategories.GetTransactionCategoryByIdAsync(categoryId);
                return transactionCategory == null ? null : transactionCategory.ToGetResponse();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<TransactionCategoryCreateResponseModel?> CreateTransactionCategoryAsync(string userId, string budgetId, TransactionCategoryCreateRequestModel requestModel)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
                if (requestModel != null && budget != null)
                {
                    if(!await IsBudgetTransactionCategoryExistAsync(budgetId, requestModel.Name))
                    {
                        var transactionCategory = requestModel.ToTransactionCategory(budget);

                        await _unitOfWork.TransactionCategories.AddAsync(transactionCategory);
                        await _unitOfWork.SaveChangesAsync();
                    
                        return transactionCategory.ToCreateResponse();
                    }
                }
                return null;
            }
            else 
            { 
                throw new UnauthorizedAccessException(); 
            }
        }

        public async Task<IEnumerable<TransactionCategoryCreateResponseModel>> CreateTransactionCategoriesAsync(string userId, string budgetId, IEnumerable<TransactionCategoryCreateRequestModel> requestModels)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
                if (requestModels != null && budget != null)
                {
                    var responses = new List<TransactionCategoryCreateResponseModel>();
                    foreach (var requestModel in requestModels)
                    {
                        if (!await IsBudgetTransactionCategoryExistAsync(budgetId, requestModel.Name))
                        {
                            var transactionCategory = requestModel.ToTransactionCategory(budget);

                            await _unitOfWork.TransactionCategories.AddAsync(transactionCategory);
                            responses.Add(transactionCategory.ToCreateResponse());
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();

                    return responses;
                }
                return [];
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<TransactionCategoryUpdateResponseModel?> UpdateTransactionCategoryAsync(string userId, string budgetId, string categoryId, TransactionCategoryUpdateRequestModel requestModel)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var transactionCategory = await _unitOfWork.TransactionCategories.GetTransactionCategoryByIdAsync(categoryId);

                if (requestModel != null && transactionCategory != null)
                {
                    transactionCategory.FromUpdateRequest(requestModel);

                    _unitOfWork.TransactionCategories.Update(transactionCategory);
                    await _unitOfWork.SaveChangesAsync();

                    return transactionCategory.ToUpdateResponse();
                }
                return null;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteTransactionCategory(string userId, string budgetId, string categoryId)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var transactionCategory = await _unitOfWork.TransactionCategories.GetTransactionCategoryByIdAsync(categoryId);

                if (transactionCategory != null)
                {
                    _unitOfWork.TransactionCategories.Delete(transactionCategory);
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

        private async Task<bool> IsBudgetTransactionCategoryExistAsync(string budgetId, string name)
        {
            var transactionCategories = await _unitOfWork.TransactionCategories.GetTransactionCategoriesByBudgetIdAsync(budgetId);
            return transactionCategories.Any(e => e.Name == name);
        }

        private async Task<bool> HasAccessToBudgetAsync(string userId, string budgetId)
        {
            var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
            return budget != null && budget.Users.Any(u => u.Id.Equals(userId));
        }
    }
}
