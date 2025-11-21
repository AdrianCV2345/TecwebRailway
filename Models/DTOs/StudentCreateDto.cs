using System.ComponentModel.DataAnnotations;

namespace StudentsCRUD.DTOs.Student
{
    public record StudentCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(18, 99)]
        public int Age { get; set; }
    }
}