using Microsoft.EntityFrameworkCore;
using TimeFlow.DAL.Models;
using TimeFlow.DAL.SideModels;
using TimeFlow.DL.Repositories;

namespace TimeFlow.DL.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<FriendRequest> _friendsRequestRepository;

        public UserService(IBaseRepository<User> userRepository, IBaseRepository<FriendRequest> friendsRequestRepository)
        {
            _userRepository = userRepository;
            _friendsRequestRepository = friendsRequestRepository;
        }

        public async Task<ResponseList<string>> GetUsersAsync(string userEmail, int page)
        {
            ResponseList<string> response = new ResponseList<string>();
            int pageSize = 5;

            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if(user == null)
            {
                response.Message = "Such user doen't exist";
                return response;
            }

            var nonFriends = await _userRepository.GetNonFriendsAsync(user.Id, page, pageSize);

            response.Enum = nonFriends.Select(obj => obj.Username).ToList();
            response.Success = true;
            response.Message = "Got some users";
            return response;
        }
        
        public async Task<ResponseList<string>> GetUsersByNameAsync(string userEmail, string friendName)
        {
            ResponseList<string> response = new ResponseList<string>();

            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if(user == null)
            {
                response.Message = "Such user doen't exist";
                return response;
            }

            var nonFriends = await _userRepository.GetNonFriendsAsyncByName(user.Id, friendName);

            response.Enum = nonFriends.Select(obj => obj.Username).ToList();
            response.Success = true;
            response.Message = "Got some users";
            return response;
        }

        public async Task<ResponseMessage> ChangeAccountVisibilityAsync(string userEmail)
        {
            ResponseMessage response = new ResponseMessage();

            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if(user == null)
            {
                response.Message = "Such user doesn't exist";
                return response;
            }
            user.isPublic = !user.isPublic;
            await _userRepository.UpdateAsync(user);

            response.Success = true;
            response.Message = "Changed account visibility";
            return response;
        }
    }
}
