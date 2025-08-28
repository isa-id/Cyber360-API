using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Cyber360.DTOs;
using backend.Data;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly NeondbContext _context; // Cambia YourDbContext por el nombre real

        public RolesController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles([FromQuery] bool? soloActivos)
        {
            IQueryable<Role> query = _context.Roles;

            if (soloActivos.HasValue && soloActivos.Value)
                query = query.Where(r => r.Activo);

            return await query.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                return NotFound();

            return role;
        }

        [HttpPost]
        public async Task<ActionResult<Role>> PostRole([FromBody] RoleCreateDto roleDto)
        {
            // 🔹 Validación de nombre duplicado
            bool existeNombre = await _context.Roles
                .AnyAsync(r => r.NombreRol.ToLower() == roleDto.NombreRol.ToLower());
            if (existeNombre)
                return BadRequest("Ya existe un rol con ese nombre.");

            // 🔹 Validación de permisos duplicados
            var rolesExistentes = await _context.Roles
                .Include(r => r.Permisoxrols)
                .ToListAsync();

            foreach (var rol in rolesExistentes)
            {
                var permisosRol = rol.Permisoxrols.Select(p => p.FkPermiso).OrderBy(p => p).ToList();
                var permisosNuevoRol = roleDto.PermisosIds.OrderBy(p => p).ToList();
                if (permisosRol.SequenceEqual(permisosNuevoRol))
                    return BadRequest("Ya existe un rol con esos mismos permisos.");
            }

            // Crear rol
            var role = new Role
            {
                NombreRol = roleDto.NombreRol,
                Descripcion = roleDto.Descripcion,
                Activo = roleDto.Activo
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            // Guardar permisos
            foreach (var permisoId in roleDto.PermisosIds)
            {
                _context.Permisoxrols.Add(new Permisoxrol
                {
                    FkRol = role.IdRol,
                    FkPermiso = permisoId
                });
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRole), new { id = role.IdRol }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, [FromBody] RoleCreateDto roleDto)
        {
            if (id != roleDto.IdRol)
                return BadRequest("El ID del rol no coincide.");

            // Validación nombre duplicado
            bool existeNombre = await _context.Roles
                .AnyAsync(r => r.NombreRol.ToLower() == roleDto.NombreRol.ToLower() && r.IdRol != id);
            if (existeNombre)
                return BadRequest("Ya existe otro rol con ese nombre.");

            // Validación permisos duplicados
            var rolesExistentes = await _context.Roles
                .Include(r => r.Permisoxrols)
                .Where(r => r.IdRol != id)
                .ToListAsync();

            foreach (var rol in rolesExistentes)
            {
                var permisosRol = rol.Permisoxrols.Select(p => p.FkPermiso).OrderBy(p => p).ToList();
                var permisosActuales = roleDto.PermisosIds.OrderBy(p => p).ToList();
                if (permisosRol.SequenceEqual(permisosActuales))
                    return BadRequest("Ya existe otro rol con esos mismos permisos.");
            }

            var role = await _context.Roles.Include(r => r.Permisoxrols).FirstOrDefaultAsync(r => r.IdRol == id);
            if (role == null) return NotFound();

            role.NombreRol = roleDto.NombreRol;
            role.Descripcion = roleDto.Descripcion;
            role.Activo = roleDto.Activo;

            _context.Permisoxrols.RemoveRange(role.Permisoxrols);
            foreach (var permisoId in roleDto.PermisosIds)
            {
                _context.Permisoxrols.Add(new Permisoxrol
                {
                    FkRol = role.IdRol,
                    FkPermiso = permisoId
                });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }




        // PATCH: api/Roles/CambiarEstado/5?activo=true
        [HttpPatch("CambiarEstado/{id}")]
        public async Task<IActionResult> CambiarEstadoRole(int id, [FromQuery] bool activo)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return NotFound();

            // Suponiendo que el ID del rol Administrador es 1
            if (role.IdRol == 1 && !activo)
            {
                return BadRequest("El rol Administrador no puede ser inactivado.");
            }

            role.Activo = activo;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        // DELETE: api/Roles/5 (borrado físico)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles
                .Include(r => r.Permisoxrols) // Incluir permisos relacionados
                .FirstOrDefaultAsync(r => r.IdRol == id);

            if (role == null)
                return NotFound();

            // Eliminar primero las relaciones en permisoxrol
            _context.Permisoxrols.RemoveRange(role.Permisoxrols);

            // Luego eliminar el rol
            _context.Roles.Remove(role);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.IdRol == id);
        }

        // GET: api/Roles/Dropdown
        [HttpGet("Dropdown")]
        public async Task<ActionResult<IEnumerable<object>>> GetRolesDropdown([FromQuery] bool? soloActivos)
        {
            IQueryable<Role> query = _context.Roles;

            if (soloActivos.HasValue && soloActivos.Value)
                query = query.Where(r => r.Activo);

            var roles = await query
                .Select(r => new
                {
                    r.IdRol,
                    r.NombreRol
                })
                .ToListAsync();

            return Ok(roles);
        }
    }
}
