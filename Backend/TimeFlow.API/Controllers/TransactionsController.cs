using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using TimeFlow.DAL.Dtos;
using TimeFlow.DL.Services;

namespace TimeFlow.API.Controllers
{
    [Authorize]
    [Route("api/transactions")]
    public class TransactionsController : BaseController
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTransactionAsync(CreateTransactionDto transaction)
        {
            try
            {
                var result = await _transactionService.CreateTransactionAsync(transaction, getUserEmail());
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return HandleException(ex);
            }
        }

        [HttpGet("self/{month}/{year}")]
        public async Task<IActionResult> GetTransactionsForSelfAsync(int month, int year)
        {
            try
            {
                var result = await _transactionService.GetTransactionsForSelfAsync(getUserEmail(), month, year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return HandleException(ex);
            }
        }

        [HttpGet("friend/{friendName}/{month}/{year}")]
        public async Task<IActionResult> GetTransactionsForFriendAsync(string friendName, int month, int year)
        {
            try
            {
                var result = await _transactionService.GetTransactionsForFriendAsync(getUserEmail(), friendName, month, year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return HandleException(ex);
            }
        }
    }
}
