-- ============================================================================
-- PREPARAR CATALOGOS DE PRUEBA PARA MOVIMIENTOS BANCARIOS
-- Modulo: Bancos / Movimientos Bancarios
--
-- Objetivo:
--   1. Completar las dos piezas de estructura que el formulario necesita para
--      asociar beneficiarios con cuentas bancarias de terceros.
--   2. Crear catalogos minimos de prueba para concepto, forma de pago y tipo
--      de transaccion.
--   3. Crear un proveedor, un cliente y cuentas de terceros identificables
--      como datos de prueba.
--
-- El script es idempotente: se puede ejecutar mas de una vez sin duplicar los
-- registros identificados por sus codigos o numeros de cuenta.
-- ============================================================================

-- ----------------------------------------------------------------------------
-- 1. Estructura que ya forma parte de bd_Bancos_Completo.sql, pero que puede
--    faltar si la base activa se creo antes de incorporar esos cambios.
-- ----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS tblCuentaBancariaTercero (
    idCuentaBancariaTercero BIGINT NOT NULL AUTO_INCREMENT,
    numeroCuentaBancariaTercero VARCHAR(100) NOT NULL,
    nombreTitularCuentaBancariaTercero VARCHAR(255) NOT NULL,
    idBanco BIGINT NOT NULL,
    idMoneda BIGINT NOT NULL,
    idEmpleado INT NULL,
    idProveedor INT NULL,
    idCliente INT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_CuentaBancariaTercero PRIMARY KEY (idCuentaBancariaTercero),
    CONSTRAINT uqCuentaBancariaTercero UNIQUE (idBanco, numeroCuentaBancariaTercero),
    CONSTRAINT ckCuentaBancariaTerceroPropietario CHECK (
        (idEmpleado IS NOT NULL) +
        (idProveedor IS NOT NULL) +
        (idCliente IS NOT NULL) = 1
    ),
    CONSTRAINT Fk_CuentaBancariaTercero_Banco
        FOREIGN KEY (idBanco) REFERENCES tblBanco(idBanco),
    CONSTRAINT Fk_CuentaBancariaTercero_Moneda
        FOREIGN KEY (idMoneda) REFERENCES tblMoneda(idMoneda),
    CONSTRAINT Fk_CuentaBancariaTercero_Empleado
        FOREIGN KEY (idEmpleado) REFERENCES tblEmpleado(idEmpleado),
    CONSTRAINT Fk_CuentaBancariaTercero_Proveedor
        FOREIGN KEY (idProveedor) REFERENCES tblProveedor(idProveedor),
    CONSTRAINT Fk_CuentaBancariaTercero_Cliente
        FOREIGN KEY (idCliente) REFERENCES tblCliente(idCliente),
    INDEX idxCuentaTerceroEmpleado (idEmpleado),
    INDEX idxCuentaTerceroProveedor (idProveedor),
    INDEX idxCuentaTerceroCliente (idCliente),
    CONSTRAINT ckCuentaTerceroActivo CHECK (is_active IN (0,1))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

SET @existe_columna_cuenta_tercero := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'tblMovimientoBancarioTercero'
      AND COLUMN_NAME = 'idCuentaBancariaTercero'
);
SET @sql_columna_cuenta_tercero := IF(
    @existe_columna_cuenta_tercero = 0,
    'ALTER TABLE tblMovimientoBancarioTercero ADD COLUMN idCuentaBancariaTercero BIGINT NULL',
    'SELECT 1'
);
PREPARE stmt_columna_cuenta_tercero FROM @sql_columna_cuenta_tercero;
EXECUTE stmt_columna_cuenta_tercero;
DEALLOCATE PREPARE stmt_columna_cuenta_tercero;

SET @existe_indice_cuenta_tercero := (
    SELECT COUNT(*)
    FROM information_schema.STATISTICS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'tblMovimientoBancarioTercero'
      AND INDEX_NAME = 'idxMovimientoTerceroCuenta'
);
SET @sql_indice_cuenta_tercero := IF(
    @existe_indice_cuenta_tercero = 0,
    'ALTER TABLE tblMovimientoBancarioTercero ADD INDEX idxMovimientoTerceroCuenta (idCuentaBancariaTercero)',
    'SELECT 1'
);
PREPARE stmt_indice_cuenta_tercero FROM @sql_indice_cuenta_tercero;
EXECUTE stmt_indice_cuenta_tercero;
DEALLOCATE PREPARE stmt_indice_cuenta_tercero;

