using System.ComponentModel.DataAnnotations;

namespace TimeFlow.DAL.Models
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
