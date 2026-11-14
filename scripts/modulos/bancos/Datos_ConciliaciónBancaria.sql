-- ================================================================
-- DATOS DE PRUEBA - Conciliacion Bancaria (modulo Bancos)
-- Ejecutar TODO el archivo, de arriba abajo. Se puede ejecutar mas de
-- una vez: cada fila lleva una referencia fija y solo se agrega si
-- todavia no existe.
--
-- Crea dos cuentas de prueba. Las fechas son relativas al dia en que
-- se ejecuta el script por primera vez (HOY = dia de la ejecucion).
-- Para la prueba usar fecha de corte = HOY.
--
-- No inserta nada en tblConciliacion ni en tblDetalleConciliacion:
-- eso lo genera la ventana al pulsar "Corte".
-- ================================================================

START TRANSACTION;

-- ----------------------------------------------------------------
-- 1. Catalogos de apoyo (solo se crean si faltan)
-- ----------------------------------------------------------------
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

SET @BancoPrueba = (SELECT idBanco FROM tblBanco WHERE codigoBanco = 'TSTCONC' LIMIT 1);
SET @MonedaPrueba = (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ' LIMIT 1);
SET @TipoCuentaPrueba = (SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria
                         WHERE codigoTipoCuentaBancaria = 'TSTCONC' LIMIT 1);
SET @TipoIngreso = (SELECT idTipoTransaccion FROM tblTipoTransaccion
                    WHERE codigoTipoTransaccion = 'TSTCONC-ING' LIMIT 1);
SET @TipoEgreso = (SELECT idTipoTransaccion FROM tblTipoTransaccion
                   WHERE codigoTipoTransaccion = 'TSTCONC-EGR' LIMIT 1);

-- ----------------------------------------------------------------
-- 2. Cuentas de prueba. created_at se retrasa 30 dias para que los
--    movimientos de los ultimos dias entren en el saldo del libro.
-- ----------------------------------------------------------------
INSERT INTO tblCuentaBancaria
    (numeroCuentaBancaria, nombreCuentaBancaria, limiteSobregiroCuentaBancaria,
     saldoInicialCuentaBancaria, Fk_Banco_Codigo, Fk_Moneda_Codigo,
     Fk_TipoCuentaBancaria_Codigo, created_at)
SELECT 'TST-CONC-0001', 'Cuenta pruebas conciliacion con diferencias', 0.0000,
       10000.0000, @BancoPrueba, @MonedaPrueba, @TipoCuentaPrueba,
       DATE_SUB(NOW(), INTERVAL 30 DAY)
WHERE NOT EXISTS (SELECT 1 FROM tblCuentaBancaria WHERE numeroCuentaBancaria = 'TST-CONC-0001');

INSERT INTO tblCuentaBancaria
    (numeroCuentaBancaria, nombreCuentaBancaria, limiteSobregiroCuentaBancaria,
     saldoInicialCuentaBancaria, Fk_Banco_Codigo, Fk_Moneda_Codigo,
     Fk_TipoCuentaBancaria_Codigo, created_at)
SELECT 'TST-CONC-0002', 'Cuenta pruebas conciliacion sin diferencias', 0.0000,
       5000.0000, @BancoPrueba, @MonedaPrueba, @TipoCuentaPrueba,
       DATE_SUB(NOW(), INTERVAL 30 DAY)
WHERE NOT EXISTS (SELECT 1 FROM tblCuentaBancaria WHERE numeroCuentaBancaria = 'TST-CONC-0002');

SET @Cuenta1 = (SELECT idCuentaBancaria FROM tblCuentaBancaria
                WHERE numeroCuentaBancaria = 'TST-CONC-0001' LIMIT 1);
SET @Cuenta2 = (SELECT idCuentaBancaria FROM tblCuentaBancaria
                WHERE numeroCuentaBancaria = 'TST-CONC-0002' LIMIT 1);

SET @Hoy = CURRENT_DATE();

-- ================================================================
-- CUENTA 1 (TST-CONC-0001): escenario con partidas de todo tipo
-- ================================================================

-- ----------------------------------------------------------------
-- 3. Movimientos del libro (tblMovimientoBancario)
-- ----------------------------------------------------------------
INSERT INTO tblMovimientoBancario
    (referenciaMovimientoBancario, conceptoMovimientoBancario,
     importeMovimientoBancario, importeGTQMovimientoBancario,
     tipoCambioAplicadoMovimientoBancario, sentidoMovimientoBancario,
     fechaOperacionMovimientoBancario, fechaValorMovimientoBancario,
     origenMovimientoBancario, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
SELECT v.ref, v.concepto, v.importe, v.importe, 1.0000, v.sentido,
       DATE_SUB(@Hoy, INTERVAL v.dias DAY), DATE_SUB(@Hoy, INTERVAL v.dias DAY),
       'MANUAL', @Cuenta1,
       CASE WHEN v.sentido = 1 THEN @TipoIngreso ELSE @TipoEgreso END
FROM (
    SELECT 'TSTC1-L01' AS ref, 'Cobro cliente factura 1001' AS concepto, 5000.0000 AS importe,  1 AS sentido, 8 AS dias
    UNION ALL SELECT 'TSTC1-L02', 'Pago proveedor papeleria',            1250.5000, -1, 7
    UNION ALL SELECT 'TSTC1-L03', 'Pago servicio de energia',             800.0000, -1, 6
    UNION ALL SELECT 'TSTC1-L04', 'Pago proveedor materia prima',        2000.0000, -1, 5
    UNION ALL SELECT 'TSTC1-L05', 'Deposito ventas del dia',             3000.0000,  1, 4
    UNION ALL SELECT 'TSTC1-L06', 'Pago de fletes',                       475.2500, -1, 3
    UNION ALL SELECT 'TSTC1-L07', 'Cobro cliente factura 1002',          1500.0000,  1, 2
    UNION ALL SELECT 'TSTC1-L08', 'Cobro cliente factura 1003',           600.0000,  1, 3
    UNION ALL SELECT 'TSTC1-L09', 'Cobro cliente factura 1004',           700.0000,  1, 9
) v
WHERE @Cuenta1 IS NOT NULL AND @TipoIngreso IS NOT NULL AND @TipoEgreso IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM tblMovimientoBancario m
      WHERE m.referenciaMovimientoBancario = v.ref
  );

-- ----------------------------------------------------------------
-- 4. Estado de cuenta (tblExtractoBancario) y sus lineas
--    Saldo inicial 10,000.00, saldo final 13,876.90
-- ----------------------------------------------------------------
INSERT INTO tblExtractoBancario
    (referenciaExtractoBancario, fechaInicioExtractoBancario, fechaFinExtractoBancario,
     saldoInicialExtractoBancario, saldoFinalExtractoBancario, Fk_CuentaBancaria_Codigo)
SELECT 'TSTC1-EXTRACTO', DATE_SUB(@Hoy, INTERVAL 10 DAY), @Hoy,
       10000.0000, 13876.9000, @Cuenta1
WHERE @Cuenta1 IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM tblExtractoBancario e
      WHERE e.Fk_CuentaBancaria_Codigo = @Cuenta1
        AND e.referenciaExtractoBancario = 'TSTC1-EXTRACTO'
  );

