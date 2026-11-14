-- ============================================================================
-- TERMINUS ERP - LOGISTICA - DATOS DE PRUEBA
-- 10 REGISTROS POR TABLA - IDENTIFICADORES 1 AL 10
-- Ejecutar DESPUES del script de creacion dbLogistica(2).sql
-- NOTA: tblEstadoSolicitudPago ya contiene el registro 1 en el script base.
-- Este archivo inserta los registros 2 al 10 de esa tabla.
-- ============================================================================
USE dbLogistica;
START TRANSACTION;

-- ============================================================================
-- TABLA: tblCategoria
-- ============================================================================
-- Registro 1
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (1, 'Embutidos', 0.10);

-- Registro 2
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (2, 'Carnes frías', 0.10);

-- Registro 3
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (3, 'Ahumados', 0.10);

-- Registro 4
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (4, 'Productos frescos', 0.10);

-- Registro 5
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (5, 'Procesados', 0.10);

-- Registro 6
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (6, 'Especialidades', 0.10);

-- Registro 7
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (7, 'Congelados', 0.10);

-- Registro 8
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (8, 'Premium', 0.10);

-- Registro 9
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (9, 'Parrilla', 0.10);

-- Registro 10
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (10, 'Deli', 0.10);

-- ============================================================================
-- TABLA: tblEmpresaTransportista
-- ============================================================================
-- Registro 1
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (1, 'Transporte Maya', 'Ana López', '55500001');

-- Registro 2
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (2, 'Carga Chapina', 'Luis Morales', '55500002');

-- Registro 3
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (3, 'Frío Express', 'María García', '55500003');

-- Registro 4
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (4, 'Ruta Segura', 'Carlos Pérez', '55500004');

-- Registro 5
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (5, 'Logística Quetzal', 'Elena Ruiz', '55500005');

-- Registro 6
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (6, 'Transportes del Valle', 'José Ramírez', '55500006');

-- Registro 7
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (7, 'Carga Regional', 'Sofía Herrera', '55500007');

-- Registro 8
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (8, 'Movilidad Centro', 'Pedro Castillo', '55500008');

-- Registro 9
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (9, 'Fletes Progreso', 'Laura Díaz', '55500009');

-- Registro 10
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (10, 'Cadena Fría GT', 'Jorge Méndez', '55500010');

-- ============================================================================
-- TABLA: tblEstado
-- ============================================================================
-- Registro 1
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (1, 'ACTIVO');

-- Registro 2
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (2, 'INACTIVO');

-- Registro 3
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (3, 'EN_MANTENIMIENTO');

-- Registro 4
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (4, 'DISPONIBLE');

-- Registro 5
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (5, 'OCUPADO');

-- Registro 6
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (6, 'SUSPENDIDO');

-- Registro 7
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (7, 'EN_RUTA');

-- Registro 8
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (8, 'RESERVADO');

-- Registro 9
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (9, 'REPARACION');

-- Registro 10
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (10, 'BAJA');

-- ============================================================================
-- TABLA: tblEstadoSolicitudPago
-- ============================================================================
-- Registro 2
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (2, 'APROBADO', 'Aprobado', 'Estado de pago: APROBADO');

-- Registro 3
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (3, 'RECHAZADO', 'Rechazado', 'Estado de pago: RECHAZADO');

-- Registro 4
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (4, 'PENDIENTE', 'Pendiente', 'Estado de pago: PENDIENTE');

-- Registro 5
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (5, 'PAGADO', 'Pagado', 'Estado de pago: PAGADO');

-- Registro 6
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (6, 'EN_REVISION', 'En Revision', 'Estado de pago: EN_REVISION');

-- Registro 7
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (7, 'PROGRAMADO', 'Programado', 'Estado de pago: PROGRAMADO');

-- Registro 8
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (8, 'CANCELADO', 'Cancelado', 'Estado de pago: CANCELADO');

-- Registro 9
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (9, 'PROCESADO', 'Procesado', 'Estado de pago: PROCESADO');

-- Registro 10
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (10, 'FINALIZADO', 'Finalizado', 'Estado de pago: FINALIZADO');

-- ============================================================================
-- TABLA: tblLinea
-- ============================================================================
-- Registro 1
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (1, 'Tradicional', 0.10);

-- Registro 2
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (2, 'Familiar', 0.10);

-- Registro 3
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (3, 'Premium', 0.10);

-- Registro 4
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (4, 'Económica', 0.10);

-- Registro 5
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (5, 'Selección', 0.10);

-- Registro 6
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (6, 'Delicatessen', 0.10);

-- Registro 7
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (7, 'Parrillera', 0.10);

-- Registro 8
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (8, 'Profesional', 0.10);

-- Registro 9
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (9, 'Especial', 0.10);

-- Registro 10
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (10, 'Institucional', 0.10);

-- ============================================================================
-- TABLA: tblMarca
-- ============================================================================
-- Registro 1
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (1, 'La Hacienda', 0.10);

-- Registro 2
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (2, 'Del Campo', 0.10);

-- Registro 3
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (3, 'Buen Sabor', 0.10);

-- Registro 4
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (4, 'La Granja', 0.10);

-- Registro 5
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (5, 'Don Chorizo', 0.10);

-- Registro 6
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (6, 'Rey del Jamón', 0.10);

-- Registro 7
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (7, 'Fresco GT', 0.10);

-- Registro 8
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (8, 'Casa Selecta', 0.10);

-- Registro 9
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (9, 'Monte Verde', 0.10);

-- Registro 10
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (10, 'Sabor Real', 0.10);

-- ============================================================================
-- TABLA: tblProveedor
-- ============================================================================
-- Registro 1
INSERT INTO tblProveedor (idProveedor, nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, estadoProveedor, saldoActualProveedor, saldoAnteriorProveedor, cargosMesProveedor, abonosMesProveedor, cargosAsignadosProveedor, cargosAcumuladosProveedor, abonosAcumuladosProveedor)
VALUES (1, 'Distribuidora Central', '100000001K', 'proveedor1@ejemplo.com', '55500001', 'Zona 1, Ciudad de Guatemala', 1, 20.0000, 20.0000, 20.0000, 20.0000, 20.0000, 20.0000, 20.0000);

-- Registro 2
INSERT INTO tblProveedor (idProveedor, nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, estadoProveedor, saldoActualProveedor, saldoAnteriorProveedor, cargosMesProveedor, abonosMesProveedor, cargosAsignadosProveedor, cargosAcumuladosProveedor, abonosAcumuladosProveedor)
VALUES (2, 'Abastos del Norte', '100000002K', 'proveedor2@ejemplo.com', '55500002', 'Zona 2, Ciudad de Guatemala', 1, 40.0000, 40.0000, 40.0000, 40.0000, 40.0000, 40.0000, 40.0000);

-- Registro 3
INSERT INTO tblProveedor (idProveedor, nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, estadoProveedor, saldoActualProveedor, saldoAnteriorProveedor, cargosMesProveedor, abonosMesProveedor, cargosAsignadosProveedor, cargosAcumuladosProveedor, abonosAcumuladosProveedor)
VALUES (3, 'Carnes del Valle', '100000003K', 'proveedor3@ejemplo.com', '55500003', 'Zona 3, Ciudad de Guatemala', 1, 60.0000, 60.0000, 60.0000, 60.0000, 60.0000, 60.0000, 60.0000);

-- Registro 4
INSERT INTO tblProveedor (idProveedor, nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, estadoProveedor, saldoActualProveedor, saldoAnteriorProveedor, cargosMesProveedor, abonosMesProveedor, cargosAsignadosProveedor, cargosAcumuladosProveedor, abonosAcumuladosProveedor)
VALUES (4, 'Frigoríficos Guatemala', '100000004K', 'proveedor4@ejemplo.com', '55500004', 'Zona 4, Ciudad de Guatemala', 1, 80.0000, 80.0000, 80.0000, 80.0000, 80.0000, 80.0000, 80.0000);

