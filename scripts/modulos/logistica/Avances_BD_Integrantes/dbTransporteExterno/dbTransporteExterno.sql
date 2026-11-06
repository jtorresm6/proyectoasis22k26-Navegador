-- ============================================================
-- 1. EMPRESA TRANSPORTISTA
-- ============================================================
CREATE TABLE IF NOT EXISTS tblEmpresaTransportista (
 Pk_EmpresaTransportista INT AUTO_INCREMENT PRIMARY KEY,
 nombreEmpresaTransportista VARCHAR(100) NOT NULL,
 contactoEmpresaTransportista VARCHAR(100),
 telefonoEmpresaTransportista VARCHAR(30),
 Fk_Proveedor_IdProveedor INT NULL, 
 is_active BOOLEAN NOT NULL DEFAULT TRUE,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqEmpresaTransportistaNombre UNIQUE (nombreEmpresaTransportista),
 CONSTRAINT uqEmpresaTransportistaProveedor UNIQUE (Fk_Proveedor_IdProveedor), 
 CONSTRAINT fkEmpresaProveedor FOREIGN KEY (Fk_Proveedor_IdProveedor) REFERENCES tblProveedor(idProveedor) 
) ENGINE=InnoDB;

-- ============================================================
-- 2. TIPO SERVICIO
-- ============================================================
CREATE TABLE IF NOT EXISTS tblTipoServicio (
 Pk_TipoServicio INT AUTO_INCREMENT PRIMARY KEY,
 nombreTipoServicio VARCHAR(100) NOT NULL,
 is_active BOOLEAN NOT NULL DEFAULT TRUE,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqTipoServicioNombre UNIQUE (nombreTipoServicio)
) ENGINE=InnoDB;

-- ============================================================
-- 3. TARIFA TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblTarifaTransporte (
 Pk_TarifaTransporte INT AUTO_INCREMENT PRIMARY KEY,
 nombreTarifaTransporte VARCHAR(100) NOT NULL,
 is_active BOOLEAN NOT NULL DEFAULT TRUE,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqTarifaTransporteNombre UNIQUE (nombreTarifaTransporte)
) ENGINE=InnoDB;

-- ============================================================
-- 4. SERVICIO TRANSPORTE EXTERNO
-- ============================================================
CREATE TABLE IF NOT EXISTS tblServicioTransporteExterno (
 Pk_ServicioTransporteExterno INT AUTO_INCREMENT PRIMARY KEY,
 Fk_EmpresaTransportista_Codigo INT NOT NULL,
 Fk_TipoServicio_Codigo INT NOT NULL,
 Fk_Traslado_IdTraslado INT NULL,
 numeroGuiaServicioTransporteExterno VARCHAR(30) NULL,
 estadoServicioTransporteExterno VARCHAR(30) NOT NULL DEFAULT 'Programado',
 origenServicioTransporteExterno VARCHAR(100) NOT NULL,
 direccionOrigenServicioTransporteExterno VARCHAR(500),
 destinoServicioTransporteExterno VARCHAR(100) NOT NULL,
 direccionDestinoServicioTransporteExterno VARCHAR(500),
 fechaContratacionServicioTransporteExterno DATE NOT NULL,
 fechaEstimadaEntregaServicioTransporteExterno DATE NOT NULL,
 fechaEntregaRealServicioTransporteExterno DATE NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqServicioNumeroGuia UNIQUE (numeroGuiaServicioTransporteExterno),
 CONSTRAINT ckServicioEstado CHECK (estadoServicioTransporteExterno IN ('Programado','En tránsito','Entregado','Cancelado')),
 CONSTRAINT ckServicioFechas CHECK (fechaEstimadaEntregaServicioTransporteExterno >= fechaContratacionServicioTransporteExterno),
 CONSTRAINT fkServicioEmpresa FOREIGN KEY (Fk_EmpresaTransportista_Codigo) REFERENCES tblEmpresaTransportista(Pk_EmpresaTransportista),
 CONSTRAINT fkServicioTipo FOREIGN KEY (Fk_TipoServicio_Codigo) REFERENCES tblTipoServicio(Pk_TipoServicio),
 CONSTRAINT fkServicioTraslado FOREIGN KEY (Fk_Traslado_IdTraslado) REFERENCES tbltraslado(idTraslado)
) ENGINE=InnoDB;