SET @Extracto1 = (SELECT Pk_ExtractoBancario FROM tblExtractoBancario
                  WHERE Fk_CuentaBancaria_Codigo = @Cuenta1
                    AND referenciaExtractoBancario = 'TSTC1-EXTRACTO' LIMIT 1);

INSERT INTO tblDetalleExtracto
    (identificadorDetalleExtracto, referenciaDetalleExtracto, conceptoDetalleExtracto,
     fechaOperacionDetalleExtracto, fechaValorDetalleExtracto,
     importeDetalleExtracto, sentidoDetalleExtracto, Fk_ExtractoBancario_Codigo)
SELECT v.ident, v.ref, v.concepto,
       DATE_SUB(@Hoy, INTERVAL v.dias DAY), DATE_SUB(@Hoy, INTERVAL v.dias DAY),
       v.importe, v.sentido, @Extracto1
FROM (
    SELECT 'TSTC1-B01' AS ident, 'TSTC1-L01' AS ref, 'Deposito cliente' AS concepto, 5000.0000 AS importe,  1 AS sentido, 8 AS dias
    UNION ALL SELECT 'TSTC1-B02', 'TSTC1-L02', 'Debito proveedor',                1250.5000, -1, 7
    UNION ALL SELECT 'TSTC1-B03', 'TSTC1-L03', 'Debito servicio',                  800.0000, -1, 6
    UNION ALL SELECT 'TSTC1-B04', 'TSTC1-L04', 'Debito proveedor',                2150.0000, -1, 5
    UNION ALL SELECT 'TSTC1-B05', NULL,        'Deposito en ventanilla',          3000.0000,  1, 2
    UNION ALL SELECT 'TSTC1-B06', 'TSTC1-COM', 'Comision bancaria',                 35.0000, -1, 3
    UNION ALL SELECT 'TSTC1-B07', 'TSTC1-INT', 'Interes ganado',                    12.4000,  1, 1
    UNION ALL SELECT 'TSTC1-B08', 'TSTC1-L08', 'Cargo por cheque devuelto',        600.0000, -1, 3
    UNION ALL SELECT 'TSTC1-B09', NULL,        'Deposito sin referencia',          700.0000,  1, 3
) v
WHERE @Extracto1 IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM tblDetalleExtracto d
      WHERE d.Fk_ExtractoBancario_Codigo = @Extracto1
        AND d.identificadorDetalleExtracto = v.ident
  );

