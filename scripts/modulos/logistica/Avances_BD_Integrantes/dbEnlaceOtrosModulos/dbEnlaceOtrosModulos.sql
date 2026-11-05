-- MODELO PROPUESTO - MySQL 8.0+
-- Estandarizacion de Base de Datos V1.3
-- NO ejecutar sobre produccion sin revisar dependencias y respaldar.
-- Se asume que tblSolicitudPagoTransporte no existe; si existe, migrar con ALTER TABLE.
-- Se asume que tblServicioTransporteExterno existe con PK Pk_Servicio_Transporte_Externo INT.
-- Ajustar el nombre/tipo de esa FK al esquema real antes de ejecutar.

USE dbsistemaembutidos;

CREATE TABLE tblEstadoSolicitudPago (
    Pk_Estado_Solicitud_Pago INT NOT NULL AUTO_INCREMENT,
    codigoEstadoSolicitudPago VARCHAR(30) NOT NULL,
    nombreEstadoSolicitudPago VARCHAR(100) NOT NULL,
    descripcionEstadoSolicitudPago VARCHAR(255) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Estado_Solicitud_Pago),
    CONSTRAINT uqCodigoEstadoSolicitudPago UNIQUE (codigoEstadoSolicitudPago)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

INSERT INTO tblEstadoSolicitudPago (codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago) VALUES
('SOLICITADA', 'Solicitada', 'Enviada desde Logistica a Bancos'),
('EN_REVISION', 'En revision', 'En revision por Bancos'),
('AUTORIZADA', 'Autorizada', 'Aprobada por Bancos, pendiente de pago'),
('RECHAZADA', 'Rechazada', 'Devuelta a Logistica para correccion'),
('PAGADA', 'Pagada', 'Pago registrado por Bancos');

CREATE TABLE tblSolicitudPagoTransporte (
    Pk_Solicitud_Pago_Transporte INT NOT NULL AUTO_INCREMENT,
    Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
    Fk_Estado_Solicitud_Pago_Codigo INT NOT NULL,
    codigoSolicitudPagoTransporte VARCHAR(30) NOT NULL,
    fechaSolicitudPagoTransporte DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    nombreBeneficiarioSolicitudPagoTransporte VARCHAR(100) NOT NULL,
    nitBeneficiarioSolicitudPagoTransporte CHAR(10) NOT NULL,
    direccionBeneficiarioSolicitudPagoTransporte VARCHAR(500) NOT NULL,
    bancoBeneficiarioSolicitudPagoTransporte VARCHAR(100) NOT NULL,
    tipoCuentaSolicitudPagoTransporte VARCHAR(30) NOT NULL,
    numeroCuentaSolicitudPagoTransporte VARCHAR(30) NOT NULL,
    monedaSolicitudPagoTransporte CHAR(3) NOT NULL,
    montoSolicitudPagoTransporte DECIMAL(12,4) NOT NULL,
    conceptoSolicitudPagoTransporte VARCHAR(500) NOT NULL,
    observacionActualSolicitudPagoTransporte TEXT NULL,
    usuarioSolicitanteSolicitudPagoTransporte VARCHAR(100) NOT NULL,
    fechaAutorizacionSolicitudPagoTransporte DATETIME NULL,
    fechaPagoSolicitudPagoTransporte DATETIME NULL,
    referenciaBancariaSolicitudPagoTransporte VARCHAR(100) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Solicitud_Pago_Transporte),
    CONSTRAINT uqCodigoSolicitudPagoTransporte UNIQUE (codigoSolicitudPagoTransporte),
    INDEX idxSolicitudEstadoFecha (Fk_Estado_Solicitud_Pago_Codigo, fechaSolicitudPagoTransporte),
    INDEX idxSolicitudServicio (Fk_Servicio_Transporte_Externo_Codigo),
    CONSTRAINT Fk_Solicitud_Pago_Estado FOREIGN KEY (Fk_Estado_Solicitud_Pago_Codigo)
        REFERENCES tblEstadoSolicitudPago(Pk_Estado_Solicitud_Pago),
    CONSTRAINT ckMontoSolicitudPago CHECK (montoSolicitudPagoTransporte > 0),
    CONSTRAINT ckMonedaSolicitudPago CHECK (monedaSolicitudPagoTransporte IN ('GTQ','USD')),
    CONSTRAINT ckTipoCuentaSolicitudPago CHECK (tipoCuentaSolicitudPagoTransporte IN ('MONETARIA','AHORRO'))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
 -- ACTIVAR SOLO TRAS CONFIRMAR EL NOMBRE Y TIPO DE LA PK REAL:
    -- CONSTRAINT Fk_Solicitud_Pago_Servicio FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo)
    --     REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo),
    
