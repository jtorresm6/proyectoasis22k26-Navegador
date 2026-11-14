-- Ejecutar después de abrir Disponibilidad Diaria y pulsar "Actualizar cálculos".
-- Si se verifica otro día, sustituir CURRENT_DATE() por la fecha de la prueba.

SET @fecha_prueba := CURRENT_DATE();

SELECT
    c.numeroCuentaBancaria AS cuenta,
    d.fechaDisponibilidadDiaria AS fecha,
    d.saldoInicialDisponibilidadDiaria AS saldoInicial,
    d.ingresosDisponibilidadDiaria AS ingresos,
    d.egresosDisponibilidadDiaria AS egresos,
    d.comprometidoDisponibilidadDiaria AS comprometido,
    d.restringidoDisponibilidadDiaria AS restringido,
    d.saldoDisponibleDisponibilidadDiaria AS disponible,
    d.fuenteDisponibilidadDiaria AS fuente,
    CASE
        WHEN d.saldoInicialDisponibilidadDiaria = 15000.0000
         AND d.ingresosDisponibilidadDiaria = 2500.0000
         AND d.egresosDisponibilidadDiaria = 900.0000
         AND d.comprometidoDisponibilidadDiaria = 1200.0000
         AND d.restringidoDisponibilidadDiaria = 0.0000
         AND d.saldoDisponibleDisponibilidadDiaria = 15400.0000
        THEN 'PRUEBA CORRECTA'
        ELSE 'REVISAR RESULTADO'
    END AS validacion
FROM tblDisponibilidadDiaria d
JOIN tblCuentaBancaria c
  ON c.idCuentaBancaria = d.Fk_CuentaBancaria_Codigo
WHERE c.numeroCuentaBancaria = 'PRUEBA-BI-001'
  AND d.fechaDisponibilidadDiaria = @fecha_prueba;
