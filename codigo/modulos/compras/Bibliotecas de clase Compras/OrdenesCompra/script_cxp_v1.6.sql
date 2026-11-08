CREATE DATABASE IF NOT EXISTS `dbsistemaembutidos` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `dbsistemaembutidos`;

-- ==========================================================================
-- INICIO DE DEPENDENCIAS
-- ==========================================================================
-- RRHH: tablas compartidas requeridas por Compras; conservar las existentes.

CREATE TABLE IF NOT EXISTS tblMetodoPago (
    idMetodoPago INT AUTO_INCREMENT NOT NULL,
    nombreMetodoPago VARCHAR(100) NOT NULL,
    descripcionMetodoPago VARCHAR(255) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_MetodoPago PRIMARY KEY (idMetodoPago),
    CONSTRAINT uqNombreMetodoPago UNIQUE (nombreMetodoPago)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 

-- BANCOS: tablas compartidas requeridas por Compras; conservar las existentes.

CREATE TABLE IF NOT EXISTS tblMoneda (
    Pk_Moneda BIGINT NOT NULL AUTO_INCREMENT,
    codigoMoneda CHAR(3) NOT NULL,
    nombreMoneda VARCHAR(255) NOT NULL,
    simboloMoneda VARCHAR(8) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Moneda),
    CONSTRAINT uqMoneda1 UNIQUE (codigoMoneda),
    CONSTRAINT ckMoneda1 CHECK (is_active IN (0,1))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblTipoCambio (
    Pk_TipoCambio BIGINT NOT NULL AUTO_INCREMENT,
    fechaTipoCambio DATE NOT NULL,
    valorTipoCambio DECIMAL(12,4) NOT NULL,
    fuenteTipoCambio VARCHAR(100) NOT NULL,
    versionTipoCambio INT NOT NULL DEFAULT 1,
    cotizacionOriginalTipoCambio TEXT NULL,
    metodoTipoCambio VARCHAR(30) NOT NULL DEFAULT 'DIRECTA',
    motivoRevisionTipoCambio VARCHAR(255) NULL,
    Fk_Moneda_Origen_Codigo BIGINT NOT NULL,
    Fk_Moneda_Destino_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_TipoCambio),
    CONSTRAINT uqTipoCambio1 UNIQUE (Fk_Moneda_Origen_Codigo,Fk_Moneda_Destino_Codigo,fechaTipoCambio,fuenteTipoCambio,versionTipoCambio),
    CONSTRAINT ckTipoCambio1 CHECK (valorTipoCambio > 0),
    CONSTRAINT ckTipoCambio2 CHECK (versionTipoCambio > 0),
    CONSTRAINT ckTipoCambio3 CHECK (metodoTipoCambio IN ('DIRECTA','INVERSA','CRUZADA')),
    CONSTRAINT ckTipoCambio4 CHECK (Fk_Moneda_Origen_Codigo <> Fk_Moneda_Destino_Codigo),
    INDEX idxTipoCambioMonedaDestino (Fk_Moneda_Destino_Codigo),
    CONSTRAINT Fk_TipoCambio_Moneda_Origen FOREIGN KEY (Fk_Moneda_Origen_Codigo) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_TipoCambio_Moneda_Destino FOREIGN KEY (Fk_Moneda_Destino_Codigo) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

-- LOGISTICA: tablas compartidas requeridas por Compras; conservar las existentes.

CREATE TABLE IF NOT EXISTS tblSucursal (
idSucursal INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreSucursal VARCHAR(100) NOT NULL,
    descripcionSucursal VARCHAR(255) NOT NULL,
    ubicacionSucursal VARCHAR(500) NOT NULL,
    paisSucursal VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblBodega (
idBodega INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreBodega VARCHAR(100) NOT NULL,
    Fk_Sucursal_Codigo INT NOT NULL,
    paisBodega VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY uqBodegaSucursal (idBodega,Fk_Sucursal_Codigo),
    CONSTRAINT Fk_Bodega_Sucursal FOREIGN KEY (Fk_Sucursal_Codigo) REFERENCES tblSucursal(idSucursal) ON DELETE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblCategoria (
idCategoria INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreCategoria VARCHAR(100) NOT NULL,
    porcentajeComisionCategoria DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqCategoriaNombre UNIQUE (nombreCategoria),
    CONSTRAINT ckPorcentajeComisionCategoriaRango CHECK (porcentajeComisionCategoria >= 0.00 AND porcentajeComisionCategoria <= 0.99)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblLinea (
idLinea INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreLinea VARCHAR(100) NOT NULL,
    porcentajeComisionLinea DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqLineaNombre UNIQUE (nombreLinea),
    CONSTRAINT ckPorcentajeComisionLineaRango CHECK (porcentajeComisionLinea >= 0.00 AND porcentajeComisionLinea <= 0.99)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblMarca (
idMarca INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreMarca VARCHAR(100) NOT NULL,
    porcentajeComisionMarca DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqMarcaNombre UNIQUE (nombreMarca),
    CONSTRAINT ckPorcentajeComisionMarcaRango CHECK (porcentajeComisionMarca >= 0.00 AND porcentajeComisionMarca <= 0.99)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblProducto (
idProducto INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    skuProducto VARCHAR(30) NOT NULL,
    clasificacionProducto CHAR(5) NOT NULL DEFAULT '?????',
    nombreProducto VARCHAR(100) NOT NULL,
    idProveedorProductos INT NOT NULL,
    Fk_Marca_Codigo INT NOT NULL,
    Fk_Categoria_Codigo INT NOT NULL,
    Fk_Linea_Codigo INT NOT NULL,
    descripcionProducto VARCHAR(255) NOT NULL,
    imagenProducto LONGBLOB NOT NULL, 
    paisProducto VARCHAR(100) NOT NULL,
    precioCostoProducto DECIMAL(12,4) NOT NULL,
    precioVentaProducto DECIMAL(12,4) NOT NULL,
    porcentajeComisionProducto DECIMAL(3,2) NOT NULL,
    stockMinimoProducto INT NOT NULL ,
    stockMaximoProducto INT NOT NULL ,
    pesoProducto DECIMAL(4,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqProductoSku UNIQUE (skuProducto),
    INDEX idxProductoProveedorProductos (idProveedorProductos),
    CONSTRAINT Fk_Producto_Marca FOREIGN KEY (Fk_Marca_Codigo) REFERENCES tblMarca(idMarca) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Categoria FOREIGN KEY (Fk_Categoria_Codigo) REFERENCES tblCategoria(idCategoria) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Linea FOREIGN KEY (Fk_Linea_Codigo) REFERENCES tblLinea(idLinea) ON DELETE RESTRICT,
    CONSTRAINT ckPorcentajeComisionProductoRango CHECK (porcentajeComisionProducto >= 0.00 AND porcentajeComisionProducto <= 0.99),
    CONSTRAINT ckProductoMinMax CHECK (stockMinimoProducto >= 0 AND stockMaximoProducto >= stockMinimoProducto)
) ENGINE=InnoDB;

-- ==========================================================================
-- FIN DE DEPENDENCIAS
-- ==========================================================================

CREATE TABLE IF NOT EXISTS tblPlazo(
    idPlazo                                      INT AUTO_INCREMENT,
    nombrePlazo                                  VARCHAR(100) NOT NULL,
    diasPlazo                                    INT NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT Pk_Plazos PRIMARY KEY (idPlazo),
    CONSTRAINT uqnombrePlazo UNIQUE (nombrePlazo),
    CONSTRAINT ckDiasPlazo CHECK (diasPlazo >= 0)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblConceptoProveedor (
    idConceptoProveedor                          INT          NOT NULL AUTO_INCREMENT,
    nombreConceptoProveedor                      VARCHAR(100) NOT NULL,
    naturalezaConceptoProveedor                  VARCHAR(10)  NOT NULL,
    created_at                                   TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_ConceptoProveedor PRIMARY KEY (idConceptoProveedor),
    CONSTRAINT uqConceptoProveedorNombre UNIQUE (nombreConceptoProveedor),
    CONSTRAINT ckNaturalezaConceptoProveedor CHECK (naturalezaConceptoProveedor IN ('Cargo','Abono'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblProveedor(
    idProveedor                                  INT AUTO_INCREMENT,
    nombreProveedor                              VARCHAR(100) NOT NULL,
    nitProveedor                                 CHAR(10) NOT NULL,
    correoProveedor                              VARCHAR(150) NOT NULL,
    telefonoProveedor                            VARCHAR(30) NOT NULL,
    direccionProveedor                           VARCHAR(500) NOT NULL,
    estadoProveedor                              BOOLEAN NOT NULL DEFAULT 1,
    idPlazo                                      INT NULL,
    saldoActualProveedor                         DECIMAL(12,4) NOT NULL DEFAULT 0,
    saldoAnteriorProveedor                       DECIMAL(12,4) NOT NULL DEFAULT 0,
    cargosMesProveedor                           DECIMAL(12,4) NOT NULL DEFAULT 0,
    abonosMesProveedor                           DECIMAL(12,4) NOT NULL DEFAULT 0,
    cargosAsignadosProveedor                     DECIMAL(12,4) NOT NULL DEFAULT 0,
    cargosAcumuladosProveedor                    DECIMAL(12,4) NOT NULL DEFAULT 0,
    abonosAcumuladosProveedor                    DECIMAL(12,4) NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT Pk_Proveedores PRIMARY KEY(idProveedor),
    CONSTRAINT uqProveedorNit UNIQUE (nitProveedor),
    CONSTRAINT Fk_Proveedor_Plazo FOREIGN KEY (idPlazo) REFERENCES tblPlazo (idPlazo) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxProveedorPlazo (idPlazo),
    CONSTRAINT ckMovimientosProveedor CHECK (cargosMesProveedor >= 0 AND abonosMesProveedor >= 0 AND cargosAsignadosProveedor >= 0 AND cargosAcumuladosProveedor >= 0 AND abonosAcumuladosProveedor >= 0)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblEncabezadoOrdenCompra (
    idOrdenCompra                                INT            NOT NULL AUTO_INCREMENT,
    idProveedor                                  INT            NOT NULL,
    idPlazo                                      INT            NOT NULL,
    fechaEmisionOrdenCompra                      DATE           NOT NULL,
    fechaVencimientoOrdenCompra                  DATE           NOT NULL,
    idMoneda                                     BIGINT         NOT NULL,
    idTipoCambio                                 BIGINT         NULL,
    tipoCambioOrdenCompra                        DECIMAL(12,6)  NOT NULL DEFAULT 1,
    subtotalOrdenCompra                          DECIMAL(12,4)  NOT NULL,
    descuentoOrdenCompra                         DECIMAL(12,4)  NOT NULL DEFAULT 0,
    impuestoOrdenCompra                          DECIMAL(12,4) NOT NULL DEFAULT 0,
    totalOrdenCompra                             DECIMAL(12,4)  NOT NULL,
    comentarioOrdenCompra                        VARCHAR(500)   NULL,
    estadoOrdenCompra                            VARCHAR(30)    NOT NULL DEFAULT 'Pendiente',
    bloqueadoOrdenCompra                         BOOLEAN NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_EncabezadoOrdenCompra PRIMARY KEY (idOrdenCompra),
    CONSTRAINT Fk_EncabezadoOrdenCompra_Proveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor (idProveedor),
    CONSTRAINT Fk_EncabezadoOrdenCompra_Plazo FOREIGN KEY (idPlazo) REFERENCES tblPlazo (idPlazo),
    INDEX idxOrdenCompraProveedor (idProveedor),
    INDEX idxOrdenCompraFecha (fechaEmisionOrdenCompra),
    CONSTRAINT Fk_EncabezadoOrdenCompra_Moneda FOREIGN KEY (idMoneda) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_EncabezadoOrdenCompra_TipoCambio FOREIGN KEY (idTipoCambio) REFERENCES tblTipoCambio(Pk_TipoCambio) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxOrdenCompraMoneda (idMoneda),
    INDEX idxOrdenCompraTipoCambio (idTipoCambio),
    CONSTRAINT ckReferenciaTipoCambioOrdenCompra CHECK (idTipoCambio IS NOT NULL OR tipoCambioOrdenCompra = 1),
    CONSTRAINT ckTipoCambioOrdenCompra CHECK (tipoCambioOrdenCompra > 0),
    CONSTRAINT ckSubtotalOrdenCompra CHECK (subtotalOrdenCompra >= 0),
    CONSTRAINT ckDescuentoOrdenCompra CHECK (descuentoOrdenCompra >= 0),
    CONSTRAINT ckImpuestoOrdenCompra CHECK (impuestoOrdenCompra >= 0),
    CONSTRAINT ckImportesOrdenCompra CHECK (descuentoOrdenCompra <= subtotalOrdenCompra AND totalOrdenCompra = subtotalOrdenCompra - descuentoOrdenCompra + impuestoOrdenCompra),
    CONSTRAINT ckTotalOrdenCompra CHECK (totalOrdenCompra >= 0),
    CONSTRAINT ckFechasOrdenCompra CHECK (fechaVencimientoOrdenCompra >= fechaEmisionOrdenCompra),
    CONSTRAINT ckEstadoOrdenCompra CHECK (estadoOrdenCompra IN ('Pendiente','Autorizada','Rechazada','Anulada','Recibida'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblDetalleOrdenCompra (
    idOrdenCompra                                INT            NOT NULL,
    ordenDetalleOrdenCompra                      INT NOT NULL,
    idProducto                                   INT            NOT NULL,
    idBodega                                     INT            NOT NULL,
    cantidadDetalleOrdenCompra                   DECIMAL(12,4)  NOT NULL,
    costoUnitarioDetalleOrdenCompra              DECIMAL(12,4) NOT NULL,
    descuentoPorcentajeDetalleOrdenCompra        DECIMAL(5,2)  NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DetalleOrdenCompra PRIMARY KEY (idOrdenCompra, ordenDetalleOrdenCompra, idProducto),
    CONSTRAINT Fk_DetalleOrdenCompra_Encabezado FOREIGN KEY (idOrdenCompra) REFERENCES tblEncabezadoOrdenCompra (idOrdenCompra),
    CONSTRAINT Fk_DetalleOrdenCompra_Producto FOREIGN KEY (idProducto) REFERENCES tblProducto (idProducto) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleOrdenCompraProducto (idProducto),
    CONSTRAINT Fk_DetalleOrdenCompra_Bodega FOREIGN KEY (idBodega) REFERENCES tblBodega (idBodega) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleOrdenCompraBodega (idBodega),
    CONSTRAINT ckOrdenDetalleOrdenCompra CHECK (ordenDetalleOrdenCompra > 0),
    CONSTRAINT uqOrdenDetalleOrdenCompra UNIQUE (idOrdenCompra, ordenDetalleOrdenCompra),
    CONSTRAINT ckCantidadDetalleOrdenCompra CHECK (cantidadDetalleOrdenCompra > 0),
    CONSTRAINT ckCostoDetalleOrdenCompra CHECK (costoUnitarioDetalleOrdenCompra >= 0),
    CONSTRAINT ckDescPorcDetalleOrdenCompra CHECK (descuentoPorcentajeDetalleOrdenCompra BETWEEN 0 AND 100)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblEncabezadoCompra (
    idCompra                                     INT            NOT NULL AUTO_INCREMENT,
    idOrdenCompra                                INT            NULL,
    idProveedor                                  INT            NOT NULL,
    serieFacturaCompra                           VARCHAR(30)    NOT NULL,
    numeroFacturaCompra                          VARCHAR(30)    NOT NULL,
    fechaCompra                                  DATE           NOT NULL,
    idMetodoPago                                 INT            NOT NULL,
    idMoneda                                     BIGINT         NOT NULL,
    idTipoCambio                                 BIGINT         NULL,
    tipoCambioCompra                             DECIMAL(12,6)  NOT NULL DEFAULT 1,
    subtotalCompra                               DECIMAL(12,4)  NOT NULL,
    descuentoCompra                              DECIMAL(12,4)  NOT NULL DEFAULT 0,
    impuestoCompra                               DECIMAL(12,4) NOT NULL DEFAULT 0,
    totalCompra                                  DECIMAL(12,4)  NOT NULL,
    estadoCompra                                 VARCHAR(30)    NOT NULL DEFAULT 'Registrada',
    bloqueadoCompra                              BOOLEAN NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_EncabezadoCompra PRIMARY KEY (idCompra),
    CONSTRAINT Fk_EncabezadoCompra_EncabezadoOrdenCompra FOREIGN KEY (idOrdenCompra) REFERENCES tblEncabezadoOrdenCompra (idOrdenCompra),
    CONSTRAINT Fk_EncabezadoCompra_Proveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor (idProveedor),
    INDEX idxCompraOrden (idOrdenCompra),
    INDEX idxCompraProveedor (idProveedor),
    CONSTRAINT uqFacturaProveedorCompra UNIQUE (idProveedor, serieFacturaCompra, numeroFacturaCompra),
    CONSTRAINT Fk_EncabezadoCompra_Moneda FOREIGN KEY (idMoneda) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_EncabezadoCompra_TipoCambio FOREIGN KEY (idTipoCambio) REFERENCES tblTipoCambio(Pk_TipoCambio) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_EncabezadoCompra_MetodoPago FOREIGN KEY (idMetodoPago) REFERENCES tblMetodoPago(idMetodoPago),
    INDEX idxCompraMoneda (idMoneda),
    INDEX idxCompraTipoCambio (idTipoCambio),
    CONSTRAINT ckReferenciaTipoCambioCompra CHECK (idTipoCambio IS NOT NULL OR tipoCambioCompra = 1),
    CONSTRAINT ckTipoCambioCompra CHECK (tipoCambioCompra > 0),
    CONSTRAINT ckSubtotalCompra CHECK (subtotalCompra >= 0),
    CONSTRAINT ckDescuentoCompra CHECK (descuentoCompra >= 0),
    CONSTRAINT ckImpuestoCompra CHECK (impuestoCompra >= 0),
    CONSTRAINT ckImportesCompra CHECK (descuentoCompra <= subtotalCompra AND totalCompra = subtotalCompra - descuentoCompra + impuestoCompra),
    CONSTRAINT ckTotalCompra CHECK (totalCompra >= 0),
    CONSTRAINT ckEstadoCompra CHECK (estadoCompra IN ('Registrada','Anulada'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblDetalleCompra (
    idCompra                                     INT            NOT NULL,
    ordenDetalleCompra                           INT NOT NULL,
    idProducto                                   INT            NOT NULL,
    idBodega                                     INT            NOT NULL,
    cantidadDetalleCompra                        DECIMAL(12,4)  NOT NULL,
    costoUnitarioDetalleCompra                   DECIMAL(12,4)  NOT NULL,
    descuentoPorcentualDetalleCompra             DECIMAL(3,2)  NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DetalleCompra PRIMARY KEY (idCompra, ordenDetalleCompra, idProducto),
    CONSTRAINT Fk_DetalleCompra_Encabezado FOREIGN KEY (idCompra) REFERENCES tblEncabezadoCompra (idCompra),
    CONSTRAINT Fk_DetalleCompra_Producto FOREIGN KEY (idProducto) REFERENCES tblProducto (idProducto) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleCompraProducto (idProducto),
    CONSTRAINT Fk_DetalleCompra_Bodega FOREIGN KEY (idBodega) REFERENCES tblBodega (idBodega) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleCompraBodega (idBodega),
    CONSTRAINT ckOrdenDetalleCompra CHECK (ordenDetalleCompra > 0),
    CONSTRAINT uqOrdenDetalleCompra UNIQUE (idCompra, ordenDetalleCompra),
    CONSTRAINT ckCantidadDetalleCompra CHECK (cantidadDetalleCompra > 0),
    CONSTRAINT ckCostoDetalleCompra CHECK (costoUnitarioDetalleCompra >= 0),
    CONSTRAINT ckDescuentoDetalleCompra CHECK (descuentoPorcentualDetalleCompra BETWEEN 0 AND 0.99)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblCuentaPorPagar(
    idProveedor                                  INT NOT NULL,
    idConceptoProveedor                          INT NOT NULL,
    serieDocumentoCuentaPorPagar                 VARCHAR(20) NOT NULL,
    numeroDocumentoCuentaPorPagar                VARCHAR(30) NOT NULL,
    fechaEmisionCuentaPorPagar                   DATE NOT NULL,
    fechaVencimientoCuentaPorPagar               DATE NOT NULL,
    idMoneda                                     BIGINT         NOT NULL,
    idTipoCambio                                 BIGINT         NULL,
    tipoCambioCuentaPorPagar                     DECIMAL(12,6) NOT NULL DEFAULT 1,
    montoTotalCuentaPorPagar                     DECIMAL(12,4) NOT NULL,
    saldoActualCuentaPorPagar                    DECIMAL(12,4) NOT NULL,
    estadoCuentaPorPagar                         VARCHAR(30) NOT NULL DEFAULT 'Pendiente',
    bloqueadoCuentaPorPagar                      BOOLEAN NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT Pk_CuentaPorPagar PRIMARY KEY (serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor),
    CONSTRAINT Fk_Proveedor_idProveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor(idProveedor),
    CONSTRAINT Fk_CuentaPorPagar_Concepto FOREIGN KEY (idConceptoProveedor) REFERENCES tblConceptoProveedor (idConceptoProveedor),
    INDEX idxCuentaPorPagarVencimiento (fechaVencimientoCuentaPorPagar),
    CONSTRAINT Fk_CuentaPorPagar_Moneda FOREIGN KEY (idMoneda) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_CuentaPorPagar_TipoCambio FOREIGN KEY (idTipoCambio) REFERENCES tblTipoCambio(Pk_TipoCambio) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxCuentaPorPagarMoneda (idMoneda),
    INDEX idxCuentaPorPagarTipoCambio (idTipoCambio),
    CONSTRAINT ckReferenciaTipoCambioCuentaPorPagar CHECK (idTipoCambio IS NOT NULL OR tipoCambioCuentaPorPagar = 1),
    CONSTRAINT ckTipoCambioCuentaPorPagar CHECK (tipoCambioCuentaPorPagar > 0),
    CONSTRAINT ckMontoTotalCuentaPorPagar CHECK (montoTotalCuentaPorPagar <> 0),
    CONSTRAINT ckFechasCuentaPorPagar CHECK (fechaVencimientoCuentaPorPagar >= fechaEmisionCuentaPorPagar),
    CONSTRAINT ckEstadoCuentaPorPagar CHECK (estadoCuentaPorPagar IN ('Pendiente','Parcial','Pagada','Anulada'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblDevolucionCompra (
    idDevolucionCompra                           INT            NOT NULL AUTO_INCREMENT,
    idCompra                                     INT            NOT NULL,
    fechaDevolucionCompra                        DATE           NOT NULL,
    motivoDevolucionCompra                       VARCHAR(500)   NOT NULL,
    serieNotaCreditoDevolucionCompra             VARCHAR(20) NULL,
    numeroNotaCreditoDevolucionCompra            VARCHAR(30) NULL,
    totalDevolucionCompra                        DECIMAL(12,4)  NOT NULL,
    estadoDevolucionCompra                       VARCHAR(30)    NOT NULL DEFAULT 'Registrada',
    bloqueadoDevolucionCompra                    BOOLEAN NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DevolucionCompra PRIMARY KEY (idDevolucionCompra),
    CONSTRAINT Fk_DevolucionCompra_Compra FOREIGN KEY (idCompra) REFERENCES tblEncabezadoCompra (idCompra),
    INDEX idxDevolucionCompraCompra (idCompra),
    CONSTRAINT uqNotaCreditoDevolucionCompra UNIQUE (idCompra, serieNotaCreditoDevolucionCompra, numeroNotaCreditoDevolucionCompra),
    CONSTRAINT ckTotalDevolucionCompra CHECK (totalDevolucionCompra > 0),
    CONSTRAINT ckEstadoDevolucionCompra CHECK (estadoDevolucionCompra IN ('Registrada','Anulada'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblDetalleDevolucionCompra (
    idDevolucionCompra                           INT            NOT NULL,
    ordenDetalleDevolucionCompra                 INT NOT NULL,
    idProducto                                   INT            NOT NULL,
    idBodega                                     INT            NOT NULL,
    cantidadDetalleDevolucionCompra              DECIMAL(12,4)  NOT NULL,
    costoUnitarioDetalleDevolucionCompra         DECIMAL(12,4)  NOT NULL,
    descuentoPorcentajeDetalleDevolucionCompra   DECIMAL(3,2) NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DetalleDevolucionCompra PRIMARY KEY (idDevolucionCompra, ordenDetalleDevolucionCompra, idProducto),
    CONSTRAINT Fk_DetalleDevolucionCompra_Devolucion FOREIGN KEY (idDevolucionCompra) REFERENCES tblDevolucionCompra (idDevolucionCompra),
    CONSTRAINT Fk_DetalleDevolucionCompra_Producto FOREIGN KEY (idProducto) REFERENCES tblProducto (idProducto) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleDevolucionCompraProducto (idProducto),
    CONSTRAINT Fk_DetalleDevolucionCompra_Bodega FOREIGN KEY (idBodega) REFERENCES tblBodega (idBodega) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleDevolucionCompraBodega (idBodega),
    CONSTRAINT ckOrdenDetalleDevolucionCompra CHECK (ordenDetalleDevolucionCompra > 0),
    CONSTRAINT uqOrdenDetalleDevolucionCompra UNIQUE (idDevolucionCompra, ordenDetalleDevolucionCompra),
    CONSTRAINT ckCantidadDetalleDevolucionCompra CHECK (cantidadDetalleDevolucionCompra > 0),
    CONSTRAINT ckDescuentoDetalleDevolucionCompra CHECK (descuentoPorcentajeDetalleDevolucionCompra BETWEEN 0 AND 0.99),
    CONSTRAINT ckCostoDetalleDevolucionCompra CHECK (costoUnitarioDetalleDevolucionCompra >= 0)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblEncabezadoPagoProveedor (
    idPagoProveedor                              INT            NOT NULL AUTO_INCREMENT,
    idProveedor                                  INT            NOT NULL,
    fechaPagoProveedor                           DATE           NOT NULL,
    montoPagoProveedor                           DECIMAL(12,4)  NOT NULL,
    idMetodoPago                                 INT            NOT NULL,
    idMoneda                                     BIGINT         NOT NULL,
    idTipoCambio                                 BIGINT         NULL,
    tipoCambioPagoProveedor                      DECIMAL(12,6)  NOT NULL DEFAULT 1,
    referenciaPagoProveedor                      VARCHAR(30)    NULL,
    estadoPagoProveedor                          VARCHAR(30)    NOT NULL DEFAULT 'Programado',
    bloqueadoPagoProveedor                       BOOLEAN NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_EncabezadoPagoProveedor PRIMARY KEY (idPagoProveedor),
    CONSTRAINT Fk_EncabezadoPagoProveedor_Proveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor (idProveedor),
    INDEX idxPagoProveedorProveedor (idProveedor),
    INDEX idxPagoProveedorFecha (fechaPagoProveedor),
    CONSTRAINT ckMontoPagoProveedor CHECK (montoPagoProveedor > 0),
    CONSTRAINT Fk_EncabezadoPagoProveedor_Moneda FOREIGN KEY (idMoneda) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_EncabezadoPagoProveedor_TipoCambio FOREIGN KEY (idTipoCambio) REFERENCES tblTipoCambio(Pk_TipoCambio) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_EncabezadoPagoProveedor_MetodoPago FOREIGN KEY (idMetodoPago) REFERENCES tblMetodoPago(idMetodoPago),
    INDEX idxPagoProveedorMoneda (idMoneda),
    INDEX idxPagoProveedorTipoCambio (idTipoCambio),
    CONSTRAINT ckReferenciaTipoCambioPagoProveedor CHECK (idTipoCambio IS NOT NULL OR tipoCambioPagoProveedor = 1),
    CONSTRAINT ckTipoCambioPagoProveedor CHECK (tipoCambioPagoProveedor > 0),
    CONSTRAINT ckEstadoPagoProveedor CHECK (estadoPagoProveedor IN ('Pagado','Programado','Rechazado','Anulado'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblDetallePagoProveedor (
    idPagoProveedor                              INT            NOT NULL,
    serieDocumentoCuentaPorPagar                 VARCHAR(20) NOT NULL,
    numeroDocumentoCuentaPorPagar                VARCHAR(30) NOT NULL,
    idConceptoProveedor                          INT NOT NULL,
    montoAplicadoDetallePago                     DECIMAL(12,4)  NOT NULL,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DetallePagoProveedor PRIMARY KEY (idPagoProveedor, serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor),
    CONSTRAINT Fk_DetallePago_Encabezado FOREIGN KEY (idPagoProveedor) REFERENCES tblEncabezadoPagoProveedor (idPagoProveedor),
    CONSTRAINT Fk_DetallePago_CuentaPorPagar FOREIGN KEY (serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor) REFERENCES tblCuentaPorPagar (serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor),
    INDEX idxDetallePagoCuenta (serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor),
    CONSTRAINT ckMontoAplicadoDetallePago CHECK (montoAplicadoDetallePago > 0)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblSolicitudPago (
    idSolicitudPago                              INT            NOT NULL AUTO_INCREMENT,
    moduloOrigenSolicitudPago                    VARCHAR(30)    NOT NULL,
    idOrigenSolicitudPago                        INT            NOT NULL,
    beneficiarioSolicitudPago                    VARCHAR(100)   NOT NULL,
    montoSolicitudPago                           DECIMAL(12,4)  NOT NULL,
    metodoSolicitudPago                          VARCHAR(30)    NOT NULL,
    estadoSolicitudPago                          VARCHAR(30)    NOT NULL DEFAULT 'Pendiente',
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_SolicitudPago PRIMARY KEY (idSolicitudPago),
    INDEX idxSolicitudPagoOrigen (moduloOrigenSolicitudPago, idOrigenSolicitudPago),
    CONSTRAINT ckMontoSolicitudPago CHECK (montoSolicitudPago > 0),
    CONSTRAINT ckEstadoSolicitudPago CHECK (estadoSolicitudPago IN ('Pendiente','Procesada','Rechazada'))
) ENGINE=InnoDB;

INSERT INTO tblPlazo (nombrePlazo, diasPlazo) SELECT 'Contado', 0 WHERE NOT EXISTS (SELECT 1 FROM tblPlazo WHERE nombrePlazo = 'Contado');
INSERT INTO tblPlazo (nombrePlazo, diasPlazo) SELECT 'Pago contra entrega', 0 WHERE NOT EXISTS (SELECT 1 FROM tblPlazo WHERE nombrePlazo = 'Pago contra entrega');
INSERT INTO tblPlazo (nombrePlazo, diasPlazo) SELECT 'Cheque contra entrega', 0 WHERE NOT EXISTS (SELECT 1 FROM tblPlazo WHERE nombrePlazo = 'Cheque contra entrega');
INSERT INTO tblPlazo (nombrePlazo, diasPlazo) SELECT '15 dias', 15 WHERE NOT EXISTS (SELECT 1 FROM tblPlazo WHERE nombrePlazo = '15 dias');
INSERT INTO tblPlazo (nombrePlazo, diasPlazo) SELECT '30 dias', 30 WHERE NOT EXISTS (SELECT 1 FROM tblPlazo WHERE nombrePlazo = '30 dias');
INSERT INTO tblPlazo (nombrePlazo, diasPlazo) SELECT '60 dias', 60 WHERE NOT EXISTS (SELECT 1 FROM tblPlazo WHERE nombrePlazo = '60 dias');
INSERT INTO tblPlazo (nombrePlazo, diasPlazo) SELECT '90 dias', 90 WHERE NOT EXISTS (SELECT 1 FROM tblPlazo WHERE nombrePlazo = '90 dias');

INSERT INTO tblConceptoProveedor (nombreConceptoProveedor, naturalezaConceptoProveedor) SELECT 'Factura de compra', 'Cargo' WHERE NOT EXISTS (SELECT 1 FROM tblConceptoProveedor WHERE nombreConceptoProveedor = 'Factura de compra');
INSERT INTO tblConceptoProveedor (nombreConceptoProveedor, naturalezaConceptoProveedor) SELECT 'Nota de debito', 'Cargo' WHERE NOT EXISTS (SELECT 1 FROM tblConceptoProveedor WHERE nombreConceptoProveedor = 'Nota de debito');
INSERT INTO tblConceptoProveedor (nombreConceptoProveedor, naturalezaConceptoProveedor) SELECT 'Pago a proveedor', 'Abono' WHERE NOT EXISTS (SELECT 1 FROM tblConceptoProveedor WHERE nombreConceptoProveedor = 'Pago a proveedor');
INSERT INTO tblConceptoProveedor (nombreConceptoProveedor, naturalezaConceptoProveedor) SELECT 'Nota de credito', 'Abono' WHERE NOT EXISTS (SELECT 1 FROM tblConceptoProveedor WHERE nombreConceptoProveedor = 'Nota de credito');
INSERT INTO tblConceptoProveedor (nombreConceptoProveedor, naturalezaConceptoProveedor) SELECT 'Factura de transporte', 'Cargo' WHERE NOT EXISTS (SELECT 1 FROM tblConceptoProveedor WHERE nombreConceptoProveedor = 'Factura de transporte');
