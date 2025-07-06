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
    public class EquiposController : ControllerBase
    {
        private readonly NeondbContext _context;

        public EquiposController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/Equipos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipos()
        {
            return await _context.Equipos.ToListAsync();
        }

        // GET: api/Equipos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipo>> GetEquipo(int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);

            if (equipo == null)
            {
                return NotFound();
            }

            return equipo;
        }

        // PUT: api/Equipos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipo(int id, Equipo equipo)
        {
            if (id != equipo.IdEquipo)
            {
                return BadRequest();
            }

            _context.Entry(equipo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipoExists(id))
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

        // POST: api/Equipos
        [HttpPost]
        public async Task<ActionResult<Equipo>> PostEquipo(Equipo equipo)
        {
            _context.Equipos.Add(equipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquipo", new { id = equipo.IdEquipo }, equipo);
        }

        // DELETE: api/Equipos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipo(int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return NotFound();
            }

            _context.Equipos.Remove(equipo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipoExists(int id)
        {
            return _context.Equipos.Any(e => e.IdEquipo == id);
        }
    }
}
