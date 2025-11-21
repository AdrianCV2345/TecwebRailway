using System.ComponentModel.DataAnnotations;

namespace StudentsCRUD.DTOs.Auth
{
    public record LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}