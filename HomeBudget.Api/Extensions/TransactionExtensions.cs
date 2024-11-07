using HomeBudget.Api.Entities;
using HomeBudget.Common.EntityDTOs.Transaction;

namespace HomeBudget.Api.Extensions
{
    public static class TransactionExtensions
    {
        public static Transaction ToTransaction(this TransactionCreateRequestModel request, Budget budget, TransactionCategory transactionCategory, string userId)
        {
            var transaction = new Transaction(budget, transactionCategory, userId)
            {
                Name = request.Name,
                Description = request.Description,
                TotalAmount = request.TotalAmount,
                Date = request.Date
            };

            return transaction;
        }

        public static TransactionCreateResponseModel ToCreateResponse(this Transaction transaction)
        {
            return new TransactionCreateResponseModel
            {
                Id = transaction.Id,
                CreatorId = transaction.CreatorId,
                TransactionCategoryId = transaction.TransactionCategoryId,
                BudgetId = transaction.BudgetId,
                Name = transaction.Name,
                Description = transaction.Description,
                TotalAmount = transaction.TotalAmount,
                Date = transaction.Date
            };
        }

        public static void FromUpdateRequest(this Transaction transaction, TransactionUpdateRequestModel request)
        {
            transaction.Name = request.Name;
            transaction.Description = request.Description;
            transaction.TotalAmount = request.TotalAmount;
            transaction.Date = request.Date;

            // TODO: Obsługa przenoszenia wydatku i zmiany jego kategori
            // Optionally: transaction.TransactionCategoryId = request.TransactionCategoryId;
            // Optionally: transaction.BudgetId = request.BudgetId;
        }

        public static TransactionUpdateResponseModel ToUpdateResponse(this Transaction transaction)
        {
            return new TransactionUpdateResponseModel
            {
                Id = transaction.Id,
                CreatorId = transaction.CreatorId,
                TransactionCategoryId = transaction.TransactionCategoryId,
                BudgetId = transaction.BudgetId,
                Name = transaction.Name,
                Description = transaction.Description,
                TotalAmount = transaction.TotalAmount,
                Date = transaction.Date
            };
        }

        public static TransactionGetResponseModel ToGetResponse(this Transaction transaction)
        {
            return new TransactionGetResponseModel
            {
                Id = transaction.Id,
                CreatorId = transaction.CreatorId,
                TransactionCategoryId = transaction.TransactionCategoryId,
                BudgetId = transaction.BudgetId,
                Name = transaction.Name,
                Description = transaction.Description,
                TotalAmount = transaction.TotalAmount,
                Date = transaction.Date
            };
        }
    }
}
