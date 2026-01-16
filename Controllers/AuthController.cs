using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blckbeast_API.Data;
using Blckbeast_API.Models;
using Blckbeast_API.DTOs;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Blckbeast_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration; // Necesitamos esto para leer el appsettings.json

        // Inyectamos IConfiguration en el constructor
        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            // 1. Validar Usuario
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null) return BadRequest("Usuario o contraseña incorrectos.");

            // 2. Validar Password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Usuario o contraseña incorrectos.");
            }

            // 3. Crear el Token (El Pasaporte)
            string token = CreateToken(user);

            // 4. Devolver el Token al usuario
            return Ok(token);
        }

        // Método privado que cocina el Token
        private string CreateToken(User user)
        {
            // A. Definimos los "Claims" (Datos que van pegados en el pasaporte)
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
                // Aquí podrías agregar roles, ej: "Admin"
            };

            // B. Leemos la clave secreta del appsettings.json
            // Nota: Usamos la sección "Jwt:Key" que configuraste
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value!));

            // C. Creamos las credenciales de firma (para que nadie falsifique el token)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // D. Creamos el Token con sus propiedades
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1), // El token vence en 1 día
                signingCredentials: creds,
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value
            );

            // E. Escribimos el token como un string largo
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}