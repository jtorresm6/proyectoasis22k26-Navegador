/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 11/11/2026
 * Versión : 1.1
 * ==================================================================
 * Propósito :
 *  Tablas del 3.8 Cierre de Movimientos de Compras y Cuentas por Pagar.
 *  Se ejecuta después de dbSistemaEmbutidos_General__v1.sql (usa tblProveedor,
 *  tblMoneda con idMoneda y tblUsuario). Es un script aparte mientras se integra
 *  al script general para el 100 %. Se puede ejecutar varias veces sin duplicar.
 *
 *  Conforme a la minuta MIN-CAT-003 (ACU-010) no hay procedimientos almacenados
 *  ni triggers: los cálculos, las validaciones, la transacción del cierre y el
 *  bloqueo de documentos están en la capa Modelo de CierreMovimientos.
 *
 *  tblPeriodoCompras          Períodos mensuales y su estado (Abierto / Cerrado).
 *  tblSaldoProveedorPeriodo   Foto del saldo de cada proveedor y moneda al cerrar.
 *  tblRangoAntiguedad         Rangos de días vencidos (configurables).
 *  tblAntiguedadSaldo         Antigüedad de saldos guardada al cerrar.
 *
 *  Cambios respecto a la versión 1.0:
 *   - Se valida contra el script general unificado (idMoneda, is_active).
 * ===================================================================
*/

USE `dbsistemaembutidos`;

