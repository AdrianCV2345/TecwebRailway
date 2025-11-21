using StudentsCRUD.DTOs.Auth;

namespace StudentsCRUD.Services
{
    
    public interface IAuthService
    {
        string Register(RegisterDto dto);
        string Login(LoginDto dto);
    }
}
