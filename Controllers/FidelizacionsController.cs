using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FidelizacionsController : ControllerBase
    {
        private readonly NeondbContext _context;

        public FidelizacionsController(NeondbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fidelizacion>>> GetFidelizacions()
        {
            return await _context.Fidelizacions
                .Select(f => new Fidelizacion
                {
                    Id = f.Id,
                    Horas = f.Horas,
                    TipoFicho = f.TipoFicho,
                    Fichos = f.Fichos,
                    FichosNa = f.FichosNa,
                    Estado = f.Estado,
                    ClienteId = f.ClienteId
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fidelizacion>> GetFidelizacion(int id)
        {
            var fidelizacion = await _context.Fidelizacions
                .Where(f => f.Id == id)
                .Select(f => new Fidelizacion
                {
                    Id = f.Id,
                    Horas = f.Horas,
                    TipoFicho = f.TipoFicho,
                    Fichos = f.Fichos,
                    FichosNa = f.FichosNa,
                    Estado = f.Estado,
                    ClienteId = f.ClienteId
                })
                .FirstOrDefaultAsync();

            if (fidelizacion == null)
            {
                return NotFound();
            }

            return fidelizacion;
        }

        [HttpPost]
        public async Task<ActionResult<Fidelizacion>> PostFidelizacion([FromBody] Fidelizacion fidelizacion)
        {
            if (fidelizacion == null)
            {
                return BadRequest("El objeto fidelizacion no puede ser nulo");
            }

            if (!fidelizacion.ClienteId.HasValue)
            {
                return BadRequest("El campo clienteId es requerido");
            }

            var nuevaFidelizacion = new Fidelizacion
            {
                Horas = fidelizacion.Horas,
                TipoFicho = fidelizacion.TipoFicho,
                Fichos = fidelizacion.Fichos,
                FichosNa = fidelizacion.FichosNa,
                Estado = fidelizacion.Estado,
                ClienteId = fidelizacion.ClienteId
            };

            _context.Fidelizacions.Add(nuevaFidelizacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFidelizacion), new { id = nuevaFidelizacion.Id }, nuevaFidelizacion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFidelizacion(int id, [FromBody] Fidelizacion fidelizacion)
        {
            if (fidelizacion == null)
            {
                return BadRequest("El objeto fidelizacion no puede ser nulo");
            }

            if (id != fidelizacion.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del objeto");
            }

            var fidelizacionExistente = await _context.Fidelizacions.FindAsync(id);
            if (fidelizacionExistente == null)
            {
                return NotFound();
            }

            fidelizacionExistente.Horas = fidelizacion.Horas;
            fidelizacionExistente.TipoFicho = fidelizacion.TipoFicho;
            fidelizacionExistente.Fichos = fidelizacion.Fichos;
            fidelizacionExistente.FichosNa = fidelizacion.FichosNa;
            fidelizacionExistente.Estado = fidelizacion.Estado;
            fidelizacionExistente.ClienteId = fidelizacion.ClienteId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FidelizacionExists(id))
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
        public async Task<IActionResult> DeleteFidelizacion(int id)
        {
            var fidelizacion = await _context.Fidelizacions.FindAsync(id);
            if (fidelizacion == null)
            {
                return NotFound();
            }

            _context.Fidelizacions.Remove(fidelizacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FidelizacionExists(int id)
        {
            return _context.Fidelizacions.Any(e => e.Id == id);
        }
    }
}