SET @existe_fk_cuenta_tercero := (
    SELECT COUNT(*)
    FROM information_schema.REFERENTIAL_CONSTRAINTS
    WHERE CONSTRAINT_SCHEMA = DATABASE()
      AND TABLE_NAME = 'tblMovimientoBancarioTercero'
      AND CONSTRAINT_NAME = 'Fk_MovimientoTercero_CuentaBancariaTercero'
);
SET @sql_fk_cuenta_tercero := IF(
    @existe_fk_cuenta_tercero = 0,
    'ALTER TABLE tblMovimientoBancarioTercero ADD CONSTRAINT Fk_MovimientoTercero_CuentaBancariaTercero FOREIGN KEY (idCuentaBancariaTercero) REFERENCES tblCuentaBancariaTercero(idCuentaBancariaTercero) ON DELETE RESTRICT ON UPDATE RESTRICT',
    'SELECT 1'
);
PREPARE stmt_fk_cuenta_tercero FROM @sql_fk_cuenta_tercero;
EXECUTE stmt_fk_cuenta_tercero;
DEALLOCATE PREPARE stmt_fk_cuenta_tercero;

-- ----------------------------------------------------------------------------
-- 2. Catalogos del formulario.
-- ----------------------------------------------------------------------------
INSERT INTO tblMetodoPago
    (nombreMetodoPago, descripcionMetodoPago, is_active)
VALUES
    ('Transferencia bancaria', 'Transferencia electronica entre cuentas bancarias.', 1),
    ('Cheque', 'Pago emitido mediante cheque bancario.', 1),
    ('Operacion bancaria', 'Cargo o abono aplicado directamente por el banco.', 1)
ON DUPLICATE KEY UPDATE
    descripcionMetodoPago = VALUES(descripcionMetodoPago),
    is_active = 1;

INSERT INTO tblTipoTransaccion
    (codigoTipoTransaccion, nombreTipoTransaccion, naturalezaTipoTransaccion, is_active)
VALUES
    ('TRANS-001', 'Banca en linea', 'AMBOS', 1),
    ('TRANS-002', 'Nota de debito', 'EGRESO', 1),
    ('TRANS-003', 'Interes ganado', 'INGRESO', 1)
ON DUPLICATE KEY UPDATE
    nombreTipoTransaccion = VALUES(nombreTipoTransaccion),
    naturalezaTipoTransaccion = VALUES(naturalezaTipoTransaccion),
    is_active = 1;

INSERT INTO tblConceptoBancario
    (
        codigoConceptoBancario,
        nombreConceptoBancario,
        descripcionConceptoBancario,
        naturalezaConceptoBancario,
        claseOperacionConceptoBancario,
        tipoTerceroConceptoBancario,
        is_active
    )
VALUES
    ('PRUEBA-COBRO-CLIENTE', 'Cobro a cliente',
     'Ingreso bancario recibido de un cliente.',
     'INGRESO', 'NORMAL', 'CLIENTE', 1),
    ('PRUEBA-PAGO-PROVEEDOR', 'Pago a proveedor',
     'Egreso bancario para cancelar una obligacion con un proveedor.',
     'EGRESO', 'NORMAL', 'PROVEEDOR', 1),
    ('PRUEBA-PAGO-EMPLEADO', 'Pago a empleado',
     'Egreso bancario dirigido a un empleado.',
     'EGRESO', 'NORMAL', 'EMPLEADO', 1),
    ('PRUEBA-OTRO-INGRESO', 'Otro ingreso bancario',
     'Ingreso sin beneficiario externo asociado.',
     'INGRESO', 'NORMAL', 'NINGUNO', 1),
    ('PRUEBA-GASTO-BANCARIO', 'Gasto bancario',
     'Egreso sin beneficiario externo asociado.',
     'EGRESO', 'NORMAL', 'NINGUNO', 1)
ON DUPLICATE KEY UPDATE
    nombreConceptoBancario = VALUES(nombreConceptoBancario),
    descripcionConceptoBancario = VALUES(descripcionConceptoBancario),
    naturalezaConceptoBancario = VALUES(naturalezaConceptoBancario),
    claseOperacionConceptoBancario = VALUES(claseOperacionConceptoBancario),
    tipoTerceroConceptoBancario = VALUES(tipoTerceroConceptoBancario),
    is_active = 1;

-- ----------------------------------------------------------------------------
-- 3. Beneficiarios de prueba. Se usa un empleado activo existente y se crean
--    un proveedor y un cliente identificados expresamente como PRUEBA BANCOS.
-- ----------------------------------------------------------------------------
SET @id_moneda_gtq := (
    SELECT idMoneda FROM tblMoneda
    WHERE codigoMoneda = 'GTQ' AND is_active = 1
    ORDER BY idMoneda LIMIT 1
);
SET @id_banco_prueba := (
    SELECT idBanco FROM tblBanco
    WHERE codigoBanco = 'BI' AND is_active = 1
    ORDER BY idBanco LIMIT 1
);
SET @id_empleado_prueba := (
    SELECT idEmpleado FROM tblEmpleado
    WHERE is_active = 1
    ORDER BY idEmpleado LIMIT 1
);

