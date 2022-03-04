using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace MovieNightScheduler.Models
{
    public class UserAuth
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
    public class AuthResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
    
    }

    public class CreateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
    public class RevokeTokenRequest
    {
        public string Token { get; set; }
    }
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
