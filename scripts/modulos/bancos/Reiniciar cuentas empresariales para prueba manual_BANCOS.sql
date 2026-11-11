-- ============================================================================
-- REINICIAR CUENTAS EMPRESARIALES PARA UNA PRUEBA MANUAL
--
-- Elimina todas las cuentas de tblCuentaBancaria y solamente los registros
-- que dependen de ellas. Conserva bancos, monedas, tipos de cuenta, conceptos,
-- metodos de pago, tipos de transaccion y cuentas de beneficiarios.
--
-- IMPORTANTE: ejecutar primero un respaldo de las cuentas y sus operaciones.
-- ============================================================================

START TRANSACTION;

-- Dependencias de tercer nivel.
DELETE dc
FROM tblDetalleConciliacion dc
LEFT JOIN tblMovimientoBancario mb
       ON mb.idMovimientoBancario = dc.Fk_MovimientoBancario_Codigo
LEFT JOIN tblDetalleExtracto de
       ON de.idDetalleExtracto = dc.Fk_DetalleExtracto_Codigo
LEFT JOIN tblExtractoBancario eb
       ON eb.Pk_ExtractoBancario = de.Fk_ExtractoBancario_Codigo
WHERE mb.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria)
   OR eb.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE dp
FROM tblDetallePolizaBancaria dp
JOIN tblPolizaBancaria pb
  ON pb.idPolizaBancaria = dp.Fk_PolizaBancaria_Codigo
JOIN tblMovimientoBancario mb
  ON mb.idMovimientoBancario = pb.Fk_MovimientoBancario_Codigo
WHERE mb.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE pc
FROM tblPartidaConsolidadaPolizaBancaria pc
JOIN tblPolizaBancaria pb
  ON pb.idPolizaBancaria = pc.Fk_PolizaBancaria_Codigo
JOIN tblMovimientoBancario mb
  ON mb.idMovimientoBancario = pb.Fk_MovimientoBancario_Codigo
WHERE mb.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

-- Dependencias de movimientos, órdenes, extractos y cortes.
DELETE mt
FROM tblMovimientoBancarioTercero mt
JOIN tblMovimientoBancario mb
  ON mb.idMovimientoBancario = mt.idMovimientoBancario
WHERE mb.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE dvd
FROM tblDepositoVentaDetalle dvd
JOIN tblDepositoVentaEncabezado dve
  ON dve.idDepositoVentaEncabezado = dvd.idDepositoVentaEncabezado
LEFT JOIN tblMovimientoBancario mb
  ON mb.idMovimientoBancario = dve.idMovimientoBancario
WHERE dve.idCuentaBancaria IN (SELECT idCuentaBancaria FROM tblCuentaBancaria)
   OR mb.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE ch
FROM tblCheque ch
LEFT JOIN tblChequera cq
  ON cq.idChequera = ch.Fk_Chequera_Codigo
LEFT JOIN tblOrdenPago op
  ON op.idOrdenPago = ch.Fk_OrdenPago_Codigo
WHERE cq.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria)
   OR op.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE ap
FROM tblAutPagoOrden ap
LEFT JOIN tblAutDisponibilidadFondos af
  ON af.idAutDisponibilidadFondos = ap.Fk_AutDisponibilidadFondos_Codigo
LEFT JOIN tblOrdenPago op
  ON op.idOrdenPago = ap.Fk_OrdenPago_Codigo
WHERE af.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria)
   OR op.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE a
FROM tblAprobacion a
JOIN tblOrdenPago op
  ON op.idOrdenPago = a.Fk_OrdenPago_Codigo
WHERE op.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE dh
FROM tblDisponibilidadDiariaHistorial dh
JOIN tblDisponibilidadDiaria d
  ON d.idDisponibilidadDiaria = dh.Fk_DisponibilidadDiaria
WHERE d.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE de
FROM tblDetalleExtracto de
JOIN tblExtractoBancario eb
  ON eb.Pk_ExtractoBancario = de.Fk_ExtractoBancario_Codigo
WHERE eb.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE pb
FROM tblPolizaBancaria pb
JOIN tblMovimientoBancario mb
  ON mb.idMovimientoBancario = pb.Fk_MovimientoBancario_Codigo
WHERE mb.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE dve
FROM tblDepositoVentaEncabezado dve
LEFT JOIN tblMovimientoBancario mb
  ON mb.idMovimientoBancario = dve.idMovimientoBancario
WHERE dve.idCuentaBancaria IN (SELECT idCuentaBancaria FROM tblCuentaBancaria)
   OR mb.Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

-- Dependencias directas de las cuentas.
DELETE FROM tblMovimientoBancario
WHERE Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE FROM tblOrdenPago
WHERE Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE FROM tblAutDisponibilidadFondos
WHERE Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE FROM tblChequera
WHERE Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE FROM tblConfiguracionPoliza
WHERE Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE FROM tblExtractoBancario
WHERE Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE FROM tblDisponibilidadDiaria
WHERE Fk_CuentaBancaria_Codigo IN (SELECT idCuentaBancaria FROM tblCuentaBancaria);

DELETE FROM tblCuentaBancaria;

COMMIT;

SELECT COUNT(*) AS cuentas_empresariales_restantes
FROM tblCuentaBancaria;
