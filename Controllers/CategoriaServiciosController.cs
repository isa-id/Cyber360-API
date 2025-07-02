using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: api/CategoriaServicio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaServicio>>> GetCategoriaServicios()
        {
            return await _context.CategoriaServicios.ToListAsync();
        }
    }

}
