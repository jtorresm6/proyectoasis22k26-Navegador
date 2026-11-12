-- Permisos de pruebas para Compras y Cuentas por Pagar.
-- Sigue el esquema de permisos_contabilidad_imelendez.sql.
-- Requiere el modulo, las aplicaciones 6000-6014 y el usuario imelendez.
-- Los permisos por rol afectan a todos los usuarios que comparten esos roles.
USE dbsistemaembutidos;

INSERT INTO tblmodulo
(nombreModulo, descripcionModulo, is_active)
VALUES
('Compras y Cuentas por Pagar', 'Gestión de compras y cuentas por pagar', 1);

INSERT INTO tblaplicacion
(idAplicacion, idModulo, nombreAplicacion, descripcionAplicacion, is_active)
VALUES
(6000, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'MDI Compras', 'Pantalla principal de Compras', 1),
(6001, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Movimientos de proveedores', 'Registro de documentos de proveedores', 1),
(6002, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Pólizas', 'Pólizas de compras', 1),
(6003, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Órdenes de compra', 'Registro y consulta de órdenes de compra', 1),
(6004, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Pagos a proveedores', 'Registro de pagos a proveedores', 1),
(6005, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Aprobación financiera', 'Aprobación financiera de órdenes', 1),
(6006, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Cierre de movimientos', 'Cierre de períodos de Compras', 1),
(6007, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Aprobar orden', 'Aprobación de órdenes', 1),
(6008, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Rechazar orden', 'Rechazo de órdenes', 1),
(6009, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Evaluar orden', 'Evaluación de órdenes', 1),
(6010, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Proveedor', 'Mantenimiento de proveedores', 1),
(6011, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Método de pago', 'Mantenimiento de métodos de pago', 1),
(6012, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Encabezados compras por pagar', 'Consulta de encabezados de compras por pagar', 1),
(6013, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Plazo', 'Mantenimiento de plazos', 1),
(6014, (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Compras y Cuentas por Pagar'), 'Concepto de proveedor', 'Mantenimiento de conceptos de proveedor', 1);

SET @compras_sql_safe_updates = @@SESSION.SQL_SAFE_UPDATES;
SET SESSION SQL_SAFE_UPDATES = 0;

START TRANSACTION;

INSERT INTO tblusuariorol (idUsuario, idRol, fechaAsignacionUsuarioRol)
SELECT u.idUsuario, r.idRol, CURDATE()
FROM tblusuario u
CROSS JOIN tblrol r
WHERE u.nombreUsuario = 'imelendez'
  AND r.nombreRol = 'Administrador'
  AND NOT EXISTS (
      SELECT 1
      FROM tblusuariorol ur
      WHERE ur.idUsuario = u.idUsuario
  );

INSERT INTO tblrolmoduloaplicacion (
    idRol,
    idModulo,
    idAplicacion,
    derInsertarRolModuloAplicacion,
    derEditarRolModuloAplicacion,
    derEliminarRolModuloAplicacion,
    derImprimirRolModuloAplicacion
)
SELECT ur.idRol, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblusuario u
JOIN tblusuariorol ur ON ur.idUsuario = u.idUsuario
JOIN tblmodulo m ON m.nombreModulo = 'Compras y Cuentas por Pagar'
JOIN tblaplicacion a ON (
    (a.idModulo = m.idModulo AND a.idAplicacion BETWEEN 6000 AND 6014)
    OR (a.idModulo = 4 AND a.idAplicacion = 12)
)
WHERE u.nombreUsuario = 'imelendez'
  AND a.is_active = 1
ON DUPLICATE KEY UPDATE
    derInsertarRolModuloAplicacion = 1,
    derEditarRolModuloAplicacion = 1,
    derEliminarRolModuloAplicacion = 1,
    derImprimirRolModuloAplicacion = 1;

INSERT INTO tblusuariomoduloaplicacion (
    idUsuario,
    idModulo,
    idAplicacion,
    derInsertarUsuarioModuloAplicacion,
    derEditarUsuarioModuloAplicacion,
    derEliminarUsuarioModuloAplicacion,
    derImprimirUsuarioModuloAplicacion
)
SELECT u.idUsuario, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblusuario u
JOIN tblmodulo m ON m.nombreModulo = 'Compras y Cuentas por Pagar'
JOIN tblaplicacion a ON (
    (a.idModulo = m.idModulo AND a.idAplicacion BETWEEN 6000 AND 6014)
    OR (a.idModulo = 4 AND a.idAplicacion = 12)
)
WHERE u.nombreUsuario = 'imelendez'
  AND a.is_active = 1
ON DUPLICATE KEY UPDATE
    derInsertarUsuarioModuloAplicacion = 1,
    derEditarUsuarioModuloAplicacion = 1,
    derEliminarUsuarioModuloAplicacion = 1,
    derImprimirUsuarioModuloAplicacion = 1;

COMMIT;