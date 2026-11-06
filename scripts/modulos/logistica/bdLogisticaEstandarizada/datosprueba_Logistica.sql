-- ============================================================================
-- TERMINUS ERP - LOGISTICA - 10 REGISTROS DE PRUEBA POR TABLA (400 EN TOTAL)
-- Compatible con: dbLogistica EST-01 V1.3, nomenclatura Pk_ / Fk_ actualizada.
-- USO: Ejecutar DESPUES del script de creacion, sobre tablas vacias.
-- IDs reservados exclusivamente para pruebas: 910001 a 910010.
-- PRECAUCION: no ejecutar dos veces; las PK y los codigos UNIQUE se duplicarian.
-- Los INSERT no borran, alteran ni desactivan FK o CHECK.
-- Todos los nombres, direcciones, empresas y contactos DEMO son ficticios.
-- Las imagenes son PNG minimo de 1 pixel; sustituirlas en la app por imagenes reales.
-- NOTA: Los 10 cierres son 1 historico cerrado (dic 2025) y 9 meses abiertos
-- (ene-sep 2026). Los 10 movimientos de enero estan cuadrados y el stock cuadra.
-- Las solicitudes de pago estan pendientes: referenciaBancaria y fechaPago NULL.
-- ============================================================================
USE dbLogistica;
START TRANSACTION;
-- [01/40] tblEmpresaTransportista - 10 filas
INSERT INTO `tblEmpresaTransportista` (
    `Pk_Empresa_Transportista`,
    `nombreEmpresaTransportista`,
    `contactoEmpresaTransportista`,
    `telefonoEmpresaTransportista`
) VALUES
    (910001, 'Transportista DEMO26 01', 'Contacto DEMO26 01', '5550-1001'),
    (910002, 'Transportista DEMO26 02', 'Contacto DEMO26 02', '5550-1002'),
    (910003, 'Transportista DEMO26 03', 'Contacto DEMO26 03', '5550-1003'),
    (910004, 'Transportista DEMO26 04', 'Contacto DEMO26 04', '5550-1004'),
    (910005, 'Transportista DEMO26 05', 'Contacto DEMO26 05', '5550-1005'),
    (910006, 'Transportista DEMO26 06', 'Contacto DEMO26 06', '5550-1006'),
    (910007, 'Transportista DEMO26 07', 'Contacto DEMO26 07', '5550-1007'),
    (910008, 'Transportista DEMO26 08', 'Contacto DEMO26 08', '5550-1008'),
    (910009, 'Transportista DEMO26 09', 'Contacto DEMO26 09', '5550-1009'),
    (910010, 'Transportista DEMO26 10', 'Contacto DEMO26 10', '5550-1010');

-- [02/40] tblTipoServicio - 10 filas
INSERT INTO `tblTipoServicio` (
    `Pk_Tipo_Servicio`,
    `nombreTipoServicio`
) VALUES
    (910001, 'Servicio DEMO26 01'),
    (910002, 'Servicio DEMO26 02'),
    (910003, 'Servicio DEMO26 03'),
    (910004, 'Servicio DEMO26 04'),
    (910005, 'Servicio DEMO26 05'),
    (910006, 'Servicio DEMO26 06'),
    (910007, 'Servicio DEMO26 07'),
    (910008, 'Servicio DEMO26 08'),
    (910009, 'Servicio DEMO26 09'),
    (910010, 'Servicio DEMO26 10');

-- [03/40] tblTarifaTransporte - 10 filas
INSERT INTO `tblTarifaTransporte` (
    `Pk_Tarifa_Transporte`,
    `nombreTarifaTransporte`
) VALUES
    (910001, 'Tarifa DEMO26 01'),
    (910002, 'Tarifa DEMO26 02'),
    (910003, 'Tarifa DEMO26 03'),
    (910004, 'Tarifa DEMO26 04'),
    (910005, 'Tarifa DEMO26 05'),
    (910006, 'Tarifa DEMO26 06'),
    (910007, 'Tarifa DEMO26 07'),
    (910008, 'Tarifa DEMO26 08'),
    (910009, 'Tarifa DEMO26 09'),
    (910010, 'Tarifa DEMO26 10');

-- [04/40] tblEstado - 10 filas
INSERT INTO `tblEstado` (
    `Pk_Estado`,
    `tipoEstado`
) VALUES
    (910001, 'Estado DEMO26 01'),
    (910002, 'Estado DEMO26 02'),
    (910003, 'Estado DEMO26 03'),
    (910004, 'Estado DEMO26 04'),
    (910005, 'Estado DEMO26 05'),
    (910006, 'Estado DEMO26 06'),
    (910007, 'Estado DEMO26 07'),
    (910008, 'Estado DEMO26 08'),
    (910009, 'Estado DEMO26 09'),
    (910010, 'Estado DEMO26 10');

-- [05/40] tblTipoTransporte - 10 filas
INSERT INTO `tblTipoTransporte` (
    `Pk_Tipo_Transporte`,
    `nombreTipoTransporte`
) VALUES
    (910001, 'Tipo Transporte DEMO26 01'),
    (910002, 'Tipo Transporte DEMO26 02'),
    (910003, 'Tipo Transporte DEMO26 03'),
    (910004, 'Tipo Transporte DEMO26 04'),
    (910005, 'Tipo Transporte DEMO26 05'),
    (910006, 'Tipo Transporte DEMO26 06'),
    (910007, 'Tipo Transporte DEMO26 07'),
    (910008, 'Tipo Transporte DEMO26 08'),
    (910009, 'Tipo Transporte DEMO26 09'),
    (910010, 'Tipo Transporte DEMO26 10');

-- [06/40] tblTransporte - 10 filas
INSERT INTO `tblTransporte` (
    `Pk_Transporte`,
    `nroPlacaTransporte`,
    `marcaTransporte`,
    `modeloTransporte`,
    `anioTransporte`,
    `esRefrigeradoTransporte`,
    `Fk_Estado_Codigo`,
    `Fk_Tipo_Transporte_Codigo`
) VALUES
    (910001, 'TRN-D26-001', 'Marca Demo 01', 'Modelo 01', 2023, 1, 910001, 910001),
    (910002, 'TRN-D26-002', 'Marca Demo 02', 'Modelo 02', 2024, 1, 910002, 910002),
    (910003, 'TRN-D26-003', 'Marca Demo 03', 'Modelo 03', 2025, 1, 910003, 910003),
    (910004, 'TRN-D26-004', 'Marca Demo 04', 'Modelo 04', 2022, 1, 910004, 910004),
    (910005, 'TRN-D26-005', 'Marca Demo 05', 'Modelo 05', 2023, 1, 910005, 910005),
    (910006, 'TRN-D26-006', 'Marca Demo 06', 'Modelo 06', 2024, 1, 910006, 910006),
    (910007, 'TRN-D26-007', 'Marca Demo 07', 'Modelo 07', 2025, 1, 910007, 910007),
    (910008, 'TRN-D26-008', 'Marca Demo 08', 'Modelo 08', 2022, 1, 910008, 910008),
    (910009, 'TRN-D26-009', 'Marca Demo 09', 'Modelo 09', 2023, 1, 910009, 910009),
    (910010, 'TRN-D26-010', 'Marca Demo 10', 'Modelo 10', 2024, 1, 910010, 910010);

-- [07/40] tblCamaraRefrigeracion - 10 filas
INSERT INTO `tblCamaraRefrigeracion` (
    `Pk_Camara_Refrigeracion`,
    `Fk_Transporte_Codigo`,
    `marcaEquipoCamaraRefrigeracion`,
    `tempMinimaCelsiusCamaraRefrigeracion`,
    `tempMaximaCelsiusCamaraRefrigeracion`,
    `estadoOperativoCamaraRefrigeracion`
) VALUES
    (910001, 910001, 'Camara DEMO26 01', -5.00, 5.00, 'OPERATIVO'),
    (910002, 910002, 'Camara DEMO26 02', -5.00, 5.00, 'OPERATIVO'),
    (910003, 910003, 'Camara DEMO26 03', -5.00, 5.00, 'OPERATIVO'),
    (910004, 910004, 'Camara DEMO26 04', -5.00, 5.00, 'OPERATIVO'),
    (910005, 910005, 'Camara DEMO26 05', -5.00, 5.00, 'OPERATIVO'),
    (910006, 910006, 'Camara DEMO26 06', -5.00, 5.00, 'OPERATIVO'),
    (910007, 910007, 'Camara DEMO26 07', -5.00, 5.00, 'OPERATIVO'),
    (910008, 910008, 'Camara DEMO26 08', -5.00, 5.00, 'OPERATIVO'),
    (910009, 910009, 'Camara DEMO26 09', -5.00, 5.00, 'OPERATIVO'),
    (910010, 910010, 'Camara DEMO26 10', -5.00, 5.00, 'OPERATIVO');

