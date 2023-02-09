CREATE DATABASE DeliboInv3; 
USE DeliboInv3; 

CREATE TABLE Empleados (
	nomina VARCHAR(10)
    ,contraseñaRegistro VARCHAR(100)
    ,puesto VARCHAR(30)
    ,nombre VARCHAR(100) 
    ,apellidoP VARCHAR(100) 
    ,apellidoM VARCHAR(100) 
    ,PRIMARY KEY(nomina)
); 

CREATE TABLE Productos (
	idProducto VARCHAR(10)
    ,precio    INT 
    ,cantidadDis INT
    ,tipo VARCHAR(30) 
    ,nombre VARCHAR(100) 
    ,PRIMARY KEY(idProducto)
); 

CREATE TABLE Pedidos (
	idPedido VARCHAR(10) 
    ,empleado VARCHAR(10)
    ,fechaEmision DATE
    ,hora TIME 
    ,producto VARCHAR(10) 
    ,cantidad INT 
    ,venta BIT 
    ,PRIMARY KEY(idPedido)
    ,FOREIGN KEY(empleado) REFERENCES Empleados(nomina)
    ,FOREIGN KEY(producto) REFERENCES Productos(idProducto)
); 

CREATE TABLE Ventas (
	idVenta VARCHAR(10)
    ,tpago  VARCHAR(3) CHECK(tpago IN ('EFE', 'CHE', 'TAR'))
    ,vendedor VARCHAR(10) 
    ,horaVenta TIME 
    ,fechaVenta DATE
    ,cantidadVen INT 
    ,pedido VARCHAR(10) 
    ,fechaEnt DATE
    ,PRIMARY KEY(idVenta)
    ,FOREIGN KEY(vendedor) REFERENCES Empleados(nomina)
    ,FOREIGN KEY(pedido) REFERENCES Pedidos(idPedido) 
); 



DELIMITER // 
CREATE PROCEDURE ActualizarPedido (IN ventaRegistrada VARCHAR(10))
BEGIN 
	DECLARE pedidoAc VARCHAR(10);  
    
	SELECT pedido 
		INTO pedidoAc
		FROM Ventas
		WHERE idVenta = ventaRegistrada;   
	
    UPDATE Pedidos
		SET venta=1 WHERE idPedido=pedidoAc; 
	   
END //  
DELIMITER ; 


DELIMITER //
CREATE PROCEDURE ActualizarInventario (IN ventaProducto VARCHAR(10)) 
BEGIN 
	DECLARE pedidoPro VARCHAR(10);  
    DECLARE productoAc VARCHAR(10); 
    DECLARE cantidadVendida INT; 
    
   SELECT cantidadVen 
		INTO cantidadVendida 
        FROM Ventas 
        WHERE idVenta=ventaProducto; 
        
   SELECT pedido 
		INTO pedidoPro
        FROM Ventas 
        WHERE idVenta= ventaProducto; 

    SELECT producto
		INTO productoAc 
        FROM Pedidos 
        WHERE idPedido = PedidoPro;
        
	UPDATE Productos 
		SET cantidadDis= cantidadDis - cantidadVendida WHERE idProducto = productoAc; 
END//
DELIMITER ; 

SELECT * FROM Empleados; 



