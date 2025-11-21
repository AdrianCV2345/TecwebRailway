
using StudentsCRUD.Models;
using StudentsCRUD.DTOs.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StudentsCRUD.Services;
using Microsoft.Extensions.Configuration;


public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    // (Simulación de la DB para Usuarios - En prod usaría el DbContext)
    private static List<User> _users = new List<User>();

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string Register(RegisterDto dto)
    {
        if (_users.Any(u => u.Username == dto.Username))
            return null; // Usuario ya existe

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            PasswordHash = HashPassword(dto.Password), // Implementar hashing real
            Role = "Student" // Rol por defecto
        };
        _users.Add(newUser);
        return GenerateJwtToken(newUser);
    }

    public string Login(LoginDto dto)
    {
        var user = _users.FirstOrDefault(u => u.Username == dto.Username && VerifyPassword(dto.Password, u.PasswordHash));
        if (user == null)
            return null; // Credenciales inválidas


        return GenerateJwtToken(user);
    }

    // Método para generar el Token
    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role) // Inclusión del Rol
        };

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["JwtSettings:DurationInMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Implementaciones dummy para este ejemplo
    private string HashPassword(string password) => password;
    private bool VerifyPassword(string inputPassword, string storedHash) => inputPassword == storedHash;
}

