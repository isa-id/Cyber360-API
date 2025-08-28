using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models.Compras;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers.Compras
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleComprasController : ControllerBase
    {
        private readonly NeondbContext _context;

        public DetalleComprasController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/detallecompras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleCompra>>> GetDetalleCompras()
        {
            return await _context.DetalleCompras
                .Include(d => d.Compra)
                .Include(d => d.Producto)
                .ToListAsync();
        }

        // GET: api/detallecompras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleCompra>> GetDetalleCompra(int id)
        {
            var detalleCompra = await _context.DetalleCompras
                .Include(d => d.Compra)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (detalleCompra == null)
            {
                return NotFound();
            }

            return detalleCompra;
        }

        // GET: api/detallecompras/compra/5
        [HttpGet("compra/{compraId}")]
        public async Task<ActionResult<IEnumerable<DetalleCompra>>> GetDetalleComprasByCompra(int compraId)
        {
            var detalles = await _context.DetalleCompras
                .Include(d => d.Compra)
                .Include(d => d.Producto)
                .Where(d => d.CompraId == compraId)
                .ToListAsync();

            return detalles;
        }

        // POST: api/detallecompras
        [HttpPost]
        public async Task<ActionResult<DetalleCompra>> PostDetalleCompra(DetalleCompra detalleCompra)
        {
            // Validar que la compra existe
            var compraExiste = await _context.Compras.AnyAsync(c => c.Id == detalleCompra.CompraId);
            if (!compraExiste)
            {
                return BadRequest("La compra especificada no existe.");
            }

            // Validar que el producto existe
            var productoExiste = await _context.Productos.AnyAsync(p => p.Id == detalleCompra.ProductoId);
            if (!productoExiste)
            {
                return BadRequest("El producto especificado no existe.");
            }

            // Validar cantidades y precios
            if (detalleCompra.Cantidad <= 0)
            {
                return BadRequest("La cantidad debe ser mayor a cero.");
            }

            if (detalleCompra.PrecioUnitario <= 0)
            {
                return BadRequest("El precio unitario debe ser mayor a cero.");
            }

            // Calcular subtotal si no viene especificado
            if (detalleCompra.SubtotalItems <= 0)
            {
                detalleCompra.SubtotalItems = detalleCompra.Cantidad * detalleCompra.PrecioUnitario;
            }

            _context.DetalleCompras.Add(detalleCompra);
            await _context.SaveChangesAsync();

            // Cargar las relaciones para la respuesta
            await _context.Entry(detalleCompra)
                .Reference(d => d.Compra)
                .LoadAsync();

            await _context.Entry(detalleCompra)
                .Reference(d => d.Producto)
                .LoadAsync();

            return CreatedAtAction(nameof(GetDetalleCompra), new { id = detalleCompra.Id }, detalleCompra);
        }

        // PUT: api/detallecompras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleCompra(int id, DetalleCompra detalleCompra)
        {
            if (id != detalleCompra.Id)
            {
                return BadRequest();
            }

            // Validar que la compra existe
            var compraExiste = await _context.Compras.AnyAsync(c => c.Id == detalleCompra.CompraId);
            if (!compraExiste)
            {
                return BadRequest("La compra especificada no existe.");
            }

            // Validar que el producto existe
            var productoExiste = await _context.Productos.AnyAsync(p => p.Id == detalleCompra.ProductoId);
            if (!productoExiste)
            {
                return BadRequest("El producto especificado no existe.");
            }

            // Validar cantidades y precios
            if (detalleCompra.Cantidad <= 0)
            {
                return BadRequest("La cantidad debe ser mayor a cero.");
            }

            if (detalleCompra.PrecioUnitario <= 0)
            {
                return BadRequest("El precio unitario debe ser mayor a cero.");
            }

            // Calcular subtotal si no viene especificado
            if (detalleCompra.SubtotalItems <= 0)
            {
                detalleCompra.SubtotalItems = detalleCompra.Cantidad * detalleCompra.PrecioUnitario;
            }

            _context.Entry(detalleCompra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleCompraExists(id))
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

        // DELETE: api/detallecompras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleCompra(int id)
        {
            var detalleCompra = await _context.DetalleCompras.FindAsync(id);
            if (detalleCompra == null)
            {
                return NotFound();
            }

            _context.DetalleCompras.Remove(detalleCompra);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetalleCompraExists(int id)
        {
            return _context.DetalleCompras.Any(e => e.Id == id);
        }
    }
}