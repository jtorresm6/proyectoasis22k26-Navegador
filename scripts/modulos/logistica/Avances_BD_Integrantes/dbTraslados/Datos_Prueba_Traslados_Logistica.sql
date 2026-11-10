-- ============================================================================
-- TERMINUS ERP - LOGISTICA - CASOS DE PRUEBA PARA TRASLADOS
-- Ejecutar despues de crear dbLogistica con dbLogistica.sql.
-- Script aditivo e idempotente: conserva los registros existentes y evita
-- duplicar los catalogos y casos de prueba identificados por sus codigos.
-- ============================================================================
USE dbLogistica;
START TRANSACTION;

-- Estados y tipo de transporte usados unicamente cuando aun no existen.
INSERT INTO tblEstado (tipoEstado)
SELECT 'DISPONIBLE'
WHERE NOT EXISTS (
    SELECT 1 FROM tblEstado WHERE UPPER(TRIM(tipoEstado)) = 'DISPONIBLE'
);
INSERT INTO tblEstado (tipoEstado)
SELECT 'NO DISPONIBLE'
WHERE NOT EXISTS (
    SELECT 1 FROM tblEstado WHERE UPPER(TRIM(tipoEstado)) = 'NO DISPONIBLE'
);
SET @EstadoDisponible = (
    SELECT idEstado FROM tblEstado
    WHERE UPPER(TRIM(tipoEstado)) = 'DISPONIBLE' LIMIT 1
);
SET @EstadoNoDisponible = (
    SELECT idEstado FROM tblEstado
    WHERE UPPER(TRIM(tipoEstado)) = 'NO DISPONIBLE' LIMIT 1
);

INSERT INTO tblTipoTransporte (nombreTipoTransporte)
SELECT 'Camion refrigerado 3.5 t - QA Traslados'
WHERE NOT EXISTS (
    SELECT 1 FROM tblTipoTransporte
    WHERE nombreTipoTransporte = 'Camion refrigerado 3.5 t - QA Traslados'
);
SET @TipoCamionQA = (
    SELECT idTipoTransporte FROM tblTipoTransporte
    WHERE nombreTipoTransporte = 'Camion refrigerado 3.5 t - QA Traslados'
    LIMIT 1
);