-- [08/40] tblConductor - 10 filas
INSERT INTO `tblConductor` (
    `Pk_Conductor`,
    `codigoConductor`,
    `nombreCompletoConductor`,
    `numeroLicenciaConductor`,
    `fechaVencimientoDocConductor`,
    `Fk_Estado_Codigo`
) VALUES
    (910001, 'COND-D26-01', 'Carlos Lopez', 'LIC-D26-0001', '2028-12-01', 910001),
    (910002, 'COND-D26-02', 'Ana Hernandez', 'LIC-D26-0002', '2028-12-02', 910002),
    (910003, 'COND-D26-03', 'Mario Morales', 'LIC-D26-0003', '2028-12-03', 910003),
    (910004, 'COND-D26-04', 'Luisa Perez', 'LIC-D26-0004', '2028-12-04', 910004),
    (910005, 'COND-D26-05', 'Pedro Ramirez', 'LIC-D26-0005', '2028-12-05', 910005),
    (910006, 'COND-D26-06', 'Marta Gomez', 'LIC-D26-0006', '2028-12-06', 910006),
    (910007, 'COND-D26-07', 'Jorge Castillo', 'LIC-D26-0007', '2028-12-07', 910007),
    (910008, 'COND-D26-08', 'Elena Rivera', 'LIC-D26-0008', '2028-12-08', 910008),
    (910009, 'COND-D26-09', 'Oscar Garcia', 'LIC-D26-0009', '2028-12-09', 910009),
    (910010, 'COND-D26-10', 'Diana Flores', 'LIC-D26-0010', '2028-12-10', 910010);

-- [09/40] tblInspeccionSanitaria - 10 filas
INSERT INTO `tblInspeccionSanitaria` (
    `Pk_Inspeccion_Sanitaria`,
    `Fk_Transporte_Codigo`,
    `fechaInspeccionSanitaria`,
    `sanitizadoCorrectoInspeccionSanitaria`,
    `libreOloresContamInspeccionSanitaria`,
    `aprobadoParaCargaInspeccionSanitaria`,
    `observacionesInspeccionSanitaria`
) VALUES
    (910001, 910001, '2026-10-03 09:00:00', 1, 1, 1, 'Inspeccion de prueba 01: transporte apto.'),
    (910002, 910002, '2026-10-04 09:00:00', 1, 1, 1, 'Inspeccion de prueba 02: transporte apto.'),
    (910003, 910003, '2026-10-05 09:00:00', 1, 1, 1, 'Inspeccion de prueba 03: transporte apto.'),
    (910004, 910004, '2026-10-06 09:00:00', 1, 1, 1, 'Inspeccion de prueba 04: transporte apto.'),
    (910005, 910005, '2026-10-07 09:00:00', 1, 1, 1, 'Inspeccion de prueba 05: transporte apto.'),
    (910006, 910006, '2026-10-08 09:00:00', 1, 1, 1, 'Inspeccion de prueba 06: transporte apto.'),
    (910007, 910007, '2026-10-09 09:00:00', 1, 1, 1, 'Inspeccion de prueba 07: transporte apto.'),
    (910008, 910008, '2026-10-10 09:00:00', 1, 1, 1, 'Inspeccion de prueba 08: transporte apto.'),
    (910009, 910009, '2026-10-11 09:00:00', 1, 1, 1, 'Inspeccion de prueba 09: transporte apto.'),
    (910010, 910010, '2026-10-12 09:00:00', 1, 1, 1, 'Inspeccion de prueba 10: transporte apto.');

-- [10/40] tblLoteProducto - 10 filas
INSERT INTO `tblLoteProducto` (
    `Pk_Lote_Producto`,
    `codigoLoteProducto`,
    `descripcionLoteProducto`,
    `requiereFrioLoteProducto`,
    `fechaFabricacionLoteProducto`,
    `fechaVencimientoLoteProducto`,
    `tempRequeridaMinLoteProducto`,
    `tempRequeridaMaxLoteProducto`
) VALUES
    (910001, 'LOTE-D26-01', 'Lote de prueba: Chorizo criollo', 1, '2026-09-01', '2027-06-01', -2.00, 4.00),
    (910002, 'LOTE-D26-02', 'Lote de prueba: Salchicha ahumada', 1, '2026-09-02', '2027-06-02', -2.00, 4.00),
    (910003, 'LOTE-D26-03', 'Lote de prueba: Jamon de pierna', 1, '2026-09-03', '2027-06-03', -2.00, 4.00),
    (910004, 'LOTE-D26-04', 'Lote de prueba: Longaniza artesanal', 1, '2026-09-04', '2027-06-04', -2.00, 4.00),
    (910005, 'LOTE-D26-05', 'Lote de prueba: Tocino empacado', 1, '2026-09-05', '2027-06-05', -2.00, 4.00),
    (910006, 'LOTE-D26-06', 'Lote de prueba: Mortadela especial', 1, '2026-09-06', '2027-06-06', -2.00, 4.00),
    (910007, 'LOTE-D26-07', 'Lote de prueba: Salchicha de pavo', 1, '2026-09-07', '2027-06-07', -2.00, 4.00),
    (910008, 'LOTE-D26-08', 'Lote de prueba: Chorizo picante', 1, '2026-09-08', '2027-06-08', -2.00, 4.00),
    (910009, 'LOTE-D26-09', 'Lote de prueba: Salami tradicional', 1, '2026-09-09', '2027-06-09', -2.00, 4.00),
    (910010, 'LOTE-D26-10', 'Lote de prueba: Jamon de pavo', 1, '2026-09-10', '2027-06-10', -2.00, 4.00);

-- [11/40] tblEncabezadoViaje - 10 filas
INSERT INTO `tblEncabezadoViaje` (
    `Pk_Encabezado_Viaje`,
    `fechaDocumentoEncabezadoViaje`,
    `horaSalidaEncabezadoViaje`,
    `horaLlegadaEncabezadoViaje`,
    `requiereCadenaFrioEncabezadoViaje`,
    `estadoViajeEncabezadoViaje`,
    `pesoTotalEncabezadoViaje`,
    `Fk_Transporte_Codigo`,
    `Fk_Conductor_Codigo`
) VALUES
    (910001, '2026-10-06 08:00:00', '2026-10-11 08:00:00', '2026-10-11 17:00:00', 1, 'FINALIZADO', 101.000, 910001, 910001),
    (910002, '2026-10-07 08:00:00', '2026-10-12 08:00:00', '2026-10-12 17:00:00', 1, 'FINALIZADO', 102.000, 910002, 910002),
    (910003, '2026-10-08 08:00:00', '2026-10-13 08:00:00', '2026-10-13 17:00:00', 1, 'FINALIZADO', 103.000, 910003, 910003),
    (910004, '2026-10-09 08:00:00', '2026-10-14 08:00:00', '2026-10-14 17:00:00', 1, 'FINALIZADO', 104.000, 910004, 910004),
    (910005, '2026-10-10 08:00:00', '2026-10-15 08:00:00', '2026-10-15 17:00:00', 1, 'FINALIZADO', 105.000, 910005, 910005),
    (910006, '2026-10-11 08:00:00', '2026-10-16 08:00:00', NULL, 1, 'EN_CURSO', 106.000, 910006, 910006),
    (910007, '2026-10-12 08:00:00', '2026-10-17 08:00:00', NULL, 1, 'EN_CURSO', 107.000, 910007, 910007),
    (910008, '2026-10-13 08:00:00', '2026-10-18 08:00:00', NULL, 1, 'EN_CURSO', 108.000, 910008, 910008),
    (910009, '2026-10-14 08:00:00', '2026-10-19 08:00:00', NULL, 1, 'EN_CURSO', 109.000, 910009, 910009),
    (910010, '2026-10-15 08:00:00', '2026-10-20 08:00:00', NULL, 1, 'EN_CURSO', 110.000, 910010, 910010);

-- [12/40] tblDetalleViaje - 10 filas
INSERT INTO `tblDetalleViaje` (
    `Pk_Detalle_Viaje`,
    `Fk_Encabezado_Viaje_Codigo`,
    `ordenDetalleViaje`,
    `descripcionDetalleViaje`,
    `cantidadDetalleViaje`,
    `pesoDetalleViaje`
) VALUES
    (910001, 910001, 1, 'Carga refrigerada prueba 01', 11.000, 101.000),
    (910002, 910002, 1, 'Carga refrigerada prueba 02', 12.000, 102.000),
    (910003, 910003, 1, 'Carga refrigerada prueba 03', 13.000, 103.000),
    (910004, 910004, 1, 'Carga refrigerada prueba 04', 14.000, 104.000),
    (910005, 910005, 1, 'Carga refrigerada prueba 05', 15.000, 105.000),
    (910006, 910006, 1, 'Carga refrigerada prueba 06', 16.000, 106.000),
    (910007, 910007, 1, 'Carga refrigerada prueba 07', 17.000, 107.000),
    (910008, 910008, 1, 'Carga refrigerada prueba 08', 18.000, 108.000),
    (910009, 910009, 1, 'Carga refrigerada prueba 09', 19.000, 109.000),
    (910010, 910010, 1, 'Carga refrigerada prueba 10', 20.000, 110.000);

