using System;
namespace DeliboInv2
{
    public class Ventas
    {
        public string idVenta;
        public string tpago;
        public string vendedor;
        public TimeSpan horaVenta;
        public DateTime fechaVenta; 
        public int cantidadVen;
        public string pedido;
        public DateTime fechaEnt;

        public Ventas()
        {
        }


        public override string ToString()
        {
            return ("id: "+idVenta + " Tipo de pago: " + tpago + " Vendedor: " + vendedor + " Hora: " + horaVenta + " Fecha: "+ fechaVenta+ " Cantidad Vendida: "+ cantidadVen + " Pedido: " + pedido + " Fecha Programada para la entrega: " + fechaEnt+"\n"); 
        }

        internal static void Add(Ventas nuevaVenta)
        {

        }

        internal static void Crear(Ventas EmpleadoVen)
        {

        }
    }
}
    