-- ============================================================
-- 5. COSTO TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblCostoTransporte (
 Pk_CostoTransporte INT AUTO_INCREMENT PRIMARY KEY,
 Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
 Fk_TarifaTransporte_Codigo INT NOT NULL,
 cantidadCostoTransporte DECIMAL(12,4) NOT NULL,
 costoUnitarioTransporte DECIMAL(12,4) NOT NULL,
 costoTotalTransporte DECIMAL(12,4) NOT NULL,
 monedaCostoTransporte CHAR(3) NOT NULL,
 tipoPagoCostoTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqCostoServicio UNIQUE (Fk_ServicioTransporteExterno_Codigo),
 CONSTRAINT ckCostoCantidad CHECK (cantidadCostoTransporte > 0),
 CONSTRAINT ckCostoUnitario CHECK (costoUnitarioTransporte >= 0),
 CONSTRAINT ckCostoMoneda CHECK (monedaCostoTransporte IN ('GTQ','USD')),
 CONSTRAINT ckCostoTipoPago CHECK (tipoPagoCostoTransporte IN ('Transferencia','Efectivo')),
 CONSTRAINT fkCostoServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(Pk_ServicioTransporteExterno),
 CONSTRAINT fkCostoTarifa FOREIGN KEY (Fk_TarifaTransporte_Codigo) REFERENCES tblTarifaTransporte(Pk_TarifaTransporte)
) ENGINE=InnoDB;

-- ============================================================
-- 6. VEHICULO TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblVehiculoTransporte (
 Pk_VehiculoTransporte INT AUTO_INCREMENT PRIMARY KEY,
 Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
 placaVehiculoTransporte VARCHAR(30),
 tipoVehiculoTransporte VARCHAR(100),
 capacidadVehiculoTransporte VARCHAR(100),
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqVehiculoServicio UNIQUE (Fk_ServicioTransporteExterno_Codigo),
 CONSTRAINT fkVehiculoServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(Pk_ServicioTransporteExterno)
) ENGINE=InnoDB;

-- ============================================================
-- 7. CONDUCTOR TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblConductorTransporte (
 Pk_ConductorTransporte INT AUTO_INCREMENT PRIMARY KEY,
 Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
 nombreConductorTransporte VARCHAR(100),
 telefonoConductorTransporte VARCHAR(30),
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqConductorServicio UNIQUE (Fk_ServicioTransporteExterno_Codigo),
 CONSTRAINT fkConductorServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(Pk_ServicioTransporteExterno)
) ENGINE=InnoDB;

-- ============================================================
-- 8. PRODUCTO TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblProductoTransporte (
 Pk_ProductoTransporte INT AUTO_INCREMENT PRIMARY KEY,
 Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
 Fk_Producto_IdProducto INT NOT NULL,
 cantidadProductoTransporte DECIMAL(12,4) NOT NULL,
 unidadMedidaProductoTransporte VARCHAR(30),
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT ckProductoCantidad CHECK (cantidadProductoTransporte > 0),
 CONSTRAINT fkProductoServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(Pk_ServicioTransporteExterno),
 CONSTRAINT fkProductoCatalogo FOREIGN KEY (Fk_Producto_IdProducto) REFERENCES tblproducto(idProducto)
) ENGINE=InnoDB;

