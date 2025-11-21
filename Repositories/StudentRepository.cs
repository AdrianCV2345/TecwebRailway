using Microsoft.EntityFrameworkCore;
using StudentsCRUD.Data;
using StudentsCRUD.Models;
using StudentsCRUD.Repositories;

namespace StudentsCRUD.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        // DI del DbContext
        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> UpdateAsync(Student updatedStudent)
        {
            var existingStudent = await _context.Students.FindAsync(updatedStudent.Id);

            if (existingStudent == null) return false;

            // Actualizar solo los campos necesarios (asumiendo que updatedStudent ya tiene el ID)
            existingStudent.Name = updatedStudent.Name;
            existingStudent.Email = updatedStudent.Email;
            existingStudent.Age = updatedStudent.Age;

            _context.Students.Update(existingStudent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}