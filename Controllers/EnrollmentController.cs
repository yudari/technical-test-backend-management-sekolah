using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentController : ControllerBase
{
    private readonly SchoolContext _context;

    public EnrollmentController(SchoolContext context)
    {
        _context = context;
    }

    // POST: api/Enrollment
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PostEnrollment([FromBody] Enrollment enrollment)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEnrollmentsByStudent), new { studentId = enrollment.StudentId }, enrollment);
    }

    // DELETE: api/Enrollment/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEnrollment(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null)
        {
            return NotFound();
        }

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Enrollment/student/5
    [HttpGet("student/{studentId}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetEnrollmentsByStudent(int studentId)
    {
        var enrollments = await _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .ToListAsync();

        return Ok(enrollments);
    }
}
