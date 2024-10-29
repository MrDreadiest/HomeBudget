using HomeBudget.Api.Entities;
using HomeBudget.Common.EntityDTOs.TransactionCategory;

namespace HomeBudget.Api.Extensions
{
    public static class TransactionCategoryExtensions
    {
        public static TransactionCategory ToTransactionCategory(this TransactionCategoryCreateRequestModel request, Budget budget)
        {
            var transactionCategory = new TransactionCategory(budget)
            {
                Name = request.Name,
                IconUnicode = request.IconUnicode,
            };

            return transactionCategory;
        }

        public static TransactionCategoryCreateResponseModel ToCreateResponse(this TransactionCategory transactionCategory)
        {
            return new TransactionCategoryCreateResponseModel
            {
                Id = transactionCategory.Id,
                BudgetId = transactionCategory.BudgetId,
                Name = transactionCategory.Name,
                IconUnicode = transactionCategory.IconUnicode,
            };
        }

        public static void FromUpdateRequest(this TransactionCategory transactionCategory, TransactionCategoryUpdateRequestModel request)
        {
            transactionCategory.Name = request.Name;
            transactionCategory.IconUnicode = request.IconUnicode;
        }

        public static TransactionCategoryUpdateResponseModel ToUpdateResponse(this TransactionCategory transactionCategory)
        {
            return new TransactionCategoryUpdateResponseModel
            {
                Id = transactionCategory.Id,
                BudgetId = transactionCategory.BudgetId,
                Name = transactionCategory.Name,
                IconUnicode = transactionCategory.IconUnicode,
            };
        }

        public static TransactionCategoryGetResponseModel ToGetResponse(this TransactionCategory transactionCategory)
        {
            return new TransactionCategoryGetResponseModel
            {
                Id = transactionCategory.Id,
                BudgetId = transactionCategory.BudgetId,
                Name = transactionCategory.Name,
                IconUnicode = transactionCategory.IconUnicode,
            };
        }
    }
}
