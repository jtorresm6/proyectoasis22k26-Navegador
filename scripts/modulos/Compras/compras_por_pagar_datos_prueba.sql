USE `dbsistemaembutidos`;

SET NAMES utf8mb4;

START TRANSACTION;

INSERT INTO tblProveedor
    (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, estadoProveedor)
VALUES
    ('Proveedor Prueba - Carnes del Norte', '9900000001', 'carnes.norte@example.com', '5550-0101', 'Zona 1, Ciudad de Guatemala', 1),
    ('Proveedor Prueba - Especias del Valle', '9900000003', 'especias.valle@example.com', '5550-0103', 'Zona 3, Mixco', 1),
    ('Proveedor Prueba - Empaques del Centro', '9900000004', 'empaques.centro@example.com', '5550-0104', 'Zona 4, Villa Nueva', 1),
    ('Proveedor Prueba - Empaques Inactivo', '9900000011', 'empaques.inactivo@example.com', '5550-0111', 'Zona 2, Mixco', 0)
ON DUPLICATE KEY UPDATE idProveedor = tblProveedor.idProveedor;

INSERT INTO tblCuentaPorPagar
    (idProveedor, numeroFacturaCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar,
     montoTotalCuentaPorPagar, saldoCuentaPorPagar, estadoCuentaPorPagar)
SELECT idProveedor, 'PRUEBA-CXP-001', DATE_SUB(CURDATE(), INTERVAL 12 DAY), DATE_ADD(CURDATE(), INTERVAL 18 DAY), 1250.0000, 1250.0000, 'Pendiente'
FROM tblProveedor WHERE nitProveedor = '9900000001'
ON DUPLICATE KEY UPDATE idCuentaPorPagar = tblCuentaPorPagar.idCuentaPorPagar;

INSERT INTO tblCuentaPorPagar
    (idProveedor, numeroFacturaCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar,
     montoTotalCuentaPorPagar, saldoCuentaPorPagar, estadoCuentaPorPagar)
SELECT idProveedor, 'PRUEBA-CXP-002', DATE_SUB(CURDATE(), INTERVAL 8 DAY), DATE_ADD(CURDATE(), INTERVAL 22 DAY), 980.7500, 480.7500, 'Parcial'
FROM tblProveedor WHERE nitProveedor = '9900000003'
ON DUPLICATE KEY UPDATE idCuentaPorPagar = tblCuentaPorPagar.idCuentaPorPagar;

INSERT INTO tblCuentaPorPagar
    (idProveedor, numeroFacturaCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar,
     montoTotalCuentaPorPagar, saldoCuentaPorPagar, estadoCuentaPorPagar)
SELECT idProveedor, 'PRUEBA-CXP-003', DATE_SUB(CURDATE(), INTERVAL 40 DAY), DATE_SUB(CURDATE(), INTERVAL 10 DAY), 2300.0000, 0.0000, 'Pagada'
FROM tblProveedor WHERE nitProveedor = '9900000004'
ON DUPLICATE KEY UPDATE idCuentaPorPagar = tblCuentaPorPagar.idCuentaPorPagar;

INSERT INTO tblCuentaPorPagar
    (idProveedor, numeroFacturaCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar,
     montoTotalCuentaPorPagar, saldoCuentaPorPagar, estadoCuentaPorPagar)
SELECT idProveedor, 'PRUEBA-CXP-004', DATE_SUB(CURDATE(), INTERVAL 5 DAY), DATE_ADD(CURDATE(), INTERVAL 25 DAY), 500.0000, 0.0000, 'Anulada'
FROM tblProveedor WHERE nitProveedor = '9900000011'
ON DUPLICATE KEY UPDATE idCuentaPorPagar = tblCuentaPorPagar.idCuentaPorPagar;

INSERT INTO tblCuentaPorPagar
    (idProveedor, numeroFacturaCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar,
     montoTotalCuentaPorPagar, saldoCuentaPorPagar, estadoCuentaPorPagar)
SELECT idProveedor, 'PRUEBA-CXP-005', DATE_SUB(CURDATE(), INTERVAL 45 DAY), DATE_SUB(CURDATE(), INTERVAL 15 DAY), 875.5000, 875.5000, 'Pendiente'
FROM tblProveedor WHERE nitProveedor = '9900000001'
ON DUPLICATE KEY UPDATE idCuentaPorPagar = tblCuentaPorPagar.idCuentaPorPagar;

INSERT INTO tblCuentaPorPagar
    (idProveedor, numeroFacturaCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar,
     montoTotalCuentaPorPagar, saldoCuentaPorPagar, estadoCuentaPorPagar)
