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
    [Authorize(Policy = "root")]

    public class TrabajadoresController : Controller
    {
        
        private readonly ProyectoVersion1Context _context;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _servicioNotificacion;

        public TrabajadoresController(ProyectoVersion1Context context, IConfiguration configuration, INotyfService oNotificacion)
        {
       
            _context = context;
            _configuration = configuration;
            _servicioNotificacion = oNotificacion;
        }

        public List<string> Tipos = new List<string>() { "Docente", "Secretaria", "Administrativo", "Técnico" };
        // GET: Trabajadores
        [BindProperty(SupportsGet =true)]
        public int? Pagina {  get; set; }
        public async Task<IActionResult> Index(string buscaTipo)
        {

            ViewData["Tipos"] = new SelectList(Tipos, "", "", buscaTipo);
            ViewData["BuscaTipo"] = buscaTipo;
            if (_context.Trabajadores!=null)
            { 
                var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 10);
                var consulta = _context.Trabajadores.Where(u => u.Cargo !="Administrador").Select(u=>u);
                    if (buscaTipo!=null)
                    {
                    consulta = consulta.Where(b => b.Tipo == buscaTipo);
                    }

                var numeroPagina = Pagina ?? 1;

                return View(await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina));
            }
            return View();
        }

        // GET: Trabajadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trabajador == null)
            {
                return NotFound();
            }

            return View(trabajador);
        }

        // GET: Trabajadores/Create
        public IActionResult Create()
        {
            ViewData["Tipo"] = new SelectList(Tipos,"","");
            return View();
        }

        // POST: Trabajadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Email,Contraseña,Telefono,Cargo,Tipo")] Trabajador trabajador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trabajador);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Custom($"¡Trabajador {trabajador.Nombre} creado correctamente!", 5, "green", "fa fa-check");
                return RedirectToAction(nameof(Index));
            }
            _servicioNotificacion.Custom($"Es necesario corregir los problemas para poder crear al trabajador {trabajador.Nombre}", 5, "red", "fa fa-exclamation-circle");
            ViewData["Tipo"] = new SelectList(Tipos,"","",trabajador.Tipo);
            return View(trabajador);
        }

        // GET: Trabajadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadores.FindAsync(id);
            if (trabajador == null)
            {
                return NotFound();
            }
            ViewData["Tipo"] = new SelectList(Tipos, "", "", trabajador.Tipo);
            return View(trabajador);
        }

        // POST: Trabajadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,Contraseña,Telefono,Cargo,Tipo")] Trabajador trabajador)


        {
            if (id != trabajador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trabajador);
                    await _context.SaveChangesAsync();
                    _servicioNotificacion.Custom($"¡Trabajador {trabajador.Nombre} editado correctamente!", 5, "blue", "fa fa-cog");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrabajadorExists(trabajador.Id))
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
            ViewData["Tipo"] = new SelectList(Tipos, "", "", trabajador.Tipo);
            _servicioNotificacion.Custom($"Es necesario corregir los problemas para poder editar al trabajador {trabajador.Nombre}", 5, "red", "fa fa-exclamation-circle");
            return View(trabajador);
        }

        // GET: Trabajadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trabajador == null)
            {
                return NotFound();
            }

            return View(trabajador);
        }

        // POST: Trabajadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trabajador = await _context.Trabajadores.FindAsync(id);
            if (trabajador != null)
            {
                _context.Trabajadores.Remove(trabajador);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Custom($"¡Trabajador {trabajador.Nombre} eliminado correctamente!", 5, "red", "fa fa-trash");
            }
            else
            {
                _servicioNotificacion.Custom($"Error al eliminar al trabajador {trabajador.Nombre}", 5, "black", "fa fa-exclamation-circle");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TrabajadorExists(int id)
        {
            return _context.Trabajadores.Any(e => e.Id == id);
        }
    }
}
