-- ============================================================
-- 1. EMPRESA TRANSPORTISTA
-- ============================================================
CREATE TABLE IF NOT EXISTS tblEmpresaTransportista (
    idEmpresaTransportista INT NOT NULL AUTO_INCREMENT,
    nombreEmpresaTransportista VARCHAR(100) NOT NULL,
    contactoEmpresaTransportista VARCHAR(100),
    telefonoEmpresaTransportista VARCHAR(30),
    nitEmpresaTransportista CHAR(10) NULL,
    direccionEmpresaTransportista VARCHAR(500) NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_EmpresaTransportista PRIMARY KEY (idEmpresaTransportista),
    CONSTRAINT uqEmpresaTransportistaNombre UNIQUE (nombreEmpresaTransportista)
) ENGINE=InnoDB;

-- ============================================================
-- 2. TIPO SERVICIO
-- ============================================================
CREATE TABLE IF NOT EXISTS tblTipoServicio (
    idTipoServicio INT NOT NULL AUTO_INCREMENT,
    nombreTipoServicio VARCHAR(100) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_TipoServicio PRIMARY KEY (idTipoServicio),
    CONSTRAINT uqTipoServicioNombre UNIQUE (nombreTipoServicio)
) ENGINE=InnoDB;

-- ============================================================
-- 3. TARIFA TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblTarifaTransporte (
    idTarifaTransporte INT NOT NULL AUTO_INCREMENT,
    nombreTarifaTransporte VARCHAR(100) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_TarifaTransporte PRIMARY KEY (idTarifaTransporte),
    CONSTRAINT uqTarifaTransporteNombre UNIQUE (nombreTarifaTransporte)
) ENGINE=InnoDB;

-- ============================================================
-- 4. SERVICIO TRANSPORTE EXTERNO
-- ============================================================
CREATE TABLE IF NOT EXISTS tblServicioTransporteExterno (
    idServicioTransporteExterno INT NOT NULL AUTO_INCREMENT,
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
    CONSTRAINT Pk_ServicioTransporteExterno PRIMARY KEY (idServicioTransporteExterno),
    CONSTRAINT uqServicioNumeroGuia UNIQUE (numeroGuiaServicioTransporteExterno),
    CONSTRAINT ckServicioEstado CHECK (estadoServicioTransporteExterno IN ('Programado','En tránsito','Entregado','Cancelado')),
    CONSTRAINT ckServicioFechas CHECK (fechaEstimadaEntregaServicioTransporteExterno >= fechaContratacionServicioTransporteExterno),
    CONSTRAINT fkServicioEmpresa FOREIGN KEY (Fk_EmpresaTransportista_Codigo) REFERENCES tblEmpresaTransportista(idEmpresaTransportista),
    CONSTRAINT fkServicioTipo FOREIGN KEY (Fk_TipoServicio_Codigo) REFERENCES tblTipoServicio(idTipoServicio),
    CONSTRAINT fkServicioTraslado FOREIGN KEY (Fk_Traslado_IdTraslado) REFERENCES tbltraslado(idTraslado)
) ENGINE=InnoDB;

-- ============================================================
-- 5. COSTO TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblCostoTransporte (
    idCostoTransporte INT NOT NULL AUTO_INCREMENT,
    Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
    Fk_TarifaTransporte_Codigo INT NOT NULL,
    cantidadCostoTransporte DECIMAL(12,4) NOT NULL,
    costoUnitarioTransporte DECIMAL(12,4) NOT NULL,
    costoTotalTransporte DECIMAL(12,4) NOT NULL,
    monedaCostoTransporte CHAR(3) NOT NULL,
    tipoPagoCostoTransporte VARCHAR(30) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_CostoTransporte PRIMARY KEY (idCostoTransporte),
    CONSTRAINT uqCostoServicio UNIQUE (Fk_ServicioTransporteExterno_Codigo),
    CONSTRAINT ckCostoCantidad CHECK (cantidadCostoTransporte > 0),
    CONSTRAINT ckCostoUnitario CHECK (costoUnitarioTransporte >= 0),
    CONSTRAINT ckCostoMoneda CHECK (monedaCostoTransporte IN ('GTQ','USD')),
    CONSTRAINT ckCostoTipoPago CHECK (tipoPagoCostoTransporte IN ('Transferencia','Efectivo')),
    CONSTRAINT fkCostoServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno),
    CONSTRAINT fkCostoTarifa FOREIGN KEY (Fk_TarifaTransporte_Codigo) REFERENCES tblTarifaTransporte(idTarifaTransporte)
) ENGINE=InnoDB;

