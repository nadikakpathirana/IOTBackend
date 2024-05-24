using System.ComponentModel.DataAnnotations;

namespace IOTBackend.Domain.Dtos
{
    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
