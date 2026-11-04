USE `dbsistemaembutidos`;
DELIMITER $$
DROP PROCEDURE IF EXISTS `spObtenerProveedor`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spObtenerProveedor`()
BEGIN
    SELECT Pk_Proveedor    AS idProveedor,
           nombreProveedor AS nombre
      FROM tblProveedor
     WHERE estadoProveedor = 1
     ORDER BY nombreProveedor;
END$$
DROP PROCEDURE IF EXISTS `spObtenerCuentaPorPagarPendiente`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spObtenerCuentaPorPagarPendiente`(
    IN pProveedor INT
)
BEGIN
    SELECT Pk_CuentaPorPagar AS idCuentaPorPagar,
           CONCAT(numeroFacturaCuentaPorPagar, ' - Q ', FORMAT(saldoCuentaPorPagar, 2)) AS descripcion,
           saldoCuentaPorPagar AS saldo
      FROM tblCuentaPorPagar
     WHERE Fk_Proveedor_Codigo = pProveedor
       AND saldoCuentaPorPagar > 0
       AND estadoCuentaPorPagar IN ('Pendiente', 'Parcial')
     ORDER BY fechaVencimientoCuentaPorPagar;
END$$
DROP PROCEDURE IF EXISTS `spConsultarPagoProveedor`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spConsultarPagoProveedor`(
    IN pProveedor INT,
    IN pEstado    VARCHAR(30),
    IN pDesde     DATE,
    IN pHasta     DATE
)
BEGIN
    SELECT p.Pk_PagoProveedor                                       AS idPago,
           CONCAT('PG-', LPAD(p.Pk_PagoProveedor, 4, '0'))          AS noPago,
           pr.nombreProveedor                                       AS proveedor,
           c.numeroFacturaCuentaPorPagar                            AS factura,
           p.fechaPagoProveedor                                     AS fecha,
           p.montoPagoProveedor                                     AS monto,
           p.estadoPagoProveedor                                    AS estado
      FROM tblPagoProveedor p
     INNER JOIN tblProveedor pr     ON pr.Pk_Proveedor = p.Fk_Proveedor_Codigo
     INNER JOIN tblCuentaPorPagar c ON c.Pk_CuentaPorPagar = p.Fk_CuentaPorPagar_Codigo
     WHERE (pProveedor IS NULL OR p.Fk_Proveedor_Codigo = pProveedor)
       AND (pEstado    IS NULL OR p.estadoPagoProveedor = pEstado)
       AND (pDesde     IS NULL OR p.fechaPagoProveedor >= pDesde)
       AND (pHasta     IS NULL OR p.fechaPagoProveedor <= pHasta)
     ORDER BY p.Pk_PagoProveedor DESC;
END$$
DROP PROCEDURE IF EXISTS `spObtenerPagoProveedor`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spObtenerPagoProveedor`(
    IN pIdPago INT
)
BEGIN
    SELECT p.Pk_PagoProveedor            AS idPago,
           p.Fk_Proveedor_Codigo         AS idProveedor,
           p.Fk_CuentaPorPagar_Codigo    AS idCuentaPorPagar,
           c.numeroFacturaCuentaPorPagar AS factura,
           p.fechaPagoProveedor          AS fecha,
           p.montoPagoProveedor          AS monto,
           p.metodoPagoProveedor         AS metodo,
           p.referenciaPagoProveedor     AS referencia,
           p.estadoPagoProveedor         AS estado
      FROM tblPagoProveedor p
     INNER JOIN tblCuentaPorPagar c ON c.Pk_CuentaPorPagar = p.Fk_CuentaPorPagar_Codigo
     WHERE p.Pk_PagoProveedor = pIdPago;
