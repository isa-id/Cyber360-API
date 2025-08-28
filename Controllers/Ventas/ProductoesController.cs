using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models.Ventas;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers.Ventas
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ProductosController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos
                .Select(p => new Producto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Cantidad = p.Cantidad,
                    Precio = p.Precio,
                    FechaCreacion = p.FechaCreacion,
                    CategoriaId = p.CategoriaId
                })
                .ToListAsync();
        }

        // GET: api/productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Where(p => p.Id == id)
                .Select(p => new Producto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Cantidad = p.Cantidad,
                    Precio = p.Precio,
                    FechaCreacion = p.FechaCreacion,
                    CategoriaId = p.CategoriaId
                })
                .FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        // POST: api/productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto([FromBody] Producto producto)
        {
            // Crear un nuevo objeto con solo los campos necesarios
            var nuevoProducto = new Producto
            {
                Nombre = producto.Nombre,
                Cantidad = producto.Cantidad,
                Precio = producto.Precio,
                FechaCreacion = producto.FechaCreacion,
                CategoriaId = producto.CategoriaId
            };

            _context.Productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = nuevoProducto.Id }, nuevoProducto);
        }

        // PUT: api/productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, [FromBody] Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            // Obtener el producto existente
            var productoExistente = await _context.Productos.FindAsync(id);
            if (productoExistente == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos necesarios
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Cantidad = producto.Cantidad;
            productoExistente.Precio = producto.Precio;
            productoExistente.FechaCreacion = producto.FechaCreacion;
            productoExistente.CategoriaId = producto.CategoriaId;

            _context.Entry(productoExistente).State = EntityState.Modified;

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

        // DELETE: api/productos/5
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
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}