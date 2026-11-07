
CREATE DATABASE IF NOT EXISTS dbSistemaEmbutidosTransporte
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;
USE dbSistemaEmbutidosTransporte;

/* ============================================================================
   1. CATÁLOGOS BASE
   ============================================================================ */

CREATE TABLE tblEstado (
    idEstado INT AUTO_INCREMENT NOT NULL,
    tipoEstado VARCHAR(100) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Estado PRIMARY KEY (idEstado),
    CONSTRAINT uqTipoEstado UNIQUE (tipoEstado)
) ENGINE=InnoDB;

CREATE TABLE tblTipoTransporte (
    idTipoTransporte INT AUTO_INCREMENT NOT NULL,
    nombreTipoTransporte VARCHAR(100) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_TipoTransporte PRIMARY KEY (idTipoTransporte),
    CONSTRAINT uqNombreTipoTransporte UNIQUE (nombreTipoTransporte)
) ENGINE=InnoDB;

/* ============================================================================
   2. ENTIDADES DE FLOTA Y CONDUCTORES
   ============================================================================ */

CREATE TABLE tblTransporte (
    idTransporte INT AUTO_INCREMENT NOT NULL,
    nroPlacaTransporte VARCHAR(30) NOT NULL,
    marcaTransporte VARCHAR(100) NULL,
    modeloTransporte VARCHAR(100) NULL,
    anioTransporte SMALLINT NULL,
    esRefrigeradoTransporte BOOLEAN NOT NULL DEFAULT FALSE,
    idEstado INT NOT NULL,
    idTipoTransporte INT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Transporte PRIMARY KEY (idTransporte),
    CONSTRAINT uqNroPlacaTransporte UNIQUE (nroPlacaTransporte),
    INDEX idxIdEstadoTransporte (idEstado),
    INDEX idxIdTipoTransporteTransporte (idTipoTransporte),
    CONSTRAINT Fk_Transporte_Estado FOREIGN KEY (idEstado) REFERENCES tblEstado(idEstado),
    CONSTRAINT Fk_Transporte_TipoTransporte FOREIGN KEY (idTipoTransporte) REFERENCES tblTipoTransporte(idTipoTransporte)
) ENGINE=InnoDB;

CREATE TABLE tblCamaraRefrigeracion (
    idCamaraRefrigeracion INT AUTO_INCREMENT NOT NULL,
    idTransporte INT NOT NULL,
    marcaEquipoCamaraRefrigeracion VARCHAR(100) NOT NULL,
    tempMinimaCelsiusCamaraRefrigeracion DECIMAL(5,2) NOT NULL,
    tempMaximaCelsiusCamaraRefrigeracion DECIMAL(5,2) NOT NULL,
    estadoOperativoCamaraRefrigeracion VARCHAR(30) NOT NULL DEFAULT 'OPERATIVO',
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_CamaraRefrigeracion PRIMARY KEY (idCamaraRefrigeracion),
    CONSTRAINT uqIdTransporteCamaraRefrigeracion UNIQUE (idTransporte),
    CONSTRAINT Fk_CamaraRefrigeracion_Transporte FOREIGN KEY (idTransporte) REFERENCES tblTransporte(idTransporte)
) ENGINE=InnoDB;

CREATE TABLE tblConductor (
    idConductor INT AUTO_INCREMENT NOT NULL,
    codigoConductor VARCHAR(30) NOT NULL,
    nombreCompletoConductor VARCHAR(100) NOT NULL,
    numeroLicenciaConductor VARCHAR(30) NOT NULL,
    fechaVencimientoDocConductor DATE NOT NULL,
    idEstado INT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Conductor PRIMARY KEY (idConductor),
    CONSTRAINT uqCodigoConductor UNIQUE (codigoConductor),
    CONSTRAINT uqNumeroLicenciaConductor UNIQUE (numeroLicenciaConductor),
    INDEX idxIdEstadoConductor (idEstado),
    CONSTRAINT Fk_Conductor_Estado FOREIGN KEY (idEstado) REFERENCES tblEstado(idEstado)
) ENGINE=InnoDB;

/* ============================================================================
   3. MÓDULO DE INOCUIDAD Y HACCP (INSPECCIÓN SANITARIA)
   ============================================================================ */

