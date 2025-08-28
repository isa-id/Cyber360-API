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
    public class ServiciosController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ServiciosController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicio>>> GetServicios()
        {
            try
            {
                return await _context.Servicios
                    .Include(s => s.Categoria)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // GET: api/servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servicio>> GetServicio(int id)
        {
            try
            {
                var servicio = await _context.Servicios
                    .Include(s => s.Categoria)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (servicio == null)
                {
                    return NotFound();
                }

                return servicio;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // POST: api/servicios
        [HttpPost]
        public async Task<ActionResult<Servicio>> CreateServicio([FromBody] Servicio servicio)
        {
            try
            {
                if (string.IsNullOrEmpty(servicio.Nombre))
                {
                    return BadRequest("Nombre es obligatorio");
                }

                if (servicio.Precio <= 0)
                {
                    return BadRequest("Precio debe ser mayor a 0");
                }

                _context.Servicios.Add(servicio);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetServicio), new { id = servicio.Id }, servicio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // PUT: api/servicios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServicio(int id, [FromBody] Servicio servicio)
        {
            try
            {
                if (id != servicio.Id)
                {
                    return BadRequest("ID no coincide");
                }

                if (string.IsNullOrEmpty(servicio.Nombre))
                {
                    return BadRequest("Nombre es obligatorio");
                }

                if (servicio.Precio <= 0)
                {
                    return BadRequest("Precio debe ser mayor a 0");
                }

                _context.Entry(servicio).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // DELETE: api/servicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            try
            {
                var servicio = await _context.Servicios.FindAsync(id);
                if (servicio == null)
                {
                    return NotFound();
                }

                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private bool ServicioExists(int id)
        {
            return _context.Servicios.Any(e => e.Id == id);
        }
    }
}