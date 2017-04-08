using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    public class LoginController : Controller
    {
        private tiendaEntities db = new tiendaEntities();
        // GET: Login
        public ActionResult Index(CarritoCompra cc)
        {
            ViewBag.carritoSize = cc.Count;
            float totalPrecio = 0;
            foreach (Producto p in cc)
            {
                totalPrecio = totalPrecio + p.precio.Value;
            }
            ViewBag.totalPrecio = totalPrecio;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login objLogin)
        {
            // controlar campos vacios
            string login = objLogin.login;
            var usuario = (from u in db.Usuarios
                           where u.login == objLogin.login
                           where u.password == objLogin.password
                           select u).First();
            if (usuario == null)
            {
                ViewBag.error = "¡El usuario o la contraseña no existen!";
                return View("Index");

            }
            else
            {
                Session["usuario"] = usuario.login;


                return RedirectToAction("Index","Productos");
            }
            

        }


    }
}
