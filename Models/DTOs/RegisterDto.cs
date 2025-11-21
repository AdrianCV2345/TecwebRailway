using StudentsCRUD.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace StudentsCRUD.DTOs.Auth
{
    public record RegisterDto : LoginDto // Hereda Username y Password
    {
        public string Username { get; set; }
        public string Role { get; set; }

    }
}