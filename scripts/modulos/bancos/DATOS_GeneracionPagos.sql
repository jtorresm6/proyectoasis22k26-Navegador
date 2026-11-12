-- ================================================================
-- DATOS DE PRUEBA - 8012 Generacion de Pagos (modulo Bancos)
-- Ejecutar TODO el archivo, de arriba abajo, con el rayito
-- (Ctrl+Shift+Enter). Esta en el orden correcto de dependencias y se
-- puede ejecutar varias veces: solo agrega lo que falta.
--
--  1. Monedas                     8. Tipos de pago
--  2. Tipo de cambio              9. Politicas de aprobacion
--  3. Bancos y tipos de cuenta   10. Usuarios Tesoreros y rol
--  4. Cuentas bancarias          11. Compras por pagar
--  5. Chequeras                  12. Planilla por pagar
--  6. Disponibilidad de hoy      13. COMMIT y verificacion
--  7. Tipos de transaccion y configuracion de poliza
--
-- Las fechas de vencimiento son relativas a HOY, asi el aviso de
-- obligaciones vencidas y por vencer siempre se puede probar.
--
-- Usuarios Tesoreros que crea el paso 10 (clave: Tesoreria2026):
--     aperez, rcastillo y lmonterroso. Solo ellos aparecen en la lista
--     Autorizador y pueden autorizar o rechazar ordenes de pago.
-- ================================================================
USE dbsistemaembutidos;
SET autocommit = 1;

-- ----------------------------------------------------------------
-- 1. Monedas (la tabla puede no tener AUTO_INCREMENT: el id se calcula)
-- ----------------------------------------------------------------
INSERT INTO tblMoneda (idMoneda, codigoMoneda, nombreMoneda, simboloMoneda)
SELECT x.nid, 'GTQ', 'Quetzal', 'Q'
FROM (SELECT IFNULL(MAX(idMoneda), 0) + 1 AS nid FROM tblMoneda) x
WHERE NOT EXISTS (SELECT 1 FROM tblMoneda WHERE codigoMoneda = 'GTQ');

INSERT INTO tblMoneda (idMoneda, codigoMoneda, nombreMoneda, simboloMoneda)
SELECT x.nid, 'USD', 'Dolar estadounidense', 'US$'
FROM (SELECT IFNULL(MAX(idMoneda), 0) + 1 AS nid FROM tblMoneda) x
WHERE NOT EXISTS (SELECT 1 FROM tblMoneda WHERE codigoMoneda = 'USD');

-- ----------------------------------------------------------------
-- 2. Tipo de cambio USD -> GTQ (ayer y hoy)
-- ----------------------------------------------------------------
INSERT INTO tblTipoCambio
    (fechaTipoCambio, valorTipoCambio, fuenteTipoCambio, versionTipoCambio, metodoTipoCambio, Fk_Moneda_Origen_Codigo, Fk_Moneda_Destino_Codigo)
