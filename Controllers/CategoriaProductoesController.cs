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
    public class CategoriaProductoController : ControllerBase
    {
        private readonly NeondbContext _context;

        public CategoriaProductoController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/CategoriaProducto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaProducto>>> GetCategoriaProductos()
        {
            return await _context.CategoriaProductos.ToListAsync();
        }

        // POST: api/CategoriaProducto
        [HttpPost]
        public async Task<ActionResult<CategoriaProducto>> PostCategoriaProducto([FromBody] CategoriaProducto categoriaProducto)
        {
            _context.CategoriaProductos.Add(categoriaProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoriaProductoById), new { id = categoriaProducto.IdCategoriaProducto }, categoriaProducto);
        }

        // GET: api/CategoriaProducto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaProducto>> GetCategoriaProductoById(int id)
        {
            var categoria = await _context.CategoriaProductos.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }
    }
}
