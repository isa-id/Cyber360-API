using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImagenesProductoesController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ImagenesProductoesController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/ImagenesProductoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImagenResponseDto>>> GetImagenesProductos()
        {
            return await _context.ImagenesProductos
                .Select(i => new ImagenResponseDto
                {
                    IdImagen = i.IdImagen,
                    Url = i.Url
                })
                .ToListAsync();
        }

        // GET: api/ImagenesProductoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImagenResponseDto>> GetImagenesProducto(int id)
        {
            var imagen = await _context.ImagenesProductos.FindAsync(id);

            if (imagen == null)
            {
                return NotFound();
            }

            return new ImagenResponseDto
            {
                IdImagen = imagen.IdImagen,
                Url = imagen.Url
            };
        }

        // POST: api/ImagenesProductoes
        [HttpPost]
        public async Task<ActionResult<ImagenResponseDto>> PostImagenesProducto([FromBody] ImagenCreateDto imagenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validación avanzada de URL de imagen
            if (!IsValidImageUrl(imagenDto.Url))
            {
                return BadRequest("La URL proporcionada no es una imagen válida");
            }

            var imagen = new ImagenesProducto
            {
                Url = imagenDto.Url
            };

            _context.ImagenesProductos.Add(imagen);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImagenesProducto),
                new { id = imagen.IdImagen },
                new ImagenResponseDto
                {
                    IdImagen = imagen.IdImagen,
                    Url = imagen.Url
                });
        }

        // PUT: api/ImagenesProductoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImagenesProducto(int id, [FromBody] ImagenUpdateDto imagenDto)
        {
            if (id != imagenDto.IdImagen)
            {
                return BadRequest();
            }

            var imagen = await _context.ImagenesProductos.FindAsync(id);
            if (imagen == null)
            {
                return NotFound();
            }

            if (!IsValidImageUrl(imagenDto.Url))
            {
                return BadRequest("La URL proporcionada no es una imagen válida");
            }

            imagen.Url = imagenDto.Url;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagenesProductoExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/ImagenesProductoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagenesProducto(int id)
        {
            var imagen = await _context.ImagenesProductos.FindAsync(id);
            if (imagen == null)
            {
                return NotFound();
            }

            _context.ImagenesProductos.Remove(imagen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImagenesProductoExists(int id)
        {
            return _context.ImagenesProductos.Any(e => e.IdImagen == id);
        }

        private bool IsValidImageUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
                return false;

            if (!(uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                return false;

            // Validar extensión de imagen
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = System.IO.Path.GetExtension(uriResult.AbsolutePath).ToLower();
            return validExtensions.Contains(fileExtension);
        }
    }

    // DTOs para mejor manejo de datos
    public class ImagenCreateDto
    {
        [Required(ErrorMessage = "La URL es obligatoria")]
        [Url(ErrorMessage = "Debe ser una URL válida")]
        public string Url { get; set; }
    }

    public class ImagenUpdateDto
    {
        public int IdImagen { get; set; }

        [Required(ErrorMessage = "La URL es obligatoria")]
        [Url(ErrorMessage = "Debe ser una URL válida")]
        public string Url { get; set; }
    }

    public class ImagenResponseDto
    {
        public int IdImagen { get; set; }
        public string Url { get; set; }
    }
}
