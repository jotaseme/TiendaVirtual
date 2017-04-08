using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class PedidoProducts
    {
        public Pedido pedido { get; set; }
        public List <Pedido_producto> pedido_producto { get; set; }
    }
}