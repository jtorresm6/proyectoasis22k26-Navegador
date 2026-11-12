-- Ejecutar en la misma base de datos que contiene tblDisponibilidadDiaria.
-- Actualizacion aditiva e idempotente: no cambia importes, tasas, fuentes,
-- fechas de corte ni fechas de generacion de los registros existentes.
-- La version inicial de los cortes existentes es 1. Su usuario anterior se
-- conserva desconocido (NULL); no se fabrica un historial retroactivo.

SET @disponibilidad_ddl = IF(
    EXISTS(SELECT 1 FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE()
        AND TABLE_NAME = 'tblDisponibilidadDiaria' AND COLUMN_NAME = 'versionDisponibilidadDiaria'),
    'SELECT 1',
    'ALTER TABLE tblDisponibilidadDiaria ADD COLUMN versionDisponibilidadDiaria BIGINT NOT NULL DEFAULT 1');
PREPARE disponibilidad_migracion FROM @disponibilidad_ddl;
EXECUTE disponibilidad_migracion;
DEALLOCATE PREPARE disponibilidad_migracion;

SET @disponibilidad_ddl = IF(
    EXISTS(SELECT 1 FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE()
        AND TABLE_NAME = 'tblDisponibilidadDiaria' AND COLUMN_NAME = 'usuarioRegistroIdDisponibilidadDiaria'),
    'SELECT 1',
    'ALTER TABLE tblDisponibilidadDiaria ADD COLUMN usuarioRegistroIdDisponibilidadDiaria INT NULL');
PREPARE disponibilidad_migracion FROM @disponibilidad_ddl;
EXECUTE disponibilidad_migracion;
DEALLOCATE PREPARE disponibilidad_migracion;

SET @disponibilidad_ddl = IF(
    EXISTS(SELECT 1 FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE()
        AND TABLE_NAME = 'tblDisponibilidadDiaria' AND COLUMN_NAME = 'usuarioRegistroDisponibilidadDiaria'),
    'SELECT 1',
    'ALTER TABLE tblDisponibilidadDiaria ADD COLUMN usuarioRegistroDisponibilidadDiaria VARCHAR(100) NULL');
PREPARE disponibilidad_migracion FROM @disponibilidad_ddl;
EXECUTE disponibilidad_migracion;
DEALLOCATE PREPARE disponibilidad_migracion;

SET @disponibilidad_ddl = IF(
    EXISTS(SELECT 1 FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE()
        AND TABLE_NAME = 'tblDisponibilidadDiaria' AND COLUMN_NAME = 'fechaActualizacionDisponibilidadDiaria'),
    'SELECT 1',
    'ALTER TABLE tblDisponibilidadDiaria ADD COLUMN fechaActualizacionDisponibilidadDiaria DATETIME(6) NULL');
PREPARE disponibilidad_migracion FROM @disponibilidad_ddl;
EXECUTE disponibilidad_migracion;
DEALLOCATE PREPARE disponibilidad_migracion;

CREATE TABLE IF NOT EXISTS tblDisponibilidadDiariaHistorial (
    Pk_DisponibilidadDiariaHistorial BIGINT NOT NULL AUTO_INCREMENT,
    Fk_DisponibilidadDiaria BIGINT NOT NULL,
    Fk_CuentaBancaria BIGINT NOT NULL,
    fechaCorte DATE NOT NULL,
    accion VARCHAR(12) NOT NULL,
    versionAnterior BIGINT NULL,
    versionNueva BIGINT NOT NULL,
    usuarioId INT NULL,
    usuarioNombre VARCHAR(100) NULL,
    fechaOperacion DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    monedaAnterior CHAR(3) NULL,
    monedaNueva CHAR(3) NOT NULL,
    saldoInicialAnterior DECIMAL(12,4) NULL,
    ingresosAnterior DECIMAL(12,4) NULL,
    egresosAnterior DECIMAL(12,4) NULL,
    comprometidoAnterior DECIMAL(12,4) NULL,
    restringidoAnterior DECIMAL(12,4) NULL,
    tasaAnterior DECIMAL(12,4) NULL,
    fuenteAnterior VARCHAR(100) NULL,
    saldoInicialNuevo DECIMAL(12,4) NOT NULL,
    ingresosNuevo DECIMAL(12,4) NOT NULL,
    egresosNuevo DECIMAL(12,4) NOT NULL,
    comprometidoNuevo DECIMAL(12,4) NOT NULL,
    restringidoNuevo DECIMAL(12,4) NOT NULL,
    tasaNueva DECIMAL(12,4) NOT NULL,
    fuenteNueva VARCHAR(100) NOT NULL,
    PRIMARY KEY (Pk_DisponibilidadDiariaHistorial),
    CONSTRAINT uqDisponibilidadHistorialVersion UNIQUE (Fk_DisponibilidadDiaria, versionNueva),
    KEY ixDisponibilidadHistorialCuentaFecha (Fk_CuentaBancaria, fechaCorte, fechaOperacion),
    CONSTRAINT fkDisponibilidadHistorialCorte FOREIGN KEY (Fk_DisponibilidadDiaria)
        REFERENCES tblDisponibilidadDiaria(Pk_DisponibilidadDiaria) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;
