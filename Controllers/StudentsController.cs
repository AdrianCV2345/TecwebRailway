
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentsCRUD.DTOs.Student;
using StudentsCRUD.Services;

[Authorize] // <--- Autenticación Requerida para todo el Controller
[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll()
    {
        return Ok(await _studentService.GetAllStudentsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetById(Guid id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null) return NotFound();
        return Ok(student);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // <--- Autorización: Solo usuarios con Rol "Admin"
    public async Task<ActionResult<StudentDto>> Create([FromBody] StudentCreateDto dto)
    {
        var newStudent = await _studentService.CreateStudentAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = newStudent.Id }, newStudent);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")] // <--- Autorización: Roles "Admin" o "Teacher"
    public async Task<ActionResult> Update(Guid id, [FromBody] StudentCreateDto dto)
    {
        var updated = await _studentService.UpdateStudentAsync(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // <--- Autorización: Solo "Admin"
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await _studentService.DeleteStudentAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}