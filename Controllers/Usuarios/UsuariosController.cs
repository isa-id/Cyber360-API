using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.DTOs;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly NeondbContext _context;

        public UsuariosController(NeondbContext context)
        {
            _context = context;
        }

        // GET: /usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuario
                .Include(u => u.FkRolNavigation) // ← para traer el Rol
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Celular = u.Celular,
                    Rol = u.FkRolNavigation != null ? u.FkRolNavigation.NombreRol : "Sin rol",
                    Estado = u.Estado
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: /usuarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario
                .Include(u => u.FkRolNavigation)
                .Where(u => u.IdUsuario == id)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Celular = u.Celular,
                    Rol = u.FkRolNavigation != null ? u.FkRolNavigation.NombreRol : "Sin rol",
                    Estado = u.Estado
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        // POST: /usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> CreateUsuario([FromBody] UsuarioCreateOrUpdateDto dto)
        {
            var usuario = new Usuario
            {
                TipoDoc = dto.TipoDoc,
                Documento = dto.Documento,
                Nombre = dto.Nombre,
                Celular = dto.Celular,
                Email = dto.Email,
                Direccion = dto.Direccion,
                FkRol = dto.FkRol,
                Estado = dto.Estado,
                Contrasena = dto.Contrasena != null 
                    ? BCrypt.Net.BCrypt.HashPassword(dto.Contrasena) 
                    : BCrypt.Net.BCrypt.HashPassword("123456") // default
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            // devolver con DTO
            var result = await _context.Usuario
                .Include(u => u.FkRolNavigation)
                .Where(u => u.IdUsuario == usuario.IdUsuario)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Celular = u.Celular,
                    Rol = u.FkRolNavigation != null ? u.FkRolNavigation.NombreRol : "Sin rol",
                    Estado = u.Estado
                })
                .FirstAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, result);
        }

        // PUT: /usuarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuarioCreateOrUpdateDto dto)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.TipoDoc = dto.TipoDoc;
            usuario.Documento = dto.Documento;
            usuario.Nombre = dto.Nombre;
            usuario.Celular = dto.Celular;
            usuario.Email = dto.Email;
            usuario.Direccion = dto.Direccion;
            usuario.FkRol = dto.FkRol;
            usuario.Estado = dto.Estado;

            if (!string.IsNullOrEmpty(dto.Contrasena))
            {
                usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /usuarios/{id} → inactivar
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.Estado = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
