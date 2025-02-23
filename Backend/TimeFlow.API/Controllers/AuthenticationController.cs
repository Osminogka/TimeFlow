using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeFlow.DAL.Models;
using TimeFlow.DL.Repositories;
using TimeFlow.DL.Services;

namespace TimeFlow.API.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : BaseController
    {
        private readonly IAccountService _authService;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAccountService authService, IAccountRepository accountRepository, IMapper mapper, ILogger<AuthenticationController> logger)

        {
            _authService = authService;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid credentials");
                var result = await _authService.LoginAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return HandleException(ex);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid credentials");
                var result = await _authService.RegisterAsync(model);
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