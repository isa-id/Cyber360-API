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
    public class VentasController : ControllerBase
    {
        private readonly NeondbContext _context;

        public VentasController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/Ventas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            return await _context.Ventas
                .Include(v => v.FkClienteNavigation)
                .Include(v => v.Ventaxproductos)
                    .ThenInclude(vxp => vxp.FkProductoNavigation)
                .ToListAsync();
        }

        // GET: api/Ventas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.FkClienteNavigation)
                .Include(v => v.Ventaxproductos)
                    .ThenInclude(vxp => vxp.FkProductoNavigation)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null)
            {
                return NotFound(new { message = $"Venta con ID {id} no encontrada" });
            }

            return venta;
        }

        // POST: api/Ventas
        [HttpPost]
        public async Task<ActionResult<Venta>> PostVenta([FromBody] Venta venta)
        {
            // Validación básica
            if (venta == null)
            {
                return BadRequest(new { message = "El objeto Venta no puede ser nulo" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar que el cliente existe
            var clienteExists = await _context.Clientes.AnyAsync(c => c.IdCliente == venta.FkCliente);
            if (!clienteExists)
            {
                return BadRequest(new { message = "El cliente especificado no existe" });
            }

            try
            {
                // Asegurar que no se envíe un ID (autogenerado)
                venta.IdVenta = 0;

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetVenta), new { id = venta.IdVenta }, venta);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al crear la venta", error = ex.InnerException?.Message });
            }
        }

        // PUT: api/Ventas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenta(int id, [FromBody] Venta venta)
        {
            if (id != venta.IdVenta)
            {
                return BadRequest(new { message = "ID de la venta no coincide" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar que existe
            var ventaExistente = await _context.Ventas.FindAsync(id);
            if (ventaExistente == null)
            {
                return NotFound(new { message = $"Venta con ID {id} no encontrada" });
            }

            // Verificar que el cliente existe
            var clienteExists = await _context.Clientes.AnyAsync(c => c.IdCliente == venta.FkCliente);
            if (!clienteExists)
            {
                return BadRequest(new { message = "El cliente especificado no existe" });
            }

            try
            {
                // Actualizar propiedades
                ventaExistente.FkCliente = venta.FkCliente;
                ventaExistente.Fecha = venta.Fecha;
                ventaExistente.MetodoPago = venta.MetodoPago;
                ventaExistente.Estado = venta.Estado;
                ventaExistente.Total = venta.Total;

                _context.Entry(ventaExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!VentaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, new { message = "Error de concurrencia al actualizar la venta", error = ex.Message });
                }
            }
        }

        // DELETE: api/Ventas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenta(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound(new { message = $"Venta con ID {id} no encontrada" });
            }

            try
            {
                _context.Ventas.Remove(venta);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al eliminar la venta", error = ex.InnerException?.Message });
            }
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.IdVenta == id);
        }
    }
}