CREATE TABLE tblDetalleSolicitudPago (
    Pk_Detalle_Solicitud_Pago INT NOT NULL AUTO_INCREMENT,
    Fk_Solicitud_Pago_Transporte_Codigo INT NOT NULL,
    numeroLineaDetalleSolicitudPago INT NOT NULL,
    descripcionDetalleSolicitudPago VARCHAR(255) NOT NULL,
    cantidadDetalleSolicitudPago DECIMAL(12,4) NOT NULL,
    costoUnitarioDetalleSolicitudPago DECIMAL(12,4) NOT NULL,
    montoDetalleSolicitudPago DECIMAL(12,4) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Detalle_Solicitud_Pago),
    CONSTRAINT uqLineaDetalleSolicitudPago UNIQUE (Fk_Solicitud_Pago_Transporte_Codigo, numeroLineaDetalleSolicitudPago),
    CONSTRAINT Fk_Detalle_Solicitud_Pago_Solicitud FOREIGN KEY (Fk_Solicitud_Pago_Transporte_Codigo)
        REFERENCES tblSolicitudPagoTransporte(Pk_Solicitud_Pago_Transporte),
    CONSTRAINT ckNumeroLineaDetalle CHECK (numeroLineaDetalleSolicitudPago > 0),
    CONSTRAINT ckCantidadDetalle CHECK (cantidadDetalleSolicitudPago > 0),
    CONSTRAINT ckCostoUnitarioDetalle CHECK (costoUnitarioDetalleSolicitudPago >= 0),
    CONSTRAINT ckMontoDetalle CHECK (montoDetalleSolicitudPago >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE tblHistorialEstadoSolicitudPago (
    Pk_Historial_Estado_Solicitud_Pago BIGINT NOT NULL AUTO_INCREMENT,
    Fk_Solicitud_Pago_Transporte_Codigo INT NOT NULL,
    Fk_Estado_Anterior_Codigo INT NULL,
    Fk_Estado_Nuevo_Codigo INT NOT NULL,
    fechaCambioHistorialEstadoSolicitudPago DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    usuarioResponsableHistorialEstadoSolicitudPago VARCHAR(100) NOT NULL,
    moduloOrigenHistorialEstadoSolicitudPago VARCHAR(30) NOT NULL,
    observacionHistorialEstadoSolicitudPago TEXT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Historial_Estado_Solicitud_Pago),
    INDEX idxHistorialSolicitudFecha (Fk_Solicitud_Pago_Transporte_Codigo, fechaCambioHistorialEstadoSolicitudPago),
    CONSTRAINT Fk_Historial_Solicitud FOREIGN KEY (Fk_Solicitud_Pago_Transporte_Codigo)
        REFERENCES tblSolicitudPagoTransporte(Pk_Solicitud_Pago_Transporte),
    CONSTRAINT Fk_Historial_Estado_Anterior FOREIGN KEY (Fk_Estado_Anterior_Codigo)
        REFERENCES tblEstadoSolicitudPago(Pk_Estado_Solicitud_Pago),
    CONSTRAINT Fk_Historial_Estado_Nuevo FOREIGN KEY (Fk_Estado_Nuevo_Codigo)
        REFERENCES tblEstadoSolicitudPago(Pk_Estado_Solicitud_Pago),
    CONSTRAINT ckModuloOrigenHistorial CHECK (moduloOrigenHistorialEstadoSolicitudPago IN ('LOGISTICA','BANCOS'))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE tblSolicitudPagoDocumento (
    Pk_Documento_Solicitud_Pago BIGINT NOT NULL AUTO_INCREMENT,
    Fk_Solicitud_Pago_Transporte_Codigo INT NOT NULL,
    nombreDocumentoSolicitudPago VARCHAR(255) NOT NULL,
    tipoDocumentoSolicitudPago VARCHAR(100) NULL,
    rutaDocumentoSolicitudPago VARCHAR(500) NOT NULL,
    usuarioCargaDocumentoSolicitudPago VARCHAR(100) NOT NULL,
    fechaCargaDocumentoSolicitudPago DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Documento_Solicitud_Pago),
    INDEX idxDocumentoSolicitud (Fk_Solicitud_Pago_Transporte_Codigo),
    CONSTRAINT Fk_Documento_Solicitud_Pago_Solicitud FOREIGN KEY (Fk_Solicitud_Pago_Transporte_Codigo)
        REFERENCES tblSolicitudPagoTransporte(Pk_Solicitud_Pago_Transporte)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
