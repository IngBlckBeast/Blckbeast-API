using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blckbeast_API.Data;
using Blckbeast_API.Models;
using Blckbeast_API.DTOs;
using Microsoft.JSInterop.Infrastructure;

namespace Blckbeast_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserscontrollersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserscontrollersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        // TODO: Proteger este endpoint con [Authorize] una vez que la autenticación esté configurada.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicUserDto>>> GetUsers()
        {
            return await _context.Users
                .Select(u => new PublicUserDto
                {
                    UserID = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = u.FullName,
                    CreatedAt = u.CreatedAt
                }).ToListAsync();
        }
    }
}