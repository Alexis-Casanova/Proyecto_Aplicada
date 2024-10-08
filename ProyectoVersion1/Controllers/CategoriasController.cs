﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoVersion1.Data;
using ProyectoVersion1.Models;
using X.PagedList;

namespace ProyectoVersion1.Controllers
{
    [Authorize(Policy = "root")]

    public class CategoriasController : Controller
    {
        private readonly ProyectoVersion1Context _context;
        private readonly INotyfService _servicioNotificacion;
        private readonly IConfiguration _configuration;

        public CategoriasController(ProyectoVersion1Context context, INotyfService oNotificacion, IConfiguration configuration)
        {
            _context = context;
            _servicioNotificacion = oNotificacion;
            _configuration = configuration;
        }

        [BindProperty(SupportsGet = true)]
        public int? Pagina { get; set; }
        public async Task<IActionResult> Index()
        {
            if (_context.Categorias != null)
            {
                var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 10);
                var consulta = _context.Categorias.Select(u => u);

                var numeroPagina = Pagina ?? 1;

                return View(await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina));
            }
            return View();
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripción")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Custom($"¡Categoría {categoria.Nombre} creado correctamente!", 5, "green", "fa fa-check");
                return RedirectToAction(nameof(Index));
            }
            _servicioNotificacion.Custom($"Es necesario corregir los problemas para poder crear la categoría {categoria.Nombre}", 5, "red", "fa fa-exclamation-circle");
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripción")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                    _servicioNotificacion.Custom($"¡Categoría {categoria.Nombre} editado correctamente!", 5, "blue", "fa fa-cog");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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
            _servicioNotificacion.Custom($"Es necesario corregir los problemas para poder editar la categoría {categoria.Nombre} ", 5, "red", "fa fa-exclamation-circle");
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Custom($"¡Categoría {categoria.Nombre} eliminado correctamente!", 5, "red", "fa fa-trash");
            }
            else
            {
                _servicioNotificacion.Custom($"Error al eliminar la categoría {categoria.Nombre}", 5, "black", "fa fa-exclamation-circle");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}
