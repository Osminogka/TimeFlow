using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeFlow.DAL.Models
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;

        public long UserId { get; set; }
        public User User { get; set; } = null!;

        public long CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
