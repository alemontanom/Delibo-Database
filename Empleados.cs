using System;
namespace DeliboInv2
{
    public class Empleados
    {
        public string nomina;
        public string contraseñaRegistro;
        public string puesto;
        public string nombre;
        public string apellidoP;
        public string apellidoM;

        public Empleados()
        {
        }

        public void guardarContraseña(string contra)
        {
            this.contraseñaRegistro = contra;
        }

        public override string ToString()
        {
            return ("Nomina: "+nomina + " " + contraseñaRegistro + " Puesto: " + puesto + " Nombre: " + nombre + " Apellido Paterno: " + apellidoP + " Apellido Materno: " + apellidoM+ "\n");
        }

        internal static void Add(Empleados nuevoEmpleado)
        {

        }
    }
}