-- ============================================================
-- 6. VEHICULO TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblVehiculoTransporte (
    idVehiculoTransporte INT NOT NULL AUTO_INCREMENT,
    Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
    placaVehiculoTransporte VARCHAR(30),
    tipoVehiculoTransporte VARCHAR(100),
    capacidadVehiculoTransporte VARCHAR(100),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_VehiculoTransporte PRIMARY KEY (idVehiculoTransporte),
    CONSTRAINT uqVehiculoServicio UNIQUE (Fk_ServicioTransporteExterno_Codigo),
    CONSTRAINT fkVehiculoServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno)
) ENGINE=InnoDB;

-- ============================================================
-- 7. CONDUCTOR TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblConductorTransporte (
    idConductorTransporte INT NOT NULL AUTO_INCREMENT,
    Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
    nombreConductorTransporte VARCHAR(100),
    telefonoConductorTransporte VARCHAR(30),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ConductorTransporte PRIMARY KEY (idConductorTransporte),
    CONSTRAINT uqConductorServicio UNIQUE (Fk_ServicioTransporteExterno_Codigo),
    CONSTRAINT fkConductorServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno)
) ENGINE=InnoDB;

-- ============================================================
-- 8. PRODUCTO TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblProductoTransporte (
    idProductoTransporte INT NOT NULL AUTO_INCREMENT,
    Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
    Fk_Producto_IdProducto INT NOT NULL,
    cantidadProductoTransporte DECIMAL(12,4) NOT NULL,
    unidadMedidaProductoTransporte VARCHAR(30),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ProductoTransporte PRIMARY KEY (idProductoTransporte),
    CONSTRAINT ckProductoCantidad CHECK (cantidadProductoTransporte > 0),
    CONSTRAINT fkProductoServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno),
    CONSTRAINT fkProductoCatalogo FOREIGN KEY (Fk_Producto_IdProducto) REFERENCES tblproducto(idProducto)
) ENGINE=InnoDB;

-- ============================================================
-- 9. UBICACION TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblUbicacionTransporte (
    idUbicacionTransporte INT NOT NULL AUTO_INCREMENT,
    Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
    ubicacionActualTransporte VARCHAR(500) NOT NULL,
    fechaActualizacionUbicacionTransporte DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_UbicacionTransporte PRIMARY KEY (idUbicacionTransporte),
    CONSTRAINT fkUbicacionServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno),
    INDEX idxUbicacionServicioFecha (Fk_ServicioTransporteExterno_Codigo, fechaActualizacionUbicacionTransporte)
) ENGINE=InnoDB;

-- ============================================================
-- 10. INCIDENCIA TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblIncidenciaTransporte (
    idIncidenciaTransporte INT NOT NULL AUTO_INCREMENT,
    Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
    codigoIncidenciaTransporte VARCHAR(30) NOT NULL,
    fechaIncidenciaTransporte DATETIME NOT NULL,
    tipoIncidenciaTransporte VARCHAR(100) NOT NULL,
    descripcionIncidenciaTransporte TEXT NOT NULL,
    responsableIncidenciaTransporte VARCHAR(100),
    estadoIncidenciaTransporte VARCHAR(30) NOT NULL DEFAULT 'Pendiente',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_IncidenciaTransporte PRIMARY KEY (idIncidenciaTransporte),
    CONSTRAINT uqIncidenciaCodigo UNIQUE (codigoIncidenciaTransporte),
    CONSTRAINT fkIncidenciaServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno)
) ENGINE=InnoDB;

-- ============================================================
-- 11. HISTORIAL TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblHistorialTransporte (
    idHistorialTransporte INT NOT NULL AUTO_INCREMENT,
    Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
    fechaMovimientoHistorial DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    tipoMovimientoHistorial VARCHAR(100) NOT NULL,
    descripcionMovimientoHistorial VARCHAR(255) NOT NULL,
    estadoTransporteHistorial VARCHAR(30),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_HistorialTransporte PRIMARY KEY (idHistorialTransporte),
    CONSTRAINT fkHistorialServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno)
) ENGINE=InnoDB;