INSERT INTO tblProveedor
    (
        nombreProveedor, nitProveedor, correoProveedor,
        telefonoProveedor, direccionProveedor, estadoProveedor
    )
VALUES
    (
        'PROVEEDOR PRUEBA BANCOS', 'TESTB00001',
        'proveedor.prueba.bancos@example.test', '0000-0001',
        'Direccion de prueba para el modulo de bancos', 1
    )
ON DUPLICATE KEY UPDATE
    nombreProveedor = VALUES(nombreProveedor),
    correoProveedor = VALUES(correoProveedor),
    telefonoProveedor = VALUES(telefonoProveedor),
    direccionProveedor = VALUES(direccionProveedor),
    estadoProveedor = 1;

INSERT INTO tblCliente
    (
        codigoCliente, tipoIdentificacionCliente, nombreCliente,
        direccionCliente, nombreContactoCliente, telefonoCliente,
        correoCliente, diasCreditoCliente, limiteCreditoCliente,
        idMonedaCredito, activoCliente, nitCliente, dpiCliente
    )
VALUES
    (
        'CLI-PRUEBA-BANCOS', 'NIT', 'CLIENTE PRUEBA BANCOS',
        'Direccion de prueba para el modulo de bancos',
        'CONTACTO PRUEBA BANCOS', '0000-0002',
        'cliente.prueba.bancos@example.test', 0, 0.0000,
        @id_moneda_gtq, 1, 'TESTC00001', NULL
    )
ON DUPLICATE KEY UPDATE
    nombreCliente = VALUES(nombreCliente),
    direccionCliente = VALUES(direccionCliente),
    nombreContactoCliente = VALUES(nombreContactoCliente),
    telefonoCliente = VALUES(telefonoCliente),
    correoCliente = VALUES(correoCliente),
    idMonedaCredito = VALUES(idMonedaCredito),
    activoCliente = 1;

SET @id_proveedor_prueba := (
    SELECT idProveedor FROM tblProveedor
    WHERE nitProveedor = 'TESTB00001'
    ORDER BY idProveedor LIMIT 1
);
SET @id_cliente_prueba := (
    SELECT idCliente FROM tblCliente
    WHERE codigoCliente = 'CLI-PRUEBA-BANCOS'
    ORDER BY idCliente LIMIT 1
);

INSERT INTO tblCuentaBancariaTercero
    (
        numeroCuentaBancariaTercero,
        nombreTitularCuentaBancariaTercero,
        idBanco, idMoneda,
        idEmpleado, idProveedor, idCliente,
        is_active
    )
VALUES
    ('PRUEBA-TER-EMP-001', 'EMPLEADO PRUEBA BANCOS',
     @id_banco_prueba, @id_moneda_gtq,
     @id_empleado_prueba, NULL, NULL, 1),
    ('PRUEBA-TER-PROV-001', 'PROVEEDOR PRUEBA BANCOS',
     @id_banco_prueba, @id_moneda_gtq,
     NULL, @id_proveedor_prueba, NULL, 1),
    ('PRUEBA-TER-CLI-001', 'CLIENTE PRUEBA BANCOS',
     @id_banco_prueba, @id_moneda_gtq,
     NULL, NULL, @id_cliente_prueba, 1)
ON DUPLICATE KEY UPDATE
    nombreTitularCuentaBancariaTercero = VALUES(nombreTitularCuentaBancariaTercero),
    idMoneda = VALUES(idMoneda),
    idEmpleado = VALUES(idEmpleado),
    idProveedor = VALUES(idProveedor),
    idCliente = VALUES(idCliente),
    is_active = 1;

-- ----------------------------------------------------------------------------
-- 4. Verificacion visible al finalizar la ejecucion manual del script.
-- ----------------------------------------------------------------------------
SELECT 'Conceptos activos' AS verificacion, COUNT(*) AS total
FROM tblConceptoBancario WHERE is_active = 1
UNION ALL
SELECT 'Metodos de pago activos', COUNT(*)
FROM tblMetodoPago WHERE is_active = 1
UNION ALL
SELECT 'Tipos de transaccion activos', COUNT(*)
FROM tblTipoTransaccion WHERE is_active = 1
UNION ALL
SELECT 'Cuentas de terceros de prueba', COUNT(*)
FROM tblCuentaBancariaTercero
WHERE numeroCuentaBancariaTercero LIKE 'PRUEBA-TER-%'
  AND is_active = 1;
