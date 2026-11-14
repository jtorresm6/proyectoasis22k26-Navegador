-- Escenario controlado para validar Disponibilidad Diaria.
-- No inserta directamente en tblDisponibilidadDiaria: el módulo debe calcularla.
-- Puede ejecutarse más de una vez; las referencias evitan duplicados.

START TRANSACTION;

SET @fecha_prueba := CURRENT_DATE();
SET @marca_prueba := CONCAT('TEST-DISP-', DATE_FORMAT(@fecha_prueba, '%Y%m%d'));
SET @ref_extracto := CONCAT(@marca_prueba, '-EXTRACTO');
SET @ref_ingreso := CONCAT(@marca_prueba, '-INGRESO');
SET @ref_egreso := CONCAT(@marca_prueba, '-EGRESO');
SET @ref_orden_compra := CONCAT(@marca_prueba, '-OC');
SET @ref_orden := CONCAT(@marca_prueba, '-ORDEN');

SET @cuenta_prueba := (
    SELECT idCuentaBancaria
    FROM tblCuentaBancaria
    WHERE numeroCuentaBancaria = 'PRUEBA-BI-001' AND is_active = 1
    LIMIT 1
);

SET @moneda_prueba := (
    SELECT Fk_Moneda_Codigo
    FROM tblCuentaBancaria
    WHERE idCuentaBancaria = @cuenta_prueba
    LIMIT 1
);

SET @tipo_ingreso := (
    SELECT idTipoTransaccion
    FROM tblTipoTransaccion
    WHERE is_active = 1 AND naturalezaTipoTransaccion IN ('INGRESO', 'AMBOS')
    ORDER BY CASE WHEN naturalezaTipoTransaccion = 'INGRESO' THEN 0 ELSE 1 END, idTipoTransaccion
    LIMIT 1
);

SET @tipo_egreso := (
    SELECT idTipoTransaccion
    FROM tblTipoTransaccion
    WHERE is_active = 1 AND naturalezaTipoTransaccion IN ('EGRESO', 'AMBOS')
    ORDER BY CASE WHEN naturalezaTipoTransaccion = 'EGRESO' THEN 0 ELSE 1 END, idTipoTransaccion
    LIMIT 1
);

-- Ancla real simulada: extracto cerrado del día anterior con saldo final Q15,000.
INSERT INTO tblExtractoBancario
    (referenciaExtractoBancario, fechaInicioExtractoBancario, fechaFinExtractoBancario,
     saldoInicialExtractoBancario, saldoFinalExtractoBancario, huellaExtractoBancario,
     Fk_CuentaBancaria_Codigo)
SELECT
    @ref_extracto, DATE_SUB(@fecha_prueba, INTERVAL 1 DAY),
    DATE_SUB(@fecha_prueba, INTERVAL 1 DAY), 15000.0000, 15000.0000,
    SHA2(@ref_extracto, 256), @cuenta_prueba
WHERE @cuenta_prueba IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM tblExtractoBancario
      WHERE referenciaExtractoBancario = @ref_extracto
  );

-- Ingreso del día: Q2,500.
INSERT INTO tblMovimientoBancario
    (referenciaMovimientoBancario, conceptoMovimientoBancario,
     importeMovimientoBancario, importeGTQMovimientoBancario,
     tipoCambioAplicadoMovimientoBancario, sentidoMovimientoBancario,
     fechaOperacionMovimientoBancario, fechaValorMovimientoBancario,
     origenMovimientoBancario, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
SELECT
    @ref_ingreso, 'Cobro acreditado de prueba',
    2500.0000, 2500.0000, 1.0000, 1,
    @fecha_prueba, @fecha_prueba, 'ESTADO_CUENTA',
    @cuenta_prueba, @tipo_ingreso
WHERE @cuenta_prueba IS NOT NULL AND @tipo_ingreso IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM tblMovimientoBancario
      WHERE referenciaMovimientoBancario = @ref_ingreso
  );

