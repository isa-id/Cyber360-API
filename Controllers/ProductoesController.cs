using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ProductoController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/Producto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos
                .Include(p => p.FkCategoriaNavigation)
                .Include(p => p.FkImagenNavigation)
                .ToListAsync();
        }

        // GET: api/Producto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.FkCategoriaNavigation)
                .Include(p => p.FkImagenNavigation)
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        // POST: api/Producto
       [HttpPost]
public async Task<ActionResult> PostProducto([FromBody] ProductoCreateDto dto)
{
    var producto = new Producto {
        Nombre = dto.Nombre,
        Cantidad = dto.Cantidad,
        Precio = dto.Precio,
        FechaCreacion = DateOnly.FromDateTime(DateTime.Now),
        FkImagen = dto.FkImagen, // Asignamos solo el ID
        FkCategoria = dto.FkCategoria // Asignamos solo el ID
    };

    _context.Productos.Add(producto);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetProducto), new { id = producto.IdProducto }, producto);
}

public class ProductoCreateDto
{
    
    public string Nombre { get; set; }
    
    public int Cantidad { get; set; }
    
    
    public decimal Precio { get; set; }
    
    
    public int FkImagen { get; set; }
    
   
    public int FkCategoria { get; set; }
}
        // PUT: api/Producto/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
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

        // DELETE: api/Producto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
        }
    }
}
