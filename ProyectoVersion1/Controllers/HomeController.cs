using Microsoft.AspNetCore.Mvc;
using ProyectoVersion1.Data;
using ProyectoVersion1.Models;
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
        [HttpGet]
        public IActionResult IndexAdministrador()
        {
            var adminId = User.FindFirst("AdministradorId").Value;
            var administrador = _context.Trabajadores.Where(b => b.Id.ToString() == adminId);
            return View(administrador);
        }
        [HttpGet]
        public IActionResult IndexTrabajador()
        {
            var trabajadorId = User.FindFirst("TrabajadorId").Value;
            var trabajador = _context.Trabajadores.Where(b => b.Id.ToString() == trabajadorId);
            return View(trabajador);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
