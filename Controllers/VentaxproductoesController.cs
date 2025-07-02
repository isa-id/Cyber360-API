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
    public class VentaxproductoController : ControllerBase
    {
        private readonly NeondbContext _context;

        public VentaxproductoController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/Ventaxproducto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ventaxproducto>>> GetVentaxproductos()
        {
            return await _context.Ventaxproductos
                .Include(v => v.FkVentaNavigation)
                .ToListAsync();
        }

        // GET: api/Ventaxproducto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ventaxproducto>> GetVentaxproducto(int id)
        {
            var ventaxproducto = await _context.Ventaxproductos
                .Include(v => v.FkVentaNavigation)
                .FirstOrDefaultAsync(v => v.IdVentaxproducto == id);

            if (ventaxproducto == null)
            {
                return NotFound();
            }

            return ventaxproducto;
        }

        // POST: api/Ventaxproducto
        [HttpPost]
        public async Task<ActionResult<Ventaxproducto>> PostVentaxproducto(Ventaxproducto ventaxproducto)
        {
            _context.Ventaxproductos.Add(ventaxproducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVentaxproducto), new { id = ventaxproducto.IdVentaxproducto }, ventaxproducto);
        }

        // PUT: api/Ventaxproducto/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVentaxproducto(int id, Ventaxproducto ventaxproducto)
        {
            if (id != ventaxproducto.IdVentaxproducto)
            {
                return BadRequest();
            }

            _context.Entry(ventaxproducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VentaxproductoExists(id))
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

        // DELETE: api/Ventaxproducto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVentaxproducto(int id)
        {
            var ventaxproducto = await _context.Ventaxproductos.FindAsync(id);
            if (ventaxproducto == null)
            {
                return NotFound();
            }

            _context.Ventaxproductos.Remove(ventaxproducto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VentaxproductoExists(int id)
        {
            return _context.Ventaxproductos.Any(e => e.IdVentaxproducto == id);
        }
    }
}
