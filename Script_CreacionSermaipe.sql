CREATE DATABASE SERMAIPE;

USE SERMAIPE;

CREATE SCHEMA PERSONA;
CREATE SCHEMA INSUMO;
CREATE SCHEMA ADMINISTRACION;
---------------------------------------------------------
CREATE TABLE PERSONA.CLIENTE
(
	idCliente bigint IDENTITY(1,1) not null,
	nombres varchar(80) not null,
	apellidoP varchar(50) not null,
	apellidoM varchar(50) not null,
	domicilio varchar(200) not null,
	telefono varchar(7) not null,
	celular varchar(10) not null,
	credito float not null,
	CONSTRAINT PK_IDCLIENTE Primary Key(idCliente)
);
CREATE TABLE PERSONA.EMPLEADO
(
	idEmpleado bigint IDENTITY(1,1) not null,
	nombres varchar(80) not null,
	apellidoP varchar(50) not null,
	apellidoM varchar(50) not null,
	domicilio varchar(200) not null,
	telefono varchar(7) not null,
	celular varchar(10) not null,
	sueldo float not null,
	CONSTRAINT PK_IDEMPLEADO Primary Key(idEmpleado)
);
CREATE TABLE PERSONA.PROVEEDOR
(
	idProveedor bigint IDENTITY(1,1) not null,
	nombres varchar(80) not null,
	apellidoP varchar(50) not null,
	apellidoM varchar(50) not null,
	domicilio varchar(200) not null,
	telefono varchar(7) not null,
	celular varchar(10) not null,
	email varchar(50) not null,
	CONSTRAINT PK_IDPROVEEDOR Primary Key(idProveedor)
);
---------------------------------------------------------
CREATE TABLE INSUMO.MATERIAL
(
	idMaterial bigint IDENTITY(1,1) not null,
	idProveedor bigint not null,
	nombre varchar(50) not null,
	descripcion varchar(100) not null,
	stock int not null,
	precio_venta float not null,
	precio_compra float not null,
	CONSTRAINT PK_IDMATERIAL Primary Key(idMaterial),
	CONSTRAINT FK_ID_PROVEEDOR Foreign Key(idProveedor) REFERENCES PERSONA.PROVEEDOR(idProveedor)
);
---------------------------------------------------------
CREATE TABLE ADMINISTRACION.SERVICIO
(
	idServicio bigint IDENTITY(1,1) not null,
	descripcion varchar(200) not null,
	tipoServicio varchar(20) not null,
	costo float not null,

	CONSTRAINT PK_IDSERVICIO Primary Key(idServicio)
);


CREATE TABLE ADMINISTRACION.SERVICIO_EMPLEADO
(
	idServicio bigint not null,
	idEmpleado bigint not null
	CONSTRAINT PK_SERVICIO_EMPLEADO Primary Key(idServicio, idEmpleado),
	CONSTRAINT FK_ID_SERVICIO Foreign Key(idServicio) REFERENCES ADMINISTRACION.SERVICIO(idServicio),
	CONSTRAINT FK_ID_EMPLEADO Foreign Key(idEmpleado) REFERENCES PERSONA.EMPLEADO(idEmpleado)
);
CREATE TABLE ADMINISTRACION.PEDIDO
(
	idPedido bigint IDENTITY(1,1) not null,
	idCliente bigint not null,
	fechaPedido date not null,
	fechaEntrega date not null,
	cargoServicio float not null,
	total float not null,
	CONSTRAINT PK_IDPEDIDO Primary key(idPedido),
	CONSTRAINT FK_ID_CLIENTE Foreign Key(idCliente) REFERENCES PERSONA.CLIENTE(idCliente)
);
CREATE TABLE ADMINISTRACION.DETALLE_PEDIDO
(
	idPedido bigint not null,
	idMaterial bigint not null,
	cantidad int not null,
	idServicio bigint not null,
	subtotal float not null
	CONSTRAINT FK_ID_PMATERIAL Foreign Key(idMaterial) REFERENCES INSUMO.MATERIAL(idMaterial),
	CONSTRAINT FK_ID_PSERVICIO Foreign Key(idServicio) REFERENCES ADMINISTRACION.SERVICIO(idServicio)
);
CREATE TABLE ADMINISTRACION.FACTURA
(
	idPedido bigint not null,
	idEmpleado bigint not null,
	fecha_factura date not null,
	total_factura float not null,
	tipoPago varchar(30) not null,

	CONSTRAINT PK_IDFPEDIDO Primary Key(idPedido),
	CONSTRAINT FK_IDFPEDIDO Foreign Key(idPedido) REFERENCES ADMINISTRACION.PEDIDO(idPedido),
	CONSTRAINT FK_ID_EMPLEADO2 Foreign Key(idEmpleado) REFERENCES PERSONA.EMPLEADO(idEmpleado)
);
--------------------REGLAS-----------------------------------------------------------------
--Agregar regla para que solo acepte: 'Instalacion','Mantenimiento','Reparacion' en Servicio
CREATE RULE TIPO_SERVICIO AS @var IN ('Instalacion', 'Mantenimiento', 'Reparacion')
EXEC sp_bindrule 'TIPO_SERVICIO','ADMINISTRACION.SERVICIO.tipoServicio'

