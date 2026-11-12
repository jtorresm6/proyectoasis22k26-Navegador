-- Ejecutar con la estructura de Seguridad ya instalada.
USE dbsistemaembutidos;
SET NAMES utf8mb4;
START TRANSACTION;

INSERT INTO tblmodulo (idModulo, nombreModulo, descripcionModulo, is_active)
VALUES (7000, 'Ventas y Cuentas por Cobrar', 'Gestión de ventas y cuentas por cobrar', 1)
ON DUPLICATE KEY UPDATE
    nombreModulo = VALUES(nombreModulo),
    descripcionModulo = VALUES(descripcionModulo),
    is_active = VALUES(is_active);

INSERT INTO tblaplicacion
    (idAplicacion, idModulo, nombreAplicacion, descripcionAplicacion, is_active)
VALUES
    (7001, 7000, 'Principal', 'Ventas y CxC: Principal', 1),
    (7002, 7000, 'Listado de precios', 'Ventas y CxC: Listado de precios', 1),
    (7003, 7000, 'Estado de cuenta', 'Ventas y CxC: Estado de cuenta', 1),
    (7004, 7000, 'Pedidos', 'Ventas y CxC: Pedidos', 1),
    (7005, 7000, 'Recepción de pagos', 'Ventas y CxC: Recepción de pagos', 1),
    (7006, 7000, 'Antigüedad de saldos', 'Ventas y CxC: Antigüedad de saldos', 1),
    (7007, 7000, 'Facturas', 'Ventas y CxC: Facturas', 1),
    (7008, 7000, 'Comisiones de vendedores', 'Ventas y CxC: Comisiones de vendedores', 1),
    (7009, 7000, 'Mantenimientos', 'Ventas y CxC: Mantenimientos', 1),
    (7010, 7000, 'Cierre de movimientos', 'Ventas y CxC: Cierre de movimientos', 1),
    (7011, 7000, 'Cotizaciones', 'Ventas y CxC: Cotizaciones', 1),
    (7012, 7000, 'Cotizaciones, pedidos y facturas', 'Ventas y CxC: Cotizaciones, pedidos y facturas', 1),
    (7013, 7000, 'Reglas y liquidación de comisiones', 'Ventas y CxC: Reglas y liquidación de comisiones', 1),
    (7014, 7000, 'Cargos y ajustes', 'Ventas y CxC: Cargos y ajustes', 1),
    (7015, 7000, 'Depósitos a Bancos', 'Ventas y CxC: Depósitos a Bancos', 1),
    (7016, 7000, 'Consulta de recibos', 'Ventas y CxC: Consulta de recibos', 1)
ON DUPLICATE KEY UPDATE
    idModulo = VALUES(idModulo),
    nombreAplicacion = VALUES(nombreAplicacion),
    descripcionAplicacion = VALUES(descripcionAplicacion),
    is_active = VALUES(is_active);

COMMIT;
SELECT idModulo, nombreModulo FROM tblmodulo WHERE idModulo = 7000;
SELECT idAplicacion, nombreAplicacion FROM tblaplicacion WHERE idModulo = 7000 ORDER BY idAplicacion;
