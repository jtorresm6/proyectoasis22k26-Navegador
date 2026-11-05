-- ============================================================================
-- TERMINUS ERP - MODULO DE LOGISTICA
-- ESTANDARIZACION DE BASE DE DATOS EST-01 V1.4
-- 43 TABLAS;
-- REVISION 03/11/2026
-- ============================================================================

CREATE DATABASE dbLogistica
    CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE dbLogistica;

/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
CREATE TABLE tblEmpresaTransportista (
idEmpresaTransportista INT NOT NULL AUTO_INCREMENT,
 nombreEmpresaTransportista VARCHAR(100) NOT NULL,
 contactoEmpresaTransportista VARCHAR(100) NOT NULL,
 telefonoEmpresaTransportista VARCHAR(30) COLLATE utf8mb4_unicode_ci NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqEmpresaTransportistaNombre UNIQUE (nombreEmpresaTransportista),
    CONSTRAINT Pk_EmpresaTransportista PRIMARY KEY (idEmpresaTransportista)
) ENGINE=InnoDB;

/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblTipoServicio
CREATE TABLE tblTipoServicio (
idTipoServicio INT NOT NULL AUTO_INCREMENT,
 nombreTipoServicio VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqTipoServicioNombre UNIQUE (nombreTipoServicio),
    CONSTRAINT Pk_TipoServicio PRIMARY KEY (idTipoServicio)
) ENGINE=InnoDB;


/* 
--	Responsable:
--	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblTarifaTransporte
CREATE TABLE tblTarifaTransporte (
idTarifaTransporte INT NOT NULL AUTO_INCREMENT,
 nombreTarifaTransporte VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqTarifaTransporteNombre UNIQUE (nombreTarifaTransporte),
    CONSTRAINT Pk_TarifaTransporte PRIMARY KEY (idTarifaTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblEstado
CREATE TABLE tblEstado (
idEstado INT AUTO_INCREMENT NOT NULL,
    tipoEstado VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Estado PRIMARY KEY (idEstado),
    CONSTRAINT uqTipoEstado UNIQUE (tipoEstado)
) ENGINE=InnoDB;

/* 
	Responsable:
	BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblTipoTransporte
CREATE TABLE tblTipoTransporte (
idTipoTransporte INT AUTO_INCREMENT NOT NULL,
    nombreTipoTransporte VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_TipoTransporte PRIMARY KEY (idTipoTransporte),
    CONSTRAINT uqNombreTipoTransporte UNIQUE (nombreTipoTransporte)
) ENGINE=InnoDB;

/* 
	Responsable:
	BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblTransporte
CREATE TABLE tblTransporte (
idTransporte INT AUTO_INCREMENT NOT NULL,
    nroPlacaTransporte VARCHAR(30) NOT NULL,
    marcaTransporte VARCHAR(100) NOT NULL,
    modeloTransporte VARCHAR(100) NOT NULL,
    anioTransporte SMALLINT NOT NULL,
    esRefrigeradoTransporte BOOLEAN NOT NULL DEFAULT FALSE,
    Fk_Estado_Codigo INT NOT NULL,
    Fk_Tipo_Transporte_Codigo INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Transporte PRIMARY KEY (idTransporte),
    CONSTRAINT uqNroPlacaTransporte UNIQUE (nroPlacaTransporte),
    INDEX idxIdEstadoTransporte (Fk_Estado_Codigo),
    INDEX idxIdTipoTransporteTransporte (Fk_Tipo_Transporte_Codigo),
    CONSTRAINT Fk_Transporte_Estado FOREIGN KEY (Fk_Estado_Codigo) REFERENCES tblEstado(idEstado) ON DELETE RESTRICT,
    CONSTRAINT Fk_Transporte_Tipo_Transporte FOREIGN KEY (Fk_Tipo_Transporte_Codigo) REFERENCES tblTipoTransporte(idTipoTransporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

/* 
	Responsable:
	BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblCamaraRefrigeracion
CREATE TABLE tblCamaraRefrigeracion (
idCamaraRefrigeracion INT AUTO_INCREMENT NOT NULL,
    Fk_Transporte_Codigo INT NOT NULL,
    marcaEquipoCamaraRefrigeracion VARCHAR(100) NOT NULL,
    tempMinimaCelsiusCamaraRefrigeracion DECIMAL(5,2) NOT NULL,
    tempMaximaCelsiusCamaraRefrigeracion DECIMAL(5,2) NOT NULL,
    estadoOperativoCamaraRefrigeracion VARCHAR(30) NOT NULL DEFAULT 'OPERATIVO',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_CamaraRefrigeracion PRIMARY KEY (idCamaraRefrigeracion),
    CONSTRAINT uqIdTransporteCamaraRefrigeracion UNIQUE (Fk_Transporte_Codigo),
    CONSTRAINT Fk_Camara_Refrigeracion_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(idTransporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

/* 
	Responsable:
	BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblConductor
CREATE TABLE tblConductor (
idConductor INT AUTO_INCREMENT NOT NULL,
    codigoConductor VARCHAR(30) NOT NULL,
    nombreCompletoConductor VARCHAR(100) NOT NULL,
    numeroLicenciaConductor VARCHAR(30) NOT NULL,
    fechaVencimientoDocConductor DATE NOT NULL,
    Fk_Estado_Codigo INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Conductor PRIMARY KEY (idConductor),
    CONSTRAINT uqCodigoConductor UNIQUE (codigoConductor),
    CONSTRAINT uqNumeroLicenciaConductor UNIQUE (numeroLicenciaConductor),
    INDEX idxIdEstadoConductor (Fk_Estado_Codigo),
    CONSTRAINT Fk_Conductor_Estado FOREIGN KEY (Fk_Estado_Codigo) REFERENCES tblEstado(idEstado) ON DELETE RESTRICT
) ENGINE=InnoDB;


/*
	Responsable:
    BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblEncabezadoViaje
CREATE TABLE tblEncabezadoViaje (
idEncabezadoViaje BIGINT AUTO_INCREMENT NOT NULL,
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
    CONSTRAINT Pk_EncabezadoViaje PRIMARY KEY (idEncabezadoViaje),
    INDEX idxIdTransporteEncabezadoViaje (Fk_Transporte_Codigo),
    INDEX idxIdConductorEncabezadoViaje (Fk_Conductor_Codigo),
    INDEX idxEstadoViajeEncabezadoViaje (estadoViajeEncabezadoViaje),
    CONSTRAINT Fk_Encabezado_Viaje_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(idTransporte) ON DELETE RESTRICT,
    CONSTRAINT Fk_Encabezado_Viaje_Conductor FOREIGN KEY (Fk_Conductor_Codigo) REFERENCES tblConductor(idConductor) ON DELETE RESTRICT
) ENGINE=InnoDB;


/*
	Responsable:
    BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblDetalleViaje
CREATE TABLE tblDetalleViaje (
idDetalleViaje BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Encabezado_Viaje_Codigo BIGINT NOT NULL,
    ordenDetalleViaje INT NOT NULL,
    descripcionDetalleViaje VARCHAR(255) NOT NULL,
    cantidadDetalleViaje DECIMAL(18,3) NOT NULL,
    pesoDetalleViaje DECIMAL(18,3) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_DetalleViaje PRIMARY KEY (idDetalleViaje),
    CONSTRAINT uqOrdenDetalleViaje UNIQUE (Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje),
    CONSTRAINT Fk_Detalle_Viaje_Encabezado_Viaje FOREIGN KEY (Fk_Encabezado_Viaje_Codigo) REFERENCES tblEncabezadoViaje(idEncabezadoViaje) ON DELETE CASCADE
) ENGINE=InnoDB;


/*
	Responsable:
    BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblTelemetriaTemperatura
CREATE TABLE tblTelemetriaTemperatura (
idTelemetriaTemperatura BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Encabezado_Viaje_Codigo BIGINT NOT NULL,
    fechaHoraTelemetriaTemperatura DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    temperaturaCelsiusTelemetriaTemperatura DECIMAL(5,2) NOT NULL,
    /* Porcentaje en formato decimal: 0.6500 = 65 % (DECIMAL(5,4) permite llegar a 100 %) */
    humedadRelativaTelemetriaTemperatura DECIMAL(5,4) NOT NULL,
    puertaAbiertaTelemetriaTemperatura BOOLEAN NOT NULL DEFAULT FALSE,
    ubicacionGpsTelemetriaTemperatura VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_TelemetriaTemperatura PRIMARY KEY (idTelemetriaTemperatura),
    INDEX idxCorrelativoFechaTelemetriaTemperatura (Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura),
    CONSTRAINT Fk_Telemetria_Temperatura_Encabezado_Viaje FOREIGN KEY (Fk_Encabezado_Viaje_Codigo) REFERENCES tblEncabezadoViaje(idEncabezadoViaje) ON DELETE CASCADE
) ENGINE=InnoDB;


