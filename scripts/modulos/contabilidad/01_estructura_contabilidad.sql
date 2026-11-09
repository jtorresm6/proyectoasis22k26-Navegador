
-- =============================================================================
--  TERMINUS ERP  |  Contabilidad
--  Script 1 de 2: estructura (tablas y vistas) para una base de datos nueva.
--  Motor: MySQL 8.0.46 o posterior
-- =============================================================================

CREATE DATABASE IF NOT EXISTS dbContabilidad CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE dbContabilidad;

-- =============================================================================
--  1. CATÁLOGOS
-- =============================================================================

CREATE TABLE tblPais (
    idPais      INT          NOT NULL AUTO_INCREMENT,
    nombrePais  VARCHAR(100) NOT NULL,
    is_active   BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at  TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at  TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Pais PRIMARY KEY (idPais),
    CONSTRAINT uqPaisNombre UNIQUE (nombrePais)
) ENGINE=InnoDB;

CREATE TABLE tblModuloContable (
    idModuloContable     INT          NOT NULL AUTO_INCREMENT,
    nombreModuloContable VARCHAR(100) NOT NULL,
    codigoModuloContable VARCHAR(30)  NOT NULL,
    is_active            BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at           TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at           TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ModuloContable PRIMARY KEY (idModuloContable),
    CONSTRAINT uqModuloNombre UNIQUE (nombreModuloContable),
    CONSTRAINT uqModuloCodigo UNIQUE (codigoModuloContable)
) ENGINE=InnoDB;

CREATE TABLE tblCuenta (
    idCuenta            INT           NOT NULL AUTO_INCREMENT,
    codigoCuenta        VARCHAR(30)   NOT NULL,
    nombreCuenta        VARCHAR(100)  NOT NULL,
    tipoCuenta          VARCHAR(30)   NOT NULL,
    clasificacionCuenta VARCHAR(10)   NOT NULL DEFAULT 'DETALLE',
    idCuentaPadre       INT           NULL,
    saldoInicialCuenta  DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    cargoPeriodoCuenta  DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    abonoPeriodoCuenta  DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    saldoActualCuenta   DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    cargoActualCuenta   DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    abonoActualCuenta   DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    periodoSaldoCuenta  CHAR(7)       NULL,
    is_active           BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at          TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at          TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Cuenta PRIMARY KEY (idCuenta),
    CONSTRAINT uqCuentaCodigo UNIQUE (codigoCuenta),
    CONSTRAINT ckCuentaTipo CHECK (tipoCuenta IN ('DEUDORA','ACREEDORA')),
    CONSTRAINT ckCuentaClasificacion CHECK (clasificacionCuenta IN ('MAYOR','DETALLE')),
    CONSTRAINT Fk_Cuenta_Cuenta FOREIGN KEY (idCuentaPadre) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxCuentaPadre (idCuentaPadre),
    INDEX idxCuentaTipo  (tipoCuenta),
    INDEX idxCuentaClasificacion (clasificacionCuenta),
    INDEX idxCuentaNombre (nombreCuenta)
) ENGINE=InnoDB;

CREATE TABLE tblModuloCuentaPais (
    idModuloContable INT       NOT NULL,
    idCuenta         INT       NOT NULL,
    idPais           INT       NOT NULL,
    is_active        BOOLEAN   NOT NULL DEFAULT TRUE,
    created_at       TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at       TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ModuloCuentaPais PRIMARY KEY (idModuloContable, idCuenta, idPais),
    CONSTRAINT Fk_ModuloCuentaPais_Modulo FOREIGN KEY (idModuloContable) REFERENCES tblModuloContable (idModuloContable) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_ModuloCuentaPais_Cuenta FOREIGN KEY (idCuenta) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_ModuloCuentaPais_Pais FOREIGN KEY (idPais) REFERENCES tblPais (idPais) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxModuloCuentaPaisCuenta (idCuenta),
    INDEX idxModuloCuentaPaisPais   (idPais)
) ENGINE=InnoDB;

-- =============================================================================
--  2. ACTUALIZACIÓN DE SALDOS Y SALDOS POR PERÍODO
-- =============================================================================

