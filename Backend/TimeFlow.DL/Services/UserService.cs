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

            var nonFriends = (
                from u in await _userRepository.GetAllAsync()
                where u.Id != user.Id
                join fr in await _friendsRequestRepository.GetAllAsync()
                    on new { UserId = user.Id, FriendId = u.Id }
                    equals new { UserId = fr.SenderId, FriendId = fr.ReceiverId }
                    into friendRequests
                from fr in friendRequests.DefaultIfEmpty()
                where fr == null || !fr.IsAccepted
                select u.Username
            )
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();

            response.Enum = nonFriends;
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
