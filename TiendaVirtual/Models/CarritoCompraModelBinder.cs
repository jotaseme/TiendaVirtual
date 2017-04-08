using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaVirtual.Models
{
    public class CarritoCompraModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //throw new NotImplementedException();
            CarritoCompra cc = (CarritoCompra) controllerContext.HttpContext.Session["key"];
            if (cc == null)
            {
                cc = new CarritoCompra();
                controllerContext.HttpContext.Session["key"] = cc;

            }
            return cc;
        }
    }
}