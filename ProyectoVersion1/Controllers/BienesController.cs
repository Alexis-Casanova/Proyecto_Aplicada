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
using X.PagedList;

namespace ProyectoVersion1.Controllers
{
    [Authorize(Policy ="root")]
    public class BienesController : Controller
    {
        private readonly ProyectoVersion1Context _context;
        private readonly IConfiguration _configuration;

        public BienesController(ProyectoVersion1Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Bienes / Reciben las consultas
        [BindProperty(SupportsGet =true)]
        public int? Pagina { get; set; }
        public async Task<IActionResult> Index()
        {
            if(_context.Bienes != null)
            {
                var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 10);
                var consulta = _context.Bienes.Include(b => b.Categoria).Include(b => b.Espacio);

                var numeroPagina = Pagina ?? 1;

                return View(await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina));
            }
            
            return View();
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
            ViewData["EspacioId"] = new SelectList(_context.Espacios, "Id", "Nombre", bien.EspacioId);
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", bien.CategoriaId);
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
