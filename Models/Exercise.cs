namespace Blckbeast_API.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Ej: "Press de Banca"
        public string MuscleGroup { get; set; } = string.Empty; // Ej: "Pecho"
        public string? VideoUrl { get; set; } // Opcional: Para poner un GIF después
    }
}