-- ============================================================
-- 9. UBICACION TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblUbicacionTransporte (
 Pk_UbicacionTransporte INT AUTO_INCREMENT PRIMARY KEY,
 Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
 ubicacionActualTransporte VARCHAR(500) NOT NULL,
 fechaActualizacionUbicacionTransporte DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT fkUbicacionServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(Pk_ServicioTransporteExterno),
 INDEX idxUbicacionServicioFecha (Fk_ServicioTransporteExterno_Codigo, fechaActualizacionUbicacionTransporte)
) ENGINE=InnoDB;

-- ============================================================
-- 10. INCIDENCIA TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblIncidenciaTransporte (
 Pk_IncidenciaTransporte INT AUTO_INCREMENT PRIMARY KEY,
 Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
 codigoIncidenciaTransporte VARCHAR(30) NOT NULL,
 fechaIncidenciaTransporte DATETIME NOT NULL,
 tipoIncidenciaTransporte VARCHAR(100) NOT NULL,
 descripcionIncidenciaTransporte TEXT NOT NULL,
 responsableIncidenciaTransporte VARCHAR(100),
 estadoIncidenciaTransporte VARCHAR(30) NOT NULL DEFAULT 'Pendiente',
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqIncidenciaCodigo UNIQUE (codigoIncidenciaTransporte),
 CONSTRAINT fkIncidenciaServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(Pk_ServicioTransporteExterno)
) ENGINE=InnoDB;

-- ============================================================
-- 11. HISTORIAL TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblHistorialTransporte (
 Pk_HistorialTransporte INT AUTO_INCREMENT PRIMARY KEY,
 Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
 fechaMovimientoHistorial DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
 tipoMovimientoHistorial VARCHAR(100) NOT NULL,
 descripcionMovimientoHistorial VARCHAR(255) NOT NULL,
 estadoTransporteHistorial VARCHAR(30),
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT fkHistorialServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(Pk_ServicioTransporteExterno)
) ENGINE=InnoDB;

-- ============================================================
-- 12. FACTURA TRANSPORTE - factura emitida por la transportista
-- Una factura por servicio. Al registrarla se crea la cuenta por pagar
-- ============================================================
CREATE TABLE IF NOT EXISTS tblFacturaTransporte (
 Pk_FacturaTransporte INT AUTO_INCREMENT PRIMARY KEY,
 Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
 Fk_Plazo_IdPlazo INT NOT NULL,                 
 Fk_CuentaPorPagar_IdCuentaPorPagar INT NULL,   -- cuenta por pagar generada con esta factura
 serieFacturaTransporte VARCHAR(20) NOT NULL,
 numeroFacturaTransporte VARCHAR(30) NOT NULL,
 fechaEmisionFacturaTransporte DATE NOT NULL,
 montoFacturaTransporte DECIMAL(12,4) NOT NULL,
 estadoFacturaTransporte VARCHAR(30) NOT NULL DEFAULT 'Registrada',
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqFacturaServicio UNIQUE (Fk_ServicioTransporteExterno_Codigo),
 CONSTRAINT uqFacturaCuentaPorPagar UNIQUE (Fk_CuentaPorPagar_IdCuentaPorPagar),
 CONSTRAINT uqFacturaDocumento UNIQUE (serieFacturaTransporte, numeroFacturaTransporte),
 CONSTRAINT ckFacturaMonto CHECK (montoFacturaTransporte > 0),
 CONSTRAINT ckFacturaEstado CHECK (estadoFacturaTransporte IN ('Registrada','Anulada')),
 CONSTRAINT fkFacturaServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(Pk_ServicioTransporteExterno),
 CONSTRAINT fkFacturaPlazo FOREIGN KEY (Fk_Plazo_IdPlazo) REFERENCES tblPlazo(idPlazo),
 CONSTRAINT fkFacturaCuentaPorPagar FOREIGN KEY (Fk_CuentaPorPagar_IdCuentaPorPagar) REFERENCES tblCuentaPorPagar(idCuentaPorPagar)
) ENGINE=InnoDB;

