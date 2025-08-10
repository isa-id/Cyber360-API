using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosRolController : ControllerBase
    {
        private readonly NeondbContext _context;

        public PermisosRolController(NeondbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/PermisosRol/rol/2
        [HttpGet("rol/{rolId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPermisosDeRol(int rolId)
        {
            if (!await _context.Roles.AnyAsync(r => r.IdRol == rolId))
                return NotFound($"No existe el rol con id {rolId}");

            var permisos = await _context.Permisoxrols
                .Where(pr => pr.FkRol == rolId)
                .Include(pr => pr.FkPermisoNavigation)
                .Select(pr => new
                {
                    pr.FkPermisoNavigation.IdPermiso,
                    pr.FkPermisoNavigation.NombrePermiso
                })
                .ToListAsync();

            return Ok(permisos);
        }

        // ✅ POST: api/PermisosRol
        [HttpPost]
        public async Task<IActionResult> AsignarPermisos([FromBody] AsignarPermisosDto dto)
        {
            if (dto == null || dto.PermisosIds == null || !dto.PermisosIds.Any())
                return BadRequest("Datos inválidos");

            if (!await _context.Roles.AnyAsync(r => r.IdRol == dto.RolId))
                return NotFound($"No existe el rol con id {dto.RolId}");

            var permisosExistentes = await _context.Permisos
                .Where(p => dto.PermisosIds.Contains(p.IdPermiso))
                .Select(p => p.IdPermiso)
                .ToListAsync();

            var idsInvalidos = dto.PermisosIds.Except(permisosExistentes).ToList();
            if (idsInvalidos.Any())
                return BadRequest($"Los siguientes permisos no existen: {string.Join(", ", idsInvalidos)}");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var permisoId in permisosExistentes)
                {
                    if (!await _context.Permisoxrols.AnyAsync(pr => pr.FkRol == dto.RolId && pr.FkPermiso == permisoId))
                    {
                        _context.Permisoxrols.Add(new Permisoxrol
                        {
                            FkRol = dto.RolId,
                            FkPermiso = permisoId
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Permisos asignados correctamente" });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // ✅ DELETE: api/PermisosRol/rol/2/permiso/3
        [HttpDelete("rol/{rolId}/permiso/{permisoId}")]
        public async Task<IActionResult> EliminarPermisoDeRol(int rolId, int permisoId)
        {
            var relacion = await _context.Permisoxrols
                .FirstOrDefaultAsync(pr => pr.FkRol == rolId && pr.FkPermiso == permisoId);

            if (relacion == null)
                return NotFound("Relación no encontrada");

            _context.Permisoxrols.Remove(relacion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Permiso eliminado del rol" });
        }

        // ✅ PUT: api/PermisosRol/rol/2
        [HttpPut("rol/{rolId}")]
        public async Task<IActionResult> ReemplazarPermisos(int rolId, [FromBody] List<int> nuevosPermisos)
        {
            if (!await _context.Roles.AnyAsync(r => r.IdRol == rolId))
                return NotFound($"No existe el rol con id {rolId}");

            var permisosExistentes = await _context.Permisos
                .Where(p => nuevosPermisos.Contains(p.IdPermiso))
                .Select(p => p.IdPermiso)
                .ToListAsync();

            var idsInvalidos = nuevosPermisos.Except(permisosExistentes).ToList();
            if (idsInvalidos.Any())
                return BadRequest($"Los siguientes permisos no existen: {string.Join(", ", idsInvalidos)}");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existentes = _context.Permisoxrols.Where(pr => pr.FkRol == rolId);
                _context.Permisoxrols.RemoveRange(existentes);

                foreach (var permisoId in permisosExistentes)
                {
                    _context.Permisoxrols.Add(new Permisoxrol
                    {
                        FkRol = rolId,
                        FkPermiso = permisoId
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Permisos reemplazados correctamente" });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        // 1️⃣ Obtener todos los permisos con indicador si el rol ya lo tiene asignado
        [HttpGet("rol/{rolId}/permisos-disponibles")]
        public async Task<ActionResult> GetPermisosDisponiblesParaRol(int rolId)
        {
            var rolExiste = await _context.Roles.AnyAsync(r => r.IdRol == rolId);
            if (!rolExiste)
                return NotFound($"No existe el rol con id {rolId}");

            var permisos = await _context.Permisos
                .Select(p => new
                {
                    p.IdPermiso,
                    p.NombrePermiso,
                    Asignado = _context.Permisoxrols
                        .Any(pr => pr.FkRol == rolId && pr.FkPermiso == p.IdPermiso)
                })
                .ToListAsync();

            return Ok(permisos);
        }

        // 2️⃣ Asignar permisos a un rol (reemplaza lo que tenga actualmente)
        [HttpPost("rol/{rolId}/asignar")]
        public async Task<ActionResult> AsignarPermisosARol(int rolId, [FromBody] List<int> permisosIds)
        {
            var rol = await _context.Roles.FindAsync(rolId);
            if (rol == null)
                return NotFound($"No existe el rol con id {rolId}");

            // Eliminar permisos actuales
            var actuales = _context.Permisoxrols.Where(pr => pr.FkRol == rolId);
            _context.Permisoxrols.RemoveRange(actuales);

            // Agregar los nuevos
            var nuevos = permisosIds.Select(pid => new Permisoxrol
            {
                FkRol = rolId,
                FkPermiso = pid
            }).ToList();

            await _context.Permisoxrols.AddRangeAsync(nuevos);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Permisos asignados correctamente" });
        }

        // DTO para el POST
        public class AsignarPermisosDto
        {
            public int RolId { get; set; }
            public required List<int> PermisosIds { get; set; } 
        }


    }
}