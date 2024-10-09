namespace HomeBudget.Common.EntityDTOs.Expense
{
    public class ExpenseBaseDTO
    {
        public required string ExpenseCategoryId { get; set; }
        public required string BudgetId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