-- Registro 5
INSERT INTO tblProveedor (idProveedor, nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, estadoProveedor, saldoActualProveedor, saldoAnteriorProveedor, cargosMesProveedor, abonosMesProveedor, cargosAsignadosProveedor, cargosAcumuladosProveedor, abonosAcumuladosProveedor)
VALUES (5, 'Empacadora Maya', '100000005K', 'proveedor5@ejemplo.com', '55500005', 'Zona 5, Ciudad de Guatemala', 1, 100.0000, 100.0000, 100.0000, 100.0000, 100.0000, 100.0000, 100.0000);

-- Registro 6
INSERT INTO tblProveedor (idProveedor, nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, estadoProveedor, saldoActualProveedor, saldoAnteriorProveedor, cargosMesProveedor, abonosMesProveedor, cargosAsignadosProveedor, cargosAcumuladosProveedor, abonosAcumuladosProveedor)
VALUES (6, 'Provisiones del Sur', '100000006K', 'proveedor6@ejemplo.com', '55500006', 'Zona 6, Ciudad de Guatemala', 1, 120.0000, 120.0000, 120.0000, 120.0000, 120.0000, 120.0000, 120.0000);

-- Registro 7
INSERT INTO tblProveedor (idProveedor, nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, estadoProveedor, saldoActualProveedor, saldoAnteriorProveedor, cargosMesProveedor, abonosMesProveedor, cargosAsignadosProveedor, cargosAcumuladosProveedor, abonosAcumuladosProveedor)
VALUES (7, 'Productos Frescos SA', '100000007K', 'proveedor7@ejemplo.com', '55500007', 'Zona 7, Ciudad de Guatemala', 1, 140.0000, 140.0000, 140.0000, 140.0000, 140.0000, 140.0000, 140.0000);

-- Registro 8
INSERT INTO tblProveedor (idProveedor, nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, estadoProveedor, saldoActualProveedor, saldoAnteriorProveedor, cargosMesProveedor, abonosMesProveedor, cargosAsignadosProveedor, cargosAcumuladosProveedor, abonosAcumuladosProveedor)
VALUES (8, 'Suministros del Este', '100000008K', 'proveedor8@ejemplo.com', '55500008', 'Zona 8, Ciudad de Guatemala', 1, 160.0000, 160.0000, 160.0000, 160.0000, 160.0000, 160.0000, 160.0000);

-- Registro 9
INSERT INTO tblProveedor (idProveedor, nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, estadoProveedor, saldoActualProveedor, saldoAnteriorProveedor, cargosMesProveedor, abonosMesProveedor, cargosAsignadosProveedor, cargosAcumuladosProveedor, abonosAcumuladosProveedor)
VALUES (9, 'Mayorista Regional', '100000009K', 'proveedor9@ejemplo.com', '55500009', 'Zona 9, Ciudad de Guatemala', 1, 180.0000, 180.0000, 180.0000, 180.0000, 180.0000, 180.0000, 180.0000);

-- Registro 10
INSERT INTO tblProveedor (idProveedor, nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, estadoProveedor, saldoActualProveedor, saldoAnteriorProveedor, cargosMesProveedor, abonosMesProveedor, cargosAsignadosProveedor, cargosAcumuladosProveedor, abonosAcumuladosProveedor)
VALUES (10, 'Alimentos Selectos', '100000010K', 'proveedor10@ejemplo.com', '55500010', 'Zona 10, Ciudad de Guatemala', 1, 200.0000, 200.0000, 200.0000, 200.0000, 200.0000, 200.0000, 200.0000);

-- ============================================================================
-- TABLA: tblSucursal
-- ============================================================================
-- Registro 1
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (1, 'Sucursal Central', 'Descripcion de prueba 1 - tblSucursal', 'Zona 1, Ciudad de Guatemala', 'Guatemala');

-- Registro 2
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (2, 'Sucursal Norte', 'Descripcion de prueba 2 - tblSucursal', 'Zona 2, Ciudad de Guatemala', 'Guatemala');

-- Registro 3
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (3, 'Sucursal Sur', 'Descripcion de prueba 3 - tblSucursal', 'Zona 3, Ciudad de Guatemala', 'Guatemala');

-- Registro 4
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (4, 'Sucursal Occidente', 'Descripcion de prueba 4 - tblSucursal', 'Zona 4, Ciudad de Guatemala', 'Guatemala');

-- Registro 5
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (5, 'Sucursal Oriente', 'Descripcion de prueba 5 - tblSucursal', 'Zona 5, Ciudad de Guatemala', 'Guatemala');

-- Registro 6
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (6, 'Sucursal Metropolitana', 'Descripcion de prueba 6 - tblSucursal', 'Zona 6, Ciudad de Guatemala', 'Guatemala');

-- Registro 7
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (7, 'Sucursal Carretera', 'Descripcion de prueba 7 - tblSucursal', 'Zona 7, Ciudad de Guatemala', 'Guatemala');

-- Registro 8
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (8, 'Sucursal Industrial', 'Descripcion de prueba 8 - tblSucursal', 'Zona 8, Ciudad de Guatemala', 'Guatemala');

-- Registro 9
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (9, 'Sucursal Zona 10', 'Descripcion de prueba 9 - tblSucursal', 'Zona 9, Ciudad de Guatemala', 'Guatemala');

-- Registro 10
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (10, 'Sucursal Zona 12', 'Descripcion de prueba 10 - tblSucursal', 'Zona 10, Ciudad de Guatemala', 'Guatemala');

-- ============================================================================
-- TABLA: tblTarifaTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (1, 'Tarifa 01 - regional');

-- Registro 2
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (2, 'Tarifa 02 - refrigerado');

-- Registro 3
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (3, 'Tarifa 03 - local');

-- Registro 4
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (4, 'Tarifa 04 - regional');

-- Registro 5
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (5, 'Tarifa 05 - refrigerado');

-- Registro 6
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (6, 'Tarifa 06 - local');

-- Registro 7
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (7, 'Tarifa 07 - regional');

-- Registro 8
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (8, 'Tarifa 08 - refrigerado');

-- Registro 9
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (9, 'Tarifa 09 - local');

-- Registro 10
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (10, 'Tarifa 10 - regional');

-- ============================================================================
-- TABLA: tblTipoMovimiento
-- ============================================================================
-- Registro 1
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (1, 'Compra', 'Descripcion de prueba 1 - tblTipoMovimiento', '+');

-- Registro 2
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (2, 'Venta', 'Descripcion de prueba 2 - tblTipoMovimiento', '-');

-- Registro 3
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (3, 'Devolución compra', 'Descripcion de prueba 3 - tblTipoMovimiento', '-');

-- Registro 4
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (4, 'Devolución venta', 'Descripcion de prueba 4 - tblTipoMovimiento', '+');

-- Registro 5
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (5, 'Entrada por ajuste', 'Descripcion de prueba 5 - tblTipoMovimiento', '+');

-- Registro 6
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (6, 'Salida por ajuste', 'Descripcion de prueba 6 - tblTipoMovimiento', '-');

-- Registro 7
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (7, 'Entrada por traslado', 'Descripcion de prueba 7 - tblTipoMovimiento', '+');

-- Registro 8
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (8, 'Salida por traslado', 'Descripcion de prueba 8 - tblTipoMovimiento', '-');

-- Registro 9
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (9, 'Ajuste positivo', 'Descripcion de prueba 9 - tblTipoMovimiento', '+');

-- Registro 10
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (10, 'Ajuste negativo', 'Descripcion de prueba 10 - tblTipoMovimiento', '-');

