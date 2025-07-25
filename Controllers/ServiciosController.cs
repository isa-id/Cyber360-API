using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ServiciosController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/Servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicio>>> GetServicios()
        {
            return await _context.Servicios
                .Include(s => s.FkCategoriaServicioNavigation)
                .Include(s => s.FkEquipoNavigation)
                .Include(s => s.FkImagenNavigation)
                .ToListAsync();
        }

        // GET: api/Servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servicio>> GetServicio(int id)
        {
            var servicio = await _context.Servicios
                .Include(s => s.FkCategoriaServicioNavigation)
                .Include(s => s.FkEquipoNavigation)
                .Include(s => s.FkImagenNavigation)
                .FirstOrDefaultAsync(s => s.IdServicio == id);

            if (servicio == null)
            {
                return NotFound();
            }

            return servicio;
        }

        // POST: api/Servicios
        [HttpPost]
        public async Task<ActionResult> PostServicio([FromBody] ServicioCreateDto dto)
        {
            var servicio = new Servicio 
            {
                NombreServicio = dto.NombreServicio,
                Precio = dto.Precio,
                Detalles = dto.Detalles,
                FkCategoriaServicio = dto.FkCategoriaServicio,
                FkImagen = dto.FkImagen,
                FkEquipo = dto.FkEquipo // Esto ahora puede ser null
            };

            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServicio), new { id = servicio.IdServicio }, servicio);
        }

        public class ServicioCreateDto
        {
            public string NombreServicio { get; set; }
            public decimal Precio { get; set; }
            public string Detalles { get; set; }
            public int FkCategoriaServicio { get; set; }
            public int FkImagen { get; set; }
            public int? FkEquipo { get; set; } // Cambiado a nullable
        }

        // PUT: api/Servicios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicio(int id, [FromBody] ServicioUpdateDto dto)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }

            servicio.NombreServicio = dto.NombreServicio;
            servicio.Precio = dto.Precio;
            servicio.Detalles = dto.Detalles;
            servicio.FkCategoriaServicio = dto.FkCategoriaServicio;
            servicio.FkImagen = dto.FkImagen;
            servicio.FkEquipo = dto.FkEquipo; // Esto ahora puede ser null

            try
            {
                await _context.SaveChangesAsync();
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

            return NoContent();
        }

        public class ServicioUpdateDto
        {
            public string NombreServicio { get; set; }
            public decimal Precio { get; set; }
            public string Detalles { get; set; }
            public int FkCategoriaServicio { get; set; }
            public int FkImagen { get; set; }
            public int? FkEquipo { get; set; } // Cambiado a nullable
        }

        // DELETE: api/Servicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
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

        private bool ServicioExists(int id)
        {
            return _context.Servicios.Any(e => e.IdServicio == id);
        }
    }
}
