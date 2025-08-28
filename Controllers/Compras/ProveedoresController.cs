using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models.Compras;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers.Compras
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly NeondbContext _context;

        public ProveedoresController(NeondbContext context)
        {
            _context = context;
        }

        // GET: api/proveedores
        [HttpGet]
        public async Task<IActionResult> GetProveedores()
        {
            var proveedores = await _context.Proveedores
                .AsNoTracking()
                .Select(p => new
                {
                    id = p.Id,
                    tipoPersona = p.TipoPersona,
                    tipoDocumento = p.TipoDocumento,
                    numeroDocumento = p.NumeroDocumento,
                    nombres = p.Nombres,
                    apellidos = p.Apellidos,
                    razonSocial = p.RazonSocial,
                    correo = p.Correo,
                    telefono = p.Telefono,
                    direccion = p.Direccion
                })
                .ToListAsync();

            return Ok(proveedores);
        }

        // GET: api/proveedores/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProveedore(int id)
        {
            var proveedore = await _context.Proveedores
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    id = p.Id,
                    tipoPersona = p.TipoPersona,
                    tipoDocumento = p.TipoDocumento,
                    numeroDocumento = p.NumeroDocumento,
                    nombres = p.Nombres,
                    apellidos = p.Apellidos,
                    razonSocial = p.RazonSocial,
                    correo = p.Correo,
                    telefono = p.Telefono,
                    direccion = p.Direccion
                })
                .FirstOrDefaultAsync();

            if (proveedore == null)
            {
                return NotFound();
            }

            return Ok(proveedore);
        }

        // POST: api/proveedores
        [HttpPost]
        public async Task<IActionResult> PostProveedore([FromBody] Proveedore proveedore)
        {
            if (proveedore == null)
            {
                return BadRequest("Los datos del proveedor son obligatorios.");
            }

            // Validaciones básicas
            if (string.IsNullOrEmpty(proveedore.Nombres) && string.IsNullOrEmpty(proveedore.RazonSocial))
            {
                return BadRequest("Debe proporcionar nombres o razón social.");
            }

            if (string.IsNullOrEmpty(proveedore.NumeroDocumento))
            {
                return BadRequest("El número de documento es obligatorio.");
            }

            if (string.IsNullOrEmpty(proveedore.TipoDocumento))
            {
                return BadRequest("El tipo de documento es obligatorio.");
            }

            // Validar que no exista otro proveedor con el mismo número de documento
            var documentoExiste = await _context.Proveedores
                .AnyAsync(p => p.NumeroDocumento == proveedore.NumeroDocumento);

            if (documentoExiste)
            {
                return Conflict("Ya existe un proveedor con este número de documento.");
            }

            _context.Proveedores.Add(proveedore);
            await _context.SaveChangesAsync();

            // Devolver representación simplificada
            var result = new
            {
                id = proveedore.Id,
                tipoPersona = proveedore.TipoPersona,
                tipoDocumento = proveedore.TipoDocumento,
                numeroDocumento = proveedore.NumeroDocumento,
                nombres = proveedore.Nombres,
                apellidos = proveedore.Apellidos,
                razonSocial = proveedore.RazonSocial,
                correo = proveedore.Correo,
                telefono = proveedore.Telefono,
                direccion = proveedore.Direccion
            };

            return CreatedAtAction(nameof(GetProveedore), new { id = proveedore.Id }, result);
        }

        // PUT: api/proveedores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProveedore(int id, [FromBody] Proveedore proveedore)
        {
            if (id != proveedore.Id)
            {
                return BadRequest("El ID del proveedor no coincide.");
            }

            // Validaciones básicas
            if (string.IsNullOrEmpty(proveedore.Nombres) && string.IsNullOrEmpty(proveedore.RazonSocial))
            {
                return BadRequest("Debe proporcionar nombres o razón social.");
            }

            if (string.IsNullOrEmpty(proveedore.NumeroDocumento))
            {
                return BadRequest("El número de documento es obligatorio.");
            }

            if (string.IsNullOrEmpty(proveedore.TipoDocumento))
            {
                return BadRequest("El tipo de documento es obligatorio.");
            }

            // Validar que no exista otro proveedor con el mismo número de documento (excluyendo el actual)
            var documentoExiste = await _context.Proveedores
                .AnyAsync(p => p.NumeroDocumento == proveedore.NumeroDocumento && p.Id != proveedore.Id);

            if (documentoExiste)
            {
                return Conflict("Ya existe otro proveedor con este número de documento.");
            }

            _context.Entry(proveedore).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProveedoreExists(id))
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

        // DELETE: api/proveedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedore(int id)
        {
            var proveedore = await _context.Proveedores.FindAsync(id);
            if (proveedore == null)
            {
                return NotFound();
            }

            // Validar si el proveedor tiene compras asociadas
            var tieneCompras = await _context.Compras.AnyAsync(c => c.ProveedorId == id);
            if (tieneCompras)
            {
                return BadRequest("No se puede eliminar el proveedor porque tiene compras asociadas.");
            }

            _context.Proveedores.Remove(proveedore);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/proveedores/documento/12345678
        [HttpGet("documento/{numeroDocumento}")]
        public async Task<IActionResult> GetProveedoreByDocumento(string numeroDocumento)
        {
            var proveedore = await _context.Proveedores
                .AsNoTracking()
                .Where(p => p.NumeroDocumento == numeroDocumento)
                .Select(p => new
                {
                    id = p.Id,
                    tipoPersona = p.TipoPersona,
                    tipoDocumento = p.TipoDocumento,
                    numeroDocumento = p.NumeroDocumento,
                    nombres = p.Nombres,
                    apellidos = p.Apellidos,
                    razonSocial = p.RazonSocial,
                    correo = p.Correo,
                    telefono = p.Telefono,
                    direccion = p.Direccion
                })
                .FirstOrDefaultAsync();

            if (proveedore == null)
            {
                return NotFound();
            }

            return Ok(proveedore);
        }

        // GET: api/proveedores/buscar/{termino}
        [HttpGet("buscar/{termino}")]
        public async Task<IActionResult> SearchProveedores(string termino)
        {
            var proveedores = await _context.Proveedores
                .AsNoTracking()
                .Where(p => p.Nombres.Contains(termino) ||
                           p.Apellidos.Contains(termino) ||
                           p.RazonSocial.Contains(termino) ||
                           p.NumeroDocumento.Contains(termino))
                .Select(p => new
                {
                    id = p.Id,
                    tipoPersona = p.TipoPersona,
                    tipoDocumento = p.TipoDocumento,
                    numeroDocumento = p.NumeroDocumento,
                    nombres = p.Nombres,
                    apellidos = p.Apellidos,
                    razonSocial = p.RazonSocial,
                    correo = p.Correo,
                    telefono = p.Telefono,
                    direccion = p.Direccion
                })
                .ToListAsync();

            return Ok(proveedores);
        }

        private bool ProveedoreExists(int id)
        {
            return _context.Proveedores.Any(e => e.Id == id);
        }
    }
}