CREATE TABLE IF NOT EXISTS tblPeriodoCompras (
    idPeriodoCompras                             INT            NOT NULL AUTO_INCREMENT,
    anioPeriodoCompras                           SMALLINT       NOT NULL,
    mesPeriodoCompras                            TINYINT        NOT NULL,
    fechaInicioPeriodoCompras                    DATE           NOT NULL,
    fechaFinPeriodoCompras                       DATE           NOT NULL,
    estadoPeriodoCompras                         VARCHAR(20)    NOT NULL DEFAULT 'Abierto',
    totalCargosPeriodoCompras                    DECIMAL(12,4)  NOT NULL DEFAULT 0,
    totalAbonosPeriodoCompras                    DECIMAL(12,4)  NOT NULL DEFAULT 0,
    fechaCierrePeriodoCompras                    DATETIME       NULL,
    idUsuarioCierrePeriodoCompras                INT            NULL,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_PeriodoCompras PRIMARY KEY (idPeriodoCompras),
    CONSTRAINT uqAnioMesPeriodoCompras UNIQUE (anioPeriodoCompras, mesPeriodoCompras),
    INDEX idxFechasPeriodoCompras (fechaInicioPeriodoCompras, fechaFinPeriodoCompras),
    INDEX idxPeriodoComprasUsuario (idUsuarioCierrePeriodoCompras),
    CONSTRAINT Fk_PeriodoCompras_Usuario FOREIGN KEY (idUsuarioCierrePeriodoCompras) REFERENCES tblUsuario (idUsuario) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT ckMesPeriodoCompras CHECK (mesPeriodoCompras BETWEEN 1 AND 12),
    CONSTRAINT ckFechasPeriodoCompras CHECK (fechaFinPeriodoCompras >= fechaInicioPeriodoCompras),
    CONSTRAINT ckEstadoPeriodoCompras CHECK (estadoPeriodoCompras IN ('Abierto','Cerrado')),
    CONSTRAINT ckTotalesPeriodoCompras CHECK (totalCargosPeriodoCompras >= 0 AND totalAbonosPeriodoCompras >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS tblSaldoProveedorPeriodo (
    idSaldoProveedorPeriodo                      INT            NOT NULL AUTO_INCREMENT,
    idPeriodoCompras                             INT            NOT NULL,
    idProveedor                                  INT            NOT NULL,
    idMoneda                                     BIGINT         NOT NULL,
    saldoInicialSaldoProveedorPeriodo            DECIMAL(12,4)  NOT NULL DEFAULT 0,
    cargosSaldoProveedorPeriodo                  DECIMAL(12,4)  NOT NULL DEFAULT 0,
    abonosSaldoProveedorPeriodo                  DECIMAL(12,4)  NOT NULL DEFAULT 0,
    saldoFinalSaldoProveedorPeriodo              DECIMAL(12,4)  NOT NULL DEFAULT 0,
    tipoCambioSaldoProveedorPeriodo              DECIMAL(12,6)  NOT NULL DEFAULT 1,
    saldoFinalLocalSaldoProveedorPeriodo         DECIMAL(12,4)  NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_SaldoProveedorPeriodo PRIMARY KEY (idSaldoProveedorPeriodo),
    CONSTRAINT uqPeriodoProveedorMonedaSaldo UNIQUE (idPeriodoCompras, idProveedor, idMoneda),
    INDEX idxSaldoProveedorPeriodoProveedor (idProveedor),
    INDEX idxSaldoProveedorPeriodoMoneda (idMoneda),
    CONSTRAINT Fk_SaldoProveedorPeriodo_Periodo FOREIGN KEY (idPeriodoCompras) REFERENCES tblPeriodoCompras (idPeriodoCompras) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_SaldoProveedorPeriodo_Proveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor (idProveedor) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_SaldoProveedorPeriodo_Moneda FOREIGN KEY (idMoneda) REFERENCES tblMoneda (idMoneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT ckMovimientosSaldoProveedorPeriodo CHECK (cargosSaldoProveedorPeriodo >= 0 AND abonosSaldoProveedorPeriodo >= 0),
    CONSTRAINT ckTipoCambioSaldoProveedorPeriodo CHECK (tipoCambioSaldoProveedorPeriodo > 0),
    CONSTRAINT ckCuadreSaldoProveedorPeriodo CHECK (saldoFinalSaldoProveedorPeriodo =
        saldoInicialSaldoProveedorPeriodo + cargosSaldoProveedorPeriodo - abonosSaldoProveedorPeriodo)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Un límite NULL significa "sin límite". Días vencidos = fecha de cierre - fecha de vencimiento
-- (0 o menos: el documento todavía no vence).
CREATE TABLE IF NOT EXISTS tblRangoAntiguedad (
    idRangoAntiguedad                            INT            NOT NULL AUTO_INCREMENT,
    nombreRangoAntiguedad                        VARCHAR(40)    NOT NULL,
    diasDesdeRangoAntiguedad                     INT            NULL,
    diasHastaRangoAntiguedad                     INT            NULL,
    ordenRangoAntiguedad                         TINYINT        NOT NULL,
    activoRangoAntiguedad                        BOOLEAN        NOT NULL DEFAULT TRUE,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_RangoAntiguedad PRIMARY KEY (idRangoAntiguedad),
    CONSTRAINT uqNombreRangoAntiguedad UNIQUE (nombreRangoAntiguedad),
    CONSTRAINT uqOrdenRangoAntiguedad UNIQUE (ordenRangoAntiguedad),
    CONSTRAINT ckLimitesRangoAntiguedad CHECK (diasDesdeRangoAntiguedad IS NULL
        OR diasHastaRangoAntiguedad IS NULL
        OR diasHastaRangoAntiguedad >= diasDesdeRangoAntiguedad)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS tblAntiguedadSaldo (
    idAntiguedadSaldo                            INT            NOT NULL AUTO_INCREMENT,
    idPeriodoCompras                             INT            NOT NULL,
    idProveedor                                  INT            NOT NULL,
    idMoneda                                     BIGINT         NOT NULL,
    idRangoAntiguedad                            INT            NOT NULL,
    montoAntiguedadSaldo                         DECIMAL(12,4)  NOT NULL,
    montoLocalAntiguedadSaldo                    DECIMAL(12,4)  NOT NULL,
    cantidadDocumentosAntiguedadSaldo            INT            NOT NULL,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_AntiguedadSaldo PRIMARY KEY (idAntiguedadSaldo),
    CONSTRAINT uqPeriodoProveedorMonedaRango UNIQUE (idPeriodoCompras, idProveedor, idMoneda, idRangoAntiguedad),
    INDEX idxAntiguedadSaldoProveedor (idProveedor),
    INDEX idxAntiguedadSaldoMoneda (idMoneda),
    INDEX idxAntiguedadSaldoRango (idRangoAntiguedad),
    CONSTRAINT Fk_AntiguedadSaldo_Periodo FOREIGN KEY (idPeriodoCompras) REFERENCES tblPeriodoCompras (idPeriodoCompras) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_AntiguedadSaldo_Proveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor (idProveedor) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_AntiguedadSaldo_Moneda FOREIGN KEY (idMoneda) REFERENCES tblMoneda (idMoneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_AntiguedadSaldo_Rango FOREIGN KEY (idRangoAntiguedad) REFERENCES tblRangoAntiguedad (idRangoAntiguedad) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT ckMontoAntiguedadSaldo CHECK (montoAntiguedadSaldo > 0 AND montoLocalAntiguedadSaldo > 0),
    CONSTRAINT ckCantidadAntiguedadSaldo CHECK (cantidadDocumentosAntiguedadSaldo > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ------------------------------------------------------------------
-- Datos iniciales
-- ------------------------------------------------------------------

INSERT INTO tblRangoAntiguedad (nombreRangoAntiguedad, diasDesdeRangoAntiguedad, diasHastaRangoAntiguedad, ordenRangoAntiguedad)
SELECT r.nombre, r.desde, r.hasta, r.orden
FROM (
    SELECT 'Por vencer' AS nombre, NULL AS desde, 0 AS hasta, 1 AS orden UNION ALL
    SELECT '1 a 30 días', 1, 30, 2 UNION ALL
    SELECT '31 a 60 días', 31, 60, 3 UNION ALL
    SELECT '61 a 90 días', 61, 90, 4 UNION ALL
    SELECT 'Más de 90 días', 91, NULL, 5
) AS r
WHERE NOT EXISTS (SELECT 1 FROM tblRangoAntiguedad x WHERE x.nombreRangoAntiguedad = r.nombre);

-- Primer período de Compras. Cambiar al mes en que el módulo empiece a operar:
-- los documentos anteriores forman su saldo inicial y los períodos siguientes
-- los abre el cierre automáticamente.
INSERT INTO tblPeriodoCompras (anioPeriodoCompras, mesPeriodoCompras, fechaInicioPeriodoCompras, fechaFinPeriodoCompras)
SELECT 2026, 10, '2026-10-01', '2026-10-31'
WHERE NOT EXISTS (SELECT 1 FROM tblPeriodoCompras);