-- ============================================================
-- 12. FACTURA TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblFacturaTransporte (
    idFacturaTransporte INT NOT NULL AUTO_INCREMENT,
    Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
    Fk_Plazo_IdPlazo INT NOT NULL,

    -- Relación con la PK compuesta de Cuenta por Pagar
    Fk_SerieDocumentoCuentaPorPagar VARCHAR(20) NULL,
    Fk_NumeroDocumentoCuentaPorPagar VARCHAR(30) NULL,
    Fk_IdConceptoProveedor INT NULL,

    serieFacturaTransporte VARCHAR(30) NOT NULL,
    numeroFacturaTransporte VARCHAR(30) NOT NULL,
    fechaEmisionFacturaTransporte DATE NOT NULL,
    montoFacturaTransporte DECIMAL(12,4) NOT NULL,
    activaFacturaTransporte BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_FacturaTransporte PRIMARY KEY (idFacturaTransporte),
    CONSTRAINT uqFacturaServicio UNIQUE (Fk_ServicioTransporteExterno_Codigo),

    CONSTRAINT uqFacturaCuentaPorPagar UNIQUE (
        Fk_SerieDocumentoCuentaPorPagar,
        Fk_NumeroDocumentoCuentaPorPagar,
        Fk_IdConceptoProveedor
    ),

    CONSTRAINT uqFacturaDocumento UNIQUE (serieFacturaTransporte, numeroFacturaTransporte),

    CONSTRAINT ckFacturaMonto CHECK (montoFacturaTransporte > 0),

    CONSTRAINT fkFacturaServicio
        FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo)
        REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno),

    CONSTRAINT fkFacturaPlazo
        FOREIGN KEY (Fk_Plazo_IdPlazo)
        REFERENCES tblPlazo(idPlazo),

    CONSTRAINT fkFacturaCuentaPorPagar
        FOREIGN KEY (
            Fk_SerieDocumentoCuentaPorPagar,
            Fk_NumeroDocumentoCuentaPorPagar,
            Fk_IdConceptoProveedor
        )
        REFERENCES tblCuentaPorPagar (
            serieDocumentoCuentaPorPagar,
            numeroDocumentoCuentaPorPagar,
            idConceptoProveedor
        )
) ENGINE=InnoDB;

-- ============================================================
-- 13. SOLICITUD PAGO TRANSPORTE
-- ============================================================
CREATE TABLE IF NOT EXISTS tblSolicitudPagoTransporte (
    idSolicitudPagoTransporte INT NOT NULL AUTO_INCREMENT,
    Fk_ServicioTransporteExterno_Codigo INT NOT NULL,
    codigoSolicitudPagoTransporte VARCHAR(30) NULL,
    estadoSolicitudPagoTransporte VARCHAR(30) NOT NULL DEFAULT 'Solicitado',
    referenciaBancariaSolicitudPagoTransporte VARCHAR(100) NULL,
    fechaPagoSolicitudPagoTransporte DATE NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_SolicitudPagoTransporte PRIMARY KEY (idSolicitudPagoTransporte),
    CONSTRAINT uqSolicitudServicio UNIQUE (Fk_ServicioTransporteExterno_Codigo),
    CONSTRAINT uqSolicitudCodigo UNIQUE (codigoSolicitudPagoTransporte),
    CONSTRAINT fkSolicitudServicio FOREIGN KEY (Fk_ServicioTransporteExterno_Codigo)
        REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno)
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
    ON e.idEmpresaTransportista = t.Fk_EmpresaTransportista_Codigo
INNER JOIN tblTipoServicio ts
    ON ts.idTipoServicio = t.Fk_TipoServicio_Codigo
INNER JOIN tblCostoTransporte c
    ON c.Fk_ServicioTransporteExterno_Codigo = t.idServicioTransporteExterno
INNER JOIN tblSolicitudPagoTransporte s
    ON s.Fk_ServicioTransporteExterno_Codigo = t.idServicioTransporteExterno;

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
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El servicio no existe o no tiene costo registrado';
    END IF;

    INSERT INTO tblSolicitudPagoTransporte (Fk_ServicioTransporteExterno_Codigo)
    VALUES (pIdServicio);

    SET vId = LAST_INSERT_ID();

    UPDATE tblSolicitudPagoTransporte
    SET codigoSolicitudPagoTransporte = CONCAT('SP-', LPAD(vId, 6, '0'))
    WHERE idSolicitudPagoTransporte = vId;

    COMMIT;

    SELECT vId AS idSolicitud,
           CONCAT('SP-', LPAD(vId, 6, '0')) AS codigoSolicitudPago;
END$$

DELIMITER ;

DROP PROCEDURE IF EXISTS spRegistrarServicioTransporteExterno;
DELIMITER $$

