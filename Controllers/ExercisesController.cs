using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blckbeast_API.Data;
using Blckbeast_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Blckbeast_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ¡Protegido con JWT!
    public class ExercisesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
        { 
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises()
        {
            return await _context.Exercises.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Exercise>> PostExercise(Exercise exercise)
        {
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetExercises", new { id = exercise.Id }, exercise);
        }
    }
}