/*
	Responsable:
    BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblAlertaCadenaFrio
CREATE TABLE tblAlertaCadenaFrio (
idAlertaCadenaFrio BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Telemetria_Temperatura_Codigo BIGINT NOT NULL,
    tipoAlertaCadenaFrio VARCHAR(30) NOT NULL,
    -- El viaje y la temperatura se consultan desde tblTelemetriaTemperatura.
    atendidaAlertaCadenaFrio BOOLEAN NOT NULL DEFAULT FALSE,
    observacionCalidadAlertaCadenaFrio VARCHAR(500) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_AlertaCadenaFrio PRIMARY KEY (idAlertaCadenaFrio),
    INDEX idxIdTelemetriaTemperaturaAlertaCadenaFrio (Fk_Telemetria_Temperatura_Codigo),
    INDEX idxAtendidaAlertaCadenaFrio (atendidaAlertaCadenaFrio),
    CONSTRAINT Fk_Alerta_Cadena_Frio_Telemetria_Temperatura FOREIGN KEY (Fk_Telemetria_Temperatura_Codigo) REFERENCES tblTelemetriaTemperatura(idTelemetriaTemperatura) ON DELETE CASCADE
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblSucursal
CREATE TABLE tblSucursal (
idSucursal INT NOT NULL AUTO_INCREMENT,
    nombreSucursal VARCHAR(100) NOT NULL,
    descripcionSucursal VARCHAR(255) NOT NULL,
    ubicacionSucursal VARCHAR(500) NOT NULL,
    paisSucursal VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Sucursal PRIMARY KEY (idSucursal)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblBodega
CREATE TABLE tblBodega (
idBodega INT NOT NULL AUTO_INCREMENT,
    nombreBodega VARCHAR(100) NOT NULL,
    Fk_Sucursal_Codigo INT NOT NULL,
    paisBodega VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	
    UNIQUE KEY uqBodegaSucursal (idBodega,Fk_Sucursal_Codigo),
    CONSTRAINT Fk_Bodega_Sucursal FOREIGN KEY (Fk_Sucursal_Codigo) REFERENCES tblSucursal(idSucursal) ON DELETE RESTRICT,
    CONSTRAINT Pk_Bodega PRIMARY KEY (idBodega)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblCategoria
CREATE TABLE tblCategoria (
idCategoria INT NOT NULL AUTO_INCREMENT,
    nombreCategoria VARCHAR(100) NOT NULL,
    porcentajeComisionCategoria DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqCategoriaNombre UNIQUE (nombreCategoria),
    CONSTRAINT ckPorcentajeComisionCategoriaRango CHECK (porcentajeComisionCategoria >= 0.00 AND porcentajeComisionCategoria <= 0.99),
    CONSTRAINT Pk_Categoria PRIMARY KEY (idCategoria)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblLinea
CREATE TABLE tblLinea (
idLinea INT NOT NULL AUTO_INCREMENT,
    nombreLinea VARCHAR(100) NOT NULL,
    porcentajeComisionLinea DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqLineaNombre UNIQUE (nombreLinea),
    CONSTRAINT ckPorcentajeComisionLineaRango CHECK (porcentajeComisionLinea >= 0.00 AND porcentajeComisionLinea <= 0.99),
    CONSTRAINT Pk_Linea PRIMARY KEY (idLinea)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblMarca
CREATE TABLE tblMarca (
idMarca INT NOT NULL AUTO_INCREMENT,
    nombreMarca VARCHAR(100) NOT NULL,
    porcentajeComisionMarca DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqMarcaNombre UNIQUE (nombreMarca),
    CONSTRAINT ckPorcentajeComisionMarcaRango CHECK (porcentajeComisionMarca >= 0.00 AND porcentajeComisionMarca <= 0.99),
    CONSTRAINT Pk_Marca PRIMARY KEY (idMarca)
) ENGINE=InnoDB;

-- tblProducto
CREATE TABLE tblProducto (
idProducto INT NOT NULL AUTO_INCREMENT,
    skuProducto VARCHAR(30) NOT NULL,
    clasificacionProducto CHAR(5) NOT NULL DEFAULT '?????',
    nombreProducto VARCHAR(100) NOT NULL,
    idProveedorProductos INT NOT NULL,
    Fk_Marca_Codigo INT NOT NULL,
    Fk_Categoria_Codigo INT NOT NULL,
    Fk_Linea_Codigo INT NOT NULL,
    descripcionProducto VARCHAR(255) NOT NULL,
    imagenProducto LONGBLOB NOT NULL, -- Campo BLOB para almacenar la imagen (Método de conversión de Img a binario en aplicación)
    paisProducto VARCHAR(100) NOT NULL,
    precioCostoProducto DECIMAL(12,4) NOT NULL,
    precioVentaProducto DECIMAL(12,4) NOT NULL,
    porcentajeComisionProducto DECIMAL(3,2) NOT NULL,
    -- Los saldos y movimientos no se almacenan aqui: se obtienen de tblStock.
    stockMinimoProducto INT NOT NULL ,
    stockMaximoProducto INT NOT NULL ,
    pesoProducto DECIMAL(4,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT uqProductoSku UNIQUE (skuProducto),
    INDEX idxProductoProveedorProductos (idProveedorProductos),
    CONSTRAINT Fk_Producto_Proveedor FOREIGN KEY (idProveedorProductos) REFERENCES tblProveedor(idProveedor) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Marca FOREIGN KEY (Fk_Marca_Codigo) REFERENCES tblMarca(idMarca) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Categoria FOREIGN KEY (Fk_Categoria_Codigo) REFERENCES tblCategoria(idCategoria) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Linea FOREIGN KEY (Fk_Linea_Codigo) REFERENCES tblLinea(idLinea) ON DELETE RESTRICT,
    CONSTRAINT ckPorcentajeComisionProductoRango CHECK (porcentajeComisionProducto >= 0.00 AND porcentajeComisionProducto <= 0.99),
    CONSTRAINT ckProductoMinMax CHECK (stockMinimoProducto >= 0 AND stockMaximoProducto >= stockMinimoProducto),
    CONSTRAINT Pk_Producto PRIMARY KEY (idProducto)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblLoteProducto
CREATE TABLE tblLoteProducto (
idLoteProducto BIGINT AUTO_INCREMENT NOT NULL,
    codigoLoteProducto VARCHAR(30) NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    descripcionLoteProducto VARCHAR(255) NOT NULL,
    requiereFrioLoteProducto BOOLEAN NOT NULL DEFAULT TRUE,
    fechaFabricacionLoteProducto DATE NOT NULL,
    fechaVencimientoLoteProducto DATE NOT NULL,
    tempRequeridaMinLoteProducto DECIMAL(5,2) NOT NULL,
    tempRequeridaMaxLoteProducto DECIMAL(5,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_LoteProducto PRIMARY KEY (idLoteProducto),
    CONSTRAINT uqCodigoLoteProducto UNIQUE (codigoLoteProducto),
    UNIQUE KEY uqLoteProductoRelacion (idLoteProducto, Fk_Producto_Codigo),
    INDEX idxLoteProducto (Fk_Producto_Codigo),
    CONSTRAINT Fk_Lote_Producto_Producto FOREIGN KEY (Fk_Producto_Codigo)
        REFERENCES tblProducto(idProducto) ON DELETE RESTRICT
) ENGINE=InnoDB;


/*
	Responsable:
    BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblDetalleViajeLoteProducto
CREATE TABLE tblDetalleViajeLoteProducto (
idDetalleViajeLoteProducto BIGINT NOT NULL AUTO_INCREMENT,
    Fk_Detalle_Viaje_Codigo BIGINT NOT NULL,
    Fk_Lote_Producto_Codigo BIGINT NOT NULL,
    cantidadCajasDetalleViajeLoteProducto INT NOT NULL,
    pesoKgDetalleViajeLoteProducto DECIMAL(18,3) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_DetalleViajeLoteProducto PRIMARY KEY (idDetalleViajeLoteProducto),
    CONSTRAINT uqDetalleViajeLoteProducto UNIQUE (Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo),
    INDEX idxIdLoteProductoDetalleViajeLoteProducto (Fk_Lote_Producto_Codigo),
    CONSTRAINT Fk_Detalle_Viaje_Lote_Producto_Detalle_Viaje FOREIGN KEY (Fk_Detalle_Viaje_Codigo) REFERENCES tblDetalleViaje(idDetalleViaje) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Viaje_Lote_Producto_Lote_Producto FOREIGN KEY (Fk_Lote_Producto_Codigo) REFERENCES tblLoteProducto(idLoteProducto) ON DELETE RESTRICT
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblStock: fuente unica de saldo actual por bodega y producto.
-- Se mantiene Fk_Sucursal_Codigo para compatibilidad con MVC y FK compuesta a tblBodega.
-- stockFisicoStock: ultimo conteo fisico observado.
-- stockVirtualStock y stockActualStock se mantienen por compatibilidad; la
-- aplicacion MVC debe mantenerlos consistentes segun las reglas del negocio.
CREATE TABLE tblStock (
idStock INT AUTO_INCREMENT NOT NULL,
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
    apartadosStock INT NOT NULL DEFAULT 0,

    estadoVentaStock BOOLEAN NOT NULL,
    estadoCompraStock BOOLEAN NOT NULL,

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_Stock PRIMARY KEY (idStock),
    CONSTRAINT uqStockSucursalBodegaProducto UNIQUE (Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo),

    CONSTRAINT Fk_Stock_Bodega FOREIGN KEY (Fk_Bodega_Codigo, Fk_Sucursal_Codigo)
    REFERENCES tblBodega(idBodega, Fk_Sucursal_Codigo) ON DELETE RESTRICT,

    CONSTRAINT Fk_Stock_Producto FOREIGN KEY (Fk_Producto_Codigo)
    REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT ckEstadoVentaStock CHECK (estadoVentaStock IN (0, 1)),
    CONSTRAINT ckEstadoCompraStock CHECK (estadoCompraStock IN (0, 1)),
    CONSTRAINT ckStockActualNoNegativo CHECK (stockActualStock >= 0)
) ENGINE=InnoDB;


-- tblSaldos
-- Relacion asociativa entre productos y bodegas.
-- Un producto tiene un unico saldo por bodega.
-- tblStock.stockActualStock es la referencia del saldo actual;
-- la aplicacion MVC debe sincronizar tblSaldos sin triggers ni procedimientos.
CREATE TABLE tblSaldos (
    idSaldo INT NOT NULL AUTO_INCREMENT,
    Fk_Producto_Codigo INT NOT NULL,
    Fk_Bodega_Codigo INT NOT NULL,
    saldoActualSaldo INT NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Saldos PRIMARY KEY (idSaldo),
    CONSTRAINT uqSaldoProductoBodega UNIQUE (Fk_Producto_Codigo, Fk_Bodega_Codigo),
    CONSTRAINT Fk_Saldos_Producto FOREIGN KEY (Fk_Producto_Codigo)
        REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT Fk_Saldos_Bodega FOREIGN KEY (Fk_Bodega_Codigo)
        REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,
    CONSTRAINT ckSaldoActualNoNegativo CHECK (saldoActualSaldo >= 0)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblTipoMovimiento
CREATE TABLE tblTipoMovimiento (
idTipoMovimiento INT NOT NULL AUTO_INCREMENT,
    nombreTipoMovimiento VARCHAR(100) NOT NULL,
    descripcionTipoMovimiento VARCHAR(255) NOT NULL,
	signoTipoMovimiento CHAR(1) NOT NULL,

	created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqTipoMovimientoNombre UNIQUE (nombreTipoMovimiento),
    CONSTRAINT ckSignoTipoMovimiento CHECK (signoTipoMovimiento IN ('+', '-')),
    CONSTRAINT Pk_TipoMovimiento PRIMARY KEY (idTipoMovimiento)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	Jose Xavier Bolaños TENAS 0901 23 2128
 */ 
