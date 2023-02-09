using System;
namespace DeliboInv2
{
    public class Pedidos
    {
        public string idPedido;
        public string empleado;
        public DateTime fechaEmision;
        public TimeSpan hora;
        public string producto;
        public int cantidad;
        public int venta;

        public Pedidos()
        {
        }

        public override string ToString()
        {
            return ("Id: "+idPedido + " Empleado: " + empleado + " Fecha: " + fechaEmision + " Hora: " +hora + " Producto: "+ producto + " Cantidad: " + cantidad+ " Venta: " + venta+"\n");
        }

        internal static void Add(Pedidos nuevoPedido)
        {

        }
    }
}
