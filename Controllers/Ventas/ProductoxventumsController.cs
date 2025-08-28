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
    public class ProductoxventaController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ProductoxventaController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/productoxventa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Productoxventum>>> GetProductoxventa()
        {
            return await _context.Productoxventa
                .Select(pv => new Productoxventum
                {
                    Id = pv.Id,
                    ProductoId = pv.ProductoId,
                    Cantidad = pv.Cantidad,
                    ValorUnitario = pv.ValorUnitario,
                    ValorTotal = pv.ValorTotal,
                    VentaId = pv.VentaId
                })
                .ToListAsync();
        }

        // GET: api/productoxventa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Productoxventum>> GetProductoxventum(int id)
        {
            var productoxventum = await _context.Productoxventa
                .Where(pv => pv.Id == id)
                .Select(pv => new Productoxventum
                {
                    Id = pv.Id,
                    ProductoId = pv.ProductoId,
                    Cantidad = pv.Cantidad,
                    ValorUnitario = pv.ValorUnitario,
                    ValorTotal = pv.ValorTotal,
                    VentaId = pv.VentaId
                })
                .FirstOrDefaultAsync();

            if (productoxventum == null)
            {
                return NotFound();
            }

            return productoxventum;
        }

        // POST: api/productoxventa
        [HttpPost]
        public async Task<ActionResult<Productoxventum>> PostProductoxventum([FromBody] Productoxventum productoxventum)
        {
            if (productoxventum == null)
            {
                return BadRequest("El objeto productoxventum no puede ser nulo");
            }

            // Validar campos requeridos
            if (!productoxventum.ProductoId.HasValue)
            {
                return BadRequest("El campo productoId es requerido");
            }

            if (!productoxventum.VentaId.HasValue)
            {
                return BadRequest("El campo ventaId es requerido");
            }

            // Crear un nuevo objeto con solo los campos necesarios
            var nuevoProductoxventa = new Productoxventum
            {
                ProductoId = productoxventum.ProductoId,
                Cantidad = productoxventum.Cantidad,
                ValorUnitario = productoxventum.ValorUnitario,
                ValorTotal = productoxventum.ValorTotal,
                VentaId = productoxventum.VentaId
                // No asignamos el Id, ya que será generado automáticamente
            };

            _context.Productoxventa.Add(nuevoProductoxventa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductoxventum), new { id = nuevoProductoxventa.Id }, nuevoProductoxventa);
        }

        // PUT: api/productoxventa/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductoxventum(int id, [FromBody] Productoxventum productoxventum)
        {
            if (productoxventum == null)
            {
                return BadRequest("El objeto productoxventum no puede ser nulo");
            }

            if (id != productoxventum.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del objeto");
            }

            // Obtener el registro existente
            var productoxventaExistente = await _context.Productoxventa.FindAsync(id);
            if (productoxventaExistente == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos necesarios
            productoxventaExistente.ProductoId = productoxventum.ProductoId;
            productoxventaExistente.Cantidad = productoxventum.Cantidad;
            productoxventaExistente.ValorUnitario = productoxventum.ValorUnitario;
            productoxventaExistente.ValorTotal = productoxventum.ValorTotal;
            productoxventaExistente.VentaId = productoxventum.VentaId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoxventumExists(id))
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

        // DELETE: api/productoxventa/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductoxventum(int id)
        {
            var productoxventum = await _context.Productoxventa.FindAsync(id);
            if (productoxventum == null)
            {
                return NotFound();
            }

            _context.Productoxventa.Remove(productoxventum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoxventumExists(int id)
        {
            return _context.Productoxventa.Any(e => e.Id == id);
        }
    }
}