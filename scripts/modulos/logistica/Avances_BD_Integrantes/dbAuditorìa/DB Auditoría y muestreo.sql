
-- ============================================================================
-- Carlos Eduardo Chin Caal
-- APARTADO: MUESTREO Y AUDITORIA
-- TABLA: tblAuditoriaInventario
-- ============================================================================

CREATE TABLE tblAuditoriaInventario (
    Pk_Auditoria_Inventario INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Fk_Movimiento_Inventario_Codigo INT NULL COMMENT 'Opcional: una auditoria puede realizarse sin movimiento previo',
    Fk_Bodega_Codigo INT NOT NULL,
    tipoAuditoriaInventario ENUM('INDIVIDUAL', 'MULTIPLE') NOT NULL,
    estadoAuditoriaInventario ENUM('EN_PROCESO', 'FINALIZADA') NOT NULL DEFAULT 'EN_PROCESO',
    fechaAuditoriaInventario DATETIME NOT NULL,
    resultadoAuditoriaInventario VARCHAR(100) NOT NULL,
    observacionesCalidadAuditoriaInventario TEXT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Fk_Auditoria_Inventario_Movimiento_Inventario
    FOREIGN KEY (Fk_Movimiento_Inventario_Codigo)
    REFERENCES tblMovimientoInventario(Pk_Movimiento_Inventario)
    ON DELETE RESTRICT,

    CONSTRAINT Fk_Auditoria_Inventario_Bodega
    FOREIGN KEY (Fk_Bodega_Codigo)
    REFERENCES tblBodega(Pk_Bodega)
    ON DELETE RESTRICT
) ENGINE=InnoDB;


-- ============================================================================
-- Carlos Eduardo Chin Caal
-- APARTADO: MUESTREO Y AUDITORIA
-- TABLA: tblDetalleAuditoria
-- ============================================================================

CREATE TABLE tblDetalleAuditoria (
    Pk_Detalle_Auditoria INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Fk_Auditoria_Inventario_Codigo INT NOT NULL,
    Fk_Stock_Codigo INT NOT NULL,
    stockTeoricoDetalleAuditoria INT NOT NULL,
    stockFisicoDetalleAuditoria INT NOT NULL,
    diferenciaDetalleAuditoria INT GENERATED ALWAYS AS
        (stockFisicoDetalleAuditoria - stockTeoricoDetalleAuditoria) STORED NOT NULL,
    observacionesDetalleAuditoria VARCHAR(500) NOT NULL DEFAULT '',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT uqDetalleAuditoriaStock
    UNIQUE (Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo),

    CONSTRAINT Fk_Detalle_Auditoria_Auditoria_Inventario
    FOREIGN KEY (Fk_Auditoria_Inventario_Codigo)
    REFERENCES tblAuditoriaInventario(Pk_Auditoria_Inventario)
    ON DELETE RESTRICT,

    CONSTRAINT Fk_Detalle_Auditoria_Stock
    FOREIGN KEY (Fk_Stock_Codigo)
    REFERENCES tblStock(Pk_Stock)
    ON DELETE RESTRICT,

    CONSTRAINT ckDetalleAuditoriaStockTeorico
    CHECK (stockTeoricoDetalleAuditoria >= 0),

    CONSTRAINT ckDetalleAuditoriaStockFisico
    CHECK (stockFisicoDetalleAuditoria >= 0)
) ENGINE=InnoDB;
