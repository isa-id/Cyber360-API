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
    public class CategoriaServicioController : ControllerBase
    {
        private readonly NeondbContext _context;

        public CategoriaServicioController(NeondbContext context)
        {
            _context = context;
        }

        // GET: CategoriaServicio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaServicio>>> GetCategoriaServicios()
        {
            return await _context.CategoriaServicios.ToListAsync();
        }

        // GET: CategoriaServicio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaServicio>> GetCategoriaServicio(int id)
        {
            var categoriaServicio = await _context.CategoriaServicios.FindAsync(id);

            if (categoriaServicio == null)
            {
                return NotFound();
            }

            return categoriaServicio;
        }

        // POST: CategoriaServicio
        [HttpPost]
        public async Task<ActionResult<CategoriaServicio>> PostCategoriaServicio(CategoriaServicio categoriaServicio)
        {
            _context.CategoriaServicios.Add(categoriaServicio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoriaServicio", new { id = categoriaServicio.IdCategoriaServicio }, categoriaServicio);
        }
    }
}