-- [13/40] tblDetalleViajeLoteProducto - 10 filas
INSERT INTO `tblDetalleViajeLoteProducto` (
    `Pk_Detalle_Viaje_Lote_Producto`,
    `Fk_Detalle_Viaje_Codigo`,
    `Fk_Lote_Producto_Codigo`,
    `cantidadCajasDetalleViajeLoteProducto`,
    `pesoKgDetalleViajeLoteProducto`
) VALUES
    (910001, 910001, 910001, 11, 101.000),
    (910002, 910002, 910002, 12, 102.000),
    (910003, 910003, 910003, 13, 103.000),
    (910004, 910004, 910004, 14, 104.000),
    (910005, 910005, 910005, 15, 105.000),
    (910006, 910006, 910006, 16, 106.000),
    (910007, 910007, 910007, 17, 107.000),
    (910008, 910008, 910008, 18, 108.000),
    (910009, 910009, 910009, 19, 109.000),
    (910010, 910010, 910010, 20, 110.000);

-- [14/40] tblTelemetriaTemperatura - 10 filas
INSERT INTO `tblTelemetriaTemperatura` (
    `Pk_Telemetria_Temperatura`,
    `Fk_Encabezado_Viaje_Codigo`,
    `fechaHoraTelemetriaTemperatura`,
    `temperaturaCelsiusTelemetriaTemperatura`,
    `humedadRelativaTelemetriaTemperatura`,
    `puertaAbiertaTelemetriaTemperatura`,
    `ubicacionGpsTelemetriaTemperatura`
) VALUES
    (910001, 910001, '2026-10-11 10:00:00', 7.10, 0.4600, 0, 'Ruta de distribucion DEMO26 01'),
    (910002, 910002, '2026-10-12 10:00:00', 7.20, 0.4700, 0, 'Ruta de distribucion DEMO26 02'),
    (910003, 910003, '2026-10-13 10:00:00', 7.30, 0.4800, 0, 'Ruta de distribucion DEMO26 03'),
    (910004, 910004, '2026-10-14 10:00:00', 7.40, 0.4900, 0, 'Ruta de distribucion DEMO26 04'),
    (910005, 910005, '2026-10-15 10:00:00', 7.50, 0.5000, 0, 'Ruta de distribucion DEMO26 05'),
    (910006, 910006, '2026-10-16 10:00:00', 7.60, 0.5100, 0, 'Ruta de distribucion DEMO26 06'),
    (910007, 910007, '2026-10-17 10:00:00', 7.70, 0.5200, 0, 'Ruta de distribucion DEMO26 07'),
    (910008, 910008, '2026-10-18 10:00:00', 7.80, 0.5300, 0, 'Ruta de distribucion DEMO26 08'),
    (910009, 910009, '2026-10-19 10:00:00', 7.90, 0.5400, 0, 'Ruta de distribucion DEMO26 09'),
    (910010, 910010, '2026-10-20 10:00:00', 8.00, 0.5500, 0, 'Ruta de distribucion DEMO26 10');

-- [15/40] tblAlertaCadenaFrio - 10 filas
INSERT INTO `tblAlertaCadenaFrio` (
    `Pk_Alerta_Cadena_Frio`,
    `Fk_Encabezado_Viaje_Codigo`,
    `Fk_Telemetria_Temperatura_Codigo`,
    `tipoAlertaCadenaFrio`,
    `temperaturaRegistradaAlertaCadenaFrio`,
    `atendidaAlertaCadenaFrio`,
    `observacionCalidadAlertaCadenaFrio`
) VALUES
    (910001, 910001, 910001, 'TEMPERATURA_ALTA', 7.10, 1, 'Alerta de prueba 01, temperatura fuera de rango.'),
    (910002, 910002, 910002, 'TEMPERATURA_ALTA', 7.20, 1, 'Alerta de prueba 02, temperatura fuera de rango.'),
    (910003, 910003, 910003, 'TEMPERATURA_ALTA', 7.30, 1, 'Alerta de prueba 03, temperatura fuera de rango.'),
    (910004, 910004, 910004, 'TEMPERATURA_ALTA', 7.40, 1, 'Alerta de prueba 04, temperatura fuera de rango.'),
    (910005, 910005, 910005, 'TEMPERATURA_ALTA', 7.50, 1, 'Alerta de prueba 05, temperatura fuera de rango.'),
    (910006, 910006, 910006, 'TEMPERATURA_ALTA', 7.60, 0, 'Alerta de prueba 06, temperatura fuera de rango.'),
    (910007, 910007, 910007, 'TEMPERATURA_ALTA', 7.70, 0, 'Alerta de prueba 07, temperatura fuera de rango.'),
    (910008, 910008, 910008, 'TEMPERATURA_ALTA', 7.80, 0, 'Alerta de prueba 08, temperatura fuera de rango.'),
    (910009, 910009, 910009, 'TEMPERATURA_ALTA', 7.90, 0, 'Alerta de prueba 09, temperatura fuera de rango.'),
    (910010, 910010, 910010, 'TEMPERATURA_ALTA', 8.00, 0, 'Alerta de prueba 10, temperatura fuera de rango.');

-- [16/40] tblSucursal - 10 filas
INSERT INTO `tblSucursal` (
    `Pk_Sucursal`,
    `nombreSucursal`,
    `descripcionSucursal`,
    `ubicacionSucursal`,
    `paisSucursal`
) VALUES
    (910001, 'Sucursal DEMO26 Guatemala', 'Sucursal de demostracion numero 01', 'Zona comercial de Guatemala, Guatemala (direccion ficticia)', 'Guatemala'),
    (910002, 'Sucursal DEMO26 Mixco', 'Sucursal de demostracion numero 02', 'Zona comercial de Mixco, Guatemala (direccion ficticia)', 'Guatemala'),
    (910003, 'Sucursal DEMO26 Villa Nueva', 'Sucursal de demostracion numero 03', 'Zona comercial de Villa Nueva, Guatemala (direccion ficticia)', 'Guatemala'),
    (910004, 'Sucursal DEMO26 Amatitlan', 'Sucursal de demostracion numero 04', 'Zona comercial de Amatitlan, Guatemala (direccion ficticia)', 'Guatemala'),
    (910005, 'Sucursal DEMO26 Antigua Guatemala', 'Sucursal de demostracion numero 05', 'Zona comercial de Antigua Guatemala, Guatemala (direccion ficticia)', 'Guatemala'),
    (910006, 'Sucursal DEMO26 Escuintla', 'Sucursal de demostracion numero 06', 'Zona comercial de Escuintla, Guatemala (direccion ficticia)', 'Guatemala'),
    (910007, 'Sucursal DEMO26 Chimaltenango', 'Sucursal de demostracion numero 07', 'Zona comercial de Chimaltenango, Guatemala (direccion ficticia)', 'Guatemala'),
    (910008, 'Sucursal DEMO26 Quetzaltenango', 'Sucursal de demostracion numero 08', 'Zona comercial de Quetzaltenango, Guatemala (direccion ficticia)', 'Guatemala'),
    (910009, 'Sucursal DEMO26 Cobán', 'Sucursal de demostracion numero 09', 'Zona comercial de Cobán, Guatemala (direccion ficticia)', 'Guatemala'),
    (910010, 'Sucursal DEMO26 Zacapa', 'Sucursal de demostracion numero 10', 'Zona comercial de Zacapa, Guatemala (direccion ficticia)', 'Guatemala');

-- [17/40] tblBodega - 10 filas
INSERT INTO `tblBodega` (
    `Pk_Bodega`,
    `nombreBodega`,
    `Fk_Sucursal_Codigo`,
    `paisBodega`
) VALUES
    (910001, 'Bodega DEMO26 01', 910001, 'Guatemala'),
    (910002, 'Bodega DEMO26 02', 910002, 'Guatemala'),
    (910003, 'Bodega DEMO26 03', 910003, 'Guatemala'),
    (910004, 'Bodega DEMO26 04', 910004, 'Guatemala'),
    (910005, 'Bodega DEMO26 05', 910005, 'Guatemala'),
    (910006, 'Bodega DEMO26 06', 910006, 'Guatemala'),
    (910007, 'Bodega DEMO26 07', 910007, 'Guatemala'),
    (910008, 'Bodega DEMO26 08', 910008, 'Guatemala'),
    (910009, 'Bodega DEMO26 09', 910009, 'Guatemala'),
    (910010, 'Bodega DEMO26 10', 910010, 'Guatemala');