CREATE TABLE tblActualizacionSaldos (
    idActualizacionSaldos              INT          NOT NULL AUTO_INCREMENT,
    modoActualizacionSaldos            VARCHAR(10)  NOT NULL,
    tipoPeriodoActualizacionSaldos     VARCHAR(10)  NOT NULL,
    fechaDesdeActualizacionSaldos      DATE         NOT NULL,
    fechaHastaActualizacionSaldos      DATE         NOT NULL,
    polizasProcesadasActualizacionSaldos INT        NOT NULL DEFAULT 0,
    usuarioActualizacionSaldos         VARCHAR(100) NULL,
    is_active                          BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at                         TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                         TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ActualizacionSaldos PRIMARY KEY (idActualizacionSaldos),
    CONSTRAINT ckActualizacionModo     CHECK (modoActualizacionSaldos IN ('LINEA','BATCH')),
    CONSTRAINT ckActualizacionTipo     CHECK (tipoPeriodoActualizacionSaldos IN ('ANUAL','MENSUAL','SEMANAL','RANGO')),
    CONSTRAINT ckActualizacionFechas   CHECK (fechaHastaActualizacionSaldos >= fechaDesdeActualizacionSaldos),
    CONSTRAINT ckActualizacionPolizas  CHECK (polizasProcesadasActualizacionSaldos >= 0),
    INDEX idxActualizacionFechas (fechaDesdeActualizacionSaldos, fechaHastaActualizacionSaldos)
) ENGINE=InnoDB;

CREATE TABLE tblSaldoCuentaPeriodo (
    idSaldoCuentaPeriodo            INT           NOT NULL AUTO_INCREMENT,
    idCuenta                        INT           NOT NULL,
    idActualizacionSaldos           INT           NULL,
    periodoContableSaldoCuentaPeriodo CHAR(7)     NOT NULL,
    saldoInicialSaldoCuentaPeriodo  DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    cargoSaldoCuentaPeriodo         DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    abonoSaldoCuentaPeriodo         DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    saldoFinalSaldoCuentaPeriodo    DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    estadoSaldoCuentaPeriodo        VARCHAR(10)   NOT NULL DEFAULT 'ABIERTO',
    is_active                       BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at                      TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                      TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_SaldoCuentaPeriodo PRIMARY KEY (idSaldoCuentaPeriodo),
    CONSTRAINT uqSaldoCuentaPeriodo UNIQUE (idCuenta, periodoContableSaldoCuentaPeriodo),
    CONSTRAINT ckSaldoEstado      CHECK (estadoSaldoCuentaPeriodo IN ('ABIERTO','CERRADO')),
    CONSTRAINT ckSaldoMovimientos CHECK (cargoSaldoCuentaPeriodo >= 0 AND abonoSaldoCuentaPeriodo >= 0),
    CONSTRAINT Fk_SaldoCuentaPeriodo_Cuenta FOREIGN KEY (idCuenta) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_SaldoCuentaPeriodo_Actualizacion FOREIGN KEY (idActualizacionSaldos) REFERENCES tblActualizacionSaldos (idActualizacionSaldos) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxSaldoCuentaPeriodoPeriodo (periodoContableSaldoCuentaPeriodo),
    INDEX idxSaldoCuentaPeriodoActualizacion (idActualizacionSaldos)
) ENGINE=InnoDB;

-- =============================================================================
--  3. PÓLIZAS CONTABLES
-- =============================================================================

