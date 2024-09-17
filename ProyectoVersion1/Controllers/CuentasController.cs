using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoVersion1.Data;
using ProyectoVersion1.Models;
using System.Security.Claims;

namespace ProyectoVersion1.Controllers
{
    public class CuentasController : Controller
    {
        private readonly ProyectoVersion1Context _context;
        public CuentasController(ProyectoVersion1Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult LoginAdministrador()    
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdministrador(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ClaimsIdentity identity = null;
            bool isAutenticated = false;


            if(model.Role == "Administrador")
            {
                var administrador = await _context.Trabajadores.FirstOrDefaultAsync(a => a.Email == model.Email && a.Contraseña == model.Password);

                if (administrador != null)
                {
                    identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, administrador.Email),
                        new Claim(ClaimTypes.Role, "Administrador"),
                        new Claim("AdminId", administrador.Id.ToString())
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                    isAutenticated = true;
                }

            }
            if (isAutenticated)
            {
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if(model.Role == "Administrador")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Valores incorrectos");
            return RedirectToAction("Index", "Home");
        }




        //Creamos las cuentas para el Trabajador

        [HttpGet]
        public IActionResult LoginTrabajador()
        {
            return View();
        }


    }
}
