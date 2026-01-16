using System.Text.Json.Serialization;

namespace Blckbeast_API.Models
{
    public class WorkoutSession
    {
        public int Id { get; set; }

        public int UserId { get; set; } // ¿Quién entrenó?
        public string RoutineName { get; set; } = string.Empty; // ¿Qué rutina hizo? (Snapshopt)
        
        public DateTime StartTime { get; set; } // ¿A qué hora llegó?
        public DateTime? EndTime { get; set; }   // ¿A qué hora se fue?

        // Relación: Una sesión tiene muchas series
        public List<WorkoutSet> Sets { get; set; } = new();
    }
}