-- [18/40] tblCategoria - 10 filas
INSERT INTO `tblCategoria` (
    `Pk_Categoria`,
    `nombreCategoria`,
    `porcentajeComisionCategoria`
) VALUES
    (910001, 'Categoria DEMO26 01', 0.06),
    (910002, 'Categoria DEMO26 02', 0.07),
    (910003, 'Categoria DEMO26 03', 0.08),
    (910004, 'Categoria DEMO26 04', 0.09),
    (910005, 'Categoria DEMO26 05', 0.10),
    (910006, 'Categoria DEMO26 06', 0.11),
    (910007, 'Categoria DEMO26 07', 0.12),
    (910008, 'Categoria DEMO26 08', 0.13),
    (910009, 'Categoria DEMO26 09', 0.14),
    (910010, 'Categoria DEMO26 10', 0.15);

-- [19/40] tblLinea - 10 filas
INSERT INTO `tblLinea` (
    `Pk_Linea`,
    `nombreLinea`,
    `porcentajeComisionLinea`
) VALUES
    (910001, 'Linea DEMO26 01', 0.05),
    (910002, 'Linea DEMO26 02', 0.06),
    (910003, 'Linea DEMO26 03', 0.07),
    (910004, 'Linea DEMO26 04', 0.08),
    (910005, 'Linea DEMO26 05', 0.09),
    (910006, 'Linea DEMO26 06', 0.10),
    (910007, 'Linea DEMO26 07', 0.11),
    (910008, 'Linea DEMO26 08', 0.12),
    (910009, 'Linea DEMO26 09', 0.13),
    (910010, 'Linea DEMO26 10', 0.14);

-- [20/40] tblMarca - 10 filas
INSERT INTO `tblMarca` (
    `Pk_Marca`,
    `nombreMarca`,
    `porcentajeComisionMarca`
) VALUES
    (910001, 'Marca DEMO26 01', 0.04),
    (910002, 'Marca DEMO26 02', 0.05),
    (910003, 'Marca DEMO26 03', 0.06),
    (910004, 'Marca DEMO26 04', 0.07),
    (910005, 'Marca DEMO26 05', 0.08),
    (910006, 'Marca DEMO26 06', 0.09),
    (910007, 'Marca DEMO26 07', 0.10),
    (910008, 'Marca DEMO26 08', 0.11),
    (910009, 'Marca DEMO26 09', 0.12),
    (910010, 'Marca DEMO26 10', 0.13);

-- [21/40] tblProducto - 10 filas
INSERT INTO `tblProducto` (
    `Pk_Producto`,
    `skuProducto`,
    `clasificacionProducto`,
    `nombreProducto`,
    `Fk_Marca_Codigo`,
    `Fk_Categoria_Codigo`,
    `Fk_Linea_Codigo`,
    `descripcionProducto`,
    `imagenProducto`,
    `paisProducto`,
    `precioCompraProducto`,
    `precioVentaProducto`,
    `porcentajeComisionProducto`,
    `stockAnteriorProducto`,
    `stockActualProducto`,
    `cargosMesProducto`,
    `abonosMesProducto`,
    `cargosAcumuladosProducto`,
    `abonosAcumuladosProducto`,
    `stockMinimoProducto`,
    `stockMaximoProducto`,
    `pesoProducto`
) VALUES
    (910001, 'DEMO26-SKU-01', 'EMB01', 'Chorizo criollo DEMO26', 910001, 910001, 910001, 'Producto de prueba para inventario y traslado 01', UNHEX('89504E470D0A1A0A0000000D49484452000000010000000108060000001F15C4890000000B49444154789C636000020000050001A5F645400000000049454E44AE426082'), 'Guatemala', 11.0000, 22.0000, 0.05, 110, 114, 4, 0, 0, 0, 5, 500, 0.60),
    (910002, 'DEMO26-SKU-02', 'EMB02', 'Salchicha ahumada DEMO26', 910002, 910002, 910002, 'Producto de prueba para inventario y traslado 02', UNHEX('89504E470D0A1A0A0000000D49484452000000010000000108060000001F15C4890000000B49444154789C636000020000050001A5F645400000000049454E44AE426082'), 'Guatemala', 12.0000, 24.0000, 0.05, 120, 125, 5, 0, 0, 0, 5, 500, 0.70),
    (910003, 'DEMO26-SKU-03', 'EMB03', 'Jamon de pierna DEMO26', 910003, 910003, 910003, 'Producto de prueba para inventario y traslado 03', UNHEX('89504E470D0A1A0A0000000D49484452000000010000000108060000001F15C4890000000B49444154789C636000020000050001A5F645400000000049454E44AE426082'), 'Guatemala', 13.0000, 26.0000, 0.05, 130, 136, 6, 0, 0, 0, 5, 500, 0.80),
    (910004, 'DEMO26-SKU-04', 'EMB04', 'Longaniza artesanal DEMO26', 910004, 910004, 910004, 'Producto de prueba para inventario y traslado 04', UNHEX('89504E470D0A1A0A0000000D49484452000000010000000108060000001F15C4890000000B49444154789C636000020000050001A5F645400000000049454E44AE426082'), 'Guatemala', 14.0000, 28.0000, 0.05, 140, 147, 7, 0, 0, 0, 5, 500, 0.90),
    (910005, 'DEMO26-SKU-05', 'EMB05', 'Tocino empacado DEMO26', 910005, 910005, 910005, 'Producto de prueba para inventario y traslado 05', UNHEX('89504E470D0A1A0A0000000D49484452000000010000000108060000001F15C4890000000B49444154789C636000020000050001A5F645400000000049454E44AE426082'), 'Guatemala', 15.0000, 30.0000, 0.05, 150, 158, 8, 0, 0, 0, 5, 500, 1.00),
    (910006, 'DEMO26-SKU-06', 'EMB06', 'Mortadela especial DEMO26', 910006, 910006, 910006, 'Producto de prueba para inventario y traslado 06', UNHEX('89504E470D0A1A0A0000000D49484452000000010000000108060000001F15C4890000000B49444154789C636000020000050001A5F645400000000049454E44AE426082'), 'Guatemala', 16.0000, 32.0000, 0.05, 160, 151, 0, 9, 0, 0, 5, 500, 1.10),
    (910007, 'DEMO26-SKU-07', 'EMB07', 'Salchicha de pavo DEMO26', 910007, 910007, 910007, 'Producto de prueba para inventario y traslado 07', UNHEX('89504E470D0A1A0A0000000D49484452000000010000000108060000001F15C4890000000B49444154789C636000020000050001A5F645400000000049454E44AE426082'), 'Guatemala', 17.0000, 34.0000, 0.05, 170, 160, 0, 10, 0, 0, 5, 500, 1.20),
    (910008, 'DEMO26-SKU-08', 'EMB08', 'Chorizo picante DEMO26', 910008, 910008, 910008, 'Producto de prueba para inventario y traslado 08', UNHEX('89504E470D0A1A0A0000000D49484452000000010000000108060000001F15C4890000000B49444154789C636000020000050001A5F645400000000049454E44AE426082'), 'Guatemala', 18.0000, 36.0000, 0.05, 180, 169, 0, 11, 0, 0, 5, 500, 1.30),
    (910009, 'DEMO26-SKU-09', 'EMB09', 'Salami tradicional DEMO26', 910009, 910009, 910009, 'Producto de prueba para inventario y traslado 09', UNHEX('89504E470D0A1A0A0000000D49484452000000010000000108060000001F15C4890000000B49444154789C636000020000050001A5F645400000000049454E44AE426082'), 'Guatemala', 19.0000, 38.0000, 0.05, 190, 178, 0, 12, 0, 0, 5, 500, 1.40),
    (910010, 'DEMO26-SKU-10', 'EMB10', 'Jamon de pavo DEMO26', 910010, 910010, 910010, 'Producto de prueba para inventario y traslado 10', UNHEX('89504E470D0A1A0A0000000D49484452000000010000000108060000001F15C4890000000B49444154789C636000020000050001A5F645400000000049454E44AE426082'), 'Guatemala', 20.0000, 40.0000, 0.05, 200, 187, 0, 13, 0, 0, 5, 500, 1.50);

-- [22/40] tblStock - 10 filas
INSERT INTO `tblStock` (
    `Pk_Stock`,
    `Fk_Sucursal_Codigo`,
    `Fk_Bodega_Codigo`,
    `Fk_Producto_Codigo`,
    `stockFisicoStock`,
    `stockVirtualStock`,
    `stockAnteriorStock`,
    `stockActualStock`,
    `cargosMesStock`,
    `abonosMesStock`,
    `cargosAcumuladosStock`,
    `abonosAcumuladosStock`,
    `estadoVentaStock`,
    `estadoCompraStock`
) VALUES
    (910001, 910001, 910001, 910001, 114, 114, 110, 114, 4, 0, 0, 0, 1, 1),
    (910002, 910002, 910002, 910002, 125, 125, 120, 125, 5, 0, 0, 0, 1, 1),
    (910003, 910003, 910003, 910003, 136, 136, 130, 136, 6, 0, 0, 0, 1, 1),
    (910004, 910004, 910004, 910004, 147, 147, 140, 147, 7, 0, 0, 0, 1, 1),
    (910005, 910005, 910005, 910005, 158, 158, 150, 158, 8, 0, 0, 0, 1, 1),
    (910006, 910006, 910006, 910006, 151, 151, 160, 151, 0, 9, 0, 0, 1, 1),
    (910007, 910007, 910007, 910007, 160, 160, 170, 160, 0, 10, 0, 0, 1, 1),
    (910008, 910008, 910008, 910008, 169, 169, 180, 169, 0, 11, 0, 0, 1, 1),
    (910009, 910009, 910009, 910009, 178, 178, 190, 178, 0, 12, 0, 0, 1, 1),
    (910010, 910010, 910010, 910010, 187, 187, 200, 187, 0, 13, 0, 0, 1, 1);