--Agregar para que solo acepte: 'Efectivo','Tarjeta de credito' ... etc en FACTURA
CREATE RULE TIPO_PAGO AS @var IN ('Efectivo','Tarjeta de debito','Tarjeta de credito', 'Cheque', 'Transferencia electronica')
EXEC sp_bindrule 'TIPO_PAGO','ADMINISTRACION.FACTURA.tipoPago'

--------------------TRIGGERS(PEDIDO_ACTUALIZA_MATERIAL)---------------------------------------------------
CREATE TRIGGER PEDIDO_MATERIAL ON ADMINISTRACION.DETALLE_PEDIDO
FOR INSERT
AS
	DECLARE @stock int
	SELECT @stock = M.stock 
	FROM INSUMO.MATERIAL M, INSERTED I
	WHERE M.idMaterial = I.idMaterial
	
	if(@stock >= (SELECT cantidad FROM INSERTED))
		UPDATE M set stock = stock-I.cantidad
		FROM INSUMO.MATERIAL M, INSERTED I
		WHERE M.idMaterial=I.idMaterial
	else
		begin
			raiserror('No cuenta con suficiente stock para este material',16,1)
			rollback transaction --Revierte los cambios que se han hecho en la tabla
		end
------------------------------------------------------------------------------------------------
INSERT INTO ADMINISTRACION.SERVICIO(descripcion, costo, tipoServicio) VALUES('NULL', 0, 'INSTALACION');
SELECT * FROM ADMINISTRACION.SERVICIO;
DBCC CHECKIDENT('ADMINISTRACION.SERVICIO', RESEED, 0);

INSERT INTO PERSONA.PROVEEDOR(nombres, apellidoP, apellidoM, domicilio, telefono, celular, email)
VALUES('LUIS GERARDO', 'SANTOS', 'MIRANDA', 'PAVO REAL_212_HOGARES POPULARES', '8098562', '4443189483', 'lgerardosm93@gmail.com')
INSERT INTO INSUMO.MATERIAL(idProveedor, nombre, descripcion, stock, precio_compra, precio_venta)
VALUES(1, 'NULL', 'NULL', 0, 0, 0);
SELECT * FROM INSUMO.MATERIAL;
--------------------TRIGGERS(INSERT CALCULA TOTAL)---------------------------------------------------
CREATE TRIGGER PEDIDO_CALCULA_TOTAL ON ADMINISTRACION.DETALLE_PEDIDO
FOR INSERT
AS
	UPDATE P set P.total = P.total + (I.cantidad*M.precio_venta) + (S.costo)
	FROM ADMINISTRACION.PEDIDO P, INSUMO.MATERIAL M, ADMINISTRACION.SERVICIO S, inserted I
	WHERE P.idPedido = I.idPedido AND
	I.idMaterial = M.idMaterial AND
	I.idServicio = S.idServicio
---------------------------------------------------------------------------------------------------
SELECT * FROM PERSONA.CLIENTE;
INSERT INTO PERSONA.CLIENTE(nombres, apellidoP, apellidoM, domicilio, telefono, celular, credito)
VALUES ('Jesus Carlos', 'Garcia', 'Ortega', 'Gorrion 138', '8149043', '4443189333', 3000);

