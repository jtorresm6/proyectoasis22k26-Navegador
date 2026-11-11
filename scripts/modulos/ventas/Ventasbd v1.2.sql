CREATE DATABASE IF NOT EXISTS dbventas
  DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE dbventas;
SET NAMES utf8mb4;
SET @ventas_fk_checks_anterior = @@FOREIGN_KEY_CHECKS;
SET FOREIGN_KEY_CHECKS = 0;

-- tblCorrelativoVenta | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7000
CREATE TABLE `tblCorrelativoVenta` (
  `idCorrelativoVenta` INT NOT NULL AUTO_INCREMENT,
  `prefijoCorrelativoVenta` VARCHAR(30) NOT NULL,
  `ultimoNumeroCorrelativoVenta` BIGINT NOT NULL DEFAULT 0,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CorrelativoVenta` PRIMARY KEY (`idCorrelativoVenta`),
  CONSTRAINT `ckCorrelativoVentaNumeroNoNegativo` CHECK (ultimoNumeroCorrelativoVenta >= 0),
  CONSTRAINT `uqCorrelativoVentaClaveNegocio` UNIQUE (`prefijoCorrelativoVenta`)
) ENGINE=InnoDB;

-- tblTipoCliente | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblTipoCliente` (
  `idTipoCliente` INT NOT NULL AUTO_INCREMENT,
  `codigoTipoCliente` VARCHAR(30) NOT NULL,
  `nombreTipoCliente` VARCHAR(100) NOT NULL,
  `porcentajeDescuentoTipoCliente` DECIMAL(5,2) NOT NULL DEFAULT 0.00,
  `descripcionTipoCliente` VARCHAR(255) NULL,
  `activoTipoCliente` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_TipoCliente` PRIMARY KEY (`idTipoCliente`),
  CONSTRAINT `uqTipoClientecodigo` UNIQUE (`codigoTipoCliente`),
  CONSTRAINT `uqTipoClientenombre` UNIQUE (`nombreTipoCliente`),
  CONSTRAINT `ckTipoClienteporcentajeDescuentoRango` CHECK (`porcentajeDescuentoTipoCliente` BETWEEN 0 AND 100),
  CONSTRAINT `ckTipoClienteactivoBooleano` CHECK (`activoTipoCliente` IN (0,1))
) ENGINE=InnoDB;

-- tblListaPrecioEncabezado | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblListaPrecioEncabezado` (
  `idListaPrecioEncabezado` INT NOT NULL AUTO_INCREMENT,
  `idTipoCliente` INT NOT NULL,
  `codigoListaPrecioEncabezado` VARCHAR(30) NOT NULL,
  `nombreListaPrecioEncabezado` VARCHAR(100) NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `vigenteDesdeListaPrecioEncabezado` DATE NOT NULL,
  `vigenteHastaListaPrecioEncabezado` DATE NULL,
  `activoListaPrecioEncabezado` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ListaPrecioEncabezado` PRIMARY KEY (`idListaPrecioEncabezado`),
  CONSTRAINT `uqListaPrecioEncabezadocodigo` UNIQUE (`codigoListaPrecioEncabezado`),
  KEY `idxListaPrecioEncabezadoTipoCliente` (`idTipoCliente`),
  KEY `idxListaPrecioEncabezadoMonedaCodigo` (`idMoneda`),
  CONSTRAINT `ckListaPrecioEncabezadoactivoListaPrecioEncabezadoBooleano` CHECK (activoListaPrecioEncabezado IN (0,1)),
  CONSTRAINT `ckListaPrecioEncabezadoFechasCoherentes` CHECK (vigenteHastaListaPrecioEncabezado >= vigenteDesdeListaPrecioEncabezado)
) ENGINE=InnoDB;

ALTER TABLE `tblListaPrecioEncabezado`
  ADD CONSTRAINT `Fk_ListaPrecioEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ListaPrecioEncabezado_TipoCliente` FOREIGN KEY (`idTipoCliente`)
    REFERENCES `tblTipoCliente` (`idTipoCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblPrecioTipoCliente | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblPrecioTipoCliente` (
  `idPrecioTipoCliente` INT NOT NULL AUTO_INCREMENT,
  `idTipoCliente` INT NOT NULL,
  `idProducto` INT NOT NULL,
  `porcentajeDescuento` DECIMAL(5,2) NOT NULL DEFAULT 0.00,
  `precioBase` DECIMAL(12,4) NOT NULL,
  `precioFinal` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(precioBase * (1 - (porcentajeDescuento / 100)), 4)) STORED,
  `activo` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_PrecioTipoCliente` PRIMARY KEY (`idPrecioTipoCliente`),
  CONSTRAINT `uqPrecioTipoCliente_TipoClienteProducto` UNIQUE (`idTipoCliente`, `idProducto`),
  CONSTRAINT `ckPrecioTipoClienteDescuentoRango` CHECK (`porcentajeDescuento` BETWEEN 0 AND 100)
) ENGINE=InnoDB;

ALTER TABLE `tblPrecioTipoCliente`
  ADD CONSTRAINT `Fk_PrecioTipoCliente_TipoCliente` FOREIGN KEY (`idTipoCliente`) REFERENCES `tblTipoCliente` (`idTipoCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_PrecioTipoCliente_Producto` FOREIGN KEY (`idProducto`) REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblHistorialModificacionPrecio | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblHistorialModificacionPrecio` (
  `idHistorialModificacionPrecio` INT NOT NULL AUTO_INCREMENT,
  `idTipoCliente` INT NOT NULL,
  `tipoModificacion` ENUM('DESCUENTO_PORCENTAJE', 'AUMENTO_PORCENTAJE', 'AUMENTO_BASE') NOT NULL,
  `valorModificacion` DECIMAL(12,4) NOT NULL,
  `alcanceModificacion` ENUM('INDIVIDUAL', 'SELECCIONADOS', 'TODOS_FILTROS') NOT NULL,
  `idMarca` INT NULL,
  `idLinea` INT NULL,
  `idCategoria` INT NULL,
  `idProveedor` INT NULL,
  `idProducto` INT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_HistorialModificacionPrecio` PRIMARY KEY (`idHistorialModificacionPrecio`)
) ENGINE=InnoDB;

ALTER TABLE `tblHistorialModificacionPrecio`
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_TipoCliente` FOREIGN KEY (`idTipoCliente`) REFERENCES `tblTipoCliente` (`idTipoCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Usuario` FOREIGN KEY (`idUsuarioRegistro`) REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Marca` FOREIGN KEY (`idMarca`) REFERENCES `tblMarca` (`Pk_Marca`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Linea` FOREIGN KEY (`idLinea`) REFERENCES `tblLinea` (`Pk_Linea`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Categoria` FOREIGN KEY (`idCategoria`) REFERENCES `tblCategoria` (`Pk_Categoria`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Proveedor` FOREIGN KEY (`idProveedor`) REFERENCES `tblProveedor` (`Pk_Proveedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Producto` FOREIGN KEY (`idProducto`) REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblListaPrecioDetalle | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblListaPrecioDetalle` (
  `idListaPrecioDetalle` INT NOT NULL AUTO_INCREMENT,
  `idListaPrecioEncabezado` INT NOT NULL,
  `idProducto` INT NOT NULL,
  `precioUnitarioListaPrecioDetalle` DECIMAL(12,4) NOT NULL,
  `activoListaPrecioDetalle` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ListaPrecioDetalle` PRIMARY KEY (`idListaPrecioEncabezado`, `idListaPrecioDetalle`),
  CONSTRAINT `uqListaPrecioDetalleListaPrecioEncabezadoCodigoProductoCodigo` UNIQUE (`idListaPrecioEncabezado`, `idProducto`),
  KEY `idxListaPrecioDetalleProductoCodigo` (`idProducto`),
  CONSTRAINT `ckListaPrecioDetalleactivoListaPrecioDetalleBooleano` CHECK (activoListaPrecioDetalle IN (0,1)),
  CONSTRAINT `ckListaPrecioDetallePrecioNoNegativo` CHECK (precioUnitarioListaPrecioDetalle >= 0),
  CONSTRAINT `uqListaPrecioDetalleId` UNIQUE (`idListaPrecioDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblListaPrecioDetalle`
  ADD CONSTRAINT `Fk_ListaPrecioDetalle_ListaPrecioEncabezado` FOREIGN KEY (`idListaPrecioEncabezado`)
    REFERENCES `tblListaPrecioEncabezado` (`idListaPrecioEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ListaPrecioDetalle_Producto` FOREIGN KEY (`idProducto`)
    REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblClienteListaPrecio | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblClienteListaPrecio` (
  `idClienteListaPrecio` INT NOT NULL AUTO_INCREMENT,
  `idCliente` INT NOT NULL,
  `idListaPrecioEncabezado` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ClienteListaPrecio` PRIMARY KEY (`idClienteListaPrecio`),
  KEY `idxClienteListaPrecioListaPrecioEncabezadoCodigo` (`idListaPrecioEncabezado`),
  CONSTRAINT `uqClienteListaPrecioClaveNegocio` UNIQUE (`idCliente`)
) ENGINE=InnoDB;

ALTER TABLE `tblClienteListaPrecio`
  ADD CONSTRAINT `Fk_ClienteListaPrecio_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ClienteListaPrecio_ListaPrecioEncabezado` FOREIGN KEY (`idListaPrecioEncabezado`)
    REFERENCES `tblListaPrecioEncabezado` (`idListaPrecioEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblHistorialModificacionPrecioDetalle | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblHistorialModificacionPrecioDetalle` (
  `idHistorialModificacionPrecioDetalle` INT NOT NULL AUTO_INCREMENT,
  `idHistorialModificacionPrecio` INT NOT NULL,
  `idProducto` INT NOT NULL,
  `porcentajeDescuentoAnterior` DECIMAL(5,2) NOT NULL,
  `precioBaseAnterior` DECIMAL(12,4) NOT NULL,
  `precioFinalAnterior` DECIMAL(12,4) NOT NULL,
  `porcentajeDescuentoNuevo` DECIMAL(5,2) NOT NULL,
  `precioBaseNuevo` DECIMAL(12,4) NOT NULL,
  `precioFinalNuevo` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_HistorialModificacionPrecioDetalle` PRIMARY KEY (`idHistorialModificacionPrecio`, `idHistorialModificacionPrecioDetalle`),
  CONSTRAINT `uqHistorialModificacionPrecioDetalleId` UNIQUE (`idHistorialModificacionPrecioDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblHistorialModificacionPrecioDetalle`
  ADD CONSTRAINT `Fk_HistorialDetalle_HistorialEncabezado` FOREIGN KEY (`idHistorialModificacionPrecio`) REFERENCES `tblHistorialModificacionPrecio` (`idHistorialModificacionPrecio`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialDetalle_Producto` FOREIGN KEY (`idProducto`) REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblNotaCxC | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7014
CREATE TABLE `tblNotaCxC` (
  `idNotaCxC` INT NOT NULL AUTO_INCREMENT,
  `numeroNotaCxC` VARCHAR(30) NOT NULL,
  `idFacturaEncabezado` INT NOT NULL,
  `tipoNotaCxC` VARCHAR(10) NOT NULL,
  `fechaNotaCxC` DATE NOT NULL,
  `montoNotaCxC` DECIMAL(12,4) NOT NULL,
  `motivoNotaCxC` TEXT NOT NULL,
  `estadoNotaCxC` VARCHAR(20) NOT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_NotaCxC` PRIMARY KEY (`idNotaCxC`),
  CONSTRAINT `uqNotaCxCnumero` UNIQUE (`numeroNotaCxC`),
  KEY `idxNotaCxCFacturaEncabezadoCodigo` (`idFacturaEncabezado`),
  KEY `idxNotaCxCUsuarioRegistro` (`idUsuarioRegistro`),
  CONSTRAINT `ckNotaCxCMontoPositivo` CHECK (montoNotaCxC > 0),
  CONSTRAINT `ckNotaCxCValoresTipoNota` CHECK (tipoNotaCxC IN ('CREDITO','DEBITO')),
  CONSTRAINT `ckNotaCxCValoresEstadoNota` CHECK (estadoNotaCxC IN ('BORRADOR','APLICADA','ANULADA'))
) ENGINE=InnoDB;

ALTER TABLE `tblNotaCxC`
  ADD CONSTRAINT `Fk_NotaCxC_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_NotaCxC_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblMovimientoCxC | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7003
CREATE TABLE `tblMovimientoCxC` (
  `idMovimientoCxC` INT NOT NULL AUTO_INCREMENT,
  `idFacturaEncabezado` INT NOT NULL,
  `idAplicacionPago` INT NULL,
  `idNotaCxC` INT NULL,
  `idMovimientoCxCRevertido` INT NULL,
  `fechaMovimientoCxC` DATE NOT NULL,
  `tipoMovimientoCxC` VARCHAR(30) NOT NULL,
  `descripcionMovimientoCxC` VARCHAR(255) NOT NULL,
  `cargoMovimientoCxC` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `abonoMovimientoCxC` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_MovimientoCxC` PRIMARY KEY (`idMovimientoCxC`),
  CONSTRAINT `uqMovimientoCxCAplicacionPagoCodigo` UNIQUE (`idAplicacionPago`),
  CONSTRAINT `uqMovimientoCxCNotaCxCCodigo` UNIQUE (`idNotaCxC`),
  CONSTRAINT `uqMovimientoCxCRevertido` UNIQUE (`idMovimientoCxCRevertido`),
  KEY `idxMovimientoCxCFacturaEncabezadoCodigofecha` (`idFacturaEncabezado`, `fechaMovimientoCxC`),
  KEY `idxMovimientoCxCfecha` (`fechaMovimientoCxC`),
  KEY `idxMovimientoCxCUsuarioRegistro` (`idUsuarioRegistro`),
  CONSTRAINT `ckMovimientoCxCCargoAbonoExcluyentes` CHECK (cargoMovimientoCxC >= 0 AND abonoMovimientoCxC >= 0 AND ((cargoMovimientoCxC > 0 AND abonoMovimientoCxC = 0) OR (abonoMovimientoCxC > 0 AND cargoMovimientoCxC = 0))),
  CONSTRAINT `ckMovimientoCxCReversionIdentificada` CHECK ((tipoMovimientoCxC = 'REVERSO' AND idMovimientoCxCRevertido IS NOT NULL) OR (tipoMovimientoCxC <> 'REVERSO' AND idMovimientoCxCRevertido IS NULL)),
  CONSTRAINT `ckMovimientoCxCValoresTipoMovimiento` CHECK (tipoMovimientoCxC IN ('FACTURA','PAGO','NOTA_CREDITO','NOTA_DEBITO','AJUSTE_POSITIVO','AJUSTE_NEGATIVO','REVERSO'))
) ENGINE=InnoDB;

ALTER TABLE `tblMovimientoCxC`
  ADD CONSTRAINT `Fk_MovimientoCxC_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_MovimientoCxC_AplicacionPago` FOREIGN KEY (`idAplicacionPago`)
    REFERENCES `tblAplicacionPago` (`idAplicacionPago`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_MovimientoCxC_NotaCxC` FOREIGN KEY (`idNotaCxC`)
    REFERENCES `tblNotaCxC` (`idNotaCxC`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_MovimientoCxC_MovimientoCxC_Revertido` FOREIGN KEY (`idMovimientoCxCRevertido`)
    REFERENCES `tblMovimientoCxC` (`idMovimientoCxC`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_MovimientoCxC_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblDepositoVentaEncabezado | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7015
CREATE TABLE `tblDepositoVentaEncabezado` (
  `idDepositoVentaEncabezado` INT NOT NULL AUTO_INCREMENT,
  `codigoDepositoVentaEncabezado` VARCHAR(30) NOT NULL,
  `fechaDepositoVentaEncabezado` DATE NOT NULL,
  `referenciaDepositoVentaEncabezado` VARCHAR(100) NULL,
  `idMoneda` BIGINT NOT NULL,
  `totalDepositoVentaEncabezado` DECIMAL(12,4) NOT NULL,
  `estadoDepositoVentaEncabezado` VARCHAR(25) NOT NULL DEFAULT 'PREPARADO',
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `idCuentaBancaria` BIGINT NOT NULL,
  `idTipoTransaccion` BIGINT NOT NULL,
  `idMovimientoBancario` BIGINT NULL,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_DepositoVentaEncabezado` PRIMARY KEY (`idDepositoVentaEncabezado`),
  CONSTRAINT `uqDepositoVentaEncabezadocodigo` UNIQUE (`codigoDepositoVentaEncabezado`),
  CONSTRAINT `uqDepositoVentaEncabezadoMovimientoBancarioCodigo` UNIQUE (`idMovimientoBancario`),
  KEY `idxDepositoVentaEncabezadoUsuarioRegistro` (`idUsuarioRegistro`),
  KEY `idxDepositoVentaEncabezadoMonedaCodigo` (`idMoneda`),
  KEY `idxDepositoVentaEncabezadoCuentaBancariaCodigo` (`idCuentaBancaria`),
  KEY `idxDepositoVentaEncabezadoTipoTransaccionCodigo` (`idTipoTransaccion`),
  CONSTRAINT `ckDepositoVentaEncabezadoTotalPositivo` CHECK (totalDepositoVentaEncabezado > 0),
  CONSTRAINT `ckDepositoVentaEncabezadoValoresEstadoDeposito` CHECK (estadoDepositoVentaEncabezado IN ('PREPARADO','ENVIADO','CONFIRMADO','ANULADO'))
) ENGINE=InnoDB;

ALTER TABLE `tblDepositoVentaEncabezado`
  ADD CONSTRAINT `Fk_DepositoVentaEncabezado_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_DepositoVentaEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_DepositoVentaEncabezado_CuentaBancaria` FOREIGN KEY (`idCuentaBancaria`)
    REFERENCES `tblCuentaBancaria` (`Pk_CuentaBancaria`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_DepositoVentaEncabezado_TipoTransaccion` FOREIGN KEY (`idTipoTransaccion`)
    REFERENCES `tblTipoTransaccion` (`Pk_TipoTransaccion`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_DepositoVentaEncabezado_MovimientoBancario` FOREIGN KEY (`idMovimientoBancario`)
    REFERENCES `tblMovimientoBancario` (`Pk_MovimientoBancario`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblReciboPago | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7005
CREATE TABLE `tblReciboPago` (
  `idReciboPago` INT NOT NULL AUTO_INCREMENT,
  `numeroReciboPago` VARCHAR(30) NOT NULL,
  `idCliente` INT NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `fechaReciboPago` DATE NOT NULL,
  `montoReciboPago` DECIMAL(12,4) NOT NULL,
  `referenciaReciboPago` VARCHAR(255) NULL,
  `estadoReciboPago` VARCHAR(20) NOT NULL,
  `observacionesReciboPago` TEXT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `idFormaPagoVenta` INT NOT NULL,
  CONSTRAINT `Pk_ReciboPago` PRIMARY KEY (`idReciboPago`),
  CONSTRAINT `uqReciboPagonumero` UNIQUE (`numeroReciboPago`),
  KEY `idxReciboPagoClienteCodigofecha` (`idCliente`, `fechaReciboPago`),
  KEY `idxReciboPagoMonedaCodigo` (`idMoneda`),
  KEY `idxReciboPagoUsuarioRegistro` (`idUsuarioRegistro`),
  KEY `idxReciboPagoFormaPagoVentaCodigo` (`idFormaPagoVenta`),
  CONSTRAINT `ckReciboPagoMontoPositivo` CHECK (montoReciboPago > 0),
  CONSTRAINT `ckReciboPagoValoresEstadoPago` CHECK (estadoReciboPago IN ('BORRADOR','CONFIRMADO','ANULADO'))
) ENGINE=InnoDB;

ALTER TABLE `tblReciboPago`
  ADD CONSTRAINT `Fk_ReciboPago_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ReciboPago_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ReciboPago_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ReciboPago_FormaPagoVenta` FOREIGN KEY (`idFormaPagoVenta`)
    REFERENCES `tblFormaPagoVenta` (`idFormaPagoVenta`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblDepositoVentaDetalle | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7015
CREATE TABLE `tblDepositoVentaDetalle` (
  `idDepositoVentaDetalle` INT NOT NULL AUTO_INCREMENT,
  `idDepositoVentaEncabezado` INT NOT NULL,
  `idReciboPago` INT NOT NULL,
  `importeDepositoVentaDetalle` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_DepositoVentaDetalle` PRIMARY KEY (`idDepositoVentaEncabezado`, `idDepositoVentaDetalle`),
  KEY `idxDepositoVentaDetalleReciboPagoCodigo` (`idReciboPago`),
  CONSTRAINT `ckDepositoVentaDetalleImportePositivo` CHECK (importeDepositoVentaDetalle > 0),
  CONSTRAINT `uqDepositoVentaDetalleClaveNegocio` UNIQUE (`idDepositoVentaEncabezado`, `idReciboPago`),
  CONSTRAINT `uqDepositoVentaDetalleId` UNIQUE (`idDepositoVentaDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblDepositoVentaDetalle`
  ADD CONSTRAINT `Fk_DepositoVentaDetalle_DepositoVentaEncabezado` FOREIGN KEY (`idDepositoVentaEncabezado`)
    REFERENCES `tblDepositoVentaEncabezado` (`idDepositoVentaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_DepositoVentaDetalle_ReciboPago` FOREIGN KEY (`idReciboPago`)
    REFERENCES `tblReciboPago` (`idReciboPago`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblAplicacionPago | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7005
CREATE TABLE `tblAplicacionPago` (
  `idAplicacionPago` INT NOT NULL AUTO_INCREMENT,
  `idReciboPago` INT NOT NULL,
  `idFacturaEncabezado` INT NOT NULL,
  `fechaAplicacionPago` DATE NOT NULL,
  `montoAplicadoAplicacionPago` DECIMAL(12,4) NOT NULL,
  `estadoAplicacionPago` VARCHAR(20) NOT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_AplicacionPago` PRIMARY KEY (`idAplicacionPago`),
  KEY `idxAplicacionPagoFacturaEncabezadoCodigofecha` (`idFacturaEncabezado`, `fechaAplicacionPago`),
  KEY `idxAplicacionPagoReciboPagoCodigo` (`idReciboPago`),
  KEY `idxAplicacionPagoUsuarioRegistro` (`idUsuarioRegistro`),
  CONSTRAINT `ckAplicacionPagoMontoPositivo` CHECK (montoAplicadoAplicacionPago > 0),
  CONSTRAINT `ckAplicacionPagoValoresEstadoAplicacion` CHECK (estadoAplicacionPago IN ('APLICADA','REVERTIDA'))
) ENGINE=InnoDB;

ALTER TABLE `tblAplicacionPago`
  ADD CONSTRAINT `Fk_AplicacionPago_ReciboPago` FOREIGN KEY (`idReciboPago`)
    REFERENCES `tblReciboPago` (`idReciboPago`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_AplicacionPago_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_AplicacionPago_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblCotizacionEncabezado | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7011
CREATE TABLE `tblCotizacionEncabezado` (
  `idCotizacionEncabezado` INT NOT NULL AUTO_INCREMENT,
  `numeroCotizacionEncabezado` VARCHAR(30) NOT NULL,
  `idCliente` INT NOT NULL,
  `idVendedor` INT NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `fechaEmisionCotizacionEncabezado` DATE NOT NULL,
  `vigenteHastaCotizacionEncabezado` DATE NOT NULL,
  `estadoCotizacionEncabezado` VARCHAR(25) NOT NULL,
  `observacionesCotizacionEncabezado` TEXT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `idListaPrecioEncabezado` INT NOT NULL,
  CONSTRAINT `Pk_CotizacionEncabezado` PRIMARY KEY (`idCotizacionEncabezado`),
  CONSTRAINT `uqCotizacionEncabezadonumero` UNIQUE (`numeroCotizacionEncabezado`),
  KEY `idxCotizacionEncabezadoClienteCodigo` (`idCliente`),
  KEY `idxCotizacionEncabezadoVendedorCodigo` (`idVendedor`),
  KEY `idxCotizacionEncabezadoMonedaCodigo` (`idMoneda`),
  KEY `idxCotizacionEncabezadoUsuarioRegistro` (`idUsuarioRegistro`),
  KEY `idxCotizacionEncabezadoListaPrecioEncabezadoCodigo` (`idListaPrecioEncabezado`),
  CONSTRAINT `ckCotizacionEncabezadoValoresEstadoCotizacion` CHECK (estadoCotizacionEncabezado IN ('BORRADOR','EMITIDA','ACEPTADA','RECHAZADA','VENCIDA','ANULADA')),
  CONSTRAINT `ckCotizacionEncabezadoFechasCoherentes` CHECK (vigenteHastaCotizacionEncabezado >= fechaEmisionCotizacionEncabezado)
) ENGINE=InnoDB;

ALTER TABLE `tblCotizacionEncabezado`
  ADD CONSTRAINT `Fk_CotizacionEncabezado_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CotizacionEncabezado_Vendedor` FOREIGN KEY (`idVendedor`)
    REFERENCES `tblVendedor` (`idVendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CotizacionEncabezado_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CotizacionEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CotizacionEncabezado_ListaPrecioEncabezado` FOREIGN KEY (`idListaPrecioEncabezado`)
    REFERENCES `tblListaPrecioEncabezado` (`idListaPrecioEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblCotizacionDetalle | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7011
CREATE TABLE `tblCotizacionDetalle` (
  `idCotizacionDetalle` INT NOT NULL AUTO_INCREMENT,
  `idCotizacionEncabezado` INT NOT NULL,
  `numeroLineaCotizacionDetalle` INT NOT NULL,
  `idProducto` INT NOT NULL,
  `descripcionItemCotizacionDetalle` VARCHAR(255) NOT NULL,
  `cantidadCotizacionDetalle` DECIMAL(12,4) NOT NULL,
  `precioUnitarioCotizacionDetalle` DECIMAL(12,4) NOT NULL,
  `descuentoMontoCotizacionDetalle` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `tasaImpuestoCotizacionDetalle` DECIMAL(3,2) NOT NULL DEFAULT 0,
  `impuestoMontoCotizacionDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND((cantidadCotizacionDetalle * precioUnitarioCotizacionDetalle - descuentoMontoCotizacionDetalle) * tasaImpuestoCotizacionDetalle, 4)) STORED,
  `totalLineaCotizacionDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(cantidadCotizacionDetalle * precioUnitarioCotizacionDetalle - descuentoMontoCotizacionDetalle + impuestoMontoCotizacionDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CotizacionDetalle` PRIMARY KEY (`idCotizacionEncabezado`, `idCotizacionDetalle`),
  CONSTRAINT `uqCotizacionDetalleCotizacionEncabezadoCodigonumeroLinea` UNIQUE (`idCotizacionEncabezado`, `numeroLineaCotizacionDetalle`),
  KEY `idxCotizacionDetalleProductoCodigo` (`idProducto`),
  CONSTRAINT `ckCotizacionDetalletasaImpuestoCotizacionDetalleRango` CHECK (tasaImpuestoCotizacionDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckCotizacionDetalleCantidadPrecioDescuentoValidos` CHECK (cantidadCotizacionDetalle > 0 AND precioUnitarioCotizacionDetalle >= 0 AND descuentoMontoCotizacionDetalle >= 0 AND descuentoMontoCotizacionDetalle <= cantidadCotizacionDetalle * precioUnitarioCotizacionDetalle),
  CONSTRAINT `ckCotizacionDetalleNumeroLineaPositivo` CHECK (numeroLineaCotizacionDetalle > 0),
  CONSTRAINT `uqCotizacionDetalleId` UNIQUE (`idCotizacionDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblCotizacionDetalle`
  ADD CONSTRAINT `Fk_CotizacionDetalle_CotizacionEncabezado` FOREIGN KEY (`idCotizacionEncabezado`)
    REFERENCES `tblCotizacionEncabezado` (`idCotizacionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CotizacionDetalle_Producto` FOREIGN KEY (`idProducto`)
    REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblPedidoEncabezado | Autor: Natali Sofía Montenegro Portillo - 0901-23-10017 | Modulo: 7000 | Aplicacion: 7004
CREATE TABLE `tblPedidoEncabezado` (
  `idPedidoEncabezado` INT NOT NULL AUTO_INCREMENT,
  `referenciaPedidoEncabezado` VARCHAR(30) NOT NULL,
  `idCotizacionEncabezado` INT NULL,
  `idCliente` INT NOT NULL,
  `idVendedor` INT NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `fechaPedidoEncabezado` DATE NOT NULL,
  `fechaEntregaPedidoEncabezado` DATE NULL,
  `fechaVencimientoPedidoEncabezado` DATE NULL,
  `estadoPedidoEncabezado` VARCHAR(25) NOT NULL DEFAULT 'PENDIENTE',
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_PedidoEncabezado` PRIMARY KEY (`idPedidoEncabezado`),
  CONSTRAINT `uqPedidoEncabezadoReferencia` UNIQUE (`referenciaPedidoEncabezado`),
  CONSTRAINT `uqPedidoEncabezadoCotizacion` UNIQUE (`idCotizacionEncabezado`),
  KEY `idxPedidoEncabezadoCliente` (`idCliente`),
  KEY `idxPedidoEncabezadoVendedor` (`idVendedor`),
  KEY `idxPedidoEncabezadoMoneda` (`idMoneda`),
  CONSTRAINT `ckPedidoEncabezadoEstado` CHECK (`estadoPedidoEncabezado` IN ('PENDIENTE','CONFIRMADO','COMPLETADO','ANULADO')),
  CONSTRAINT `ckPedidoEncabezadoFechas` CHECK (
    (`fechaEntregaPedidoEncabezado` IS NULL OR `fechaEntregaPedidoEncabezado` >= `fechaPedidoEncabezado`)
    AND (`fechaVencimientoPedidoEncabezado` IS NULL OR `fechaVencimientoPedidoEncabezado` >= `fechaPedidoEncabezado`)
  )
) ENGINE=InnoDB;

ALTER TABLE `tblPedidoEncabezado`
  ADD CONSTRAINT `Fk_PedidoEncabezado_Cotizacion` FOREIGN KEY (`idCotizacionEncabezado`)
    REFERENCES `tblCotizacionEncabezado` (`idCotizacionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_PedidoEncabezado_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_PedidoEncabezado_Vendedor` FOREIGN KEY (`idVendedor`)
    REFERENCES `tblVendedor` (`idVendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_PedidoEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblPedidoDetalle | Autor: Natali Sofía Montenegro Portillo - 0901-23-10017 | Modulo: 7000 | Aplicacion: 7004
CREATE TABLE `tblPedidoDetalle` (
  `idPedidoDetalle` INT NOT NULL AUTO_INCREMENT,
  `idPedidoEncabezado` INT NOT NULL,
  `OrdenPedido` INT NOT NULL,
  `idProducto` INT NOT NULL,
  `cantidadPedidoDetalle` DECIMAL(12,4) NOT NULL,
  `precioUnitarioPedidoDetalle` DECIMAL(12,4) NOT NULL,
  `descuentoMontoPedidoDetalle` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_PedidoDetalle` PRIMARY KEY (`idPedidoEncabezado`, `idPedidoDetalle`),
  KEY `idxPedidoDetalleProducto` (`idProducto`),
  CONSTRAINT `ckPedidoDetalleOrdenPositivo` CHECK (`OrdenPedido` > 0),
  CONSTRAINT `ckPedidoDetalleCantidadPrecioDescuento` CHECK (
    `cantidadPedidoDetalle` > 0
    AND `precioUnitarioPedidoDetalle` >= 0
    AND `descuentoMontoPedidoDetalle` >= 0
    AND `descuentoMontoPedidoDetalle` <= `cantidadPedidoDetalle` * `precioUnitarioPedidoDetalle`
  ),
  CONSTRAINT `uqPedidoDetalleClaveNegocio` UNIQUE (`idPedidoEncabezado`, `OrdenPedido`, `idProducto`),
  CONSTRAINT `uqPedidoDetalleId` UNIQUE (`idPedidoDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblPedidoDetalle`
  ADD CONSTRAINT `Fk_PedidoDetalle_PedidoEncabezado` FOREIGN KEY (`idPedidoEncabezado`)
    REFERENCES `tblPedidoEncabezado` (`idPedidoEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_PedidoDetalle_Producto` FOREIGN KEY (`idProducto`)
    REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblClienteFacturaVersion | Autor: Matthew Andreé Juárez Xoy - 0901-23-4250 | Modulo: 7000 | Aplicacion: 7007
CREATE TABLE `tblClienteFacturaVersion` (
  `idClienteFacturaVersion` INT NOT NULL AUTO_INCREMENT,
  `idCliente` INT NOT NULL,
  `numeroVersionClienteFacturaVersion` INT NOT NULL,
  `tipoIdentificacionClienteFacturaVersion` ENUM('NIT','CUI','CF') NOT NULL,
  `nitClienteFacturaVersion` CHAR(10) NULL,
  `dpiClienteFacturaVersion` CHAR(13) NULL,
  `nombreClienteFacturaVersion` VARCHAR(100) NOT NULL,
  `direccionClienteFacturaVersion` VARCHAR(500) NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ClienteFacturaVersion` PRIMARY KEY (`idClienteFacturaVersion`),
  CONSTRAINT `uqClienteFacturaVersionClienteCodigonumeroVersion` UNIQUE (`idCliente`, `numeroVersionClienteFacturaVersion`),
  CONSTRAINT `ckClienteFacturaVersionIdentificacionCoherente` CHECK ((tipoIdentificacionClienteFacturaVersion = 'CF' AND nitClienteFacturaVersion IS NULL AND dpiClienteFacturaVersion IS NULL) OR (tipoIdentificacionClienteFacturaVersion = 'NIT' AND nitClienteFacturaVersion IS NOT NULL AND dpiClienteFacturaVersion IS NULL) OR (tipoIdentificacionClienteFacturaVersion = 'CUI' AND dpiClienteFacturaVersion IS NOT NULL AND nitClienteFacturaVersion IS NULL)),
  CONSTRAINT `ckClienteFacturaVersionVersionPositiva` CHECK (numeroVersionClienteFacturaVersion > 0)
) ENGINE=InnoDB;

ALTER TABLE `tblClienteFacturaVersion`
  ADD CONSTRAINT `Fk_ClienteFacturaVersion_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblFacturaEncabezado | Autor: Matthew Andreé Juárez Xoy - 0901-23-4250 | Modulo: 7000 | Aplicacion: 7007
CREATE TABLE `tblFacturaEncabezado` (
  `idFacturaEncabezado` INT NOT NULL AUTO_INCREMENT,
  `uuidDteFacturaEncabezado` CHAR(36) NOT NULL,
  `serieFacturaEncabezado` VARCHAR(30) NOT NULL,
  `numeroFacturaEncabezado` VARCHAR(30) NOT NULL,
  `idSucursal` INT NOT NULL,
  `idClienteFacturaVersion` INT NOT NULL,
  `idVendedor` INT NULL,
  `idMoneda` BIGINT NOT NULL,
  `idCotizacionEncabezado` INT NULL,
  `idPedidoEncabezado` INT NULL,
  `fechaEmisionFacturaEncabezado` DATETIME NOT NULL,
  `fechaVencimientoFacturaEncabezado` DATE NOT NULL,
  `tipoDteFacturaEncabezado` VARCHAR(30) NOT NULL DEFAULT 'Factura',
  `condicionPagoFacturaEncabezado` VARCHAR(15) NOT NULL,
  `estadoFacturaEncabezado` VARCHAR(25) NOT NULL DEFAULT 'EMITIDA',
  `fechaCertificacionFacturaEncabezado` DATETIME NULL,
  `subtotalFacturaEncabezado` DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
  `impuestoIvaFacturaEncabezado` DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
  `totalFacturaEncabezado` DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
  `observacionesFacturaEncabezado` TEXT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_FacturaEncabezado` PRIMARY KEY (`idFacturaEncabezado`),
  CONSTRAINT `uqFacturaEncabezadoSucursalCodigoserienumero` UNIQUE (`idSucursal`, `serieFacturaEncabezado`, `numeroFacturaEncabezado`),
  CONSTRAINT `uqFacturaEncabezadoCotizacionEncabezadoCodigo` UNIQUE (`idCotizacionEncabezado`),
  KEY `idxFacturaEncabezadoPedidoEncabezadoCodigo` (`idPedidoEncabezado`),
  KEY `idxFacturaEncabezadoVendedorCodigo` (`idVendedor`),
  KEY `idxFacturaEncabezadoMonedaCodigo` (`idMoneda`),
  KEY `idxFacturaEncabezadoUsuarioRegistro` (`idUsuarioRegistro`),
  KEY `idxFacturaEncabezadoClienteFacturaVersionCodigo` (`idClienteFacturaVersion`),
  CONSTRAINT `ckFacturaEncabezadoCondicionPagoValida` CHECK (condicionPagoFacturaEncabezado IN ('CONTADO','CREDITO')),
  CONSTRAINT `ckFacturaEncabezadoValoresEstadoFactura` CHECK (estadoFacturaEncabezado IN ('BORRADOR','EMITIDA','ANULADA')),
  CONSTRAINT `ckFacturaEncabezadoFechasCoherentes` CHECK (fechaVencimientoFacturaEncabezado >= DATE(fechaEmisionFacturaEncabezado)),
  CONSTRAINT `uqFacturaEncabezadoClaveNegocio` UNIQUE (`uuidDteFacturaEncabezado`)
) ENGINE=InnoDB;

ALTER TABLE `tblFacturaEncabezado`
  ADD CONSTRAINT `Fk_FacturaEncabezado_Sucursal` FOREIGN KEY (`idSucursal`)
REFERENCES `tblSucursal` (`Pk_Sucursal`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_ClienteFacturaVersion` FOREIGN KEY (`idClienteFacturaVersion`)
REFERENCES `tblClienteFacturaVersion` (`idClienteFacturaVersion`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_Vendedor` FOREIGN KEY (`idVendedor`)
REFERENCES `tblVendedor` (`idVendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_CotizacionEncabezado` FOREIGN KEY (`idCotizacionEncabezado`)
REFERENCES `tblCotizacionEncabezado` (`idCotizacionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_PedidoEncabezado` FOREIGN KEY (`idPedidoEncabezado`)
REFERENCES `tblPedidoEncabezado` (`idPedidoEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblFacturaDetalle | Autor: Matthew Andreé Juárez Xoy - 0901-23-4250 | Modulo: 7000 | Aplicacion: 7007
CREATE TABLE `tblFacturaDetalle` (
  `idFacturaEncabezadoUuidDte` CHAR(36) NOT NULL,
  `numeroLineaFacturaDetalle` INT NOT NULL,
  `idFacturaDetalle` INT NOT NULL AUTO_INCREMENT,
  `idProducto` INT NOT NULL,
  `cantidadFacturaDetalle` DECIMAL(12,4) NOT NULL,
  `precioUnitarioFacturaDetalle` DECIMAL(12,4) NOT NULL,
  `descuentoMontoFacturaDetalle` DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
  `tasaImpuestoFacturaDetalle` DECIMAL(3,2) NOT NULL DEFAULT 0.12,
  `impuestoMontoFacturaDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND((cantidadFacturaDetalle * precioUnitarioFacturaDetalle - descuentoMontoFacturaDetalle) * tasaImpuestoFacturaDetalle, 4)) STORED,
  `subtotalLineaFacturaDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(cantidadFacturaDetalle * precioUnitarioFacturaDetalle - descuentoMontoFacturaDetalle, 4)) STORED,
  `totalLineaFacturaDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(cantidadFacturaDetalle * precioUnitarioFacturaDetalle - descuentoMontoFacturaDetalle + impuestoMontoFacturaDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_FacturaDetalle` PRIMARY KEY (`idFacturaEncabezadoUuidDte`, `idFacturaDetalle`),
  KEY `idxFacturaDetalleProductoCodigo` (`idProducto`),
  CONSTRAINT `ckFacturaDetalletasaImpuestoFacturaDetalleRango` CHECK (tasaImpuestoFacturaDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckFacturaDetalleCantidadPrecioDescuentoValidos` CHECK (cantidadFacturaDetalle > 0 AND precioUnitarioFacturaDetalle >= 0 AND descuentoMontoFacturaDetalle >= 0 AND descuentoMontoFacturaDetalle <= cantidadFacturaDetalle * precioUnitarioFacturaDetalle),
  CONSTRAINT `ckFacturaDetalleNumeroLineaPositivo` CHECK (numeroLineaFacturaDetalle > 0),
  CONSTRAINT `uqFacturaDetalleClaveNegocio` UNIQUE (`idFacturaEncabezadoUuidDte`, `numeroLineaFacturaDetalle`),
  CONSTRAINT `uqFacturaDetalleId` UNIQUE (`idFacturaDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblFacturaDetalle`
  ADD CONSTRAINT `Fk_FacturaDetalle_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezadoUuidDte`)
REFERENCES `tblFacturaEncabezado` (`uuidDteFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaDetalle_Producto` FOREIGN KEY (`idProducto`)
REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblReglaComision | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblReglaComision` (
  `idReglaComision` INT NOT NULL AUTO_INCREMENT,
  `codigoReglaComision` VARCHAR(30) NOT NULL,
  `nombreReglaComision` VARCHAR(100) NOT NULL,
  `baseCalculoReglaComision` VARCHAR(30) NOT NULL,
  `tasaComisionReglaComision` DECIMAL(3,2) NOT NULL,
  `vigenteDesdeReglaComision` DATE NOT NULL,
  `vigenteHastaReglaComision` DATE NULL,
  `requiereAutorizacionReglaComision` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ReglaComision` PRIMARY KEY (`idReglaComision`),
  CONSTRAINT `uqReglaComisioncodigo` UNIQUE (`codigoReglaComision`),
  CONSTRAINT `ckReglaComisiontasaComisionReglaComisionRango` CHECK (tasaComisionReglaComision BETWEEN 0 AND 0.99),
  CONSTRAINT `ckReglaComisionrequiereAutorizacionReglaComisionBooleano` CHECK (requiereAutorizacionReglaComision IN (0,1)),
  CONSTRAINT `ckReglaComisionValoresBaseCalculo` CHECK (baseCalculoReglaComision IN ('VENTA_NETA','COBRO_NETO')),
  CONSTRAINT `ckReglaComisionFechasCoherentes` CHECK (vigenteHastaReglaComision >= vigenteDesdeReglaComision)
) ENGINE=InnoDB;

-- tblComisionEncabezado | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblComisionEncabezado` (
  `idComisionEncabezado` INT NOT NULL AUTO_INCREMENT,
  `numeroComisionEncabezado` VARCHAR(30) NOT NULL,
  `idVendedor` INT NOT NULL,
  `idReglaComision` INT NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `fechaDesdeComisionEncabezado` DATE NOT NULL,
  `fechaHastaComisionEncabezado` DATE NOT NULL,
  `estadoComisionEncabezado` VARCHAR(25) NOT NULL,
  `idUsuarioSolicita` INT NULL,
  `fechaSolicitudComisionEncabezado` DATETIME NULL,
  `idUsuarioAutoriza` INT NULL,
  `fechaAutorizacionComisionEncabezado` DATETIME NULL,
  `comentarioAutorizacionComisionEncabezado` TEXT NULL,
  `referenciaContableComisionEncabezado` VARCHAR(100) NULL,
  `fechaEnvioComisionEncabezado` DATETIME NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionEncabezado` PRIMARY KEY (`idComisionEncabezado`),
  CONSTRAINT `uqComisionEncabezadonumero` UNIQUE (`numeroComisionEncabezado`),
  KEY `idxComisionEncabezadoVendedorCodigofechaDesdefechaHasta` (`idVendedor`, `fechaDesdeComisionEncabezado`, `fechaHastaComisionEncabezado`),
  KEY `idxComisionEncabezadoReglaComisionCodigo` (`idReglaComision`),
  KEY `idxComisionEncabezadoMonedaCodigo` (`idMoneda`),
  KEY `idxComisionEncabezadoUsuarioSolicita` (`idUsuarioSolicita`),
  KEY `idxComisionEncabezadoUsuarioAutoriza` (`idUsuarioAutoriza`),
  CONSTRAINT `ckComisionEncabezadoValoresEstadoComision` CHECK (estadoComisionEncabezado IN ('BORRADOR','SOLICITADA','APROBADA','RECHAZADA','ENVIADA')),
  CONSTRAINT `ckComisionEncabezadoFechasCoherentes` CHECK (fechaHastaComisionEncabezado >= fechaDesdeComisionEncabezado)
) ENGINE=InnoDB;

ALTER TABLE `tblComisionEncabezado`
  ADD CONSTRAINT `Fk_ComisionEncabezado_Vendedor` FOREIGN KEY (`idVendedor`)
    REFERENCES `tblVendedor` (`idVendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionEncabezado_ReglaComision` FOREIGN KEY (`idReglaComision`)
    REFERENCES `tblReglaComision` (`idReglaComision`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionEncabezado_Usuario_Solicita` FOREIGN KEY (`idUsuarioSolicita`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionEncabezado_Usuario_Autoriza` FOREIGN KEY (`idUsuarioAutoriza`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblComisionFacturaEncabezado | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblComisionFacturaEncabezado` (
  `idComisionFacturaEncabezado` INT NOT NULL AUTO_INCREMENT,
  `idFacturaEncabezado` INT NOT NULL,
  `metodoComisionFacturaEncabezado` ENUM('LINEA','PRODUCTO','CATEGORIA','MARCA','MIXTA') NOT NULL,
  `estadoComisionFacturaEncabezado` ENUM('CALCULADA','ANULADA') NOT NULL DEFAULT 'CALCULADA',
  `observacionesComisionFacturaEncabezado` TEXT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionFacturaEncabezado` PRIMARY KEY (`idComisionFacturaEncabezado`),
  CONSTRAINT `uqComisionFacturaEncabezadoFacturaEncabezadoCodigo` UNIQUE (`idFacturaEncabezado`)
) ENGINE=InnoDB;

ALTER TABLE `tblComisionFacturaEncabezado`
  ADD CONSTRAINT `Fk_ComisionFacturaEncabezado_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblComisionDetalle | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblComisionDetalle` (
  `idComisionDetalle` INT NOT NULL AUTO_INCREMENT,
  `idComisionEncabezado` INT NOT NULL,
  `idFacturaEncabezado` INT NOT NULL,
  `idAplicacionPago` INT NULL,
  `baseComisionDetalle` DECIMAL(12,4) NOT NULL,
  `tasaAplicadaComisionDetalle` DECIMAL(3,2) NOT NULL,
  `montoComisionDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(baseComisionDetalle * tasaAplicadaComisionDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionDetalle` PRIMARY KEY (`idComisionEncabezado`, `idComisionDetalle`),
  KEY `idxComisionDetalleComisionEncabezadoCodigo` (`idComisionEncabezado`),
  KEY `idxComisionDetalleFacturaEncabezadoCodigo` (`idFacturaEncabezado`),
  KEY `idxComisionDetalleAplicacionPagoCodigo` (`idAplicacionPago`),
  CONSTRAINT `ckComisionDetalletasaAplicadaComisionDetalleRango` CHECK (tasaAplicadaComisionDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckComisionDetalleBaseNoNegativa` CHECK (baseComisionDetalle >= 0),
  CONSTRAINT `uqComisionDetalleId` UNIQUE (`idComisionDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblComisionDetalle`
  ADD CONSTRAINT `Fk_ComisionDetalle_ComisionEncabezado` FOREIGN KEY (`idComisionEncabezado`)
    REFERENCES `tblComisionEncabezado` (`idComisionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionDetalle_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionDetalle_AplicacionPago` FOREIGN KEY (`idAplicacionPago`)
    REFERENCES `tblAplicacionPago` (`idAplicacionPago`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblComisionAbono | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblComisionAbono` (
  `idComisionAbono` INT NOT NULL AUTO_INCREMENT,
  `idComisionFacturaEncabezado` INT NOT NULL,
  `idAplicacionPago` INT NOT NULL,
  `tasaFacturaPagadaAcumuladaComisionAbono` DECIMAL(5,4) NOT NULL,
  `montoComisionDevengadoComisionAbono` DECIMAL(12,4) NOT NULL,
  `estadoPagoVendedorComisionAbono` ENUM('PENDIENTE','PAGADO','REVERSADO') NOT NULL DEFAULT 'PENDIENTE',
  `fechaPagoVendedorComisionAbono` DATE NULL,
  `referenciaNominaComisionAbono` VARCHAR(100) NULL,
  `observacionesComisionAbono` TEXT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionAbono` PRIMARY KEY (`idComisionAbono`),
  CONSTRAINT `uqComisionAbonoAplicacionPagoCodigo` UNIQUE (`idAplicacionPago`),
  KEY `idxComisionAbonoComisionFacturaEncabezadoCodigoestadoPa19c1f12f` (`idComisionFacturaEncabezado`, `estadoPagoVendedorComisionAbono`),
  CONSTRAINT `ckComisionAbonotasaFacturaPagadaAcumuladaComisionAbonoRango` CHECK (tasaFacturaPagadaAcumuladaComisionAbono BETWEEN 0 AND 1),
  CONSTRAINT `ckComisionAbonoDevengoNoNegativo` CHECK (montoComisionDevengadoComisionAbono >= 0),
  CONSTRAINT `ckComisionAbonoFechaPagoVendedor` CHECK (estadoPagoVendedorComisionAbono <> 'PAGADO' OR fechaPagoVendedorComisionAbono IS NOT NULL)
) ENGINE=InnoDB;

ALTER TABLE `tblComisionAbono`
  ADD CONSTRAINT `Fk_ComisionAbono_ComisionFacturaEncabezado` FOREIGN KEY (`idComisionFacturaEncabezado`)
    REFERENCES `tblComisionFacturaEncabezado` (`idComisionFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionAbono_AplicacionPago` FOREIGN KEY (`idAplicacionPago`)
    REFERENCES `tblAplicacionPago` (`idAplicacionPago`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblComisionFacturaDetalle | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblComisionFacturaDetalle` (
  `idComisionFacturaDetalle` INT NOT NULL AUTO_INCREMENT,
  `idComisionFacturaEncabezado` INT NOT NULL,
  `idFacturaDetalle` INT NOT NULL,
  `metodoComisionFacturaDetalle` ENUM('LINEA','PRODUCTO','CATEGORIA','MARCA') NOT NULL,
  `nombreClasificacionHistoricaComisionFacturaDetalle` VARCHAR(100) NOT NULL,
  `tasaAplicadaComisionFacturaDetalle` DECIMAL(3,2) NOT NULL,
  `baseComisionFacturaDetalle` DECIMAL(12,4) NOT NULL,
  `montoComisionLineaComisionFacturaDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(baseComisionFacturaDetalle * tasaAplicadaComisionFacturaDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `idLinea` INT NULL,
  `idCategoria` INT NULL,
  `idMarca` INT NULL,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionFacturaDetalle` PRIMARY KEY (`idComisionFacturaEncabezado`, `idComisionFacturaDetalle`),
  CONSTRAINT `uqComisionFacturaDetalleFacturaDetalleCodigo` UNIQUE (`idFacturaDetalle`),
  KEY `idxComisionFacturaDetalleComisionFacturaEncabezadoCodigo` (`idComisionFacturaEncabezado`),
  KEY `idxComisionFacturaDetalleLineaCodigo` (`idLinea`),
  KEY `idxComisionFacturaDetalleCategoriaCodigo` (`idCategoria`),
  KEY `idxComisionFacturaDetalleMarcaCodigo` (`idMarca`),
  CONSTRAINT `ckComisionFacturaDetalletasaAplicadaComisionFacturaDetalleRango` CHECK (tasaAplicadaComisionFacturaDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckComisionFacturaDetalleBaseNoNegativa` CHECK (baseComisionFacturaDetalle >= 0),
  CONSTRAINT `ckComisionFacturaDetalleClasificacionTipada` CHECK ((metodoComisionFacturaDetalle = 'PRODUCTO' AND idLinea IS NULL AND idCategoria IS NULL AND idMarca IS NULL) OR (metodoComisionFacturaDetalle = 'LINEA' AND idLinea IS NOT NULL AND idCategoria IS NULL AND idMarca IS NULL) OR (metodoComisionFacturaDetalle = 'CATEGORIA' AND idCategoria IS NOT NULL AND idLinea IS NULL AND idMarca IS NULL) OR (metodoComisionFacturaDetalle = 'MARCA' AND idMarca IS NOT NULL AND idLinea IS NULL AND idCategoria IS NULL)),
  CONSTRAINT `uqComisionFacturaDetalleId` UNIQUE (`idComisionFacturaDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblComisionFacturaDetalle`
  ADD CONSTRAINT `Fk_ComisionFacturaDetalle_ComisionFacturaEncabezado` FOREIGN KEY (`idComisionFacturaEncabezado`)
    REFERENCES `tblComisionFacturaEncabezado` (`idComisionFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionFacturaDetalle_FacturaDetalle` FOREIGN KEY (`idFacturaDetalle`)
    REFERENCES `tblFacturaDetalle` (`idFacturaDetalle`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionFacturaDetalle_Linea` FOREIGN KEY (`idLinea`)
    REFERENCES `tblLinea` (`Pk_Linea`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionFacturaDetalle_Categoria` FOREIGN KEY (`idCategoria`)
    REFERENCES `tblCategoria` (`Pk_Categoria`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionFacturaDetalle_Marca` FOREIGN KEY (`idMarca`)
    REFERENCES `tblMarca` (`Pk_Marca`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblCliente | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7009
CREATE TABLE `tblCliente` (
  `idCliente` INT NOT NULL AUTO_INCREMENT,
  `codigoCliente` VARCHAR(30) NOT NULL,
  `tipoIdentificacionCliente` ENUM('NIT','CUI','CF') NOT NULL,
  `nombreCliente` VARCHAR(100) NOT NULL,
  `direccionCliente` VARCHAR(500) NULL,
  `nombreContactoCliente` VARCHAR(100) NULL,
  `telefonoCliente` VARCHAR(30) NULL,
  `correoCliente` VARCHAR(150) NULL,
  `diasCreditoCliente` INT NOT NULL DEFAULT 0,
  `limiteCreditoCliente` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `idMonedaCredito` BIGINT NOT NULL,
  `activoCliente` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `nitCliente` CHAR(10) NULL,
  `dpiCliente` CHAR(13) NULL,
  CONSTRAINT `Pk_Cliente` PRIMARY KEY (`idCliente`),
  CONSTRAINT `uqClientecodigo` UNIQUE (`codigoCliente`),
  CONSTRAINT `uqClientenit` UNIQUE (`nitCliente`),
  CONSTRAINT `uqClientedpi` UNIQUE (`dpiCliente`),
  KEY `idxClienteMonedaCredito` (`idMonedaCredito`),
  CONSTRAINT `ckClienteactivoClienteBooleano` CHECK (activoCliente IN (0,1)),
  CONSTRAINT `ckClienteIdentificacionCoherente` CHECK ((tipoIdentificacionCliente = 'CF' AND nitCliente IS NULL AND dpiCliente IS NULL) OR (tipoIdentificacionCliente = 'NIT' AND nitCliente IS NOT NULL AND dpiCliente IS NULL) OR (tipoIdentificacionCliente = 'CUI' AND dpiCliente IS NOT NULL AND nitCliente IS NULL)),
  CONSTRAINT `ckClienteCreditoNoNegativo` CHECK (diasCreditoCliente >= 0 AND limiteCreditoCliente >= 0)
) ENGINE=InnoDB;

ALTER TABLE `tblCliente`
  ADD CONSTRAINT `Fk_Cliente_Moneda_Credito` FOREIGN KEY (`idMonedaCredito`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblVendedor | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7009
CREATE TABLE `tblVendedor` (
  `idVendedor` INT NOT NULL AUTO_INCREMENT,
  `codigoVendedor` VARCHAR(30) NOT NULL,
  `idEmpleado` INT NOT NULL,
  `activoVendedor` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_Vendedor` PRIMARY KEY (`idVendedor`),
  CONSTRAINT `uqVendedorcodigo` UNIQUE (`codigoVendedor`),
  CONSTRAINT `uqVendedorEmpleadoCodigo` UNIQUE (`idEmpleado`),
  CONSTRAINT `ckVendedoractivoVendedorBooleano` CHECK (activoVendedor IN (0,1))
) ENGINE=InnoDB;

ALTER TABLE `tblVendedor`
  ADD CONSTRAINT `Fk_Vendedor_Empleado` FOREIGN KEY (`idEmpleado`)
    REFERENCES `tblEmpleado` (`idEmpleado`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblProductoVenta | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7009
CREATE TABLE `tblProductoVenta` (
  `idProductoVenta` INT NOT NULL,
  `tipoItemProductoVenta` CHAR(1) NOT NULL,
  `unidadMedidaProductoVenta` VARCHAR(30) NOT NULL,
  `tasaImpuestoProductoVenta` DECIMAL(3,2) NOT NULL DEFAULT 0,
  `activoProductoVenta` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ProductoVenta` PRIMARY KEY (`idProductoVenta`),
  CONSTRAINT `ckProductoVentatasaImpuestoProductoVentaRango` CHECK (tasaImpuestoProductoVenta BETWEEN 0 AND 0.99),
  CONSTRAINT `ckProductoVentaactivoProductoVentaBooleano` CHECK (activoProductoVenta IN (0,1)),
  CONSTRAINT `ckProductoVentaTipoItemValido` CHECK (tipoItemProductoVenta IN ('B','S'))
) ENGINE=InnoDB;

ALTER TABLE `tblProductoVenta`
  ADD CONSTRAINT `Fk_ProductoVenta_Producto` FOREIGN KEY (`idProductoVenta`)
    REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblFormaPagoVenta | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7009
CREATE TABLE `tblFormaPagoVenta` (
  `idFormaPagoVenta` INT NOT NULL AUTO_INCREMENT,
  `codigoFormaPagoVenta` VARCHAR(30) NOT NULL,
  `nombreFormaPagoVenta` VARCHAR(100) NOT NULL,
  `requiereReferenciaFormaPagoVenta` BOOLEAN NOT NULL DEFAULT FALSE,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_FormaPagoVenta` PRIMARY KEY (`idFormaPagoVenta`),
  CONSTRAINT `uqFormaPagoVentacodigo` UNIQUE (`codigoFormaPagoVenta`),
  CONSTRAINT `ckFormaPagoVentarequiereReferenciaFormaPagoVentaBooleano` CHECK (requiereReferenciaFormaPagoVenta IN (0,1))
) ENGINE=InnoDB;

-- tblCierreMovimientoCxCEncabezado | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblCierreMovimientoCxCEncabezado` (
  `idCierreMovimientoCxCEncabezado` INT NOT NULL AUTO_INCREMENT,
  `numeroCierreMovimientoCxCEncabezado` VARCHAR(30) NOT NULL,
  `fechaDesdeCierreMovimientoCxCEncabezado` DATE NOT NULL,
  `fechaHastaCierreMovimientoCxCEncabezado` DATE NOT NULL,
  `fechaCorteCierreMovimientoCxCEncabezado` DATE NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `tipoCierreMovimientoCxCEncabezado` VARCHAR(20) NOT NULL,
  `estadoCierreMovimientoCxCEncabezado` VARCHAR(20) NOT NULL,
  `documentosRevisadosCierreMovimientoCxCEncabezado` BOOLEAN NOT NULL DEFAULT 0,
  `pagosConciliadosCierreMovimientoCxCEncabezado` BOOLEAN NOT NULL DEFAULT 0,
  `observacionesCierreMovimientoCxCEncabezado` TEXT NULL,
  `idUsuarioResponsable` INT NOT NULL,
  `fechaConfirmacionCierreMovimientoCxCEncabezado` DATETIME NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CierreMovimientoCxCEncabezado` PRIMARY KEY (`idCierreMovimientoCxCEncabezado`),
  CONSTRAINT `uqCierreMovimientoCxCEncabezadonumero` UNIQUE (`numeroCierreMovimientoCxCEncabezado`),
  KEY `idxCierreMovimientoCxCEncabezadoMonedaCodigofechaDesdefechaHasta` (`idMoneda`, `fechaDesdeCierreMovimientoCxCEncabezado`, `fechaHastaCierreMovimientoCxCEncabezado`),
  KEY `idxCierreMovimientoCxCEncabezadoUsuarioResponsable` (`idUsuarioResponsable`),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadodocumentosRevisadosCierra3aaae2a` CHECK (documentosRevisadosCierreMovimientoCxCEncabezado IN (0,1)),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadopagosConciliadosCierreMob8223bf3` CHECK (pagosConciliadosCierreMovimientoCxCEncabezado IN (0,1)),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadoValoresTipoCierre` CHECK (tipoCierreMovimientoCxCEncabezado IN ('MENSUAL','ANUAL')),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadoValoresEstadoCierre` CHECK (estadoCierreMovimientoCxCEncabezado IN ('BORRADOR','CONFIRMADO','ANULADO')),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadoFechasCoherentes` CHECK (fechaHastaCierreMovimientoCxCEncabezado >= fechaDesdeCierreMovimientoCxCEncabezado)
) ENGINE=InnoDB;

ALTER TABLE `tblCierreMovimientoCxCEncabezado`
  ADD CONSTRAINT `Fk_CierreMovimientoCxCEncabezado_Usuario_Responsable` FOREIGN KEY (`idUsuarioResponsable`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CierreMovimientoCxCEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblControlPeriodoCxC | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblControlPeriodoCxC` (
  `idControlPeriodoCxC` INT NOT NULL AUTO_INCREMENT,
  `idMoneda` BIGINT NOT NULL,
  `fechaUltimoCierreControlPeriodoCxC` DATE NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ControlPeriodoCxC` PRIMARY KEY (`idControlPeriodoCxC`),
  CONSTRAINT `uqControlPeriodoCxCClaveNegocio` UNIQUE (`idMoneda`)
) ENGINE=InnoDB;

ALTER TABLE `tblControlPeriodoCxC`
  ADD CONSTRAINT `Fk_ControlPeriodoCxC_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblCuentaContableCxC | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7000
CREATE TABLE `tblCuentaContableCxC` (
  `idCuentaContableCxC` INT NOT NULL AUTO_INCREMENT,
  `idMoneda` BIGINT NOT NULL,
  `tipoCuentaContableCxC` VARCHAR(30) NOT NULL,
  `cuentaClientesCuentaContableCxC` VARCHAR(30) NOT NULL,
  `cuentaContrapartidaCuentaContableCxC` VARCHAR(30) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CuentaContableCxC` PRIMARY KEY (`idCuentaContableCxC`),
  CONSTRAINT `uqCuentaContableCxCClaveNegocio` UNIQUE (`idMoneda`, `tipoCuentaContableCxC`)
) ENGINE=InnoDB;

ALTER TABLE `tblCuentaContableCxC`
  ADD CONSTRAINT `Fk_CuentaContableCxC_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblCierreSaldoCliente | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblCierreSaldoCliente` (
  `idCierreSaldoCliente` INT NOT NULL AUTO_INCREMENT,
  `idCierreMovimientoCxCEncabezado` INT NOT NULL,
  `idCliente` INT NOT NULL,
  `codigoClienteCierreSaldoCliente` VARCHAR(30) NOT NULL,
  `nombreClienteCierreSaldoCliente` VARCHAR(100) NOT NULL,
  `saldoAnteriorCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `cargosPeriodoCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `abonosPeriodoCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `cargosAcumuladosCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `abonosAcumuladosCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `saldoActualCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CierreSaldoCliente` PRIMARY KEY (`idCierreMovimientoCxCEncabezado`, `idCierreSaldoCliente`),
  KEY `idxCierreSaldoClienteClienteCodigo` (`idCliente`),
  CONSTRAINT `uqCierreSaldoClienteClaveNegocio` UNIQUE (`idCierreMovimientoCxCEncabezado`, `idCliente`),
  CONSTRAINT `uqCierreSaldoClienteId` UNIQUE (`idCierreSaldoCliente`)
) ENGINE=InnoDB;

ALTER TABLE `tblCierreSaldoCliente`
  ADD CONSTRAINT `Fk_CierreSaldoCliente_CierreMovimientoCxCEncabezado` FOREIGN KEY (`idCierreMovimientoCxCEncabezado`)
    REFERENCES `tblCierreMovimientoCxCEncabezado` (`idCierreMovimientoCxCEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CierreSaldoCliente_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblTrasladoCxCEncabezado | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblTrasladoCxCEncabezado` (
  `idTrasladoCxCEncabezado` INT NOT NULL AUTO_INCREMENT,
  `idCierreMovimientoCxCEncabezado` INT NOT NULL,
  `claveIntegracionTrasladoCxCEncabezado` VARCHAR(80) NOT NULL,
  `estadoTrasladoCxCEncabezado` VARCHAR(20) NOT NULL DEFAULT 'PENDIENTE',
  `referenciaContableTrasladoCxCEncabezado` VARCHAR(100) NULL,
  `idUsuario` INT NOT NULL,
  `fechaConfirmacionTrasladoCxCEncabezado` DATETIME NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_TrasladoCxCEncabezado` PRIMARY KEY (`idTrasladoCxCEncabezado`),
  CONSTRAINT `uqTrasladoCxCEncabezadoclaveIntegracion` UNIQUE (`claveIntegracionTrasladoCxCEncabezado`),
  KEY `idxTrasladoCxCEncabezadoUsuarioCodigo` (`idUsuario`),
  CONSTRAINT `ckTrasladoCxCEncabezadoValoresEstado` CHECK (estadoTrasladoCxCEncabezado IN ('PENDIENTE','PREPARADO','ENVIADO','CONFIRMADO','RECHAZADO')),
  CONSTRAINT `uqTrasladoCxCEncabezadoClaveNegocio` UNIQUE (`idCierreMovimientoCxCEncabezado`)
) ENGINE=InnoDB;

ALTER TABLE `tblTrasladoCxCEncabezado`
  ADD CONSTRAINT `Fk_TrasladoCxCEncabezado_CierreMovimientoCxCEncabezado` FOREIGN KEY (`idCierreMovimientoCxCEncabezado`)
    REFERENCES `tblCierreMovimientoCxCEncabezado` (`idCierreMovimientoCxCEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_TrasladoCxCEncabezado_Usuario` FOREIGN KEY (`idUsuario`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblTrasladoCxCDetalle | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblTrasladoCxCDetalle` (
  `idTrasladoCxCDetalle` INT NOT NULL AUTO_INCREMENT,
  `idTrasladoCxCEncabezado` INT NOT NULL,
  `numeroLineaTrasladoCxCDetalle` INT NOT NULL,
  `cuentaTrasladoCxCDetalle` VARCHAR(30) NOT NULL,
  `descripcionTrasladoCxCDetalle` VARCHAR(100) NOT NULL,
  `debeTrasladoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `haberTrasladoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_TrasladoCxCDetalle` PRIMARY KEY (`idTrasladoCxCEncabezado`, `idTrasladoCxCDetalle`),
  CONSTRAINT `ckTrasladoCxCDetalleDebeHaberNoNegativos` CHECK (debeTrasladoCxCDetalle >= 0 AND haberTrasladoCxCDetalle >= 0),
  CONSTRAINT `uqTrasladoCxCDetalleClaveNegocio` UNIQUE (`idTrasladoCxCEncabezado`, `numeroLineaTrasladoCxCDetalle`),
  CONSTRAINT `uqTrasladoCxCDetalleId` UNIQUE (`idTrasladoCxCDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblTrasladoCxCDetalle`
  ADD CONSTRAINT `Fk_TrasladoCxCDetalle_TrasladoCxCEncabezado` FOREIGN KEY (`idTrasladoCxCEncabezado`)
    REFERENCES `tblTrasladoCxCEncabezado` (`idTrasladoCxCEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblCierreMovimientoCxCDetalle | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblCierreMovimientoCxCDetalle` (
  `idCierreMovimientoCxCDetalle` INT NOT NULL AUTO_INCREMENT,
  `idCierreMovimientoCxCEncabezado` INT NOT NULL,
  `idFacturaEncabezado` INT NOT NULL,
  `saldoInicialCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `cargosPeriodoCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `abonosPeriodoCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `saldoFinalCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CierreMovimientoCxCDetalle` PRIMARY KEY (`idCierreMovimientoCxCEncabezado`, `idCierreMovimientoCxCDetalle`),
  CONSTRAINT `uqCierreMovimientoCxCDetalleCierreMovimientoCxCEncabeza4f1c6502` UNIQUE (`idCierreMovimientoCxCEncabezado`, `idFacturaEncabezado`),
  KEY `idxCierreMovimientoCxCDetalleFacturaEncabezadoCodigo` (`idFacturaEncabezado`),
  CONSTRAINT `uqCierreMovimientoCxCDetalleId` UNIQUE (`idCierreMovimientoCxCDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblCierreMovimientoCxCDetalle`
  ADD CONSTRAINT `Fk_CierreMovimientoCxCDetalle_CierreMovimientoCxCEncabezado` FOREIGN KEY (`idCierreMovimientoCxCEncabezado`)
    REFERENCES `tblCierreMovimientoCxCEncabezado` (`idCierreMovimientoCxCEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CierreMovimientoCxCDetalle_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT;

SET FOREIGN_KEY_CHECKS = @ventas_fk_checks_anterior;