-- [23/40] tblTipoMovimiento - 10 filas
INSERT INTO `tblTipoMovimiento` (
    `Pk_Tipo_Movimiento`,
    `nombreTipoMovimiento`,
    `descripcionTipoMovimiento`,
    `signoTipoMovimiento`
) VALUES
    (910001, 'COMPRA DEMO26', 'Tipo de movimiento de prueba 01', '+'),
    (910002, 'DEVOLUCION_CLIENTE DEMO26', 'Tipo de movimiento de prueba 02', '+'),
    (910003, 'AJUSTE_POSITIVO DEMO26', 'Tipo de movimiento de prueba 03', '+'),
    (910004, 'INGRESO_PRODUCCION DEMO26', 'Tipo de movimiento de prueba 04', '+'),
    (910005, 'RECEPCION_TRASLADO DEMO26', 'Tipo de movimiento de prueba 05', '+'),
    (910006, 'VENTA DEMO26', 'Tipo de movimiento de prueba 06', '-'),
    (910007, 'DEVOLUCION_PROVEEDOR DEMO26', 'Tipo de movimiento de prueba 07', '-'),
    (910008, 'AJUSTE_NEGATIVO DEMO26', 'Tipo de movimiento de prueba 08', '-'),
    (910009, 'CONSUMO_INTERNO DEMO26', 'Tipo de movimiento de prueba 09', '-'),
    (910010, 'SALIDA_TRASLADO DEMO26', 'Tipo de movimiento de prueba 10', '-');

-- [24/40] tblCierreMovimiento - 10 filas
INSERT INTO `tblCierreMovimiento` (
    `Pk_Cierre_Movimiento`,
    `nombreCierreMovimiento`,
    `fechaInicioCierreMovimiento`,
    `fechaFinCierreMovimiento`,
    `estadoCierreMovimiento`,
    `esCierreAnualCierreMovimiento`,
    `toleranciaCierreMovimiento`,
    `diferenciaUnidadesCierreMovimiento`,
    `diferenciaMontoCierreMovimiento`,
    `motivoAceptacionCierreMovimiento`,
    `motivoAnulacionCierreMovimiento`,
    `observacionesCierreMovimiento`,
    `fechaEjecucionCierreMovimiento`
) VALUES
    (910001, 'Cierre DEMO26 Diciembre 2025', '2025-12-01', '2025-12-31', 'CERRADO', 1, 0, 0, 0.0000, 'Sin diferencias registradas', 'No aplica', 'Cierre historico de prueba', '2026-01-02 09:00:00'),
    (910002, 'Cierre DEMO26 Enero 2026', '2026-01-01', '2026-01-31', 'ABIERTO', 0, 0, 0, 0.0000, 'Pendiente de validacion', 'No aplica', 'Periodo listo para pruebas de cierre', NULL),
    (910003, 'Cierre DEMO26 Febrero 2026', '2026-02-01', '2026-02-28', 'ABIERTO', 0, 0, 0, 0.0000, 'Pendiente de validacion', 'No aplica', 'Periodo listo para pruebas de cierre', NULL),
    (910004, 'Cierre DEMO26 Marzo 2026', '2026-03-01', '2026-03-31', 'ABIERTO', 0, 0, 0, 0.0000, 'Pendiente de validacion', 'No aplica', 'Periodo listo para pruebas de cierre', NULL),
    (910005, 'Cierre DEMO26 Abril 2026', '2026-04-01', '2026-04-30', 'ABIERTO', 0, 0, 0, 0.0000, 'Pendiente de validacion', 'No aplica', 'Periodo listo para pruebas de cierre', NULL),
    (910006, 'Cierre DEMO26 Mayo 2026', '2026-05-01', '2026-05-31', 'ABIERTO', 0, 0, 0, 0.0000, 'Pendiente de validacion', 'No aplica', 'Periodo listo para pruebas de cierre', NULL),
    (910007, 'Cierre DEMO26 Junio 2026', '2026-06-01', '2026-06-30', 'ABIERTO', 0, 0, 0, 0.0000, 'Pendiente de validacion', 'No aplica', 'Periodo listo para pruebas de cierre', NULL),
    (910008, 'Cierre DEMO26 Julio 2026', '2026-07-01', '2026-07-31', 'ABIERTO', 0, 0, 0, 0.0000, 'Pendiente de validacion', 'No aplica', 'Periodo listo para pruebas de cierre', NULL),
    (910009, 'Cierre DEMO26 Agosto 2026', '2026-08-01', '2026-08-31', 'ABIERTO', 0, 0, 0, 0.0000, 'Pendiente de validacion', 'No aplica', 'Periodo listo para pruebas de cierre', NULL),
    (910010, 'Cierre DEMO26 Septiembre 2026', '2026-09-01', '2026-09-30', 'ABIERTO', 0, 0, 0, 0.0000, 'Pendiente de validacion', 'No aplica', 'Periodo listo para pruebas de cierre', NULL);

-- [25/40] tblMovimientoInventario - 10 filas
INSERT INTO `tblMovimientoInventario` (
    `Pk_Movimiento_Inventario`,
    `Fk_Tipo_Movimiento_Codigo`,
    `Fk_Bodega_Origen_Codigo`,
    `Fk_Bodega_Destino_Codigo`,
    `Fk_Transporte_Codigo`,
    `fechaMovimientoInventario`,
    `observacionesMovimientoInventario`,
    `estadoMovimientoInventario`,
    `Fk_Cierre_Movimiento_Codigo`
) VALUES
    (910001, 910001, NULL, 910001, NULL, '2026-01-07 10:00:00', 'Movimiento cuadrado de demostracion 01', 'CUADRADO', NULL),
    (910002, 910002, NULL, 910002, NULL, '2026-01-08 10:00:00', 'Movimiento cuadrado de demostracion 02', 'CUADRADO', NULL),
    (910003, 910003, NULL, 910003, NULL, '2026-01-09 10:00:00', 'Movimiento cuadrado de demostracion 03', 'CUADRADO', NULL),
    (910004, 910004, NULL, 910004, NULL, '2026-01-10 10:00:00', 'Movimiento cuadrado de demostracion 04', 'CUADRADO', NULL),
    (910005, 910005, NULL, 910005, 910005, '2026-01-11 10:00:00', 'Movimiento cuadrado de demostracion 05', 'CUADRADO', NULL),
    (910006, 910006, 910006, NULL, NULL, '2026-01-12 10:00:00', 'Movimiento cuadrado de demostracion 06', 'CUADRADO', NULL),
    (910007, 910007, 910007, NULL, NULL, '2026-01-13 10:00:00', 'Movimiento cuadrado de demostracion 07', 'CUADRADO', NULL),
    (910008, 910008, 910008, NULL, NULL, '2026-01-14 10:00:00', 'Movimiento cuadrado de demostracion 08', 'CUADRADO', NULL),
    (910009, 910009, 910009, NULL, NULL, '2026-01-15 10:00:00', 'Movimiento cuadrado de demostracion 09', 'CUADRADO', NULL),
    (910010, 910010, 910010, NULL, 910010, '2026-01-16 10:00:00', 'Movimiento cuadrado de demostracion 10', 'CUADRADO', NULL);

-- [26/40] tblDetalleMovimiento - 10 filas
INSERT INTO `tblDetalleMovimiento` (
    `Pk_Detalle_Movimiento`,
    `Fk_Movimiento_Inventario_Codigo`,
    `loteDetalleMovimiento`,
    `Fk_Producto_Codigo`,
    `cantidadDetalleMovimiento`,
    `precioCompraDetalleMovimiento`
) VALUES
    (910001, 910001, 'LOTE-D26-01', 910001, 4, 11.0000),
    (910002, 910002, 'LOTE-D26-02', 910002, 5, 12.0000),
    (910003, 910003, 'LOTE-D26-03', 910003, 6, 13.0000),
    (910004, 910004, 'LOTE-D26-04', 910004, 7, 14.0000),
    (910005, 910005, 'LOTE-D26-05', 910005, 8, 15.0000),
    (910006, 910006, 'LOTE-D26-06', 910006, 9, 16.0000),
    (910007, 910007, 'LOTE-D26-07', 910007, 10, 17.0000),
    (910008, 910008, 'LOTE-D26-08', 910008, 11, 18.0000),
    (910009, 910009, 'LOTE-D26-09', 910009, 12, 19.0000),
    (910010, 910010, 'LOTE-D26-10', 910010, 13, 20.0000);

