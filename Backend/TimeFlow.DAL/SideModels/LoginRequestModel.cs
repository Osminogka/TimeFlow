using System.ComponentModel.DataAnnotations;

namespace TimeFlow.DAL.Models
{
    public class LoginRequestModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}