SELECT idProveedor, 'PRUEBA-CXP-006', DATE_SUB(CURDATE(), INTERVAL 60 DAY), DATE_SUB(CURDATE(), INTERVAL 30 DAY), 3600.0000, 1200.0000, 'Parcial'
FROM tblProveedor WHERE nitProveedor = '9900000003'
ON DUPLICATE KEY UPDATE idCuentaPorPagar = tblCuentaPorPagar.idCuentaPorPagar;

INSERT INTO tblCuentaPorPagar
    (idProveedor, numeroFacturaCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar,
     montoTotalCuentaPorPagar, saldoCuentaPorPagar, estadoCuentaPorPagar)
SELECT idProveedor, 'PRUEBA-CXP-007', CURDATE(), DATE_ADD(CURDATE(), INTERVAL 30 DAY), 120.9900, 120.9900, 'Pendiente'
FROM tblProveedor WHERE nitProveedor = '9900000004'
ON DUPLICATE KEY UPDATE idCuentaPorPagar = tblCuentaPorPagar.idCuentaPorPagar;

INSERT INTO tblCuentaPorPagar
    (idProveedor, numeroFacturaCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar,
     montoTotalCuentaPorPagar, saldoCuentaPorPagar, estadoCuentaPorPagar)
SELECT idProveedor, 'PRUEBA-CXP-008', DATE_SUB(CURDATE(), INTERVAL 90 DAY), DATE_SUB(CURDATE(), INTERVAL 60 DAY), 475.0000, 0.0000, 'Pagada'
FROM tblProveedor WHERE nitProveedor = '9900000001'
ON DUPLICATE KEY UPDATE idCuentaPorPagar = tblCuentaPorPagar.idCuentaPorPagar;
INSERT INTO tblPagoProveedor
    (idCuentaPorPagar, idProveedor, fechaPagoProveedor, montoPagoProveedor,
     metodoPagoProveedor, referenciaPagoProveedor, estadoPagoProveedor)
SELECT c.idCuentaPorPagar, c.idProveedor,
       DATE_ADD(LAST_DAY(DATE_SUB(CURDATE(), INTERVAL 1 MONTH)), INTERVAL d.dia DAY),
       d.monto, d.metodo, d.referencia, d.estado
FROM (
    SELECT '9900000003' AS nit, 'PRUEBA-CXP-002' AS factura, 1 AS dia,
           500.0000 AS monto, 'Transferencia' AS metodo,
           'PRUEBA-PAGO-001' AS referencia, 'Programado' AS estado
    UNION ALL SELECT '9900000004', 'PRUEBA-CXP-003', 1, 2300.0000, 'Cheque', 'PRUEBA-PAGO-002', 'Pagado'
    UNION ALL SELECT '9900000003', 'PRUEBA-CXP-006', 1, 2400.0000, 'Deposito', 'PRUEBA-PAGO-003', 'Pagado'
    UNION ALL SELECT '9900000001', 'PRUEBA-CXP-008', 1, 475.0000, 'Efectivo', 'PRUEBA-PAGO-004', 'Pagado'
    UNION ALL SELECT '9900000001', 'PRUEBA-CXP-001', 1, 100.0000, 'Transferencia', 'PRUEBA-PAGO-005', 'Rechazado'
    UNION ALL SELECT '9900000001', 'PRUEBA-CXP-005', 1, 150.0000, 'Cheque', 'PRUEBA-PAGO-006', 'Anulado'
) d
INNER JOIN tblProveedor pr ON pr.nitProveedor = d.nit
INNER JOIN tblCuentaPorPagar c
        ON c.idProveedor = pr.idProveedor AND c.numeroFacturaCuentaPorPagar = d.factura
WHERE NOT EXISTS (
    SELECT 1 FROM tblPagoProveedor existente
    WHERE existente.idCuentaPorPagar = c.idCuentaPorPagar
      AND existente.referenciaPagoProveedor = d.referencia
);
INSERT INTO tblSolicitudPago
    (moduloOrigenSolicitudPago, idOrigenSolicitudPago, beneficiarioSolicitudPago,
     montoSolicitudPago, metodoSolicitudPago, estadoSolicitudPago)
SELECT 'Compras', p.idPagoProveedor, pr.nombreProveedor,
       p.montoPagoProveedor, p.metodoPagoProveedor,
       CASE WHEN p.estadoPagoProveedor = 'Pagado' THEN 'Procesada' ELSE 'Pendiente' END