-- [27/40] tblTraslado - 10 filas
INSERT INTO `tblTraslado` (
    `Pk_Traslado`,
    `codigoTraslado`,
    `Fk_Bodega_Origen_Codigo`,
    `Fk_Bodega_Destino_Codigo`,
    `Fk_Transporte_Codigo`,
    `fechaSolicitudTraslado`,
    `fechaSalidaTraslado`,
    `fechaLlegadaTraslado`,
    `estadoTraslado`,
    `observacionesTraslado`
) VALUES
    (910001, 'TR-DEMO26-0001', 910001, 910002, NULL, '2026-10-07 09:00:00', NULL, NULL, 'SOLICITADO', 'Traslado de prueba 01, pendiente de envio'),
    (910002, 'TR-DEMO26-0002', 910002, 910003, NULL, '2026-10-08 09:00:00', NULL, NULL, 'SOLICITADO', 'Traslado de prueba 02, pendiente de envio'),
    (910003, 'TR-DEMO26-0003', 910003, 910004, NULL, '2026-10-09 09:00:00', NULL, NULL, 'SOLICITADO', 'Traslado de prueba 03, pendiente de envio'),
    (910004, 'TR-DEMO26-0004', 910004, 910005, NULL, '2026-10-10 09:00:00', NULL, NULL, 'SOLICITADO', 'Traslado de prueba 04, pendiente de envio'),
    (910005, 'TR-DEMO26-0005', 910005, 910006, NULL, '2026-10-11 09:00:00', NULL, NULL, 'SOLICITADO', 'Traslado de prueba 05, pendiente de envio'),
    (910006, 'TR-DEMO26-0006', 910006, 910007, NULL, '2026-10-12 09:00:00', NULL, NULL, 'SOLICITADO', 'Traslado de prueba 06, pendiente de envio'),
    (910007, 'TR-DEMO26-0007', 910007, 910008, NULL, '2026-10-13 09:00:00', NULL, NULL, 'SOLICITADO', 'Traslado de prueba 07, pendiente de envio'),
    (910008, 'TR-DEMO26-0008', 910008, 910009, NULL, '2026-10-14 09:00:00', NULL, NULL, 'SOLICITADO', 'Traslado de prueba 08, pendiente de envio'),
    (910009, 'TR-DEMO26-0009', 910009, 910010, NULL, '2026-10-15 09:00:00', NULL, NULL, 'SOLICITADO', 'Traslado de prueba 09, pendiente de envio'),
    (910010, 'TR-DEMO26-0010', 910010, 910001, NULL, '2026-10-16 09:00:00', NULL, NULL, 'SOLICITADO', 'Traslado de prueba 10, pendiente de envio');

-- [28/40] tblDetalleTraslado - 10 filas
INSERT INTO `tblDetalleTraslado` (
    `Pk_Detalle_Traslado`,
    `Fk_Traslado_Codigo`,
    `Fk_Producto_Codigo`,
    `cantidadDetalleTraslado`
) VALUES
    (910001, 910001, 910001, 3),
    (910002, 910002, 910002, 4),
    (910003, 910003, 910003, 5),
    (910004, 910004, 910004, 6),
    (910005, 910005, 910005, 7),
    (910006, 910006, 910006, 8),
    (910007, 910007, 910007, 9),
    (910008, 910008, 910008, 10),
    (910009, 910009, 910009, 11),
    (910010, 910010, 910010, 12);

-- [29/40] tblAuditoriaInventario - 10 filas
INSERT INTO `tblAuditoriaInventario` (
    `Pk_Auditoria_Inventario`,
    `Fk_Movimiento_Inventario_Codigo`,
    `fechaAuditoriaInventario`,
    `resultadoAuditoriaInventario`,
    `observacionesCalidadAuditoriaInventario`,
    `detallesAuditoriaInventario`
) VALUES
    (910001, 910001, '2026-01-22 12:00:00', 'CONFORME', 'Auditoria demostrativa 01: cantidades verificadas.', 'Producto Chorizo criollo, registro de movimiento 01, sin diferencia.'),
    (910002, 910002, '2026-01-22 13:00:00', 'CONFORME', 'Auditoria demostrativa 02: cantidades verificadas.', 'Producto Salchicha ahumada, registro de movimiento 02, sin diferencia.'),
    (910003, 910003, '2026-01-22 14:00:00', 'CONFORME', 'Auditoria demostrativa 03: cantidades verificadas.', 'Producto Jamon de pierna, registro de movimiento 03, sin diferencia.'),
    (910004, 910004, '2026-01-22 15:00:00', 'CONFORME', 'Auditoria demostrativa 04: cantidades verificadas.', 'Producto Longaniza artesanal, registro de movimiento 04, sin diferencia.'),
    (910005, 910005, '2026-01-22 16:00:00', 'CONFORME', 'Auditoria demostrativa 05: cantidades verificadas.', 'Producto Tocino empacado, registro de movimiento 05, sin diferencia.'),
    (910006, 910006, '2026-01-22 17:00:00', 'CONFORME', 'Auditoria demostrativa 06: cantidades verificadas.', 'Producto Mortadela especial, registro de movimiento 06, sin diferencia.'),
    (910007, 910007, '2026-01-22 18:00:00', 'CONFORME', 'Auditoria demostrativa 07: cantidades verificadas.', 'Producto Salchicha de pavo, registro de movimiento 07, sin diferencia.'),
    (910008, 910008, '2026-01-22 19:00:00', 'CONFORME', 'Auditoria demostrativa 08: cantidades verificadas.', 'Producto Chorizo picante, registro de movimiento 08, sin diferencia.'),
    (910009, 910009, '2026-01-22 20:00:00', 'CONFORME', 'Auditoria demostrativa 09: cantidades verificadas.', 'Producto Salami tradicional, registro de movimiento 09, sin diferencia.'),
    (910010, 910010, '2026-01-22 21:00:00', 'CONFORME', 'Auditoria demostrativa 10: cantidades verificadas.', 'Producto Jamon de pavo, registro de movimiento 10, sin diferencia.');

-- [30/40] tblDetalleCierre - 10 filas
INSERT INTO `tblDetalleCierre` (
    `Pk_Detalle_Cierre`,
    `Fk_Cierre_Movimiento_Codigo`,
    `Fk_Stock_Codigo`,
    `stockAnteriorDetalleCierre`,
    `stockActualDetalleCierre`,
    `cargosMesDetalleCierre`,
    `abonosMesDetalleCierre`,
    `cargosAcumuladosDetalleCierre`,
    `abonosAcumuladosDetalleCierre`
) VALUES
    (910001, 910001, 910001, 110, 110, 0, 0, 0, 0),
    (910002, 910001, 910002, 120, 120, 0, 0, 0, 0),
    (910003, 910001, 910003, 130, 130, 0, 0, 0, 0),
    (910004, 910001, 910004, 140, 140, 0, 0, 0, 0),
    (910005, 910001, 910005, 150, 150, 0, 0, 0, 0),
    (910006, 910001, 910006, 160, 160, 0, 0, 0, 0),
    (910007, 910001, 910007, 170, 170, 0, 0, 0, 0),
    (910008, 910001, 910008, 180, 180, 0, 0, 0, 0),
    (910009, 910001, 910009, 190, 190, 0, 0, 0, 0),
    (910010, 910001, 910010, 200, 200, 0, 0, 0, 0);

-- [31/40] tblHistoricoInventario - 10 filas
INSERT INTO `tblHistoricoInventario` (
    `Pk_Historico_Inventario`,
    `Fk_Cierre_Movimiento_Codigo`,
    `Fk_Stock_Codigo`,
    `Fk_Producto_Codigo`,
    `Fk_Sucursal_Codigo`,
    `Fk_Bodega_Codigo`,
    `fechaCorteHistoricoInventario`,
    `stockAnteriorHistoricoInventario`,
    `stockActualHistoricoInventario`,
    `cargosMesHistoricoInventario`,
    `abonosMesHistoricoInventario`,
    `cargosAcumuladosHistoricoInventario`,
    `abonosAcumuladosHistoricoInventario`
) VALUES
    (910001, 910001, 910001, 910001, 910001, 910001, '2025-12-31', 110, 110, 0, 0, 0, 0),
    (910002, 910001, 910002, 910002, 910002, 910002, '2025-12-31', 120, 120, 0, 0, 0, 0),
    (910003, 910001, 910003, 910003, 910003, 910003, '2025-12-31', 130, 130, 0, 0, 0, 0),
    (910004, 910001, 910004, 910004, 910004, 910004, '2025-12-31', 140, 140, 0, 0, 0, 0),
    (910005, 910001, 910005, 910005, 910005, 910005, '2025-12-31', 150, 150, 0, 0, 0, 0),
    (910006, 910001, 910006, 910006, 910006, 910006, '2025-12-31', 160, 160, 0, 0, 0, 0),
    (910007, 910001, 910007, 910007, 910007, 910007, '2025-12-31', 170, 170, 0, 0, 0, 0),
    (910008, 910001, 910008, 910008, 910008, 910008, '2025-12-31', 180, 180, 0, 0, 0, 0),
    (910009, 910001, 910009, 910009, 910009, 910009, '2025-12-31', 190, 190, 0, 0, 0, 0),
    (910010, 910001, 910010, 910010, 910010, 910010, '2025-12-31', 200, 200, 0, 0, 0, 0);

