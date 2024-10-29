namespace HomeBudget.Common.EntityDTOs.TransactionCategory
{
    public sealed class TransactionCategoryGetResponseModel : TransactionCategoryBaseDTO
    {
        public required string Id { get; set; }
        public required string BudgetId { get; set; }
    }
}
