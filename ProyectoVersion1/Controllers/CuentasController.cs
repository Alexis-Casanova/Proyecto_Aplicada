using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
                var administrador = await _context.Trabajadores
                    .FirstOrDefaultAsync(a => a.Email == model.Email && a.Contraseña == model.Password && a.Cargo == model.Role);

                if (administrador != null)
                {
                    identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, administrador.Email),
                        new Claim(ClaimTypes.Role, "Administrador"),
                        new Claim("AdministradorId", administrador.Id.ToString())
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
                    return RedirectToAction("IndexAdministrador", "Home");
                }
            }
            ModelState.AddModelError("", "Verifique su Correo o Contraseña");
            return View(model);
        }




        //Creamos las cuentas para el Trabajador

        [HttpGet]
        public IActionResult LoginTrabajador()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginTrabajador(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ClaimsIdentity identity = null; //Inicializo la identidad del usuario
            bool isAutenticated = false; //Si ya esta autenticado

            if(model.Role == "Trabajador")
            {
                var trabajador = await _context.Trabajadores
                    .FirstOrDefaultAsync(a => a.Email == model.Email && a.Contraseña == model.Password && a.Cargo == model.Role);
                if(trabajador != null)
                {
                    identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.Role, "Trabajador"),
                        new Claim("TrabajadorId", trabajador.Id.ToString())
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                    isAutenticated = true;
                }

            }

            if (isAutenticated)
            {
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal);

                if(model.Role == "Trabajador")
                {
                    return RedirectToAction("IndexTrabajador", "Home"); //Este debe de mandar a la consulta
                }
            }
            ModelState.AddModelError("", "Verifique su Correo o Contraseña");
            return View(model);
        }


        public async  Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
