using System.ComponentModel.DataAnnotations;
namespace MovieNightScheduler.Models
{
    public class UserAuth
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
