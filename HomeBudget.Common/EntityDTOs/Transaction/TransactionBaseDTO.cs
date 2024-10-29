namespace HomeBudget.Common.EntityDTOs.Transaction
{
    public class TransactionBaseDTO
    {
        public required string TransactionCategoryId { get; set; }
        public required string BudgetId { get; set; }
        public required string CreatorId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
