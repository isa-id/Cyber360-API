using Microsoft.AspNetCore.Mvc;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly NeondbContext _context;

        public UsuarioController(NeondbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuario.ToListAsync();
            return Ok(usuarios);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
                return Unauthorized(new { message = "Correo no encontrado." });

            var hash = usuario.Contrasena;

            bool contraseñaValida;

            // Si parece una contraseña encriptada con BCrypt
            if (hash.StartsWith("$2a$") || hash.StartsWith("$2b$") || hash.StartsWith("$2y$"))
            {
                contraseñaValida = BCrypt.Net.BCrypt.Verify(request.Contrasena, hash);
            }
            else
            {
                // Comparación directa para contraseñas sin encriptar (solo para pruebas)
                contraseñaValida = request.Contrasena == hash;
            }

            if (!contraseñaValida)
                return Unauthorized(new { message = "Contraseña incorrecta." });

            return Ok(new
            {
                message = "Inicio de sesión exitoso",
                usuario.IdUsuario,
                usuario.Nombre,
                usuario.Email,
                usuario.FkRol
            });
        }



    }

}
