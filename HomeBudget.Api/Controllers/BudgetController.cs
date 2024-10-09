using HomeBudget.Api.Entities;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.Budget;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.Api.Controllers
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly UserManager<User> _userManager;

        public BudgetController(IBudgetService budgetService,UserManager<User> userManager)
        {
            _budgetService = budgetService;
            _userManager = userManager;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<BudgetGetResponseModel>>> GetAllBudgets()
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var budgets = await _budgetService.GetBudgetsAsync(authorization.User!.Id);
                return Ok(budgets);
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

        [HttpGet("{budgetId}"), Authorize]
        public async Task<ActionResult<BudgetGetResponseModel>> GetBudgetById(string budgetId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var budget = await _budgetService.GetBudgetByIdsAsync(authorization.User!.Id, budgetId);
                return budget == null ? NotFound() : Ok(budget);
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
        public async Task<ActionResult<BudgetCreateResponseModel>> CreateBudget([FromBody] BudgetCreateRequestModel budgetRequestModel)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _budgetService.CreateBudgetAsync(authorization.User!.Id, budgetRequestModel);
                return response == null ? BadRequest() : CreatedAtAction(nameof(CreateBudget), new { authorization.User!.Id, budgetId = response.Id }, response);
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

        [HttpPut("{budgetId}"), Authorize]
        public async Task<ActionResult<BudgetUpdateResponseModel>> UpdateBudget(string budgetId,[FromBody] BudgetUpdateRequestModel requestModel)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _budgetService.UpdateBudgetAsync(authorization.User!.Id, budgetId, requestModel);
                return response == null ? BadRequest() : CreatedAtAction(nameof(UpdateBudget), new { authorization.User!.Id, budgetId = response.Id }, response);
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

        [HttpDelete("{budgetId}"), Authorize]
        public async Task<IActionResult> DeleteBudgetById(string budgetId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _budgetService.DeleteBudgetAsync(authorization.User!.Id, budgetId);
                return response == false ? BadRequest() : Ok();
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