-- ============================================================
-- 13. SOLICITUD PAGO TRANSPORTE - relacion con bancos
-- ============================================================
CREATE TABLE IF NOT EXISTS tblSolicitudPagoTransporte (
 Pk_SolicitudPagoTransporte INT AUTO_INCREMENT PRIMARY KEY,
 Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
 codigoSolicitudPagoTransporte VARCHAR(30) NULL,
 estadoSolicitudPagoTransporte VARCHAR(30) NOT NULL DEFAULT 'Solicitado',
 referenciaBancariaSolicitudPagoTransporte VARCHAR(100) NULL,
 fechaPagoSolicitudPagoTransporte DATE NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqSolicitudServicio UNIQUE (Fk_ServicioTransporteExterno_Codigo),
 CONSTRAINT uqSolicitudCodigo UNIQUE (codigoSolicitudPagoTransporte),
 CONSTRAINT fkSolicitudServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo)
  REFERENCES tblServicioTransporteExterno(Pk_ServicioTransporteExterno)
) ENGINE=InnoDB;

-- ============================================================
-- VISTA: SOLICITUDES DE PAGO
-- ============================================================
CREATE OR REPLACE VIEW vwSolicitudesPagoTransporte AS
SELECT
 s.codigoSolicitudPagoTransporte AS solicitud,
 t.numeroGuiaServicioTransporteExterno AS numeroGuia,
 e.nombreEmpresaTransportista AS beneficiario,
 CASE ts.nombreTipoServicio
  WHEN 'Refrigerado' THEN 'Servicio de transporte externo refrigerado'
  WHEN 'Congelado' THEN 'Servicio de transporte externo congelado'
  WHEN 'Carga seca' THEN 'Servicio de transporte externo de carga seca'
  WHEN 'Carga especial' THEN 'Servicio de transporte externo de carga especial'
  ELSE CONCAT('Servicio de transporte externo ', LOWER(ts.nombreTipoServicio))
 END AS concepto,
 c.costoTotalTransporte AS montoTotal,
 s.fechaPagoSolicitudPagoTransporte AS fechaPago,
 s.estadoSolicitudPagoTransporte AS estadoSolicitud,
 s.referenciaBancariaSolicitudPagoTransporte AS referenciaBancaria
FROM tblServicioTransporteExterno t
INNER JOIN tblEmpresaTransportista e
 ON e.Pk_EmpresaTransportista = t.Fk_EmpresaTransportista_Codigo
INNER JOIN tblTipoServicio ts
 ON ts.Pk_TipoServicio = t.Fk_TipoServicio_Codigo
INNER JOIN tblCostoTransporte c
 ON c.Fk_ServicioTransporteExterno_Codigo = t.Pk_ServicioTransporteExterno
INNER JOIN tblSolicitudPagoTransporte s
 ON s.Fk_ServicioTransporteExterno_Codigo = t.Pk_ServicioTransporteExterno;

DROP PROCEDURE IF EXISTS spGenerarSolicitudPagoTransporte;
DELIMITER $$
-- ============================================================
-- PROCEDIMIENTO: GENERAR SOLICITUD DE PAGO
-- ============================================================
CREATE PROCEDURE spGenerarSolicitudPagoTransporte(IN pIdServicio INT)
BEGIN
 DECLARE vId INT;
 DECLARE EXIT HANDLER FOR SQLEXCEPTION
 BEGIN
  ROLLBACK;
  RESIGNAL;
 END;
 START TRANSACTION;
 IF NOT EXISTS (
  SELECT 1 FROM tblCostoTransporte
  WHERE Fk_ServicioTransporteExterno_Codigo = pIdServicio
 ) THEN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El servicio no existe o no tiene costo registrado';
 END IF;
 INSERT INTO tblSolicitudPagoTransporte (Fk_ServicioTransporteExterno_Codigo)
 VALUES (pIdServicio);
 SET vId = LAST_INSERT_ID();
 UPDATE tblSolicitudPagoTransporte
 SET codigoSolicitudPagoTransporte = CONCAT('SP-', LPAD(vId, 6, '0'))
 WHERE Pk_SolicitudPagoTransporte = vId;
 COMMIT;
 SELECT vId AS idSolicitud, CONCAT('SP-', LPAD(vId, 6, '0')) AS codigoSolicitudPago;
