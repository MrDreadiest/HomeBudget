using HomeBudget.Api.Entities;
using HomeBudget.Common.EntityDTOs.Budget;

namespace HomeBudget.Api.Extensions
{
    public static class BudgetExtensions
    {
        public static BudgetGetResponseModel ToBudgetGetResponseModel(this Budget budget)
        {
            return new BudgetGetResponseModel
            {
                Id = budget.Id,
                Name = budget.Name,
                Description = budget.Description
            };
        }
    }
}
