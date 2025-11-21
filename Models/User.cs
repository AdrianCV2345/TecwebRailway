namespace StudentsCRUD.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        // En una aplicación real, esto sería un hash seguro (e.g., usando BCrypt).
        public string PasswordHash { get; set; }
        // Campo usado por el atributo [Authorize(Roles = "Admin")]
        public string Role { get; set; }
    }
}