-- Egreso realizado del día: Q900.
INSERT INTO tblMovimientoBancario
    (referenciaMovimientoBancario, conceptoMovimientoBancario,
     importeMovimientoBancario, importeGTQMovimientoBancario,
     tipoCambioAplicadoMovimientoBancario, sentidoMovimientoBancario,
     fechaOperacionMovimientoBancario, fechaValorMovimientoBancario,
     origenMovimientoBancario, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
SELECT
    @ref_egreso, 'Pago realizado de prueba',
    900.0000, 900.0000, 1.0000, -1,
    @fecha_prueba, @fecha_prueba, 'ESTADO_CUENTA',
    @cuenta_prueba, @tipo_egreso
WHERE @cuenta_prueba IS NOT NULL AND @tipo_egreso IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM tblMovimientoBancario
      WHERE referenciaMovimientoBancario = @ref_egreso
  );

-- Origen empresarial requerido por la orden de pago.
INSERT INTO tblOrdenCompraRecibida
    (referenciaExternaOrdenCompraRecibida, proveedorOrdenCompraRecibida,
     fechaOrdenCompraRecibida, fechaVencimientoOrdenCompraRecibida,
     importeOrdenCompraRecibida, estadoOrdenCompraRecibida,
     referenciaDocumentoOrdenCompraRecibida, identificadorSolicitanteOrdenCompraRecibida,
     Fk_Moneda_Codigo)
SELECT
    @ref_orden_compra, 'Proveedor de prueba', @fecha_prueba, DATE_ADD(@fecha_prueba, INTERVAL 15 DAY),
    1200.0000, 'APROBADA', @ref_orden_compra, 'PRUEBA_DISPONIBILIDAD', @moneda_prueba
WHERE @moneda_prueba IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM tblOrdenCompraRecibida
      WHERE referenciaExternaOrdenCompraRecibida = @ref_orden_compra
  );

SET @orden_compra_prueba := (
    SELECT Pk_OrdenCompraRecibida
    FROM tblOrdenCompraRecibida
    WHERE referenciaExternaOrdenCompraRecibida = @ref_orden_compra
    LIMIT 1
);

-- Orden autorizada todavía no ejecutada: Q1,200 comprometidos.
INSERT INTO tblOrdenPago
    (referenciaOrdenPago, beneficiarioOrdenPago, importeOrdenPago,
     medioOrdenPago, estadoOrdenPago, fechaProgramadaOrdenPago,
     identificadorSolicitanteOrdenPago, Fk_CuentaBancaria_Codigo, Fk_Moneda_Codigo,
     Fk_OrdenCompraRecibida_Codigo)
SELECT
    @ref_orden, 'Proveedor de prueba', 1200.0000,
    'TRANSFERENCIA', 'AUTORIZADA', @fecha_prueba,
    'PRUEBA_DISPONIBILIDAD', @cuenta_prueba, @moneda_prueba, @orden_compra_prueba
WHERE @cuenta_prueba IS NOT NULL AND @moneda_prueba IS NOT NULL AND @orden_compra_prueba IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM tblOrdenPago
      WHERE referenciaOrdenPago = @ref_orden
  );

COMMIT;

-- Verificación de las fuentes insertadas. Debe devolver 1 extracto,
-- 2 movimientos y 1 orden de pago.
SELECT
    @cuenta_prueba AS cuentaId,
    (SELECT COUNT(*) FROM tblExtractoBancario
     WHERE referenciaExtractoBancario = @ref_extracto) AS extractos,
    (SELECT COUNT(*) FROM tblMovimientoBancario
     WHERE referenciaMovimientoBancario IN (@ref_ingreso, @ref_egreso)) AS movimientos,
    (SELECT COUNT(*) FROM tblOrdenPago
     WHERE referenciaOrdenPago = @ref_orden) AS ordenes;

-- Resultado esperado después de abrir Disponibilidad Diaria y pulsar
-- "Actualizar cálculos":
-- Saldo inicial:  15000.0000
-- Ingresos:         2500.0000
-- Egresos:           900.0000
-- Comprometido:     1200.0000
-- Restringido:         0.0000
-- Disponible:      15400.0000
