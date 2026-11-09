-- ============================================================
-- Submódulo: Traslado entre bodegas (Módulo de Logística)
-- Depende de tablas del submódulo de Inventarios y Transporte:
--   tblbodega (idBodega), tbltransporte (idTransporte), tblproducto (idProducto)
-- ============================================================

CREATE TABLE tbltraslado (
    idTraslado INT NOT NULL AUTO_INCREMENT,
    codigoTraslado VARCHAR(30) NOT NULL,
    fkBodegaOrigenIdBodega INT NOT NULL,
    fkBodegaDestinoIdBodega INT NOT NULL,
    fkTransporteIdTransporte INT NULL,
    fechaSolicitudTraslado DATETIME NOT NULL,
    fechaSalidaTraslado DATETIME NULL,
    fechaLlegadaTraslado DATETIME NULL,
    estadoTraslado ENUM('SOLICITADO','EN_TRANSITO','RECIBIDO','CANCELADO') NOT NULL DEFAULT 'SOLICITADO',
    observacionesTraslado VARCHAR(500) NULL,
    isActive TINYINT(1) NOT NULL DEFAULT 1,
    createdAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    PRIMARY KEY (idTraslado),
    UNIQUE KEY uqCodigoTraslado (codigoTraslado),
    CONSTRAINT fkTrasladoBodegaOrigen FOREIGN KEY (fkBodegaOrigenIdBodega) REFERENCES tblbodega (idBodega),
    CONSTRAINT fkTrasladoBodegaDestino FOREIGN KEY (fkBodegaDestinoIdBodega) REFERENCES tblbodega (idBodega),
    CONSTRAINT fkTrasladoTransporte FOREIGN KEY (fkTransporteIdTransporte) REFERENCES tbltransporte (idTransporte),
    CONSTRAINT chkTrasladoBodegasDistintas CHECK (fkBodegaOrigenIdBodega <> fkBodegaDestinoIdBodega) -- Origen y destino no pueden ser la misma bodega
);

CREATE TABLE tbldetalletraslado (
    fkTrasladoIdTraslado INT NOT NULL,
    fkProductoIdProducto INT NOT NULL,
    cantidadDetalleTraslado INT NOT NULL,
    isActive TINYINT(1) NOT NULL DEFAULT 1,
    createdAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    PRIMARY KEY (fkTrasladoIdTraslado, fkProductoIdProducto),
    CONSTRAINT fkDetalleTrasladoTraslado FOREIGN KEY (fkTrasladoIdTraslado) REFERENCES tbltraslado (idTraslado),
    CONSTRAINT fkDetalleTrasladoProducto FOREIGN KEY (fkProductoIdProducto) REFERENCES tblproducto (idProducto),
    CONSTRAINT chkDetalleTrasladoCantidad CHECK (cantidadDetalleTraslado > 0)
);