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
    public class AbonoesController : ControllerBase
    {
        private readonly NeondbContext _context;

        public AbonoesController(NeondbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Abono>>> GetAbonos()
        {
            return await _context.Abonos
                .Select(a => new Abono
                {
                    Id = a.Id,
                    NumAbono = a.NumAbono,
                    FechaAbono = a.FechaAbono,
                    PrecioPagar = a.PrecioPagar,
                    Debe = a.Debe,
                    ListaTotalAbonos = a.ListaTotalAbonos,
                    ReparacionId = a.ReparacionId
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Abono>> GetAbono(int id)
        {
            var abono = await _context.Abonos
                .Where(a => a.Id == id)
                .Select(a => new Abono
                {
                    Id = a.Id,
                    NumAbono = a.NumAbono,
                    FechaAbono = a.FechaAbono,
                    PrecioPagar = a.PrecioPagar,
                    Debe = a.Debe,
                    ListaTotalAbonos = a.ListaTotalAbonos,
                    ReparacionId = a.ReparacionId
                })
                .FirstOrDefaultAsync();

            if (abono == null)
            {
                return NotFound();
            }

            return abono;
        }

        [HttpPost]
        public async Task<ActionResult<Abono>> PostAbono([FromBody] Abono abono)
        {
            if (abono == null)
            {
                return BadRequest("El objeto abono no puede ser nulo");
            }

            if (!abono.ReparacionId.HasValue)
            {
                return BadRequest("El campo reparacionId es requerido");
            }

            var nuevoAbono = new Abono
            {
                NumAbono = abono.NumAbono,
                FechaAbono = abono.FechaAbono,
                PrecioPagar = abono.PrecioPagar,
                Debe = abono.Debe,
                ListaTotalAbonos = abono.ListaTotalAbonos,
                ReparacionId = abono.ReparacionId
            };

            _context.Abonos.Add(nuevoAbono);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAbono), new { id = nuevoAbono.Id }, nuevoAbono);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbono(int id, [FromBody] Abono abono)
        {
            if (abono == null)
            {
                return BadRequest("El objeto abono no puede ser nulo");
            }

            if (id != abono.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del objeto");
            }

            var abonoExistente = await _context.Abonos.FindAsync(id);
            if (abonoExistente == null)
            {
                return NotFound();
            }

            abonoExistente.NumAbono = abono.NumAbono;
            abonoExistente.FechaAbono = abono.FechaAbono;
            abonoExistente.PrecioPagar = abono.PrecioPagar;
            abonoExistente.Debe = abono.Debe;
            abonoExistente.ListaTotalAbonos = abono.ListaTotalAbonos;
            abonoExistente.ReparacionId = abono.ReparacionId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbonoExists(id))
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
        public async Task<IActionResult> DeleteAbono(int id)
        {
            var abono = await _context.Abonos.FindAsync(id);
            if (abono == null)
            {
                return NotFound();
            }

            _context.Abonos.Remove(abono);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AbonoExists(int id)
        {
            return _context.Abonos.Any(e => e.Id == id);
        }
    }
}