-- ============================================================================
-- TABLA: tblTipoServicio
-- ============================================================================
-- Registro 1
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (1, 'Carga refrigerada');

-- Registro 2
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (2, 'Carga congelada');

-- Registro 3
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (3, 'Carga seca');

-- Registro 4
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (4, 'Distribución');

-- Registro 5
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (5, 'Entrega urbana');

-- Registro 6
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (6, 'Carga pesada');

-- Registro 7
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (7, 'Transporte dedicado');

-- Registro 8
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (8, 'Reparto local');

-- Registro 9
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (9, 'Carga consolidada');

-- Registro 10
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (10, 'Ruta especial');

-- ============================================================================
-- TABLA: tblTipoTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (1, 'Camión');

-- Registro 2
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (2, 'Camioneta');

-- Registro 3
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (3, 'Furgón');

-- Registro 4
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (4, 'Pick-up');

-- Registro 5
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (5, 'Panel');

-- Registro 6
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (6, 'Tráiler');

-- Registro 7
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (7, 'Cabezal');

-- Registro 8
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (8, 'Motocicleta');

-- Registro 9
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (9, 'Microbús');

-- Registro 10
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (10, 'Camión refrigerado');

-- ============================================================================
-- TABLA: tblBodega
-- ============================================================================
-- Registro 1
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (1, 'Bodega Central', 1, 'Guatemala');

-- Registro 2
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (2, 'Bodega Norte', 2, 'Guatemala');

-- Registro 3
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (3, 'Bodega Sur', 3, 'Guatemala');

-- Registro 4
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (4, 'Bodega Occidente', 4, 'Guatemala');

-- Registro 5
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (5, 'Bodega Oriente', 5, 'Guatemala');

-- Registro 6
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (6, 'Bodega Metropolitana', 6, 'Guatemala');

-- Registro 7
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (7, 'Bodega Carretera', 7, 'Guatemala');

-- Registro 8
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (8, 'Bodega Industrial', 8, 'Guatemala');

-- Registro 9
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (9, 'Bodega Zona 10', 9, 'Guatemala');

-- Registro 10
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (10, 'Bodega Zona 12', 10, 'Guatemala');

-- ============================================================================
-- TABLA: tblCierreMovimiento
-- ============================================================================
-- Registro 1
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (1, 'Cierre octubre 1', 1, '2026-01-01', '2026-01-31', 'CERRADO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 1', '2026-11-01 10:00:00');

-- Registro 2
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (2, 'Cierre octubre 2', 2, '2026-02-01', '2026-02-28', 'ABIERTO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 2', NULL);

-- Registro 3
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (3, 'Cierre octubre 3', 3, '2026-03-01', '2026-03-31', 'CERRADO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 3', '2026-11-01 10:00:00');

-- Registro 4
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (4, 'Cierre octubre 4', 4, '2026-04-01', '2026-04-30', 'ABIERTO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 4', NULL);

-- Registro 5
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (5, 'Cierre octubre 5', 5, '2026-05-01', '2026-05-31', 'CERRADO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 5', '2026-11-01 10:00:00');

-- Registro 6
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (6, 'Cierre octubre 6', 6, '2026-06-01', '2026-06-30', 'ABIERTO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 6', NULL);

-- Registro 7
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (7, 'Cierre octubre 7', 7, '2026-07-01', '2026-07-31', 'CERRADO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 7', '2026-11-01 10:00:00');

-- Registro 8
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (8, 'Cierre octubre 8', 8, '2026-08-01', '2026-08-31', 'ABIERTO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 8', NULL);

-- Registro 9
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (9, 'Cierre octubre 9', 9, '2026-09-01', '2026-09-30', 'CERRADO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 9', '2026-11-01 10:00:00');

-- Registro 10
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (10, 'Cierre octubre 10', 10, '2026-10-01', '2026-10-31', 'ABIERTO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 10', NULL);

-- ============================================================================
-- TABLA: tblConductor
-- ============================================================================
-- Registro 1
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (1, 'COND-001', 'Ana López', 'LIC-0001', '2028-12-31', 1);

-- Registro 2
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (2, 'COND-002', 'Luis Morales', 'LIC-0002', '2028-12-31', 2);

-- Registro 3
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (3, 'COND-003', 'María García', 'LIC-0003', '2028-12-31', 3);

-- Registro 4
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (4, 'COND-004', 'Carlos Pérez', 'LIC-0004', '2028-12-31', 4);

-- Registro 5
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (5, 'COND-005', 'Elena Ruiz', 'LIC-0005', '2028-12-31', 5);

-- Registro 6
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (6, 'COND-006', 'José Ramírez', 'LIC-0006', '2028-12-31', 6);

-- Registro 7
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (7, 'COND-007', 'Sofía Herrera', 'LIC-0007', '2028-12-31', 7);

-- Registro 8
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (8, 'COND-008', 'Pedro Castillo', 'LIC-0008', '2028-12-31', 8);

-- Registro 9
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (9, 'COND-009', 'Laura Díaz', 'LIC-0009', '2028-12-31', 9);

-- Registro 10
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (10, 'COND-010', 'Jorge Méndez', 'LIC-0010', '2028-12-31', 10);

