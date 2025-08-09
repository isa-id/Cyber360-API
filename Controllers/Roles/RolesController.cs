using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;

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

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            role.Activo = true; // Forzamos que siempre sea activo al crearlo
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRole), new { id = role.IdRol }, role);
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            if (id != role.IdRol)
                return BadRequest();

            _context.Entry(role).State = EntityState.Modified;

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

            role.Activo = activo;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        // DELETE: api/Roles/5 (borrado físico)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return NotFound();

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