FROM tblPagoProveedor p
INNER JOIN tblProveedor pr ON pr.idProveedor = p.idProveedor
WHERE p.referenciaPagoProveedor IN
      ('PRUEBA-PAGO-001', 'PRUEBA-PAGO-002', 'PRUEBA-PAGO-003', 'PRUEBA-PAGO-004')
  AND p.estadoPagoProveedor IN ('Pagado', 'Programado')
  AND NOT EXISTS (
      SELECT 1 FROM tblSolicitudPago s
      WHERE s.moduloOrigenSolicitudPago = 'Compras'
        AND s.idOrigenSolicitudPago = p.idPagoProveedor
  );

COMMIT;



DELIMITER $$

DROP PROCEDURE IF EXISTS spInsertarPagoProveedor$$
CREATE PROCEDURE spInsertarPagoProveedor(
    IN pCuentaPorPagar INT,
    IN pFecha          DATE,
    IN pMonto          DECIMAL(12,4),
    IN pMetodo         VARCHAR(30),
    IN pReferencia     VARCHAR(30),
    IN pEstado         VARCHAR(30)
)
BEGIN
    DECLARE vSaldo        DECIMAL(12,4);
    DECLARE vNuevoSaldo   DECIMAL(12,4);
    DECLARE vEstadoCuenta VARCHAR(30);
    DECLARE vProveedor    INT;
    DECLARE vNombre       VARCHAR(100);
    DECLARE vIdPago       INT;
    DECLARE vEstado       VARCHAR(30);

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    SET vEstado = IFNULL(pEstado, 'Programado');

    IF vEstado NOT IN ('Pagado', 'Programado') THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Un pago nuevo solo puede quedar Pagado o Programado.';
    END IF;

    IF pMonto IS NULL OR pMonto <= 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El monto debe ser mayor a cero.';
    END IF;

    START TRANSACTION;

    SELECT saldoCuentaPorPagar, idProveedor, estadoCuentaPorPagar
      INTO vSaldo, vProveedor, vEstadoCuenta
      FROM tblCuentaPorPagar
     WHERE idCuentaPorPagar = pCuentaPorPagar
       FOR UPDATE;

    IF vSaldo IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'La cuenta por pagar no existe.';
    END IF;

    IF vEstadoCuenta = 'Anulada' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'La cuenta por pagar esta anulada.';
    END IF;

    IF pMonto > vSaldo THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El monto excede el saldo pendiente de la cuenta.';
    END IF;

    INSERT INTO tblPagoProveedor
        (idCuentaPorPagar, idProveedor, fechaPagoProveedor,
         montoPagoProveedor, metodoPagoProveedor, referenciaPagoProveedor, estadoPagoProveedor)
    VALUES
        (pCuentaPorPagar, vProveedor, pFecha, pMonto, pMetodo, pReferencia, vEstado);

    SET vIdPago = LAST_INSERT_ID();

    SELECT nombreProveedor INTO vNombre
      FROM tblProveedor
     WHERE idProveedor = vProveedor;

    INSERT INTO tblSolicitudPago
        (moduloOrigenSolicitudPago, idOrigenSolicitudPago, beneficiarioSolicitudPago,
         montoSolicitudPago, metodoSolicitudPago)
    VALUES
        ('Compras', vIdPago, vNombre, pMonto, pMetodo);

    SET vNuevoSaldo = vSaldo - pMonto;

    UPDATE tblCuentaPorPagar
       SET saldoCuentaPorPagar  = vNuevoSaldo,
           estadoCuentaPorPagar = IF(vNuevoSaldo = 0, 'Pagada', 'Parcial')
     WHERE idCuentaPorPagar = pCuentaPorPagar;

    COMMIT;

    SELECT vIdPago AS idPago;
END$$

