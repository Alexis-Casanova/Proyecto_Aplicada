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
            ViewData["Trabajadores"] = new SelectList(_context.Trabajadores, "Id", "Nombre", buscaTrabajador);
            ViewData["BuscaTrabajador"] = buscaTrabajador;

            if (_context.Encargos != null)
            {
                var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 10);
                var consulta = _context.Encargos.Include(b => b.Trabajador).Include(b => b.Bien).Select(u => u);

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
            ViewData["BienId"] = new SelectList(_context.Bienes, "Id", "Nombre");
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadores, "Id", "Nombre");
            ViewData["EstadoActual"] = new SelectList(Estados);
            return View();
        }

        // POST: Encargos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TrabajadorId,BienId,EstadoActual,FechaInicio,FechaFin")] Encargo encargo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(encargo);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Success($"¡Encargo del Bien {encargo.BienId} al trabajador {encargo.TrabajadorId} creado correctamente!");
                return RedirectToAction(nameof(Index));
            }
            ViewData["BienId"] = new SelectList(_context.Bienes, "Id", "Nombre", encargo.BienId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadores, "Id", "Nombre", encargo.TrabajadorId);
            ViewData["EstadoActual"] = new SelectList(Estados);
            _servicioNotificacion.Error($"Es necesario corregir los problemas para poder crear el encargo del Bien {encargo.BienId} al trabajador {encargo.TrabajadorId}");
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
            ViewData["BienId"] = new SelectList(_context.Bienes, "Id", "Nombre", encargo.BienId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadores, "Id", "Nombre", encargo.TrabajadorId);
            ViewData["EstadoActual"] = new SelectList(Estados);
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encargo);
                    await _context.SaveChangesAsync();
                    _servicioNotificacion.Success($"¡Encargo del Bien {encargo.BienId} al trabajador {encargo.TrabajadorId} editado correctamente!");
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
            ViewData["EstadoActual"] = new SelectList(Estados);
            _servicioNotificacion.Error($"Es necesario corregir los problemas para poder editar el encargo del Bien {encargo.BienId} al trabajador {encargo.TrabajadorId}");
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
            if (encargo != null)
            {
                _context.Encargos.Remove(encargo);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Success($"¡Encargo del Bien {encargo.BienId} al trabajador {encargo.TrabajadorId} eliminado correctamente!");
            }
            else
            {
                _servicioNotificacion.Error($"Error al eliminar el encargo del Bien {encargo.BienId} al trabajador {encargo.TrabajadorId}");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EncargoExists(int id)
        {
            return _context.Encargos.Any(e => e.Id == id);
        }
    }
}
