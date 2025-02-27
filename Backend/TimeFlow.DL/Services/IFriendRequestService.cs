using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeFlow.DAL.Models;
using TimeFlow.DAL.SideModels;

namespace TimeFlow.DL.Services
{
    public interface IFriendRequestService
    {
        Task<ResponseList<string>> GetFriendRequestsAsync(string userEmail);
        Task<ResponseList<string>> GetFriendsListAsync(string userEmail);
        Task<ResponseMessage> DeleteFriendAsync(string userEmail, string friendName);
        Task<ResponseMessage> SendFriendRequestAsync(string userEmail, string receiverUsername);
        Task<ResponseMessage> AcceptFriendRequestAsync(string userEmail, string senderUsername);
        Task<ResponseMessage> RejectFriendRequestAsync(string userEmail, string senderUsername);
    }
}
