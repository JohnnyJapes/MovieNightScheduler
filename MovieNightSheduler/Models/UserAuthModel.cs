using System.ComponentModel.DataAnnotations;
namespace MovieNightSheduler.Models
{
    public class UserAuth
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