CREATE TABLE tblPolizaEncabezado (
    idPolizaEncabezado            INT          NOT NULL AUTO_INCREMENT,
    idModuloContable              INT          NOT NULL DEFAULT 1,
    idActualizacionSaldos         INT          NULL,
    codigoPolizaEncabezado        VARCHAR(30)  NOT NULL,
    fechaPolizaEncabezado         DATE         NOT NULL,
    periodoContablePolizaEncabezado CHAR(7)    NOT NULL,
    estadoPolizaEncabezado        VARCHAR(30)  NOT NULL DEFAULT 'BORRADOR',
    documentoOrigenPolizaEncabezado VARCHAR(30) NULL,
    descripcionPolizaEncabezado   VARCHAR(255) NULL,
    is_active                     BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at                    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PolizaEncabezado PRIMARY KEY (idPolizaEncabezado),
    CONSTRAINT uqPolizaCodigo UNIQUE (codigoPolizaEncabezado),
    CONSTRAINT uqPolizaDocumentoOrigen UNIQUE (idModuloContable, documentoOrigenPolizaEncabezado),
    CONSTRAINT ckPolizaEstado CHECK (estadoPolizaEncabezado IN ('BORRADOR','VALIDADA','APLICADA','ANULADA')),
    CONSTRAINT Fk_PolizaEncabezado_Modulo FOREIGN KEY (idModuloContable) REFERENCES tblModuloContable (idModuloContable) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_PolizaEncabezado_Actualizacion FOREIGN KEY (idActualizacionSaldos) REFERENCES tblActualizacionSaldos (idActualizacionSaldos) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxPolizaModulo  (idModuloContable),
    INDEX idxPolizaPeriodo (periodoContablePolizaEncabezado),
    INDEX idxPolizaFecha   (fechaPolizaEncabezado),
    INDEX idxPolizaEstado  (estadoPolizaEncabezado),
    INDEX idxPolizaActualizacion (idActualizacionSaldos)
) ENGINE=InnoDB;

