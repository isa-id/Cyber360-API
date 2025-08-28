using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly NeondbContext _context;

        public PermisosController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/permisos
        // Devuelve lista simple de permisos { id, nombre } para consumir en el frontend
        [HttpGet]
        public async Task<IActionResult> GetPermisos()
        {
            var permisos = await _context.Permisos
                .AsNoTracking()
                .Select(p => new
                {
                    id = p.IdPermiso,
                    nombre = p.NombrePermiso
                })
                .ToListAsync();

            return Ok(permisos);
        }

        // GET: api/permisos/5
        // Devuelve el permiso y los roles que lo contienen
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermiso(int id)
        {
            var permiso = await _context.Permisos
                .AsNoTracking()
                .Where(p => p.IdPermiso == id)
                .Select(p => new
                {
                    id = p.IdPermiso,
                    nombre = p.NombrePermiso,
                    roles = p.Permisoxrols
                        .Select(px => new
                        {
                            id = px.FkRolNavigation.IdRol,
                            nombre = px.FkRolNavigation.NombreRol
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (permiso == null)
                return NotFound();

            return Ok(permiso);
        }

        // POST: api/permisos
        [HttpPost]
        public async Task<IActionResult> PostPermiso([FromBody] Permiso permiso)
        {
            if (permiso == null || string.IsNullOrWhiteSpace(permiso.NombrePermiso))
                return BadRequest("NombrePermiso es obligatorio.");

            // Evitar duplicados por nombre
            var existe = await _context.Permisos.AnyAsync(p => p.NombrePermiso == permiso.NombrePermiso);
            if (existe)
                return Conflict("Ya existe un permiso con ese nombre.");

            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();

            // Devolvemos la representación ligera creada
            return CreatedAtAction(nameof(GetPermiso), new { id = permiso.IdPermiso }, new { id = permiso.IdPermiso, nombre = permiso.NombrePermiso });
        }

        // PUT: api/permisos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermiso(int id, [FromBody] Permiso permiso)
        {
            if (permiso == null || id != permiso.IdPermiso)
                return BadRequest("Request inválido.");

            var existente = await _context.Permisos.FindAsync(id);
            if (existente == null)
                return NotFound();

            existente.NombrePermiso = permiso.NombrePermiso;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar el permiso.");
            }

            return NoContent();
        }

        // DELETE: api/permisos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermiso(int id)
        {
            var permiso = await _context.Permisos
                .Include(p => p.Permisoxrols)
                .FirstOrDefaultAsync(p => p.IdPermiso == id);

            if (permiso == null)
                return NotFound();

            // Evitamos borrado si está asignado a roles; podrías cambiar esto para eliminar relaciones primero si lo prefieres
            if (permiso.Permisoxrols != null && permiso.Permisoxrols.Any())
            {
                return BadRequest("No se puede eliminar el permiso: está asignado a uno o más roles. Elimina las relaciones primero.");
            }

            _context.Permisos.Remove(permiso);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
