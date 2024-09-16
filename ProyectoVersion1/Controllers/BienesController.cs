using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoVersion1.Data;
using ProyectoVersion1.Models;

namespace ProyectoVersion1.Controllers
{
    public class BienesController : Controller
    {
        private readonly ProyectoVersion1Context _context;

        public BienesController(ProyectoVersion1Context context)
        {
            _context = context;
        }

        // GET: Bienes
        public async Task<IActionResult> Index()
        {
            var proyectoVersion1Context = _context.Bienes.Include(b => b.Categoria).Include(b => b.Espacio);
            return View(await proyectoVersion1Context.ToListAsync());
        }

        // GET: Bienes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bien = await _context.Bienes
                .Include(b => b.Categoria)
                .Include(b => b.Espacio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bien == null)
            {
                return NotFound();
            }

            return View(bien);
        }

        // GET: Bienes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewData["EspacioId"] = new SelectList(_context.Espacios, "Id", "Nombre");
            return View();
        }

        // POST: Bienes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Nombre,Descripcion,Precio,Estado,FechaIngreso,EspacioId,CategoriaId")] Bien bien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", bien.CategoriaId);
            ViewData["EspacioId"] = new SelectList(_context.Espacios, "Id", "Nombre", bien.EspacioId);
            return View(bien);
        }

        // GET: Bienes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bien = await _context.Bienes.FindAsync(id);
            if (bien == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", bien.CategoriaId);
            ViewData["EspacioId"] = new SelectList(_context.Espacios, "Id", "Id", bien.EspacioId);
            return View(bien);
        }

        // POST: Bienes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nombre,Descripcion,Precio,Estado,FechaIngreso,EspacioId,CategoriaId")] Bien bien)
        {
            if (id != bien.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BienExists(bien.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id", bien.CategoriaId);
            ViewData["EspacioId"] = new SelectList(_context.Espacios, "Id", "Nombre", bien.EspacioId);
            return View(bien);
        }

        // GET: Bienes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bien = await _context.Bienes
                .Include(b => b.Categoria)
                .Include(b => b.Espacio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bien == null)
            {
                return NotFound();
            }

            return View(bien);
        }

        // POST: Bienes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bien = await _context.Bienes.FindAsync(id);
            if (bien != null)
            {
                _context.Bienes.Remove(bien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BienExists(int id)
        {
            return _context.Bienes.Any(e => e.Id == id);
        }
    }
}
