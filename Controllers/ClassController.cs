using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ClassController : ControllerBase
{
    private readonly SchoolContext _context;

    public ClassController(SchoolContext context)
    {
        _context = context;
    }

    // GET: api/Class
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetClasses()
    {
        var classes = await _context.Classes.ToListAsync();
        return Ok(classes);
    }

    // GET: api/Class/5
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetClass(int id)
    {
        var classEntity = await _context.Classes.FindAsync(id);

        if (classEntity == null)
        {
            return NotFound();
        }

        return Ok(classEntity);
    }

    // POST: api/Class
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PostClass([FromBody] Class classEntity)
    {
        _context.Classes.Add(classEntity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetClass), new { id = classEntity.Id }, classEntity);
    }

    // PUT: api/Class/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutClass(int id, [FromBody] Class classEntity)
    {
        if (id != classEntity.Id)
        {
            return BadRequest();
        }

        _context.Entry(classEntity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClassExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Class/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        var classEntity = await _context.Classes.FindAsync(id);
        if (classEntity == null)
        {
            return NotFound();
        }

        _context.Classes.Remove(classEntity);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ClassExists(int id)
    {
        return _context.Classes.Any(e => e.Id == id);
    }
}
