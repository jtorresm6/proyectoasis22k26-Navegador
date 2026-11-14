USE dbsistemaembutidos;

START TRANSACTION;

INSERT INTO tblBanco (codigoBanco, nombreBanco)
SELECT 'TSTCIER', 'Banco de pruebas cierre'
WHERE NOT EXISTS (SELECT 1 FROM tblBanco WHERE codigoBanco = 'TSTCIER');

INSERT INTO tblMoneda (idMoneda, codigoMoneda, nombreMoneda, simboloMoneda)
SELECT x.nid, 'GTQ', 'Quetzal', 'Q'
FROM (SELECT IFNULL(MAX(idMoneda), 0) + 1 AS nid FROM tblMoneda) x
WHERE NOT EXISTS (SELECT 1 FROM tblMoneda WHERE codigoMoneda = 'GTQ');

INSERT INTO tblTipoCuentaBancaria (codigoTipoCuentaBancaria, nombreTipoCuentaBancaria)
SELECT 'TSTCIER', 'Cuenta de pruebas cierre'
WHERE NOT EXISTS (SELECT 1 FROM tblTipoCuentaBancaria WHERE codigoTipoCuentaBancaria = 'TSTCIER');

SET @Banco  = (SELECT idBanco FROM tblBanco WHERE codigoBanco = 'TSTCIER' LIMIT 1);
SET @Moneda = (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ' LIMIT 1);
SET @Tipo   = (SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria
               WHERE codigoTipoCuentaBancaria = 'TSTCIER' LIMIT 1);

INSERT INTO tblCuentaBancaria
    (numeroCuentaBancaria, nombreCuentaBancaria, limiteSobregiroCuentaBancaria,
     saldoInicialCuentaBancaria, Fk_Banco_Codigo, Fk_Moneda_Codigo,
     Fk_TipoCuentaBancaria_Codigo)
SELECT 'TST-CIERRE-0002', 'Cuenta de prueba B', 0.0000, 10000.0000, @Banco, @Moneda, @Tipo
WHERE NOT EXISTS (SELECT 1 FROM tblCuentaBancaria
                  WHERE Fk_Banco_Codigo = @Banco AND numeroCuentaBancaria = 'TST-CIERRE-0002');

INSERT INTO tblCuentaBancaria
    (numeroCuentaBancaria, nombreCuentaBancaria, limiteSobregiroCuentaBancaria,
     saldoInicialCuentaBancaria, Fk_Banco_Codigo, Fk_Moneda_Codigo,
     Fk_TipoCuentaBancaria_Codigo)
SELECT 'TST-CIERRE-0003', 'Cuenta de prueba C', 0.0000, 20000.0000, @Banco, @Moneda, @Tipo
WHERE NOT EXISTS (SELECT 1 FROM tblCuentaBancaria
                  WHERE Fk_Banco_Codigo = @Banco AND numeroCuentaBancaria = 'TST-CIERRE-0003');

INSERT INTO tblExtractoBancario
    (referenciaExtractoBancario, fechaInicioExtractoBancario, fechaFinExtractoBancario,
     saldoInicialExtractoBancario, saldoFinalExtractoBancario, Fk_CuentaBancaria_Codigo)
SELECT 'TSTCIER-EXTRACTO', DATE('2026-07-01'), DATE('2026-12-31'),
       cb.saldoInicialCuentaBancaria, cb.saldoInicialCuentaBancaria + 30000.0000,
       cb.idCuentaBancaria
FROM tblCuentaBancaria cb
WHERE cb.numeroCuentaBancaria IN ('TST-CIERRE-0002', 'TST-CIERRE-0003')
  AND NOT EXISTS (SELECT 1 FROM tblExtractoBancario e
                  WHERE e.Fk_CuentaBancaria_Codigo = cb.idCuentaBancaria
                    AND e.referenciaExtractoBancario = 'TSTCIER-EXTRACTO');

INSERT INTO tblConciliacion
    (fechaCorteConciliacion, estadoConciliacion, metodoConciliacion,
     saldoLibroConciliacion, saldoBancoConciliacion, diferenciaConciliacion,
     identificadorResponsableConciliacion, fechaCierreConciliacion,
     Fk_CuentaBancaria_Codigo, Fk_ExtractoBancario_Codigo)
SELECT f.fecha, 'CONCILIADA', 'AUTOMATICA',
       f.saldo + IF(cb.numeroCuentaBancaria = 'TST-CIERRE-0003', 1000.0000, 0.0000),
       f.saldo + IF(cb.numeroCuentaBancaria = 'TST-CIERRE-0003', 1000.0000, 0.0000),
       0, 'PRUEBA CIERRE', NULL,
       cb.idCuentaBancaria,
       (SELECT e.Pk_ExtractoBancario FROM tblExtractoBancario e
         WHERE e.Fk_CuentaBancaria_Codigo = cb.idCuentaBancaria
           AND e.referenciaExtractoBancario = 'TSTCIER-EXTRACTO' LIMIT 1)
FROM tblCuentaBancaria cb
INNER JOIN (
    SELECT DATE('2026-07-31') AS fecha, 25000.0000 AS saldo
    UNION ALL SELECT DATE('2026-08-31'), 27000.0000
    UNION ALL SELECT DATE('2026-09-30'), 29000.0000
    UNION ALL SELECT DATE('2026-10-07'), 31000.0000
    UNION ALL SELECT DATE('2026-10-14'), 31500.0000
    UNION ALL SELECT DATE('2026-10-21'), 32000.0000
    UNION ALL SELECT DATE('2026-10-31'), 32500.0000
) f
WHERE cb.numeroCuentaBancaria IN ('TST-CIERRE-0002', 'TST-CIERRE-0003')
  AND NOT EXISTS (SELECT 1 FROM tblConciliacion x
                  WHERE x.Fk_CuentaBancaria_Codigo = cb.idCuentaBancaria
                    AND x.fechaCorteConciliacion = f.fecha);

SELECT cb.numeroCuentaBancaria, c.fechaCorteConciliacion, c.estadoConciliacion,
       c.saldoLibroConciliacion, c.saldoBancoConciliacion, c.diferenciaConciliacion
FROM tblConciliacion c
INNER JOIN tblCuentaBancaria cb ON cb.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo
WHERE cb.numeroCuentaBancaria IN ('TST-CIERRE-0002', 'TST-CIERRE-0003')
ORDER BY c.fechaCorteConciliacion, cb.numeroCuentaBancaria;

COMMIT;