-- ============================================================================
-- TABLA: tblProducto
-- ============================================================================
-- Registro 1
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (1, 'EMB-0001', 'EMB01', 'Jamón de pavo', 1, 1, 1, 1, 'Descripcion de prueba 1 - tblProducto', X'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 2
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (2, 'EMB-0002', 'EMB01', 'Salchicha tipo viena', 2, 2, 2, 2, 'Descripcion de prueba 2 - tblProducto', X'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 3
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (3, 'EMB-0003', 'EMB01', 'Chorizo tradicional', 3, 3, 3, 3, 'Descripcion de prueba 3 - tblProducto', X'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 4
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (4, 'EMB-0004', 'EMB01', 'Tocino ahumado', 4, 4, 4, 4, 'Descripcion de prueba 4 - tblProducto', X'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 5
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (5, 'EMB-0005', 'EMB01', 'Mortadela especial', 5, 5, 5, 5, 'Descripcion de prueba 5 - tblProducto', X'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 6
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (6, 'EMB-0006', 'EMB01', 'Salami premium', 6, 6, 6, 6, 'Descripcion de prueba 6 - tblProducto', X'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 7
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (7, 'EMB-0007', 'EMB01', 'Jamón cocido', 7, 7, 7, 7, 'Descripcion de prueba 7 - tblProducto', X'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 8
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (8, 'EMB-0008', 'EMB01', 'Longaniza fresca', 8, 8, 8, 8, 'Descripcion de prueba 8 - tblProducto', X'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 9
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (9, 'EMB-0009', 'EMB01', 'Salchicha de pollo', 9, 9, 9, 9, 'Descripcion de prueba 9 - tblProducto', X'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 10
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (10, 'EMB-0010', 'EMB01', 'Chorizo parrillero', 10, 10, 10, 10, 'Descripcion de prueba 10 - tblProducto', X'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- ============================================================================
-- TABLA: tblTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (1, 'P001ABC', 'Toyota', 'Modelo 2021', 2017, 1, 1, 1);

-- Registro 2
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (2, 'P002ABC', 'Isuzu', 'Modelo 2022', 2018, 0, 2, 2);

-- Registro 3
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (3, 'P003ABC', 'Toyota', 'Modelo 2023', 2019, 1, 3, 3);

-- Registro 4
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (4, 'P004ABC', 'Isuzu', 'Modelo 2024', 2020, 0, 4, 4);

-- Registro 5
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (5, 'P005ABC', 'Toyota', 'Modelo 2025', 2021, 1, 5, 5);

-- Registro 6
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (6, 'P006ABC', 'Isuzu', 'Modelo 2020', 2022, 0, 6, 6);

-- Registro 7
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (7, 'P007ABC', 'Toyota', 'Modelo 2021', 2023, 1, 7, 7);

-- Registro 8
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (8, 'P008ABC', 'Isuzu', 'Modelo 2022', 2024, 0, 8, 8);

-- Registro 9
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (9, 'P009ABC', 'Toyota', 'Modelo 2023', 2025, 1, 9, 9);

-- Registro 10
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (10, 'P010ABC', 'Isuzu', 'Modelo 2024', 2026, 0, 10, 10);

-- ============================================================================
-- TABLA: tblCamaraRefrigeracion
-- ============================================================================
-- Registro 1
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (1, 1, 'ThermoKing 1', -18.00, 4.00, 'OPERATIVO');

-- Registro 2
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (2, 2, 'ThermoKing 2', -18.00, 4.00, 'OPERATIVO');

-- Registro 3
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (3, 3, 'ThermoKing 3', -18.00, 4.00, 'OPERATIVO');

-- Registro 4
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (4, 4, 'ThermoKing 4', -18.00, 4.00, 'OPERATIVO');

-- Registro 5
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (5, 5, 'ThermoKing 5', -18.00, 4.00, 'OPERATIVO');

-- Registro 6
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (6, 6, 'ThermoKing 6', -18.00, 4.00, 'OPERATIVO');

-- Registro 7
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (7, 7, 'ThermoKing 7', -18.00, 4.00, 'OPERATIVO');

-- Registro 8
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (8, 8, 'ThermoKing 8', -18.00, 4.00, 'OPERATIVO');

-- Registro 9
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (9, 9, 'ThermoKing 9', -18.00, 4.00, 'OPERATIVO');

-- Registro 10
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (10, 10, 'ThermoKing 10', -18.00, 4.00, 'OPERATIVO');

-- ============================================================================
-- TABLA: tblEncabezadoViaje
-- ============================================================================
-- Registro 1
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (1, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 1, 'EN_CURSO', 150.000, 1, 1);

-- Registro 2
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (2, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 0, 'EN_CURSO', 150.000, 2, 2);

-- Registro 3
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (3, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 1, 'EN_CURSO', 150.000, 3, 3);

-- Registro 4
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (4, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 0, 'EN_CURSO', 150.000, 4, 4);

-- Registro 5
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (5, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 1, 'EN_CURSO', 150.000, 5, 5);

-- Registro 6
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (6, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 0, 'EN_CURSO', 150.000, 6, 6);

-- Registro 7
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (7, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 1, 'EN_CURSO', 150.000, 7, 7);

-- Registro 8
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (8, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 0, 'EN_CURSO', 150.000, 8, 8);

-- Registro 9
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (9, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 1, 'EN_CURSO', 150.000, 9, 9);

-- Registro 10
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (10, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 0, 'EN_CURSO', 150.000, 10, 10);

-- ============================================================================
-- TABLA: tblLoteProducto
-- ============================================================================
-- Registro 1
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (1, 'LOTE-0001', 1, 'Descripcion de prueba 1 - tblLoteProducto', 1, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 2
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (2, 'LOTE-0002', 2, 'Descripcion de prueba 2 - tblLoteProducto', 0, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 3
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (3, 'LOTE-0003', 3, 'Descripcion de prueba 3 - tblLoteProducto', 1, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 4
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (4, 'LOTE-0004', 4, 'Descripcion de prueba 4 - tblLoteProducto', 0, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 5
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (5, 'LOTE-0005', 5, 'Descripcion de prueba 5 - tblLoteProducto', 1, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 6
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (6, 'LOTE-0006', 6, 'Descripcion de prueba 6 - tblLoteProducto', 0, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 7
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (7, 'LOTE-0007', 7, 'Descripcion de prueba 7 - tblLoteProducto', 1, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 8
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (8, 'LOTE-0008', 8, 'Descripcion de prueba 8 - tblLoteProducto', 0, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 9
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (9, 'LOTE-0009', 9, 'Descripcion de prueba 9 - tblLoteProducto', 1, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 10
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (10, 'LOTE-0010', 10, 'Descripcion de prueba 10 - tblLoteProducto', 0, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- ============================================================================
-- TABLA: tblSaldos
-- ============================================================================
-- Registro 1
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (1, 1, 1, 101);

-- Registro 2
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (2, 2, 2, 102);

-- Registro 3
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (3, 3, 3, 103);

-- Registro 4
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (4, 4, 4, 104);

-- Registro 5
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (5, 5, 5, 105);

-- Registro 6
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (6, 6, 6, 106);

-- Registro 7
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (7, 7, 7, 107);

-- Registro 8
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (8, 8, 8, 108);

-- Registro 9
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (9, 9, 9, 109);

-- Registro 10
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (10, 10, 10, 110);

-- ============================================================================
-- TABLA: tblStock
-- ============================================================================
-- Registro 1
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (1, 1, 1, 1, 101, 101, 91, 101, 15, 5, 50, 20, 2, 1, 1);

-- Registro 2
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (2, 2, 2, 2, 102, 102, 92, 102, 15, 5, 50, 20, 2, 1, 1);

-- Registro 3
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (3, 3, 3, 3, 103, 103, 93, 103, 15, 5, 50, 20, 2, 1, 1);

-- Registro 4
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (4, 4, 4, 4, 104, 104, 94, 104, 15, 5, 50, 20, 2, 1, 1);

-- Registro 5
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (5, 5, 5, 5, 105, 105, 95, 105, 15, 5, 50, 20, 2, 1, 1);

-- Registro 6
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (6, 6, 6, 6, 106, 106, 96, 106, 15, 5, 50, 20, 2, 1, 1);

-- Registro 7
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (7, 7, 7, 7, 107, 107, 97, 107, 15, 5, 50, 20, 2, 1, 1);

-- Registro 8
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (8, 8, 8, 8, 108, 108, 98, 108, 15, 5, 50, 20, 2, 1, 1);

-- Registro 9
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (9, 9, 9, 9, 109, 109, 99, 109, 15, 5, 50, 20, 2, 1, 1);

-- Registro 10
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (10, 10, 10, 10, 110, 110, 100, 110, 15, 5, 50, 20, 2, 1, 1);

-- ============================================================================
-- TABLA: tblTraslado
-- ============================================================================
-- Registro 1
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (1, 'TR-0001', 1, 2, 1, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'SOLICITADO', 'Traslado de prueba 1');

-- Registro 2
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (2, 'TR-0002', 2, 3, 2, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'EN_TRANSITO', 'Traslado de prueba 2');

-- Registro 3
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (3, 'TR-0003', 3, 4, 3, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'SOLICITADO', 'Traslado de prueba 3');

-- Registro 4
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (4, 'TR-0004', 4, 5, 4, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'EN_TRANSITO', 'Traslado de prueba 4');

-- Registro 5
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (5, 'TR-0005', 5, 6, 5, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'SOLICITADO', 'Traslado de prueba 5');

-- Registro 6
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (6, 'TR-0006', 6, 7, 6, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'EN_TRANSITO', 'Traslado de prueba 6');

-- Registro 7
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (7, 'TR-0007', 7, 8, 7, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'SOLICITADO', 'Traslado de prueba 7');

-- Registro 8
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (8, 'TR-0008', 8, 9, 8, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'EN_TRANSITO', 'Traslado de prueba 8');

-- Registro 9
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (9, 'TR-0009', 9, 10, 9, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'SOLICITADO', 'Traslado de prueba 9');

-- Registro 10
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (10, 'TR-0010', 10, 1, 10, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'EN_TRANSITO', 'Traslado de prueba 10');

-- ============================================================================
-- TABLA: tblDetalleCierre
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (1, 1, 1, 91, 101, 15, 5, 50, 20);

-- Registro 2
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (2, 2, 2, 92, 102, 15, 5, 50, 20);

-- Registro 3
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (3, 3, 3, 93, 103, 15, 5, 50, 20);

-- Registro 4
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (4, 4, 4, 94, 104, 15, 5, 50, 20);

-- Registro 5
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (5, 5, 5, 95, 105, 15, 5, 50, 20);

-- Registro 6
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (6, 6, 6, 96, 106, 15, 5, 50, 20);

-- Registro 7
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (7, 7, 7, 97, 107, 15, 5, 50, 20);

-- Registro 8
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (8, 8, 8, 98, 108, 15, 5, 50, 20);

-- Registro 9
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (9, 9, 9, 99, 109, 15, 5, 50, 20);

-- Registro 10
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (10, 10, 10, 100, 110, 15, 5, 50, 20);

-- ============================================================================
-- TABLA: tblDetalleTraslado
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (1, 1, 1, 6);

-- Registro 2
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (2, 2, 2, 7);

-- Registro 3
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (3, 3, 3, 8);

-- Registro 4
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (4, 4, 4, 9);

-- Registro 5
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (5, 5, 5, 10);

-- Registro 6
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (6, 6, 6, 11);

-- Registro 7
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (7, 7, 7, 12);

-- Registro 8
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (8, 8, 8, 13);

-- Registro 9
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (9, 9, 9, 14);

-- Registro 10
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (10, 10, 10, 15);

-- ============================================================================
-- TABLA: tblDetalleViaje
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (1, 1, 1, 'Descripcion de prueba 1 - tblDetalleViaje', 6, 12.500);

-- Registro 2
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (2, 2, 2, 'Descripcion de prueba 2 - tblDetalleViaje', 7, 12.500);

-- Registro 3
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (3, 3, 3, 'Descripcion de prueba 3 - tblDetalleViaje', 8, 12.500);

-- Registro 4
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (4, 4, 4, 'Descripcion de prueba 4 - tblDetalleViaje', 9, 12.500);

-- Registro 5
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (5, 5, 5, 'Descripcion de prueba 5 - tblDetalleViaje', 10, 12.500);

-- Registro 6
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (6, 6, 6, 'Descripcion de prueba 6 - tblDetalleViaje', 11, 12.500);

-- Registro 7
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (7, 7, 7, 'Descripcion de prueba 7 - tblDetalleViaje', 12, 12.500);

-- Registro 8
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (8, 8, 8, 'Descripcion de prueba 8 - tblDetalleViaje', 13, 12.500);

-- Registro 9
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (9, 9, 9, 'Descripcion de prueba 9 - tblDetalleViaje', 14, 12.500);

-- Registro 10
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (10, 10, 10, 'Descripcion de prueba 10 - tblDetalleViaje', 15, 12.500);

-- ============================================================================
-- TABLA: tblHistoricoInventario
-- ============================================================================
-- Registro 1
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (1, 1, 1, 1, 1, 1, 101, 101, 91, 101, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 2
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (2, 2, 2, 2, 2, 2, 102, 102, 92, 102, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 3
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (3, 3, 3, 3, 3, 3, 103, 103, 93, 103, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 4
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (4, 4, 4, 4, 4, 4, 104, 104, 94, 104, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 5
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (5, 5, 5, 5, 5, 5, 105, 105, 95, 105, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 6
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (6, 6, 6, 6, 6, 6, 106, 106, 96, 106, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 7
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (7, 7, 7, 7, 7, 7, 107, 107, 97, 107, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 8
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (8, 8, 8, 8, 8, 8, 108, 108, 98, 108, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 9
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (9, 9, 9, 9, 9, 9, 109, 109, 99, 109, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 10
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (10, 10, 10, 10, 10, 10, 110, 110, 100, 110, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- ============================================================================
-- TABLA: tblMovimientoInventario
-- ============================================================================
-- Registro 1
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (1, 1, 1, 2, 1, 1, '2026-10-02 10:30:00', 'Movimiento de prueba 1', 'CUADRADO', 1);

-- Registro 2
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (2, 2, 2, 3, 2, 2, '2026-10-03 10:30:00', 'Movimiento de prueba 2', 'PENDIENTE', 2);

-- Registro 3
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (3, 3, 3, 4, 3, 3, '2026-10-04 10:30:00', 'Movimiento de prueba 3', 'CUADRADO', 3);

-- Registro 4
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (4, 4, 4, 5, 4, 4, '2026-10-05 10:30:00', 'Movimiento de prueba 4', 'PENDIENTE', 4);

-- Registro 5
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (5, 5, 5, 6, 5, 5, '2026-10-06 10:30:00', 'Movimiento de prueba 5', 'CUADRADO', 5);

-- Registro 6
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (6, 6, 6, 7, 6, 6, '2026-10-07 10:30:00', 'Movimiento de prueba 6', 'PENDIENTE', 6);

-- Registro 7
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (7, 7, 7, 8, 7, 7, '2026-10-08 10:30:00', 'Movimiento de prueba 7', 'CUADRADO', 7);

-- Registro 8
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (8, 8, 8, 9, 8, 8, '2026-10-09 10:30:00', 'Movimiento de prueba 8', 'PENDIENTE', 8);

-- Registro 9
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (9, 9, 9, 10, 9, 9, '2026-10-10 10:30:00', 'Movimiento de prueba 9', 'CUADRADO', 9);

-- Registro 10
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (10, 10, 10, 1, 10, 10, '2026-10-11 10:30:00', 'Movimiento de prueba 10', 'PENDIENTE', 10);

-- ============================================================================
-- TABLA: tblServicioTransporteExterno
-- ============================================================================
-- Registro 1
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (1, 1, 1, 1, 'GUIA-0001', 'Programado', 'Sucursal Central', 'Zona 1, Ciudad de Guatemala', 'Sucursal Norte', 'Zona 1, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 2
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (2, 2, 2, 2, 'GUIA-0002', 'Entregado', 'Sucursal Norte', 'Zona 2, Ciudad de Guatemala', 'Sucursal Sur', 'Zona 2, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 3
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (3, 3, 3, 3, 'GUIA-0003', 'Programado', 'Sucursal Sur', 'Zona 3, Ciudad de Guatemala', 'Sucursal Occidente', 'Zona 3, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 4
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (4, 4, 4, 4, 'GUIA-0004', 'Entregado', 'Sucursal Occidente', 'Zona 4, Ciudad de Guatemala', 'Sucursal Oriente', 'Zona 4, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 5
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (5, 5, 5, 5, 'GUIA-0005', 'Programado', 'Sucursal Oriente', 'Zona 5, Ciudad de Guatemala', 'Sucursal Metropolitana', 'Zona 5, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 6
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (6, 6, 6, 6, 'GUIA-0006', 'Entregado', 'Sucursal Metropolitana', 'Zona 6, Ciudad de Guatemala', 'Sucursal Carretera', 'Zona 6, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 7
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (7, 7, 7, 7, 'GUIA-0007', 'Programado', 'Sucursal Carretera', 'Zona 7, Ciudad de Guatemala', 'Sucursal Industrial', 'Zona 7, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 8
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (8, 8, 8, 8, 'GUIA-0008', 'Entregado', 'Sucursal Industrial', 'Zona 8, Ciudad de Guatemala', 'Sucursal Zona 10', 'Zona 8, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 9
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (9, 9, 9, 9, 'GUIA-0009', 'Programado', 'Sucursal Zona 10', 'Zona 9, Ciudad de Guatemala', 'Sucursal Zona 12', 'Zona 9, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 10
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (10, 10, 10, 10, 'GUIA-0010', 'Entregado', 'Sucursal Zona 12', 'Zona 10, Ciudad de Guatemala', 'Sucursal Central', 'Zona 10, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- ============================================================================
-- TABLA: tblTelemetriaTemperatura
-- ============================================================================
-- Registro 1
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (1, 1, '2026-10-02 10:30:00', 2.50, 0.6500, 1, '14.6001, -90.5001');

-- Registro 2
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (2, 2, '2026-10-03 10:30:00', 2.50, 0.6500, 0, '14.6002, -90.5002');

-- Registro 3
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (3, 3, '2026-10-04 10:30:00', 2.50, 0.6500, 1, '14.6003, -90.5003');

-- Registro 4
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (4, 4, '2026-10-05 10:30:00', 2.50, 0.6500, 0, '14.6004, -90.5004');

-- Registro 5
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (5, 5, '2026-10-06 10:30:00', 2.50, 0.6500, 1, '14.6005, -90.5005');

-- Registro 6
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (6, 6, '2026-10-07 10:30:00', 2.50, 0.6500, 0, '14.6006, -90.5006');

-- Registro 7
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (7, 7, '2026-10-08 10:30:00', 2.50, 0.6500, 1, '14.6007, -90.5007');

-- Registro 8
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (8, 8, '2026-10-09 10:30:00', 2.50, 0.6500, 0, '14.6008, -90.5008');

-- Registro 9
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (9, 9, '2026-10-10 10:30:00', 2.50, 0.6500, 1, '14.6009, -90.5009');

-- Registro 10
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (10, 10, '2026-10-11 10:30:00', 2.50, 0.6500, 0, '14.6010, -90.5010');

-- ============================================================================
-- TABLA: tblAlertaCadenaFrio
-- ============================================================================
-- Registro 1
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (1, 1, 'TEMPERATURA_ALTA', 1, 'Alerta revisada para el viaje 1');

-- Registro 2
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (2, 2, 'PUERTA_ABIERTA', 0, 'Alerta revisada para el viaje 2');

-- Registro 3
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (3, 3, 'TEMPERATURA_ALTA', 1, 'Alerta revisada para el viaje 3');

-- Registro 4
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (4, 4, 'PUERTA_ABIERTA', 0, 'Alerta revisada para el viaje 4');

-- Registro 5
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (5, 5, 'TEMPERATURA_ALTA', 1, 'Alerta revisada para el viaje 5');

-- Registro 6
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (6, 6, 'PUERTA_ABIERTA', 0, 'Alerta revisada para el viaje 6');

-- Registro 7
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (7, 7, 'TEMPERATURA_ALTA', 1, 'Alerta revisada para el viaje 7');

-- Registro 8
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (8, 8, 'PUERTA_ABIERTA', 0, 'Alerta revisada para el viaje 8');

-- Registro 9
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (9, 9, 'TEMPERATURA_ALTA', 1, 'Alerta revisada para el viaje 9');

-- Registro 10
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (10, 10, 'PUERTA_ABIERTA', 0, 'Alerta revisada para el viaje 10');

-- ============================================================================
-- TABLA: tblAuditoriaInventario
-- ============================================================================
-- Registro 1
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (1, 1, 1, 'INDIVIDUAL', 'FINALIZADA', '2026-10-02 10:30:00', 'Sin diferencias', 'Conteo de verificación de inventario 1');

-- Registro 2
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (2, 2, 2, 'MULTIPLE', 'EN_PROCESO', '2026-10-03 10:30:00', 'Diferencia detectada', 'Conteo de verificación de inventario 2');

-- Registro 3
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (3, 3, 3, 'INDIVIDUAL', 'FINALIZADA', '2026-10-04 10:30:00', 'Sin diferencias', 'Conteo de verificación de inventario 3');

-- Registro 4
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (4, 4, 4, 'MULTIPLE', 'EN_PROCESO', '2026-10-05 10:30:00', 'Diferencia detectada', 'Conteo de verificación de inventario 4');

-- Registro 5
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (5, 5, 5, 'INDIVIDUAL', 'FINALIZADA', '2026-10-06 10:30:00', 'Sin diferencias', 'Conteo de verificación de inventario 5');

-- Registro 6
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (6, 6, 6, 'MULTIPLE', 'EN_PROCESO', '2026-10-07 10:30:00', 'Diferencia detectada', 'Conteo de verificación de inventario 6');

-- Registro 7
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (7, 7, 7, 'INDIVIDUAL', 'FINALIZADA', '2026-10-08 10:30:00', 'Sin diferencias', 'Conteo de verificación de inventario 7');

-- Registro 8
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (8, 8, 8, 'MULTIPLE', 'EN_PROCESO', '2026-10-09 10:30:00', 'Diferencia detectada', 'Conteo de verificación de inventario 8');

-- Registro 9
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (9, 9, 9, 'INDIVIDUAL', 'FINALIZADA', '2026-10-10 10:30:00', 'Sin diferencias', 'Conteo de verificación de inventario 9');

-- Registro 10
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (10, 10, 10, 'MULTIPLE', 'EN_PROCESO', '2026-10-11 10:30:00', 'Diferencia detectada', 'Conteo de verificación de inventario 10');

-- ============================================================================
-- TABLA: tblConductorTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (1, 1, 'Ana López', '55500001');

-- Registro 2
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (2, 2, 'Luis Morales', '55500002');

-- Registro 3
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (3, 3, 'María García', '55500003');

-- Registro 4
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (4, 4, 'Carlos Pérez', '55500004');

-- Registro 5
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (5, 5, 'Elena Ruiz', '55500005');

-- Registro 6
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (6, 6, 'José Ramírez', '55500006');

-- Registro 7
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (7, 7, 'Sofía Herrera', '55500007');

-- Registro 8
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (8, 8, 'Pedro Castillo', '55500008');

-- Registro 9
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (9, 9, 'Laura Díaz', '55500009');

-- Registro 10
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (10, 10, 'Jorge Méndez', '55500010');

-- ============================================================================
-- TABLA: tblCostoTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (1, 1, 1, 6, 25.0000, 'GTQ', 'Transferencia');

-- Registro 2
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (2, 2, 2, 7, 25.0000, 'GTQ', 'Efectivo');

-- Registro 3
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (3, 3, 3, 8, 25.0000, 'GTQ', 'Transferencia');

-- Registro 4
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (4, 4, 4, 9, 25.0000, 'GTQ', 'Efectivo');

-- Registro 5
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (5, 5, 5, 10, 25.0000, 'GTQ', 'Transferencia');

-- Registro 6
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (6, 6, 6, 11, 25.0000, 'GTQ', 'Efectivo');

-- Registro 7
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (7, 7, 7, 12, 25.0000, 'GTQ', 'Transferencia');

-- Registro 8
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (8, 8, 8, 13, 25.0000, 'GTQ', 'Efectivo');

-- Registro 9
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (9, 9, 9, 14, 25.0000, 'GTQ', 'Transferencia');

-- Registro 10
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (10, 10, 10, 15, 25.0000, 'GTQ', 'Efectivo');

-- ============================================================================
-- TABLA: tblDetalleMovimiento
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (1, 1, 1, 1, 6, 25.0000);

-- Registro 2
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (2, 2, 2, 2, 7, 25.0000);

-- Registro 3
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (3, 3, 3, 3, 8, 25.0000);

-- Registro 4
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (4, 4, 4, 4, 9, 25.0000);

-- Registro 5
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (5, 5, 5, 5, 10, 25.0000);

-- Registro 6
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (6, 6, 6, 6, 11, 25.0000);

-- Registro 7
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (7, 7, 7, 7, 12, 25.0000);

-- Registro 8
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (8, 8, 8, 8, 13, 25.0000);

-- Registro 9
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (9, 9, 9, 9, 14, 25.0000);

-- Registro 10
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (10, 10, 10, 10, 15, 25.0000);

-- ============================================================================
-- TABLA: tblDetalleViajeLoteProducto
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (1, 1, 1, 3, 12.500);

-- Registro 2
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (2, 2, 2, 4, 12.500);

-- Registro 3
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (3, 3, 3, 5, 12.500);

-- Registro 4
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (4, 4, 4, 6, 12.500);

-- Registro 5
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (5, 5, 5, 7, 12.500);

-- Registro 6
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (6, 6, 6, 8, 12.500);

-- Registro 7
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (7, 7, 7, 9, 12.500);

-- Registro 8
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (8, 8, 8, 10, 12.500);

-- Registro 9
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (9, 9, 9, 11, 12.500);

-- Registro 10
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (10, 10, 10, 12, 12.500);

-- ============================================================================
-- TABLA: tblHistorialTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (1, 1, '2026-10-02 10:30:00', 'Salida', 'Descripcion de prueba 1 - tblHistorialTransporte', 'En tránsito');

-- Registro 2
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (2, 2, '2026-10-03 10:30:00', 'Recepción', 'Descripcion de prueba 2 - tblHistorialTransporte', 'En tránsito');

-- Registro 3
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (3, 3, '2026-10-04 10:30:00', 'Salida', 'Descripcion de prueba 3 - tblHistorialTransporte', 'En tránsito');

-- Registro 4
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (4, 4, '2026-10-05 10:30:00', 'Recepción', 'Descripcion de prueba 4 - tblHistorialTransporte', 'En tránsito');

-- Registro 5
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (5, 5, '2026-10-06 10:30:00', 'Salida', 'Descripcion de prueba 5 - tblHistorialTransporte', 'En tránsito');

-- Registro 6
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (6, 6, '2026-10-07 10:30:00', 'Recepción', 'Descripcion de prueba 6 - tblHistorialTransporte', 'En tránsito');

-- Registro 7
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (7, 7, '2026-10-08 10:30:00', 'Salida', 'Descripcion de prueba 7 - tblHistorialTransporte', 'En tránsito');

-- Registro 8
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (8, 8, '2026-10-09 10:30:00', 'Recepción', 'Descripcion de prueba 8 - tblHistorialTransporte', 'En tránsito');

-- Registro 9
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (9, 9, '2026-10-10 10:30:00', 'Salida', 'Descripcion de prueba 9 - tblHistorialTransporte', 'En tránsito');

-- Registro 10
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (10, 10, '2026-10-11 10:30:00', 'Recepción', 'Descripcion de prueba 10 - tblHistorialTransporte', 'En tránsito');

-- ============================================================================
-- TABLA: tblIncidenciaTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (1, 1, 'INC-0001', '2026-10-02 10:30:00', 'Retraso', 'Descripcion de prueba 1 - tblIncidenciaTransporte', 'Ana López', 'Pendiente');

-- Registro 2
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (2, 2, 'INC-0002', '2026-10-03 10:30:00', 'Desvío', 'Descripcion de prueba 2 - tblIncidenciaTransporte', 'Luis Morales', 'Resuelto');

-- Registro 3
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (3, 3, 'INC-0003', '2026-10-04 10:30:00', 'Retraso', 'Descripcion de prueba 3 - tblIncidenciaTransporte', 'María García', 'Pendiente');

-- Registro 4
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (4, 4, 'INC-0004', '2026-10-05 10:30:00', 'Desvío', 'Descripcion de prueba 4 - tblIncidenciaTransporte', 'Carlos Pérez', 'Resuelto');

-- Registro 5
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (5, 5, 'INC-0005', '2026-10-06 10:30:00', 'Retraso', 'Descripcion de prueba 5 - tblIncidenciaTransporte', 'Elena Ruiz', 'Pendiente');

-- Registro 6
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (6, 6, 'INC-0006', '2026-10-07 10:30:00', 'Desvío', 'Descripcion de prueba 6 - tblIncidenciaTransporte', 'José Ramírez', 'Resuelto');

-- Registro 7
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (7, 7, 'INC-0007', '2026-10-08 10:30:00', 'Retraso', 'Descripcion de prueba 7 - tblIncidenciaTransporte', 'Sofía Herrera', 'Pendiente');

-- Registro 8
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (8, 8, 'INC-0008', '2026-10-09 10:30:00', 'Desvío', 'Descripcion de prueba 8 - tblIncidenciaTransporte', 'Pedro Castillo', 'Resuelto');

-- Registro 9
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (9, 9, 'INC-0009', '2026-10-10 10:30:00', 'Retraso', 'Descripcion de prueba 9 - tblIncidenciaTransporte', 'Laura Díaz', 'Pendiente');

-- Registro 10
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (10, 10, 'INC-0010', '2026-10-11 10:30:00', 'Desvío', 'Descripcion de prueba 10 - tblIncidenciaTransporte', 'Jorge Méndez', 'Resuelto');

-- ============================================================================
-- TABLA: tblProductoTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (1, 1, 1, 6, 'cajas');

-- Registro 2
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (2, 2, 2, 7, 'cajas');

-- Registro 3
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (3, 3, 3, 8, 'cajas');

-- Registro 4
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (4, 4, 4, 9, 'cajas');

-- Registro 5
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (5, 5, 5, 10, 'cajas');

-- Registro 6
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (6, 6, 6, 11, 'cajas');

-- Registro 7
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (7, 7, 7, 12, 'cajas');

-- Registro 8
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (8, 8, 8, 13, 'cajas');

-- Registro 9
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (9, 9, 9, 14, 'cajas');

-- Registro 10
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (10, 10, 10, 15, 'cajas');

-- ============================================================================
-- TABLA: tblSolicitudPagoTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (1, 1, 'SP-0001', 1, 'REF-0001', '2026-10-20');

-- Registro 2
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (2, 2, 'SP-0002', 2, 'REF-0002', '2026-10-20');

-- Registro 3
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (3, 3, 'SP-0003', 3, 'REF-0003', '2026-10-20');

-- Registro 4
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (4, 4, 'SP-0004', 4, 'REF-0004', '2026-10-20');

-- Registro 5
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (5, 5, 'SP-0005', 5, 'REF-0005', '2026-10-20');

-- Registro 6
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (6, 6, 'SP-0006', 6, 'REF-0006', '2026-10-20');

-- Registro 7
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (7, 7, 'SP-0007', 7, 'REF-0007', '2026-10-20');

-- Registro 8
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (8, 8, 'SP-0008', 8, 'REF-0008', '2026-10-20');

-- Registro 9
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (9, 9, 'SP-0009', 9, 'REF-0009', '2026-10-20');

-- Registro 10
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (10, 10, 'SP-0010', 10, 'REF-0010', '2026-10-20');

-- ============================================================================
-- TABLA: tblUbicacionTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (1, 1, 'Zona 1, Ciudad de Guatemala', '2026-10-02 10:30:00');

-- Registro 2
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (2, 2, 'Zona 2, Ciudad de Guatemala', '2026-10-03 10:30:00');

-- Registro 3
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (3, 3, 'Zona 3, Ciudad de Guatemala', '2026-10-04 10:30:00');

-- Registro 4
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (4, 4, 'Zona 4, Ciudad de Guatemala', '2026-10-05 10:30:00');

-- Registro 5
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (5, 5, 'Zona 5, Ciudad de Guatemala', '2026-10-06 10:30:00');

-- Registro 6
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (6, 6, 'Zona 6, Ciudad de Guatemala', '2026-10-07 10:30:00');

-- Registro 7
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (7, 7, 'Zona 7, Ciudad de Guatemala', '2026-10-08 10:30:00');

-- Registro 8
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (8, 8, 'Zona 8, Ciudad de Guatemala', '2026-10-09 10:30:00');

-- Registro 9
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (9, 9, 'Zona 9, Ciudad de Guatemala', '2026-10-10 10:30:00');

-- Registro 10
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (10, 10, 'Zona 10, Ciudad de Guatemala', '2026-10-11 10:30:00');

-- ============================================================================
-- TABLA: tblVehiculoTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (1, 1, 'C001DEF', 'Camión', '1250 kg');

-- Registro 2
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (2, 2, 'C002DEF', 'Camioneta', '1500 kg');

-- Registro 3
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (3, 3, 'C003DEF', 'Furgón', '1750 kg');

-- Registro 4
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (4, 4, 'C004DEF', 'Pick-up', '2000 kg');

-- Registro 5
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (5, 5, 'C005DEF', 'Panel', '2250 kg');

-- Registro 6
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (6, 6, 'C006DEF', 'Tráiler', '2500 kg');

-- Registro 7
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (7, 7, 'C007DEF', 'Cabezal', '2750 kg');

-- Registro 8
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (8, 8, 'C008DEF', 'Motocicleta', '3000 kg');

-- Registro 9
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (9, 9, 'C009DEF', 'Microbús', '3250 kg');

-- Registro 10
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (10, 10, 'C010DEF', 'Camión refrigerado', '3500 kg');

-- ============================================================================
-- TABLA: tblDetalleAuditoria
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (1, 1, 1, 101, 101, 'Detalle del conteo 1');

-- Registro 2
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (2, 2, 2, 102, 102, 'Detalle del conteo 2');

-- Registro 3
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (3, 3, 3, 103, 103, 'Detalle del conteo 3');

-- Registro 4
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (4, 4, 4, 104, 104, 'Detalle del conteo 4');

-- Registro 5
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (5, 5, 5, 105, 105, 'Detalle del conteo 5');

-- Registro 6
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (6, 6, 6, 106, 106, 'Detalle del conteo 6');

-- Registro 7
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (7, 7, 7, 107, 107, 'Detalle del conteo 7');

-- Registro 8
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (8, 8, 8, 108, 108, 'Detalle del conteo 8');

-- Registro 9
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (9, 9, 9, 109, 109, 'Detalle del conteo 9');

-- Registro 10
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (10, 10, 10, 110, 110, 'Detalle del conteo 10');

COMMIT;

-- VERIFICACION DE REGISTROS POR TABLA
SELECT 'tblCategoria' AS tabla, COUNT(*) AS total_registros FROM tblCategoria;
SELECT 'tblEmpresaTransportista' AS tabla, COUNT(*) AS total_registros FROM tblEmpresaTransportista;
SELECT 'tblEstado' AS tabla, COUNT(*) AS total_registros FROM tblEstado;
SELECT 'tblEstadoSolicitudPago' AS tabla, COUNT(*) AS total_registros FROM tblEstadoSolicitudPago;
SELECT 'tblLinea' AS tabla, COUNT(*) AS total_registros FROM tblLinea;
SELECT 'tblMarca' AS tabla, COUNT(*) AS total_registros FROM tblMarca;
SELECT 'tblProveedor' AS tabla, COUNT(*) AS total_registros FROM tblProveedor;
SELECT 'tblSucursal' AS tabla, COUNT(*) AS total_registros FROM tblSucursal;
SELECT 'tblTarifaTransporte' AS tabla, COUNT(*) AS total_registros FROM tblTarifaTransporte;
SELECT 'tblTipoMovimiento' AS tabla, COUNT(*) AS total_registros FROM tblTipoMovimiento;
SELECT 'tblTipoServicio' AS tabla, COUNT(*) AS total_registros FROM tblTipoServicio;
SELECT 'tblTipoTransporte' AS tabla, COUNT(*) AS total_registros FROM tblTipoTransporte;
SELECT 'tblBodega' AS tabla, COUNT(*) AS total_registros FROM tblBodega;
SELECT 'tblCierreMovimiento' AS tabla, COUNT(*) AS total_registros FROM tblCierreMovimiento;
SELECT 'tblConductor' AS tabla, COUNT(*) AS total_registros FROM tblConductor;
SELECT 'tblProducto' AS tabla, COUNT(*) AS total_registros FROM tblProducto;
SELECT 'tblTransporte' AS tabla, COUNT(*) AS total_registros FROM tblTransporte;
SELECT 'tblCamaraRefrigeracion' AS tabla, COUNT(*) AS total_registros FROM tblCamaraRefrigeracion;
SELECT 'tblEncabezadoViaje' AS tabla, COUNT(*) AS total_registros FROM tblEncabezadoViaje;
SELECT 'tblLoteProducto' AS tabla, COUNT(*) AS total_registros FROM tblLoteProducto;
SELECT 'tblSaldos' AS tabla, COUNT(*) AS total_registros FROM tblSaldos;
SELECT 'tblStock' AS tabla, COUNT(*) AS total_registros FROM tblStock;
SELECT 'tblTraslado' AS tabla, COUNT(*) AS total_registros FROM tblTraslado;
SELECT 'tblDetalleCierre' AS tabla, COUNT(*) AS total_registros FROM tblDetalleCierre;
SELECT 'tblDetalleTraslado' AS tabla, COUNT(*) AS total_registros FROM tblDetalleTraslado;
SELECT 'tblDetalleViaje' AS tabla, COUNT(*) AS total_registros FROM tblDetalleViaje;
SELECT 'tblHistoricoInventario' AS tabla, COUNT(*) AS total_registros FROM tblHistoricoInventario;
SELECT 'tblMovimientoInventario' AS tabla, COUNT(*) AS total_registros FROM tblMovimientoInventario;
SELECT 'tblServicioTransporteExterno' AS tabla, COUNT(*) AS total_registros FROM tblServicioTransporteExterno;
SELECT 'tblTelemetriaTemperatura' AS tabla, COUNT(*) AS total_registros FROM tblTelemetriaTemperatura;
SELECT 'tblAlertaCadenaFrio' AS tabla, COUNT(*) AS total_registros FROM tblAlertaCadenaFrio;
SELECT 'tblAuditoriaInventario' AS tabla, COUNT(*) AS total_registros FROM tblAuditoriaInventario;
SELECT 'tblConductorTransporte' AS tabla, COUNT(*) AS total_registros FROM tblConductorTransporte;
SELECT 'tblCostoTransporte' AS tabla, COUNT(*) AS total_registros FROM tblCostoTransporte;
SELECT 'tblDetalleMovimiento' AS tabla, COUNT(*) AS total_registros FROM tblDetalleMovimiento;
SELECT 'tblDetalleViajeLoteProducto' AS tabla, COUNT(*) AS total_registros FROM tblDetalleViajeLoteProducto;
SELECT 'tblHistorialTransporte' AS tabla, COUNT(*) AS total_registros FROM tblHistorialTransporte;
SELECT 'tblIncidenciaTransporte' AS tabla, COUNT(*) AS total_registros FROM tblIncidenciaTransporte;
SELECT 'tblProductoTransporte' AS tabla, COUNT(*) AS total_registros FROM tblProductoTransporte;
SELECT 'tblSolicitudPagoTransporte' AS tabla, COUNT(*) AS total_registros FROM tblSolicitudPagoTransporte;
SELECT 'tblUbicacionTransporte' AS tabla, COUNT(*) AS total_registros FROM tblUbicacionTransporte;
SELECT 'tblVehiculoTransporte' AS tabla, COUNT(*) AS total_registros FROM tblVehiculoTransporte;
SELECT 'tblDetalleAuditoria' AS tabla, COUNT(*) AS total_registros FROM tblDetalleAuditoria;