-- Sucursales y bodegas: planta de origen y tres centros de distribucion.
INSERT INTO tblSucursal (nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
SELECT 'Planta Embutidos Mixco - QA', 'Planta de produccion y despacho de embutidos.',
       'Zona industrial de Mixco, Guatemala', 'Guatemala'
WHERE NOT EXISTS (
    SELECT 1 FROM tblSucursal WHERE nombreSucursal = 'Planta Embutidos Mixco - QA'
);
INSERT INTO tblSucursal (nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
SELECT 'Centro Distribucion Guatemala - QA', 'Centro de distribucion metropolitano.',
       'Zona 12, Ciudad de Guatemala', 'Guatemala'
WHERE NOT EXISTS (
    SELECT 1 FROM tblSucursal WHERE nombreSucursal = 'Centro Distribucion Guatemala - QA'
);
INSERT INTO tblSucursal (nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
SELECT 'Centro Distribucion Occidente - QA', 'Centro de distribucion regional de occidente.',
       'Quetzaltenango, Guatemala', 'Guatemala'
WHERE NOT EXISTS (
    SELECT 1 FROM tblSucursal WHERE nombreSucursal = 'Centro Distribucion Occidente - QA'
);
INSERT INTO tblSucursal (nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
SELECT 'Centro Distribucion Sur - QA', 'Centro de distribucion regional del sur.',
       'Escuintla, Guatemala', 'Guatemala'
WHERE NOT EXISTS (
    SELECT 1 FROM tblSucursal WHERE nombreSucursal = 'Centro Distribucion Sur - QA'
);

SET @SucursalPlanta = (
    SELECT idSucursal FROM tblSucursal
    WHERE nombreSucursal = 'Planta Embutidos Mixco - QA' LIMIT 1
);
SET @SucursalGuatemala = (
    SELECT idSucursal FROM tblSucursal
    WHERE nombreSucursal = 'Centro Distribucion Guatemala - QA' LIMIT 1
);
SET @SucursalOccidente = (
    SELECT idSucursal FROM tblSucursal
    WHERE nombreSucursal = 'Centro Distribucion Occidente - QA' LIMIT 1
);
SET @SucursalSur = (
    SELECT idSucursal FROM tblSucursal
    WHERE nombreSucursal = 'Centro Distribucion Sur - QA' LIMIT 1
);

INSERT INTO tblBodega (nombreBodega, Fk_Sucursal_Codigo, paisBodega)
SELECT 'Camara fria de producto terminado - QA', @SucursalPlanta, 'Guatemala'
WHERE NOT EXISTS (
    SELECT 1 FROM tblBodega
    WHERE nombreBodega = 'Camara fria de producto terminado - QA'
);
INSERT INTO tblBodega (nombreBodega, Fk_Sucursal_Codigo, paisBodega)
SELECT 'Bodega metropolitana de embutidos - QA', @SucursalGuatemala, 'Guatemala'
WHERE NOT EXISTS (
    SELECT 1 FROM tblBodega
    WHERE nombreBodega = 'Bodega metropolitana de embutidos - QA'
);
INSERT INTO tblBodega (nombreBodega, Fk_Sucursal_Codigo, paisBodega)
SELECT 'Bodega regional de occidente - QA', @SucursalOccidente, 'Guatemala'
WHERE NOT EXISTS (
    SELECT 1 FROM tblBodega
    WHERE nombreBodega = 'Bodega regional de occidente - QA'
);
INSERT INTO tblBodega (nombreBodega, Fk_Sucursal_Codigo, paisBodega)
SELECT 'Bodega regional del sur - QA', @SucursalSur, 'Guatemala'
WHERE NOT EXISTS (
    SELECT 1 FROM tblBodega
    WHERE nombreBodega = 'Bodega regional del sur - QA'
);

SET @BodegaOrigen = (
    SELECT idBodega FROM tblBodega
    WHERE nombreBodega = 'Camara fria de producto terminado - QA' LIMIT 1
);
SET @BodegaGuatemala = (
    SELECT idBodega FROM tblBodega
    WHERE nombreBodega = 'Bodega metropolitana de embutidos - QA' LIMIT 1
);
SET @BodegaOccidente = (
    SELECT idBodega FROM tblBodega
    WHERE nombreBodega = 'Bodega regional de occidente - QA' LIMIT 1
);
SET @BodegaSur = (
    SELECT idBodega FROM tblBodega
    WHERE nombreBodega = 'Bodega regional del sur - QA' LIMIT 1
);

-- Catalogos propios para que el script no dependa de IDs de otros datos de prueba.
INSERT INTO tblCategoria (nombreCategoria, porcentajeComisionCategoria)
SELECT 'Embutidos refrigerados - QA', 0.10
WHERE NOT EXISTS (
    SELECT 1 FROM tblCategoria WHERE nombreCategoria = 'Embutidos refrigerados - QA'
);
INSERT INTO tblLinea (nombreLinea, porcentajeComisionLinea)
SELECT 'Cadena fria - QA', 0.10
WHERE NOT EXISTS (
    SELECT 1 FROM tblLinea WHERE nombreLinea = 'Cadena fria - QA'
);
INSERT INTO tblMarca (nombreMarca, porcentajeComisionMarca)
SELECT 'Embutidos del Valle - QA', 0.10
WHERE NOT EXISTS (
    SELECT 1 FROM tblMarca WHERE nombreMarca = 'Embutidos del Valle - QA'
);
INSERT INTO tblProveedor
    (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor)
SELECT 'Empacadora del Valle - QA', '5487219QA1', 'calidad.qa@empacadoravalle.test',
       '502-5550-7201', 'Zona industrial de Mixco, Guatemala'
WHERE NOT EXISTS (
    SELECT 1 FROM tblProveedor WHERE nitProveedor = '5487219QA1'
);

SET @CategoriaQA = (
    SELECT idCategoria FROM tblCategoria
    WHERE nombreCategoria = 'Embutidos refrigerados - QA' LIMIT 1
);
SET @LineaQA = (
    SELECT idLinea FROM tblLinea
    WHERE nombreLinea = 'Cadena fria - QA' LIMIT 1
);
SET @MarcaQA = (
    SELECT idMarca FROM tblMarca
    WHERE nombreMarca = 'Embutidos del Valle - QA' LIMIT 1
);
SET @ProveedorQA = (
    SELECT idProveedor FROM tblProveedor WHERE nitProveedor = '5487219QA1' LIMIT 1
);

INSERT INTO tblProducto
    (skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos,
     Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto,
     imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto,
     porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
SELECT 'QA-EMB-001', 'EMB01', 'Jamon cocido familiar 2.5 kg', @ProveedorQA,
       @MarcaQA, @CategoriaQA, @LineaQA,
       'Jamon cocido en pieza, empacado al vacio y conservado en cadena fria.',
       X'FFD8FFD9', 'Guatemala', 68.5000, 94.9000, 0.10, 30, 600, 2.50
WHERE NOT EXISTS (SELECT 1 FROM tblProducto WHERE skuProducto = 'QA-EMB-001');
INSERT INTO tblProducto
    (skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos,
     Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto,
     imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto,
     porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
SELECT 'QA-EMB-002', 'EMB02', 'Chorizo parrillero 1 kg', @ProveedorQA,
       @MarcaQA, @CategoriaQA, @LineaQA,
       'Chorizo de cerdo sazonado, en empaque sellado de un kilogramo.',
       X'FFD8FFD9', 'Guatemala', 32.7500, 47.5000, 0.10, 40, 800, 1.00
WHERE NOT EXISTS (SELECT 1 FROM tblProducto WHERE skuProducto = 'QA-EMB-002');
INSERT INTO tblProducto
    (skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos,
     Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto,
     imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto,
     porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
SELECT 'QA-EMB-003', 'EMB03', 'Salchicha tipo viena paquete 12 unidades', @ProveedorQA,
       @MarcaQA, @CategoriaQA, @LineaQA,
       'Salchicha tipo viena cocida, paquete familiar refrigerado de doce unidades.',
       X'FFD8FFD9', 'Guatemala', 18.2500, 29.9000, 0.10, 50, 900, 0.60
WHERE NOT EXISTS (SELECT 1 FROM tblProducto WHERE skuProducto = 'QA-EMB-003');
INSERT INTO tblProducto
    (skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos,
     Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto,
     imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto,
     porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
SELECT 'QA-EMB-004', 'EMB04', 'Longaniza fresca 1 kg', @ProveedorQA,
       @MarcaQA, @CategoriaQA, @LineaQA,
       'Longaniza fresca condimentada, empacada y lista para cocinar.',
       X'FFD8FFD9', 'Guatemala', 28.0000, 42.0000, 0.10, 25, 500, 1.00
WHERE NOT EXISTS (SELECT 1 FROM tblProducto WHERE skuProducto = 'QA-EMB-004');

SET @ProductoJamon = (SELECT idProducto FROM tblProducto WHERE skuProducto = 'QA-EMB-001' LIMIT 1);
SET @ProductoChorizo = (SELECT idProducto FROM tblProducto WHERE skuProducto = 'QA-EMB-002' LIMIT 1);
SET @ProductoSalchicha = (SELECT idProducto FROM tblProducto WHERE skuProducto = 'QA-EMB-003' LIMIT 1);
SET @ProductoLonganiza = (SELECT idProducto FROM tblProducto WHERE skuProducto = 'QA-EMB-004' LIMIT 1);

-- Transporte y conductores: tres camiones y cinco conductores para probar
-- disponibilidad, exclusion por traslado activo y asignacion opcional del camion.
INSERT INTO tblTransporte
    (nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte,
     esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
SELECT 'QA-TR-001', 'Hino', '300 Refrigerado', 2023, 1, @EstadoNoDisponible, @TipoCamionQA
WHERE NOT EXISTS (SELECT 1 FROM tblTransporte WHERE nroPlacaTransporte = 'QA-TR-001');
INSERT INTO tblTransporte
    (nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte,
     esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
SELECT 'QA-TR-002', 'Isuzu', 'NPR Refrigerado', 2022, 1, @EstadoNoDisponible, @TipoCamionQA
WHERE NOT EXISTS (SELECT 1 FROM tblTransporte WHERE nroPlacaTransporte = 'QA-TR-002');
INSERT INTO tblTransporte
    (nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte,
     esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
SELECT 'QA-TR-003', 'Fuso', 'Canter Refrigerado', 2024, 1, @EstadoDisponible, @TipoCamionQA
WHERE NOT EXISTS (SELECT 1 FROM tblTransporte WHERE nroPlacaTransporte = 'QA-TR-003');

SET @TransporteSolicitado = (
    SELECT idTransporte FROM tblTransporte WHERE nroPlacaTransporte = 'QA-TR-001' LIMIT 1
);
SET @TransporteTransito = (
    SELECT idTransporte FROM tblTransporte WHERE nroPlacaTransporte = 'QA-TR-002' LIMIT 1
);
SET @TransporteLibre = (
    SELECT idTransporte FROM tblTransporte WHERE nroPlacaTransporte = 'QA-TR-003' LIMIT 1
);

INSERT INTO tblConductor
    (codigoConductor, nombreCompletoConductor, numeroLicenciaConductor,
     fechaVencimientoDocConductor, Fk_Estado_Codigo)
SELECT 'QA-COND-001', 'Carlos Mendez', 'LIC-QA-001', '2029-08-31', @EstadoDisponible
WHERE NOT EXISTS (SELECT 1 FROM tblConductor WHERE codigoConductor = 'QA-COND-001');
INSERT INTO tblConductor
    (codigoConductor, nombreCompletoConductor, numeroLicenciaConductor,
     fechaVencimientoDocConductor, Fk_Estado_Codigo)
SELECT 'QA-COND-002', 'Maria Lopez', 'LIC-QA-002', '2028-11-30', @EstadoDisponible
WHERE NOT EXISTS (SELECT 1 FROM tblConductor WHERE codigoConductor = 'QA-COND-002');
INSERT INTO tblConductor
    (codigoConductor, nombreCompletoConductor, numeroLicenciaConductor,
     fechaVencimientoDocConductor, Fk_Estado_Codigo)
SELECT 'QA-COND-003', 'Jose Ramirez', 'LIC-QA-003', '2029-04-30', @EstadoDisponible
WHERE NOT EXISTS (SELECT 1 FROM tblConductor WHERE codigoConductor = 'QA-COND-003');
INSERT INTO tblConductor
    (codigoConductor, nombreCompletoConductor, numeroLicenciaConductor,
     fechaVencimientoDocConductor, Fk_Estado_Codigo)
SELECT 'QA-COND-004', 'Ana Castillo', 'LIC-QA-004', '2028-06-30', @EstadoDisponible
WHERE NOT EXISTS (SELECT 1 FROM tblConductor WHERE codigoConductor = 'QA-COND-004');
INSERT INTO tblConductor
    (codigoConductor, nombreCompletoConductor, numeroLicenciaConductor,
     fechaVencimientoDocConductor, Fk_Estado_Codigo)
SELECT 'QA-COND-005', 'Luis Herrera', 'LIC-QA-005', '2029-02-28', @EstadoDisponible
WHERE NOT EXISTS (SELECT 1 FROM tblConductor WHERE codigoConductor = 'QA-COND-005');

SET @ConductorSolicitado = (
    SELECT idConductor FROM tblConductor WHERE codigoConductor = 'QA-COND-001' LIMIT 1
);
SET @ConductorSinCamion = (
    SELECT idConductor FROM tblConductor WHERE codigoConductor = 'QA-COND-002' LIMIT 1
);
SET @ConductorTransito = (
    SELECT idConductor FROM tblConductor WHERE codigoConductor = 'QA-COND-003' LIMIT 1
);
SET @ConductorRecibido = (
    SELECT idConductor FROM tblConductor WHERE codigoConductor = 'QA-COND-004' LIMIT 1
);
SET @ConductorCancelado = (
    SELECT idConductor FROM tblConductor WHERE codigoConductor = 'QA-COND-005' LIMIT 1
);

-- Existencias en la camara de origen: el producto en transito ya salio;
-- los productos de traslados SOLICITADO tienen sus unidades apartadas.
INSERT INTO tblStock
    (Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo,
     stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock,
     cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock,
     apartadosStock, estadoVentaStock, estadoCompraStock)
SELECT @SucursalPlanta, @BodegaOrigen, @ProductoJamon, 500, 500, 500, 500,
       0, 0, 0, 0, 24, 1, 1
WHERE NOT EXISTS (
    SELECT 1 FROM tblStock
    WHERE Fk_Bodega_Codigo = @BodegaOrigen AND Fk_Producto_Codigo = @ProductoJamon
);
INSERT INTO tblStock
    (Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo,
     stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock,
     cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock,
     apartadosStock, estadoVentaStock, estadoCompraStock)
SELECT @SucursalPlanta, @BodegaOrigen, @ProductoChorizo, 400, 400, 400, 400,
       0, 0, 0, 0, 26, 1, 1
WHERE NOT EXISTS (
    SELECT 1 FROM tblStock
    WHERE Fk_Bodega_Codigo = @BodegaOrigen AND Fk_Producto_Codigo = @ProductoChorizo
);
INSERT INTO tblStock
    (Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo,
     stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock,
     cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock,
     apartadosStock, estadoVentaStock, estadoCompraStock)
SELECT @SucursalPlanta, @BodegaOrigen, @ProductoSalchicha, 300, 300, 300, 292,
       0, 8, 0, 8, 18, 1, 1
WHERE NOT EXISTS (
    SELECT 1 FROM tblStock
    WHERE Fk_Bodega_Codigo = @BodegaOrigen AND Fk_Producto_Codigo = @ProductoSalchicha
);
INSERT INTO tblStock
    (Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo,
     stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock,
     cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock,
     apartadosStock, estadoVentaStock, estadoCompraStock)
SELECT @SucursalPlanta, @BodegaOrigen, @ProductoLonganiza, 110, 110, 120, 110,
       0, 10, 0, 10, 0, 1, 1
WHERE NOT EXISTS (
    SELECT 1 FROM tblStock
    WHERE Fk_Bodega_Codigo = @BodegaOrigen AND Fk_Producto_Codigo = @ProductoLonganiza
);
INSERT INTO tblStock
    (Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo,
     stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock,
     cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock,
     apartadosStock, estadoVentaStock, estadoCompraStock)
SELECT @SucursalGuatemala, @BodegaGuatemala, @ProductoLonganiza, 10, 10, 0, 10,
       10, 0, 10, 0, 0, 1, 1
WHERE NOT EXISTS (
    SELECT 1 FROM tblStock
    WHERE Fk_Bodega_Codigo = @BodegaGuatemala AND Fk_Producto_Codigo = @ProductoLonganiza
);

-- Casos: dos solicitudes editables (con y sin transporte), uno en transito,
-- uno recibido y uno cancelado. Los estados finales son historicos/solo lectura.
INSERT INTO tblTraslado
    (codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo,
     Fk_Transporte_Codigo, Fk_Conductor_Codigo, fechaSolicitudTraslado,
     fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
SELECT 'QA-TRAS-REQ-001', @BodegaOrigen, @BodegaGuatemala,
       @TransporteSolicitado, @ConductorSolicitado, CURRENT_TIMESTAMP - INTERVAL 2 DAY,
       NULL, NULL, 'SOLICITADO',
       'Reposicion semanal de jamon y chorizo para el centro metropolitano.'
WHERE NOT EXISTS (SELECT 1 FROM tblTraslado WHERE codigoTraslado = 'QA-TRAS-REQ-001');
INSERT INTO tblTraslado
    (codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo,
     Fk_Transporte_Codigo, Fk_Conductor_Codigo, fechaSolicitudTraslado,
     fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
SELECT 'QA-TRAS-REQ-002', @BodegaOrigen, @BodegaOccidente,
       NULL, @ConductorSinCamion, CURRENT_TIMESTAMP - INTERVAL 1 DAY,
       NULL, NULL, 'SOLICITADO',
       'Solicitud pendiente de asignacion de camion para producto refrigerado.'
WHERE NOT EXISTS (SELECT 1 FROM tblTraslado WHERE codigoTraslado = 'QA-TRAS-REQ-002');
INSERT INTO tblTraslado
    (codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo,
     Fk_Transporte_Codigo, Fk_Conductor_Codigo, fechaSolicitudTraslado,
     fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
SELECT 'QA-TRAS-ENR-001', @BodegaOrigen, @BodegaSur,
       @TransporteTransito, @ConductorTransito, CURRENT_TIMESTAMP - INTERVAL 5 HOUR,
       CURRENT_TIMESTAMP - INTERVAL 2 HOUR, NULL, 'EN_TRANSITO',
       'Traslado refrigerado en ruta hacia el centro regional del sur.'
WHERE NOT EXISTS (SELECT 1 FROM tblTraslado WHERE codigoTraslado = 'QA-TRAS-ENR-001');
INSERT INTO tblTraslado
    (codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo,
     Fk_Transporte_Codigo, Fk_Conductor_Codigo, fechaSolicitudTraslado,
     fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
SELECT 'QA-TRAS-REC-001', @BodegaOrigen, @BodegaGuatemala,
       @TransporteLibre, @ConductorRecibido, CURRENT_TIMESTAMP - INTERVAL 4 DAY,
       CURRENT_TIMESTAMP - INTERVAL 3 DAY, CURRENT_TIMESTAMP - INTERVAL 3 DAY,
       'RECIBIDO', 'Recepcion confirmada de longaniza fresca; revisar el detalle historico.'
WHERE NOT EXISTS (SELECT 1 FROM tblTraslado WHERE codigoTraslado = 'QA-TRAS-REC-001');
INSERT INTO tblTraslado
    (codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo,
     Fk_Transporte_Codigo, Fk_Conductor_Codigo, fechaSolicitudTraslado,
     fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
SELECT 'QA-TRAS-CAN-001', @BodegaOrigen, @BodegaOccidente,
       NULL, @ConductorCancelado, CURRENT_TIMESTAMP - INTERVAL 6 DAY,
       NULL, NULL, 'CANCELADO',
       'Cancelado por ajuste de demanda antes del despacho.'
WHERE NOT EXISTS (SELECT 1 FROM tblTraslado WHERE codigoTraslado = 'QA-TRAS-CAN-001');

SET @TrasladoSolicitud = (
    SELECT idTraslado FROM tblTraslado WHERE codigoTraslado = 'QA-TRAS-REQ-001' LIMIT 1
);
SET @TrasladoSinCamion = (
    SELECT idTraslado FROM tblTraslado WHERE codigoTraslado = 'QA-TRAS-REQ-002' LIMIT 1
);
SET @TrasladoTransito = (
    SELECT idTraslado FROM tblTraslado WHERE codigoTraslado = 'QA-TRAS-ENR-001' LIMIT 1
);
SET @TrasladoRecibido = (
    SELECT idTraslado FROM tblTraslado WHERE codigoTraslado = 'QA-TRAS-REC-001' LIMIT 1
);
SET @TrasladoCancelado = (
    SELECT idTraslado FROM tblTraslado WHERE codigoTraslado = 'QA-TRAS-CAN-001' LIMIT 1
);

INSERT INTO tblDetalleTraslado (Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
SELECT @TrasladoSolicitud, @ProductoJamon, 24
WHERE NOT EXISTS (
    SELECT 1 FROM tblDetalleTraslado
    WHERE Fk_Traslado_Codigo = @TrasladoSolicitud AND Fk_Producto_Codigo = @ProductoJamon
);
INSERT INTO tblDetalleTraslado (Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
SELECT @TrasladoSolicitud, @ProductoChorizo, 14
WHERE NOT EXISTS (
    SELECT 1 FROM tblDetalleTraslado
    WHERE Fk_Traslado_Codigo = @TrasladoSolicitud AND Fk_Producto_Codigo = @ProductoChorizo
);
INSERT INTO tblDetalleTraslado (Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
SELECT @TrasladoSinCamion, @ProductoChorizo, 12
WHERE NOT EXISTS (
    SELECT 1 FROM tblDetalleTraslado
    WHERE Fk_Traslado_Codigo = @TrasladoSinCamion AND Fk_Producto_Codigo = @ProductoChorizo
);
INSERT INTO tblDetalleTraslado (Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
SELECT @TrasladoSinCamion, @ProductoSalchicha, 18
WHERE NOT EXISTS (
    SELECT 1 FROM tblDetalleTraslado
    WHERE Fk_Traslado_Codigo = @TrasladoSinCamion AND Fk_Producto_Codigo = @ProductoSalchicha
);
INSERT INTO tblDetalleTraslado (Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
SELECT @TrasladoTransito, @ProductoSalchicha, 8
WHERE NOT EXISTS (
    SELECT 1 FROM tblDetalleTraslado
    WHERE Fk_Traslado_Codigo = @TrasladoTransito AND Fk_Producto_Codigo = @ProductoSalchicha
);
INSERT INTO tblDetalleTraslado (Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
SELECT @TrasladoRecibido, @ProductoLonganiza, 10
WHERE NOT EXISTS (
    SELECT 1 FROM tblDetalleTraslado
    WHERE Fk_Traslado_Codigo = @TrasladoRecibido AND Fk_Producto_Codigo = @ProductoLonganiza
);
INSERT INTO tblDetalleTraslado (Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
SELECT @TrasladoCancelado, @ProductoJamon, 5
WHERE NOT EXISTS (
    SELECT 1 FROM tblDetalleTraslado
    WHERE Fk_Traslado_Codigo = @TrasladoCancelado AND Fk_Producto_Codigo = @ProductoJamon
);

-- Diario minimo para que el caso EN_TRANSITO pueda completarse o anularse
-- desde el formulario como un traslado despachado por la aplicacion.
INSERT INTO tblTipoMovimiento
    (nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
SELECT 'Salida por traslado - QA', 'Salida de inventario para traslado de prueba.', '-'
WHERE NOT EXISTS (
    SELECT 1 FROM tblTipoMovimiento
    WHERE nombreTipoMovimiento = 'Salida por traslado - QA'
);
INSERT INTO tblTipoMovimiento
    (nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
SELECT 'Entrada por traslado - QA', 'Entrada de inventario para traslado de prueba.', '+'
WHERE NOT EXISTS (
    SELECT 1 FROM tblTipoMovimiento
    WHERE nombreTipoMovimiento = 'Entrada por traslado - QA'
);
SET @TipoSalidaQA = (
    SELECT idTipoMovimiento FROM tblTipoMovimiento
    WHERE signoTipoMovimiento = '-' AND nombreTipoMovimiento LIKE '%traslado%'
    ORDER BY idTipoMovimiento LIMIT 1
);

INSERT INTO tblMovimientoInventario
    (Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo,
     Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario,
     observacionesMovimientoInventario, estadoMovimientoInventario)
SELECT @TipoSalidaQA, @BodegaOrigen, @BodegaSur, @TransporteTransito, @TrasladoTransito,
       CURRENT_TIMESTAMP - INTERVAL 2 HOUR,
       'Salida por traslado QA-TRAS-ENR-001', 'PENDIENTE'
WHERE NOT EXISTS (
    SELECT 1 FROM tblMovimientoInventario
    WHERE Fk_Traslado_Codigo = @TrasladoTransito
      AND Fk_Tipo_Movimiento_Codigo = @TipoSalidaQA
);
SET @MovimientoTransito = (
    SELECT idMovimientoInventario FROM tblMovimientoInventario
    WHERE Fk_Traslado_Codigo = @TrasladoTransito
      AND Fk_Tipo_Movimiento_Codigo = @TipoSalidaQA
    ORDER BY idMovimientoInventario LIMIT 1
);
INSERT INTO tblDetalleMovimiento
    (Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo,
     Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
SELECT @MovimientoTransito, NULL, @ProductoSalchicha, 8, 18.2500
WHERE NOT EXISTS (
    SELECT 1 FROM tblDetalleMovimiento
    WHERE Fk_Movimiento_Inventario_Codigo = @MovimientoTransito
      AND Fk_Producto_Codigo = @ProductoSalchicha
);

INSERT INTO tblEncabezadoViaje
    (fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje,
     requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje,
     pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
SELECT CURRENT_TIMESTAMP - INTERVAL 2 HOUR, CURRENT_TIMESTAMP - INTERVAL 2 HOUR,
       1, 'EN_CURSO', 4.800, @TransporteTransito, @ConductorTransito
WHERE NOT EXISTS (
    SELECT 1 FROM tblEncabezadoViaje
    WHERE Fk_Transporte_Codigo = @TransporteTransito
      AND estadoViajeEncabezadoViaje = 'EN_CURSO'
);
SET @ViajeTransito = (
    SELECT idEncabezadoViaje FROM tblEncabezadoViaje
    WHERE Fk_Transporte_Codigo = @TransporteTransito
      AND estadoViajeEncabezadoViaje = 'EN_CURSO'
    ORDER BY idEncabezadoViaje LIMIT 1
);
INSERT INTO tblDetalleViaje
    (Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje,
     cantidadDetalleViaje, pesoDetalleViaje)
SELECT @ViajeTransito, 1, 'Salchicha tipo viena paquete 12 unidades',
       8, 4.800
WHERE NOT EXISTS (
    SELECT 1 FROM tblDetalleViaje
    WHERE Fk_Encabezado_Viaje_Codigo = @ViajeTransito AND ordenDetalleViaje = 1
);

COMMIT;

-- Resumen para confirmar los casos visibles en el formulario.
SELECT codigoTraslado, estadoTraslado, origen.nombreBodega AS BodegaOrigen,
       destino.nombreBodega AS BodegaDestino, COUNT(detalle.idDetalleTraslado) AS Productos
FROM tblTraslado traslado
INNER JOIN tblBodega origen ON origen.idBodega = traslado.Fk_Bodega_Origen_Codigo
INNER JOIN tblBodega destino ON destino.idBodega = traslado.Fk_Bodega_Destino_Codigo
LEFT JOIN tblDetalleTraslado detalle ON detalle.Fk_Traslado_Codigo = traslado.idTraslado
WHERE traslado.codigoTraslado LIKE 'QA-TRAS-%'
GROUP BY traslado.idTraslado, codigoTraslado, estadoTraslado,
         origen.nombreBodega, destino.nombreBodega
ORDER BY traslado.codigoTraslado;