END$$
DROP PROCEDURE IF EXISTS `spInsertarPagoProveedor`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spInsertarPagoProveedor`(
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

    SELECT saldoCuentaPorPagar, Fk_Proveedor_Codigo, estadoCuentaPorPagar
      INTO vSaldo, vProveedor, vEstadoCuenta
      FROM tblCuentaPorPagar
     WHERE Pk_CuentaPorPagar = pCuentaPorPagar
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
        (Fk_CuentaPorPagar_Codigo, Fk_Proveedor_Codigo, fechaPagoProveedor,
         montoPagoProveedor, metodoPagoProveedor, referenciaPagoProveedor, estadoPagoProveedor)
    VALUES
        (pCuentaPorPagar, vProveedor, pFecha, pMonto, pMetodo, pReferencia, vEstado);

    SET vIdPago = LAST_INSERT_ID();

    SELECT nombreProveedor INTO vNombre
      FROM tblProveedor
     WHERE Pk_Proveedor = vProveedor;

    INSERT INTO tblSolicitudPago
        (moduloOrigenSolicitudPago, idOrigenSolicitudPago, beneficiarioSolicitudPago,
         montoSolicitudPago, metodoSolicitudPago)
    VALUES
        ('Compras', vIdPago, vNombre, pMonto, pMetodo);

    SET vNuevoSaldo = vSaldo - pMonto;

    UPDATE tblCuentaPorPagar
       SET saldoCuentaPorPagar  = vNuevoSaldo,
           estadoCuentaPorPagar = IF(vNuevoSaldo = 0, 'Pagada', 'Parcial')
     WHERE Pk_CuentaPorPagar = pCuentaPorPagar;

    COMMIT;

    SELECT vIdPago AS idPago;
END$$
DROP PROCEDURE IF EXISTS `spActualizarPagoProveedor`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spActualizarPagoProveedor`(
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

    SELECT Fk_CuentaPorPagar_Codigo, montoPagoProveedor, estadoPagoProveedor
      INTO vCuenta, vMontoAnt, vEstadoAnt
      FROM tblPagoProveedor
     WHERE Pk_PagoProveedor = pIdPago
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
     WHERE Pk_CuentaPorPagar = vCuenta
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
     WHERE Pk_PagoProveedor = pIdPago;

    UPDATE tblCuentaPorPagar
       SET saldoCuentaPorPagar  = vNuevoSaldo,
           estadoCuentaPorPagar = CASE
               WHEN vEstadoCuenta = 'Anulada' THEN 'Anulada'
               WHEN vNuevoSaldo <= 0          THEN 'Pagada'
               WHEN vNuevoSaldo >= vTotal     THEN 'Pendiente'
               ELSE 'Parcial'
           END
     WHERE Pk_CuentaPorPagar = vCuenta;

    UPDATE tblSolicitudPago
       SET montoSolicitudPago  = pMonto,
           metodoSolicitudPago = pMetodo,
           estadoSolicitudPago = IF(pEstado IN ('Rechazado', 'Anulado'), 'Rechazada', 'Pendiente')
     WHERE moduloOrigenSolicitudPago = 'Compras'
       AND idOrigenSolicitudPago = pIdPago;

    COMMIT;
END$$
DROP PROCEDURE IF EXISTS `spEliminarPagoProveedor`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spEliminarPagoProveedor`(
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

    SELECT Fk_CuentaPorPagar_Codigo, montoPagoProveedor, estadoPagoProveedor
      INTO vCuenta, vMonto, vEstado
      FROM tblPagoProveedor
     WHERE Pk_PagoProveedor = pIdPago
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
     WHERE Pk_CuentaPorPagar = vCuenta
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
     WHERE Pk_CuentaPorPagar = vCuenta;

    DELETE FROM tblSolicitudPago
     WHERE moduloOrigenSolicitudPago = 'Compras'
       AND idOrigenSolicitudPago = pIdPago;

    DELETE FROM tblPagoProveedor
     WHERE Pk_PagoProveedor = pIdPago;

    COMMIT;
END$$
DELIMITER ;