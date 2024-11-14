using HomeBudget.Api.Entities;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.TransactionCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.Api.Controllers
{
    [Route("api/User/Budget/{budgetId}/[controller]")]
    [ApiController]
    public class TransactionCategoryController : ControllerBase
    {
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly UserManager<User> _userManager;

        public TransactionCategoryController(ITransactionCategoryService transactionCategoryService, UserManager<User> userManager)
        {
            _transactionCategoryService = transactionCategoryService;
            _userManager = userManager;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<TransactionCategoryGetResponseModel>>> GetAllTransactionCategoriesInBudget(string budgetId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var transactionCategories = await _transactionCategoryService.GetTransactionCategoriesByBudgetIdAsync(authorization.User!.Id, budgetId);
                return Ok(transactionCategories);
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

        [HttpGet("TopAmount"), Authorize]
        public async Task<ActionResult<IEnumerable<TransactionCategoryGetResponseModel>>> GetTopAmountTransactionCategoriesInDateRange(
            string budgetId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int count = 5)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var transactionCategories = await _transactionCategoryService.GetTopAmountTransactionCategoriesByBudgetIdInDateRangeAsync(authorization.User!.Id, budgetId, count, startDate, endDate);
                return Ok(transactionCategories);
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

        [HttpGet("TopCount"), Authorize]
        public async Task<ActionResult<IEnumerable<TransactionCategoryGetResponseModel>>> GetTopCountTransactionCategoriesInDateRange(
            string budgetId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int count = 5)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var transactionCategories = await _transactionCategoryService.GetTopCountTransactionCategoriesByBudgetIdInDateRangeAsync(authorization.User!.Id, budgetId, count, startDate, endDate);
                return Ok(transactionCategories);
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
        public async Task<ActionResult<TransactionCategoryGetResponseModel>> GetTransactionCategoryById(string budgetId, string categoryId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var transactionCategory = await _transactionCategoryService.GetTransactionCategoryByIdsAsync(authorization.User!.Id, budgetId, categoryId);
                return transactionCategory == null ? NotFound() : Ok(transactionCategory);
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
        public async Task<ActionResult<IEnumerable<TransactionCategoryCreateResponseModel>>> CreateTransactionCategories(string budgetId, [FromBody] List<TransactionCategoryCreateRequestModel> requestModels)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _transactionCategoryService.CreateTransactionCategoriesAsync(authorization.User!.Id, budgetId, requestModels);
                return !response.Any() ? BadRequest() : Ok(response);
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
        public async Task<ActionResult<TransactionCategoryUpdateResponseModel>> UpdateTransactionCategory(string budgetId, string categoryId, [FromBody] TransactionCategoryUpdateRequestModel requestModel)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _transactionCategoryService.UpdateTransactionCategoryAsync(authorization.User!.Id, budgetId, categoryId, requestModel);
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
        public async Task<IActionResult> DeleteTransactionCategory(string budgetId, string categoryId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _transactionCategoryService.DeleteTransactionCategory(authorization.User!.Id, budgetId, categoryId);
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

