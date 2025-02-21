using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeFlow.DAL.Models
{
    public class FriendRequest : BaseEntity
    {
        public long SenderId { get; set; }
        public User Sender { get; set; } = null!;

        public long ReceiverId { get; set; }
        public User Receiver { get; set; } = null!;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsAccepted { get; set; } = false;
    }
}
