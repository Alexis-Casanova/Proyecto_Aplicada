using Microsoft.AspNetCore.Mvc;

namespace ProyectoVersion1.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult NotFoundPage()
        {
            return View("404");
        }
    }
}
