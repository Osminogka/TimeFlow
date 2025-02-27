using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeFlow.DL.Services;

namespace TimeFlow.API.Controllers
{
    [Authorize]
    [Route("api/user")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("get/{page}")]
        public async Task<IActionResult> GetAllUsersAsync(int page)
        {
            try
            {
                var result = await _userService.GetUsersAsync(getUserEmail(), page);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return HandleException(ex);
            }
        }
        
        [HttpGet("getbyname/{username}")]
        public async Task<IActionResult> GetAllUsersAsync(string username)
        {
            try
            {
                var result = await _userService.GetUsersByNameAsync(getUserEmail(), username);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return HandleException(ex);
            }
        }

        [HttpGet("visibility")]
        public async Task<IActionResult> ChangeAccountVisibilityAsync()
        {
            try
            {
                var result = await _userService.ChangeAccountVisibilityAsync(getUserEmail());
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
