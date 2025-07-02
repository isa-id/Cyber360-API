using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicioxventaController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ServicioxventaController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/Servicioxventa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicioxventum>>> GetServicioxventa()
        {
            return await _context.Servicioxventa
                .Include(s => s.FkServicioNavigation)
                .Include(s => s.FkVentaNavigation)
                .ToListAsync();
        }

        // GET: api/Servicioxventa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servicioxventum>> GetServicioxventum(int id)
        {
            var servicioxventum = await _context.Servicioxventa
                .Include(s => s.FkServicioNavigation)
                .Include(s => s.FkVentaNavigation)
                .FirstOrDefaultAsync(s => s.IdServicioxventa == id);

            if (servicioxventum == null)
            {
                return NotFound();
            }

            return servicioxventum;
        }

        // POST: api/Servicioxventa
        [HttpPost]
        public async Task<ActionResult<Servicioxventum>> PostServicioxventum(Servicioxventum servicioxventum)
        {
            _context.Servicioxventa.Add(servicioxventum);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServicioxventum), new { id = servicioxventum.IdServicioxventa }, servicioxventum);
        }

        // PUT: api/Servicioxventa/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicioxventum(int id, Servicioxventum servicioxventum)
        {
            if (id != servicioxventum.IdServicioxventa)
            {
                return BadRequest();
            }

            _context.Entry(servicioxventum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioxventumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Servicioxventa/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicioxventum(int id)
        {
            var servicioxventum = await _context.Servicioxventa.FindAsync(id);
            if (servicioxventum == null)
            {
                return NotFound();
            }

            _context.Servicioxventa.Remove(servicioxventum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicioxventumExists(int id)
        {
            return _context.Servicioxventa.Any(e => e.IdServicioxventa == id);
        }
    }
}
