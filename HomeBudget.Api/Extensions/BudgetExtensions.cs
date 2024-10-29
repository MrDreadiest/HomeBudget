using HomeBudget.Api.Entities;
using HomeBudget.Common.EntityDTOs.Budget;

namespace HomeBudget.Api.Extensions
{
    public static class BudgetExtensions
    {
        public static Budget ToBudget(this BudgetCreateRequestModel request, User user)
        {
            var budget = new Budget(user)
            {
                Name = request.Name,
                Description = request.Description,
                IconUnicode = request.IconUnicode
            };

            return budget;
        }

        public static BudgetCreateResponseModel ToCreateResponse(this Budget budget)
        {
            return new BudgetCreateResponseModel
            {
                Id = budget.Id,
                OwnerId = budget.OwnerId,
                Name = budget.Name,
                Description = budget.Description,
                IconUnicode = budget.IconUnicode
            };
        }

        public static void FromUpdateRequest(this Budget budget, BudgetUpdateRequestModel requestModel)
        {
            budget.Name = requestModel.Name;
            budget.Description = requestModel.Description;
            budget.IconUnicode = requestModel.IconUnicode;
        }

        public static BudgetUpdateResponseModel ToUpdateResponse(this Budget budget)
        {
            return new BudgetUpdateResponseModel
            {
                Id = budget.Id,
                OwnerId = budget.OwnerId,
                Name = budget.Name,
                Description = budget.Description,
                IconUnicode = budget.IconUnicode
            };
        }

        public static BudgetGetResponseModel ToGetResponse(this Budget budget)
        {
            return new BudgetGetResponseModel
            {
                Id = budget.Id,
                OwnerId = budget.OwnerId,
                Name = budget.Name,
                Description = budget.Description,
                IconUnicode = budget.IconUnicode
            };
        }
    }
}
