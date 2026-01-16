using System;

namespace Blckbeast_API.Models
{
    public class User
    {
        public int Id { get; set; } // Clave primaria
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Aquí guardaremos la contraseña encriptada
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Se llena sola con la fecha actual
        public bool IsActive { get; set; } = true;
    }
}