-- tblTraslado
CREATE TABLE tblTraslado (
idTraslado INT NOT NULL AUTO_INCREMENT,
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

    CONSTRAINT Pk_Traslado PRIMARY KEY (idTraslado),
    UNIQUE KEY uqCodigoTraslado (codigoTraslado),
    CONSTRAINT Fk_Traslado_Bodega_Origen FOREIGN KEY (Fk_Bodega_Origen_Codigo) REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,
    CONSTRAINT Fk_Traslado_Bodega_Destino FOREIGN KEY (Fk_Bodega_Destino_Codigo) REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,
    CONSTRAINT Fk_Traslado_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(idTransporte) ON DELETE RESTRICT,
    CONSTRAINT ckTrasladoBodegasDistintas CHECK (Fk_Bodega_Origen_Codigo <> Fk_Bodega_Destino_Codigo) -- Origen y destino no pueden ser la misma bodega
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	Jose Xavier Bolaños TENAS 0901 23 2128
 */ 
-- tblDetalleTraslado
CREATE TABLE tblDetalleTraslado (
idDetalleTraslado INT NOT NULL AUTO_INCREMENT,
    Fk_Traslado_Codigo INT NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    cantidadDetalleTraslado INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DetalleTraslado PRIMARY KEY (idDetalleTraslado),
    CONSTRAINT uqDetalleTrasladoProducto UNIQUE (Fk_Traslado_Codigo, Fk_Producto_Codigo),
    CONSTRAINT Fk_Detalle_Traslado_Traslado FOREIGN KEY (Fk_Traslado_Codigo) REFERENCES tblTraslado(idTraslado) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Traslado_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT ckDetalleTrasladoCantidad CHECK (cantidadDetalleTraslado > 0)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	NILLS BERDUCIDO GOMEZ 0901 23 14275
*/ 
-- tblCierreMovimiento
CREATE TABLE tblCierreMovimiento (
idCierreMovimiento INT NOT NULL AUTO_INCREMENT,
    nombreCierreMovimiento VARCHAR(100) NOT NULL,
    Fk_Sucursal_Codigo INT NOT NULL,
    fechaInicioCierreMovimiento DATE NOT NULL,
    fechaFinCierreMovimiento DATE NOT NULL,
    estadoCierreMovimiento ENUM('ABIERTO', 'EN_PROCESO', 'CERRADO') NOT NULL DEFAULT 'ABIERTO',
    esCierreAnualCierreMovimiento BOOLEAN NOT NULL DEFAULT FALSE,
    toleranciaCierreMovimiento INT NOT NULL DEFAULT 0,
    diferenciaUnidadesCierreMovimiento INT NOT NULL DEFAULT 0,
    diferenciaMontoCierreMovimiento DECIMAL(12,4) NOT NULL DEFAULT 0,
    motivoAceptacionCierreMovimiento VARCHAR(500) NOT NULL,
    -- El cierre ejecutado no puede anularse; por eso no se guarda motivoAnulacion.
    observacionesCierreMovimiento VARCHAR(500) NOT NULL,
    fechaEjecucionCierreMovimiento DATETIME NULL,
    CONSTRAINT uqCierreMovimientoSucursalFin UNIQUE (Fk_Sucursal_Codigo, fechaFinCierreMovimiento, esCierreAnualCierreMovimiento),
    CONSTRAINT ckCierreMovimientoFechas CHECK (fechaInicioCierreMovimiento <= fechaFinCierreMovimiento),
    CONSTRAINT ckCierreEjecucion CHECK (estadoCierreMovimiento <> 'CERRADO' OR fechaEjecucionCierreMovimiento IS NOT NULL),
    CONSTRAINT Fk_Cierre_Movimiento_Sucursal FOREIGN KEY (Fk_Sucursal_Codigo)
        REFERENCES tblSucursal(idSucursal) ON DELETE RESTRICT,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_CierreMovimiento PRIMARY KEY (idCierreMovimiento)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblMovimientoInventario
CREATE TABLE tblMovimientoInventario (
idMovimientoInventario INT NOT NULL AUTO_INCREMENT,
    Fk_Tipo_Movimiento_Codigo INT NOT NULL,
-- agregar sucursales destino y origen 
    Fk_Bodega_Origen_Codigo INT NULL,
    Fk_Bodega_Destino_Codigo INT NULL,
    Fk_Transporte_Codigo INT NULL,
    Fk_Traslado_Codigo INT NULL COMMENT 'Relaciona movimientos de entrada/salida con su traslado',
    fechaMovimientoInventario DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    observacionesMovimientoInventario VARCHAR(500) NOT NULL,
    estadoMovimientoInventario ENUM('PENDIENTE', 'CUADRADO', 'CERRADO', 'ANULADO') NOT NULL DEFAULT 'PENDIENTE',
    Fk_Cierre_Movimiento_Codigo INT NULL COMMENT 'FK al cierre; permite NULL mientras el movimiento no se haya cerrado', 
    
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Fk_Movimiento_Inventario_Tipo_Movimiento FOREIGN KEY (Fk_Tipo_Movimiento_Codigo)
	REFERENCES tblTipoMovimiento(idTipoMovimiento) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Bodega_Origen FOREIGN KEY (Fk_Bodega_Origen_Codigo)
    REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Bodega_Destino FOREIGN KEY (Fk_Bodega_Destino_Codigo)
    REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Transporte FOREIGN KEY (Fk_Transporte_Codigo)
    REFERENCES tblTransporte(idTransporte) ON DELETE RESTRICT,
    CONSTRAINT Fk_Movimiento_Inventario_Traslado FOREIGN KEY (Fk_Traslado_Codigo)
        REFERENCES tblTraslado(idTraslado) ON DELETE RESTRICT,
    CONSTRAINT Fk_Movimiento_Inventario_Cierre_Movimiento FOREIGN KEY (Fk_Cierre_Movimiento_Codigo)
        REFERENCES tblCierreMovimiento(idCierreMovimiento) ON DELETE RESTRICT,
    CONSTRAINT Pk_MovimientoInventario PRIMARY KEY (idMovimientoInventario)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblDetalleMovimiento
CREATE TABLE tblDetalleMovimiento (
idDetalleMovimiento INT NOT NULL AUTO_INCREMENT,
    Fk_Movimiento_Inventario_Codigo INT NOT NULL,
    Fk_Lote_Producto_Codigo BIGINT NULL COMMENT 'Opcional cuando un producto aun no tiene lote',
    Fk_Producto_Codigo INT NOT NULL,
    cantidadDetalleMovimiento INT NOT NULL,
    precioCostoDetalleMovimiento DECIMAL(12,4) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Fk_Detalle_Movimiento_Movimiento_Inventario FOREIGN KEY (Fk_Movimiento_Inventario_Codigo)
    REFERENCES tblMovimientoInventario(idMovimientoInventario) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Movimiento_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT Fk_Detalle_Movimiento_Lote_Producto FOREIGN KEY (Fk_Lote_Producto_Codigo, Fk_Producto_Codigo)
        REFERENCES tblLoteProducto(idLoteProducto, Fk_Producto_Codigo) ON DELETE RESTRICT,
    CONSTRAINT ckDetalleMovimientoCantidad CHECK (cantidadDetalleMovimiento > 0),
    CONSTRAINT Pk_DetalleMovimiento PRIMARY KEY (idDetalleMovimiento)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	CARLOS EDUARDO CHIN CAAL 0901 23 5686
*/ 
-- tblAuditoriaInventario
CREATE TABLE tblAuditoriaInventario (
idAuditoriaInventario INT NOT NULL AUTO_INCREMENT,
    Fk_Movimiento_Inventario_Codigo INT NULL COMMENT 'Opcional: una auditoria puede realizarse sin movimiento previo',
    Fk_Bodega_Codigo INT NOT NULL,
    tipoAuditoriaInventario ENUM('INDIVIDUAL', 'MULTIPLE') NOT NULL,
    estadoAuditoriaInventario ENUM('EN_PROCESO', 'FINALIZADA') NOT NULL DEFAULT 'EN_PROCESO',
    fechaAuditoriaInventario DATETIME NOT NULL,
    resultadoAuditoriaInventario VARCHAR(100) NOT NULL,
    observacionesCalidadAuditoriaInventario TEXT NOT NULL, -- Observaciones generales de la auditoria
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Fk_Auditoria_Inventario_Movimiento_Inventario FOREIGN KEY (Fk_Movimiento_Inventario_Codigo) REFERENCES tblMovimientoInventario(idMovimientoInventario) ON DELETE RESTRICT,
    CONSTRAINT Fk_Auditoria_Inventario_Bodega FOREIGN KEY (Fk_Bodega_Codigo) REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,
    CONSTRAINT Pk_AuditoriaInventario PRIMARY KEY (idAuditoriaInventario)
) ENGINE=InnoDB;


-- tblDetalleAuditoria: conteo individualizado, sin repetir los detalles como texto.
CREATE TABLE tblDetalleAuditoria (
    idDetalleAuditoria INT NOT NULL AUTO_INCREMENT,
    Fk_Auditoria_Inventario_Codigo INT NOT NULL,
    Fk_Stock_Codigo INT NOT NULL,
    stockTeoricoDetalleAuditoria INT NOT NULL,
    stockFisicoDetalleAuditoria INT NOT NULL,
    diferenciaDetalleAuditoria INT GENERATED ALWAYS AS
        (stockFisicoDetalleAuditoria - stockTeoricoDetalleAuditoria) STORED NOT NULL,
    observacionesDetalleAuditoria VARCHAR(500) NOT NULL DEFAULT '',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqDetalleAuditoriaStock UNIQUE (Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo),
    CONSTRAINT Fk_Detalle_Auditoria_Auditoria_Inventario FOREIGN KEY (Fk_Auditoria_Inventario_Codigo)
        REFERENCES tblAuditoriaInventario(idAuditoriaInventario) ON DELETE RESTRICT,
    CONSTRAINT Fk_Detalle_Auditoria_Stock FOREIGN KEY (Fk_Stock_Codigo)
        REFERENCES tblStock(idStock) ON DELETE RESTRICT,
    CONSTRAINT ckDetalleAuditoriaStockTeorico CHECK (stockTeoricoDetalleAuditoria >= 0),
    CONSTRAINT ckDetalleAuditoriaStockFisico CHECK (stockFisicoDetalleAuditoria >= 0),
    CONSTRAINT Pk_DetalleAuditoria PRIMARY KEY (idDetalleAuditoria)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	NILLS BERDUCIDO GOMEZ 0901 23 14275
*/ 
-- tblDetalleCierre
CREATE TABLE tblDetalleCierre (
idDetalleCierre INT NOT NULL AUTO_INCREMENT,
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
    CONSTRAINT Fk_Detalle_Cierre_Cierre_Movimiento FOREIGN KEY (Fk_Cierre_Movimiento_Codigo) REFERENCES tblCierreMovimiento(idCierreMovimiento) ON DELETE RESTRICT,
    CONSTRAINT Fk_Detalle_Cierre_Stock FOREIGN KEY (Fk_Stock_Codigo) REFERENCES tblStock(idStock) ON DELETE RESTRICT,
    CONSTRAINT uqDetalleCierreStock UNIQUE (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo),
    CONSTRAINT Pk_DetalleCierre PRIMARY KEY (idDetalleCierre)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901 23 6269
*/ 
-- tblHistoricoInventario: copia permanente de los valores de tblStock.
-- Por cada cierre se guarda una instantanea del stock, con los mismos campos
-- de cantidades y estados de tblStock. No se actualiza al cambiar tblStock.
-- Fk_Stock_Codigo conserva el idStock original; idHistoricoInventario es PK.
-- No modificar ni eliminar instantaneas desde la aplicacion luego del cierre.
CREATE TABLE tblHistoricoInventario (
    idHistoricoInventario BIGINT NOT NULL AUTO_INCREMENT,
    Fk_Cierre_Movimiento_Codigo INT NOT NULL,
    Fk_Stock_Codigo INT NOT NULL,
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
    apartadosStock INT NOT NULL DEFAULT 0,
    estadoVentaStock BOOLEAN NOT NULL,
    estadoCompraStock BOOLEAN NOT NULL,

    fechaCorteHistoricoInventario DATE NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_HistoricoInventario PRIMARY KEY (idHistoricoInventario),
    CONSTRAINT uqHistoricoInventarioCierreStock UNIQUE (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo),
    CONSTRAINT Fk_Historico_Inventario_Cierre_Movimiento
        FOREIGN KEY (Fk_Cierre_Movimiento_Codigo)
        REFERENCES tblCierreMovimiento(idCierreMovimiento) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Stock
        FOREIGN KEY (Fk_Stock_Codigo)
        REFERENCES tblStock(idStock) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Producto
        FOREIGN KEY (Fk_Producto_Codigo)
        REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Bodega
        FOREIGN KEY (Fk_Bodega_Codigo, Fk_Sucursal_Codigo)
        REFERENCES tblBodega(idBodega, Fk_Sucursal_Codigo) ON DELETE RESTRICT,
    CONSTRAINT ckHistoricoStockActualNoNegativo CHECK (stockActualStock >= 0),
    CONSTRAINT ckHistoricoEstadoVenta CHECK (estadoVentaStock IN (0, 1)),
    CONSTRAINT ckHistoricoEstadoCompra CHECK (estadoCompraStock IN (0, 1))
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblServicioTransporteExterno
CREATE TABLE tblServicioTransporteExterno (
idServicioTransporteExterno INT NOT NULL AUTO_INCREMENT,
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
 CONSTRAINT Fk_Servicio_Transporte_Externo_Empresa_Transportista FOREIGN KEY (Fk_Empresa_Transportista_Codigo) REFERENCES tblEmpresaTransportista(idEmpresaTransportista) ON DELETE RESTRICT,
 CONSTRAINT Fk_Servicio_Transporte_Externo_Tipo_Servicio FOREIGN KEY (Fk_Tipo_Servicio_Codigo) REFERENCES tblTipoServicio(idTipoServicio) ON DELETE RESTRICT,
 CONSTRAINT Fk_Servicio_Transporte_Externo_Traslado FOREIGN KEY (Fk_Traslado_Codigo) REFERENCES tblTraslado(idTraslado) ON DELETE RESTRICT,
    CONSTRAINT ckServicioEstado CHECK (estadoServicioTransporteExterno IN ('Programado','En tránsito','Entregado','Cancelado')),
    CONSTRAINT ckServicioFechas CHECK (fechaEstimadaEntregaServicioTransporteExterno >= fechaContratacionServicioTransporteExterno),
    CONSTRAINT Pk_ServicioTransporteExterno PRIMARY KEY (idServicioTransporteExterno)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblCostoTransporte
CREATE TABLE tblCostoTransporte (
idCostoTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 Fk_Tarifa_Transporte_Codigo INT NOT NULL,
 cantidadCostoTransporte DECIMAL(12,4) NOT NULL,
 costoUnitarioCostoTransporte DECIMAL(12,4) NOT NULL,
 costoTotalCostoTransporte DECIMAL(12,4) GENERATED ALWAYS AS
     (ROUND(cantidadCostoTransporte * costoUnitarioCostoTransporte, 4)) STORED NOT NULL,
 monedaCostoTransporte CHAR(3) NOT NULL,
 tipoPagoCostoTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqCostoServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Costo_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
 CONSTRAINT Fk_Costo_Transporte_Tarifa_Transporte FOREIGN KEY (Fk_Tarifa_Transporte_Codigo) REFERENCES tblTarifaTransporte(idTarifaTransporte) ON DELETE RESTRICT,
    CONSTRAINT ckCostoCantidad CHECK (cantidadCostoTransporte > 0),
    CONSTRAINT ckCostoUnitario CHECK (costoUnitarioCostoTransporte >= 0),
    CONSTRAINT ckCostoMoneda CHECK (monedaCostoTransporte IN ('GTQ','USD')),
    CONSTRAINT ckCostoTipoPago CHECK (tipoPagoCostoTransporte IN ('Transferencia','Efectivo')),
    CONSTRAINT Pk_CostoTransporte PRIMARY KEY (idCostoTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblVehiculoTransporte
CREATE TABLE tblVehiculoTransporte (
idVehiculoTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 placaVehiculoTransporte VARCHAR(30) NOT NULL,
 tipoVehiculoTransporte VARCHAR(100) NOT NULL,
 capacidadVehiculoTransporte VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqVehiculoServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Vehiculo_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
    CONSTRAINT Pk_VehiculoTransporte PRIMARY KEY (idVehiculoTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblConductorTransporte
CREATE TABLE tblConductorTransporte (
idConductorTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 nombreConductorTransporte VARCHAR(100) NOT NULL,
 telefonoConductorTransporte VARCHAR(30) COLLATE utf8mb4_unicode_ci NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqConductorServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Conductor_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
    CONSTRAINT Pk_ConductorTransporte PRIMARY KEY (idConductorTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblProductoTransporte
CREATE TABLE tblProductoTransporte (
idProductoTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 Fk_Producto_Codigo INT NOT NULL,
 cantidadProductoTransporte DECIMAL(12,4) NOT NULL,
 unidadMedidaProductoTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Producto_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
 CONSTRAINT Fk_Producto_Transporte_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT ckProductoCantidad CHECK (cantidadProductoTransporte > 0),
    CONSTRAINT Pk_ProductoTransporte PRIMARY KEY (idProductoTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblUbicacionTransporte
CREATE TABLE tblUbicacionTransporte (
idUbicacionTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 ubicacionActualUbicacionTransporte VARCHAR(500) NOT NULL,
 fechaActualizacionUbicacionTransporte DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Ubicacion_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
 INDEX idxUbicacionServicioFecha (Fk_Servicio_Transporte_Externo_Codigo, fechaActualizacionUbicacionTransporte),
    CONSTRAINT Pk_UbicacionTransporte PRIMARY KEY (idUbicacionTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblIncidenciaTransporte
CREATE TABLE tblIncidenciaTransporte (
idIncidenciaTransporte INT NOT NULL AUTO_INCREMENT,
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
 CONSTRAINT Fk_Incidencia_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
    CONSTRAINT Pk_IncidenciaTransporte PRIMARY KEY (idIncidenciaTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblHistorialTransporte
CREATE TABLE tblHistorialTransporte (
idHistorialTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 fechaMovimientoHistorialTransporte DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
 tipoMovimientoHistorialTransporte VARCHAR(100) NOT NULL,
 descripcionMovimientoHistorialTransporte VARCHAR(255) NOT NULL,
 estadoTransporteHistorialTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Historial_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
    CONSTRAINT Pk_HistorialTransporte PRIMARY KEY (idHistorialTransporte)
) ENGINE=InnoDB;


-- ============================================================================
-- tblEstadoSolicitudPago
-- Responsable: BRAYAN MOISES PINZON LOPEZ (carne incompleto en solicitud).
-- Catalogo de estados para las solicitudes de pago de transporte.
-- ============================================================================
CREATE TABLE tblEstadoSolicitudPago (
    idEstadoSolicitudPago INT NOT NULL AUTO_INCREMENT,
    codigoEstadoSolicitudPago VARCHAR(30) NOT NULL,
    nombreEstadoSolicitudPago VARCHAR(100) NOT NULL,
    descripcionEstadoSolicitudPago VARCHAR(255) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_EstadoSolicitudPago PRIMARY KEY (idEstadoSolicitudPago),
    CONSTRAINT uqCodigoEstadoSolicitudPago UNIQUE (codigoEstadoSolicitudPago)
) ENGINE=InnoDB;

-- Estado inicial utilizado por el valor predeterminado de tblSolicitudPagoTransporte.
INSERT INTO tblEstadoSolicitudPago
    (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES
    (1, 'SOLICITADO', 'Solicitado', 'Solicitud de pago registrada');


/*
    Responsable:
    GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblSolicitudPagoTransporte
CREATE TABLE tblSolicitudPagoTransporte (
idSolicitudPagoTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 codigoSolicitudPagoTransporte VARCHAR(30) NOT NULL,
 Fk_Estado_Solicitud_Pago_Codigo INT NOT NULL DEFAULT 1,
 referenciaBancariaSolicitudPagoTransporte VARCHAR(100) NULL,
 fechaPagoSolicitudPagoTransporte DATE NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqSolicitudServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT uqSolicitudCodigo UNIQUE (codigoSolicitudPagoTransporte),
 INDEX idxSolicitudPagoEstado (Fk_Estado_Solicitud_Pago_Codigo),
 CONSTRAINT Fk_Solicitud_Pago_Transporte_Estado_Solicitud_Pago
     FOREIGN KEY (Fk_Estado_Solicitud_Pago_Codigo)
     REFERENCES tblEstadoSolicitudPago(idEstadoSolicitudPago) ON DELETE RESTRICT,
 CONSTRAINT Fk_Solicitud_Pago_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo)
  REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
    CONSTRAINT Pk_SolicitudPagoTransporte PRIMARY KEY (idSolicitudPagoTransporte)
) ENGINE=InnoDB;