CREATE TABLE tblPolizaDetalle (
    idPolizaDetalle             INT           NOT NULL AUTO_INCREMENT,
    idPolizaEncabezado          INT           NOT NULL,
    idCuenta                    INT           NOT NULL,
    lineaPolizaDetalle          INT           NOT NULL,
    debePolizaDetalle           DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    haberPolizaDetalle          DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    descripcionPolizaDetalle    VARCHAR(255)  NULL,
    is_active                   BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at                  TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                  TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PolizaDetalle PRIMARY KEY (idPolizaDetalle),
    CONSTRAINT uqPolizaDetalleLinea UNIQUE (idPolizaEncabezado, lineaPolizaDetalle),
    CONSTRAINT ckDetalleMontoPositivo CHECK (debePolizaDetalle >= 0 AND haberPolizaDetalle >= 0),
    CONSTRAINT ckDetallePartidaDoble  CHECK (debePolizaDetalle = 0 OR haberPolizaDetalle = 0),
    CONSTRAINT Fk_PolizaDetalle_PolizaEncabezado FOREIGN KEY (idPolizaEncabezado) REFERENCES tblPolizaEncabezado (idPolizaEncabezado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_PolizaDetalle_Cuenta FOREIGN KEY (idCuenta) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxPolizaDetalleEncabezado (idPolizaEncabezado),
    INDEX idxPolizaDetalleCuenta     (idCuenta)
) ENGINE=InnoDB;

-- =============================================================================
--  4. PRESUPUESTO
-- =============================================================================

CREATE TABLE tblPresupuestoEncabezado (
    idPresupuestoEncabezado            INT           NOT NULL AUTO_INCREMENT,
    nombrePresupuestoEncabezado        VARCHAR(100)  NOT NULL,
    totalPresupuestoEncabezado         DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    gastadoPresupuestoEncabezado       DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    restantePresupuestoEncabezado      DECIMAL(12,4) GENERATED ALWAYS AS (totalPresupuestoEncabezado - gastadoPresupuestoEncabezado) STORED,
    fechaInicioPresupuestoEncabezado   DATE          NOT NULL,
    fechaCierrePresupuestoEncabezado   DATE          NULL,
    is_active                          BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at                         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PresupuestoEncabezado PRIMARY KEY (idPresupuestoEncabezado),
    CONSTRAINT ckPresupuestoFechas CHECK (fechaCierrePresupuestoEncabezado IS NULL OR fechaCierrePresupuestoEncabezado >= fechaInicioPresupuestoEncabezado),
    CONSTRAINT ckPresupuestoMontos CHECK (totalPresupuestoEncabezado >= 0 AND gastadoPresupuestoEncabezado >= 0)
) ENGINE=InnoDB;

CREATE TABLE tblPresupuestoDetalle (
    idPresupuestoDetalle                 INT           NOT NULL AUTO_INCREMENT,
    idPresupuestoEncabezado              INT           NOT NULL,
    idModuloContable                     INT           NOT NULL,
    idPolizaDetalle                      INT           NULL,
    nombrePresupuestoDetalle             VARCHAR(100)  NOT NULL,
    cantidadPresupuestoDetalle           DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    is_active                            BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at                           TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                           TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PresupuestoDetalle PRIMARY KEY (idPresupuestoDetalle),
    CONSTRAINT ckPresupuestoDetalleCantidad CHECK (cantidadPresupuestoDetalle >= 0),
    CONSTRAINT Fk_PresupuestoDetalle_PresupuestoEncabezado FOREIGN KEY (idPresupuestoEncabezado) REFERENCES tblPresupuestoEncabezado (idPresupuestoEncabezado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_PresupuestoDetalle_Modulo FOREIGN KEY (idModuloContable) REFERENCES tblModuloContable (idModuloContable) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_PresupuestoDetalle_PolizaDetalle FOREIGN KEY (idPolizaDetalle) REFERENCES tblPolizaDetalle (idPolizaDetalle) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxPresupuestoDetalleEncabezado (idPresupuestoEncabezado),
    INDEX idxPresupuestoDetalleModulo     (idModuloContable),
    INDEX idxPresupuestoDetallePolizaDet  (idPolizaDetalle)
) ENGINE=InnoDB;

-- =============================================================================
--  5. ACTIVOS FIJOS
-- =============================================================================

CREATE TABLE tblMaestroVidaUtil (
    idMaestroVidaUtil                    INT          NOT NULL AUTO_INCREMENT,
    codigoMaestroVidaUtil                VARCHAR(30)  NOT NULL,
    nombreMaestroVidaUtil                VARCHAR(100) NOT NULL,
    cantidadMesesMaestroVidaUtil         INT          NOT NULL,
    tasaDepreciacionMaestroVidaUtil      DECIMAL(3,2) NOT NULL DEFAULT 0.20,
    is_active                            BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at                           TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                           TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_MaestroVidaUtil PRIMARY KEY (idMaestroVidaUtil),
    CONSTRAINT uqMaestroVidaUtilCodigo UNIQUE (codigoMaestroVidaUtil),
    CONSTRAINT ckMaestroVidaUtilMeses CHECK (cantidadMesesMaestroVidaUtil > 0)
) ENGINE=InnoDB;

CREATE TABLE tblActivoFijo (
    idActivoFijo                        INT           NOT NULL AUTO_INCREMENT,
    idModuloContable                    INT           NOT NULL,
    idMaestroVidaUtil                   INT           NOT NULL,
    codigoActivoFijo                    VARCHAR(30)   NOT NULL,
    descripcionActivoFijo               VARCHAR(255)  NOT NULL,
    codigoCentroCostoActivoFijo         VARCHAR(30)   NOT NULL,
    valorCompraActivoFijo               DECIMAL(12,4) NOT NULL,
    valorResidualActivoFijo             DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    valorDepreciableActivoFijo          DECIMAL(12,4) GENERATED ALWAYS AS (valorCompraActivoFijo - valorResidualActivoFijo) STORED,
    valorEnLibrosActivoFijo             DECIMAL(12,4) NOT NULL,
    mesesVidaUtilActivoFijo             INT           NOT NULL,
    fechaAdquisicionActivoFijo          DATE          NOT NULL,
    fechaInicioUsoActivoFijo            DATE          NOT NULL,
    is_active                           BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at                          TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                          TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ActivoFijo PRIMARY KEY (idActivoFijo),
    CONSTRAINT uqActivoFijoCodigo UNIQUE (codigoActivoFijo),
    CONSTRAINT ckActivoFijoResidual CHECK (valorResidualActivoFijo >= 0 AND valorResidualActivoFijo <= valorCompraActivoFijo),
    CONSTRAINT ckActivoFijoFechas   CHECK (fechaInicioUsoActivoFijo >= fechaAdquisicionActivoFijo),
    CONSTRAINT ckActivoFijoMeses    CHECK (mesesVidaUtilActivoFijo > 0),
    CONSTRAINT Fk_ActivoFijo_Modulo FOREIGN KEY (idModuloContable) REFERENCES tblModuloContable (idModuloContable) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_ActivoFijo_MaestroVidaUtil FOREIGN KEY (idMaestroVidaUtil) REFERENCES tblMaestroVidaUtil (idMaestroVidaUtil) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxActivoFijoModulo      (idModuloContable),
    INDEX idxActivoFijoMaestroVida (idMaestroVidaUtil)
) ENGINE=InnoDB;

-- =============================================================================
--  6. VISTAS DE VISUALIZACIÓN
-- =============================================================================

CREATE OR REPLACE VIEW vwLibroDiario AS
SELECT  pe.codigoPolizaEncabezado        AS codigoPoliza,
        pe.fechaPolizaEncabezado         AS fechaPoliza,
        pe.periodoContablePolizaEncabezado AS periodoContablePoliza,
        pe.estadoPolizaEncabezado        AS estadoPoliza,
        m.nombreModuloContable           AS moduloPoliza,
        pd.lineaPolizaDetalle            AS lineaDetalle,
        c.codigoCuenta                   AS codigoCuenta,
        c.nombreCuenta                   AS nombreCuenta,
        c.tipoCuenta                     AS tipoCuenta,
        pd.debePolizaDetalle             AS debeDetalle,
        pd.haberPolizaDetalle            AS haberDetalle,
        pd.descripcionPolizaDetalle      AS descripcionDetalle
FROM    tblPolizaDetalle    pd
JOIN    tblPolizaEncabezado pe ON pe.idPolizaEncabezado = pd.idPolizaEncabezado
JOIN    tblCuenta           c  ON c.idCuenta            = pd.idCuenta
JOIN    tblModuloContable   m  ON m.idModuloContable    = pe.idModuloContable
WHERE   pd.is_active = TRUE
  AND   pe.is_active = TRUE;

CREATE OR REPLACE VIEW vwPolizaCuadre AS
SELECT  pe.codigoPolizaEncabezado        AS codigoPoliza,
        pe.fechaPolizaEncabezado         AS fechaPoliza,
        pe.estadoPolizaEncabezado        AS estadoPoliza,
        pe.descripcionPolizaEncabezado   AS descripcionPoliza,
        COUNT(pd.idPolizaDetalle)        AS totalLineasPoliza,
        SUM(pd.debePolizaDetalle)        AS totalDebePoliza,
        SUM(pd.haberPolizaDetalle)       AS totalHaberPoliza,
        (SUM(pd.debePolizaDetalle) - SUM(pd.haberPolizaDetalle)) AS diferenciaPoliza,
        CASE WHEN SUM(pd.debePolizaDetalle) = SUM(pd.haberPolizaDetalle)
             THEN 'CUADRADA' ELSE 'DESCUADRADA' END AS resultadoCuadre
FROM    tblPolizaEncabezado pe
JOIN    tblPolizaDetalle    pd ON pd.idPolizaEncabezado = pe.idPolizaEncabezado
WHERE   pe.is_active = TRUE AND pd.is_active = TRUE
GROUP BY pe.idPolizaEncabezado, pe.codigoPolizaEncabezado, pe.fechaPolizaEncabezado,
         pe.estadoPolizaEncabezado, pe.descripcionPolizaEncabezado;

CREATE OR REPLACE VIEW vwPolizaEncabezadoGeneral AS
SELECT  pe.idPolizaEncabezado          AS idPoliza,
        pe.codigoPolizaEncabezado      AS codigoPoliza,
        pe.fechaPolizaEncabezado       AS fechaPoliza,
        pe.periodoContablePolizaEncabezado AS periodoContablePoliza,
        pe.estadoPolizaEncabezado      AS estadoPoliza,
        m.idModuloContable             AS idModulo,
        m.nombreModuloContable         AS moduloPoliza,
        pe.documentoOrigenPolizaEncabezado AS documentoOrigenPoliza,
        pe.descripcionPolizaEncabezado AS descripcionPoliza,
        COUNT(pd.idPolizaDetalle)      AS totalLineasPoliza,
        IFNULL(SUM(pd.debePolizaDetalle), 0)  AS totalDebePoliza,
        IFNULL(SUM(pd.haberPolizaDetalle), 0) AS totalHaberPoliza,
        pe.idActualizacionSaldos       AS idActualizacionSaldos,
        pe.created_at                  AS fechaRegistroPoliza
FROM    tblPolizaEncabezado pe
JOIN    tblModuloContable   m  ON m.idModuloContable = pe.idModuloContable
LEFT JOIN tblPolizaDetalle  pd ON pd.idPolizaEncabezado = pe.idPolizaEncabezado AND pd.is_active = TRUE
WHERE   pe.is_active = TRUE
GROUP BY pe.idPolizaEncabezado, pe.codigoPolizaEncabezado, pe.fechaPolizaEncabezado, pe.periodoContablePolizaEncabezado,
         pe.estadoPolizaEncabezado, m.idModuloContable, m.nombreModuloContable, pe.documentoOrigenPolizaEncabezado,
         pe.descripcionPolizaEncabezado, pe.idActualizacionSaldos, pe.created_at;

CREATE OR REPLACE VIEW vwBalanceCuenta AS
SELECT  c.codigoCuenta                   AS codigoCuenta,
        c.nombreCuenta                   AS nombreCuenta,
        c.tipoCuenta                     AS tipoCuenta,
        c.clasificacionCuenta            AS clasificacionCuenta,
        s.periodoContableSaldoCuentaPeriodo AS periodoContableSaldo,
        s.saldoInicialSaldoCuentaPeriodo AS saldoInicialCuenta,
        s.cargoSaldoCuentaPeriodo        AS totalDebeCuenta,
        s.abonoSaldoCuentaPeriodo        AS totalHaberCuenta,
        s.saldoFinalSaldoCuentaPeriodo   AS saldoCuenta,
        s.estadoSaldoCuentaPeriodo       AS estadoSaldo
FROM    tblSaldoCuentaPeriodo s
JOIN    tblCuenta             c ON c.idCuenta = s.idCuenta
WHERE   s.is_active = TRUE AND c.is_active = TRUE;

CREATE OR REPLACE VIEW vwPolizaTraslado AS
SELECT  m.nombreModuloContable           AS moduloOrigen,
        pe.documentoOrigenPolizaEncabezado AS documentoOrigen,
        pe.codigoPolizaEncabezado        AS codigoPoliza,
        pe.fechaPolizaEncabezado         AS fechaPoliza,
        pe.estadoPolizaEncabezado        AS estadoPoliza,
        SUM(pd.debePolizaDetalle)        AS montoTrasladado,
        pe.created_at                    AS fechaTraslado
FROM    tblPolizaEncabezado pe
JOIN    tblModuloContable   m  ON m.idModuloContable = pe.idModuloContable
JOIN    tblPolizaDetalle    pd ON pd.idPolizaEncabezado = pe.idPolizaEncabezado
WHERE   pe.documentoOrigenPolizaEncabezado IS NOT NULL
GROUP BY pe.idPolizaEncabezado, m.nombreModuloContable, pe.documentoOrigenPolizaEncabezado,
         pe.codigoPolizaEncabezado, pe.fechaPolizaEncabezado, pe.estadoPolizaEncabezado, pe.created_at;

-- =============================================================================
--  7. AJUSTES SOBRE TABLAS EXISTENTES
--  Para bases que ya tienen las tablas creadas: ejecutar solo esta sección.
-- =============================================================================

-- Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
ALTER TABLE tblModuloContable
    ADD COLUMN metodoActualizacionModuloContable VARCHAR(30) NOT NULL DEFAULT 'BATCH' AFTER codigoModuloContable,
    ADD CONSTRAINT ckModuloMetodoActualizacion CHECK (metodoActualizacionModuloContable IN ('LINEA','BATCH'));

ALTER TABLE tblCuenta
    RENAME COLUMN cargoActualCuenta TO cargoAcumuladoCuenta,
    RENAME COLUMN abonoActualCuenta TO abonoAcumuladoCuenta;

ALTER TABLE tblActivoFijo
    DROP COLUMN codigoCentroCostoActivoFijo;
-- Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026

-- Inicio de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026
ALTER TABLE tblPais
    ADD COLUMN codigoMonedaPais CHAR(3) NOT NULL DEFAULT 'GTQ' AFTER nombrePais;

UPDATE tblPais SET codigoMonedaPais = CASE nombrePais
    WHEN 'El Salvador' THEN 'USD'
    WHEN 'Honduras'    THEN 'HNL'
    ELSE 'GTQ' END
WHERE idPais > 0;

ALTER TABLE tblPolizaEncabezado
    ADD COLUMN idPais INT NOT NULL DEFAULT 1 AFTER idModuloContable,
    ADD CONSTRAINT Fk_PolizaEncabezado_Pais FOREIGN KEY (idPais) REFERENCES tblPais (idPais) ON DELETE RESTRICT ON UPDATE CASCADE;
-- Fin de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026

-- Inicio de cambio: Dylan Hernández - 0901-23-519 - 30/10/2026
CREATE TABLE IF NOT EXISTS tblImpuesto (
    idImpuesto                    INT          NOT NULL AUTO_INCREMENT,
    nombreImpuesto                VARCHAR(100) NOT NULL,
    descripcionImpuesto           VARCHAR(255) NULL,
    tipoImpuesto                  VARCHAR(30)  NOT NULL,
    operacionImpuesto             VARCHAR(30)  NOT NULL DEFAULT 'AMBAS',
    valorImpuesto                 DECIMAL(5,4) NOT NULL,
    periodicidadImpuesto          VARCHAR(30)  NOT NULL,
    idCuentaAcreditableImpuesto   INT          NULL,
    idCuentaPorPagarImpuesto      INT          NULL,
    is_active                     BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at                    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Impuesto PRIMARY KEY (idImpuesto),
    CONSTRAINT ckImpuestoTipo CHECK (tipoImpuesto IN ('CONSUMO','RENTA','PATRIMONIO','CONTRIBUCION','OTRO')),
    CONSTRAINT ckImpuestoOperacion CHECK (operacionImpuesto IN ('COMPRA','VENTA','AMBAS','PLANILLA','PERIODO')),
    CONSTRAINT ckImpuestoPeriodicidad CHECK (periodicidadImpuesto IN ('MENSUAL','TRIMESTRAL','ANUAL','POR_EVENTO')),
    CONSTRAINT Fk_Impuesto_CuentaAcreditable FOREIGN KEY (idCuentaAcreditableImpuesto) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_Impuesto_CuentaPorPagar FOREIGN KEY (idCuentaPorPagarImpuesto) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxImpuestoTipo (tipoImpuesto),
    INDEX idxImpuestoOperacion (operacionImpuesto)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblImpuestoTramo (
    idImpuestoTramo              INT           NOT NULL AUTO_INCREMENT,
    idImpuesto                   INT           NOT NULL,
    montoDesdeImpuestoTramo      DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    montoHastaImpuestoTramo      DECIMAL(12,4) NULL,
    tasaImpuestoTramo            DECIMAL(5,4)  NOT NULL,
    importeFijoImpuestoTramo     DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    is_active                    BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at                   TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                   TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ImpuestoTramo PRIMARY KEY (idImpuestoTramo),
    CONSTRAINT uqImpuestoTramo UNIQUE (idImpuesto, montoDesdeImpuestoTramo),
    CONSTRAINT ckImpuestoTramoRango CHECK (montoHastaImpuestoTramo IS NULL OR montoHastaImpuestoTramo > montoDesdeImpuestoTramo),
    CONSTRAINT Fk_ImpuestoTramo_Impuesto FOREIGN KEY (idImpuesto) REFERENCES tblImpuesto (idImpuesto) ON DELETE CASCADE ON UPDATE CASCADE,
    INDEX idxImpuestoTramoImpuesto (idImpuesto)
) ENGINE=InnoDB;
-- Fin de cambio: Dylan Hernández - 0901-23-519 - 30/10/2026