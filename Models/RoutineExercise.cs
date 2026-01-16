using System.Text.Json.Serialization;

namespace Blckbeast_API.Models
{
    public class RoutineExercise
    {
        public int Id { get; set; }

        // RELACIONES (Foreign Keys)
        public int RoutineId { get; set; }
        [JsonIgnore] // Evita ciclos infinitos al convertir a JSON
        public Routine? Routine { get; set; }

        public int ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }

        // DATOS DE LA RUTINA
        public int OrderIndex { get; set; } // Para ordenar (1ro, 2do, 3ro...)
        public int Sets { get; set; } // Ej: 4 series
        public int Reps { get; set; } // Ej: 12 repeticiones
    }
}