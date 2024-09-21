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

    public class EspaciosController : Controller
    {
        private readonly ProyectoVersion1Context _context;

        public EspaciosController(ProyectoVersion1Context context)
        {
            _context = context;
        }

        // GET: Espacios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Espacios.ToListAsync());
        }

        // GET: Espacios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var espacio = await _context.Espacios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (espacio == null)
            {
                return NotFound();
            }

            return View(espacio);
        }

        public List<string> Tipos = new List<string>() { "Aula", "Laboratorio", "Taller", "Oficina" };
        // GET: Espacios/Create
        public IActionResult Create()
        {
            ViewData["Tipo"] = new SelectList(Tipos);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Tipo,Ubicacion")] Espacio espacio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(espacio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Tipo"] = new SelectList(Tipos);
            return View(espacio);
        }

        // GET: Espacios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var espacio = await _context.Espacios.FindAsync(id);
            if (espacio == null)
            {
                return NotFound();
            }
            ViewData["Tipo"] = new SelectList(Tipos);
            return View(espacio);
        }

        // POST: Espacios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Tipo,Ubicacion")] Espacio espacio)
        {
            if (id != espacio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(espacio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspacioExists(espacio.Id))
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
            ViewData["Tipo"] = new SelectList(Tipos);
            return View(espacio);
        }

        // GET: Espacios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var espacio = await _context.Espacios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (espacio == null)
            {
                return NotFound();
            }

            return View(espacio);
        }

        // POST: Espacios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var espacio = await _context.Espacios.FindAsync(id);
            if (espacio != null)
            {
                _context.Espacios.Remove(espacio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EspacioExists(int id)
        {
            return _context.Espacios.Any(e => e.Id == id);
        }
    }
}
