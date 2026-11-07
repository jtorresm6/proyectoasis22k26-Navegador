-- ============================================================================
-- TERMINUS ERP | Logistica | Estandarizacion BD V1.3 | 24/10/2026
-- SCRIPT DE CREACION DE TABLAS Y OBJETOS DEL MODULO DE LOGISTICA
-- Base de datos: dbLogistica (nomenclatura camelCase segun EST-01 V1.3)
-- Ejecutar sobre una base vacia para reconstruir la estructura del modulo
-- ============================================================================

CREATE DATABASE IF NOT EXISTS dbLogistica
    CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE dbLogistica;

/* RESPONSABLE BRIAN DE LA CRUZ */ 
-- tblEmpresaTransportista
CREATE TABLE IF NOT EXISTS tblEmpresaTransportista (
Pk_Empresa_Transportista INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 nombreEmpresaTransportista VARCHAR(100) NOT NULL,
 contactoEmpresaTransportista VARCHAR(100) NOT NULL,
 telefonoEmpresaTransportista VARCHAR(30) COLLATE utf8mb4_unicode_ci NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqEmpresaTransportistaNombre UNIQUE (nombreEmpresaTransportista)
) ENGINE=InnoDB;

-- tblTipoServicio
CREATE TABLE IF NOT EXISTS tblTipoServicio (
Pk_Tipo_Servicio INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 nombreTipoServicio VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqTipoServicioNombre UNIQUE (nombreTipoServicio)
) ENGINE=InnoDB;

-- tblTarifaTransporte
CREATE TABLE IF NOT EXISTS tblTarifaTransporte (
Pk_Tarifa_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 nombreTarifaTransporte VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqTarifaTransporteNombre UNIQUE (nombreTarifaTransporte)
) ENGINE=InnoDB;

-- tblEstado
CREATE TABLE IF NOT EXISTS tblEstado (
Pk_Estado INT AUTO_INCREMENT NOT NULL,
    tipoEstado VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Estado PRIMARY KEY (Pk_Estado),
    CONSTRAINT uqTipoEstado UNIQUE (tipoEstado)
) ENGINE=InnoDB;

-- tblTipoTransporte
CREATE TABLE IF NOT EXISTS tblTipoTransporte (
Pk_Tipo_Transporte INT AUTO_INCREMENT NOT NULL,
    nombreTipoTransporte VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Tipo_Transporte PRIMARY KEY (Pk_Tipo_Transporte),
    CONSTRAINT uqNombreTipoTransporte UNIQUE (nombreTipoTransporte)
) ENGINE=InnoDB;

