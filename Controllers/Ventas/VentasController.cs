using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models.Ventas;

namespace backend.Controllers.Ventas
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly NeondbContext _context;

        public VentasController(NeondbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            return await _context.Ventas
                .Select(v => new Venta
                {
                    Id = v.Id,
                    Fecha = v.Fecha,
                    ClienteId = v.ClienteId,
                    Total = v.Total
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            var venta = await _context.Ventas
                .Where(v => v.Id == id)
                .Select(v => new Venta
                {
                    Id = v.Id,
                    Fecha = v.Fecha,
                    ClienteId = v.ClienteId,
                    Total = v.Total
                })
                .FirstOrDefaultAsync();

            if (venta == null)
            {
                return NotFound();
            }

            return venta;
        }

        [HttpPost]
        public async Task<ActionResult<Venta>> PostVenta([FromBody] Venta venta)
        {
            if (venta == null)
            {
                return BadRequest("El objeto venta no puede ser nulo");
            }

            // Validar campos requeridos
            if (!venta.ClienteId.HasValue)
            {
                return BadRequest("El campo clienteId es requerido");
            }

            // Verificar que el Cliente existe
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == venta.ClienteId);
            if (!clienteExiste)
            {
                return BadRequest($"El Cliente con ID {venta.ClienteId} no existe");
            }

            // Crear un nuevo objeto con solo los campos necesarios
            var nuevaVenta = new Venta
            {
                Fecha = venta.Fecha ?? DateOnly.FromDateTime(DateTime.Now), // Si no viene fecha, usar fecha actual
                ClienteId = venta.ClienteId,
                Total = venta.Total
                // No asignamos el Id, ya que será generado automáticamente
            };

            _context.Ventas.Add(nuevaVenta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVenta), new { id = nuevaVenta.Id }, nuevaVenta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenta(int id, [FromBody] Venta venta)
        {
            if (venta == null)
            {
                return BadRequest("El objeto venta no puede ser nulo");
            }

            if (id != venta.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del objeto");
            }

            // Verificar que el Cliente existe
            if (venta.ClienteId.HasValue)
            {
                var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == venta.ClienteId);
                if (!clienteExiste)
                {
                    return BadRequest($"El Cliente con ID {venta.ClienteId} no existe");
                }
            }

            // Obtener la venta existente
            var ventaExistente = await _context.Ventas.FindAsync(id);
            if (ventaExistente == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos necesarios
            ventaExistente.Fecha = venta.Fecha;
            ventaExistente.ClienteId = venta.ClienteId;
            ventaExistente.Total = venta.Total;

            _context.Entry(ventaExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VentaExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenta(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }
    }
}