﻿using AspNetCoreHero.ToastNotification.Abstractions;
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
    public class EncargosController : Controller
    {
        private readonly ProyectoVersion1Context _context;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _servicioNotificacion;

        public EncargosController(ProyectoVersion1Context context, IConfiguration configuration, INotyfService oNotificacion)
        {
            _context = context;
            _configuration = configuration;
            _servicioNotificacion = oNotificacion;
        }

        // GET: Encargos
        [BindProperty(SupportsGet = true)]
        public int? Pagina { get; set; }

        public async Task<IActionResult> Index(string buscaTrabajador)
        {
            ViewData["Trabajadores"] = new SelectList(_context.Trabajadores.Where(t => t.Cargo != "Administrador"), "Id", "Nombre", buscaTrabajador);
            ViewData["BuscaTrabajador"] = buscaTrabajador;

            if (_context.Encargos != null)
            {
                var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 10);
                var consulta = _context.Encargos.Include(b => b.Trabajador).Include(b => b.Bien).Where(e => e.Trabajador.Cargo != "Administrador").Select(u => u);

                if (buscaTrabajador != null)
                {
                    consulta = consulta.Where(b => b.TrabajadorId == Int32.Parse(buscaTrabajador));
                }

                var numeroPagina = Pagina ?? 1;

                return View(await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina));
            }

            return View();
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
                .Where(e => e.Trabajador.Cargo != "Administrador")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (encargo == null)
            {
                return NotFound();
            }

            return View(encargo);
        }

        public List<string> Estados = new List<string>() { "Activo", "Mantenimiento", "Dañado", "Perdido" };

        // GET: Encargos/Create
        public IActionResult Create()
        {
            ViewData["BienId"] = new SelectList(_context.Bienes, "Id", "CodigoNombre");
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadores.Where(t => t.Cargo != "Administrador"), "Id", "Nombre");
            ViewData["EstadoActual"] = new SelectList(Estados,"","");
            return View();
        }

        // POST: Encargos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TrabajadorId,BienId,EstadoActual,FechaInicio,FechaFin")] Encargo encargo)
        {
            //Para mostrar el nombre del bien y del tr abajador en las notifiaciones
            encargo.Bien = await _context.Bienes.FirstOrDefaultAsync(b => b.Id == encargo.BienId);
            encargo.Trabajador = await _context.Trabajadores.FirstOrDefaultAsync(t => t.Id == encargo.TrabajadorId);

                    ViewData["BienId"] = new SelectList(_context.Bienes, "Id", "CodigoNombre", encargo.BienId);
                    ViewData["TrabajadorId"] = new SelectList(_context.Trabajadores, "Id", "Nombre", encargo.TrabajadorId);
                    ViewData["EstadoActual"] = new SelectList(Estados, "", "", encargo.EstadoActual);
            if (ModelState.IsValid)
            {
                if(encargo.FechaInicio > encargo.FechaFin)
                {
                    
                    _servicioNotificacion.Error("Error: Las fechas no son correctas.");
                    return View(encargo);
                }

                _context.Add(encargo);
                await _context.SaveChangesAsync();

                if (encargo.Bien == null || encargo.Trabajador == null)
                {
                    _servicioNotificacion.Error("Error: No se pudo encontrar el Bien o el Trabajador.");
                    return View(encargo);
                }

                    _servicioNotificacion.Custom($"¡Encargo del Bien {encargo.Bien.Nombre} al trabajador {encargo.Trabajador.Nombre} creado correctamente!",5, "green", "fa fa-check");
                    return RedirectToAction(nameof(Index));
            }
            
            _servicioNotificacion.Custom($"Es necesario corregir los problemas para poder editar el encargo del Bien {encargo.Bien.Nombre} al trabajador {encargo.Trabajador.Nombre}", 5, "red", "fa fa-exclamation-circle");
            return View(encargo);
        }

        // GET: Encargos/Edit/5
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
            ViewData["BienId"] = new SelectList(_context.Bienes, "Id", "CodigoNombre", encargo.BienId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadores.Where(t => t.Cargo != "Administrador"), "Id", "Nombre", encargo.TrabajadorId);
            ViewData["EstadoActual"] = new SelectList(Estados, "", "", encargo.EstadoActual);
            return View(encargo);
        }

        // POST: Encargos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TrabajadorId,BienId,EstadoActual,FechaInicio,FechaFin")] Encargo encargo)
        {
            if (id != encargo.Id)
            {
                return NotFound();
            }

            //Para mostrar el nombre del bien y del trabajador en las notifiaciones
            encargo.Bien = await _context.Bienes.FirstOrDefaultAsync(b => b.Id == encargo.BienId);
            encargo.Trabajador = await _context.Trabajadores.FirstOrDefaultAsync(t => t.Id == encargo.TrabajadorId);

            if (encargo.Bien == null || encargo.Trabajador == null)
            {
                _servicioNotificacion.Error("Error: No se pudo encontrar el Bien o el Trabajador.");
                return View(encargo);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encargo);
                    await _context.SaveChangesAsync();
                    _servicioNotificacion.Custom($"¡Encargo del Bien {encargo.Bien.Nombre} al trabajador {encargo.Trabajador.Nombre} creado correctamente!", 5, "blue", "fa fa-cog");

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
            ViewData["BienId"] = new SelectList(_context.Bienes, "Id", "CodigoNombre", encargo.BienId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadores.Where(t => t.Cargo != "Administrador"), "Id", "Nombre", encargo.TrabajadorId);
            ViewData["EstadoActual"] = new SelectList(Estados, "", "", encargo.EstadoActual);
            _servicioNotificacion.Custom($"Es necesario corregir los problemas para poder editar el encargo del Bien {encargo.Bien.Nombre} al trabajador {encargo.Trabajador.Nombre}", 5, "red", "fa fa-exclamation-circle");
            return View(encargo);
        }

        // GET: Encargos/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var encargo = await _context.Encargos.FindAsync(id);

            ///Para mostrar el nombre del bien y del trabajador en las notifiaciones
            encargo.Bien = await _context.Bienes.FirstOrDefaultAsync(b => b.Id == encargo.BienId);
            encargo.Trabajador = await _context.Trabajadores.FirstOrDefaultAsync(t => t.Id == encargo.TrabajadorId);

            if (encargo.Bien == null || encargo.Trabajador == null)
            {
                _servicioNotificacion.Error("Error: No se pudo encontrar el Bien o el Trabajador.");
                return View(encargo);
            }

            if (encargo != null)
            {
                _context.Encargos.Remove(encargo);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Custom($"¡Encargo del Bien {encargo.Bien.Nombre} al trabajador {encargo.Trabajador.Nombre} eliminado correctamente!", 5, "red", "fa fa-trash");
            }
            else
            {
                _servicioNotificacion.Custom($"Error al eliminar el encargo del Bien {encargo.Bien.Nombre} al trabajador {encargo.Trabajador.Nombre}", 5, "black", "fa fa-exclamation-circle");
                return View(encargo);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EncargoExists(int id)
        {
            return _context.Encargos.Any(e => e.Id == id);
        }
    }
}