-- [32/40] tblServicioTransporteExterno - 10 filas
INSERT INTO `tblServicioTransporteExterno` (
    `Pk_Servicio_Transporte_Externo`,
    `Fk_Empresa_Transportista_Codigo`,
    `Fk_Tipo_Servicio_Codigo`,
    `Fk_Traslado_Codigo`,
    `numeroGuiaServicioTransporteExterno`,
    `estadoServicioTransporteExterno`,
    `origenServicioTransporteExterno`,
    `direccionOrigenServicioTransporteExterno`,
    `destinoServicioTransporteExterno`,
    `direccionDestinoServicioTransporteExterno`,
    `fechaContratacionServicioTransporteExterno`,
    `fechaEstimadaEntregaServicioTransporteExterno`,
    `fechaEntregaRealServicioTransporteExterno`
) VALUES
    (910001, 910001, 910001, 910001, 'GUIA-D26-0001', 'Programado', 'Guatemala', 'Bodega DEMO26 01 (direccion de prueba)', 'Mixco', 'Bodega DEMO26 02 (direccion de prueba)', '2026-10-11', '2026-10-29', NULL),
    (910002, 910002, 910002, 910002, 'GUIA-D26-0002', 'Programado', 'Mixco', 'Bodega DEMO26 02 (direccion de prueba)', 'Villa Nueva', 'Bodega DEMO26 03 (direccion de prueba)', '2026-10-12', '2026-10-30', NULL),
    (910003, 910003, 910003, 910003, 'GUIA-D26-0003', 'Programado', 'Villa Nueva', 'Bodega DEMO26 03 (direccion de prueba)', 'Amatitlan', 'Bodega DEMO26 04 (direccion de prueba)', '2026-10-13', '2026-10-31', NULL),
    (910004, 910004, 910004, 910004, 'GUIA-D26-0004', 'Programado', 'Amatitlan', 'Bodega DEMO26 04 (direccion de prueba)', 'Antigua Guatemala', 'Bodega DEMO26 05 (direccion de prueba)', '2026-10-14', '2026-11-01', NULL),
    (910005, 910005, 910005, 910005, 'GUIA-D26-0005', 'Programado', 'Antigua Guatemala', 'Bodega DEMO26 05 (direccion de prueba)', 'Escuintla', 'Bodega DEMO26 06 (direccion de prueba)', '2026-10-15', '2026-11-02', NULL),
    (910006, 910006, 910006, 910006, 'GUIA-D26-0006', 'Programado', 'Escuintla', 'Bodega DEMO26 06 (direccion de prueba)', 'Chimaltenango', 'Bodega DEMO26 07 (direccion de prueba)', '2026-10-16', '2026-11-03', NULL),
    (910007, 910007, 910007, 910007, 'GUIA-D26-0007', 'Programado', 'Chimaltenango', 'Bodega DEMO26 07 (direccion de prueba)', 'Quetzaltenango', 'Bodega DEMO26 08 (direccion de prueba)', '2026-10-17', '2026-11-04', NULL),
    (910008, 910008, 910008, 910008, 'GUIA-D26-0008', 'Programado', 'Quetzaltenango', 'Bodega DEMO26 08 (direccion de prueba)', 'Cobán', 'Bodega DEMO26 09 (direccion de prueba)', '2026-10-18', '2026-11-05', NULL),
    (910009, 910009, 910009, 910009, 'GUIA-D26-0009', 'Programado', 'Cobán', 'Bodega DEMO26 09 (direccion de prueba)', 'Zacapa', 'Bodega DEMO26 10 (direccion de prueba)', '2026-10-19', '2026-11-06', NULL),
    (910010, 910010, 910010, 910010, 'GUIA-D26-0010', 'Programado', 'Zacapa', 'Bodega DEMO26 10 (direccion de prueba)', 'Guatemala', 'Bodega DEMO26 01 (direccion de prueba)', '2026-10-20', '2026-11-07', NULL);

-- [33/40] tblCostoTransporte - 10 filas
INSERT INTO `tblCostoTransporte` (
    `Pk_Costo_Transporte`,
    `Fk_Servicio_Transporte_Externo_Codigo`,
    `Fk_Tarifa_Transporte_Codigo`,
    `cantidadCostoTransporte`,
    `costoUnitarioCostoTransporte`,
    `costoTotalCostoTransporte`,
    `monedaCostoTransporte`,
    `tipoPagoCostoTransporte`
) VALUES
    (910001, 910001, 910001, 1.0000, 315.0000, 315.0000, 'GTQ', 'Transferencia'),
    (910002, 910002, 910002, 1.0000, 330.0000, 330.0000, 'GTQ', 'Transferencia'),
    (910003, 910003, 910003, 1.0000, 345.0000, 345.0000, 'GTQ', 'Transferencia'),
    (910004, 910004, 910004, 1.0000, 360.0000, 360.0000, 'GTQ', 'Transferencia'),
    (910005, 910005, 910005, 1.0000, 375.0000, 375.0000, 'GTQ', 'Transferencia'),
    (910006, 910006, 910006, 1.0000, 390.0000, 390.0000, 'GTQ', 'Transferencia'),
    (910007, 910007, 910007, 1.0000, 405.0000, 405.0000, 'GTQ', 'Transferencia'),
    (910008, 910008, 910008, 1.0000, 420.0000, 420.0000, 'GTQ', 'Transferencia'),
    (910009, 910009, 910009, 1.0000, 435.0000, 435.0000, 'GTQ', 'Transferencia'),
    (910010, 910010, 910010, 1.0000, 450.0000, 450.0000, 'GTQ', 'Transferencia');

-- [34/40] tblVehiculoTransporte - 10 filas
INSERT INTO `tblVehiculoTransporte` (
    `Pk_Vehiculo_Transporte`,
    `Fk_Servicio_Transporte_Externo_Codigo`,
    `placaVehiculoTransporte`,
    `tipoVehiculoTransporte`,
    `capacidadVehiculoTransporte`
) VALUES
    (910001, 910001, 'EXT-D26-001', 'Camion refrigerado', '1200 kg'),
    (910002, 910002, 'EXT-D26-002', 'Camion refrigerado', '1200 kg'),
    (910003, 910003, 'EXT-D26-003', 'Camion refrigerado', '1200 kg'),
    (910004, 910004, 'EXT-D26-004', 'Camion refrigerado', '1200 kg'),
    (910005, 910005, 'EXT-D26-005', 'Camion refrigerado', '1200 kg'),
    (910006, 910006, 'EXT-D26-006', 'Camion refrigerado', '1200 kg'),
    (910007, 910007, 'EXT-D26-007', 'Camion refrigerado', '1200 kg'),
    (910008, 910008, 'EXT-D26-008', 'Camion refrigerado', '1200 kg'),
    (910009, 910009, 'EXT-D26-009', 'Camion refrigerado', '1200 kg'),
    (910010, 910010, 'EXT-D26-010', 'Camion refrigerado', '1200 kg');

-- [35/40] tblConductorTransporte - 10 filas
INSERT INTO `tblConductorTransporte` (
    `Pk_Conductor_Transporte`,
    `Fk_Servicio_Transporte_Externo_Codigo`,
    `nombreConductorTransporte`,
    `telefonoConductorTransporte`
) VALUES
    (910001, 910001, 'Carlos Lopez', '5551-1001'),
    (910002, 910002, 'Ana Hernandez', '5551-1002'),
    (910003, 910003, 'Mario Morales', '5551-1003'),
    (910004, 910004, 'Luisa Perez', '5551-1004'),
    (910005, 910005, 'Pedro Ramirez', '5551-1005'),
    (910006, 910006, 'Marta Gomez', '5551-1006'),
    (910007, 910007, 'Jorge Castillo', '5551-1007'),
    (910008, 910008, 'Elena Rivera', '5551-1008'),
    (910009, 910009, 'Oscar Garcia', '5551-1009'),
    (910010, 910010, 'Diana Flores', '5551-1010');

