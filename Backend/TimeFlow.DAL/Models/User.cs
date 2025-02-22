using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeFlow.DAL.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool isPublic { get; set; } = false;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<FriendRequest> SentRequests { get; set; } = new List<FriendRequest>();
        public ICollection<FriendRequest> ReceivedRequests { get; set; } = new List<FriendRequest>();
    }
}
