using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimeFlow.DAL.Models;
using TimeFlow.DAL.SideModels;
using TimeFlow.DL.Repositories;

namespace TimeFlow.DL.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<FriendRequest> _friendRequestsRepository;

        public FriendRequestService(IBaseRepository<User> userRepository, IBaseRepository<FriendRequest> friendRequestsRepository)
        {
            _userRepository = userRepository;
            _friendRequestsRepository = friendRequestsRepository;
        }

        public async Task<ResponseList<string>> GetFriendRequestsAsync(string userEmail)
        {
            ResponseList<string> response = new ResponseList<string>();

            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if(user == null)
            {
                response.Message = "Such user doesn't exist";
                return response;
            }

            var requests = await _friendRequestsRepository.Where(obj => obj.ReceiverId == user.Id && !obj.IsAccepted).Include(obj => obj.Sender).ToListAsync();
            foreach (FriendRequest request in requests)
                response.Enum.Add(request.Sender.Username);

            response.Success = true;
            response.Message = "Got all your friend requests";
            return response;
        }

        public async Task<ResponseList<string>> GetFriendsListAsync(string userEmail)
        {
            ResponseList<string> response = new ResponseList<string>();

            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if (user == null)
            {
                response.Message = "Such user doesn't exist";
                return response;
            }

            var requests = await _friendRequestsRepository.Where(obj => (obj.ReceiverId == user.Id || obj.SenderId == user.Id) && obj.IsAccepted).Include(obj => obj.Sender).Include(obj => obj.Receiver).ToListAsync();
            foreach (FriendRequest request in requests)
                response.Enum.Add(request.SenderId == user.Id ? request.Receiver.Username : request.Sender.Username);

            response.Success = true;
            response.Message = "Got all your friend requests";
            return response;
        }

        public async Task<ResponseMessage> DeleteFriendAsync(string userEmail, string friendName)
        {
            ResponseMessage response = new ResponseMessage();
            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if (user == null)
            {
                response.Message = "Such user doesn't exist";
                return response;
            }
            
            var friend = await _userRepository.SingleOrDefaultAsync(obj=> obj.Username == friendName);
            if (friend == null)
            {
                response.Message = "Such friend doesn't exist";
                return response;
            }

            var checkFriendStatus = await _friendRequestsRepository.SingleOrDefaultAsync(obj =>
                ((obj.SenderId == user.Id && obj.ReceiverId == friend.Id)
                 || obj.ReceiverId == user.Id && obj.SenderId == friend.Id)
                && obj.IsAccepted);
            if (checkFriendStatus == null)
            {
                response.Message = "You are not friends";
                return response;
            }

            await _friendRequestsRepository.DeleteAsync(checkFriendStatus);
            
            response.Success = true;
            response.Message = "Friend deleted";
            return response;
        }

        public async Task<ResponseMessage> SendFriendRequestAsync(string userEmail, string receiverUsername)
        {
            ResponseMessage response = new ResponseMessage();
            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if (user == null)
            {
                response.Message = "Such user(you) doesn't exist";
                return response;
            }

            var receiver = await _userRepository.SingleOrDefaultAsync(obj => obj.Username == receiverUsername);
            if (receiver == null)
            {
                response.Message = "Such user(receiver) doesn't exist";
                return response;
            }

            var alreadyFriends = await _friendRequestsRepository.SingleOrDefaultAsync(obj => 
                (((obj.ReceiverId == user.Id && obj.SenderId == receiver.Id)
                || (obj.ReceiverId == receiver.Id && obj.SenderId == user.Id)))
                && obj.IsAccepted);
            if(alreadyFriends != null)
            {
                response.Message = "You are already friends with this user";
                return response;
            }

            var entryExist = await _friendRequestsRepository.SingleOrDefaultAsync(obj => (obj.ReceiverId == receiver.Id && obj.SenderId == user.Id)
                && !obj.IsAccepted);
            if(entryExist != null)
            {
                response.Success = true;
                response.Message = "Friend request successfully sent";
                return response;
            }

            FriendRequest friendRequest = new FriendRequest
            {
                SenderId = user.Id,
                ReceiverId = receiver.Id,
                SentAt = DateTime.Now,
                IsAccepted = false,
            };
            await _friendRequestsRepository.AddAsync(friendRequest);

            response.Success = true;
            response.Message = "Friend request successfully sent";
            return response;
        }

        public async Task<ResponseMessage> AcceptFriendRequestAsync(string userEmail, string senderUsername)
        {
            ResponseMessage response = new ResponseMessage();
            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if (user == null)
            {
                response.Message = "Such user(you) doesn't exist";
                return response;
            }

            var sender = await _userRepository.SingleOrDefaultAsync(obj => obj.Username == senderUsername);
            if (sender == null)
            {
                response.Message = "Such user(receiver) doesn't exist";
                return response;
            }

            var entryExist = await _friendRequestsRepository.SingleOrDefaultAsync(obj => obj.ReceiverId == user.Id && obj.SenderId == sender.Id && !obj.IsAccepted);
            if (entryExist == null)
            {
                response.Message = "Friend request does not exist";
                return response;
            }
            entryExist.IsAccepted = true;
            await _friendRequestsRepository.UpdateAsync(entryExist);

            var secondRequest = await _friendRequestsRepository.SingleOrDefaultAsync(obj => obj.ReceiverId == sender.Id && obj.SenderId == user.Id && !obj.IsAccepted);
            if(secondRequest != null)
                await _friendRequestsRepository.DeleteAsync(secondRequest);

            response.Success = true;
            response.Message = "Friend request accepted";
            return response;
        }

        public async Task<ResponseMessage> RejectFriendRequestAsync(string userEmail, string senderUsername)
        {
            ResponseMessage response = new ResponseMessage();
            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if (user == null)
            {
                response.Message = "Such user(you) doesn't exist";
                return response;
            }

            var sender = await _userRepository.SingleOrDefaultAsync(obj => obj.Username == senderUsername);
            if (sender == null)
            {
                response.Message = "Such user(receiver) doesn't exist";
                return response;
            }

            var entryExist = await _friendRequestsRepository.SingleOrDefaultAsync(obj => obj.ReceiverId == user.Id && obj.SenderId == sender.Id && !obj.IsAccepted);
            if (entryExist == null)
            {
                response.Message = "Friend request does not exist";
                return response;
            }
            await _friendRequestsRepository.DeleteAsync(entryExist);

            response.Success = true;
            response.Message = "Friend request rejected";
            return response;
        }
    }
}
