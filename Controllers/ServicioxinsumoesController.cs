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
    public class ServicioxinsumoesController : Controller
    {
        private readonly NeondbContext _context;

        public ServicioxinsumoesController(NeondbContext context)
        {
            _context = context;
        }

        // GET: Servicioxinsumoes
        public async Task<IActionResult> Index()
        {
            var neondbContext = _context.Servicioxinsumos.Include(s => s.FkProductoNavigation).Include(s => s.FkServicioNavigation);
            return View(await neondbContext.ToListAsync());
        }

        // GET: Servicioxinsumoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicioxinsumo = await _context.Servicioxinsumos
                .Include(s => s.FkProductoNavigation)
                .Include(s => s.FkServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdServicioxinsumo == id);
            if (servicioxinsumo == null)
            {
                return NotFound();
            }

            return View(servicioxinsumo);
        }

        // GET: Servicioxinsumoes/Create
        public IActionResult Create()
        {
            ViewData["FkProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto");
            ViewData["FkServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio");
            return View();
        }

        // POST: Servicioxinsumoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdServicioxinsumo,FkProducto,FkServicio")] Servicioxinsumo servicioxinsumo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servicioxinsumo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", servicioxinsumo.FkProducto);
            ViewData["FkServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", servicioxinsumo.FkServicio);
            return View(servicioxinsumo);
        }

        // GET: Servicioxinsumoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicioxinsumo = await _context.Servicioxinsumos.FindAsync(id);
            if (servicioxinsumo == null)
            {
                return NotFound();
            }
            ViewData["FkProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", servicioxinsumo.FkProducto);
            ViewData["FkServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", servicioxinsumo.FkServicio);
            return View(servicioxinsumo);
        }

        // POST: Servicioxinsumoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdServicioxinsumo,FkProducto,FkServicio")] Servicioxinsumo servicioxinsumo)
        {
            if (id != servicioxinsumo.IdServicioxinsumo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicioxinsumo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicioxinsumoExists(servicioxinsumo.IdServicioxinsumo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", servicioxinsumo.FkProducto);
            ViewData["FkServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", servicioxinsumo.FkServicio);
            return View(servicioxinsumo);
        }

        // GET: Servicioxinsumoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicioxinsumo = await _context.Servicioxinsumos
                .Include(s => s.FkProductoNavigation)
                .Include(s => s.FkServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdServicioxinsumo == id);
            if (servicioxinsumo == null)
            {
                return NotFound();
            }

            return View(servicioxinsumo);
        }

        // POST: Servicioxinsumoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicioxinsumo = await _context.Servicioxinsumos.FindAsync(id);
            if (servicioxinsumo != null)
            {
                _context.Servicioxinsumos.Remove(servicioxinsumo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicioxinsumoExists(int id)
        {
            return _context.Servicioxinsumos.Any(e => e.IdServicioxinsumo == id);
        }
    }
}
