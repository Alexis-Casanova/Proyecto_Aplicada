using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Authorize(Policy = "root")]

        [HttpGet]
        public IActionResult IndexAdministrador()
        {
            var administradorId = User.FindFirst("AdministradorId").Value;
            var administrador = _context.Trabajadores.Find(Int32.Parse(administradorId));
            return View(administrador);
        }
        [Authorize(Policy = "mortal")]

        [HttpGet] //Este trabaja con encargos, porque es mas conveniente
        public IActionResult IndexTrabajador()
        {
            var trabajadorId = User.FindFirst("TrabajadorId").Value;
            var encargo = _context.Encargos.Include(b=>b.Trabajador).Where(b=>b.TrabajadorId.ToString() == trabajadorId).ToList();
            return View(encargo);    
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
