using System;
namespace DeliboInv2
{
    public class Productos
    { 
       public string idProducto;
       public double precio;
       public int cantidadDis;
       public string tipo;
       public string nombre;


         public Productos()
        {
        }


        public override string ToString()
        {
            return ("Id: "+idProducto + " Precio: " + precio + " Cantidad Disponible: " + cantidadDis + " Tipo: " + tipo + " Nombre: " + nombre+"\n");
        }



        internal static void Add(Productos nuevoProducto)
        {

        }
    
    }
}
