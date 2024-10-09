using HomeBudget.Api.Entities;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.Expense;
using HomeBudget.Common.EntityDTOs.ExpenseCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.Api.Controllers
{
    [Route("api/User/Budget/{budgetId}/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly UserManager<User> _userManager;

        public ExpenseController(IExpenseService expenseService, UserManager<User> userManager)
        {
            _expenseService = expenseService;
            _userManager = userManager;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ExpenseGetResponseModel>>> GetAllExpensesInBudget(string budgetId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var expenses = await _expenseService.GetExpensesByBudgetIdAsync(authorization.User!.Id, budgetId);
                return Ok(expenses);
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

        [HttpGet("{expenseId}"), Authorize]
        public async Task<ActionResult<ExpenseGetResponseModel>> GetExpenseById(string budgetId, string expenseId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var expense = await _expenseService.GetExpenseByIdsAsync(authorization.User!.Id, budgetId, expenseId);
                return expense == null ? NotFound() : Ok(expense);
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
        public async Task<ActionResult<ExpenseCreateResponseModel>> CreateExpense(string budgetId, [FromBody] ExpenseCreateRequestModel requestModel)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _expenseService.CreateExpenseAsync(authorization.User!.Id, budgetId, requestModel);
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

        [HttpPut("{expenseId}"), Authorize]
        public async Task<ActionResult<ExpenseUpdateResponseModel>> UpdateExpenseCategory(string budgetId, string expenseId, [FromBody] ExpenseUpdateRequestModel requestModel)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _expenseService.UpdateExpenseAsync(authorization.User!.Id, budgetId, expenseId, requestModel);
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

        [HttpDelete("{expenseId}"), Authorize]
        public async Task<IActionResult> DeleteExpenseCategory(string budgetId, string expenseId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _expenseService.DeleteExpense(authorization.User!.Id, budgetId, expenseId);
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