-- ================================================================
-- CUENTA 2 (TST-CONC-0002): escenario que concilia completo
-- ================================================================
INSERT INTO tblMovimientoBancario
    (referenciaMovimientoBancario, conceptoMovimientoBancario,
     importeMovimientoBancario, importeGTQMovimientoBancario,
     tipoCambioAplicadoMovimientoBancario, sentidoMovimientoBancario,
     fechaOperacionMovimientoBancario, fechaValorMovimientoBancario,
     origenMovimientoBancario, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
SELECT v.ref, v.concepto, v.importe, v.importe, 1.0000, v.sentido,
       DATE_SUB(@Hoy, INTERVAL v.dias DAY), DATE_SUB(@Hoy, INTERVAL v.dias DAY),
       'MANUAL', @Cuenta2,
       CASE WHEN v.sentido = 1 THEN @TipoIngreso ELSE @TipoEgreso END
FROM (
    SELECT 'TSTC2-L01' AS ref, 'Cobro cliente factura 2001' AS concepto, 2000.0000 AS importe,  1 AS sentido, 5 AS dias
    UNION ALL SELECT 'TSTC2-L02', 'Pago proveedor limpieza',             500.0000, -1, 3
) v
WHERE @Cuenta2 IS NOT NULL AND @TipoIngreso IS NOT NULL AND @TipoEgreso IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM tblMovimientoBancario m
      WHERE m.referenciaMovimientoBancario = v.ref
  );

INSERT INTO tblExtractoBancario
    (referenciaExtractoBancario, fechaInicioExtractoBancario, fechaFinExtractoBancario,
     saldoInicialExtractoBancario, saldoFinalExtractoBancario, Fk_CuentaBancaria_Codigo)
SELECT 'TSTC2-EXTRACTO', DATE_SUB(@Hoy, INTERVAL 10 DAY), @Hoy,
       5000.0000, 6500.0000, @Cuenta2
WHERE @Cuenta2 IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM tblExtractoBancario e
      WHERE e.Fk_CuentaBancaria_Codigo = @Cuenta2
        AND e.referenciaExtractoBancario = 'TSTC2-EXTRACTO'
  );

SET @Extracto2 = (SELECT Pk_ExtractoBancario FROM tblExtractoBancario
                  WHERE Fk_CuentaBancaria_Codigo = @Cuenta2
                    AND referenciaExtractoBancario = 'TSTC2-EXTRACTO' LIMIT 1);

INSERT INTO tblDetalleExtracto
    (identificadorDetalleExtracto, referenciaDetalleExtracto, conceptoDetalleExtracto,
     fechaOperacionDetalleExtracto, fechaValorDetalleExtracto,
     importeDetalleExtracto, sentidoDetalleExtracto, Fk_ExtractoBancario_Codigo)
