using HomeBudget.Api.Entities;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.Api.Controllers
{
    [Route("api/User/Budget/{budgetId}/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly UserManager<User> _userManager;

        public TransactionController(ITransactionService transactionService, UserManager<User> userManager)
        {
            _transactionService = transactionService;
            _userManager = userManager;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<TransactionGetResponseModel>>> GetAllTransactionsInBudget(string budgetId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var transactions = await _transactionService.GetTransactionsByBudgetIdAsync(authorization.User!.Id, budgetId);
                return Ok(transactions);
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

        [HttpGet("{transactionId}"), Authorize]
        public async Task<ActionResult<TransactionGetResponseModel>> GetTransactionById(string budgetId, string transactionId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var transaction = await _transactionService.GetTransactionByIdsAsync(authorization.User!.Id, budgetId, transactionId);
                return transaction == null ? NotFound() : Ok(transaction);
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

        [HttpGet("DateRange"), Authorize]
        public async Task<ActionResult<IEnumerable<TransactionGetResponseModel>>> GetTransactionsInDateRange(
            string budgetId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var transactions = await _transactionService.GetTransactionsByBudgetIdInDateRangeAsync(
                    authorization.User!.Id, budgetId, startDate, endDate);

                return Ok(transactions);
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
        public async Task<ActionResult<IEnumerable<TransactionCreateResponseModel>>> CreateTransactions(string budgetId, [FromBody] List<TransactionCreateRequestModel> requestModels)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _transactionService.CreateTransactionsAsync(authorization.User!.Id, budgetId, requestModels);
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

        [HttpPut("{transactionId}"), Authorize]
        public async Task<ActionResult<TransactionUpdateResponseModel>> UpdateTransaction(string budgetId, string transactionId, [FromBody] TransactionUpdateRequestModel requestModel)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _transactionService.UpdateTransactionAsync(authorization.User!.Id, budgetId, transactionId, requestModel);
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

        [HttpDelete("{transactionId}"), Authorize]
        public async Task<IActionResult> DeleteTransaction(string budgetId, string transactionId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _transactionService.DeleteTransaction(authorization.User!.Id, budgetId, transactionId);
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
