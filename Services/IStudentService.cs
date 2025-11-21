
using StudentsCRUD.DTOs.Student;

namespace StudentsCRUD.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto?> GetStudentByIdAsync(Guid id);
        Task<StudentDto> CreateStudentAsync(StudentCreateDto dto);
        Task<bool> UpdateStudentAsync(Guid id, StudentCreateDto dto);
        Task<bool> DeleteStudentAsync(Guid id);
    }
}