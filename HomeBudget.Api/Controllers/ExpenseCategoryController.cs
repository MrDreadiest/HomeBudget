using HomeBudget.Api.Entities;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.ExpenseCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.Api.Controllers
{
    [Route("api/User/Budget/{budgetId}/[controller]")]
    [ApiController]
    public class ExpenseCategoryController : ControllerBase
    {
        private readonly IExpenseCategoryService _expenseCategoryService;
        private readonly UserManager<User> _userManager;

        public ExpenseCategoryController(IExpenseCategoryService expenseService, UserManager<User> userManager)
        {
            _expenseCategoryService = expenseService;
            _userManager = userManager;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ExpenseCategoryGetResponseModel>>> GetAllExpenseCategoriesInBudget(string budgetId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var expenseCategories = await _expenseCategoryService.GetExpenseCategoriesByBudgetIdAsync(authorization.User!.Id, budgetId);
                return Ok(expenseCategories);
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    return Unauthorized(ex.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("{categoryId}"), Authorize]
        public async Task<ActionResult<ExpenseCategoryGetResponseModel>> GetExpenseCategoryById(string budgetId, string categoryId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var expenseCategory = await _expenseCategoryService.GetExpenseCategoryByIdsAsync(authorization.User!.Id, budgetId, categoryId);
                return expenseCategory == null ? NotFound() : Ok(expenseCategory);
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    return Unauthorized(ex.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<ExpenseCategoryCreateResponseModel>> CreateExpenseCategory(string budgetId, [FromBody] ExpenseCategoryCreateRequestModel requestModel)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _expenseCategoryService.CreateExpenseCategoriesAsync(authorization.User!.Id, budgetId, requestModel);
                return response == null ? BadRequest() : Ok(response);
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    return Unauthorized(ex.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("{categoryId}"), Authorize]
        public async Task<ActionResult<ExpenseCategoryUpdateResponseModel>> UpdateExpenseCategory(string budgetId, string categoryId, [FromBody] ExpenseCategoryUpdateRequestModel requestModel)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _expenseCategoryService.UpdateExpenseCategoriesAsync(authorization.User!.Id, budgetId, categoryId, requestModel);
                return response == null ? BadRequest() : Ok(response);
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    return Unauthorized(ex.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("{categoryId}"), Authorize]
        public async Task<IActionResult> DeleteExpenseCategory(string budgetId, string categoryId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _expenseCategoryService.DeleteExpenseCategory(authorization.User!.Id, budgetId, categoryId);
                return response == false ? BadRequest() : Ok(response);
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    return Unauthorized(ex.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        private async Task<(bool Result, User? User)> IsAuthorizedUser()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            return (loggedInUser != null, loggedInUser);
        }
    }
}

