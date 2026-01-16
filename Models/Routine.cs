using System.ComponentModel.DataAnnotations;

namespace Blckbeast_API.Models
{
    public class Routine
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MuscleGroup { get; set; } = string.Empty; // Ej: "Pecho", "Pierna"
        public string Difficulty { get; set; } = string.Empty; // Ej: "Principiante"
        public string ImageUrl { get; set; } = string.Empty; // Para mostrar una foto en la app
        public List<RoutineExercise> RoutineExercises { get; set; } = new();
    }
}