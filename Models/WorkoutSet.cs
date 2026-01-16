using System.Text.Json.Serialization;

namespace Blckbeast_API.Models
{
    public class WorkoutSet
    {
        public int Id { get; set; }

        // Relación con la sesión
        public int WorkoutSessionId { get; set; }
        [JsonIgnore]
        public WorkoutSession? WorkoutSession { get; set; }

        // Relación con el ejercicio (para saber qué hizo)
        public int ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }

        // Los datos reales del esfuerzo
        public int SetNumber { get; set; } // Serie 1, 2, 3...
        public double Weight { get; set; } // Peso en kg/lbs
        public int Reps { get; set; }      // Repeticiones reales
    }
}