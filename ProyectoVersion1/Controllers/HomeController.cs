using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoVersion1.Data;
using ProyectoVersion1.Models;
using Rotativa.AspNetCore;
using System.Diagnostics;
using X.PagedList;

namespace ProyectoVersion1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProyectoVersion1Context _context;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, ProyectoVersion1Context context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
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

        [BindProperty(SupportsGet =true)]
        public int? Pagina { get; set; }

        [Authorize(Policy = "mortal")]
        [HttpGet] //Este trabaja con encargos, porque es mas conveniente
        public IActionResult IndexTrabajador(string estadoBien)
        {

            // Para la lista desplegable de estados en la vista
            ViewData["Estados"] = new SelectList(Estados, "", "", estadoBien);

            var trabajadorId = User.FindFirst("TrabajadorId").Value;
            var encargos = _context.Encargos.Include(b => b.Trabajador).Include(b => b.Bien)
                .Where(b => b.TrabajadorId.ToString() == trabajadorId);

            // Obtener los datos del trabajador para mostrarlos en la vista
            var trabajador = _context.Trabajadores.Find(Int32.Parse(trabajadorId));

            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 10);
            var consulta = encargos.Select(u => u);

            if (!string.IsNullOrEmpty(estadoBien))
            {
                consulta = consulta.Where(b => b.EstadoActual == estadoBien);
            }

            var numeroPagina = Pagina ?? 1;
            var pagedEncargos = consulta.ToPagedList(numeroPagina, registrosPorPagina);
            
            var viewModel = new TrabajadorEncargoViewModel
            {
                Trabajador = trabajador,
                EstadoBien = estadoBien,
                EncargosPaginados = pagedEncargos
            };

            return View(viewModel);    
        }
        public ActionResult DownloadViewPDF(string estadoBien)
        {
			// Para la lista desplegable de estados en la vista
			ViewData["Estados"] = new SelectList(Estados, "", "", estadoBien);

			var trabajadorId = User.FindFirst("TrabajadorId").Value;
			var encargos = _context.Encargos.Include(b => b.Trabajador).Include(b => b.Bien)
				.Where(b => b.TrabajadorId.ToString() == trabajadorId);

			// Obtener los datos del trabajador para mostrarlos en la vista
			var trabajador = _context.Trabajadores.Find(Int32.Parse(trabajadorId));

			var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 10);
			var consulta = encargos.Select(u => u);

			if (!string.IsNullOrEmpty(estadoBien))
			{
				consulta = consulta.Where(b => b.EstadoActual == estadoBien);
			}

			var numeroPagina = Pagina ?? 1;
			var pagedEncargos = consulta.ToPagedList(numeroPagina, registrosPorPagina);

			var viewModel = new TrabajadorEncargoViewModel
			{
				Trabajador = trabajador,
				EstadoBien = estadoBien,
				EncargosPaginados = pagedEncargos
			};

			// Renderizar la vista como PDF con los datos aplicados (incluyendo el filtro)
			var pdfView = new ViewAsPdf("IndexTrabajador", viewModel)
            {
                FileName = "Bienes_Trabajador.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,  // Tamaño de la página (A4, Letter, etc.)
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,  // Orientación de la página (Vertical)
                PageMargins = new Rotativa.AspNetCore.Options.Margins(15, 15, 15, 15),  // Márgenes de la página en milímetros (arriba, derecha, abajo, izquierda)
    
                // Añadir encabezado (con HTML simple o texto)
                CustomSwitches = "--header-center \"Reporte de Bienes del Trabajador\" " +
                     "--header-font-size 12 " +
                     "--header-spacing 5 " +  // Espacio entre el encabezado y el contenido
                     "--header-line " +  // Añade una línea debajo del encabezado

                     // Añadir pie de página
                     "--footer-center \"Página: [page] de [topage]\" " +  // Numeración de páginas
                     "--footer-font-size 10 " +
                     "--footer-spacing 5 " +  // Espacio entre el pie de página y el contenido
                     "--footer-line "  // Añade una línea encima del pie de página
            };

            return pdfView;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