-- tblTransporte
CREATE TABLE IF NOT EXISTS tblTransporte (
Pk_Transporte INT AUTO_INCREMENT NOT NULL,
    nroPlacaTransporte VARCHAR(30) NOT NULL,
    marcaTransporte VARCHAR(100) NOT NULL,
    modeloTransporte VARCHAR(100) NOT NULL,
    anioTransporte SMALLINT NOT NULL,
    esRefrigeradoTransporte BOOLEAN NOT NULL DEFAULT FALSE,
    Fk_Estado_Codigo INT NOT NULL,
    Fk_Tipo_Transporte_Codigo INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Transporte PRIMARY KEY (Pk_Transporte),
    CONSTRAINT uqNroPlacaTransporte UNIQUE (nroPlacaTransporte),
    INDEX idxIdEstadoTransporte (Fk_Estado_Codigo),
    INDEX idxIdTipoTransporteTransporte (Fk_Tipo_Transporte_Codigo),
    CONSTRAINT Fk_Transporte_Estado FOREIGN KEY (Fk_Estado_Codigo) REFERENCES tblEstado(Pk_Estado) ON DELETE RESTRICT,
    CONSTRAINT Fk_Transporte_Tipo_Transporte FOREIGN KEY (Fk_Tipo_Transporte_Codigo) REFERENCES tblTipoTransporte(Pk_Tipo_Transporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblCamaraRefrigeracion
CREATE TABLE IF NOT EXISTS tblCamaraRefrigeracion (
Pk_Camara_Refrigeracion INT AUTO_INCREMENT NOT NULL,
    Fk_Transporte_Codigo INT NOT NULL,
    marcaEquipoCamaraRefrigeracion VARCHAR(100) NOT NULL,
    tempMinimaCelsiusCamaraRefrigeracion DECIMAL(5,2) NOT NULL,
    tempMaximaCelsiusCamaraRefrigeracion DECIMAL(5,2) NOT NULL,
    estadoOperativoCamaraRefrigeracion VARCHAR(30) NOT NULL DEFAULT 'OPERATIVO',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Camara_Refrigeracion PRIMARY KEY (Pk_Camara_Refrigeracion),
    CONSTRAINT uqIdTransporteCamaraRefrigeracion UNIQUE (Fk_Transporte_Codigo),
    CONSTRAINT Fk_Camara_Refrigeracion_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(Pk_Transporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblConductor
CREATE TABLE IF NOT EXISTS tblConductor (
Pk_Conductor INT AUTO_INCREMENT NOT NULL,
    codigoConductor VARCHAR(30) NOT NULL,
    nombreCompletoConductor VARCHAR(100) NOT NULL,
    numeroLicenciaConductor VARCHAR(30) NOT NULL,
    fechaVencimientoDocConductor DATE NOT NULL,
    Fk_Estado_Codigo INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Conductor PRIMARY KEY (Pk_Conductor),
    CONSTRAINT uqCodigoConductor UNIQUE (codigoConductor),
    CONSTRAINT uqNumeroLicenciaConductor UNIQUE (numeroLicenciaConductor),
    INDEX idxIdEstadoConductor (Fk_Estado_Codigo),
    CONSTRAINT Fk_Conductor_Estado FOREIGN KEY (Fk_Estado_Codigo) REFERENCES tblEstado(Pk_Estado) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblInspeccionSanitaria
CREATE TABLE IF NOT EXISTS tblInspeccionSanitaria (
Pk_Inspeccion_Sanitaria BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Transporte_Codigo INT NOT NULL,
    fechaInspeccionSanitaria DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    sanitizadoCorrectoInspeccionSanitaria BOOLEAN NOT NULL,
    libreOloresContamInspeccionSanitaria BOOLEAN NOT NULL,
    aprobadoParaCargaInspeccionSanitaria BOOLEAN NOT NULL,
    observacionesInspeccionSanitaria VARCHAR(500) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Inspeccion_Sanitaria PRIMARY KEY (Pk_Inspeccion_Sanitaria),
    INDEX idxIdTransporteInspeccionSanitaria (Fk_Transporte_Codigo),
    CONSTRAINT Fk_Inspeccion_Sanitaria_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(Pk_Transporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblLoteProducto
CREATE TABLE IF NOT EXISTS tblLoteProducto (
Pk_Lote_Producto BIGINT AUTO_INCREMENT NOT NULL,
    codigoLoteProducto VARCHAR(30) NOT NULL,
    descripcionLoteProducto VARCHAR(255) NOT NULL,
    requiereFrioLoteProducto BOOLEAN NOT NULL DEFAULT TRUE,
    fechaFabricacionLoteProducto DATE NOT NULL,
    fechaVencimientoLoteProducto DATE NOT NULL,
    tempRequeridaMinLoteProducto DECIMAL(5,2) NOT NULL,
    tempRequeridaMaxLoteProducto DECIMAL(5,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Lote_Producto PRIMARY KEY (Pk_Lote_Producto),
    CONSTRAINT uqCodigoLoteProducto UNIQUE (codigoLoteProducto)
) ENGINE=InnoDB;

-- tblEncabezadoViaje
CREATE TABLE IF NOT EXISTS tblEncabezadoViaje (
Pk_Encabezado_Viaje BIGINT AUTO_INCREMENT NOT NULL,
    fechaDocumentoEncabezadoViaje DATETIME NOT NULL,
    horaSalidaEncabezadoViaje DATETIME NOT NULL,
    horaLlegadaEncabezadoViaje DATETIME NULL,
    requiereCadenaFrioEncabezadoViaje BOOLEAN NOT NULL DEFAULT TRUE,
    estadoViajeEncabezadoViaje VARCHAR(30) NOT NULL DEFAULT 'EN_CURSO',
    pesoTotalEncabezadoViaje DECIMAL(18,3) NOT NULL,
    Fk_Transporte_Codigo INT NOT NULL,
    Fk_Conductor_Codigo INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Encabezado_Viaje PRIMARY KEY (Pk_Encabezado_Viaje),
    INDEX idxIdTransporteEncabezadoViaje (Fk_Transporte_Codigo),
    INDEX idxIdConductorEncabezadoViaje (Fk_Conductor_Codigo),
    INDEX idxEstadoViajeEncabezadoViaje (estadoViajeEncabezadoViaje),
    CONSTRAINT Fk_Encabezado_Viaje_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(Pk_Transporte) ON DELETE RESTRICT,
    CONSTRAINT Fk_Encabezado_Viaje_Conductor FOREIGN KEY (Fk_Conductor_Codigo) REFERENCES tblConductor(Pk_Conductor) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblDetalleViaje
CREATE TABLE IF NOT EXISTS tblDetalleViaje (
Pk_Detalle_Viaje BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Encabezado_Viaje_Codigo BIGINT NOT NULL,
    ordenDetalleViaje INT NOT NULL,
    descripcionDetalleViaje VARCHAR(255) NOT NULL,
    cantidadDetalleViaje DECIMAL(18,3) NOT NULL,
    pesoDetalleViaje DECIMAL(18,3) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Detalle_Viaje PRIMARY KEY (Pk_Detalle_Viaje),
    CONSTRAINT uqOrdenDetalleViaje UNIQUE (Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje),
    CONSTRAINT Fk_Detalle_Viaje_Encabezado_Viaje FOREIGN KEY (Fk_Encabezado_Viaje_Codigo) REFERENCES tblEncabezadoViaje(Pk_Encabezado_Viaje) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblDetalleViajeLoteProducto
CREATE TABLE IF NOT EXISTS tblDetalleViajeLoteProducto (
Pk_Detalle_Viaje_Lote_Producto BIGINT NOT NULL AUTO_INCREMENT,
    Fk_Detalle_Viaje_Codigo BIGINT NOT NULL,
    Fk_Lote_Producto_Codigo BIGINT NOT NULL,
    cantidadCajasDetalleViajeLoteProducto INT NOT NULL,
    pesoKgDetalleViajeLoteProducto DECIMAL(18,3) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Detalle_Viaje_Lote_Producto PRIMARY KEY (Pk_Detalle_Viaje_Lote_Producto),
    CONSTRAINT uqDetalleViajeLoteProducto UNIQUE (Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo),
    INDEX idxIdLoteProductoDetalleViajeLoteProducto (Fk_Lote_Producto_Codigo),
    CONSTRAINT Fk_Detalle_Viaje_Lote_Producto_Detalle_Viaje FOREIGN KEY (Fk_Detalle_Viaje_Codigo) REFERENCES tblDetalleViaje(Pk_Detalle_Viaje) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Viaje_Lote_Producto_Lote_Producto FOREIGN KEY (Fk_Lote_Producto_Codigo) REFERENCES tblLoteProducto(Pk_Lote_Producto) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblTelemetriaTemperatura
CREATE TABLE IF NOT EXISTS tblTelemetriaTemperatura (
Pk_Telemetria_Temperatura BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Encabezado_Viaje_Codigo BIGINT NOT NULL,
    fechaHoraTelemetriaTemperatura DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    temperaturaCelsiusTelemetriaTemperatura DECIMAL(5,2) NOT NULL,
    /* Porcentaje en formato decimal: 0.6500 = 65 % (DECIMAL(5,4) permite llegar a 100 %) */
    humedadRelativaTelemetriaTemperatura DECIMAL(5,4) NOT NULL,
    puertaAbiertaTelemetriaTemperatura BOOLEAN NOT NULL DEFAULT FALSE,
    ubicacionGpsTelemetriaTemperatura VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Telemetria_Temperatura PRIMARY KEY (Pk_Telemetria_Temperatura),
    INDEX idxCorrelativoFechaTelemetriaTemperatura (Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura),
    CONSTRAINT Fk_Telemetria_Temperatura_Encabezado_Viaje FOREIGN KEY (Fk_Encabezado_Viaje_Codigo) REFERENCES tblEncabezadoViaje(Pk_Encabezado_Viaje) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblAlertaCadenaFrio
CREATE TABLE IF NOT EXISTS tblAlertaCadenaFrio (
Pk_Alerta_Cadena_Frio BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Encabezado_Viaje_Codigo BIGINT NOT NULL,
    Fk_Telemetria_Temperatura_Codigo BIGINT NOT NULL,
    tipoAlertaCadenaFrio VARCHAR(30) NOT NULL,
    temperaturaRegistradaAlertaCadenaFrio DECIMAL(5,2) NOT NULL,
    atendidaAlertaCadenaFrio BOOLEAN NOT NULL DEFAULT FALSE,
    observacionCalidadAlertaCadenaFrio VARCHAR(500) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Alerta_Cadena_Frio PRIMARY KEY (Pk_Alerta_Cadena_Frio),
    INDEX idxCorrelativoEncabezadoViajeAlertaCadenaFrio (Fk_Encabezado_Viaje_Codigo),
    INDEX idxIdTelemetriaTemperaturaAlertaCadenaFrio (Fk_Telemetria_Temperatura_Codigo),
    INDEX idxAtendidaAlertaCadenaFrio (atendidaAlertaCadenaFrio),
    CONSTRAINT Fk_Alerta_Cadena_Frio_Encabezado_Viaje FOREIGN KEY (Fk_Encabezado_Viaje_Codigo) REFERENCES tblEncabezadoViaje(Pk_Encabezado_Viaje) ON DELETE CASCADE,
    CONSTRAINT Fk_Alerta_Cadena_Frio_Telemetria_Temperatura FOREIGN KEY (Fk_Telemetria_Temperatura_Codigo) REFERENCES tblTelemetriaTemperatura(Pk_Telemetria_Temperatura) ON DELETE CASCADE
) ENGINE=InnoDB;


/* RESPONSABLE VELVETH SARAI CHAVEZ MEJIA*/ 
-- tblSucursal
CREATE TABLE IF NOT EXISTS tblSucursal (
Pk_Sucursal INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreSucursal VARCHAR(100) NOT NULL,
    descripcionSucursal VARCHAR(255) NOT NULL,
    ubicacionSucursal VARCHAR(500) NOT NULL,
    paisSucursal VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- tblBodega
CREATE TABLE IF NOT EXISTS tblBodega (
Pk_Bodega INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreBodega VARCHAR(100) NOT NULL,
    Fk_Sucursal_Codigo INT NOT NULL,
    paisBodega VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	
    UNIQUE KEY uqBodegaSucursal (Pk_Bodega,Fk_Sucursal_Codigo),
    CONSTRAINT Fk_Bodega_Sucursal FOREIGN KEY (Fk_Sucursal_Codigo) REFERENCES tblSucursal(Pk_Sucursal) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblCategoria
CREATE TABLE IF NOT EXISTS tblCategoria (
Pk_Categoria INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreCategoria VARCHAR(100) NOT NULL,
    porcentajeComisionCategoria DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqCategoriaNombre UNIQUE (nombreCategoria),
    CONSTRAINT ckPorcentajeComisionCategoriaRango CHECK (porcentajeComisionCategoria >= 0.00 AND porcentajeComisionCategoria <= 0.99)
) ENGINE=InnoDB;

-- tblLinea
CREATE TABLE IF NOT EXISTS tblLinea (
Pk_Linea INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreLinea VARCHAR(100) NOT NULL,
    porcentajeComisionLinea DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqLineaNombre UNIQUE (nombreLinea),
    CONSTRAINT ckPorcentajeComisionLineaRango CHECK (porcentajeComisionLinea >= 0.00 AND porcentajeComisionLinea <= 0.99)
) ENGINE=InnoDB;

-- tblMarca
CREATE TABLE IF NOT EXISTS tblMarca (
Pk_Marca INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreMarca VARCHAR(100) NOT NULL,
    porcentajeComisionMarca DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqMarcaNombre UNIQUE (nombreMarca),
    CONSTRAINT ckPorcentajeComisionMarcaRango CHECK (porcentajeComisionMarca >= 0.00 AND porcentajeComisionMarca <= 0.99)
) ENGINE=InnoDB;

-- tblProducto
CREATE TABLE IF NOT EXISTS tblProducto (
Pk_Producto INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    skuProducto VARCHAR(30) NOT NULL,
    clasificacionProducto CHAR(5) NOT NULL DEFAULT '?????',
    nombreProducto VARCHAR(100) NOT NULL,
    Fk_Marca_Codigo INT NOT NULL,
    Fk_Categoria_Codigo INT NOT NULL,
    Fk_Linea_Codigo INT NOT NULL,
    descripcionProducto VARCHAR(255) NOT NULL,
    imagenProducto LONGBLOB NOT NULL, -- Campo BLOB para almacenar la imagen (Método de conversión de Img a binario en aplicación)
    paisProducto VARCHAR(100) NOT NULL,
    precioCompraProducto DECIMAL(12,4) NOT NULL,
    precioVentaProducto DECIMAL(12,4) NOT NULL,
    
    porcentajeComisionProducto DECIMAL(3,2) NOT NULL,
    stockAnteriorProducto INT NOT NULL DEFAULT 0,
    stockActualProducto INT NOT NULL DEFAULT 0,
    cargosMesProducto INT NOT NULL DEFAULT 0,
    abonosMesProducto INT NOT NULL DEFAULT 0,
    cargosAcumuladosProducto BIGINT NOT NULL DEFAULT 0,
    abonosAcumuladosProducto BIGINT NOT NULL DEFAULT 0,
    stockMinimoProducto INT NOT NULL ,
    stockMaximoProducto INT NOT NULL ,
    pesoProducto DECIMAL(4,2) NOT NULL,

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT uqProductoSku UNIQUE (skuProducto),
    CONSTRAINT Fk_Producto_Marca FOREIGN KEY (Fk_Marca_Codigo) REFERENCES tblMarca(Pk_Marca) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Categoria FOREIGN KEY (Fk_Categoria_Codigo) REFERENCES tblCategoria(Pk_Categoria) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Linea FOREIGN KEY (Fk_Linea_Codigo) REFERENCES tblLinea(Pk_Linea) ON DELETE RESTRICT,
    CONSTRAINT ckPorcentajeComisionProductoRango CHECK (porcentajeComisionProducto >= 0.00 AND porcentajeComisionProducto <= 0.99)
) ENGINE=InnoDB;

-- tblStock
CREATE TABLE IF NOT EXISTS tblStock (
Pk_Stock INT AUTO_INCREMENT NOT NULL,
Fk_Sucursal_Codigo INT NOT NULL,
    Fk_Bodega_Codigo INT NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    
    stockFisicoStock INT NOT NULL DEFAULT 0,
    stockVirtualStock INT NOT NULL DEFAULT 0,
    stockAnteriorStock INT NOT NULL DEFAULT 0,
    stockActualStock INT NOT NULL DEFAULT 0,
    cargosMesStock INT NOT NULL DEFAULT 0,
    abonosMesStock INT NOT NULL DEFAULT 0,
    cargosAcumuladosStock BIGINT NOT NULL DEFAULT 0,
    abonosAcumuladosStock BIGINT NOT NULL DEFAULT 0,

    estadoVentaStock BOOLEAN NOT NULL,
    estadoCompraStock BOOLEAN NOT NULL,

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_Stock PRIMARY KEY (Pk_Stock),
    CONSTRAINT uqStockSucursalBodegaProducto UNIQUE (Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo),

    CONSTRAINT Fk_Stock_Bodega FOREIGN KEY (Fk_Bodega_Codigo, Fk_Sucursal_Codigo)
    REFERENCES tblBodega(Pk_Bodega, Fk_Sucursal_Codigo) ON DELETE RESTRICT,

    CONSTRAINT Fk_Stock_Producto FOREIGN KEY (Fk_Producto_Codigo)
    REFERENCES tblProducto(Pk_Producto) ON DELETE RESTRICT,
    CONSTRAINT ckEstadoVentaStock CHECK (estadoVentaStock IN (0, 1)),
    CONSTRAINT ckEstadoCompraStock CHECK (estadoCompraStock IN (0, 1))
) ENGINE=InnoDB;

-- tblTipoMovimiento
CREATE TABLE IF NOT EXISTS tblTipoMovimiento (
Pk_Tipo_Movimiento INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreTipoMovimiento VARCHAR(100) NOT NULL,
    descripcionTipoMovimiento VARCHAR(255) NOT NULL,
	signoTipoMovimiento CHAR(1) NOT NULL,

	created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqTipoMovimientoNombre UNIQUE (nombreTipoMovimiento),
    CONSTRAINT ckSignoTipoMovimiento CHECK (signoTipoMovimiento IN ('+', '-'))
) ENGINE=InnoDB;

-- tblMovimientoInventario
CREATE TABLE IF NOT EXISTS tblMovimientoInventario (
Pk_Movimiento_Inventario INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Fk_Tipo_Movimiento_Codigo INT NOT NULL,

    Fk_Bodega_Origen_Codigo INT NULL,
    Fk_Bodega_Destino_Codigo INT NULL,
    Fk_Transporte_Codigo INT NULL,
    fechaMovimientoInventario DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    observacionesMovimientoInventario VARCHAR(500) NOT NULL,
    estadoMovimientoInventario ENUM('PENDIENTE', 'CUADRADO', 'CERRADO', 'ANULADO') NOT NULL DEFAULT 'PENDIENTE',
    Fk_Cierre_Movimiento_Codigo INT NULL COMMENT 'FK al cierre; permite NULL mientras el movimiento no se haya cerrado', 
    
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Fk_Movimiento_Inventario_Tipo_Movimiento FOREIGN KEY (Fk_Tipo_Movimiento_Codigo)
	REFERENCES tblTipoMovimiento(Pk_Tipo_Movimiento) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Bodega_Origen FOREIGN KEY (Fk_Bodega_Origen_Codigo)
    REFERENCES tblBodega(Pk_Bodega) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Bodega_Destino FOREIGN KEY (Fk_Bodega_Destino_Codigo)
    REFERENCES tblBodega(Pk_Bodega) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Transporte FOREIGN KEY (Fk_Transporte_Codigo)
    REFERENCES tblTransporte(Pk_Transporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblDetalleMovimiento
CREATE TABLE IF NOT EXISTS tblDetalleMovimiento (
Pk_Detalle_Movimiento INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Fk_Movimiento_Inventario_Codigo INT NOT NULL,
	loteDetalleMovimiento VARCHAR(30) NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    cantidadDetalleMovimiento INT NOT NULL,
    precioCompraDetalleMovimiento DECIMAL(12,4) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Fk_Detalle_Movimiento_Movimiento_Inventario FOREIGN KEY (Fk_Movimiento_Inventario_Codigo)
    REFERENCES tblMovimientoInventario(Pk_Movimiento_Inventario) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Movimiento_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(Pk_Producto) ON DELETE RESTRICT
) ENGINE=InnoDB;

/* RESPONSABLE JOSE XAVIER BOLAÑOS TENAS */ 
-- tblTraslado
CREATE TABLE IF NOT EXISTS tblTraslado (
Pk_Traslado INT NOT NULL AUTO_INCREMENT,
    codigoTraslado VARCHAR(30) NOT NULL,
    Fk_Bodega_Origen_Codigo INT NOT NULL,
    Fk_Bodega_Destino_Codigo INT NOT NULL,
    Fk_Transporte_Codigo INT NULL,
    fechaSolicitudTraslado DATETIME NOT NULL,
    fechaSalidaTraslado DATETIME NULL,
    fechaLlegadaTraslado DATETIME NULL,
    estadoTraslado ENUM('SOLICITADO','EN_TRANSITO','RECIBIDO','CANCELADO') NOT NULL DEFAULT 'SOLICITADO',
    observacionesTraslado VARCHAR(500) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    PRIMARY KEY (Pk_Traslado),
    UNIQUE KEY uqCodigoTraslado (codigoTraslado),
    CONSTRAINT Fk_Traslado_Bodega_Origen FOREIGN KEY (Fk_Bodega_Origen_Codigo) REFERENCES tblBodega(Pk_Bodega) ON DELETE RESTRICT,
    CONSTRAINT Fk_Traslado_Bodega_Destino FOREIGN KEY (Fk_Bodega_Destino_Codigo) REFERENCES tblBodega(Pk_Bodega) ON DELETE RESTRICT,
    CONSTRAINT Fk_Traslado_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(Pk_Transporte) ON DELETE RESTRICT,
    CONSTRAINT ckTrasladoBodegasDistintas CHECK (Fk_Bodega_Origen_Codigo <> Fk_Bodega_Destino_Codigo) -- Origen y destino no pueden ser la misma bodega
) ENGINE=InnoDB;

-- tblDetalleTraslado
CREATE TABLE IF NOT EXISTS tblDetalleTraslado (
Pk_Detalle_Traslado INT NOT NULL AUTO_INCREMENT,
    Fk_Traslado_Codigo INT NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    cantidadDetalleTraslado INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_Detalle_Traslado PRIMARY KEY (Pk_Detalle_Traslado),
    CONSTRAINT uqDetalleTrasladoProducto UNIQUE (Fk_Traslado_Codigo, Fk_Producto_Codigo),
    CONSTRAINT Fk_Detalle_Traslado_Traslado FOREIGN KEY (Fk_Traslado_Codigo) REFERENCES tblTraslado(Pk_Traslado) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Traslado_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(Pk_Producto) ON DELETE RESTRICT,
    CONSTRAINT ckDetalleTrasladoCantidad CHECK (cantidadDetalleTraslado > 0)
) ENGINE=InnoDB;

/* RESPONSABLE CARLOS EDUARDO CHIN CAAL */ 
-- tblAuditoriaInventario
CREATE TABLE IF NOT EXISTS tblAuditoriaInventario (
Pk_Auditoria_Inventario INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Fk_Movimiento_Inventario_Codigo INT NOT NULL,
    fechaAuditoriaInventario DATETIME NOT NULL,
    resultadoAuditoriaInventario VARCHAR(100) NOT NULL,
    observacionesCalidadAuditoriaInventario TEXT NOT NULL, -- Observaciones extensas, pueden superar 255 caracteres
    detallesAuditoriaInventario TEXT NOT NULL, -- Detalles extensos de auditoria, sin limite corto predefinido
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Fk_Auditoria_Inventario_Movimiento_Inventario FOREIGN KEY (Fk_Movimiento_Inventario_Codigo) REFERENCES tblMovimientoInventario(Pk_Movimiento_Inventario) ON DELETE RESTRICT
) ENGINE=InnoDB;

/* RESPONSABLE NILLS BERDUCIDO GOMEZ */ 

-- tblCierreMovimiento
CREATE TABLE IF NOT EXISTS tblCierreMovimiento (
Pk_Cierre_Movimiento INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreCierreMovimiento VARCHAR(100) NOT NULL,
    fechaInicioCierreMovimiento DATE NOT NULL,
    fechaFinCierreMovimiento DATE NOT NULL,
    estadoCierreMovimiento VARCHAR(30) NOT NULL DEFAULT 'ABIERTO',
    esCierreAnualCierreMovimiento BOOLEAN NOT NULL DEFAULT FALSE,
    toleranciaCierreMovimiento INT NOT NULL DEFAULT 0,
    diferenciaUnidadesCierreMovimiento INT NOT NULL DEFAULT 0,
    diferenciaMontoCierreMovimiento DECIMAL(12,4) NOT NULL DEFAULT 0,
    motivoAceptacionCierreMovimiento VARCHAR(500) NOT NULL,
    motivoAnulacionCierreMovimiento VARCHAR(500) NOT NULL,
    observacionesCierreMovimiento VARCHAR(500) NOT NULL,
    fechaEjecucionCierreMovimiento DATETIME NULL,
    CONSTRAINT uqCierreMovimientoFin UNIQUE (fechaFinCierreMovimiento),
    CONSTRAINT ckCierreMovimientoFechas CHECK (fechaInicioCierreMovimiento <= fechaFinCierreMovimiento),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- tblDetalleCierre
CREATE TABLE IF NOT EXISTS tblDetalleCierre (
Pk_Detalle_Cierre INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Fk_Cierre_Movimiento_Codigo INT NOT NULL,
    Fk_Stock_Codigo INT NOT NULL,
    stockAnteriorDetalleCierre INT NOT NULL DEFAULT 0,
    stockActualDetalleCierre INT NOT NULL DEFAULT 0,
    cargosMesDetalleCierre INT NOT NULL DEFAULT 0,
    abonosMesDetalleCierre INT NOT NULL DEFAULT 0,
    cargosAcumuladosDetalleCierre BIGINT NOT NULL DEFAULT 0,
    abonosAcumuladosDetalleCierre BIGINT NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Fk_Detalle_Cierre_Cierre_Movimiento FOREIGN KEY (Fk_Cierre_Movimiento_Codigo) REFERENCES tblCierreMovimiento(Pk_Cierre_Movimiento) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Cierre_Stock FOREIGN KEY (Fk_Stock_Codigo) REFERENCES tblStock(Pk_Stock) ON DELETE RESTRICT,
    CONSTRAINT uqDetalleCierreStock UNIQUE (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo)
) ENGINE=InnoDB;


-- DOCUMENTOS Y CIERRES: vinculo y fotografia del inventario antes del cierre.
-- La tabla MovimientoInventario se crea antes que CierreMovimiento.
-- Comprobar la existencia del vinculo para poder ejecutar de nuevo el script.
SET @sqlFkCierre = (
    SELECT IF(
        EXISTS (
            SELECT 1
            FROM information_schema.KEY_COLUMN_USAGE
            WHERE TABLE_SCHEMA = DATABASE()
              AND TABLE_NAME = 'tblMovimientoInventario'
              AND COLUMN_NAME = 'Fk_Cierre_Movimiento_Codigo'
              AND REFERENCED_TABLE_NAME = 'tblCierreMovimiento'
              AND REFERENCED_COLUMN_NAME = 'Pk_Cierre_Movimiento'
        ),
        'SELECT ''La FK del cierre ya existe'' AS mensaje',
        'ALTER TABLE tblMovimientoInventario ADD CONSTRAINT Fk_Movimiento_Inventario_Cierre_Movimiento FOREIGN KEY (Fk_Cierre_Movimiento_Codigo) REFERENCES tblCierreMovimiento(Pk_Cierre_Movimiento) ON DELETE RESTRICT'
    )
);
PREPARE stmFkCierre FROM @sqlFkCierre;
EXECUTE stmFkCierre;
DEALLOCATE PREPARE stmFkCierre;

-- tblHistoricoInventario: copia permanente del final de cada periodo, por producto y ubicación.
CREATE TABLE IF NOT EXISTS tblHistoricoInventario (
    Pk_Historico_Inventario BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Fk_Cierre_Movimiento_Codigo INT NOT NULL,
    Fk_Stock_Codigo INT NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    Fk_Sucursal_Codigo INT NOT NULL,
    Fk_Bodega_Codigo INT NOT NULL,
    fechaCorteHistoricoInventario DATE NOT NULL,
    stockAnteriorHistoricoInventario INT NOT NULL,
    stockActualHistoricoInventario INT NOT NULL,
    cargosMesHistoricoInventario INT NOT NULL,
    abonosMesHistoricoInventario INT NOT NULL,
    cargosAcumuladosHistoricoInventario BIGINT NOT NULL,
    abonosAcumuladosHistoricoInventario BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqHistoricoInventarioCierreStock UNIQUE (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo),
    CONSTRAINT Fk_Historico_Inventario_Cierre_Movimiento FOREIGN KEY (Fk_Cierre_Movimiento_Codigo)
        REFERENCES tblCierreMovimiento(Pk_Cierre_Movimiento) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Stock FOREIGN KEY (Fk_Stock_Codigo)
        REFERENCES tblStock(Pk_Stock) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Producto FOREIGN KEY (Fk_Producto_Codigo)
        REFERENCES tblProducto(Pk_Producto) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Bodega FOREIGN KEY (Fk_Bodega_Codigo, Fk_Sucursal_Codigo)
        REFERENCES tblBodega(Pk_Bodega, Fk_Sucursal_Codigo) ON DELETE RESTRICT
) ENGINE=InnoDB;

/* RESPONSABLE GABRIELA PINTO GARCIA  */ 
-- tblServicioTransporteExterno
CREATE TABLE IF NOT EXISTS tblServicioTransporteExterno (
Pk_Servicio_Transporte_Externo INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Empresa_Transportista_Codigo INT NOT NULL,
 Fk_Tipo_Servicio_Codigo INT NOT NULL,
 Fk_Traslado_Codigo INT NOT NULL,
 numeroGuiaServicioTransporteExterno VARCHAR(30) NOT NULL,
 estadoServicioTransporteExterno VARCHAR(30) NOT NULL DEFAULT 'Programado',
 origenServicioTransporteExterno VARCHAR(100) NOT NULL,
 direccionOrigenServicioTransporteExterno VARCHAR(500) NOT NULL,
 destinoServicioTransporteExterno VARCHAR(100) NOT NULL,
 direccionDestinoServicioTransporteExterno VARCHAR(500) NOT NULL,
 fechaContratacionServicioTransporteExterno DATE NOT NULL,
 fechaEstimadaEntregaServicioTransporteExterno DATE NOT NULL,
 fechaEntregaRealServicioTransporteExterno DATE NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqServicioNumeroGuia UNIQUE (numeroGuiaServicioTransporteExterno),
 CONSTRAINT Fk_Servicio_Transporte_Externo_Empresa_Transportista FOREIGN KEY (Fk_Empresa_Transportista_Codigo) REFERENCES tblEmpresaTransportista(Pk_Empresa_Transportista) ON DELETE RESTRICT,
 CONSTRAINT Fk_Servicio_Transporte_Externo_Tipo_Servicio FOREIGN KEY (Fk_Tipo_Servicio_Codigo) REFERENCES tblTipoServicio(Pk_Tipo_Servicio) ON DELETE RESTRICT,
 CONSTRAINT Fk_Servicio_Transporte_Externo_Traslado FOREIGN KEY (Fk_Traslado_Codigo) REFERENCES tblTraslado(Pk_Traslado) ON DELETE RESTRICT,
    CONSTRAINT ckServicioEstado CHECK (estadoServicioTransporteExterno IN ('Programado','En tránsito','Entregado','Cancelado')),
    CONSTRAINT ckServicioFechas CHECK (fechaEstimadaEntregaServicioTransporteExterno >= fechaContratacionServicioTransporteExterno)
) ENGINE=InnoDB;

-- tblCostoTransporte
CREATE TABLE IF NOT EXISTS tblCostoTransporte (
Pk_Costo_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 Fk_Tarifa_Transporte_Codigo INT NOT NULL,
 cantidadCostoTransporte DECIMAL(12,4) NOT NULL,
 costoUnitarioCostoTransporte DECIMAL(12,4) NOT NULL,
 costoTotalCostoTransporte DECIMAL(12,4) NOT NULL,
 monedaCostoTransporte CHAR(3) NOT NULL,
 tipoPagoCostoTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqCostoServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Costo_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE,
 CONSTRAINT Fk_Costo_Transporte_Tarifa_Transporte FOREIGN KEY (Fk_Tarifa_Transporte_Codigo) REFERENCES tblTarifaTransporte(Pk_Tarifa_Transporte) ON DELETE RESTRICT,
    CONSTRAINT ckCostoCantidad CHECK (cantidadCostoTransporte > 0),
    CONSTRAINT ckCostoUnitario CHECK (costoUnitarioCostoTransporte >= 0),
    CONSTRAINT ckCostoMoneda CHECK (monedaCostoTransporte IN ('GTQ','USD')),
    CONSTRAINT ckCostoTipoPago CHECK (tipoPagoCostoTransporte IN ('Transferencia','Efectivo'))
) ENGINE=InnoDB;

-- tblVehiculoTransporte
CREATE TABLE IF NOT EXISTS tblVehiculoTransporte (
Pk_Vehiculo_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 placaVehiculoTransporte VARCHAR(30) NOT NULL,
 tipoVehiculoTransporte VARCHAR(100) NOT NULL,
 capacidadVehiculoTransporte VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqVehiculoServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Vehiculo_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblConductorTransporte
CREATE TABLE IF NOT EXISTS tblConductorTransporte (
Pk_Conductor_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 nombreConductorTransporte VARCHAR(100) NOT NULL,
 telefonoConductorTransporte VARCHAR(30) COLLATE utf8mb4_unicode_ci NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqConductorServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Conductor_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblProductoTransporte
CREATE TABLE IF NOT EXISTS tblProductoTransporte (
Pk_Producto_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 Fk_Producto_Codigo INT NOT NULL,
 cantidadProductoTransporte DECIMAL(12,4) NOT NULL,
 unidadMedidaProductoTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Producto_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE,
 CONSTRAINT Fk_Producto_Transporte_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(Pk_Producto) ON DELETE RESTRICT,
    CONSTRAINT ckProductoCantidad CHECK (cantidadProductoTransporte > 0)
) ENGINE=InnoDB;

-- tblUbicacionTransporte
CREATE TABLE IF NOT EXISTS tblUbicacionTransporte (
Pk_Ubicacion_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 ubicacionActualUbicacionTransporte VARCHAR(500) NOT NULL,
 fechaActualizacionUbicacionTransporte DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Ubicacion_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE,
 INDEX idxUbicacionServicioFecha (Fk_Servicio_Transporte_Externo_Codigo, fechaActualizacionUbicacionTransporte)
) ENGINE=InnoDB;

-- tblIncidenciaTransporte
CREATE TABLE IF NOT EXISTS tblIncidenciaTransporte (
Pk_Incidencia_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 codigoIncidenciaTransporte VARCHAR(30) NOT NULL,
 fechaIncidenciaTransporte DATETIME NOT NULL,
 tipoIncidenciaTransporte VARCHAR(100) NOT NULL,
 descripcionIncidenciaTransporte TEXT NOT NULL, -- Relato amplio de incidencia, puede superar 255 caracteres
 responsableIncidenciaTransporte VARCHAR(100) NOT NULL,
 estadoIncidenciaTransporte VARCHAR(30) NOT NULL DEFAULT 'Pendiente',
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqIncidenciaCodigo UNIQUE (codigoIncidenciaTransporte),
 CONSTRAINT Fk_Incidencia_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblHistorialTransporte
CREATE TABLE IF NOT EXISTS tblHistorialTransporte (
Pk_Historial_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 fechaMovimientoHistorialTransporte DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
 tipoMovimientoHistorialTransporte VARCHAR(100) NOT NULL,
 descripcionMovimientoHistorialTransporte VARCHAR(255) NOT NULL,
 estadoTransporteHistorialTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Historial_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblSolicitudPagoTransporte
CREATE TABLE IF NOT EXISTS tblSolicitudPagoTransporte (
Pk_Solicitud_Pago_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 codigoSolicitudPagoTransporte VARCHAR(30) NOT NULL,
 estadoSolicitudPagoTransporte VARCHAR(30) NOT NULL DEFAULT 'Solicitado',
 referenciaBancariaSolicitudPagoTransporte VARCHAR(100) NULL,
 fechaPagoSolicitudPagoTransporte DATE NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqSolicitudServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT uqSolicitudCodigo UNIQUE (codigoSolicitudPagoTransporte),
 CONSTRAINT Fk_Solicitud_Pago_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo)
  REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE
) ENGINE=InnoDB;

-- VISTA DE SOLICITUDES DE PAGO
CREATE OR REPLACE VIEW vwSolicitudesPagoTransporte AS
SELECT s.codigoSolicitudPagoTransporte AS solicitud,
       t.numeroGuiaServicioTransporteExterno AS numeroGuia,
       e.nombreEmpresaTransportista AS beneficiario,
       CASE ts.nombreTipoServicio
           WHEN 'Refrigerado' THEN 'Servicio de transporte externo refrigerado'
           WHEN 'Congelado' THEN 'Servicio de transporte externo congelado'
           WHEN 'Carga seca' THEN 'Servicio de transporte externo de carga seca'
           WHEN 'Carga especial' THEN 'Servicio de transporte externo de carga especial'
           ELSE CONCAT('Servicio de transporte externo ', LOWER(ts.nombreTipoServicio))
       END AS concepto,
       c.costoTotalCostoTransporte AS montoTotal,
       s.fechaPagoSolicitudPagoTransporte AS fechaPago,
       s.estadoSolicitudPagoTransporte AS estadoSolicitud,
       s.referenciaBancariaSolicitudPagoTransporte AS referenciaBancaria
FROM tblServicioTransporteExterno t
JOIN tblEmpresaTransportista e ON e.Pk_Empresa_Transportista = t.Fk_Empresa_Transportista_Codigo
JOIN tblTipoServicio ts ON ts.Pk_Tipo_Servicio = t.Fk_Tipo_Servicio_Codigo
JOIN tblCostoTransporte c ON c.Fk_Servicio_Transporte_Externo_Codigo = t.Pk_Servicio_Transporte_Externo
JOIN tblSolicitudPagoTransporte s ON s.Fk_Servicio_Transporte_Externo_Codigo = t.Pk_Servicio_Transporte_Externo;


DROP PROCEDURE IF EXISTS spGenerarSolicitudPagoTransporte;
DELIMITER $$
-- La referencia bancaria y la fecha de pago pueden quedar NULL mientras el pago esté pendiente.
CREATE PROCEDURE spGenerarSolicitudPagoTransporte(
    IN pIdServicio INT,
    IN pReferenciaBancaria VARCHAR(100),
    IN pFechaPago DATE
)
BEGIN
    DECLARE vId INT;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;
    START TRANSACTION;
    IF NOT EXISTS (SELECT 1 FROM tblCostoTransporte
                   WHERE Fk_Servicio_Transporte_Externo_Codigo = pIdServicio) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El servicio no existe o no tiene costo registrado';
    END IF;
    INSERT INTO tblSolicitudPagoTransporte (
        Fk_Servicio_Transporte_Externo_Codigo,
        codigoSolicitudPagoTransporte,
        referenciaBancariaSolicitudPagoTransporte,
        fechaPagoSolicitudPagoTransporte
    ) VALUES (
        pIdServicio,
        CONCAT('TMP-', LEFT(REPLACE(UUID(), '-', ''), 26)),
        pReferenciaBancaria,
        pFechaPago
    );
    SET vId = LAST_INSERT_ID();
    UPDATE tblSolicitudPagoTransporte
    SET codigoSolicitudPagoTransporte = CONCAT('SP-', LPAD(vId, 6, '0'))
    WHERE Pk_Solicitud_Pago_Transporte = vId;
    COMMIT;
    SELECT vId AS idSolicitud, CONCAT('SP-', LPAD(vId, 6, '0')) AS codigoSolicitudPago;
END$$
DELIMITER ;

DROP PROCEDURE IF EXISTS spRegistrarServicioTransporteExterno;
DELIMITER $$
CREATE PROCEDURE spRegistrarServicioTransporteExterno(
    IN pEmpresa INT, IN pTipoServicio INT, IN pTraslado INT,
    IN pOrigen VARCHAR(100), IN pDireccionOrigen VARCHAR(500),
    IN pDestino VARCHAR(100), IN pDireccionDestino VARCHAR(500),
    IN pFechaContratacion DATE, IN pFechaEstimada DATE, IN pFechaReal DATE,
    IN pTarifa INT, IN pCantidad DECIMAL(12,4), IN pCosto DECIMAL(12,4),
    IN pMoneda CHAR(3), IN pTipoPago VARCHAR(30)
)
BEGIN
    DECLARE vId INT;
    DECLARE vEmpresa VARCHAR(100);
    DECLARE vPrefijo VARCHAR(10);
    DECLARE vGuia VARCHAR(30);
    DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;
    START TRANSACTION;
    IF pTraslado IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El traslado es obligatorio';
    END IF;
    SELECT nombreEmpresaTransportista INTO vEmpresa
    FROM tblEmpresaTransportista WHERE Pk_Empresa_Transportista = pEmpresa;
    IF vEmpresa IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Empresa transportista no válida';
    END IF;
    SET vPrefijo = CASE UPPER(TRIM(vEmpresa))
        WHEN 'FORZA' THEN 'FZ' WHEN 'CARGO EXPRESO' THEN 'CE'
        WHEN 'GUATEX' THEN 'GX' WHEN 'DHL EXPRESS' THEN 'DHL'
        WHEN 'FEDEX' THEN 'FDX' WHEN 'UPS' THEN 'UPS' ELSE 'TR' END;
    INSERT INTO tblServicioTransporteExterno (
       Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo,
       origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno,
       destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno,
       fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno,
       fechaEntregaRealServicioTransporteExterno,
       numeroGuiaServicioTransporteExterno
    ) VALUES (pEmpresa, pTipoServicio, pTraslado, pOrigen, pDireccionOrigen,
              pDestino, pDireccionDestino, pFechaContratacion, pFechaEstimada, pFechaReal,
              CONCAT('TMP-', LEFT(REPLACE(UUID(), '-', ''), 26)));
    SET vId = LAST_INSERT_ID();
    SET vGuia = CONCAT(vPrefijo, '-', LPAD(vId, 6, '0'));
    UPDATE tblServicioTransporteExterno
    SET numeroGuiaServicioTransporteExterno = vGuia
    WHERE Pk_Servicio_Transporte_Externo = vId;
    INSERT INTO tblCostoTransporte (
       Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo,
       cantidadCostoTransporte, costoUnitarioCostoTransporte, costoTotalCostoTransporte,
       monedaCostoTransporte, tipoPagoCostoTransporte
    ) VALUES (vId, pTarifa, pCantidad, pCosto, ROUND(pCantidad * pCosto, 4), pMoneda, pTipoPago);
    COMMIT;
    SELECT vId AS idServicio, vGuia AS numeroGuia;
END$$
DELIMITER ;


-- ============================================================================
-- CIERRE MENSUAL DE INVENTARIO (compatible con MySQL 8.0+)
-- Los contadores mensuales se actualizan AL CONFIRMAR CADA MOVIMIENTO en la
-- aplicación/servicio transaccional. Este procedimiento NO los vuelve a sumar.
-- stockActualStock debe representar stockAnteriorStock + cargosMes - abonosMes
-- antes del cierre. El cierre guarda la fotografía, acumula, y reinicia el mes.
-- ============================================================================

DROP PROCEDURE IF EXISTS spEjecutarCierreInventario;
DELIMITER $$
CREATE PROCEDURE spEjecutarCierreInventario(IN pIdCierre INT)
BEGIN
    DECLARE vInicio DATE;
    DECLARE vFin DATE;
    DECLARE vEstado VARCHAR(30);
    DECLARE vCantidad INT DEFAULT 0;
    DECLARE vUltimoFin DATE;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;
    SELECT fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento
      INTO vInicio, vFin, vEstado
      FROM tblCierreMovimiento
     WHERE Pk_Cierre_Movimiento = pIdCierre FOR UPDATE;

    IF vInicio IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Cierre no encontrado';
    END IF;
    IF vEstado <> 'ABIERTO' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Solo se puede ejecutar un cierre ABIERTO';
    END IF;
    IF vInicio <> DATE_FORMAT(vInicio, '%Y-%m-01')
       OR vFin <> LAST_DAY(vInicio) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El cierre debe abarcar un mes calendario completo';
    END IF;
    IF vFin >= CURRENT_DATE() THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Solo puede cerrarse un mes ya terminado';
    END IF;

    SELECT MAX(fechaFinCierreMovimiento) INTO vUltimoFin
      FROM tblCierreMovimiento
     WHERE estadoCierreMovimiento = 'CERRADO';
    IF vUltimoFin IS NOT NULL AND vInicio <> DATE_ADD(vUltimoFin, INTERVAL 1 DAY) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El cierre debe seguir consecutivamente al ultimo cierre';
    END IF;

    -- Bloquear existencias durante todo el corte; ejecutar sin movimientos concurrentes.
    UPDATE tblStock SET stockActualStock = stockActualStock;

    -- No puede haber documentos sin revisar ni datos de detalle defectuosos.
    SELECT COUNT(*) INTO vCantidad
      FROM tblMovimientoInventario m
     WHERE m.fechaMovimientoInventario >= vInicio
       AND m.fechaMovimientoInventario < DATE_ADD(vFin, INTERVAL 1 DAY)
       AND m.estadoMovimientoInventario NOT IN ('CUADRADO', 'ANULADO');
    IF vCantidad > 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Hay documentos pendientes de cuadre';
    END IF;

    SELECT COUNT(*) INTO vCantidad
      FROM tblMovimientoInventario m
     WHERE m.fechaMovimientoInventario >= vInicio
       AND m.fechaMovimientoInventario < DATE_ADD(vFin, INTERVAL 1 DAY)
       AND m.estadoMovimientoInventario = 'CUADRADO'
       AND NOT EXISTS (SELECT 1 FROM tblDetalleMovimiento d
                        WHERE d.Fk_Movimiento_Inventario_Codigo = m.Pk_Movimiento_Inventario
                          AND d.cantidadDetalleMovimiento > 0);
    IF vCantidad > 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Hay movimientos cuadrados sin detalle valido';
    END IF;

    SELECT COUNT(*) INTO vCantidad FROM tblStock
     WHERE stockAnteriorStock < 0 OR stockActualStock < 0
        OR cargosMesStock < 0 OR abonosMesStock < 0
        OR stockActualStock <> stockAnteriorStock + cargosMesStock - abonosMesStock;
    IF vCantidad > 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Existen saldos sin cuadrar en stock';
    END IF;

    -- Fotografía del periodo ANTES de cualquier actualización (incluye ceros).
    INSERT INTO tblHistoricoInventario
        (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Producto_Codigo,
         Fk_Sucursal_Codigo, Fk_Bodega_Codigo, fechaCorteHistoricoInventario,
         stockAnteriorHistoricoInventario, stockActualHistoricoInventario,
         cargosMesHistoricoInventario, abonosMesHistoricoInventario,
         cargosAcumuladosHistoricoInventario, abonosAcumuladosHistoricoInventario)
    SELECT pIdCierre, Pk_Stock, Fk_Producto_Codigo, Fk_Sucursal_Codigo,
           Fk_Bodega_Codigo, vFin, stockAnteriorStock, stockActualStock,
           cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock
      FROM tblStock;

    -- La tabla DetalleCierre conserva el resumen del corte.
    INSERT INTO tblDetalleCierre
        (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo,
         stockAnteriorDetalleCierre, stockActualDetalleCierre,
         cargosMesDetalleCierre, abonosMesDetalleCierre,
         cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
    SELECT pIdCierre, Pk_Stock, stockAnteriorStock, stockActualStock,
           cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock
      FROM tblStock;

    -- Inmovilizar documentos del mes antes de marcar cerrado el corte.
    UPDATE tblMovimientoInventario
       SET estadoMovimientoInventario = 'CERRADO', Fk_Cierre_Movimiento_Codigo = pIdCierre
     WHERE fechaMovimientoInventario >= vInicio
       AND fechaMovimientoInventario < DATE_ADD(vFin, INTERVAL 1 DAY)
       AND estadoMovimientoInventario = 'CUADRADO';

    -- El stock actual permanece; el anterior pasa a ser el stock actual.
    UPDATE tblStock
       SET stockAnteriorStock = stockActualStock,
           cargosAcumuladosStock = cargosAcumuladosStock + cargosMesStock,
           abonosAcumuladosStock = abonosAcumuladosStock + abonosMesStock,
           cargosMesStock = 0,
           abonosMesStock = 0;

    -- Los totales por producto son la suma de sus ubicaciones.
    UPDATE tblProducto p
    LEFT JOIN (
        SELECT Fk_Producto_Codigo AS idProducto,
               SUM(stockAnteriorStock) AS anterior, SUM(stockActualStock) AS actual,
               SUM(cargosMesStock) AS cargos, SUM(abonosMesStock) AS abonos,
               SUM(cargosAcumuladosStock) AS cargosAc,
               SUM(abonosAcumuladosStock) AS abonosAc
          FROM tblStock GROUP BY Fk_Producto_Codigo
    ) a ON a.idProducto = p.Pk_Producto
       SET p.stockAnteriorProducto = COALESCE(a.anterior, 0),
           p.stockActualProducto = COALESCE(a.actual, 0),
           p.cargosMesProducto = COALESCE(a.cargos, 0),
           p.abonosMesProducto = COALESCE(a.abonos, 0),
           p.cargosAcumuladosProducto = COALESCE(a.cargosAc, 0),
           p.abonosAcumuladosProducto = COALESCE(a.abonosAc, 0);

    UPDATE tblCierreMovimiento
       SET estadoCierreMovimiento = 'CERRADO',
           fechaEjecucionCierreMovimiento = NOW()
     WHERE Pk_Cierre_Movimiento = pIdCierre;
    COMMIT;
END$$
DELIMITER ;

-- Evitar modificaciones o borrados de movimientos de periodos cerrados.
-- Eliminar nombres anteriores y actuales para permitir ejecutar de nuevo el script.
DROP TRIGGER IF EXISTS trMovimientoInventarioNoEditarCerrado;
DROP TRIGGER IF EXISTS trMovimientoInventarioBeforeUpdateCerrado;
DELIMITER $$
CREATE TRIGGER trMovimientoInventarioBeforeUpdateCerrado
BEFORE UPDATE ON tblMovimientoInventario FOR EACH ROW
BEGIN
    IF OLD.estadoMovimientoInventario = 'CERRADO' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Documento cerrado: no se puede editar';
    END IF;
END$$
DELIMITER ;

DROP TRIGGER IF EXISTS trMovimientoInventarioNoBorrarCerrado;
DROP TRIGGER IF EXISTS trMovimientoInventarioBeforeDeleteCerrado;
DELIMITER $$
CREATE TRIGGER trMovimientoInventarioBeforeDeleteCerrado
BEFORE DELETE ON tblMovimientoInventario FOR EACH ROW
BEGIN
    IF OLD.estadoMovimientoInventario = 'CERRADO' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Documento cerrado: no se puede borrar';
    END IF;
END$$
DELIMITER ;

DROP TRIGGER IF EXISTS trDetalleMovimientoNoEditarCerrado;
DROP TRIGGER IF EXISTS trDetalleMovimientoBeforeUpdateCerrado;
DELIMITER $$
CREATE TRIGGER trDetalleMovimientoBeforeUpdateCerrado
BEFORE UPDATE ON tblDetalleMovimiento FOR EACH ROW
BEGIN
    IF EXISTS (SELECT 1 FROM tblMovimientoInventario m
               WHERE m.Pk_Movimiento_Inventario = OLD.Fk_Movimiento_Inventario_Codigo
                 AND m.estadoMovimientoInventario = 'CERRADO')
       OR EXISTS (SELECT 1 FROM tblMovimientoInventario m
               WHERE m.Pk_Movimiento_Inventario = NEW.Fk_Movimiento_Inventario_Codigo
                 AND m.estadoMovimientoInventario = 'CERRADO') THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Detalle de documento cerrado: no se puede editar';
    END IF;
END$$
DELIMITER ;

DROP TRIGGER IF EXISTS trDetalleMovimientoNoBorrarCerrado;
DROP TRIGGER IF EXISTS trDetalleMovimientoBeforeDeleteCerrado;
DELIMITER $$
CREATE TRIGGER trDetalleMovimientoBeforeDeleteCerrado
BEFORE DELETE ON tblDetalleMovimiento FOR EACH ROW
BEGIN
    IF EXISTS (SELECT 1 FROM tblMovimientoInventario m
               WHERE m.Pk_Movimiento_Inventario = OLD.Fk_Movimiento_Inventario_Codigo
                 AND m.estadoMovimientoInventario = 'CERRADO') THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Detalle de documento cerrado: no se puede borrar';
    END IF;
END$$
DELIMITER ;

DROP TRIGGER IF EXISTS trDetalleMovimientoNoAgregarCerrado;
DROP TRIGGER IF EXISTS trDetalleMovimientoBeforeInsertCerrado;
DELIMITER $$
CREATE TRIGGER trDetalleMovimientoBeforeInsertCerrado
BEFORE INSERT ON tblDetalleMovimiento FOR EACH ROW
BEGIN
    IF EXISTS (SELECT 1 FROM tblMovimientoInventario m
               WHERE m.Pk_Movimiento_Inventario = NEW.Fk_Movimiento_Inventario_Codigo
                 AND m.estadoMovimientoInventario = 'CERRADO') THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Detalle de documento cerrado: no se puede agregar';
    END IF;
END$$
DELIMITER ;


-- Verificar los 3 procedimientos y 5 triggers:
SELECT ROUTINE_NAME, ROUTINE_TYPE
FROM information_schema.ROUTINES
WHERE ROUTINE_SCHEMA = DATABASE()
  AND ROUTINE_NAME IN (
    'spGenerarSolicitudPagoTransporte',
    'spRegistrarServicioTransporteExterno',
    'spEjecutarCierreInventario'
  );

SELECT TRIGGER_NAME, EVENT_MANIPULATION, EVENT_OBJECT_TABLE
FROM information_schema.TRIGGERS
WHERE TRIGGER_SCHEMA = DATABASE()
  AND TRIGGER_NAME IN (
    'trMovimientoInventarioBeforeUpdateCerrado',
    'trMovimientoInventarioBeforeDeleteCerrado',
    'trDetalleMovimientoBeforeUpdateCerrado',
    'trDetalleMovimientoBeforeDeleteCerrado',
    'trDetalleMovimientoBeforeInsertCerrado'
  );

