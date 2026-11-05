
-- CIERRE DE INVENTARIOS NILLS BERDUCIDO GOMEZ

CREATE TABLE tblCierreMovimiento (
    idCierreMovimiento INT AUTO_INCREMENT PRIMARY KEY,
    nombreCierreMovimiento VARCHAR(100) NOT NULL,
    fechaInicioCierreMovimiento DATE NOT NULL,
    fechaFinCierreMovimiento DATE NOT NULL,
    estadoCierreMovimiento VARCHAR(20) NOT NULL DEFAULT 'ABIERTO',
    esCierreAnualCierreMovimiento BOOLEAN NOT NULL DEFAULT FALSE,
    toleranciaCierreMovimiento INT NOT NULL DEFAULT 0,
    diferenciaUnidadesCierreMovimiento INT NOT NULL DEFAULT 0,
    diferenciaMontoCierreMovimiento DECIMAL(12,2) NOT NULL DEFAULT 0,
    motivoAceptacionCierreMovimiento VARCHAR(500) NULL,
    motivoAnulacionCierreMovimiento VARCHAR(500) NULL,
    observacionesCierreMovimiento VARCHAR(500) NULL,
  );

-- Tabla detalle

CREATE TABLE tblDetalleCierre (
    idDetalleCierre INT AUTO_INCREMENT PRIMARY KEY,
    fkIdCierreMovimiento INT NOT NULL,
    fkSucursalIdSucursal INT NOT NULL,
    fkBodegaIdBodega INT NOT NULL,
    fkProductoIdProducto INT NOT NULL,

);