using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
    public class BienesController : Controller
    {
        private readonly ProyectoVersion1Context _context;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _servicioNotificacion;

        public BienesController(ProyectoVersion1Context context, IConfiguration configuration, INotyfService oNotificacion)
        {
            _context = context;
            _configuration = configuration;
            _servicioNotificacion = oNotificacion;
        }

        // GET: Bienes / Reciben las consultas
        [BindProperty(SupportsGet =true)]
        public int? Pagina { get; set; }
        public async Task<IActionResult> Index(string buscaEspacio, string buscaCategoria)
        {
            ViewData["Espacios"] = new SelectList(_context.Espacios, "Id", "Nombre", buscaEspacio);
            ViewData["Categoria"] = new SelectList(_context.Categorias, "Id", "Nombre",buscaCategoria);
            ViewData["BuscaEspacio"] = buscaEspacio;
            ViewData["BuscaCategoria"] = buscaCategoria;

            if (_context.Bienes != null)
            {
                var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 10);
                var consulta = _context.Bienes.Include(b => b.Categoria).Include(b => b.Espacio).Select(u=>u);

                if(buscaEspacio != null)
                {
                    consulta = consulta.Where(b => b.EspacioId == Int32.Parse(buscaEspacio));
                }

                if(buscaCategoria != null)
                {
                    consulta = consulta.Where(c => c.CategoriaId == Int32.Parse(buscaCategoria));
                }

                if (buscaEspacio!=null && buscaCategoria!=null)
                {
                    consulta = consulta.Where(b => b.EspacioId == Int32.Parse(buscaEspacio) && b.CategoriaId == Int32.Parse(buscaCategoria));
                }
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

        public List<string> Estados = new List<string>() { "Activo", "Mantenimiento", "Dañado", "Perdido" };

        // GET: Bienes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewData["EspacioId"] = new SelectList(_context.Espacios, "Id", "Nombre");
            ViewData["EstadoInicial"] = new SelectList(Estados,"","");
            return View();
        }


        // POST: Bienes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Nombre,Descripcion,Precio,EstadoInicial,FechaIngreso,EspacioId,CategoriaId")] Bien bien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bien);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Custom($"¡Bien {bien.Nombre} creado correctamente!", 5, "green", "fa fa-check");
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", bien.CategoriaId);
            ViewData["EspacioId"] = new SelectList(_context.Espacios, "Id", "Nombre", bien.EspacioId);
            ViewData["EstadoInicial"] = new SelectList(Estados, "", "",bien.EstadoInicial);
            _servicioNotificacion.Custom($"Es necesario corregir los problemas para poder crear el bien {bien.Nombre}", 5, "red", "fa fa-exclamation-circle");
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
            ViewData["EstadoInicial"] = new SelectList(Estados, "", "", bien.EstadoInicial);
            return View(bien);
        }

        // POST: Bienes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nombre,Descripcion,Precio,EstadoInicial,FechaIngreso,EspacioId,CategoriaId")] Bien bien)
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
                    _servicioNotificacion.Custom($"¡Bien {bien.Nombre} editado correctamente!", 5, "blue", "fa fa-cog");
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
            ViewData["EstadoInicial"] = new SelectList(Estados, "", "", bien.EstadoInicial);
            _servicioNotificacion.Custom($"Es necesario corregir los problemas para poder editar el bien {bien.Nombre}", 5, "red", "fa fa-exclamation-circle");
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
                await _context.SaveChangesAsync();
                _servicioNotificacion.Custom($"¡Bien {bien.Nombre} eliminado correctamente!", 5, "red", "fa fa-trash");
            }
            else
            {
                _servicioNotificacion.Custom($"Error al eliminar el bien {bien.Nombre}", 5, "black", "fa fa-exclamation-circle");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BienExists(int id)
        {
            return _context.Bienes.Any(e => e.Id == id);
        }
    }
}
