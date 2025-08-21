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
                .Include(u => u.FkRolNavigation)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Celular = u.Celular,
                    Rol = u.FkRolNavigation != null ? u.FkRolNavigation.NombreRol : "Sin rol",
                    Estado = u.Estado,
                    TipoDoc = u.TipoDoc,
                    Documento = u.Documento,
                    Direccion = u.Direccion
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
                    Estado = u.Estado,
                    TipoDoc = u.TipoDoc,
                    Documento = u.Documento,
                    Direccion = u.Direccion
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
            dto.Email = dto.Email.Trim().ToLower(); // Normalizamos

            // Validar si el email ya existe
            var emailExists = await _context.Usuario.AnyAsync(u => u.Email.ToLower() == dto.Email);
            if (emailExists)
            {
                return BadRequest("Ya existe un usuario con ese email.");
            }

            // Validar si el documento ya existe
            var docExists = await _context.Usuario.AnyAsync(u => u.Documento == dto.Documento);
            if (docExists)
            {
                return BadRequest("Ya existe un usuario con ese documento.");
            }

            // Validar si el rol existe y está activo
            var rol = await _context.Roles
                .Where(r => r.IdRol == dto.FkRol && r.Activo == true)
                .FirstOrDefaultAsync();

            if (rol == null)
            {
                return BadRequest($"El rol con id {dto.FkRol} no existe o está inactivo.");
            }

            var usuario = new Usuario
            {
                TipoDoc = dto.TipoDoc,
                Documento = dto.Documento,
                Nombre = dto.Nombre,
                Celular = dto.Celular,
                Email = dto.Email, // guardamos en minúsculas
                Direccion = dto.Direccion,
                FkRol = dto.FkRol,
                Estado = true,
                Contrasena = dto.Contrasena != null
                    ? BCrypt.Net.BCrypt.HashPassword(dto.Contrasena)
                    : BCrypt.Net.BCrypt.HashPassword("123456")
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            var result = new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Celular = usuario.Celular,
                Rol = rol.NombreRol,
                Estado = usuario.Estado
            };

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, result);
        }


        // PUT: /usuarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuarioCreateOrUpdateDto dto)
        {
            dto.Email = dto.Email.Trim().ToLower(); // Normalizamos

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
                return NotFound();

            // Validar que no haya otro usuario con el mismo email
            var emailExists = await _context.Usuario.AnyAsync(u => u.Email.ToLower() == dto.Email && u.IdUsuario != id);
            if (emailExists)
            {
                return BadRequest("Ya existe otro usuario con ese email.");
            }

            // Validar que no haya otro usuario con el mismo documento
            var docExists = await _context.Usuario.AnyAsync(u => u.Documento == dto.Documento && u.IdUsuario != id);
            if (docExists)
            {
                return BadRequest("Ya existe otro usuario con ese documento.");
            }

            // Validar rol (activo)
            var rol = await _context.Roles
                .Where(r => r.IdRol == dto.FkRol && r.Activo == true)
                .FirstOrDefaultAsync();

            if (rol == null)
            {
                return BadRequest($"El rol con id {dto.FkRol} no existe o está inactivo.");
            }

            usuario.TipoDoc = dto.TipoDoc;
            usuario.Documento = dto.Documento;
            usuario.Nombre = dto.Nombre;
            usuario.Celular = dto.Celular;
            usuario.Email = dto.Email; // siempre guardamos en minúsculas
            usuario.Direccion = dto.Direccion;
            usuario.FkRol = dto.FkRol;

            if (!string.IsNullOrEmpty(dto.Contrasena))
            {
                usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
        // PATCH: /usuarios/{id}/estado


        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoUsuario(int id, [FromBody] bool estado)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.Estado = estado;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /usuarios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
