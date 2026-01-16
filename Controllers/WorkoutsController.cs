using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blckbeast_API.Data;
using Blckbeast_API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Blckbeast_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkoutsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Workouts/start
        [HttpPost("start")]
        public async Task<ActionResult<WorkoutSession>> StartWorkout(string routineName)
        {
            // Validar UserId del token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("Token inválido");

            var userId = int.Parse(userIdClaim.Value);

            var session = new WorkoutSession
            {
                UserId = userId,
                RoutineName = routineName,
                StartTime = DateTime.Now
            };

            _context.WorkoutSessions.Add(session);
            await _context.SaveChangesAsync();

            return Ok(session);
        }

        // POST: api/Workouts/log-set
        [HttpPost("log-set")]
        public async Task<ActionResult<WorkoutSet>> LogSet(WorkoutSet set)
        {
            var session = await _context.WorkoutSessions.FindAsync(set.WorkoutSessionId);
            if (session == null) return NotFound("Sesión no encontrada");

            // Seguridad extra: verificar dueño de la sesión
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            
            var userId = int.Parse(userIdClaim.Value);
            if (session.UserId != userId) return Unauthorized("Esta sesión no es tuya");

            _context.WorkoutSets.Add(set);
            await _context.SaveChangesAsync();

            return Ok(set);
        }

        // POST: api/Workouts/finish/5
        [HttpPost("finish/{sessionId}")]
        public async Task<IActionResult> FinishWorkout(int sessionId)
        {
            var session = await _context.WorkoutSessions.FindAsync(sessionId);
            if (session == null) return NotFound();

            session.EndTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new 
            { 
                message = "Entrenamiento finalizado", 
                durationMinutes = (session.EndTime.Value - session.StartTime).TotalMinutes 
            });
        }

        // GET: api/Workouts/history
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<WorkoutSession>>> GetMyHistory()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            return await _context.WorkoutSessions
                .Where(w => w.UserId == userId)
                .Include(w => w.Sets)
                .ThenInclude(s => s.Exercise)
                .OrderByDescending(w => w.StartTime)
                .ToListAsync();
        }
    }
}