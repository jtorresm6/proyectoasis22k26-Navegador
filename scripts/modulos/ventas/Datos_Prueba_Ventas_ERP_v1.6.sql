-- Datos ficticios para el esquema dbSistemaEmbutidos_Modulos_ERP__v1.6.sql.
-- Ejecutar después de Seguridad y del ERP v1.6, en una base de pruebas.
-- Los cinco clientes de la muestra usan GTQ. Se reutiliza un usuario activo de Seguridad.
-- Se dejan saldos pendientes para probar cobros adicionales.
-- Cinco INSERT explícitos por tabla, agrupados en orden de dependencias.
-- El control de períodos tiene una fila por moneda. Los clientes usan GTQ.
-- Los diarios incluyen las líneas que cuadran cada caso.
USE dbSistemaEmbutidos;
SET NAMES utf8mb4;

DROP PROCEDURE IF EXISTS spCargarPruebasVentasERP16;
DELIMITER $$
CREATE PROCEDURE spCargarPruebasVentasERP16()
carga: BEGIN
    DECLARE moneda, operador, departamento, jornada BIGINT;
    DECLARE banco1, banco2, banco3, banco4, banco5 BIGINT;
    DECLARE tipoCuenta1, tipoCuenta2, tipoCuenta3, tipoCuenta4, tipoCuenta5 BIGINT;
    DECLARE empleado1, empleado2, empleado3, empleado4, empleado5 BIGINT;
    DECLARE vendedor1, vendedor2, vendedor3, vendedor4, vendedor5 BIGINT;
    DECLARE cliente1, cliente2, cliente3, cliente4, cliente5 BIGINT;
    DECLARE tipoCliente1, tipoCliente2, tipoCliente3, tipoCliente4, tipoCliente5 BIGINT;
    DECLARE lista1, lista2, lista3, lista4, lista5 BIGINT;
    DECLARE historial1, historial2, historial3, historial4, historial5 BIGINT;
    DECLARE proveedor1, proveedor2, proveedor3, proveedor4, proveedor5 BIGINT;
    DECLARE marca1, marca2, marca3, marca4, marca5 BIGINT;
    DECLARE categoria1, categoria2, categoria3, categoria4, categoria5 BIGINT;
    DECLARE linea1, linea2, linea3, linea4, linea5 BIGINT;
    DECLARE producto1, producto2, producto3, producto4, producto5 BIGINT;
    DECLARE sucursal1, sucursal2, sucursal3, sucursal4, sucursal5 BIGINT;
    DECLARE clienteFiscal1, clienteFiscal2, clienteFiscal3, clienteFiscal4, clienteFiscal5 BIGINT;
    DECLARE cotizacion1, cotizacion2, cotizacion3, cotizacion4, cotizacion5 BIGINT;
    DECLARE pedido1, pedido2, pedido3, pedido4, pedido5 BIGINT;
    DECLARE factura1, factura2, factura3, factura4, factura5 BIGINT;
    DECLARE facturaDetalle1, facturaDetalle2, facturaDetalle3, facturaDetalle4, facturaDetalle5 BIGINT;
    DECLARE nota1, nota2, nota3, nota4, nota5 BIGINT;
    DECLARE formaPago1, formaPago2, formaPago3, formaPago4, formaPago5 BIGINT;
    DECLARE recibo1, recibo2, recibo3, recibo4, recibo5 BIGINT;
    DECLARE aplicacion1, aplicacion2, aplicacion3, aplicacion4, aplicacion5 BIGINT;
    DECLARE tipoTransaccion1, tipoTransaccion2, tipoTransaccion3, tipoTransaccion4, tipoTransaccion5 BIGINT;
    DECLARE cuentaBanco1, cuentaBanco2, cuentaBanco3, cuentaBanco4, cuentaBanco5 BIGINT;
    DECLARE movimientoBanco1, movimientoBanco2, movimientoBanco3, movimientoBanco4, movimientoBanco5 BIGINT;
    DECLARE deposito1, deposito2, deposito3, deposito4, deposito5 BIGINT;
    DECLARE regla1, regla2, regla3, regla4, regla5 BIGINT;
    DECLARE comision1, comision2, comision3, comision4, comision5 BIGINT;
    DECLARE comisionFactura1, comisionFactura2, comisionFactura3, comisionFactura4, comisionFactura5 BIGINT;
    DECLARE cierre1, cierre2, cierre3, cierre4, cierre5 BIGINT;
    DECLARE traslado1, traslado2, traslado3, traslado4, traslado5 BIGINT;
    DECLARE etiqueta1, etiqueta2, etiqueta3, etiqueta4, etiqueta5 VARCHAR(30);
    DECLARE nombreClienteDemo1, nombreClienteDemo2, nombreClienteDemo3, nombreClienteDemo4, nombreClienteDemo5 VARCHAR(100);
    DECLARE nombreProductoDemo1, nombreProductoDemo2, nombreProductoDemo3, nombreProductoDemo4, nombreProductoDemo5 VARCHAR(100);
    DECLARE codigoForma1, codigoForma2, codigoForma3, codigoForma4, codigoForma5 VARCHAR(30);
    DECLARE uuidFactura1, uuidFactura2, uuidFactura3, uuidFactura4, uuidFactura5 CHAR(36);
    DECLARE finMes1, finMes2, finMes3, finMes4, finMes5 DATE;
    DECLARE inicioMes1, inicioMes2, inicioMes3, inicioMes4, inicioMes5 DATE;
    DECLARE fechaFactura1, fechaFactura2, fechaFactura3, fechaFactura4, fechaFactura5 DATE;
    DECLARE fechaPago1, fechaPago2, fechaPago3, fechaPago4, fechaPago5 DATE;
    DECLARE precio1, precio2, precio3, precio4, precio5 DECIMAL(12,4);
    DECLARE baseFactura1, baseFactura2, baseFactura3, baseFactura4, baseFactura5 DECIMAL(12,4);
    DECLARE iva1, iva2, iva3, iva4, iva5 DECIMAL(12,4);
    DECLARE total1, total2, total3, total4, total5 DECIMAL(12,4);
    DECLARE cobro1, cobro2, cobro3, cobro4, cobro5 DECIMAL(12,4);
    DECLARE ajuste1, ajuste2, ajuste3, ajuste4, ajuste5 DECIMAL(12,4);
    DECLARE saldo1, saldo2, saldo3, saldo4, saldo5 DECIMAL(12,4);
    DECLARE importeDeposito1, importeDeposito2, importeDeposito3, importeDeposito4, importeDeposito5 DECIMAL(12,4);
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;
    IF EXISTS (SELECT 1 FROM tblCliente WHERE codigoCliente = 'V16-DEMO-1') THEN
        COMMIT;
        SELECT 'Los datos de prueba V16-DEMO ya están cargados.' AS resultado;
        LEAVE carga;
    END IF;

    SELECT MIN(idUsuario) INTO operador FROM tblUsuario WHERE is_active = 1;
    SELECT MIN(idDepartamento) INTO departamento FROM tblDepartamento WHERE is_active = 1;
    SELECT MIN(idJornada) INTO jornada FROM tblJornada WHERE is_active = 1;
    IF operador IS NULL OR departamento IS NULL OR jornada IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Se necesita un usuario activo, un departamento y una jornada del ERP.';
    END IF;

    -- tblMoneda
    INSERT INTO tblMoneda (codigoMoneda, nombreMoneda, simboloMoneda, is_active)
    VALUES ('GTQ', 'Quetzal', 'Q', 1)
    ON DUPLICATE KEY UPDATE is_active = 1;

    INSERT INTO tblMoneda (codigoMoneda, nombreMoneda, simboloMoneda, is_active)
    VALUES ('USD', 'Dólar estadounidense', 'US$', 0)
    ON DUPLICATE KEY UPDATE is_active = tblMoneda.is_active;

    INSERT INTO tblMoneda (codigoMoneda, nombreMoneda, simboloMoneda, is_active)
    VALUES ('EUR', 'Euro', '€', 0)
    ON DUPLICATE KEY UPDATE is_active = tblMoneda.is_active;

    INSERT INTO tblMoneda (codigoMoneda, nombreMoneda, simboloMoneda, is_active)
    VALUES ('MXN', 'Peso mexicano', 'MX$', 0)
    ON DUPLICATE KEY UPDATE is_active = tblMoneda.is_active;

    INSERT INTO tblMoneda (codigoMoneda, nombreMoneda, simboloMoneda, is_active)
    VALUES ('CAD', 'Dólar canadiense', 'CA$', 0)
    ON DUPLICATE KEY UPDATE is_active = tblMoneda.is_active;

    SELECT idMoneda INTO moneda FROM tblMoneda WHERE codigoMoneda = 'GTQ';

    -- tblControlPeriodoCxC
    INSERT INTO tblControlPeriodoCxC (idMoneda, fechaUltimoCierreControlPeriodoCxC)
    SELECT idMoneda, NULL FROM tblMoneda WHERE codigoMoneda = 'GTQ'
    ON DUPLICATE KEY UPDATE fechaUltimoCierreControlPeriodoCxC = tblControlPeriodoCxC.fechaUltimoCierreControlPeriodoCxC;

    INSERT INTO tblControlPeriodoCxC (idMoneda, fechaUltimoCierreControlPeriodoCxC)
    SELECT idMoneda, NULL FROM tblMoneda WHERE codigoMoneda = 'USD'
    ON DUPLICATE KEY UPDATE fechaUltimoCierreControlPeriodoCxC = tblControlPeriodoCxC.fechaUltimoCierreControlPeriodoCxC;

    INSERT INTO tblControlPeriodoCxC (idMoneda, fechaUltimoCierreControlPeriodoCxC)
    SELECT idMoneda, NULL FROM tblMoneda WHERE codigoMoneda = 'EUR'
    ON DUPLICATE KEY UPDATE fechaUltimoCierreControlPeriodoCxC = tblControlPeriodoCxC.fechaUltimoCierreControlPeriodoCxC;

    INSERT INTO tblControlPeriodoCxC (idMoneda, fechaUltimoCierreControlPeriodoCxC)
    SELECT idMoneda, NULL FROM tblMoneda WHERE codigoMoneda = 'MXN'
    ON DUPLICATE KEY UPDATE fechaUltimoCierreControlPeriodoCxC = tblControlPeriodoCxC.fechaUltimoCierreControlPeriodoCxC;

    INSERT INTO tblControlPeriodoCxC (idMoneda, fechaUltimoCierreControlPeriodoCxC)
    SELECT idMoneda, NULL FROM tblMoneda WHERE codigoMoneda = 'CAD'
    ON DUPLICATE KEY UPDATE fechaUltimoCierreControlPeriodoCxC = tblControlPeriodoCxC.fechaUltimoCierreControlPeriodoCxC;

    -- tblFormaPagoVenta
    INSERT INTO tblFormaPagoVenta (codigoFormaPagoVenta, nombreFormaPagoVenta, requiereReferenciaFormaPagoVenta)
    VALUES ('EFECTIVO', 'Efectivo', 0)
    ON DUPLICATE KEY UPDATE
        nombreFormaPagoVenta = VALUES(nombreFormaPagoVenta),
        requiereReferenciaFormaPagoVenta = VALUES(requiereReferenciaFormaPagoVenta);

    INSERT INTO tblFormaPagoVenta (codigoFormaPagoVenta, nombreFormaPagoVenta, requiereReferenciaFormaPagoVenta)
    VALUES ('TRANSFERENCIA', 'Transferencia', 1)
    ON DUPLICATE KEY UPDATE
        nombreFormaPagoVenta = VALUES(nombreFormaPagoVenta),
        requiereReferenciaFormaPagoVenta = VALUES(requiereReferenciaFormaPagoVenta);

    INSERT INTO tblFormaPagoVenta (codigoFormaPagoVenta, nombreFormaPagoVenta, requiereReferenciaFormaPagoVenta)
    VALUES ('DEPOSITO', 'Depósito', 1)
    ON DUPLICATE KEY UPDATE
        nombreFormaPagoVenta = VALUES(nombreFormaPagoVenta),
        requiereReferenciaFormaPagoVenta = VALUES(requiereReferenciaFormaPagoVenta);

    INSERT INTO tblFormaPagoVenta (codigoFormaPagoVenta, nombreFormaPagoVenta, requiereReferenciaFormaPagoVenta)
    VALUES ('CHEQUE', 'Cheque', 1)
    ON DUPLICATE KEY UPDATE
        nombreFormaPagoVenta = VALUES(nombreFormaPagoVenta),
        requiereReferenciaFormaPagoVenta = VALUES(requiereReferenciaFormaPagoVenta);

    INSERT INTO tblFormaPagoVenta (codigoFormaPagoVenta, nombreFormaPagoVenta, requiereReferenciaFormaPagoVenta)
    VALUES ('TARJETA', 'Tarjeta', 1)
    ON DUPLICATE KEY UPDATE
        nombreFormaPagoVenta = VALUES(nombreFormaPagoVenta),
        requiereReferenciaFormaPagoVenta = VALUES(requiereReferenciaFormaPagoVenta);

    -- tblCuentaContableCxC
    INSERT INTO tblCuentaContableCxC
        (idMoneda, tipoCuentaContableCxC, cuentaClientesCuentaContableCxC, cuentaContrapartidaCuentaContableCxC)
    VALUES (moneda, 'FACTURA', '1103', '4101')
    ON DUPLICATE KEY UPDATE cuentaClientesCuentaContableCxC = tblCuentaContableCxC.cuentaClientesCuentaContableCxC;

    INSERT INTO tblCuentaContableCxC
        (idMoneda, tipoCuentaContableCxC, cuentaClientesCuentaContableCxC, cuentaContrapartidaCuentaContableCxC)
    VALUES (moneda, 'PAGO', '1103', '1101')
    ON DUPLICATE KEY UPDATE cuentaClientesCuentaContableCxC = tblCuentaContableCxC.cuentaClientesCuentaContableCxC;

    INSERT INTO tblCuentaContableCxC
        (idMoneda, tipoCuentaContableCxC, cuentaClientesCuentaContableCxC, cuentaContrapartidaCuentaContableCxC)
    VALUES (moneda, 'NOTA_CREDITO', '1103', '4102')
    ON DUPLICATE KEY UPDATE cuentaClientesCuentaContableCxC = tblCuentaContableCxC.cuentaClientesCuentaContableCxC;

    INSERT INTO tblCuentaContableCxC
        (idMoneda, tipoCuentaContableCxC, cuentaClientesCuentaContableCxC, cuentaContrapartidaCuentaContableCxC)
    VALUES (moneda, 'NOTA_DEBITO', '1103', '4102')
    ON DUPLICATE KEY UPDATE cuentaClientesCuentaContableCxC = tblCuentaContableCxC.cuentaClientesCuentaContableCxC;

    INSERT INTO tblCuentaContableCxC
        (idMoneda, tipoCuentaContableCxC, cuentaClientesCuentaContableCxC, cuentaContrapartidaCuentaContableCxC)
    VALUES (moneda, 'AJUSTE_POSITIVO', '1103', '5101')
    ON DUPLICATE KEY UPDATE cuentaClientesCuentaContableCxC = tblCuentaContableCxC.cuentaClientesCuentaContableCxC;

    -- tblBanco
    INSERT INTO tblBanco (codigoBanco, nombreBanco) VALUES ('V16-DEMO-1', 'Banco ficticio V16 1');
    SET banco1 = LAST_INSERT_ID();

    INSERT INTO tblBanco (codigoBanco, nombreBanco) VALUES ('V16-DEMO-2', 'Banco ficticio V16 2');
    SET banco2 = LAST_INSERT_ID();

    INSERT INTO tblBanco (codigoBanco, nombreBanco) VALUES ('V16-DEMO-3', 'Banco ficticio V16 3');
    SET banco3 = LAST_INSERT_ID();

    INSERT INTO tblBanco (codigoBanco, nombreBanco) VALUES ('V16-DEMO-4', 'Banco ficticio V16 4');
    SET banco4 = LAST_INSERT_ID();

    INSERT INTO tblBanco (codigoBanco, nombreBanco) VALUES ('V16-DEMO-5', 'Banco ficticio V16 5');
    SET banco5 = LAST_INSERT_ID();

    -- tblTipoCuentaBancaria
    INSERT INTO tblTipoCuentaBancaria (codigoTipoCuentaBancaria, nombreTipoCuentaBancaria)
    VALUES ('V16-DEMO-1', 'Monetaria ficticia V16 1');
    SET tipoCuenta1 = LAST_INSERT_ID();

    INSERT INTO tblTipoCuentaBancaria (codigoTipoCuentaBancaria, nombreTipoCuentaBancaria)
    VALUES ('V16-DEMO-2', 'Monetaria ficticia V16 2');
    SET tipoCuenta2 = LAST_INSERT_ID();

    INSERT INTO tblTipoCuentaBancaria (codigoTipoCuentaBancaria, nombreTipoCuentaBancaria)
    VALUES ('V16-DEMO-3', 'Monetaria ficticia V16 3');
    SET tipoCuenta3 = LAST_INSERT_ID();

    INSERT INTO tblTipoCuentaBancaria (codigoTipoCuentaBancaria, nombreTipoCuentaBancaria)
    VALUES ('V16-DEMO-4', 'Monetaria ficticia V16 4');
    SET tipoCuenta4 = LAST_INSERT_ID();

    INSERT INTO tblTipoCuentaBancaria (codigoTipoCuentaBancaria, nombreTipoCuentaBancaria)
    VALUES ('V16-DEMO-5', 'Monetaria ficticia V16 5');
    SET tipoCuenta5 = LAST_INSERT_ID();

    -- Datos comunes de los cinco casos
    SET etiqueta1 = CONCAT('V16-DEMO-', 1);
    SET nombreClienteDemo1 = ELT(1, 'Tienda Los Pinos (prueba)', 'Abarrotes La Ceiba (prueba)',
        'Mercado El Roble (prueba)', 'Comercial Las Flores (prueba)', 'Despensa El Mirador (prueba)');
    SET nombreProductoDemo1 = ELT(1, 'Jamón cocido 500 g (prueba)', 'Chorizo 400 g (prueba)',
        'Salchicha 500 g (prueba)', 'Longaniza 400 g (prueba)', 'Tocino 250 g (prueba)');
    SET codigoForma1 = ELT(1, 'EFECTIVO', 'TRANSFERENCIA', 'DEPOSITO', 'CHEQUE', 'TARJETA');
    SET uuidFactura1 = UUID();
    SET finMes1 = LAST_DAY(DATE_SUB(CURRENT_DATE, INTERVAL 1 MONTH));
    SET inicioMes1 = DATE_SUB(finMes1, INTERVAL (DAY(finMes1) - 1) DAY);
    SET fechaFactura1 = DATE_ADD(inicioMes1, INTERVAL 5 DAY);
    SET fechaPago1 = DATE_ADD(inicioMes1, INTERVAL 14 DAY);
    SET precio1 = 50 + 10 * 1;
    SET baseFactura1 = 4 * precio1;
    SET iva1 = ROUND(baseFactura1 * 0.12, 4);
    SET total1 = baseFactura1 + iva1;
    SET cobro1 = ROUND(total1 * 0.50, 4);
    SET ajuste1 = 10;
    SET saldo1 = total1 - cobro1 - ajuste1;
    SET importeDeposito1 = ROUND(cobro1 * 0.50, 4);

    SET etiqueta2 = CONCAT('V16-DEMO-', 2);
    SET nombreClienteDemo2 = ELT(2, 'Tienda Los Pinos (prueba)', 'Abarrotes La Ceiba (prueba)',
        'Mercado El Roble (prueba)', 'Comercial Las Flores (prueba)', 'Despensa El Mirador (prueba)');
    SET nombreProductoDemo2 = ELT(2, 'Jamón cocido 500 g (prueba)', 'Chorizo 400 g (prueba)',
        'Salchicha 500 g (prueba)', 'Longaniza 400 g (prueba)', 'Tocino 250 g (prueba)');
    SET codigoForma2 = ELT(2, 'EFECTIVO', 'TRANSFERENCIA', 'DEPOSITO', 'CHEQUE', 'TARJETA');
    SET uuidFactura2 = UUID();
    SET finMes2 = LAST_DAY(DATE_SUB(CURRENT_DATE, INTERVAL 2 MONTH));
    SET inicioMes2 = DATE_SUB(finMes2, INTERVAL (DAY(finMes2) - 1) DAY);
    SET fechaFactura2 = DATE_ADD(inicioMes2, INTERVAL 5 DAY);
    SET fechaPago2 = DATE_ADD(inicioMes2, INTERVAL 14 DAY);
    SET precio2 = 50 + 10 * 2;
    SET baseFactura2 = 4 * precio2;
    SET iva2 = ROUND(baseFactura2 * 0.12, 4);
    SET total2 = baseFactura2 + iva2;
    SET cobro2 = ROUND(total2 * 0.50, 4);
    SET ajuste2 = 10;
    SET saldo2 = total2 - cobro2 - ajuste2;
    SET importeDeposito2 = ROUND(cobro2 * 0.50, 4);

    SET etiqueta3 = CONCAT('V16-DEMO-', 3);
    SET nombreClienteDemo3 = ELT(3, 'Tienda Los Pinos (prueba)', 'Abarrotes La Ceiba (prueba)',
        'Mercado El Roble (prueba)', 'Comercial Las Flores (prueba)', 'Despensa El Mirador (prueba)');
    SET nombreProductoDemo3 = ELT(3, 'Jamón cocido 500 g (prueba)', 'Chorizo 400 g (prueba)',
        'Salchicha 500 g (prueba)', 'Longaniza 400 g (prueba)', 'Tocino 250 g (prueba)');
    SET codigoForma3 = ELT(3, 'EFECTIVO', 'TRANSFERENCIA', 'DEPOSITO', 'CHEQUE', 'TARJETA');
    SET uuidFactura3 = UUID();
    SET finMes3 = LAST_DAY(DATE_SUB(CURRENT_DATE, INTERVAL 3 MONTH));
    SET inicioMes3 = DATE_SUB(finMes3, INTERVAL (DAY(finMes3) - 1) DAY);
    SET fechaFactura3 = DATE_ADD(inicioMes3, INTERVAL 5 DAY);
    SET fechaPago3 = DATE_ADD(inicioMes3, INTERVAL 14 DAY);
    SET precio3 = 50 + 10 * 3;
    SET baseFactura3 = 4 * precio3;
    SET iva3 = ROUND(baseFactura3 * 0.12, 4);
    SET total3 = baseFactura3 + iva3;
    SET cobro3 = ROUND(total3 * 0.50, 4);
    SET ajuste3 = 10;
    SET saldo3 = total3 - cobro3 - ajuste3;
    SET importeDeposito3 = ROUND(cobro3 * 0.50, 4);

    SET etiqueta4 = CONCAT('V16-DEMO-', 4);
    SET nombreClienteDemo4 = ELT(4, 'Tienda Los Pinos (prueba)', 'Abarrotes La Ceiba (prueba)',
        'Mercado El Roble (prueba)', 'Comercial Las Flores (prueba)', 'Despensa El Mirador (prueba)');
    SET nombreProductoDemo4 = ELT(4, 'Jamón cocido 500 g (prueba)', 'Chorizo 400 g (prueba)',
        'Salchicha 500 g (prueba)', 'Longaniza 400 g (prueba)', 'Tocino 250 g (prueba)');
    SET codigoForma4 = ELT(4, 'EFECTIVO', 'TRANSFERENCIA', 'DEPOSITO', 'CHEQUE', 'TARJETA');
    SET uuidFactura4 = UUID();
    SET finMes4 = LAST_DAY(DATE_SUB(CURRENT_DATE, INTERVAL 4 MONTH));
    SET inicioMes4 = DATE_SUB(finMes4, INTERVAL (DAY(finMes4) - 1) DAY);
    SET fechaFactura4 = DATE_ADD(inicioMes4, INTERVAL 5 DAY);
    SET fechaPago4 = DATE_ADD(inicioMes4, INTERVAL 14 DAY);
    SET precio4 = 50 + 10 * 4;
    SET baseFactura4 = 4 * precio4;
    SET iva4 = ROUND(baseFactura4 * 0.12, 4);
    SET total4 = baseFactura4 + iva4;
    SET cobro4 = ROUND(total4 * 0.50, 4);
    SET ajuste4 = 10;
    SET saldo4 = total4 - cobro4 - ajuste4;
    SET importeDeposito4 = ROUND(cobro4 * 0.50, 4);

    SET etiqueta5 = CONCAT('V16-DEMO-', 5);
    SET nombreClienteDemo5 = ELT(5, 'Tienda Los Pinos (prueba)', 'Abarrotes La Ceiba (prueba)',
        'Mercado El Roble (prueba)', 'Comercial Las Flores (prueba)', 'Despensa El Mirador (prueba)');
    SET nombreProductoDemo5 = ELT(5, 'Jamón cocido 500 g (prueba)', 'Chorizo 400 g (prueba)',
        'Salchicha 500 g (prueba)', 'Longaniza 400 g (prueba)', 'Tocino 250 g (prueba)');
    SET codigoForma5 = ELT(5, 'EFECTIVO', 'TRANSFERENCIA', 'DEPOSITO', 'CHEQUE', 'TARJETA');
    SET uuidFactura5 = UUID();
    SET finMes5 = LAST_DAY(DATE_SUB(CURRENT_DATE, INTERVAL 5 MONTH));
    SET inicioMes5 = DATE_SUB(finMes5, INTERVAL (DAY(finMes5) - 1) DAY);
    SET fechaFactura5 = DATE_ADD(inicioMes5, INTERVAL 5 DAY);
    SET fechaPago5 = DATE_ADD(inicioMes5, INTERVAL 14 DAY);
    SET precio5 = 50 + 10 * 5;
    SET baseFactura5 = 4 * precio5;
    SET iva5 = ROUND(baseFactura5 * 0.12, 4);
    SET total5 = baseFactura5 + iva5;
    SET cobro5 = ROUND(total5 * 0.50, 4);
    SET ajuste5 = 10;
    SET saldo5 = total5 - cobro5 - ajuste5;
    SET importeDeposito5 = ROUND(cobro5 * 0.50, 4);

    -- tblEmpleado
    INSERT INTO tblEmpleado
        (codigoEmpleado, dpiEmpleado, nitEmpleado, nombresEmpleado, apellidosEmpleado, generoEmpleado, fechaNacimientoEmpleado, fechaContratacionEmpleado, telefonoEmpleado, correoEmpleado, idDepartamento, idJornada, salarioEmpleado)
    VALUES (etiqueta1, CONCAT('000000000000', 1), CONCAT('DEMOEMP00', 1), CONCAT('Vendedor ', 1), 'Demo ERP V16', 'M', '1990-01-01', inicioMes1, '00000000', CONCAT(etiqueta1, '@example.invalid'), departamento, jornada, 0);

    SET empleado1 = LAST_INSERT_ID();

    INSERT INTO tblEmpleado
        (codigoEmpleado, dpiEmpleado, nitEmpleado, nombresEmpleado, apellidosEmpleado, generoEmpleado, fechaNacimientoEmpleado, fechaContratacionEmpleado, telefonoEmpleado, correoEmpleado, idDepartamento, idJornada, salarioEmpleado)
    VALUES (etiqueta2, CONCAT('000000000000', 2), CONCAT('DEMOEMP00', 2), CONCAT('Vendedor ', 2), 'Demo ERP V16', 'M', '1990-01-01', inicioMes2, '00000000', CONCAT(etiqueta2, '@example.invalid'), departamento, jornada, 0);

    SET empleado2 = LAST_INSERT_ID();

    INSERT INTO tblEmpleado
        (codigoEmpleado, dpiEmpleado, nitEmpleado, nombresEmpleado, apellidosEmpleado, generoEmpleado, fechaNacimientoEmpleado, fechaContratacionEmpleado, telefonoEmpleado, correoEmpleado, idDepartamento, idJornada, salarioEmpleado)
    VALUES (etiqueta3, CONCAT('000000000000', 3), CONCAT('DEMOEMP00', 3), CONCAT('Vendedor ', 3), 'Demo ERP V16', 'M', '1990-01-01', inicioMes3, '00000000', CONCAT(etiqueta3, '@example.invalid'), departamento, jornada, 0);

    SET empleado3 = LAST_INSERT_ID();

    INSERT INTO tblEmpleado
        (codigoEmpleado, dpiEmpleado, nitEmpleado, nombresEmpleado, apellidosEmpleado, generoEmpleado, fechaNacimientoEmpleado, fechaContratacionEmpleado, telefonoEmpleado, correoEmpleado, idDepartamento, idJornada, salarioEmpleado)
    VALUES (etiqueta4, CONCAT('000000000000', 4), CONCAT('DEMOEMP00', 4), CONCAT('Vendedor ', 4), 'Demo ERP V16', 'M', '1990-01-01', inicioMes4, '00000000', CONCAT(etiqueta4, '@example.invalid'), departamento, jornada, 0);

    SET empleado4 = LAST_INSERT_ID();

    INSERT INTO tblEmpleado
        (codigoEmpleado, dpiEmpleado, nitEmpleado, nombresEmpleado, apellidosEmpleado, generoEmpleado, fechaNacimientoEmpleado, fechaContratacionEmpleado, telefonoEmpleado, correoEmpleado, idDepartamento, idJornada, salarioEmpleado)
    VALUES (etiqueta5, CONCAT('000000000000', 5), CONCAT('DEMOEMP00', 5), CONCAT('Vendedor ', 5), 'Demo ERP V16', 'M', '1990-01-01', inicioMes5, '00000000', CONCAT(etiqueta5, '@example.invalid'), departamento, jornada, 0);

    SET empleado5 = LAST_INSERT_ID();

    -- tblVendedor
    INSERT INTO tblVendedor
        (codigoVendedor, idEmpleado)
    VALUES (etiqueta1, empleado1);

    SET vendedor1 = LAST_INSERT_ID();

    INSERT INTO tblVendedor
        (codigoVendedor, idEmpleado)
    VALUES (etiqueta2, empleado2);

    SET vendedor2 = LAST_INSERT_ID();

    INSERT INTO tblVendedor
        (codigoVendedor, idEmpleado)
    VALUES (etiqueta3, empleado3);

    SET vendedor3 = LAST_INSERT_ID();

    INSERT INTO tblVendedor
        (codigoVendedor, idEmpleado)
    VALUES (etiqueta4, empleado4);

    SET vendedor4 = LAST_INSERT_ID();

    INSERT INTO tblVendedor
        (codigoVendedor, idEmpleado)
    VALUES (etiqueta5, empleado5);

    SET vendedor5 = LAST_INSERT_ID();

    -- tblSucursal
    INSERT INTO tblSucursal
        (nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
    VALUES (CONCAT('Sucursal ', etiqueta1), 'Sucursal ficticia', 'Guatemala', 'Guatemala');

    SET sucursal1 = LAST_INSERT_ID();

    INSERT INTO tblSucursal
        (nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
    VALUES (CONCAT('Sucursal ', etiqueta2), 'Sucursal ficticia', 'Guatemala', 'Guatemala');

    SET sucursal2 = LAST_INSERT_ID();

    INSERT INTO tblSucursal
        (nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
    VALUES (CONCAT('Sucursal ', etiqueta3), 'Sucursal ficticia', 'Guatemala', 'Guatemala');

    SET sucursal3 = LAST_INSERT_ID();

    INSERT INTO tblSucursal
        (nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
    VALUES (CONCAT('Sucursal ', etiqueta4), 'Sucursal ficticia', 'Guatemala', 'Guatemala');

    SET sucursal4 = LAST_INSERT_ID();

    INSERT INTO tblSucursal
        (nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
    VALUES (CONCAT('Sucursal ', etiqueta5), 'Sucursal ficticia', 'Guatemala', 'Guatemala');

    SET sucursal5 = LAST_INSERT_ID();

    -- tblProveedor
    INSERT INTO tblProveedor
        (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor)
    VALUES (CONCAT('Proveedor ', etiqueta1), CONCAT('DEMODIST0', 1), CONCAT(etiqueta1, '-p@example.invalid'), '00000000', 'Guatemala');

    SET proveedor1 = LAST_INSERT_ID();

    INSERT INTO tblProveedor
        (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor)
    VALUES (CONCAT('Proveedor ', etiqueta2), CONCAT('DEMODIST0', 2), CONCAT(etiqueta2, '-p@example.invalid'), '00000000', 'Guatemala');

    SET proveedor2 = LAST_INSERT_ID();

    INSERT INTO tblProveedor
        (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor)
    VALUES (CONCAT('Proveedor ', etiqueta3), CONCAT('DEMODIST0', 3), CONCAT(etiqueta3, '-p@example.invalid'), '00000000', 'Guatemala');

    SET proveedor3 = LAST_INSERT_ID();

    INSERT INTO tblProveedor
        (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor)
    VALUES (CONCAT('Proveedor ', etiqueta4), CONCAT('DEMODIST0', 4), CONCAT(etiqueta4, '-p@example.invalid'), '00000000', 'Guatemala');

    SET proveedor4 = LAST_INSERT_ID();

    INSERT INTO tblProveedor
        (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor)
    VALUES (CONCAT('Proveedor ', etiqueta5), CONCAT('DEMODIST0', 5), CONCAT(etiqueta5, '-p@example.invalid'), '00000000', 'Guatemala');

    SET proveedor5 = LAST_INSERT_ID();

    -- tblMarca
    INSERT INTO tblMarca
        (nombreMarca, porcentajeComisionMarca)
    VALUES (CONCAT('Marca ', etiqueta1), 0.05);

    SET marca1 = LAST_INSERT_ID();

    INSERT INTO tblMarca
        (nombreMarca, porcentajeComisionMarca)
    VALUES (CONCAT('Marca ', etiqueta2), 0.05);

    SET marca2 = LAST_INSERT_ID();

    INSERT INTO tblMarca
        (nombreMarca, porcentajeComisionMarca)
    VALUES (CONCAT('Marca ', etiqueta3), 0.05);

    SET marca3 = LAST_INSERT_ID();

    INSERT INTO tblMarca
        (nombreMarca, porcentajeComisionMarca)
    VALUES (CONCAT('Marca ', etiqueta4), 0.05);

    SET marca4 = LAST_INSERT_ID();

    INSERT INTO tblMarca
        (nombreMarca, porcentajeComisionMarca)
    VALUES (CONCAT('Marca ', etiqueta5), 0.05);

    SET marca5 = LAST_INSERT_ID();

    -- tblCategoria
    INSERT INTO tblCategoria
        (nombreCategoria, porcentajeComisionCategoria)
    VALUES (CONCAT('Categoria ', etiqueta1), 0.05);

    SET categoria1 = LAST_INSERT_ID();

    INSERT INTO tblCategoria
        (nombreCategoria, porcentajeComisionCategoria)
    VALUES (CONCAT('Categoria ', etiqueta2), 0.05);

    SET categoria2 = LAST_INSERT_ID();

    INSERT INTO tblCategoria
        (nombreCategoria, porcentajeComisionCategoria)
    VALUES (CONCAT('Categoria ', etiqueta3), 0.05);

    SET categoria3 = LAST_INSERT_ID();

    INSERT INTO tblCategoria
        (nombreCategoria, porcentajeComisionCategoria)
    VALUES (CONCAT('Categoria ', etiqueta4), 0.05);

    SET categoria4 = LAST_INSERT_ID();

    INSERT INTO tblCategoria
        (nombreCategoria, porcentajeComisionCategoria)
    VALUES (CONCAT('Categoria ', etiqueta5), 0.05);

    SET categoria5 = LAST_INSERT_ID();

    -- tblLinea
    INSERT INTO tblLinea
        (nombreLinea, porcentajeComisionLinea)
    VALUES (CONCAT('Linea ', etiqueta1), 0.05);

    SET linea1 = LAST_INSERT_ID();

    INSERT INTO tblLinea
        (nombreLinea, porcentajeComisionLinea)
    VALUES (CONCAT('Linea ', etiqueta2), 0.05);

    SET linea2 = LAST_INSERT_ID();

    INSERT INTO tblLinea
        (nombreLinea, porcentajeComisionLinea)
    VALUES (CONCAT('Linea ', etiqueta3), 0.05);

    SET linea3 = LAST_INSERT_ID();

    INSERT INTO tblLinea
        (nombreLinea, porcentajeComisionLinea)
    VALUES (CONCAT('Linea ', etiqueta4), 0.05);

    SET linea4 = LAST_INSERT_ID();

    INSERT INTO tblLinea
        (nombreLinea, porcentajeComisionLinea)
    VALUES (CONCAT('Linea ', etiqueta5), 0.05);

    SET linea5 = LAST_INSERT_ID();

    -- tblProducto
    INSERT INTO tblProducto
        (skuProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
    VALUES (etiqueta1, nombreProductoDemo1, proveedor1, marca1, categoria1, linea1, 'Producto ficticio para pruebas', FROM_BASE64('iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+aX1cAAAAASUVORK5CYII='), 'Guatemala', ROUND(precio1 * 0.6, 4), precio1, 0.05, 1, 100, 0.50);

    SET producto1 = LAST_INSERT_ID();

    INSERT INTO tblProducto
        (skuProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
    VALUES (etiqueta2, nombreProductoDemo2, proveedor2, marca2, categoria2, linea2, 'Producto ficticio para pruebas', FROM_BASE64('iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+aX1cAAAAASUVORK5CYII='), 'Guatemala', ROUND(precio2 * 0.6, 4), precio2, 0.05, 1, 100, 0.50);

    SET producto2 = LAST_INSERT_ID();

    INSERT INTO tblProducto
        (skuProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
    VALUES (etiqueta3, nombreProductoDemo3, proveedor3, marca3, categoria3, linea3, 'Producto ficticio para pruebas', FROM_BASE64('iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+aX1cAAAAASUVORK5CYII='), 'Guatemala', ROUND(precio3 * 0.6, 4), precio3, 0.05, 1, 100, 0.50);

    SET producto3 = LAST_INSERT_ID();

    INSERT INTO tblProducto
        (skuProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
    VALUES (etiqueta4, nombreProductoDemo4, proveedor4, marca4, categoria4, linea4, 'Producto ficticio para pruebas', FROM_BASE64('iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+aX1cAAAAASUVORK5CYII='), 'Guatemala', ROUND(precio4 * 0.6, 4), precio4, 0.05, 1, 100, 0.50);

    SET producto4 = LAST_INSERT_ID();

    INSERT INTO tblProducto
        (skuProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
    VALUES (etiqueta5, nombreProductoDemo5, proveedor5, marca5, categoria5, linea5, 'Producto ficticio para pruebas', FROM_BASE64('iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+aX1cAAAAASUVORK5CYII='), 'Guatemala', ROUND(precio5 * 0.6, 4), precio5, 0.05, 1, 100, 0.50);

    SET producto5 = LAST_INSERT_ID();

    -- tblProductoVenta
    INSERT INTO tblProductoVenta
        (idProductoVenta, tipoItemProductoVenta, unidadMedidaProductoVenta, tasaImpuestoProductoVenta)
    VALUES (producto1, 'B', 'UNI', 0.12);

    INSERT INTO tblProductoVenta
        (idProductoVenta, tipoItemProductoVenta, unidadMedidaProductoVenta, tasaImpuestoProductoVenta)
    VALUES (producto2, 'B', 'UNI', 0.12);

    INSERT INTO tblProductoVenta
        (idProductoVenta, tipoItemProductoVenta, unidadMedidaProductoVenta, tasaImpuestoProductoVenta)
    VALUES (producto3, 'B', 'UNI', 0.12);

    INSERT INTO tblProductoVenta
        (idProductoVenta, tipoItemProductoVenta, unidadMedidaProductoVenta, tasaImpuestoProductoVenta)
    VALUES (producto4, 'B', 'UNI', 0.12);

    INSERT INTO tblProductoVenta
        (idProductoVenta, tipoItemProductoVenta, unidadMedidaProductoVenta, tasaImpuestoProductoVenta)
    VALUES (producto5, 'B', 'UNI', 0.12);

    -- tblCorrelativoVenta
    INSERT INTO tblCorrelativoVenta
        (prefijoCorrelativoVenta, ultimoNumeroCorrelativoVenta)
    VALUES (etiqueta1, 1);

    INSERT INTO tblCorrelativoVenta
        (prefijoCorrelativoVenta, ultimoNumeroCorrelativoVenta)
    VALUES (etiqueta2, 1);

    INSERT INTO tblCorrelativoVenta
        (prefijoCorrelativoVenta, ultimoNumeroCorrelativoVenta)
    VALUES (etiqueta3, 1);

    INSERT INTO tblCorrelativoVenta
        (prefijoCorrelativoVenta, ultimoNumeroCorrelativoVenta)
    VALUES (etiqueta4, 1);

    INSERT INTO tblCorrelativoVenta
        (prefijoCorrelativoVenta, ultimoNumeroCorrelativoVenta)
    VALUES (etiqueta5, 1);

    -- tblCliente
    INSERT INTO tblCliente
        (codigoCliente, tipoIdentificacionCliente, nombreCliente, direccionCliente, correoCliente, diasCreditoCliente, limiteCreditoCliente, idMonedaCredito)
    VALUES (etiqueta1, 'CF', nombreClienteDemo1, 'Guatemala', CONCAT(etiqueta1, '-c@example.invalid'), 30, 5000, moneda);

    SET cliente1 = LAST_INSERT_ID();

    INSERT INTO tblCliente
        (codigoCliente, tipoIdentificacionCliente, nombreCliente, direccionCliente, correoCliente, diasCreditoCliente, limiteCreditoCliente, idMonedaCredito)
    VALUES (etiqueta2, 'CF', nombreClienteDemo2, 'Guatemala', CONCAT(etiqueta2, '-c@example.invalid'), 30, 5000, moneda);

    SET cliente2 = LAST_INSERT_ID();

    INSERT INTO tblCliente
        (codigoCliente, tipoIdentificacionCliente, nombreCliente, direccionCliente, correoCliente, diasCreditoCliente, limiteCreditoCliente, idMonedaCredito)
    VALUES (etiqueta3, 'CF', nombreClienteDemo3, 'Guatemala', CONCAT(etiqueta3, '-c@example.invalid'), 30, 5000, moneda);

    SET cliente3 = LAST_INSERT_ID();

    INSERT INTO tblCliente
        (codigoCliente, tipoIdentificacionCliente, nombreCliente, direccionCliente, correoCliente, diasCreditoCliente, limiteCreditoCliente, idMonedaCredito)
    VALUES (etiqueta4, 'CF', nombreClienteDemo4, 'Guatemala', CONCAT(etiqueta4, '-c@example.invalid'), 30, 5000, moneda);

    SET cliente4 = LAST_INSERT_ID();

    INSERT INTO tblCliente
        (codigoCliente, tipoIdentificacionCliente, nombreCliente, direccionCliente, correoCliente, diasCreditoCliente, limiteCreditoCliente, idMonedaCredito)
    VALUES (etiqueta5, 'CF', nombreClienteDemo5, 'Guatemala', CONCAT(etiqueta5, '-c@example.invalid'), 30, 5000, moneda);

    SET cliente5 = LAST_INSERT_ID();

    -- tblTipoCliente
    INSERT INTO tblTipoCliente
        (codigoTipoCliente, nombreTipoCliente, porcentajeDescuentoTipoCliente)
    VALUES (etiqueta1, CONCAT('Tipo ', etiqueta1), 0);

    SET tipoCliente1 = LAST_INSERT_ID();

    INSERT INTO tblTipoCliente
        (codigoTipoCliente, nombreTipoCliente, porcentajeDescuentoTipoCliente)
    VALUES (etiqueta2, CONCAT('Tipo ', etiqueta2), 0);

    SET tipoCliente2 = LAST_INSERT_ID();

    INSERT INTO tblTipoCliente
        (codigoTipoCliente, nombreTipoCliente, porcentajeDescuentoTipoCliente)
    VALUES (etiqueta3, CONCAT('Tipo ', etiqueta3), 0);

    SET tipoCliente3 = LAST_INSERT_ID();

    INSERT INTO tblTipoCliente
        (codigoTipoCliente, nombreTipoCliente, porcentajeDescuentoTipoCliente)
    VALUES (etiqueta4, CONCAT('Tipo ', etiqueta4), 0);

    SET tipoCliente4 = LAST_INSERT_ID();

    INSERT INTO tblTipoCliente
        (codigoTipoCliente, nombreTipoCliente, porcentajeDescuentoTipoCliente)
    VALUES (etiqueta5, CONCAT('Tipo ', etiqueta5), 0);

    SET tipoCliente5 = LAST_INSERT_ID();

    -- tblListaPrecioEncabezado
    INSERT INTO tblListaPrecioEncabezado
        (idTipoCliente, codigoListaPrecioEncabezado, nombreListaPrecioEncabezado, idMoneda, vigenteDesdeListaPrecioEncabezado)
    VALUES (tipoCliente1, etiqueta1, CONCAT('Lista GTQ ', etiqueta1), moneda, inicioMes1);

    SET lista1 = LAST_INSERT_ID();

    INSERT INTO tblListaPrecioEncabezado
        (idTipoCliente, codigoListaPrecioEncabezado, nombreListaPrecioEncabezado, idMoneda, vigenteDesdeListaPrecioEncabezado)
    VALUES (tipoCliente2, etiqueta2, CONCAT('Lista GTQ ', etiqueta2), moneda, inicioMes2);

    SET lista2 = LAST_INSERT_ID();

    INSERT INTO tblListaPrecioEncabezado
        (idTipoCliente, codigoListaPrecioEncabezado, nombreListaPrecioEncabezado, idMoneda, vigenteDesdeListaPrecioEncabezado)
    VALUES (tipoCliente3, etiqueta3, CONCAT('Lista GTQ ', etiqueta3), moneda, inicioMes3);

    SET lista3 = LAST_INSERT_ID();

    INSERT INTO tblListaPrecioEncabezado
        (idTipoCliente, codigoListaPrecioEncabezado, nombreListaPrecioEncabezado, idMoneda, vigenteDesdeListaPrecioEncabezado)
    VALUES (tipoCliente4, etiqueta4, CONCAT('Lista GTQ ', etiqueta4), moneda, inicioMes4);

    SET lista4 = LAST_INSERT_ID();

    INSERT INTO tblListaPrecioEncabezado
        (idTipoCliente, codigoListaPrecioEncabezado, nombreListaPrecioEncabezado, idMoneda, vigenteDesdeListaPrecioEncabezado)
    VALUES (tipoCliente5, etiqueta5, CONCAT('Lista GTQ ', etiqueta5), moneda, inicioMes5);

    SET lista5 = LAST_INSERT_ID();

    -- tblPrecioTipoCliente
    INSERT INTO tblPrecioTipoCliente
        (idTipoCliente, idProducto, porcentajeDescuento, precioBase)
    VALUES (tipoCliente1, producto1, 0, precio1);

    INSERT INTO tblPrecioTipoCliente
        (idTipoCliente, idProducto, porcentajeDescuento, precioBase)
    VALUES (tipoCliente2, producto2, 0, precio2);

    INSERT INTO tblPrecioTipoCliente
        (idTipoCliente, idProducto, porcentajeDescuento, precioBase)
    VALUES (tipoCliente3, producto3, 0, precio3);

    INSERT INTO tblPrecioTipoCliente
        (idTipoCliente, idProducto, porcentajeDescuento, precioBase)
    VALUES (tipoCliente4, producto4, 0, precio4);

    INSERT INTO tblPrecioTipoCliente
        (idTipoCliente, idProducto, porcentajeDescuento, precioBase)
    VALUES (tipoCliente5, producto5, 0, precio5);

    -- tblListaPrecioDetalle
    INSERT INTO tblListaPrecioDetalle
        (idListaPrecioEncabezado, idProducto, precioUnitarioListaPrecioDetalle)
    VALUES (lista1, producto1, precio1);

    INSERT INTO tblListaPrecioDetalle
        (idListaPrecioEncabezado, idProducto, precioUnitarioListaPrecioDetalle)
    VALUES (lista2, producto2, precio2);

    INSERT INTO tblListaPrecioDetalle
        (idListaPrecioEncabezado, idProducto, precioUnitarioListaPrecioDetalle)
    VALUES (lista3, producto3, precio3);

    INSERT INTO tblListaPrecioDetalle
        (idListaPrecioEncabezado, idProducto, precioUnitarioListaPrecioDetalle)
    VALUES (lista4, producto4, precio4);

    INSERT INTO tblListaPrecioDetalle
        (idListaPrecioEncabezado, idProducto, precioUnitarioListaPrecioDetalle)
    VALUES (lista5, producto5, precio5);

    -- tblClienteListaPrecio
    INSERT INTO tblClienteListaPrecio
        (idCliente, idListaPrecioEncabezado)
    VALUES (cliente1, lista1);

    INSERT INTO tblClienteListaPrecio
        (idCliente, idListaPrecioEncabezado)
    VALUES (cliente2, lista2);

    INSERT INTO tblClienteListaPrecio
        (idCliente, idListaPrecioEncabezado)
    VALUES (cliente3, lista3);

    INSERT INTO tblClienteListaPrecio
        (idCliente, idListaPrecioEncabezado)
    VALUES (cliente4, lista4);

    INSERT INTO tblClienteListaPrecio
        (idCliente, idListaPrecioEncabezado)
    VALUES (cliente5, lista5);

    -- tblHistorialModificacionPrecio
    INSERT INTO tblHistorialModificacionPrecio
        (idTipoCliente, tipoModificacion, valorModificacion, alcanceModificacion, idMarca, idLinea, idCategoria, idProveedor, idProducto, idUsuarioRegistro)
    VALUES (tipoCliente1, 'AUMENTO_BASE', 5, 'INDIVIDUAL', marca1, linea1, categoria1, proveedor1, producto1, operador);

    SET historial1 = LAST_INSERT_ID();

    INSERT INTO tblHistorialModificacionPrecio
        (idTipoCliente, tipoModificacion, valorModificacion, alcanceModificacion, idMarca, idLinea, idCategoria, idProveedor, idProducto, idUsuarioRegistro)
    VALUES (tipoCliente2, 'AUMENTO_BASE', 5, 'INDIVIDUAL', marca2, linea2, categoria2, proveedor2, producto2, operador);

    SET historial2 = LAST_INSERT_ID();

    INSERT INTO tblHistorialModificacionPrecio
        (idTipoCliente, tipoModificacion, valorModificacion, alcanceModificacion, idMarca, idLinea, idCategoria, idProveedor, idProducto, idUsuarioRegistro)
    VALUES (tipoCliente3, 'AUMENTO_BASE', 5, 'INDIVIDUAL', marca3, linea3, categoria3, proveedor3, producto3, operador);

    SET historial3 = LAST_INSERT_ID();

    INSERT INTO tblHistorialModificacionPrecio
        (idTipoCliente, tipoModificacion, valorModificacion, alcanceModificacion, idMarca, idLinea, idCategoria, idProveedor, idProducto, idUsuarioRegistro)
    VALUES (tipoCliente4, 'AUMENTO_BASE', 5, 'INDIVIDUAL', marca4, linea4, categoria4, proveedor4, producto4, operador);

    SET historial4 = LAST_INSERT_ID();

    INSERT INTO tblHistorialModificacionPrecio
        (idTipoCliente, tipoModificacion, valorModificacion, alcanceModificacion, idMarca, idLinea, idCategoria, idProveedor, idProducto, idUsuarioRegistro)
    VALUES (tipoCliente5, 'AUMENTO_BASE', 5, 'INDIVIDUAL', marca5, linea5, categoria5, proveedor5, producto5, operador);

    SET historial5 = LAST_INSERT_ID();

    -- tblHistorialModificacionPrecioDetalle
    INSERT INTO tblHistorialModificacionPrecioDetalle
        (idHistorialModificacionPrecio, idProducto, porcentajeDescuentoAnterior, precioBaseAnterior, precioFinalAnterior, porcentajeDescuentoNuevo, precioBaseNuevo, precioFinalNuevo)
    VALUES (historial1, producto1, 0, precio1 - 5, precio1 - 5, 0, precio1, precio1);

    INSERT INTO tblHistorialModificacionPrecioDetalle
        (idHistorialModificacionPrecio, idProducto, porcentajeDescuentoAnterior, precioBaseAnterior, precioFinalAnterior, porcentajeDescuentoNuevo, precioBaseNuevo, precioFinalNuevo)
    VALUES (historial2, producto2, 0, precio2 - 5, precio2 - 5, 0, precio2, precio2);

    INSERT INTO tblHistorialModificacionPrecioDetalle
        (idHistorialModificacionPrecio, idProducto, porcentajeDescuentoAnterior, precioBaseAnterior, precioFinalAnterior, porcentajeDescuentoNuevo, precioBaseNuevo, precioFinalNuevo)
    VALUES (historial3, producto3, 0, precio3 - 5, precio3 - 5, 0, precio3, precio3);

    INSERT INTO tblHistorialModificacionPrecioDetalle
        (idHistorialModificacionPrecio, idProducto, porcentajeDescuentoAnterior, precioBaseAnterior, precioFinalAnterior, porcentajeDescuentoNuevo, precioBaseNuevo, precioFinalNuevo)
    VALUES (historial4, producto4, 0, precio4 - 5, precio4 - 5, 0, precio4, precio4);

    INSERT INTO tblHistorialModificacionPrecioDetalle
        (idHistorialModificacionPrecio, idProducto, porcentajeDescuentoAnterior, precioBaseAnterior, precioFinalAnterior, porcentajeDescuentoNuevo, precioBaseNuevo, precioFinalNuevo)
    VALUES (historial5, producto5, 0, precio5 - 5, precio5 - 5, 0, precio5, precio5);

    -- tblClienteFacturaVersion
    INSERT INTO tblClienteFacturaVersion
        (idCliente, numeroVersionClienteFacturaVersion, tipoIdentificacionClienteFacturaVersion, nombreClienteFacturaVersion, direccionClienteFacturaVersion)
    VALUES (cliente1, 1, 'CF', nombreClienteDemo1, 'Guatemala');

    SET clienteFiscal1 = LAST_INSERT_ID();

    INSERT INTO tblClienteFacturaVersion
        (idCliente, numeroVersionClienteFacturaVersion, tipoIdentificacionClienteFacturaVersion, nombreClienteFacturaVersion, direccionClienteFacturaVersion)
    VALUES (cliente2, 1, 'CF', nombreClienteDemo2, 'Guatemala');

    SET clienteFiscal2 = LAST_INSERT_ID();

    INSERT INTO tblClienteFacturaVersion
        (idCliente, numeroVersionClienteFacturaVersion, tipoIdentificacionClienteFacturaVersion, nombreClienteFacturaVersion, direccionClienteFacturaVersion)
    VALUES (cliente3, 1, 'CF', nombreClienteDemo3, 'Guatemala');

    SET clienteFiscal3 = LAST_INSERT_ID();

    INSERT INTO tblClienteFacturaVersion
        (idCliente, numeroVersionClienteFacturaVersion, tipoIdentificacionClienteFacturaVersion, nombreClienteFacturaVersion, direccionClienteFacturaVersion)
    VALUES (cliente4, 1, 'CF', nombreClienteDemo4, 'Guatemala');

    SET clienteFiscal4 = LAST_INSERT_ID();

    INSERT INTO tblClienteFacturaVersion
        (idCliente, numeroVersionClienteFacturaVersion, tipoIdentificacionClienteFacturaVersion, nombreClienteFacturaVersion, direccionClienteFacturaVersion)
    VALUES (cliente5, 1, 'CF', nombreClienteDemo5, 'Guatemala');

    SET clienteFiscal5 = LAST_INSERT_ID();

    -- tblCotizacionEncabezado
    INSERT INTO tblCotizacionEncabezado
        (numeroCotizacionEncabezado, idCliente, idVendedor, idMoneda, fechaEmisionCotizacionEncabezado, vigenteHastaCotizacionEncabezado, estadoCotizacionEncabezado, idUsuarioRegistro, idListaPrecioEncabezado)
    VALUES (etiqueta1, cliente1, vendedor1, moneda, inicioMes1, DATE_ADD(inicioMes1, INTERVAL 30 DAY), 'ACEPTADA', operador, lista1);

    SET cotizacion1 = LAST_INSERT_ID();

    INSERT INTO tblCotizacionEncabezado
        (numeroCotizacionEncabezado, idCliente, idVendedor, idMoneda, fechaEmisionCotizacionEncabezado, vigenteHastaCotizacionEncabezado, estadoCotizacionEncabezado, idUsuarioRegistro, idListaPrecioEncabezado)
    VALUES (etiqueta2, cliente2, vendedor2, moneda, inicioMes2, DATE_ADD(inicioMes2, INTERVAL 30 DAY), 'ACEPTADA', operador, lista2);

    SET cotizacion2 = LAST_INSERT_ID();

    INSERT INTO tblCotizacionEncabezado
        (numeroCotizacionEncabezado, idCliente, idVendedor, idMoneda, fechaEmisionCotizacionEncabezado, vigenteHastaCotizacionEncabezado, estadoCotizacionEncabezado, idUsuarioRegistro, idListaPrecioEncabezado)
    VALUES (etiqueta3, cliente3, vendedor3, moneda, inicioMes3, DATE_ADD(inicioMes3, INTERVAL 30 DAY), 'ACEPTADA', operador, lista3);

    SET cotizacion3 = LAST_INSERT_ID();

    INSERT INTO tblCotizacionEncabezado
        (numeroCotizacionEncabezado, idCliente, idVendedor, idMoneda, fechaEmisionCotizacionEncabezado, vigenteHastaCotizacionEncabezado, estadoCotizacionEncabezado, idUsuarioRegistro, idListaPrecioEncabezado)
    VALUES (etiqueta4, cliente4, vendedor4, moneda, inicioMes4, DATE_ADD(inicioMes4, INTERVAL 30 DAY), 'ACEPTADA', operador, lista4);

    SET cotizacion4 = LAST_INSERT_ID();

    INSERT INTO tblCotizacionEncabezado
        (numeroCotizacionEncabezado, idCliente, idVendedor, idMoneda, fechaEmisionCotizacionEncabezado, vigenteHastaCotizacionEncabezado, estadoCotizacionEncabezado, idUsuarioRegistro, idListaPrecioEncabezado)
    VALUES (etiqueta5, cliente5, vendedor5, moneda, inicioMes5, DATE_ADD(inicioMes5, INTERVAL 30 DAY), 'ACEPTADA', operador, lista5);

    SET cotizacion5 = LAST_INSERT_ID();

    -- tblCotizacionDetalle
    INSERT INTO tblCotizacionDetalle
        (idCotizacionEncabezado, numeroLineaCotizacionDetalle, idProducto, descripcionItemCotizacionDetalle, cantidadCotizacionDetalle, precioUnitarioCotizacionDetalle, tasaImpuestoCotizacionDetalle)
    VALUES (cotizacion1, 1, producto1, nombreProductoDemo1, 4, precio1, 0.12);

    INSERT INTO tblCotizacionDetalle
        (idCotizacionEncabezado, numeroLineaCotizacionDetalle, idProducto, descripcionItemCotizacionDetalle, cantidadCotizacionDetalle, precioUnitarioCotizacionDetalle, tasaImpuestoCotizacionDetalle)
    VALUES (cotizacion2, 1, producto2, nombreProductoDemo2, 4, precio2, 0.12);

    INSERT INTO tblCotizacionDetalle
        (idCotizacionEncabezado, numeroLineaCotizacionDetalle, idProducto, descripcionItemCotizacionDetalle, cantidadCotizacionDetalle, precioUnitarioCotizacionDetalle, tasaImpuestoCotizacionDetalle)
    VALUES (cotizacion3, 1, producto3, nombreProductoDemo3, 4, precio3, 0.12);

    INSERT INTO tblCotizacionDetalle
        (idCotizacionEncabezado, numeroLineaCotizacionDetalle, idProducto, descripcionItemCotizacionDetalle, cantidadCotizacionDetalle, precioUnitarioCotizacionDetalle, tasaImpuestoCotizacionDetalle)
    VALUES (cotizacion4, 1, producto4, nombreProductoDemo4, 4, precio4, 0.12);

    INSERT INTO tblCotizacionDetalle
        (idCotizacionEncabezado, numeroLineaCotizacionDetalle, idProducto, descripcionItemCotizacionDetalle, cantidadCotizacionDetalle, precioUnitarioCotizacionDetalle, tasaImpuestoCotizacionDetalle)
    VALUES (cotizacion5, 1, producto5, nombreProductoDemo5, 4, precio5, 0.12);

    -- tblPedidoEncabezado
    INSERT INTO tblPedidoEncabezado
        (referenciaPedidoEncabezado, idCotizacionEncabezado, idCliente, idVendedor, idMoneda, fechaPedidoEncabezado, fechaEntregaPedidoEncabezado, fechaVencimientoPedidoEncabezado, estadoPedidoEncabezado)
    VALUES (etiqueta1, cotizacion1, cliente1, vendedor1, moneda, DATE_ADD(inicioMes1, INTERVAL 2 DAY), fechaFactura1, finMes1, 'COMPLETADO');

    SET pedido1 = LAST_INSERT_ID();

    INSERT INTO tblPedidoEncabezado
        (referenciaPedidoEncabezado, idCotizacionEncabezado, idCliente, idVendedor, idMoneda, fechaPedidoEncabezado, fechaEntregaPedidoEncabezado, fechaVencimientoPedidoEncabezado, estadoPedidoEncabezado)
    VALUES (etiqueta2, cotizacion2, cliente2, vendedor2, moneda, DATE_ADD(inicioMes2, INTERVAL 2 DAY), fechaFactura2, finMes2, 'COMPLETADO');

    SET pedido2 = LAST_INSERT_ID();

    INSERT INTO tblPedidoEncabezado
        (referenciaPedidoEncabezado, idCotizacionEncabezado, idCliente, idVendedor, idMoneda, fechaPedidoEncabezado, fechaEntregaPedidoEncabezado, fechaVencimientoPedidoEncabezado, estadoPedidoEncabezado)
    VALUES (etiqueta3, cotizacion3, cliente3, vendedor3, moneda, DATE_ADD(inicioMes3, INTERVAL 2 DAY), fechaFactura3, finMes3, 'COMPLETADO');

    SET pedido3 = LAST_INSERT_ID();

    INSERT INTO tblPedidoEncabezado
        (referenciaPedidoEncabezado, idCotizacionEncabezado, idCliente, idVendedor, idMoneda, fechaPedidoEncabezado, fechaEntregaPedidoEncabezado, fechaVencimientoPedidoEncabezado, estadoPedidoEncabezado)
    VALUES (etiqueta4, cotizacion4, cliente4, vendedor4, moneda, DATE_ADD(inicioMes4, INTERVAL 2 DAY), fechaFactura4, finMes4, 'COMPLETADO');

    SET pedido4 = LAST_INSERT_ID();

    INSERT INTO tblPedidoEncabezado
        (referenciaPedidoEncabezado, idCotizacionEncabezado, idCliente, idVendedor, idMoneda, fechaPedidoEncabezado, fechaEntregaPedidoEncabezado, fechaVencimientoPedidoEncabezado, estadoPedidoEncabezado)
    VALUES (etiqueta5, cotizacion5, cliente5, vendedor5, moneda, DATE_ADD(inicioMes5, INTERVAL 2 DAY), fechaFactura5, finMes5, 'COMPLETADO');

    SET pedido5 = LAST_INSERT_ID();

    -- tblPedidoDetalle
    INSERT INTO tblPedidoDetalle
        (idPedidoEncabezado, OrdenPedido, idProducto, cantidadPedidoDetalle, precioUnitarioPedidoDetalle)
    VALUES (pedido1, 1, producto1, 4, precio1);

    INSERT INTO tblPedidoDetalle
        (idPedidoEncabezado, OrdenPedido, idProducto, cantidadPedidoDetalle, precioUnitarioPedidoDetalle)
    VALUES (pedido2, 1, producto2, 4, precio2);

    INSERT INTO tblPedidoDetalle
        (idPedidoEncabezado, OrdenPedido, idProducto, cantidadPedidoDetalle, precioUnitarioPedidoDetalle)
    VALUES (pedido3, 1, producto3, 4, precio3);

    INSERT INTO tblPedidoDetalle
        (idPedidoEncabezado, OrdenPedido, idProducto, cantidadPedidoDetalle, precioUnitarioPedidoDetalle)
    VALUES (pedido4, 1, producto4, 4, precio4);

    INSERT INTO tblPedidoDetalle
        (idPedidoEncabezado, OrdenPedido, idProducto, cantidadPedidoDetalle, precioUnitarioPedidoDetalle)
    VALUES (pedido5, 1, producto5, 4, precio5);

    -- tblFacturaEncabezado
    INSERT INTO tblFacturaEncabezado
        (uuidDteFacturaEncabezado, serieFacturaEncabezado, numeroFacturaEncabezado, idSucursal, idClienteFacturaVersion, idVendedor, idMoneda, idCotizacionEncabezado, idPedidoEncabezado, fechaEmisionFacturaEncabezado, fechaVencimientoFacturaEncabezado, condicionPagoFacturaEncabezado, estadoFacturaEncabezado, subtotalFacturaEncabezado, impuestoIvaFacturaEncabezado, totalFacturaEncabezado, observacionesFacturaEncabezado, idUsuarioRegistro)
    VALUES (uuidFactura1, 'V16-DEMO', etiqueta1, sucursal1, clienteFiscal1, vendedor1, moneda, cotizacion1, pedido1, fechaFactura1, DATE_ADD(fechaFactura1, INTERVAL 30 DAY), 'CREDITO', 'EMITIDA', baseFactura1, iva1, total1, 'Factura ficticia sin certificación', operador);

    SET factura1 = LAST_INSERT_ID();

    INSERT INTO tblFacturaEncabezado
        (uuidDteFacturaEncabezado, serieFacturaEncabezado, numeroFacturaEncabezado, idSucursal, idClienteFacturaVersion, idVendedor, idMoneda, idCotizacionEncabezado, idPedidoEncabezado, fechaEmisionFacturaEncabezado, fechaVencimientoFacturaEncabezado, condicionPagoFacturaEncabezado, estadoFacturaEncabezado, subtotalFacturaEncabezado, impuestoIvaFacturaEncabezado, totalFacturaEncabezado, observacionesFacturaEncabezado, idUsuarioRegistro)
    VALUES (uuidFactura2, 'V16-DEMO', etiqueta2, sucursal2, clienteFiscal2, vendedor2, moneda, cotizacion2, pedido2, fechaFactura2, DATE_ADD(fechaFactura2, INTERVAL 30 DAY), 'CREDITO', 'EMITIDA', baseFactura2, iva2, total2, 'Factura ficticia sin certificación', operador);

    SET factura2 = LAST_INSERT_ID();

    INSERT INTO tblFacturaEncabezado
        (uuidDteFacturaEncabezado, serieFacturaEncabezado, numeroFacturaEncabezado, idSucursal, idClienteFacturaVersion, idVendedor, idMoneda, idCotizacionEncabezado, idPedidoEncabezado, fechaEmisionFacturaEncabezado, fechaVencimientoFacturaEncabezado, condicionPagoFacturaEncabezado, estadoFacturaEncabezado, subtotalFacturaEncabezado, impuestoIvaFacturaEncabezado, totalFacturaEncabezado, observacionesFacturaEncabezado, idUsuarioRegistro)
    VALUES (uuidFactura3, 'V16-DEMO', etiqueta3, sucursal3, clienteFiscal3, vendedor3, moneda, cotizacion3, pedido3, fechaFactura3, DATE_ADD(fechaFactura3, INTERVAL 30 DAY), 'CREDITO', 'EMITIDA', baseFactura3, iva3, total3, 'Factura ficticia sin certificación', operador);

    SET factura3 = LAST_INSERT_ID();

    INSERT INTO tblFacturaEncabezado
        (uuidDteFacturaEncabezado, serieFacturaEncabezado, numeroFacturaEncabezado, idSucursal, idClienteFacturaVersion, idVendedor, idMoneda, idCotizacionEncabezado, idPedidoEncabezado, fechaEmisionFacturaEncabezado, fechaVencimientoFacturaEncabezado, condicionPagoFacturaEncabezado, estadoFacturaEncabezado, subtotalFacturaEncabezado, impuestoIvaFacturaEncabezado, totalFacturaEncabezado, observacionesFacturaEncabezado, idUsuarioRegistro)
    VALUES (uuidFactura4, 'V16-DEMO', etiqueta4, sucursal4, clienteFiscal4, vendedor4, moneda, cotizacion4, pedido4, fechaFactura4, DATE_ADD(fechaFactura4, INTERVAL 30 DAY), 'CREDITO', 'EMITIDA', baseFactura4, iva4, total4, 'Factura ficticia sin certificación', operador);

    SET factura4 = LAST_INSERT_ID();

    INSERT INTO tblFacturaEncabezado
        (uuidDteFacturaEncabezado, serieFacturaEncabezado, numeroFacturaEncabezado, idSucursal, idClienteFacturaVersion, idVendedor, idMoneda, idCotizacionEncabezado, idPedidoEncabezado, fechaEmisionFacturaEncabezado, fechaVencimientoFacturaEncabezado, condicionPagoFacturaEncabezado, estadoFacturaEncabezado, subtotalFacturaEncabezado, impuestoIvaFacturaEncabezado, totalFacturaEncabezado, observacionesFacturaEncabezado, idUsuarioRegistro)
    VALUES (uuidFactura5, 'V16-DEMO', etiqueta5, sucursal5, clienteFiscal5, vendedor5, moneda, cotizacion5, pedido5, fechaFactura5, DATE_ADD(fechaFactura5, INTERVAL 30 DAY), 'CREDITO', 'EMITIDA', baseFactura5, iva5, total5, 'Factura ficticia sin certificación', operador);

    SET factura5 = LAST_INSERT_ID();

    -- tblFacturaDetalle
    INSERT INTO tblFacturaDetalle
        (idFacturaEncabezadoUuidDte, numeroLineaFacturaDetalle, idProducto, cantidadFacturaDetalle, precioUnitarioFacturaDetalle, tasaImpuestoFacturaDetalle)
    VALUES (uuidFactura1, 1, producto1, 4, precio1, 0.12);

    SET facturaDetalle1 = LAST_INSERT_ID();

    INSERT INTO tblFacturaDetalle
        (idFacturaEncabezadoUuidDte, numeroLineaFacturaDetalle, idProducto, cantidadFacturaDetalle, precioUnitarioFacturaDetalle, tasaImpuestoFacturaDetalle)
    VALUES (uuidFactura2, 1, producto2, 4, precio2, 0.12);

    SET facturaDetalle2 = LAST_INSERT_ID();

    INSERT INTO tblFacturaDetalle
        (idFacturaEncabezadoUuidDte, numeroLineaFacturaDetalle, idProducto, cantidadFacturaDetalle, precioUnitarioFacturaDetalle, tasaImpuestoFacturaDetalle)
    VALUES (uuidFactura3, 1, producto3, 4, precio3, 0.12);

    SET facturaDetalle3 = LAST_INSERT_ID();

    INSERT INTO tblFacturaDetalle
        (idFacturaEncabezadoUuidDte, numeroLineaFacturaDetalle, idProducto, cantidadFacturaDetalle, precioUnitarioFacturaDetalle, tasaImpuestoFacturaDetalle)
    VALUES (uuidFactura4, 1, producto4, 4, precio4, 0.12);

    SET facturaDetalle4 = LAST_INSERT_ID();

    INSERT INTO tblFacturaDetalle
        (idFacturaEncabezadoUuidDte, numeroLineaFacturaDetalle, idProducto, cantidadFacturaDetalle, precioUnitarioFacturaDetalle, tasaImpuestoFacturaDetalle)
    VALUES (uuidFactura5, 1, producto5, 4, precio5, 0.12);

    SET facturaDetalle5 = LAST_INSERT_ID();

    -- tblNotaCxC
    INSERT INTO tblNotaCxC
        (numeroNotaCxC, idFacturaEncabezado, tipoNotaCxC, fechaNotaCxC, montoNotaCxC, motivoNotaCxC, estadoNotaCxC, idUsuarioRegistro)
    VALUES (etiqueta1, factura1, 'CREDITO', fechaPago1, ajuste1, 'Ajuste de prueba', 'APLICADA', operador);

    SET nota1 = LAST_INSERT_ID();

    INSERT INTO tblNotaCxC
        (numeroNotaCxC, idFacturaEncabezado, tipoNotaCxC, fechaNotaCxC, montoNotaCxC, motivoNotaCxC, estadoNotaCxC, idUsuarioRegistro)
    VALUES (etiqueta2, factura2, 'CREDITO', fechaPago2, ajuste2, 'Ajuste de prueba', 'APLICADA', operador);

    SET nota2 = LAST_INSERT_ID();

    INSERT INTO tblNotaCxC
        (numeroNotaCxC, idFacturaEncabezado, tipoNotaCxC, fechaNotaCxC, montoNotaCxC, motivoNotaCxC, estadoNotaCxC, idUsuarioRegistro)
    VALUES (etiqueta3, factura3, 'CREDITO', fechaPago3, ajuste3, 'Ajuste de prueba', 'APLICADA', operador);

    SET nota3 = LAST_INSERT_ID();

    INSERT INTO tblNotaCxC
        (numeroNotaCxC, idFacturaEncabezado, tipoNotaCxC, fechaNotaCxC, montoNotaCxC, motivoNotaCxC, estadoNotaCxC, idUsuarioRegistro)
    VALUES (etiqueta4, factura4, 'CREDITO', fechaPago4, ajuste4, 'Ajuste de prueba', 'APLICADA', operador);

    SET nota4 = LAST_INSERT_ID();

    INSERT INTO tblNotaCxC
        (numeroNotaCxC, idFacturaEncabezado, tipoNotaCxC, fechaNotaCxC, montoNotaCxC, motivoNotaCxC, estadoNotaCxC, idUsuarioRegistro)
    VALUES (etiqueta5, factura5, 'CREDITO', fechaPago5, ajuste5, 'Ajuste de prueba', 'APLICADA', operador);

    SET nota5 = LAST_INSERT_ID();

    SELECT idFormaPagoVenta INTO formaPago1 FROM tblFormaPagoVenta WHERE codigoFormaPagoVenta = codigoForma1;
    SELECT idFormaPagoVenta INTO formaPago2 FROM tblFormaPagoVenta WHERE codigoFormaPagoVenta = codigoForma2;
    SELECT idFormaPagoVenta INTO formaPago3 FROM tblFormaPagoVenta WHERE codigoFormaPagoVenta = codigoForma3;
    SELECT idFormaPagoVenta INTO formaPago4 FROM tblFormaPagoVenta WHERE codigoFormaPagoVenta = codigoForma4;
    SELECT idFormaPagoVenta INTO formaPago5 FROM tblFormaPagoVenta WHERE codigoFormaPagoVenta = codigoForma5;

    -- tblReciboPago
    INSERT INTO tblReciboPago
        (numeroReciboPago, idCliente, idMoneda, fechaReciboPago, montoReciboPago, referenciaReciboPago, estadoReciboPago, observacionesReciboPago, idUsuarioRegistro, idFormaPagoVenta)
    VALUES (etiqueta1, cliente1, moneda, fechaPago1, cobro1, IF(codigoForma1 = 'EFECTIVO', NULL, CONCAT('REF-', etiqueta1)), 'CONFIRMADO', 'Cobro parcial de prueba', operador, formaPago1);

    SET recibo1 = LAST_INSERT_ID();

    INSERT INTO tblReciboPago
        (numeroReciboPago, idCliente, idMoneda, fechaReciboPago, montoReciboPago, referenciaReciboPago, estadoReciboPago, observacionesReciboPago, idUsuarioRegistro, idFormaPagoVenta)
    VALUES (etiqueta2, cliente2, moneda, fechaPago2, cobro2, IF(codigoForma2 = 'EFECTIVO', NULL, CONCAT('REF-', etiqueta2)), 'CONFIRMADO', 'Cobro parcial de prueba', operador, formaPago2);

    SET recibo2 = LAST_INSERT_ID();

    INSERT INTO tblReciboPago
        (numeroReciboPago, idCliente, idMoneda, fechaReciboPago, montoReciboPago, referenciaReciboPago, estadoReciboPago, observacionesReciboPago, idUsuarioRegistro, idFormaPagoVenta)
    VALUES (etiqueta3, cliente3, moneda, fechaPago3, cobro3, IF(codigoForma3 = 'EFECTIVO', NULL, CONCAT('REF-', etiqueta3)), 'CONFIRMADO', 'Cobro parcial de prueba', operador, formaPago3);

    SET recibo3 = LAST_INSERT_ID();

    INSERT INTO tblReciboPago
        (numeroReciboPago, idCliente, idMoneda, fechaReciboPago, montoReciboPago, referenciaReciboPago, estadoReciboPago, observacionesReciboPago, idUsuarioRegistro, idFormaPagoVenta)
    VALUES (etiqueta4, cliente4, moneda, fechaPago4, cobro4, IF(codigoForma4 = 'EFECTIVO', NULL, CONCAT('REF-', etiqueta4)), 'CONFIRMADO', 'Cobro parcial de prueba', operador, formaPago4);

    SET recibo4 = LAST_INSERT_ID();

    INSERT INTO tblReciboPago
        (numeroReciboPago, idCliente, idMoneda, fechaReciboPago, montoReciboPago, referenciaReciboPago, estadoReciboPago, observacionesReciboPago, idUsuarioRegistro, idFormaPagoVenta)
    VALUES (etiqueta5, cliente5, moneda, fechaPago5, cobro5, IF(codigoForma5 = 'EFECTIVO', NULL, CONCAT('REF-', etiqueta5)), 'CONFIRMADO', 'Cobro parcial de prueba', operador, formaPago5);

    SET recibo5 = LAST_INSERT_ID();

    -- tblAplicacionPago
    INSERT INTO tblAplicacionPago
        (idReciboPago, idFacturaEncabezado, fechaAplicacionPago, montoAplicadoAplicacionPago, estadoAplicacionPago, idUsuarioRegistro)
    VALUES (recibo1, factura1, fechaPago1, cobro1, 'APLICADA', operador);

    SET aplicacion1 = LAST_INSERT_ID();

    INSERT INTO tblAplicacionPago
        (idReciboPago, idFacturaEncabezado, fechaAplicacionPago, montoAplicadoAplicacionPago, estadoAplicacionPago, idUsuarioRegistro)
    VALUES (recibo2, factura2, fechaPago2, cobro2, 'APLICADA', operador);

    SET aplicacion2 = LAST_INSERT_ID();

    INSERT INTO tblAplicacionPago
        (idReciboPago, idFacturaEncabezado, fechaAplicacionPago, montoAplicadoAplicacionPago, estadoAplicacionPago, idUsuarioRegistro)
    VALUES (recibo3, factura3, fechaPago3, cobro3, 'APLICADA', operador);

    SET aplicacion3 = LAST_INSERT_ID();

    INSERT INTO tblAplicacionPago
        (idReciboPago, idFacturaEncabezado, fechaAplicacionPago, montoAplicadoAplicacionPago, estadoAplicacionPago, idUsuarioRegistro)
    VALUES (recibo4, factura4, fechaPago4, cobro4, 'APLICADA', operador);

    SET aplicacion4 = LAST_INSERT_ID();

    INSERT INTO tblAplicacionPago
        (idReciboPago, idFacturaEncabezado, fechaAplicacionPago, montoAplicadoAplicacionPago, estadoAplicacionPago, idUsuarioRegistro)
    VALUES (recibo5, factura5, fechaPago5, cobro5, 'APLICADA', operador);

    SET aplicacion5 = LAST_INSERT_ID();

    -- tblMovimientoCxC
    INSERT INTO tblMovimientoCxC
        (idFacturaEncabezado, idAplicacionPago, idNotaCxC, fechaMovimientoCxC,
         tipoMovimientoCxC, descripcionMovimientoCxC, cargoMovimientoCxC, abonoMovimientoCxC, idUsuarioRegistro)
    VALUES (factura1, NULL, NULL, fechaFactura1, 'FACTURA', 'Factura de prueba', total1, 0, operador),
           (factura1, aplicacion1, NULL, fechaPago1, 'PAGO', 'Cobro de prueba', 0, cobro1, operador),
           (factura1, NULL, nota1, fechaPago1, 'NOTA_CREDITO', 'Crédito de prueba', 0, ajuste1, operador);

    INSERT INTO tblMovimientoCxC
        (idFacturaEncabezado, idAplicacionPago, idNotaCxC, fechaMovimientoCxC,
         tipoMovimientoCxC, descripcionMovimientoCxC, cargoMovimientoCxC, abonoMovimientoCxC, idUsuarioRegistro)
    VALUES (factura2, NULL, NULL, fechaFactura2, 'FACTURA', 'Factura de prueba', total2, 0, operador),
           (factura2, aplicacion2, NULL, fechaPago2, 'PAGO', 'Cobro de prueba', 0, cobro2, operador),
           (factura2, NULL, nota2, fechaPago2, 'NOTA_CREDITO', 'Crédito de prueba', 0, ajuste2, operador);

    INSERT INTO tblMovimientoCxC
        (idFacturaEncabezado, idAplicacionPago, idNotaCxC, fechaMovimientoCxC,
         tipoMovimientoCxC, descripcionMovimientoCxC, cargoMovimientoCxC, abonoMovimientoCxC, idUsuarioRegistro)
    VALUES (factura3, NULL, NULL, fechaFactura3, 'FACTURA', 'Factura de prueba', total3, 0, operador),
           (factura3, aplicacion3, NULL, fechaPago3, 'PAGO', 'Cobro de prueba', 0, cobro3, operador),
           (factura3, NULL, nota3, fechaPago3, 'NOTA_CREDITO', 'Crédito de prueba', 0, ajuste3, operador);

    INSERT INTO tblMovimientoCxC
        (idFacturaEncabezado, idAplicacionPago, idNotaCxC, fechaMovimientoCxC,
         tipoMovimientoCxC, descripcionMovimientoCxC, cargoMovimientoCxC, abonoMovimientoCxC, idUsuarioRegistro)
    VALUES (factura4, NULL, NULL, fechaFactura4, 'FACTURA', 'Factura de prueba', total4, 0, operador),
           (factura4, aplicacion4, NULL, fechaPago4, 'PAGO', 'Cobro de prueba', 0, cobro4, operador),
           (factura4, NULL, nota4, fechaPago4, 'NOTA_CREDITO', 'Crédito de prueba', 0, ajuste4, operador);

    INSERT INTO tblMovimientoCxC
        (idFacturaEncabezado, idAplicacionPago, idNotaCxC, fechaMovimientoCxC,
         tipoMovimientoCxC, descripcionMovimientoCxC, cargoMovimientoCxC, abonoMovimientoCxC, idUsuarioRegistro)
    VALUES (factura5, NULL, NULL, fechaFactura5, 'FACTURA', 'Factura de prueba', total5, 0, operador),
           (factura5, aplicacion5, NULL, fechaPago5, 'PAGO', 'Cobro de prueba', 0, cobro5, operador),
           (factura5, NULL, nota5, fechaPago5, 'NOTA_CREDITO', 'Crédito de prueba', 0, ajuste5, operador);

    -- tblTipoTransaccion
    INSERT INTO tblTipoTransaccion
        (codigoTipoTransaccion, nombreTipoTransaccion, naturalezaTipoTransaccion)
    VALUES (etiqueta1, CONCAT('Depósito ', etiqueta1), 'INGRESO');

    SET tipoTransaccion1 = LAST_INSERT_ID();

    INSERT INTO tblTipoTransaccion
        (codigoTipoTransaccion, nombreTipoTransaccion, naturalezaTipoTransaccion)
    VALUES (etiqueta2, CONCAT('Depósito ', etiqueta2), 'INGRESO');

    SET tipoTransaccion2 = LAST_INSERT_ID();

    INSERT INTO tblTipoTransaccion
        (codigoTipoTransaccion, nombreTipoTransaccion, naturalezaTipoTransaccion)
    VALUES (etiqueta3, CONCAT('Depósito ', etiqueta3), 'INGRESO');

    SET tipoTransaccion3 = LAST_INSERT_ID();

    INSERT INTO tblTipoTransaccion
        (codigoTipoTransaccion, nombreTipoTransaccion, naturalezaTipoTransaccion)
    VALUES (etiqueta4, CONCAT('Depósito ', etiqueta4), 'INGRESO');

    SET tipoTransaccion4 = LAST_INSERT_ID();

    INSERT INTO tblTipoTransaccion
        (codigoTipoTransaccion, nombreTipoTransaccion, naturalezaTipoTransaccion)
    VALUES (etiqueta5, CONCAT('Depósito ', etiqueta5), 'INGRESO');

    SET tipoTransaccion5 = LAST_INSERT_ID();

    -- tblCuentaBancaria
    INSERT INTO tblCuentaBancaria
        (numeroCuentaBancaria, nombreCuentaBancaria, Fk_Banco_Codigo, Fk_Moneda_Codigo, Fk_TipoCuentaBancaria_Codigo)
    VALUES (etiqueta1, CONCAT('Cuenta ficticia GTQ ', 1), banco1, moneda, tipoCuenta1);

    SET cuentaBanco1 = LAST_INSERT_ID();

    INSERT INTO tblCuentaBancaria
        (numeroCuentaBancaria, nombreCuentaBancaria, Fk_Banco_Codigo, Fk_Moneda_Codigo, Fk_TipoCuentaBancaria_Codigo)
    VALUES (etiqueta2, CONCAT('Cuenta ficticia GTQ ', 2), banco2, moneda, tipoCuenta2);

    SET cuentaBanco2 = LAST_INSERT_ID();

    INSERT INTO tblCuentaBancaria
        (numeroCuentaBancaria, nombreCuentaBancaria, Fk_Banco_Codigo, Fk_Moneda_Codigo, Fk_TipoCuentaBancaria_Codigo)
    VALUES (etiqueta3, CONCAT('Cuenta ficticia GTQ ', 3), banco3, moneda, tipoCuenta3);

    SET cuentaBanco3 = LAST_INSERT_ID();

    INSERT INTO tblCuentaBancaria
        (numeroCuentaBancaria, nombreCuentaBancaria, Fk_Banco_Codigo, Fk_Moneda_Codigo, Fk_TipoCuentaBancaria_Codigo)
    VALUES (etiqueta4, CONCAT('Cuenta ficticia GTQ ', 4), banco4, moneda, tipoCuenta4);

    SET cuentaBanco4 = LAST_INSERT_ID();

    INSERT INTO tblCuentaBancaria
        (numeroCuentaBancaria, nombreCuentaBancaria, Fk_Banco_Codigo, Fk_Moneda_Codigo, Fk_TipoCuentaBancaria_Codigo)
    VALUES (etiqueta5, CONCAT('Cuenta ficticia GTQ ', 5), banco5, moneda, tipoCuenta5);

    SET cuentaBanco5 = LAST_INSERT_ID();

    -- tblMovimientoBancario
    INSERT INTO tblMovimientoBancario
        (referenciaMovimientoBancario, conceptoMovimientoBancario, importeMovimientoBancario, importeGTQMovimientoBancario, sentidoMovimientoBancario, fechaOperacionMovimientoBancario, fechaValorMovimientoBancario, origenMovimientoBancario, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
    VALUES (etiqueta1, 'Depósito de Ventas de prueba', importeDeposito1, importeDeposito1, 1, fechaPago1, fechaPago1, 'MANUAL', cuentaBanco1, tipoTransaccion1);

    SET movimientoBanco1 = LAST_INSERT_ID();

    INSERT INTO tblMovimientoBancario
        (referenciaMovimientoBancario, conceptoMovimientoBancario, importeMovimientoBancario, importeGTQMovimientoBancario, sentidoMovimientoBancario, fechaOperacionMovimientoBancario, fechaValorMovimientoBancario, origenMovimientoBancario, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
    VALUES (etiqueta2, 'Depósito de Ventas de prueba', importeDeposito2, importeDeposito2, 1, fechaPago2, fechaPago2, 'MANUAL', cuentaBanco2, tipoTransaccion2);

    SET movimientoBanco2 = LAST_INSERT_ID();

    INSERT INTO tblMovimientoBancario
        (referenciaMovimientoBancario, conceptoMovimientoBancario, importeMovimientoBancario, importeGTQMovimientoBancario, sentidoMovimientoBancario, fechaOperacionMovimientoBancario, fechaValorMovimientoBancario, origenMovimientoBancario, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
    VALUES (etiqueta3, 'Depósito de Ventas de prueba', importeDeposito3, importeDeposito3, 1, fechaPago3, fechaPago3, 'MANUAL', cuentaBanco3, tipoTransaccion3);

    SET movimientoBanco3 = LAST_INSERT_ID();

    INSERT INTO tblMovimientoBancario
        (referenciaMovimientoBancario, conceptoMovimientoBancario, importeMovimientoBancario, importeGTQMovimientoBancario, sentidoMovimientoBancario, fechaOperacionMovimientoBancario, fechaValorMovimientoBancario, origenMovimientoBancario, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
    VALUES (etiqueta4, 'Depósito de Ventas de prueba', importeDeposito4, importeDeposito4, 1, fechaPago4, fechaPago4, 'MANUAL', cuentaBanco4, tipoTransaccion4);

    SET movimientoBanco4 = LAST_INSERT_ID();

    INSERT INTO tblMovimientoBancario
        (referenciaMovimientoBancario, conceptoMovimientoBancario, importeMovimientoBancario, importeGTQMovimientoBancario, sentidoMovimientoBancario, fechaOperacionMovimientoBancario, fechaValorMovimientoBancario, origenMovimientoBancario, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
    VALUES (etiqueta5, 'Depósito de Ventas de prueba', importeDeposito5, importeDeposito5, 1, fechaPago5, fechaPago5, 'MANUAL', cuentaBanco5, tipoTransaccion5);

    SET movimientoBanco5 = LAST_INSERT_ID();

    -- tblMovimientoBancarioTercero
    INSERT INTO tblMovimientoBancarioTercero
        (idMovimientoBancario, idCliente)
    VALUES (movimientoBanco1, cliente1);

    INSERT INTO tblMovimientoBancarioTercero
        (idMovimientoBancario, idCliente)
    VALUES (movimientoBanco2, cliente2);

    INSERT INTO tblMovimientoBancarioTercero
        (idMovimientoBancario, idCliente)
    VALUES (movimientoBanco3, cliente3);

    INSERT INTO tblMovimientoBancarioTercero
        (idMovimientoBancario, idCliente)
    VALUES (movimientoBanco4, cliente4);

    INSERT INTO tblMovimientoBancarioTercero
        (idMovimientoBancario, idCliente)
    VALUES (movimientoBanco5, cliente5);

    -- tblDepositoVentaEncabezado
    INSERT INTO tblDepositoVentaEncabezado
        (codigoDepositoVentaEncabezado, fechaDepositoVentaEncabezado, referenciaDepositoVentaEncabezado, idMoneda, totalDepositoVentaEncabezado, estadoDepositoVentaEncabezado, idUsuarioRegistro, idCuentaBancaria, idTipoTransaccion, idMovimientoBancario)
    VALUES (etiqueta1, fechaPago1, CONCAT('BAN-', etiqueta1), moneda, importeDeposito1, 'CONFIRMADO', operador, cuentaBanco1, tipoTransaccion1, movimientoBanco1);

    SET deposito1 = LAST_INSERT_ID();

    INSERT INTO tblDepositoVentaEncabezado
        (codigoDepositoVentaEncabezado, fechaDepositoVentaEncabezado, referenciaDepositoVentaEncabezado, idMoneda, totalDepositoVentaEncabezado, estadoDepositoVentaEncabezado, idUsuarioRegistro, idCuentaBancaria, idTipoTransaccion, idMovimientoBancario)
    VALUES (etiqueta2, fechaPago2, CONCAT('BAN-', etiqueta2), moneda, importeDeposito2, 'CONFIRMADO', operador, cuentaBanco2, tipoTransaccion2, movimientoBanco2);

    SET deposito2 = LAST_INSERT_ID();

    INSERT INTO tblDepositoVentaEncabezado
        (codigoDepositoVentaEncabezado, fechaDepositoVentaEncabezado, referenciaDepositoVentaEncabezado, idMoneda, totalDepositoVentaEncabezado, estadoDepositoVentaEncabezado, idUsuarioRegistro, idCuentaBancaria, idTipoTransaccion, idMovimientoBancario)
    VALUES (etiqueta3, fechaPago3, CONCAT('BAN-', etiqueta3), moneda, importeDeposito3, 'CONFIRMADO', operador, cuentaBanco3, tipoTransaccion3, movimientoBanco3);

    SET deposito3 = LAST_INSERT_ID();

    INSERT INTO tblDepositoVentaEncabezado
        (codigoDepositoVentaEncabezado, fechaDepositoVentaEncabezado, referenciaDepositoVentaEncabezado, idMoneda, totalDepositoVentaEncabezado, estadoDepositoVentaEncabezado, idUsuarioRegistro, idCuentaBancaria, idTipoTransaccion, idMovimientoBancario)
    VALUES (etiqueta4, fechaPago4, CONCAT('BAN-', etiqueta4), moneda, importeDeposito4, 'CONFIRMADO', operador, cuentaBanco4, tipoTransaccion4, movimientoBanco4);

    SET deposito4 = LAST_INSERT_ID();

    INSERT INTO tblDepositoVentaEncabezado
        (codigoDepositoVentaEncabezado, fechaDepositoVentaEncabezado, referenciaDepositoVentaEncabezado, idMoneda, totalDepositoVentaEncabezado, estadoDepositoVentaEncabezado, idUsuarioRegistro, idCuentaBancaria, idTipoTransaccion, idMovimientoBancario)
    VALUES (etiqueta5, fechaPago5, CONCAT('BAN-', etiqueta5), moneda, importeDeposito5, 'CONFIRMADO', operador, cuentaBanco5, tipoTransaccion5, movimientoBanco5);

    SET deposito5 = LAST_INSERT_ID();

    -- tblDepositoVentaDetalle
    INSERT INTO tblDepositoVentaDetalle
        (idDepositoVentaEncabezado, idReciboPago, importeDepositoVentaDetalle)
    VALUES (deposito1, recibo1, importeDeposito1);

    INSERT INTO tblDepositoVentaDetalle
        (idDepositoVentaEncabezado, idReciboPago, importeDepositoVentaDetalle)
    VALUES (deposito2, recibo2, importeDeposito2);

    INSERT INTO tblDepositoVentaDetalle
        (idDepositoVentaEncabezado, idReciboPago, importeDepositoVentaDetalle)
    VALUES (deposito3, recibo3, importeDeposito3);

    INSERT INTO tblDepositoVentaDetalle
        (idDepositoVentaEncabezado, idReciboPago, importeDepositoVentaDetalle)
    VALUES (deposito4, recibo4, importeDeposito4);

    INSERT INTO tblDepositoVentaDetalle
        (idDepositoVentaEncabezado, idReciboPago, importeDepositoVentaDetalle)
    VALUES (deposito5, recibo5, importeDeposito5);

    -- tblReglaComision
    INSERT INTO tblReglaComision
        (codigoReglaComision, nombreReglaComision, baseCalculoReglaComision, tasaComisionReglaComision, vigenteDesdeReglaComision)
    VALUES (etiqueta1, CONCAT('Comisión 5% ', etiqueta1), 'COBRO_NETO', 0.05, inicioMes1);

    SET regla1 = LAST_INSERT_ID();

    INSERT INTO tblReglaComision
        (codigoReglaComision, nombreReglaComision, baseCalculoReglaComision, tasaComisionReglaComision, vigenteDesdeReglaComision)
    VALUES (etiqueta2, CONCAT('Comisión 5% ', etiqueta2), 'COBRO_NETO', 0.05, inicioMes2);

    SET regla2 = LAST_INSERT_ID();

    INSERT INTO tblReglaComision
        (codigoReglaComision, nombreReglaComision, baseCalculoReglaComision, tasaComisionReglaComision, vigenteDesdeReglaComision)
    VALUES (etiqueta3, CONCAT('Comisión 5% ', etiqueta3), 'COBRO_NETO', 0.05, inicioMes3);

    SET regla3 = LAST_INSERT_ID();

    INSERT INTO tblReglaComision
        (codigoReglaComision, nombreReglaComision, baseCalculoReglaComision, tasaComisionReglaComision, vigenteDesdeReglaComision)
    VALUES (etiqueta4, CONCAT('Comisión 5% ', etiqueta4), 'COBRO_NETO', 0.05, inicioMes4);

    SET regla4 = LAST_INSERT_ID();

    INSERT INTO tblReglaComision
        (codigoReglaComision, nombreReglaComision, baseCalculoReglaComision, tasaComisionReglaComision, vigenteDesdeReglaComision)
    VALUES (etiqueta5, CONCAT('Comisión 5% ', etiqueta5), 'COBRO_NETO', 0.05, inicioMes5);

    SET regla5 = LAST_INSERT_ID();

    -- tblComisionEncabezado
    INSERT INTO tblComisionEncabezado
        (numeroComisionEncabezado, idVendedor, idReglaComision, idMoneda, fechaDesdeComisionEncabezado, fechaHastaComisionEncabezado, estadoComisionEncabezado, idUsuarioSolicita)
    VALUES (etiqueta1, vendedor1, regla1, moneda, inicioMes1, finMes1, 'BORRADOR', operador);

    SET comision1 = LAST_INSERT_ID();

    INSERT INTO tblComisionEncabezado
        (numeroComisionEncabezado, idVendedor, idReglaComision, idMoneda, fechaDesdeComisionEncabezado, fechaHastaComisionEncabezado, estadoComisionEncabezado, idUsuarioSolicita)
    VALUES (etiqueta2, vendedor2, regla2, moneda, inicioMes2, finMes2, 'BORRADOR', operador);

    SET comision2 = LAST_INSERT_ID();

    INSERT INTO tblComisionEncabezado
        (numeroComisionEncabezado, idVendedor, idReglaComision, idMoneda, fechaDesdeComisionEncabezado, fechaHastaComisionEncabezado, estadoComisionEncabezado, idUsuarioSolicita)
    VALUES (etiqueta3, vendedor3, regla3, moneda, inicioMes3, finMes3, 'BORRADOR', operador);

    SET comision3 = LAST_INSERT_ID();

    INSERT INTO tblComisionEncabezado
        (numeroComisionEncabezado, idVendedor, idReglaComision, idMoneda, fechaDesdeComisionEncabezado, fechaHastaComisionEncabezado, estadoComisionEncabezado, idUsuarioSolicita)
    VALUES (etiqueta4, vendedor4, regla4, moneda, inicioMes4, finMes4, 'BORRADOR', operador);

    SET comision4 = LAST_INSERT_ID();

    INSERT INTO tblComisionEncabezado
        (numeroComisionEncabezado, idVendedor, idReglaComision, idMoneda, fechaDesdeComisionEncabezado, fechaHastaComisionEncabezado, estadoComisionEncabezado, idUsuarioSolicita)
    VALUES (etiqueta5, vendedor5, regla5, moneda, inicioMes5, finMes5, 'BORRADOR', operador);

    SET comision5 = LAST_INSERT_ID();

    -- tblComisionDetalle
    INSERT INTO tblComisionDetalle
        (idComisionEncabezado, idFacturaEncabezado, idAplicacionPago, baseComisionDetalle, tasaAplicadaComisionDetalle)
    VALUES (comision1, factura1, aplicacion1, ROUND(baseFactura1 * cobro1 / total1, 4), 0.05);

    INSERT INTO tblComisionDetalle
        (idComisionEncabezado, idFacturaEncabezado, idAplicacionPago, baseComisionDetalle, tasaAplicadaComisionDetalle)
    VALUES (comision2, factura2, aplicacion2, ROUND(baseFactura2 * cobro2 / total2, 4), 0.05);

    INSERT INTO tblComisionDetalle
        (idComisionEncabezado, idFacturaEncabezado, idAplicacionPago, baseComisionDetalle, tasaAplicadaComisionDetalle)
    VALUES (comision3, factura3, aplicacion3, ROUND(baseFactura3 * cobro3 / total3, 4), 0.05);

    INSERT INTO tblComisionDetalle
        (idComisionEncabezado, idFacturaEncabezado, idAplicacionPago, baseComisionDetalle, tasaAplicadaComisionDetalle)
    VALUES (comision4, factura4, aplicacion4, ROUND(baseFactura4 * cobro4 / total4, 4), 0.05);

    INSERT INTO tblComisionDetalle
        (idComisionEncabezado, idFacturaEncabezado, idAplicacionPago, baseComisionDetalle, tasaAplicadaComisionDetalle)
    VALUES (comision5, factura5, aplicacion5, ROUND(baseFactura5 * cobro5 / total5, 4), 0.05);

    -- tblComisionFacturaEncabezado
    INSERT INTO tblComisionFacturaEncabezado
        (idFacturaEncabezado, metodoComisionFacturaEncabezado)
    VALUES (factura1, 'PRODUCTO');

    SET comisionFactura1 = LAST_INSERT_ID();

    INSERT INTO tblComisionFacturaEncabezado
        (idFacturaEncabezado, metodoComisionFacturaEncabezado)
    VALUES (factura2, 'PRODUCTO');

    SET comisionFactura2 = LAST_INSERT_ID();

    INSERT INTO tblComisionFacturaEncabezado
        (idFacturaEncabezado, metodoComisionFacturaEncabezado)
    VALUES (factura3, 'PRODUCTO');

    SET comisionFactura3 = LAST_INSERT_ID();

    INSERT INTO tblComisionFacturaEncabezado
        (idFacturaEncabezado, metodoComisionFacturaEncabezado)
    VALUES (factura4, 'PRODUCTO');

    SET comisionFactura4 = LAST_INSERT_ID();

    INSERT INTO tblComisionFacturaEncabezado
        (idFacturaEncabezado, metodoComisionFacturaEncabezado)
    VALUES (factura5, 'PRODUCTO');

    SET comisionFactura5 = LAST_INSERT_ID();

    -- tblComisionFacturaDetalle
    INSERT INTO tblComisionFacturaDetalle
        (idComisionFacturaEncabezado, idFacturaDetalle, metodoComisionFacturaDetalle, nombreClasificacionHistoricaComisionFacturaDetalle, tasaAplicadaComisionFacturaDetalle, baseComisionFacturaDetalle)
    VALUES (comisionFactura1, facturaDetalle1, 'PRODUCTO', nombreProductoDemo1, 0.05, baseFactura1);

    INSERT INTO tblComisionFacturaDetalle
        (idComisionFacturaEncabezado, idFacturaDetalle, metodoComisionFacturaDetalle, nombreClasificacionHistoricaComisionFacturaDetalle, tasaAplicadaComisionFacturaDetalle, baseComisionFacturaDetalle)
    VALUES (comisionFactura2, facturaDetalle2, 'PRODUCTO', nombreProductoDemo2, 0.05, baseFactura2);

    INSERT INTO tblComisionFacturaDetalle
        (idComisionFacturaEncabezado, idFacturaDetalle, metodoComisionFacturaDetalle, nombreClasificacionHistoricaComisionFacturaDetalle, tasaAplicadaComisionFacturaDetalle, baseComisionFacturaDetalle)
    VALUES (comisionFactura3, facturaDetalle3, 'PRODUCTO', nombreProductoDemo3, 0.05, baseFactura3);

    INSERT INTO tblComisionFacturaDetalle
        (idComisionFacturaEncabezado, idFacturaDetalle, metodoComisionFacturaDetalle, nombreClasificacionHistoricaComisionFacturaDetalle, tasaAplicadaComisionFacturaDetalle, baseComisionFacturaDetalle)
    VALUES (comisionFactura4, facturaDetalle4, 'PRODUCTO', nombreProductoDemo4, 0.05, baseFactura4);

    INSERT INTO tblComisionFacturaDetalle
        (idComisionFacturaEncabezado, idFacturaDetalle, metodoComisionFacturaDetalle, nombreClasificacionHistoricaComisionFacturaDetalle, tasaAplicadaComisionFacturaDetalle, baseComisionFacturaDetalle)
    VALUES (comisionFactura5, facturaDetalle5, 'PRODUCTO', nombreProductoDemo5, 0.05, baseFactura5);

    -- tblComisionAbono
    INSERT INTO tblComisionAbono
        (idComisionFacturaEncabezado, idAplicacionPago, tasaFacturaPagadaAcumuladaComisionAbono, montoComisionDevengadoComisionAbono)
    VALUES (comisionFactura1, aplicacion1, ROUND(cobro1 / total1, 4), ROUND(baseFactura1 * 0.05 * cobro1 / total1, 4));

    INSERT INTO tblComisionAbono
        (idComisionFacturaEncabezado, idAplicacionPago, tasaFacturaPagadaAcumuladaComisionAbono, montoComisionDevengadoComisionAbono)
    VALUES (comisionFactura2, aplicacion2, ROUND(cobro2 / total2, 4), ROUND(baseFactura2 * 0.05 * cobro2 / total2, 4));

    INSERT INTO tblComisionAbono
        (idComisionFacturaEncabezado, idAplicacionPago, tasaFacturaPagadaAcumuladaComisionAbono, montoComisionDevengadoComisionAbono)
    VALUES (comisionFactura3, aplicacion3, ROUND(cobro3 / total3, 4), ROUND(baseFactura3 * 0.05 * cobro3 / total3, 4));

    INSERT INTO tblComisionAbono
        (idComisionFacturaEncabezado, idAplicacionPago, tasaFacturaPagadaAcumuladaComisionAbono, montoComisionDevengadoComisionAbono)
    VALUES (comisionFactura4, aplicacion4, ROUND(cobro4 / total4, 4), ROUND(baseFactura4 * 0.05 * cobro4 / total4, 4));

    INSERT INTO tblComisionAbono
        (idComisionFacturaEncabezado, idAplicacionPago, tasaFacturaPagadaAcumuladaComisionAbono, montoComisionDevengadoComisionAbono)
    VALUES (comisionFactura5, aplicacion5, ROUND(cobro5 / total5, 4), ROUND(baseFactura5 * 0.05 * cobro5 / total5, 4));

    -- tblCierreMovimientoCxCEncabezado
    INSERT INTO tblCierreMovimientoCxCEncabezado
        (numeroCierreMovimientoCxCEncabezado, fechaDesdeCierreMovimientoCxCEncabezado, fechaHastaCierreMovimientoCxCEncabezado, fechaCorteCierreMovimientoCxCEncabezado, idMoneda, tipoCierreMovimientoCxCEncabezado, estadoCierreMovimientoCxCEncabezado, idUsuarioResponsable)
    VALUES (etiqueta1, inicioMes1, finMes1, finMes1, moneda, 'MENSUAL', 'BORRADOR', operador);

    SET cierre1 = LAST_INSERT_ID();

    INSERT INTO tblCierreMovimientoCxCEncabezado
        (numeroCierreMovimientoCxCEncabezado, fechaDesdeCierreMovimientoCxCEncabezado, fechaHastaCierreMovimientoCxCEncabezado, fechaCorteCierreMovimientoCxCEncabezado, idMoneda, tipoCierreMovimientoCxCEncabezado, estadoCierreMovimientoCxCEncabezado, idUsuarioResponsable)
    VALUES (etiqueta2, inicioMes2, finMes2, finMes2, moneda, 'MENSUAL', 'BORRADOR', operador);

    SET cierre2 = LAST_INSERT_ID();

    INSERT INTO tblCierreMovimientoCxCEncabezado
        (numeroCierreMovimientoCxCEncabezado, fechaDesdeCierreMovimientoCxCEncabezado, fechaHastaCierreMovimientoCxCEncabezado, fechaCorteCierreMovimientoCxCEncabezado, idMoneda, tipoCierreMovimientoCxCEncabezado, estadoCierreMovimientoCxCEncabezado, idUsuarioResponsable)
    VALUES (etiqueta3, inicioMes3, finMes3, finMes3, moneda, 'MENSUAL', 'BORRADOR', operador);

    SET cierre3 = LAST_INSERT_ID();

    INSERT INTO tblCierreMovimientoCxCEncabezado
        (numeroCierreMovimientoCxCEncabezado, fechaDesdeCierreMovimientoCxCEncabezado, fechaHastaCierreMovimientoCxCEncabezado, fechaCorteCierreMovimientoCxCEncabezado, idMoneda, tipoCierreMovimientoCxCEncabezado, estadoCierreMovimientoCxCEncabezado, idUsuarioResponsable)
    VALUES (etiqueta4, inicioMes4, finMes4, finMes4, moneda, 'MENSUAL', 'BORRADOR', operador);

    SET cierre4 = LAST_INSERT_ID();

    INSERT INTO tblCierreMovimientoCxCEncabezado
        (numeroCierreMovimientoCxCEncabezado, fechaDesdeCierreMovimientoCxCEncabezado, fechaHastaCierreMovimientoCxCEncabezado, fechaCorteCierreMovimientoCxCEncabezado, idMoneda, tipoCierreMovimientoCxCEncabezado, estadoCierreMovimientoCxCEncabezado, idUsuarioResponsable)
    VALUES (etiqueta5, inicioMes5, finMes5, finMes5, moneda, 'MENSUAL', 'BORRADOR', operador);

    SET cierre5 = LAST_INSERT_ID();

    -- tblCierreSaldoCliente
    INSERT INTO tblCierreSaldoCliente
        (idCierreMovimientoCxCEncabezado, idCliente, codigoClienteCierreSaldoCliente, nombreClienteCierreSaldoCliente, saldoAnteriorCierreSaldoCliente, cargosPeriodoCierreSaldoCliente, abonosPeriodoCierreSaldoCliente, cargosAcumuladosCierreSaldoCliente, abonosAcumuladosCierreSaldoCliente, saldoActualCierreSaldoCliente)
    VALUES (cierre1, cliente1, etiqueta1, nombreClienteDemo1, 0, total1, cobro1 + ajuste1, total1, cobro1 + ajuste1, saldo1);

    INSERT INTO tblCierreSaldoCliente
        (idCierreMovimientoCxCEncabezado, idCliente, codigoClienteCierreSaldoCliente, nombreClienteCierreSaldoCliente, saldoAnteriorCierreSaldoCliente, cargosPeriodoCierreSaldoCliente, abonosPeriodoCierreSaldoCliente, cargosAcumuladosCierreSaldoCliente, abonosAcumuladosCierreSaldoCliente, saldoActualCierreSaldoCliente)
    VALUES (cierre2, cliente2, etiqueta2, nombreClienteDemo2, 0, total2, cobro2 + ajuste2, total2, cobro2 + ajuste2, saldo2);

    INSERT INTO tblCierreSaldoCliente
        (idCierreMovimientoCxCEncabezado, idCliente, codigoClienteCierreSaldoCliente, nombreClienteCierreSaldoCliente, saldoAnteriorCierreSaldoCliente, cargosPeriodoCierreSaldoCliente, abonosPeriodoCierreSaldoCliente, cargosAcumuladosCierreSaldoCliente, abonosAcumuladosCierreSaldoCliente, saldoActualCierreSaldoCliente)
    VALUES (cierre3, cliente3, etiqueta3, nombreClienteDemo3, 0, total3, cobro3 + ajuste3, total3, cobro3 + ajuste3, saldo3);

    INSERT INTO tblCierreSaldoCliente
        (idCierreMovimientoCxCEncabezado, idCliente, codigoClienteCierreSaldoCliente, nombreClienteCierreSaldoCliente, saldoAnteriorCierreSaldoCliente, cargosPeriodoCierreSaldoCliente, abonosPeriodoCierreSaldoCliente, cargosAcumuladosCierreSaldoCliente, abonosAcumuladosCierreSaldoCliente, saldoActualCierreSaldoCliente)
    VALUES (cierre4, cliente4, etiqueta4, nombreClienteDemo4, 0, total4, cobro4 + ajuste4, total4, cobro4 + ajuste4, saldo4);

    INSERT INTO tblCierreSaldoCliente
        (idCierreMovimientoCxCEncabezado, idCliente, codigoClienteCierreSaldoCliente, nombreClienteCierreSaldoCliente, saldoAnteriorCierreSaldoCliente, cargosPeriodoCierreSaldoCliente, abonosPeriodoCierreSaldoCliente, cargosAcumuladosCierreSaldoCliente, abonosAcumuladosCierreSaldoCliente, saldoActualCierreSaldoCliente)
    VALUES (cierre5, cliente5, etiqueta5, nombreClienteDemo5, 0, total5, cobro5 + ajuste5, total5, cobro5 + ajuste5, saldo5);

    -- tblCierreMovimientoCxCDetalle
    INSERT INTO tblCierreMovimientoCxCDetalle
        (idCierreMovimientoCxCEncabezado, idFacturaEncabezado, saldoInicialCierreMovimientoCxCDetalle, cargosPeriodoCierreMovimientoCxCDetalle, abonosPeriodoCierreMovimientoCxCDetalle, saldoFinalCierreMovimientoCxCDetalle)
    VALUES (cierre1, factura1, 0, total1, cobro1 + ajuste1, saldo1);

    INSERT INTO tblCierreMovimientoCxCDetalle
        (idCierreMovimientoCxCEncabezado, idFacturaEncabezado, saldoInicialCierreMovimientoCxCDetalle, cargosPeriodoCierreMovimientoCxCDetalle, abonosPeriodoCierreMovimientoCxCDetalle, saldoFinalCierreMovimientoCxCDetalle)
    VALUES (cierre2, factura2, 0, total2, cobro2 + ajuste2, saldo2);

    INSERT INTO tblCierreMovimientoCxCDetalle
        (idCierreMovimientoCxCEncabezado, idFacturaEncabezado, saldoInicialCierreMovimientoCxCDetalle, cargosPeriodoCierreMovimientoCxCDetalle, abonosPeriodoCierreMovimientoCxCDetalle, saldoFinalCierreMovimientoCxCDetalle)
    VALUES (cierre3, factura3, 0, total3, cobro3 + ajuste3, saldo3);

    INSERT INTO tblCierreMovimientoCxCDetalle
        (idCierreMovimientoCxCEncabezado, idFacturaEncabezado, saldoInicialCierreMovimientoCxCDetalle, cargosPeriodoCierreMovimientoCxCDetalle, abonosPeriodoCierreMovimientoCxCDetalle, saldoFinalCierreMovimientoCxCDetalle)
    VALUES (cierre4, factura4, 0, total4, cobro4 + ajuste4, saldo4);

    INSERT INTO tblCierreMovimientoCxCDetalle
        (idCierreMovimientoCxCEncabezado, idFacturaEncabezado, saldoInicialCierreMovimientoCxCDetalle, cargosPeriodoCierreMovimientoCxCDetalle, abonosPeriodoCierreMovimientoCxCDetalle, saldoFinalCierreMovimientoCxCDetalle)
    VALUES (cierre5, factura5, 0, total5, cobro5 + ajuste5, saldo5);

    -- tblTrasladoCxCEncabezado
    INSERT INTO tblTrasladoCxCEncabezado
        (idCierreMovimientoCxCEncabezado, claveIntegracionTrasladoCxCEncabezado, estadoTrasladoCxCEncabezado, idUsuario)
    VALUES (cierre1, etiqueta1, 'PREPARADO', operador);

    SET traslado1 = LAST_INSERT_ID();

    INSERT INTO tblTrasladoCxCEncabezado
        (idCierreMovimientoCxCEncabezado, claveIntegracionTrasladoCxCEncabezado, estadoTrasladoCxCEncabezado, idUsuario)
    VALUES (cierre2, etiqueta2, 'PREPARADO', operador);

    SET traslado2 = LAST_INSERT_ID();

    INSERT INTO tblTrasladoCxCEncabezado
        (idCierreMovimientoCxCEncabezado, claveIntegracionTrasladoCxCEncabezado, estadoTrasladoCxCEncabezado, idUsuario)
    VALUES (cierre3, etiqueta3, 'PREPARADO', operador);

    SET traslado3 = LAST_INSERT_ID();

    INSERT INTO tblTrasladoCxCEncabezado
        (idCierreMovimientoCxCEncabezado, claveIntegracionTrasladoCxCEncabezado, estadoTrasladoCxCEncabezado, idUsuario)
    VALUES (cierre4, etiqueta4, 'PREPARADO', operador);

    SET traslado4 = LAST_INSERT_ID();

    INSERT INTO tblTrasladoCxCEncabezado
        (idCierreMovimientoCxCEncabezado, claveIntegracionTrasladoCxCEncabezado, estadoTrasladoCxCEncabezado, idUsuario)
    VALUES (cierre5, etiqueta5, 'PREPARADO', operador);

    SET traslado5 = LAST_INSERT_ID();

    -- tblTrasladoCxCDetalle
    INSERT INTO tblTrasladoCxCDetalle
        (idTrasladoCxCEncabezado, numeroLineaTrasladoCxCDetalle, cuentaTrasladoCxCDetalle,
         descripcionTrasladoCxCDetalle, debeTrasladoCxCDetalle, haberTrasladoCxCDetalle)
    VALUES (traslado1, 1, '1103', 'Saldo neto de cliente demo', saldo1, 0),
           (traslado1, 2, '4101', 'Contrapartida de saldo demo', 0, saldo1);

    INSERT INTO tblTrasladoCxCDetalle
        (idTrasladoCxCEncabezado, numeroLineaTrasladoCxCDetalle, cuentaTrasladoCxCDetalle,
         descripcionTrasladoCxCDetalle, debeTrasladoCxCDetalle, haberTrasladoCxCDetalle)
    VALUES (traslado2, 1, '1103', 'Saldo neto de cliente demo', saldo2, 0),
           (traslado2, 2, '4101', 'Contrapartida de saldo demo', 0, saldo2);

    INSERT INTO tblTrasladoCxCDetalle
        (idTrasladoCxCEncabezado, numeroLineaTrasladoCxCDetalle, cuentaTrasladoCxCDetalle,
         descripcionTrasladoCxCDetalle, debeTrasladoCxCDetalle, haberTrasladoCxCDetalle)
    VALUES (traslado3, 1, '1103', 'Saldo neto de cliente demo', saldo3, 0),
           (traslado3, 2, '4101', 'Contrapartida de saldo demo', 0, saldo3);

    INSERT INTO tblTrasladoCxCDetalle
        (idTrasladoCxCEncabezado, numeroLineaTrasladoCxCDetalle, cuentaTrasladoCxCDetalle,
         descripcionTrasladoCxCDetalle, debeTrasladoCxCDetalle, haberTrasladoCxCDetalle)
    VALUES (traslado4, 1, '1103', 'Saldo neto de cliente demo', saldo4, 0),
           (traslado4, 2, '4101', 'Contrapartida de saldo demo', 0, saldo4);

    INSERT INTO tblTrasladoCxCDetalle
        (idTrasladoCxCEncabezado, numeroLineaTrasladoCxCDetalle, cuentaTrasladoCxCDetalle,
         descripcionTrasladoCxCDetalle, debeTrasladoCxCDetalle, haberTrasladoCxCDetalle)
    VALUES (traslado5, 1, '1103', 'Saldo neto de cliente demo', saldo5, 0),
           (traslado5, 2, '4101', 'Contrapartida de saldo demo', 0, saldo5);

    COMMIT;
END$$
DELIMITER ;

CALL spCargarPruebasVentasERP16();
DROP PROCEDURE spCargarPruebasVentasERP16;

SELECT c.codigoCliente, c.nombreCliente, m.codigoMoneda, p.codigoFormaPagoVenta,
       r.numeroReciboPago, r.montoReciboPago, r.referenciaReciboPago
FROM tblCliente c
JOIN tblMoneda m ON m.idMoneda = c.idMonedaCredito
JOIN tblReciboPago r ON r.idCliente = c.idCliente AND r.numeroReciboPago = c.codigoCliente
JOIN tblFormaPagoVenta p ON p.idFormaPagoVenta = r.idFormaPagoVenta
WHERE c.codigoCliente IN ('V16-DEMO-1', 'V16-DEMO-2', 'V16-DEMO-3', 'V16-DEMO-4', 'V16-DEMO-5')
ORDER BY c.codigoCliente;
