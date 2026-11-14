USE dbsistemaembutidos;

START TRANSACTION;

-- Catálogos de apoyo (solo se crean si faltan)
INSERT INTO tblBanco (codigoBanco, nombreBanco)
SELECT 'TSTCONC', 'Banco de pruebas conciliacion'
WHERE NOT EXISTS (SELECT 1 FROM tblBanco WHERE codigoBanco = 'TSTCONC');

INSERT INTO tblMoneda (idMoneda, codigoMoneda, nombreMoneda, simboloMoneda)
SELECT x.nid, 'GTQ', 'Quetzal', 'Q'
FROM (SELECT IFNULL(MAX(idMoneda), 0) + 1 AS nid FROM tblMoneda) x
WHERE NOT EXISTS (SELECT 1 FROM tblMoneda WHERE codigoMoneda = 'GTQ');

INSERT INTO tblTipoCuentaBancaria (codigoTipoCuentaBancaria, nombreTipoCuentaBancaria)
SELECT 'TSTCONC', 'Cuenta de pruebas conciliacion'
WHERE NOT EXISTS (SELECT 1 FROM tblTipoCuentaBancaria WHERE codigoTipoCuentaBancaria = 'TSTCONC');

INSERT INTO tblTipoTransaccion (codigoTipoTransaccion, nombreTipoTransaccion, naturalezaTipoTransaccion)
SELECT 'TSTCONC-ING', 'Ingreso de prueba conciliacion', 'INGRESO'
WHERE NOT EXISTS (SELECT 1 FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TSTCONC-ING');

INSERT INTO tblTipoTransaccion (codigoTipoTransaccion, nombreTipoTransaccion, naturalezaTipoTransaccion)
SELECT 'TSTCONC-EGR', 'Egreso de prueba conciliacion', 'EGRESO'
WHERE NOT EXISTS (SELECT 1 FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TSTCONC-EGR');

SET @Banco = (SELECT idBanco FROM tblBanco WHERE codigoBanco = 'TSTCONC' LIMIT 1);
SET @Moneda = (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ' LIMIT 1);
SET @TipoCuenta = (SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria WHERE codigoTipoCuentaBancaria = 'TSTCONC' LIMIT 1);
SET @TipoIngreso = (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TSTCONC-ING' LIMIT 1);
SET @TipoEgreso = (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TSTCONC-EGR' LIMIT 1);
SET @Hoy = CURRENT_DATE();

-- Cuenta de prueba
INSERT INTO tblCuentaBancaria
    (numeroCuentaBancaria, nombreCuentaBancaria, limiteSobregiroCuentaBancaria,
     saldoInicialCuentaBancaria, Fk_Banco_Codigo, Fk_Moneda_Codigo,
     Fk_TipoCuentaBancaria_Codigo, created_at)
SELECT 'TST-CLIC-0001', 'Cuenta pruebas marcado con clic', 0.0000,
       10000.0000, @Banco, @Moneda, @TipoCuenta, DATE_SUB(NOW(), INTERVAL 30 DAY)
WHERE NOT EXISTS (SELECT 1 FROM tblCuentaBancaria WHERE numeroCuentaBancaria = 'TST-CLIC-0001');

SET @Cuenta = (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = 'TST-CLIC-0001' LIMIT 1);

-- Movimientos del libro
INSERT INTO tblMovimientoBancario
    (referenciaMovimientoBancario, conceptoMovimientoBancario,
     importeMovimientoBancario, importeGTQMovimientoBancario,
     tipoCambioAplicadoMovimientoBancario, sentidoMovimientoBancario,
     fechaOperacionMovimientoBancario, fechaValorMovimientoBancario,
     origenMovimientoBancario, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
SELECT v.ref, v.concepto, v.importe, v.importe, 1.0000, v.sentido,
       DATE_SUB(@Hoy, INTERVAL v.dias DAY), DATE_SUB(@Hoy, INTERVAL v.dias DAY),
       'MANUAL', @Cuenta,
       CASE WHEN v.sentido = 1 THEN @TipoIngreso ELSE @TipoEgreso END
FROM (
    SELECT 'TSTK-L01' AS ref, 'Cobro cliente factura 3001' AS concepto, 5000.0000 AS importe,  1 AS sentido, 8 AS dias
    UNION ALL SELECT 'TSTK-L02', 'Pago proveedor papeleria',     1250.5000, -1, 7
    UNION ALL SELECT 'TSTK-L03', 'Pago proveedor materia prima', 2000.0000, -1, 5
    UNION ALL SELECT 'TSTK-L04', 'Cobro cliente factura 3002',    700.0000,  1, 9
    UNION ALL SELECT 'TSTK-L05', 'Pago servicio de agua',         475.2500, -1, 2
    UNION ALL SELECT 'TSTK-L06', 'Cobro cliente factura 3003',    600.0000,  1, 3
    UNION ALL SELECT 'TSTK-L07', 'Pago de fletes',                300.0000, -1, 2
    UNION ALL SELECT 'TSTK-L08', 'Pago de mantenimiento',         120.0000, -1, 4
) v
WHERE @Cuenta IS NOT NULL AND @TipoIngreso IS NOT NULL AND @TipoEgreso IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM tblMovimientoBancario m WHERE m.referenciaMovimientoBancario = v.ref);

-- Estado de cuenta (extracto) y sus líneas
INSERT INTO tblExtractoBancario
    (referenciaExtractoBancario, fechaInicioExtractoBancario, fechaFinExtractoBancario,
     saldoInicialExtractoBancario, saldoFinalExtractoBancario, Fk_CuentaBancaria_Codigo)
SELECT 'TSTK-EXTRACTO', DATE_SUB(@Hoy, INTERVAL 10 DAY), @Hoy, 10000.0000, 11083.1500, @Cuenta
WHERE @Cuenta IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM tblExtractoBancario e
                  WHERE e.Fk_CuentaBancaria_Codigo = @Cuenta AND e.referenciaExtractoBancario = 'TSTK-EXTRACTO');

SET @Extracto = (SELECT Pk_ExtractoBancario FROM tblExtractoBancario
                 WHERE Fk_CuentaBancaria_Codigo = @Cuenta AND referenciaExtractoBancario = 'TSTK-EXTRACTO' LIMIT 1);

INSERT INTO tblDetalleExtracto
    (identificadorDetalleExtracto, referenciaDetalleExtracto, conceptoDetalleExtracto,
     fechaOperacionDetalleExtracto, fechaValorDetalleExtracto,
     importeDetalleExtracto, sentidoDetalleExtracto, Fk_ExtractoBancario_Codigo)
SELECT v.ident, v.ref, v.concepto,
       DATE_SUB(@Hoy, INTERVAL v.dias DAY), DATE_SUB(@Hoy, INTERVAL v.dias DAY),
       v.importe, v.sentido, @Extracto
FROM (
    SELECT 'TSTK-B01' AS ident, 'TSTK-L01' AS ref, 'Deposito cliente' AS concepto, 5000.0000 AS importe,  1 AS sentido, 8 AS dias
    UNION ALL SELECT 'TSTK-B02', 'TSTK-L02', 'Debito proveedor',                    1250.5000, -1, 7
    UNION ALL SELECT 'TSTK-B03', 'TSTK-L03', 'Debito proveedor',                    2150.0000, -1, 5
    UNION ALL SELECT 'TSTK-B04', NULL,       'Deposito sin referencia',              700.0000,  1, 3
    UNION ALL SELECT 'TSTK-B05', 'BCO-9001', 'Debito servicio de agua',              475.2500, -1, 1
    UNION ALL SELECT 'TSTK-B06', 'TSTK-L06', 'Cargo por cheque devuelto',            600.0000, -1, 3
    UNION ALL SELECT 'TSTK-B07', 'TSTK-COM', 'Comision bancaria',                     35.0000, -1, 3
    UNION ALL SELECT 'TSTK-B08', 'TSTK-INT', 'Interes ganado',                        12.4000,  1, 1
    UNION ALL SELECT 'TSTK-B09', 'BCO-9002', 'Debito mantenimiento',                 118.5000, -1, 4
) v
WHERE @Extracto IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM tblDetalleExtracto d
                  WHERE d.Fk_ExtractoBancario_Codigo = @Extracto AND d.identificadorDetalleExtracto = v.ident);

COMMIT;

-- Verificación: debe dar 8 movimientos, 1 extracto, 9 líneas
SELECT c.numeroCuentaBancaria AS cuenta,
       (SELECT COUNT(*) FROM tblMovimientoBancario m WHERE m.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria AND m.referenciaMovimientoBancario LIKE 'TSTK-%') AS movimientos,
       (SELECT COUNT(*) FROM tblExtractoBancario e WHERE e.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria AND e.referenciaExtractoBancario = 'TSTK-EXTRACTO') AS extractos,
       (SELECT COUNT(*) FROM tblDetalleExtracto d INNER JOIN tblExtractoBancario e ON e.Pk_ExtractoBancario = d.Fk_ExtractoBancario_Codigo
        WHERE e.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria AND d.identificadorDetalleExtracto LIKE 'TSTK-%') AS lineas
FROM tblCuentaBancaria c WHERE c.numeroCuentaBancaria = 'TST-CLIC-0001';