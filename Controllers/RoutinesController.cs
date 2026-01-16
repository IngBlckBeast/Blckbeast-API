using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blckbeast_API.Data;
using Blckbeast_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Blckbeast_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoutinesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoutinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Routines
        // Este endpoint devuelve TODAS las rutinas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Routine>>> GetRoutines()
        {
            return await _context.Routines.ToListAsync();
        }

        // POST: api/Routines
        // Este endpoint sirve para CREAR una rutina nueva (lo usaremos para llenar datos de prueba)
        [HttpPost]
        public async Task<ActionResult<Routine>> PostRoutine(Routine routine)
        {
            _context.Routines.Add(routine);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoutines), new { id = routine.Id }, routine);
        }

        // PUT: api/Routines/5
        // Sirve para ACTUALIZAR una rutina existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoutine(int id, Routine routine)
        {
            if (id != routine.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo");
            }

            // Marcamos la entidad como "modificada" para que EF sepa que debe actualizarla
            _context.Entry(routine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Routines.Any(e => e.Id == id))
                {
                    return NotFound("La rutina no existe");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // 204 No Content (Significa: "Lo hice, todo bien")
        }

        // DELETE: api/Routines/5
        // Sirve para BORRAR una rutina
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoutine(int id)
        {
            var routine = await _context.Routines.FindAsync(id);
            if (routine == null)
            {
                return NotFound("No encontré esa rutina para borrar");
            }

            _context.Routines.Remove(routine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Routines/5/add-exercise
        // Agrega un ejercicio existente a una rutina
        [HttpPost("{routineId}/add-exercise")]
        public async Task<ActionResult<RoutineExercise>> AddExerciseToRoutine(int routineId, RoutineExercise routineExercise)
        {
            // 1. Validar que la rutina sea del usuario o pública (Simplificado por ahora)
            if (!_context.Routines.Any(r => r.Id == routineId))
                return NotFound("Rutina no encontrada");

            // 2. Asignar el ID de la URL al objeto
            routineExercise.RoutineId = routineId;

            // 3. Guardar
            _context.RoutineExercises.Add(routineExercise);
            await _context.SaveChangesAsync();

            return Ok(routineExercise);
        }

        // GET: api/Routines/5/details
        // Obtiene la rutina CON sus ejercicios
        [HttpGet("{id}/details")]
        public async Task<ActionResult<Routine>> GetRoutineDetails(int id)
        {
            var routine = await _context.Routines
                .Include(r => r.RoutineExercises) // <--- OJO: Incluye la tabla intermedia
                .ThenInclude(re => re.Exercise)   // <--- OJO: Y trae el nombre del ejercicio
                .FirstOrDefaultAsync(r => r.Id == id);

            if (routine == null) return NotFound();

            return routine;
        }
    }
}