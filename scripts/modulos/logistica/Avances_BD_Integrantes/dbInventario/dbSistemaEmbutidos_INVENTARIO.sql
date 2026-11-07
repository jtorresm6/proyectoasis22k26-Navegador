
use dbsistemaembutidos;


-- BLOQUE 1: SUCURSALES

CREATE TABLE tblSucursal(
    idSucursal INT AUTO_INCREMENT PRIMARY KEY,
    nombreSucursal VARCHAR(100) NOT NULL,
    descripcionSucursal VARCHAR(255),
    ubicacionSucursal VARCHAR(500) NOT NULL,
    paisSucursal VARCHAR(100) NOT NULL,
    estadoSucursal BOOLEAN DEFAULT TRUE,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- BLOQUE 2: BODEGAS
-- La ubicación se administra desde la sucursal.

CREATE TABLE tblBodega(
    idBodega INT AUTO_INCREMENT PRIMARY KEY,
    nombreBodega VARCHAR(100) NOT NULL,
    fkSucursalIdSucursal INT NOT NULL,
    paisBodega VARCHAR(100) NOT NULL,
    estadoBodega BOOLEAN DEFAULT TRUE,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	
    UNIQUE KEY UQ_Bodega_Sucursal (idBodega,fkSucursalIdSucursal),
    CONSTRAINT FK_Bodega_Sucursal FOREIGN KEY (fkSucursalIdSucursal) REFERENCES tblSucursal(idSucursal)
);

-- BLOQUE 3: CATÁLOGOS DE PRODUCTOS

CREATE TABLE tblCategoria(
    idCategoria INT AUTO_INCREMENT PRIMARY KEY,
    nombreCategoria VARCHAR(100) NOT NULL UNIQUE,
    porcentajeComisionCategoria DECIMAL(3,2) NOT NULL,
    estadoCategoria BOOLEAN DEFAULT TRUE,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE tblLinea (
    idLinea INT AUTO_INCREMENT PRIMARY KEY,
    nombreLinea VARCHAR(100) NOT NULL UNIQUE,
    porcentajeComisionLinea DECIMAL(3,2) NOT NULL,
    estadoLinea BOOLEAN DEFAULT TRUE,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE tblMarca(
    idMarca INT AUTO_INCREMENT PRIMARY KEY,
    nombreMarca VARCHAR(100) NOT NULL UNIQUE,
    porcentajeComisionMarca DECIMAL(3,2) NOT NULL,
    estadoMarca BOOLEAN DEFAULT TRUE,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- BLOQUE 4: PRODUCTOS

CREATE TABLE tblProducto(
    idProducto INT AUTO_INCREMENT PRIMARY KEY,
    skuProducto VARCHAR(30) NOT NULL UNIQUE,
    clasificacionProducto VARCHAR(5) NOT NULL DEFAULT '?????',
    nombreProducto VARCHAR(100) NOT NULL,
    fkMarcaIdMarca INT NOT NULL,
    fkCategoriaIdCategoria INT NOT NULL,
    fkLineaIdLinea INT NOT NULL,
    descripcionProducto VARCHAR(255),
    imagenProducto LONGBLOB NULL, -- Campo BLOB para almacenar la imagen (Método de conversión de Img a binario en aplicación)
    paisProducto VARCHAR(100) NOT NULL,
    precioCompraProducto DECIMAL(6,2) NOT NULL,
    precioVentaProducto DECIMAL(6,2) NOT NULL,
    
    porcentajeComisionProducto DECIMAL(3,2) NOT NULL,
    stockMinimoProducto INT NOT NULL ,
    stockMaximoProducto INT NOT NULL ,
    pesoProducto DECIMAL(4,2) NOT NULL,

    estadoProducto BOOLEAN DEFAULT TRUE,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT FK_Producto_Marca FOREIGN KEY (fkMarcaIdMarca) REFERENCES tblMarca(idMarca),
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (fkCategoriaIdCategoria) REFERENCES tblCategoria(idCategoria),
    CONSTRAINT FK_Producto_Linea FOREIGN KEY (fkLineaIdLinea) REFERENCES tblLinea(idLinea)

);

-- BLOQUE 5: STOCK POR SUCURSAL Y BODEGA
-- No se utiliza idStock.
-- La clave primaria es compuesta.

CREATE TABLE tblStock(
    fkSucursalIdSucursal INT NOT NULL,
    fkBodegaIdBodega INT NOT NULL,
    fkProductoIdProducto INT NOT NULL,
    
    stockFisicoStock INT NOT NULL DEFAULT 0,
    stockVirtualStock INT NOT NULL DEFAULT 0,

    estadoVentaStock BOOLEAN DEFAULT TRUE,
    estadoCompraStock BOOLEAN DEFAULT TRUE,

    estadoStock BOOLEAN DEFAULT TRUE,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    PRIMARY KEY (fkSucursalIdSucursal, fkBodegaIdBodega,fkProductoIdProducto),

    CONSTRAINT FK_Stock_SucursalBodega FOREIGN KEY (fkBodegaIdBodega, fkSucursalIdSucursal)
    REFERENCES tblBodega (idBodega,fkSucursalIdSucursal),

    CONSTRAINT FK_Stock_Producto FOREIGN KEY (fkProductoIdProducto)
    REFERENCES tblProducto(idProducto)
);


-- BLOQUE6: TIPOS DE MOVIMIENTO
-- Cada tipo de movimiento tiene un efecto asociado.

CREATE TABLE tblTipoMovimiento(
    idTipoMovimiento INT AUTO_INCREMENT PRIMARY KEY,
    nombreTipoMovimiento VARCHAR(50) NOT NULL UNIQUE,
    descripcionTipoMovimiento VARCHAR(255),
	signoTipoMovimiento CHAR(1) NOT NULL,
    
    estadoTipoMovimiento BOOLEAN DEFAULT TRUE,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP

);

-- BLOQUE 7 TRANSPORTE

CREATE TABLE tblTransporte(
    idTransporte INT AUTO_INCREMENT PRIMARY KEY,
    placaVehiculoTransporte VARCHAR(30) NOT NULL,
    conductorTransporte VARCHAR(100) NOT NULL,
    empresaTransporteTransporte VARCHAR(100) NOT NULL,

    estadoTransporte BOOLEAN DEFAULT TRUE,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- BLOQUE 8: MOVIMIENTOS DE INVENTARIO

CREATE TABLE tblMovimientoInventario(
    idMovimientoInventario INT AUTO_INCREMENT PRIMARY KEY,
    fkTipoMovimientoIdTipoMovimiento INT NOT NULL,

    fkBodegaOrigenIdBodega INT NULL,
    fkBodegaDestinoIdBodega INT NULL,
    fkTransporteIdTransporte INT NULL,
    fechaMovimientoInventario DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    observacionesMovimientoInventario VARCHAR(500) NOT NULL,
    
    estadoMovimientoInventario BOOLEAN DEFAULT TRUE,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT FK_Movimiento_Tipo FOREIGN KEY (fkTipoMovimientoIdTipoMovimiento)
	REFERENCES tblTipoMovimiento(idTipoMovimiento),

    CONSTRAINT FK_Movimiento_BodegaOrigen FOREIGN KEY (fkBodegaOrigenIdBodega)
    REFERENCES tblBodega(idBodega),

    CONSTRAINT FK_Movimiento_BodegaDestino FOREIGN KEY (fkBodegaDestinoIdBodega)
    REFERENCES tblBodega(idBodega),

    CONSTRAINT FK_Movimiento_Transporte FOREIGN KEY (fkTransporteIdTransporte)
    REFERENCES tblTransporte(idTransporte)
);


-- BLOQUE 9: DETALLE DE MOVIMIENTOS

CREATE TABLE tblDetalleMovimiento(
    idDetalleMovimiento INT AUTO_INCREMENT PRIMARY KEY,
    fkMovimientoInventarioIdMovimientoInventario INT NOT NULL,
	loteMovimientoInventario VARCHAR(30),
    fkProductoIdProducto INT NOT NULL,
    cantidadDetalleMovimiento INT NOT NULL,
    precioCompraDetalleMovimiento DECIMAL(6,2) NOT NULL,
    estadoDetalleMovimiento BOOLEAN DEFAULT TRUE,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT FK_DetalleMovimiento_Movimiento FOREIGN KEY (fkMovimientoInventarioIdMovimientoInventario)
    REFERENCES tblMovimientoInventario(idMovimientoInventario)
);

