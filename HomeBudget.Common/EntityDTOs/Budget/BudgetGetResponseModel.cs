namespace HomeBudget.Common.EntityDTOs.Budget
{
    public sealed class BudgetGetResponseModel : BudgetBaseDTO
    {
        public required string Id { get; set; }
        public required string OwnerId { get; set; }
    }
}
