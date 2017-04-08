using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    public class PedidosController : Controller
    {
        private tiendaEntities db = new tiendaEntities();

        // GET: Pedidos
        public ActionResult Index(CarritoCompra cc)
        {
            ViewBag.carritoSize = cc.Count;
            float totalPrecio = 0;
            foreach (Producto p in cc)
            {
                totalPrecio = totalPrecio + p.precio.Value;
            }
            ViewBag.totalPrecio = totalPrecio;

            if (Session["usuario"]!=null)
            {

                string login_user = (string)Session["usuario"];
                Usuario usuario = (from u in db.Usuarios
                                            where u.login == login_user
                                            select u).First();
                List <Pedido> pedidos = (List<Pedido>)(from p in db.Pedidos
                                        where p.IdUsuario==usuario.Id
                                        select p).ToList();

                return View(pedidos);
            }
            else
            {
                return RedirectToAction("Index","Login");
            }
            
        }

        // GET: Pedidos/Details/5
        public ActionResult Details(int? id, CarritoCompra cc)
        {
            ViewBag.carritoSize = cc.Count;
            float totalPrecio = 0;
            foreach (Producto p in cc)
            {
                totalPrecio = totalPrecio + p.precio.Value;
            }
            ViewBag.totalPrecio = totalPrecio;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }

            List<Pedido_producto> pedido_final = (List<Pedido_producto>)(from pp in db.Pedido_producto
                                                                         where pp.idPedido == id
                                                                         select pp).ToList();
            PedidoProducts ped_prod = new PedidoProducts();
            ped_prod.pedido = pedido;
            ped_prod.pedido_producto = pedido_final;


            return View(ped_prod);
        }

        // GET: Pedidos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pedidos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int total, CarritoCompra cc)
        {


            // Controlar que el usuario esta registrado
            if (Session["usuario"]!=null)
            {
                string login_user = (string)Session["usuario"];
                Usuario usuario = (from u in db.Usuarios
                               where u.login == login_user
                               select u).First();

                Pedido pedido = new Pedido
                {
                    IdUsuario = usuario.Id,
                    fecha = DateTime.Now,
                    total = total

                };
                db.Pedidos.Add(pedido);
                db.SaveChanges();
                var id = pedido.IdPedido;

                foreach (Producto p in cc)
                {
                    Pedido_producto pro = (Pedido_producto)(from pp in db.Pedido_producto
                                                            where pp.idProducto == p.Id
                                                            where pp.idPedido == id
                                                            select pp).FirstOrDefault();



                    if (pro != null)
                    {
                        pro.cantidad = pro.cantidad + 1;
                        db.Entry(pro).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        Pedido_producto productos_pedido = new Pedido_producto
                        {
                            idPedido = id,
                            idProducto = p.Id,
                            nombre = p.nombre,
                            image = p.image,
                            cantidad = 1
                        };
                        db.Pedido_producto.Add(productos_pedido);
                        db.SaveChanges();
                    }

                }
                cc.RemoveRange(0, cc.Count);

                return RedirectToAction("Details", "Pedidos", new { id = id });
            }else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }
       

        // GET: Pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPedido,IdUsuario,fecha,total,idProducto")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedidos.Find(id);
            db.Pedidos.Remove(pedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