DROP PROCEDURE IF EXISTS spActualizarPagoProveedor$$
CREATE PROCEDURE spActualizarPagoProveedor(
    IN pIdPago     INT,
    IN pFecha      DATE,
    IN pMonto      DECIMAL(12,4),
    IN pMetodo     VARCHAR(30),
    IN pReferencia VARCHAR(30),
    IN pEstado     VARCHAR(30)
)
BEGIN
    DECLARE vCuenta          INT;
    DECLARE vMontoAnt        DECIMAL(12,4);
    DECLARE vEstadoAnt       VARCHAR(30);
    DECLARE vSaldo           DECIMAL(12,4);
    DECLARE vTotal           DECIMAL(12,4);
    DECLARE vEstadoCuenta    VARCHAR(30);
    DECLARE vNuevoSaldo      DECIMAL(12,4);
    DECLARE vEfectivoAnt     DECIMAL(12,4);
    DECLARE vEfectivoNuevo   DECIMAL(12,4);
    DECLARE vEstadoSolicitud VARCHAR(30);

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pMonto IS NULL OR pMonto <= 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El monto debe ser mayor a cero.';
    END IF;

    START TRANSACTION;

    SELECT idCuentaPorPagar, montoPagoProveedor, estadoPagoProveedor
      INTO vCuenta, vMontoAnt, vEstadoAnt
      FROM tblPagoProveedor
     WHERE idPagoProveedor = pIdPago
       FOR UPDATE;

    IF vCuenta IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El pago no existe.';
    END IF;

    SELECT estadoSolicitudPago INTO vEstadoSolicitud
      FROM tblSolicitudPago
     WHERE moduloOrigenSolicitudPago = 'Compras'
       AND idOrigenSolicitudPago = pIdPago
     LIMIT 1;

    IF vEstadoSolicitud = 'Procesada' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Bancos ya proceso este pago, no se puede modificar.';
    END IF;

    SELECT saldoCuentaPorPagar, montoTotalCuentaPorPagar, estadoCuentaPorPagar
      INTO vSaldo, vTotal, vEstadoCuenta
      FROM tblCuentaPorPagar
     WHERE idCuentaPorPagar = vCuenta
       FOR UPDATE;

    SET vEfectivoAnt   = IF(vEstadoAnt IN ('Pagado', 'Programado'), vMontoAnt, 0);
    SET vEfectivoNuevo = IF(pEstado    IN ('Pagado', 'Programado'), pMonto, 0);
    SET vNuevoSaldo    = vSaldo + vEfectivoAnt - vEfectivoNuevo;

    IF vNuevoSaldo < 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El monto excede el saldo pendiente de la cuenta.';
    END IF;

    UPDATE tblPagoProveedor
       SET fechaPagoProveedor      = pFecha,
           montoPagoProveedor      = pMonto,
           metodoPagoProveedor     = pMetodo,
           referenciaPagoProveedor = pReferencia,
           estadoPagoProveedor     = pEstado
     WHERE idPagoProveedor = pIdPago;

    UPDATE tblCuentaPorPagar
       SET saldoCuentaPorPagar  = vNuevoSaldo,
           estadoCuentaPorPagar = CASE
               WHEN vEstadoCuenta = 'Anulada' THEN 'Anulada'
               WHEN vNuevoSaldo <= 0          THEN 'Pagada'
               WHEN vNuevoSaldo >= vTotal     THEN 'Pendiente'
               ELSE 'Parcial'
           END
     WHERE idCuentaPorPagar = vCuenta;

    UPDATE tblSolicitudPago
       SET montoSolicitudPago  = pMonto,
           metodoSolicitudPago = pMetodo,
           estadoSolicitudPago = IF(pEstado IN ('Rechazado', 'Anulado'), 'Rechazada', 'Pendiente')
     WHERE moduloOrigenSolicitudPago = 'Compras'
       AND idOrigenSolicitudPago = pIdPago;

    COMMIT;
END$$

DROP PROCEDURE IF EXISTS spEliminarPagoProveedor$$
CREATE PROCEDURE spEliminarPagoProveedor(
    IN pIdPago INT
)
BEGIN
    DECLARE vCuenta          INT;
    DECLARE vMonto           DECIMAL(12,4);
    DECLARE vEstado          VARCHAR(30);
    DECLARE vSaldo           DECIMAL(12,4);
    DECLARE vTotal           DECIMAL(12,4);
    DECLARE vEstadoCuenta    VARCHAR(30);
    DECLARE vNuevoSaldo      DECIMAL(12,4);
    DECLARE vEstadoSolicitud VARCHAR(30);

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT idCuentaPorPagar, montoPagoProveedor, estadoPagoProveedor
      INTO vCuenta, vMonto, vEstado
      FROM tblPagoProveedor
     WHERE idPagoProveedor = pIdPago
       FOR UPDATE;

    IF vCuenta IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El pago no existe.';
    END IF;

    SELECT estadoSolicitudPago INTO vEstadoSolicitud
      FROM tblSolicitudPago
     WHERE moduloOrigenSolicitudPago = 'Compras'
       AND idOrigenSolicitudPago = pIdPago
     LIMIT 1;

    IF vEstadoSolicitud = 'Procesada' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Bancos ya proceso este pago, no se puede eliminar.';
    END IF;

    SELECT saldoCuentaPorPagar, montoTotalCuentaPorPagar, estadoCuentaPorPagar
      INTO vSaldo, vTotal, vEstadoCuenta
      FROM tblCuentaPorPagar
     WHERE idCuentaPorPagar = vCuenta
       FOR UPDATE;

    IF vEstado IN ('Pagado', 'Programado') THEN
        SET vNuevoSaldo = vSaldo + vMonto;
    ELSE
        SET vNuevoSaldo = vSaldo;
    END IF;

    UPDATE tblCuentaPorPagar
       SET saldoCuentaPorPagar  = vNuevoSaldo,
           estadoCuentaPorPagar = CASE
               WHEN vEstadoCuenta = 'Anulada' THEN 'Anulada'
               WHEN vNuevoSaldo <= 0          THEN 'Pagada'
               WHEN vNuevoSaldo >= vTotal     THEN 'Pendiente'
               ELSE 'Parcial'
           END
     WHERE idCuentaPorPagar = vCuenta;

    DELETE FROM tblSolicitudPago
     WHERE moduloOrigenSolicitudPago = 'Compras'
       AND idOrigenSolicitudPago = pIdPago;

    DELETE FROM tblPagoProveedor
     WHERE idPagoProveedor = pIdPago;

    COMMIT;
