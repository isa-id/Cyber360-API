using System;
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
    public class ServicioxinsumosController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ServicioxinsumosController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/Servicioxinsumos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicioxinsumo>>> GetServicioxinsumos()
        {
            return await _context.Servicioxinsumos
                .Include(s => s.FkProductoNavigation)
                .Include(s => s.FkServicioNavigation)
                .ToListAsync();
        }

        // GET: api/Servicioxinsumos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servicioxinsumo>> GetServicioxinsumo(int id)
        {
            var servicioxinsumo = await _context.Servicioxinsumos
                .Include(s => s.FkProductoNavigation)
                .Include(s => s.FkServicioNavigation)
                .FirstOrDefaultAsync(s => s.IdServicioxinsumo == id);

            if (servicioxinsumo == null)
            {
                return NotFound();
            }

            return servicioxinsumo;
        }

        // POST: api/Servicioxinsumos
        [HttpPost]
        public async Task<ActionResult<Servicioxinsumo>> PostServicioxinsumo(Servicioxinsumo servicioxinsumo)
        {
            // Validar existencia de las entidades relacionadas
            if (!await _context.Productos.AnyAsync(p => p.IdProducto == servicioxinsumo.FkProducto))
            {
                return BadRequest("El producto especificado no existe");
            }

            if (!await _context.Servicios.AnyAsync(s => s.IdServicio == servicioxinsumo.FkServicio))
            {
                return BadRequest("El servicio especificado no existe");
            }

            _context.Servicioxinsumos.Add(servicioxinsumo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServicioxinsumo", new { id = servicioxinsumo.IdServicioxinsumo }, servicioxinsumo);
        }

        // PUT: api/Servicioxinsumos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicioxinsumo(int id, Servicioxinsumo servicioxinsumo)
        {
            if (id != servicioxinsumo.IdServicioxinsumo)
            {
                return BadRequest();
            }

            // Validar existencia de las entidades relacionadas
            if (!await _context.Productos.AnyAsync(p => p.IdProducto == servicioxinsumo.FkProducto))
            {
                return BadRequest("El producto especificado no existe");
            }

            if (!await _context.Servicios.AnyAsync(s => s.IdServicio == servicioxinsumo.FkServicio))
            {
                return BadRequest("El servicio especificado no existe");
            }

            _context.Entry(servicioxinsumo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioxinsumoExists(id))
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

        // DELETE: api/Servicioxinsumos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicioxinsumo(int id)
        {
            var servicioxinsumo = await _context.Servicioxinsumos.FindAsync(id);
            if (servicioxinsumo == null)
            {
                return NotFound();
            }

            _context.Servicioxinsumos.Remove(servicioxinsumo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicioxinsumoExists(int id)
        {
            return _context.Servicioxinsumos.Any(e => e.IdServicioxinsumo == id);
        }
    }
}
