using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers.Ventas
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReparacionesController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ReparacionesController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/reparaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reparacion>>> GetReparaciones()
        {
            try
            {
                return await _context.Reparacions
                    .Where(r => r.Estado == true)
                    .Include(r => r.Cliente)
                    .Include(r => r.Abonos)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/reparaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reparacion>> GetReparacion(int id)
        {
            try
            {
                var reparacion = await _context.Reparacions
                    .Include(r => r.Cliente)
                    .Include(r => r.Abonos)
                    .FirstOrDefaultAsync(r => r.Id == id && r.Estado == true);

                if (reparacion == null)
                {
                    return NotFound("Reparación no encontrada o inactiva");
                }

                return reparacion;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/reparaciones
        [HttpPost]
        public async Task<ActionResult<Reparacion>> PostReparacion([FromBody] Reparacion reparacion)
        {
            try
            {
                // Validaciones
                if (reparacion.ClienteId == null || reparacion.ClienteId <= 0)
                {
                    return BadRequest("ClienteId es obligatorio");
                }

                if (string.IsNullOrEmpty(reparacion.DetallesDano))
                {
                    return BadRequest("Detalles del daño son obligatorios");
                }

                // Validar que el cliente exista
                var clienteExists = await _context.Clientes.AnyAsync(c => c.Id == reparacion.ClienteId && c.Estado == true);
                if (!clienteExists)
                {
                    return BadRequest("El cliente especificado no existe o está inactivo");
                }

                // Asignar valores por defecto
                reparacion.Estado ??= true;
                reparacion.Fecha ??= DateOnly.FromDateTime(DateTime.Now);
                reparacion.Prioridad ??= false;
                reparacion.TipoReparacion ??= false;
                reparacion.Valor ??= 0;

                _context.Reparacions.Add(reparacion);
                await _context.SaveChangesAsync();

                // Cargar relaciones para la respuesta
                await _context.Entry(reparacion)
                    .Reference(r => r.Cliente)
                    .LoadAsync();

                await _context.Entry(reparacion)
                    .Collection(r => r.Abonos)
                    .LoadAsync();

                return CreatedAtAction(nameof(GetReparacion), new { id = reparacion.Id }, reparacion);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al crear la reparación: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/reparaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReparacion(int id, [FromBody] Reparacion reparacion)
        {
            try
            {
                if (id != reparacion.Id)
                {
                    return BadRequest("ID de la reparación no coincide");
                }

                // Validar que la reparación exista y esté activa
                var existingReparacion = await _context.Reparacions
                    .FirstOrDefaultAsync(r => r.Id == id && r.Estado == true);

                if (existingReparacion == null)
                {
                    return NotFound("Reparación no encontrada o inactiva");
                }

                // Validar cliente si se está actualizando
                if (reparacion.ClienteId != null && reparacion.ClienteId != existingReparacion.ClienteId)
                {
                    var clienteExists = await _context.Clientes.AnyAsync(c => c.Id == reparacion.ClienteId && c.Estado == true);
                    if (!clienteExists)
                    {
                        return BadRequest("El nuevo cliente especificado no existe o está inactivo");
                    }
                }

                _context.Entry(reparacion).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReparacionExists(id))
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/reparaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReparacion(int id)
        {
            try
            {
                var reparacion = await _context.Reparacions.FindAsync(id);
                if (reparacion == null)
                {
                    return NotFound();
                }

                // Soft delete en lugar de eliminación física
                reparacion.Estado = false;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/reparaciones/cliente/5
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Reparacion>>> GetReparacionesByCliente(int clienteId)
        {
            try
            {
                var reparaciones = await _context.Reparacions
                    .Where(r => r.ClienteId == clienteId && r.Estado == true)
                    .Include(r => r.Cliente)
                    .Include(r => r.Abonos)
                    .ToListAsync();

                return reparaciones;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        private bool ReparacionExists(int id)
        {
            return _context.Reparacions.Any(e => e.Id == id);
        }
    }
}