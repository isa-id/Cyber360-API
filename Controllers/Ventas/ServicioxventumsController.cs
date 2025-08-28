using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models.Ventas;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers.Ventas
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioxventumsController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ServicioxventumsController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/servicioxventums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicioxventum>>> GetServicioxventa()
        {
            return await _context.Servicioxventa
                .Select(sv => new Servicioxventum
                {
                    Id = sv.Id,
                    ServicioId = sv.ServicioId,
                    Detalles = sv.Detalles,
                    ValorTotal = sv.ValorTotal,
                    VentaId = sv.VentaId
                })
                .ToListAsync();
        }

        // GET: api/servicioxventums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servicioxventum>> GetServicioxventum(int id)
        {
            var servicioxventum = await _context.Servicioxventa
                .Where(sv => sv.Id == id)
                .Select(sv => new Servicioxventum
                {
                    Id = sv.Id,
                    ServicioId = sv.ServicioId,
                    Detalles = sv.Detalles,
                    ValorTotal = sv.ValorTotal,
                    VentaId = sv.VentaId
                })
                .FirstOrDefaultAsync();

            if (servicioxventum == null)
            {
                return NotFound();
            }

            return servicioxventum;
        }

        // POST: api/servicioxventums
        [HttpPost]
        public async Task<ActionResult<Servicioxventum>> PostServicioxventum([FromBody] Servicioxventum servicioxventum)
        {
            if (servicioxventum == null)
            {
                return BadRequest("El objeto servicioxventum no puede ser nulo");
            }

            if (!servicioxventum.ServicioId.HasValue)
            {
                return BadRequest("El campo servicioId es requerido");
            }

            if (!servicioxventum.VentaId.HasValue)
            {
                return BadRequest("El campo ventaId es requerido");
            }

            var nuevoServicioxventa = new Servicioxventum
            {
                ServicioId = servicioxventum.ServicioId,
                Detalles = servicioxventum.Detalles,
                ValorTotal = servicioxventum.ValorTotal,
                VentaId = servicioxventum.VentaId
            };

            _context.Servicioxventa.Add(nuevoServicioxventa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServicioxventum), new { id = nuevoServicioxventa.Id }, nuevoServicioxventa);
        }

        // PUT: api/servicioxventums/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicioxventum(int id, [FromBody] Servicioxventum servicioxventum)
        {
            if (servicioxventum == null)
            {
                return BadRequest("El objeto servicioxventum no puede ser nulo");
            }

            if (id != servicioxventum.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del objeto");
            }

            var servicioxventaExistente = await _context.Servicioxventa.FindAsync(id);
            if (servicioxventaExistente == null)
            {
                return NotFound();
            }

            servicioxventaExistente.ServicioId = servicioxventum.ServicioId;
            servicioxventaExistente.Detalles = servicioxventum.Detalles;
            servicioxventaExistente.ValorTotal = servicioxventum.ValorTotal;
            servicioxventaExistente.VentaId = servicioxventum.VentaId;

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

        // DELETE: api/servicioxventums/5
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
            return _context.Servicioxventa.Any(e => e.Id == id);
        }
    }
}