END$$

DROP PROCEDURE IF EXISTS spConsultarPagoProveedor$$
CREATE PROCEDURE spConsultarPagoProveedor(
    IN pProveedor INT,
    IN pEstado    VARCHAR(30),
    IN pDesde     DATE,
    IN pHasta     DATE
)
BEGIN
    SELECT p.idPagoProveedor                                       AS idPago,
           CONCAT('PG-', LPAD(p.idPagoProveedor, 4, '0'))          AS noPago,
           pr.nombreProveedor                                       AS proveedor,
           c.numeroFacturaCuentaPorPagar                            AS factura,
           p.fechaPagoProveedor                                     AS fecha,
           p.montoPagoProveedor                                     AS monto,
           p.estadoPagoProveedor                                    AS estado
      FROM tblPagoProveedor p
     INNER JOIN tblProveedor pr     ON pr.idProveedor = p.idProveedor
     INNER JOIN tblCuentaPorPagar c ON c.idCuentaPorPagar = p.idCuentaPorPagar
     WHERE (pProveedor IS NULL OR p.idProveedor = pProveedor)
       AND (pEstado    IS NULL OR p.estadoPagoProveedor = pEstado)
       AND (pDesde     IS NULL OR p.fechaPagoProveedor >= pDesde)
       AND (pHasta     IS NULL OR p.fechaPagoProveedor <= pHasta)
     ORDER BY p.idPagoProveedor DESC;
END$$

DROP PROCEDURE IF EXISTS spObtenerPagoProveedor$$
CREATE PROCEDURE spObtenerPagoProveedor(
    IN pIdPago INT
)
BEGIN
    SELECT p.idPagoProveedor            AS idPago,
           p.idProveedor         AS idProveedor,
           p.idCuentaPorPagar    AS idCuentaPorPagar,
           c.numeroFacturaCuentaPorPagar AS factura,
           p.fechaPagoProveedor          AS fecha,
           p.montoPagoProveedor          AS monto,
           p.metodoPagoProveedor         AS metodo,
           p.referenciaPagoProveedor     AS referencia,
           p.estadoPagoProveedor         AS estado
      FROM tblPagoProveedor p
     INNER JOIN tblCuentaPorPagar c ON c.idCuentaPorPagar = p.idCuentaPorPagar
     WHERE p.idPagoProveedor = pIdPago;
END$$

DROP PROCEDURE IF EXISTS spObtenerProveedor$$
CREATE PROCEDURE spObtenerProveedor()
BEGIN
    SELECT idProveedor    AS idProveedor,
           nombreProveedor AS nombre
      FROM tblProveedor
     WHERE estadoProveedor = 1
     ORDER BY nombreProveedor;
END$$

DROP PROCEDURE IF EXISTS spObtenerCuentaPorPagarPendiente$$
CREATE PROCEDURE spObtenerCuentaPorPagarPendiente(
    IN pProveedor INT
)
BEGIN
    SELECT idCuentaPorPagar AS idCuentaPorPagar,
           CONCAT(numeroFacturaCuentaPorPagar, ' - Q ', FORMAT(saldoCuentaPorPagar, 2)) AS descripcion,
           saldoCuentaPorPagar AS saldo
      FROM tblCuentaPorPagar
     WHERE idProveedor = pProveedor
       AND saldoCuentaPorPagar > 0
       AND estadoCuentaPorPagar IN ('Pendiente', 'Parcial')
     ORDER BY fechaVencimientoCuentaPorPagar;
END$$

DELIMITER ;

