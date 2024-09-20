using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoVersion1.Data;
using ProyectoVersion1.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ProyectoVersion1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProyectoVersion1Context _context; 

        public HomeController(ILogger<HomeController> logger, ProyectoVersion1Context context)
        {
            _logger = logger;
            _context = context; 
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Policy = "root")]

        [HttpGet]
        public IActionResult IndexAdministrador()
        {
            var administradorId = User.FindFirst("AdministradorId").Value;
            var administrador = _context.Trabajadores.Find(Int32.Parse(administradorId));
            return View(administrador);
        }

        public List<string> Estados = new List<string>() { "Activo", "Mantenimiento", "Dañado", "Perdido"};

        [Authorize(Policy = "mortal")]
        [HttpGet] //Este trabaja con encargos, porque es mas conveniente
        public IActionResult IndexTrabajador(string estadoBien)
        {
            var trabajadorId = User.FindFirst("TrabajadorId").Value;
            var encargos = _context.Encargos.Include(b=>b.Trabajador).Include(b=>b.Bien).Where(b=>b.TrabajadorId.ToString() == trabajadorId).ToList();

            var trabajador = _context.Trabajadores.Find(Int32.Parse(trabajadorId)); //PAra trabajar el nombre

            if(estadoBien != null)
            {
                encargos = encargos.Where(b => b.Bien.Estado == estadoBien).ToList();
            }
            ViewData["Estados"] = new SelectList(Estados); // PAra la lista desplegable

            // Mantiene el valor constante en la aplicacion 
            ViewData["EstadoBien"] = estadoBien;
            ViewData["Trabajador"] = trabajador;
            return View(encargos);    
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
