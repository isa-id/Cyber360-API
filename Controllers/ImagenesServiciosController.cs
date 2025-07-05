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
    public class ImagenesServiciosController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ImagenesServiciosController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/ImagenesServicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImagenServicioResponseDto>>> GetImagenesServicios()
        {
            return await _context.ImagenesServicios
                .Select(i => new ImagenServicioResponseDto
                {
                    IdImagen = i.IdImagen,
                    Url = i.Url
                })
                .ToListAsync();
        }

        // GET: api/ImagenesServicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImagenServicioResponseDto>> GetImagenServicio(int id)
        {
            var imagen = await _context.ImagenesServicios.FindAsync(id);

            if (imagen == null)
            {
                return NotFound();
            }

            return new ImagenServicioResponseDto
            {
                IdImagen = imagen.IdImagen,
                Url = imagen.Url
            };
        }

        // POST: api/ImagenesServicios
        [HttpPost]
        public async Task<ActionResult<ImagenServicioResponseDto>> PostImagenServicio([FromBody] ImagenServicioCreateDto imagenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsValidImageUrl(imagenDto.Url))
            {
                return BadRequest("La URL proporcionada no es una imagen válida");
            }

            var imagen = new ImagenesServicio
            {
                Url = imagenDto.Url
            };

            _context.ImagenesServicios.Add(imagen);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImagenServicio),
                new { id = imagen.IdImagen },
                new ImagenServicioResponseDto
                {
                    IdImagen = imagen.IdImagen,
                    Url = imagen.Url
                });
        }

        // PUT: api/ImagenesServicios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImagenServicio(int id, [FromBody] ImagenServicioUpdateDto imagenDto)
        {
            if (id != imagenDto.IdImagen)
            {
                return BadRequest();
            }

            var imagen = await _context.ImagenesServicios.FindAsync(id);
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
                if (!ImagenServicioExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/ImagenesServicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagenServicio(int id)
        {
            var imagen = await _context.ImagenesServicios.FindAsync(id);
            if (imagen == null)
            {
                return NotFound();
            }

            _context.ImagenesServicios.Remove(imagen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImagenServicioExists(int id)
        {
            return _context.ImagenesServicios.Any(e => e.IdImagen == id);
        }

        private bool IsValidImageUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
                return false;

            if (!(uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                return false;

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = System.IO.Path.GetExtension(uriResult.AbsolutePath).ToLower();
            return validExtensions.Contains(fileExtension);
        }
    }

    // DTOs para ImagenesServicio
    public class ImagenServicioCreateDto
    {
        [Required(ErrorMessage = "La URL es obligatoria")]
        [Url(ErrorMessage = "Debe ser una URL válida")]
        public string Url { get; set; }
    }

    public class ImagenServicioUpdateDto
    {
        public int IdImagen { get; set; }

        [Required(ErrorMessage = "La URL es obligatoria")]
        [Url(ErrorMessage = "Debe ser una URL válida")]
        public string Url { get; set; }
    }

    public class ImagenServicioResponseDto
    {
        public int IdImagen { get; set; }
        public string Url { get; set; }
    }
}