CREATE TABLE tblInspeccionSanitaria (
    idInspeccionSanitaria BIGINT AUTO_INCREMENT NOT NULL,
    idTransporte INT NOT NULL,
    fechaUtcInspeccionSanitaria DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    sanitizadoCorrectoInspeccionSanitaria BOOLEAN NOT NULL,
    libreOloresContamInspeccionSanitaria BOOLEAN NOT NULL,
    aprobadoParaCargaInspeccionSanitaria BOOLEAN NOT NULL,
    observacionesInspeccionSanitaria VARCHAR(500) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_InspeccionSanitaria PRIMARY KEY (idInspeccionSanitaria),
    INDEX idxIdTransporteInspeccionSanitaria (idTransporte),
    CONSTRAINT Fk_InspeccionSanitaria_Transporte FOREIGN KEY (idTransporte) REFERENCES tblTransporte(idTransporte)
) ENGINE=InnoDB;

/* ============================================================================
   4. SOLICITUDES, VIAJES Y LOTES DE PRODUCTO
   ============================================================================ */

CREATE TABLE tblLoteProducto (
    idLoteProducto BIGINT AUTO_INCREMENT NOT NULL,
    codigoLoteProducto VARCHAR(30) NOT NULL,
    descripcionLoteProducto VARCHAR(255) NOT NULL,
    requiereFrioLoteProducto BOOLEAN NOT NULL DEFAULT TRUE,
    fechaFabricacionLoteProducto DATE NOT NULL,
    fechaVencimientoLoteProducto DATE NOT NULL,
    tempRequeridaMinLoteProducto DECIMAL(5,2) NULL,
    tempRequeridaMaxLoteProducto DECIMAL(5,2) NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_LoteProducto PRIMARY KEY (idLoteProducto),
    CONSTRAINT uqCodigoLoteProducto UNIQUE (codigoLoteProducto)
) ENGINE=InnoDB;

CREATE TABLE tblEncabezadoViaje (
    correlativoEncabezadoViaje BIGINT AUTO_INCREMENT NOT NULL,
    fechaDocumentoEncabezadoViaje DATETIME NOT NULL,
    horaSalidaUtcEncabezadoViaje DATETIME NULL,
    horaLlegadaUtcEncabezadoViaje DATETIME NULL,
    requiereCadenaFrioEncabezadoViaje BOOLEAN NOT NULL DEFAULT TRUE,
    estadoViajeEncabezadoViaje VARCHAR(30) NOT NULL DEFAULT 'EN_CURSO',
    pesoTotalEncabezadoViaje DECIMAL(18,3) NULL,
    idTransporte INT NOT NULL,
    idConductor INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_EncabezadoViaje PRIMARY KEY (correlativoEncabezadoViaje),
    INDEX idxIdTransporteEncabezadoViaje (idTransporte),
    INDEX idxIdConductorEncabezadoViaje (idConductor),
    INDEX idxEstadoViajeEncabezadoViaje (estadoViajeEncabezadoViaje),
    CONSTRAINT Fk_EncabezadoViaje_Transporte FOREIGN KEY (idTransporte) REFERENCES tblTransporte(idTransporte),
    CONSTRAINT Fk_EncabezadoViaje_Conductor FOREIGN KEY (idConductor) REFERENCES tblConductor(idConductor)
) ENGINE=InnoDB;

CREATE TABLE tblDetalleViaje (
    idDetalleViaje BIGINT AUTO_INCREMENT NOT NULL,
    correlativoEncabezadoViaje BIGINT NOT NULL,
    ordenDetalleViaje INT NOT NULL,
    descripcionDetalleViaje VARCHAR(255) NOT NULL,
    cantidadDetalleViaje DECIMAL(18,3) NOT NULL,
    pesoDetalleViaje DECIMAL(18,3) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_DetalleViaje PRIMARY KEY (idDetalleViaje),
    CONSTRAINT uqOrdenDetalleViaje UNIQUE (correlativoEncabezadoViaje, ordenDetalleViaje),
    CONSTRAINT Fk_DetalleViaje_EncabezadoViaje FOREIGN KEY (correlativoEncabezadoViaje) REFERENCES tblEncabezadoViaje(correlativoEncabezadoViaje)
) ENGINE=InnoDB;

