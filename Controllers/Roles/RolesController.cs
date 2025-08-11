using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Cyber360.DTOs;

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
            var role = new Role
            {
                NombreRol = roleDto.NombreRol,
                Descripcion = roleDto.Descripcion,
                Activo = roleDto.Activo
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            // Guardar permisos relacionados (creamos entradas en Permisoxrol)
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
            if (id != roleDto.IdRol) // Si decides agregar IdRol en el DTO para PUT, o pásalo como parámetro separado
                return BadRequest("El ID del rol no coincide.");

            var role = await _context.Roles.Include(r => r.Permisoxrols).FirstOrDefaultAsync(r => r.IdRol == id);
            if (role == null)
                return NotFound();

            // Actualizar campos simples
            role.NombreRol = roleDto.NombreRol;
            role.Descripcion = roleDto.Descripcion;
            role.Activo = roleDto.Activo;

            // Actualizar permisos relacionados

            // 1. Eliminar permisos anteriores
            _context.Permisoxrols.RemoveRange(role.Permisoxrols);

            // 2. Agregar nuevos permisos
            if (roleDto.PermisosIds != null && roleDto.PermisosIds.Any())
            {
                foreach (var permisoId in roleDto.PermisosIds)
                {
                    _context.Permisoxrols.Add(new Permisoxrol
                    {
                        FkRol = role.IdRol,
                        FkPermiso = permisoId
                    });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                    return NotFound();
                else
                    throw;
            }

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
    }
}
