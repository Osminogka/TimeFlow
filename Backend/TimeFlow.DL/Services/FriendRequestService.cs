using Azure.Core;
using Microsoft.EntityFrameworkCore;
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

            var requests = await _friendRequestsRepository.Where(obj => obj.ReceiverId == user.Id).Include(obj => obj.Sender).ToListAsync();
            foreach (FriendRequest request in requests)
                response.Enum.Append(request.Sender.Username);

            response.Success = true;
            response.Message = "Got all your friend requests";
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

            var entryExist = await _friendRequestsRepository.SingleOrDefaultAsync(obj => obj.SenderId == user.Id && obj.ReceiverId == receiver.Id);
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

            var entryExist = await _friendRequestsRepository.SingleOrDefaultAsync(obj => obj.SenderId == user.Id && obj.ReceiverId == sender.Id);
            if (entryExist == null)
            {
                response.Message = "Friend request does not exist";
                return response;
            }
            entryExist.IsAccepted = true;
            await _friendRequestsRepository.UpdateAsync(entryExist);

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

            var entryExist = await _friendRequestsRepository.SingleOrDefaultAsync(obj => obj.SenderId == user.Id && obj.ReceiverId == sender.Id);
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
