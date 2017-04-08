using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    public class CarritoController : Controller
    {
        // GET: Carrito
        public ActionResult Index(CarritoCompra cc)
        {
            
            ViewBag.carritoSize = cc.Count;
            float totalPrecio = 0;
            foreach (Producto p in cc)
            {
                totalPrecio = totalPrecio + p.precio.Value;
            }
            ViewBag.totalPrecio = totalPrecio;
            return View(cc);
        }


        // GET: Add product to Carrito
        public ActionResult AddToCarrito(CarritoCompra cc, int id)
        {
            tiendaEntities db = new tiendaEntities();
            Producto producto = db.Productos.Find(id);
            cc.Add(producto);
            return RedirectToAction("Index", "Productos");
        }

        // GET: Delete product from Carrito
        public ActionResult DeleteFromCarrito(CarritoCompra cc, int id)
        {
            tiendaEntities db = new tiendaEntities();
            Producto producto = db.Productos.Find(id);

            cc.RemoveAll(x => x.Id == producto.Id);

            return RedirectToAction("Index", "Carrito");
        }
    }
}