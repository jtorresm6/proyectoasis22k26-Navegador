CREATE DATABASE IF NOT EXISTS `dbsistemaembutidos`
  DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `dbsistemaembutidos`;

CREATE TABLE IF NOT EXISTS `Vendedores` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(120) NOT NULL,
  `porcentaje` DECIMAL(7,4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  CONSTRAINT `ck_vendedores_porcentaje` CHECK (`porcentaje` >= 0 AND `porcentaje` <= 100)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `Clientes` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(160) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `ClienteVendedor` (
  `id_cliente` INT NOT NULL,
  `id_vendedor` INT NOT NULL,
  PRIMARY KEY (`id_cliente`, `id_vendedor`),
  CONSTRAINT `fk_clientevendedor_cliente` FOREIGN KEY (`id_cliente`) REFERENCES `Clientes` (`id`),
  CONSTRAINT `fk_clientevendedor_vendedor` FOREIGN KEY (`id_vendedor`) REFERENCES `Vendedores` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `Productos` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(160) NOT NULL,
  `marca` VARCHAR(100) NOT NULL,
  `linea` VARCHAR(100) NOT NULL,
  `categoria` VARCHAR(100) NOT NULL,
  `porcentaje` DECIMAL(7,4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `Facturas` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `id_cliente` INT NOT NULL,
  `fecha` DATETIME NOT NULL,
  `total` DECIMAL(18,2) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ix_facturas_fecha_cliente` (`fecha`, `id_cliente`),
  CONSTRAINT `fk_facturas_cliente` FOREIGN KEY (`id_cliente`) REFERENCES `Clientes` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `DetalleFactura` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `id_factura` INT NOT NULL,
  `id_producto` INT NOT NULL,
  `cantidad` DECIMAL(18,3) NOT NULL,
  `precio_unitario` DECIMAL(18,2) NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_detalle_factura` FOREIGN KEY (`id_factura`) REFERENCES `Facturas` (`id`),
  CONSTRAINT `fk_detalle_producto` FOREIGN KEY (`id_producto`) REFERENCES `Productos` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `MovimientosCliente` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `id_factura` INT NOT NULL,
  `fecha` DATETIME NOT NULL,
  `tipo` VARCHAR(30) NOT NULL,
  `monto` DECIMAL(18,2) NOT NULL,
  `saldo` DECIMAL(18,2) NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_movimientos_factura` FOREIGN KEY (`id_factura`) REFERENCES `Facturas` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `ComisionCalculada` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `id_vendedor` INT NOT NULL,
  `fecha_ini` DATE NOT NULL,
  `fecha_fin` DATE NOT NULL,
  `metodo` VARCHAR(30) NOT NULL,
  `base` VARCHAR(20) NOT NULL,
  `monto` DECIMAL(18,2) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ix_comision_calculada_periodo` (`id_vendedor`, `fecha_ini`, `fecha_fin`, `metodo`, `base`),
  CONSTRAINT `fk_comision_vendedor` FOREIGN KEY (`id_vendedor`) REFERENCES `Vendedores` (`id`),
  CONSTRAINT `ck_comision_rango` CHECK (`fecha_ini` <= `fecha_fin`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT INTO `Vendedores` (`nombre`, `porcentaje`)
SELECT 'Ana López', 2.0000 WHERE NOT EXISTS (SELECT 1 FROM `Vendedores` WHERE `nombre` = 'Ana López');
INSERT INTO `Vendedores` (`nombre`, `porcentaje`)
SELECT 'Mario Castillo', 1.5000 WHERE NOT EXISTS (SELECT 1 FROM `Vendedores` WHERE `nombre` = 'Mario Castillo');

INSERT INTO `Clientes` (`nombre`)
SELECT 'Distribuidora El Roble' WHERE NOT EXISTS (SELECT 1 FROM `Clientes` WHERE `nombre` = 'Distribuidora El Roble');
INSERT INTO `Clientes` (`nombre`)
SELECT 'Abarrotería San Miguel' WHERE NOT EXISTS (SELECT 1 FROM `Clientes` WHERE `nombre` = 'Abarrotería San Miguel');

INSERT INTO `Productos` (`nombre`, `marca`, `linea`, `categoria`, `porcentaje`)
SELECT 'Jamón de pavo familiar', 'Embutidos de Calidad', 'Embutidos cocidos', 'Jamones', 8.0000
WHERE NOT EXISTS (SELECT 1 FROM `Productos` WHERE `nombre` = 'Jamón de pavo familiar');
INSERT INTO `Productos` (`nombre`, `marca`, `linea`, `categoria`, `porcentaje`)
SELECT 'Queso mozzarella', 'Embutidos de Calidad', 'Lácteos', 'Quesos', 4.5000
WHERE NOT EXISTS (SELECT 1 FROM `Productos` WHERE `nombre` = 'Queso mozzarella');

INSERT INTO `ClienteVendedor` (`id_cliente`, `id_vendedor`)
SELECT c.`id`, v.`id` FROM `Clientes` c CROSS JOIN `Vendedores` v
WHERE c.`nombre` = 'Distribuidora El Roble' AND v.`nombre` = 'Ana López'
  AND NOT EXISTS (SELECT 1 FROM `ClienteVendedor` cv WHERE cv.`id_cliente` = c.`id` AND cv.`id_vendedor` = v.`id`);
INSERT INTO `ClienteVendedor` (`id_cliente`, `id_vendedor`)
SELECT c.`id`, v.`id` FROM `Clientes` c CROSS JOIN `Vendedores` v
WHERE c.`nombre` = 'Abarrotería San Miguel' AND v.`nombre` = 'Mario Castillo'
  AND NOT EXISTS (SELECT 1 FROM `ClienteVendedor` cv WHERE cv.`id_cliente` = c.`id` AND cv.`id_vendedor` = v.`id`);

INSERT INTO `Facturas` (`id_cliente`, `fecha`, `total`)
SELECT c.`id`, '2026-10-12 10:00:00', 100000.00 FROM `Clientes` c
WHERE c.`nombre` = 'Distribuidora El Roble'
  AND NOT EXISTS (SELECT 1 FROM `Facturas` f WHERE f.`id_cliente` = c.`id` AND f.`fecha` = '2026-10-12 10:00:00');
INSERT INTO `Facturas` (`id_cliente`, `fecha`, `total`)
SELECT c.`id`, '2026-10-16 11:00:00', 50000.00 FROM `Clientes` c
WHERE c.`nombre` = 'Abarrotería San Miguel'
  AND NOT EXISTS (SELECT 1 FROM `Facturas` f WHERE f.`id_cliente` = c.`id` AND f.`fecha` = '2026-10-16 11:00:00');

INSERT INTO `DetalleFactura` (`id_factura`, `id_producto`, `cantidad`, `precio_unitario`)
SELECT f.`id`, p.`id`, 20, 5000.00 FROM `Facturas` f CROSS JOIN `Productos` p
WHERE f.`total` = 100000.00 AND p.`nombre` = 'Jamón de pavo familiar'
  AND NOT EXISTS (SELECT 1 FROM `DetalleFactura` d WHERE d.`id_factura` = f.`id` AND d.`id_producto` = p.`id`);
INSERT INTO `DetalleFactura` (`id_factura`, `id_producto`, `cantidad`, `precio_unitario`)
SELECT f.`id`, p.`id`, 10, 5000.00 FROM `Facturas` f CROSS JOIN `Productos` p
WHERE f.`total` = 50000.00 AND p.`nombre` = 'Queso mozzarella'
  AND NOT EXISTS (SELECT 1 FROM `DetalleFactura` d WHERE d.`id_factura` = f.`id` AND d.`id_producto` = p.`id`);

INSERT INTO `MovimientosCliente` (`id_factura`, `fecha`, `tipo`, `monto`, `saldo`)
SELECT f.`id`, f.`fecha`, 'PAGO', f.`total`, 0 FROM `Facturas` f
WHERE NOT EXISTS (SELECT 1 FROM `MovimientosCliente` m WHERE m.`id_factura` = f.`id`);