SELECT * FROM ADMINISTRACION.PEDIDO;
INSERT INTO ADMINISTRACION.PEDIDO(idCliente, fechaPedido, fechaEntrega, total) values(1, '2017-03-28','2017-03-29', 0);
--------------------TRIGGERS(DECREMENTAR CREDITO-PENDIENTE)---------------------------------------------------
CREATE TRIGGER PEDIDO_DECREMENTA_CREDITO ON ADMINISTRACION.DETALLE_PEDIDO
FOR INSERT
AS
	UPDATE C set C.credito = C.credito - (I.cantidad*M.precio_venta) - (S.costo)
	FROM ADMINISTRACION.PEDIDO P, PERSONA.CLIENTE C,INSUMO.MATERIAL M, ADMINISTRACION.SERVICIO S, inserted I
	WHERE I.idPedido = P.idPedido AND
	P.idCliente = C.idCliente AND
	I.idMaterial = M.idMaterial AND
	I.idServicio = S.idServicio

-------------------TRIGGERS(ACTUALIZAR_CREDITO[ELIMINA MATERIAL])---------------------------------
CREATE TRIGGER INCREMENTAR_CREDITO ON ADMINISTRACION.DETALLE_PEDIDO
FOR DELETE
AS
	UPDATE P set P.total = P.total - (D.cantidad*M.precio_venta) - (S.costo)
	FROM ADMINISTRACION.PEDIDO P, INSUMO.MATERIAL M, ADMINISTRACION.SERVICIO S, deleted D
	WHERE P.idPedido = D.idPedido AND
	D.idMaterial = M.idMaterial AND
	D.idServicio = S.idServicio

-------------------TRIGGERS(INCREMENTAR_STOCK[ELIMINA MATERIAL])----------------------------------
CREATE TRIGGER INCREMENTAR_STOCK ON ADMINISTRACION.DETALLE_PEDIDO
FOR DELETE
AS
	UPDATE M set M.stock = M.stock + D.cantidad
	FROM INSUMO.MATERIAL M, deleted D
	WHERE D.idMaterial = M.idMaterial

-------------------TRIGGERS(ACTUALIZA_TOTAL[ELIMINA MATERIAL-SERVICIO])------------------------------------
CREATE TRIGGER PEDIDO_RECALCULA_TOTAL ON ADMINISTRACION.DETALLE_PEDIDO
FOR DELETE
AS
	UPDATE P set P.total = P.total - (D.cantidad*M.precio_venta) - (S.costo)
	FROM ADMINISTRACION.PEDIDO P, INSUMO.MATERIAL M, ADMINISTRACION.SERVICIO S, deleted D
	WHERE P.idPedido = D.idPedido AND
	D.idMaterial = M.idMaterial AND
	D.idServicio = S.idServicio

--------------------------------------------------------------------------------------------------
SELECT * FROM INSUMO.MATERIAL;
INSERT INTO INSUMO.MATERIAL(idProveedor, nombre, descripcion, stock, precio_venta, precio_compra)
VALUES(1, 'Tronillo 3/4', 'Tornillo metálico', 200, 30, 18);
SELECT * FROM PERSONA.PROVEEDOR;

SELECT * FROM ADMINISTRACION.SERVICIO;
INSERT INTO ADMINISTRACION.SERVICIO(descripcion, tipoServicio, costo)
VALUES('INSTALACION DE RACKS', 'INSTALACION', 300);



SELECT * FROM ADMINISTRACION.PEDIDO;
--INSERTAR DP_SERVICIO
INSERT INTO ADMINISTRACION.DETALLE_PEDIDO(idPedido, idMaterial, cantidad, idServicio, subtotal) 
VALUES(1, 1, 0, 2, 0);
--INSERTAR DP_MATERIAL
INSERT INTO ADMINISTRACION.DETALLE_PEDIDO(idPedido, idMaterial, cantidad, idServicio, subtotal) 
VALUES(1, 2, 5, 1, 0);
INSERT INTO ADMINISTRACION.DETALLE_PEDIDO(idPedido, idMaterial, cantidad, idServicio, subtotal) 
VALUES(1, 2, 3, 1, 0);
INSERT INTO ADMINISTRACION.DETALLE_PEDIDO(idPedido, idMaterial, cantidad, idServicio, subtotal) 
VALUES(1, 2, 2, 1, 0);
SELECT * FROM ADMINISTRACION.DETALLE_PEDIDO;