SELECT v.ident, v.ref, v.concepto,
       DATE_SUB(@Hoy, INTERVAL v.dias DAY), DATE_SUB(@Hoy, INTERVAL v.dias DAY),
       v.importe, v.sentido, @Extracto2
FROM (
    SELECT 'TSTC2-B01' AS ident, 'TSTC2-L01' AS ref, 'Deposito cliente' AS concepto, 2000.0000 AS importe,  1 AS sentido, 5 AS dias
    UNION ALL SELECT 'TSTC2-B02', 'TSTC2-L02', 'Debito proveedor',                    500.0000, -1, 3
) v
WHERE @Extracto2 IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM tblDetalleExtracto d
      WHERE d.Fk_ExtractoBancario_Codigo = @Extracto2
        AND d.identificadorDetalleExtracto = v.ident
  );

COMMIT;

-- ----------------------------------------------------------------
-- Verificacion de lo insertado. Debe devolver:
--   cuenta 1: 9 movimientos, 1 extracto, 9 lineas de extracto
--   cuenta 2: 2 movimientos, 1 extracto, 2 lineas de extracto
-- ----------------------------------------------------------------
SELECT c.numeroCuentaBancaria AS cuenta,
       (SELECT COUNT(*) FROM tblMovimientoBancario m
        WHERE m.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria
          AND m.referenciaMovimientoBancario LIKE 'TSTC%') AS movimientos,
       (SELECT COUNT(*) FROM tblExtractoBancario e
        WHERE e.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria
          AND e.referenciaExtractoBancario LIKE 'TSTC%') AS extractos,
       (SELECT COUNT(*) FROM tblDetalleExtracto d
        INNER JOIN tblExtractoBancario e ON e.Pk_ExtractoBancario = d.Fk_ExtractoBancario_Codigo
        WHERE e.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria
          AND d.identificadorDetalleExtracto LIKE 'TSTC%') AS lineasExtracto
FROM tblCuentaBancaria c
WHERE c.numeroCuentaBancaria IN ('TST-CONC-0001', 'TST-CONC-0002')
ORDER BY c.numeroCuentaBancaria;

-- ================================================================
-- RESULTADO ESPERADO en la ventana de Conciliacion Bancaria
-- (cuenta, extracto de prueba, fecha de corte = HOY, pulsar "Corte")
--
-- CUENTA TST-CONC-0001 / extracto TSTC1-EXTRACTO
--   Coinciden por referencia e importe ......... L01, L02, L03
--   Con diferencia (libro 2000.00, banco 2150.00)  L04 / B04, diferencia 150.00
--   Coinciden sin referencia (libro con ref,
--     banco sin ref, 2 dias de distancia) ...... L05 / B05
--   Solo libro (pendientes) .................... L06 (egreso 475.25),
--                                                L07 (ingreso 1500.00)
--   Solo banco (pendientes) .................... B06 (comision 35.00),
--                                                B07 (interes 12.40)
--   Mismo importe y referencia, sentido
--     contrario (no empareja) .................. L08 ingreso 600.00 y
--                                                B08 egreso 600.00, ambos pendientes
--   Fuera de la tolerancia de 3 dias
--     (6 dias de distancia, no empareja) ....... L09 / B09, ambos pendientes
--   Saldo libro:  16274.25
--   Saldo banco:  13876.90
--   Diferencia:   -2397.35 (banco menos libro)
--   Estado:       CON_DIFERENCIAS
--   Para probar el marcado manual: seleccionar L09 y B09 (700.00 en
--   ambos lados) y pulsar "Marcar". Si se selecciona L06 y B06
--   (importes distintos) la partida queda marcada con diferencia.
--
-- CUENTA TST-CONC-0002 / extracto TSTC2-EXTRACTO
--   Coinciden las 2 partidas (L01 / B01 y L02 / B02)
--   Saldo libro:  6500.00
--   Saldo banco:  6500.00
--   Diferencia:   0.00
--   Estado:       CONCILIADA
-- ================================================================