-- ============================================================
-- PROCEDIMIENTO: REGISTRAR SERVICIO DE TRANSPORTE EXTERNO
-- ============================================================
CREATE PROCEDURE spRegistrarServicioTransporteExterno(
    IN pEmpresa INT,
    IN pTipoServicio INT,
    IN pOrigen VARCHAR(100),
    IN pDireccionOrigen VARCHAR(500),
    IN pDestino VARCHAR(100),
    IN pDireccionDestino VARCHAR(500),
    IN pFechaContratacion DATE,
    IN pFechaEstimada DATE,
    IN pFechaReal DATE,
    IN pTarifa INT,
    IN pCantidad DECIMAL(12,4),
    IN pCosto DECIMAL(12,4),
    IN pMoneda CHAR(3),
    IN pTipoPago VARCHAR(30)
)
BEGIN
    DECLARE vId INT;
    DECLARE vEmpresa VARCHAR(100);
    DECLARE vPrefijo VARCHAR(10);
    DECLARE vGuia VARCHAR(30);

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT nombreEmpresaTransportista INTO vEmpresa
    FROM tblEmpresaTransportista
    WHERE idEmpresaTransportista = pEmpresa
      AND is_active = TRUE;

    IF vEmpresa IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Empresa transportista no válida';
    END IF;

    SET vPrefijo = CASE UPPER(TRIM(vEmpresa))
        WHEN 'FORZA' THEN 'FZ'
        WHEN 'CARGO EXPRESO' THEN 'CE'
        WHEN 'GUATEX' THEN 'GX'
        WHEN 'DHL EXPRESS' THEN 'DHL'
        WHEN 'FEDEX' THEN 'FDX'
        WHEN 'UPS' THEN 'UPS'
        ELSE 'TR'
    END;

    INSERT INTO tblServicioTransporteExterno (
        Fk_EmpresaTransportista_Codigo,
        Fk_TipoServicio_Codigo,
        origenServicioTransporteExterno,
        direccionOrigenServicioTransporteExterno,
        destinoServicioTransporteExterno,
        direccionDestinoServicioTransporteExterno,
        fechaContratacionServicioTransporteExterno,
        fechaEstimadaEntregaServicioTransporteExterno,
        fechaEntregaRealServicioTransporteExterno
    )
    VALUES (
        pEmpresa, pTipoServicio, pOrigen, pDireccionOrigen,
        pDestino, pDireccionDestino, pFechaContratacion,
        pFechaEstimada, pFechaReal
    );

    SET vId = LAST_INSERT_ID();
    SET vGuia = CONCAT(vPrefijo, '-', LPAD(vId, 6, '0'));

    UPDATE tblServicioTransporteExterno
    SET numeroGuiaServicioTransporteExterno = vGuia
    WHERE idServicioTransporteExterno = vId;

    INSERT INTO tblCostoTransporte (
        Fk_ServicioTransporteExterno_Codigo,
        Fk_TarifaTransporte_Codigo,
        cantidadCostoTransporte,
        costoUnitarioTransporte,
        costoTotalTransporte,
        monedaCostoTransporte,
        tipoPagoCostoTransporte
    )
    VALUES (
        vId, pTarifa, pCantidad, pCosto,
        ROUND(pCantidad * pCosto, 4), pMoneda, pTipoPago
    );

    COMMIT;

    SELECT vId AS idServicio, vGuia AS numeroGuia;
END$$

DELIMITER ;

-- ============================================================
-- TABLA: CUENTA BANCARIA TRANSPORTISTA
-- ============================================================
CREATE TABLE IF NOT EXISTS tblCuentaBancariaTransportista (
    idCuentaBancariaTransportista INT NOT NULL AUTO_INCREMENT,
    Fk_Empresa_Transportista_Codigo INT NOT NULL,
    Fk_Banco_Codigo BIGINT NOT NULL,
    Fk_TipoCuentaBancaria_Codigo BIGINT NOT NULL,
    Fk_Moneda_Codigo BIGINT NOT NULL,
    numeroCuentaBancariaTransportista VARCHAR(100) NOT NULL,
    titularCuentaBancariaTransportista VARCHAR(100) NOT NULL,
    activaCuentaBancariaTransportista BOOLEAN NOT NULL DEFAULT TRUE,
    predeterminadaCuentaBancariaTransportista BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_Cuenta_Bancaria_Transportista PRIMARY KEY (idCuentaBancariaTransportista),

    CONSTRAINT uqCuentaBancariaTransportista
        UNIQUE (Fk_Banco_Codigo, numeroCuentaBancariaTransportista),

    CONSTRAINT Fk_CuentaTransportista_Empresa
        FOREIGN KEY (Fk_Empresa_Transportista_Codigo)
        REFERENCES tblEmpresaTransportista(idEmpresaTransportista)
        ON DELETE RESTRICT,

    CONSTRAINT Fk_CuentaTransportista_Banco
        FOREIGN KEY (Fk_Banco_Codigo)
        REFERENCES dbsistemaembutidos.tblBanco(idBanco)
        ON DELETE RESTRICT,

    CONSTRAINT Fk_CuentaTransportista_Tipo
        FOREIGN KEY (Fk_TipoCuentaBancaria_Codigo)
        REFERENCES dbsistemaembutidos.tblTipoCuentaBancaria(idTipoCuentaBancaria)
        ON DELETE RESTRICT,

    CONSTRAINT Fk_CuentaTransportista_Moneda
        FOREIGN KEY (Fk_Moneda_Codigo)
        REFERENCES dbsistemaembutidos.tblMoneda(idMoneda)
        ON DELETE RESTRICT
) ENGINE=InnoDB;