/* Tabla intermedia (N:M) entre tblDetalleViaje y tblLoteProducto */
CREATE TABLE tblDetalleViajeLoteProducto (
    idDetalleViaje BIGINT NOT NULL,
    idLoteProducto BIGINT NOT NULL,
    cantidadCajasDetalleViajeLoteProducto INT NOT NULL,
    pesoKgDetalleViajeLoteProducto DECIMAL(18,3) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_DetalleViajeLoteProducto PRIMARY KEY (idDetalleViaje, idLoteProducto),
    INDEX idxIdLoteProductoDetalleViajeLoteProducto (idLoteProducto),
    CONSTRAINT Fk_DetalleViajeLoteProducto_DetalleViaje FOREIGN KEY (idDetalleViaje) REFERENCES tblDetalleViaje(idDetalleViaje),
    CONSTRAINT Fk_DetalleViajeLoteProducto_LoteProducto FOREIGN KEY (idLoteProducto) REFERENCES tblLoteProducto(idLoteProducto)
) ENGINE=InnoDB;

/* ============================================================================
   5. MÓDULO DE TELEMETRÍA IOT Y ALERTAS EN TIEMPO REAL
   ============================================================================ */

CREATE TABLE tblTelemetriaTemperatura (
    idTelemetriaTemperatura BIGINT AUTO_INCREMENT NOT NULL,
    correlativoEncabezadoViaje BIGINT NOT NULL,
    fechaHoraUtcTelemetriaTemperatura DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    temperaturaCelsiusTelemetriaTemperatura DECIMAL(5,2) NOT NULL,
    /* Porcentaje en formato decimal: 0.6500 = 65 % (DECIMAL(5,4) permite llegar a 100 %) */
    humedadRelativaTelemetriaTemperatura DECIMAL(5,4) NULL,
    puertaAbiertaTelemetriaTemperatura BOOLEAN NOT NULL DEFAULT FALSE,
    ubicacionGpsTelemetriaTemperatura VARCHAR(100) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_TelemetriaTemperatura PRIMARY KEY (idTelemetriaTemperatura),
    INDEX idxCorrelativoFechaTelemetriaTemperatura (correlativoEncabezadoViaje, fechaHoraUtcTelemetriaTemperatura),
    CONSTRAINT Fk_TelemetriaTemperatura_EncabezadoViaje FOREIGN KEY (correlativoEncabezadoViaje) REFERENCES tblEncabezadoViaje(correlativoEncabezadoViaje)
) ENGINE=InnoDB;

CREATE TABLE tblAlertaCadenaFrio (
    idAlertaCadenaFrio BIGINT AUTO_INCREMENT NOT NULL,
    correlativoEncabezadoViaje BIGINT NOT NULL,
    idTelemetriaTemperatura BIGINT NOT NULL,
    tipoAlertaCadenaFrio VARCHAR(30) NOT NULL,
    temperaturaRegistradaAlertaCadenaFrio DECIMAL(5,2) NOT NULL,
    atendidaAlertaCadenaFrio BOOLEAN NOT NULL DEFAULT FALSE,
    observacionCalidadAlertaCadenaFrio VARCHAR(500) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_AlertaCadenaFrio PRIMARY KEY (idAlertaCadenaFrio),
    INDEX idxCorrelativoEncabezadoViajeAlertaCadenaFrio (correlativoEncabezadoViaje),
    INDEX idxIdTelemetriaTemperaturaAlertaCadenaFrio (idTelemetriaTemperatura),
    INDEX idxAtendidaAlertaCadenaFrio (atendidaAlertaCadenaFrio),
    CONSTRAINT Fk_AlertaCadenaFrio_EncabezadoViaje FOREIGN KEY (correlativoEncabezadoViaje) REFERENCES tblEncabezadoViaje(correlativoEncabezadoViaje),
    CONSTRAINT Fk_AlertaCadenaFrio_TelemetriaTemperatura FOREIGN KEY (idTelemetriaTemperatura) REFERENCES tblTelemetriaTemperatura(idTelemetriaTemperatura)
) ENGINE=InnoDB;
