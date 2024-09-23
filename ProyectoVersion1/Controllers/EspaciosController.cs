using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoVersion1.Data;
using ProyectoVersion1.Models;
using X.PagedList;

namespace ProyectoVersion1.Controllers
{
    [Authorize(Policy = "root")]

    public class EspaciosController : Controller
    {
        private readonly ProyectoVersion1Context _context;
        private readonly INotyfService _servicioNotificacion;
        private readonly IConfiguration _configuration;

        public EspaciosController(ProyectoVersion1Context context, INotyfService oNotificacion, IConfiguration configuration)
        {
            _context = context;
            _servicioNotificacion = oNotificacion;
            _configuration = configuration;
        }

        // GET: Espacios
        [BindProperty(SupportsGet = true)]
        public int? Pagina { get; set; }
        public async Task<IActionResult> Index()
        {
            if (_context.Espacios != null)
            {
                var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 10);
                var consulta = _context.Espacios.Select(u => u);

                var numeroPagina = Pagina ?? 1;

                return View(await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina));
            }
            return View();
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
            ViewData["Tipo"] = new SelectList(Tipos,"","");
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

                _servicioNotificacion.Custom($"¡Espacio { espacio.Nombre } creado correctamente!", 5, "green", "fa fa-check");

                return RedirectToAction(nameof(Index));
            }
            ViewData["Tipo"] = new SelectList(Tipos, "", "",espacio.Tipo);
            _servicioNotificacion.Custom($"Es necesario corregir los problemas para poder crear el espacio { espacio.Nombre } ", 5, "red", "fa fa-exclamation-circle");
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
            ViewData["Tipo"] = new SelectList(Tipos, "", "", espacio.Tipo);
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
                    _servicioNotificacion.Custom($"¡Espacio {espacio.Nombre} editado correctamente!", 5, "blue", "fa fa-cog");
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
            ViewData["Tipo"] = new SelectList(Tipos, "", "", espacio.Tipo);
            _servicioNotificacion.Custom($"Es necesario corregir los problemas para poder editar el espacio {espacio.Nombre}", 5, "red", "fa fa-exclamation-circle");
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
                await _context.SaveChangesAsync();

                _servicioNotificacion.Custom($"¡Espacio {espacio.Nombre} eliminado correctamente!", 5, "red", "fa fa-trash");
            }
            else
            {
                _servicioNotificacion.Custom($"Error al eliminar el espacio {espacio.Nombre}", 5, "black", "fa fa-exclamation-circle");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EspacioExists(int id)
        {
            return _context.Espacios.Any(e => e.Id == id);
        }
    }
}