END$$
DELIMITER ;

DROP PROCEDURE IF EXISTS spRegistrarServicioTransporteExterno;
DELIMITER $$
-- ============================================================
-- PROCEDIMIENTO: REGISTRAR SERVICIO DE TRANSPORTE EXTERNO
-- ============================================================
CREATE PROCEDURE spRegistrarServicioTransporteExterno(
 IN pEmpresa INT, IN pTipoServicio INT,
 IN pOrigen VARCHAR(100), IN pDireccionOrigen VARCHAR(500),
 IN pDestino VARCHAR(100), IN pDireccionDestino VARCHAR(500),
 IN pFechaContratacion DATE, IN pFechaEstimada DATE, IN pFechaReal DATE,
 IN pTarifa INT, IN pCantidad DECIMAL(12,4), IN pCosto DECIMAL(12,4),
 IN pMoneda CHAR(3), IN pTipoPago VARCHAR(30)
)
BEGIN
 DECLARE vId INT;
 DECLARE vEmpresa VARCHAR(100);
 DECLARE vPrefijo VARCHAR(10);
 DECLARE vGuia VARCHAR(30);
 DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;
 START TRANSACTION;
 SELECT nombreEmpresaTransportista INTO vEmpresa
 FROM tblEmpresaTransportista WHERE Pk_EmpresaTransportista=pEmpresa AND is_active=TRUE;
 IF vEmpresa IS NULL THEN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Empresa transportista no válida';
 END IF;
 SET vPrefijo=CASE UPPER(TRIM(vEmpresa))
  WHEN 'FORZA' THEN 'FZ'
  WHEN 'CARGO EXPRESO' THEN 'CE'
  WHEN 'GUATEX' THEN 'GX'
  WHEN 'DHL EXPRESS' THEN 'DHL'
  WHEN 'FEDEX' THEN 'FDX'
  WHEN 'UPS' THEN 'UPS'
  ELSE 'TR' END;
 INSERT INTO tblServicioTransporteExterno (
  Fk_EmpresaTransportista_Codigo,Fk_TipoServicio_Codigo,
  origenServicioTransporteExterno,direccionOrigenServicioTransporteExterno,
  destinoServicioTransporteExterno,direccionDestinoServicioTransporteExterno,
  fechaContratacionServicioTransporteExterno,fechaEstimadaEntregaServicioTransporteExterno,
  fechaEntregaRealServicioTransporteExterno
 ) VALUES (pEmpresa,pTipoServicio,pOrigen,pDireccionOrigen,
           pDestino,pDireccionDestino,pFechaContratacion,pFechaEstimada,pFechaReal);
 SET vId=LAST_INSERT_ID();
 SET vGuia=CONCAT(vPrefijo,'-',LPAD(vId,6,'0'));
 UPDATE tblServicioTransporteExterno SET numeroGuiaServicioTransporteExterno=vGuia
 WHERE Pk_ServicioTransporteExterno=vId;
 INSERT INTO tblCostoTransporte (
  Fk_ServicioTransporteExterno_Codigo,Fk_TarifaTransporte_Codigo,
  cantidadCostoTransporte,costoUnitarioTransporte,costoTotalTransporte,
  monedaCostoTransporte,tipoPagoCostoTransporte
 ) VALUES (vId,pTarifa,pCantidad,pCosto,ROUND(pCantidad*pCosto,4),pMoneda,pTipoPago);
 COMMIT;
 SELECT vId AS idServicio, vGuia AS numeroGuia;
END$$

DELIMITER ;