-- [36/40] tblProductoTransporte - 10 filas
INSERT INTO `tblProductoTransporte` (
    `Pk_Producto_Transporte`,
    `Fk_Servicio_Transporte_Externo_Codigo`,
    `Fk_Producto_Codigo`,
    `cantidadProductoTransporte`,
    `unidadMedidaProductoTransporte`
) VALUES
    (910001, 910001, 910001, 6.0000, 'Caja'),
    (910002, 910002, 910002, 7.0000, 'Caja'),
    (910003, 910003, 910003, 8.0000, 'Caja'),
    (910004, 910004, 910004, 9.0000, 'Caja'),
    (910005, 910005, 910005, 10.0000, 'Caja'),
    (910006, 910006, 910006, 11.0000, 'Caja'),
    (910007, 910007, 910007, 12.0000, 'Caja'),
    (910008, 910008, 910008, 13.0000, 'Caja'),
    (910009, 910009, 910009, 14.0000, 'Caja'),
    (910010, 910010, 910010, 15.0000, 'Caja');

-- [37/40] tblUbicacionTransporte - 10 filas
INSERT INTO `tblUbicacionTransporte` (
    `Pk_Ubicacion_Transporte`,
    `Fk_Servicio_Transporte_Externo_Codigo`,
    `ubicacionActualUbicacionTransporte`,
    `fechaActualizacionUbicacionTransporte`
) VALUES
    (910001, 910001, 'Pendiente de salida desde Guatemala', '2026-10-25 08:01:00'),
    (910002, 910002, 'Pendiente de salida desde Mixco', '2026-10-25 08:02:00'),
    (910003, 910003, 'Pendiente de salida desde Villa Nueva', '2026-10-25 08:03:00'),
    (910004, 910004, 'Pendiente de salida desde Amatitlan', '2026-10-25 08:04:00'),
    (910005, 910005, 'Pendiente de salida desde Antigua Guatemala', '2026-10-25 08:05:00'),
    (910006, 910006, 'Pendiente de salida desde Escuintla', '2026-10-25 08:06:00'),
    (910007, 910007, 'Pendiente de salida desde Chimaltenango', '2026-10-25 08:07:00'),
    (910008, 910008, 'Pendiente de salida desde Quetzaltenango', '2026-10-25 08:08:00'),
    (910009, 910009, 'Pendiente de salida desde Cobán', '2026-10-25 08:09:00'),
    (910010, 910010, 'Pendiente de salida desde Zacapa', '2026-10-25 08:10:00');

-- [38/40] tblIncidenciaTransporte - 10 filas
INSERT INTO `tblIncidenciaTransporte` (
    `Pk_Incidencia_Transporte`,
    `Fk_Servicio_Transporte_Externo_Codigo`,
    `codigoIncidenciaTransporte`,
    `fechaIncidenciaTransporte`,
    `tipoIncidenciaTransporte`,
    `descripcionIncidenciaTransporte`,
    `responsableIncidenciaTransporte`,
    `estadoIncidenciaTransporte`
) VALUES
    (910001, 910001, 'INC-D26-001', '2026-10-20 11:01:00', 'Consulta de programacion', 'Incidencia de prueba 01: confirmacion de horario pendiente.', 'Carlos Lopez', 'Pendiente'),
    (910002, 910002, 'INC-D26-002', '2026-10-20 11:02:00', 'Consulta de programacion', 'Incidencia de prueba 02: confirmacion de horario pendiente.', 'Ana Hernandez', 'Pendiente'),
    (910003, 910003, 'INC-D26-003', '2026-10-20 11:03:00', 'Consulta de programacion', 'Incidencia de prueba 03: confirmacion de horario pendiente.', 'Mario Morales', 'Pendiente'),
    (910004, 910004, 'INC-D26-004', '2026-10-20 11:04:00', 'Consulta de programacion', 'Incidencia de prueba 04: confirmacion de horario pendiente.', 'Luisa Perez', 'Pendiente'),
    (910005, 910005, 'INC-D26-005', '2026-10-20 11:05:00', 'Consulta de programacion', 'Incidencia de prueba 05: confirmacion de horario pendiente.', 'Pedro Ramirez', 'Pendiente'),
    (910006, 910006, 'INC-D26-006', '2026-10-20 11:06:00', 'Consulta de programacion', 'Incidencia de prueba 06: confirmacion de horario pendiente.', 'Marta Gomez', 'Pendiente'),
    (910007, 910007, 'INC-D26-007', '2026-10-20 11:07:00', 'Consulta de programacion', 'Incidencia de prueba 07: confirmacion de horario pendiente.', 'Jorge Castillo', 'Pendiente'),
    (910008, 910008, 'INC-D26-008', '2026-10-20 11:08:00', 'Consulta de programacion', 'Incidencia de prueba 08: confirmacion de horario pendiente.', 'Elena Rivera', 'Pendiente'),
    (910009, 910009, 'INC-D26-009', '2026-10-20 11:09:00', 'Consulta de programacion', 'Incidencia de prueba 09: confirmacion de horario pendiente.', 'Oscar Garcia', 'Pendiente'),
    (910010, 910010, 'INC-D26-010', '2026-10-20 11:10:00', 'Consulta de programacion', 'Incidencia de prueba 10: confirmacion de horario pendiente.', 'Diana Flores', 'Pendiente');

-- [39/40] tblHistorialTransporte - 10 filas
INSERT INTO `tblHistorialTransporte` (
    `Pk_Historial_Transporte`,
    `Fk_Servicio_Transporte_Externo_Codigo`,
    `fechaMovimientoHistorialTransporte`,
    `tipoMovimientoHistorialTransporte`,
    `descripcionMovimientoHistorialTransporte`,
    `estadoTransporteHistorialTransporte`
) VALUES
    (910001, 910001, '2026-10-20 12:01:00', 'PROGRAMACION', 'Registro inicial de servicio DEMO26 01', 'Programado'),
    (910002, 910002, '2026-10-20 12:02:00', 'PROGRAMACION', 'Registro inicial de servicio DEMO26 02', 'Programado'),
    (910003, 910003, '2026-10-20 12:03:00', 'PROGRAMACION', 'Registro inicial de servicio DEMO26 03', 'Programado'),
    (910004, 910004, '2026-10-20 12:04:00', 'PROGRAMACION', 'Registro inicial de servicio DEMO26 04', 'Programado'),
    (910005, 910005, '2026-10-20 12:05:00', 'PROGRAMACION', 'Registro inicial de servicio DEMO26 05', 'Programado'),
    (910006, 910006, '2026-10-20 12:06:00', 'PROGRAMACION', 'Registro inicial de servicio DEMO26 06', 'Programado'),
    (910007, 910007, '2026-10-20 12:07:00', 'PROGRAMACION', 'Registro inicial de servicio DEMO26 07', 'Programado'),
    (910008, 910008, '2026-10-20 12:08:00', 'PROGRAMACION', 'Registro inicial de servicio DEMO26 08', 'Programado'),
    (910009, 910009, '2026-10-20 12:09:00', 'PROGRAMACION', 'Registro inicial de servicio DEMO26 09', 'Programado'),
    (910010, 910010, '2026-10-20 12:10:00', 'PROGRAMACION', 'Registro inicial de servicio DEMO26 10', 'Programado');

-- [40/40] tblSolicitudPagoTransporte - 10 filas
INSERT INTO `tblSolicitudPagoTransporte` (
    `Pk_Solicitud_Pago_Transporte`,
    `Fk_Servicio_Transporte_Externo_Codigo`,
    `codigoSolicitudPagoTransporte`,
    `estadoSolicitudPagoTransporte`,
    `referenciaBancariaSolicitudPagoTransporte`,
    `fechaPagoSolicitudPagoTransporte`
) VALUES
    (910001, 910001, 'PAGO-D26-0001', 'Solicitado', NULL, NULL),
    (910002, 910002, 'PAGO-D26-0002', 'Solicitado', NULL, NULL),
    (910003, 910003, 'PAGO-D26-0003', 'Solicitado', NULL, NULL),
    (910004, 910004, 'PAGO-D26-0004', 'Solicitado', NULL, NULL),
    (910005, 910005, 'PAGO-D26-0005', 'Solicitado', NULL, NULL),
    (910006, 910006, 'PAGO-D26-0006', 'Solicitado', NULL, NULL),
    (910007, 910007, 'PAGO-D26-0007', 'Solicitado', NULL, NULL),
    (910008, 910008, 'PAGO-D26-0008', 'Solicitado', NULL, NULL),
    (910009, 910009, 'PAGO-D26-0009', 'Solicitado', NULL, NULL),
    (910010, 910010, 'PAGO-D26-0010', 'Solicitado', NULL, NULL);

COMMIT;

-- Resultado esperado: 10 filas de prueba en cada una de las 40 tablas (400 en total).
-- Revisar el archivo VERIFICAR_10_REGISTROS_dbLogistica.sql para validar todos.
