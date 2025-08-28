using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly NeondbContext _context;

        public CategoriasController(NeondbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria([FromBody] Categoria categoria)
        {
            // Crear nueva instancia solo con los campos necesarios
            var nuevaCategoria = new Categoria
            {
                Id = 0, // Explicitly set to 0 to let database generate the ID
                TipoCategoria = categoria.TipoCategoria,
                NombreCategoria = categoria.NombreCategoria,
                Descripcion = categoria.Descripcion
            };

            _context.Categorias.Add(nuevaCategoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoria), new { id = nuevaCategoria.Id }, nuevaCategoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            var categoriaExistente = await _context.Categorias.FindAsync(id);
            if (categoriaExistente == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos necesarios
            categoriaExistente.TipoCategoria = categoria.TipoCategoria;
            categoriaExistente.NombreCategoria = categoria.NombreCategoria;
            categoriaExistente.Descripcion = categoria.Descripcion;

            _context.Entry(categoriaExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
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
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}