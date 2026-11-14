-- Limpieza exclusiva del escenario TEST-DISP creado por el script 001.
-- Revisar los SELECT iniciales antes de ejecutar los DELETE.

-- Si se limpia otro día, sustituir CURRENT_DATE() por la fecha de la prueba.
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
    WHERE numeroCuentaBancaria = 'PRUEBA-BI-001'
    LIMIT 1
);

SET @disponibilidad_prueba := (
    SELECT idDisponibilidadDiaria
    FROM tblDisponibilidadDiaria
    WHERE Fk_CuentaBancaria_Codigo = @cuenta_prueba
      AND fechaDisponibilidadDiaria = @fecha_prueba
      AND fuenteDisponibilidadDiaria LIKE 'AUTO;%'
    LIMIT 1
);

-- Vista previa: sólo deben aparecer las filas generadas por esta prueba.
SELECT * FROM tblExtractoBancario
WHERE referenciaExtractoBancario = @ref_extracto;

SELECT * FROM tblMovimientoBancario
WHERE referenciaMovimientoBancario IN (@ref_ingreso, @ref_egreso);

SELECT * FROM tblOrdenPago
WHERE referenciaOrdenPago = @ref_orden;

SELECT * FROM tblOrdenCompraRecibida
WHERE referenciaExternaOrdenCompraRecibida = @ref_orden_compra;

SELECT * FROM tblDisponibilidadDiaria
WHERE idDisponibilidadDiaria = @disponibilidad_prueba;

START TRANSACTION;

DELETE FROM tblDisponibilidadDiariaHistorial
WHERE Fk_DisponibilidadDiaria = @disponibilidad_prueba;

DELETE FROM tblDisponibilidadDiaria
WHERE idDisponibilidadDiaria = @disponibilidad_prueba
  AND fuenteDisponibilidadDiaria LIKE 'AUTO;%';

DELETE FROM tblMovimientoBancario
WHERE referenciaMovimientoBancario IN (@ref_ingreso, @ref_egreso)
  AND origenMovimientoBancario = 'ESTADO_CUENTA';

DELETE FROM tblOrdenPago
WHERE referenciaOrdenPago = @ref_orden
  AND identificadorSolicitanteOrdenPago = 'PRUEBA_DISPONIBILIDAD';

DELETE FROM tblOrdenCompraRecibida
WHERE referenciaExternaOrdenCompraRecibida = @ref_orden_compra
  AND identificadorSolicitanteOrdenCompraRecibida = 'PRUEBA_DISPONIBILIDAD';

DELETE FROM tblExtractoBancario
WHERE referenciaExtractoBancario = @ref_extracto;

COMMIT;

SELECT 'Escenario TEST-DISP eliminado' AS resultado;
