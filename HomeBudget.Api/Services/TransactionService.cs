using HomeBudget.Api.Extensions;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Api.UnitOfWork.Interfaces;
using HomeBudget.Common.EntityDTOs.Transaction;

namespace HomeBudget.Api.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TransactionGetResponseModel>> GetTransactionsByBudgetIdAsync(string userId, string budgetId)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var transactions = await _unitOfWork.Transactions.GetTransactionsByBudgetIdAsync(budgetId);
                return transactions.Select(t => t.ToGetResponse());
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<IEnumerable<TransactionGetResponseModel>> GetTransactionsByBudgetIdInDateRangeAsync(string userId, string budgetId, DateTime? startDate, DateTime? endDate)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var transactions = await _unitOfWork.Transactions.GetTransactionsByBudgetIdInDateRangeAsync(
                    budgetId, startDate, endDate);

                return transactions.Select(t => t.ToGetResponse());
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<TransactionGetResponseModel?> GetTransactionByIdsAsync(string userId, string budgetId, string transactionId)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var transaction = await _unitOfWork.Transactions.GetTransactionByIdAsync(transactionId);
                return transaction == null ? null : transaction.ToGetResponse();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }


        public async Task<TransactionCreateResponseModel?> CreateTransactionAsync(string userId, string budgetId, TransactionCreateRequestModel requestModel)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
                var transactionCategory = await _unitOfWork.TransactionCategories.GetTransactionCategoryByIdAsync(requestModel.TransactionCategoryId);

                if (requestModel != null && budget != null && transactionCategory != null)
                {
                    var transaction = requestModel.ToTransaction(budget, transactionCategory, userId);

                    await _unitOfWork.Transactions.AddAsync(transaction);
                    await _unitOfWork.SaveChangesAsync();

                    return transaction.ToCreateResponse();
                }
                return null;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<IEnumerable<TransactionCreateResponseModel>> CreateTransactionsAsync(string userId, string budgetId, IEnumerable<TransactionCreateRequestModel> requestModels)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
                var response = new List<TransactionCreateResponseModel>();

                if (budget != null)
                {
                    foreach (var requestModel in requestModels)
                    {
                        var transactionCategory = await _unitOfWork.TransactionCategories.GetTransactionCategoryByIdAsync(requestModel.TransactionCategoryId);

                        if (requestModel != null && transactionCategory != null)
                        {
                            var transaction = requestModel.ToTransaction(budget, transactionCategory, userId);
                            response.Add(transaction.ToCreateResponse());
                            await _unitOfWork.Transactions.AddAsync(transaction);
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
                return response;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<TransactionUpdateResponseModel?> UpdateTransactionAsync(string userId, string budgetId, string transactionId, TransactionUpdateRequestModel requestModel)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
                var transaction = await _unitOfWork.Transactions.GetTransactionByIdAsync(transactionId);

                if (requestModel != null && budget != null && transaction != null)
                {
                    if (transaction.TransactionCategoryId != requestModel.TransactionCategoryId)
                    {
                        var newTransactionCategory = await _unitOfWork.TransactionCategories.GetTransactionCategoryByIdAsync(requestModel.TransactionCategoryId);

                        if (newTransactionCategory != null)
                        {
                            transaction.TransactionCategory.Transactions.Remove(transaction);
                            transaction.TransactionCategory = newTransactionCategory;
                            newTransactionCategory.Transactions.Add(transaction);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    transaction.FromUpdateRequest(requestModel);
                }
                return null;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteTransaction(string userId, string budgetId, string transactionId)
        {
            if (await HasAccessToBudgetAsync(userId, budgetId))
            {
                var transaction = await _unitOfWork.Transactions.GetTransactionByIdAsync(transactionId);

                if (transaction != null)
                {
                    _unitOfWork.Transactions.Delete(transaction);

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

        private async Task<bool> HasAccessToBudgetAsync(string userId, string budgetId)
        {
            var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
            return budget != null && budget.Users.Any(u => u.Id.Equals(userId));
        }
    }
}
