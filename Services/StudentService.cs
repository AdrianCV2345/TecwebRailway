
using StudentsCRUD.DTOs.Student;
using StudentsCRUD.Models;
using StudentsCRUD.Repositories;

namespace StudentsCRUD.Services
{
    public class StudentService : IStudentService
    {
        // Se inyecta el contrato del repositorio, no el DbContext directamente.
        private readonly IStudentRepository _studentRepository;

        // Inyección de Dependencias (DI) del Repositorio
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            // Mapeo de Entidad a DTO
            return students.Select(s => new StudentDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Age = s.Age
            });
        }

        public async Task<StudentDto?> GetStudentByIdAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return null;

            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Age = student.Age
            };
        }

        public async Task<StudentDto> CreateStudentAsync(StudentCreateDto dto)
        {
            var student = new Student
            {
                // ID será asignado por la DB/Repositorio
                Name = dto.Name,
                Email = dto.Email,
                Age = dto.Age
            };

            var newStudent = await _studentRepository.AddAsync(student);

            return new StudentDto
            {
                Id = newStudent.Id,
                Name = newStudent.Name,
                Email = newStudent.Email,
                Age = newStudent.Age
            };
        }

        public async Task<bool> UpdateStudentAsync(Guid id, StudentCreateDto dto)
        {
            var studentToUpdate = await _studentRepository.GetByIdAsync(id);
            if (studentToUpdate == null) return false;

            // Mapear DTO a Entidad existente
            studentToUpdate.Name = dto.Name;
            studentToUpdate.Email = dto.Email;
            studentToUpdate.Age = dto.Age;

            return await _studentRepository.UpdateAsync(studentToUpdate);
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            return await _studentRepository.DeleteAsync(id);
        }
    }
}