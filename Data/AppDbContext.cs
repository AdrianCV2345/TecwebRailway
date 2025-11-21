using Microsoft.EntityFrameworkCore;
using StudentsCRUD.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StudentsCRUD.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            // Inicializar datos de usuario para pruebas
            Database.EnsureCreated();
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Datos de prueba para simular usuarios en la DB.
            // La contraseña debe coincidir con la validación en AuthService.cs.
            modelBuilder.Entity<User>().HasData(
                new User { Id = Guid.NewGuid(), Username = "admin", PasswordHash = "adminpass", Role = "Admin" },
                new User { Id = Guid.NewGuid(), Username = "teacher", PasswordHash = "teacherpass", Role = "Teacher" },
                new User { Id = Guid.NewGuid(), Username = "user", PasswordHash = "userpass", Role = "Student" }
            );

            // Datos de prueba para estudiantes.
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = Guid.NewGuid(), Name = "Alice Smith", Email = "alice@example.com", Age = 25 },
                new Student { Id = Guid.NewGuid(), Name = "Bob Johnson", Email = "bob@example.com", Age = 22 }
            );
        }
    }
}