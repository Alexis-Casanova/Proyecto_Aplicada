using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoVersion1.Data;
using ProyectoVersion1.Models;

namespace ProyectoVersion1.Controllers
{
    [Authorize(Policy = "root")]

    public class EncargosController : Controller
    {
        private readonly ProyectoVersion1Context _context;

        public EncargosController(ProyectoVersion1Context context)
        {
            _context = context;
        }

        // GET: Encargos
        public async Task<IActionResult> Index()
        {
            var proyectoVersion1Context = _context.Encargos.Include(e => e.Bien).Include(e => e.Trabajador);
            return View(await proyectoVersion1Context.ToListAsync());
        }

        // GET: Encargos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encargo = await _context.Encargos
                .Include(e => e.Bien)
                .Include(e => e.Trabajador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (encargo == null)
            {
                return NotFound();
            }

            return View(encargo);
        }

        // GET: Encargos/Create
        [Authorize(Policy = "root")]

        public IActionResult Create()
        {
            ViewData["BienId"] = new SelectList(_context.Bienes, "Id", "Nombre");
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadores, "Id", "Nombre");
            return View();
        }

        // POST: Encargos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "root")]

        public async Task<IActionResult> Create([Bind("Id,TrabajadorId,BienId,FechaInicio,FechaFin")] Encargo encargo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(encargo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BienId"] = new SelectList(_context.Bienes, "Id", "Nombre", encargo.BienId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadores, "Id", "Nombre", encargo.TrabajadorId);
            return View(encargo);
        }

        // GET: Encargos/Edit/5
        [Authorize(Policy = "root")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encargo = await _context.Encargos.FindAsync(id);
            if (encargo == null)
            {
                return NotFound();
            }
            ViewData["BienId"] = new SelectList(_context.Bienes, "Id", "Nombre", encargo.BienId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadores, "Id", "Nombre", encargo.TrabajadorId);
            return View(encargo);
        }
        [Authorize(Policy = "root")]


        // POST: Encargos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TrabajadorId,BienId,FechaInicio,FechaFin")] Encargo encargo)
        {
            if (id != encargo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encargo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EncargoExists(encargo.Id))
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
            ViewData["BienId"] = new SelectList(_context.Bienes, "Id", "Nombre", encargo.BienId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadores, "Id", "Nombre", encargo.TrabajadorId);
            return View(encargo);
        }

        // GET: Encargos/Delete/5
        [Authorize(Policy = "root")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encargo = await _context.Encargos
                .Include(e => e.Bien)
                .Include(e => e.Trabajador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (encargo == null)
            {
                return NotFound();
            }

            return View(encargo);
        }

        // POST: Encargos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "root")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var encargo = await _context.Encargos.FindAsync(id);
            if (encargo != null)
            {
                _context.Encargos.Remove(encargo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EncargoExists(int id)
        {
            return _context.Encargos.Any(e => e.Id == id);
        }
    }
}
