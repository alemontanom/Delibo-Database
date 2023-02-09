using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace DeliboInv2
{
    class Program
    {
        static String conexionString = "Server=localhost;Database=DeliboInv3;Uid=root;Pwd=A8917mm@";
        MySqlConnection miConexion = new MySqlConnection(conexionString);


        static void Main(string[] args)
        {
            probarConexion(); 
            Login();
        }

        public static void Login()
        {
            Console.WriteLine("Ingrese su nómina:\n");
            string nominaI = Console.ReadLine();
            Console.WriteLine("\nIngrese su contraseña:\n");
            string contraseñaI = Console.ReadLine();
            IngresoEmpleados(nominaI, contraseñaI);
        }


        public static void IngresoEmpleados(string Empleado, string Contra)
        {
            MySqlConnection miConexion = new MySqlConnection(conexionString);
            MySqlCommand miComando = new MySqlCommand();


            try
            {
                miConexion.Open();
                miComando.Connection = miConexion;
                Empleados empleadito = new Empleados();
                miComando.CommandText = "SELECT* FROM Empleados WHERE nomina=@nomina AND contraseñaRegistro=@contraseñaRegistro";


                MySqlParameter paramNomina = new MySqlParameter();
                paramNomina.ParameterName = "@nomina";
                paramNomina.DbType = DbType.String;
                paramNomina.Value = Empleado;

                miComando.Parameters.Add(paramNomina);

                MySqlParameter paramContraseña = new MySqlParameter();
                paramContraseña.ParameterName = "@contraseñaRegistro";
                paramContraseña.DbType = DbType.String;
                paramContraseña.Value = Contra;

                miComando.Parameters.Add(paramContraseña);

                miComando.ExecuteNonQuery();

                MySqlDataReader miLector = null;
                miLector = miComando.ExecuteReader();
                while (miLector.Read() == true)
                {
                    empleadito.contraseñaRegistro = miLector[1].ToString();
                }

                if (empleadito.contraseñaRegistro == Contra)
                {
                    Console.WriteLine("\nBienvenido");
                    DesplegarMenu();
                }
                else
                    Console.WriteLine("\nEse usuario no está registrado");



            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

            finally
            {

                miConexion.Close();
            }

        }

        static void DesplegarMenu()
        {
            Console.WriteLine("\nIngrese la letra de la acción que quiera realizar:\n" +
                "A) Consultar tablas\n" +
                "B) Agregar registro\n" +
                "C) Modificar registros\n" +
                "D) Dar de baja a un empleado\n"+
                "E) Salir\n");
            char choice;
            char.TryParse(Console.ReadLine(), out choice);
            switch (choice)
            {
                case 'A':
                    {
                        Console.WriteLine("\nIngrese la tabla que quiere consultar:\n");
                        string tablaC = Console.ReadLine();
                        if (tablaC == "Productos")
                            LeerProductos();
                        else if (tablaC == "Pedidos")
                            LeerPedidos();
                        else if (tablaC == "Ventas")
                            LeerVentas();
                        else if (tablaC == "Empleados")
                            LeerEmpleados();
                        else if (tablaC == "Ventas por empleado")
                        {
                            Ventas EmpleadoVen = new Ventas();
                            LeerVentasEmpleados(EmpleadoVen);
                        }  
                        else
                            Console.WriteLine("\nNo se encontró esa tabla");
                        DesplegarMenu();
                    }
                    break;
                case 'B':
                    {
                        Console.WriteLine("\nIngrese qué tipo de registro quiere agregar:");
                        string registro = Console.ReadLine();
                        if (registro == "Producto")
                        {
                            Productos nuevoProducto = new Productos();
                            crearProducto(nuevoProducto);
                        }
                        else if (registro == "Pedido")
                        {
                            Pedidos nuevoPedido = new Pedidos();
                            crearPedido(nuevoPedido);
                        }

                        else if (registro == "Venta")
                        {
                            Ventas nuevaVenta = new Ventas();
                            crearVenta(nuevaVenta);
                        }
                        else if (registro == "Empleado")
                        {
                            Empleados nuevoEmpleado = new Empleados();
                            crearEmpleado(nuevoEmpleado);
                        }
                        else
                            Console.WriteLine("\nEse registro no existe");
                        DesplegarMenu();
                    }
                    break;
                case 'C':
                    {
                        Console.WriteLine("\nIngrese qué tipo de registro quiere modificar, solo se pueden modificar los productos o los pedidos");
                        string modificar = Console.ReadLine();
                        if (modificar == "Producto")
                            ModificarProducto();
                        else if (modificar == "Pedido")
                            ModificarPedido();
                        else
                            Console.WriteLine("\nEse registro no existe o no se puede modificar");
                        DesplegarMenu();

                    }
                    break;
                case 'D':
                    {
                        BajaEmpleado(); 
                    }
                    break; 
                case 'E':
                    {
                        Console.WriteLine("\nHasta la próxima"); 
                        Environment.Exit(0);
                    }
                    break;
            }
        }


        public static void BajaEmpleado()
        {
            Console.WriteLine("\nIngrese la nómina del empleado que dará de baja");
            string empleadoBye = Console.ReadLine();

            MySqlConnection miConexion = new MySqlConnection(conexionString);
            MySqlCommand miComando = new MySqlCommand();

            try
            {
                miConexion.Open();
                miComando.Connection = miConexion;
                miComando.CommandText = "DELETE FROM Empleados WHERE nomina=@nominaBye";
                miComando.Parameters.Add("@nominaBye", MySqlDbType.VarChar).Value = empleadoBye;

                miComando.ExecuteNonQuery();

                Console.WriteLine("\nEl empleado ha sido eliminado");


            }
            catch (MySqlException)
            {
                Console.WriteLine("\nError al eliminar el empleado");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            finally
            {
                miConexion.Close();
            }

            DesplegarMenu(); 

         }




        public static void ModificarProducto()
        {
            Console.WriteLine("\n¿Qué campo quiere cambiar? Escriba precio o cantidadDis");
            string campoCamProI = Console.ReadLine();

            Console.WriteLine("\nIngrese el id del producto que quiere modificar");
            string idProCamI = Console.ReadLine();

            Console.WriteLine("Ingrese el nuevo valor de " + campoCamProI);
            int valorCamProI = int.Parse(Console.ReadLine()); 

            MySqlConnection miConexion = new MySqlConnection(conexionString);
           
            try
            {
                miConexion.Open();

                MySqlCommand miComando = new MySqlCommand("UPDATE Productos SET " +campoCamProI+ " = @valorCamPro WHERE idProducto=@idProCam");

                miComando.Parameters.Add("@valorCamPro", MySqlDbType.Int16).Value = valorCamProI;
                miComando.Parameters.Add("@idProCam", MySqlDbType.VarChar).Value = idProCamI;

                miComando.Connection = miConexion;

                miComando.ExecuteNonQuery();

                Console.WriteLine("\nSe han actualizado los datos"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }
        }

        public static void ModificarPedido()
        {
           
            MySqlConnection miConexion = new MySqlConnection(conexionString);

            try
            {
                miConexion.Open();

                Console.WriteLine("\n¿Qué campo quiere cambiar? Escriba producto o cantidad");
                string campoCamPedI = Console.ReadLine();

                Console.WriteLine("\nIngrese el id del pedido que quiere modificar");
                string idPedCamI = Console.ReadLine();

                MySqlCommand miComando = new MySqlCommand("UPDATE Pedidos SET " + campoCamPedI + " = @valorCamPed WHERE idPedido=@idPedCam");


                if (campoCamPedI == "cantidad")
                {
                    Console.WriteLine("Ingrese el nuevo valor de " + campoCamPedI);
                    int valorCamPedIC = int.Parse(Console.ReadLine());
                    miComando.Parameters.Add("@valorCamPed", MySqlDbType.Int16).Value = valorCamPedIC;
                }
                else if (campoCamPedI == "producto")
                {
                    Console.WriteLine("Ingrese el nuevo valor de " + campoCamPedI);
                    string valorCamPedIP = Console.ReadLine();
                    miComando.Parameters.Add("@valorCamPed", MySqlDbType.VarChar).Value = valorCamPedIP;
                }
                else
                    Console.WriteLine("Campo inválido"); 

               
                miComando.Parameters.Add("@idPedCam", MySqlDbType.VarChar).Value = idPedCamI;

                miComando.Connection = miConexion;

                miComando.ExecuteNonQuery();

                Console.WriteLine("\nSe han actualizado los datos");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }
        }

        static void crearProducto(Productos nuevoProducto)
        {

            Console.WriteLine("\nIngrese el id del nuevo producto");
            nuevoProducto.idProducto = Console.ReadLine();

            Console.WriteLine("\nIngrese el precio del producto");
            nuevoProducto.precio = double.Parse(Console.ReadLine());

            Console.WriteLine("\n¿Cuánto del producto hay actualmente?");
            nuevoProducto.cantidadDis = int.Parse(Console.ReadLine());

            Console.WriteLine("\nIngrese el tipo de producto");
            nuevoProducto.tipo = Console.ReadLine();

            Console.WriteLine("\nIngrese el nombre del producto");
            nuevoProducto.nombre = Console.ReadLine();

            guardarProducto(nuevoProducto);

        }

        static void crearVenta(Ventas nuevaVenta)
        {
            Console.WriteLine("\nIngrese el id de la nueva venta");
            nuevaVenta.idVenta = Console.ReadLine();

            Console.WriteLine("\nIngrese el tipo de pago que se utilizó en la venta, con EFE para efectivo, TAR para tarjeta y CHE para cheque");
            nuevaVenta.tpago = Console.ReadLine();

            Console.WriteLine("\nIngrese la nómina del empleado que realizó la venta");
            nuevaVenta.vendedor = Console.ReadLine();

            nuevaVenta.horaVenta = DateTime.Now.TimeOfDay;

            nuevaVenta.fechaVenta = DateTime.Now.Date; 

            Console.WriteLine("\nIngrese la cantidad vendida");
            nuevaVenta.cantidadVen = int.Parse(Console.ReadLine());

            Console.WriteLine("\nIngrese el id del pedido");
            nuevaVenta.pedido = Console.ReadLine();

            Console.WriteLine("\nEstablezca la fecha de entrega");
            nuevaVenta.fechaEnt = DateTime.Parse(Console.ReadLine());

            guardarVenta(nuevaVenta);

        }

        static void crearEmpleado(Empleados nuevoEmpleado)
        {
            Console.WriteLine("\nIngrese la nomina del nuevo empleado");
            nuevoEmpleado.nomina = Console.ReadLine();

            Console.WriteLine("\nIngrese la contraseña");
            nuevoEmpleado.contraseñaRegistro = Console.ReadLine();

            Console.WriteLine("\nIngrese el puesto del empleado");
            nuevoEmpleado.puesto = Console.ReadLine();

            Console.WriteLine("\nIngrese el nombre del empleado");
            nuevoEmpleado.nombre = Console.ReadLine();

            Console.WriteLine("\nIngrese el apellido paterno");
            nuevoEmpleado.apellidoP = Console.ReadLine();

            Console.WriteLine("\nIngrese el apellido materno");
            nuevoEmpleado.apellidoM = Console.ReadLine();

            guardarEmpleado(nuevoEmpleado);

        }

        static void crearPedido(Pedidos nuevoPedido)
        {
            Console.WriteLine("\nIngrese el id del nuevo pedido");
            nuevoPedido.idPedido = Console.ReadLine();

            Console.WriteLine("\nIngrese la nómina del empleado que consiguió el pedido");
            nuevoPedido.empleado = Console.ReadLine();

            nuevoPedido.fechaEmision = DateTime.Now.Date;

            nuevoPedido.hora = DateTime.Now.TimeOfDay;

            Console.WriteLine("\nIngrese el id del producto pedido");
            nuevoPedido.producto = Console.ReadLine();

            Console.WriteLine("\nIngrese la cantidad que se pidió del producto");
            nuevoPedido.cantidad = int.Parse(Console.ReadLine());

            nuevoPedido.venta = 0; 

            guardarPedido(nuevoPedido);

        }




        static void guardarProducto(Productos nuevoProducto)
        {
            MySqlConnection miConexion = new MySqlConnection(conexionString);
            MySqlCommand miComando = new MySqlCommand();

            try
            {
                miConexion.Open();

                miComando.Connection = miConexion;
                miComando.CommandText = "INSERT INTO Productos VALUES " +
                                        "(@idProducto,@precio,@cantidadDis,@tipo,@nombre)";

                miComando.Parameters.Add("@idProducto", MySqlDbType.VarChar).Value = nuevoProducto.idProducto;
                miComando.Parameters.Add("@precio", MySqlDbType.Double).Value = nuevoProducto.precio;
                miComando.Parameters.Add("@cantidadDis", MySqlDbType.Int16).Value = nuevoProducto.cantidadDis;
                miComando.Parameters.Add("@tipo", MySqlDbType.VarChar).Value = nuevoProducto.tipo;
                miComando.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = nuevoProducto.nombre;


                miComando.ExecuteNonQuery();

                Console.WriteLine("\nProducto almacenado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }

        }

        static void guardarVenta(Ventas nuevaVenta)
        {
            MySqlConnection miConexion = new MySqlConnection(conexionString);
            MySqlCommand miComando = new MySqlCommand();

            try
            {
                miConexion.Open();

                miComando.Connection = miConexion;
                miComando.CommandText = "INSERT INTO Ventas VALUES " +
                                        "(@idVenta,@tpago,@vendedor,@horaVenta,@fechaVenta,@cantidadVen,@pedido,@fechaEnt)";

                miComando.Parameters.Add("@idVenta", MySqlDbType.VarChar).Value = nuevaVenta.idVenta;
                miComando.Parameters.Add("@tpago", MySqlDbType.VarChar).Value = nuevaVenta.tpago;
                miComando.Parameters.Add("@vendedor", MySqlDbType.VarChar).Value = nuevaVenta.vendedor;
                miComando.Parameters.Add("@horaVenta", MySqlDbType.Time).Value = nuevaVenta.horaVenta;
                miComando.Parameters.Add("@fechaVenta", MySqlDbType.Date).Value = nuevaVenta.fechaVenta; 
                miComando.Parameters.Add("@cantidadVen", MySqlDbType.Int16).Value = nuevaVenta.cantidadVen;
                miComando.Parameters.Add("@pedido", MySqlDbType.VarChar).Value = nuevaVenta.pedido;
                miComando.Parameters.Add("@fechaEnt", MySqlDbType.Date).Value = nuevaVenta.fechaEnt;

                miComando.ExecuteNonQuery();

                Console.WriteLine("\nVenta registrada");
            }
            catch (MySqlException)
            {
                Console.WriteLine("\nError al ingresar los datos a la base"); 
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }


            try
            {
                miConexion.Open();

                miComando.Connection = miConexion;
                miComando.CommandText = "CALL ActualizarPedido(@ventaRegistrada)";

                miComando.Parameters.Add("@ventaRegistrada", MySqlDbType.VarChar).Value = nuevaVenta.idVenta;

                miComando.ExecuteNonQuery();

                Console.WriteLine("\nLa tabla de pedidos ha sido actualizada");
            }
            catch (MySqlException)
            {
                Console.WriteLine("\nError al actualizar la tabla de pedidos");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }


            try
            {
                miConexion.Open();

                miComando.Connection = miConexion;
                miComando.CommandText = "CALL ActualizarInventario(@ventaProducto)";

                miComando.Parameters.Add("@ventaProducto", MySqlDbType.VarChar).Value = nuevaVenta.idVenta;

                miComando.ExecuteNonQuery();

                Console.WriteLine("\nLa tabla de productos ha sido actualizada");
            }
            catch (MySqlException)
            {
                Console.WriteLine("\nError al actualizar la tabla de productos");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }


        }

        static void guardarEmpleado(Empleados nuevoEmpleado)
        {
            MySqlConnection miConexion = new MySqlConnection(conexionString);
            MySqlCommand miComando = new MySqlCommand();

            try
            {
                miConexion.Open();

                miComando.Connection = miConexion;
                miComando.CommandText = "INSERT INTO Empleados VALUES " +
                                        "(@nomina,@contraseñaRegistro,@puesto,@nombre,@apellidoP,@apellidoM)";

                miComando.Parameters.Add("@nomina", MySqlDbType.VarChar).Value = nuevoEmpleado.nomina;
                miComando.Parameters.Add("@contraseñaRegistro", MySqlDbType.VarChar).Value = nuevoEmpleado.contraseñaRegistro;
                miComando.Parameters.Add("@puesto", MySqlDbType.VarChar).Value = nuevoEmpleado.puesto;
                miComando.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = nuevoEmpleado.nombre;
                miComando.Parameters.Add("@apellidoP", MySqlDbType.VarChar).Value = nuevoEmpleado.apellidoP;
                miComando.Parameters.Add("@apellidoM", MySqlDbType.VarChar).Value = nuevoEmpleado.apellidoM;

                miComando.ExecuteNonQuery();

                Console.WriteLine("\nEmpleado registrado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }

        }

        static void guardarPedido(Pedidos nuevoPedido)
        {
            MySqlConnection miConexion = new MySqlConnection(conexionString);
            MySqlCommand miComando = new MySqlCommand();

            try
            {
                miConexion.Open();

                miComando.Connection = miConexion;
                miComando.CommandText = "INSERT INTO Pedidos VALUES " +
                                        "(@idPedido,@empleado,@fechaEmision,@hora,@producto,@cantidad,@venta)";

                miComando.Parameters.Add("@idPedido", MySqlDbType.VarChar).Value = nuevoPedido.idPedido;
                miComando.Parameters.Add("@empleado", MySqlDbType.VarChar).Value = nuevoPedido.empleado;
                miComando.Parameters.Add("@fechaEmision", MySqlDbType.Date).Value = nuevoPedido.fechaEmision;
                miComando.Parameters.Add("@hora", MySqlDbType.Time).Value = nuevoPedido.hora;
                miComando.Parameters.Add("@producto", MySqlDbType.VarChar).Value = nuevoPedido.producto;
                miComando.Parameters.Add("@cantidad", MySqlDbType.Int16).Value = nuevoPedido.cantidad;
                miComando.Parameters.Add("@venta", MySqlDbType.Bit).Value = nuevoPedido.venta;

                miComando.ExecuteNonQuery();

                Console.WriteLine("\nPedido registrado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }

        }




        static void LeerProductos()
        {
            List<Productos> producto = new List<Productos>();
            Productos leerProducto = null;

            MySqlConnection miConexion = new MySqlConnection(conexionString);
            MySqlCommand miComando = new MySqlCommand("SELECT * FROM Productos");
            MySqlDataReader miLector = null;


            try
            {
                miConexion.Open();
                miComando.Connection = miConexion;

                miLector = miComando.ExecuteReader();
                miLector.Read();

                do
                {
                    leerProducto = new Productos();

                    leerProducto.idProducto = miLector[0].ToString();
                    leerProducto.precio = double.Parse(miLector[1].ToString());
                    leerProducto.cantidadDis = int.Parse(miLector[2].ToString());
                    leerProducto.tipo = miLector[3].ToString();
                    leerProducto.nombre = miLector[4].ToString();

                    Productos.Add(leerProducto);
                    Console.WriteLine(leerProducto);
                }
                while (miLector.Read() == true);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }

        }

        static void LeerVentas()
        {
            List<Ventas> venta = new List<Ventas>();
            Ventas leerVenta = null;

            MySqlConnection miConexion = new MySqlConnection(conexionString);
            MySqlCommand miComando = new MySqlCommand("SELECT * FROM Ventas");
            MySqlDataReader miLector = null;


            try
            {
                miConexion.Open();
                miComando.Connection = miConexion;

                miLector = miComando.ExecuteReader();
                miLector.Read();

                do
                {
                    leerVenta = new Ventas();

                    leerVenta.idVenta = miLector[0].ToString();
                    leerVenta.tpago = miLector[1].ToString();
                    leerVenta.vendedor = miLector[2].ToString();
                    leerVenta.horaVenta = TimeSpan.Parse(miLector[3].ToString());
                    leerVenta.fechaVenta = DateTime.Parse(miLector[4].ToString());
                    leerVenta.cantidadVen = int.Parse(miLector[5].ToString());
                    leerVenta.pedido = miLector[6].ToString();
                    leerVenta.fechaEnt = DateTime.Parse(miLector[7].ToString()); 

                    Ventas.Add(leerVenta);
                    Console.WriteLine(leerVenta);
                }
                while (miLector.Read() == true);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }

        }

        static void LeerEmpleados()
        {
            List<Empleados> empleado = new List<Empleados>();
            Empleados leerEmpleado = null;

            MySqlConnection miConexion = new MySqlConnection(conexionString);
            MySqlCommand miComando = new MySqlCommand("SELECT * FROM Empleados");
            MySqlDataReader miLector = null;


            try
            {
                miConexion.Open();
                miComando.Connection = miConexion;

                miLector = miComando.ExecuteReader();
                miLector.Read();

                do
                {
                    leerEmpleado = new Empleados();

                    leerEmpleado.nomina = miLector[0].ToString();
                    leerEmpleado.puesto = miLector[2].ToString();
                    leerEmpleado.nombre = miLector[3].ToString();
                    leerEmpleado.apellidoP = miLector[4].ToString();
                    leerEmpleado.apellidoM = miLector[5].ToString();

                    Empleados.Add(leerEmpleado);
                    Console.WriteLine(leerEmpleado);
                }
                while (miLector.Read() == true);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }

        }

        static void LeerPedidos()
        {
            List<Pedidos> pedido = new List<Pedidos>();
            Pedidos leerPedido = null;

            MySqlConnection miConexion = new MySqlConnection(conexionString);
            MySqlCommand miComando = new MySqlCommand("SELECT * FROM Pedidos");
            MySqlDataReader miLector = null;


            try
            {
                miConexion.Open();
                miComando.Connection = miConexion;

                miLector = miComando.ExecuteReader();
                miLector.Read();

                do
                {
                    leerPedido = new Pedidos();

                    leerPedido.idPedido = miLector[0].ToString();
                    leerPedido.empleado = miLector[1].ToString();
                    leerPedido.fechaEmision = DateTime.Parse(miLector[2].ToString()); 
                    leerPedido.hora = TimeSpan.Parse(miLector[3].ToString());
                    leerPedido.producto = miLector[4].ToString();
                    leerPedido.cantidad = int.Parse(miLector[5].ToString());
                    leerPedido.venta = int.Parse(miLector[6].ToString()); 

                    Pedidos.Add(leerPedido);
                    Console.WriteLine(leerPedido);
                }
                while (miLector.Read() == true);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }

        }

        static void LeerVentasEmpleados(Ventas EmpleadoVen)
        {
            Console.WriteLine("\nIngrese la nómina del empleado del que va a consultar las ventas");
            EmpleadoVen.vendedor = Console.ReadLine();

            List<Ventas> ventasEmp = new List<Ventas>();
            Ventas leerVentasEmp = null;

            MySqlConnection miConexion = new MySqlConnection(conexionString);
            MySqlCommand miComando = new MySqlCommand("SELECT * FROM Ventas WHERE vendedor=@empleado");
            MySqlDataReader miLector = null;

            miComando.Parameters.Add("@empleado", MySqlDbType.VarChar).Value = EmpleadoVen.vendedor;


            try
            {
                miConexion.Open();
                miComando.Connection = miConexion;

                miLector = miComando.ExecuteReader();
                miLector.Read();

                do
                {
                    leerVentasEmp = new Ventas();

                    leerVentasEmp.idVenta = miLector[0].ToString();
                    leerVentasEmp.tpago = miLector[1].ToString();
                    leerVentasEmp.vendedor = miLector[2].ToString();
                    leerVentasEmp.horaVenta = TimeSpan.Parse(miLector[3].ToString());
                    leerVentasEmp.fechaVenta = DateTime.Parse(miLector[4].ToString());
                    leerVentasEmp.cantidadVen = int.Parse(miLector[5].ToString());
                    leerVentasEmp.pedido = miLector[6].ToString();
                    leerVentasEmp.fechaEnt = DateTime.Parse(miLector[7].ToString());

                    Ventas.Add(leerVentasEmp);
                    Console.WriteLine(leerVentasEmp);
                }
                while (miLector.Read() == true);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                miConexion.Close();
            }

        }
    


        static void probarConexion()
        {
            MySqlConnection miConexion = new MySqlConnection(conexionString);

            try
            {
                miConexion.Open();
                Console.WriteLine("\nSe ha establecido la conexión a la base");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nNo se pudo establecer la conexión\n" + ex.Message);
            }
            finally
            {
                miConexion.Close();
            }

            Console.ReadKey();
        }





    }
} 

