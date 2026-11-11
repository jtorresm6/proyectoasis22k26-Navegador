-- Ejecutar primero Insert_Modulo_Aplicaciones_VENTAS.sql.
-- Permisos de prueba para el rol 1 y el usuario existente imelendez.
USE dbsistemaembutidos;

-- Permisos de imelendez sobre TODAS las aplicaciones del módulo 7000 (Ventas y Cuentas por Cobrar)

-- 1) Por rol: Administrador (rol 1)
INSERT INTO tblrolmoduloaplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion, derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion, derImprimirRolModuloAplicacion)
SELECT 1, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblaplicacion a
WHERE a.idModulo = 7000
ON DUPLICATE KEY UPDATE
    derInsertarRolModuloAplicacion = VALUES(derInsertarRolModuloAplicacion),
    derEditarRolModuloAplicacion   = VALUES(derEditarRolModuloAplicacion),
    derEliminarRolModuloAplicacion = VALUES(derEliminarRolModuloAplicacion),
    derImprimirRolModuloAplicacion = VALUES(derImprimirRolModuloAplicacion);

-- 2) Directo al usuario imelendez
INSERT INTO tblusuariomoduloaplicacion
    (idUsuario, idModulo, idAplicacion,
     derInsertarUsuarioModuloAplicacion, derEditarUsuarioModuloAplicacion,
     derEliminarUsuarioModuloAplicacion, derImprimirUsuarioModuloAplicacion)
SELECT u.idUsuario, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblusuario u
JOIN tblaplicacion a ON a.idModulo = 7000
WHERE u.nombreUsuario = 'imelendez'
ON DUPLICATE KEY UPDATE
    derInsertarUsuarioModuloAplicacion = VALUES(derInsertarUsuarioModuloAplicacion),
    derEditarUsuarioModuloAplicacion   = VALUES(derEditarUsuarioModuloAplicacion),
    derEliminarUsuarioModuloAplicacion = VALUES(derEliminarUsuarioModuloAplicacion),
    derImprimirUsuarioModuloAplicacion = VALUES(derImprimirUsuarioModuloAplicacion);

-- 3) Verificar: aplicaciones de Ventas y permisos que quedaron
SELECT idAplicacion, nombreAplicacion FROM tblaplicacion WHERE idModulo = 7000 ORDER BY idAplicacion;
SELECT * FROM tblrolmoduloaplicacion WHERE idModulo = 7000 ORDER BY idAplicacion;
SELECT * FROM tblusuariomoduloaplicacion WHERE idModulo = 7000 ORDER BY idAplicacion;