SELECT d.fecha, d.valor, 'Banguat', 1, 'DIRECTA',
       (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD'),
       (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')
FROM (SELECT DATE_SUB(CURDATE(), INTERVAL 1 DAY) AS fecha, 7.7532 AS valor
      UNION ALL SELECT CURDATE(), 7.7550) d
WHERE NOT EXISTS (SELECT 1 FROM tblTipoCambio t
                  WHERE t.fechaTipoCambio = d.fecha AND t.versionTipoCambio = 1
                    AND t.Fk_Moneda_Origen_Codigo = (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD')
                    AND t.Fk_Moneda_Destino_Codigo = (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'));

-- ----------------------------------------------------------------
-- 3. Bancos y tipo de cuenta
-- ----------------------------------------------------------------
INSERT IGNORE INTO tblBanco (codigoBanco, nombreBanco) VALUES
    ('BIND', 'Banco Industrial'),
    ('BRUR', 'Banrural'),
    ('BAC', 'BAC Credomatic');
INSERT IGNORE INTO tblTipoCuentaBancaria (codigoTipoCuentaBancaria, nombreTipoCuentaBancaria) VALUES
    ('MONETARIA', 'Cuenta monetaria');

-- ----------------------------------------------------------------
-- 4. Cuentas bancarias (2 en GTQ y 1 en USD)
-- ----------------------------------------------------------------
INSERT INTO tblCuentaBancaria
    (numeroCuentaBancaria, nombreCuentaBancaria, codigoCuentaContableCuentaBancaria, limiteSobregiroCuentaBancaria,
     Fk_Banco_Codigo, Fk_Moneda_Codigo, Fk_TipoCuentaBancaria_Codigo)
SELECT '000-215487-9', 'Cuenta operativa Industrial GTQ', '1.01.02.001', 50000.0000,
       (SELECT idBanco FROM tblBanco WHERE codigoBanco = 'BIND'),
       (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'),
       (SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria WHERE codigoTipoCuentaBancaria = 'MONETARIA')
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9');

INSERT INTO tblCuentaBancaria
    (numeroCuentaBancaria, nombreCuentaBancaria, codigoCuentaContableCuentaBancaria, limiteSobregiroCuentaBancaria,
     Fk_Banco_Codigo, Fk_Moneda_Codigo, Fk_TipoCuentaBancaria_Codigo)
SELECT '3011-045678-2', 'Cuenta de proveedores Banrural GTQ', '1.01.02.002', 25000.0000,
       (SELECT idBanco FROM tblBanco WHERE codigoBanco = 'BRUR'),
       (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'),
       (SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria WHERE codigoTipoCuentaBancaria = 'MONETARIA')
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2');

INSERT INTO tblCuentaBancaria
    (numeroCuentaBancaria, nombreCuentaBancaria, codigoCuentaContableCuentaBancaria, limiteSobregiroCuentaBancaria,
     Fk_Banco_Codigo, Fk_Moneda_Codigo, Fk_TipoCuentaBancaria_Codigo)
SELECT '90-4421-3', 'Cuenta operativa BAC USD', '1.01.02.003', 0.0000,
       (SELECT idBanco FROM tblBanco WHERE codigoBanco = 'BAC'),
       (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD'),
       (SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria WHERE codigoTipoCuentaBancaria = 'MONETARIA')
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '90-4421-3');

-- ----------------------------------------------------------------
-- 5. Chequeras (para probar el flujo de cheque)
-- ----------------------------------------------------------------
INSERT IGNORE INTO tblChequera
    (serieChequera, numeroInicialChequera, numeroFinalChequera, siguienteNumeroChequera, Fk_CuentaBancaria_Codigo)
SELECT 'BIND-A', 1001, 1100, 1001, idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'
UNION ALL SELECT 'BRUR-A', 5001, 5050, 5001, idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2'
UNION ALL SELECT 'BAC-A', 3001, 3025, 3001, idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '90-4421-3';

-- ----------------------------------------------------------------
-- 6. Disponibilidad diaria de HOY (sin ella la cuenta no aparece en la ventana)
-- ----------------------------------------------------------------
INSERT INTO tblDisponibilidadDiaria
    (fechaDisponibilidadDiaria, saldoInicialDisponibilidadDiaria, ingresosDisponibilidadDiaria, egresosDisponibilidadDiaria,
     comprometidoDisponibilidadDiaria, restringidoDisponibilidadDiaria, saldoDisponibleDisponibilidadDiaria,
     saldoDisponibleGTQDisponibilidadDiaria, tipoCambioAplicadoDisponibilidadDiaria, fuenteDisponibilidadDiaria,
     Fk_CuentaBancaria_Codigo, Fk_TipoCambio_Codigo)
SELECT CURDATE(), 126550.0000, 0, 0, 0, 0, 126550.0000, 126550.0000, 1.0000, 'Cierre bancario diario', c.idCuentaBancaria, NULL
FROM tblCuentaBancaria c
WHERE c.numeroCuentaBancaria = '000-215487-9'
  AND NOT EXISTS (SELECT 1 FROM tblDisponibilidadDiaria d WHERE d.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria AND d.fechaDisponibilidadDiaria = CURDATE());

INSERT INTO tblDisponibilidadDiaria
    (fechaDisponibilidadDiaria, saldoInicialDisponibilidadDiaria, ingresosDisponibilidadDiaria, egresosDisponibilidadDiaria,
     comprometidoDisponibilidadDiaria, restringidoDisponibilidadDiaria, saldoDisponibleDisponibilidadDiaria,
     saldoDisponibleGTQDisponibilidadDiaria, tipoCambioAplicadoDisponibilidadDiaria, fuenteDisponibilidadDiaria,
     Fk_CuentaBancaria_Codigo, Fk_TipoCambio_Codigo)
SELECT CURDATE(), 64379.5000, 0, 0, 0, 0, 64379.5000, 64379.5000, 1.0000, 'Cierre bancario diario', c.idCuentaBancaria, NULL
FROM tblCuentaBancaria c
WHERE c.numeroCuentaBancaria = '3011-045678-2'
  AND NOT EXISTS (SELECT 1 FROM tblDisponibilidadDiaria d WHERE d.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria AND d.fechaDisponibilidadDiaria = CURDATE());

INSERT INTO tblDisponibilidadDiaria
    (fechaDisponibilidadDiaria, saldoInicialDisponibilidadDiaria, ingresosDisponibilidadDiaria, egresosDisponibilidadDiaria,
     comprometidoDisponibilidadDiaria, restringidoDisponibilidadDiaria, saldoDisponibleDisponibilidadDiaria,
     saldoDisponibleGTQDisponibilidadDiaria, tipoCambioAplicadoDisponibilidadDiaria, fuenteDisponibilidadDiaria,
     Fk_CuentaBancaria_Codigo, Fk_TipoCambio_Codigo)
SELECT CURDATE(), 18000.0000, 0, 0, 0, 0, 18000.0000, 139590.0000, 7.7550, 'Cierre bancario diario', c.idCuentaBancaria, (SELECT t.idTipoCambio FROM tblTipoCambio t WHERE t.Fk_Moneda_Origen_Codigo = c.Fk_Moneda_Codigo AND t.fechaTipoCambio <= CURDATE() ORDER BY t.fechaTipoCambio DESC, t.versionTipoCambio DESC LIMIT 1)
FROM tblCuentaBancaria c
WHERE c.numeroCuentaBancaria = '90-4421-3'
  AND NOT EXISTS (SELECT 1 FROM tblDisponibilidadDiaria d WHERE d.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria AND d.fechaDisponibilidadDiaria = CURDATE());

-- ----------------------------------------------------------------
-- 7. Tipos de transaccion y configuracion de poliza de egreso
-- ----------------------------------------------------------------
INSERT IGNORE INTO tblTipoTransaccion (codigoTipoTransaccion, nombreTipoTransaccion, naturalezaTipoTransaccion) VALUES
    ('TRANS-101', 'Pago a proveedores por cheque',          'EGRESO'),
    ('TRANS-102', 'Pago de planilla por cheque',            'EGRESO'),
    ('TRANS-103', 'Deposito de cliente',                    'INGRESO'),
    ('TRANS-104', 'Comision bancaria',                      'EGRESO'),
    ('TRANS-105', 'Transferencia entre cuentas propias',    'AMBOS');

INSERT INTO tblConfiguracionPoliza
    (codigoCuentaContableBancoConfiguracionPoliza, codigoCuentaContableContrapartidaConfiguracionPoliza,
     Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
SELECT c.codigoCuentaContableCuentaBancaria, '2.01.01.001', c.idCuentaBancaria, t.idTipoTransaccion
FROM tblCuentaBancaria c
INNER JOIN tblTipoTransaccion t ON t.codigoTipoTransaccion = 'TRANS-101'
WHERE c.numeroCuentaBancaria IN ('000-215487-9', '3011-045678-2', '90-4421-3')
  AND NOT EXISTS (SELECT 1 FROM tblConfiguracionPoliza x
                  WHERE x.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria AND x.Fk_TipoTransaccion_Codigo = t.idTipoTransaccion);

-- ----------------------------------------------------------------
-- 8. Tipos de pago (Cheque usa Emitir cheque; los demas usan Enviar)
-- ----------------------------------------------------------------
INSERT IGNORE INTO tblMetodoPago (nombreMetodoPago, descripcionMetodoPago, is_active) VALUES
    ('Cheque',                 'Pago con cheque emitido de una chequera de la cuenta', 1),
    ('Transferencia bancaria', 'Transferencia electronica entre cuentas bancarias',    1),
    ('Efectivo',               'Pago en efectivo desde caja',                          1),
    ('Deposito bancario',      'Deposito directo en la cuenta del beneficiario',       1),
    ('Tarjeta corporativa',    'Pago con tarjeta corporativa de la empresa',           1);

-- ----------------------------------------------------------------
-- 9. Politicas de aprobacion de ordenes de pago
-- ----------------------------------------------------------------
INSERT INTO tblPoliticaAprobacion
    (nombrePoliticaAprobacion, tipoOperacionPoliticaAprobacion, montoMinimoPoliticaAprobacion, montoMaximoPoliticaAprobacion,
     cantidadAprobacionesPoliticaAprobacion, fechaInicioPoliticaAprobacion, Fk_Moneda_Codigo)
SELECT P.nombre, 'ORDEN_PAGO', P.minimo, P.maximo, P.cantidad, '2026-01-01', M.idMoneda
FROM (
    SELECT 'Orden de pago en quetzales hasta Q50,000'    AS nombre, 'GTQ' AS moneda,     0.0000 AS minimo,   50000.0000 AS maximo, 1 AS cantidad
    UNION ALL SELECT 'Orden de pago en quetzales mayor a Q50,000',  'GTQ',  50000.0100, 1000000.0000, 2
    UNION ALL SELECT 'Orden de pago en dolares hasta US$6,500',     'USD',      0.0000,    6500.0000, 1
    UNION ALL SELECT 'Orden de pago en dolares mayor a US$6,500',   'USD',   6500.0100,  200000.0000, 2
) P
INNER JOIN tblMoneda M ON M.codigoMoneda = P.moneda
WHERE NOT EXISTS (SELECT 1 FROM tblPoliticaAprobacion X WHERE X.nombrePoliticaAprobacion = P.nombre);

-- ----------------------------------------------------------------
-- 10. Usuarios Tesoreros (empleado, usuario, rol y asignacion)
--     Contrasena de los tres usuarios: Tesoreria2026
-- ----------------------------------------------------------------
INSERT INTO tblempleado (codigoEmpleado, dpiEmpleado, nitEmpleado, nombresEmpleado, apellidosEmpleado, idDepartamento, idJornada, salarioEmpleado, generoEmpleado, fechaNacimientoEmpleado, fechaContratacionEmpleado, telefonoEmpleado, correoEmpleado, is_active)
SELECT e.cod, e.dpi, e.nit, e.nom, e.ape,
       (SELECT MIN(idDepartamento) FROM tblDepartamento),
       (SELECT MIN(idJornada) FROM tblJornada),
       e.salario, e.gen, e.nac, '2025-02-03', e.tel, e.correo, 1
FROM (
    SELECT 'EMP-T01' AS cod, '2456789012301' AS dpi, '4567890-1' AS nit, 'Ana Lucia' AS nom, 'Perez Morales' AS ape, 6500.0000 AS salario, 'F' AS gen, '1990-04-12' AS nac, '55541001' AS tel, 'ana.perez@terminus.com' AS correo
    UNION ALL SELECT 'EMP-T02', '2567890123402', '5678901-2', 'Roberto Carlos', 'Castillo Mendez', 6500.0000, 'M', '1987-08-25', '55541002', 'roberto.castillo@terminus.com'
    UNION ALL SELECT 'EMP-T03', '2678901234503', '6789012-3', 'Luisa Fernanda', 'Monterroso Lima', 6500.0000, 'F', '1993-11-03', '55541003', 'luisa.monterroso@terminus.com'
) e
WHERE NOT EXISTS (SELECT 1 FROM tblempleado x WHERE x.codigoEmpleado = e.cod);

INSERT INTO tblusuario (idEmpleado, nombreUsuario, contrasenaUsuario, ultimoAccesoUsuario, is_active)
SELECT em.idEmpleado, u.usr, '$2b$10$DrmD5ibho0sBGa0wom6UdeC1NrDW4y/YazpPtCw1CUo9JdwaIfnny', NOW(), 1
FROM (
    SELECT 'EMP-T01' AS cod, 'aperez' AS usr
    UNION ALL SELECT 'EMP-T02', 'rcastillo'
    UNION ALL SELECT 'EMP-T03', 'lmonterroso'
) u
INNER JOIN tblempleado em ON em.codigoEmpleado = u.cod
WHERE NOT EXISTS (SELECT 1 FROM tblusuario x WHERE x.nombreUsuario = u.usr);

INSERT INTO tblrol (nombreRol, descripcionRol, is_active)
SELECT 'Tesorero', 'Autoriza y rechaza ordenes de pago en Bancos', 1
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tblrol WHERE nombreRol = 'Tesorero');

INSERT INTO tblusuariorol (idUsuario, idRol, fechaAsignacionUsuarioRol)
SELECT u.idUsuario, r.idRol, CURDATE()
FROM tblusuario u
INNER JOIN tblrol r ON r.nombreRol = 'Tesorero'
WHERE u.nombreUsuario IN ('aperez', 'rcastillo', 'lmonterroso')
  AND NOT EXISTS (SELECT 1 FROM tblusuariorol x WHERE x.idUsuario = u.idUsuario AND x.idRol = r.idRol);

-- ----------------------------------------------------------------
-- 11. Compras aprobadas por pagar (vencida hace 4 dias, vence en 2 y en 17 dias)
-- ----------------------------------------------------------------
INSERT INTO tblOrdenCompraRecibida
    (referenciaExternaOrdenCompraRecibida, proveedorOrdenCompraRecibida, fechaOrdenCompraRecibida, fechaVencimientoOrdenCompraRecibida,
     importeOrdenCompraRecibida, estadoOrdenCompraRecibida, referenciaDocumentoOrdenCompraRecibida, identificadorSolicitanteOrdenCompraRecibida, Fk_Moneda_Codigo)
SELECT o.ref, o.proveedor, DATE_SUB(CURDATE(), INTERVAL 15 DAY), DATE_ADD(CURDATE(), INTERVAL o.dias DAY),
       o.importe, 'APROBADA', CONCAT('compras/oc/', o.ref, '.pdf'), o.solicitante,
       (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = o.moneda)
FROM (
    SELECT 'OC-2026-0301' AS ref, 'Productos Quimicos de Limpieza, S.A.' AS proveedor, -4 AS dias, 15800.0000 AS importe, 'GTQ' AS moneda, 'compras.jlopez' AS solicitante
    UNION ALL SELECT 'OC-2026-0302', 'Servicios de Mantenimiento Industrial, S.A.',  2, 22450.7500, 'GTQ', 'compras.mrivera'
    UNION ALL SELECT 'OC-2026-0303', 'Equipos de Refrigeracion del Norte, S.A.',    17,  2800.0000, 'USD', 'compras.jlopez'
) o
WHERE NOT EXISTS (SELECT 1 FROM tblOrdenCompraRecibida x WHERE x.referenciaExternaOrdenCompraRecibida = o.ref);

-- ----------------------------------------------------------------
-- 12. Planilla recibida con 3 empleados (el vencimiento es el fin del periodo: en 7 dias)
-- ----------------------------------------------------------------
INSERT INTO tblPlanillaRecibida
    (referenciaExternaPlanillaRecibida, inicioPeriodoPlanillaRecibida, finPeriodoPlanillaRecibida, estadoPlanillaRecibida,
     referenciaDocumentoPlanillaRecibida, Fk_Moneda_Codigo)
SELECT 'PL-2026-11-Q1', DATE_SUB(CURDATE(), INTERVAL 8 DAY), DATE_ADD(CURDATE(), INTERVAL 7 DAY), 'RECIBIDA',
       'rrhh/planillas/PL-2026-11-Q1.pdf', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tblPlanillaRecibida WHERE referenciaExternaPlanillaRecibida = 'PL-2026-11-Q1');

INSERT INTO tblDetallePlanillaRecibida
    (referenciaEmpleadoDetallePlanillaRecibida, beneficiarioDetallePlanillaRecibida, importeNetoDetallePlanillaRecibida, Fk_PlanillaRecibida_Codigo)
SELECT e.emp, e.nombre, e.importe, p.Pk_PlanillaRecibida
FROM tblPlanillaRecibida p
INNER JOIN (
    SELECT 'EMP-0301' AS emp, 'Luis Fernando Morales Cano' AS nombre, 4100.0000 AS importe
    UNION ALL SELECT 'EMP-0302', 'Karla Beatriz Ordonez Lima', 5325.5000
    UNION ALL SELECT 'EMP-0303', 'Pedro Antonio Tzoc Garcia',  3890.0000
) e ON 1 = 1
WHERE p.referenciaExternaPlanillaRecibida = 'PL-2026-11-Q1'
  AND NOT EXISTS (SELECT 1 FROM tblDetallePlanillaRecibida d
                  WHERE d.Fk_PlanillaRecibida_Codigo = p.Pk_PlanillaRecibida AND d.referenciaEmpleadoDetallePlanillaRecibida = e.emp);

-- ----------------------------------------------------------------
-- 13. Confirmar los cambios y verificar
-- ----------------------------------------------------------------
COMMIT;

-- Cada fila debe mostrar el minimo esperado; si alguna sale en 0, avisar.
SELECT 'Monedas (GTQ y USD)'                 AS dato, COUNT(*) AS filas, 2 AS minimo FROM tblMoneda WHERE codigoMoneda IN ('GTQ', 'USD')
UNION ALL SELECT 'Cuentas con disponibilidad de hoy',      COUNT(*), 3 FROM tblDisponibilidadDiaria WHERE fechaDisponibilidadDiaria = CURDATE()
UNION ALL SELECT 'Configuraciones de poliza de egreso',    COUNT(*), 3 FROM tblConfiguracionPoliza cp INNER JOIN tblTipoTransaccion t ON t.idTipoTransaccion = cp.Fk_TipoTransaccion_Codigo WHERE t.naturalezaTipoTransaccion = 'EGRESO'
UNION ALL SELECT 'Tipos de pago activos',                  COUNT(*), 5 FROM tblMetodoPago WHERE is_active = 1
UNION ALL SELECT 'Politicas de orden de pago',             COUNT(*), 4 FROM tblPoliticaAprobacion WHERE tipoOperacionPoliticaAprobacion = 'ORDEN_PAGO' AND is_active = 1
UNION ALL SELECT 'Usuarios con rol Tesorero (autorizadores)', COUNT(DISTINCT u.idUsuario), 1 FROM tblusuario u INNER JOIN tblusuariorol ur ON ur.idUsuario = u.idUsuario INNER JOIN tblrol r ON r.idRol = ur.idRol WHERE u.is_active = 1 AND r.is_active = 1 AND r.nombreRol = 'Tesorero'
UNION ALL SELECT 'Compras aprobadas por pagar',            COUNT(*), 3 FROM tblOrdenCompraRecibida WHERE estadoOrdenCompraRecibida = 'APROBADA'
UNION ALL SELECT 'Detalles de planilla recibida',          COUNT(*), 3 FROM tblDetallePlanillaRecibida d INNER JOIN tblPlanillaRecibida p ON p.Pk_PlanillaRecibida = d.Fk_PlanillaRecibida_Codigo WHERE p.estadoPlanillaRecibida = 'RECIBIDA';
