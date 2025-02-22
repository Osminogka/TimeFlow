using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeFlow.DL.Services;

namespace TimeFlow.API.Controllers
{
    [Authorize]
    [Route("api/friends")]
    public class FriendsController : BaseController
    {
        private readonly IFriendRequestService _friendRequestService;
        private readonly ILogger<FriendsController> _logger;

        public FriendsController(IFriendRequestService friendRequestService, ILogger<FriendsController> logger)
        {
            _friendRequestService = friendRequestService;
            _logger = logger;
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetFriendRequestsAsync()
        {
            try
            {
                var result = await _friendRequestService.GetFriendRequestsAsync(getUserEmail());
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return HandleException(ex);
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetFriendsListAsync()
        {
            try
            {
                var result = await _friendRequestService.GetFriendsListAsync(getUserEmail());
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return HandleException(ex);
            }
        }

        [HttpGet("send/{receiverName}")]
        public async Task<IActionResult> SendFriendRequestAsync(string receiverName)
        {
            try
            {
                var result = await _friendRequestService.SendFriendRequestAsync(getUserEmail(), receiverName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return HandleException(ex);
            }
        }

        [HttpGet("accept/{senderName}")]
        public async Task<IActionResult> AcceptFriendRequestAsync(string senderName)
        {
            try
            {
                var result = await _friendRequestService.AcceptFriendRequestAsync(getUserEmail(), senderName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return HandleException(ex);
            }
        }

        [HttpGet("reject/{senderName}")]
        public async Task<IActionResult> RejectFriendRequestAsync(string senderName)
        {
            try
            {
                var result = await _friendRequestService.RejectFriendRequestAsync(getUserEmail(), senderName);
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
