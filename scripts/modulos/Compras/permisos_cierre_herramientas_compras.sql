-- ==================================================================
-- Área   : Compras
-- Autor  : José Pablo Cano Cóbar
-- Carné  : 0901-23-1727
-- Fecha  : 11/11/2026
-- ==================================================================
-- Propósito :
--  Registra la aplicación 6015 "Herramientas de moneda" y concede las
--  aplicaciones 6006 (Cierre de movimientos) y 6015 a los roles que ya
--  tienen acceso al MDI de Compras (6000).
-- Requisitos :
--  Ejecutar después del script general y de permisos_imelendez_compras.sql.
--  El script se puede ejecutar varias veces sin duplicar registros.
-- ===================================================================
USE dbsistemaembutidos;

INSERT INTO tblaplicacion
(idAplicacion, idModulo, nombreAplicacion, descripcionAplicacion, is_active)
SELECT 6015, m.idModulo, 'Herramientas de moneda',
       'Tipo de cambio, conversor y moneda de visualización de Compras', 1
FROM tblmodulo m
WHERE m.nombreModulo = 'Compras y Cuentas por Pagar'
ON DUPLICATE KEY UPDATE
    nombreAplicacion = VALUES(nombreAplicacion),
    descripcionAplicacion = VALUES(descripcionAplicacion),
    is_active = 1;

START TRANSACTION;

INSERT INTO tblrolmoduloaplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion, derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion, derImprimirRolModuloAplicacion)
SELECT mdi.idRol, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblrolmoduloaplicacion mdi
JOIN tblaplicacion a ON a.idModulo = mdi.idModulo AND a.idAplicacion IN (6006, 6015)
WHERE mdi.idAplicacion = 6000
  AND a.is_active = 1
ON DUPLICATE KEY UPDATE
    derInsertarRolModuloAplicacion = 1,
    derEditarRolModuloAplicacion = 1,
    derEliminarRolModuloAplicacion = 1,
    derImprimirRolModuloAplicacion = 1;

INSERT INTO tblusuariomoduloaplicacion
    (idUsuario, idModulo, idAplicacion,
     derInsertarUsuarioModuloAplicacion, derEditarUsuarioModuloAplicacion,
     derEliminarUsuarioModuloAplicacion, derImprimirUsuarioModuloAplicacion)
SELECT mdi.idUsuario, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblusuariomoduloaplicacion mdi
JOIN tblaplicacion a ON a.idModulo = mdi.idModulo AND a.idAplicacion IN (6006, 6015)
WHERE mdi.idAplicacion = 6000
  AND a.is_active = 1
ON DUPLICATE KEY UPDATE
    derInsertarUsuarioModuloAplicacion = 1,
    derEditarUsuarioModuloAplicacion = 1,
    derEliminarUsuarioModuloAplicacion = 1,
    derImprimirUsuarioModuloAplicacion = 1;

COMMIT;

SELECT a.idAplicacion, a.nombreAplicacion,
       COUNT(DISTINCT r.idRol) AS roles_con_acceso,
       COUNT(DISTINCT u.idUsuario) AS usuarios_con_acceso
FROM tblaplicacion a
LEFT JOIN tblrolmoduloaplicacion r ON r.idModulo = a.idModulo AND r.idAplicacion = a.idAplicacion
LEFT JOIN tblusuariomoduloaplicacion u ON u.idModulo = a.idModulo AND u.idAplicacion = a.idAplicacion
WHERE a.idAplicacion IN (6006, 6015)
GROUP BY a.idAplicacion, a.nombreAplicacion;
