CREATE DATABASE IF NOT EXISTS `dbsistemaembutidos` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `dbsistemaembutidos`;

-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡ Componentes !!!!!!!!!!!!!!!!!!!!!!!!!-------------

-- ------------------------------------------- Seguridad ------------------------------------------------
CREATE TABLE `tblempleado` (
  `idEmpleado` int NOT NULL AUTO_INCREMENT,
  `codigoEmpleado` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `dpiEmpleado` char(13) COLLATE utf8mb4_unicode_ci NOT NULL,
  `nitEmpleado` char(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `nombresEmpleado` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `apellidosEmpleado` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `puestoEmpleado` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `generoEmpleado` char(1) COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'M = Masculino, F = Femenino',
  `fechaNacimientoEmpleado` date NOT NULL,
  `fechaContratacionEmpleado` date NOT NULL,
  `telefonoEmpleado` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `correoEmpleado` varchar(150) COLLATE utf8mb4_unicode_ci NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idEmpleado`),
  UNIQUE KEY `uqCodigoEmpleado` (`codigoEmpleado`),
  UNIQUE KEY `uqDpiEmpleado` (`dpiEmpleado`),
  UNIQUE KEY `uqNitEmpleado` (`nitEmpleado`),
  UNIQUE KEY `uqCorreoEmpleado` (`correoEmpleado`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblrol` (
  `idRol` int NOT NULL AUTO_INCREMENT,
  `nombreRol` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcionRol` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idRol`),
  UNIQUE KEY `uqNombreRol` (`nombreRol`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblmodulo` (
  `idModulo` int NOT NULL AUTO_INCREMENT,
  `nombreModulo` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcionModulo` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idModulo`),
  UNIQUE KEY `uqNombreModulo` (`nombreModulo`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblaplicacion` (
  `idAplicacion` int NOT NULL AUTO_INCREMENT,
  `idModulo` int NOT NULL,
  `nombreAplicacion` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcionAplicacion` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idAplicacion`),
  KEY `idxAplicacionIdModulo` (`idModulo`),
  CONSTRAINT `Fk_Aplicacion_Modulo` FOREIGN KEY (`idModulo`) REFERENCES `tblmodulo` (`idModulo`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblusuario` (
  `idUsuario` int NOT NULL AUTO_INCREMENT,
  `idEmpleado` int NOT NULL,
  `nombreUsuario` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `contrasenaUsuario` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ultimoAccesoUsuario` datetime NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idUsuario`),
  UNIQUE KEY `uqIdEmpleadoUsuario` (`idEmpleado`),
  UNIQUE KEY `uqUsuarioUsuario` (`nombreUsuario`),
  KEY `idxUsuarioIdEmpleado` (`idEmpleado`),
  CONSTRAINT `Fk_Usuario_Empleado` FOREIGN KEY (`idEmpleado`) REFERENCES `tblempleado` (`idEmpleado`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblrecuperacioncontrasena` (
  `idRecuperacionContrasena` int NOT NULL AUTO_INCREMENT,
  `idUsuario` int NOT NULL,
  `tokenRecuperacionContrasena` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `fechaExpiracionRecuperacionContrasena` datetime NOT NULL,
  `usadoRecuperacionContrasena` tinyint(1) NOT NULL DEFAULT '0',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idRecuperacionContrasena`),
  UNIQUE KEY `uqTokenRecuperacionContrasena` (`tokenRecuperacionContrasena`),
  KEY `idxRecuperacionContrasenaIdUsuario` (`idUsuario`),
  CONSTRAINT `Fk_RecuperacionContrasena_Usuario` FOREIGN KEY (`idUsuario`) REFERENCES `tblusuario` (`idUsuario`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblusuariorol` (
  `idUsuario` int NOT NULL,
  `idRol` int NOT NULL,
  `fechaAsignacionUsuarioRol` date NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idUsuario`,`idRol`),
  KEY `idxUsuarioRolIdRol` (`idRol`),
  CONSTRAINT `Fk_UsuarioRol_Rol` FOREIGN KEY (`idRol`) REFERENCES `tblrol` (`idRol`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Fk_UsuarioRol_Usuario` FOREIGN KEY (`idUsuario`) REFERENCES `tblusuario` (`idUsuario`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblrolmoduloaplicacion` (
  `idRol` int NOT NULL,
  `idModulo` int NOT NULL,
  `idAplicacion` int NOT NULL,
  `derInsertarRolModuloAplicacion` tinyint(1) NOT NULL DEFAULT '0',
  `derEditarRolModuloAplicacion` tinyint(1) NOT NULL DEFAULT '0',
  `derEliminarRolModuloAplicacion` tinyint(1) NOT NULL DEFAULT '0',
  `derImprimirRolModuloAplicacion` tinyint(1) NOT NULL DEFAULT '0',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idRol`,`idModulo`,`idAplicacion`),
  KEY `idxRolModAplIdModulo` (`idModulo`),
  KEY `idxRolModAplIdAplicacion` (`idAplicacion`),
  CONSTRAINT `Fk_RolModuloAplicacion_Aplicacion` FOREIGN KEY (`idAplicacion`) REFERENCES `tblaplicacion` (`idAplicacion`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Fk_RolModuloAplicacion_Modulo` FOREIGN KEY (`idModulo`) REFERENCES `tblmodulo` (`idModulo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Fk_RolModuloAplicacion_Rol` FOREIGN KEY (`idRol`) REFERENCES `tblrol` (`idRol`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblusuariomoduloaplicacion` (
  `idUsuario` int NOT NULL,
  `idModulo` int NOT NULL,
  `idAplicacion` int NOT NULL,
  `derInsertarUsuarioModuloAplicacion` tinyint(1) NOT NULL DEFAULT '0',
  `derEditarUsuarioModuloAplicacion` tinyint(1) NOT NULL DEFAULT '0',
  `derEliminarUsuarioModuloAplicacion` tinyint(1) NOT NULL DEFAULT '0',
  `derImprimirUsuarioModuloAplicacion` tinyint(1) NOT NULL DEFAULT '0',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idUsuario`,`idModulo`,`idAplicacion`),
  KEY `idxUsrModAplIdModulo` (`idModulo`),
  KEY `idxUsrModAplIdAplicacion` (`idAplicacion`),
  CONSTRAINT `Fk_UsuarioModuloAplicacion_Aplicacion` FOREIGN KEY (`idAplicacion`) REFERENCES `tblaplicacion` (`idAplicacion`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Fk_UsuarioModuloAplicacion_Modulo` FOREIGN KEY (`idModulo`) REFERENCES `tblmodulo` (`idModulo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Fk_UsuarioModuloAplicacion_Usuario` FOREIGN KEY (`idUsuario`) REFERENCES `tblusuario` (`idUsuario`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblbitacora` (
  `idBitacora` int NOT NULL AUTO_INCREMENT,
  `idUsuario` int DEFAULT NULL,
  `accionBitacora` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `tablaBitacora` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `idRegistroBitacora` int NOT NULL,
  `detallesBitacora` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `ipBitacora` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `fechaHoraBitacora` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`idBitacora`),
  KEY `idxBitacoraIdUsuario` (`idUsuario`),
  KEY `idxBitacoraFechaHora` (`fechaHoraBitacora`),
  KEY `idxBitacoraTabla` (`tablaBitacora`),
  CONSTRAINT `Fk_Bitacora_Usuario` FOREIGN KEY (`idUsuario`) REFERENCES `tblusuario` (`idUsuario`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=249 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT INTO `tblempleado` VALUES (1,'EMP-001','1234567890123','1234567-K','Isabel','Meléndez','Coordinadora de Implementación','F','2000-05-14','2026-01-10','55512345','isabel@gmail.com',1,'2026-09-17 14:07:07','2026-09-23 23:40:07'),(2,'EMP-002','2345678901234','2345678K','Carlos','Ramírez','Analista de Sistemas','M','1998-03-22','2025-11-05','55523456','carlos.ramirez@terminus.com',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(3,'EMP-003','3456789012345','3456789K','María','López','Administradora de Base de Datos','F','1995-09-30','2025-08-19','55534567','maria.lopez@terminus.com',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(13,'EMP-006','8463778648763','3727836-7','Oscar Emilio','Morales Lemus','Gerente','M','2005-05-28','2026-09-21','32323232','oscaremilio@gmail.com',1,'2026-09-22 04:30:17','2026-09-22 04:30:17'),(14,'EMP-007','5463212315453','5454542-1','Empleado','Examen','Pruebas','F','1990-09-22','2026-09-22','87897654','prueba@gmail.com',1,'2026-09-22 13:11:30','2026-09-22 13:11:30'),(19,'EMP-010','9876543219876','9876543-2','Pruebas','Capacitacion','Probador','M','1990-01-01','2026-01-10','12345678','terminus.seguridad22026@gmail.com',1,'2026-09-23 23:42:57','2026-09-23 23:42:57');

INSERT INTO `tblrol` VALUES (1,'Administrador','Rol con acceso total al sistema',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(2,'Supervisor','Rol con acceso a reportes y aprobaciones',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(11,'Pruebas','pruebas del sistema',0,'2026-09-18 21:45:38','2026-09-18 21:45:38'),(12,'PerfilExamen','Permisos personalizados para el examen',1,'2026-09-22 13:16:29','2026-09-22 13:16:29'),(14,'Prueba','Esta es una prueba controlada por parte de Seguridad',1,'2026-09-23 23:47:34','2026-09-23 23:47:34');

INSERT INTO `tblmodulo` VALUES (1,'Producción','Módulo de control de producción de embutidos',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(2,'Inventario','Módulo de control de materia prima y producto terminado',1,'2026-09-17 14:07:07','2026-09-20 05:29:01'),(3,'Ventas','Módulo de gestión de pedidos y clientes',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(4,'Seguridad','Módulo de administración y seguridad del sistema',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(13,'Pruebas','Esto es una prueba controlada por parte de Seguridad.',1,'2026-09-23 23:44:26','2026-09-23 23:44:26');

INSERT INTO `tblaplicacion` VALUES (3,3,'Gestión de Pedidos','Registro y seguimiento de pedidos de clientes',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(4,4,'Empleados','Gestión de empleados del sistema',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(5,4,'Usuarios','Gestión de usuarios del sistema',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(6,4,'Módulos','Administración de módulos del sistema',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(7,4,'Aplicaciones','Administración de aplicaciones del sistema',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(8,4,'Perfiles','Administración de roles/perfiles',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(9,4,'Asignación de Perfiles','Asignación de roles a usuarios',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(10,4,'Asignación Aplicación Perfiles','Asignación de permisos por rol/módulo/aplicación',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(11,4,'Asignación Aplicación Usuario','Asignación de permisos por usuario/módulo/aplicación',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(12,4,'Bitácora','Consulta de bitácora del sistema',1,'2026-09-17 14:07:07','2026-09-20 02:48:38'),(15,4,'Prueba','Esto es una prueba controlada por parte de Seguridad.',1,'2026-09-23 23:46:12','2026-09-23 23:46:12');

INSERT INTO `tblusuario` VALUES (1,1,'imelendez','$2b$10$e9CQKArQRmpf.IUtqpxYmOoO9vyh2./7ped56qEokwfgn7DkcacPC','2026-09-17 08:07:07',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(2,2,'cramirez','$2b$10$ojN1MyPkUaylOuupBEGmhemdx5tQi4BdbbJrpSF2ZJjD0ZWM/lbe2','2026-09-17 08:07:07',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(3,3,'mlopez','$2b$10$Bo9RDmJRGKKLxm1tSmRO1ODP0CiGmuqy/wcnJ6bs2MkOVe1OQml5i','2026-09-17 08:07:07',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(12,14,'UsuarioExamen','$2a$11$m6Fi2WikM6YW5QbVZikQBuxMCXRfLjnxPleDRY75FLmVjYncKxEc6','2026-09-22 07:12:30',1,'2026-09-22 13:12:30','2026-09-22 13:12:30'),(13,19,'Pcapacitacion','$2a$11$Vee.0P876yc/Mnwy2ePINeGVsQoOLofGZT30VkeJz/B1y3ND7tSAq','2026-09-23 17:43:51',1,'2026-09-23 23:43:51','2026-09-23 23:43:51');

INSERT INTO `tblrecuperacioncontrasena` VALUES (1,1,'TOKEN-A1B2C3D4','2026-09-18 08:07:07',0,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(2,2,'TOKEN-E5F6G7H8','2026-09-18 08:07:07',0,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(3,3,'TOKEN-I9J0K1L2','2026-09-18 08:07:07',1,'2026-09-17 14:07:07','2026-09-17 14:07:07');

INSERT INTO `tblusuariorol` VALUES (1,1,'2026-09-17','2026-09-17 14:07:07','2026-09-17 14:07:07'),(2,2,'2026-09-17','2026-09-17 14:07:07','2026-09-17 14:07:07'),(3,2,'2026-09-17','2026-09-17 14:09:00','2026-09-17 14:09:00'),(12,12,'2026-09-22','2026-09-22 13:22:32','2026-09-22 13:22:32'),(13,14,'2026-09-23','2026-09-23 23:47:47','2026-09-23 23:47:47');

INSERT INTO `tblrolmoduloaplicacion` VALUES (1,4,4,1,1,1,1,'2026-09-17 14:07:07','2026-09-17 14:19:43'),(1,4,5,1,1,1,1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(1,4,6,1,1,1,1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(1,4,7,1,1,1,1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(1,4,8,1,1,1,1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(1,4,9,1,1,1,1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(1,4,10,1,1,1,1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(1,4,11,1,1,1,1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(1,4,12,1,1,1,1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(2,4,4,1,1,0,0,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(12,4,8,1,1,0,0,'2026-09-22 13:20:38','2026-09-22 13:20:38'),(14,4,15,1,1,1,1,'2026-09-23 23:48:40','2026-09-23 23:48:40');

INSERT INTO `tblusuariomoduloaplicacion` VALUES (3,3,3,1,0,0,0,'2026-09-17 14:07:07','2026-09-17 14:07:07'),(13,13,15,0,0,0,0,'2026-09-23 23:49:15','2026-09-23 23:49:15');

INSERT INTO `tblbitacora` VALUES (1,1,'INSERT','tblEmpleado',1,'Alta de empleado de prueba','192.168.1.10','2026-09-17 08:07:07','2026-09-17 14:07:07'),(2,2,'UPDATE','tblAplicacion',2,'Actualización de datos de aplicación','192.168.1.11','2026-09-17 08:07:07','2026-09-17 14:07:07'),(3,3,'DELETE','tblUsuarioRol',3,'Eliminación de asignación de rol','192.168.1.12','2026-09-17 08:07:07','2026-09-17 14:07:07'),(4,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-17 08:07:53','2026-09-17 14:07:53'),(5,1,'INSERT','tblUsuarioRol',3,'Se asignó el perfil 2 al usuario 3','192.168.167.48','2026-09-17 08:09:00','2026-09-17 14:09:00'),(6,1,'INSERT','tblEmpleado',0,'Se agregó el empleado: Oscar Moralesl','192.168.167.48','2026-09-17 08:13:37','2026-09-17 14:13:37'),(7,1,'INSERT','tblUsuario',0,'Se agregó el usuario: SofiGoty','192.168.167.48','2026-09-17 08:14:24','2026-09-17 14:14:24'),(8,1,'INSERT','tblUsuarioRol',4,'Se asignó el perfil 1 al usuario 4','192.168.167.48','2026-09-17 08:14:33','2026-09-17 14:14:33'),(9,1,'LOGIN','tblUsuario',4,'Inicio de sesión exitoso del usuario: SofiGoty','192.168.167.48','2026-09-17 08:14:55','2026-09-17 14:14:55'),(10,1,'UPDATE','tblAsigAppPerf',1,'Se actualizaron permisos de aplicación 4 al rol 1','192.168.167.48','2026-09-17 08:16:33','2026-09-17 14:16:33'),(11,1,'LOGIN','tblUsuario',4,'Inicio de sesión exitoso del usuario: SofiGoty','192.168.167.48','2026-09-17 08:17:20','2026-09-17 14:17:20'),(12,1,'UPDATE','tblAsigAppPerf',1,'Se actualizaron permisos de aplicación 4 al rol 1','192.168.167.48','2026-09-17 08:19:43','2026-09-17 14:19:43'),(13,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-17 08:22:18','2026-09-17 14:22:18'),(14,1,'DELETE','tblEmpleado',4,'Se eliminó el empleado ID: 4','192.168.167.48','2026-09-17 08:23:26','2026-09-17 14:23:26'),(15,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-17 08:27:14','2026-09-17 14:27:14'),(16,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-17 13:53:54','2026-09-17 19:53:54'),(17,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-17 14:04:42','2026-09-17 20:04:42'),(18,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-17 19:21:21','2026-09-18 01:21:21'),(19,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-17 19:51:48','2026-09-18 01:51:48'),(20,1,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.11','2026-09-17 19:53:15','2026-09-18 01:53:16'),(21,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-17 19:53:42','2026-09-18 01:53:42'),(22,1,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.11','2026-09-18 08:24:51','2026-09-18 14:24:51'),(23,NULL,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.11','2026-09-18 08:44:12','2026-09-18 14:44:12'),(24,NULL,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 08:44:48','2026-09-18 14:44:48'),(25,NULL,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.11','2026-09-18 08:58:45','2026-09-18 14:58:45'),(26,NULL,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 08:58:59','2026-09-18 14:58:59'),(27,NULL,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 09:04:20','2026-09-18 15:04:20'),(28,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 09:07:48','2026-09-18 15:07:48'),(29,2,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.11','2026-09-18 09:08:15','2026-09-18 15:08:15'),(30,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 09:08:38','2026-09-18 15:08:38'),(31,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 09:17:02','2026-09-18 15:17:02'),(32,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 11:41:10','2026-09-18 17:41:10'),(33,1,'INSERT','tblEmpleado',0,'Se agregó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-18 11:48:58','2026-09-18 17:48:58'),(34,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-18 11:49:13','2026-09-18 17:49:13'),(35,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-18 11:49:54','2026-09-18 17:49:55'),(36,1,'INSERT','tblEmpleado',0,'Se agregó el empleado: dfdsf dsfdsf','192.168.1.11','2026-09-18 11:58:46','2026-09-18 17:58:46'),(37,1,'UPDATE','tblEmpleado',8,'Se actualizó el empleado: dfdsf dsfdsf','192.168.1.11','2026-09-18 11:59:21','2026-09-18 17:59:21'),(38,1,'DELETE','tblEmpleado',8,'Se eliminó el empleado ID: 8','192.168.1.11','2026-09-18 11:59:25','2026-09-18 17:59:25'),(39,1,'INSERT','tblUsuario',0,'Se agregó el usuario: Omorales','192.168.1.11','2026-09-18 12:02:42','2026-09-18 18:02:42'),(40,1,'INSERT','tblModulo',0,'Se agregó el módulo: Compras','192.168.1.11','2026-09-18 12:10:26','2026-09-18 18:10:26'),(41,1,'UPDATE','tblModulo',5,'Se actualizó el módulo: Compras','192.168.1.11','2026-09-18 12:10:33','2026-09-18 18:10:33'),(42,1,'DELETE','tblModulo',5,'Se eliminó el módulo ID: 5','192.168.1.11','2026-09-18 12:10:38','2026-09-18 18:10:38'),(43,1,'INSERT','tblModulo',5,'Se agregó el módulo: Comprasdfd','192.168.1.11','2026-09-18 12:10:52','2026-09-18 18:10:53'),(44,1,'UPDATE','tblModulo',6,'Se actualizó el módulo: Comprasdfd','192.168.1.11','2026-09-18 12:10:56','2026-09-18 18:10:56'),(45,1,'UPDATE','tblModulo',4,'Se actualizó el módulo: Seguridad','192.168.1.11','2026-09-18 12:11:19','2026-09-18 18:11:19'),(46,1,'DELETE','tblModulo',6,'Se eliminó el módulo ID: 6','192.168.1.11','2026-09-18 12:13:28','2026-09-18 18:13:28'),(47,1,'INSERT','tblAplicacion',0,'Se agregó la aplicación: fdsfsdfdsf','192.168.1.11','2026-09-18 12:24:05','2026-09-18 18:24:05'),(48,1,'UPDATE','tblAplicacion',13,'Se actualizó la aplicación: fdsfsdfds','192.168.1.11','2026-09-18 12:24:20','2026-09-18 18:24:20'),(49,1,'DELETE','tblAplicacion',13,'Se eliminó la aplicación ID: 13','192.168.1.11','2026-09-18 12:24:25','2026-09-18 18:24:25'),(50,1,'INSERT','tblAplicacion',13,'Se agregó la aplicación: Perfiles','192.168.1.11','2026-09-18 12:24:41','2026-09-18 18:24:41'),(51,1,'INSERT','tblModulo',0,'Se agregó el módulo: dsolpkojdfkdsf','192.168.1.11','2026-09-18 12:26:47','2026-09-18 18:26:47'),(52,1,'DELETE','tblModulo',8,'Se eliminó el módulo ID: 8','192.168.1.11','2026-09-18 12:26:51','2026-09-18 18:26:51'),(53,1,'DELETE','tblAplicacion',14,'Se eliminó la aplicación ID: 14','192.168.1.11','2026-09-18 12:27:35','2026-09-18 18:27:35'),(54,1,'INSERT','tblRol',0,'Se agregó el perfil: Probador','192.168.1.11','2026-09-18 12:30:24','2026-09-18 18:30:24'),(55,1,'UPDATE','tblRol',5,'Se actualizó el perfil: Probador','192.168.1.11','2026-09-18 12:33:51','2026-09-18 18:33:51'),(56,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 12:42:44','2026-09-18 18:42:44'),(57,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 12:43:19','2026-09-18 18:43:19'),(58,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 12:43:56','2026-09-18 18:43:56'),(59,1,'DELETE','tblAsigAppPerf',3,'Se eliminó asignación de aplicación 3 al rol 3','192.168.1.11','2026-09-18 12:44:12','2026-09-18 18:44:12'),(60,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 12:45:34','2026-09-18 18:45:34'),(61,1,'UPDATE','tblAsigAppPerf',2,'Se actualizaron permisos de aplicación 2 al rol 2','192.168.1.11','2026-09-18 12:46:57','2026-09-18 18:46:57'),(62,1,'DELETE','tblAsigAppPerf',2,'Se eliminó asignación de aplicación 5 al rol 2','192.168.1.11','2026-09-18 12:47:07','2026-09-18 18:47:07'),(63,1,'INSERT','tblAsigAppUsuario',1,'Se asignó aplicación 1 al módulo 1','192.168.1.11','2026-09-18 12:48:09','2026-09-18 18:48:09'),(64,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 15:40:45','2026-09-18 21:40:45'),(65,1,'UPDATE','tblAsigAppPerf',1,'Se actualizaron permisos de aplicación 1 al rol 1','192.168.1.11','2026-09-18 15:41:54','2026-09-18 21:41:54'),(66,1,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 15:44:36','2026-09-18 21:44:36'),(67,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 15:44:58','2026-09-18 21:44:59'),(68,1,'INSERT','tblRol',0,'Se agregó el perfil: Pruebas','192.168.1.11','2026-09-18 15:45:38','2026-09-18 21:45:38'),(69,1,'DELETE','tblRol',5,'Se eliminó el rol perfil: 5','192.168.1.11','2026-09-18 15:45:45','2026-09-18 21:45:45'),(70,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 15:46:11','2026-09-18 21:46:11'),(71,1,'INSERT','tblUsuarioRol',5,'Se asignó el perfil 3 al usuario 5','192.168.1.11','2026-09-18 15:56:26','2026-09-18 21:56:27'),(72,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 11 al rol 3','192.168.1.11','2026-09-18 15:57:29','2026-09-18 21:57:29'),(73,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 10 al rol 3','192.168.1.11','2026-09-18 15:57:37','2026-09-18 21:57:37'),(74,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 8 al rol 3','192.168.1.11','2026-09-18 15:57:43','2026-09-18 21:57:43'),(75,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 7 al rol 3','192.168.1.11','2026-09-18 15:57:51','2026-09-18 21:57:51'),(76,1,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 15:58:13','2026-09-18 21:58:13'),(77,1,'INSERT','tblAsigAppPerf',1,'Se asignó aplicación 1 al rol 1','192.168.1.11','2026-09-18 15:58:39','2026-09-18 21:58:39'),(78,1,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 16:13:17','2026-09-18 22:13:17'),(79,1,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 17:43:22','2026-09-18 23:43:22'),(80,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 5 al rol 3','192.168.1.11','2026-09-18 17:44:16','2026-09-18 23:44:16'),(81,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 6 al rol 3','192.168.1.11','2026-09-18 17:44:33','2026-09-18 23:44:33'),(82,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 4 al rol 3','192.168.1.11','2026-09-18 17:44:53','2026-09-18 23:44:53'),(83,NULL,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 18:19:05','2026-09-19 00:19:05'),(84,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 18:19:24','2026-09-19 00:19:24'),(85,NULL,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 18:20:11','2026-09-19 00:20:11'),(86,NULL,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-18 18:20:32','2026-09-19 00:20:32'),(87,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 18:20:48','2026-09-19 00:20:48'),(88,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 19:58:29','2026-09-19 01:58:29'),(89,NULL,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 20:25:30','2026-09-19 02:25:30'),(90,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 20:38:01','2026-09-19 02:38:01'),(91,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 22:15:35','2026-09-19 04:15:35'),(92,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 12 al rol 3','192.168.1.11','2026-09-18 22:16:49','2026-09-19 04:16:49'),(93,NULL,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 22:17:09','2026-09-19 04:17:09'),(94,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 09:19:13','2026-09-19 15:19:13'),(95,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 16:24:37','2026-09-19 22:24:37'),(96,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 16:26:02','2026-09-19 22:26:02'),(97,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 16:45:59','2026-09-19 22:45:59'),(98,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-19 16:46:27','2026-09-19 22:46:27'),(99,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-19 16:46:42','2026-09-19 22:46:42'),(100,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-19 16:47:25','2026-09-19 22:47:25'),(101,1,'INSERT','tblEmpleado',5,'Se agregó el empleado: Pepito Muralles','192.168.1.11','2026-09-19 16:49:06','2026-09-19 22:49:06'),(102,1,'UPDATE','tblEmpleado',12,'Se actualizó el empleado: Pepito Muralles','192.168.1.11','2026-09-19 16:49:29','2026-09-19 22:49:29'),(103,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-19 16:50:24','2026-09-19 22:50:24'),(104,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 20:46:49','2026-09-20 02:46:49'),(105,1,'UPDATE','tblEmpleado',12,'Se actualizó el empleado: Pepito Muralles','192.168.1.11','2026-09-19 20:47:08','2026-09-20 02:47:08'),(106,1,'UPDATE','tblAplicacion',12,'Se actualizó la aplicación: Bitácora','192.168.1.11','2026-09-19 20:48:28','2026-09-20 02:48:28'),(107,1,'UPDATE','tblAplicacion',12,'Se actualizó la aplicación: Bitácora','192.168.1.11','2026-09-19 20:48:38','2026-09-20 02:48:38'),(108,1,'DELETE','tblAplicacion',1,'Se eliminó la aplicación ID: 1','192.168.1.11','2026-09-19 20:48:52','2026-09-20 02:48:52'),(109,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 20:49:17','2026-09-20 02:49:17'),(110,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 22:01:50','2026-09-20 04:01:50'),(111,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 22:33:27','2026-09-20 04:33:27'),(112,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 22:35:23','2026-09-20 04:35:23'),(113,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 22:36:26','2026-09-20 04:36:26'),(114,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 22:55:23','2026-09-20 04:55:23'),(115,1,'DELETE','tblRol',3,'Se eliminó el rol perfil: 3','192.168.1.11','2026-09-19 22:56:55','2026-09-20 04:56:55'),(116,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:25:33','2026-09-20 05:25:33'),(117,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:27:56','2026-09-20 05:27:56'),(118,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:28:33','2026-09-20 05:28:33'),(119,1,'UPDATE','tblModulo',2,'Se actualizó el módulo: Inventario','192.168.1.11','2026-09-19 23:28:56','2026-09-20 05:28:56'),(120,1,'UPDATE','tblModulo',2,'Se actualizó el módulo: Inventario','192.168.1.11','2026-09-19 23:29:01','2026-09-20 05:29:01'),(121,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:30:10','2026-09-20 05:30:10'),(122,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:43:23','2026-09-20 05:43:23'),(123,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:48:49','2026-09-20 05:48:49'),(124,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:50:17','2026-09-20 05:50:17'),(125,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:52:57','2026-09-20 05:52:58'),(126,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:54:44','2026-09-20 05:54:44'),(127,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:05:44','2026-09-20 06:05:44'),(128,1,'DELETE','tblAplicacion',2,'Se eliminó la aplicación ID: 2','192.168.1.11','2026-09-20 00:06:15','2026-09-20 06:06:15'),(129,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:11:20','2026-09-20 06:11:20'),(130,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:14:04','2026-09-20 06:14:04'),(131,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:17:19','2026-09-20 06:17:19'),(132,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:22:42','2026-09-20 06:22:42'),(133,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:26:44','2026-09-20 06:26:44'),(134,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','172.20.10.4','2026-09-20 00:37:21','2026-09-20 06:37:21'),(135,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','172.20.10.4','2026-09-20 00:39:27','2026-09-20 06:39:27'),(136,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:40:49','2026-09-20 14:40:49'),(137,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:43:23','2026-09-20 14:43:23'),(138,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:44:51','2026-09-20 14:44:51'),(139,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:46:20','2026-09-20 14:46:20'),(140,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:47:12','2026-09-20 14:47:12'),(141,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:55:47','2026-09-20 14:55:48'),(142,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:57:12','2026-09-20 14:57:12'),(143,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:58:00','2026-09-20 14:58:00'),(144,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:58:46','2026-09-20 14:58:46'),(145,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:59:38','2026-09-20 14:59:38'),(146,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:14:45','2026-09-20 15:14:45'),(147,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:16:11','2026-09-20 15:16:11'),(148,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:26:36','2026-09-20 15:26:36'),(149,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:29:58','2026-09-20 15:29:58'),(150,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:31:54','2026-09-20 15:31:54'),(151,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:34:18','2026-09-20 15:34:18'),(152,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:35:27','2026-09-20 15:35:27'),(153,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:39:39','2026-09-20 15:39:39'),(154,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:42:23','2026-09-20 15:42:23'),(155,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:54:49','2026-09-20 15:54:49'),(156,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:57:32','2026-09-20 15:57:32'),(157,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:59:16','2026-09-20 15:59:16'),(158,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 10:04:06','2026-09-20 16:04:06'),(159,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 11:47:28','2026-09-20 17:47:28'),(160,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 12:23:34','2026-09-20 18:23:34'),(161,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 15:35:11','2026-09-20 21:35:11'),(162,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 15:41:55','2026-09-20 21:41:55'),(163,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 18:11:00','2026-09-21 00:11:00'),(164,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 18:14:17','2026-09-21 00:14:17'),(165,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 19:52:31','2026-09-21 01:52:31'),(166,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 20:24:57','2026-09-21 02:24:58'),(167,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 21:39:02','2026-09-21 03:39:02'),(168,1,'INSERT','tblUsuario',0,'Se agregó el usuario: Ppito','192.168.1.2','2026-09-20 21:39:53','2026-09-21 03:39:53'),(169,1,'UPDATE','tblUsuario',9,'Se actualizó el usuario: Ppito','192.168.1.2','2026-09-20 21:40:30','2026-09-21 03:40:30'),(170,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 15:21:33','2026-09-21 21:21:33'),(171,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 22:04:15','2026-09-22 04:04:15'),(172,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 22:09:15','2026-09-22 04:09:15'),(173,1,'UPDATE','tblUsuario',9,'Se actualizó el usuario: Ppitote','192.168.1.2','2026-09-21 22:09:53','2026-09-22 04:09:53'),(174,1,'DELETE','tblUsuario',9,'Se eliminó el usuario ID: 9','192.168.1.2','2026-09-21 22:09:59','2026-09-22 04:10:00'),(175,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 22:11:32','2026-09-22 04:11:32'),(176,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 22:24:51','2026-09-22 04:24:51'),(177,1,'DELETE','tblUsuario',5,'Se eliminó el usuario ID: 5','192.168.1.2','2026-09-21 22:25:15','2026-09-22 04:25:15'),(178,1,'DELETE','tblEmpleado',5,'Se eliminó el empleado ID: 5','192.168.1.2','2026-09-21 22:25:25','2026-09-22 04:25:25'),(179,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 22:29:12','2026-09-22 04:29:12'),(180,1,'INSERT','tblEmpleado',0,'Se agregó el empleado: Oscar Emilio Morales Lemus','192.168.1.2','2026-09-21 22:30:17','2026-09-22 04:30:17'),(181,1,'INSERT','tblUsuario',0,'Se agregó el usuario: Omorales','192.168.1.2','2026-09-21 22:30:38','2026-09-22 04:30:38'),(182,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-22 06:47:05','2026-09-22 12:47:05'),(183,1,'DELETE','tblUsuario',11,'Se eliminó el usuario ID: 11','192.168.167.48','2026-09-22 06:48:42','2026-09-22 12:48:42'),(184,1,'DELETE','tblEmpleado',12,'Se eliminó el empleado ID: 12','192.168.167.48','2026-09-22 06:50:50','2026-09-22 12:50:51'),(185,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-22 07:08:29','2026-09-22 13:08:29'),(186,1,'INSERT','tblEmpleado',0,'Se agregó el empleado: Empleado Examen','192.168.167.48','2026-09-22 07:11:30','2026-09-22 13:11:31'),(187,1,'INSERT','tblUsuario',0,'Se agregó el usuario: UsuarioExamen','192.168.167.48','2026-09-22 07:12:30','2026-09-22 13:12:30'),(188,1,'INSERT','tblRol',0,'Se agregó el perfil: PerfilExamen','192.168.167.48','2026-09-22 07:16:29','2026-09-22 13:16:29'),(189,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.167.48','2026-09-22 07:17:23','2026-09-22 13:17:23'),(190,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-22 07:19:39','2026-09-22 13:19:39'),(191,1,'INSERT','tblAsigAppPerf',12,'Se asignó aplicación 8 al rol 12','192.168.167.48','2026-09-22 07:20:38','2026-09-22 13:20:39'),(192,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.167.48','2026-09-22 07:21:11','2026-09-22 13:21:11'),(193,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-22 07:21:45','2026-09-22 13:21:45'),(194,1,'INSERT','tblUsuarioRol',12,'Se asignó el perfil 12 al usuario 12','192.168.167.48','2026-09-22 07:22:32','2026-09-22 13:22:32'),(195,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.167.48','2026-09-22 07:22:48','2026-09-22 13:22:48'),(196,12,'INSERT','tblRol',0,'Se agregó el perfil: Prueba','192.168.167.48','2026-09-22 07:25:13','2026-09-22 13:25:13'),(197,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-22 07:26:54','2026-09-22 13:26:54'),(198,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 16:22:37','2026-09-22 22:22:37'),(199,1,'INSERT','tblModulo',0,'Se agregó el módulo: Contabilidad','192.168.1.2','2026-09-22 16:24:31','2026-09-22 22:24:31'),(200,1,'UPDATE','tblModulo',12,'Se actualizó el módulo: Contabilidad','192.168.1.2','2026-09-22 16:26:01','2026-09-22 22:26:01'),(201,1,'DELETE','tblModulo',12,'Se eliminó el módulo ID: 12','192.168.1.2','2026-09-22 16:28:47','2026-09-22 22:28:47'),(202,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 16:57:01','2026-09-22 22:57:01'),(203,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.1.2','2026-09-22 16:57:27','2026-09-22 22:57:27'),(204,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 17:48:41','2026-09-22 23:48:41'),(205,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 17:49:18','2026-09-22 23:49:18'),(206,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 17:52:57','2026-09-22 23:52:57'),(207,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 18:07:57','2026-09-23 00:07:57'),(208,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 18:09:35','2026-09-23 00:09:35'),(209,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.1.2','2026-09-22 18:10:11','2026-09-23 00:10:11'),(210,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 19:16:51','2026-09-23 01:16:51'),(211,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 20:21:32','2026-09-23 02:21:32'),(212,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 20:28:21','2026-09-23 02:28:21'),(213,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 20:39:51','2026-09-23 02:39:51'),(214,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 21:38:32','2026-09-23 03:38:32'),(215,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:32:12','2026-09-23 05:32:12'),(216,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.1.2','2026-09-22 23:33:18','2026-09-23 05:33:18'),(217,2,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.2','2026-09-22 23:33:42','2026-09-23 05:33:42'),(218,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:34:17','2026-09-23 05:34:17'),(219,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.1.2','2026-09-22 23:38:03','2026-09-23 05:38:03'),(220,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:42:18','2026-09-23 05:42:18'),(221,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:43:12','2026-09-23 05:43:12'),(222,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:44:58','2026-09-23 05:44:58'),(223,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:47:01','2026-09-23 05:47:01'),(224,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:48:54','2026-09-23 05:48:54'),(225,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:51:43','2026-09-23 05:51:43'),(226,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:52:55','2026-09-23 05:52:55'),(227,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:53:56','2026-09-23 05:53:56'),(228,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:56:37','2026-09-23 05:56:37'),(229,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:57:34','2026-09-23 05:57:34'),(230,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:58:23','2026-09-23 05:58:23'),(231,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 00:00:00','2026-09-23 06:00:00'),(232,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 00:01:45','2026-09-23 06:01:45'),(233,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 00:03:49','2026-09-23 06:03:49'),(234,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 00:15:17','2026-09-23 06:15:17'),(235,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.1.2','2026-09-23 00:15:49','2026-09-23 06:15:49'),(236,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 17:32:40','2026-09-23 23:32:40'),(237,1,'UPDATE','tblEmpleado',1,'Se actualizó el empleado: Isabel Meléndez','192.168.1.2','2026-09-23 17:40:07','2026-09-23 23:40:07'),(238,1,'INSERT','tblEmpleado',1,'Se agregó el empleado: Pruebas Capacitacion','192.168.1.2','2026-09-23 17:42:57','2026-09-23 23:42:57'),(239,1,'INSERT','tblUsuario',0,'Se agregó el usuario: Pcapacitacion','192.168.1.2','2026-09-23 17:43:51','2026-09-23 23:43:51'),(240,1,'INSERT','tblModulo',0,'Se agregó el módulo: Pruebas','192.168.1.2','2026-09-23 17:44:26','2026-09-23 23:44:26'),(241,1,'INSERT','tblAplicacion',0,'Se agregó la aplicación: Prueba','192.168.1.2','2026-09-23 17:46:12','2026-09-23 23:46:12'),(242,1,'DELETE','tblRol',13,'Se eliminó el rol perfil: 13','192.168.1.2','2026-09-23 17:46:19','2026-09-23 23:46:19'),(243,1,'UPDATE','tblRol',1,'Se actualizó el perfil: Administrador','192.168.1.2','2026-09-23 17:46:22','2026-09-23 23:46:22'),(244,1,'INSERT','tblRol',1,'Se agregó el perfil: Prueba','192.168.1.2','2026-09-23 17:47:34','2026-09-23 23:47:34'),(245,1,'INSERT','tblUsuarioRol',13,'Se asignó el perfil 14 al usuario 13','192.168.1.2','2026-09-23 17:47:47','2026-09-23 23:47:47'),(246,1,'INSERT','tblAsigAppPerf',14,'Se asignó aplicación 15 al rol 14','192.168.1.2','2026-09-23 17:48:40','2026-09-23 23:48:40'),(247,1,'INSERT','tblAsigAppUsuario',13,'Se asignó aplicación 15 al módulo 13','192.168.1.2','2026-09-23 17:49:15','2026-09-23 23:49:15'),(248,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 19:44:06','2026-09-24 01:44:07');




SELECT a.idModulo, a.idAplicacion, a.nombreAplicacion, r.nombreRol,
       rma.derInsertarRolModuloAplicacion AS Insertar,
       rma.derEditarRolModuloAplicacion AS Editar,
       rma.derEliminarRolModuloAplicacion AS Eliminar,
       rma.derImprimirRolModuloAplicacion AS Imprimir
FROM tblAplicacion a
LEFT JOIN tblRolModuloAplicacion rma ON rma.idAplicacion = a.idAplicacion AND rma.idModulo = a.idModulo
LEFT JOIN tblRol r ON r.idRol = rma.idRol
WHERE a.idModulo = (SELECT idModulo FROM tblModulo WHERE nombreModulo = 'Seguridad')
ORDER BY a.idAplicacion, r.nombreRol;
SELECT idAplicacion, nombreAplicacion
FROM tblAplicacion
WHERE idModulo = (SELECT idModulo FROM tblModulo WHERE nombreModulo = 'Seguridad')
ORDER BY idAplicacion;

-- ------------------------------------------- Reporteador ----------------------------------------------
CREATE TABLE tblReporte (
    numeroReporte INT NOT NULL,
    nombreReporte VARCHAR(150) NOT NULL,
    rutaReporte VARCHAR(500) NOT NULL,
    fechaReporte DATE NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_Reporte PRIMARY KEY (numeroReporte),
    CONSTRAINT uqNombreReporte UNIQUE (nombreReporte)
) ENGINE=InnoDB;
-- TABLA INTERMEDIA: APLICACION - REPORTE
CREATE TABLE tblAplicacionReporte (
    idAplicacion INT NOT NULL,
    numeroReporte INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_AplicacionReporte PRIMARY KEY (idAplicacion, numeroReporte),
    CONSTRAINT Fk_Aplicacion_IdAplicacion FOREIGN KEY (idAplicacion)
        REFERENCES tblAplicacion (idAplicacion) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_Reporte_NumeroReporte FOREIGN KEY (numeroReporte)
        REFERENCES tblReporte (numeroReporte) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB;

-- ------------------------------------------- Consultas ------------------------------------------------
CREATE TABLE tblConsulta (
    Pk_Consulta INT AUTO_INCREMENT,
    nombreConsulta VARCHAR(100) NOT NULL,
    tablaConsulta VARCHAR(255) NOT NULL, 
    queryConsulta TEXT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_Consulta PRIMARY KEY (Pk_Consulta),
    CONSTRAINT Uq_NombreConsulta UNIQUE (nombreConsulta)
);

-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡ Modulos !!!!!!!!!!!!!!!!!!!!!!!!!-------------

-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡  RECURSOS HUMANOS !!!!!!!!!!!!!!!!!!!!!!!!!-------------

-- ---------- Departamentos ----------
CREATE TABLE IF NOT EXISTS tblDepartamento (
    idDepartamento INT AUTO_INCREMENT NOT NULL,
    nombreDepartamento VARCHAR(100) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Departamento PRIMARY KEY (idDepartamento),
    CONSTRAINT uqNombreDepartamento UNIQUE (nombreDepartamento)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
-- ---------- Empresas (sistema multiempresa) ----------
CREATE TABLE tblEmpresa (
    idEmpresa INT AUTO_INCREMENT NOT NULL,
    nitEmpresa CHAR(10) NOT NULL,
    razonSocialEmpresa VARCHAR(100) NOT NULL,
    nombreComercialEmpresa VARCHAR(100) NULL,         -- opcional
    direccionEmpresa VARCHAR(500) NOT NULL,
    representanteLegalEmpresa VARCHAR(100) NOT NULL,  -- aparece en el finiquito
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Empresa PRIMARY KEY (idEmpresa),
    CONSTRAINT uqNitEmpresa UNIQUE (nitEmpresa)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
-- ---------- Puestos y empleados ----------
CREATE TABLE tblTipoPuesto (
    idTipoPuesto INT AUTO_INCREMENT NOT NULL,
    nombreTipoPuesto VARCHAR(50) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_TipoPuesto PRIMARY KEY (idTipoPuesto),
    CONSTRAINT uqNombreTipoPuesto UNIQUE (nombreTipoPuesto)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE tblPuesto (
    idPuesto INT AUTO_INCREMENT NOT NULL,
    idTipoPuesto INT NOT NULL,
    nombrePuesto VARCHAR(100) NOT NULL,
    descripcionPuesto VARCHAR(255) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Puesto PRIMARY KEY (idPuesto),
    CONSTRAINT Fk_Puesto_TipoPuesto FOREIGN KEY (idTipoPuesto)
        REFERENCES tblTipoPuesto (idTipoPuesto) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT uqNombrePuesto UNIQUE (nombrePuesto)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------- Jornadas ----------

CREATE TABLE tblJornada (
    idJornada INT AUTO_INCREMENT NOT NULL,
    nombreJornada VARCHAR(50) NOT NULL,
    horaInicioJornada TIME NOT NULL,
    horaFinJornada TIME NOT NULL,
    minutosToleranciaJornada INT NOT NULL DEFAULT 5,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Jornada PRIMARY KEY (idJornada),
    CONSTRAINT uqNombreJornada UNIQUE (nombreJornada),
    CONSTRAINT ckJornadaHoras CHECK (horaFinJornada <> horaInicioJornada),
    CONSTRAINT ckJornadaTolerancia CHECK (minutosToleranciaJornada >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------- Datos de RRHH en el empleado ----------
-- 1) Datos base (padres)
INSERT INTO tblTipoPuesto (nombreTipoPuesto) VALUES ('Administrativo'), ('Operativo');

INSERT INTO tblPuesto (idTipoPuesto, nombrePuesto, descripcionPuesto) VALUES
(1, 'Coordinadora de Implementación', 'Coordina la implementación del sistema'),
(1, 'Analista de Sistemas', 'Análisis de sistemas'),
(1, 'Administradora de Base de Datos', 'Administración de BD'),
(1, 'Gerente', 'Gerencia general'),
(1, 'Pruebas', 'Puesto de pruebas'),
(1, 'Probador', 'Puesto de capacitación y pruebas');

INSERT INTO tblDepartamento (nombreDepartamento) VALUES ('Sistemas'), ('Gerencia');

INSERT INTO tblJornada (nombreJornada, horaInicioJornada, horaFinJornada)
VALUES ('Diurna', '08:00:00', '17:00:00');

-- 2) Historial de puestos: se crea ANTES de eliminar puestoEmpleado
-- vigentePuestoEmpleado es una columna generada A PROPÓSITO: vale idEmpleado mientras
-- el puesto está vigente y NULL cuando no; con el UNIQUE garantiza un solo puesto vigente por empleado.
CREATE TABLE tblPuestoEmpleado (
    idPuestoEmpleado INT AUTO_INCREMENT NOT NULL,
    idEmpleado INT NOT NULL,
    idPuesto INT NOT NULL,
    fechaAsignacionPuestoEmpleado DATE NOT NULL,
    fechaFinPuestoEmpleado DATE NULL,
    vigentePuestoEmpleado INT
        AS (IF(fechaFinPuestoEmpleado IS NULL AND is_active = 1, idEmpleado, NULL)) STORED,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PuestoEmpleado PRIMARY KEY (idPuestoEmpleado),
    CONSTRAINT Fk_PuestoEmpleado_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_PuestoEmpleado_Puesto FOREIGN KEY (idPuesto)
        REFERENCES tblPuesto (idPuesto) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT uqPuestoEmpleadoVigente UNIQUE (vigentePuestoEmpleado),
    CONSTRAINT ckPuestoEmpleadoFechas CHECK
        (fechaFinPuestoEmpleado IS NULL OR fechaFinPuestoEmpleado >= fechaAsignacionPuestoEmpleado),
    KEY idxPuestoEmpleadoEmpleadoFecha (idEmpleado, fechaAsignacionPuestoEmpleado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 3) Cargar el puesto actual de cada empleado como primer registro del historial
INSERT INTO tblPuestoEmpleado (idEmpleado, idPuesto, fechaAsignacionPuestoEmpleado)
SELECT e.idEmpleado, p.idPuesto, e.fechaContratacionEmpleado
FROM tblEmpleado e
JOIN tblPuesto p ON p.nombrePuesto = e.puestoEmpleado;

-- 4) Columnas nuevas en tblEmpleado (el puesto ya NO vive aquí)
ALTER TABLE tblEmpleado
    ADD COLUMN idDepartamento INT NULL AFTER apellidosEmpleado,
    ADD COLUMN idJornada INT NULL AFTER idDepartamento,
    ADD COLUMN salarioEmpleado DECIMAL(12,4) NULL AFTER idJornada;

UPDATE tblEmpleado e
JOIN tblDepartamento d ON d.nombreDepartamento = IF(e.puestoEmpleado = 'Gerente', 'Gerencia', 'Sistemas')
SET e.idDepartamento = d.idDepartamento
WHERE e.idEmpleado > 0;

UPDATE tblEmpleado
SET idJornada = (SELECT idJornada FROM tblJornada WHERE nombreJornada = 'Diurna'),
    salarioEmpleado = 0
WHERE idEmpleado > 0;

-- 5) NOT NULL, quitar columna vieja y crear FK
ALTER TABLE tblEmpleado
    MODIFY idDepartamento INT NOT NULL,
    MODIFY idJornada INT NOT NULL,
    MODIFY salarioEmpleado DECIMAL(12,4) NOT NULL,
    DROP COLUMN puestoEmpleado,
    ADD CONSTRAINT Fk_Empleado_Departamento FOREIGN KEY (idDepartamento)
        REFERENCES tblDepartamento (idDepartamento) ON DELETE RESTRICT ON UPDATE CASCADE,
    ADD CONSTRAINT Fk_Empleado_Jornada FOREIGN KEY (idJornada)
        REFERENCES tblJornada (idJornada) ON DELETE RESTRICT ON UPDATE CASCADE;


-- ---------- Metodos de pago ----------
CREATE TABLE tblMetodoPago (
    idMetodoPago INT AUTO_INCREMENT NOT NULL,
    nombreMetodoPago VARCHAR(100) NOT NULL,
    descripcionMetodoPago VARCHAR(255) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_MetodoPago PRIMARY KEY (idMetodoPago),
    CONSTRAINT uqNombreMetodoPago UNIQUE (nombreMetodoPago)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblEmpleadoMetodoPago (
    idEmpleado INT NOT NULL,
    idMetodoPago INT NOT NULL,
    fechaAsignacionEmpleadoMetodoPago DATE NOT NULL,
    montoEmpleadoMetodoPago DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_EmpleadoMetodoPago PRIMARY KEY (idEmpleado, idMetodoPago),
    CONSTRAINT Fk_EmpleadoMetodoPago_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT Fk_EmpleadoMetodoPago_MetodoPago FOREIGN KEY (idMetodoPago)
        REFERENCES tblMetodoPago (idMetodoPago) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
-- ---------- Asistencia ----------
CREATE TABLE tblAsistenciaEmpleado (
    idAsistenciaEmpleado INT AUTO_INCREMENT NOT NULL,
    idEmpleado INT NOT NULL,
    fechaAsistenciaEmpleado DATE NOT NULL,
    horaEntradaAsistenciaEmpleado TIME NULL,
    horaSalidaAsistenciaEmpleado TIME NULL,
    estadoAsistenciaEmpleado ENUM('PRESENTE', 'TARDANZA', 'AUSENTE', 'PERMISO') NOT NULL DEFAULT 'PRESENTE',
    observacionAsistenciaEmpleado VARCHAR(255) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_AsistenciaEmpleado PRIMARY KEY (idAsistenciaEmpleado),
    CONSTRAINT Fk_AsistenciaEmpleado_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT uqAsistenciaEmpleadoFecha UNIQUE (idEmpleado, fechaAsistenciaEmpleado),
    CONSTRAINT ckAsistenciaEmpleadoHoras CHECK (
        horaEntradaAsistenciaEmpleado IS NULL
        OR horaSalidaAsistenciaEmpleado IS NULL
        OR horaSalidaAsistenciaEmpleado > horaEntradaAsistenciaEmpleado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
-- ---------- Percepciones y deducciones ----------

CREATE TABLE tblPercepcionDeduccion (
    idPercepcionDeduccion INT AUTO_INCREMENT NOT NULL,
    nombrePercepcionDeduccion VARCHAR(100) NOT NULL,
    periodoPercepcionDeduccion ENUM('MENSUAL', 'ANUAL') NOT NULL,
    efectoPercepcionDeduccion BOOLEAN NOT NULL DEFAULT FALSE,
    tipoPercepcionDeduccion ENUM('Tabla', '%', 'Monto') NOT NULL,
    valorTipoPercepcionDeduccion DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    aplicacionPercepcionDeduccion ENUM('Todos', 'Algunos') NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PercepcionDeduccion PRIMARY KEY (idPercepcionDeduccion),
    CONSTRAINT uqNombrePercepcionDeduccion UNIQUE (nombrePercepcionDeduccion)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblPercepcionDeduccionEmpleado (
    idPercepcionDeduccionEmpleado INT AUTO_INCREMENT NOT NULL,
    idPercepcionDeduccion INT NOT NULL,
    idEmpleado INT NULL,
    idDepartamento INT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PercepcionDeduccionEmpleado PRIMARY KEY (idPercepcionDeduccionEmpleado),
    CONSTRAINT Fk_PercepcionDeduccionEmpleado_PercepcionDeduccion FOREIGN KEY (idPercepcionDeduccion)
        REFERENCES tblPercepcionDeduccion (idPercepcionDeduccion),
    CONSTRAINT Fk_PercepcionDeduccionEmpleado_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado),
    CONSTRAINT Fk_PercepcionDeduccionEmpleado_Departamento FOREIGN KEY (idDepartamento)
        REFERENCES tblDepartamento (idDepartamento),
    -- Evita duplicados por empleado y por departamento (cada uno por separado)
    CONSTRAINT uqPercepcionDeduccionEmpleadoEmpleado UNIQUE (idPercepcionDeduccion, idEmpleado),
    CONSTRAINT uqPercepcionDeduccionEmpleadoDepartamento UNIQUE (idPercepcionDeduccion, idDepartamento)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblPercepcionDeduccionRango (
    idPercepcionDeduccionRango INT AUTO_INCREMENT NOT NULL,
    idPercepcionDeduccion INT NOT NULL,
    salarioDesdePercepcionDeduccionRango DECIMAL(12,4) NOT NULL,
    salarioHastaPercepcionDeduccionRango DECIMAL(12,4) NULL,
    modoValorPercepcionDeduccionRango ENUM('%', 'Monto') NOT NULL,
    valorPercepcionDeduccionRango DECIMAL(12,4) NOT NULL,
    montoFijoBasePercepcionDeduccionRango DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PercepcionDeduccionRango PRIMARY KEY (idPercepcionDeduccionRango),
    CONSTRAINT Fk_PercepcionDeduccionRango_PercepcionDeduccion FOREIGN KEY (idPercepcionDeduccion)
        REFERENCES tblPercepcionDeduccion (idPercepcionDeduccion),
    CONSTRAINT uqPercepcionDeduccionRangoDesde UNIQUE (idPercepcionDeduccion, salarioDesdePercepcionDeduccionRango)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
-- ---------- Nomina ----------

CREATE TABLE tblNominaEncabezado (
    idNominaEncabezado INT AUTO_INCREMENT NOT NULL,
    idEmpleado INT NOT NULL,
    fechaInicioNominaEncabezado DATE NOT NULL,
    fechaFinalNominaEncabezado DATE NOT NULL,
    salarioBaseNominaEncabezado DECIMAL(12,4) NOT NULL,
    duracionHoraExtraNominaEncabezado INT NOT NULL DEFAULT 0,
    totalPercepcionNominaEncabezado DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalDeduccionNominaEncabezado DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalNetoNominaEncabezado DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    estadoNminaEncabezado ENUM('Aprobada', 'Rechazada', 'Pagada') NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_NominaEncabezado PRIMARY KEY (idNominaEncabezado),
    CONSTRAINT Fk_NominaEncabezado_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT ckNominaEncabezadoFechas CHECK (fechaFinalNominaEncabezado >= fechaInicioNominaEncabezado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblNominaDetalle (
    idNominaDetalle INT AUTO_INCREMENT NOT NULL,
    idNominaEncabezado INT NOT NULL,
    idPercepcionDeduccion INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_NominaDetalle PRIMARY KEY (idNominaDetalle),
    CONSTRAINT Fk_NominaDetalle_NominaEncabezado FOREIGN KEY (idNominaEncabezado)
        REFERENCES tblNominaEncabezado (idNominaEncabezado),
    CONSTRAINT Fk_NominaDetalle_PercepcionDeduccion FOREIGN KEY (idPercepcionDeduccion)
        REFERENCES tblPercepcionDeduccion (idPercepcionDeduccion)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 

CREATE TABLE tblHoraExtra (
    idHoraExtra INT AUTO_INCREMENT NOT NULL,
    idPercepcionDeduccion INT NOT NULL,
    idEmpleado INT NOT NULL,
    idAsistenciaEmpleado INT NOT NULL,
    duracionHoraExtra INT NOT NULL,
    valorHoraOrdinariaHoraExtra DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    porcentajeRecargoHoraExtra DECIMAL(3,2) NOT NULL DEFAULT 0.50,
    totalPagoHoraExtra DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    estadoHoraExtra ENUM('PENDIENTE', 'APROBADA', 'RECHAZADA', 'PAGADA') NOT NULL DEFAULT 'PENDIENTE',
    idUsuarioAprueba INT NULL,
    fechaAprobacionHoraExtra DATETIME NULL,
    idNominaEncabezado INT NULL,
    observacionHoraExtra VARCHAR(255) NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_HoraExtra PRIMARY KEY (idHoraExtra),
    CONSTRAINT Fk_HoraExtra_PercepcionDeduccion FOREIGN KEY (idPercepcionDeduccion)
        REFERENCES tblPercepcionDeduccion (idPercepcionDeduccion) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_HoraExtra_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_HoraExtra_AsistenciaEmpleado FOREIGN KEY (idAsistenciaEmpleado)
        REFERENCES tblAsistenciaEmpleado (idAsistenciaEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_HoraExtra_UsuarioAprueba FOREIGN KEY (idUsuarioAprueba)
        REFERENCES tblUsuario (idUsuario) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_HoraExtra_NominaEncabezado FOREIGN KEY (idNominaEncabezado)
        REFERENCES tblNominaEncabezado (idNominaEncabezado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT uqHoraExtraAsistencia UNIQUE (idAsistenciaEmpleado),
    CONSTRAINT ckHoraExtraDuracion CHECK (duracionHoraExtra > 0 AND duracionHoraExtra <= 360),
    CONSTRAINT ckHoraExtraRecargo CHECK (porcentajeRecargoHoraExtra >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
-- ---------- Evaluacion de desempeno ----------

CREATE TABLE tblCicloEvaluacion (
    idCicloEvaluacion INT AUTO_INCREMENT NOT NULL,
    nombreCicloEvaluacion VARCHAR(100) NOT NULL,
    fechaInicioCicloEvaluacion DATE NOT NULL,
    fechaFinCicloEvaluacion DATE NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_CicloEvaluacion PRIMARY KEY (idCicloEvaluacion),
    CONSTRAINT uqNombreCicloEvaluacion UNIQUE (nombreCicloEvaluacion),
    CONSTRAINT ckCicloEvaluacionFechas CHECK (fechaFinCicloEvaluacion >= fechaInicioCicloEvaluacion)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblCriterioEvaluacion (
    idCriterioEvaluacion INT AUTO_INCREMENT NOT NULL,
    nombreCriterioEvaluacion VARCHAR(100) NOT NULL,
    descripcionCriterioEvaluacion VARCHAR(255) NOT NULL,
    pesoCriterioEvaluacion DECIMAL(3,2) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_CriterioEvaluacion PRIMARY KEY (idCriterioEvaluacion),
    CONSTRAINT uqNombreCriterioEvaluacion UNIQUE (nombreCriterioEvaluacion),
    CONSTRAINT ckCriterioEvaluacionPeso CHECK (pesoCriterioEvaluacion > 0 AND pesoCriterioEvaluacion <= 1)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblNivelDesempeno (
    idNivelDesempeno INT AUTO_INCREMENT NOT NULL,
    nombreNivelDesempeno VARCHAR(100) NOT NULL,
    puntajeMinimoNivelDesempeno DECIMAL(3,2) NOT NULL,
    puntajeMaximoNivelDesempeno DECIMAL(3,2) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_NivelDesempeno PRIMARY KEY (idNivelDesempeno),
    CONSTRAINT uqNombreNivelDesempeno UNIQUE (nombreNivelDesempeno),
    CONSTRAINT ckNivelDesempenoRango CHECK (
        puntajeMinimoNivelDesempeno >= 0
        AND puntajeMaximoNivelDesempeno <= 1
        AND puntajeMaximoNivelDesempeno >= puntajeMinimoNivelDesempeno)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblPlanMejora (
    idPlanMejora INT AUTO_INCREMENT NOT NULL,
    nombrePlanMejora VARCHAR(100) NOT NULL,
    descripcionPlanMejora VARCHAR(255) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PlanMejora PRIMARY KEY (idPlanMejora),
    CONSTRAINT uqNombrePlanMejora UNIQUE (nombrePlanMejora)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblEvaluacionDesempeno (
    idEvaluacionDesempeno INT AUTO_INCREMENT NOT NULL,
    idEmpleado INT NOT NULL,
    idEvaluador INT NOT NULL,
    idCicloEvaluacion INT NOT NULL,
    idNivelDesempeno INT NULL,
    idPlanMejora INT NULL,
    fechaEvaluacionDesempeno DATE NOT NULL,
    puntajeEvaluacionDesempeno DECIMAL(4,2) NOT NULL DEFAULT 0.00,      -- sobre 5.00
    porcentajeEvaluacionDesempeno DECIMAL(3,2) NOT NULL DEFAULT 0.00,   -- fraccion: 0.00 a 1.00
    retroalimentacionEvaluacionDesempeno VARCHAR(500) NOT NULL,
    estadoEvaluacionDesempeno ENUM('BORRADOR', 'ENVIADA', 'COMPARTIDA', 'APROBADA', 'ANULADA') NOT NULL DEFAULT 'BORRADOR',
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_EvaluacionDesempeno PRIMARY KEY (idEvaluacionDesempeno),
    CONSTRAINT Fk_EvaluacionDesempeno_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_EvaluacionDesempeno_Evaluador FOREIGN KEY (idEvaluador)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_EvaluacionDesempeno_CicloEvaluacion FOREIGN KEY (idCicloEvaluacion)
        REFERENCES tblCicloEvaluacion (idCicloEvaluacion) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_EvaluacionDesempeno_NivelDesempeno FOREIGN KEY (idNivelDesempeno)
        REFERENCES tblNivelDesempeno (idNivelDesempeno) ON DELETE SET NULL ON UPDATE CASCADE,
    CONSTRAINT Fk_EvaluacionDesempeno_PlanMejora FOREIGN KEY (idPlanMejora)
        REFERENCES tblPlanMejora (idPlanMejora) ON DELETE SET NULL ON UPDATE CASCADE,
    CONSTRAINT uqEvaluacionDesempenoEmpleadoCiclo UNIQUE (idEmpleado, idCicloEvaluacion),
    CONSTRAINT ckEvaluacionDesempenoPuntaje CHECK (puntajeEvaluacionDesempeno BETWEEN 0 AND 5),
    CONSTRAINT ckEvaluacionDesempenoPorcentaje CHECK (porcentajeEvaluacionDesempeno BETWEEN 0 AND 1)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblDetalleEvaluacionDesempeno (
    idEvaluacionDesempeno INT NOT NULL,
    idCriterioEvaluacion INT NOT NULL,
    pesoDetalleEvaluacionDesempeno DECIMAL(3,2) NOT NULL,     -- copia del peso al momento de evaluar
    notaDetalleEvaluacionDesempeno TINYINT NOT NULL,
    ponderadoDetalleEvaluacionDesempeno DECIMAL(4,2) NOT NULL DEFAULT 0.00,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_DetalleEvaluacionDesempeno PRIMARY KEY (idEvaluacionDesempeno, idCriterioEvaluacion),
    CONSTRAINT Fk_DetalleEvaluacionDesempeno_EvaluacionDesempeno FOREIGN KEY (idEvaluacionDesempeno)
        REFERENCES tblEvaluacionDesempeno (idEvaluacionDesempeno) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT Fk_DetalleEvaluacionDesempeno_CriterioEvaluacion FOREIGN KEY (idCriterioEvaluacion)
        REFERENCES tblCriterioEvaluacion (idCriterioEvaluacion) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT ckDetalleEvaluacionDesempenoNota CHECK (notaDetalleEvaluacionDesempeno BETWEEN 1 AND 5)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
-- ---------- Permisos y vacaciones ----------
CREATE TABLE tblTipoPermiso (
    idTipoPermiso INT AUTO_INCREMENT NOT NULL,
    nombreTipoPermiso VARCHAR(100) NOT NULL,
    descripcionTipoPermiso VARCHAR(255) NOT NULL,
    conGoceSueldoTipoPermiso BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_TipoPermiso PRIMARY KEY (idTipoPermiso),
    CONSTRAINT uqNombreTipoPermiso UNIQUE (nombreTipoPermiso)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblPermisoEmpleado (
    idPermisoEmpleado INT AUTO_INCREMENT NOT NULL,
    idEmpleado INT NOT NULL,
    idTipoPermiso INT NOT NULL,
    fechaInicioPermisoEmpleado DATE NOT NULL,
    fechaFinPermisoEmpleado DATE NOT NULL,
    horaInicioPermisoEmpleado TIME NULL,                      -- solo si se calcula en horas
    horaFinPermisoEmpleado TIME NULL,
    unidadPermisoEmpleado ENUM('DIAS', 'HORAS') NOT NULL DEFAULT 'DIAS',
    totalPermisoEmpleado DECIMAL(6,2) NOT NULL DEFAULT 0.00,  -- dias u horas calculados (ver nota de duracion)
    motivoPermisoEmpleado VARCHAR(255) NOT NULL,
    estadoPermisoEmpleado ENUM('PENDIENTE', 'APROBADO', 'RECHAZADO', 'ANULADO') NOT NULL DEFAULT 'PENDIENTE',
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PermisoEmpleado PRIMARY KEY (idPermisoEmpleado),
    CONSTRAINT Fk_PermisoEmpleado_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_PermisoEmpleado_TipoPermiso FOREIGN KEY (idTipoPermiso)
        REFERENCES tblTipoPermiso (idTipoPermiso) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT ckPermisoEmpleadoFechas CHECK (fechaFinPermisoEmpleado >= fechaInicioPermisoEmpleado),
    CONSTRAINT ckPermisoEmpleadoTotal CHECK (totalPermisoEmpleado >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblPeriodoVacacion (
    idPeriodoVacacion INT AUTO_INCREMENT NOT NULL,
    idEmpleado INT NOT NULL,
    fechaInicioPeriodoVacacion DATE NOT NULL,
    fechaFinPeriodoVacacion DATE NOT NULL,
    diasCorrespondientesPeriodoVacacion DECIMAL(5,2) NOT NULL DEFAULT 15.00,
    diasTomadosPeriodoVacacion DECIMAL(5,2) NOT NULL DEFAULT 0.00,
    diasDisponiblesPeriodoVacacion DECIMAL(5,2)
AS (diasCorrespondientesPeriodoVacacion - diasTomadosPeriodoVacacion) STORED,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PeriodoVacacion PRIMARY KEY (idPeriodoVacacion),
    CONSTRAINT Fk_PeriodoVacacion_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT uqPeriodoVacacionEmpleadoInicio UNIQUE (idEmpleado, fechaInicioPeriodoVacacion),
    CONSTRAINT ckPeriodoVacacionFechas CHECK (fechaFinPeriodoVacacion > fechaInicioPeriodoVacacion),
    CONSTRAINT ckPeriodoVacacionDias CHECK (
        diasTomadosPeriodoVacacion >= 0
        AND diasTomadosPeriodoVacacion <= diasCorrespondientesPeriodoVacacion)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblVacacionEmpleado (
    idVacacionEmpleado INT AUTO_INCREMENT NOT NULL,
    idEmpleado INT NOT NULL,
    idPeriodoVacacion INT NOT NULL,
    fechaInicioVacacionEmpleado DATE NOT NULL,
    fechaFinVacacionEmpleado DATE NOT NULL,
    diasTomarVacacionEmpleado DECIMAL(5,2) NOT NULL DEFAULT 0.00,
    observacionVacacionEmpleado VARCHAR(255) NULL,
    estadoVacacionEmpleado ENUM('PENDIENTE', 'APROBADO', 'EN CURSO', 'FINALIZADO', 'ANULADO') NOT NULL DEFAULT 'PENDIENTE',
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_VacacionEmpleado PRIMARY KEY (idVacacionEmpleado),
    CONSTRAINT Fk_VacacionEmpleado_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_VacacionEmpleado_PeriodoVacacion FOREIGN KEY (idPeriodoVacacion)
        REFERENCES tblPeriodoVacacion (idPeriodoVacacion) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT ckVacacionEmpleadoFechas CHECK (fechaFinVacacionEmpleado >= fechaInicioVacacionEmpleado),
    CONSTRAINT ckVacacionEmpleadoDias CHECK (diasTomarVacacionEmpleado > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
-- ---------- Capacitaciones ----------
CREATE TABLE tblInstructor (
    idInstructor INT AUTO_INCREMENT NOT NULL,
    nombreInstructor VARCHAR(100) NOT NULL,
    especialidadInstructor VARCHAR(100) NOT NULL,
    correoInstructor VARCHAR(150) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Instructor PRIMARY KEY (idInstructor),
    CONSTRAINT uqCorreoInstructor UNIQUE (correoInstructor)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblCapacitacion (
    idCapacitacion INT AUTO_INCREMENT NOT NULL,
    idDepartamento INT NULL,                                  
    nombreCapacitacion VARCHAR(100) NOT NULL,
    descripcionCapacitacion VARCHAR(255) NULL,
    fechaInicioCapacitacion DATE NOT NULL,
    fechaFinCapacitacion DATE NOT NULL,
    costoCapacitacion DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    cupoCapacitacion INT NOT NULL,
    estadoCapacitacion BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Capacitacion PRIMARY KEY (idCapacitacion),
    CONSTRAINT Fk_Capacitacion_Departamento FOREIGN KEY (idDepartamento)
        REFERENCES tblDepartamento (idDepartamento)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblAsignacionCapacitacion (
    idAsignacionCapacitacion INT AUTO_INCREMENT NOT NULL,
    idEmpleado INT NOT NULL,
    idCapacitacion INT NOT NULL,
    fechaAsignacionCapacitacion DATE NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_AsignacionCapacitacion PRIMARY KEY (idAsignacionCapacitacion),
    CONSTRAINT Fk_AsignacionCapacitacion_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado),
    CONSTRAINT Fk_AsignacionCapacitacion_Capacitacion FOREIGN KEY (idCapacitacion)
        REFERENCES tblCapacitacion (idCapacitacion),
    CONSTRAINT uqAsignacionCapacitacion UNIQUE (idEmpleado, idCapacitacion)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
CREATE TABLE tblEvaluacionCapacitacion (
    idEvaluacionCapacitacion INT AUTO_INCREMENT NOT NULL,
    idAsignacionCapacitacion INT NOT NULL,
    puntualidadEvaluacionCapacitacion DECIMAL(5,2) NOT NULL,
    calidadEvaluacionCapacitacion DECIMAL(5,2) NOT NULL,
    responsabilidadEvaluacionCapacitacion DECIMAL(5,2) NOT NULL,
    desarrolloEvaluacionCapacitacion DECIMAL(5,2) NOT NULL,
    resultadoFinalEvaluacionCapacitacion DECIMAL(5,2) NOT NULL DEFAULT 0.00,
    observacionEvaluacionCapacitacion VARCHAR(255) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_EvaluacionCapacitacion PRIMARY KEY (idEvaluacionCapacitacion),
    CONSTRAINT Fk_EvaluacionCapacitacion_AsignacionCapacitacion FOREIGN KEY (idAsignacionCapacitacion)
        REFERENCES tblAsignacionCapacitacion (idAsignacionCapacitacion),
    CONSTRAINT uqEvaluacionCapacitacion UNIQUE (idAsignacionCapacitacion)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 
 
CREATE TABLE tblMotivoBaja (
    idMotivoBaja INT AUTO_INCREMENT NOT NULL,
    nombreMotivoBaja VARCHAR(100) NOT NULL,
    aplicaIndemnizacionMotivoBaja BOOLEAN NOT NULL DEFAULT FALSE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_MotivoBaja PRIMARY KEY (idMotivoBaja),
    CONSTRAINT uqNombreMotivoBaja UNIQUE (nombreMotivoBaja)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 

CREATE TABLE tblBajaEmpleado (
    idBajaEmpleado INT AUTO_INCREMENT NOT NULL,
    idEmpleado INT NOT NULL,
    idMotivoBaja INT NOT NULL,
    fechaBajaEmpleado DATE NOT NULL,
    observacionBajaEmpleado VARCHAR(255) NULL,
    idUsuarioRegistra INT NOT NULL,                       
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_BajaEmpleado PRIMARY KEY (idBajaEmpleado),
    CONSTRAINT Fk_BajaEmpleado_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_BajaEmpleado_Usuario FOREIGN KEY (idUsuarioRegistra)
        REFERENCES tblUsuario (idUsuario) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_BajaEmpleado_MotivoBaja FOREIGN KEY (idMotivoBaja)
        REFERENCES tblMotivoBaja (idMotivoBaja) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 

CREATE TABLE tblParametroLey (
    idParametroLey INT AUTO_INCREMENT NOT NULL,
    idEmpresa INT NULL,                               
    codigoParametroLey VARCHAR(30) NOT NULL,
    descripcionParametroLey VARCHAR(255) NOT NULL,
    valorParametroLey DECIMAL(12,4) NOT NULL,
    vigenteDesdeParametroLey DATE NOT NULL,
    vigenteHastaParametroLey DATE NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ParametroLey PRIMARY KEY (idParametroLey),
    CONSTRAINT Fk_ParametroLey_Empresa FOREIGN KEY (idEmpresa)
        REFERENCES tblEmpresa (idEmpresa) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT ckParametroLeyVigencia CHECK
        (vigenteHastaParametroLey IS NULL OR vigenteHastaParametroLey >= vigenteDesdeParametroLey),
    KEY idxParametroLeyBusqueda (codigoParametroLey, idEmpresa, vigenteDesdeParametroLey)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 

CREATE TABLE tblLiquidacion (
    idLiquidacion INT AUTO_INCREMENT NOT NULL,
    idEmpresa INT NOT NULL,
    idEmpleado INT NOT NULL,
    idBajaEmpleado INT NOT NULL,
    fechaIngresoLiquidacion DATE NOT NULL,
    fechaBajaLiquidacion DATE NOT NULL,
    diasLaboradosLiquidacion INT NOT NULL,
    salarioMensualLiquidacion DECIMAL(12,4) NOT NULL,
    salarioPromedioLiquidacion DECIMAL(12,4) NOT NULL,
    baseIndemnizacionLiquidacion DECIMAL(12,4) NOT NULL,
    salarioDiaLiquidacion DECIMAL(12,4) NOT NULL,
    totalPercepcionLiquidacion DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalDeduccionLiquidacion DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalNetoLiquidacion DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    parametrosUsadosLiquidacion TEXT NULL,            -- JSON con los valores aplicados y su origen (LEY/EMPRESA)
    numeroFiniquitoLiquidacion VARCHAR(30) NULL,
    fechaEmisionFiniquitoLiquidacion DATE NULL,
    montoLetrasFiniquitoLiquidacion VARCHAR(255) NULL,
    estadoLiquidacion ENUM('BORRADOR', 'APROBADA', 'PAGADA', 'ANULADA') NOT NULL DEFAULT 'BORRADOR',
    idUsuarioCrea INT NOT NULL,                        
    idUsuarioAprueba INT NOT NULL,
    fechaAprobacionLiquidacion DATETIME NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Liquidacion PRIMARY KEY (idLiquidacion),
    CONSTRAINT Fk_Liquidacion_Empresa FOREIGN KEY (idEmpresa)
        REFERENCES tblEmpresa (idEmpresa) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_Liquidacion_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_Liquidacion_BajaEmpleado FOREIGN KEY (idBajaEmpleado)
        REFERENCES tblBajaEmpleado (idBajaEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_Liquidacion_UsuarioCrea FOREIGN KEY (idUsuarioCrea)
        REFERENCES tblUsuario (idUsuario) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_Liquidacion_UsuarioAprueba FOREIGN KEY (idUsuarioAprueba)
        REFERENCES tblUsuario (idUsuario) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT uqNumeroFiniquitoLiquidacion UNIQUE (numeroFiniquitoLiquidacion),
    CONSTRAINT ckLiquidacionFechas CHECK (fechaBajaLiquidacion >= fechaIngresoLiquidacion),
    CONSTRAINT ckLiquidacionDias CHECK (diasLaboradosLiquidacion >= 0),
    KEY idxLiquidacionEmpresaEstado (idEmpresa, estadoLiquidacion)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
 

CREATE TABLE tblDetalleLiquidacion (
    idDetalleLiquidacion INT AUTO_INCREMENT NOT NULL,
    idLiquidacion INT NOT NULL,
    idPercepcionDeduccion INT NOT NULL,
    diasDetalleLiquidacion DECIMAL(8,2) NOT NULL DEFAULT 0.00,
    baseDetalleLiquidacion DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    montoDetalleLiquidacion DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_DetalleLiquidacion PRIMARY KEY (idDetalleLiquidacion),
    CONSTRAINT Fk_DetalleLiquidacion_Liquidacion FOREIGN KEY (idLiquidacion)
        REFERENCES tblLiquidacion (idLiquidacion) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT Fk_DetalleLiquidacion_PercepcionDeduccion FOREIGN KEY (idPercepcionDeduccion)
        REFERENCES tblPercepcionDeduccion (idPercepcionDeduccion) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT uqDetalleLiquidacionConcepto UNIQUE (idLiquidacion, idPercepcionDeduccion),
    CONSTRAINT ckDetalleLiquidacionMonto CHECK (montoDetalleLiquidacion >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;





-- ---------- 1. Nómina: detalle y encabezado ----------
-- Inicio de cambio: Cristian Sipac - 9959-23-1567 - 31/10/2026

ALTER TABLE tblNominaDetalle
    ADD COLUMN montoNominaDetalle DECIMAL(12,4) NOT NULL DEFAULT 0.0000
    AFTER idPercepcionDeduccion;

ALTER TABLE tblNominaEncabezado
    ADD COLUMN totalHorasExtraNominaEncabezado DECIMAL(12,4) NOT NULL DEFAULT 0.0000
    AFTER duracionHoraExtraNominaEncabezado;

ALTER TABLE tblNominaDetalle
    ADD CONSTRAINT uqNominaDetalleConcepto UNIQUE (idNominaEncabezado, idPercepcionDeduccion);

-- Fin de cambio: Cristian Sipac - 9959-23-1567 - 31/10/2026

-- ---------- 2. Capacitación: instructor ----------
-- Inicio de cambio: Carlos Calderon - 9959-23-848 - 31/10/2026

ALTER TABLE tblCapacitacion
    ADD COLUMN idInstructor INT NULL AFTER idCapacitacion;

ALTER TABLE tblCapacitacion
    ADD CONSTRAINT Fk_Capacitacion_Instructor FOREIGN KEY (idInstructor)
        REFERENCES tblInstructor (idInstructor) ON DELETE RESTRICT ON UPDATE CASCADE;

-- Fin de cambio: Carlos Calderon - 9959-23-848 - 31/10/2026




-- ---------- 4. Cierre de nómina (un registro por período cerrado) ----------

CREATE TABLE tblCierreNomina (
    idCierreNomina INT AUTO_INCREMENT NOT NULL,
    periodoCierreNomina CHAR(7) NOT NULL COMMENT 'AAAA-MM, mismo formato del período contable',
    fechaInicioCierreNomina DATE NOT NULL COMMENT 'Primer día del mes cerrado',
    fechaFinalCierreNomina DATE NOT NULL COMMENT 'Último día del mes cerrado',
    cantidadEmpleadoCierreNomina INT NOT NULL DEFAULT 0,
    totalPercepcionCierreNomina DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalDeduccionCierreNomina DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalNetoCierreNomina DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalPatronalCierreNomina DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalDebeCierreNomina DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalHaberCierreNomina DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    documentoOrigenCierreNomina VARCHAR(30) NOT NULL COMMENT 'Documento origen enviado a Contabilidad (máx. 30, igual que tblPolizaEncabezado; ej. NOM-2026-10 o NOM-2026-10-Q2)',
    codigoPolizaCierreNomina VARCHAR(30) NOT NULL COMMENT 'CodigoPoliza devuelto por Contabilidad',
    fechaPolizaCierreNomina DATE NOT NULL,
    estadoPolizaCierreNomina VARCHAR(30) NOT NULL COMMENT 'Estado devuelto por Contabilidad (BORRADOR, VALIDADA, APLICADA o ANULADA)',
    idUsuarioCierra INT NOT NULL,
    fechaEjecucionCierreNomina DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_CierreNomina PRIMARY KEY (idCierreNomina),
    CONSTRAINT Fk_CierreNomina_Usuario FOREIGN KEY (idUsuarioCierra)
        REFERENCES tblUsuario (idUsuario) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT uqCierreNominaPeriodo UNIQUE (periodoCierreNomina),
    CONSTRAINT uqCierreNominaDocumentoOrigen UNIQUE (documentoOrigenCierreNomina),
    CONSTRAINT uqCierreNominaCodigoPoliza UNIQUE (codigoPolizaCierreNomina),
    CONSTRAINT ckCierreNominaFechas CHECK (fechaFinalCierreNomina >= fechaInicioCierreNomina),
    -- Contabilidad rechaza pólizas descuadradas; aquí tampoco se permite guardarlas
    CONSTRAINT ckCierreNominaCuadre CHECK (totalDebeCierreNomina = totalHaberCierreNomina)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------- 5. Histórico por empleado del cierre ----------
-- Foto de los totales de cada empleado al momento de cerrar (lo que muestra la grilla).
CREATE TABLE tblCierreNominaEmpleado (
    idCierreNominaEmpleado INT AUTO_INCREMENT NOT NULL,
    idCierreNomina INT NOT NULL,
    idNominaEncabezado INT NOT NULL,
    idEmpleado INT NOT NULL,
    salarioBaseCierreNominaEmpleado DECIMAL(12,4) NOT NULL,
    totalPercepcionCierreNominaEmpleado DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalDeduccionCierreNominaEmpleado DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalNetoCierreNominaEmpleado DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    acumuladoPercepcionAnioCierreNominaEmpleado DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    acumuladoDeduccionAnioCierreNominaEmpleado DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_CierreNominaEmpleado PRIMARY KEY (idCierreNominaEmpleado),
    CONSTRAINT Fk_CierreNominaEmpleado_CierreNomina FOREIGN KEY (idCierreNomina)
        REFERENCES tblCierreNomina (idCierreNomina) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_CierreNominaEmpleado_NominaEncabezado FOREIGN KEY (idNominaEncabezado)
        REFERENCES tblNominaEncabezado (idNominaEncabezado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_CierreNominaEmpleado_Empleado FOREIGN KEY (idEmpleado)
        REFERENCES tblEmpleado (idEmpleado) ON DELETE RESTRICT ON UPDATE CASCADE,
    -- Una nómina de empleado solo puede cerrarse una vez (también sirve para bloquear su edición)
    CONSTRAINT uqCierreNominaEmpleadoEncabezado UNIQUE (idNominaEncabezado),
    CONSTRAINT ckCierreNominaEmpleadoNeto CHECK
        (totalNetoCierreNominaEmpleado = totalPercepcionCierreNominaEmpleado - totalDeduccionCierreNominaEmpleado),
    KEY idxCierreNominaEmpleadoCierre (idCierreNomina)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
-- Fin de cambio: Isabel Meléndez - 9959-23-1379 - 31/10/2026


-- Inicio de Cambios Daniella Jurado 0901-23-3679

-- ---------- 6. Evaluación de desempeño (simplificada) ----------
-- Se borra primero la tabla hija que depende de tblEvaluacionDesempeno y tblCriterioEvaluacion
DROP TABLE tblDetalleEvaluacionDesempeno;

ALTER TABLE tblEvaluacionDesempeno
    DROP FOREIGN KEY Fk_EvaluacionDesempeno_CicloEvaluacion,
    DROP FOREIGN KEY Fk_EvaluacionDesempeno_NivelDesempeno,
    DROP FOREIGN KEY Fk_EvaluacionDesempeno_PlanMejora,
    DROP CHECK ckEvaluacionDesempenoPorcentaje,
    DROP INDEX uqEvaluacionDesempenoEmpleadoCiclo,
    ADD INDEX idxEvaluacionDesempenoEmpleado (idEmpleado),
    DROP COLUMN idCicloEvaluacion,
    DROP COLUMN idNivelDesempeno,
    DROP COLUMN idPlanMejora,
    DROP COLUMN porcentajeEvaluacionDesempeno,
    DROP COLUMN retroalimentacionEvaluacionDesempeno,
    DROP COLUMN estadoEvaluacionDesempeno,
    ADD COLUMN calidadEvaluacionDesempeno INT NOT NULL DEFAULT 1 AFTER fechaEvaluacionDesempeno,
    ADD COLUMN productividadEvaluacionDesempeno INT NOT NULL DEFAULT 1 AFTER calidadEvaluacionDesempeno,
    ADD COLUMN trabajoEquipoEvaluacionDesempeno INT NOT NULL DEFAULT 1 AFTER productividadEvaluacionDesempeno,
    ADD COLUMN puntualidadEvaluacionDesempeno INT NOT NULL DEFAULT 1 AFTER trabajoEquipoEvaluacionDesempeno,
    ADD COLUMN iniciativaEvaluacionDesempeno INT NOT NULL DEFAULT 1 AFTER puntualidadEvaluacionDesempeno,
    MODIFY COLUMN puntajeEvaluacionDesempeno DECIMAL(4,2) NULL DEFAULT 0 AFTER iniciativaEvaluacionDesempeno,
    ADD CONSTRAINT ckEvaluacionDesempenoNotas CHECK (
        calidadEvaluacionDesempeno BETWEEN 1 AND 5 AND productividadEvaluacionDesempeno BETWEEN 1 AND 5 AND
        trabajoEquipoEvaluacionDesempeno BETWEEN 1 AND 5 AND puntualidadEvaluacionDesempeno BETWEEN 1 AND 5 AND
        iniciativaEvaluacionDesempeno BETWEEN 1 AND 5);

-- Quitar el DEFAULT temporal: las notas deben ingresarse siempre
ALTER TABLE tblEvaluacionDesempeno
    ALTER COLUMN calidadEvaluacionDesempeno DROP DEFAULT,
    ALTER COLUMN productividadEvaluacionDesempeno DROP DEFAULT,
    ALTER COLUMN trabajoEquipoEvaluacionDesempeno DROP DEFAULT,
    ALTER COLUMN puntualidadEvaluacionDesempeno DROP DEFAULT,
    ALTER COLUMN iniciativaEvaluacionDesempeno DROP DEFAULT;

-- El plan de mejora ahora apunta a la evaluación (relación invertida)
ALTER TABLE tblPlanMejora
    ADD COLUMN idEvaluacionDesempeno INT NULL AFTER idPlanMejora,
    ADD CONSTRAINT uqPlanMejoraEvaluacion UNIQUE (idEvaluacionDesempeno),
    ADD CONSTRAINT Fk_PlanMejora_EvaluacionDesempeno FOREIGN KEY (idEvaluacionDesempeno)
        REFERENCES tblEvaluacionDesempeno (idEvaluacionDesempeno) ON DELETE SET NULL ON UPDATE CASCADE;

DROP TABLE tblCriterioEvaluacion, tblCicloEvaluacion, tblNivelDesempeno;

-- fin de cambios Daniella Jurado 0901-23-3679


-- ---------- 7. Permisos y vacaciones ----------
-- Inicio de cambio: Isabel Meléndez - 9959-23-1379 - 07/11/2026
--
ALTER TABLE tblPermisoEmpleado
    ADD COLUMN conGoceSueldoPermisoEmpleado BOOLEAN NOT NULL DEFAULT TRUE AFTER motivoPermisoEmpleado,
    MODIFY COLUMN totalPermisoEmpleado DECIMAL(6,2) NULL DEFAULT 0;

-- Fin de cambio: Isabel Meléndez - 9959-23-1379 - 07/11/2026


-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡ Bancos !!!!!!!!!!!!!!!!!!!!!!!!!-------------

CREATE TABLE tblMoneda (
    Pk_Moneda BIGINT NOT NULL AUTO_INCREMENT,
    codigoMoneda CHAR(3) NOT NULL,
    nombreMoneda VARCHAR(255) NOT NULL,
    simboloMoneda VARCHAR(8) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Moneda),
    CONSTRAINT uqMoneda1 UNIQUE (codigoMoneda),
    CONSTRAINT ckMoneda1 CHECK (is_active IN (0,1))
) ENGINE=InnoDB;

CREATE TABLE tblTipoCambio (
    Pk_TipoCambio BIGINT NOT NULL AUTO_INCREMENT,
    fechaTipoCambio DATE NOT NULL,
    valorTipoCambio DECIMAL(12,4) NOT NULL,
    fuenteTipoCambio VARCHAR(100) NOT NULL,
    versionTipoCambio INT NOT NULL DEFAULT 1,
    cotizacionOriginalTipoCambio TEXT NULL,
    metodoTipoCambio VARCHAR(30) NOT NULL DEFAULT 'DIRECTA',
    motivoRevisionTipoCambio VARCHAR(255) NULL,
    Fk_Moneda_Origen_Codigo BIGINT NOT NULL,
    Fk_Moneda_Destino_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_TipoCambio),
    CONSTRAINT uqTipoCambio1 UNIQUE (Fk_Moneda_Origen_Codigo,Fk_Moneda_Destino_Codigo,fechaTipoCambio,fuenteTipoCambio,versionTipoCambio),
    CONSTRAINT ckTipoCambio1 CHECK (valorTipoCambio > 0),
    CONSTRAINT ckTipoCambio2 CHECK (versionTipoCambio > 0),
    CONSTRAINT ckTipoCambio3 CHECK (metodoTipoCambio IN ('DIRECTA','INVERSA','CRUZADA')),
    CONSTRAINT ckTipoCambio4 CHECK (Fk_Moneda_Origen_Codigo <> Fk_Moneda_Destino_Codigo),
    INDEX idxTipoCambioMonedaDestino (Fk_Moneda_Destino_Codigo),
    CONSTRAINT Fk_TipoCambio_Moneda_Origen FOREIGN KEY (Fk_Moneda_Origen_Codigo) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_TipoCambio_Moneda_Destino FOREIGN KEY (Fk_Moneda_Destino_Codigo) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblBanco (
    Pk_Banco BIGINT NOT NULL AUTO_INCREMENT,
    codigoBanco VARCHAR(30) NOT NULL,
    nombreBanco VARCHAR(100) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Banco),
    CONSTRAINT uqBanco1 UNIQUE (codigoBanco),
    CONSTRAINT ckBanco1 CHECK (is_active IN (0,1))
) ENGINE=InnoDB;

CREATE TABLE tblTipoCuentaBancaria (
    Pk_TipoCuentaBancaria BIGINT NOT NULL AUTO_INCREMENT,
    codigoTipoCuentaBancaria VARCHAR(30) NOT NULL,
    nombreTipoCuentaBancaria VARCHAR(255) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_TipoCuentaBancaria),
    CONSTRAINT uqTipoCuentaBancaria1 UNIQUE (codigoTipoCuentaBancaria),
    CONSTRAINT ckTipoCuentaBancaria1 CHECK (is_active IN (0,1))
) ENGINE=InnoDB;

CREATE TABLE tblTipoTransaccion (
    Pk_TipoTransaccion BIGINT NOT NULL AUTO_INCREMENT,
    codigoTipoTransaccion VARCHAR(30) NOT NULL,
    nombreTipoTransaccion VARCHAR(255) NOT NULL,
    naturalezaTipoTransaccion VARCHAR(30) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_TipoTransaccion),
    CONSTRAINT uqTipoTransaccion1 UNIQUE (codigoTipoTransaccion),
    CONSTRAINT ckTipoTransaccion1 CHECK (naturalezaTipoTransaccion IN ('INGRESO','EGRESO','AMBOS')),
    CONSTRAINT ckTipoTransaccion2 CHECK (is_active IN (0,1))
) ENGINE=InnoDB;

CREATE TABLE tblCuentaBancaria (
    Pk_CuentaBancaria BIGINT NOT NULL AUTO_INCREMENT,
    numeroCuentaBancaria VARCHAR(100) NOT NULL,
    nombreCuentaBancaria VARCHAR(255) NOT NULL,
    codigoCuentaContableCuentaBancaria VARCHAR(30) NULL,
    limiteSobregiroCuentaBancaria DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    Fk_Banco_Codigo BIGINT NOT NULL,
    Fk_Moneda_Codigo BIGINT NOT NULL,
    Fk_TipoCuentaBancaria_Codigo BIGINT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_CuentaBancaria),
    CONSTRAINT uqCuentaBancaria1 UNIQUE (Fk_Banco_Codigo,numeroCuentaBancaria),
    CONSTRAINT ckCuentaBancaria1 CHECK (limiteSobregiroCuentaBancaria >= 0),
    CONSTRAINT ckCuentaBancaria2 CHECK (is_active IN (0,1)),
    INDEX idxCuentaBancariaMoneda (Fk_Moneda_Codigo),
    INDEX idxCuentaBancariaTipo (Fk_TipoCuentaBancaria_Codigo),
    CONSTRAINT Fk_CuentaBancaria_Banco FOREIGN KEY (Fk_Banco_Codigo) REFERENCES tblBanco(Pk_Banco) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_CuentaBancaria_Moneda FOREIGN KEY (Fk_Moneda_Codigo) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_CuentaBancaria_Tipo FOREIGN KEY (Fk_TipoCuentaBancaria_Codigo) REFERENCES tblTipoCuentaBancaria(Pk_TipoCuentaBancaria) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;


CREATE TABLE tblDisponibilidadDiaria (
    Pk_DisponibilidadDiaria BIGINT NOT NULL AUTO_INCREMENT,
    fechaDisponibilidadDiaria DATE NOT NULL,
    saldoInicialDisponibilidadDiaria DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    ingresosDisponibilidadDiaria DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    egresosDisponibilidadDiaria DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    comprometidoDisponibilidadDiaria DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    restringidoDisponibilidadDiaria DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    saldoDisponibleDisponibilidadDiaria DECIMAL(12,4) NOT NULL,
    saldoDisponibleGTQDisponibilidadDiaria DECIMAL(12,4) NOT NULL,
    tipoCambioAplicadoDisponibilidadDiaria DECIMAL(12,4) NOT NULL DEFAULT 1.0000,
    fuenteDisponibilidadDiaria VARCHAR(100) NOT NULL,
    fechaGeneracionDisponibilidadDiaria DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Fk_CuentaBancaria_Codigo BIGINT NOT NULL,
    Fk_TipoCambio_Codigo BIGINT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_DisponibilidadDiaria),
    CONSTRAINT uqDisponibilidadDiaria1 UNIQUE (Fk_CuentaBancaria_Codigo,fechaDisponibilidadDiaria),
    CONSTRAINT ckDisponibilidadDiaria1 CHECK (saldoInicialDisponibilidadDiaria >= 0),
    CONSTRAINT ckDisponibilidadDiaria2 CHECK (ingresosDisponibilidadDiaria >= 0 AND egresosDisponibilidadDiaria >= 0 AND comprometidoDisponibilidadDiaria >= 0),
    CONSTRAINT ckDisponibilidadDiaria4 CHECK (restringidoDisponibilidadDiaria >= 0),
    CONSTRAINT ckDisponibilidadDiaria3 CHECK (tipoCambioAplicadoDisponibilidadDiaria > 0),
    INDEX idxDisponibilidadDiariaTipoCambio (Fk_TipoCambio_Codigo),
    CONSTRAINT Fk_DisponibilidadDiaria_Cuenta FOREIGN KEY (Fk_CuentaBancaria_Codigo) REFERENCES tblCuentaBancaria(Pk_CuentaBancaria) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_DisponibilidadDiaria_TipoCambio FOREIGN KEY (Fk_TipoCambio_Codigo) REFERENCES tblTipoCambio(Pk_TipoCambio) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblPoliticaAprobacion (
    Pk_PoliticaAprobacion BIGINT NOT NULL AUTO_INCREMENT,
    nombrePoliticaAprobacion VARCHAR(255) NOT NULL,
    tipoOperacionPoliticaAprobacion VARCHAR(30) NOT NULL,
    montoMinimoPoliticaAprobacion DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    montoMaximoPoliticaAprobacion DECIMAL(12,4) NOT NULL,
    cantidadAprobacionesPoliticaAprobacion INT NOT NULL DEFAULT 1,
    fechaInicioPoliticaAprobacion DATE NOT NULL,
    fechaFinPoliticaAprobacion DATE NULL,
    Fk_Moneda_Codigo BIGINT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_PoliticaAprobacion),
    CONSTRAINT ckPoliticaAprobacion1 CHECK (tipoOperacionPoliticaAprobacion IN ('ORDEN_COMPRA','ORDEN_PAGO')),
    CONSTRAINT ckPoliticaAprobacion2 CHECK (montoMinimoPoliticaAprobacion >= 0 AND montoMaximoPoliticaAprobacion >= montoMinimoPoliticaAprobacion),
    CONSTRAINT ckPoliticaAprobacion3 CHECK (cantidadAprobacionesPoliticaAprobacion > 0),
    CONSTRAINT ckPoliticaAprobacion4 CHECK (fechaFinPoliticaAprobacion IS NULL OR fechaFinPoliticaAprobacion >= fechaInicioPoliticaAprobacion),
    CONSTRAINT ckPoliticaAprobacion5 CHECK (is_active IN (0,1)),
    INDEX idxPoliticaAprobacionMoneda (Fk_Moneda_Codigo),
    CONSTRAINT Fk_PoliticaAprobacion_Moneda FOREIGN KEY (Fk_Moneda_Codigo) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;


CREATE TABLE tblOrdenCompraRecibida (
    Pk_OrdenCompraRecibida BIGINT NOT NULL AUTO_INCREMENT,
    referenciaExternaOrdenCompraRecibida VARCHAR(100) NOT NULL,
    proveedorOrdenCompraRecibida VARCHAR(100) NOT NULL,
    fechaOrdenCompraRecibida DATE NOT NULL,
    fechaVencimientoOrdenCompraRecibida DATE NOT NULL,
    importeOrdenCompraRecibida DECIMAL(12,4) NOT NULL,
    estadoOrdenCompraRecibida VARCHAR(30) NOT NULL DEFAULT 'PENDIENTE',
    referenciaDocumentoOrdenCompraRecibida VARCHAR(500) NULL,
    identificadorSolicitanteOrdenCompraRecibida VARCHAR(100) NULL,
    Fk_Moneda_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_OrdenCompraRecibida),
    CONSTRAINT uqOrdenCompraRecibida1 UNIQUE (referenciaExternaOrdenCompraRecibida),
    CONSTRAINT ckOrdenCompraRecibida1 CHECK (importeOrdenCompraRecibida > 0),
    CONSTRAINT ckOrdenCompraRecibida2 CHECK (fechaVencimientoOrdenCompraRecibida >= fechaOrdenCompraRecibida),
    CONSTRAINT ckOrdenCompraRecibida3 CHECK (estadoOrdenCompraRecibida IN ('PENDIENTE','APROBADA','RECHAZADA','PAGADA')),
    INDEX idxOrdenCompraRecibidaMoneda (Fk_Moneda_Codigo),
    CONSTRAINT Fk_OrdenCompraRecibida_Moneda FOREIGN KEY (Fk_Moneda_Codigo) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblPlanillaRecibida (
    Pk_PlanillaRecibida BIGINT NOT NULL AUTO_INCREMENT,
    referenciaExternaPlanillaRecibida VARCHAR(100) NOT NULL,
    inicioPeriodoPlanillaRecibida DATE NOT NULL,
    finPeriodoPlanillaRecibida DATE NOT NULL,
    estadoPlanillaRecibida VARCHAR(30) NOT NULL DEFAULT 'RECIBIDA',
    referenciaDocumentoPlanillaRecibida VARCHAR(500) NULL,
    Fk_Moneda_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_PlanillaRecibida),
    CONSTRAINT uqPlanillaRecibida1 UNIQUE (referenciaExternaPlanillaRecibida),
    CONSTRAINT ckPlanillaRecibida1 CHECK (finPeriodoPlanillaRecibida >= inicioPeriodoPlanillaRecibida),
    CONSTRAINT ckPlanillaRecibida2 CHECK (estadoPlanillaRecibida IN ('RECIBIDA','PROCESADA','ANULADA')),
    INDEX idxPlanillaRecibidaMoneda (Fk_Moneda_Codigo),
    CONSTRAINT Fk_PlanillaRecibida_Moneda FOREIGN KEY (Fk_Moneda_Codigo) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblDetallePlanillaRecibida (
    Pk_DetallePlanillaRecibida BIGINT NOT NULL AUTO_INCREMENT,
    referenciaEmpleadoDetallePlanillaRecibida VARCHAR(100) NOT NULL,
    beneficiarioDetallePlanillaRecibida VARCHAR(100) NOT NULL,
    importeNetoDetallePlanillaRecibida DECIMAL(12,4) NOT NULL,
    Fk_PlanillaRecibida_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_DetallePlanillaRecibida),
    CONSTRAINT uqDetallePlanillaRecibida1 UNIQUE (Fk_PlanillaRecibida_Codigo,referenciaEmpleadoDetallePlanillaRecibida),
    CONSTRAINT ckDetallePlanillaRecibida1 CHECK (importeNetoDetallePlanillaRecibida > 0),
    CONSTRAINT Fk_DetallePlanillaRecibida_Planilla FOREIGN KEY (Fk_PlanillaRecibida_Codigo) REFERENCES tblPlanillaRecibida(Pk_PlanillaRecibida) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblOrdenPago (
    Pk_OrdenPago BIGINT NOT NULL AUTO_INCREMENT,
    referenciaOrdenPago VARCHAR(30) NOT NULL,
    beneficiarioOrdenPago VARCHAR(100) NOT NULL,
    importeOrdenPago DECIMAL(12,4) NOT NULL,
    medioOrdenPago VARCHAR(30) NOT NULL,
    estadoOrdenPago VARCHAR(30) NOT NULL DEFAULT 'PENDIENTE',
    fechaProgramadaOrdenPago DATE NOT NULL,
    identificadorSolicitanteOrdenPago VARCHAR(100) NULL,
    Fk_CuentaBancaria_Codigo BIGINT NOT NULL,
    Fk_Moneda_Codigo BIGINT NOT NULL,
    Fk_OrdenCompraRecibida_Codigo BIGINT NULL,
    Fk_DetallePlanillaRecibida_Codigo BIGINT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_OrdenPago),
    CONSTRAINT uqOrdenPago1 UNIQUE (referenciaOrdenPago),
    CONSTRAINT ckOrdenPago1 CHECK (importeOrdenPago > 0),
    CONSTRAINT ckOrdenPago2 CHECK (medioOrdenPago IN ('CHEQUE','TRANSFERENCIA')),
    CONSTRAINT ckOrdenPago3 CHECK (estadoOrdenPago IN ('PENDIENTE','AUTORIZADA','RECHAZADA','EMITIDA','PAGADA','ANULADA')),
    CONSTRAINT ckOrdenPago4 CHECK ((Fk_OrdenCompraRecibida_Codigo IS NOT NULL) + (Fk_DetallePlanillaRecibida_Codigo IS NOT NULL) = 1),
    CONSTRAINT ckOrdenPago5 CHECK (Fk_DetallePlanillaRecibida_Codigo IS NULL OR medioOrdenPago = 'CHEQUE'),
    INDEX idxOrdenPagoCuenta (Fk_CuentaBancaria_Codigo),
    INDEX idxOrdenPagoMoneda (Fk_Moneda_Codigo),
    INDEX idxOrdenPagoCompra (Fk_OrdenCompraRecibida_Codigo),
    INDEX idxOrdenPagoPlanilla (Fk_DetallePlanillaRecibida_Codigo),
    CONSTRAINT Fk_OrdenPago_Cuenta FOREIGN KEY (Fk_CuentaBancaria_Codigo) REFERENCES tblCuentaBancaria(Pk_CuentaBancaria) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_OrdenPago_Moneda FOREIGN KEY (Fk_Moneda_Codigo) REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_OrdenPago_Compra FOREIGN KEY (Fk_OrdenCompraRecibida_Codigo) REFERENCES tblOrdenCompraRecibida(Pk_OrdenCompraRecibida) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_OrdenPago_Planilla FOREIGN KEY (Fk_DetallePlanillaRecibida_Codigo) REFERENCES tblDetallePlanillaRecibida(Pk_DetallePlanillaRecibida) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblAprobacion (
    Pk_Aprobacion BIGINT NOT NULL AUTO_INCREMENT,
    decisionAprobacion VARCHAR(30) NOT NULL,
    comentarioAprobacion VARCHAR(255) NULL,
    identificadorAprobadorAprobacion VARCHAR(100) NOT NULL,
    referenciaEvidencia2FAAprobacion VARCHAR(255) NOT NULL,
    fechaValidacion2FAAprobacion DATETIME NOT NULL,
    Fk_OrdenCompraRecibida_Codigo BIGINT NULL,
    Fk_OrdenPago_Codigo BIGINT NULL,
    Fk_PoliticaAprobacion_Codigo BIGINT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Aprobacion),
    CONSTRAINT ckAprobacion1 CHECK (decisionAprobacion IN ('APROBADA','RECHAZADA')),
    CONSTRAINT ckAprobacion2 CHECK ((Fk_OrdenCompraRecibida_Codigo IS NOT NULL) + (Fk_OrdenPago_Codigo IS NOT NULL) = 1),
    INDEX idxAprobacionOrdenCompra (Fk_OrdenCompraRecibida_Codigo),
    INDEX idxAprobacionOrdenPago (Fk_OrdenPago_Codigo),
    INDEX idxAprobacionPolitica (Fk_PoliticaAprobacion_Codigo),
    CONSTRAINT Fk_Aprobacion_OrdenCompra FOREIGN KEY (Fk_OrdenCompraRecibida_Codigo) REFERENCES tblOrdenCompraRecibida(Pk_OrdenCompraRecibida) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_Aprobacion_OrdenPago FOREIGN KEY (Fk_OrdenPago_Codigo) REFERENCES tblOrdenPago(Pk_OrdenPago) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_Aprobacion_Politica FOREIGN KEY (Fk_PoliticaAprobacion_Codigo) REFERENCES tblPoliticaAprobacion(Pk_PoliticaAprobacion) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblChequera (
    Pk_Chequera BIGINT NOT NULL AUTO_INCREMENT,
    serieChequera VARCHAR(30) NOT NULL,
    numeroInicialChequera BIGINT NOT NULL,
    numeroFinalChequera BIGINT NOT NULL,
    siguienteNumeroChequera BIGINT NOT NULL,
    Fk_CuentaBancaria_Codigo BIGINT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Chequera),
    CONSTRAINT uqChequera1 UNIQUE (Fk_CuentaBancaria_Codigo,serieChequera),
    CONSTRAINT ckChequera1 CHECK (numeroInicialChequera > 0),
    CONSTRAINT ckChequera2 CHECK (numeroFinalChequera >= numeroInicialChequera),
    CONSTRAINT ckChequera3 CHECK (siguienteNumeroChequera BETWEEN numeroInicialChequera AND numeroFinalChequera + 1),
    CONSTRAINT ckChequera4 CHECK (is_active IN (0,1)),
    CONSTRAINT Fk_Chequera_Cuenta FOREIGN KEY (Fk_CuentaBancaria_Codigo) REFERENCES tblCuentaBancaria(Pk_CuentaBancaria) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblCheque (
    Pk_Cheque BIGINT NOT NULL AUTO_INCREMENT,
    numeroCheque BIGINT NOT NULL,
    beneficiarioCheque VARCHAR(100) NOT NULL,
    estadoCheque VARCHAR(30) NOT NULL DEFAULT 'GENERADO',
    referenciaEvidencia2FACheque VARCHAR(255) NOT NULL,
    fechaValidacion2FACheque DATETIME NOT NULL,
    identificadorGeneradorCheque VARCHAR(100) NOT NULL,
    Fk_Chequera_Codigo BIGINT NOT NULL,
    Fk_OrdenPago_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Cheque),
    CONSTRAINT uqCheque1 UNIQUE (Fk_Chequera_Codigo,numeroCheque),
    CONSTRAINT uqCheque2 UNIQUE (Fk_OrdenPago_Codigo),
    CONSTRAINT ckCheque1 CHECK (estadoCheque IN ('GENERADO','ENTREGADO','COBRADO','ANULADO')),
    CONSTRAINT Fk_Cheque_Chequera FOREIGN KEY (Fk_Chequera_Codigo) REFERENCES tblChequera(Pk_Chequera) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_Cheque_OrdenPago FOREIGN KEY (Fk_OrdenPago_Codigo) REFERENCES tblOrdenPago(Pk_OrdenPago) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;


CREATE TABLE tblMovimientoBancario (
    Pk_MovimientoBancario BIGINT NOT NULL AUTO_INCREMENT,
    referenciaMovimientoBancario VARCHAR(100) NULL,
    conceptoMovimientoBancario VARCHAR(255) NOT NULL,
    importeMovimientoBancario DECIMAL(12,4) NOT NULL,
    importeGTQMovimientoBancario DECIMAL(12,4) NOT NULL,
    tipoCambioAplicadoMovimientoBancario DECIMAL(12,4) NOT NULL DEFAULT 1.0000,
    sentidoMovimientoBancario INT NOT NULL,
    fechaOperacionMovimientoBancario DATE NOT NULL,
    fechaValorMovimientoBancario DATE NOT NULL,
    origenMovimientoBancario VARCHAR(30) NOT NULL DEFAULT 'MANUAL',
    Fk_CuentaBancaria_Codigo BIGINT NOT NULL,
    Fk_TipoTransaccion_Codigo BIGINT NOT NULL,
    Fk_TipoCambio_Codigo BIGINT NULL,
    Fk_OrdenPago_Codigo BIGINT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_MovimientoBancario),
    CONSTRAINT uqMovimientoBancario1 UNIQUE (Fk_OrdenPago_Codigo),
    CONSTRAINT ckMovimientoBancario1 CHECK (importeMovimientoBancario > 0 AND importeGTQMovimientoBancario > 0),
    CONSTRAINT ckMovimientoBancario2 CHECK (tipoCambioAplicadoMovimientoBancario > 0),
    CONSTRAINT ckMovimientoBancario3 CHECK (sentidoMovimientoBancario IN (-1,1)),
    CONSTRAINT ckMovimientoBancario4 CHECK (origenMovimientoBancario IN ('MANUAL','ESTADO_CUENTA','ORDEN_PAGO')),
    INDEX idxMovimientoBancarioCuentaFecha (Fk_CuentaBancaria_Codigo,fechaOperacionMovimientoBancario),
    INDEX idxMovimientoBancarioTipo (Fk_TipoTransaccion_Codigo),
    INDEX idxMovimientoBancarioTipoCambio (Fk_TipoCambio_Codigo),
    CONSTRAINT Fk_MovimientoBancario_Cuenta FOREIGN KEY (Fk_CuentaBancaria_Codigo) REFERENCES tblCuentaBancaria(Pk_CuentaBancaria) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_MovimientoBancario_Tipo FOREIGN KEY (Fk_TipoTransaccion_Codigo) REFERENCES tblTipoTransaccion(Pk_TipoTransaccion) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_MovimientoBancario_TipoCambio FOREIGN KEY (Fk_TipoCambio_Codigo) REFERENCES tblTipoCambio(Pk_TipoCambio) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_MovimientoBancario_OrdenPago FOREIGN KEY (Fk_OrdenPago_Codigo) REFERENCES tblOrdenPago(Pk_OrdenPago) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblExtractoBancario (
    Pk_ExtractoBancario BIGINT NOT NULL AUTO_INCREMENT,
    referenciaExtractoBancario VARCHAR(100) NOT NULL,
    fechaInicioExtractoBancario DATE NOT NULL,
    fechaFinExtractoBancario DATE NOT NULL,
    saldoInicialExtractoBancario DECIMAL(12,4) NOT NULL,
    saldoFinalExtractoBancario DECIMAL(12,4) NOT NULL,
    huellaExtractoBancario CHAR(64) NULL,
    fechaImportacionExtractoBancario DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Fk_CuentaBancaria_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_ExtractoBancario),
    CONSTRAINT uqExtractoBancario1 UNIQUE (Fk_CuentaBancaria_Codigo,referenciaExtractoBancario),
    CONSTRAINT uqExtractoBancario2 UNIQUE (Fk_CuentaBancaria_Codigo,huellaExtractoBancario),
    CONSTRAINT ckExtractoBancario1 CHECK (fechaFinExtractoBancario >= fechaInicioExtractoBancario),
    CONSTRAINT Fk_ExtractoBancario_Cuenta FOREIGN KEY (Fk_CuentaBancaria_Codigo) REFERENCES tblCuentaBancaria(Pk_CuentaBancaria) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblDetalleExtracto (
    Pk_DetalleExtracto BIGINT NOT NULL AUTO_INCREMENT,
    identificadorDetalleExtracto VARCHAR(100) NOT NULL,
    referenciaDetalleExtracto VARCHAR(100) NULL,
    conceptoDetalleExtracto VARCHAR(255) NOT NULL,
    fechaOperacionDetalleExtracto DATE NOT NULL,
    fechaValorDetalleExtracto DATE NOT NULL,
    importeDetalleExtracto DECIMAL(12,4) NOT NULL,
    sentidoDetalleExtracto INT NOT NULL,
    Fk_ExtractoBancario_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_DetalleExtracto),
    CONSTRAINT uqDetalleExtracto1 UNIQUE (Fk_ExtractoBancario_Codigo,identificadorDetalleExtracto),
    CONSTRAINT ckDetalleExtracto1 CHECK (importeDetalleExtracto > 0),
    CONSTRAINT ckDetalleExtracto2 CHECK (sentidoDetalleExtracto IN (-1,1)),
    CONSTRAINT Fk_DetalleExtracto_Extracto FOREIGN KEY (Fk_ExtractoBancario_Codigo) REFERENCES tblExtractoBancario(Pk_ExtractoBancario) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblConciliacion (
    Pk_Conciliacion BIGINT NOT NULL AUTO_INCREMENT,
    fechaCorteConciliacion DATE NOT NULL,
    estadoConciliacion VARCHAR(30) NOT NULL DEFAULT 'ABIERTA',
    metodoConciliacion VARCHAR(30) NOT NULL DEFAULT 'AUTOMATICA',
    saldoLibroConciliacion DECIMAL(12,4) NULL,
    saldoBancoConciliacion DECIMAL(12,4) NULL,
    diferenciaConciliacion DECIMAL(12,4) NULL,
    identificadorResponsableConciliacion VARCHAR(100) NULL,
    fechaCierreConciliacion DATETIME NULL,
    Fk_CuentaBancaria_Codigo BIGINT NOT NULL,
    Fk_ExtractoBancario_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_Conciliacion),
    CONSTRAINT uqConciliacion1 UNIQUE (Fk_CuentaBancaria_Codigo,fechaCorteConciliacion),
    CONSTRAINT ckConciliacion1 CHECK (estadoConciliacion IN ('ABIERTA','PROCESANDO','CONCILIADA','CON_DIFERENCIAS','CERRADA')),
    CONSTRAINT ckConciliacion2 CHECK (metodoConciliacion IN ('AUTOMATICA','MANUAL','MIXTA')),
    INDEX idxConciliacionExtracto (Fk_ExtractoBancario_Codigo),
    CONSTRAINT Fk_Conciliacion_Cuenta FOREIGN KEY (Fk_CuentaBancaria_Codigo) REFERENCES tblCuentaBancaria(Pk_CuentaBancaria) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_Conciliacion_Extracto FOREIGN KEY (Fk_ExtractoBancario_Codigo) REFERENCES tblExtractoBancario(Pk_ExtractoBancario) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblDetalleConciliacion (
    Pk_DetalleConciliacion BIGINT NOT NULL AUTO_INCREMENT,
    resultadoDetalleConciliacion VARCHAR(30) NOT NULL,
    metodoDetalleConciliacion VARCHAR(30) NOT NULL,
    diferenciaDetalleConciliacion DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    motivoDetalleConciliacion VARCHAR(255) NULL,
    Fk_Conciliacion_Codigo BIGINT NOT NULL,
    Fk_MovimientoBancario_Codigo BIGINT NULL,
    Fk_DetalleExtracto_Codigo BIGINT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_DetalleConciliacion),
    CONSTRAINT uqDetalleConciliacion1 UNIQUE (Fk_Conciliacion_Codigo,Fk_MovimientoBancario_Codigo),
    CONSTRAINT uqDetalleConciliacion2 UNIQUE (Fk_Conciliacion_Codigo,Fk_DetalleExtracto_Codigo),
    CONSTRAINT ckDetalleConciliacion1 CHECK (resultadoDetalleConciliacion IN ('COINCIDE','PENDIENTE','DIFERENCIA')),
    CONSTRAINT ckDetalleConciliacion2 CHECK (metodoDetalleConciliacion IN ('AUTOMATICA','MANUAL')),
    CONSTRAINT ckDetalleConciliacion3 CHECK (Fk_MovimientoBancario_Codigo IS NOT NULL OR Fk_DetalleExtracto_Codigo IS NOT NULL),
    INDEX idxDetalleConciliacionMovimiento (Fk_MovimientoBancario_Codigo),
    INDEX idxDetalleConciliacionExtracto (Fk_DetalleExtracto_Codigo),
    CONSTRAINT Fk_DetalleConciliacion_Conciliacion FOREIGN KEY (Fk_Conciliacion_Codigo) REFERENCES tblConciliacion(Pk_Conciliacion) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_DetalleConciliacion_Movimiento FOREIGN KEY (Fk_MovimientoBancario_Codigo) REFERENCES tblMovimientoBancario(Pk_MovimientoBancario) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_DetalleConciliacion_Extracto FOREIGN KEY (Fk_DetalleExtracto_Codigo) REFERENCES tblDetalleExtracto(Pk_DetalleExtracto) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;


CREATE TABLE tblConfiguracionPoliza (
    Pk_ConfiguracionPoliza BIGINT NOT NULL AUTO_INCREMENT,
    codigoCuentaContableBancoConfiguracionPoliza VARCHAR(30) NOT NULL,
    codigoCuentaContableContrapartidaConfiguracionPoliza VARCHAR(30) NOT NULL,
    Fk_CuentaBancaria_Codigo BIGINT NOT NULL,
    Fk_TipoTransaccion_Codigo BIGINT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_ConfiguracionPoliza),
    CONSTRAINT uqConfiguracionPoliza1 UNIQUE (Fk_CuentaBancaria_Codigo,Fk_TipoTransaccion_Codigo),
    CONSTRAINT ckConfiguracionPoliza1 CHECK (is_active IN (0,1)),
    INDEX idxConfiguracionPolizaTipo (Fk_TipoTransaccion_Codigo),
    CONSTRAINT Fk_ConfiguracionPoliza_Cuenta FOREIGN KEY (Fk_CuentaBancaria_Codigo) REFERENCES tblCuentaBancaria(Pk_CuentaBancaria) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_ConfiguracionPoliza_Tipo FOREIGN KEY (Fk_TipoTransaccion_Codigo) REFERENCES tblTipoTransaccion(Pk_TipoTransaccion) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblPolizaBancaria (
    Pk_PolizaBancaria BIGINT NOT NULL AUTO_INCREMENT,
    fechaPolizaBancaria DATE NOT NULL,
    estadoPolizaBancaria VARCHAR(30) NOT NULL DEFAULT 'BORRADOR',
    referenciaContabilidadPolizaBancaria VARCHAR(100) NULL,
    fechaEnvioContabilidadPolizaBancaria DATETIME NULL,
    Fk_MovimientoBancario_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_PolizaBancaria),
    CONSTRAINT uqPolizaBancaria1 UNIQUE (Fk_MovimientoBancario_Codigo),
    CONSTRAINT ckPolizaBancaria1 CHECK (estadoPolizaBancaria IN ('BORRADOR','VALIDADA','PENDIENTE_ENVIO','ENVIADA','ACEPTADA','RECHAZADA','ANULADA')),
    CONSTRAINT Fk_PolizaBancaria_Movimiento FOREIGN KEY (Fk_MovimientoBancario_Codigo) REFERENCES tblMovimientoBancario(Pk_MovimientoBancario) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblDetallePolizaBancaria (
    Pk_DetallePolizaBancaria BIGINT NOT NULL AUTO_INCREMENT,
    codigoCuentaContableDetallePolizaBancaria VARCHAR(30) NOT NULL,
    debeDetallePolizaBancaria DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    haberDetallePolizaBancaria DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    Fk_PolizaBancaria_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_DetallePolizaBancaria),
    CONSTRAINT ckDetallePolizaBancaria1 CHECK ((debeDetallePolizaBancaria > 0 AND haberDetallePolizaBancaria = 0) OR (haberDetallePolizaBancaria > 0 AND debeDetallePolizaBancaria = 0)),
    INDEX idxDetallePolizaBancariaPoliza (Fk_PolizaBancaria_Codigo),
    CONSTRAINT Fk_DetallePolizaBancaria_Poliza FOREIGN KEY (Fk_PolizaBancaria_Codigo) REFERENCES tblPolizaBancaria(Pk_PolizaBancaria) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;


CREATE TABLE tblAutOrdenCompra (
    Pk_AutOrdenCompra BIGINT NOT NULL AUTO_INCREMENT,
    Fk_OrdenCompraRecibida_Codigo BIGINT NOT NULL,
    identificadorEmpresaAutOrdenCompra INT NOT NULL,
    estadoAutOrdenCompra VARCHAR(30) NOT NULL DEFAULT 'PENDIENTE',
    identificadorUsuarioAutorizaAutOrdenCompra INT NULL,
    fechaDecisionAutOrdenCompra DATETIME NULL,
    comentarioAutOrdenCompra VARCHAR(255) NULL,
    motivoRechazoAutOrdenCompra VARCHAR(255) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_AutOrdenCompra),
    CONSTRAINT uqAutOrdenCompra1 UNIQUE (Fk_OrdenCompraRecibida_Codigo),
    CONSTRAINT ckAutOrdenCompra1 CHECK (identificadorEmpresaAutOrdenCompra > 0),
    CONSTRAINT ckAutOrdenCompra2 CHECK (estadoAutOrdenCompra IN ('PENDIENTE','AUTORIZADA','RECHAZADA')),
    CONSTRAINT ckAutOrdenCompra3 CHECK (
        (estadoAutOrdenCompra = 'PENDIENTE' AND identificadorUsuarioAutorizaAutOrdenCompra IS NULL AND fechaDecisionAutOrdenCompra IS NULL)
        OR
        (estadoAutOrdenCompra IN ('AUTORIZADA','RECHAZADA') AND identificadorUsuarioAutorizaAutOrdenCompra IS NOT NULL AND fechaDecisionAutOrdenCompra IS NOT NULL)
    ),
    CONSTRAINT ckAutOrdenCompra4 CHECK (identificadorUsuarioAutorizaAutOrdenCompra IS NULL OR identificadorUsuarioAutorizaAutOrdenCompra > 0),
    CONSTRAINT ckAutOrdenCompra5 CHECK (estadoAutOrdenCompra <> 'RECHAZADA' OR (motivoRechazoAutOrdenCompra IS NOT NULL AND CHAR_LENGTH(TRIM(motivoRechazoAutOrdenCompra)) > 0)),
    CONSTRAINT Fk_AutOrdenCompra_Compra FOREIGN KEY (Fk_OrdenCompraRecibida_Codigo)
        REFERENCES tblOrdenCompraRecibida(Pk_OrdenCompraRecibida) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblAutDisponibilidadFondos (
    Pk_AutDisponibilidadFondos BIGINT NOT NULL AUTO_INCREMENT,
    Fk_AutOrdenCompra_Codigo BIGINT NOT NULL,
    Fk_CuentaBancaria_Codigo BIGINT NOT NULL,
    fechaCorteAutDisponibilidadFondos DATE NOT NULL,
    fechaEvaluacionAutDisponibilidadFondos DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    saldoDisponibleAutDisponibilidadFondos DECIMAL(12,4) NOT NULL,
    Fk_Moneda_Origen_Codigo BIGINT NOT NULL,
    Fk_Moneda_Destino_Codigo BIGINT NOT NULL,
    tipoCambioAutDisponibilidadFondos DECIMAL(12,4) NOT NULL,
    Fk_TipoCambio_Codigo BIGINT NULL,
    fuenteAutDisponibilidadFondos VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_AutDisponibilidadFondos),
    CONSTRAINT uqAutDisponibilidadFondos1 UNIQUE
        (Pk_AutDisponibilidadFondos,Fk_AutOrdenCompra_Codigo,Fk_CuentaBancaria_Codigo),
    CONSTRAINT ckAutDisponibilidadFondos1 CHECK (tipoCambioAutDisponibilidadFondos > 0),
    CONSTRAINT ckAutDisponibilidadFondos2 CHECK (
        (Fk_Moneda_Origen_Codigo = Fk_Moneda_Destino_Codigo AND tipoCambioAutDisponibilidadFondos = 1.0000 AND Fk_TipoCambio_Codigo IS NULL)
        OR (Fk_Moneda_Origen_Codigo <> Fk_Moneda_Destino_Codigo AND Fk_TipoCambio_Codigo IS NOT NULL)
    ),
    INDEX idxAutDisponibilidadFondosEvaluacion
        (Fk_AutOrdenCompra_Codigo,Fk_CuentaBancaria_Codigo,fechaEvaluacionAutDisponibilidadFondos),
    INDEX idxAutDisponibilidadFondosCuenta (Fk_CuentaBancaria_Codigo),
    INDEX idxAutDisponibilidadFondosMonedaOrigen (Fk_Moneda_Origen_Codigo),
    INDEX idxAutDisponibilidadFondosMonedaDestino (Fk_Moneda_Destino_Codigo),
    INDEX idxAutDisponibilidadFondosTipoCambio (Fk_TipoCambio_Codigo),
    CONSTRAINT Fk_AutDisponibilidadFondos_Autorizacion FOREIGN KEY (Fk_AutOrdenCompra_Codigo)
        REFERENCES tblAutOrdenCompra(Pk_AutOrdenCompra) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_AutDisponibilidadFondos_Cuenta FOREIGN KEY (Fk_CuentaBancaria_Codigo)
        REFERENCES tblCuentaBancaria(Pk_CuentaBancaria) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_AutDisponibilidadFondos_MonedaOrigen FOREIGN KEY (Fk_Moneda_Origen_Codigo)
        REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_AutDisponibilidadFondos_MonedaDestino FOREIGN KEY (Fk_Moneda_Destino_Codigo)
        REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_AutDisponibilidadFondos_TipoCambio FOREIGN KEY (Fk_TipoCambio_Codigo)
        REFERENCES tblTipoCambio(Pk_TipoCambio) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblAutPagoOrden (
    Pk_AutPagoOrden BIGINT NOT NULL AUTO_INCREMENT,
    Fk_AutOrdenCompra_Codigo BIGINT NOT NULL,
    Fk_CuentaBancaria_Codigo BIGINT NOT NULL,
    Fk_AutDisponibilidadFondos_Codigo BIGINT NOT NULL,
    montoAsignadoAutPagoOrden DECIMAL(12,4) NOT NULL,
    estadoAutPagoOrden VARCHAR(30) NOT NULL DEFAULT 'PROPUESTA',
    fechaReservaAutPagoOrden DATETIME NULL,
    Fk_OrdenPago_Codigo BIGINT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_AutPagoOrden),
    CONSTRAINT uqAutPagoOrden1 UNIQUE (Fk_OrdenPago_Codigo),
    CONSTRAINT ckAutPagoOrden1 CHECK (montoAsignadoAutPagoOrden > 0),
    CONSTRAINT ckAutPagoOrden2 CHECK (estadoAutPagoOrden IN ('PROPUESTA','RESERVADA','LIBERADA','APLICADA')),
    CONSTRAINT ckAutPagoOrden3 CHECK (estadoAutPagoOrden NOT IN ('RESERVADA','APLICADA') OR fechaReservaAutPagoOrden IS NOT NULL),
    CONSTRAINT ckAutPagoOrden4 CHECK (estadoAutPagoOrden <> 'APLICADA' OR Fk_OrdenPago_Codigo IS NOT NULL),
    INDEX idxAutPagoOrdenAutorizacion (Fk_AutOrdenCompra_Codigo),
    INDEX idxAutPagoOrdenCuenta (Fk_CuentaBancaria_Codigo),
    INDEX idxAutPagoOrdenEvidencia (Fk_AutDisponibilidadFondos_Codigo,Fk_AutOrdenCompra_Codigo,Fk_CuentaBancaria_Codigo),
    CONSTRAINT Fk_AutPagoOrden_Autorizacion FOREIGN KEY (Fk_AutOrdenCompra_Codigo)
        REFERENCES tblAutOrdenCompra(Pk_AutOrdenCompra) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_AutPagoOrden_Cuenta FOREIGN KEY (Fk_CuentaBancaria_Codigo)
        REFERENCES tblCuentaBancaria(Pk_CuentaBancaria) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_AutPagoOrden_Evidencia FOREIGN KEY
        (Fk_AutDisponibilidadFondos_Codigo,Fk_AutOrdenCompra_Codigo,Fk_CuentaBancaria_Codigo)
        REFERENCES tblAutDisponibilidadFondos
        (Pk_AutDisponibilidadFondos,Fk_AutOrdenCompra_Codigo,Fk_CuentaBancaria_Codigo)
        ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_AutPagoOrden_OrdenPago FOREIGN KEY (Fk_OrdenPago_Codigo)
        REFERENCES tblOrdenPago(Pk_OrdenPago) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;


CREATE TABLE tblPartidaConsolidada (
    Pk_PartidaConsolidada BIGINT NOT NULL AUTO_INCREMENT,
    identificadorEmpresaPartidaConsolidada INT NOT NULL,
    fechaInicioPartidaConsolidada DATE NOT NULL,
    fechaFinPartidaConsolidada DATE NOT NULL,
    conceptoPartidaConsolidada VARCHAR(255) NOT NULL,
    Fk_Moneda_Codigo BIGINT NOT NULL,
    totalDebePartidaConsolidada DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    totalHaberPartidaConsolidada DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    estadoPartidaConsolidada VARCHAR(30) NOT NULL DEFAULT 'GENERADA',
    identificadorUsuarioGeneraPartidaConsolidada VARCHAR(100) NOT NULL,
    fechaGeneracionPartidaConsolidada DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    identificadorUsuarioTrasladaPartidaConsolidada VARCHAR(100) NULL,
    fechaTrasladoPartidaConsolidada DATETIME NULL,
    claveTrasladoPartidaConsolidada VARCHAR(100) NOT NULL,
    referenciaContabilidadPartidaConsolidada VARCHAR(100) NULL,
    mensajeResultadoTrasladoPartidaConsolidada VARCHAR(500) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_PartidaConsolidada),
    CONSTRAINT uqPartidaConsolidadaClave UNIQUE (claveTrasladoPartidaConsolidada),
    CONSTRAINT uqPartidaConsolidadaPeriodo UNIQUE
        (identificadorEmpresaPartidaConsolidada,fechaInicioPartidaConsolidada,fechaFinPartidaConsolidada,Fk_Moneda_Codigo),
    CONSTRAINT ckPartidaConsolidadaEmpresa CHECK (identificadorEmpresaPartidaConsolidada > 0),
    CONSTRAINT ckPartidaConsolidadaPeriodo CHECK (
        DAYOFMONTH(fechaInicioPartidaConsolidada) = 1
        AND fechaFinPartidaConsolidada = LAST_DAY(fechaInicioPartidaConsolidada)
    ),
    CONSTRAINT ckPartidaConsolidadaDatos CHECK (
        CHAR_LENGTH(TRIM(conceptoPartidaConsolidada)) > 0
        AND CHAR_LENGTH(TRIM(claveTrasladoPartidaConsolidada)) > 0
        AND CHAR_LENGTH(TRIM(identificadorUsuarioGeneraPartidaConsolidada)) > 0
    ),
    CONSTRAINT ckPartidaConsolidadaTotales CHECK (
        totalDebePartidaConsolidada >= 0 AND totalHaberPartidaConsolidada >= 0
    ),
    CONSTRAINT ckPartidaConsolidadaEstado CHECK (
        estadoPartidaConsolidada IN ('GENERADA','VALIDADA','EN_PROCESO','TRASLADADA','ERROR')
    ),
    CONSTRAINT ckPartidaConsolidadaCuadre CHECK (
        estadoPartidaConsolidada NOT IN ('VALIDADA','EN_PROCESO','TRASLADADA')
        OR (totalDebePartidaConsolidada = totalHaberPartidaConsolidada AND totalDebePartidaConsolidada > 0)
    ),
    CONSTRAINT ckPartidaConsolidadaUsuarioTraslado CHECK (
        estadoPartidaConsolidada NOT IN ('EN_PROCESO','TRASLADADA')
        OR (
            identificadorUsuarioTrasladaPartidaConsolidada IS NOT NULL
            AND CHAR_LENGTH(TRIM(identificadorUsuarioTrasladaPartidaConsolidada)) > 0
        )
    ),
    CONSTRAINT ckPartidaConsolidadaFechaTraslado CHECK (
        estadoPartidaConsolidada <> 'TRASLADADA' OR fechaTrasladoPartidaConsolidada IS NOT NULL
    ),
    CONSTRAINT ckPartidaConsolidadaReferenciaTraslado CHECK (
        estadoPartidaConsolidada <> 'TRASLADADA'
        OR (
            referenciaContabilidadPartidaConsolidada IS NOT NULL
            AND CHAR_LENGTH(TRIM(referenciaContabilidadPartidaConsolidada)) > 0
        )
    ),
    CONSTRAINT ckPartidaConsolidadaOrdenFechas CHECK (
        fechaTrasladoPartidaConsolidada IS NULL
        OR fechaTrasladoPartidaConsolidada >= fechaGeneracionPartidaConsolidada
    ),
    INDEX idxPartidaConsolidadaEmpresaEstado (identificadorEmpresaPartidaConsolidada,estadoPartidaConsolidada),
    INDEX idxPartidaConsolidadaMoneda (Fk_Moneda_Codigo),
    CONSTRAINT Fk_PartidaConsolidada_Moneda FOREIGN KEY (Fk_Moneda_Codigo)
        REFERENCES tblMoneda(Pk_Moneda) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblDetallePartidaConsolidada (
    Pk_DetallePartidaConsolidada BIGINT NOT NULL AUTO_INCREMENT,
    Fk_PartidaConsolidada_Codigo BIGINT NOT NULL,
    codigoCuentaContableDetallePartidaConsolidada VARCHAR(30) NOT NULL,
    nombreCuentaContableDetallePartidaConsolidada VARCHAR(255) NOT NULL,
    debeDetallePartidaConsolidada DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    haberDetallePartidaConsolidada DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Pk_DetallePartidaConsolidada),
    CONSTRAINT uqDetallePartidaCuenta UNIQUE
        (Fk_PartidaConsolidada_Codigo,codigoCuentaContableDetallePartidaConsolidada),
    CONSTRAINT ckDetallePartidaCuenta CHECK (
        CHAR_LENGTH(TRIM(codigoCuentaContableDetallePartidaConsolidada)) > 0
        AND CHAR_LENGTH(TRIM(nombreCuentaContableDetallePartidaConsolidada)) > 0
    ),
    CONSTRAINT ckDetallePartidaImportes CHECK (
        debeDetallePartidaConsolidada >= 0
        AND haberDetallePartidaConsolidada >= 0
        AND (debeDetallePartidaConsolidada > 0 OR haberDetallePartidaConsolidada > 0)
    ),
    CONSTRAINT Fk_DetallePartidaConsolidada_Partida FOREIGN KEY (Fk_PartidaConsolidada_Codigo)
        REFERENCES tblPartidaConsolidada(Pk_PartidaConsolidada)
        ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE tblPartidaConsolidadaPolizaBancaria (
    Fk_PartidaConsolidada_Codigo BIGINT NOT NULL,
    Fk_PolizaBancaria_Codigo BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (Fk_PartidaConsolidada_Codigo,Fk_PolizaBancaria_Codigo),
    CONSTRAINT uqPartidaConsolidadaPolizaBancariaPoliza UNIQUE (Fk_PolizaBancaria_Codigo),
    CONSTRAINT Fk_PartidaConsolidadaPolizaBancaria_Partida FOREIGN KEY (Fk_PartidaConsolidada_Codigo)
        REFERENCES tblPartidaConsolidada(Pk_PartidaConsolidada)
        ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_PartidaConsolidadaPolizaBancaria_Poliza FOREIGN KEY (Fk_PolizaBancaria_Codigo)
        REFERENCES tblPolizaBancaria(Pk_PolizaBancaria)
        ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;

-- 1. Retirar relaciones/indices que cambian en V1.1
ALTER TABLE `tblConciliacion` DROP FOREIGN KEY `Fk_Conciliacion_Cuenta`, DROP FOREIGN KEY `Fk_Conciliacion_Extracto`, DROP INDEX `idxConciliacionExtracto`;
ALTER TABLE `tblAutPagoOrden` DROP FOREIGN KEY `Fk_AutPagoOrden_Autorizacion`, DROP FOREIGN KEY `Fk_AutPagoOrden_Cuenta`, DROP FOREIGN KEY `Fk_AutPagoOrden_Evidencia`, DROP INDEX `idxAutPagoOrdenAutorizacion`, DROP INDEX `idxAutPagoOrdenCuenta`, DROP INDEX `idxAutPagoOrdenEvidencia`;
ALTER TABLE `tblAutDisponibilidadFondos` DROP INDEX `uqAutDisponibilidadFondos1`;
    
-- 2. Renombrar IDs existentes conservando los valores y sus claves primarias
ALTER TABLE `tblMoneda` RENAME COLUMN `Pk_Moneda` TO `idMoneda`;
ALTER TABLE `tblTipoCambio` RENAME COLUMN `Pk_TipoCambio` TO `idTipoCambio`;
ALTER TABLE `tblBanco` RENAME COLUMN `Pk_Banco` TO `idBanco`;
ALTER TABLE `tblTipoCuentaBancaria` RENAME COLUMN `Pk_TipoCuentaBancaria` TO `idTipoCuentaBancaria`;
ALTER TABLE `tblTipoTransaccion` RENAME COLUMN `Pk_TipoTransaccion` TO `idTipoTransaccion`;
ALTER TABLE `tblCuentaBancaria` RENAME COLUMN `Pk_CuentaBancaria` TO `idCuentaBancaria`;
ALTER TABLE `tblDisponibilidadDiaria`RENAME COLUMN `Pk_DisponibilidadDiaria` TO `idDisponibilidadDiaria`;
ALTER TABLE `tblDetallePlanillaRecibida` RENAME COLUMN `Pk_DetallePlanillaRecibida` TO `idDetallePlanillaRecibida`;
ALTER TABLE `tblOrdenPago` RENAME COLUMN `Pk_OrdenPago` TO `idOrdenPago`;
ALTER TABLE `tblAprobacion` RENAME COLUMN `Pk_Aprobacion` TO `idAprobacion`;
ALTER TABLE `tblChequera` RENAME COLUMN `Pk_Chequera` TO `idChequera`;
ALTER TABLE `tblCheque` RENAME COLUMN `Pk_Cheque` TO `idCheque`;
ALTER TABLE `tblMovimientoBancario` RENAME COLUMN `Pk_MovimientoBancario` TO `idMovimientoBancario`;
ALTER TABLE `tblDetalleExtracto` RENAME COLUMN `Pk_DetalleExtracto` TO `idDetalleExtracto`;
ALTER TABLE `tblConciliacion` RENAME COLUMN `Pk_Conciliacion` TO `idConciliacion`;
ALTER TABLE `tblDetalleConciliacion` RENAME COLUMN `Pk_DetalleConciliacion` TO `idDetalleConciliacion`;
ALTER TABLE `tblConfiguracionPoliza` RENAME COLUMN `Pk_ConfiguracionPoliza` TO `idConfiguracionPoliza`;
ALTER TABLE `tblPolizaBancaria` RENAME COLUMN `Pk_PolizaBancaria` TO `idPolizaBancaria`;
ALTER TABLE `tblDetallePolizaBancaria` RENAME COLUMN `Pk_DetallePolizaBancaria` TO `idDetallePolizaBancaria`;
ALTER TABLE `tblAutDisponibilidadFondos` RENAME COLUMN `Pk_AutDisponibilidadFondos` TO `idAutDisponibilidadFondos`;
ALTER TABLE `tblAutPagoOrden` RENAME COLUMN `Pk_AutPagoOrden` TO `idAutPagoOrden`;
ALTER TABLE `tblDetallePartidaConsolidada` RENAME COLUMN `Pk_DetallePartidaConsolidada` TO `idDetallePartidaConsolidada`;
    
-- 3. Ajustes de longitud reales de V1.0 a V1.1
ALTER TABLE `tblMoneda` MODIFY COLUMN `nombreMoneda` VARCHAR(100) NOT NULL;
ALTER TABLE `tblTipoCuentaBancaria` MODIFY COLUMN `nombreTipoCuentaBancaria` VARCHAR(100) NOT NULL;
ALTER TABLE `tblTipoTransaccion` MODIFY COLUMN `codigoTipoTransaccion` VARCHAR(30) COLLATE utf8mb4_unicode_ci NOT NULL, MODIFY COLUMN `nombreTipoTransaccion` VARCHAR(100) COLLATE utf8mb4_unicode_ci NOT NULL, MODIFY COLUMN `naturalezaTipoTransaccion` VARCHAR(10) COLLATE utf8mb4_unicode_ci NOT NULL;
    
-- 4. AutPagoOrden: simplificacion de FK y columnas en V1.1
ALTER TABLE `tblAutPagoOrden` ADD INDEX `idxAutPagoOrdenEvidencia` (`Fk_AutDisponibilidadFondos_Codigo`), ADD CONSTRAINT `Fk_AutPagoOrden_Evidencia` FOREIGN KEY (`Fk_AutDisponibilidadFondos_Codigo`) REFERENCES `tblAutDisponibilidadFondos` (`idAutDisponibilidadFondos`) ON DELETE RESTRICT ON UPDATE RESTRICT;
ALTER TABLE `tblAutPagoOrden` DROP COLUMN `Fk_AutOrdenCompra_Codigo`, DROP COLUMN `Fk_CuentaBancaria_Codigo`;
    
-- 5. Partida consolidada: periodo mensual calculado desde fechaInicio
ALTER TABLE `tblPartidaConsolidada` DROP CHECK `ckPartidaConsolidadaPeriodo`, DROP INDEX `uqPartidaConsolidadaPeriodo`;
ALTER TABLE `tblPartidaConsolidada` ADD CONSTRAINT `uqPartidaConsolidadaPeriodo` UNIQUE (`identificadorEmpresaPartidaConsolidada`, `fechaInicioPartidaConsolidada`, `Fk_Moneda_Codigo`), ADD CONSTRAINT `ckPartidaConsolidadaPeriodo` CHECK (DAYOFMONTH(`fechaInicioPartidaConsolidada`) = 1);
ALTER TABLE `tblPartidaConsolidada` DROP COLUMN `fechaFinPartidaConsolidada`;


-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡  Contabilidad  !!!!!!!!!!!!!!!!!!!!!!!!!-------------

-- =============================================================================
--  1. CATÁLOGOS (Tablas Originales Intactas)
-- =============================================================================

CREATE TABLE tblPais (
    idPais      INT          NOT NULL AUTO_INCREMENT,
    nombrePais  VARCHAR(100) NOT NULL,
    is_active   BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at  TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at  TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Pais PRIMARY KEY (idPais),
    CONSTRAINT uqPaisNombre UNIQUE (nombrePais)
) ENGINE=InnoDB;

CREATE TABLE tblModuloContable (
    idModuloContable     INT          NOT NULL AUTO_INCREMENT,
    nombreModuloContable VARCHAR(100) NOT NULL,
    codigoModuloContable VARCHAR(30)  NOT NULL,
    is_active            BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at           TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at           TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ModuloContable PRIMARY KEY (idModuloContable),
    CONSTRAINT uqModuloNombre UNIQUE (nombreModuloContable),
    CONSTRAINT uqModuloCodigo UNIQUE (codigoModuloContable)
) ENGINE=InnoDB;

CREATE TABLE tblCuenta (
    idCuenta            INT           NOT NULL AUTO_INCREMENT,
    codigoCuenta        VARCHAR(30)   NOT NULL,
    nombreCuenta        VARCHAR(100)  NOT NULL,
    tipoCuenta          VARCHAR(30)   NOT NULL,
    clasificacionCuenta VARCHAR(10)   NOT NULL DEFAULT 'DETALLE',
    idCuentaPadre       INT           NULL,
    saldoInicialCuenta  DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    cargoPeriodoCuenta  DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    abonoPeriodoCuenta  DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    saldoActualCuenta   DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    cargoActualCuenta   DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    abonoActualCuenta   DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    periodoSaldoCuenta  CHAR(7)       NULL,
    is_active           BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at          TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at          TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Cuenta PRIMARY KEY (idCuenta),
    CONSTRAINT uqCuentaCodigo UNIQUE (codigoCuenta),
    CONSTRAINT ckCuentaTipo CHECK (tipoCuenta IN ('DEUDORA','ACREEDORA')),
    CONSTRAINT ckCuentaClasificacion CHECK (clasificacionCuenta IN ('MAYOR','DETALLE')),
    CONSTRAINT Fk_Cuenta_Cuenta FOREIGN KEY (idCuentaPadre) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxCuentaPadre (idCuentaPadre),
    INDEX idxCuentaTipo  (tipoCuenta),
    INDEX idxCuentaClasificacion (clasificacionCuenta),
    INDEX idxCuentaNombre (nombreCuenta)
) ENGINE=InnoDB;

CREATE TABLE tblModuloCuentaPais (
    idModuloContable INT       NOT NULL,
    idCuenta         INT       NOT NULL,
    idPais           INT       NOT NULL,
    is_active        BOOLEAN   NOT NULL DEFAULT TRUE,
    created_at       TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at       TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ModuloCuentaPais PRIMARY KEY (idModuloContable, idCuenta, idPais),
    CONSTRAINT Fk_ModuloCuentaPais_Modulo FOREIGN KEY (idModuloContable) REFERENCES tblModuloContable (idModuloContable) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_ModuloCuentaPais_Cuenta FOREIGN KEY (idCuenta) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_ModuloCuentaPais_Pais FOREIGN KEY (idPais) REFERENCES tblPais (idPais) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxModuloCuentaPaisCuenta (idCuenta),
    INDEX idxModuloCuentaPaisPais   (idPais)
) ENGINE=InnoDB;

-- =============================================================================
--  2. ACTUALIZACIÓN DE SALDOS Y SALDOS POR PERÍODO
-- =============================================================================

CREATE TABLE tblActualizacionSaldos (
    idActualizacionSaldos              INT          NOT NULL AUTO_INCREMENT,
    modoActualizacionSaldos            VARCHAR(10)  NOT NULL,
    tipoPeriodoActualizacionSaldos     VARCHAR(10)  NOT NULL,
    fechaDesdeActualizacionSaldos      DATE         NOT NULL,
    fechaHastaActualizacionSaldos      DATE         NOT NULL,
    polizasProcesadasActualizacionSaldos INT        NOT NULL DEFAULT 0,
    usuarioActualizacionSaldos         VARCHAR(100) NULL,
    is_active                          BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at                         TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                         TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ActualizacionSaldos PRIMARY KEY (idActualizacionSaldos),
    CONSTRAINT ckActualizacionModo     CHECK (modoActualizacionSaldos IN ('LINEA','BATCH')),
    CONSTRAINT ckActualizacionTipo     CHECK (tipoPeriodoActualizacionSaldos IN ('ANUAL','MENSUAL','SEMANAL','RANGO')),
    CONSTRAINT ckActualizacionFechas   CHECK (fechaHastaActualizacionSaldos >= fechaDesdeActualizacionSaldos),
    CONSTRAINT ckActualizacionPolizas  CHECK (polizasProcesadasActualizacionSaldos >= 0),
    INDEX idxActualizacionFechas (fechaDesdeActualizacionSaldos, fechaHastaActualizacionSaldos)
) ENGINE=InnoDB;

CREATE TABLE tblSaldoCuentaPeriodo (
    idSaldoCuentaPeriodo            INT           NOT NULL AUTO_INCREMENT,
    idCuenta                        INT           NOT NULL,
    idActualizacionSaldos           INT           NULL,
    periodoContableSaldoCuentaPeriodo CHAR(7)     NOT NULL,
    saldoInicialSaldoCuentaPeriodo  DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    cargoSaldoCuentaPeriodo         DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    abonoSaldoCuentaPeriodo         DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    saldoFinalSaldoCuentaPeriodo    DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    estadoSaldoCuentaPeriodo        VARCHAR(10)   NOT NULL DEFAULT 'ABIERTO',
    is_active                       BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at                      TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                      TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_SaldoCuentaPeriodo PRIMARY KEY (idSaldoCuentaPeriodo),
    CONSTRAINT uqSaldoCuentaPeriodo UNIQUE (idCuenta, periodoContableSaldoCuentaPeriodo),
    CONSTRAINT ckSaldoEstado      CHECK (estadoSaldoCuentaPeriodo IN ('ABIERTO','CERRADO')),
    CONSTRAINT ckSaldoMovimientos CHECK (cargoSaldoCuentaPeriodo >= 0 AND abonoSaldoCuentaPeriodo >= 0),
    CONSTRAINT Fk_SaldoCuentaPeriodo_Cuenta FOREIGN KEY (idCuenta) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_SaldoCuentaPeriodo_Actualizacion FOREIGN KEY (idActualizacionSaldos) REFERENCES tblActualizacionSaldos (idActualizacionSaldos) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxSaldoCuentaPeriodoPeriodo (periodoContableSaldoCuentaPeriodo),
    INDEX idxSaldoCuentaPeriodoActualizacion (idActualizacionSaldos)
) ENGINE=InnoDB;

-- =============================================================================
--  3. PÓLIZAS CONTABLES
-- =============================================================================

CREATE TABLE tblPolizaEncabezado (
    idPolizaEncabezado            INT          NOT NULL AUTO_INCREMENT,
    idModuloContable              INT          NOT NULL DEFAULT 1,
    idActualizacionSaldos         INT          NULL,
    codigoPolizaEncabezado        VARCHAR(30)  NOT NULL,
    fechaPolizaEncabezado         DATE         NOT NULL,
    periodoContablePolizaEncabezado CHAR(7)    NOT NULL,
    estadoPolizaEncabezado        VARCHAR(30)  NOT NULL DEFAULT 'BORRADOR',
    documentoOrigenPolizaEncabezado VARCHAR(30) NULL,
    descripcionPolizaEncabezado   VARCHAR(255) NULL,
    is_active                     BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at                    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PolizaEncabezado PRIMARY KEY (idPolizaEncabezado),
    CONSTRAINT uqPolizaCodigo UNIQUE (codigoPolizaEncabezado),
    CONSTRAINT uqPolizaDocumentoOrigen UNIQUE (idModuloContable, documentoOrigenPolizaEncabezado),
    CONSTRAINT ckPolizaEstado CHECK (estadoPolizaEncabezado IN ('BORRADOR','VALIDADA','APLICADA','ANULADA')),
    CONSTRAINT Fk_PolizaEncabezado_Modulo FOREIGN KEY (idModuloContable) REFERENCES tblModuloContable (idModuloContable) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_PolizaEncabezado_Actualizacion FOREIGN KEY (idActualizacionSaldos) REFERENCES tblActualizacionSaldos (idActualizacionSaldos) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxPolizaModulo  (idModuloContable),
    INDEX idxPolizaPeriodo (periodoContablePolizaEncabezado),
    INDEX idxPolizaFecha   (fechaPolizaEncabezado),
    INDEX idxPolizaEstado  (estadoPolizaEncabezado),
    INDEX idxPolizaActualizacion (idActualizacionSaldos)
) ENGINE=InnoDB;

CREATE TABLE tblPolizaDetalle (
    idPolizaDetalle             INT           NOT NULL AUTO_INCREMENT,
    idPolizaEncabezado          INT           NOT NULL,
    idCuenta                    INT           NOT NULL,
    lineaPolizaDetalle          INT           NOT NULL,
    debePolizaDetalle           DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    haberPolizaDetalle          DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    descripcionPolizaDetalle    VARCHAR(255)  NULL,
    is_active                   BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at                  TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                  TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PolizaDetalle PRIMARY KEY (idPolizaDetalle),
    CONSTRAINT uqPolizaDetalleLinea UNIQUE (idPolizaEncabezado, lineaPolizaDetalle),
    CONSTRAINT ckDetalleMontoPositivo CHECK (debePolizaDetalle >= 0 AND haberPolizaDetalle >= 0),
    CONSTRAINT ckDetallePartidaDoble  CHECK (debePolizaDetalle = 0 OR haberPolizaDetalle = 0),
    CONSTRAINT Fk_PolizaDetalle_PolizaEncabezado FOREIGN KEY (idPolizaEncabezado) REFERENCES tblPolizaEncabezado (idPolizaEncabezado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_PolizaDetalle_Cuenta FOREIGN KEY (idCuenta) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxPolizaDetalleEncabezado (idPolizaEncabezado),
    INDEX idxPolizaDetalleCuenta     (idCuenta)
) ENGINE=InnoDB;

-- =============================================================================
--  4. PRESUPUESTO
-- =============================================================================

CREATE TABLE tblPresupuestoEncabezado (
    idPresupuestoEncabezado            INT           NOT NULL AUTO_INCREMENT,
    nombrePresupuestoEncabezado        VARCHAR(100)  NOT NULL,
    totalPresupuestoEncabezado         DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    gastadoPresupuestoEncabezado       DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    restantePresupuestoEncabezado      DECIMAL(12,4) GENERATED ALWAYS AS (totalPresupuestoEncabezado - gastadoPresupuestoEncabezado) STORED,
    fechaInicioPresupuestoEncabezado   DATE          NOT NULL,
    fechaCierrePresupuestoEncabezado   DATE          NULL,
    is_active                          BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at                         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PresupuestoEncabezado PRIMARY KEY (idPresupuestoEncabezado),
    CONSTRAINT ckPresupuestoFechas CHECK (fechaCierrePresupuestoEncabezado IS NULL OR fechaCierrePresupuestoEncabezado >= fechaInicioPresupuestoEncabezado),
    CONSTRAINT ckPresupuestoMontos CHECK (totalPresupuestoEncabezado >= 0 AND gastadoPresupuestoEncabezado >= 0)
) ENGINE=InnoDB;

CREATE TABLE tblPresupuestoDetalle (
    idPresupuestoDetalle                 INT           NOT NULL AUTO_INCREMENT,
    idPresupuestoEncabezado              INT           NOT NULL,
    idModuloContable                     INT           NOT NULL,
    idPolizaDetalle                      INT           NULL,
    nombrePresupuestoDetalle             VARCHAR(100)  NOT NULL,
    cantidadPresupuestoDetalle           DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    is_active                            BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at                           TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                           TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_PresupuestoDetalle PRIMARY KEY (idPresupuestoDetalle),
    CONSTRAINT ckPresupuestoDetalleCantidad CHECK (cantidadPresupuestoDetalle >= 0),
    CONSTRAINT Fk_PresupuestoDetalle_PresupuestoEncabezado FOREIGN KEY (idPresupuestoEncabezado) REFERENCES tblPresupuestoEncabezado (idPresupuestoEncabezado) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_PresupuestoDetalle_Modulo FOREIGN KEY (idModuloContable) REFERENCES tblModuloContable (idModuloContable) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_PresupuestoDetalle_PolizaDetalle FOREIGN KEY (idPolizaDetalle) REFERENCES tblPolizaDetalle (idPolizaDetalle) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxPresupuestoDetalleEncabezado (idPresupuestoEncabezado),
    INDEX idxPresupuestoDetalleModulo     (idModuloContable),
    INDEX idxPresupuestoDetallePolizaDet  (idPolizaDetalle)
) ENGINE=InnoDB;

-- =============================================================================
--  5. ACTIVOS FIJOS
-- =============================================================================

CREATE TABLE tblMaestroVidaUtil (
    idMaestroVidaUtil                    INT          NOT NULL AUTO_INCREMENT,
    codigoMaestroVidaUtil                VARCHAR(30)  NOT NULL,
    nombreMaestroVidaUtil                VARCHAR(100) NOT NULL,
    cantidadMesesMaestroVidaUtil         INT          NOT NULL,
    tasaDepreciacionMaestroVidaUtil      DECIMAL(3,2) NOT NULL DEFAULT 0.20,
    is_active                            BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at                           TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                           TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_MaestroVidaUtil PRIMARY KEY (idMaestroVidaUtil),
    CONSTRAINT uqMaestroVidaUtilCodigo UNIQUE (codigoMaestroVidaUtil),
    CONSTRAINT ckMaestroVidaUtilMeses CHECK (cantidadMesesMaestroVidaUtil > 0)
) ENGINE=InnoDB;

CREATE TABLE tblActivoFijo (
    idActivoFijo                        INT           NOT NULL AUTO_INCREMENT,
    idModuloContable                    INT           NOT NULL,
    idMaestroVidaUtil                   INT           NOT NULL,
    codigoActivoFijo                    VARCHAR(30)   NOT NULL,
    descripcionActivoFijo               VARCHAR(255)  NOT NULL,
    codigoCentroCostoActivoFijo         VARCHAR(30)   NOT NULL,
    valorCompraActivoFijo               DECIMAL(12,4) NOT NULL,
    valorResidualActivoFijo             DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    valorDepreciableActivoFijo          DECIMAL(12,4) GENERATED ALWAYS AS (valorCompraActivoFijo - valorResidualActivoFijo) STORED,
    valorEnLibrosActivoFijo             DECIMAL(12,4) NOT NULL,
    mesesVidaUtilActivoFijo             INT           NOT NULL,
    fechaAdquisicionActivoFijo          DATE          NOT NULL,
    fechaInicioUsoActivoFijo            DATE          NOT NULL,
    is_active                           BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at                          TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                          TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ActivoFijo PRIMARY KEY (idActivoFijo),
    CONSTRAINT uqActivoFijoCodigo UNIQUE (codigoActivoFijo),
    CONSTRAINT ckActivoFijoResidual CHECK (valorResidualActivoFijo >= 0 AND valorResidualActivoFijo <= valorCompraActivoFijo),
    CONSTRAINT ckActivoFijoFechas   CHECK (fechaInicioUsoActivoFijo >= fechaAdquisicionActivoFijo),
    CONSTRAINT ckActivoFijoMeses    CHECK (mesesVidaUtilActivoFijo > 0),
    CONSTRAINT Fk_ActivoFijo_Modulo FOREIGN KEY (idModuloContable) REFERENCES tblModuloContable (idModuloContable) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_ActivoFijo_MaestroVidaUtil FOREIGN KEY (idMaestroVidaUtil) REFERENCES tblMaestroVidaUtil (idMaestroVidaUtil) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxActivoFijoModulo      (idModuloContable),
    INDEX idxActivoFijoMaestroVida (idMaestroVidaUtil)
) ENGINE=InnoDB;

-- =============================================================================
--  6. VISTAS DE VISUALIZACIÓN
-- =============================================================================

CREATE OR REPLACE VIEW vwLibroDiario AS
SELECT  pe.codigoPolizaEncabezado        AS codigoPoliza,
        pe.fechaPolizaEncabezado         AS fechaPoliza,
        pe.periodoContablePolizaEncabezado AS periodoContablePoliza,
        pe.estadoPolizaEncabezado        AS estadoPoliza,
        m.nombreModuloContable           AS moduloPoliza,
        pd.lineaPolizaDetalle            AS lineaDetalle,
        c.codigoCuenta                   AS codigoCuenta,
        c.nombreCuenta                   AS nombreCuenta,
        c.tipoCuenta                     AS tipoCuenta,
        pd.debePolizaDetalle             AS debeDetalle,
        pd.haberPolizaDetalle            AS haberDetalle,
        pd.descripcionPolizaDetalle      AS descripcionDetalle
FROM    tblPolizaDetalle    pd
JOIN    tblPolizaEncabezado pe ON pe.idPolizaEncabezado = pd.idPolizaEncabezado
JOIN    tblCuenta           c  ON c.idCuenta            = pd.idCuenta
JOIN    tblModuloContable   m  ON m.idModuloContable    = pe.idModuloContable
WHERE   pd.is_active = TRUE
  AND   pe.is_active = TRUE;

CREATE OR REPLACE VIEW vwPolizaCuadre AS
SELECT  pe.codigoPolizaEncabezado        AS codigoPoliza,
        pe.fechaPolizaEncabezado         AS fechaPoliza,
        pe.estadoPolizaEncabezado        AS estadoPoliza,
        pe.descripcionPolizaEncabezado   AS descripcionPoliza,
        COUNT(pd.idPolizaDetalle)        AS totalLineasPoliza,
        SUM(pd.debePolizaDetalle)        AS totalDebePoliza,
        SUM(pd.haberPolizaDetalle)       AS totalHaberPoliza,
        (SUM(pd.debePolizaDetalle) - SUM(pd.haberPolizaDetalle)) AS diferenciaPoliza,
        CASE WHEN SUM(pd.debePolizaDetalle) = SUM(pd.haberPolizaDetalle)
             THEN 'CUADRADA' ELSE 'DESCUADRADA' END AS resultadoCuadre
FROM    tblPolizaEncabezado pe
JOIN    tblPolizaDetalle    pd ON pd.idPolizaEncabezado = pe.idPolizaEncabezado
WHERE   pe.is_active = TRUE AND pd.is_active = TRUE
GROUP BY pe.idPolizaEncabezado, pe.codigoPolizaEncabezado, pe.fechaPolizaEncabezado,
         pe.estadoPolizaEncabezado, pe.descripcionPolizaEncabezado;

CREATE OR REPLACE VIEW vwPolizaEncabezadoGeneral AS
SELECT  pe.idPolizaEncabezado          AS idPoliza,
        pe.codigoPolizaEncabezado      AS codigoPoliza,
        pe.fechaPolizaEncabezado       AS fechaPoliza,
        pe.periodoContablePolizaEncabezado AS periodoContablePoliza,
        pe.estadoPolizaEncabezado      AS estadoPoliza,
        m.idModuloContable             AS idModulo,
        m.nombreModuloContable         AS moduloPoliza,
        pe.documentoOrigenPolizaEncabezado AS documentoOrigenPoliza,
        pe.descripcionPolizaEncabezado AS descripcionPoliza,
        COUNT(pd.idPolizaDetalle)      AS totalLineasPoliza,
        IFNULL(SUM(pd.debePolizaDetalle), 0)  AS totalDebePoliza,
        IFNULL(SUM(pd.haberPolizaDetalle), 0) AS totalHaberPoliza,
        pe.idActualizacionSaldos       AS idActualizacionSaldos,
        pe.created_at                  AS fechaRegistroPoliza
FROM    tblPolizaEncabezado pe
JOIN    tblModuloContable   m  ON m.idModuloContable = pe.idModuloContable
LEFT JOIN tblPolizaDetalle  pd ON pd.idPolizaEncabezado = pe.idPolizaEncabezado AND pd.is_active = TRUE
WHERE   pe.is_active = TRUE
GROUP BY pe.idPolizaEncabezado, pe.codigoPolizaEncabezado, pe.fechaPolizaEncabezado, pe.periodoContablePolizaEncabezado,
         pe.estadoPolizaEncabezado, m.idModuloContable, m.nombreModuloContable, pe.documentoOrigenPolizaEncabezado,
         pe.descripcionPolizaEncabezado, pe.idActualizacionSaldos, pe.created_at;

CREATE OR REPLACE VIEW vwBalanceCuenta AS
SELECT  c.codigoCuenta                   AS codigoCuenta,
        c.nombreCuenta                   AS nombreCuenta,
        c.tipoCuenta                     AS tipoCuenta,
        c.clasificacionCuenta            AS clasificacionCuenta,
        s.periodoContableSaldoCuentaPeriodo AS periodoContableSaldo,
        s.saldoInicialSaldoCuentaPeriodo AS saldoInicialCuenta,
        s.cargoSaldoCuentaPeriodo        AS totalDebeCuenta,
        s.abonoSaldoCuentaPeriodo        AS totalHaberCuenta,
        s.saldoFinalSaldoCuentaPeriodo   AS saldoCuenta,
        s.estadoSaldoCuentaPeriodo       AS estadoSaldo
FROM    tblSaldoCuentaPeriodo s
JOIN    tblCuenta             c ON c.idCuenta = s.idCuenta
WHERE   s.is_active = TRUE AND c.is_active = TRUE;

CREATE OR REPLACE VIEW vwPolizaTraslado AS
SELECT  m.nombreModuloContable           AS moduloOrigen,
        pe.documentoOrigenPolizaEncabezado AS documentoOrigen,
        pe.codigoPolizaEncabezado        AS codigoPoliza,
        pe.fechaPolizaEncabezado         AS fechaPoliza,
        pe.estadoPolizaEncabezado        AS estadoPoliza,
        SUM(pd.debePolizaDetalle)        AS montoTrasladado,
        pe.created_at                    AS fechaTraslado
FROM    tblPolizaEncabezado pe
JOIN    tblModuloContable   m  ON m.idModuloContable = pe.idModuloContable
JOIN    tblPolizaDetalle    pd ON pd.idPolizaEncabezado = pe.idPolizaEncabezado
WHERE   pe.documentoOrigenPolizaEncabezado IS NOT NULL
GROUP BY pe.idPolizaEncabezado, m.nombreModuloContable, pe.documentoOrigenPolizaEncabezado,
         pe.codigoPolizaEncabezado, pe.fechaPolizaEncabezado, pe.estadoPolizaEncabezado, pe.created_at;

-- =============================================================================
--  7. AJUSTES SOBRE TABLAS EXISTENTES
--  Para bases que ya tienen las tablas creadas: ejecutar solo esta sección.
-- =============================================================================

-- Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
ALTER TABLE tblModuloContable
    ADD COLUMN metodoActualizacionModuloContable VARCHAR(30) NOT NULL DEFAULT 'BATCH' AFTER codigoModuloContable,
    ADD CONSTRAINT ckModuloMetodoActualizacion CHECK (metodoActualizacionModuloContable IN ('LINEA','BATCH'));

ALTER TABLE tblCuenta
    RENAME COLUMN cargoActualCuenta TO cargoAcumuladoCuenta,
    RENAME COLUMN abonoActualCuenta TO abonoAcumuladoCuenta;

ALTER TABLE tblActivoFijo
    DROP COLUMN codigoCentroCostoActivoFijo;
-- Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026

-- Inicio de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026
ALTER TABLE tblPais
    ADD COLUMN codigoMonedaPais CHAR(3) NOT NULL DEFAULT 'GTQ' AFTER nombrePais;

UPDATE tblPais SET codigoMonedaPais = CASE nombrePais
    WHEN 'El Salvador' THEN 'USD'
    WHEN 'Honduras'    THEN 'HNL'
    ELSE 'GTQ' END
WHERE idPais > 0;

ALTER TABLE tblPolizaEncabezado
    ADD COLUMN idPais INT NOT NULL DEFAULT 1 AFTER idModuloContable,
    ADD CONSTRAINT Fk_PolizaEncabezado_Pais FOREIGN KEY (idPais) REFERENCES tblPais (idPais) ON DELETE RESTRICT ON UPDATE CASCADE;
-- Fin de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026

-- Inicio de cambio: Dylan Hernández - 0901-23-519 - 30/10/2026
CREATE TABLE IF NOT EXISTS tblImpuesto (
    idImpuesto                    INT          NOT NULL AUTO_INCREMENT,
    nombreImpuesto                VARCHAR(100) NOT NULL,
    descripcionImpuesto           VARCHAR(255) NULL,
    tipoImpuesto                  VARCHAR(30)  NOT NULL,
    operacionImpuesto             VARCHAR(30)  NOT NULL DEFAULT 'AMBAS',
    valorImpuesto                 DECIMAL(7,4) NOT NULL,
    periodicidadImpuesto          VARCHAR(30)  NOT NULL,
    idCuentaAcreditableImpuesto   INT          NULL,
    idCuentaPorPagarImpuesto      INT          NULL,
    created_at                    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Impuesto PRIMARY KEY (idImpuesto),
    CONSTRAINT ckImpuestoTipo CHECK (tipoImpuesto IN ('CONSUMO','RENTA','PATRIMONIO','CONTRIBUCION','OTRO')),
    CONSTRAINT ckImpuestoOperacion CHECK (operacionImpuesto IN ('COMPRA','VENTA','AMBAS','PLANILLA','PERIODO')),
    CONSTRAINT ckImpuestoPeriodicidad CHECK (periodicidadImpuesto IN ('MENSUAL','TRIMESTRAL','ANUAL','POR_EVENTO')),
    CONSTRAINT Fk_Impuesto_CuentaAcreditable FOREIGN KEY (idCuentaAcreditableImpuesto) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT Fk_Impuesto_CuentaPorPagar FOREIGN KEY (idCuentaPorPagarImpuesto) REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE,
    INDEX idxImpuestoTipo (tipoImpuesto),
    INDEX idxImpuestoOperacion (operacionImpuesto)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblImpuestoTramo (
    idImpuestoTramo              INT           NOT NULL AUTO_INCREMENT,
    idImpuesto                   INT           NOT NULL,
    montoDesdeImpuestoTramo      DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    montoHastaImpuestoTramo      DECIMAL(12,4) NULL,
    tasaImpuestoTramo            DECIMAL(5,4)  NOT NULL,
    importeFijoImpuestoTramo     DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
    created_at                   TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                   TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ImpuestoTramo PRIMARY KEY (idImpuestoTramo),
    CONSTRAINT uqImpuestoTramo UNIQUE (idImpuesto, montoDesdeImpuestoTramo),
    CONSTRAINT ckImpuestoTramoRango CHECK (montoHastaImpuestoTramo IS NULL OR montoHastaImpuestoTramo > montoDesdeImpuestoTramo),
    CONSTRAINT Fk_ImpuestoTramo_Impuesto FOREIGN KEY (idImpuesto) REFERENCES tblImpuesto (idImpuesto) ON DELETE CASCADE ON UPDATE CASCADE,
    INDEX idxImpuestoTramoImpuesto (idImpuesto)
) ENGINE=InnoDB;
-- Fin de cambio: Dylan Hernández - 0901-23-519 - 30/10/2026
         
-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡ alter de  recusos humanos  una de sus tablas !!!!!!!!!!!!!!!!!!!!!!!!!-------------        

         -- ---------- 3. Percepciones/Deducciones: cuenta contable ----------
-- Inicio de cambio: Isabel Meléndez - 9959-23-1379 - 31/10/2026
ALTER TABLE tblPercepcionDeduccion
    ADD COLUMN idCuenta INT NULL AFTER aplicacionPercepcionDeduccion,
    ADD CONSTRAINT Fk_PercepcionDeduccion_Cuenta FOREIGN KEY (idCuenta)
        REFERENCES tblCuenta (idCuenta) ON DELETE RESTRICT ON UPDATE CASCADE;
        
  -- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡ tablas de Compras que necesita Logistica para funcionar !!!!!!!!!!!!!!!!!!!!!!!!!-------------              
        
        CREATE TABLE IF NOT EXISTS tblPlazo(
    idPlazo                                      INT AUTO_INCREMENT,
    nombrePlazo                                  VARCHAR(100) NOT NULL,
    diasPlazo                                    INT NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT Pk_Plazos PRIMARY KEY (idPlazo),
    CONSTRAINT uqnombrePlazo UNIQUE (nombrePlazo),
    CONSTRAINT ckDiasPlazo CHECK (diasPlazo >= 0)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblConceptoProveedor (
    idConceptoProveedor                          INT          NOT NULL AUTO_INCREMENT,
    nombreConceptoProveedor                      VARCHAR(100) NOT NULL,
    naturalezaConceptoProveedor                  VARCHAR(10)  NOT NULL,
    created_at                                   TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_ConceptoProveedor PRIMARY KEY (idConceptoProveedor),
    CONSTRAINT uqConceptoProveedorNombre UNIQUE (nombreConceptoProveedor),
    CONSTRAINT ckNaturalezaConceptoProveedor CHECK (naturalezaConceptoProveedor IN ('Cargo','Abono'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblProveedor(
    idProveedor                                  INT AUTO_INCREMENT,
    nombreProveedor                              VARCHAR(100) NOT NULL,
    nitProveedor                                 CHAR(10) NOT NULL,
    correoProveedor                              VARCHAR(150) NOT NULL,
    telefonoProveedor                            VARCHAR(30) NOT NULL,
    direccionProveedor                           VARCHAR(500) NOT NULL,
    estadoProveedor                              BOOLEAN NOT NULL DEFAULT 1,
    idPlazo                                      INT NULL,
    saldoActualProveedor                         DECIMAL(12,4) NOT NULL DEFAULT 0,
    saldoAnteriorProveedor                       DECIMAL(12,4) NOT NULL DEFAULT 0,
    cargosMesProveedor                           DECIMAL(12,4) NOT NULL DEFAULT 0,
    abonosMesProveedor                           DECIMAL(12,4) NOT NULL DEFAULT 0,
    cargosAsignadosProveedor                     DECIMAL(12,4) NOT NULL DEFAULT 0,
    cargosAcumuladosProveedor                    DECIMAL(12,4) NOT NULL DEFAULT 0,
    abonosAcumuladosProveedor                    DECIMAL(12,4) NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT Pk_Proveedores PRIMARY KEY(idProveedor),
    CONSTRAINT uqProveedorNit UNIQUE (nitProveedor),
    CONSTRAINT Fk_Proveedor_Plazo FOREIGN KEY (idPlazo) REFERENCES tblPlazo (idPlazo) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxProveedorPlazo (idPlazo),
    CONSTRAINT ckMovimientosProveedor CHECK (cargosMesProveedor >= 0 AND abonosMesProveedor >= 0 AND cargosAsignadosProveedor >= 0 AND cargosAcumuladosProveedor >= 0 AND abonosAcumuladosProveedor >= 0)
) ENGINE=InnoDB;
        
   -- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡ Logistica !!!!!!!!!!!!!!!!!!!!!!!!!-------------     

/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
CREATE TABLE tblEmpresaTransportista (
idEmpresaTransportista INT NOT NULL AUTO_INCREMENT,
 nombreEmpresaTransportista VARCHAR(100) NOT NULL,
 contactoEmpresaTransportista VARCHAR(100) NOT NULL,
 telefonoEmpresaTransportista VARCHAR(30) COLLATE utf8mb4_unicode_ci NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqEmpresaTransportistaNombre UNIQUE (nombreEmpresaTransportista),
    CONSTRAINT Pk_EmpresaTransportista PRIMARY KEY (idEmpresaTransportista)
) ENGINE=InnoDB;

/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblTipoServicio
CREATE TABLE tblTipoServicio (
idTipoServicio INT NOT NULL AUTO_INCREMENT,
 nombreTipoServicio VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqTipoServicioNombre UNIQUE (nombreTipoServicio),
    CONSTRAINT Pk_TipoServicio PRIMARY KEY (idTipoServicio)
) ENGINE=InnoDB;


/* 
--	Responsable:
--	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblTarifaTransporte
CREATE TABLE tblTarifaTransporte (
idTarifaTransporte INT NOT NULL AUTO_INCREMENT,
 nombreTarifaTransporte VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqTarifaTransporteNombre UNIQUE (nombreTarifaTransporte),
    CONSTRAINT Pk_TarifaTransporte PRIMARY KEY (idTarifaTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblEstado
CREATE TABLE tblEstado (
idEstado INT AUTO_INCREMENT NOT NULL,
    tipoEstado VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Estado PRIMARY KEY (idEstado),
    CONSTRAINT uqTipoEstado UNIQUE (tipoEstado)
) ENGINE=InnoDB;

/* 
	Responsable:
	BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblTipoTransporte
CREATE TABLE tblTipoTransporte (
idTipoTransporte INT AUTO_INCREMENT NOT NULL,
    nombreTipoTransporte VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_TipoTransporte PRIMARY KEY (idTipoTransporte),
    CONSTRAINT uqNombreTipoTransporte UNIQUE (nombreTipoTransporte)
) ENGINE=InnoDB;

/* 
	Responsable:
	BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblTransporte
CREATE TABLE tblTransporte (
idTransporte INT AUTO_INCREMENT NOT NULL,
    nroPlacaTransporte VARCHAR(30) NOT NULL,
    marcaTransporte VARCHAR(100) NOT NULL,
    modeloTransporte VARCHAR(100) NOT NULL,
    anioTransporte SMALLINT NOT NULL,
    esRefrigeradoTransporte BOOLEAN NOT NULL DEFAULT FALSE,
    Fk_Estado_Codigo INT NOT NULL,
    Fk_Tipo_Transporte_Codigo INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Transporte PRIMARY KEY (idTransporte),
    CONSTRAINT uqNroPlacaTransporte UNIQUE (nroPlacaTransporte),
    INDEX idxIdEstadoTransporte (Fk_Estado_Codigo),
    INDEX idxIdTipoTransporteTransporte (Fk_Tipo_Transporte_Codigo),
    CONSTRAINT Fk_Transporte_Estado FOREIGN KEY (Fk_Estado_Codigo) REFERENCES tblEstado(idEstado) ON DELETE RESTRICT,
    CONSTRAINT Fk_Transporte_Tipo_Transporte FOREIGN KEY (Fk_Tipo_Transporte_Codigo) REFERENCES tblTipoTransporte(idTipoTransporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

/* 
	Responsable:
	BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblCamaraRefrigeracion
CREATE TABLE tblCamaraRefrigeracion (
idCamaraRefrigeracion INT AUTO_INCREMENT NOT NULL,
    Fk_Transporte_Codigo INT NOT NULL,
    marcaEquipoCamaraRefrigeracion VARCHAR(100) NOT NULL,
    tempMinimaCelsiusCamaraRefrigeracion DECIMAL(5,2) NOT NULL,
    tempMaximaCelsiusCamaraRefrigeracion DECIMAL(5,2) NOT NULL,
    estadoOperativoCamaraRefrigeracion VARCHAR(30) NOT NULL DEFAULT 'OPERATIVO',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_CamaraRefrigeracion PRIMARY KEY (idCamaraRefrigeracion),
    CONSTRAINT uqIdTransporteCamaraRefrigeracion UNIQUE (Fk_Transporte_Codigo),
    CONSTRAINT Fk_Camara_Refrigeracion_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(idTransporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

/* 
	Responsable:
	BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblConductor
CREATE TABLE tblConductor (
idConductor INT AUTO_INCREMENT NOT NULL,
    codigoConductor VARCHAR(30) NOT NULL,
    nombreCompletoConductor VARCHAR(100) NOT NULL,
    numeroLicenciaConductor VARCHAR(30) NOT NULL,
    fechaVencimientoDocConductor DATE NOT NULL,
    Fk_Estado_Codigo INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Conductor PRIMARY KEY (idConductor),
    CONSTRAINT uqCodigoConductor UNIQUE (codigoConductor),
    CONSTRAINT uqNumeroLicenciaConductor UNIQUE (numeroLicenciaConductor),
    INDEX idxIdEstadoConductor (Fk_Estado_Codigo),
    CONSTRAINT Fk_Conductor_Estado FOREIGN KEY (Fk_Estado_Codigo) REFERENCES tblEstado(idEstado) ON DELETE RESTRICT
) ENGINE=InnoDB;


/*
	Responsable:
    BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblEncabezadoViaje
CREATE TABLE tblEncabezadoViaje (
idEncabezadoViaje BIGINT AUTO_INCREMENT NOT NULL,
    fechaDocumentoEncabezadoViaje DATETIME NOT NULL,
    horaSalidaEncabezadoViaje DATETIME NOT NULL,
    horaLlegadaEncabezadoViaje DATETIME NULL,
    requiereCadenaFrioEncabezadoViaje BOOLEAN NOT NULL DEFAULT TRUE,
    estadoViajeEncabezadoViaje VARCHAR(30) NOT NULL DEFAULT 'EN_CURSO',
    pesoTotalEncabezadoViaje DECIMAL(18,3) NOT NULL,
    Fk_Transporte_Codigo INT NOT NULL,
    Fk_Conductor_Codigo INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_EncabezadoViaje PRIMARY KEY (idEncabezadoViaje),
    INDEX idxIdTransporteEncabezadoViaje (Fk_Transporte_Codigo),
    INDEX idxIdConductorEncabezadoViaje (Fk_Conductor_Codigo),
    INDEX idxEstadoViajeEncabezadoViaje (estadoViajeEncabezadoViaje),
    CONSTRAINT Fk_Encabezado_Viaje_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(idTransporte) ON DELETE RESTRICT,
    CONSTRAINT Fk_Encabezado_Viaje_Conductor FOREIGN KEY (Fk_Conductor_Codigo) REFERENCES tblConductor(idConductor) ON DELETE RESTRICT
) ENGINE=InnoDB;


/*
	Responsable:
    BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblDetalleViaje
CREATE TABLE tblDetalleViaje (
idDetalleViaje BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Encabezado_Viaje_Codigo BIGINT NOT NULL,
    ordenDetalleViaje INT NOT NULL,
    descripcionDetalleViaje VARCHAR(255) NOT NULL,
    cantidadDetalleViaje DECIMAL(18,3) NOT NULL,
    pesoDetalleViaje DECIMAL(18,3) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_DetalleViaje PRIMARY KEY (idDetalleViaje),
    CONSTRAINT uqOrdenDetalleViaje UNIQUE (Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje),
    CONSTRAINT Fk_Detalle_Viaje_Encabezado_Viaje FOREIGN KEY (Fk_Encabezado_Viaje_Codigo) REFERENCES tblEncabezadoViaje(idEncabezadoViaje) ON DELETE CASCADE
) ENGINE=InnoDB;


/*
	Responsable:
    BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblTelemetriaTemperatura
CREATE TABLE tblTelemetriaTemperatura (
idTelemetriaTemperatura BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Encabezado_Viaje_Codigo BIGINT NOT NULL,
    fechaHoraTelemetriaTemperatura DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    temperaturaCelsiusTelemetriaTemperatura DECIMAL(5,2) NOT NULL,
    /* Porcentaje en formato decimal: 0.6500 = 65 % (DECIMAL(5,4) permite llegar a 100 %) */
    humedadRelativaTelemetriaTemperatura DECIMAL(5,4) NOT NULL,
    puertaAbiertaTelemetriaTemperatura BOOLEAN NOT NULL DEFAULT FALSE,
    ubicacionGpsTelemetriaTemperatura VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_TelemetriaTemperatura PRIMARY KEY (idTelemetriaTemperatura),
    INDEX idxCorrelativoFechaTelemetriaTemperatura (Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura),
    CONSTRAINT Fk_Telemetria_Temperatura_Encabezado_Viaje FOREIGN KEY (Fk_Encabezado_Viaje_Codigo) REFERENCES tblEncabezadoViaje(idEncabezadoViaje) ON DELETE CASCADE
) ENGINE=InnoDB;


/*
	Responsable:
    BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblAlertaCadenaFrio
CREATE TABLE tblAlertaCadenaFrio (
idAlertaCadenaFrio BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Telemetria_Temperatura_Codigo BIGINT NOT NULL,
    tipoAlertaCadenaFrio VARCHAR(30) NOT NULL,
    -- El viaje y la temperatura se consultan desde tblTelemetriaTemperatura.
    atendidaAlertaCadenaFrio BOOLEAN NOT NULL DEFAULT FALSE,
    observacionCalidadAlertaCadenaFrio VARCHAR(500) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_AlertaCadenaFrio PRIMARY KEY (idAlertaCadenaFrio),
    INDEX idxIdTelemetriaTemperaturaAlertaCadenaFrio (Fk_Telemetria_Temperatura_Codigo),
    INDEX idxAtendidaAlertaCadenaFrio (atendidaAlertaCadenaFrio),
    CONSTRAINT Fk_Alerta_Cadena_Frio_Telemetria_Temperatura FOREIGN KEY (Fk_Telemetria_Temperatura_Codigo) REFERENCES tblTelemetriaTemperatura(idTelemetriaTemperatura) ON DELETE CASCADE
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblSucursal
CREATE TABLE tblSucursal (
idSucursal INT NOT NULL AUTO_INCREMENT,
    nombreSucursal VARCHAR(100) NOT NULL,
    descripcionSucursal VARCHAR(255) NOT NULL,
    ubicacionSucursal VARCHAR(500) NOT NULL,
    paisSucursal VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Sucursal PRIMARY KEY (idSucursal)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblBodega
CREATE TABLE tblBodega (
idBodega INT NOT NULL AUTO_INCREMENT,
    nombreBodega VARCHAR(100) NOT NULL,
    Fk_Sucursal_Codigo INT NOT NULL,
    paisBodega VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	
    UNIQUE KEY uqBodegaSucursal (idBodega,Fk_Sucursal_Codigo),
    CONSTRAINT Fk_Bodega_Sucursal FOREIGN KEY (Fk_Sucursal_Codigo) REFERENCES tblSucursal(idSucursal) ON DELETE RESTRICT,
    CONSTRAINT Pk_Bodega PRIMARY KEY (idBodega)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblCategoria
CREATE TABLE tblCategoria (
idCategoria INT NOT NULL AUTO_INCREMENT,
    nombreCategoria VARCHAR(100) NOT NULL,
    porcentajeComisionCategoria DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqCategoriaNombre UNIQUE (nombreCategoria),
    CONSTRAINT ckPorcentajeComisionCategoriaRango CHECK (porcentajeComisionCategoria >= 0.00 AND porcentajeComisionCategoria <= 0.99),
    CONSTRAINT Pk_Categoria PRIMARY KEY (idCategoria)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblLinea
CREATE TABLE tblLinea (
idLinea INT NOT NULL AUTO_INCREMENT,
    nombreLinea VARCHAR(100) NOT NULL,
    porcentajeComisionLinea DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqLineaNombre UNIQUE (nombreLinea),
    CONSTRAINT ckPorcentajeComisionLineaRango CHECK (porcentajeComisionLinea >= 0.00 AND porcentajeComisionLinea <= 0.99),
    CONSTRAINT Pk_Linea PRIMARY KEY (idLinea)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblMarca
CREATE TABLE tblMarca (
idMarca INT NOT NULL AUTO_INCREMENT,
    nombreMarca VARCHAR(100) NOT NULL,
    porcentajeComisionMarca DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqMarcaNombre UNIQUE (nombreMarca),
    CONSTRAINT ckPorcentajeComisionMarcaRango CHECK (porcentajeComisionMarca >= 0.00 AND porcentajeComisionMarca <= 0.99),
    CONSTRAINT Pk_Marca PRIMARY KEY (idMarca)
) ENGINE=InnoDB;

-- tblProducto
CREATE TABLE tblProducto (
idProducto INT NOT NULL AUTO_INCREMENT,
    skuProducto VARCHAR(30) NOT NULL,
    clasificacionProducto CHAR(5) NOT NULL DEFAULT '?????',
    nombreProducto VARCHAR(100) NOT NULL,
    idProveedorProductos INT NOT NULL,
    Fk_Marca_Codigo INT NOT NULL,
    Fk_Categoria_Codigo INT NOT NULL,
    Fk_Linea_Codigo INT NOT NULL,
    descripcionProducto VARCHAR(255) NOT NULL,
    imagenProducto LONGBLOB NOT NULL, -- Campo BLOB para almacenar la imagen (Método de conversión de Img a binario en aplicación)
    paisProducto VARCHAR(100) NOT NULL,
    precioCostoProducto DECIMAL(12,4) NOT NULL,
    precioVentaProducto DECIMAL(12,4) NOT NULL,
    porcentajeComisionProducto DECIMAL(3,2) NOT NULL,
    -- Los saldos y movimientos no se almacenan aqui: se obtienen de tblStock.
    stockMinimoProducto INT NOT NULL ,
    stockMaximoProducto INT NOT NULL ,
    pesoProducto DECIMAL(4,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT uqProductoSku UNIQUE (skuProducto),
    INDEX idxProductoProveedorProductos (idProveedorProductos),
    CONSTRAINT Fk_Producto_Proveedor FOREIGN KEY (idProveedorProductos) REFERENCES tblProveedor(idProveedor) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Marca FOREIGN KEY (Fk_Marca_Codigo) REFERENCES tblMarca(idMarca) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Categoria FOREIGN KEY (Fk_Categoria_Codigo) REFERENCES tblCategoria(idCategoria) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Linea FOREIGN KEY (Fk_Linea_Codigo) REFERENCES tblLinea(idLinea) ON DELETE RESTRICT,
    CONSTRAINT ckPorcentajeComisionProductoRango CHECK (porcentajeComisionProducto >= 0.00 AND porcentajeComisionProducto <= 0.99),
    CONSTRAINT ckProductoMinMax CHECK (stockMinimoProducto >= 0 AND stockMaximoProducto >= stockMinimoProducto),
    CONSTRAINT Pk_Producto PRIMARY KEY (idProducto)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblLoteProducto
CREATE TABLE tblLoteProducto (
idLoteProducto BIGINT AUTO_INCREMENT NOT NULL,
    codigoLoteProducto VARCHAR(30) NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    descripcionLoteProducto VARCHAR(255) NOT NULL,
    requiereFrioLoteProducto BOOLEAN NOT NULL DEFAULT TRUE,
    fechaFabricacionLoteProducto DATE NOT NULL,
    fechaVencimientoLoteProducto DATE NOT NULL,
    tempRequeridaMinLoteProducto DECIMAL(5,2) NOT NULL,
    tempRequeridaMaxLoteProducto DECIMAL(5,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_LoteProducto PRIMARY KEY (idLoteProducto),
    CONSTRAINT uqCodigoLoteProducto UNIQUE (codigoLoteProducto),
    UNIQUE KEY uqLoteProductoRelacion (idLoteProducto, Fk_Producto_Codigo),
    INDEX idxLoteProducto (Fk_Producto_Codigo),
    CONSTRAINT Fk_Lote_Producto_Producto FOREIGN KEY (Fk_Producto_Codigo)
        REFERENCES tblProducto(idProducto) ON DELETE RESTRICT
) ENGINE=InnoDB;


/*
	Responsable:
    BRIAN ANDREE DE LA CRUZ SOSA 0901 23 6124
*/
-- tblDetalleViajeLoteProducto
CREATE TABLE tblDetalleViajeLoteProducto (
idDetalleViajeLoteProducto BIGINT NOT NULL AUTO_INCREMENT,
    Fk_Detalle_Viaje_Codigo BIGINT NOT NULL,
    Fk_Lote_Producto_Codigo BIGINT NOT NULL,
    cantidadCajasDetalleViajeLoteProducto INT NOT NULL,
    pesoKgDetalleViajeLoteProducto DECIMAL(18,3) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_DetalleViajeLoteProducto PRIMARY KEY (idDetalleViajeLoteProducto),
    CONSTRAINT uqDetalleViajeLoteProducto UNIQUE (Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo),
    INDEX idxIdLoteProductoDetalleViajeLoteProducto (Fk_Lote_Producto_Codigo),
    CONSTRAINT Fk_Detalle_Viaje_Lote_Producto_Detalle_Viaje FOREIGN KEY (Fk_Detalle_Viaje_Codigo) REFERENCES tblDetalleViaje(idDetalleViaje) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Viaje_Lote_Producto_Lote_Producto FOREIGN KEY (Fk_Lote_Producto_Codigo) REFERENCES tblLoteProducto(idLoteProducto) ON DELETE RESTRICT
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblStock: fuente unica de saldo actual por bodega y producto.
-- Se mantiene Fk_Sucursal_Codigo para compatibilidad con MVC y FK compuesta a tblBodega.
-- stockFisicoStock: ultimo conteo fisico observado.
-- stockVirtualStock y stockActualStock se mantienen por compatibilidad; la
-- aplicacion MVC debe mantenerlos consistentes segun las reglas del negocio.
CREATE TABLE tblStock (
idStock INT AUTO_INCREMENT NOT NULL,
Fk_Sucursal_Codigo INT NOT NULL,
    Fk_Bodega_Codigo INT NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    
    stockFisicoStock INT NOT NULL DEFAULT 0,
    stockVirtualStock INT NOT NULL DEFAULT 0,
    stockAnteriorStock INT NOT NULL DEFAULT 0,
    stockActualStock INT NOT NULL DEFAULT 0,
    cargosMesStock INT NOT NULL DEFAULT 0,
    abonosMesStock INT NOT NULL DEFAULT 0,
    cargosAcumuladosStock BIGINT NOT NULL DEFAULT 0,
    abonosAcumuladosStock BIGINT NOT NULL DEFAULT 0,
    apartadosStock INT NOT NULL DEFAULT 0,

    estadoVentaStock BOOLEAN NOT NULL,
    estadoCompraStock BOOLEAN NOT NULL,

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_Stock PRIMARY KEY (idStock),
    CONSTRAINT uqStockSucursalBodegaProducto UNIQUE (Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo),

    CONSTRAINT Fk_Stock_Bodega FOREIGN KEY (Fk_Bodega_Codigo, Fk_Sucursal_Codigo)
    REFERENCES tblBodega(idBodega, Fk_Sucursal_Codigo) ON DELETE RESTRICT,

    CONSTRAINT Fk_Stock_Producto FOREIGN KEY (Fk_Producto_Codigo)
    REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT ckEstadoVentaStock CHECK (estadoVentaStock IN (0, 1)),
    CONSTRAINT ckEstadoCompraStock CHECK (estadoCompraStock IN (0, 1)),
    CONSTRAINT ckStockActualNoNegativo CHECK (stockActualStock >= 0)
) ENGINE=InnoDB;


-- tblSaldos
-- Relacion asociativa entre productos y bodegas.
-- Un producto tiene un unico saldo por bodega.
-- tblStock.stockActualStock es la referencia del saldo actual;
-- la aplicacion MVC debe sincronizar tblSaldos sin triggers ni procedimientos.
CREATE TABLE tblSaldos (
    idSaldo INT NOT NULL AUTO_INCREMENT,
    Fk_Producto_Codigo INT NOT NULL,
    Fk_Bodega_Codigo INT NOT NULL,
    saldoActualSaldo INT NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Saldos PRIMARY KEY (idSaldo),
    CONSTRAINT uqSaldoProductoBodega UNIQUE (Fk_Producto_Codigo, Fk_Bodega_Codigo),
    CONSTRAINT Fk_Saldos_Producto FOREIGN KEY (Fk_Producto_Codigo)
        REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT Fk_Saldos_Bodega FOREIGN KEY (Fk_Bodega_Codigo)
        REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,
    CONSTRAINT ckSaldoActualNoNegativo CHECK (saldoActualSaldo >= 0)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblTipoMovimiento
CREATE TABLE tblTipoMovimiento (
idTipoMovimiento INT NOT NULL AUTO_INCREMENT,
    nombreTipoMovimiento VARCHAR(100) NOT NULL,
    descripcionTipoMovimiento VARCHAR(255) NOT NULL,
	signoTipoMovimiento CHAR(1) NOT NULL,

	created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqTipoMovimientoNombre UNIQUE (nombreTipoMovimiento),
    CONSTRAINT ckSignoTipoMovimiento CHECK (signoTipoMovimiento IN ('+', '-')),
    CONSTRAINT Pk_TipoMovimiento PRIMARY KEY (idTipoMovimiento)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	Jose Xavier Bolaños TENAS 0901 23 2128
 */ 
-- tblTraslado
CREATE TABLE tblTraslado (
idTraslado INT NOT NULL AUTO_INCREMENT,
    codigoTraslado VARCHAR(30) NOT NULL,
    Fk_Bodega_Origen_Codigo INT NOT NULL,
    Fk_Bodega_Destino_Codigo INT NOT NULL,
    Fk_Transporte_Codigo INT NULL,
    fechaSolicitudTraslado DATETIME NOT NULL,
    fechaSalidaTraslado DATETIME NULL,
    fechaLlegadaTraslado DATETIME NULL,
    estadoTraslado ENUM('SOLICITADO','EN_TRANSITO','RECIBIDO','CANCELADO') NOT NULL DEFAULT 'SOLICITADO',
    observacionesTraslado VARCHAR(500) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_Traslado PRIMARY KEY (idTraslado),
    UNIQUE KEY uqCodigoTraslado (codigoTraslado),
    CONSTRAINT Fk_Traslado_Bodega_Origen FOREIGN KEY (Fk_Bodega_Origen_Codigo) REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,
    CONSTRAINT Fk_Traslado_Bodega_Destino FOREIGN KEY (Fk_Bodega_Destino_Codigo) REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,
    CONSTRAINT Fk_Traslado_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(idTransporte) ON DELETE RESTRICT,
    CONSTRAINT ckTrasladoBodegasDistintas CHECK (Fk_Bodega_Origen_Codigo <> Fk_Bodega_Destino_Codigo) -- Origen y destino no pueden ser la misma bodega
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	Jose Xavier Bolaños TENAS 0901 23 2128
 */ 
-- tblDetalleTraslado
CREATE TABLE tblDetalleTraslado (
idDetalleTraslado INT NOT NULL AUTO_INCREMENT,
    Fk_Traslado_Codigo INT NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    cantidadDetalleTraslado INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DetalleTraslado PRIMARY KEY (idDetalleTraslado),
    CONSTRAINT uqDetalleTrasladoProducto UNIQUE (Fk_Traslado_Codigo, Fk_Producto_Codigo),
    CONSTRAINT Fk_Detalle_Traslado_Traslado FOREIGN KEY (Fk_Traslado_Codigo) REFERENCES tblTraslado(idTraslado) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Traslado_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT ckDetalleTrasladoCantidad CHECK (cantidadDetalleTraslado > 0)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	NILLS BERDUCIDO GOMEZ 0901 23 14275
*/ 
-- tblCierreMovimiento
CREATE TABLE tblCierreMovimiento (
idCierreMovimiento INT NOT NULL AUTO_INCREMENT,
    nombreCierreMovimiento VARCHAR(100) NOT NULL,
    Fk_Sucursal_Codigo INT NOT NULL,
    fechaInicioCierreMovimiento DATE NOT NULL,
    fechaFinCierreMovimiento DATE NOT NULL,
    estadoCierreMovimiento ENUM('ABIERTO', 'EN_PROCESO', 'CERRADO') NOT NULL DEFAULT 'ABIERTO',
    esCierreAnualCierreMovimiento BOOLEAN NOT NULL DEFAULT FALSE,
    toleranciaCierreMovimiento INT NOT NULL DEFAULT 0,
    diferenciaUnidadesCierreMovimiento INT NOT NULL DEFAULT 0,
    diferenciaMontoCierreMovimiento DECIMAL(12,4) NOT NULL DEFAULT 0,
    motivoAceptacionCierreMovimiento VARCHAR(500) NOT NULL,
    -- El cierre ejecutado no puede anularse; por eso no se guarda motivoAnulacion.
    observacionesCierreMovimiento VARCHAR(500) NOT NULL,
    fechaEjecucionCierreMovimiento DATETIME NULL,
    CONSTRAINT uqCierreMovimientoSucursalFin UNIQUE (Fk_Sucursal_Codigo, fechaFinCierreMovimiento, esCierreAnualCierreMovimiento),
    CONSTRAINT ckCierreMovimientoFechas CHECK (fechaInicioCierreMovimiento <= fechaFinCierreMovimiento),
    CONSTRAINT ckCierreEjecucion CHECK (estadoCierreMovimiento <> 'CERRADO' OR fechaEjecucionCierreMovimiento IS NOT NULL),
    CONSTRAINT Fk_Cierre_Movimiento_Sucursal FOREIGN KEY (Fk_Sucursal_Codigo)
        REFERENCES tblSucursal(idSucursal) ON DELETE RESTRICT,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_CierreMovimiento PRIMARY KEY (idCierreMovimiento)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblMovimientoInventario
CREATE TABLE tblMovimientoInventario (
idMovimientoInventario INT NOT NULL AUTO_INCREMENT,
    Fk_Tipo_Movimiento_Codigo INT NOT NULL,
-- agregar sucursales destino y origen 
    Fk_Bodega_Origen_Codigo INT NULL,
    Fk_Bodega_Destino_Codigo INT NULL,
    Fk_Transporte_Codigo INT NULL,
    Fk_Traslado_Codigo INT NULL COMMENT 'Relaciona movimientos de entrada/salida con su traslado',
    fechaMovimientoInventario DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    observacionesMovimientoInventario VARCHAR(500) NOT NULL,
    estadoMovimientoInventario ENUM('PENDIENTE', 'CUADRADO', 'CERRADO', 'ANULADO') NOT NULL DEFAULT 'PENDIENTE',
    Fk_Cierre_Movimiento_Codigo INT NULL COMMENT 'FK al cierre; permite NULL mientras el movimiento no se haya cerrado', 
    
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Fk_Movimiento_Inventario_Tipo_Movimiento FOREIGN KEY (Fk_Tipo_Movimiento_Codigo)
	REFERENCES tblTipoMovimiento(idTipoMovimiento) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Bodega_Origen FOREIGN KEY (Fk_Bodega_Origen_Codigo)
    REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Bodega_Destino FOREIGN KEY (Fk_Bodega_Destino_Codigo)
    REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Transporte FOREIGN KEY (Fk_Transporte_Codigo)
    REFERENCES tblTransporte(idTransporte) ON DELETE RESTRICT,
    CONSTRAINT Fk_Movimiento_Inventario_Traslado FOREIGN KEY (Fk_Traslado_Codigo)
        REFERENCES tblTraslado(idTraslado) ON DELETE RESTRICT,
    CONSTRAINT Fk_Movimiento_Inventario_Cierre_Movimiento FOREIGN KEY (Fk_Cierre_Movimiento_Codigo)
        REFERENCES tblCierreMovimiento(idCierreMovimiento) ON DELETE RESTRICT,
    CONSTRAINT Pk_MovimientoInventario PRIMARY KEY (idMovimientoInventario)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901  23 6269
 */ 
-- tblDetalleMovimiento
CREATE TABLE tblDetalleMovimiento (
idDetalleMovimiento INT NOT NULL AUTO_INCREMENT,
    Fk_Movimiento_Inventario_Codigo INT NOT NULL,
    Fk_Lote_Producto_Codigo BIGINT NULL COMMENT 'Opcional cuando un producto aun no tiene lote',
    Fk_Producto_Codigo INT NOT NULL,
    cantidadDetalleMovimiento INT NOT NULL,
    precioCostoDetalleMovimiento DECIMAL(12,4) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Fk_Detalle_Movimiento_Movimiento_Inventario FOREIGN KEY (Fk_Movimiento_Inventario_Codigo)
    REFERENCES tblMovimientoInventario(idMovimientoInventario) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Movimiento_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT Fk_Detalle_Movimiento_Lote_Producto FOREIGN KEY (Fk_Lote_Producto_Codigo, Fk_Producto_Codigo)
        REFERENCES tblLoteProducto(idLoteProducto, Fk_Producto_Codigo) ON DELETE RESTRICT,
    CONSTRAINT ckDetalleMovimientoCantidad CHECK (cantidadDetalleMovimiento > 0),
    CONSTRAINT Pk_DetalleMovimiento PRIMARY KEY (idDetalleMovimiento)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	CARLOS EDUARDO CHIN CAAL 0901 23 5686
*/ 
-- tblAuditoriaInventario
CREATE TABLE tblAuditoriaInventario (
idAuditoriaInventario INT NOT NULL AUTO_INCREMENT,
    Fk_Movimiento_Inventario_Codigo INT NULL COMMENT 'Opcional: una auditoria puede realizarse sin movimiento previo',
    Fk_Bodega_Codigo INT NOT NULL,
    tipoAuditoriaInventario ENUM('INDIVIDUAL', 'MULTIPLE') NOT NULL,
    estadoAuditoriaInventario ENUM('EN_PROCESO', 'FINALIZADA') NOT NULL DEFAULT 'EN_PROCESO',
    fechaAuditoriaInventario DATETIME NOT NULL,
    resultadoAuditoriaInventario VARCHAR(100) NOT NULL,
    observacionesCalidadAuditoriaInventario TEXT NOT NULL, -- Observaciones generales de la auditoria
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Fk_Auditoria_Inventario_Movimiento_Inventario FOREIGN KEY (Fk_Movimiento_Inventario_Codigo) REFERENCES tblMovimientoInventario(idMovimientoInventario) ON DELETE RESTRICT,
    CONSTRAINT Fk_Auditoria_Inventario_Bodega FOREIGN KEY (Fk_Bodega_Codigo) REFERENCES tblBodega(idBodega) ON DELETE RESTRICT,
    CONSTRAINT Pk_AuditoriaInventario PRIMARY KEY (idAuditoriaInventario)
) ENGINE=InnoDB;


-- tblDetalleAuditoria: conteo individualizado, sin repetir los detalles como texto.
CREATE TABLE tblDetalleAuditoria (
    idDetalleAuditoria INT NOT NULL AUTO_INCREMENT,
    Fk_Auditoria_Inventario_Codigo INT NOT NULL,
    Fk_Stock_Codigo INT NOT NULL,
    stockTeoricoDetalleAuditoria INT NOT NULL,
    stockFisicoDetalleAuditoria INT NOT NULL,
    diferenciaDetalleAuditoria INT GENERATED ALWAYS AS
        (stockFisicoDetalleAuditoria - stockTeoricoDetalleAuditoria) STORED NOT NULL,
    observacionesDetalleAuditoria VARCHAR(500) NOT NULL DEFAULT '',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqDetalleAuditoriaStock UNIQUE (Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo),
    CONSTRAINT Fk_Detalle_Auditoria_Auditoria_Inventario FOREIGN KEY (Fk_Auditoria_Inventario_Codigo)
        REFERENCES tblAuditoriaInventario(idAuditoriaInventario) ON DELETE RESTRICT,
    CONSTRAINT Fk_Detalle_Auditoria_Stock FOREIGN KEY (Fk_Stock_Codigo)
        REFERENCES tblStock(idStock) ON DELETE RESTRICT,
    CONSTRAINT ckDetalleAuditoriaStockTeorico CHECK (stockTeoricoDetalleAuditoria >= 0),
    CONSTRAINT ckDetalleAuditoriaStockFisico CHECK (stockFisicoDetalleAuditoria >= 0),
    CONSTRAINT Pk_DetalleAuditoria PRIMARY KEY (idDetalleAuditoria)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	NILLS BERDUCIDO GOMEZ 0901 23 14275
*/ 
-- tblDetalleCierre
CREATE TABLE tblDetalleCierre (
idDetalleCierre INT NOT NULL AUTO_INCREMENT,
    Fk_Cierre_Movimiento_Codigo INT NOT NULL,
    Fk_Stock_Codigo INT NOT NULL,
    stockAnteriorDetalleCierre INT NOT NULL DEFAULT 0,
    stockActualDetalleCierre INT NOT NULL DEFAULT 0,
    cargosMesDetalleCierre INT NOT NULL DEFAULT 0,
    abonosMesDetalleCierre INT NOT NULL DEFAULT 0,
    cargosAcumuladosDetalleCierre BIGINT NOT NULL DEFAULT 0,
    abonosAcumuladosDetalleCierre BIGINT NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Fk_Detalle_Cierre_Cierre_Movimiento FOREIGN KEY (Fk_Cierre_Movimiento_Codigo) REFERENCES tblCierreMovimiento(idCierreMovimiento) ON DELETE RESTRICT,
    CONSTRAINT Fk_Detalle_Cierre_Stock FOREIGN KEY (Fk_Stock_Codigo) REFERENCES tblStock(idStock) ON DELETE RESTRICT,
    CONSTRAINT uqDetalleCierreStock UNIQUE (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo),
    CONSTRAINT Pk_DetalleCierre PRIMARY KEY (idDetalleCierre)
) ENGINE=InnoDB;


/* 	RESPONSABLE:
	VELVETH SARAI CHAVEZ MEJIA 0901 23 6269
*/ 
-- tblHistoricoInventario: copia permanente de los valores de tblStock.
-- Por cada cierre se guarda una instantanea del stock, con los mismos campos
-- de cantidades y estados de tblStock. No se actualiza al cambiar tblStock.
-- Fk_Stock_Codigo conserva el idStock original; idHistoricoInventario es PK.
-- No modificar ni eliminar instantaneas desde la aplicacion luego del cierre.
CREATE TABLE tblHistoricoInventario (
    idHistoricoInventario BIGINT NOT NULL AUTO_INCREMENT,
    Fk_Cierre_Movimiento_Codigo INT NOT NULL,
    Fk_Stock_Codigo INT NOT NULL,
    Fk_Sucursal_Codigo INT NOT NULL,
    Fk_Bodega_Codigo INT NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,

    stockFisicoStock INT NOT NULL DEFAULT 0,
    stockVirtualStock INT NOT NULL DEFAULT 0,
    stockAnteriorStock INT NOT NULL DEFAULT 0,
    stockActualStock INT NOT NULL DEFAULT 0,
    cargosMesStock INT NOT NULL DEFAULT 0,
    abonosMesStock INT NOT NULL DEFAULT 0,
    cargosAcumuladosStock BIGINT NOT NULL DEFAULT 0,
    abonosAcumuladosStock BIGINT NOT NULL DEFAULT 0,
    apartadosStock INT NOT NULL DEFAULT 0,
    estadoVentaStock BOOLEAN NOT NULL,
    estadoCompraStock BOOLEAN NOT NULL,

    fechaCorteHistoricoInventario DATE NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_HistoricoInventario PRIMARY KEY (idHistoricoInventario),
    CONSTRAINT uqHistoricoInventarioCierreStock UNIQUE (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo),
    CONSTRAINT Fk_Historico_Inventario_Cierre_Movimiento
        FOREIGN KEY (Fk_Cierre_Movimiento_Codigo)
        REFERENCES tblCierreMovimiento(idCierreMovimiento) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Stock
        FOREIGN KEY (Fk_Stock_Codigo)
        REFERENCES tblStock(idStock) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Producto
        FOREIGN KEY (Fk_Producto_Codigo)
        REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Bodega
        FOREIGN KEY (Fk_Bodega_Codigo, Fk_Sucursal_Codigo)
        REFERENCES tblBodega(idBodega, Fk_Sucursal_Codigo) ON DELETE RESTRICT,
    CONSTRAINT ckHistoricoStockActualNoNegativo CHECK (stockActualStock >= 0),
    CONSTRAINT ckHistoricoEstadoVenta CHECK (estadoVentaStock IN (0, 1)),
    CONSTRAINT ckHistoricoEstadoCompra CHECK (estadoCompraStock IN (0, 1))
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblServicioTransporteExterno
CREATE TABLE tblServicioTransporteExterno (
idServicioTransporteExterno INT NOT NULL AUTO_INCREMENT,
 Fk_Empresa_Transportista_Codigo INT NOT NULL,
 Fk_Tipo_Servicio_Codigo INT NOT NULL,
 Fk_Traslado_Codigo INT NOT NULL,
 numeroGuiaServicioTransporteExterno VARCHAR(30) NOT NULL,
 estadoServicioTransporteExterno VARCHAR(30) NOT NULL DEFAULT 'Programado',
 origenServicioTransporteExterno VARCHAR(100) NOT NULL,
 direccionOrigenServicioTransporteExterno VARCHAR(500) NOT NULL,
 destinoServicioTransporteExterno VARCHAR(100) NOT NULL,
 direccionDestinoServicioTransporteExterno VARCHAR(500) NOT NULL,
 fechaContratacionServicioTransporteExterno DATE NOT NULL,
 fechaEstimadaEntregaServicioTransporteExterno DATE NOT NULL,
 fechaEntregaRealServicioTransporteExterno DATE NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqServicioNumeroGuia UNIQUE (numeroGuiaServicioTransporteExterno),
 CONSTRAINT Fk_Servicio_Transporte_Externo_Empresa_Transportista FOREIGN KEY (Fk_Empresa_Transportista_Codigo) REFERENCES tblEmpresaTransportista(idEmpresaTransportista) ON DELETE RESTRICT,
 CONSTRAINT Fk_Servicio_Transporte_Externo_Tipo_Servicio FOREIGN KEY (Fk_Tipo_Servicio_Codigo) REFERENCES tblTipoServicio(idTipoServicio) ON DELETE RESTRICT,
 CONSTRAINT Fk_Servicio_Transporte_Externo_Traslado FOREIGN KEY (Fk_Traslado_Codigo) REFERENCES tblTraslado(idTraslado) ON DELETE RESTRICT,
    CONSTRAINT ckServicioEstado CHECK (estadoServicioTransporteExterno IN ('Programado','En tránsito','Entregado','Cancelado')),
    CONSTRAINT ckServicioFechas CHECK (fechaEstimadaEntregaServicioTransporteExterno >= fechaContratacionServicioTransporteExterno),
    CONSTRAINT Pk_ServicioTransporteExterno PRIMARY KEY (idServicioTransporteExterno)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblCostoTransporte
CREATE TABLE tblCostoTransporte (
idCostoTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 Fk_Tarifa_Transporte_Codigo INT NOT NULL,
 cantidadCostoTransporte DECIMAL(12,4) NOT NULL,
 costoUnitarioCostoTransporte DECIMAL(12,4) NOT NULL,
 costoTotalCostoTransporte DECIMAL(12,4) GENERATED ALWAYS AS
     (ROUND(cantidadCostoTransporte * costoUnitarioCostoTransporte, 4)) STORED NOT NULL,
 monedaCostoTransporte CHAR(3) NOT NULL,
 tipoPagoCostoTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqCostoServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Costo_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
 CONSTRAINT Fk_Costo_Transporte_Tarifa_Transporte FOREIGN KEY (Fk_Tarifa_Transporte_Codigo) REFERENCES tblTarifaTransporte(idTarifaTransporte) ON DELETE RESTRICT,
    CONSTRAINT ckCostoCantidad CHECK (cantidadCostoTransporte > 0),
    CONSTRAINT ckCostoUnitario CHECK (costoUnitarioCostoTransporte >= 0),
    CONSTRAINT ckCostoMoneda CHECK (monedaCostoTransporte IN ('GTQ','USD')),
    CONSTRAINT ckCostoTipoPago CHECK (tipoPagoCostoTransporte IN ('Transferencia','Efectivo')),
    CONSTRAINT Pk_CostoTransporte PRIMARY KEY (idCostoTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblVehiculoTransporte
CREATE TABLE tblVehiculoTransporte (
idVehiculoTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 placaVehiculoTransporte VARCHAR(30) NOT NULL,
 tipoVehiculoTransporte VARCHAR(100) NOT NULL,
 capacidadVehiculoTransporte VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqVehiculoServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Vehiculo_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
    CONSTRAINT Pk_VehiculoTransporte PRIMARY KEY (idVehiculoTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblConductorTransporte
CREATE TABLE tblConductorTransporte (
idConductorTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 nombreConductorTransporte VARCHAR(100) NOT NULL,
 telefonoConductorTransporte VARCHAR(30) COLLATE utf8mb4_unicode_ci NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqConductorServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Conductor_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
    CONSTRAINT Pk_ConductorTransporte PRIMARY KEY (idConductorTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblProductoTransporte
CREATE TABLE tblProductoTransporte (
idProductoTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 Fk_Producto_Codigo INT NOT NULL,
 cantidadProductoTransporte DECIMAL(12,4) NOT NULL,
 unidadMedidaProductoTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Producto_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
 CONSTRAINT Fk_Producto_Transporte_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(idProducto) ON DELETE RESTRICT,
    CONSTRAINT ckProductoCantidad CHECK (cantidadProductoTransporte > 0),
    CONSTRAINT Pk_ProductoTransporte PRIMARY KEY (idProductoTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblUbicacionTransporte
CREATE TABLE tblUbicacionTransporte (
idUbicacionTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 ubicacionActualUbicacionTransporte VARCHAR(500) NOT NULL,
 fechaActualizacionUbicacionTransporte DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Ubicacion_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
 INDEX idxUbicacionServicioFecha (Fk_Servicio_Transporte_Externo_Codigo, fechaActualizacionUbicacionTransporte),
    CONSTRAINT Pk_UbicacionTransporte PRIMARY KEY (idUbicacionTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblIncidenciaTransporte
CREATE TABLE tblIncidenciaTransporte (
idIncidenciaTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 codigoIncidenciaTransporte VARCHAR(30) NOT NULL,
 fechaIncidenciaTransporte DATETIME NOT NULL,
 tipoIncidenciaTransporte VARCHAR(100) NOT NULL,
 descripcionIncidenciaTransporte TEXT NOT NULL, -- Relato amplio de incidencia, puede superar 255 caracteres
 responsableIncidenciaTransporte VARCHAR(100) NOT NULL,
 estadoIncidenciaTransporte VARCHAR(30) NOT NULL DEFAULT 'Pendiente',
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqIncidenciaCodigo UNIQUE (codigoIncidenciaTransporte),
 CONSTRAINT Fk_Incidencia_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
    CONSTRAINT Pk_IncidenciaTransporte PRIMARY KEY (idIncidenciaTransporte)
) ENGINE=InnoDB;


/* 
	Responsable:
	GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblHistorialTransporte
CREATE TABLE tblHistorialTransporte (
idHistorialTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 fechaMovimientoHistorialTransporte DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
 tipoMovimientoHistorialTransporte VARCHAR(100) NOT NULL,
 descripcionMovimientoHistorialTransporte VARCHAR(255) NOT NULL,
 estadoTransporteHistorialTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Historial_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
    CONSTRAINT Pk_HistorialTransporte PRIMARY KEY (idHistorialTransporte)
) ENGINE=InnoDB;


-- ============================================================================
-- tblEstadoSolicitudPago
-- Responsable: BRAYAN MOISES PINZON LOPEZ (carne incompleto en solicitud).
-- Catalogo de estados para las solicitudes de pago de transporte.
-- ============================================================================
CREATE TABLE tblEstadoSolicitudPago (
    idEstadoSolicitudPago INT NOT NULL AUTO_INCREMENT,
    codigoEstadoSolicitudPago VARCHAR(30) NOT NULL,
    nombreEstadoSolicitudPago VARCHAR(100) NOT NULL,
    descripcionEstadoSolicitudPago VARCHAR(255) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_EstadoSolicitudPago PRIMARY KEY (idEstadoSolicitudPago),
    CONSTRAINT uqCodigoEstadoSolicitudPago UNIQUE (codigoEstadoSolicitudPago)
) ENGINE=InnoDB;

-- Estado inicial utilizado por el valor predeterminado de tblSolicitudPagoTransporte.
INSERT INTO tblEstadoSolicitudPago
    (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES
    (1, 'SOLICITADO', 'Solicitado', 'Solicitud de pago registrada');


/*
    Responsable:
    GABRIELA PINTO GARCIA 9959 23 1087
*/
-- tblSolicitudPagoTransporte
CREATE TABLE tblSolicitudPagoTransporte (
idSolicitudPagoTransporte INT NOT NULL AUTO_INCREMENT,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 codigoSolicitudPagoTransporte VARCHAR(30) NOT NULL,
 Fk_Estado_Solicitud_Pago_Codigo INT NOT NULL DEFAULT 1,
 referenciaBancariaSolicitudPagoTransporte VARCHAR(100) NULL,
 fechaPagoSolicitudPagoTransporte DATE NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqSolicitudServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT uqSolicitudCodigo UNIQUE (codigoSolicitudPagoTransporte),
 INDEX idxSolicitudPagoEstado (Fk_Estado_Solicitud_Pago_Codigo),
 CONSTRAINT Fk_Solicitud_Pago_Transporte_Estado_Solicitud_Pago
     FOREIGN KEY (Fk_Estado_Solicitud_Pago_Codigo)
     REFERENCES tblEstadoSolicitudPago(idEstadoSolicitudPago) ON DELETE RESTRICT,
 CONSTRAINT Fk_Solicitud_Pago_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo)
  REFERENCES tblServicioTransporteExterno(idServicioTransporteExterno) ON DELETE CASCADE,
    CONSTRAINT Pk_SolicitudPagoTransporte PRIMARY KEY (idSolicitudPagoTransporte)
) ENGINE=InnoDB;
-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡  Modulo de compras !!!!!!!!!!!!!!!!!!!!!!!!!-------------



CREATE TABLE IF NOT EXISTS tblEncabezadoOrdenCompra (
    idOrdenCompra                                INT            NOT NULL AUTO_INCREMENT,
    idProveedor                                  INT            NOT NULL,
    idPlazo                                      INT            NOT NULL,
    fechaEmisionOrdenCompra                      DATE           NOT NULL,
    fechaVencimientoOrdenCompra                  DATE           NOT NULL,
    idMoneda                                     BIGINT         NOT NULL,
    idTipoCambio                                 BIGINT         NULL,
    tipoCambioOrdenCompra                        DECIMAL(12,6)  NOT NULL DEFAULT 1,
    subtotalOrdenCompra                          DECIMAL(12,4)  NOT NULL,
    descuentoOrdenCompra                         DECIMAL(12,4)  NOT NULL DEFAULT 0,
    impuestoOrdenCompra                          DECIMAL(12,4) NOT NULL DEFAULT 0,
    totalOrdenCompra                             DECIMAL(12,4)  NOT NULL,
    comentarioOrdenCompra                        VARCHAR(500)   NULL,
    estadoOrdenCompra                            VARCHAR(30)    NOT NULL DEFAULT 'Pendiente',
    bloqueadoOrdenCompra                         BOOLEAN NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_EncabezadoOrdenCompra PRIMARY KEY (idOrdenCompra),
    CONSTRAINT Fk_EncabezadoOrdenCompra_Proveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor (idProveedor),
    CONSTRAINT Fk_EncabezadoOrdenCompra_Plazo FOREIGN KEY (idPlazo) REFERENCES tblPlazo (idPlazo),
    INDEX idxOrdenCompraProveedor (idProveedor),
    INDEX idxOrdenCompraFecha (fechaEmisionOrdenCompra),
    CONSTRAINT Fk_EncabezadoOrdenCompra_Moneda FOREIGN KEY (idMoneda) REFERENCES tblMoneda (idMoneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_EncabezadoOrdenCompra_TipoCambio FOREIGN KEY (idTipoCambio) REFERENCES tblTipoCambio (idTipoCambio) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxOrdenCompraMoneda (idMoneda),
    INDEX idxOrdenCompraTipoCambio (idTipoCambio),
    CONSTRAINT ckReferenciaTipoCambioOrdenCompra CHECK (idTipoCambio IS NOT NULL OR tipoCambioOrdenCompra = 1),
    CONSTRAINT ckTipoCambioOrdenCompra CHECK (tipoCambioOrdenCompra > 0),
    CONSTRAINT ckSubtotalOrdenCompra CHECK (subtotalOrdenCompra >= 0),
    CONSTRAINT ckDescuentoOrdenCompra CHECK (descuentoOrdenCompra >= 0),
    CONSTRAINT ckImpuestoOrdenCompra CHECK (impuestoOrdenCompra >= 0),
    CONSTRAINT ckImportesOrdenCompra CHECK (descuentoOrdenCompra <= subtotalOrdenCompra AND totalOrdenCompra = subtotalOrdenCompra - descuentoOrdenCompra + impuestoOrdenCompra),
    CONSTRAINT ckTotalOrdenCompra CHECK (totalOrdenCompra >= 0),
    CONSTRAINT ckFechasOrdenCompra CHECK (fechaVencimientoOrdenCompra >= fechaEmisionOrdenCompra),
    CONSTRAINT ckEstadoOrdenCompra CHECK (estadoOrdenCompra IN ('Pendiente','Autorizada','Rechazada','Anulada','Recibida'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblDetalleOrdenCompra (
    idOrdenCompra                                INT            NOT NULL,
    ordenDetalleOrdenCompra                      INT NOT NULL,
    idProducto                                   INT            NOT NULL,
    idBodega                                     INT            NOT NULL,
    cantidadDetalleOrdenCompra                   DECIMAL(12,4)  NOT NULL,
    costoUnitarioDetalleOrdenCompra              DECIMAL(12,4) NOT NULL,
    descuentoPorcentajeDetalleOrdenCompra        DECIMAL(5,2)  NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DetalleOrdenCompra PRIMARY KEY (idOrdenCompra, ordenDetalleOrdenCompra, idProducto),
    CONSTRAINT Fk_DetalleOrdenCompra_Encabezado FOREIGN KEY (idOrdenCompra) REFERENCES tblEncabezadoOrdenCompra (idOrdenCompra),
    CONSTRAINT Fk_DetalleOrdenCompra_Producto FOREIGN KEY (idProducto) REFERENCES tblProducto (idProducto) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleOrdenCompraProducto (idProducto),
    CONSTRAINT Fk_DetalleOrdenCompra_Bodega FOREIGN KEY (idBodega) REFERENCES tblBodega (idBodega) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleOrdenCompraBodega (idBodega),
    CONSTRAINT ckOrdenDetalleOrdenCompra CHECK (ordenDetalleOrdenCompra > 0),
    CONSTRAINT uqOrdenDetalleOrdenCompra UNIQUE (idOrdenCompra, ordenDetalleOrdenCompra),
    CONSTRAINT ckCantidadDetalleOrdenCompra CHECK (cantidadDetalleOrdenCompra > 0),
    CONSTRAINT ckCostoDetalleOrdenCompra CHECK (costoUnitarioDetalleOrdenCompra >= 0),
    CONSTRAINT ckDescPorcDetalleOrdenCompra CHECK (descuentoPorcentajeDetalleOrdenCompra BETWEEN 0 AND 100)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblEncabezadoCompra (
    idCompra                                     INT            NOT NULL AUTO_INCREMENT,
    idOrdenCompra                                INT            NULL,
    idProveedor                                  INT            NOT NULL,
    serieFacturaCompra                           VARCHAR(30)    NOT NULL,
    numeroFacturaCompra                          VARCHAR(30)    NOT NULL,
    fechaCompra                                  DATE           NOT NULL,
    idMetodoPago                                 INT            NOT NULL,
    idMoneda                                     BIGINT         NOT NULL,
    idTipoCambio                                 BIGINT         NULL,
    tipoCambioCompra                             DECIMAL(12,6)  NOT NULL DEFAULT 1,
    subtotalCompra                               DECIMAL(12,4)  NOT NULL,
    descuentoCompra                              DECIMAL(12,4)  NOT NULL DEFAULT 0,
    impuestoCompra                               DECIMAL(12,4) NOT NULL DEFAULT 0,
    totalCompra                                  DECIMAL(12,4)  NOT NULL,
    estadoCompra                                 VARCHAR(30)    NOT NULL DEFAULT 'Registrada',
    bloqueadoCompra                              BOOLEAN NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_EncabezadoCompra PRIMARY KEY (idCompra),
    CONSTRAINT Fk_EncabezadoCompra_EncabezadoOrdenCompra FOREIGN KEY (idOrdenCompra) REFERENCES tblEncabezadoOrdenCompra (idOrdenCompra),
    CONSTRAINT Fk_EncabezadoCompra_Proveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor (idProveedor),
    INDEX idxCompraOrden (idOrdenCompra),
    INDEX idxCompraProveedor (idProveedor),
    CONSTRAINT uqFacturaProveedorCompra UNIQUE (idProveedor, serieFacturaCompra, numeroFacturaCompra),
    CONSTRAINT Fk_EncabezadoCompra_Moneda FOREIGN KEY (idMoneda) REFERENCES tblMoneda (idMoneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_EncabezadoCompra_TipoCambio FOREIGN KEY (idTipoCambio) REFERENCES tblTipoCambio (idTipoCambio) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_EncabezadoCompra_MetodoPago FOREIGN KEY (idMetodoPago) REFERENCES tblMetodoPago(idMetodoPago),
    INDEX idxCompraMoneda (idMoneda),
    INDEX idxCompraTipoCambio (idTipoCambio),
    CONSTRAINT ckReferenciaTipoCambioCompra CHECK (idTipoCambio IS NOT NULL OR tipoCambioCompra = 1),
    CONSTRAINT ckTipoCambioCompra CHECK (tipoCambioCompra > 0),
    CONSTRAINT ckSubtotalCompra CHECK (subtotalCompra >= 0),
    CONSTRAINT ckDescuentoCompra CHECK (descuentoCompra >= 0),
    CONSTRAINT ckImpuestoCompra CHECK (impuestoCompra >= 0),
    CONSTRAINT ckImportesCompra CHECK (descuentoCompra <= subtotalCompra AND totalCompra = subtotalCompra - descuentoCompra + impuestoCompra),
    CONSTRAINT ckTotalCompra CHECK (totalCompra >= 0),
    CONSTRAINT ckEstadoCompra CHECK (estadoCompra IN ('Registrada','Anulada'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblDetalleCompra (
    idCompra                                     INT            NOT NULL,
    ordenDetalleCompra                           INT NOT NULL,
    idProducto                                   INT            NOT NULL,
    idBodega                                     INT            NOT NULL,
    cantidadDetalleCompra                        DECIMAL(12,4)  NOT NULL,
    costoUnitarioDetalleCompra                   DECIMAL(12,4)  NOT NULL,
    descuentoPorcentualDetalleCompra             DECIMAL(3,2)  NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DetalleCompra PRIMARY KEY (idCompra, ordenDetalleCompra, idProducto),
    CONSTRAINT Fk_DetalleCompra_Encabezado FOREIGN KEY (idCompra) REFERENCES tblEncabezadoCompra (idCompra),
    CONSTRAINT Fk_DetalleCompra_Producto FOREIGN KEY (idProducto) REFERENCES tblProducto (idProducto) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleCompraProducto (idProducto),
    CONSTRAINT Fk_DetalleCompra_Bodega FOREIGN KEY (idBodega) REFERENCES tblBodega (idBodega) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleCompraBodega (idBodega),
    CONSTRAINT ckOrdenDetalleCompra CHECK (ordenDetalleCompra > 0),
    CONSTRAINT uqOrdenDetalleCompra UNIQUE (idCompra, ordenDetalleCompra),
    CONSTRAINT ckCantidadDetalleCompra CHECK (cantidadDetalleCompra > 0),
    CONSTRAINT ckCostoDetalleCompra CHECK (costoUnitarioDetalleCompra >= 0),
    CONSTRAINT ckDescuentoDetalleCompra CHECK (descuentoPorcentualDetalleCompra BETWEEN 0 AND 0.99)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblCuentaPorPagar(
    idProveedor                                  INT NOT NULL,
    idConceptoProveedor                          INT NOT NULL,
    serieDocumentoCuentaPorPagar                 VARCHAR(20) NOT NULL,
    numeroDocumentoCuentaPorPagar                VARCHAR(30) NOT NULL,
    fechaEmisionCuentaPorPagar                   DATE NOT NULL,
    fechaVencimientoCuentaPorPagar               DATE NOT NULL,
    idMoneda                                     BIGINT         NOT NULL,
    idTipoCambio                                 BIGINT         NULL,
    tipoCambioCuentaPorPagar                     DECIMAL(12,6) NOT NULL DEFAULT 1,
    montoTotalCuentaPorPagar                     DECIMAL(12,4) NOT NULL,
    saldoActualCuentaPorPagar                    DECIMAL(12,4) NOT NULL,
    estadoCuentaPorPagar                         VARCHAR(30) NOT NULL DEFAULT 'Pendiente',
    bloqueadoCuentaPorPagar                      BOOLEAN NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT Pk_CuentaPorPagar PRIMARY KEY (serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor),
    CONSTRAINT Fk_Proveedor_idProveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor(idProveedor),
    CONSTRAINT Fk_CuentaPorPagar_Concepto FOREIGN KEY (idConceptoProveedor) REFERENCES tblConceptoProveedor (idConceptoProveedor),
    INDEX idxCuentaPorPagarVencimiento (fechaVencimientoCuentaPorPagar),
    CONSTRAINT Fk_CuentaPorPagar_Moneda FOREIGN KEY (idMoneda) REFERENCES tblMoneda (idMoneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_CuentaPorPagar_TipoCambio FOREIGN KEY (idTipoCambio) REFERENCES tblTipoCambio (idTipoCambio) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxCuentaPorPagarMoneda (idMoneda),
    INDEX idxCuentaPorPagarTipoCambio (idTipoCambio),
    CONSTRAINT ckReferenciaTipoCambioCuentaPorPagar CHECK (idTipoCambio IS NOT NULL OR tipoCambioCuentaPorPagar = 1),
    CONSTRAINT ckTipoCambioCuentaPorPagar CHECK (tipoCambioCuentaPorPagar > 0),
    CONSTRAINT ckMontoTotalCuentaPorPagar CHECK (montoTotalCuentaPorPagar <> 0),
    CONSTRAINT ckFechasCuentaPorPagar CHECK (fechaVencimientoCuentaPorPagar >= fechaEmisionCuentaPorPagar),
    CONSTRAINT ckEstadoCuentaPorPagar CHECK (estadoCuentaPorPagar IN ('Pendiente','Parcial','Pagada','Anulada'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblDevolucionCompra (
    idDevolucionCompra                           INT            NOT NULL AUTO_INCREMENT,
    idCompra                                     INT            NOT NULL,
    fechaDevolucionCompra                        DATE           NOT NULL,
    motivoDevolucionCompra                       VARCHAR(500)   NOT NULL,
    serieNotaCreditoDevolucionCompra             VARCHAR(20) NULL,
    numeroNotaCreditoDevolucionCompra            VARCHAR(30) NULL,
    totalDevolucionCompra                        DECIMAL(12,4)  NOT NULL,
    estadoDevolucionCompra                       VARCHAR(30)    NOT NULL DEFAULT 'Registrada',
    bloqueadoDevolucionCompra                    BOOLEAN NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DevolucionCompra PRIMARY KEY (idDevolucionCompra),
    CONSTRAINT Fk_DevolucionCompra_Compra FOREIGN KEY (idCompra) REFERENCES tblEncabezadoCompra (idCompra),
    INDEX idxDevolucionCompraCompra (idCompra),
    CONSTRAINT uqNotaCreditoDevolucionCompra UNIQUE (idCompra, serieNotaCreditoDevolucionCompra, numeroNotaCreditoDevolucionCompra),
    CONSTRAINT ckTotalDevolucionCompra CHECK (totalDevolucionCompra > 0),
    CONSTRAINT ckEstadoDevolucionCompra CHECK (estadoDevolucionCompra IN ('Registrada','Anulada'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblDetalleDevolucionCompra (
    idDevolucionCompra                           INT            NOT NULL,
    ordenDetalleDevolucionCompra                 INT NOT NULL,
    idProducto                                   INT            NOT NULL,
    idBodega                                     INT            NOT NULL,
    cantidadDetalleDevolucionCompra              DECIMAL(12,4)  NOT NULL,
    costoUnitarioDetalleDevolucionCompra         DECIMAL(12,4)  NOT NULL,
    descuentoPorcentajeDetalleDevolucionCompra   DECIMAL(3,2) NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DetalleDevolucionCompra PRIMARY KEY (idDevolucionCompra, ordenDetalleDevolucionCompra, idProducto),
    CONSTRAINT Fk_DetalleDevolucionCompra_Devolucion FOREIGN KEY (idDevolucionCompra) REFERENCES tblDevolucionCompra (idDevolucionCompra),
    CONSTRAINT Fk_DetalleDevolucionCompra_Producto FOREIGN KEY (idProducto) REFERENCES tblProducto (idProducto) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleDevolucionCompraProducto (idProducto),
    CONSTRAINT Fk_DetalleDevolucionCompra_Bodega FOREIGN KEY (idBodega) REFERENCES tblBodega (idBodega) ON DELETE RESTRICT ON UPDATE RESTRICT,
    INDEX idxDetalleDevolucionCompraBodega (idBodega),
    CONSTRAINT ckOrdenDetalleDevolucionCompra CHECK (ordenDetalleDevolucionCompra > 0),
    CONSTRAINT uqOrdenDetalleDevolucionCompra UNIQUE (idDevolucionCompra, ordenDetalleDevolucionCompra),
    CONSTRAINT ckCantidadDetalleDevolucionCompra CHECK (cantidadDetalleDevolucionCompra > 0),
    CONSTRAINT ckDescuentoDetalleDevolucionCompra CHECK (descuentoPorcentajeDetalleDevolucionCompra BETWEEN 0 AND 0.99),
    CONSTRAINT ckCostoDetalleDevolucionCompra CHECK (costoUnitarioDetalleDevolucionCompra >= 0)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblEncabezadoPagoProveedor (
    idPagoProveedor                              INT            NOT NULL AUTO_INCREMENT,
    idProveedor                                  INT            NOT NULL,
    fechaPagoProveedor                           DATE           NOT NULL,
    montoPagoProveedor                           DECIMAL(12,4)  NOT NULL,
    idMetodoPago                                 INT            NOT NULL,
    idMoneda                                     BIGINT         NOT NULL,
    idTipoCambio                                 BIGINT         NULL,
    tipoCambioPagoProveedor                      DECIMAL(12,6)  NOT NULL DEFAULT 1,
    referenciaPagoProveedor                      VARCHAR(30)    NULL,
    estadoPagoProveedor                          VARCHAR(30)    NOT NULL DEFAULT 'Programado',
    bloqueadoPagoProveedor                       BOOLEAN NOT NULL DEFAULT 0,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_EncabezadoPagoProveedor PRIMARY KEY (idPagoProveedor),
    CONSTRAINT Fk_EncabezadoPagoProveedor_Proveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor (idProveedor),
    INDEX idxPagoProveedorProveedor (idProveedor),
    INDEX idxPagoProveedorFecha (fechaPagoProveedor),
    CONSTRAINT ckMontoPagoProveedor CHECK (montoPagoProveedor > 0),
    CONSTRAINT Fk_EncabezadoPagoProveedor_Moneda FOREIGN KEY (idMoneda) REFERENCES tblMoneda (idMoneda) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_EncabezadoPagoProveedor_TipoCambio FOREIGN KEY (idTipoCambio) REFERENCES tblTipoCambio (idTipoCambio) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_EncabezadoPagoProveedor_MetodoPago FOREIGN KEY (idMetodoPago) REFERENCES tblMetodoPago(idMetodoPago),
    INDEX idxPagoProveedorMoneda (idMoneda),
    INDEX idxPagoProveedorTipoCambio (idTipoCambio),
    CONSTRAINT ckReferenciaTipoCambioPagoProveedor CHECK (idTipoCambio IS NOT NULL OR tipoCambioPagoProveedor = 1),
    CONSTRAINT ckTipoCambioPagoProveedor CHECK (tipoCambioPagoProveedor > 0),
    CONSTRAINT ckEstadoPagoProveedor CHECK (estadoPagoProveedor IN ('Pagado','Programado','Rechazado','Anulado'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblDetallePagoProveedor (
    idPagoProveedor                              INT            NOT NULL,
    serieDocumentoCuentaPorPagar                 VARCHAR(20) NOT NULL,
    numeroDocumentoCuentaPorPagar                VARCHAR(30) NOT NULL,
    idConceptoProveedor                          INT NOT NULL,
    montoAplicadoDetallePago                     DECIMAL(12,4)  NOT NULL,
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_DetallePagoProveedor PRIMARY KEY (idPagoProveedor, serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor),
    CONSTRAINT Fk_DetallePago_Encabezado FOREIGN KEY (idPagoProveedor) REFERENCES tblEncabezadoPagoProveedor (idPagoProveedor),
    CONSTRAINT Fk_DetallePago_CuentaPorPagar FOREIGN KEY (serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor) REFERENCES tblCuentaPorPagar (serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor),
    INDEX idxDetallePagoCuenta (serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor),
    CONSTRAINT ckMontoAplicadoDetallePago CHECK (montoAplicadoDetallePago > 0)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tblSolicitudPago (
    idSolicitudPago                              INT            NOT NULL AUTO_INCREMENT,
    moduloOrigenSolicitudPago                    VARCHAR(30)    NOT NULL,
    idOrigenSolicitudPago                        INT            NOT NULL,
    beneficiarioSolicitudPago                    VARCHAR(100)   NOT NULL,
    montoSolicitudPago                           DECIMAL(12,4)  NOT NULL,
    metodoSolicitudPago                          VARCHAR(30)    NOT NULL,
    estadoSolicitudPago                          VARCHAR(30)    NOT NULL DEFAULT 'Pendiente',
    created_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                                   TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_SolicitudPago PRIMARY KEY (idSolicitudPago),
    INDEX idxSolicitudPagoOrigen (moduloOrigenSolicitudPago, idOrigenSolicitudPago),
    CONSTRAINT ckMontoSolicitudPago CHECK (montoSolicitudPago > 0),
    CONSTRAINT ckEstadoSolicitudPago CHECK (estadoSolicitudPago IN ('Pendiente','Procesada','Rechazada'))
) ENGINE=InnoDB;

         
-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡ Ventas y Cuentas por Cobrar !!!!!!!!!!!!!!!!!!!!!!!!!-------------
-- tblVendedor | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7009
CREATE TABLE `tblVendedor` (
  `idVendedor` INT NOT NULL AUTO_INCREMENT,
  `codigoVendedor` VARCHAR(30) NOT NULL,
  `idEmpleado` INT NOT NULL,
  `activoVendedor` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_Vendedor` PRIMARY KEY (`idVendedor`),
  CONSTRAINT `uqVendedorcodigo` UNIQUE (`codigoVendedor`),
  CONSTRAINT `uqVendedorEmpleadoCodigo` UNIQUE (`idEmpleado`),
  CONSTRAINT `ckVendedoractivoVendedorBooleano` CHECK (activoVendedor IN (0,1))
) ENGINE=InnoDB;

ALTER TABLE `tblVendedor`
  ADD CONSTRAINT `Fk_Vendedor_Empleado` FOREIGN KEY (`idEmpleado`)
    REFERENCES `tblEmpleado` (`idEmpleado`) ON DELETE RESTRICT ON UPDATE RESTRICT;
-- tblCorrelativoVenta | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7000
CREATE TABLE `tblCorrelativoVenta` (
  `idCorrelativoVenta` INT NOT NULL AUTO_INCREMENT,
  `prefijoCorrelativoVenta` VARCHAR(30) NOT NULL,
  `ultimoNumeroCorrelativoVenta` BIGINT NOT NULL DEFAULT 0,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CorrelativoVenta` PRIMARY KEY (`idCorrelativoVenta`),
  CONSTRAINT `ckCorrelativoVentaNumeroNoNegativo` CHECK (ultimoNumeroCorrelativoVenta >= 0),
  CONSTRAINT `uqCorrelativoVentaClaveNegocio` UNIQUE (`prefijoCorrelativoVenta`)
) ENGINE=InnoDB;

-- tblCliente | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7009
CREATE TABLE `tblCliente` (
  `idCliente` INT NOT NULL AUTO_INCREMENT,
  `codigoCliente` VARCHAR(30) NOT NULL,
  `tipoIdentificacionCliente` ENUM('NIT','CUI','CF') NOT NULL,
  `nombreCliente` VARCHAR(100) NOT NULL,
  `direccionCliente` VARCHAR(500) NULL,
  `nombreContactoCliente` VARCHAR(100) NULL,
  `telefonoCliente` VARCHAR(30) NULL,
  `correoCliente` VARCHAR(150) NULL,
  `diasCreditoCliente` INT NOT NULL DEFAULT 0,
  `limiteCreditoCliente` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `idMonedaCredito` BIGINT NOT NULL,
  `activoCliente` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `nitCliente` CHAR(10) NULL,
  `dpiCliente` CHAR(13) NULL,
  CONSTRAINT `Pk_Cliente` PRIMARY KEY (`idCliente`),
  CONSTRAINT `uqClientecodigo` UNIQUE (`codigoCliente`),
  CONSTRAINT `uqClientenit` UNIQUE (`nitCliente`),
  CONSTRAINT `uqClientedpi` UNIQUE (`dpiCliente`),
  KEY `idxClienteMonedaCredito` (`idMonedaCredito`),
  CONSTRAINT `ckClienteactivoClienteBooleano` CHECK (activoCliente IN (0,1)),
  CONSTRAINT `ckClienteIdentificacionCoherente` CHECK ((tipoIdentificacionCliente = 'CF' AND nitCliente IS NULL AND dpiCliente IS NULL) OR (tipoIdentificacionCliente = 'NIT' AND nitCliente IS NOT NULL AND dpiCliente IS NULL) OR (tipoIdentificacionCliente = 'CUI' AND dpiCliente IS NOT NULL AND nitCliente IS NULL)),
  CONSTRAINT `ckClienteCreditoNoNegativo` CHECK (diasCreditoCliente >= 0 AND limiteCreditoCliente >= 0)
) ENGINE=InnoDB;

ALTER TABLE `tblCliente`
  ADD CONSTRAINT `Fk_Cliente_Moneda_Credito` FOREIGN KEY (`idMonedaCredito`)
    REFERENCES `tblMoneda` (`idMoneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblTipoCliente | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblTipoCliente` (
  `idTipoCliente` INT NOT NULL AUTO_INCREMENT,
  `codigoTipoCliente` VARCHAR(30) NOT NULL,
  `nombreTipoCliente` VARCHAR(100) NOT NULL,
  `porcentajeDescuentoTipoCliente` DECIMAL(5,2) NOT NULL DEFAULT 0.00,
  `descripcionTipoCliente` VARCHAR(255) NULL,
  `activoTipoCliente` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_TipoCliente` PRIMARY KEY (`idTipoCliente`),
  CONSTRAINT `uqTipoClientecodigo` UNIQUE (`codigoTipoCliente`),
  CONSTRAINT `uqTipoClientenombre` UNIQUE (`nombreTipoCliente`),
  CONSTRAINT `ckTipoClienteporcentajeDescuentoRango` CHECK (`porcentajeDescuentoTipoCliente` BETWEEN 0 AND 100),
  CONSTRAINT `ckTipoClienteactivoBooleano` CHECK (`activoTipoCliente` IN (0,1))
) ENGINE=InnoDB;

-- tblListaPrecioEncabezado | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblListaPrecioEncabezado` (
  `idListaPrecioEncabezado` INT NOT NULL AUTO_INCREMENT,
  `idTipoCliente` INT NOT NULL,
  `codigoListaPrecioEncabezado` VARCHAR(30) NOT NULL,
  `nombreListaPrecioEncabezado` VARCHAR(100) NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `vigenteDesdeListaPrecioEncabezado` DATE NOT NULL,
  `vigenteHastaListaPrecioEncabezado` DATE NULL,
  `activoListaPrecioEncabezado` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ListaPrecioEncabezado` PRIMARY KEY (`idListaPrecioEncabezado`),
  CONSTRAINT `uqListaPrecioEncabezadocodigo` UNIQUE (`codigoListaPrecioEncabezado`),
  KEY `idxListaPrecioEncabezadoTipoCliente` (`idTipoCliente`),
  KEY `idxListaPrecioEncabezadoMonedaCodigo` (`idMoneda`),
  CONSTRAINT `ckListaPrecioEncabezadoactivoListaPrecioEncabezadoBooleano` CHECK (activoListaPrecioEncabezado IN (0,1)),
  CONSTRAINT `ckListaPrecioEncabezadoFechasCoherentes` CHECK (vigenteHastaListaPrecioEncabezado >= vigenteDesdeListaPrecioEncabezado)
) ENGINE=InnoDB;

ALTER TABLE `tblListaPrecioEncabezado`
  ADD CONSTRAINT `Fk_ListaPrecioEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`idMoneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ListaPrecioEncabezado_TipoCliente` FOREIGN KEY (`idTipoCliente`)
    REFERENCES `tblTipoCliente` (`idTipoCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblPrecioTipoCliente | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblPrecioTipoCliente` (
  `idPrecioTipoCliente` INT NOT NULL AUTO_INCREMENT,
  `idTipoCliente` INT NOT NULL,
  `idProducto` INT NOT NULL,
  `porcentajeDescuento` DECIMAL(5,2) NOT NULL DEFAULT 0.00,
  `precioBase` DECIMAL(12,4) NOT NULL,
  `precioFinal` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(precioBase * (1 - (porcentajeDescuento / 100)), 4)) STORED,
  `activo` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_PrecioTipoCliente` PRIMARY KEY (`idPrecioTipoCliente`),
  CONSTRAINT `uqPrecioTipoCliente_TipoClienteProducto` UNIQUE (`idTipoCliente`, `idProducto`),
  CONSTRAINT `ckPrecioTipoClienteDescuentoRango` CHECK (`porcentajeDescuento` BETWEEN 0 AND 100)
) ENGINE=InnoDB;

ALTER TABLE `tblPrecioTipoCliente`
  ADD CONSTRAINT `Fk_PrecioTipoCliente_TipoCliente` FOREIGN KEY (`idTipoCliente`) REFERENCES `tblTipoCliente` (`idTipoCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_PrecioTipoCliente_Producto` FOREIGN KEY (`idProducto`) REFERENCES `tblProducto` (`idProducto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblHistorialModificacionPrecio | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblHistorialModificacionPrecio` (
  `idHistorialModificacionPrecio` INT NOT NULL AUTO_INCREMENT,
  `idTipoCliente` INT NOT NULL,
  `tipoModificacion` ENUM('DESCUENTO_PORCENTAJE', 'AUMENTO_PORCENTAJE', 'AUMENTO_BASE') NOT NULL,
  `valorModificacion` DECIMAL(12,4) NOT NULL,
  `alcanceModificacion` ENUM('INDIVIDUAL', 'SELECCIONADOS', 'TODOS_FILTROS') NOT NULL,
  `idMarca` INT NULL,
  `idLinea` INT NULL,
  `idCategoria` INT NULL,
  `idProveedor` INT NULL,
  `idProducto` INT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_HistorialModificacionPrecio` PRIMARY KEY (`idHistorialModificacionPrecio`)
) ENGINE=InnoDB;

ALTER TABLE `tblHistorialModificacionPrecio`
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_TipoCliente` FOREIGN KEY (`idTipoCliente`) REFERENCES `tblTipoCliente` (`idTipoCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Usuario` FOREIGN KEY (`idUsuarioRegistro`) REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Marca` FOREIGN KEY (`idMarca`) REFERENCES `tblMarca` (`idMarca`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Linea` FOREIGN KEY (`idLinea`) REFERENCES `tblLinea` (`idLinea`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Categoria` FOREIGN KEY (`idCategoria`) REFERENCES `tblCategoria` (`idCategoria`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Proveedor` FOREIGN KEY (`idProveedor`) REFERENCES `tblProveedor` (`idProveedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialModificacionPrecio_Producto` FOREIGN KEY (`idProducto`) REFERENCES `tblProducto` (`idProducto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblListaPrecioDetalle | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblListaPrecioDetalle` (
  `idListaPrecioDetalle` INT NOT NULL AUTO_INCREMENT,
  `idListaPrecioEncabezado` INT NOT NULL,
  `idProducto` INT NOT NULL,
  `precioUnitarioListaPrecioDetalle` DECIMAL(12,4) NOT NULL,
  `activoListaPrecioDetalle` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ListaPrecioDetalle` PRIMARY KEY (`idListaPrecioEncabezado`, `idListaPrecioDetalle`),
  CONSTRAINT `uqListaPrecioDetalleListaPrecioEncabezadoCodigoProductoCodigo` UNIQUE (`idListaPrecioEncabezado`, `idProducto`),
  KEY `idxListaPrecioDetalleProductoCodigo` (`idProducto`),
  CONSTRAINT `ckListaPrecioDetalleactivoListaPrecioDetalleBooleano` CHECK (activoListaPrecioDetalle IN (0,1)),
  CONSTRAINT `ckListaPrecioDetallePrecioNoNegativo` CHECK (precioUnitarioListaPrecioDetalle >= 0),
  CONSTRAINT `uqListaPrecioDetalleId` UNIQUE (`idListaPrecioDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblListaPrecioDetalle`
  ADD CONSTRAINT `Fk_ListaPrecioDetalle_ListaPrecioEncabezado` FOREIGN KEY (`idListaPrecioEncabezado`)
    REFERENCES `tblListaPrecioEncabezado` (`idListaPrecioEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ListaPrecioDetalle_Producto` FOREIGN KEY (`idProducto`)
    REFERENCES `tblProducto` (`idProducto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblClienteListaPrecio | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblClienteListaPrecio` (
  `idClienteListaPrecio` INT NOT NULL AUTO_INCREMENT,
  `idCliente` INT NOT NULL,
  `idListaPrecioEncabezado` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ClienteListaPrecio` PRIMARY KEY (`idClienteListaPrecio`),
  KEY `idxClienteListaPrecioListaPrecioEncabezadoCodigo` (`idListaPrecioEncabezado`),
  CONSTRAINT `uqClienteListaPrecioClaveNegocio` UNIQUE (`idCliente`)
) ENGINE=InnoDB;

ALTER TABLE `tblClienteListaPrecio`
  ADD CONSTRAINT `Fk_ClienteListaPrecio_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ClienteListaPrecio_ListaPrecioEncabezado` FOREIGN KEY (`idListaPrecioEncabezado`)
    REFERENCES `tblListaPrecioEncabezado` (`idListaPrecioEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblHistorialModificacionPrecioDetalle | Autor: Diego Alejandro Cheng Peña - 0901-22-8091 | Modulo: 7000 | Aplicacion: 7002
CREATE TABLE `tblHistorialModificacionPrecioDetalle` (
  `idHistorialModificacionPrecioDetalle` INT NOT NULL AUTO_INCREMENT,
  `idHistorialModificacionPrecio` INT NOT NULL,
  `idProducto` INT NOT NULL,
  `porcentajeDescuentoAnterior` DECIMAL(5,2) NOT NULL,
  `precioBaseAnterior` DECIMAL(12,4) NOT NULL,
  `precioFinalAnterior` DECIMAL(12,4) NOT NULL,
  `porcentajeDescuentoNuevo` DECIMAL(5,2) NOT NULL,
  `precioBaseNuevo` DECIMAL(12,4) NOT NULL,
  `precioFinalNuevo` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_HistorialModificacionPrecioDetalle` PRIMARY KEY (`idHistorialModificacionPrecio`, `idHistorialModificacionPrecioDetalle`),
  CONSTRAINT `uqHistorialModificacionPrecioDetalleId` UNIQUE (`idHistorialModificacionPrecioDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblHistorialModificacionPrecioDetalle`
  ADD CONSTRAINT `Fk_HistorialDetalle_HistorialEncabezado` FOREIGN KEY (`idHistorialModificacionPrecio`) REFERENCES `tblHistorialModificacionPrecio` (`idHistorialModificacionPrecio`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_HistorialDetalle_Producto` FOREIGN KEY (`idProducto`) REFERENCES `tblProducto` (`idProducto`) ON DELETE RESTRICT ON UPDATE RESTRICT;
-- tblClienteFacturaVersion | Autor: Matthew Andreé Juárez Xoy - 0901-23-4250 | Modulo: 7000 | Aplicacion: 7007
CREATE TABLE `tblClienteFacturaVersion` (
  `idClienteFacturaVersion` INT NOT NULL AUTO_INCREMENT,
  `idCliente` INT NOT NULL,
  `numeroVersionClienteFacturaVersion` INT NOT NULL,
  `tipoIdentificacionClienteFacturaVersion` ENUM('NIT','CUI','CF') NOT NULL,
  `nitClienteFacturaVersion` CHAR(10) NULL,
  `dpiClienteFacturaVersion` CHAR(13) NULL,
  `nombreClienteFacturaVersion` VARCHAR(100) NOT NULL,
  `direccionClienteFacturaVersion` VARCHAR(500) NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ClienteFacturaVersion` PRIMARY KEY (`idClienteFacturaVersion`),
  CONSTRAINT `uqClienteFacturaVersionClienteCodigonumeroVersion` UNIQUE (`idCliente`, `numeroVersionClienteFacturaVersion`),
  CONSTRAINT `ckClienteFacturaVersionIdentificacionCoherente` CHECK ((tipoIdentificacionClienteFacturaVersion = 'CF' AND nitClienteFacturaVersion IS NULL AND dpiClienteFacturaVersion IS NULL) OR (tipoIdentificacionClienteFacturaVersion = 'NIT' AND nitClienteFacturaVersion IS NOT NULL AND dpiClienteFacturaVersion IS NULL) OR (tipoIdentificacionClienteFacturaVersion = 'CUI' AND dpiClienteFacturaVersion IS NOT NULL AND nitClienteFacturaVersion IS NULL)),
  CONSTRAINT `ckClienteFacturaVersionVersionPositiva` CHECK (numeroVersionClienteFacturaVersion > 0)
) ENGINE=InnoDB;

ALTER TABLE `tblClienteFacturaVersion`
  ADD CONSTRAINT `Fk_ClienteFacturaVersion_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT;
    
-- tblCotizacionEncabezado | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7011
CREATE TABLE `tblCotizacionEncabezado` (
  `idCotizacionEncabezado` INT NOT NULL AUTO_INCREMENT,
  `numeroCotizacionEncabezado` VARCHAR(30) NOT NULL,
  `idCliente` INT NOT NULL,
  `idVendedor` INT NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `fechaEmisionCotizacionEncabezado` DATE NOT NULL,
  `vigenteHastaCotizacionEncabezado` DATE NOT NULL,
  `estadoCotizacionEncabezado` VARCHAR(25) NOT NULL,
  `observacionesCotizacionEncabezado` TEXT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `idListaPrecioEncabezado` INT NOT NULL,
  CONSTRAINT `Pk_CotizacionEncabezado` PRIMARY KEY (`idCotizacionEncabezado`),
  CONSTRAINT `uqCotizacionEncabezadonumero` UNIQUE (`numeroCotizacionEncabezado`),
  KEY `idxCotizacionEncabezadoClienteCodigo` (`idCliente`),
  KEY `idxCotizacionEncabezadoVendedorCodigo` (`idVendedor`),
  KEY `idxCotizacionEncabezadoMonedaCodigo` (`idMoneda`),
  KEY `idxCotizacionEncabezadoUsuarioRegistro` (`idUsuarioRegistro`),
  KEY `idxCotizacionEncabezadoListaPrecioEncabezadoCodigo` (`idListaPrecioEncabezado`),
  CONSTRAINT `ckCotizacionEncabezadoValoresEstadoCotizacion` CHECK (estadoCotizacionEncabezado IN ('BORRADOR','EMITIDA','ACEPTADA','RECHAZADA','VENCIDA','ANULADA')),
  CONSTRAINT `ckCotizacionEncabezadoFechasCoherentes` CHECK (vigenteHastaCotizacionEncabezado >= fechaEmisionCotizacionEncabezado)
) ENGINE=InnoDB;

ALTER TABLE `tblCotizacionEncabezado`
  ADD CONSTRAINT `Fk_CotizacionEncabezado_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CotizacionEncabezado_Vendedor` FOREIGN KEY (`idVendedor`)
    REFERENCES `tblVendedor` (`idVendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CotizacionEncabezado_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CotizacionEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`idMoneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CotizacionEncabezado_ListaPrecioEncabezado` FOREIGN KEY (`idListaPrecioEncabezado`)
    REFERENCES `tblListaPrecioEncabezado` (`idListaPrecioEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT;
    -- tblPedidoEncabezado | Autor: Natali Sofía Montenegro Portillo - 0901-23-10017 | Modulo: 7000 | Aplicacion: 7004
CREATE TABLE `tblPedidoEncabezado` (
  `idPedidoEncabezado` INT NOT NULL AUTO_INCREMENT,
  `referenciaPedidoEncabezado` VARCHAR(30) NOT NULL,
  `idCotizacionEncabezado` INT NULL,
  `idCliente` INT NOT NULL,
  `idVendedor` INT NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `fechaPedidoEncabezado` DATE NOT NULL,
  `fechaEntregaPedidoEncabezado` DATE NULL,
  `fechaVencimientoPedidoEncabezado` DATE NULL,
  `estadoPedidoEncabezado` VARCHAR(25) NOT NULL DEFAULT 'PENDIENTE',
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_PedidoEncabezado` PRIMARY KEY (`idPedidoEncabezado`),
  CONSTRAINT `uqPedidoEncabezadoReferencia` UNIQUE (`referenciaPedidoEncabezado`),
  CONSTRAINT `uqPedidoEncabezadoCotizacion` UNIQUE (`idCotizacionEncabezado`),
  KEY `idxPedidoEncabezadoCliente` (`idCliente`),
  KEY `idxPedidoEncabezadoVendedor` (`idVendedor`),
  KEY `idxPedidoEncabezadoMoneda` (`idMoneda`),
  CONSTRAINT `ckPedidoEncabezadoEstado` CHECK (`estadoPedidoEncabezado` IN ('PENDIENTE','CONFIRMADO','COMPLETADO','ANULADO')),
  CONSTRAINT `ckPedidoEncabezadoFechas` CHECK (
    (`fechaEntregaPedidoEncabezado` IS NULL OR `fechaEntregaPedidoEncabezado` >= `fechaPedidoEncabezado`)
    AND (`fechaVencimientoPedidoEncabezado` IS NULL OR `fechaVencimientoPedidoEncabezado` >= `fechaPedidoEncabezado`)
  )
) ENGINE=InnoDB;

ALTER TABLE `tblPedidoEncabezado`
  ADD CONSTRAINT `Fk_PedidoEncabezado_Cotizacion` FOREIGN KEY (`idCotizacionEncabezado`)
    REFERENCES `tblCotizacionEncabezado` (`idCotizacionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_PedidoEncabezado_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_PedidoEncabezado_Vendedor` FOREIGN KEY (`idVendedor`)
    REFERENCES `tblVendedor` (`idVendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_PedidoEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`idMoneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

    
-- tblFacturaEncabezado | Autor: Matthew Andreé Juárez Xoy - 0901-23-4250 | Modulo: 7000 | Aplicacion: 7007
CREATE TABLE `tblFacturaEncabezado` (
  `idFacturaEncabezado` INT NOT NULL AUTO_INCREMENT,
  `uuidDteFacturaEncabezado` CHAR(36) NOT NULL,
  `serieFacturaEncabezado` VARCHAR(30) NOT NULL,
  `numeroFacturaEncabezado` VARCHAR(30) NOT NULL,
  `idSucursal` INT NOT NULL,
  `idClienteFacturaVersion` INT NOT NULL,
  `idVendedor` INT NULL,
  `idMoneda` BIGINT NOT NULL,
  `idCotizacionEncabezado` INT NULL,
  `idPedidoEncabezado` INT NULL,
  `fechaEmisionFacturaEncabezado` DATETIME NOT NULL,
  `fechaVencimientoFacturaEncabezado` DATE NOT NULL,
  `tipoDteFacturaEncabezado` VARCHAR(30) NOT NULL DEFAULT 'Factura',
  `condicionPagoFacturaEncabezado` VARCHAR(15) NOT NULL,
  `estadoFacturaEncabezado` VARCHAR(25) NOT NULL DEFAULT 'EMITIDA',
  `fechaCertificacionFacturaEncabezado` DATETIME NULL,
  `subtotalFacturaEncabezado` DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
  `impuestoIvaFacturaEncabezado` DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
  `totalFacturaEncabezado` DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
  `observacionesFacturaEncabezado` TEXT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_FacturaEncabezado` PRIMARY KEY (`idFacturaEncabezado`),
  CONSTRAINT `uqFacturaEncabezadoSucursalCodigoserienumero` UNIQUE (`idSucursal`, `serieFacturaEncabezado`, `numeroFacturaEncabezado`),
  CONSTRAINT `uqFacturaEncabezadoCotizacionEncabezadoCodigo` UNIQUE (`idCotizacionEncabezado`),
  KEY `idxFacturaEncabezadoPedidoEncabezadoCodigo` (`idPedidoEncabezado`),
  KEY `idxFacturaEncabezadoVendedorCodigo` (`idVendedor`),
  KEY `idxFacturaEncabezadoMonedaCodigo` (`idMoneda`),
  KEY `idxFacturaEncabezadoUsuarioRegistro` (`idUsuarioRegistro`),
  KEY `idxFacturaEncabezadoClienteFacturaVersionCodigo` (`idClienteFacturaVersion`),
  CONSTRAINT `ckFacturaEncabezadoCondicionPagoValida` CHECK (condicionPagoFacturaEncabezado IN ('CONTADO','CREDITO')),
  CONSTRAINT `ckFacturaEncabezadoValoresEstadoFactura` CHECK (estadoFacturaEncabezado IN ('BORRADOR','EMITIDA','ANULADA')),
  CONSTRAINT `ckFacturaEncabezadoFechasCoherentes` CHECK (fechaVencimientoFacturaEncabezado >= DATE(fechaEmisionFacturaEncabezado)),
  CONSTRAINT `uqFacturaEncabezadoClaveNegocio` UNIQUE (`uuidDteFacturaEncabezado`)
) ENGINE=InnoDB;

ALTER TABLE `tblFacturaEncabezado`
  ADD CONSTRAINT `Fk_FacturaEncabezado_Sucursal` FOREIGN KEY (`idSucursal`)
REFERENCES `tblSucursal` (`idSucursal`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_ClienteFacturaVersion` FOREIGN KEY (`idClienteFacturaVersion`)
REFERENCES `tblClienteFacturaVersion` (`idClienteFacturaVersion`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_Vendedor` FOREIGN KEY (`idVendedor`)
REFERENCES `tblVendedor` (`idVendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
REFERENCES `tblMoneda` (`idMoneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_CotizacionEncabezado` FOREIGN KEY (`idCotizacionEncabezado`)
REFERENCES `tblCotizacionEncabezado` (`idCotizacionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_PedidoEncabezado` FOREIGN KEY (`idPedidoEncabezado`)
REFERENCES `tblPedidoEncabezado` (`idPedidoEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaEncabezado_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblNotaCxC | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7014
CREATE TABLE `tblNotaCxC` (
  `idNotaCxC` INT NOT NULL AUTO_INCREMENT,
  `numeroNotaCxC` VARCHAR(30) NOT NULL,
  `idFacturaEncabezado` INT NOT NULL,
  `tipoNotaCxC` VARCHAR(10) NOT NULL,
  `fechaNotaCxC` DATE NOT NULL,
  `montoNotaCxC` DECIMAL(12,4) NOT NULL,
  `motivoNotaCxC` TEXT NOT NULL,
  `estadoNotaCxC` VARCHAR(20) NOT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_NotaCxC` PRIMARY KEY (`idNotaCxC`),
  CONSTRAINT `uqNotaCxCnumero` UNIQUE (`numeroNotaCxC`),
  KEY `idxNotaCxCFacturaEncabezadoCodigo` (`idFacturaEncabezado`),
  KEY `idxNotaCxCUsuarioRegistro` (`idUsuarioRegistro`),
  CONSTRAINT `ckNotaCxCMontoPositivo` CHECK (montoNotaCxC > 0),
  CONSTRAINT `ckNotaCxCValoresTipoNota` CHECK (tipoNotaCxC IN ('CREDITO','DEBITO')),
  CONSTRAINT `ckNotaCxCValoresEstadoNota` CHECK (estadoNotaCxC IN ('BORRADOR','APLICADA','ANULADA'))
) ENGINE=InnoDB;

ALTER TABLE `tblNotaCxC`
  ADD CONSTRAINT `Fk_NotaCxC_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_NotaCxC_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT;


-- tblDepositoVentaEncabezado | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7015
CREATE TABLE `tblDepositoVentaEncabezado` (
  `idDepositoVentaEncabezado` INT NOT NULL AUTO_INCREMENT,
  `codigoDepositoVentaEncabezado` VARCHAR(30) NOT NULL,
  `fechaDepositoVentaEncabezado` DATE NOT NULL,
  `referenciaDepositoVentaEncabezado` VARCHAR(100) NULL,
  `idMoneda` BIGINT NOT NULL,
  `totalDepositoVentaEncabezado` DECIMAL(12,4) NOT NULL,
  `estadoDepositoVentaEncabezado` VARCHAR(25) NOT NULL DEFAULT 'PREPARADO',
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `idCuentaBancaria` BIGINT NOT NULL,
  `idTipoTransaccion` BIGINT NOT NULL,
  `idMovimientoBancario` BIGINT NULL,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_DepositoVentaEncabezado` PRIMARY KEY (`idDepositoVentaEncabezado`),
  CONSTRAINT `uqDepositoVentaEncabezadocodigo` UNIQUE (`codigoDepositoVentaEncabezado`),
  CONSTRAINT `uqDepositoVentaEncabezadoMovimientoBancarioCodigo` UNIQUE (`idMovimientoBancario`),
  KEY `idxDepositoVentaEncabezadoUsuarioRegistro` (`idUsuarioRegistro`),
  KEY `idxDepositoVentaEncabezadoMonedaCodigo` (`idMoneda`),
  KEY `idxDepositoVentaEncabezadoCuentaBancariaCodigo` (`idCuentaBancaria`),
  KEY `idxDepositoVentaEncabezadoTipoTransaccionCodigo` (`idTipoTransaccion`),
  CONSTRAINT `ckDepositoVentaEncabezadoTotalPositivo` CHECK (totalDepositoVentaEncabezado > 0),
  CONSTRAINT `ckDepositoVentaEncabezadoValoresEstadoDeposito` CHECK (estadoDepositoVentaEncabezado IN ('PREPARADO','ENVIADO','CONFIRMADO','ANULADO'))
) ENGINE=InnoDB;

ALTER TABLE `tblDepositoVentaEncabezado`
  ADD CONSTRAINT `Fk_DepositoVentaEncabezado_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_DepositoVentaEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`idMoneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_DepositoVentaEncabezado_CuentaBancaria` FOREIGN KEY (`idCuentaBancaria`)
    REFERENCES `tblCuentaBancaria` (`idCuentaBancaria`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_DepositoVentaEncabezado_TipoTransaccion` FOREIGN KEY (`idTipoTransaccion`)
    REFERENCES `tblTipoTransaccion` (`idTipoTransaccion`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_DepositoVentaEncabezado_MovimientoBancario` FOREIGN KEY (`idMovimientoBancario`)
    REFERENCES `tblMovimientoBancario` (`idMovimientoBancario`) ON DELETE RESTRICT ON UPDATE RESTRICT;
    
-- tblFormaPagoVenta | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7009
CREATE TABLE `tblFormaPagoVenta` (
  `idFormaPagoVenta` INT NOT NULL AUTO_INCREMENT,
  `codigoFormaPagoVenta` VARCHAR(30) NOT NULL,
  `nombreFormaPagoVenta` VARCHAR(100) NOT NULL,
  `requiereReferenciaFormaPagoVenta` BOOLEAN NOT NULL DEFAULT FALSE,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_FormaPagoVenta` PRIMARY KEY (`idFormaPagoVenta`),
  CONSTRAINT `uqFormaPagoVentacodigo` UNIQUE (`codigoFormaPagoVenta`),
  CONSTRAINT `ckFormaPagoVentarequiereReferenciaFormaPagoVentaBooleano` CHECK (requiereReferenciaFormaPagoVenta IN (0,1))
) ENGINE=InnoDB;
-- tblReciboPago | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7005
CREATE TABLE `tblReciboPago` (
  `idReciboPago` INT NOT NULL AUTO_INCREMENT,
  `numeroReciboPago` VARCHAR(30) NOT NULL,
  `idCliente` INT NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `fechaReciboPago` DATE NOT NULL,
  `montoReciboPago` DECIMAL(12,4) NOT NULL,
  `referenciaReciboPago` VARCHAR(255) NULL,
  `estadoReciboPago` VARCHAR(20) NOT NULL,
  `observacionesReciboPago` TEXT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `idFormaPagoVenta` INT NOT NULL,
  CONSTRAINT `Pk_ReciboPago` PRIMARY KEY (`idReciboPago`),
  CONSTRAINT `uqReciboPagonumero` UNIQUE (`numeroReciboPago`),
  KEY `idxReciboPagoClienteCodigofecha` (`idCliente`, `fechaReciboPago`),
  KEY `idxReciboPagoMonedaCodigo` (`idMoneda`),
  KEY `idxReciboPagoUsuarioRegistro` (`idUsuarioRegistro`),
  KEY `idxReciboPagoFormaPagoVentaCodigo` (`idFormaPagoVenta`),
  CONSTRAINT `ckReciboPagoMontoPositivo` CHECK (montoReciboPago > 0),
  CONSTRAINT `ckReciboPagoValoresEstadoPago` CHECK (estadoReciboPago IN ('BORRADOR','CONFIRMADO','ANULADO'))
) ENGINE=InnoDB;

ALTER TABLE `tblReciboPago`
  ADD CONSTRAINT `Fk_ReciboPago_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ReciboPago_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ReciboPago_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`idMoneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ReciboPago_FormaPagoVenta` FOREIGN KEY (`idFormaPagoVenta`)
    REFERENCES `tblFormaPagoVenta` (`idFormaPagoVenta`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblDepositoVentaDetalle | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7015
CREATE TABLE `tblDepositoVentaDetalle` (
  `idDepositoVentaDetalle` INT NOT NULL AUTO_INCREMENT,
  `idDepositoVentaEncabezado` INT NOT NULL,
  `idReciboPago` INT NOT NULL,
  `importeDepositoVentaDetalle` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_DepositoVentaDetalle` PRIMARY KEY (`idDepositoVentaEncabezado`, `idDepositoVentaDetalle`),
  KEY `idxDepositoVentaDetalleReciboPagoCodigo` (`idReciboPago`),
  CONSTRAINT `ckDepositoVentaDetalleImportePositivo` CHECK (importeDepositoVentaDetalle > 0),
  CONSTRAINT `uqDepositoVentaDetalleClaveNegocio` UNIQUE (`idDepositoVentaEncabezado`, `idReciboPago`),
  CONSTRAINT `uqDepositoVentaDetalleId` UNIQUE (`idDepositoVentaDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblDepositoVentaDetalle`
  ADD CONSTRAINT `Fk_DepositoVentaDetalle_DepositoVentaEncabezado` FOREIGN KEY (`idDepositoVentaEncabezado`)
    REFERENCES `tblDepositoVentaEncabezado` (`idDepositoVentaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_DepositoVentaDetalle_ReciboPago` FOREIGN KEY (`idReciboPago`)
    REFERENCES `tblReciboPago` (`idReciboPago`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblAplicacionPago | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7005
CREATE TABLE `tblAplicacionPago` (
  `idAplicacionPago` INT NOT NULL AUTO_INCREMENT,
  `idReciboPago` INT NOT NULL,
  `idFacturaEncabezado` INT NOT NULL,
  `fechaAplicacionPago` DATE NOT NULL,
  `montoAplicadoAplicacionPago` DECIMAL(12,4) NOT NULL,
  `estadoAplicacionPago` VARCHAR(20) NOT NULL,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_AplicacionPago` PRIMARY KEY (`idAplicacionPago`),
  KEY `idxAplicacionPagoFacturaEncabezadoCodigofecha` (`idFacturaEncabezado`, `fechaAplicacionPago`),
  KEY `idxAplicacionPagoReciboPagoCodigo` (`idReciboPago`),
  KEY `idxAplicacionPagoUsuarioRegistro` (`idUsuarioRegistro`),
  CONSTRAINT `ckAplicacionPagoMontoPositivo` CHECK (montoAplicadoAplicacionPago > 0),
  CONSTRAINT `ckAplicacionPagoValoresEstadoAplicacion` CHECK (estadoAplicacionPago IN ('APLICADA','REVERTIDA'))
) ENGINE=InnoDB;

ALTER TABLE `tblAplicacionPago`
  ADD CONSTRAINT `Fk_AplicacionPago_ReciboPago` FOREIGN KEY (`idReciboPago`)
    REFERENCES `tblReciboPago` (`idReciboPago`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_AplicacionPago_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_AplicacionPago_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT;
-- tblMovimientoCxC | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7003
CREATE TABLE `tblMovimientoCxC` (
  `idMovimientoCxC` INT NOT NULL AUTO_INCREMENT,
  `idFacturaEncabezado` INT NOT NULL,
  `idAplicacionPago` INT NULL,
  `idNotaCxC` INT NULL,
  `idMovimientoCxCRevertido` INT NULL,
  `fechaMovimientoCxC` DATE NOT NULL,
  `tipoMovimientoCxC` VARCHAR(30) NOT NULL,
  `descripcionMovimientoCxC` VARCHAR(255) NOT NULL,
  `cargoMovimientoCxC` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `abonoMovimientoCxC` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `idUsuarioRegistro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_MovimientoCxC` PRIMARY KEY (`idMovimientoCxC`),
  CONSTRAINT `uqMovimientoCxCAplicacionPagoCodigo` UNIQUE (`idAplicacionPago`),
  CONSTRAINT `uqMovimientoCxCNotaCxCCodigo` UNIQUE (`idNotaCxC`),
  CONSTRAINT `uqMovimientoCxCRevertido` UNIQUE (`idMovimientoCxCRevertido`),
  KEY `idxMovimientoCxCFacturaEncabezadoCodigofecha` (`idFacturaEncabezado`, `fechaMovimientoCxC`),
  KEY `idxMovimientoCxCfecha` (`fechaMovimientoCxC`),
  KEY `idxMovimientoCxCUsuarioRegistro` (`idUsuarioRegistro`),
  CONSTRAINT `ckMovimientoCxCCargoAbonoExcluyentes` CHECK (cargoMovimientoCxC >= 0 AND abonoMovimientoCxC >= 0 AND ((cargoMovimientoCxC > 0 AND abonoMovimientoCxC = 0) OR (abonoMovimientoCxC > 0 AND cargoMovimientoCxC = 0))),
  CONSTRAINT `ckMovimientoCxCReversionIdentificada` CHECK ((tipoMovimientoCxC = 'REVERSO' AND idMovimientoCxCRevertido IS NOT NULL) OR (tipoMovimientoCxC <> 'REVERSO' AND idMovimientoCxCRevertido IS NULL)),
  CONSTRAINT `ckMovimientoCxCValoresTipoMovimiento` CHECK (tipoMovimientoCxC IN ('FACTURA','PAGO','NOTA_CREDITO','NOTA_DEBITO','AJUSTE_POSITIVO','AJUSTE_NEGATIVO','REVERSO'))
) ENGINE=InnoDB;

ALTER TABLE `tblMovimientoCxC`
  ADD CONSTRAINT `Fk_MovimientoCxC_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_MovimientoCxC_AplicacionPago` FOREIGN KEY (`idAplicacionPago`)
    REFERENCES `tblAplicacionPago` (`idAplicacionPago`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_MovimientoCxC_NotaCxC` FOREIGN KEY (`idNotaCxC`)
    REFERENCES `tblNotaCxC` (`idNotaCxC`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_MovimientoCxC_MovimientoCxC_Revertido` FOREIGN KEY (`idMovimientoCxCRevertido`)
    REFERENCES `tblMovimientoCxC` (`idMovimientoCxC`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_MovimientoCxC_Usuario_Registro` FOREIGN KEY (`idUsuarioRegistro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT;


-- tblCotizacionDetalle | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7011
CREATE TABLE `tblCotizacionDetalle` (
  `idCotizacionDetalle` INT NOT NULL AUTO_INCREMENT,
  `idCotizacionEncabezado` INT NOT NULL,
  `numeroLineaCotizacionDetalle` INT NOT NULL,
  `idProducto` INT NOT NULL,
  `descripcionItemCotizacionDetalle` VARCHAR(255) NOT NULL,
  `cantidadCotizacionDetalle` DECIMAL(12,4) NOT NULL,
  `precioUnitarioCotizacionDetalle` DECIMAL(12,4) NOT NULL,
  `descuentoMontoCotizacionDetalle` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `tasaImpuestoCotizacionDetalle` DECIMAL(3,2) NOT NULL DEFAULT 0,
  `impuestoMontoCotizacionDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND((cantidadCotizacionDetalle * precioUnitarioCotizacionDetalle - descuentoMontoCotizacionDetalle) * tasaImpuestoCotizacionDetalle, 4)) STORED,
  `totalLineaCotizacionDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(cantidadCotizacionDetalle * precioUnitarioCotizacionDetalle - descuentoMontoCotizacionDetalle + impuestoMontoCotizacionDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CotizacionDetalle` PRIMARY KEY (`idCotizacionEncabezado`, `idCotizacionDetalle`),
  CONSTRAINT `uqCotizacionDetalleCotizacionEncabezadoCodigonumeroLinea` UNIQUE (`idCotizacionEncabezado`, `numeroLineaCotizacionDetalle`),
  KEY `idxCotizacionDetalleProductoCodigo` (`idProducto`),
  CONSTRAINT `ckCotizacionDetalletasaImpuestoCotizacionDetalleRango` CHECK (tasaImpuestoCotizacionDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckCotizacionDetalleCantidadPrecioDescuentoValidos` CHECK (cantidadCotizacionDetalle > 0 AND precioUnitarioCotizacionDetalle >= 0 AND descuentoMontoCotizacionDetalle >= 0 AND descuentoMontoCotizacionDetalle <= cantidadCotizacionDetalle * precioUnitarioCotizacionDetalle),
  CONSTRAINT `ckCotizacionDetalleNumeroLineaPositivo` CHECK (numeroLineaCotizacionDetalle > 0),
  CONSTRAINT `uqCotizacionDetalleId` UNIQUE (`idCotizacionDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblCotizacionDetalle`
  ADD CONSTRAINT `Fk_CotizacionDetalle_CotizacionEncabezado` FOREIGN KEY (`idCotizacionEncabezado`)
    REFERENCES `tblCotizacionEncabezado` (`idCotizacionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CotizacionDetalle_Producto` FOREIGN KEY (`idProducto`)
    REFERENCES `tblProducto` (`idProducto`) ON DELETE RESTRICT ON UPDATE RESTRICT;


-- tblPedidoDetalle | Autor: Natali Sofía Montenegro Portillo - 0901-23-10017 | Modulo: 7000 | Aplicacion: 7004
CREATE TABLE `tblPedidoDetalle` (
  `idPedidoDetalle` INT NOT NULL AUTO_INCREMENT,
  `idPedidoEncabezado` INT NOT NULL,
  `OrdenPedido` INT NOT NULL,
  `idProducto` INT NOT NULL,
  `cantidadPedidoDetalle` DECIMAL(12,4) NOT NULL,
  `precioUnitarioPedidoDetalle` DECIMAL(12,4) NOT NULL,
  `descuentoMontoPedidoDetalle` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_PedidoDetalle` PRIMARY KEY (`idPedidoEncabezado`, `idPedidoDetalle`),
  KEY `idxPedidoDetalleProducto` (`idProducto`),
  CONSTRAINT `ckPedidoDetalleOrdenPositivo` CHECK (`OrdenPedido` > 0),
  CONSTRAINT `ckPedidoDetalleCantidadPrecioDescuento` CHECK (
    `cantidadPedidoDetalle` > 0
    AND `precioUnitarioPedidoDetalle` >= 0
    AND `descuentoMontoPedidoDetalle` >= 0
    AND `descuentoMontoPedidoDetalle` <= `cantidadPedidoDetalle` * `precioUnitarioPedidoDetalle`
  ),
  CONSTRAINT `uqPedidoDetalleClaveNegocio` UNIQUE (`idPedidoEncabezado`, `OrdenPedido`, `idProducto`),
  CONSTRAINT `uqPedidoDetalleId` UNIQUE (`idPedidoDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblPedidoDetalle`
  ADD CONSTRAINT `Fk_PedidoDetalle_PedidoEncabezado` FOREIGN KEY (`idPedidoEncabezado`)
    REFERENCES `tblPedidoEncabezado` (`idPedidoEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_PedidoDetalle_Producto` FOREIGN KEY (`idProducto`)
    REFERENCES `tblProducto` (`idProducto`) ON DELETE RESTRICT ON UPDATE RESTRICT;




-- tblFacturaDetalle | Autor: Matthew Andreé Juárez Xoy - 0901-23-4250 | Modulo: 7000 | Aplicacion: 7007
CREATE TABLE `tblFacturaDetalle` (
  `idFacturaEncabezadoUuidDte` CHAR(36) NOT NULL,
  `numeroLineaFacturaDetalle` INT NOT NULL,
  `idFacturaDetalle` INT NOT NULL AUTO_INCREMENT,
  `idProducto` INT NOT NULL,
  `cantidadFacturaDetalle` DECIMAL(12,4) NOT NULL,
  `precioUnitarioFacturaDetalle` DECIMAL(12,4) NOT NULL,
  `descuentoMontoFacturaDetalle` DECIMAL(12,4) NOT NULL DEFAULT 0.0000,
  `tasaImpuestoFacturaDetalle` DECIMAL(3,2) NOT NULL DEFAULT 0.12,
  `impuestoMontoFacturaDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND((cantidadFacturaDetalle * precioUnitarioFacturaDetalle - descuentoMontoFacturaDetalle) * tasaImpuestoFacturaDetalle, 4)) STORED,
  `subtotalLineaFacturaDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(cantidadFacturaDetalle * precioUnitarioFacturaDetalle - descuentoMontoFacturaDetalle, 4)) STORED,
  `totalLineaFacturaDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(cantidadFacturaDetalle * precioUnitarioFacturaDetalle - descuentoMontoFacturaDetalle + impuestoMontoFacturaDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_FacturaDetalle` PRIMARY KEY (`idFacturaEncabezadoUuidDte`, `idFacturaDetalle`),
  KEY `idxFacturaDetalleProductoCodigo` (`idProducto`),
  CONSTRAINT `ckFacturaDetalletasaImpuestoFacturaDetalleRango` CHECK (tasaImpuestoFacturaDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckFacturaDetalleCantidadPrecioDescuentoValidos` CHECK (cantidadFacturaDetalle > 0 AND precioUnitarioFacturaDetalle >= 0 AND descuentoMontoFacturaDetalle >= 0 AND descuentoMontoFacturaDetalle <= cantidadFacturaDetalle * precioUnitarioFacturaDetalle),
  CONSTRAINT `ckFacturaDetalleNumeroLineaPositivo` CHECK (numeroLineaFacturaDetalle > 0),
  CONSTRAINT `uqFacturaDetalleClaveNegocio` UNIQUE (`idFacturaEncabezadoUuidDte`, `numeroLineaFacturaDetalle`),
  CONSTRAINT `uqFacturaDetalleId` UNIQUE (`idFacturaDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblFacturaDetalle`
  ADD CONSTRAINT `Fk_FacturaDetalle_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezadoUuidDte`)
REFERENCES `tblFacturaEncabezado` (`uuidDteFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_FacturaDetalle_Producto` FOREIGN KEY (`idProducto`)
REFERENCES `tblProducto` (`idProducto`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblReglaComision | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblReglaComision` (
  `idReglaComision` INT NOT NULL AUTO_INCREMENT,
  `codigoReglaComision` VARCHAR(30) NOT NULL,
  `nombreReglaComision` VARCHAR(100) NOT NULL,
  `baseCalculoReglaComision` VARCHAR(30) NOT NULL,
  `tasaComisionReglaComision` DECIMAL(3,2) NOT NULL,
  `vigenteDesdeReglaComision` DATE NOT NULL,
  `vigenteHastaReglaComision` DATE NULL,
  `requiereAutorizacionReglaComision` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ReglaComision` PRIMARY KEY (`idReglaComision`),
  CONSTRAINT `uqReglaComisioncodigo` UNIQUE (`codigoReglaComision`),
  CONSTRAINT `ckReglaComisiontasaComisionReglaComisionRango` CHECK (tasaComisionReglaComision BETWEEN 0 AND 0.99),
  CONSTRAINT `ckReglaComisionrequiereAutorizacionReglaComisionBooleano` CHECK (requiereAutorizacionReglaComision IN (0,1)),
  CONSTRAINT `ckReglaComisionValoresBaseCalculo` CHECK (baseCalculoReglaComision IN ('VENTA_NETA','COBRO_NETO')),
  CONSTRAINT `ckReglaComisionFechasCoherentes` CHECK (vigenteHastaReglaComision >= vigenteDesdeReglaComision)
) ENGINE=InnoDB;

-- tblComisionEncabezado | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblComisionEncabezado` (
  `idComisionEncabezado` INT NOT NULL AUTO_INCREMENT,
  `numeroComisionEncabezado` VARCHAR(30) NOT NULL,
  `idVendedor` INT NOT NULL,
  `idReglaComision` INT NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `fechaDesdeComisionEncabezado` DATE NOT NULL,
  `fechaHastaComisionEncabezado` DATE NOT NULL,
  `estadoComisionEncabezado` VARCHAR(25) NOT NULL,
  `idUsuarioSolicita` INT NULL,
  `fechaSolicitudComisionEncabezado` DATETIME NULL,
  `idUsuarioAutoriza` INT NULL,
  `fechaAutorizacionComisionEncabezado` DATETIME NULL,
  `comentarioAutorizacionComisionEncabezado` TEXT NULL,
  `referenciaContableComisionEncabezado` VARCHAR(100) NULL,
  `fechaEnvioComisionEncabezado` DATETIME NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionEncabezado` PRIMARY KEY (`idComisionEncabezado`),
  CONSTRAINT `uqComisionEncabezadonumero` UNIQUE (`numeroComisionEncabezado`),
  KEY `idxComisionEncabezadoVendedorCodigofechaDesdefechaHasta` (`idVendedor`, `fechaDesdeComisionEncabezado`, `fechaHastaComisionEncabezado`),
  KEY `idxComisionEncabezadoReglaComisionCodigo` (`idReglaComision`),
  KEY `idxComisionEncabezadoMonedaCodigo` (`idMoneda`),
  KEY `idxComisionEncabezadoUsuarioSolicita` (`idUsuarioSolicita`),
  KEY `idxComisionEncabezadoUsuarioAutoriza` (`idUsuarioAutoriza`),
  CONSTRAINT `ckComisionEncabezadoValoresEstadoComision` CHECK (estadoComisionEncabezado IN ('BORRADOR','SOLICITADA','APROBADA','RECHAZADA','ENVIADA')),
  CONSTRAINT `ckComisionEncabezadoFechasCoherentes` CHECK (fechaHastaComisionEncabezado >= fechaDesdeComisionEncabezado)
) ENGINE=InnoDB;

ALTER TABLE `tblComisionEncabezado`
  ADD CONSTRAINT `Fk_ComisionEncabezado_Vendedor` FOREIGN KEY (`idVendedor`)
    REFERENCES `tblVendedor` (`idVendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionEncabezado_ReglaComision` FOREIGN KEY (`idReglaComision`)
    REFERENCES `tblReglaComision` (`idReglaComision`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionEncabezado_Usuario_Solicita` FOREIGN KEY (`idUsuarioSolicita`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionEncabezado_Usuario_Autoriza` FOREIGN KEY (`idUsuarioAutoriza`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`idMoneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblComisionFacturaEncabezado | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblComisionFacturaEncabezado` (
  `idComisionFacturaEncabezado` INT NOT NULL AUTO_INCREMENT,
  `idFacturaEncabezado` INT NOT NULL,
  `metodoComisionFacturaEncabezado` ENUM('LINEA','PRODUCTO','CATEGORIA','MARCA','MIXTA') NOT NULL,
  `estadoComisionFacturaEncabezado` ENUM('CALCULADA','ANULADA') NOT NULL DEFAULT 'CALCULADA',
  `observacionesComisionFacturaEncabezado` TEXT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionFacturaEncabezado` PRIMARY KEY (`idComisionFacturaEncabezado`),
  CONSTRAINT `uqComisionFacturaEncabezadoFacturaEncabezadoCodigo` UNIQUE (`idFacturaEncabezado`)
) ENGINE=InnoDB;

ALTER TABLE `tblComisionFacturaEncabezado`
  ADD CONSTRAINT `Fk_ComisionFacturaEncabezado_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblComisionDetalle | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblComisionDetalle` (
  `idComisionDetalle` INT NOT NULL AUTO_INCREMENT,
  `idComisionEncabezado` INT NOT NULL,
  `idFacturaEncabezado` INT NOT NULL,
  `idAplicacionPago` INT NULL,
  `baseComisionDetalle` DECIMAL(12,4) NOT NULL,
  `tasaAplicadaComisionDetalle` DECIMAL(3,2) NOT NULL,
  `montoComisionDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(baseComisionDetalle * tasaAplicadaComisionDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionDetalle` PRIMARY KEY (`idComisionEncabezado`, `idComisionDetalle`),
  KEY `idxComisionDetalleComisionEncabezadoCodigo` (`idComisionEncabezado`),
  KEY `idxComisionDetalleFacturaEncabezadoCodigo` (`idFacturaEncabezado`),
  KEY `idxComisionDetalleAplicacionPagoCodigo` (`idAplicacionPago`),
  CONSTRAINT `ckComisionDetalletasaAplicadaComisionDetalleRango` CHECK (tasaAplicadaComisionDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckComisionDetalleBaseNoNegativa` CHECK (baseComisionDetalle >= 0),
  CONSTRAINT `uqComisionDetalleId` UNIQUE (`idComisionDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblComisionDetalle`
  ADD CONSTRAINT `Fk_ComisionDetalle_ComisionEncabezado` FOREIGN KEY (`idComisionEncabezado`)
    REFERENCES `tblComisionEncabezado` (`idComisionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionDetalle_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionDetalle_AplicacionPago` FOREIGN KEY (`idAplicacionPago`)
    REFERENCES `tblAplicacionPago` (`idAplicacionPago`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblComisionAbono | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblComisionAbono` (
  `idComisionAbono` INT NOT NULL AUTO_INCREMENT,
  `idComisionFacturaEncabezado` INT NOT NULL,
  `idAplicacionPago` INT NOT NULL,
  `tasaFacturaPagadaAcumuladaComisionAbono` DECIMAL(5,4) NOT NULL,
  `montoComisionDevengadoComisionAbono` DECIMAL(12,4) NOT NULL,
  `estadoPagoVendedorComisionAbono` ENUM('PENDIENTE','PAGADO','REVERSADO') NOT NULL DEFAULT 'PENDIENTE',
  `fechaPagoVendedorComisionAbono` DATE NULL,
  `referenciaNominaComisionAbono` VARCHAR(100) NULL,
  `observacionesComisionAbono` TEXT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionAbono` PRIMARY KEY (`idComisionAbono`),
  CONSTRAINT `uqComisionAbonoAplicacionPagoCodigo` UNIQUE (`idAplicacionPago`),
  KEY `idxComisionAbonoComisionFacturaEncabezadoCodigoestadoPa19c1f12f` (`idComisionFacturaEncabezado`, `estadoPagoVendedorComisionAbono`),
  CONSTRAINT `ckComisionAbonotasaFacturaPagadaAcumuladaComisionAbonoRango` CHECK (tasaFacturaPagadaAcumuladaComisionAbono BETWEEN 0 AND 1),
  CONSTRAINT `ckComisionAbonoDevengoNoNegativo` CHECK (montoComisionDevengadoComisionAbono >= 0),
  CONSTRAINT `ckComisionAbonoFechaPagoVendedor` CHECK (estadoPagoVendedorComisionAbono <> 'PAGADO' OR fechaPagoVendedorComisionAbono IS NOT NULL)
) ENGINE=InnoDB;

ALTER TABLE `tblComisionAbono`
  ADD CONSTRAINT `Fk_ComisionAbono_ComisionFacturaEncabezado` FOREIGN KEY (`idComisionFacturaEncabezado`)
    REFERENCES `tblComisionFacturaEncabezado` (`idComisionFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionAbono_AplicacionPago` FOREIGN KEY (`idAplicacionPago`)
    REFERENCES `tblAplicacionPago` (`idAplicacionPago`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblComisionFacturaDetalle | Autor: Roger Yankhel de Jesús Herrera Alcántara - 0901-23-2429 | Modulo: 7000 | Aplicacion: 7013
CREATE TABLE `tblComisionFacturaDetalle` (
  `idComisionFacturaDetalle` INT NOT NULL AUTO_INCREMENT,
  `idComisionFacturaEncabezado` INT NOT NULL,
  `idFacturaDetalle` INT NOT NULL,
  `metodoComisionFacturaDetalle` ENUM('LINEA','PRODUCTO','CATEGORIA','MARCA') NOT NULL,
  `nombreClasificacionHistoricaComisionFacturaDetalle` VARCHAR(100) NOT NULL,
  `tasaAplicadaComisionFacturaDetalle` DECIMAL(3,2) NOT NULL,
  `baseComisionFacturaDetalle` DECIMAL(12,4) NOT NULL,
  `montoComisionLineaComisionFacturaDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(baseComisionFacturaDetalle * tasaAplicadaComisionFacturaDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `idLinea` INT NULL,
  `idCategoria` INT NULL,
  `idMarca` INT NULL,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionFacturaDetalle` PRIMARY KEY (`idComisionFacturaEncabezado`, `idComisionFacturaDetalle`),
  CONSTRAINT `uqComisionFacturaDetalleFacturaDetalleCodigo` UNIQUE (`idFacturaDetalle`),
  KEY `idxComisionFacturaDetalleComisionFacturaEncabezadoCodigo` (`idComisionFacturaEncabezado`),
  KEY `idxComisionFacturaDetalleLineaCodigo` (`idLinea`),
  KEY `idxComisionFacturaDetalleCategoriaCodigo` (`idCategoria`),
  KEY `idxComisionFacturaDetalleMarcaCodigo` (`idMarca`),
  CONSTRAINT `ckComisionFacturaDetalletasaAplicadaComisionFacturaDetalleRango` CHECK (tasaAplicadaComisionFacturaDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckComisionFacturaDetalleBaseNoNegativa` CHECK (baseComisionFacturaDetalle >= 0),
  CONSTRAINT `ckComisionFacturaDetalleClasificacionTipada` CHECK ((metodoComisionFacturaDetalle = 'PRODUCTO' AND idLinea IS NULL AND idCategoria IS NULL AND idMarca IS NULL) OR (metodoComisionFacturaDetalle = 'LINEA' AND idLinea IS NOT NULL AND idCategoria IS NULL AND idMarca IS NULL) OR (metodoComisionFacturaDetalle = 'CATEGORIA' AND idCategoria IS NOT NULL AND idLinea IS NULL AND idMarca IS NULL) OR (metodoComisionFacturaDetalle = 'MARCA' AND idMarca IS NOT NULL AND idLinea IS NULL AND idCategoria IS NULL)),
  CONSTRAINT `uqComisionFacturaDetalleId` UNIQUE (`idComisionFacturaDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblComisionFacturaDetalle`
  ADD CONSTRAINT `Fk_ComisionFacturaDetalle_ComisionFacturaEncabezado` FOREIGN KEY (`idComisionFacturaEncabezado`)
    REFERENCES `tblComisionFacturaEncabezado` (`idComisionFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionFacturaDetalle_FacturaDetalle` FOREIGN KEY (`idFacturaDetalle`)
    REFERENCES `tblFacturaDetalle` (`idFacturaDetalle`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionFacturaDetalle_Linea` FOREIGN KEY (`idLinea`)
    REFERENCES `tblLinea` (`idLinea`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionFacturaDetalle_Categoria` FOREIGN KEY (`idCategoria`)
    REFERENCES `tblCategoria` (`idCategoria`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_ComisionFacturaDetalle_Marca` FOREIGN KEY (`idMarca`)
    REFERENCES `tblMarca` (`idMarca`) ON DELETE RESTRICT ON UPDATE RESTRICT;


-- tblProductoVenta | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7009
CREATE TABLE `tblProductoVenta` (
  `idProductoVenta` INT NOT NULL,
  `tipoItemProductoVenta` CHAR(1) NOT NULL,
  `unidadMedidaProductoVenta` VARCHAR(30) NOT NULL,
  `tasaImpuestoProductoVenta` DECIMAL(3,2) NOT NULL DEFAULT 0,
  `activoProductoVenta` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ProductoVenta` PRIMARY KEY (`idProductoVenta`),
  CONSTRAINT `ckProductoVentatasaImpuestoProductoVentaRango` CHECK (tasaImpuestoProductoVenta BETWEEN 0 AND 0.99),
  CONSTRAINT `ckProductoVentaactivoProductoVentaBooleano` CHECK (activoProductoVenta IN (0,1)),
  CONSTRAINT `ckProductoVentaTipoItemValido` CHECK (tipoItemProductoVenta IN ('B','S'))
) ENGINE=InnoDB;

ALTER TABLE `tblProductoVenta`
  ADD CONSTRAINT `Fk_ProductoVenta_Producto` FOREIGN KEY (`idProductoVenta`)
    REFERENCES `tblProducto` (`idProducto`) ON DELETE RESTRICT ON UPDATE RESTRICT;



-- tblCierreMovimientoCxCEncabezado | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblCierreMovimientoCxCEncabezado` (
  `idCierreMovimientoCxCEncabezado` INT NOT NULL AUTO_INCREMENT,
  `numeroCierreMovimientoCxCEncabezado` VARCHAR(30) NOT NULL,
  `fechaDesdeCierreMovimientoCxCEncabezado` DATE NOT NULL,
  `fechaHastaCierreMovimientoCxCEncabezado` DATE NOT NULL,
  `fechaCorteCierreMovimientoCxCEncabezado` DATE NOT NULL,
  `idMoneda` BIGINT NOT NULL,
  `tipoCierreMovimientoCxCEncabezado` VARCHAR(20) NOT NULL,
  `estadoCierreMovimientoCxCEncabezado` VARCHAR(20) NOT NULL,
  `documentosRevisadosCierreMovimientoCxCEncabezado` BOOLEAN NOT NULL DEFAULT 0,
  `pagosConciliadosCierreMovimientoCxCEncabezado` BOOLEAN NOT NULL DEFAULT 0,
  `observacionesCierreMovimientoCxCEncabezado` TEXT NULL,
  `idUsuarioResponsable` INT NOT NULL,
  `fechaConfirmacionCierreMovimientoCxCEncabezado` DATETIME NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CierreMovimientoCxCEncabezado` PRIMARY KEY (`idCierreMovimientoCxCEncabezado`),
  CONSTRAINT `uqCierreMovimientoCxCEncabezadonumero` UNIQUE (`numeroCierreMovimientoCxCEncabezado`),
  KEY `idxCierreMovimientoCxCEncabezadoMonedaCodigofechaDesdefechaHasta` (`idMoneda`, `fechaDesdeCierreMovimientoCxCEncabezado`, `fechaHastaCierreMovimientoCxCEncabezado`),
  KEY `idxCierreMovimientoCxCEncabezadoUsuarioResponsable` (`idUsuarioResponsable`),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadodocumentosRevisadosCierra3aaae2a` CHECK (documentosRevisadosCierreMovimientoCxCEncabezado IN (0,1)),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadopagosConciliadosCierreMob8223bf3` CHECK (pagosConciliadosCierreMovimientoCxCEncabezado IN (0,1)),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadoValoresTipoCierre` CHECK (tipoCierreMovimientoCxCEncabezado IN ('MENSUAL','ANUAL')),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadoValoresEstadoCierre` CHECK (estadoCierreMovimientoCxCEncabezado IN ('BORRADOR','CONFIRMADO','ANULADO')),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadoFechasCoherentes` CHECK (fechaHastaCierreMovimientoCxCEncabezado >= fechaDesdeCierreMovimientoCxCEncabezado)
) ENGINE=InnoDB;

ALTER TABLE `tblCierreMovimientoCxCEncabezado`
  ADD CONSTRAINT `Fk_CierreMovimientoCxCEncabezado_Usuario_Responsable` FOREIGN KEY (`idUsuarioResponsable`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CierreMovimientoCxCEncabezado_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`idMoneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblControlPeriodoCxC | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblControlPeriodoCxC` (
  `idControlPeriodoCxC` INT NOT NULL AUTO_INCREMENT,
  `idMoneda` BIGINT NOT NULL,
  `fechaUltimoCierreControlPeriodoCxC` DATE NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ControlPeriodoCxC` PRIMARY KEY (`idControlPeriodoCxC`),
  CONSTRAINT `uqControlPeriodoCxCClaveNegocio` UNIQUE (`idMoneda`)
) ENGINE=InnoDB;

ALTER TABLE `tblControlPeriodoCxC`
  ADD CONSTRAINT `Fk_ControlPeriodoCxC_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`idMoneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblCuentaContableCxC | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7000
CREATE TABLE `tblCuentaContableCxC` (
  `idCuentaContableCxC` INT NOT NULL AUTO_INCREMENT,
  `idMoneda` BIGINT NOT NULL,
  `tipoCuentaContableCxC` VARCHAR(30) NOT NULL,
  `cuentaClientesCuentaContableCxC` VARCHAR(30) NOT NULL,
  `cuentaContrapartidaCuentaContableCxC` VARCHAR(30) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CuentaContableCxC` PRIMARY KEY (`idCuentaContableCxC`),
  CONSTRAINT `uqCuentaContableCxCClaveNegocio` UNIQUE (`idMoneda`, `tipoCuentaContableCxC`)
) ENGINE=InnoDB;

ALTER TABLE `tblCuentaContableCxC`
  ADD CONSTRAINT `Fk_CuentaContableCxC_Moneda` FOREIGN KEY (`idMoneda`)
    REFERENCES `tblMoneda` (`idMoneda`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblCierreSaldoCliente | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblCierreSaldoCliente` (
  `idCierreSaldoCliente` INT NOT NULL AUTO_INCREMENT,
  `idCierreMovimientoCxCEncabezado` INT NOT NULL,
  `idCliente` INT NOT NULL,
  `codigoClienteCierreSaldoCliente` VARCHAR(30) NOT NULL,
  `nombreClienteCierreSaldoCliente` VARCHAR(100) NOT NULL,
  `saldoAnteriorCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `cargosPeriodoCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `abonosPeriodoCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `cargosAcumuladosCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `abonosAcumuladosCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `saldoActualCierreSaldoCliente` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CierreSaldoCliente` PRIMARY KEY (`idCierreMovimientoCxCEncabezado`, `idCierreSaldoCliente`),
  KEY `idxCierreSaldoClienteClienteCodigo` (`idCliente`),
  CONSTRAINT `uqCierreSaldoClienteClaveNegocio` UNIQUE (`idCierreMovimientoCxCEncabezado`, `idCliente`),
  CONSTRAINT `uqCierreSaldoClienteId` UNIQUE (`idCierreSaldoCliente`)
) ENGINE=InnoDB;

ALTER TABLE `tblCierreSaldoCliente`
  ADD CONSTRAINT `Fk_CierreSaldoCliente_CierreMovimientoCxCEncabezado` FOREIGN KEY (`idCierreMovimientoCxCEncabezado`)
    REFERENCES `tblCierreMovimientoCxCEncabezado` (`idCierreMovimientoCxCEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CierreSaldoCliente_Cliente` FOREIGN KEY (`idCliente`)
    REFERENCES `tblCliente` (`idCliente`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblTrasladoCxCEncabezado | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblTrasladoCxCEncabezado` (
  `idTrasladoCxCEncabezado` INT NOT NULL AUTO_INCREMENT,
  `idCierreMovimientoCxCEncabezado` INT NOT NULL,
  `claveIntegracionTrasladoCxCEncabezado` VARCHAR(80) NOT NULL,
  `estadoTrasladoCxCEncabezado` VARCHAR(20) NOT NULL DEFAULT 'PENDIENTE',
  `referenciaContableTrasladoCxCEncabezado` VARCHAR(100) NULL,
  `idUsuario` INT NOT NULL,
  `fechaConfirmacionTrasladoCxCEncabezado` DATETIME NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_TrasladoCxCEncabezado` PRIMARY KEY (`idTrasladoCxCEncabezado`),
  CONSTRAINT `uqTrasladoCxCEncabezadoclaveIntegracion` UNIQUE (`claveIntegracionTrasladoCxCEncabezado`),
  KEY `idxTrasladoCxCEncabezadoUsuarioCodigo` (`idUsuario`),
  CONSTRAINT `ckTrasladoCxCEncabezadoValoresEstado` CHECK (estadoTrasladoCxCEncabezado IN ('PENDIENTE','PREPARADO','ENVIADO','CONFIRMADO','RECHAZADO')),
  CONSTRAINT `uqTrasladoCxCEncabezadoClaveNegocio` UNIQUE (`idCierreMovimientoCxCEncabezado`)
) ENGINE=InnoDB;

ALTER TABLE `tblTrasladoCxCEncabezado`
  ADD CONSTRAINT `Fk_TrasladoCxCEncabezado_CierreMovimientoCxCEncabezado` FOREIGN KEY (`idCierreMovimientoCxCEncabezado`)
    REFERENCES `tblCierreMovimientoCxCEncabezado` (`idCierreMovimientoCxCEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_TrasladoCxCEncabezado_Usuario` FOREIGN KEY (`idUsuario`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblTrasladoCxCDetalle | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblTrasladoCxCDetalle` (
  `idTrasladoCxCDetalle` INT NOT NULL AUTO_INCREMENT,
  `idTrasladoCxCEncabezado` INT NOT NULL,
  `numeroLineaTrasladoCxCDetalle` INT NOT NULL,
  `cuentaTrasladoCxCDetalle` VARCHAR(30) NOT NULL,
  `descripcionTrasladoCxCDetalle` VARCHAR(100) NOT NULL,
  `debeTrasladoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `haberTrasladoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_TrasladoCxCDetalle` PRIMARY KEY (`idTrasladoCxCEncabezado`, `idTrasladoCxCDetalle`),
  CONSTRAINT `ckTrasladoCxCDetalleDebeHaberNoNegativos` CHECK (debeTrasladoCxCDetalle >= 0 AND haberTrasladoCxCDetalle >= 0),
  CONSTRAINT `uqTrasladoCxCDetalleClaveNegocio` UNIQUE (`idTrasladoCxCEncabezado`, `numeroLineaTrasladoCxCDetalle`),
  CONSTRAINT `uqTrasladoCxCDetalleId` UNIQUE (`idTrasladoCxCDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblTrasladoCxCDetalle`
  ADD CONSTRAINT `Fk_TrasladoCxCDetalle_TrasladoCxCEncabezado` FOREIGN KEY (`idTrasladoCxCEncabezado`)
    REFERENCES `tblTrasladoCxCEncabezado` (`idTrasladoCxCEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- tblCierreMovimientoCxCDetalle | Autor: Jose Torres - 0901-23-1091 | Modulo: 7000 | Aplicacion: 7010
CREATE TABLE `tblCierreMovimientoCxCDetalle` (
  `idCierreMovimientoCxCDetalle` INT NOT NULL AUTO_INCREMENT,
  `idCierreMovimientoCxCEncabezado` INT NOT NULL,
  `idFacturaEncabezado` INT NOT NULL,
  `saldoInicialCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `cargosPeriodoCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `abonosPeriodoCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `saldoFinalCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CierreMovimientoCxCDetalle` PRIMARY KEY (`idCierreMovimientoCxCEncabezado`, `idCierreMovimientoCxCDetalle`),
  CONSTRAINT `uqCierreMovimientoCxCDetalleCierreMovimientoCxCEncabeza4f1c6502` UNIQUE (`idCierreMovimientoCxCEncabezado`, `idFacturaEncabezado`),
  KEY `idxCierreMovimientoCxCDetalleFacturaEncabezadoCodigo` (`idFacturaEncabezado`),
  CONSTRAINT `uqCierreMovimientoCxCDetalleId` UNIQUE (`idCierreMovimientoCxCDetalle`)
) ENGINE=InnoDB;

ALTER TABLE `tblCierreMovimientoCxCDetalle`
  ADD CONSTRAINT `Fk_CierreMovimientoCxCDetalle_CierreMovimientoCxCEncabezado` FOREIGN KEY (`idCierreMovimientoCxCEncabezado`)
    REFERENCES `tblCierreMovimientoCxCEncabezado` (`idCierreMovimientoCxCEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `Fk_CierreMovimientoCxCDetalle_FacturaEncabezado` FOREIGN KEY (`idFacturaEncabezado`)
    REFERENCES `tblFacturaEncabezado` (`idFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT;

-- bancos 2


-- Victor Samayoa 0901-23-3424
SET @bancoBI = (
SELECT idBanco FROM tblBanco
WHERE codigoBanco = 'BI'
);
SET @bancoBanrural = (
SELECT idBanco FROM tblBanco
WHERE codigoBanco = 'BANRURAL'
);
SET @bancoGYT = (
SELECT idBanco FROM tblBanco
WHERE codigoBanco = 'GYT'
);
SET @bancoBAC = (
SELECT idBanco FROM tblBanco
WHERE codigoBanco = 'BAC'
);
-- Obtener los IDs de las monedas
SET @monedaGTQ = (
SELECT idMoneda FROM tblMoneda
WHERE codigoMoneda = 'GTQ'
);
SET @monedaUSD = (
SELECT idMoneda FROM tblMoneda
WHERE codigoMoneda = 'USD'
);
SET @monedaEUR = (
SELECT idMoneda FROM tblMoneda
WHERE codigoMoneda = 'EUR'
);
-- Obtener los IDs de los tipos de cuenta
SET @tipoMonetaria = (
SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria
WHERE codigoTipoCuentaBancaria = 'MONETARIA'
);
SET @tipoAhorro = (
SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria
WHERE codigoTipoCuentaBancaria = 'AHORRO'
);
SET @tipoPlazoFijo = (
SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria
WHERE codigoTipoCuentaBancaria = 'PLAZO_FIJO'
);

-- Victor Samayoa 0901-23-3424
-- Vistas de consulta: datos derivados sin almacenarlos dos veces.
-- Usar tblAutPagoOrden para INSERT/UPDATE de los campos propios de la asignacion.
CREATE SQL SECURITY INVOKER VIEW vwAutPagoOrdenDetalle AS
SELECT
    p.idAutPagoOrden,
    d.Fk_AutOrdenCompra_Codigo,
    d.Fk_CuentaBancaria_Codigo,
    p.Fk_AutDisponibilidadFondos_Codigo,
    p.montoAsignadoAutPagoOrden,
    p.estadoAutPagoOrden,
    p.fechaReservaAutPagoOrden,
    p.Fk_OrdenPago_Codigo,
    p.created_at,
    p.updated_at
FROM tblAutPagoOrden AS p
INNER JOIN tblAutDisponibilidadFondos AS d
    ON d.idAutDisponibilidadFondos = p.Fk_AutDisponibilidadFondos_Codigo;

-- Victor Samayoa 0901-23-3424
-- Usar tblPartidaConsolidada para INSERT/UPDATE, sin enviar fechaFinPartidaConsolidada.
CREATE SQL SECURITY INVOKER VIEW vwPartidaConsolidadaPeriodo AS
SELECT
    p.Pk_PartidaConsolidada AS idPartidaConsolidada,
    p.identificadorEmpresaPartidaConsolidada,
    p.fechaInicioPartidaConsolidada,
    LAST_DAY(p.fechaInicioPartidaConsolidada) AS fechaFinPartidaConsolidada,
    p.conceptoPartidaConsolidada,
    p.Fk_Moneda_Codigo,
    p.totalDebePartidaConsolidada,
    p.totalHaberPartidaConsolidada,
    p.estadoPartidaConsolidada,
    p.identificadorUsuarioGeneraPartidaConsolidada,
    p.fechaGeneracionPartidaConsolidada,
    p.identificadorUsuarioTrasladaPartidaConsolidada,
    p.fechaTrasladoPartidaConsolidada,
    p.claveTrasladoPartidaConsolidada,
    p.referenciaContabilidadPartidaConsolidada,
    p.mensajeResultadoTrasladoPartidaConsolidada,
    p.created_at,
    p.updated_at
FROM tblPartidaConsolidada AS p;    

-- Tablas y modificaciones para Oscar Morales 9959-23-3070
CREATE TABLE tblConceptoBancario (
    idConceptoBancario BIGINT NOT NULL AUTO_INCREMENT,
    codigoConceptoBancario VARCHAR(30) NOT NULL,
    nombreConceptoBancario VARCHAR(255) NOT NULL,
    descripcionConceptoBancario VARCHAR(500) NULL,
    naturalezaConceptoBancario VARCHAR(30) NOT NULL,
    claseOperacionConceptoBancario VARCHAR(30) NOT NULL DEFAULT 'NORMAL',
    tipoTerceroConceptoBancario VARCHAR(30) NOT NULL DEFAULT 'NINGUNO',
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_ConceptoBancario PRIMARY KEY (idConceptoBancario),
    CONSTRAINT uqConceptoBancarioCodigo UNIQUE (codigoConceptoBancario),
    CONSTRAINT ckConceptoBancarioNaturaleza CHECK (naturalezaConceptoBancario IN ('INGRESO','EGRESO','AMBOS')),
    CONSTRAINT ckConceptoBancarioClase CHECK (claseOperacionConceptoBancario IN ('NORMAL','TRANSFERENCIA_INTERNA')),
    CONSTRAINT ckConceptoBancarioTercero CHECK (tipoTerceroConceptoBancario IN ('EMPLEADO','PROVEEDOR','CLIENTE','NINGUNO')),
    CONSTRAINT ckConceptoBancarioTransferencia CHECK (claseOperacionConceptoBancario <> 'TRANSFERENCIA_INTERNA' OR (naturalezaConceptoBancario = 'AMBOS' AND tipoTerceroConceptoBancario = 'NINGUNO')),
    CONSTRAINT ckConceptoBancarioActivo CHECK (is_active IN (0,1))
) ENGINE=InnoDB;

CREATE TABLE tblTransferenciaInterna (
    idTransferenciaInterna BIGINT NOT NULL AUTO_INCREMENT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_TransferenciaInterna PRIMARY KEY (idTransferenciaInterna)
) ENGINE=InnoDB;

-- Tablas y modificaciones para Oscar Morales 9959-23-3070
-- CAMBIOS DE MOVIMIENTO BANCARIO -- 
-- CAMBIOS DE MOVIMIENTO BANCARIO (CORREGIDO)
ALTER TABLE tblMovimientoBancario
ADD COLUMN idConceptoBancario BIGINT NULL,
ADD COLUMN idMetodoPago INT NULL,
ADD COLUMN idTransferenciaInterna BIGINT NULL,
ADD COLUMN idMovimientoRevertido BIGINT NULL,

ADD INDEX idxMovimientoConcepto (idConceptoBancario),
ADD INDEX idxMovimientoMetodoPago (idMetodoPago),
ADD INDEX idxMovimientoTransferencia (idTransferenciaInterna),
ADD UNIQUE KEY uqMovimientoReversion (idMovimientoRevertido),

-- ✅ Restricciones con prefijo Fk_
ADD CONSTRAINT Fk_MovimientoBancario_ConceptoBancario FOREIGN KEY (idConceptoBancario) REFERENCES tblConceptoBancario(idConceptoBancario) ON DELETE RESTRICT ON UPDATE RESTRICT,

ADD CONSTRAINT Fk_MovimientoBancario_MetodoPago FOREIGN KEY (idMetodoPago) REFERENCES tblMetodoPago(idMetodoPago) ON DELETE RESTRICT ON UPDATE RESTRICT,

ADD CONSTRAINT Fk_MovimientoBancario_TransferenciaInterna FOREIGN KEY (idTransferenciaInterna) REFERENCES tblTransferenciaInterna(idTransferenciaInterna) ON DELETE RESTRICT ON UPDATE RESTRICT,

ADD CONSTRAINT Fk_MovimientoBancario_MovimientoRevertido FOREIGN KEY (idMovimientoRevertido) REFERENCES tblMovimientoBancario(idMovimientoBancario) ON DELETE RESTRICT ON UPDATE RESTRICT;


-- Tablas y modificaciones para Movimiento Bancario Tercero (CORREGIDO)
CREATE TABLE tblMovimientoBancarioTercero (
    idMovimientoBancarioTercero BIGINT NOT NULL AUTO_INCREMENT,
    idMovimientoBancario BIGINT NOT NULL,
    idEmpleado INT NULL,
    idProveedor INT NULL,
    idCliente INT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT Pk_MovimientoBancarioTercero PRIMARY KEY (idMovimientoBancarioTercero),
    
    CONSTRAINT uqMovimientoBancarioTercero UNIQUE (idMovimientoBancario),
    
    CONSTRAINT ckAsociativaUnTercero CHECK ((idEmpleado IS NOT NULL) + (idProveedor IS NOT NULL) + (idCliente IS NOT NULL) = 1),
    CONSTRAINT ckAsociativaEstado CHECK (is_active IN (0,1)),
    

    CONSTRAINT Fk_MovimientoBancarioTercero_MovimientoBancario FOREIGN KEY (idMovimientoBancario) REFERENCES tblMovimientoBancario(idMovimientoBancario) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_MovimientoBancarioTercero_Empleado FOREIGN KEY (idEmpleado) REFERENCES tblEmpleado(idEmpleado) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_MovimientoBancarioTercero_Proveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor(idProveedor) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT Fk_MovimientoBancarioTercero_Cliente FOREIGN KEY (idCliente) REFERENCES tblCliente(idCliente) ON DELETE RESTRICT ON UPDATE RESTRICT,
    
    INDEX idxAsociativaEmpleado (idEmpleado),
    INDEX idxAsociativaProveedor (idProveedor),
    INDEX idxAsociativaCliente (idCliente)
) ENGINE=InnoDB;

ALTER TABLE tblOrdenPago
    ADD COLUMN Fk_SolicitudPagoTransporte_Codigo INT NULL AFTER Fk_DetallePlanillaRecibida_Codigo,
    ADD INDEX idxOrdenPagoTransporte (Fk_SolicitudPagoTransporte_Codigo),
    ADD CONSTRAINT Fk_OrdenPago_SolicitudTransporte FOREIGN KEY (Fk_SolicitudPagoTransporte_Codigo)
        REFERENCES tblSolicitudPagoTransporte (idSolicitudPagoTransporte) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE tblOrdenPago DROP CHECK ckOrdenPago4;

ALTER TABLE tblOrdenPago
    ADD CONSTRAINT ckOrdenPago4 CHECK
        ((Fk_OrdenCompraRecibida_Codigo IS NOT NULL) + (Fk_DetallePlanillaRecibida_Codigo IS NOT NULL)
         + (Fk_SolicitudPagoTransporte_Codigo IS NOT NULL) = 1);

INSERT INTO tblEstadoSolicitudPago (codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES ('PAGADO', 'Pagado', 'Pago realizado desde Bancos');
ALTER TABLE tblOrdenPago DROP CHECK ckOrdenPago2;
ALTER TABLE tblOrdenPago DROP CHECK ckOrdenPago5;
ALTER TABLE tblCuentaBancaria ADD COLUMN saldoInicialCuentaBancaria DECIMAL(12,4) NOT
NULL DEFAULT 0.0000 AFTER limiteSobregiroCuentaBancaria;
CREATE TABLE tblCuentaBancariaTercero (
idCuentaBancariaTercero BIGINT NOT NULL AUTO_INCREMENT,
numeroCuentaBancariaTercero VARCHAR(100) NOT NULL,
nombreTitularCuentaBancariaTercero VARCHAR(255) NOT NULL,
idBanco BIGINT NOT NULL,
idMoneda BIGINT NOT NULL,
idEmpleado INT NULL,
idProveedor INT NULL,
idCliente INT NULL,
is_active BOOLEAN NOT NULL DEFAULT TRUE,
created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE
CURRENT_TIMESTAMP,
CONSTRAINT Pk_CuentaBancariaTercero PRIMARY KEY (idCuentaBancariaTercero),
CONSTRAINT uqCuentaBancariaTercero UNIQUE (idBanco, numeroCuentaBancariaTercero),
CONSTRAINT ckCuentaBancariaTerceroPropietario CHECK ((idEmpleado IS NOT NULL) +
(idProveedor IS NOT NULL) + (idCliente IS NOT NULL) = 1),
CONSTRAINT Fk_CuentaBancariaTercero_Banco FOREIGN KEY (idBanco) REFERENCES
tblBanco(idBanco),
CONSTRAINT Fk_CuentaBancariaTercero_Moneda FOREIGN KEY (idMoneda) REFERENCES
tblMoneda(idMoneda),
CONSTRAINT Fk_CuentaBancariaTercero_Empleado FOREIGN KEY (idEmpleado) REFERENCES
tblEmpleado(idEmpleado),
CONSTRAINT Fk_CuentaBancariaTercero_Proveedor FOREIGN KEY (idProveedor) REFERENCES
tblProveedor(idProveedor),
CONSTRAINT Fk_CuentaBancariaTercero_Cliente FOREIGN KEY (idCliente) REFERENCES
tblCliente(idCliente),
INDEX idxCuentaTerceroEmpleado (idEmpleado),
INDEX idxCuentaTerceroProveedor (idProveedor),
INDEX idxCuentaTerceroCliente (idCliente),
CONSTRAINT ckCuentaTerceroActivo CHECK (is_active IN (0,1))
) ENGINE=InnoDB;
ALTER TABLE tblMovimientoBancarioTercero
ADD COLUMN idCuentaBancariaTercero BIGINT NULL,
ADD INDEX idxMovimientoTerceroCuenta (idCuentaBancariaTercero),
ADD CONSTRAINT Fk_MovimientoTercero_CuentaBancariaTercero
FOREIGN KEY (idCuentaBancariaTercero)
REFERENCES tblCuentaBancariaTercero(idCuentaBancariaTercero)
ON DELETE RESTRICT
ON UPDATE RESTRICT;
ALTER TABLE tblMovimientoBancario
ADD CONSTRAINT uqMovimientoReferencia
UNIQUE (referenciaMovimientoBancario);
CREATE TABLE IF NOT EXISTS tblDisponibilidadDiariaHistorial (
idDisponibilidadDiariaHistorial BIGINT NOT NULL AUTO_INCREMENT,
Fk_DisponibilidadDiaria BIGINT NOT NULL,
Fk_CuentaBancaria BIGINT NOT NULL,
fechaCorte DATE NOT NULL,
accion VARCHAR(12) NOT NULL,
versionAnterior BIGINT NULL,
versionNueva BIGINT NOT NULL,
usuarioId INT NULL,
usuarioNombre VARCHAR(100) NULL,
fechaOperacion DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
monedaAnterior CHAR(3) NULL,
monedaNueva CHAR(3) NOT NULL,
saldoInicialAnterior DECIMAL(12,4) NULL,
ingresosAnterior DECIMAL(12,4) NULL,
egresosAnterior DECIMAL(12,4) NULL,
comprometidoAnterior DECIMAL(12,4) NULL,
restringidoAnterior DECIMAL(12,4) NULL,
tasaAnterior DECIMAL(12,4) NULL,
fuenteAnterior VARCHAR(100) NULL,
saldoInicialNuevo DECIMAL(12,4) NOT NULL,
ingresosNuevo DECIMAL(12,4) NOT NULL,
egresosNuevo DECIMAL(12,4) NOT NULL,
comprometidoNuevo DECIMAL(12,4) NOT NULL,
restringidoNuevo DECIMAL(12,4) NOT NULL,
tasaNueva DECIMAL(12,4) NOT NULL,
fuenteNueva VARCHAR(100) NOT NULL,
PRIMARY KEY (idDisponibilidadDiariaHistorial),
CONSTRAINT uqDisponibilidadHistorialVersion UNIQUE (Fk_DisponibilidadDiaria, versionNueva),
KEY ixDisponibilidadHistorialCuentaFecha (Fk_CuentaBancaria, fechaCorte, fechaOperacion),
CONSTRAINT fkDisponibilidadHistorialCorte FOREIGN KEY (Fk_DisponibilidadDiaria)
REFERENCES tblDisponibilidadDiaria(idDisponibilidadDiaria)
ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB;
-- =============================================================================
--  TERMINUS ERP  |  Contabilidad
--  Script 2 de 2: datos
-- =============================================================================

-- =============================================================================
--  1. DATOS DE CATÁLOGOS
-- =============================================================================

-- Inicio de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026
INSERT INTO tblPais (idPais, nombrePais, codigoMonedaPais) VALUES
(1, 'Guatemala',   'GTQ'),
(2, 'El Salvador', 'USD'),
(3, 'Honduras',    'HNL');
-- Fin de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026

INSERT INTO tblModuloContable (idModuloContable, nombreModuloContable, codigoModuloContable) VALUES
(1, 'Contabilidad',    'MOD-CONTA'),
(2, 'Logística',       'MOD-LOGIS'),
(3, 'Compras',         'MOD-COMPR'),
(4, 'Recursos Humanos','MOD-RRHH'),
(5, 'Ventas',          'MOD-VENTA'),
(6, 'Bancos',          'MOD-BANCO');

INSERT INTO tblMaestroVidaUtil
    (idMaestroVidaUtil, codigoMaestroVidaUtil, nombreMaestroVidaUtil,
     cantidadMesesMaestroVidaUtil, tasaDepreciacionMaestroVidaUtil) VALUES
(1, 'EDIFICIO',    'Edificios e instalaciones',        240, 0.05),
(2, 'MAQUINARIA',  'Maquinaria y equipo de planta',    120, 0.20),
(3, 'VEHICULO',    'Equipo de transporte',              60, 0.20),
(4, 'MOBILIARIO',  'Mobiliario y equipo de oficina',    60, 0.20),
(5, 'EQ_COMPUTO',  'Equipo electronico de computo',     36, 0.33);

-- Catálogo de cuentas contables (254 registros)
INSERT INTO tblCuenta (idCuenta, codigoCuenta, nombreCuenta, tipoCuenta, idCuentaPadre, is_active) VALUES
(1, '0', 'Padre', 'DEUDORA', NULL, TRUE),
(2, '1', 'Activo', 'DEUDORA', 1, TRUE),
(3, '1.01', 'Activo Corriente', 'DEUDORA', 2, TRUE),
(4, '1.01.01', 'Caja', 'DEUDORA', 3, TRUE),
(5, '1.01.01.001', 'Caja General', 'DEUDORA', 4, TRUE),
(6, '1.01.01.002', 'Caja Chica', 'DEUDORA', 4, TRUE),
(7, '1.01.02', 'Bancos', 'DEUDORA', 3, TRUE),
(8, '1.01.02.001', 'Banco Industrial (Moneda Nacional)', 'DEUDORA', 7, TRUE),
(9, '1.01.02.002', 'Banrural (Moneda Nacional)', 'DEUDORA', 7, TRUE),
(10, '1.01.02.003', 'Banco G&T Continental (Moneda Nacional)', 'DEUDORA', 7, TRUE),
(11, '1.01.02.004', 'Banco Industrial (Moneda Extranjera)', 'DEUDORA', 7, TRUE),
(12, '1.01.03', 'Inversiones Temporales', 'DEUDORA', 3, TRUE),
(13, '1.01.03.001', 'Inversiones en Moneda Nacional', 'DEUDORA', 12, TRUE),
(14, '1.01.03.002', 'Inversiones en Moneda Extranjera', 'DEUDORA', 12, TRUE),
(15, '1.01.04', 'Cuentas por Cobrar Comerciales', 'DEUDORA', 3, TRUE),
(16, '1.01.04.001', 'Clientes Nacionales', 'DEUDORA', 15, TRUE),
(17, '1.01.04.002', 'Clientes del Extranjero (Exportación)', 'DEUDORA', 15, TRUE),
(18, '1.01.04.003', 'Estimación para Cuentas Incobrables', 'ACREEDORA', 15, TRUE),
(19, '1.01.05', 'Otras Cuentas por Cobrar', 'DEUDORA', 3, TRUE),
(20, '1.01.05.001', 'Deudores Diversos', 'DEUDORA', 19, TRUE),
(21, '1.01.05.002', 'Gastos por Comprobar', 'DEUDORA', 19, TRUE),
(22, '1.01.05.003', 'Anticipos a Proveedores', 'DEUDORA', 19, TRUE),
(23, '1.01.06', 'Inventarios', 'DEUDORA', 3, TRUE),
(24, '1.01.06.001', 'Inventario de Materias Primas', 'DEUDORA', 23, TRUE),
(25, '1.01.06.002', 'Inventario de Producción en Proceso', 'DEUDORA', 23, TRUE),
(26, '1.01.06.003', 'Inventario de Productos Terminados', 'DEUDORA', 23, TRUE),
(27, '1.01.06.004', 'Mercancías en Tránsito', 'DEUDORA', 23, TRUE),
(28, '1.01.07', 'Impuestos por Cobrar', 'DEUDORA', 3, TRUE),
(29, '1.01.07.001', 'IVA Acreditable', 'DEUDORA', 28, TRUE),
(30, '1.02', 'Propiedad, Planta y Equipo', 'DEUDORA', 2, TRUE),
(31, '1.02.01', 'Terrenos', 'DEUDORA', 30, TRUE),
(32, '1.02.01.001', 'Terrenos de Planta de Producción', 'DEUDORA', 31, TRUE),
(33, '1.02.01.002', 'Terrenos de Oficinas Administrativas', 'DEUDORA', 31, TRUE),
(34, '1.02.02', 'Edificios', 'DEUDORA', 30, TRUE),
(35, '1.02.02.001', 'Edificios de Planta', 'DEUDORA', 34, TRUE),
(36, '1.02.02.002', 'Edificios de Oficinas', 'DEUDORA', 34, TRUE),
(37, '1.02.03', 'Maquinaria y Equipo', 'DEUDORA', 30, TRUE),
(38, '1.02.03.001', 'Maquinaria de Producción de Embutidos', 'DEUDORA', 37, TRUE),
(39, '1.02.03.002', 'Equipo Auxiliar de Planta', 'DEUDORA', 37, TRUE),
(40, '1.02.04', 'Equipo de Transporte', 'DEUDORA', 30, TRUE),
(41, '1.02.04.001', 'Camiones de Reparto Refrigerados', 'DEUDORA', 40, TRUE),
(42, '1.02.04.002', 'Camionetas de Transporte de Personal', 'DEUDORA', 40, TRUE),
(43, '1.02.05', 'Mobiliario y Equipo de Oficina', 'DEUDORA', 30, TRUE),
(44, '1.02.05.001', 'Mobiliario de Oficinas', 'DEUDORA', 43, TRUE),
(45, '1.02.05.002', 'Equipo de Oficina', 'DEUDORA', 43, TRUE),
(46, '1.02.06', 'Equipo Electrónico de Cómputo', 'DEUDORA', 30, TRUE),
(47, '1.02.06.001', 'Computadoras y Servidores', 'DEUDORA', 46, TRUE),
(48, '1.02.06.002', 'Equipo Periférico', 'DEUDORA', 46, TRUE),
(49, '1.02.07', 'Depreciaciones y Amortizaciones', 'ACREEDORA', 30, TRUE),
(50, '1.02.07.001', 'Depreciación Acumulada de Edificios', 'ACREEDORA', 49, TRUE),
(51, '1.02.07.002', 'Depreciación Acumulada de Maquinaria y Equipo', 'ACREEDORA', 49, TRUE),
(52, '1.02.07.003', 'Depreciación Acumulada de Equipo de Transporte', 'ACREEDORA', 49, TRUE),
(53, '1.02.07.004', 'Depreciación Acumulada de Mobiliario y Equipo de Oficina', 'ACREEDORA', 49, TRUE),
(54, '1.02.07.005', 'Depreciación Acumulada de Equipo Electrónico de Cómputo', 'ACREEDORA', 49, TRUE),
(55, '1.02.07.006', 'Amortización Acumulada de Gastos de Instalación', 'ACREEDORA', 49, TRUE),
(56, '1.02.07.007', 'Amortización Acumulada de Gastos de Organización', 'ACREEDORA', 49, TRUE),
(57, '1.03', 'Activo Diferido', 'DEUDORA', 2, TRUE),
(58, '1.03.01', 'Gastos de Instalación', 'DEUDORA', 57, TRUE),
(59, '1.03.01.001', 'Gastos de Instalación de Planta', 'DEUDORA', 58, TRUE),
(60, '1.03.02', 'Gastos de Organización', 'DEUDORA', 57, TRUE),
(61, '1.03.02.001', 'Gastos de Organización', 'DEUDORA', 60, TRUE),
(62, '1.03.03', 'Pagos Anticipados', 'DEUDORA', 57, TRUE),
(63, '1.03.03.001', 'Rentas Pagadas por Anticipado', 'DEUDORA', 62, TRUE),
(64, '1.03.03.002', 'Primas de Seguros Pagadas por Anticipado', 'DEUDORA', 62, TRUE),
(65, '1.03.03.003', 'Papelería y Útiles de Escritorio', 'DEUDORA', 62, TRUE),
(66, '1.03.03.004', 'Propaganda y Publicidad', 'DEUDORA', 62, TRUE),
(67, '1.04', 'Activos Intangibles', 'DEUDORA', 2, TRUE),
(68, '1.04.01', 'Derecho de Llave', 'DEUDORA', 67, TRUE),
(69, '1.04.01.001', 'Derecho de Llave', 'DEUDORA', 68, TRUE),
(70, '1.04.02', 'Marcas y Patentes', 'DEUDORA', 67, TRUE),
(71, '1.04.02.001', 'Marcas', 'DEUDORA', 70, TRUE),
(72, '1.04.02.002', 'Patentes', 'DEUDORA', 70, TRUE),
(73, '1.04.03', 'Licencias y Software', 'DEUDORA', 67, TRUE),
(74, '1.04.03.001', 'Licencias', 'DEUDORA', 73, TRUE),
(75, '1.04.03.002', 'Software', 'DEUDORA', 73, TRUE),
(76, '1.04.04', 'Amortización Acumulada de Activos Intangibles', 'ACREEDORA', 67, TRUE),
(77, '1.04.04.001', 'Amortización Acumulada de Derecho de Llave', 'ACREEDORA', 76, TRUE),
(78, '1.04.04.002', 'Amortización Acumulada de Marcas y Patentes', 'ACREEDORA', 76, TRUE),
(79, '1.04.04.003', 'Amortización Acumulada de Licencias y Software', 'ACREEDORA', 76, TRUE),
(80, '2', 'Pasivo', 'ACREEDORA', 1, TRUE),
(81, '2.01', 'Pasivo Corriente', 'ACREEDORA', 80, TRUE),
(82, '2.01.01', 'Proveedores', 'ACREEDORA', 81, TRUE),
(83, '2.01.01.001', 'Proveedores Nacionales', 'ACREEDORA', 82, TRUE),
(84, '2.01.01.002', 'Proveedores Extranjeros', 'ACREEDORA', 82, TRUE),
(85, '2.01.02', 'Sueldos y Salarios por Pagar', 'ACREEDORA', 81, TRUE),
(86, '2.01.02.001', 'Sueldos por Pagar (Personal Administrativo)', 'ACREEDORA', 85, TRUE),
(87, '2.01.02.002', 'Salarios por Pagar (Personal de Planta)', 'ACREEDORA', 85, TRUE),
(88, '2.01.02.003', 'Comisiones por Pagar (Ventas)', 'ACREEDORA', 85, TRUE),
(89, '2.01.03', 'Impuestos por Pagar', 'ACREEDORA', 81, TRUE),
(90, '2.01.03.001', 'ISR por Pagar', 'ACREEDORA', 89, TRUE),
(91, '2.01.03.002', 'IVA Trasladado (por pagar)', 'ACREEDORA', 89, TRUE),
(92, '2.01.03.003', 'Cuotas Obrero-Patronales IGSS', 'ACREEDORA', 89, TRUE),
(93, '2.01.03.004', 'Cuotas Obrero-Patronales IRTRA', 'ACREEDORA', 89, TRUE),
(94, '2.01.03.005', '2% Sobre Nóminas', 'ACREEDORA', 89, TRUE),
(95, '2.01.04', 'Créditos Bancarios por Pagar (Corto Plazo)', 'ACREEDORA', 81, TRUE),
(96, '2.01.04.001', 'Banco Industrial (MN)', 'ACREEDORA', 95, TRUE),
(97, '2.01.04.002', 'Banrural (MN)', 'ACREEDORA', 95, TRUE),
(98, '2.01.04.003', 'Banco G&T Continental (MN)', 'ACREEDORA', 95, TRUE),
(99, '2.01.05', 'Documentos por Pagar (Corto Plazo)', 'ACREEDORA', 81, TRUE),
(100, '2.01.05.001', 'Documentos por Pagar a Proveedores Nacionales', 'ACREEDORA', 99, TRUE),
(101, '2.01.05.002', 'Documentos por Pagar a Proveedores Extranjeros', 'ACREEDORA', 99, TRUE),
(102, '2.01.06', 'Acreedores Diversos', 'ACREEDORA', 81, TRUE),
(103, '2.01.06.001', 'Acreedores Diversos por Conceptos Varios', 'ACREEDORA', 102, TRUE),
(104, '2.02', 'Pasivo Fijo', 'ACREEDORA', 80, TRUE),
(105, '2.02.01', 'Créditos Bancarios (Largo Plazo)', 'ACREEDORA', 104, TRUE),
(106, '2.02.01.001', 'Banco Industrial (Largo Plazo)', 'ACREEDORA', 105, TRUE),
(107, '2.02.01.002', 'Banco G&T Continental (Largo Plazo)', 'ACREEDORA', 105, TRUE),
(108, '2.02.02', 'Acreedores Hipotecarios', 'ACREEDORA', 104, TRUE),
(109, '2.02.02.001', 'Acreedores Hipotecarios', 'ACREEDORA', 108, TRUE),
(110, '2.02.03', 'Documentos por Pagar (Largo Plazo)', 'ACREEDORA', 104, TRUE),
(111, '2.02.03.001', 'Documentos por Pagar a Proveedores Nacionales (LP)', 'ACREEDORA', 110, TRUE),
(112, '2.02.03.002', 'Documentos por Pagar a Proveedores Extranjeros (LP)', 'ACREEDORA', 110, TRUE),
(113, '2.03', 'Pasivo Diferido', 'ACREEDORA', 80, TRUE),
(114, '2.03.01', 'Rentas Cobradas por Anticipado', 'ACREEDORA', 113, TRUE),
(115, '2.03.01.001', 'Rentas Cobradas por Anticipado', 'ACREEDORA', 114, TRUE),
(116, '2.03.02', 'Intereses Cobrados por Anticipado', 'ACREEDORA', 113, TRUE),
(117, '2.03.02.001', 'Intereses Cobrados por Anticipado', 'ACREEDORA', 116, TRUE),
(118, '3', 'Capital', 'ACREEDORA', 1, TRUE),
(119, '3.01', 'Capital Social', 'ACREEDORA', 118, TRUE),
(120, '3.01.01', 'Capital Social Fijo', 'ACREEDORA', 119, TRUE),
(121, '3.01.01.001', 'Inversionistas Nacionales', 'ACREEDORA', 120, TRUE),
(122, '3.01.01.002', 'Inversionistas Extranjeros', 'ACREEDORA', 120, TRUE),
(123, '3.01.02', 'Capital Social Variable', 'ACREEDORA', 119, TRUE),
(124, '3.01.02.001', 'Inversionistas Nacionales', 'ACREEDORA', 123, TRUE),
(125, '3.01.02.002', 'Inversionistas Extranjeros', 'ACREEDORA', 123, TRUE),
(126, '3.02', 'Resultados', 'ACREEDORA', 118, TRUE),
(127, '3.02.01', 'Resultado del Ejercicio', 'ACREEDORA', 126, TRUE),
(128, '3.02.01.001', 'Resultado del Ejercicio en Curso', 'ACREEDORA', 127, TRUE),
(129, '3.02.02', 'Resultados de Ejercicios Anteriores', 'ACREEDORA', 126, TRUE),
(130, '3.02.02.001', 'Resultados de Ejercicios Anteriores', 'ACREEDORA', 129, TRUE),
(131, '4', 'Resultados Acreedoras', 'ACREEDORA', 1, TRUE),
(132, '4.01', 'Ventas', 'ACREEDORA', 131, TRUE),
(133, '4.01.01', 'Ventas en Territorio Nacional', 'ACREEDORA', 132, TRUE),
(134, '4.01.01.001', 'Ventas de Embutidos – Línea Res', 'ACREEDORA', 133, TRUE),
(135, '4.01.01.002', 'Ventas de Embutidos – Línea Cerdo', 'ACREEDORA', 133, TRUE),
(136, '4.01.01.003', 'Ventas de Embutidos – Línea Pollo', 'ACREEDORA', 133, TRUE),
(137, '4.01.02', 'Ventas en Territorio Extranjero (Exportación)', 'ACREEDORA', 132, TRUE),
(138, '4.01.02.001', 'Ventas de Exportación', 'ACREEDORA', 137, TRUE),
(139, '4.02', 'Productos Financieros', 'ACREEDORA', 131, TRUE),
(140, '4.02.01', 'Descuentos Aprovechados por Pronto Pago', 'ACREEDORA', 139, TRUE),
(141, '4.02.01.001', 'Descuentos Aprovechados por Pronto Pago', 'ACREEDORA', 140, TRUE),
(142, '4.03', 'Otros Ingresos', 'ACREEDORA', 131, TRUE),
(143, '4.03.01', 'Ventas de Subproductos', 'ACREEDORA', 142, TRUE),
(144, '4.03.01.001', 'Venta de Subproductos (No considerados mercancía)', 'ACREEDORA', 143, TRUE),
(145, '4.03.02', 'Otros Ingresos Diversos', 'ACREEDORA', 142, TRUE),
(146, '4.03.02.001', 'Otros Ingresos Diversos', 'ACREEDORA', 145, TRUE),
(147, '4.04', 'Devoluciones, Rebajas y Bonificaciones sobre Compras', 'ACREEDORA', 131, TRUE),
(148, '4.04.01', 'Devoluciones sobre Compras', 'ACREEDORA', 147, TRUE),
(149, '4.04.01.001', 'Devoluciones sobre Compras Nacionales', 'ACREEDORA', 148, TRUE),
(150, '4.04.01.002', 'Devoluciones sobre Compras de Importación', 'ACREEDORA', 148, TRUE),
(151, '4.04.02', 'Rebajas sobre Compras', 'ACREEDORA', 147, TRUE),
(152, '4.04.02.001', 'Rebajas sobre Compras Nacionales', 'ACREEDORA', 151, TRUE),
(153, '4.04.02.002', 'Rebajas sobre Compras de Importación', 'ACREEDORA', 151, TRUE),
(154, '4.04.03', 'Bonificaciones sobre Compras', 'ACREEDORA', 147, TRUE),
(155, '4.04.03.001', 'Bonificaciones sobre Compras Nacionales', 'ACREEDORA', 154, TRUE),
(156, '4.04.03.002', 'Bonificaciones sobre Compras de Importación', 'ACREEDORA', 154, TRUE),
(157, '4.04.04', 'Descuentos sobre Compras', 'ACREEDORA', 147, TRUE),
(158, '4.04.04.001', 'Descuentos sobre Compras Nacionales', 'ACREEDORA', 157, TRUE),
(159, '4.04.04.002', 'Descuentos sobre Compras de Importación', 'ACREEDORA', 157, TRUE),
(160, '5', 'Resultados Deudoras', 'DEUDORA', 1, TRUE),
(161, '5.01', 'Compras', 'DEUDORA', 160, TRUE),
(162, '5.01.01', 'Compras de Materias Primas', 'DEUDORA', 161, TRUE),
(163, '5.01.01.001', 'Compras a Proveedores Nacionales', 'DEUDORA', 162, TRUE),
(164, '5.01.01.002', 'Compras a Proveedores Extranjeros (Importación)', 'DEUDORA', 162, TRUE),
(165, '5.01.02', 'Gastos sobre Compras', 'DEUDORA', 161, TRUE),
(166, '5.01.02.001', 'Gastos sobre Compras Nacionales', 'DEUDORA', 165, TRUE),
(167, '5.01.02.002', 'Gastos sobre Compras de Importación', 'DEUDORA', 165, TRUE),
(168, '5.02', 'Costo de Ventas', 'DEUDORA', 160, TRUE),
(169, '5.02.01', 'Costo de Ventas de Productos Terminados', 'DEUDORA', 168, TRUE),
(170, '5.02.01.001', 'Costo de Ventas – Línea Res', 'DEUDORA', 169, TRUE),
(171, '5.02.01.002', 'Costo de Ventas – Línea Cerdo', 'DEUDORA', 169, TRUE),
(172, '5.02.01.003', 'Costo de Ventas – Línea Pollo', 'DEUDORA', 169, TRUE),
(173, '5.02.02', 'Costo de Ventas de Mercancías', 'DEUDORA', 168, TRUE),
(174, '5.02.02.001', 'Costo de Ventas de Mercancías', 'DEUDORA', 173, TRUE),
(175, '5.02.03', 'Depreciaciones y Amortizaciones (Costo)', 'DEUDORA', 168, TRUE),
(176, '5.02.03.001', 'Depreciación de Maquinaria y Equipo (Producción)', 'DEUDORA', 175, TRUE),
(177, '5.02.03.002', 'Depreciación de Edificios de Planta', 'DEUDORA', 175, TRUE),
(178, '5.02.03.003', 'Amortización de Gastos de Instalación (Producción)', 'DEUDORA', 175, TRUE),
(179, '5.03', 'Gastos de Venta', 'DEUDORA', 160, TRUE),
(180, '5.03.01', 'Sueldos y Prestaciones (Personal de Ventas)', 'DEUDORA', 179, TRUE),
(181, '5.03.01.001', 'Sueldos Normales', 'DEUDORA', 180, TRUE),
(182, '5.03.01.002', 'Prestaciones Normales', 'DEUDORA', 180, TRUE),
(183, '5.03.01.003', 'Comisiones', 'DEUDORA', 180, TRUE),
(184, '5.03.01.004', 'Impuestos y Aportaciones Patronales', 'DEUDORA', 180, TRUE),
(185, '5.03.02', 'Gastos de Viaje', 'DEUDORA', 179, TRUE),
(186, '5.03.02.001', 'Gastos de Viaje', 'DEUDORA', 185, TRUE),
(187, '5.03.03', 'Servicios Básicos', 'DEUDORA', 179, TRUE),
(188, '5.03.03.001', 'Energía Eléctrica', 'DEUDORA', 187, TRUE),
(189, '5.03.03.002', 'Servicio Telefónico', 'DEUDORA', 187, TRUE),
(190, '5.03.03.003', 'Arrendamientos de Oficinas y Locales', 'DEUDORA', 187, TRUE),
(191, '5.03.04', 'Depreciaciones y Amortizaciones', 'DEUDORA', 179, TRUE),
(192, '5.03.04.001', 'Depreciación de Edificios', 'DEUDORA', 191, TRUE),
(193, '5.03.04.002', 'Depreciación de Maquinaria y Equipo', 'DEUDORA', 191, TRUE),
(194, '5.03.04.003', 'Depreciación de Equipo de Transporte', 'DEUDORA', 191, TRUE),
(195, '5.03.04.004', 'Depreciación de Mobiliario y Equipo de Oficina', 'DEUDORA', 191, TRUE),
(196, '5.03.04.005', 'Depreciación de Equipo Electrónico de Cómputo', 'DEUDORA', 191, TRUE),
(197, '5.03.04.006', 'Amortización de Gastos de Instalación', 'DEUDORA', 191, TRUE),
(198, '5.03.04.007', 'Amortización de Gastos de Organización', 'DEUDORA', 191, TRUE),
(199, '5.03.04.008', 'Amortización de Activos Intangibles', 'DEUDORA', 191, TRUE),
(200, '5.03.05', 'Propaganda y Publicidad', 'DEUDORA', 179, TRUE),
(201, '5.03.05.001', 'Propaganda Impresa', 'DEUDORA', 200, TRUE),
(202, '5.03.05.002', 'Propaganda en Televisión', 'DEUDORA', 200, TRUE),
(203, '5.03.05.003', 'Propaganda en Radio', 'DEUDORA', 200, TRUE),
(204, '5.03.05.004', 'Propaganda en Medios Impresos', 'DEUDORA', 200, TRUE),
(205, '5.04', 'Gastos de Administración', 'DEUDORA', 160, TRUE),
(206, '5.04.01', 'Sueldos y Prestaciones (Personal Administrativo)', 'DEUDORA', 205, TRUE),
(207, '5.04.01.001', 'Sueldos Normales', 'DEUDORA', 206, TRUE),
(208, '5.04.01.002', 'Sueldos Extraordinarios', 'DEUDORA', 206, TRUE),
(209, '5.04.01.003', 'Prestaciones Normales', 'DEUDORA', 206, TRUE),
(210, '5.04.01.004', 'Impuestos y Aportaciones Patronales', 'DEUDORA', 206, TRUE),
(211, '5.04.02', 'Gastos de Viaje', 'DEUDORA', 205, TRUE),
(212, '5.04.02.001', 'Gastos de Viaje', 'DEUDORA', 211, TRUE),
(213, '5.04.03', 'Servicios Básicos', 'DEUDORA', 205, TRUE),
(214, '5.04.03.001', 'Energía Eléctrica', 'DEUDORA', 213, TRUE),
(215, '5.04.03.002', 'Servicio Telefónico', 'DEUDORA', 213, TRUE),
(216, '5.04.03.003', 'Arrendamientos de Oficinas y Locales', 'DEUDORA', 213, TRUE),
(217, '5.04.04', 'Depreciaciones y Amortizaciones', 'DEUDORA', 205, TRUE),
(218, '5.04.04.001', 'Depreciación de Edificios', 'DEUDORA', 217, TRUE),
(219, '5.04.04.002', 'Depreciación de Maquinaria y Equipo', 'DEUDORA', 217, TRUE),
(220, '5.04.04.003', 'Depreciación de Equipo de Transporte', 'DEUDORA', 217, TRUE),
(221, '5.04.04.004', 'Depreciación de Mobiliario y Equipo de Oficina', 'DEUDORA', 217, TRUE),
(222, '5.04.04.005', 'Depreciación de Equipo Electrónico de Cómputo', 'DEUDORA', 217, TRUE),
(223, '5.04.04.006', 'Amortización de Gastos de Instalación', 'DEUDORA', 217, TRUE),
(224, '5.04.04.007', 'Amortización de Gastos de Organización', 'DEUDORA', 217, TRUE),
(225, '5.04.04.008', 'Amortización de Activos Intangibles', 'DEUDORA', 217, TRUE),
(226, '5.05', 'Gastos Financieros', 'DEUDORA', 160, TRUE),
(227, '5.05.01', 'Comisiones Bancarias', 'DEUDORA', 226, TRUE),
(228, '5.05.01.001', 'Comisiones Diversas', 'DEUDORA', 227, TRUE),
(229, '5.05.02', 'Intereses Pagados', 'DEUDORA', 226, TRUE),
(230, '5.05.02.001', 'Intereses Pagados a Instituciones Bancarias', 'DEUDORA', 229, TRUE),
(231, '5.05.02.002', 'Intereses Pagados a Compañías del Grupo', 'DEUDORA', 229, TRUE),
(232, '5.05.02.003', 'Intereses Pagados a Terceros', 'DEUDORA', 229, TRUE),
(233, '5.05.03', 'Pérdida en Cambios', 'DEUDORA', 226, TRUE),
(234, '5.05.03.001', 'Pérdida en Cambios por Compras en Moneda Extranjera', 'DEUDORA', 233, TRUE),
(235, '5.05.03.002', 'Pérdida en Cambios por Importaciones', 'DEUDORA', 233, TRUE),
(236, '5.05.03.003', 'Pérdida en Cambios por Operaciones Diversas', 'DEUDORA', 233, TRUE),
(237, '5.05.04', 'Descuentos por Pronto Pago', 'DEUDORA', 226, TRUE),
(238, '5.05.04.001', 'Descuentos por Pronto Pago', 'DEUDORA', 237, TRUE),
(239, '5.06', 'Otros Gastos', 'DEUDORA', 160, TRUE),
(240, '5.06.01', 'Otros Gastos Diversos', 'DEUDORA', 239, TRUE),
(241, '5.06.01.001', 'Otros Gastos Extraordinarios', 'DEUDORA', 240, TRUE),
(242, '5.06.01.002', 'Otros Gastos Diversos', 'DEUDORA', 240, TRUE),
(243, '6', 'Contingencia', 'ACREEDORA', 1, TRUE),
(244, '6.01', 'Cuentas de Contingencia', 'ACREEDORA', 243, TRUE),
(245, '6.01.01', 'Contingencias Laborales', 'ACREEDORA', 244, TRUE),
(246, '6.01.02', 'Contingencias Fiscales', 'ACREEDORA', 244, TRUE),
(247, '6.01.03', 'Contingencias Legales', 'ACREEDORA', 244, TRUE),
(248, '7', 'Cuentas de Orden', 'DEUDORA', 1, TRUE),
(249, '7.01', 'Cuentas de Orden Deudoras', 'DEUDORA', 248, TRUE),
(250, '7.01.01', 'Documentos en Custodia', 'DEUDORA', 249, TRUE),
(251, '7.01.02', 'Valores en Garantía', 'DEUDORA', 249, TRUE),
(252, '7.02', 'Cuentas de Orden Acreedoras', 'ACREEDORA', 248, TRUE),
(253, '7.02.01', 'Contrapartida de Documentos en Custodia', 'ACREEDORA', 252, TRUE),
(254, '7.02.02', 'Contrapartida de Valores en Garantía', 'ACREEDORA', 252, TRUE);

-- Clasificación de las cuentas: MAYOR si tiene cuentas hijas, DETALLE si es de último nivel
UPDATE tblCuenta
   SET clasificacionCuenta = 'MAYOR'
 WHERE idCuenta IN (SELECT idPadre
                       FROM (SELECT DISTINCT idCuentaPadre AS idPadre
                               FROM tblCuenta
                              WHERE idCuentaPadre IS NOT NULL) AS padres);

-- =============================================================================
--  1.1 ASIGNACIÓN INICIAL MÓDULO - CUENTA - PAÍS
-- =============================================================================

INSERT INTO tblModuloCuentaPais (idModuloContable, idCuenta, idPais)
SELECT m.idModuloContable, c.idCuenta, 1
FROM   tblModuloContable m
JOIN   tblCuenta c ON c.is_active = TRUE AND c.clasificacionCuenta = 'DETALLE'
WHERE  m.codigoModuloContable = 'MOD-CONTA'
   OR (m.codigoModuloContable = 'MOD-LOGIS' AND (c.codigoCuenta LIKE '1.01.06.%' OR c.codigoCuenta LIKE '1.02.04.%'
                                         OR c.codigoCuenta = '1.02.07.003' OR c.codigoCuenta LIKE '5.01.02.%'))
   OR (m.codigoModuloContable = 'MOD-COMPR' AND (c.codigoCuenta LIKE '1.01.06.%' OR c.codigoCuenta = '1.01.05.003'
                                         OR c.codigoCuenta LIKE '1.01.07.%' OR c.codigoCuenta LIKE '2.01.01.%'
                                         OR c.codigoCuenta LIKE '2.01.05.%' OR c.codigoCuenta LIKE '5.01.%'
                                         OR c.codigoCuenta LIKE '4.04.%'))
   OR (m.codigoModuloContable = 'MOD-RRHH' AND (c.codigoCuenta LIKE '5.03.01.%' OR c.codigoCuenta LIKE '5.04.01.%'
                                        OR c.codigoCuenta LIKE '2.01.02.%' OR c.codigoCuenta LIKE '2.01.03.%'))
   OR (m.codigoModuloContable = 'MOD-VENTA' AND (c.codigoCuenta LIKE '1.01.01.%' OR c.codigoCuenta LIKE '1.01.02.%'
                                         OR c.codigoCuenta LIKE '1.01.04.%' OR c.codigoCuenta LIKE '4.01.%'
                                         OR c.codigoCuenta LIKE '5.02.01.%' OR c.codigoCuenta = '1.01.06.003'
                                         OR c.codigoCuenta = '2.01.03.002'))
   OR (m.codigoModuloContable = 'MOD-BANCO' AND (c.codigoCuenta LIKE '1.01.01.%' OR c.codigoCuenta LIKE '1.01.02.%'
                                         OR c.codigoCuenta LIKE '1.01.03.%' OR c.codigoCuenta LIKE '1.01.04.%'
                                         OR c.codigoCuenta LIKE '2.01.01.%' OR c.codigoCuenta LIKE '2.01.04.%'
                                         OR c.codigoCuenta LIKE '2.02.01.%' OR c.codigoCuenta LIKE '4.02.%'
                                         OR c.codigoCuenta LIKE '5.05.%'));

-- Inicio de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026
INSERT IGNORE INTO tblModuloCuentaPais (idModuloContable, idCuenta, idPais)
SELECT mcp.idModuloContable, mcp.idCuenta, p.idPais
FROM   tblModuloCuentaPais mcp
JOIN   tblPais p ON p.idPais <> 1 AND p.is_active = TRUE
WHERE  mcp.idPais = 1;
-- Fin de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026

-- =============================================================================
--  1.2 CATÁLOGO DE IMPUESTOS (Solo Guatemala)
-- =============================================================================

-- Inicio de cambio: Dylan Hernández - 0901-23-519 - 30/10/2026
INSERT INTO tblImpuesto
    (idImpuesto, nombreImpuesto, descripcionImpuesto, tipoImpuesto, operacionImpuesto,
     valorImpuesto, periodicidadImpuesto, idCuentaAcreditableImpuesto, idCuentaPorPagarImpuesto) VALUES
(1,  'IVA',                      'Impuesto al Valor Agregado (Decreto 27-92)',              'CONSUMO',     'AMBAS',    0.1200, 'MENSUAL',    29,   91),
(2,  'IVA Pequeño Contribuyente','IVA Régimen Pequeño Contribuyente (Decreto 27-92)',      'CONSUMO',     'VENTA',    0.0500, 'MENSUAL',    NULL, 91),
(3,  'ISR Utilidades',           'ISR Régimen sobre las Utilidades (Decreto 10-2012)',      'RENTA',       'PERIODO',  0.2500, 'ANUAL',      NULL, 90),
(4,  'ISR Simplificado',         'ISR Régimen Opcional Simplificado (Decreto 10-2012)',     'RENTA',       'PERIODO',  0.0000, 'ANUAL',      NULL, 90),
(5,  'ISR Rentas del Trabajo',   'ISR Rentas del Trabajo en relación de dependencia',       'RENTA',       'PLANILLA', 0.0000, 'MENSUAL',    NULL, 90),
(6,  'ISO',                      'Impuesto de Solidaridad (Decreto 73-2008)',               'OTRO',        'PERIODO',  0.0100, 'TRIMESTRAL', NULL, NULL),
(7,  'IUSI',                     'Impuesto Único Sobre Inmuebles (Decreto 15-98)',          'PATRIMONIO',  'PERIODO',  0.0000, 'TRIMESTRAL', NULL, NULL),
(8,  'Timbres Fiscales',         'Impuesto de Timbres Fiscales (Decreto 37-92)',            'OTRO',        'AMBAS',    0.0300, 'POR_EVENTO', NULL, NULL),
(9,  'IGSS Patronal',            'Cuota patronal IGSS',                                     'CONTRIBUCION','PLANILLA', 0.1067, 'MENSUAL',    NULL, 92),
(10, 'IGSS Laboral',             'Cuota laboral IGSS',                                      'CONTRIBUCION','PLANILLA', 0.0483, 'MENSUAL',    NULL, 92),
(11, 'IRTRA',                    'Cuota patronal IRTRA',                                    'CONTRIBUCION','PLANILLA', 0.0100, 'MENSUAL',    NULL, 93);

-- Tramos ISR Simplificado (id 4)
INSERT INTO tblImpuestoTramo (idImpuesto, montoDesdeImpuestoTramo, montoHastaImpuestoTramo, tasaImpuestoTramo, importeFijoImpuestoTramo) VALUES
(4, 0.0000,    30000.0000, 0.0500, 0.0000),
(4, 30000.0000, NULL,      0.0700, 1500.0000);

-- Tramos ISR Rentas del Trabajo (id 5)
INSERT INTO tblImpuestoTramo (idImpuesto, montoDesdeImpuestoTramo, montoHastaImpuestoTramo, tasaImpuestoTramo, importeFijoImpuestoTramo) VALUES
(5, 0.0000,     300000.0000, 0.0500, 0.0000),
(5, 300000.0000, NULL,       0.0700, 15000.0000);

-- Tramos IUSI (id 7) - Decreto 15-98
INSERT INTO tblImpuestoTramo (idImpuesto, montoDesdeImpuestoTramo, montoHastaImpuestoTramo, tasaImpuestoTramo, importeFijoImpuestoTramo) VALUES
(7, 0.0000,     2000.0000,  0.0000, 0.0000),
(7, 2000.0000,  20000.0000, 0.0020, 0.0000),
(7, 20000.0000, 70000.0000, 0.0060, 36.0000),
(7, 70000.0000, NULL,       0.0090, 336.0000);
-- Fin de cambio: Dylan Hernández - 0901-23-519 - 30/10/2026

-- =============================================================================
--  1.3 BITÁCORA DE ACTUALIZACIÓN DE SALDOS
-- =============================================================================

INSERT INTO tblActualizacionSaldos
    (idActualizacionSaldos, modoActualizacionSaldos, tipoPeriodoActualizacionSaldos,
     fechaDesdeActualizacionSaldos, fechaHastaActualizacionSaldos, polizasProcesadasActualizacionSaldos, usuarioActualizacionSaldos) VALUES
(1, 'BATCH', 'MENSUAL', '2026-10-01', '2026-10-31', 5, 'Administrador');

-- =============================================================================
--  2. DATOS: PÓLIZAS — ENCABEZADOS
-- =============================================================================

INSERT INTO tblPolizaEncabezado
    (idPolizaEncabezado, idModuloContable, idActualizacionSaldos, codigoPolizaEncabezado, fechaPolizaEncabezado, periodoContablePolizaEncabezado,
     estadoPolizaEncabezado, documentoOrigenPolizaEncabezado, descripcionPolizaEncabezado, is_active) VALUES
(1, 5, 1,    'POL-2026-10-0001', '2026-10-02', '2026-10', 'APLICADA', 'FAC-A-1045',
    'Venta de embutidos al crédito, líneas Res y Cerdo, factura serie A-1045', TRUE),
(2, 5, 1,    'POL-2026-10-0002', '2026-10-02', '2026-10', 'APLICADA', 'FAC-A-1045-CV',
    'Registro del costo de ventas de la factura serie A-1045', TRUE),
(3, 3, 1,    'POL-2026-10-0003', '2026-10-05', '2026-10', 'APLICADA', 'FAC-B-7781',
    'Compra de materia prima al crédito a proveedor nacional, factura B-7781', TRUE),
(4, 4, NULL, 'POL-2026-10-0004', '2026-10-15', '2026-10', 'VALIDADA', 'PLA-2026-10-Q2',
    'Planilla quincenal del personal administrativo, segunda quincena', TRUE),
(5, 1, 1,    'POL-2026-10-0005', '2026-10-20', '2026-10', 'APLICADA', 'CHQ-004521',
    'Pago a proveedor nacional mediante cheque de Banco Industrial', TRUE),
(6, 1, NULL, 'POL-2026-10-0006', '2026-10-31', '2026-10', 'VALIDADA', 'DEP-2026-10',
    'Depreciación mensual de maquinaria de planta y equipo de cómputo', TRUE),
(7, 1, NULL, 'POL-2026-10-0007', '2026-10-31', '2026-10', 'BORRADOR', NULL,
    'Provisión de campaña publicitaria impresa, pendiente de aprobación', TRUE),
(8, 5, NULL, 'POL-2026-10-0008', '2026-10-28', '2026-10', 'ANULADA', 'REC-0098',
    'Venta de contado anulada por error de digitación en el monto', FALSE),
(9, 6, 1,    'POL-2026-10-0009', '2026-10-22', '2026-10', 'APLICADA', 'BOL-2026-9912',
    'Deposito por cobro de factura A-1045', TRUE),
(10, 6, NULL, 'POL-2026-10-0010', '2026-10-30', '2026-10', 'VALIDADA', 'NDB-2026-0451',
    'Nota de debito por comisiones bancarias del mes', TRUE);

-- =============================================================================
--  3. DATOS: PÓLIZAS — DETALLES
-- =============================================================================

INSERT INTO tblPolizaDetalle
    (idPolizaEncabezado, idCuenta, lineaPolizaDetalle, debePolizaDetalle, haberPolizaDetalle, descripcionPolizaDetalle) VALUES
(1,  16, 1, 56000.0000,     0.0000, 'Clientes Nacionales - factura A-1045'),
(1, 134, 2,     0.0000, 30000.0000, 'Ventas de Embutidos - Línea Res'),
(1, 135, 3,     0.0000, 20000.0000, 'Ventas de Embutidos - Línea Cerdo'),
(1,  91, 4,     0.0000,  6000.0000, 'IVA Trasladado 12% sobre la venta'),
(2, 170, 1, 18000.0000,     0.0000, 'Costo de Ventas - Línea Res'),
(2, 171, 2, 12000.0000,     0.0000, 'Costo de Ventas - Línea Cerdo'),
(2,  26, 3,     0.0000, 30000.0000, 'Salida de Inventario de Productos Terminados'),
(3,  24, 1, 40000.0000,     0.0000, 'Inventario de Materias Primas - carne y especias'),
(3,  29, 2,  4800.0000,     0.0000, 'IVA Acreditable 12% sobre la compra'),
(3,  83, 3,     0.0000, 44800.0000, 'Proveedores Nacionales - factura B-7781'),
(4, 207, 1, 25000.0000,     0.0000, 'Sueldos Normales - personal administrativo'),
(4, 210, 2,  3167.5000,     0.0000, 'Cuota patronal IGSS 12.67%'),
(4,  92, 3,     0.0000,  4375.0000, 'Cuotas Obrero-Patronales IGSS por pagar'),
(4,  86, 4,     0.0000, 23792.5000, 'Sueldos por Pagar (Personal Administrativo)'),
(5,  83, 1, 44800.0000,     0.0000, 'Cancelación de factura B-7781'),
(5,   8, 2,     0.0000, 44800.0000, 'Banco Industrial (MN) - cheque 004521'),
(6, 176, 1,  4166.6667,     0.0000, 'Depreciación de Maquinaria de Producción'),
(6, 222, 2,   833.3333,     0.0000, 'Depreciación de Equipo Electrónico de Cómputo'),
(6,  51, 3,     0.0000,  4166.6667, 'Depreciación Acumulada de Maquinaria y Equipo'),
(6,  54, 4,     0.0000,   833.3333, 'Depreciación Acumulada de Equipo de Cómputo'),
(7, 201, 1,  7500.0000,     0.0000, 'Propaganda Impresa - campaña noviembre'),
(7,  29, 2,   900.0000,     0.0000, 'IVA Acreditable 12%'),
(7, 103, 3,     0.0000,  8400.0000, 'Acreedores Diversos - agencia publicitaria'),
(8,   5, 1,  1120.0000,     0.0000, 'Caja General - recibo 0098 (anulado)'),
(8, 136, 2,     0.0000,  1000.0000, 'Ventas de Embutidos - Línea Pollo (anulado)'),
(8,  91, 3,     0.0000,   120.0000, 'IVA Trasladado (anulado)'),
(9,   8, 1, 25000.0000,     0.0000, 'Ingreso bancario doc. BOL-2026-9912'),
(9,  16, 2,     0.0000, 25000.0000, 'Contrapartida doc. BOL-2026-9912'),
(10, 228, 1,   350.0000,     0.0000, 'Contrapartida doc. NDB-2026-0451'),
(10,   8, 2,     0.0000,   350.0000, 'Egreso bancario doc. NDB-2026-0451');

-- =============================================================================
--  4. SALDOS DEL PERÍODO 2026-10
-- =============================================================================

INSERT INTO tblSaldoCuentaPeriodo
    (idCuenta, idActualizacionSaldos, periodoContableSaldoCuentaPeriodo, saldoInicialSaldoCuentaPeriodo,
     cargoSaldoCuentaPeriodo, abonoSaldoCuentaPeriodo, saldoFinalSaldoCuentaPeriodo, estadoSaldoCuentaPeriodo)
WITH RECURSIVE movimiento AS (
    SELECT pd.idCuenta AS idCuenta, SUM(pd.debePolizaDetalle) AS debe, SUM(pd.haberPolizaDetalle) AS haber
      FROM tblPolizaDetalle pd
      JOIN tblPolizaEncabezado pe ON pe.idPolizaEncabezado = pd.idPolizaEncabezado
     WHERE pe.estadoPolizaEncabezado = 'APLICADA' AND pe.periodoContablePolizaEncabezado = '2026-10'
       AND pe.is_active = TRUE AND pd.is_active = TRUE
     GROUP BY pd.idCuenta
), ascenso AS (
    SELECT idCuenta, debe, haber FROM movimiento
    UNION ALL
    SELECT c.idCuentaPadre, a.debe, a.haber
      FROM ascenso a
      JOIN tblCuenta c ON c.idCuenta = a.idCuenta
     WHERE c.idCuentaPadre IS NOT NULL
)
SELECT c.idCuenta, 1, '2026-10', 0.0000, SUM(a.debe), SUM(a.haber),
       CASE WHEN c.tipoCuenta = 'DEUDORA' THEN SUM(a.debe) - SUM(a.haber)
            ELSE SUM(a.haber) - SUM(a.debe) END,
       'ABIERTO'
  FROM ascenso a
  JOIN tblCuenta c ON c.idCuenta = a.idCuenta
 WHERE c.codigoCuenta <> '0'
 GROUP BY c.idCuenta, c.tipoCuenta;

-- Saldos vigentes en el catálogo de cuentas (período 2026-10)
UPDATE tblCuenta c
  JOIN tblSaldoCuentaPeriodo s ON s.idCuenta = c.idCuenta AND s.periodoContableSaldoCuentaPeriodo = '2026-10'
   SET c.saldoInicialCuenta = s.saldoInicialSaldoCuentaPeriodo,
       c.cargoPeriodoCuenta = s.cargoSaldoCuentaPeriodo,
       c.abonoPeriodoCuenta = s.abonoSaldoCuentaPeriodo,
       c.saldoActualCuenta  = s.saldoFinalSaldoCuentaPeriodo,
       -- Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
       c.cargoAcumuladoCuenta = (SELECT SUM(a.cargoSaldoCuentaPeriodo) FROM tblSaldoCuentaPeriodo a
                                  WHERE a.idCuenta = c.idCuenta AND a.periodoContableSaldoCuentaPeriodo BETWEEN '2026-01' AND '2026-10'),
       c.abonoAcumuladoCuenta = (SELECT SUM(a.abonoSaldoCuentaPeriodo) FROM tblSaldoCuentaPeriodo a
                                  WHERE a.idCuenta = c.idCuenta AND a.periodoContableSaldoCuentaPeriodo BETWEEN '2026-01' AND '2026-10'),
       -- Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
       c.periodoSaldoCuenta = '2026-10';
       -- registro de bancos
       
-- INSERTS DE PRUEBA - MODULO BANCOS (5 registros por tabla)
-- Base de datos: dbSistemaEmbutidos
-- Ejecutar TODO el archivo de una vez (rayito). Ya viene en el orden correcto.
-- ============================================================
USE dbSistemaEmbutidos;

-- ------------------------------------------------------------
-- 1. tblMoneda
-- ------------------------------------------------------------
INSERT IGNORE INTO tblMoneda (idMoneda, codigoMoneda, nombreMoneda, simboloMoneda) VALUES
    (1, 'GTQ', 'Quetzal', 'Q'),
    (2, 'USD', 'Dolar estadounidense', 'US$'),
    (3, 'EUR', 'Euro', 'EUR'),
    (4, 'MXN', 'Peso mexicano', 'MX$'),
    (5, 'HNL', 'Lempira hondureno', 'L');


-- ------------------------------------------------------------
-- 2. tblTipoCambio
-- ------------------------------------------------------------
INSERT INTO tblTipoCambio
    (fechaTipoCambio, valorTipoCambio, fuenteTipoCambio, versionTipoCambio, metodoTipoCambio, Fk_Moneda_Origen_Codigo, Fk_Moneda_Destino_Codigo) VALUES
    ('2026-11-04', 7.7498, 'Banguat', 1, 'DIRECTA', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('2026-11-05', 7.7515, 'Banguat', 1, 'DIRECTA', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('2026-11-06', 7.7532, 'Banguat', 1, 'DIRECTA', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('2026-11-06', 8.9840, 'Banguat', 1, 'DIRECTA', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'EUR'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('2026-11-06', 0.4215, 'Banguat', 1, 'DIRECTA', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'MXN'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'));


-- ------------------------------------------------------------
-- 3. tblBanco
-- ------------------------------------------------------------
INSERT IGNORE INTO tblBanco (codigoBanco, nombreBanco) VALUES
    ('BIND', 'Banco Industrial'),
    ('BRUR', 'Banrural'),
    ('BAC', 'BAC Credomatic'),
    ('GYT', 'Banco G&T Continental'),
    ('BAM', 'Banco Agromercantil');


-- ------------------------------------------------------------
-- 4. tblTipoCuentaBancaria
-- ------------------------------------------------------------
INSERT IGNORE INTO tblTipoCuentaBancaria (codigoTipoCuentaBancaria, nombreTipoCuentaBancaria) VALUES
    ('MONETARIA', 'Cuenta monetaria'),
    ('AHORRO', 'Cuenta de ahorro'),
    ('NOMINA', 'Cuenta de nomina'),
    ('PLAZO_FIJO', 'Deposito a plazo fijo'),
    ('INVERSION', 'Cuenta de inversion');


-- ------------------------------------------------------------
-- 5. tblTipoTransaccion
-- ------------------------------------------------------------
INSERT INTO tblTipoTransaccion (codigoTipoTransaccion, nombreTipoTransaccion, naturalezaTipoTransaccion) VALUES
    ('TRANS-101', 'Pago a proveedores por cheque', 'EGRESO'),
    ('TRANS-102', 'Pago de planilla por cheque', 'EGRESO'),
    ('TRANS-103', 'Deposito de cliente', 'INGRESO'),
    ('TRANS-104', 'Comision bancaria', 'EGRESO'),
    ('TRANS-105', 'Transferencia entre cuentas propias', 'AMBOS');


-- ------------------------------------------------------------
-- 6. tblCuentaBancaria
-- ------------------------------------------------------------
INSERT INTO tblCuentaBancaria
    (numeroCuentaBancaria, nombreCuentaBancaria, codigoCuentaContableCuentaBancaria, limiteSobregiroCuentaBancaria,
     Fk_Banco_Codigo, Fk_Moneda_Codigo, Fk_TipoCuentaBancaria_Codigo) VALUES
    ('000-215487-9', 'Cuenta operativa Industrial GTQ', '1.01.02.001', 50000.0000,
        (SELECT idBanco FROM tblBanco WHERE codigoBanco = 'BIND'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'),
        (SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria WHERE codigoTipoCuentaBancaria = 'MONETARIA')),
    ('3011-045678-2', 'Cuenta de proveedores Banrural GTQ', '1.01.02.002', 25000.0000,
        (SELECT idBanco FROM tblBanco WHERE codigoBanco = 'BRUR'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'),
        (SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria WHERE codigoTipoCuentaBancaria = 'MONETARIA')),
    ('90-4421-3', 'Cuenta operativa BAC USD', '1.01.02.003', 0.0000,
        (SELECT idBanco FROM tblBanco WHERE codigoBanco = 'BAC'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD'),
        (SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria WHERE codigoTipoCuentaBancaria = 'MONETARIA')),
    ('450-0123456-1', 'Cuenta de nomina G&T GTQ', '1.01.02.004', 0.0000,
        (SELECT idBanco FROM tblBanco WHERE codigoBanco = 'GYT'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'),
        (SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria WHERE codigoTipoCuentaBancaria = 'NOMINA')),
    ('7701-889012-6', 'Cuenta de ahorro reserva Agromercantil GTQ', '1.01.03.001', 0.0000,
        (SELECT idBanco FROM tblBanco WHERE codigoBanco = 'BAM'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'),
        (SELECT idTipoCuentaBancaria FROM tblTipoCuentaBancaria WHERE codigoTipoCuentaBancaria = 'AHORRO'));


-- ------------------------------------------------------------
-- 7. tblDisponibilidadDiaria
-- ------------------------------------------------------------
INSERT INTO tblDisponibilidadDiaria
    (fechaDisponibilidadDiaria, saldoInicialDisponibilidadDiaria, ingresosDisponibilidadDiaria, egresosDisponibilidadDiaria,
     comprometidoDisponibilidadDiaria, restringidoDisponibilidadDiaria, saldoDisponibleDisponibilidadDiaria,
     saldoDisponibleGTQDisponibilidadDiaria, tipoCambioAplicadoDisponibilidadDiaria, fuenteDisponibilidadDiaria,
     Fk_CuentaBancaria_Codigo, Fk_TipoCambio_Codigo) VALUES
    ('2026-11-06', 126550.0000, 0, 0, 41300.0000, 0, 85250.0000, 85250.0000, 1.0000, 'Cierre bancario diario',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'), NULL),
    ('2026-11-06', 64379.5000, 0, 0, 0, 0, 64379.5000, 64379.5000, 1.0000, 'Cierre bancario diario',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2'), NULL),
    ('2026-11-06', 18000.0000, 0, 0, 3200.0000, 0, 14800.0000, 114747.3600, 7.7532, 'Cierre bancario diario',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '90-4421-3'),
        (SELECT t.idTipoCambio FROM tblTipoCambio t JOIN tblMoneda mo ON mo.idMoneda = t.Fk_Moneda_Origen_Codigo
          WHERE mo.codigoMoneda = 'USD' AND t.fechaTipoCambio = '2026-11-06' AND t.fuenteTipoCambio = 'Banguat' AND t.versionTipoCambio = 1)),
    ('2026-11-06', 80750.0000, 0, 0, 0, 0, 80750.0000, 80750.0000, 1.0000, 'Cierre bancario diario',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '450-0123456-1'), NULL),
    ('2026-11-06', 250000.0000, 0, 0, 0, 0, 250000.0000, 250000.0000, 1.0000, 'Cierre bancario diario',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '7701-889012-6'), NULL);


-- ------------------------------------------------------------
-- 8. tblPoliticaAprobacion
-- ------------------------------------------------------------
INSERT INTO tblPoliticaAprobacion
    (nombrePoliticaAprobacion, tipoOperacionPoliticaAprobacion, montoMinimoPoliticaAprobacion, montoMaximoPoliticaAprobacion,
     cantidadAprobacionesPoliticaAprobacion, fechaInicioPoliticaAprobacion, Fk_Moneda_Codigo) VALUES
    ('Orden de pago en quetzales hasta Q50,000', 'ORDEN_PAGO', 0.0000, 50000.0000, 1, '2026-01-01', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('Orden de pago en quetzales mayor a Q50,000', 'ORDEN_PAGO', 50000.0100, 1000000.0000, 2, '2026-01-01', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('Orden de pago en dolares hasta US$6,500', 'ORDEN_PAGO', 0.0000, 6500.0000, 1, '2026-01-01', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD')),
    ('Orden de pago en dolares mayor a US$6,500', 'ORDEN_PAGO', 6500.0100, 200000.0000, 2, '2026-01-01', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD')),
    ('Orden de compra en quetzales hasta Q25,000', 'ORDEN_COMPRA', 0.0000, 25000.0000, 1, '2026-01-01', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'));


-- ------------------------------------------------------------
-- 9. tblConceptoBancario
-- ------------------------------------------------------------
INSERT INTO tblConceptoBancario
    (codigoConceptoBancario, nombreConceptoBancario, descripcionConceptoBancario, naturalezaConceptoBancario, claseOperacionConceptoBancario, tipoTerceroConceptoBancario) VALUES
    ('CB-001', 'Pago a proveedores', 'Egreso por pago de facturas u ordenes de compra de proveedores', 'EGRESO', 'NORMAL', 'PROVEEDOR'),
    ('CB-002', 'Pago de planilla', 'Egreso por pago de sueldos a empleados', 'EGRESO', 'NORMAL', 'EMPLEADO'),
    ('CB-003', 'Cobro a clientes', 'Ingreso por deposito o transferencia de un cliente', 'INGRESO', 'NORMAL', 'CLIENTE'),
    ('CB-004', 'Transferencia entre cuentas propias', 'Movimiento entre dos cuentas de la empresa', 'AMBOS', 'TRANSFERENCIA_INTERNA', 'NINGUNO'),
    ('CB-005', 'Comision bancaria', 'Cargo del banco por manejo de cuenta o servicios', 'EGRESO', 'NORMAL', 'NINGUNO');


-- ------------------------------------------------------------
-- 10. tblOrdenCompraRecibida
-- ------------------------------------------------------------
INSERT INTO tblOrdenCompraRecibida
    (referenciaExternaOrdenCompraRecibida, proveedorOrdenCompraRecibida, fechaOrdenCompraRecibida, fechaVencimientoOrdenCompraRecibida,
     importeOrdenCompraRecibida, estadoOrdenCompraRecibida, referenciaDocumentoOrdenCompraRecibida, identificadorSolicitanteOrdenCompraRecibida, Fk_Moneda_Codigo) VALUES
    ('OC-2026-0201', 'Distribuidora de Carnes La Esperanza, S.A.', '2026-10-15', '2026-11-03', 18450.0000, 'PAGADA', 'compras/oc/OC-2026-0201.pdf', 'compras.jlopez', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('OC-2026-0202', 'Empaques y Etiquetas de Guatemala, S.A.', '2026-10-20', '2026-11-04', 7920.5000, 'PAGADA', 'compras/oc/OC-2026-0202.pdf', 'compras.jlopez', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('OC-2026-0203', 'Importadora de Maquinaria Alimenticia, S.A.', '2026-10-28', '2026-11-10', 3200.0000, 'APROBADA', 'compras/oc/OC-2026-0203.pdf', 'compras.mrivera', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD')),
    ('OC-2026-0204', 'Especias y Condimentos del Pacifico, S.A.', '2026-10-30', '2026-11-09', 41300.0000, 'APROBADA', 'compras/oc/OC-2026-0204.pdf', 'compras.mrivera', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('OC-2026-0205', 'Transportes y Fletes Rapidos, S.A.', '2026-11-03', '2026-11-20', 12875.0000, 'PENDIENTE', 'compras/oc/OC-2026-0205.pdf', 'compras.jlopez', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'));


-- ------------------------------------------------------------
-- 11. tblPlanillaRecibida
-- ------------------------------------------------------------
INSERT INTO tblPlanillaRecibida
    (referenciaExternaPlanillaRecibida, inicioPeriodoPlanillaRecibida, finPeriodoPlanillaRecibida, estadoPlanillaRecibida, referenciaDocumentoPlanillaRecibida, Fk_Moneda_Codigo) VALUES
    ('PL-2026-08-Q2', '2026-08-16', '2026-08-31', 'PROCESADA', 'rrhh/planillas/PL-2026-08-Q2.pdf', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('PL-2026-09-Q1', '2026-09-01', '2026-09-15', 'PROCESADA', 'rrhh/planillas/PL-2026-09-Q1.pdf', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('PL-2026-09-Q2', '2026-09-16', '2026-09-30', 'PROCESADA', 'rrhh/planillas/PL-2026-09-Q2.pdf', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('PL-2026-10-Q1', '2026-10-01', '2026-10-15', 'PROCESADA', 'rrhh/planillas/PL-2026-10-Q1.pdf', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('PL-2026-10-Q2', '2026-10-16', '2026-10-31', 'RECIBIDA', 'rrhh/planillas/PL-2026-10-Q2.pdf', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'));


-- ------------------------------------------------------------
-- 12. tblDetallePlanillaRecibida
-- ------------------------------------------------------------
INSERT INTO tblDetallePlanillaRecibida
    (referenciaEmpleadoDetallePlanillaRecibida, beneficiarioDetallePlanillaRecibida, importeNetoDetallePlanillaRecibida, Fk_PlanillaRecibida_Codigo) VALUES
    ('EMP-0101', 'Carlos Mendez Ruiz', 4250.0000, (SELECT Pk_PlanillaRecibida FROM tblPlanillaRecibida WHERE referenciaExternaPlanillaRecibida = 'PL-2026-10-Q2')),
    ('EMP-0102', 'Maria Fernanda Lopez Barrios', 5120.0000, (SELECT Pk_PlanillaRecibida FROM tblPlanillaRecibida WHERE referenciaExternaPlanillaRecibida = 'PL-2026-10-Q2')),
    ('EMP-0103', 'Jose Luis Ajanel Tzoc', 3980.5000, (SELECT Pk_PlanillaRecibida FROM tblPlanillaRecibida WHERE referenciaExternaPlanillaRecibida = 'PL-2026-10-Q2')),
    ('EMP-0104', 'Ana Lucia Castillo Perez', 4675.0000, (SELECT Pk_PlanillaRecibida FROM tblPlanillaRecibida WHERE referenciaExternaPlanillaRecibida = 'PL-2026-10-Q2')),
    ('EMP-0105', 'Roberto Giron Pineda', 6010.0000, (SELECT Pk_PlanillaRecibida FROM tblPlanillaRecibida WHERE referenciaExternaPlanillaRecibida = 'PL-2026-10-Q2'));


-- ------------------------------------------------------------
-- 13. tblOrdenPago
-- ------------------------------------------------------------
INSERT INTO tblOrdenPago
    (referenciaOrdenPago, beneficiarioOrdenPago, importeOrdenPago, medioOrdenPago, estadoOrdenPago, fechaProgramadaOrdenPago,
     identificadorSolicitanteOrdenPago, Fk_CuentaBancaria_Codigo, Fk_Moneda_Codigo, Fk_OrdenCompraRecibida_Codigo, Fk_DetallePlanillaRecibida_Codigo) VALUES
    ('OP-2026-0201', 'Distribuidora de Carnes La Esperanza, S.A.', 18450.0000, 'CHEQUE', 'PAGADA', '2026-11-03', 'tesoreria.ana',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'),
        (SELECT Pk_OrdenCompraRecibida FROM tblOrdenCompraRecibida WHERE referenciaExternaOrdenCompraRecibida = 'OC-2026-0201'), NULL),
    ('OP-2026-0202', 'Empaques y Etiquetas de Guatemala, S.A.', 7920.5000, 'CHEQUE', 'PAGADA', '2026-11-04', 'tesoreria.ana',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'),
        (SELECT Pk_OrdenCompraRecibida FROM tblOrdenCompraRecibida WHERE referenciaExternaOrdenCompraRecibida = 'OC-2026-0202'), NULL),
    ('OP-2026-0203', 'Carlos Mendez Ruiz', 4250.0000, 'CHEQUE', 'PAGADA', '2026-11-05', 'tesoreria.ana',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '450-0123456-1'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'),
        NULL, (SELECT d.idDetallePlanillaRecibida FROM tblDetallePlanillaRecibida d JOIN tblPlanillaRecibida p ON p.Pk_PlanillaRecibida = d.Fk_PlanillaRecibida_Codigo
                WHERE p.referenciaExternaPlanillaRecibida = 'PL-2026-10-Q2' AND d.referenciaEmpleadoDetallePlanillaRecibida = 'EMP-0101')),
    ('OP-2026-0204', 'Importadora de Maquinaria Alimenticia, S.A.', 3200.0000, 'CHEQUE', 'EMITIDA', '2026-11-06', 'tesoreria.luis',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '90-4421-3'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD'),
        (SELECT Pk_OrdenCompraRecibida FROM tblOrdenCompraRecibida WHERE referenciaExternaOrdenCompraRecibida = 'OC-2026-0203'), NULL),
    ('OP-2026-0205', 'Especias y Condimentos del Pacifico, S.A.', 41300.0000, 'CHEQUE', 'EMITIDA', '2026-11-06', 'tesoreria.luis',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'),
        (SELECT Pk_OrdenCompraRecibida FROM tblOrdenCompraRecibida WHERE referenciaExternaOrdenCompraRecibida = 'OC-2026-0204'), NULL);


-- ------------------------------------------------------------
-- 14. tblAprobacion
-- ------------------------------------------------------------
INSERT INTO tblAprobacion
    (decisionAprobacion, comentarioAprobacion, identificadorAprobadorAprobacion, referenciaEvidencia2FAAprobacion, fechaValidacion2FAAprobacion,
     Fk_OrdenCompraRecibida_Codigo, Fk_OrdenPago_Codigo, Fk_PoliticaAprobacion_Codigo) VALUES
    ('APROBADA', 'Factura verificada contra la orden de compra', 'gerencia.carlos', '2FA-20261103-0001', '2026-11-03 09:12:00', NULL,
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0201'),
        (SELECT Pk_PoliticaAprobacion FROM tblPoliticaAprobacion WHERE nombrePoliticaAprobacion = 'Orden de pago en quetzales hasta Q50,000')),
    ('APROBADA', 'Pago dentro del presupuesto de empaques', 'gerencia.carlos', '2FA-20261104-0001', '2026-11-04 10:05:00', NULL,
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0202'),
        (SELECT Pk_PoliticaAprobacion FROM tblPoliticaAprobacion WHERE nombrePoliticaAprobacion = 'Orden de pago en quetzales hasta Q50,000')),
    ('APROBADA', 'Planilla revisada por Recursos Humanos', 'gerencia.sofia', '2FA-20261105-0001', '2026-11-05 08:40:00', NULL,
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0203'),
        (SELECT Pk_PoliticaAprobacion FROM tblPoliticaAprobacion WHERE nombrePoliticaAprobacion = 'Orden de pago en quetzales hasta Q50,000')),
    ('APROBADA', 'Importacion de maquinaria autorizada por gerencia', 'gerencia.sofia', '2FA-20261106-0001', '2026-11-06 08:20:00', NULL,
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0204'),
        (SELECT Pk_PoliticaAprobacion FROM tblPoliticaAprobacion WHERE nombrePoliticaAprobacion = 'Orden de pago en dolares hasta US$6,500')),
    ('APROBADA', 'Compra de especias para la produccion de noviembre', 'gerencia.carlos', '2FA-20261106-0002', '2026-11-06 08:55:00', NULL,
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0205'),
        (SELECT Pk_PoliticaAprobacion FROM tblPoliticaAprobacion WHERE nombrePoliticaAprobacion = 'Orden de pago en quetzales hasta Q50,000'));


-- ------------------------------------------------------------
-- 15. tblChequera
-- ------------------------------------------------------------
INSERT INTO tblChequera
    (serieChequera, numeroInicialChequera, numeroFinalChequera, siguienteNumeroChequera, Fk_CuentaBancaria_Codigo) VALUES
    ('BIND-A', 1001, 1100, 1003, (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9')),
    ('BRUR-A', 5001, 5050, 5002, (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2')),
    ('BAC-A', 3001, 3025, 3002, (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '90-4421-3')),
    ('GYT-A', 8001, 8050, 8002, (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '450-0123456-1')),
    ('BAM-A', 9001, 9025, 9001, (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '7701-889012-6'));


-- ------------------------------------------------------------
-- 16. tblCheque
-- ------------------------------------------------------------
INSERT INTO tblCheque
    (numeroCheque, beneficiarioCheque, estadoCheque, referenciaEvidencia2FACheque, fechaValidacion2FACheque, identificadorGeneradorCheque, Fk_Chequera_Codigo, Fk_OrdenPago_Codigo) VALUES
    (1001, 'Distribuidora de Carnes La Esperanza, S.A.', 'COBRADO', '2FA-20261103-0002', '2026-11-03 11:30:00', 'tesoreria.ana',
        (SELECT c.idChequera FROM tblChequera c JOIN tblCuentaBancaria b ON b.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo WHERE b.numeroCuentaBancaria = '000-215487-9' AND c.serieChequera = 'BIND-A'),
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0201')),
    (5001, 'Empaques y Etiquetas de Guatemala, S.A.', 'COBRADO', '2FA-20261104-0002', '2026-11-04 11:15:00', 'tesoreria.ana',
        (SELECT c.idChequera FROM tblChequera c JOIN tblCuentaBancaria b ON b.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo WHERE b.numeroCuentaBancaria = '3011-045678-2' AND c.serieChequera = 'BRUR-A'),
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0202')),
    (8001, 'Carlos Mendez Ruiz', 'ENTREGADO', '2FA-20261105-0002', '2026-11-05 09:50:00', 'tesoreria.ana',
        (SELECT c.idChequera FROM tblChequera c JOIN tblCuentaBancaria b ON b.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo WHERE b.numeroCuentaBancaria = '450-0123456-1' AND c.serieChequera = 'GYT-A'),
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0203')),
    (3001, 'Importadora de Maquinaria Alimenticia, S.A.', 'GENERADO', '2FA-20261106-0003', '2026-11-06 09:10:00', 'tesoreria.luis',
        (SELECT c.idChequera FROM tblChequera c JOIN tblCuentaBancaria b ON b.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo WHERE b.numeroCuentaBancaria = '90-4421-3' AND c.serieChequera = 'BAC-A'),
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0204')),
    (1002, 'Especias y Condimentos del Pacifico, S.A.', 'GENERADO', '2FA-20261106-0004', '2026-11-06 09:25:00', 'tesoreria.luis',
        (SELECT c.idChequera FROM tblChequera c JOIN tblCuentaBancaria b ON b.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo WHERE b.numeroCuentaBancaria = '000-215487-9' AND c.serieChequera = 'BIND-A'),
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0205'));


-- ------------------------------------------------------------
-- 17. tblMovimientoBancario
-- ------------------------------------------------------------
INSERT INTO tblMovimientoBancario
    (referenciaMovimientoBancario, conceptoMovimientoBancario, importeMovimientoBancario, importeGTQMovimientoBancario, tipoCambioAplicadoMovimientoBancario,
     sentidoMovimientoBancario, fechaOperacionMovimientoBancario, fechaValorMovimientoBancario, origenMovimientoBancario,
     Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo, Fk_TipoCambio_Codigo, Fk_OrdenPago_Codigo, idConceptoBancario, idMetodoPago) VALUES
    ('MB-2026-0001', 'Pago OP-2026-0201 a Distribuidora de Carnes La Esperanza', 18450.0000, 18450.0000, 1.0000, -1, '2026-11-03', '2026-11-03', 'ORDEN_PAGO',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'),
        (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TRANS-101'), NULL,
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0201'),
        (SELECT idConceptoBancario FROM tblConceptoBancario WHERE codigoConceptoBancario = 'CB-001'),
        (SELECT MIN(idMetodoPago) FROM tblMetodoPago WHERE nombreMetodoPago LIKE '%cheque%')),
    ('MB-2026-0002', 'Pago OP-2026-0202 a Empaques y Etiquetas de Guatemala', 7920.5000, 7920.5000, 1.0000, -1, '2026-11-04', '2026-11-04', 'ORDEN_PAGO',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2'),
        (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TRANS-101'), NULL,
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0202'),
        (SELECT idConceptoBancario FROM tblConceptoBancario WHERE codigoConceptoBancario = 'CB-001'),
        (SELECT MIN(idMetodoPago) FROM tblMetodoPago WHERE nombreMetodoPago LIKE '%cheque%')),
    ('MB-2026-0003', 'Pago OP-2026-0203 de planilla a Carlos Mendez Ruiz', 4250.0000, 4250.0000, 1.0000, -1, '2026-11-05', '2026-11-05', 'ORDEN_PAGO',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '450-0123456-1'),
        (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TRANS-102'), NULL,
        (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0203'),
        (SELECT idConceptoBancario FROM tblConceptoBancario WHERE codigoConceptoBancario = 'CB-002'),
        (SELECT MIN(idMetodoPago) FROM tblMetodoPago WHERE nombreMetodoPago LIKE '%cheque%')),
    ('MB-2026-0004', 'Deposito en efectivo de cliente Supermercados El Ahorro', 25000.0000, 25000.0000, 1.0000, 1, '2026-11-02', '2026-11-02', 'MANUAL',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'),
        (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TRANS-103'), NULL, NULL,
        (SELECT idConceptoBancario FROM tblConceptoBancario WHERE codigoConceptoBancario = 'CB-003'), NULL),
    ('MB-2026-0005', 'Credito por transferencia ACH de cliente Restaurantes Gran Sabor', 12300.0000, 12300.0000, 1.0000, 1, '2026-11-04', '2026-11-04', 'ESTADO_CUENTA',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2'),
        (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TRANS-103'), NULL, NULL,
        (SELECT idConceptoBancario FROM tblConceptoBancario WHERE codigoConceptoBancario = 'CB-003'), NULL);


-- ------------------------------------------------------------
-- 18. tblTransferenciaInterna
-- ------------------------------------------------------------
INSERT INTO tblTransferenciaInterna (is_active) VALUES (1), (1), (1), (1), (1);


-- ------------------------------------------------------------
-- 19. tblMovimientoBancarioTercero
-- ------------------------------------------------------------
-- Requisito: 2 proveedores y 2 clientes de prueba (tablas de otros modulos). Si ya existen, no se duplican.
INSERT IGNORE INTO tblProveedor (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor) VALUES
    ('Distribuidora de Carnes La Esperanza, S.A.', '4521879-3', 'ventas@carnesesperanza.com.gt', '22345678', 'Zona 12, Ciudad de Guatemala'),
    ('Empaques y Etiquetas de Guatemala, S.A.', '6098123-5', 'pedidos@empaquesgt.com.gt', '23456789', 'Zona 4 de Mixco, Guatemala');

INSERT IGNORE INTO tblCliente (codigoCliente, tipoIdentificacionCliente, nombreCliente, nitCliente, idMonedaCredito) VALUES
    ('CLI-BNK-001', 'NIT', 'Supermercados El Ahorro, S.A.', '7345612-1', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ')),
    ('CLI-BNK-002', 'NIT', 'Restaurantes Gran Sabor, S.A.', '8123456-7', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'));

INSERT INTO tblMovimientoBancarioTercero (idMovimientoBancario, idEmpleado, idProveedor, idCliente) VALUES
    ((SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0001'), NULL,
        (SELECT idProveedor FROM tblProveedor WHERE nitProveedor = '4521879-3'), NULL),
    ((SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0002'), NULL,
        (SELECT idProveedor FROM tblProveedor WHERE nitProveedor = '6098123-5'), NULL),
    ((SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0003'),
        (SELECT idEmpleado FROM (SELECT idEmpleado FROM tblEmpleado ORDER BY idEmpleado LIMIT 1) e), NULL, NULL),
    ((SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0004'), NULL, NULL,
        (SELECT idCliente FROM tblCliente WHERE nitCliente = '7345612-1')),
    ((SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0005'), NULL, NULL,
        (SELECT idCliente FROM tblCliente WHERE nitCliente = '8123456-7'));


-- ------------------------------------------------------------
-- 20. tblExtractoBancario
-- ------------------------------------------------------------
INSERT INTO tblExtractoBancario
    (referenciaExtractoBancario, fechaInicioExtractoBancario, fechaFinExtractoBancario, saldoInicialExtractoBancario, saldoFinalExtractoBancario, huellaExtractoBancario, Fk_CuentaBancaria_Codigo) VALUES
    ('EXT-BIND-2026-11', '2026-11-01', '2026-11-06', 120000.0000, 126515.0000, SHA2('EXT-BIND-2026-11', 256), (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9')),
    ('EXT-BRUR-2026-11', '2026-11-01', '2026-11-06', 60000.0000, 64379.5000, SHA2('EXT-BRUR-2026-11', 256), (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2')),
    ('EXT-BAC-2026-11', '2026-11-01', '2026-11-06', 18000.0000, 18000.0000, SHA2('EXT-BAC-2026-11', 256), (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '90-4421-3')),
    ('EXT-GYT-2026-11', '2026-11-01', '2026-11-06', 85000.0000, 85000.0000, SHA2('EXT-GYT-2026-11', 256), (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '450-0123456-1')),
    ('EXT-BAM-2026-11', '2026-11-01', '2026-11-06', 250000.0000, 250000.0000, SHA2('EXT-BAM-2026-11', 256), (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '7701-889012-6'));


-- ------------------------------------------------------------
-- 21. tblDetalleExtracto
-- ------------------------------------------------------------
INSERT INTO tblDetalleExtracto
    (identificadorDetalleExtracto, referenciaDetalleExtracto, conceptoDetalleExtracto, fechaOperacionDetalleExtracto, fechaValorDetalleExtracto,
     importeDetalleExtracto, sentidoDetalleExtracto, Fk_ExtractoBancario_Codigo) VALUES
    ('BIND-20261102-001', 'DEP-88341', 'Deposito en efectivo', '2026-11-02', '2026-11-02', 25000.0000, 1,
        (SELECT Pk_ExtractoBancario FROM tblExtractoBancario WHERE referenciaExtractoBancario = 'EXT-BIND-2026-11')),
    ('BIND-20261105-002', 'CHQ-1001', 'Cheque pagado numero 1001', '2026-11-05', '2026-11-05', 18450.0000, -1,
        (SELECT Pk_ExtractoBancario FROM tblExtractoBancario WHERE referenciaExtractoBancario = 'EXT-BIND-2026-11')),
    ('BIND-20261106-003', 'COM-1106', 'Comision por manejo de cuenta', '2026-11-06', '2026-11-06', 35.0000, -1,
        (SELECT Pk_ExtractoBancario FROM tblExtractoBancario WHERE referenciaExtractoBancario = 'EXT-BIND-2026-11')),
    ('BRUR-20261104-001', 'DEP-20417', 'Credito por transferencia ACH', '2026-11-04', '2026-11-04', 12300.0000, 1,
        (SELECT Pk_ExtractoBancario FROM tblExtractoBancario WHERE referenciaExtractoBancario = 'EXT-BRUR-2026-11')),
    ('BRUR-20261105-002', 'CHQ-5001', 'Cheque pagado numero 5001', '2026-11-05', '2026-11-05', 7920.5000, -1,
        (SELECT Pk_ExtractoBancario FROM tblExtractoBancario WHERE referenciaExtractoBancario = 'EXT-BRUR-2026-11'));


-- ------------------------------------------------------------
-- 22. tblConciliacion
-- ------------------------------------------------------------
INSERT INTO tblConciliacion
    (fechaCorteConciliacion, estadoConciliacion, metodoConciliacion, saldoLibroConciliacion, saldoBancoConciliacion, diferenciaConciliacion,
     identificadorResponsableConciliacion, fechaCierreConciliacion, Fk_CuentaBancaria_Codigo, Fk_ExtractoBancario_Codigo) VALUES
    ('2026-11-06', 'CON_DIFERENCIAS', 'MIXTA', 126550.0000, 126515.0000, 35.0000, 'tesoreria.ana', NULL,
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'),
        (SELECT Pk_ExtractoBancario FROM tblExtractoBancario WHERE referenciaExtractoBancario = 'EXT-BIND-2026-11')),
    ('2026-11-06', 'CERRADA', 'AUTOMATICA', 64379.5000, 64379.5000, 0.0000, 'tesoreria.ana', '2026-11-06 17:30:00',
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2'),
        (SELECT Pk_ExtractoBancario FROM tblExtractoBancario WHERE referenciaExtractoBancario = 'EXT-BRUR-2026-11')),
    ('2026-11-06', 'CONCILIADA', 'AUTOMATICA', 18000.0000, 18000.0000, 0.0000, 'tesoreria.luis', NULL,
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '90-4421-3'),
        (SELECT Pk_ExtractoBancario FROM tblExtractoBancario WHERE referenciaExtractoBancario = 'EXT-BAC-2026-11')),
    ('2026-11-06', 'CON_DIFERENCIAS', 'MANUAL', 80750.0000, 85000.0000, -4250.0000, 'tesoreria.luis', NULL,
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '450-0123456-1'),
        (SELECT Pk_ExtractoBancario FROM tblExtractoBancario WHERE referenciaExtractoBancario = 'EXT-GYT-2026-11')),
    ('2026-11-06', 'ABIERTA', 'AUTOMATICA', NULL, NULL, NULL, NULL, NULL,
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '7701-889012-6'),
        (SELECT Pk_ExtractoBancario FROM tblExtractoBancario WHERE referenciaExtractoBancario = 'EXT-BAM-2026-11'));


-- ------------------------------------------------------------
-- 23. tblDetalleConciliacion
-- ------------------------------------------------------------
INSERT INTO tblDetalleConciliacion
    (resultadoDetalleConciliacion, metodoDetalleConciliacion, diferenciaDetalleConciliacion, motivoDetalleConciliacion,
     Fk_Conciliacion_Codigo, Fk_MovimientoBancario_Codigo, Fk_DetalleExtracto_Codigo) VALUES
    ('COINCIDE', 'AUTOMATICA', 0.0000, NULL,
        (SELECT c.idConciliacion FROM tblConciliacion c JOIN tblCuentaBancaria b ON b.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo WHERE b.numeroCuentaBancaria = '000-215487-9' AND c.fechaCorteConciliacion = '2026-11-06'),
        (SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0001'),
        (SELECT idDetalleExtracto FROM tblDetalleExtracto WHERE identificadorDetalleExtracto = 'BIND-20261105-002')),
    ('COINCIDE', 'AUTOMATICA', 0.0000, NULL,
        (SELECT c.idConciliacion FROM tblConciliacion c JOIN tblCuentaBancaria b ON b.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo WHERE b.numeroCuentaBancaria = '000-215487-9' AND c.fechaCorteConciliacion = '2026-11-06'),
        (SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0004'),
        (SELECT idDetalleExtracto FROM tblDetalleExtracto WHERE identificadorDetalleExtracto = 'BIND-20261102-001')),
    ('DIFERENCIA', 'MANUAL', 35.0000, 'Comision bancaria que aun no esta registrada en libros',
        (SELECT c.idConciliacion FROM tblConciliacion c JOIN tblCuentaBancaria b ON b.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo WHERE b.numeroCuentaBancaria = '000-215487-9' AND c.fechaCorteConciliacion = '2026-11-06'),
        NULL,
        (SELECT idDetalleExtracto FROM tblDetalleExtracto WHERE identificadorDetalleExtracto = 'BIND-20261106-003')),
    ('COINCIDE', 'AUTOMATICA', 0.0000, NULL,
        (SELECT c.idConciliacion FROM tblConciliacion c JOIN tblCuentaBancaria b ON b.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo WHERE b.numeroCuentaBancaria = '3011-045678-2' AND c.fechaCorteConciliacion = '2026-11-06'),
        (SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0002'),
        (SELECT idDetalleExtracto FROM tblDetalleExtracto WHERE identificadorDetalleExtracto = 'BRUR-20261105-002')),
    ('COINCIDE', 'AUTOMATICA', 0.0000, NULL,
        (SELECT c.idConciliacion FROM tblConciliacion c JOIN tblCuentaBancaria b ON b.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo WHERE b.numeroCuentaBancaria = '3011-045678-2' AND c.fechaCorteConciliacion = '2026-11-06'),
        (SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0005'),
        (SELECT idDetalleExtracto FROM tblDetalleExtracto WHERE identificadorDetalleExtracto = 'BRUR-20261104-001'));


-- ------------------------------------------------------------
-- 24. tblConfiguracionPoliza
-- ------------------------------------------------------------
INSERT INTO tblConfiguracionPoliza
    (codigoCuentaContableBancoConfiguracionPoliza, codigoCuentaContableContrapartidaConfiguracionPoliza, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo) VALUES
    ('1.01.02.001', '2.01.01.001', (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'), (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TRANS-101')),
    ('1.01.02.002', '2.01.01.001', (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2'), (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TRANS-101')),
    ('1.01.02.004', '2.01.02.001', (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '450-0123456-1'), (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TRANS-102')),
    ('1.01.02.001', '1.01.04.001', (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'), (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TRANS-103')),
    ('1.01.02.002', '1.01.04.001', (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2'), (SELECT idTipoTransaccion FROM tblTipoTransaccion WHERE codigoTipoTransaccion = 'TRANS-103'));


-- ------------------------------------------------------------
-- 25. tblPolizaBancaria
-- ------------------------------------------------------------
INSERT INTO tblPolizaBancaria
    (fechaPolizaBancaria, estadoPolizaBancaria, referenciaContabilidadPolizaBancaria, fechaEnvioContabilidadPolizaBancaria, Fk_MovimientoBancario_Codigo) VALUES
    ('2026-11-03', 'ENVIADA', 'POL-CONT-2026-1103-01', '2026-11-03 16:45:00', (SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0001')),
    ('2026-11-04', 'ENVIADA', 'POL-CONT-2026-1104-01', '2026-11-04 16:30:00', (SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0002')),
    ('2026-11-05', 'VALIDADA', NULL, NULL, (SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0003')),
    ('2026-11-02', 'ENVIADA', 'POL-CONT-2026-1102-01', '2026-11-02 16:10:00', (SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0004')),
    ('2026-11-04', 'BORRADOR', NULL, NULL, (SELECT idMovimientoBancario FROM tblMovimientoBancario WHERE referenciaMovimientoBancario = 'MB-2026-0005'));


-- ------------------------------------------------------------
-- 26. tblDetallePolizaBancaria
-- ------------------------------------------------------------
INSERT INTO tblDetallePolizaBancaria
    (codigoCuentaContableDetallePolizaBancaria, debeDetallePolizaBancaria, haberDetallePolizaBancaria, Fk_PolizaBancaria_Codigo) VALUES
    ('2.01.01.001', 18450.0000, 0.0000, (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0001')),
    ('1.01.02.001', 0.0000, 18450.0000, (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0001')),
    ('2.01.01.001', 7920.5000, 0.0000, (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0002')),
    ('1.01.02.002', 0.0000, 7920.5000, (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0002')),
    ('2.01.02.001', 4250.0000, 0.0000, (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0003')),
    ('1.01.02.004', 0.0000, 4250.0000, (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0003')),
    ('1.01.02.001', 25000.0000, 0.0000, (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0004')),
    ('1.01.04.001', 0.0000, 25000.0000, (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0004')),
    ('1.01.02.002', 12300.0000, 0.0000, (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0005')),
    ('1.01.04.001', 0.0000, 12300.0000, (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0005'));


-- ------------------------------------------------------------
-- 27. tblPartidaConsolidada
-- ------------------------------------------------------------
INSERT INTO tblPartidaConsolidada
    (identificadorEmpresaPartidaConsolidada, fechaInicioPartidaConsolidada, conceptoPartidaConsolidada, Fk_Moneda_Codigo,
     totalDebePartidaConsolidada, totalHaberPartidaConsolidada, estadoPartidaConsolidada, identificadorUsuarioGeneraPartidaConsolidada,
     fechaGeneracionPartidaConsolidada, identificadorUsuarioTrasladaPartidaConsolidada, fechaTrasladoPartidaConsolidada,
     claveTrasladoPartidaConsolidada, referenciaContabilidadPartidaConsolidada, mensajeResultadoTrasladoPartidaConsolidada) VALUES
    (1, '2026-08-01', 'Consolidado bancario de agosto 2026', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), 185400.0000, 185400.0000, 'TRASLADADA',
        'tesoreria.ana', '2026-09-01 08:00:00', 'contabilidad.jgomez', '2026-09-01 09:30:00', 'PCB-1-202608-GTQ', 'PART-CONT-2026-0831', 'Partida trasladada correctamente a Contabilidad'),
    (1, '2026-09-01', 'Consolidado bancario de septiembre 2026', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), 232750.5000, 232750.5000, 'TRASLADADA',
        'tesoreria.ana', '2026-10-01 08:00:00', 'contabilidad.jgomez', '2026-10-01 10:15:00', 'PCB-1-202609-GTQ', 'PART-CONT-2026-0930', 'Partida trasladada correctamente a Contabilidad'),
    (1, '2026-10-01', 'Consolidado bancario de octubre 2026', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), 264300.0000, 264300.0000, 'VALIDADA',
        'tesoreria.ana', '2026-11-02 08:00:00', NULL, NULL, 'PCB-1-202610-GTQ', NULL, NULL),
    (1, '2026-10-01', 'Consolidado bancario de octubre 2026 en dolares', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD'), 9600.0000, 9600.0000, 'GENERADA',
        'tesoreria.luis', '2026-11-02 08:05:00', NULL, NULL, 'PCB-1-202610-USD', NULL, NULL),
    (1, '2026-11-01', 'Consolidado bancario de noviembre 2026 (parcial)', (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), 41550.0000, 41550.0000, 'GENERADA',
        'tesoreria.ana', '2026-11-06 18:00:00', NULL, NULL, 'PCB-1-202611-GTQ', NULL, NULL);


-- ------------------------------------------------------------
-- 28. tblDetallePartidaConsolidada
-- ------------------------------------------------------------
INSERT INTO tblDetallePartidaConsolidada
    (Fk_PartidaConsolidada_Codigo, codigoCuentaContableDetallePartidaConsolidada, nombreCuentaContableDetallePartidaConsolidada, debeDetallePartidaConsolidada, haberDetallePartidaConsolidada) VALUES
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202608-GTQ'), '2.01.01.001', 'Proveedores locales', 185400.0000, 0.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202608-GTQ'), '1.01.02.001', 'Bancos moneda nacional', 0.0000, 185400.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202609-GTQ'), '2.01.01.001', 'Proveedores locales', 232750.5000, 0.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202609-GTQ'), '1.01.02.001', 'Bancos moneda nacional', 0.0000, 232750.5000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202610-GTQ'), '2.01.01.001', 'Proveedores locales', 264300.0000, 0.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202610-GTQ'), '1.01.02.001', 'Bancos moneda nacional', 0.0000, 264300.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202610-USD'), '2.01.01.001', 'Proveedores locales', 9600.0000, 0.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202610-USD'), '1.01.02.003', 'Bancos moneda extranjera', 0.0000, 9600.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202611-GTQ'), '1.01.02.001', 'Bancos moneda nacional (Industrial)', 6550.0000, 0.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202611-GTQ'), '1.01.02.002', 'Bancos moneda nacional (Banrural)', 4379.5000, 0.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202611-GTQ'), '2.01.01.001', 'Proveedores locales', 26370.5000, 0.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202611-GTQ'), '2.01.02.001', 'Sueldos por pagar', 4250.0000, 0.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202611-GTQ'), '1.01.02.004', 'Bancos moneda nacional (G&T nomina)', 0.0000, 4250.0000),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202611-GTQ'), '1.01.04.001', 'Clientes locales', 0.0000, 37300.0000);


-- ------------------------------------------------------------
-- 29. tblPartidaConsolidadaPolizaBancaria
-- ------------------------------------------------------------
INSERT INTO tblPartidaConsolidadaPolizaBancaria (Fk_PartidaConsolidada_Codigo, Fk_PolizaBancaria_Codigo) VALUES
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202611-GTQ'),
     (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0001')),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202611-GTQ'),
     (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0002')),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202611-GTQ'),
     (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0003')),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202611-GTQ'),
     (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0004')),
    ((SELECT Pk_PartidaConsolidada FROM tblPartidaConsolidada WHERE claveTrasladoPartidaConsolidada = 'PCB-1-202611-GTQ'),
     (SELECT p.idPolizaBancaria FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario = p.Fk_MovimientoBancario_Codigo WHERE m.referenciaMovimientoBancario = 'MB-2026-0005'));


-- ------------------------------------------------------------
-- 30. tblAutOrdenCompra
-- ------------------------------------------------------------
INSERT INTO tblAutOrdenCompra
    (Fk_OrdenCompraRecibida_Codigo, identificadorEmpresaAutOrdenCompra, estadoAutOrdenCompra, identificadorUsuarioAutorizaAutOrdenCompra,
     fechaDecisionAutOrdenCompra, comentarioAutOrdenCompra, motivoRechazoAutOrdenCompra) VALUES
    ((SELECT Pk_OrdenCompraRecibida FROM tblOrdenCompraRecibida WHERE referenciaExternaOrdenCompraRecibida = 'OC-2026-0201'), 1, 'AUTORIZADA', 3, '2026-10-16 10:20:00', 'Autorizada segun presupuesto de materia prima', NULL),
    ((SELECT Pk_OrdenCompraRecibida FROM tblOrdenCompraRecibida WHERE referenciaExternaOrdenCompraRecibida = 'OC-2026-0202'), 1, 'AUTORIZADA', 3, '2026-10-21 09:45:00', 'Autorizada segun presupuesto de empaques', NULL),
    ((SELECT Pk_OrdenCompraRecibida FROM tblOrdenCompraRecibida WHERE referenciaExternaOrdenCompraRecibida = 'OC-2026-0203'), 1, 'AUTORIZADA', 2, '2026-10-29 14:10:00', 'Compra de maquinaria autorizada por gerencia', NULL),
    ((SELECT Pk_OrdenCompraRecibida FROM tblOrdenCompraRecibida WHERE referenciaExternaOrdenCompraRecibida = 'OC-2026-0204'), 1, 'AUTORIZADA', 2, '2026-10-31 11:00:00', 'Especias para la produccion de noviembre', NULL),
    ((SELECT Pk_OrdenCompraRecibida FROM tblOrdenCompraRecibida WHERE referenciaExternaOrdenCompraRecibida = 'OC-2026-0205'), 1, 'PENDIENTE', NULL, NULL, NULL, NULL);


-- ------------------------------------------------------------
-- 31. tblAutDisponibilidadFondos
-- ------------------------------------------------------------
INSERT INTO tblAutDisponibilidadFondos
    (Fk_AutOrdenCompra_Codigo, Fk_CuentaBancaria_Codigo, fechaCorteAutDisponibilidadFondos, saldoDisponibleAutDisponibilidadFondos,
     Fk_Moneda_Origen_Codigo, Fk_Moneda_Destino_Codigo, tipoCambioAutDisponibilidadFondos, Fk_TipoCambio_Codigo, fuenteAutDisponibilidadFondos) VALUES
    ((SELECT a.Pk_AutOrdenCompra FROM tblAutOrdenCompra a JOIN tblOrdenCompraRecibida oc ON oc.Pk_OrdenCompraRecibida = a.Fk_OrdenCompraRecibida_Codigo WHERE oc.referenciaExternaOrdenCompraRecibida = 'OC-2026-0201'),
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'), '2026-10-16', 150000.0000,
        (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), 1.0000, NULL, 'Disponibilidad diaria'),
    ((SELECT a.Pk_AutOrdenCompra FROM tblAutOrdenCompra a JOIN tblOrdenCompraRecibida oc ON oc.Pk_OrdenCompraRecibida = a.Fk_OrdenCompraRecibida_Codigo WHERE oc.referenciaExternaOrdenCompraRecibida = 'OC-2026-0202'),
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '3011-045678-2'), '2026-10-21', 60000.0000,
        (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), 1.0000, NULL, 'Disponibilidad diaria'),
    ((SELECT a.Pk_AutOrdenCompra FROM tblAutOrdenCompra a JOIN tblOrdenCompraRecibida oc ON oc.Pk_OrdenCompraRecibida = a.Fk_OrdenCompraRecibida_Codigo WHERE oc.referenciaExternaOrdenCompraRecibida = 'OC-2026-0203'),
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '90-4421-3'), '2026-10-29', 18000.0000,
        (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'USD'), 1.0000, NULL, 'Disponibilidad diaria'),
    ((SELECT a.Pk_AutOrdenCompra FROM tblAutOrdenCompra a JOIN tblOrdenCompraRecibida oc ON oc.Pk_OrdenCompraRecibida = a.Fk_OrdenCompraRecibida_Codigo WHERE oc.referenciaExternaOrdenCompraRecibida = 'OC-2026-0204'),
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'), '2026-10-31', 120000.0000,
        (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), 1.0000, NULL, 'Disponibilidad diaria'),
    ((SELECT a.Pk_AutOrdenCompra FROM tblAutOrdenCompra a JOIN tblOrdenCompraRecibida oc ON oc.Pk_OrdenCompraRecibida = a.Fk_OrdenCompraRecibida_Codigo WHERE oc.referenciaExternaOrdenCompraRecibida = 'OC-2026-0205'),
        (SELECT idCuentaBancaria FROM tblCuentaBancaria WHERE numeroCuentaBancaria = '000-215487-9'), '2026-11-06', 85250.0000,
        (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ'), 1.0000, NULL, 'Disponibilidad diaria');


-- ------------------------------------------------------------
-- 32. tblAutPagoOrden
-- ------------------------------------------------------------
INSERT INTO tblAutPagoOrden
    (Fk_AutDisponibilidadFondos_Codigo, montoAsignadoAutPagoOrden, estadoAutPagoOrden, fechaReservaAutPagoOrden, Fk_OrdenPago_Codigo) VALUES
    ((SELECT d.idAutDisponibilidadFondos FROM tblAutDisponibilidadFondos d JOIN tblAutOrdenCompra a ON a.Pk_AutOrdenCompra = d.Fk_AutOrdenCompra_Codigo JOIN tblOrdenCompraRecibida oc ON oc.Pk_OrdenCompraRecibida = a.Fk_OrdenCompraRecibida_Codigo WHERE oc.referenciaExternaOrdenCompraRecibida = 'OC-2026-0201'),
        18450.0000, 'APLICADA', '2026-10-16 10:30:00', (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0201')),
    ((SELECT d.idAutDisponibilidadFondos FROM tblAutDisponibilidadFondos d JOIN tblAutOrdenCompra a ON a.Pk_AutOrdenCompra = d.Fk_AutOrdenCompra_Codigo JOIN tblOrdenCompraRecibida oc ON oc.Pk_OrdenCompraRecibida = a.Fk_OrdenCompraRecibida_Codigo WHERE oc.referenciaExternaOrdenCompraRecibida = 'OC-2026-0202'),
        7920.5000, 'APLICADA', '2026-10-21 09:55:00', (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0202')),
    ((SELECT d.idAutDisponibilidadFondos FROM tblAutDisponibilidadFondos d JOIN tblAutOrdenCompra a ON a.Pk_AutOrdenCompra = d.Fk_AutOrdenCompra_Codigo JOIN tblOrdenCompraRecibida oc ON oc.Pk_OrdenCompraRecibida = a.Fk_OrdenCompraRecibida_Codigo WHERE oc.referenciaExternaOrdenCompraRecibida = 'OC-2026-0203'),
        3200.0000, 'RESERVADA', '2026-11-06 09:10:00', (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0204')),
    ((SELECT d.idAutDisponibilidadFondos FROM tblAutDisponibilidadFondos d JOIN tblAutOrdenCompra a ON a.Pk_AutOrdenCompra = d.Fk_AutOrdenCompra_Codigo JOIN tblOrdenCompraRecibida oc ON oc.Pk_OrdenCompraRecibida = a.Fk_OrdenCompraRecibida_Codigo WHERE oc.referenciaExternaOrdenCompraRecibida = 'OC-2026-0204'),
        41300.0000, 'RESERVADA', '2026-11-06 09:25:00', (SELECT idOrdenPago FROM tblOrdenPago WHERE referenciaOrdenPago = 'OP-2026-0205')),
    ((SELECT d.idAutDisponibilidadFondos FROM tblAutDisponibilidadFondos d JOIN tblAutOrdenCompra a ON a.Pk_AutOrdenCompra = d.Fk_AutOrdenCompra_Codigo JOIN tblOrdenCompraRecibida oc ON oc.Pk_OrdenCompraRecibida = a.Fk_OrdenCompraRecibida_Codigo WHERE oc.referenciaExternaOrdenCompraRecibida = 'OC-2026-0205'),
        12875.0000, 'PROPUESTA', NULL, NULL);

-- resusos Humanos 
-- ---------- 1. CATALOGOS ----------
INSERT INTO tblTipoPuesto (idTipoPuesto, nombreTipoPuesto) VALUES
(3, 'Gerencial'),
(4, 'Técnico'),
(5, 'Ventas'),
(6, 'Supervisión'),
(7, 'Temporal');

INSERT INTO tblPuesto (idPuesto, idTipoPuesto, nombrePuesto, descripcionPuesto) VALUES
(7,  1, 'Analista de Recursos Humanos', 'Gestión de personal, asistencia y nómina'),
(8,  1, 'Contador General',             'Control contable y financiero de la empresa'),
(9,  2, 'Operario de Producción',       'Elaboración y empaque de embutidos'),
(10, 5, 'Vendedor de Campo',            'Atención y visita de clientes y rutas de venta'),
(11, 6, 'Supervisora de Logística',     'Supervisa bodega, despacho y distribución');

INSERT INTO tblDepartamento (idDepartamento, nombreDepartamento) VALUES
(3, 'Recursos Humanos'),
(4, 'Contabilidad'),
(5, 'Producción'),
(6, 'Ventas'),
(7, 'Logística'),
(8, 'bancos'),
(9, 'compras');

INSERT INTO tblJornada (idJornada, nombreJornada, horaInicioJornada, horaFinJornada, minutosToleranciaJornada) VALUES
(2, 'Nocturna',     '19:00:00', '05:00:00', 5),
(3, 'Mixta',        '13:00:00', '21:00:00', 10),
(4, 'Medio Tiempo', '08:00:00', '12:00:00', 5),
(5, 'Matutina',     '06:00:00', '14:00:00', 5),
(6, 'Vespertina',   '14:00:00', '22:00:00', 10);

INSERT INTO tblEmpresa (idEmpresa, nitEmpresa, razonSocialEmpresa, nombreComercialEmpresa, direccionEmpresa, representanteLegalEmpresa) VALUES
(1, '1234567-8', 'Embutidos Terminus, S.A.',             'Terminus Embutidos', '5a. Avenida 10-20 Zona 12, Ciudad de Guatemala', 'Roberto Alejandro Estrada Paz'),
(2, '2345678-9', 'Distribuidora Don Carlos, S.A.',       'Don Carlos',         '12 Calle 3-45 Zona 1, Quetzaltenango',           'Carlos Enrique Díaz Ruiz'),
(3, '3456789-0', 'Carnes y Embutidos del Norte, S.A.',   NULL,                 'Avenida Las Américas 7-88 Zona 14, Guatemala',   'Marta Lucía Ordóñez Vela'),
(4, '4567890-1', 'Industrias Cárnicas La Antigua, S.A.', 'La Antigua',         '3a. Calle Poniente 15, Antigua Guatemala',       'Hugo René Chávez Lara'),
(5, '5678901-2', 'Comercial Embutidos del Sur, S.A.',    NULL,                 'Km 28 Ruta al Pacífico, Palín, Escuintla',       'Silvia Marina Cabrera Gómez');

INSERT INTO tblMetodoPago (idMetodoPago, nombreMetodoPago, descripcionMetodoPago) VALUES
(1, 'Efectivo',               'Pago en efectivo en caja de la empresa'),
(2, 'Transferencia bancaria', 'Acreditación directa a la cuenta del empleado'),
(3, 'Cheque',                 'Pago mediante cheque a nombre del empleado'),
(4, 'Depósito monetario',     'Depósito en cuenta bancaria del empleado'),
(5, 'Tarjeta de nómina',      'Acreditación en tarjeta de débito de nómina');

-- idCuenta (cuenta contable) se deja en NULL; se puede asignar luego con UPDATE.
-- efecto: 1 = percepción, 0 = deducción
INSERT INTO tblPercepcionDeduccion
    (idPercepcionDeduccion, nombrePercepcionDeduccion, periodoPercepcionDeduccion, efectoPercepcionDeduccion,
     tipoPercepcionDeduccion, valorTipoPercepcionDeduccion, aplicacionPercepcionDeduccion) VALUES
(1, 'Horas Extra',            'MENSUAL', 1, 'Monto', 0.0000, 'Todos'),
(2, 'Cuota IGSS Laboral',     'MENSUAL', 0, '%',     4.8300, 'Todos'),
(3, 'ISR Retención',          'ANUAL',   0, 'Tabla', 0.0000, 'Algunos'),
(4, 'Indemnización',          'ANUAL',   1, 'Monto', 0.0000, 'Algunos'),
(5, 'Aguinaldo Proporcional', 'ANUAL',   1, 'Monto', 0.0000, 'Todos');

-- Tabla del ISR (concepto 3), valores de prueba
INSERT INTO tblPercepcionDeduccionRango
    (idPercepcionDeduccionRango, idPercepcionDeduccion, salarioDesdePercepcionDeduccionRango,
     salarioHastaPercepcionDeduccionRango, modoValorPercepcionDeduccionRango,
     valorPercepcionDeduccionRango, montoFijoBasePercepcionDeduccionRango) VALUES
(1, 3,      0.0000,  50000.0000, '%', 0.0000,     0.0000),
(2, 3,  50000.0100, 100000.0000, '%', 3.0000,     0.0000),
(3, 3, 100000.0100, 200000.0000, '%', 5.0000,  1500.0000),
(4, 3, 200000.0100, 300000.0000, '%', 6.0000,  6500.0000),
(5, 3, 300000.0100,        NULL, '%', 7.0000, 12500.0000);

INSERT INTO tblMotivoBaja (idMotivoBaja, nombreMotivoBaja, aplicaIndemnizacionMotivoBaja) VALUES
(1, 'Renuncia voluntaria',      0),
(2, 'Despido justificado',      0),
(3, 'Despido injustificado',    1),
(4, 'Mutuo acuerdo',            0),
(5, 'Finalización de contrato', 0);

INSERT INTO tblParametroLey
    (idParametroLey, idEmpresa, codigoParametroLey, descripcionParametroLey, valorParametroLey, vigenteDesdeParametroLey, vigenteHastaParametroLey) VALUES
(1, NULL, 'IGSS_LABORAL',                 'Porcentaje de cuota laboral IGSS',                             4.8300,   '2026-01-01', NULL),
(2, NULL, 'DIAS_BASE_INDEMNIZACION',      'Días base del año para calcular la indemnización',             365.0000, '2026-01-01', NULL),
(3, NULL, 'DIAS_VACACIONES_ANIO',         'Días de vacaciones por año laborado',                          15.0000,  '2026-01-01', NULL),
(4, NULL, 'MESES_PROMEDIO_INDEMNIZACION', 'Meses usados para el salario promedio de indemnización',       6.0000,   '2026-01-01', NULL),
(5, 1,    'DIAS_VACACIONES_ANIO',         'Días de vacaciones por año (beneficio propio de la empresa)',  20.0000,  '2026-01-01', NULL);

INSERT INTO tblInstructor (idInstructor, nombreInstructor, especialidadInstructor, correoInstructor) VALUES
(1, 'Mario Estuardo Barrios',    'Seguridad industrial',          'mbarrios@capacitaciones.gt'),
(2, 'Lucía Fernanda Aguilar',    'Ofimática y análisis de datos', 'laguilar@capacitaciones.gt'),
(3, 'Pedro Antonio Sandoval',    'Servicio al cliente',           'psandoval@capacitaciones.gt'),
(4, 'Gabriela Noemí Cifuentes',  'Liderazgo y recursos humanos',  'gcifuentes@capacitaciones.gt'),
(5, 'Rodrigo Javier Montenegro', 'Sistemas ERP',                  'rmontenegro@capacitaciones.gt');

-- Tipos de permiso (los usa la pantalla de Asistencia / Permisos).
-- conGoceSueldoTipoPermiso: 1 = con goce de sueldo, 0 = sin goce
INSERT INTO tblTipoPermiso (idTipoPermiso, nombreTipoPermiso, descripcionTipoPermiso, conGoceSueldoTipoPermiso) VALUES
(1, 'Cita médica',     'Consulta o examen médico',                 1),
(2, 'Asunto personal', 'Trámites o asuntos personales sin goce',   0),
(3, 'Duelo familiar',  'Fallecimiento de un familiar',             1),
(4, 'Trámite personal','Gestiones en entidades públicas o privadas', 1),
(5, 'Asunto familiar', 'Situaciones familiares que requieren presencia', 1);

-- ---------- 2. EMPLEADOS ----------
-- tblEmpleado final: sin puestoEmpleado; con idDepartamento, idJornada y salarioEmpleado (NOT NULL).
-- (idPuesto, si existe en tu tabla, queda en NULL: el puesto vive en tblPuestoEmpleado.)
INSERT INTO tblEmpleado
    (idEmpleado, codigoEmpleado, dpiEmpleado, nitEmpleado, nombresEmpleado, apellidosEmpleado,
     idDepartamento, idJornada, salarioEmpleado, generoEmpleado, fechaNacimientoEmpleado,
     fechaContratacionEmpleado, telefonoEmpleado, correoEmpleado) VALUES
(20, 'EMP-011', '2456123400101', '4455667-1', 'Ana Lucía',       'Pérez Gómez',    3, 1, 6500.0000, 'F', '1996-04-12', '2024-03-01', '55501111', 'ana.perez@terminus.com'),
(21, 'EMP-012', '2987456120101', '5566778-2', 'Luis Fernando',   'Castillo Ruiz',  4, 1, 7200.0000, 'M', '1992-08-25', '2023-07-17', '55502222', 'luis.castillo@terminus.com'),
(22, 'EMP-013', '3012456780101', '6677889-3', 'Sofía Alejandra', 'Morales Díaz',   5, 1, 5800.0000, 'F', '1999-01-30', '2025-02-03', '55503333', 'sofia.morales@terminus.com'),
(23, 'EMP-014', '1987456320101', '7788990-4', 'Jorge Mario',     'Hernández Soto', 6, 1, 6000.0000, 'M', '1994-11-09', '2022-11-14', '55504444', 'jorge.hernandez@terminus.com'),
(24, 'EMP-015', '2234567890101', '8899001-5', 'Karla Beatriz',   'Ortiz Méndez',   7, 5, 5500.0000, 'F', '2000-06-18', '2025-06-02', '55505555', 'karla.ortiz@terminus.com');

-- Puesto vigente de cada empleado nuevo.
-- vigentePuestoEmpleado es columna generada: no se inserta.
-- No se fijan ids a propósito: tblPuestoEmpleado ya trae filas de la migración del script
-- general (puestos de los empleados existentes) y los ids fijos chocaban (error 1062).
INSERT INTO tblPuestoEmpleado (idEmpleado, idPuesto, fechaAsignacionPuestoEmpleado, fechaFinPuestoEmpleado) VALUES
(20, 7,  '2024-03-01', NULL),
(21, 8,  '2023-07-17', NULL),
(22, 9,  '2025-02-03', NULL),
(23, 10, '2022-11-14', NULL),
(24, 11, '2025-06-02', NULL);

INSERT INTO tblEmpleadoMetodoPago (idEmpleado, idMetodoPago, fechaAsignacionEmpleadoMetodoPago, montoEmpleadoMetodoPago) VALUES
(20, 2, '2024-03-01', 6500.0000),
(21, 4, '2023-07-17', 7200.0000),
(22, 5, '2025-02-03', 5800.0000),
(23, 2, '2022-11-14', 6000.0000),
(24, 1, '2025-06-02', 5500.0000);

-- Conceptos con aplicación 'Algunos' (empleado XOR departamento)
INSERT INTO tblPercepcionDeduccionEmpleado
    (idPercepcionDeduccionEmpleado, idPercepcionDeduccion, idEmpleado, idDepartamento) VALUES
(1, 3, 20,   NULL),
(2, 3, 21,   NULL),
(3, 3, 22,   NULL),
(4, 4, 22,   NULL),
(5, 4, NULL, 3);

-- ---------- 3. ASISTENCIA (empleado 20, una por mes de jun a oct; todas con salida extendida) ----------
INSERT INTO tblAsistenciaEmpleado
    (idAsistenciaEmpleado, idEmpleado, fechaAsistenciaEmpleado, horaEntradaAsistenciaEmpleado,
     horaSalidaAsistenciaEmpleado, estadoAsistenciaEmpleado, observacionAsistenciaEmpleado) VALUES
(1, 20, '2026-06-15', '08:00:00', '19:00:00', 'PRESENTE', 'Cierre de proyecto'),
(2, 20, '2026-07-15', '07:58:00', '18:00:00', 'PRESENTE', NULL),
(3, 20, '2026-08-14', '08:12:00', '18:30:00', 'TARDANZA', 'Llegó 12 minutos tarde'),
(4, 20, '2026-09-15', '08:00:00', '19:30:00', 'PRESENTE', 'Cierre de planilla'),
(5, 20, '2026-10-15', '08:00:00', '19:00:00', 'PRESENTE', NULL);

-- ---------- 4. NOMINA (empleado 20, jun a oct 2026) ----------
-- totalPercepcion incluye el salario base; totalDeduccion = IGSS 4.83% de la percepción;
-- totalNeto = totalPercepcion - totalDeduccion. duracionHoraExtra en minutos.
INSERT INTO tblNominaEncabezado
    (idNominaEncabezado, idEmpleado, fechaInicioNominaEncabezado, fechaFinalNominaEncabezado,
     salarioBaseNominaEncabezado, duracionHoraExtraNominaEncabezado, totalHorasExtraNominaEncabezado,
     totalPercepcionNominaEncabezado, totalDeduccionNominaEncabezado, totalNetoNominaEncabezado,
     estadoNminaEncabezado) VALUES
(1, 20, '2026-06-01', '2026-06-30', 6500.0000, 120,  81.2499, 6581.2499, 317.8744, 6263.3755, 'Pagada'),
(2, 20, '2026-07-01', '2026-07-31', 6500.0000,  60,  40.6250, 6540.6250, 315.9122, 6224.7128, 'Pagada'),
(3, 20, '2026-08-01', '2026-08-31', 6500.0000,  90,  60.9374, 6560.9374, 316.8933, 6244.0441, 'Pagada'),
(4, 20, '2026-09-01', '2026-09-30', 6500.0000, 150, 101.5624, 6601.5624, 318.8555, 6282.7069, 'Pagada'),
(5, 20, '2026-10-01', '2026-10-31', 6500.0000, 120,  81.2499, 6581.2499, 317.8744, 6263.3755, 'Aprobada');

-- Detalle: 2 conceptos por nómina (1 = Horas Extra, 2 = Cuota IGSS Laboral) -> 10 filas para que cuadre
INSERT INTO tblNominaDetalle (idNominaDetalle, idNominaEncabezado, idPercepcionDeduccion, montoNominaDetalle) VALUES
(1,  1, 1,  81.2499),
(2,  1, 2, 317.8744),
(3,  2, 1,  40.6250),
(4,  2, 2, 315.9122),
(5,  3, 1,  60.9374),
(6,  3, 2, 316.8933),
(7,  4, 1, 101.5624),
(8,  4, 2, 318.8555),
(9,  5, 1,  81.2499),
(10, 5, 2, 317.8744);

-- valor hora = 6500 / 30 / 8 = 27.0833; recargo 50%
INSERT INTO tblHoraExtra
    (idHoraExtra, idPercepcionDeduccion, idEmpleado, idAsistenciaEmpleado, duracionHoraExtra,
     valorHoraOrdinariaHoraExtra, porcentajeRecargoHoraExtra, totalPagoHoraExtra, estadoHoraExtra,
     idUsuarioAprueba, fechaAprobacionHoraExtra, idNominaEncabezado, observacionHoraExtra) VALUES
(1, 1, 20, 1, 120, 27.0833, 0.50,  81.2499, 'PAGADA',   1, '2026-06-16 09:00:00', 1, 'Cierre de proyecto'),
(2, 1, 20, 2,  60, 27.0833, 0.50,  40.6250, 'PAGADA',   1, '2026-07-16 09:00:00', 2, NULL),
(3, 1, 20, 3,  90, 27.0833, 0.50,  60.9374, 'PAGADA',   1, '2026-08-17 09:00:00', 3, NULL),
(4, 1, 20, 4, 150, 27.0833, 0.50, 101.5624, 'PAGADA',   2, '2026-09-16 09:00:00', 4, 'Cierre de planilla'),
(5, 1, 20, 5, 120, 27.0833, 0.50,  81.2499, 'APROBADA', 1, '2026-10-16 09:00:00', 5, NULL);

-- ---------- 5. CIERRE DE NOMINA ----------
-- Un cierre por mes (jun a oct). Debe = percepción + patronal (12.67%); Haber = deducción + patronal + neto
INSERT INTO tblCierreNomina
    (idCierreNomina, periodoCierreNomina, fechaInicioCierreNomina, fechaFinalCierreNomina,
     cantidadEmpleadoCierreNomina, totalPercepcionCierreNomina, totalDeduccionCierreNomina,
     totalNetoCierreNomina, totalPatronalCierreNomina, totalDebeCierreNomina, totalHaberCierreNomina,
     documentoOrigenCierreNomina, codigoPolizaCierreNomina, fechaPolizaCierreNomina,
     estadoPolizaCierreNomina, idUsuarioCierra, fechaEjecucionCierreNomina) VALUES
(1, '2026-06', '2026-06-01', '2026-06-30', 1, 6581.2499, 317.8744, 6263.3755, 833.8444, 7415.0943, 7415.0943, 'NOM-2026-06', 'POL-NOM-2026-06', '2026-06-30', 'APLICADA', 1, '2026-07-01 08:30:00'),
(2, '2026-07', '2026-07-01', '2026-07-31', 1, 6540.6250, 315.9122, 6224.7128, 828.6972, 7369.3222, 7369.3222, 'NOM-2026-07', 'POL-NOM-2026-07', '2026-07-31', 'APLICADA', 1, '2026-08-03 08:30:00'),
(3, '2026-08', '2026-08-01', '2026-08-31', 1, 6560.9374, 316.8933, 6244.0441, 831.2708, 7392.2082, 7392.2082, 'NOM-2026-08', 'POL-NOM-2026-08', '2026-08-31', 'APLICADA', 1, '2026-09-01 08:30:00'),
(4, '2026-09', '2026-09-01', '2026-09-30', 1, 6601.5624, 318.8555, 6282.7069, 836.4180, 7437.9804, 7437.9804, 'NOM-2026-09', 'POL-NOM-2026-09', '2026-09-30', 'APLICADA', 1, '2026-10-01 08:30:00'),
(5, '2026-10', '2026-10-01', '2026-10-31', 1, 6581.2499, 317.8744, 6263.3755, 833.8444, 7415.0943, 7415.0943, 'NOM-2026-10', 'POL-NOM-2026-10', '2026-10-31', 'VALIDADA', 1, '2026-11-02 08:30:00');

-- Acumulados del año tomando jun a oct como base de prueba
INSERT INTO tblCierreNominaEmpleado
    (idCierreNominaEmpleado, idCierreNomina, idNominaEncabezado, idEmpleado, salarioBaseCierreNominaEmpleado,
     totalPercepcionCierreNominaEmpleado, totalDeduccionCierreNominaEmpleado, totalNetoCierreNominaEmpleado,
     acumuladoPercepcionAnioCierreNominaEmpleado, acumuladoDeduccionAnioCierreNominaEmpleado) VALUES
(1, 1, 1, 20, 6500.0000, 6581.2499, 317.8744, 6263.3755,  6581.2499,  317.8744),
(2, 2, 2, 20, 6500.0000, 6540.6250, 315.9122, 6224.7128, 13121.8749,  633.7866),
(3, 3, 3, 20, 6500.0000, 6560.9374, 316.8933, 6244.0441, 19682.8123,  950.6799),
(4, 4, 4, 20, 6500.0000, 6601.5624, 318.8555, 6282.7069, 26284.3747, 1269.5354),
(5, 5, 5, 20, 6500.0000, 6581.2499, 317.8744, 6263.3755, 32865.6246, 1587.4098);

-- ---------- 6. EVALUACION DE DESEMPEÑO (esquema final: 5 notas + puntaje) ----------
INSERT INTO tblEvaluacionDesempeno
    (idEvaluacionDesempeno, idEmpleado, idEvaluador, fechaEvaluacionDesempeno,
     calidadEvaluacionDesempeno, productividadEvaluacionDesempeno, trabajoEquipoEvaluacionDesempeno,
     puntualidadEvaluacionDesempeno, iniciativaEvaluacionDesempeno, puntajeEvaluacionDesempeno) VALUES
(1, 20, 1,  '2026-10-15', 5, 4, 5, 5, 4, 4.60),
(2, 21, 1,  '2026-10-15', 4, 4, 3, 5, 4, 4.00),
(3, 22, 3,  '2026-10-16', 3, 3, 4, 2, 3, 3.00),
(4, 23, 3,  '2026-10-16', 5, 5, 5, 4, 5, 4.80),
(5, 24, 13, '2026-10-16', 2, 3, 3, 4, 2, 2.80);

INSERT INTO tblPlanMejora (idPlanMejora, idEvaluacionDesempeno, nombrePlanMejora, descripcionPlanMejora) VALUES
(1, 5, 'Plan de Refuerzo en Calidad e Iniciativa', 'Acompañamiento semanal y metas de calidad para el siguiente trimestre'),
(2, 3, 'Plan de Puntualidad y Productividad',      'Seguimiento diario de horarios y metas de producción'),
(3, 2, 'Plan de Trabajo en Equipo',                'Talleres de comunicación y proyectos colaborativos'),
(4, 1, 'Plan de Desarrollo de Iniciativa',         'Asignación de proyectos de mejora continua'),
(5, 4, 'Plan de Liderazgo',                        'Preparación para coordinar equipos de trabajo');

-- ---------- 7. PERMISOS Y VACACIONES (estructura original: con tipo, unidad, estado y periodo) ----------
-- Permisos: unidad DIAS (horas NULL) o HORAS (con horaInicio/horaFin).
-- conGoceSueldoPermisoEmpleado se mantiene (columna agregada en el script general).
INSERT INTO tblPermisoEmpleado
    (idPermisoEmpleado, idEmpleado, idTipoPermiso, fechaInicioPermisoEmpleado, fechaFinPermisoEmpleado,
     horaInicioPermisoEmpleado, horaFinPermisoEmpleado, unidadPermisoEmpleado, totalPermisoEmpleado,
     motivoPermisoEmpleado, conGoceSueldoPermisoEmpleado, estadoPermisoEmpleado) VALUES
(1, 20, 4, '2026-11-09', '2026-11-09', NULL,       NULL,       'DIAS',  1.00, 'Trámite personal en el Registro Civil', 1, 'PENDIENTE'),
(2, 21, 5, '2026-11-10', '2026-11-11', NULL,       NULL,       'DIAS',  2.00, 'Asuntos familiares',                    1, 'APROBADO'),
(3, 22, 2, '2026-11-12', '2026-11-12', '08:00:00', '12:00:00', 'HORAS', 4.00, 'Trámite bancario',                      0, 'PENDIENTE'),
(4, 23, 2, '2026-11-18', '2026-11-20', NULL,       NULL,       'DIAS',  3.00, 'Curso externo de carácter personal',    0, 'RECHAZADO'),
(5, 24, 4, '2026-11-25', '2026-11-25', NULL,       NULL,       'DIAS',  1.00, 'Gestión de documentos personales',      1, 'PENDIENTE'),
(6, 1,  1, '2026-11-09', '2026-11-09', NULL,       NULL,       'DIAS',  1.00, 'Cita médica',                           1, 'APROBADO'),
(7, 13, 3, '2026-11-18', '2026-11-19', NULL,       NULL,       'DIAS',  2.00, 'Duelo familiar',                        1, 'APROBADO');

-- Un periodo de vacaciones 2026 por cada empleado que tiene vacaciones
INSERT INTO tblPeriodoVacacion
    (idPeriodoVacacion, idEmpleado, fechaInicioPeriodoVacacion, fechaFinPeriodoVacacion,
     diasCorrespondientesPeriodoVacacion, diasTomadosPeriodoVacacion) VALUES
(1, 20, '2026-01-01', '2026-12-31', 15.00, 0.00),
(2, 21, '2026-01-01', '2026-12-31', 15.00, 0.00),
(3, 22, '2026-01-01', '2026-12-31', 15.00, 0.00),
(4, 23, '2026-01-01', '2026-12-31', 15.00, 0.00),
(5, 24, '2026-01-01', '2026-12-31', 15.00, 0.00);

INSERT INTO tblVacacionEmpleado
    (idVacacionEmpleado, idEmpleado, idPeriodoVacacion, fechaInicioVacacionEmpleado, fechaFinVacacionEmpleado,
     diasTomarVacacionEmpleado, observacionVacacionEmpleado, estadoVacacionEmpleado) VALUES
(1, 20, 1, '2026-11-30', '2026-12-04', 5.00, 'Vacaciones de fin de año', 'PENDIENTE'),
(2, 21, 2, '2026-11-23', '2026-11-27', 5.00, NULL,                       'APROBADO'),
(3, 22, 3, '2026-11-23', '2026-11-27', 5.00, NULL,                       'PENDIENTE'),
(4, 23, 4, '2026-11-30', '2026-12-04', 5.00, 'Viaje familiar',           'APROBADO'),
(5, 24, 5, '2026-11-09', '2026-11-13', 5.00, NULL,                       'APROBADO');

-- ---------- 8. CAPACITACIONES (ahora con idInstructor) ----------
INSERT INTO tblCapacitacion
    (idCapacitacion, idInstructor, idDepartamento, nombreCapacitacion, descripcionCapacitacion,
     fechaInicioCapacitacion, fechaFinCapacitacion, costoCapacitacion, cupoCapacitacion, estadoCapacitacion) VALUES
(1, 1, 5,    'Seguridad e Higiene Industrial', 'Manejo seguro de maquinaria y normas de higiene en planta', '2026-09-14', '2026-09-18', 1200.0000, 20, 1),
(2, 2, 4,    'Excel Avanzado',                 'Fórmulas, tablas dinámicas y reportes',                        '2026-09-21', '2026-09-25',  850.0000, 15, 1),
(3, 3, 6,    'Atención al Cliente',            'Técnicas de servicio y manejo de quejas',                      '2026-10-05', '2026-10-09',  600.0000, 25, 1),
(4, 4, NULL, 'Liderazgo y Trabajo en Equipo',  'Habilidades de liderazgo para todas las áreas',                '2026-10-12', '2026-10-16', 1500.0000, 30, 1),
(5, 5, 1,    'Inducción a Sistemas ERP',       'Uso básico del ERP y flujos de información',                   '2026-10-19', '2026-10-23',  900.0000, 12, 1);

INSERT INTO tblAsignacionCapacitacion (idAsignacionCapacitacion, idEmpleado, idCapacitacion, fechaAsignacionCapacitacion) VALUES
(1, 20, 4, '2026-10-01'),
(2, 21, 2, '2026-09-10'),
(3, 22, 1, '2026-09-05'),
(4, 23, 3, '2026-09-28'),
(5, 24, 4, '2026-10-01');

INSERT INTO tblEvaluacionCapacitacion
    (idEvaluacionCapacitacion, idAsignacionCapacitacion, puntualidadEvaluacionCapacitacion,
     calidadEvaluacionCapacitacion, responsabilidadEvaluacionCapacitacion, desarrolloEvaluacionCapacitacion,
     resultadoFinalEvaluacionCapacitacion, observacionEvaluacionCapacitacion) VALUES
(1, 1, 90.00, 85.00, 88.00, 92.00, 88.75, 'Excelente participación'),
(2, 2, 95.00, 90.00, 92.00, 94.00, 92.75, 'Aplica lo aprendido en su trabajo'),
(3, 3, 80.00, 85.00, 78.00, 82.00, 81.25, NULL),
(4, 4, 70.00, 75.00, 80.00, 72.00, 74.25, 'Debe reforzar puntualidad'),
(5, 5, 88.00, 90.00, 85.00, 87.00, 87.50, NULL);

-- ---------- 9. BAJAS Y LIQUIDACIONES (fechas de prueba, diciembre 2026) ----------
INSERT INTO tblBajaEmpleado
    (idBajaEmpleado, idEmpleado, idMotivoBaja, fechaBajaEmpleado, observacionBajaEmpleado, idUsuarioRegistra) VALUES
(1, 20, 1, '2026-12-15', 'Renuncia presentada por escrito',    1),
(2, 21, 3, '2026-12-18', 'Terminación sin causa justificada',  1),
(3, 22, 4, '2026-12-22', 'Acuerdo firmado entre ambas partes', 1),
(4, 23, 5, '2026-12-28', 'Vencimiento del contrato',           1),
(5, 24, 2, '2026-12-31', 'Falta grave comprobada',             1);

-- salarioDia = salarioMensual / 30 (valores ilustrativos)
INSERT INTO tblLiquidacion
    (idLiquidacion, idEmpresa, idEmpleado, idBajaEmpleado, fechaIngresoLiquidacion, fechaBajaLiquidacion,
     diasLaboradosLiquidacion, salarioMensualLiquidacion, salarioPromedioLiquidacion,
     baseIndemnizacionLiquidacion, salarioDiaLiquidacion, totalPercepcionLiquidacion,
     totalDeduccionLiquidacion, totalNetoLiquidacion, parametrosUsadosLiquidacion,
     numeroFiniquitoLiquidacion, fechaEmisionFiniquitoLiquidacion, montoLetrasFiniquitoLiquidacion,
     estadoLiquidacion, idUsuarioCrea, idUsuarioAprueba, fechaAprobacionLiquidacion) VALUES
(1, 1, 20, 1, '2024-03-01', '2026-12-15', 1019, 6500.0000, 6500.0000, 6500.0000, 216.6667,   267.1233, 0.0000,   267.1233,
 '{"DIAS_BASE_INDEMNIZACION":{"valor":365,"origen":"LEY"},"MESES_PROMEDIO_INDEMNIZACION":{"valor":6,"origen":"LEY"}}',
 NULL, NULL, NULL, 'BORRADOR', 1, 2, NULL),
(2, 1, 21, 2, '2023-07-17', '2026-12-18', 1250, 7200.0000, 7200.0000, 7200.0000, 240.0000, 24657.5342, 0.0000, 24657.5342,
 '{"DIAS_BASE_INDEMNIZACION":{"valor":365,"origen":"LEY"},"MESES_PROMEDIO_INDEMNIZACION":{"valor":6,"origen":"LEY"}}',
 'FIN-2026-0001', '2026-12-18', 'Veinticuatro mil seiscientos cincuenta y siete quetzales con 53/100', 'PAGADA', 1, 2, '2026-12-18 10:00:00'),
(3, 1, 22, 3, '2025-02-03', '2026-12-22', 687, 5800.0000, 5800.0000, 5800.0000, 193.3333,    349.5890, 0.0000,   349.5890,
 '{"DIAS_BASE_INDEMNIZACION":{"valor":365,"origen":"LEY"},"MESES_PROMEDIO_INDEMNIZACION":{"valor":6,"origen":"LEY"}}',
 'FIN-2026-0002', '2026-12-22', 'Trescientos cuarenta y nueve quetzales con 59/100', 'APROBADA', 1, 2, '2026-12-22 09:30:00'),
(4, 1, 23, 4, '2022-11-14', '2026-12-28', 1505, 6000.0000, 6000.0000, 6000.0000, 200.0000,   460.2740, 0.0000,   460.2740,
 '{"DIAS_BASE_INDEMNIZACION":{"valor":365,"origen":"LEY"},"MESES_PROMEDIO_INDEMNIZACION":{"valor":6,"origen":"LEY"}}',
 'FIN-2026-0003', '2026-12-28', 'Cuatrocientos sesenta quetzales con 27/100', 'APROBADA', 1, 2, '2026-12-28 09:00:00'),
(5, 1, 24, 5, '2025-06-02', '2026-12-31', 577, 5500.0000, 5500.0000, 5500.0000, 183.3333,     467.1233, 0.0000,   467.1233,
 '{"DIAS_BASE_INDEMNIZACION":{"valor":365,"origen":"LEY"},"MESES_PROMEDIO_INDEMNIZACION":{"valor":6,"origen":"LEY"}}',
 'FIN-2026-0004', '2026-12-31', 'Cuatrocientos sesenta y siete quetzales con 12/100', 'ANULADA', 1, 2, '2026-12-31 09:00:00');

-- monto = base / 365 * días (indemnización solo en el despido injustificado)
INSERT INTO tblDetalleLiquidacion
    (idDetalleLiquidacion, idLiquidacion, idPercepcionDeduccion, diasDetalleLiquidacion, baseDetalleLiquidacion, montoDetalleLiquidacion) VALUES
(1, 1, 5,   15.00, 6500.0000,   267.1233),
(2, 2, 4, 1250.00, 7200.0000, 24657.5342),
(3, 3, 5,   22.00, 5800.0000,   349.5890),
(4, 4, 5,   28.00, 6000.0000,   460.2740),
(5, 5, 5,   31.00, 5500.0000,   467.1233);


-- =====================================================================


UPDATE tblEmpleado
SET salarioEmpleado = 7500.0000
WHERE idEmpleado IN (1, 3, 13)
  AND salarioEmpleado = 0;

-- ---------- 1. ASISTENCIA (semana del 2 al 6 de nov-2026) ----------

INSERT INTO tblAsistenciaEmpleado
    (idAsistenciaEmpleado, idEmpleado, fechaAsistenciaEmpleado, horaEntradaAsistenciaEmpleado,
     horaSalidaAsistenciaEmpleado, estadoAsistenciaEmpleado, observacionAsistenciaEmpleado) VALUES
-- Empleado 1
(6,  1,  '2026-11-02', '08:00:00', '17:00:00', 'PRESENTE', NULL),
(7,  1,  '2026-11-03', '08:09:00', '17:00:00', 'TARDANZA', 'Llegó 9 minutos tarde'),
(8,  1,  '2026-11-04', '07:55:00', '18:00:00', 'PRESENTE', 'Apoyo en cierre mensual'),
(9,  1,  '2026-11-05', '08:00:00', '19:00:00', 'PRESENTE', 'Revisión de planilla'),
(10, 1,  '2026-11-06', NULL,       NULL,       'AUSENTE',  'Ausencia sin aviso'),
-- Empleado 3
(11, 3,  '2026-11-02', '08:00:00', '17:00:00', 'PRESENTE', NULL),
(12, 3,  '2026-11-03', '08:00:00', '18:30:00', 'PRESENTE', 'Atención de auditoría'),
(13, 3,  '2026-11-04', '08:15:00', '17:00:00', 'TARDANZA', 'Llegó 15 minutos tarde'),
(14, 3,  '2026-11-05', '08:00:00', '17:00:00', 'PRESENTE', NULL),
(15, 3,  '2026-11-06', '08:00:00', '17:00:00', 'PRESENTE', NULL),
-- Empleado 13
(16, 13, '2026-11-02', '08:00:00', '17:00:00', 'PRESENTE', NULL),
(17, 13, '2026-11-03', '08:00:00', '17:00:00', 'PRESENTE', NULL),
(18, 13, '2026-11-04', '08:00:00', '20:00:00', 'PRESENTE', 'Reunión con proveedores'),
(19, 13, '2026-11-05', NULL,       NULL,       'PERMISO',  'Cita médica (permiso aprobado)'),
(20, 13, '2026-11-06', '08:00:00', '17:00:00', 'PRESENTE', NULL),
-- Empleados 21, 22 y 24
(21, 21, '2026-11-05', '08:00:00', '18:00:00', 'PRESENTE', 'Cierre contable'),
(22, 22, '2026-11-04', '08:00:00', '19:00:00', 'PRESENTE', 'Pedido urgente de producción'),
(23, 24, '2026-11-05', '06:00:00', '16:00:00', 'PRESENTE', 'Despacho extraordinario');

-- ---------- 2. HORAS EXTRA ----------

INSERT INTO tblHoraExtra
    (idHoraExtra, idPercepcionDeduccion, idEmpleado, idAsistenciaEmpleado, duracionHoraExtra,
     valorHoraOrdinariaHoraExtra, porcentajeRecargoHoraExtra, totalPagoHoraExtra, estadoHoraExtra,
     idUsuarioAprueba, fechaAprobacionHoraExtra, idNominaEncabezado, observacionHoraExtra) VALUES
(6,  1, 1,  8,   60, 31.2500, 0.50,  46.8750, 'APROBADA',  1,    '2026-11-05 09:00:00', NULL, 'Apoyo en cierre mensual'),
(7,  1, 1,  9,  120, 31.2500, 0.50,  93.7500, 'PENDIENTE', NULL, NULL,                  NULL, 'Revisión de planilla'),
(8,  1, 3,  12,  90, 31.2500, 0.50,  70.3125, 'APROBADA',  1,    '2026-11-04 09:00:00', NULL, 'Atención de auditoría'),
(9,  1, 13, 18, 180, 31.2500, 0.50, 140.6250, 'PENDIENTE', NULL, NULL,                  NULL, 'Reunión con proveedores'),
(10, 1, 21, 21,  60, 30.0000, 0.50,  45.0000, 'APROBADA',  1,    '2026-11-06 09:00:00', NULL, 'Cierre contable'),
(11, 1, 22, 22, 120, 24.1667, 0.50,  72.5001, 'RECHAZADA', 3,    '2026-11-05 09:00:00', NULL, 'No fue autorizada previamente'),
(12, 1, 24, 23, 120, 22.9167, 0.50,  68.7501, 'PENDIENTE', NULL, NULL,                  NULL, 'Despacho extraordinario');

-- ---------- 3. PERMISOS (estructura original: tipo, unidad, estado) ----------

INSERT INTO tblPermisoEmpleado
    (idPermisoEmpleado, idEmpleado, idTipoPermiso, fechaInicioPermisoEmpleado, fechaFinPermisoEmpleado,
     horaInicioPermisoEmpleado, horaFinPermisoEmpleado, unidadPermisoEmpleado, totalPermisoEmpleado,
     motivoPermisoEmpleado, conGoceSueldoPermisoEmpleado, estadoPermisoEmpleado) VALUES
(8,  13, 1, '2026-11-05', '2026-11-05', NULL,       NULL,       'DIAS',  1.00, 'Cita médica',                         1, 'APROBADO'),
(9,  3,  4, '2026-11-16', '2026-11-16', '14:00:00', '17:00:00', 'HORAS', 3.00, 'Trámite en la SAT',                    1, 'PENDIENTE'),
(10, 1,  2, '2026-11-23', '2026-11-23', NULL,       NULL,       'DIAS',  1.00, 'Asunto personal',                     0, 'PENDIENTE'),
(11, 3,  5, '2026-11-12', '2026-11-13', NULL,       NULL,       'DIAS',  2.00, 'Atención a un familiar',              1, 'APROBADO'),
(12, 13, 4, '2026-11-26', '2026-11-26', '08:00:00', '10:00:00', 'HORAS', 2.00, 'Gestión en el banco',                  1, 'RECHAZADO'),
(13, 21, 1, '2026-11-17', '2026-11-17', '09:00:00', '11:00:00', 'HORAS', 2.00, 'Control médico',                       1, 'APROBADO'),
(14, 23, 3, '2026-11-30', '2026-12-01', NULL,       NULL,       'DIAS',  2.00, 'Duelo familiar',                      1, 'PENDIENTE'),
(15, 20, 5, '2026-11-20', '2026-11-20', NULL,       NULL,       'DIAS',  1.00, 'Asunto familiar (solicitud cancelada)', 1, 'ANULADO');

-- ---------- 4. EVALUACIONES DE DESEMPEÑO ----------

INSERT INTO tblEvaluacionDesempeno
    (idEvaluacionDesempeno, idEmpleado, idEvaluador, fechaEvaluacionDesempeno,
     calidadEvaluacionDesempeno, productividadEvaluacionDesempeno, trabajoEquipoEvaluacionDesempeno,
     puntualidadEvaluacionDesempeno, iniciativaEvaluacionDesempeno, puntajeEvaluacionDesempeno) VALUES
-- Empleado 1 (evalúa el empleado 3)
(6,  1,  3, '2026-04-15', 3, 3, 4, 4, 3, 3.35),
(7,  1,  3, '2026-07-15', 4, 3, 4, 5, 4, 3.90),
(8,  1,  3, '2026-10-20', 4, 4, 5, 5, 4, 4.35),
-- Empleado 3 (evalúa el empleado 1)
(9,  3,  1, '2026-04-16', 4, 4, 3, 5, 3, 3.80),
(10, 3,  1, '2026-07-16', 4, 5, 4, 4, 4, 4.25),
(11, 3,  1, '2026-10-21', 5, 4, 4, 5, 5, 4.55),
-- Empleado 13 (evalúa el empleado 1)
(12, 13, 1, '2026-04-17', 2, 3, 3, 3, 2, 2.60),
(13, 13, 1, '2026-07-17', 3, 3, 4, 3, 3, 3.20),
(14, 13, 1, '2026-10-22', 4, 4, 4, 5, 4, 4.15);

-- Planes de mejora (uno por evaluación como máximo; nombre único)
INSERT INTO tblPlanMejora (idPlanMejora, idEvaluacionDesempeno, nombrePlanMejora, descripcionPlanMejora) VALUES
(6, 6,  'Plan de Productividad y Calidad',          'Metas semanales de productividad y revisión de calidad con el jefe inmediato'),
(7, 9,  'Plan de Trabajo en Equipo e Iniciativa',   'Participación en proyectos colaborativos y propuesta de al menos una mejora por mes'),
(8, 12, 'Plan de Nivelación General',               'Capacitación en calidad e iniciativa con seguimiento quincenal'),
(9, 13, 'Plan de Seguimiento Trimestral',           'Reunión mensual de seguimiento para sostener la mejora en calidad y productividad');

-- ---------- Verificación ----------
SELECT 'tblAsistenciaEmpleado' AS tabla, COUNT(*) AS filas FROM tblAsistenciaEmpleado
UNION ALL SELECT 'tblHoraExtra',            COUNT(*) FROM tblHoraExtra
UNION ALL SELECT 'tblPermisoEmpleado',      COUNT(*) FROM tblPermisoEmpleado
UNION ALL SELECT 'tblEvaluacionDesempeno',  COUNT(*) FROM tblEvaluacionDesempeno
UNION ALL SELECT 'tblPlanMejora',           COUNT(*) FROM tblPlanMejora;

-- Logistica
START TRANSACTION;

-- ============================================================================
-- TABLA: tblCategoria
-- ============================================================================
-- Registro 1
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (1, 'Embutidos', 0.10);

-- Registro 2
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (2, 'Carnes frías', 0.10);

-- Registro 3
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (3, 'Ahumados', 0.10);

-- Registro 4
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (4, 'Productos frescos', 0.10);

-- Registro 5
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (5, 'Procesados', 0.10);

-- Registro 6
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (6, 'Especialidades', 0.10);

-- Registro 7
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (7, 'Congelados', 0.10);

-- Registro 8
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (8, 'Premium', 0.10);

-- Registro 9
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (9, 'Parrilla', 0.10);

-- Registro 10
INSERT INTO tblCategoria (idCategoria, nombreCategoria, porcentajeComisionCategoria)
VALUES (10, 'Deli', 0.10);

-- ============================================================================
-- TABLA: tblEmpresaTransportista
-- ============================================================================
-- Registro 1
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (1, 'Transporte Maya', 'Ana López', '55500001');

-- Registro 2
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (2, 'Carga Chapina', 'Luis Morales', '55500002');

-- Registro 3
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (3, 'Frío Express', 'María García', '55500003');

-- Registro 4
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (4, 'Ruta Segura', 'Carlos Pérez', '55500004');

-- Registro 5
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (5, 'Logística Quetzal', 'Elena Ruiz', '55500005');

-- Registro 6
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (6, 'Transportes del Valle', 'José Ramírez', '55500006');

-- Registro 7
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (7, 'Carga Regional', 'Sofía Herrera', '55500007');

-- Registro 8
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (8, 'Movilidad Centro', 'Pedro Castillo', '55500008');

-- Registro 9
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (9, 'Fletes Progreso', 'Laura Díaz', '55500009');

-- Registro 10
INSERT INTO tblEmpresaTransportista (idEmpresaTransportista, nombreEmpresaTransportista, contactoEmpresaTransportista, telefonoEmpresaTransportista)
VALUES (10, 'Cadena Fría GT', 'Jorge Méndez', '55500010');

-- ============================================================================
-- TABLA: tblEstado
-- ============================================================================
-- Registro 1
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (1, 'ACTIVO');

-- Registro 2
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (2, 'INACTIVO');

-- Registro 3
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (3, 'EN_MANTENIMIENTO');

-- Registro 4
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (4, 'DISPONIBLE');

-- Registro 5
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (5, 'OCUPADO');

-- Registro 6
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (6, 'SUSPENDIDO');

-- Registro 7
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (7, 'EN_RUTA');

-- Registro 8
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (8, 'RESERVADO');

-- Registro 9
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (9, 'REPARACION');

-- Registro 10
INSERT INTO tblEstado (idEstado, tipoEstado)
VALUES (10, 'BAJA');

-- ============================================================================
-- TABLA: tblEstadoSolicitudPago
-- ============================================================================
-- Registro 2
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (2, 'APROBADO', 'Aprobado', 'Estado de pago: APROBADO');

-- Registro 3
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (3, 'RECHAZADO', 'Rechazado', 'Estado de pago: RECHAZADO');

-- Registro 4
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (4, 'PENDIENTE', 'Pendiente', 'Estado de pago: PENDIENTE');

-- Registro 5
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (5, 'PAGADO', 'Pagado', 'Estado de pago: PAGADO');

-- Registro 6
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (6, 'EN_REVISION', 'En Revision', 'Estado de pago: EN_REVISION');

-- Registro 7
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (7, 'PROGRAMADO', 'Programado', 'Estado de pago: PROGRAMADO');

-- Registro 8
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (8, 'CANCELADO', 'Cancelado', 'Estado de pago: CANCELADO');

-- Registro 9
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (9, 'PROCESADO', 'Procesado', 'Estado de pago: PROCESADO');

-- Registro 10
INSERT INTO tblEstadoSolicitudPago (idEstadoSolicitudPago, codigoEstadoSolicitudPago, nombreEstadoSolicitudPago, descripcionEstadoSolicitudPago)
VALUES (10, 'FINALIZADO', 'Finalizado', 'Estado de pago: FINALIZADO');

-- ============================================================================
-- TABLA: tblLinea
-- ============================================================================
-- Registro 1
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (1, 'Tradicional', 0.10);

-- Registro 2
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (2, 'Familiar', 0.10);

-- Registro 3
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (3, 'Premium', 0.10);

-- Registro 4
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (4, 'Económica', 0.10);

-- Registro 5
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (5, 'Selección', 0.10);

-- Registro 6
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (6, 'Delicatessen', 0.10);

-- Registro 7
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (7, 'Parrillera', 0.10);

-- Registro 8
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (8, 'Profesional', 0.10);

-- Registro 9
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (9, 'Especial', 0.10);

-- Registro 10
INSERT INTO tblLinea (idLinea, nombreLinea, porcentajeComisionLinea)
VALUES (10, 'Institucional', 0.10);

-- ============================================================================
-- TABLA: tblMarca
-- ============================================================================
-- Registro 1
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (1, 'La Hacienda', 0.10);

-- Registro 2
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (2, 'Del Campo', 0.10);

-- Registro 3
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (3, 'Buen Sabor', 0.10);

-- Registro 4
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (4, 'La Granja', 0.10);

-- Registro 5
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (5, 'Don Chorizo', 0.10);

-- Registro 6
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (6, 'Rey del Jamón', 0.10);

-- Registro 7
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (7, 'Fresco GT', 0.10);

-- Registro 8
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (8, 'Casa Selecta', 0.10);

-- Registro 9
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (9, 'Monte Verde', 0.10);

-- Registro 10
INSERT INTO tblMarca (idMarca, nombreMarca, porcentajeComisionMarca)
VALUES (10, 'Sabor Real', 0.10);

-- =====================================================
-- INSERTAR 10 REGISTROS EN tblPlazo
-- =====================================================

INSERT INTO tblPlazo (nombrePlazo, diasPlazo) VALUES
('Contado', 0),
('8 días', 8),
('15 días', 15),
('30 días', 30),
('45 días', 45),
('60 días', 60),
('90 días', 90),
('120 días', 120),
('150 días', 150),
('180 días', 180);


-- =====================================================
-- INSERTAR 10 REGISTROS EN tblConceptoProveedor
-- =====================================================

INSERT INTO tblConceptoProveedor (
    nombreConceptoProveedor,
    naturalezaConceptoProveedor
) VALUES
('Compra de mercadería', 'Cargo'),
('Compra de materia prima', 'Cargo'),
('Compra de suministros', 'Cargo'),
('Servicios de transporte', 'Cargo'),
('Servicios de mantenimiento', 'Cargo'),
('Pago a proveedor', 'Abono'),
('Nota de crédito', 'Abono'),
('Devolución de mercadería', 'Abono'),
('Descuento por pronto pago', 'Abono'),
('Ajuste a favor del proveedor', 'Abono');


-- =====================================================
-- INSERTAR 10 REGISTROS EN tblProveedor
-- =====================================================

INSERT INTO tblProveedor (
    nombreProveedor,
    nitProveedor,
    correoProveedor,
    telefonoProveedor,
    direccionProveedor,
    estadoProveedor,
    saldoActualProveedor,
    saldoAnteriorProveedor,
    cargosMesProveedor,
    abonosMesProveedor,
    cargosAsignadosProveedor,
    cargosAcumuladosProveedor,
    abonosAcumuladosProveedor
) VALUES
(
    'Distribuidora del Norte',
    '1234567890',
    'ventas@distribuidoranorte.com',
    '5551-1001',
    'Zona 1, Ciudad de Guatemala',
    1, 15000.0000, 12000.0000, 5000.0000, 2000.0000,
    3000.0000, 25000.0000, 10000.0000
),
(
    'Comercializadora Central',
    '1234567891',
    'contacto@comercialcentral.com',
    '5551-1002',
    'Zona 4, Ciudad de Guatemala',
    1, 8500.0000, 7000.0000, 3000.0000, 1500.0000,
    1500.0000, 18000.0000, 9500.0000
),
(
    'Materias Primas del Sur',
    '1234567892',
    'ventas@materiasprimas.com',
    '5551-1003',
    'Escuintla, Guatemala',
    1, 22000.0000, 18000.0000, 7000.0000, 3000.0000,
    4000.0000, 35000.0000, 13000.0000
),
(
    'Empaques Industriales',
    '1234567893',
    'info@empaquesindustriales.com',
    '5551-1004',
    'Zona 12, Ciudad de Guatemala',
    1, 6500.0000, 5000.0000, 2500.0000, 1000.0000,
    1500.0000, 12000.0000, 5500.0000
),
(
    'Transportes Express',
    '1234567894',
    'servicios@transportesexpress.com',
    '5551-1005',
    'Villa Nueva, Guatemala',
    1, 4500.0000, 4000.0000, 1800.0000, 1300.0000,
    500.0000, 9000.0000, 4500.0000
),
(
    'Tecnología Empresarial',
    '1234567895',
    'ventas@tecnologiaempresarial.com',
    '5551-1006',
    'Zona 10, Ciudad de Guatemala',
    1, 12000.0000, 10000.0000, 4000.0000, 2000.0000,
    2000.0000, 20000.0000, 8000.0000
),
(
    'Suministros Industriales',
    '1234567896',
    'contacto@suministrosindustriales.com',
    '5551-1007',
    'Mixco, Guatemala',
    1, 7800.0000, 6000.0000, 2800.0000, 1000.0000,
    1800.0000, 15000.0000, 7200.0000
),
(
    'Repuestos y Servicios',
    '1234567897',
    'ventas@repuestosyservicios.com',
    '5551-1008',
    'Zona 7, Ciudad de Guatemala',
    1, 3200.0000, 2500.0000, 1200.0000, 500.0000,
    700.0000, 8000.0000, 4800.0000
),
(
    'Productos Alimenticios',
    '1234567898',
    'pedidos@productosalimenticios.com',
    '5551-1009',
    'Chimaltenango, Guatemala',
    1, 18500.0000, 15000.0000, 6000.0000, 2500.0000,
    3500.0000, 28000.0000, 9500.0000
),
(
    'Servicios Generales GT',
    '1234567899',
    'info@serviciosgeneralesgt.com',
    '5551-1010',
    'Zona 3, Ciudad de Guatemala',
    0, 0.0000, 0.0000, 0.0000, 0.0000,
    0.0000, 0.0000, 0.0000
);
-- ============================================================================
-- TABLA: tblSucursal
-- ============================================================================
-- Registro 1
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (1, 'Sucursal Central', 'Descripcion de prueba 1 - tblSucursal', 'Zona 1, Ciudad de Guatemala', 'Guatemala');

-- Registro 2
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (2, 'Sucursal Norte', 'Descripcion de prueba 2 - tblSucursal', 'Zona 2, Ciudad de Guatemala', 'Guatemala');

-- Registro 3
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (3, 'Sucursal Sur', 'Descripcion de prueba 3 - tblSucursal', 'Zona 3, Ciudad de Guatemala', 'Guatemala');

-- Registro 4
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (4, 'Sucursal Occidente', 'Descripcion de prueba 4 - tblSucursal', 'Zona 4, Ciudad de Guatemala', 'Guatemala');

-- Registro 5
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (5, 'Sucursal Oriente', 'Descripcion de prueba 5 - tblSucursal', 'Zona 5, Ciudad de Guatemala', 'Guatemala');

-- Registro 6
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (6, 'Sucursal Metropolitana', 'Descripcion de prueba 6 - tblSucursal', 'Zona 6, Ciudad de Guatemala', 'Guatemala');

-- Registro 7
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (7, 'Sucursal Carretera', 'Descripcion de prueba 7 - tblSucursal', 'Zona 7, Ciudad de Guatemala', 'Guatemala');

-- Registro 8
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (8, 'Sucursal Industrial', 'Descripcion de prueba 8 - tblSucursal', 'Zona 8, Ciudad de Guatemala', 'Guatemala');

-- Registro 9
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (9, 'Sucursal Zona 10', 'Descripcion de prueba 9 - tblSucursal', 'Zona 9, Ciudad de Guatemala', 'Guatemala');

-- Registro 10
INSERT INTO tblSucursal (idSucursal, nombreSucursal, descripcionSucursal, ubicacionSucursal, paisSucursal)
VALUES (10, 'Sucursal Zona 12', 'Descripcion de prueba 10 - tblSucursal', 'Zona 10, Ciudad de Guatemala', 'Guatemala');

-- ============================================================================
-- TABLA: tblTarifaTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (1, 'Tarifa 01 - regional');

-- Registro 2
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (2, 'Tarifa 02 - refrigerado');

-- Registro 3
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (3, 'Tarifa 03 - local');

-- Registro 4
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (4, 'Tarifa 04 - regional');

-- Registro 5
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (5, 'Tarifa 05 - refrigerado');

-- Registro 6
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (6, 'Tarifa 06 - local');

-- Registro 7
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (7, 'Tarifa 07 - regional');

-- Registro 8
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (8, 'Tarifa 08 - refrigerado');

-- Registro 9
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (9, 'Tarifa 09 - local');

-- Registro 10
INSERT INTO tblTarifaTransporte (idTarifaTransporte, nombreTarifaTransporte)
VALUES (10, 'Tarifa 10 - regional');

-- ============================================================================
-- TABLA: tblTipoMovimiento
-- ============================================================================
-- Registro 1
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (1, 'Compra', 'Descripcion de prueba 1 - tblTipoMovimiento', '+');

-- Registro 2
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (2, 'Venta', 'Descripcion de prueba 2 - tblTipoMovimiento', '-');

-- Registro 3
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (3, 'Devolución compra', 'Descripcion de prueba 3 - tblTipoMovimiento', '-');

-- Registro 4
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (4, 'Devolución venta', 'Descripcion de prueba 4 - tblTipoMovimiento', '+');

-- Registro 5
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (5, 'Entrada por ajuste', 'Descripcion de prueba 5 - tblTipoMovimiento', '+');

-- Registro 6
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (6, 'Salida por ajuste', 'Descripcion de prueba 6 - tblTipoMovimiento', '-');

-- Registro 7
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (7, 'Entrada por traslado', 'Descripcion de prueba 7 - tblTipoMovimiento', '+');

-- Registro 8
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (8, 'Salida por traslado', 'Descripcion de prueba 8 - tblTipoMovimiento', '-');

-- Registro 9
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (9, 'Ajuste positivo', 'Descripcion de prueba 9 - tblTipoMovimiento', '+');

-- Registro 10
INSERT INTO tblTipoMovimiento (idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento)
VALUES (10, 'Ajuste negativo', 'Descripcion de prueba 10 - tblTipoMovimiento', '-');

-- ============================================================================
-- TABLA: tblTipoServicio
-- ============================================================================
-- Registro 1
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (1, 'Carga refrigerada');

-- Registro 2
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (2, 'Carga congelada');

-- Registro 3
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (3, 'Carga seca');

-- Registro 4
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (4, 'Distribución');

-- Registro 5
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (5, 'Entrega urbana');

-- Registro 6
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (6, 'Carga pesada');

-- Registro 7
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (7, 'Transporte dedicado');

-- Registro 8
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (8, 'Reparto local');

-- Registro 9
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (9, 'Carga consolidada');

-- Registro 10
INSERT INTO tblTipoServicio (idTipoServicio, nombreTipoServicio)
VALUES (10, 'Ruta especial');

-- ============================================================================
-- TABLA: tblTipoTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (1, 'Camión');

-- Registro 2
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (2, 'Camioneta');

-- Registro 3
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (3, 'Furgón');

-- Registro 4
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (4, 'Pick-up');

-- Registro 5
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (5, 'Panel');

-- Registro 6
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (6, 'Tráiler');

-- Registro 7
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (7, 'Cabezal');

-- Registro 8
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (8, 'Motocicleta');

-- Registro 9
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (9, 'Microbús');

-- Registro 10
INSERT INTO tblTipoTransporte (idTipoTransporte, nombreTipoTransporte)
VALUES (10, 'Camión refrigerado');

-- ============================================================================
-- TABLA: tblBodega
-- ============================================================================
-- Registro 1
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (1, 'Bodega Central', 1, 'Guatemala');

-- Registro 2
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (2, 'Bodega Norte', 2, 'Guatemala');

-- Registro 3
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (3, 'Bodega Sur', 3, 'Guatemala');

-- Registro 4
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (4, 'Bodega Occidente', 4, 'Guatemala');

-- Registro 5
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (5, 'Bodega Oriente', 5, 'Guatemala');

-- Registro 6
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (6, 'Bodega Metropolitana', 6, 'Guatemala');

-- Registro 7
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (7, 'Bodega Carretera', 7, 'Guatemala');

-- Registro 8
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (8, 'Bodega Industrial', 8, 'Guatemala');

-- Registro 9
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (9, 'Bodega Zona 10', 9, 'Guatemala');

-- Registro 10
INSERT INTO tblBodega (idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega)
VALUES (10, 'Bodega Zona 12', 10, 'Guatemala');

-- ============================================================================
-- TABLA: tblCierreMovimiento
-- ============================================================================
-- Registro 1
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (1, 'Cierre octubre 1', 1, '2026-01-01', '2026-01-31', 'CERRADO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 1', '2026-11-01 10:00:00');

-- Registro 2
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (2, 'Cierre octubre 2', 2, '2026-02-01', '2026-02-28', 'ABIERTO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 2', NULL);

-- Registro 3
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (3, 'Cierre octubre 3', 3, '2026-03-01', '2026-03-31', 'CERRADO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 3', '2026-11-01 10:00:00');

-- Registro 4
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (4, 'Cierre octubre 4', 4, '2026-04-01', '2026-04-30', 'ABIERTO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 4', NULL);

-- Registro 5
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (5, 'Cierre octubre 5', 5, '2026-05-01', '2026-05-31', 'CERRADO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 5', '2026-11-01 10:00:00');

-- Registro 6
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (6, 'Cierre octubre 6', 6, '2026-06-01', '2026-06-30', 'ABIERTO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 6', NULL);

-- Registro 7
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (7, 'Cierre octubre 7', 7, '2026-07-01', '2026-07-31', 'CERRADO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 7', '2026-11-01 10:00:00');

-- Registro 8
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (8, 'Cierre octubre 8', 8, '2026-08-01', '2026-08-31', 'ABIERTO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 8', NULL);

-- Registro 9
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (9, 'Cierre octubre 9', 9, '2026-09-01', '2026-09-30', 'CERRADO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 9', '2026-11-01 10:00:00');

-- Registro 10
INSERT INTO tblCierreMovimiento (idCierreMovimiento, nombreCierreMovimiento, Fk_Sucursal_Codigo, fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento, esCierreAnualCierreMovimiento, toleranciaCierreMovimiento, diferenciaUnidadesCierreMovimiento, diferenciaMontoCierreMovimiento, motivoAceptacionCierreMovimiento, observacionesCierreMovimiento, fechaEjecucionCierreMovimiento)
VALUES (10, 'Cierre octubre 10', 10, '2026-10-01', '2026-10-31', 'ABIERTO', 0, 0, 0, 0.0000, 'Registro de prueba para validación', 'Cierre de prueba 10', NULL);

-- ============================================================================
-- TABLA: tblConductor
-- ============================================================================
-- Registro 1
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (1, 'COND-001', 'Ana López', 'LIC-0001', '2028-12-31', 1);

-- Registro 2
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (2, 'COND-002', 'Luis Morales', 'LIC-0002', '2028-12-31', 2);

-- Registro 3
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (3, 'COND-003', 'María García', 'LIC-0003', '2028-12-31', 3);

-- Registro 4
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (4, 'COND-004', 'Carlos Pérez', 'LIC-0004', '2028-12-31', 4);

-- Registro 5
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (5, 'COND-005', 'Elena Ruiz', 'LIC-0005', '2028-12-31', 5);

-- Registro 6
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (6, 'COND-006', 'José Ramírez', 'LIC-0006', '2028-12-31', 6);

-- Registro 7
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (7, 'COND-007', 'Sofía Herrera', 'LIC-0007', '2028-12-31', 7);

-- Registro 8
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (8, 'COND-008', 'Pedro Castillo', 'LIC-0008', '2028-12-31', 8);

-- Registro 9
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (9, 'COND-009', 'Laura Díaz', 'LIC-0009', '2028-12-31', 9);

-- Registro 10
INSERT INTO tblConductor (idConductor, codigoConductor, nombreCompletoConductor, numeroLicenciaConductor, fechaVencimientoDocConductor, Fk_Estado_Codigo)
VALUES (10, 'COND-010', 'Jorge Méndez', 'LIC-0010', '2028-12-31', 10);

-- ============================================================================
-- TABLA: tblProducto
-- ============================================================================
-- Registro 1
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (1, 'EMB-0001', 'EMB01', 'Jamón de pavo', 1, 1, 1, 1, 'Descripcion de prueba 1 - tblProducto', 'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 2
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (2, 'EMB-0002', 'EMB01', 'Salchicha tipo viena', 2, 2, 2, 2, 'Descripcion de prueba 2 - tblProducto', 'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 3
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (3, 'EMB-0003', 'EMB01', 'Chorizo tradicional', 3, 3, 3, 3, 'Descripcion de prueba 3 - tblProducto', 'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 4
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (4, 'EMB-0004', 'EMB01', 'Tocino ahumado', 4, 4, 4, 4, 'Descripcion de prueba 4 - tblProducto', 'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 5
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (5, 'EMB-0005', 'EMB01', 'Mortadela especial', 5, 5, 5, 5, 'Descripcion de prueba 5 - tblProducto', 'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 6
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (6, 'EMB-0006', 'EMB01', 'Salami premium', 6, 6, 6, 6, 'Descripcion de prueba 6 - tblProducto', 'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 7
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (7, 'EMB-0007', 'EMB01', 'Jamón cocido', 7, 7, 7, 7, 'Descripcion de prueba 7 - tblProducto', 'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 8
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (8, 'EMB-0008', 'EMB01', 'Longaniza fresca', 8, 8, 8, 8, 'Descripcion de prueba 8 - tblProducto', 'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 9
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (9, 'EMB-0009', 'EMB01', 'Salchicha de pollo', 9, 9, 9, 9, 'Descripcion de prueba 9 - tblProducto', 'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- Registro 10
INSERT INTO tblProducto (idProducto, skuProducto, clasificacionProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
VALUES (10, 'EMB-0010', 'EMB01', 'Chorizo parrillero', 10, 10, 10, 10, 'Descripcion de prueba 10 - tblProducto', 'FFD8FF', 'Guatemala', 25.0000, 45.0000, 0.10, 5, 500, 1.50);

-- ============================================================================
-- TABLA: tblTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (1, 'P001ABC', 'Toyota', 'Modelo 2021', 2017, 1, 1, 1);

-- Registro 2
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (2, 'P002ABC', 'Isuzu', 'Modelo 2022', 2018, 0, 2, 2);

-- Registro 3
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (3, 'P003ABC', 'Toyota', 'Modelo 2023', 2019, 1, 3, 3);

-- Registro 4
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (4, 'P004ABC', 'Isuzu', 'Modelo 2024', 2020, 0, 4, 4);

-- Registro 5
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (5, 'P005ABC', 'Toyota', 'Modelo 2025', 2021, 1, 5, 5);

-- Registro 6
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (6, 'P006ABC', 'Isuzu', 'Modelo 2020', 2022, 0, 6, 6);

-- Registro 7
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (7, 'P007ABC', 'Toyota', 'Modelo 2021', 2023, 1, 7, 7);

-- Registro 8
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (8, 'P008ABC', 'Isuzu', 'Modelo 2022', 2024, 0, 8, 8);

-- Registro 9
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (9, 'P009ABC', 'Toyota', 'Modelo 2023', 2025, 1, 9, 9);

-- Registro 10
INSERT INTO tblTransporte (idTransporte, nroPlacaTransporte, marcaTransporte, modeloTransporte, anioTransporte, esRefrigeradoTransporte, Fk_Estado_Codigo, Fk_Tipo_Transporte_Codigo)
VALUES (10, 'P010ABC', 'Isuzu', 'Modelo 2024', 2026, 0, 10, 10);

-- ============================================================================
-- TABLA: tblCamaraRefrigeracion
-- ============================================================================
-- Registro 1
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (1, 1, 'ThermoKing 1', -18.00, 4.00, 'OPERATIVO');

-- Registro 2
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (2, 2, 'ThermoKing 2', -18.00, 4.00, 'OPERATIVO');

-- Registro 3
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (3, 3, 'ThermoKing 3', -18.00, 4.00, 'OPERATIVO');

-- Registro 4
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (4, 4, 'ThermoKing 4', -18.00, 4.00, 'OPERATIVO');

-- Registro 5
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (5, 5, 'ThermoKing 5', -18.00, 4.00, 'OPERATIVO');

-- Registro 6
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (6, 6, 'ThermoKing 6', -18.00, 4.00, 'OPERATIVO');

-- Registro 7
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (7, 7, 'ThermoKing 7', -18.00, 4.00, 'OPERATIVO');

-- Registro 8
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (8, 8, 'ThermoKing 8', -18.00, 4.00, 'OPERATIVO');

-- Registro 9
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (9, 9, 'ThermoKing 9', -18.00, 4.00, 'OPERATIVO');

-- Registro 10
INSERT INTO tblCamaraRefrigeracion (idCamaraRefrigeracion, Fk_Transporte_Codigo, marcaEquipoCamaraRefrigeracion, tempMinimaCelsiusCamaraRefrigeracion, tempMaximaCelsiusCamaraRefrigeracion, estadoOperativoCamaraRefrigeracion)
VALUES (10, 10, 'ThermoKing 10', -18.00, 4.00, 'OPERATIVO');

-- ============================================================================
-- TABLA: tblEncabezadoViaje
-- ============================================================================
-- Registro 1
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (1, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 1, 'EN_CURSO', 150.000, 1, 1);

-- Registro 2
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (2, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 0, 'EN_CURSO', 150.000, 2, 2);

-- Registro 3
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (3, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 1, 'EN_CURSO', 150.000, 3, 3);

-- Registro 4
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (4, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 0, 'EN_CURSO', 150.000, 4, 4);

-- Registro 5
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (5, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 1, 'EN_CURSO', 150.000, 5, 5);

-- Registro 6
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (6, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 0, 'EN_CURSO', 150.000, 6, 6);

-- Registro 7
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (7, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 1, 'EN_CURSO', 150.000, 7, 7);

-- Registro 8
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (8, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 0, 'EN_CURSO', 150.000, 8, 8);

-- Registro 9
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (9, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 1, 'EN_CURSO', 150.000, 9, 9);

-- Registro 10
INSERT INTO tblEncabezadoViaje (idEncabezadoViaje, fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, horaLlegadaEncabezadoViaje, requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo)
VALUES (10, '2026-10-15 07:30:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 0, 'EN_CURSO', 150.000, 10, 10);

-- ============================================================================
-- TABLA: tblLoteProducto
-- ============================================================================
-- Registro 1
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (1, 'LOTE-0001', 1, 'Descripcion de prueba 1 - tblLoteProducto', 1, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 2
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (2, 'LOTE-0002', 2, 'Descripcion de prueba 2 - tblLoteProducto', 0, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 3
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (3, 'LOTE-0003', 3, 'Descripcion de prueba 3 - tblLoteProducto', 1, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 4
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (4, 'LOTE-0004', 4, 'Descripcion de prueba 4 - tblLoteProducto', 0, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 5
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (5, 'LOTE-0005', 5, 'Descripcion de prueba 5 - tblLoteProducto', 1, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 6
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (6, 'LOTE-0006', 6, 'Descripcion de prueba 6 - tblLoteProducto', 0, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 7
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (7, 'LOTE-0007', 7, 'Descripcion de prueba 7 - tblLoteProducto', 1, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 8
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (8, 'LOTE-0008', 8, 'Descripcion de prueba 8 - tblLoteProducto', 0, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 9
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (9, 'LOTE-0009', 9, 'Descripcion de prueba 9 - tblLoteProducto', 1, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- Registro 10
INSERT INTO tblLoteProducto (idLoteProducto, codigoLoteProducto, Fk_Producto_Codigo, descripcionLoteProducto, requiereFrioLoteProducto, fechaFabricacionLoteProducto, fechaVencimientoLoteProducto, tempRequeridaMinLoteProducto, tempRequeridaMaxLoteProducto)
VALUES (10, 'LOTE-0010', 10, 'Descripcion de prueba 10 - tblLoteProducto', 0, '2026-09-01', '2027-09-01', -18.00, 4.00);

-- ============================================================================
-- TABLA: tblSaldos
-- ============================================================================
-- Registro 1
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (1, 1, 1, 101);

-- Registro 2
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (2, 2, 2, 102);

-- Registro 3
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (3, 3, 3, 103);

-- Registro 4
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (4, 4, 4, 104);

-- Registro 5
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (5, 5, 5, 105);

-- Registro 6
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (6, 6, 6, 106);

-- Registro 7
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (7, 7, 7, 107);

-- Registro 8
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (8, 8, 8, 108);

-- Registro 9
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (9, 9, 9, 109);

-- Registro 10
INSERT INTO tblSaldos (idSaldo, Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo)
VALUES (10, 10, 10, 110);

-- ============================================================================
-- TABLA: tblStock
-- ============================================================================
-- Registro 1
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (1, 1, 1, 1, 101, 101, 91, 101, 15, 5, 50, 20, 2, 1, 1);

-- Registro 2
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (2, 2, 2, 2, 102, 102, 92, 102, 15, 5, 50, 20, 2, 1, 1);

-- Registro 3
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (3, 3, 3, 3, 103, 103, 93, 103, 15, 5, 50, 20, 2, 1, 1);

-- Registro 4
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (4, 4, 4, 4, 104, 104, 94, 104, 15, 5, 50, 20, 2, 1, 1);

-- Registro 5
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (5, 5, 5, 5, 105, 105, 95, 105, 15, 5, 50, 20, 2, 1, 1);

-- Registro 6
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (6, 6, 6, 6, 106, 106, 96, 106, 15, 5, 50, 20, 2, 1, 1);

-- Registro 7
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (7, 7, 7, 7, 107, 107, 97, 107, 15, 5, 50, 20, 2, 1, 1);

-- Registro 8
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (8, 8, 8, 8, 108, 108, 98, 108, 15, 5, 50, 20, 2, 1, 1);

-- Registro 9
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (9, 9, 9, 9, 109, 109, 99, 109, 15, 5, 50, 20, 2, 1, 1);

-- Registro 10
INSERT INTO tblStock (idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock)
VALUES (10, 10, 10, 10, 110, 110, 100, 110, 15, 5, 50, 20, 2, 1, 1);

-- ============================================================================
-- TABLA: tblTraslado
-- ============================================================================
-- Registro 1
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (1, 'TR-0001', 1, 2, 1, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'SOLICITADO', 'Traslado de prueba 1');

-- Registro 2
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (2, 'TR-0002', 2, 3, 2, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'EN_TRANSITO', 'Traslado de prueba 2');

-- Registro 3
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (3, 'TR-0003', 3, 4, 3, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'SOLICITADO', 'Traslado de prueba 3');

-- Registro 4
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (4, 'TR-0004', 4, 5, 4, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'EN_TRANSITO', 'Traslado de prueba 4');

-- Registro 5
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (5, 'TR-0005', 5, 6, 5, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'SOLICITADO', 'Traslado de prueba 5');

-- Registro 6
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (6, 'TR-0006', 6, 7, 6, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'EN_TRANSITO', 'Traslado de prueba 6');

-- Registro 7
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (7, 'TR-0007', 7, 8, 7, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'SOLICITADO', 'Traslado de prueba 7');

-- Registro 8
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (8, 'TR-0008', 8, 9, 8, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'EN_TRANSITO', 'Traslado de prueba 8');

-- Registro 9
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (9, 'TR-0009', 9, 10, 9, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'SOLICITADO', 'Traslado de prueba 9');

-- Registro 10
INSERT INTO tblTraslado (idTraslado, codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, fechaSolicitudTraslado, fechaSalidaTraslado, fechaLlegadaTraslado, estadoTraslado, observacionesTraslado)
VALUES (10, 'TR-0010', 10, 1, 10, '2026-10-14 12:00:00', '2026-10-15 08:00:00', '2026-10-15 16:00:00', 'EN_TRANSITO', 'Traslado de prueba 10');

-- ============================================================================
-- TABLA: tblDetalleCierre
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (1, 1, 1, 91, 101, 15, 5, 50, 20);

-- Registro 2
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (2, 2, 2, 92, 102, 15, 5, 50, 20);

-- Registro 3
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (3, 3, 3, 93, 103, 15, 5, 50, 20);

-- Registro 4
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (4, 4, 4, 94, 104, 15, 5, 50, 20);

-- Registro 5
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (5, 5, 5, 95, 105, 15, 5, 50, 20);

-- Registro 6
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (6, 6, 6, 96, 106, 15, 5, 50, 20);

-- Registro 7
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (7, 7, 7, 97, 107, 15, 5, 50, 20);

-- Registro 8
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (8, 8, 8, 98, 108, 15, 5, 50, 20);

-- Registro 9
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (9, 9, 9, 99, 109, 15, 5, 50, 20);

-- Registro 10
INSERT INTO tblDetalleCierre (idDetalleCierre, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, stockAnteriorDetalleCierre, stockActualDetalleCierre, cargosMesDetalleCierre, abonosMesDetalleCierre, cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
VALUES (10, 10, 10, 100, 110, 15, 5, 50, 20);

-- ============================================================================
-- TABLA: tblDetalleTraslado
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (1, 1, 1, 6);

-- Registro 2
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (2, 2, 2, 7);

-- Registro 3
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (3, 3, 3, 8);

-- Registro 4
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (4, 4, 4, 9);

-- Registro 5
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (5, 5, 5, 10);

-- Registro 6
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (6, 6, 6, 11);

-- Registro 7
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (7, 7, 7, 12);

-- Registro 8
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (8, 8, 8, 13);

-- Registro 9
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (9, 9, 9, 14);

-- Registro 10
INSERT INTO tblDetalleTraslado (idDetalleTraslado, Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado)
VALUES (10, 10, 10, 15);

-- ============================================================================
-- TABLA: tblDetalleViaje
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (1, 1, 1, 'Descripcion de prueba 1 - tblDetalleViaje', 6, 12.500);

-- Registro 2
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (2, 2, 2, 'Descripcion de prueba 2 - tblDetalleViaje', 7, 12.500);

-- Registro 3
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (3, 3, 3, 'Descripcion de prueba 3 - tblDetalleViaje', 8, 12.500);

-- Registro 4
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (4, 4, 4, 'Descripcion de prueba 4 - tblDetalleViaje', 9, 12.500);

-- Registro 5
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (5, 5, 5, 'Descripcion de prueba 5 - tblDetalleViaje', 10, 12.500);

-- Registro 6
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (6, 6, 6, 'Descripcion de prueba 6 - tblDetalleViaje', 11, 12.500);

-- Registro 7
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (7, 7, 7, 'Descripcion de prueba 7 - tblDetalleViaje', 12, 12.500);

-- Registro 8
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (8, 8, 8, 'Descripcion de prueba 8 - tblDetalleViaje', 13, 12.500);

-- Registro 9
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (9, 9, 9, 'Descripcion de prueba 9 - tblDetalleViaje', 14, 12.500);

-- Registro 10
INSERT INTO tblDetalleViaje (idDetalleViaje, Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje)
VALUES (10, 10, 10, 'Descripcion de prueba 10 - tblDetalleViaje', 15, 12.500);

-- ============================================================================
-- TABLA: tblHistoricoInventario
-- ============================================================================
-- Registro 1
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (1, 1, 1, 1, 1, 1, 101, 101, 91, 101, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 2
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (2, 2, 2, 2, 2, 2, 102, 102, 92, 102, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 3
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (3, 3, 3, 3, 3, 3, 103, 103, 93, 103, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 4
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (4, 4, 4, 4, 4, 4, 104, 104, 94, 104, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 5
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (5, 5, 5, 5, 5, 5, 105, 105, 95, 105, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 6
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (6, 6, 6, 6, 6, 6, 106, 106, 96, 106, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 7
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (7, 7, 7, 7, 7, 7, 107, 107, 97, 107, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 8
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (8, 8, 8, 8, 8, 8, 108, 108, 98, 108, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 9
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (9, 9, 9, 9, 9, 9, 109, 109, 99, 109, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- Registro 10
INSERT INTO tblHistoricoInventario (idHistoricoInventario, Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, apartadosStock, estadoVentaStock, estadoCompraStock, fechaCorteHistoricoInventario)
VALUES (10, 10, 10, 10, 10, 10, 110, 110, 100, 110, 15, 5, 50, 20, 2, 1, 1, '2026-10-31');

-- ============================================================================
-- TABLA: tblMovimientoInventario
-- ============================================================================
-- Registro 1
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (1, 1, 1, 2, 1, 1, '2026-10-02 10:30:00', 'Movimiento de prueba 1', 'CUADRADO', 1);

-- Registro 2
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (2, 2, 2, 3, 2, 2, '2026-10-03 10:30:00', 'Movimiento de prueba 2', 'PENDIENTE', 2);

-- Registro 3
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (3, 3, 3, 4, 3, 3, '2026-10-04 10:30:00', 'Movimiento de prueba 3', 'CUADRADO', 3);

-- Registro 4
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (4, 4, 4, 5, 4, 4, '2026-10-05 10:30:00', 'Movimiento de prueba 4', 'PENDIENTE', 4);

-- Registro 5
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (5, 5, 5, 6, 5, 5, '2026-10-06 10:30:00', 'Movimiento de prueba 5', 'CUADRADO', 5);

-- Registro 6
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (6, 6, 6, 7, 6, 6, '2026-10-07 10:30:00', 'Movimiento de prueba 6', 'PENDIENTE', 6);

-- Registro 7
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (7, 7, 7, 8, 7, 7, '2026-10-08 10:30:00', 'Movimiento de prueba 7', 'CUADRADO', 7);

-- Registro 8
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (8, 8, 8, 9, 8, 8, '2026-10-09 10:30:00', 'Movimiento de prueba 8', 'PENDIENTE', 8);

-- Registro 9
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (9, 9, 9, 10, 9, 9, '2026-10-10 10:30:00', 'Movimiento de prueba 9', 'CUADRADO', 9);

-- Registro 10
INSERT INTO tblMovimientoInventario (idMovimientoInventario, Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, observacionesMovimientoInventario, estadoMovimientoInventario, Fk_Cierre_Movimiento_Codigo)
VALUES (10, 10, 10, 1, 10, 10, '2026-10-11 10:30:00', 'Movimiento de prueba 10', 'PENDIENTE', 10);

-- ============================================================================
-- TABLA: tblServicioTransporteExterno
-- ============================================================================
-- Registro 1
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (1, 1, 1, 1, 'GUIA-0001', 'Programado', 'Sucursal Central', 'Zona 1, Ciudad de Guatemala', 'Sucursal Norte', 'Zona 1, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 2
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (2, 2, 2, 2, 'GUIA-0002', 'Entregado', 'Sucursal Norte', 'Zona 2, Ciudad de Guatemala', 'Sucursal Sur', 'Zona 2, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 3
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (3, 3, 3, 3, 'GUIA-0003', 'Programado', 'Sucursal Sur', 'Zona 3, Ciudad de Guatemala', 'Sucursal Occidente', 'Zona 3, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 4
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (4, 4, 4, 4, 'GUIA-0004', 'Entregado', 'Sucursal Occidente', 'Zona 4, Ciudad de Guatemala', 'Sucursal Oriente', 'Zona 4, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 5
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (5, 5, 5, 5, 'GUIA-0005', 'Programado', 'Sucursal Oriente', 'Zona 5, Ciudad de Guatemala', 'Sucursal Metropolitana', 'Zona 5, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 6
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (6, 6, 6, 6, 'GUIA-0006', 'Entregado', 'Sucursal Metropolitana', 'Zona 6, Ciudad de Guatemala', 'Sucursal Carretera', 'Zona 6, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 7
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (7, 7, 7, 7, 'GUIA-0007', 'Programado', 'Sucursal Carretera', 'Zona 7, Ciudad de Guatemala', 'Sucursal Industrial', 'Zona 7, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 8
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (8, 8, 8, 8, 'GUIA-0008', 'Entregado', 'Sucursal Industrial', 'Zona 8, Ciudad de Guatemala', 'Sucursal Zona 10', 'Zona 8, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 9
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (9, 9, 9, 9, 'GUIA-0009', 'Programado', 'Sucursal Zona 10', 'Zona 9, Ciudad de Guatemala', 'Sucursal Zona 12', 'Zona 9, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- Registro 10
INSERT INTO tblServicioTransporteExterno (idServicioTransporteExterno, Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo, numeroGuiaServicioTransporteExterno, estadoServicioTransporteExterno, origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno, destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno, fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno, fechaEntregaRealServicioTransporteExterno)
VALUES (10, 10, 10, 10, 'GUIA-0010', 'Entregado', 'Sucursal Zona 12', 'Zona 10, Ciudad de Guatemala', 'Sucursal Central', 'Zona 10, Ciudad de Guatemala', '2026-10-01', '2026-10-15', '2026-10-14');

-- ============================================================================
-- TABLA: tblTelemetriaTemperatura
-- ============================================================================
-- Registro 1
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (1, 1, '2026-10-02 10:30:00', 2.50, 0.6500, 1, '14.6001, -90.5001');

-- Registro 2
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (2, 2, '2026-10-03 10:30:00', 2.50, 0.6500, 0, '14.6002, -90.5002');

-- Registro 3
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (3, 3, '2026-10-04 10:30:00', 2.50, 0.6500, 1, '14.6003, -90.5003');

-- Registro 4
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (4, 4, '2026-10-05 10:30:00', 2.50, 0.6500, 0, '14.6004, -90.5004');

-- Registro 5
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (5, 5, '2026-10-06 10:30:00', 2.50, 0.6500, 1, '14.6005, -90.5005');

-- Registro 6
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (6, 6, '2026-10-07 10:30:00', 2.50, 0.6500, 0, '14.6006, -90.5006');

-- Registro 7
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (7, 7, '2026-10-08 10:30:00', 2.50, 0.6500, 1, '14.6007, -90.5007');

-- Registro 8
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (8, 8, '2026-10-09 10:30:00', 2.50, 0.6500, 0, '14.6008, -90.5008');

-- Registro 9
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (9, 9, '2026-10-10 10:30:00', 2.50, 0.6500, 1, '14.6009, -90.5009');

-- Registro 10
INSERT INTO tblTelemetriaTemperatura (idTelemetriaTemperatura, Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura, temperaturaCelsiusTelemetriaTemperatura, humedadRelativaTelemetriaTemperatura, puertaAbiertaTelemetriaTemperatura, ubicacionGpsTelemetriaTemperatura)
VALUES (10, 10, '2026-10-11 10:30:00', 2.50, 0.6500, 0, '14.6010, -90.5010');

-- ============================================================================
-- TABLA: tblAlertaCadenaFrio
-- ============================================================================
-- Registro 1
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (1, 1, 'TEMPERATURA_ALTA', 1, 'Alerta revisada para el viaje 1');

-- Registro 2
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (2, 2, 'PUERTA_ABIERTA', 0, 'Alerta revisada para el viaje 2');

-- Registro 3
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (3, 3, 'TEMPERATURA_ALTA', 1, 'Alerta revisada para el viaje 3');

-- Registro 4
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (4, 4, 'PUERTA_ABIERTA', 0, 'Alerta revisada para el viaje 4');

-- Registro 5
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (5, 5, 'TEMPERATURA_ALTA', 1, 'Alerta revisada para el viaje 5');

-- Registro 6
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (6, 6, 'PUERTA_ABIERTA', 0, 'Alerta revisada para el viaje 6');

-- Registro 7
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (7, 7, 'TEMPERATURA_ALTA', 1, 'Alerta revisada para el viaje 7');

-- Registro 8
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (8, 8, 'PUERTA_ABIERTA', 0, 'Alerta revisada para el viaje 8');

-- Registro 9
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (9, 9, 'TEMPERATURA_ALTA', 1, 'Alerta revisada para el viaje 9');

-- Registro 10
INSERT INTO tblAlertaCadenaFrio (idAlertaCadenaFrio, Fk_Telemetria_Temperatura_Codigo, tipoAlertaCadenaFrio, atendidaAlertaCadenaFrio, observacionCalidadAlertaCadenaFrio)
VALUES (10, 10, 'PUERTA_ABIERTA', 0, 'Alerta revisada para el viaje 10');

-- ============================================================================
-- TABLA: tblAuditoriaInventario
-- ============================================================================
-- Registro 1
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (1, 1, 1, 'INDIVIDUAL', 'FINALIZADA', '2026-10-02 10:30:00', 'Sin diferencias', 'Conteo de verificación de inventario 1');

-- Registro 2
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (2, 2, 2, 'MULTIPLE', 'EN_PROCESO', '2026-10-03 10:30:00', 'Diferencia detectada', 'Conteo de verificación de inventario 2');

-- Registro 3
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (3, 3, 3, 'INDIVIDUAL', 'FINALIZADA', '2026-10-04 10:30:00', 'Sin diferencias', 'Conteo de verificación de inventario 3');

-- Registro 4
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (4, 4, 4, 'MULTIPLE', 'EN_PROCESO', '2026-10-05 10:30:00', 'Diferencia detectada', 'Conteo de verificación de inventario 4');

-- Registro 5
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (5, 5, 5, 'INDIVIDUAL', 'FINALIZADA', '2026-10-06 10:30:00', 'Sin diferencias', 'Conteo de verificación de inventario 5');

-- Registro 6
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (6, 6, 6, 'MULTIPLE', 'EN_PROCESO', '2026-10-07 10:30:00', 'Diferencia detectada', 'Conteo de verificación de inventario 6');

-- Registro 7
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (7, 7, 7, 'INDIVIDUAL', 'FINALIZADA', '2026-10-08 10:30:00', 'Sin diferencias', 'Conteo de verificación de inventario 7');

-- Registro 8
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (8, 8, 8, 'MULTIPLE', 'EN_PROCESO', '2026-10-09 10:30:00', 'Diferencia detectada', 'Conteo de verificación de inventario 8');

-- Registro 9
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (9, 9, 9, 'INDIVIDUAL', 'FINALIZADA', '2026-10-10 10:30:00', 'Sin diferencias', 'Conteo de verificación de inventario 9');

-- Registro 10
INSERT INTO tblAuditoriaInventario (idAuditoriaInventario, Fk_Movimiento_Inventario_Codigo, Fk_Bodega_Codigo, tipoAuditoriaInventario, estadoAuditoriaInventario, fechaAuditoriaInventario, resultadoAuditoriaInventario, observacionesCalidadAuditoriaInventario)
VALUES (10, 10, 10, 'MULTIPLE', 'EN_PROCESO', '2026-10-11 10:30:00', 'Diferencia detectada', 'Conteo de verificación de inventario 10');

-- ============================================================================
-- TABLA: tblConductorTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (1, 1, 'Ana López', '55500001');

-- Registro 2
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (2, 2, 'Luis Morales', '55500002');

-- Registro 3
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (3, 3, 'María García', '55500003');

-- Registro 4
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (4, 4, 'Carlos Pérez', '55500004');

-- Registro 5
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (5, 5, 'Elena Ruiz', '55500005');

-- Registro 6
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (6, 6, 'José Ramírez', '55500006');

-- Registro 7
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (7, 7, 'Sofía Herrera', '55500007');

-- Registro 8
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (8, 8, 'Pedro Castillo', '55500008');

-- Registro 9
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (9, 9, 'Laura Díaz', '55500009');

-- Registro 10
INSERT INTO tblConductorTransporte (idConductorTransporte, Fk_Servicio_Transporte_Externo_Codigo, nombreConductorTransporte, telefonoConductorTransporte)
VALUES (10, 10, 'Jorge Méndez', '55500010');

-- ============================================================================
-- TABLA: tblCostoTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (1, 1, 1, 6, 25.0000, 'GTQ', 'Transferencia');

-- Registro 2
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (2, 2, 2, 7, 25.0000, 'GTQ', 'Efectivo');

-- Registro 3
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (3, 3, 3, 8, 25.0000, 'GTQ', 'Transferencia');

-- Registro 4
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (4, 4, 4, 9, 25.0000, 'GTQ', 'Efectivo');

-- Registro 5
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (5, 5, 5, 10, 25.0000, 'GTQ', 'Transferencia');

-- Registro 6
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (6, 6, 6, 11, 25.0000, 'GTQ', 'Efectivo');

-- Registro 7
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (7, 7, 7, 12, 25.0000, 'GTQ', 'Transferencia');

-- Registro 8
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (8, 8, 8, 13, 25.0000, 'GTQ', 'Efectivo');

-- Registro 9
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (9, 9, 9, 14, 25.0000, 'GTQ', 'Transferencia');

-- Registro 10
INSERT INTO tblCostoTransporte (idCostoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo, cantidadCostoTransporte, costoUnitarioCostoTransporte, monedaCostoTransporte, tipoPagoCostoTransporte)
VALUES (10, 10, 10, 15, 25.0000, 'GTQ', 'Efectivo');

-- ============================================================================
-- TABLA: tblDetalleMovimiento
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (1, 1, 1, 1, 6, 25.0000);

-- Registro 2
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (2, 2, 2, 2, 7, 25.0000);

-- Registro 3
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (3, 3, 3, 3, 8, 25.0000);

-- Registro 4
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (4, 4, 4, 4, 9, 25.0000);

-- Registro 5
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (5, 5, 5, 5, 10, 25.0000);

-- Registro 6
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (6, 6, 6, 6, 11, 25.0000);

-- Registro 7
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (7, 7, 7, 7, 12, 25.0000);

-- Registro 8
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (8, 8, 8, 8, 13, 25.0000);

-- Registro 9
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (9, 9, 9, 9, 14, 25.0000);

-- Registro 10
INSERT INTO tblDetalleMovimiento (idDetalleMovimiento, Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, cantidadDetalleMovimiento, precioCostoDetalleMovimiento)
VALUES (10, 10, 10, 10, 15, 25.0000);

-- ============================================================================
-- TABLA: tblDetalleViajeLoteProducto
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (1, 1, 1, 3, 12.500);

-- Registro 2
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (2, 2, 2, 4, 12.500);

-- Registro 3
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (3, 3, 3, 5, 12.500);

-- Registro 4
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (4, 4, 4, 6, 12.500);

-- Registro 5
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (5, 5, 5, 7, 12.500);

-- Registro 6
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (6, 6, 6, 8, 12.500);

-- Registro 7
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (7, 7, 7, 9, 12.500);

-- Registro 8
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (8, 8, 8, 10, 12.500);

-- Registro 9
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (9, 9, 9, 11, 12.500);

-- Registro 10
INSERT INTO tblDetalleViajeLoteProducto (idDetalleViajeLoteProducto, Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto)
VALUES (10, 10, 10, 12, 12.500);

-- ============================================================================
-- TABLA: tblHistorialTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (1, 1, '2026-10-02 10:30:00', 'Salida', 'Descripcion de prueba 1 - tblHistorialTransporte', 'En tránsito');

-- Registro 2
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (2, 2, '2026-10-03 10:30:00', 'Recepción', 'Descripcion de prueba 2 - tblHistorialTransporte', 'En tránsito');

-- Registro 3
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (3, 3, '2026-10-04 10:30:00', 'Salida', 'Descripcion de prueba 3 - tblHistorialTransporte', 'En tránsito');

-- Registro 4
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (4, 4, '2026-10-05 10:30:00', 'Recepción', 'Descripcion de prueba 4 - tblHistorialTransporte', 'En tránsito');

-- Registro 5
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (5, 5, '2026-10-06 10:30:00', 'Salida', 'Descripcion de prueba 5 - tblHistorialTransporte', 'En tránsito');

-- Registro 6
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (6, 6, '2026-10-07 10:30:00', 'Recepción', 'Descripcion de prueba 6 - tblHistorialTransporte', 'En tránsito');

-- Registro 7
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (7, 7, '2026-10-08 10:30:00', 'Salida', 'Descripcion de prueba 7 - tblHistorialTransporte', 'En tránsito');

-- Registro 8
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (8, 8, '2026-10-09 10:30:00', 'Recepción', 'Descripcion de prueba 8 - tblHistorialTransporte', 'En tránsito');

-- Registro 9
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (9, 9, '2026-10-10 10:30:00', 'Salida', 'Descripcion de prueba 9 - tblHistorialTransporte', 'En tránsito');

-- Registro 10
INSERT INTO tblHistorialTransporte (idHistorialTransporte, Fk_Servicio_Transporte_Externo_Codigo, fechaMovimientoHistorialTransporte, tipoMovimientoHistorialTransporte, descripcionMovimientoHistorialTransporte, estadoTransporteHistorialTransporte)
VALUES (10, 10, '2026-10-11 10:30:00', 'Recepción', 'Descripcion de prueba 10 - tblHistorialTransporte', 'En tránsito');

-- ============================================================================
-- TABLA: tblIncidenciaTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (1, 1, 'INC-0001', '2026-10-02 10:30:00', 'Retraso', 'Descripcion de prueba 1 - tblIncidenciaTransporte', 'Ana López', 'Pendiente');

-- Registro 2
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (2, 2, 'INC-0002', '2026-10-03 10:30:00', 'Desvío', 'Descripcion de prueba 2 - tblIncidenciaTransporte', 'Luis Morales', 'Resuelto');

-- Registro 3
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (3, 3, 'INC-0003', '2026-10-04 10:30:00', 'Retraso', 'Descripcion de prueba 3 - tblIncidenciaTransporte', 'María García', 'Pendiente');

-- Registro 4
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (4, 4, 'INC-0004', '2026-10-05 10:30:00', 'Desvío', 'Descripcion de prueba 4 - tblIncidenciaTransporte', 'Carlos Pérez', 'Resuelto');

-- Registro 5
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (5, 5, 'INC-0005', '2026-10-06 10:30:00', 'Retraso', 'Descripcion de prueba 5 - tblIncidenciaTransporte', 'Elena Ruiz', 'Pendiente');

-- Registro 6
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (6, 6, 'INC-0006', '2026-10-07 10:30:00', 'Desvío', 'Descripcion de prueba 6 - tblIncidenciaTransporte', 'José Ramírez', 'Resuelto');

-- Registro 7
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (7, 7, 'INC-0007', '2026-10-08 10:30:00', 'Retraso', 'Descripcion de prueba 7 - tblIncidenciaTransporte', 'Sofía Herrera', 'Pendiente');

-- Registro 8
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (8, 8, 'INC-0008', '2026-10-09 10:30:00', 'Desvío', 'Descripcion de prueba 8 - tblIncidenciaTransporte', 'Pedro Castillo', 'Resuelto');

-- Registro 9
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (9, 9, 'INC-0009', '2026-10-10 10:30:00', 'Retraso', 'Descripcion de prueba 9 - tblIncidenciaTransporte', 'Laura Díaz', 'Pendiente');

-- Registro 10
INSERT INTO tblIncidenciaTransporte (idIncidenciaTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoIncidenciaTransporte, fechaIncidenciaTransporte, tipoIncidenciaTransporte, descripcionIncidenciaTransporte, responsableIncidenciaTransporte, estadoIncidenciaTransporte)
VALUES (10, 10, 'INC-0010', '2026-10-11 10:30:00', 'Desvío', 'Descripcion de prueba 10 - tblIncidenciaTransporte', 'Jorge Méndez', 'Resuelto');

-- ============================================================================
-- TABLA: tblProductoTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (1, 1, 1, 6, 'cajas');

-- Registro 2
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (2, 2, 2, 7, 'cajas');

-- Registro 3
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (3, 3, 3, 8, 'cajas');

-- Registro 4
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (4, 4, 4, 9, 'cajas');

-- Registro 5
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (5, 5, 5, 10, 'cajas');

-- Registro 6
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (6, 6, 6, 11, 'cajas');

-- Registro 7
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (7, 7, 7, 12, 'cajas');

-- Registro 8
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (8, 8, 8, 13, 'cajas');

-- Registro 9
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (9, 9, 9, 14, 'cajas');

-- Registro 10
INSERT INTO tblProductoTransporte (idProductoTransporte, Fk_Servicio_Transporte_Externo_Codigo, Fk_Producto_Codigo, cantidadProductoTransporte, unidadMedidaProductoTransporte)
VALUES (10, 10, 10, 15, 'cajas');

-- ============================================================================
-- TABLA: tblSolicitudPagoTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (1, 1, 'SP-0001', 1, 'REF-0001', '2026-10-20');

-- Registro 2
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (2, 2, 'SP-0002', 2, 'REF-0002', '2026-10-20');

-- Registro 3
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (3, 3, 'SP-0003', 3, 'REF-0003', '2026-10-20');

-- Registro 4
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (4, 4, 'SP-0004', 4, 'REF-0004', '2026-10-20');

-- Registro 5
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (5, 5, 'SP-0005', 5, 'REF-0005', '2026-10-20');

-- Registro 6
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (6, 6, 'SP-0006', 6, 'REF-0006', '2026-10-20');

-- Registro 7
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (7, 7, 'SP-0007', 7, 'REF-0007', '2026-10-20');

-- Registro 8
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (8, 8, 'SP-0008', 8, 'REF-0008', '2026-10-20');

-- Registro 9
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (9, 9, 'SP-0009', 9, 'REF-0009', '2026-10-20');

-- Registro 10
INSERT INTO tblSolicitudPagoTransporte (idSolicitudPagoTransporte, Fk_Servicio_Transporte_Externo_Codigo, codigoSolicitudPagoTransporte, Fk_Estado_Solicitud_Pago_Codigo, referenciaBancariaSolicitudPagoTransporte, fechaPagoSolicitudPagoTransporte)
VALUES (10, 10, 'SP-0010', 10, 'REF-0010', '2026-10-20');

-- ============================================================================
-- TABLA: tblUbicacionTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (1, 1, 'Zona 1, Ciudad de Guatemala', '2026-10-02 10:30:00');

-- Registro 2
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (2, 2, 'Zona 2, Ciudad de Guatemala', '2026-10-03 10:30:00');

-- Registro 3
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (3, 3, 'Zona 3, Ciudad de Guatemala', '2026-10-04 10:30:00');

-- Registro 4
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (4, 4, 'Zona 4, Ciudad de Guatemala', '2026-10-05 10:30:00');

-- Registro 5
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (5, 5, 'Zona 5, Ciudad de Guatemala', '2026-10-06 10:30:00');

-- Registro 6
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (6, 6, 'Zona 6, Ciudad de Guatemala', '2026-10-07 10:30:00');

-- Registro 7
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (7, 7, 'Zona 7, Ciudad de Guatemala', '2026-10-08 10:30:00');

-- Registro 8
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (8, 8, 'Zona 8, Ciudad de Guatemala', '2026-10-09 10:30:00');

-- Registro 9
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (9, 9, 'Zona 9, Ciudad de Guatemala', '2026-10-10 10:30:00');

-- Registro 10
INSERT INTO tblUbicacionTransporte (idUbicacionTransporte, Fk_Servicio_Transporte_Externo_Codigo, ubicacionActualUbicacionTransporte, fechaActualizacionUbicacionTransporte)
VALUES (10, 10, 'Zona 10, Ciudad de Guatemala', '2026-10-11 10:30:00');

-- ============================================================================
-- TABLA: tblVehiculoTransporte
-- ============================================================================
-- Registro 1
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (1, 1, 'C001DEF', 'Camión', '1250 kg');

-- Registro 2
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (2, 2, 'C002DEF', 'Camioneta', '1500 kg');

-- Registro 3
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (3, 3, 'C003DEF', 'Furgón', '1750 kg');

-- Registro 4
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (4, 4, 'C004DEF', 'Pick-up', '2000 kg');

-- Registro 5
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (5, 5, 'C005DEF', 'Panel', '2250 kg');

-- Registro 6
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (6, 6, 'C006DEF', 'Tráiler', '2500 kg');

-- Registro 7
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (7, 7, 'C007DEF', 'Cabezal', '2750 kg');

-- Registro 8
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (8, 8, 'C008DEF', 'Motocicleta', '3000 kg');

-- Registro 9
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (9, 9, 'C009DEF', 'Microbús', '3250 kg');

-- Registro 10
INSERT INTO tblVehiculoTransporte (idVehiculoTransporte, Fk_Servicio_Transporte_Externo_Codigo, placaVehiculoTransporte, tipoVehiculoTransporte, capacidadVehiculoTransporte)
VALUES (10, 10, 'C010DEF', 'Camión refrigerado', '3500 kg');

-- ============================================================================
-- TABLA: tblDetalleAuditoria
-- ============================================================================
-- Registro 1
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (1, 1, 1, 101, 101, 'Detalle del conteo 1');

-- Registro 2
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (2, 2, 2, 102, 102, 'Detalle del conteo 2');

-- Registro 3
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (3, 3, 3, 103, 103, 'Detalle del conteo 3');

-- Registro 4
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (4, 4, 4, 104, 104, 'Detalle del conteo 4');

-- Registro 5
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (5, 5, 5, 105, 105, 'Detalle del conteo 5');

-- Registro 6
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (6, 6, 6, 106, 106, 'Detalle del conteo 6');

-- Registro 7
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (7, 7, 7, 107, 107, 'Detalle del conteo 7');

-- Registro 8
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (8, 8, 8, 108, 108, 'Detalle del conteo 8');

-- Registro 9
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (9, 9, 9, 109, 109, 'Detalle del conteo 9');

-- Registro 10
INSERT INTO tblDetalleAuditoria (idDetalleAuditoria, Fk_Auditoria_Inventario_Codigo, Fk_Stock_Codigo, stockTeoricoDetalleAuditoria, stockFisicoDetalleAuditoria, observacionesDetalleAuditoria)
VALUES (10, 10, 10, 110, 110, 'Detalle del conteo 10');

COMMIT;
-- compras 
SET @lote = LEFT(REPLACE(UUID(), '-', ''), 12);
SET @fecha = CURRENT_DATE();
SET @serie = CONCAT('PR-', @lote);
SET @moneda = (SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = 'GTQ' AND is_active = 1 LIMIT 1);
SET @metodo = (SELECT idMetodoPago FROM tblMetodoPago WHERE is_active = 1 ORDER BY idMetodoPago LIMIT 1);
SET @bodega = (SELECT idBodega FROM tblBodega ORDER BY idBodega LIMIT 1);
SET @marca = (SELECT idMarca FROM tblMarca ORDER BY idMarca LIMIT 1);
SET @categoria = (SELECT idCategoria FROM tblCategoria ORDER BY idCategoria LIMIT 1);
SET @linea = (SELECT idLinea FROM tblLinea ORDER BY idLinea LIMIT 1);
SET @tipo_cambio = 1;
-- Moneda local: idTipoCambio queda NULL, tipo de cambio = 1.

DELIMITER $$
CREATE PROCEDURE sp_cargar_compras_5_3()
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    IF @moneda IS NULL OR @metodo IS NULL OR @bodega IS NULL
       OR @marca IS NULL OR @categoria IS NULL OR @linea IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Faltan dependencias: GTQ activa, metodo activo, bodega, marca, categoria o linea. No se insertaron datos.';
    END IF;
    START TRANSACTION;

    INSERT INTO tblPlazo (nombrePlazo, diasPlazo)
    VALUES (CONCAT('Prueba ', @lote, ' - 15 dias'), 15);
    SET @plazo1 = LAST_INSERT_ID();
    INSERT INTO tblPlazo (nombrePlazo, diasPlazo)
    VALUES (CONCAT('Prueba ', @lote, ' - 30 dias'), 30);
    SET @plazo2 = LAST_INSERT_ID();
    INSERT INTO tblPlazo (nombrePlazo, diasPlazo)
    VALUES (CONCAT('Prueba ', @lote, ' - 45 dias'), 45);
    SET @plazo3 = LAST_INSERT_ID();
    INSERT INTO tblPlazo (nombrePlazo, diasPlazo)
    VALUES (CONCAT('Prueba ', @lote, ' - 60 dias'), 60);
    SET @plazo4 = LAST_INSERT_ID();
    INSERT INTO tblPlazo (nombrePlazo, diasPlazo)
    VALUES (CONCAT('Prueba ', @lote, ' - 75 dias'), 75);
    SET @plazo5 = LAST_INSERT_ID();
    INSERT INTO tblConceptoProveedor (nombreConceptoProveedor, naturalezaConceptoProveedor)
    VALUES (CONCAT('Factura prueba ', @lote), 'Cargo');
    SET @concepto1 = LAST_INSERT_ID();
    INSERT INTO tblConceptoProveedor (nombreConceptoProveedor, naturalezaConceptoProveedor)
    VALUES (CONCAT('Debito prueba ', @lote), 'Cargo');
    SET @concepto2 = LAST_INSERT_ID();
    INSERT INTO tblConceptoProveedor (nombreConceptoProveedor, naturalezaConceptoProveedor)
    VALUES (CONCAT('Pago prueba ', @lote), 'Abono');
    SET @concepto3 = LAST_INSERT_ID();
    INSERT INTO tblConceptoProveedor (nombreConceptoProveedor, naturalezaConceptoProveedor)
    VALUES (CONCAT('Credito prueba ', @lote), 'Abono');
    SET @concepto4 = LAST_INSERT_ID();
    INSERT INTO tblConceptoProveedor (nombreConceptoProveedor, naturalezaConceptoProveedor)
    VALUES (CONCAT('Transporte prueba ', @lote), 'Cargo');
    SET @concepto5 = LAST_INSERT_ID();
    INSERT INTO tblProveedor (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, idPlazo, saldoActualProveedor, cargosMesProveedor, cargosAcumuladosProveedor)
    VALUES (CONCAT('Proveedor prueba 1 ', @lote), CONCAT(LEFT(@lote, 9), '1'), 'proveedor1@example.com', '55550001', 'Guatemala', @plazo1, 100, 100, 100);
    SET @proveedor1 = LAST_INSERT_ID();
    INSERT INTO tblProducto (skuProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
    VALUES (CONCAT('PR-', @lote, '-1'), 'Producto prueba 1', @proveedor1, @marca, @categoria, @linea, 'Producto de ejemplo', '', 'Guatemala', 10, 15, 0, 5, 100, 1);
    SET @producto1 = LAST_INSERT_ID();
    INSERT INTO tblProveedor (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, idPlazo, saldoActualProveedor, cargosMesProveedor, cargosAcumuladosProveedor)
    VALUES (CONCAT('Proveedor prueba 2 ', @lote), CONCAT(LEFT(@lote, 9), '2'), 'proveedor2@example.com', '55550002', 'Guatemala', @plazo2, 100, 100, 100);
    SET @proveedor2 = LAST_INSERT_ID();
    INSERT INTO tblProducto (skuProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
    VALUES (CONCAT('PR-', @lote, '-2'), 'Producto prueba 2', @proveedor2, @marca, @categoria, @linea, 'Producto de ejemplo','', 'Guatemala', 10, 15, 0, 5, 100, 1);
    SET @producto2 = LAST_INSERT_ID();
    INSERT INTO tblProveedor (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, idPlazo, saldoActualProveedor, cargosMesProveedor, cargosAcumuladosProveedor)
    VALUES (CONCAT('Proveedor prueba 3 ', @lote), CONCAT(LEFT(@lote, 9), '3'), 'proveedor3@example.com', '55550003', 'Guatemala', @plazo3, 100, 100, 100);
    SET @proveedor3 = LAST_INSERT_ID();
    INSERT INTO tblProducto (skuProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
    VALUES (CONCAT('PR-', @lote, '-3'), 'Producto prueba 3', @proveedor3, @marca, @categoria, @linea, 'Producto de ejemplo', '', 'Guatemala', 10, 15, 0, 5, 100, 1);
    SET @producto3 = LAST_INSERT_ID();
    INSERT INTO tblProveedor (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, idPlazo, saldoActualProveedor, cargosMesProveedor, cargosAcumuladosProveedor)
    VALUES (CONCAT('Proveedor prueba 4 ', @lote), CONCAT(LEFT(@lote, 9), '4'), 'proveedor4@example.com', '55550004', 'Guatemala', @plazo4, 100, 100, 100);
    SET @proveedor4 = LAST_INSERT_ID();
    INSERT INTO tblProducto (skuProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
    VALUES (CONCAT('PR-', @lote, '-4'), 'Producto prueba 4', @proveedor4, @marca, @categoria, @linea, 'Producto de ejemplo', '', 'Guatemala', 10, 15, 0, 5, 100, 1);
    SET @producto4 = LAST_INSERT_ID();
    INSERT INTO tblProveedor (nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor, idPlazo, saldoActualProveedor, cargosMesProveedor, cargosAcumuladosProveedor)
    VALUES (CONCAT('Proveedor prueba 5 ', @lote), CONCAT(LEFT(@lote, 9), '5'), 'proveedor5@example.com', '55550005', 'Guatemala', @plazo5, 100, 100, 100);
    SET @proveedor5 = LAST_INSERT_ID();
    INSERT INTO tblProducto (skuProducto, nombreProducto, idProveedorProductos, Fk_Marca_Codigo, Fk_Categoria_Codigo, Fk_Linea_Codigo, descripcionProducto, imagenProducto, paisProducto, precioCostoProducto, precioVentaProducto, porcentajeComisionProducto, stockMinimoProducto, stockMaximoProducto, pesoProducto)
    VALUES (CONCAT('PR-', @lote, '-5'), 'Producto prueba 5', @proveedor5, @marca, @categoria, @linea, 'Producto de ejemplo', '', 'Guatemala', 10, 15, 0, 5, 100, 1);
    SET @producto5 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoOrdenCompra (idProveedor, idPlazo, fechaEmisionOrdenCompra, fechaVencimientoOrdenCompra, idMoneda, tipoCambioOrdenCompra, subtotalOrdenCompra, totalOrdenCompra, estadoOrdenCompra)
    VALUES (@proveedor1, @plazo1, @fecha, DATE_ADD(@fecha, INTERVAL 15 DAY), @moneda, @tipo_cambio, 100, 100, 'Pendiente');
    SET @orden1 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoCompra (idOrdenCompra, idProveedor, serieFacturaCompra, numeroFacturaCompra, fechaCompra, idMetodoPago, idMoneda, tipoCambioCompra, subtotalCompra, totalCompra, estadoCompra)
    VALUES (@orden1, @proveedor1, @serie, '1', @fecha, @metodo, @moneda, @tipo_cambio, 100, 100, 'Registrada');
    SET @compra1 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoOrdenCompra (idProveedor, idPlazo, fechaEmisionOrdenCompra, fechaVencimientoOrdenCompra, idMoneda, tipoCambioOrdenCompra, subtotalOrdenCompra, totalOrdenCompra, estadoOrdenCompra)
    VALUES (@proveedor2, @plazo2, @fecha, DATE_ADD(@fecha, INTERVAL 30 DAY), @moneda, @tipo_cambio, 100, 100, 'Pendiente');
    SET @orden2 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoCompra (idOrdenCompra, idProveedor, serieFacturaCompra, numeroFacturaCompra, fechaCompra, idMetodoPago, idMoneda, tipoCambioCompra, subtotalCompra, totalCompra, estadoCompra)
    VALUES (@orden2, @proveedor2, @serie, '2', @fecha, @metodo, @moneda, @tipo_cambio, 100, 100, 'Registrada');
    SET @compra2 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoOrdenCompra (idProveedor, idPlazo, fechaEmisionOrdenCompra, fechaVencimientoOrdenCompra, idMoneda, tipoCambioOrdenCompra, subtotalOrdenCompra, totalOrdenCompra, estadoOrdenCompra)
    VALUES (@proveedor3, @plazo3, @fecha, DATE_ADD(@fecha, INTERVAL 45 DAY), @moneda, @tipo_cambio, 100, 100, 'Pendiente');
    SET @orden3 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoCompra (idOrdenCompra, idProveedor, serieFacturaCompra, numeroFacturaCompra, fechaCompra, idMetodoPago, idMoneda, tipoCambioCompra, subtotalCompra, totalCompra, estadoCompra)
    VALUES (@orden3, @proveedor3, @serie, '3', @fecha, @metodo, @moneda, @tipo_cambio, 100, 100, 'Registrada');
    SET @compra3 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoOrdenCompra (idProveedor, idPlazo, fechaEmisionOrdenCompra, fechaVencimientoOrdenCompra, idMoneda, tipoCambioOrdenCompra, subtotalOrdenCompra, totalOrdenCompra, estadoOrdenCompra)
    VALUES (@proveedor4, @plazo4, @fecha, DATE_ADD(@fecha, INTERVAL 60 DAY), @moneda, @tipo_cambio, 0, 0, 'Pendiente');
    SET @orden4 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoCompra (idOrdenCompra, idProveedor, serieFacturaCompra, numeroFacturaCompra, fechaCompra, idMetodoPago, idMoneda, tipoCambioCompra, subtotalCompra, totalCompra, estadoCompra)
    VALUES (@orden4, @proveedor4, @serie, '4', @fecha, @metodo, @moneda, @tipo_cambio, 0, 0, 'Anulada');
    SET @compra4 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoOrdenCompra (idProveedor, idPlazo, fechaEmisionOrdenCompra, fechaVencimientoOrdenCompra, idMoneda, tipoCambioOrdenCompra, subtotalOrdenCompra, totalOrdenCompra, estadoOrdenCompra)
    VALUES (@proveedor5, @plazo5, @fecha, DATE_ADD(@fecha, INTERVAL 75 DAY), @moneda, @tipo_cambio, 0, 0, 'Pendiente');
    SET @orden5 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoCompra (idOrdenCompra, idProveedor, serieFacturaCompra, numeroFacturaCompra, fechaCompra, idMetodoPago, idMoneda, tipoCambioCompra, subtotalCompra, totalCompra, estadoCompra)
    VALUES (@orden5, @proveedor5, @serie, '5', @fecha, @metodo, @moneda, @tipo_cambio, 0, 0, 'Anulada');
    SET @compra5 = LAST_INSERT_ID();
    INSERT INTO tblDetalleOrdenCompra (idOrdenCompra, ordenDetalleOrdenCompra, idProducto, idBodega, cantidadDetalleOrdenCompra, costoUnitarioDetalleOrdenCompra)
    VALUES (@orden1, 1, @producto1, @bodega, 10, 10);
    INSERT INTO tblDetalleCompra (idCompra, ordenDetalleCompra, idProducto, idBodega, cantidadDetalleCompra, costoUnitarioDetalleCompra)
    VALUES (@compra1, 1, @producto1, @bodega, 10, 10);
    INSERT INTO tblDetalleOrdenCompra (idOrdenCompra, ordenDetalleOrdenCompra, idProducto, idBodega, cantidadDetalleOrdenCompra, costoUnitarioDetalleOrdenCompra)
    VALUES (@orden2, 1, @producto2, @bodega, 10, 10);
    INSERT INTO tblDetalleCompra (idCompra, ordenDetalleCompra, idProducto, idBodega, cantidadDetalleCompra, costoUnitarioDetalleCompra)
    VALUES (@compra2, 1, @producto2, @bodega, 10, 10);
    INSERT INTO tblDetalleOrdenCompra (idOrdenCompra, ordenDetalleOrdenCompra, idProducto, idBodega, cantidadDetalleOrdenCompra, costoUnitarioDetalleOrdenCompra)
    VALUES (@orden3, 1, @producto3, @bodega, 10, 10);
    INSERT INTO tblDetalleCompra (idCompra, ordenDetalleCompra, idProducto, idBodega, cantidadDetalleCompra, costoUnitarioDetalleCompra)
    VALUES (@compra3, 1, @producto3, @bodega, 10, 10);
    -- Las cuentas 4 y 5 son documentos independientes de las compras anuladas.
    -- Pagos programados y devoluciones anuladas no afectan saldos ni existencias.
    INSERT INTO tblCuentaPorPagar (idProveedor, idConceptoProveedor, serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar, idMoneda, tipoCambioCuentaPorPagar, montoTotalCuentaPorPagar, saldoActualCuentaPorPagar)
    VALUES (@proveedor1, @concepto1, @serie, '1', @fecha, DATE_ADD(@fecha, INTERVAL 15 DAY), @moneda, @tipo_cambio, 100, 100);
    INSERT INTO tblDevolucionCompra (idCompra, fechaDevolucionCompra, motivoDevolucionCompra, totalDevolucionCompra, estadoDevolucionCompra)
    VALUES (@compra1, @fecha, 'Ejemplo anulado sin efecto en saldo ni inventario', 10, 'Anulada');
    SET @devolucion1 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoPagoProveedor (idProveedor, fechaPagoProveedor, montoPagoProveedor, idMetodoPago, idMoneda, tipoCambioPagoProveedor, referenciaPagoProveedor, estadoPagoProveedor)
    VALUES (@proveedor1, @fecha, 20, @metodo, @moneda, @tipo_cambio, CONCAT(@serie, '-1'), 'Programado');
    SET @pago1 = LAST_INSERT_ID();
    INSERT INTO tblSolicitudPago (moduloOrigenSolicitudPago, idOrigenSolicitudPago, beneficiarioSolicitudPago, montoSolicitudPago, metodoSolicitudPago, estadoSolicitudPago)
    VALUES ('Compras', @pago1, CONCAT('Proveedor prueba 1 ', @lote), 20, (SELECT LEFT(nombreMetodoPago, 30) FROM tblMetodoPago WHERE idMetodoPago = @metodo), 'Pendiente');
    INSERT INTO tblCuentaPorPagar (idProveedor, idConceptoProveedor, serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar, idMoneda, tipoCambioCuentaPorPagar, montoTotalCuentaPorPagar, saldoActualCuentaPorPagar)
    VALUES (@proveedor2, @concepto1, @serie, '2', @fecha, DATE_ADD(@fecha, INTERVAL 30 DAY), @moneda, @tipo_cambio, 100, 100);
    INSERT INTO tblDevolucionCompra (idCompra, fechaDevolucionCompra, motivoDevolucionCompra, totalDevolucionCompra, estadoDevolucionCompra)
    VALUES (@compra2, @fecha, 'Ejemplo anulado sin efecto en saldo ni inventario', 10, 'Anulada');
    SET @devolucion2 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoPagoProveedor (idProveedor, fechaPagoProveedor, montoPagoProveedor, idMetodoPago, idMoneda, tipoCambioPagoProveedor, referenciaPagoProveedor, estadoPagoProveedor)
    VALUES (@proveedor2, @fecha, 20, @metodo, @moneda, @tipo_cambio, CONCAT(@serie, '-2'), 'Programado');
    SET @pago2 = LAST_INSERT_ID();
    INSERT INTO tblSolicitudPago (moduloOrigenSolicitudPago, idOrigenSolicitudPago, beneficiarioSolicitudPago, montoSolicitudPago, metodoSolicitudPago, estadoSolicitudPago)
    VALUES ('Compras', @pago2, CONCAT('Proveedor prueba 2 ', @lote), 20, (SELECT LEFT(nombreMetodoPago, 30) FROM tblMetodoPago WHERE idMetodoPago = @metodo), 'Pendiente');
    INSERT INTO tblCuentaPorPagar (idProveedor, idConceptoProveedor, serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar, idMoneda, tipoCambioCuentaPorPagar, montoTotalCuentaPorPagar, saldoActualCuentaPorPagar)
    VALUES (@proveedor3, @concepto1, @serie, '3', @fecha, DATE_ADD(@fecha, INTERVAL 45 DAY), @moneda, @tipo_cambio, 100, 100);
    INSERT INTO tblDevolucionCompra (idCompra, fechaDevolucionCompra, motivoDevolucionCompra, totalDevolucionCompra, estadoDevolucionCompra)
    VALUES (@compra3, @fecha, 'Ejemplo anulado sin efecto en saldo ni inventario', 10, 'Anulada');
    SET @devolucion3 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoPagoProveedor (idProveedor, fechaPagoProveedor, montoPagoProveedor, idMetodoPago, idMoneda, tipoCambioPagoProveedor, referenciaPagoProveedor, estadoPagoProveedor)
    VALUES (@proveedor3, @fecha, 20, @metodo, @moneda, @tipo_cambio, CONCAT(@serie, '-3'), 'Programado');
    SET @pago3 = LAST_INSERT_ID();
    INSERT INTO tblSolicitudPago (moduloOrigenSolicitudPago, idOrigenSolicitudPago, beneficiarioSolicitudPago, montoSolicitudPago, metodoSolicitudPago, estadoSolicitudPago)
    VALUES ('Compras', @pago3, CONCAT('Proveedor prueba 3 ', @lote), 20, (SELECT LEFT(nombreMetodoPago, 30) FROM tblMetodoPago WHERE idMetodoPago = @metodo), 'Pendiente');
    INSERT INTO tblCuentaPorPagar (idProveedor, idConceptoProveedor, serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar, idMoneda, tipoCambioCuentaPorPagar, montoTotalCuentaPorPagar, saldoActualCuentaPorPagar)
    VALUES (@proveedor4, @concepto1, @serie, '4', @fecha, DATE_ADD(@fecha, INTERVAL 60 DAY), @moneda, @tipo_cambio, 100, 100);
    INSERT INTO tblDevolucionCompra (idCompra, fechaDevolucionCompra, motivoDevolucionCompra, totalDevolucionCompra, estadoDevolucionCompra)
    VALUES (@compra3, @fecha, 'Ejemplo anulado sin efecto en saldo ni inventario', 10, 'Anulada');
    SET @devolucion4 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoPagoProveedor (idProveedor, fechaPagoProveedor, montoPagoProveedor, idMetodoPago, idMoneda, tipoCambioPagoProveedor, referenciaPagoProveedor, estadoPagoProveedor)
    VALUES (@proveedor4, @fecha, 20, @metodo, @moneda, @tipo_cambio, CONCAT(@serie, '-4'), 'Rechazado');
    SET @pago4 = LAST_INSERT_ID();
    INSERT INTO tblSolicitudPago (moduloOrigenSolicitudPago, idOrigenSolicitudPago, beneficiarioSolicitudPago, montoSolicitudPago, metodoSolicitudPago, estadoSolicitudPago)
    VALUES ('Compras', @pago4, CONCAT('Proveedor prueba 4 ', @lote), 20, (SELECT LEFT(nombreMetodoPago, 30) FROM tblMetodoPago WHERE idMetodoPago = @metodo), 'Rechazada');
    INSERT INTO tblCuentaPorPagar (idProveedor, idConceptoProveedor, serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar, idMoneda, tipoCambioCuentaPorPagar, montoTotalCuentaPorPagar, saldoActualCuentaPorPagar)
    VALUES (@proveedor5, @concepto1, @serie, '5', @fecha, DATE_ADD(@fecha, INTERVAL 75 DAY), @moneda, @tipo_cambio, 100, 100);
    INSERT INTO tblDevolucionCompra (idCompra, fechaDevolucionCompra, motivoDevolucionCompra, totalDevolucionCompra, estadoDevolucionCompra)
    VALUES (@compra3, @fecha, 'Ejemplo anulado sin efecto en saldo ni inventario', 10, 'Anulada');
    SET @devolucion5 = LAST_INSERT_ID();
    INSERT INTO tblEncabezadoPagoProveedor (idProveedor, fechaPagoProveedor, montoPagoProveedor, idMetodoPago, idMoneda, tipoCambioPagoProveedor, referenciaPagoProveedor, estadoPagoProveedor)
    VALUES (@proveedor5, @fecha, 20, @metodo, @moneda, @tipo_cambio, CONCAT(@serie, '-5'), 'Rechazado');
    SET @pago5 = LAST_INSERT_ID();
    INSERT INTO tblSolicitudPago (moduloOrigenSolicitudPago, idOrigenSolicitudPago, beneficiarioSolicitudPago, montoSolicitudPago, metodoSolicitudPago, estadoSolicitudPago)
    VALUES ('Compras', @pago5, CONCAT('Proveedor prueba 5 ', @lote), 20, (SELECT LEFT(nombreMetodoPago, 30) FROM tblMetodoPago WHERE idMetodoPago = @metodo), 'Rechazada');
    INSERT INTO tblDetalleDevolucionCompra (idDevolucionCompra, ordenDetalleDevolucionCompra, idProducto, idBodega, cantidadDetalleDevolucionCompra, costoUnitarioDetalleDevolucionCompra)
    VALUES (@devolucion1, 1, @producto1, @bodega, 1, 10);
    INSERT INTO tblDetallePagoProveedor (idPagoProveedor, serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor, montoAplicadoDetallePago)
    VALUES (@pago1, @serie, '1', @concepto1, 20);
    INSERT INTO tblDetalleDevolucionCompra (idDevolucionCompra, ordenDetalleDevolucionCompra, idProducto, idBodega, cantidadDetalleDevolucionCompra, costoUnitarioDetalleDevolucionCompra)
    VALUES (@devolucion2, 1, @producto2, @bodega, 1, 10);
    INSERT INTO tblDetallePagoProveedor (idPagoProveedor, serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor, montoAplicadoDetallePago)
    VALUES (@pago2, @serie, '2', @concepto1, 20);
    INSERT INTO tblDetalleDevolucionCompra (idDevolucionCompra, ordenDetalleDevolucionCompra, idProducto, idBodega, cantidadDetalleDevolucionCompra, costoUnitarioDetalleDevolucionCompra)
    VALUES (@devolucion3, 1, @producto3, @bodega, 1, 10);
    INSERT INTO tblDetallePagoProveedor (idPagoProveedor, serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar, idConceptoProveedor, montoAplicadoDetallePago)
    VALUES (@pago3, @serie, '3', @concepto1, 20);
    COMMIT;
END$$
DELIMITER ;
CALL sp_cargar_compras_5_3();
DROP PROCEDURE sp_cargar_compras_5_3;

-- ventas
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

-- permisos de las aplicaciones
-- ---------- 10. MODULO 9000 PARA RECURSOS HUMANOS  ----------
-- Primero creo el modulo para tenerlo en seguridad
-- Módulo 9000
INSERT INTO tblModulo (idModulo, nombreModulo, descripcionModulo, is_active)
VALUES (9000, 'Recursos Humanos',
        'Módulo de administración del personal: empleados, asistencia, permisos, capacitaciones, desempeño, nómina y liquidaciones', 1) AS nuevo
ON DUPLICATE KEY UPDATE
    nombreModulo      = nuevo.nombreModulo,
    descripcionModulo = nuevo.descripcionModulo,
    is_active         = 1;

-- AHORA CREAMOS TODAS APLICACIONES TANTO PARA TRANSACCIONALES Y NAVEGDORES 
-- Aplicaciones del módulo 9000
INSERT INTO tblAplicacion (idAplicacion, idModulo, nombreAplicacion, descripcionAplicacion, is_active)
VALUES
-- Empleados
(9001, 9000, 'Empleados',                 'Gestión de empleados, departamentos, jornadas y puestos', 1),
(9002, 9000, 'Departamentos',             'Administra los departamentos de la empresa para la asignación', 1),
(9003, 9000, 'Jornadas',                  'Gestiona las jornadas laborales disponibles en la empresa', 1),
(9004, 9000, 'Puestos',                   'Administra los puestos de trabajo disponibles para los empleados', 1),
(9005, 9000, 'Tipo de Puestos',           'Gestiona los tipos de puesto (operativo, administrativo, etc.)', 1),
(9006, 9000, 'Asignación de Puestos',     'Relaciona los empleados con sus puestos y controla su historial', 1),
-- Asistencia
(9101, 9000, 'Asistencia',                'Registro y control de asistencia de los empleados', 1),
(9102, 9000, 'Horas Extra', 'Cálculo y aprobación del pago de horas extra generadas desde Asistencia', 1),
-- Permisos y vacaciones
(9201, 9000, 'Permisos y Vacaciones',     'Gestión de permisos, períodos de vacaciones y su aprobación', 1),
(9202, 9000, 'Permisos',                  'Registra y da seguimiento a las solicitudes de permisos del personal: tipos, fechas, motivos y estados de aprobación', 1),
(9203, 9000, 'Vacaciones',                'Gestiona las solicitudes de vacaciones del personal: días disponibles, períodos vacacionales y estado de cada solicitud', 1),
-- Capacitaciones
(9301, 9000, 'Capacitación',              'Gestión de capacitaciones, instructores, evaluaciones y asignaciones', 1),
(9302, 9000, 'Capacitaciones',            'Define el nombre, las fechas, el costo y el cupo de cada capacitación por departamento', 1),
(9303, 9000, 'Instructores',              'Mantiene el listado de instructores con su especialidad y correo de contacto', 1),
(9304, 9000, 'Evaluaciones',              'Registra puntualidad, calidad, responsabilidad y desarrollo; el resultado final se calcula', 1),
(9305, 9000, 'Asignación de Capacitación','Asigna empleados a cada capacitación; un empleado no puede repetirse en la misma', 1),
-- Desempeño, nómina y cierres
(9401, 9000, 'Evaluación de Desempeño',   'Evaluación del desempeño de los empleados por ciclo y criterio', 1),
(9402, 9000, 'Realizar Evaluación de Desempeño', 'Registra y evalúa el desempeño del personal según los criterios establecidos para cada ciclo', 1),
(9403, 9000, 'Plan de Mejora',            'Define acciones y da seguimiento a los planes de mejora de los colaboradores', 1),
(9404, 9000, 'Ver Evaluaciones',          'Consulta resultados, puntajes y retroalimentación de desempeño', 1),
(9501, 9000, 'Nómina',                    'Cálculo y gestión de la nómina de los empleados', 1),
(9601, 9000, 'Cierres de Nómina',         'Cierre de períodos de nómina', 1),
-- Liquidación
(9701, 9000, 'Liquidación del Empleado',  'Bajas, liquidaciones y finiquitos de empleados', 1),
(9702, 9000, 'Motivo de Baja',            'Catálogo de motivos de baja de los empleados', 1),
(9703, 9000, 'Parámetros de Ley',         'Parámetros legales para el cálculo de indemnizaciones y liquidaciones', 1),
(9704, 9000, 'Baja de Empleado',          'Registro de la baja de un empleado', 1) AS nuevo
ON DUPLICATE KEY UPDATE
    idModulo              = nuevo.idModulo,
    nombreAplicacion      = nuevo.nombreAplicacion,
    descripcionAplicacion = nuevo.descripcionAplicacion,
    is_active             = 1;

-- 1. Modulo. El MDI y el login lo buscan por el nombre "Contabilidad".
INSERT INTO tblmodulo (idModulo, nombreModulo, descripcionModulo, is_active)
SELECT 10000, 'Contabilidad', 'Módulo de Contabilidad', 1 FROM DUAL
WHERE NOT EXISTS (SELECT 1 FROM tblmodulo WHERE nombreModulo = 'Contabilidad');

UPDATE tblmodulo SET is_active = 1 WHERE nombreModulo = 'Contabilidad';

SET @idModulo = (SELECT idModulo FROM tblmodulo WHERE nombreModulo = 'Contabilidad');

-- 2. Aplicaciones. Los nombres deben quedar exactos: Contabilidad las busca por nombre.
INSERT INTO tblaplicacion (idAplicacion, idModulo, nombreAplicacion, descripcionAplicacion, is_active) VALUES
    (10000, @idModulo, 'Contabilidad', '10000 - Menú principal del módulo de Contabilidad', 1),
    (10001, @idModulo, 'Pólizas locales', '10001 - Registro de pólizas locales de Contabilidad', 1),
    (10002, @idModulo, 'Presupuestos', '10002 - Planificación de presupuestos por módulo', 1),
    (10003, @idModulo, 'Activos fijos', '10003 - Ficha y depreciación de activos fijos', 1),
    (10004, @idModulo, 'Cierre contable', '10004 - Revisión y cierre del período contable', 1),
    (10005, @idModulo, 'Estados financieros', '10005 - Consulta del estado de resultados', 1),
    (10006, @idModulo, 'Detalle de póliza', '10006 - Encabezado y detalle de la póliza con el navegador', 1),
    (10011, @idModulo, 'Mantenimiento de países', '10011 - Mantenimiento de tblPais con el control Navegador', 1),
    (10012, @idModulo, 'Mantenimiento de módulos contables', '10012 - Mantenimiento de tblModuloContable', 1),
    (10013, @idModulo, 'Mantenimiento de vida útil', '10013 - Mantenimiento de tblMaestroVidaUtil', 1),
    (10014, @idModulo, 'Mantenimiento de cuentas', '10014 - Mantenimiento del catálogo de cuentas tblCuenta', 1),
    (10015, @idModulo, 'Asignación de cuentas por módulo', '10015 - Mantenimiento de tblModuloCuentaPais', 1)
ON DUPLICATE KEY UPDATE
    idModulo = VALUES(idModulo),
    nombreAplicacion = VALUES(nombreAplicacion),
    descripcionAplicacion = VALUES(descripcionAplicacion),
    is_active = 1;
    
-- 1) Módulo Bancos
INSERT INTO `tblmodulo` (`idModulo`, `nombreModulo`, `descripcionModulo`, `is_active`)
VALUES (8000, 'Bancos', 'Modulo bancario del sistema de Enbutidos S.A.', 1)
ON DUPLICATE KEY UPDATE `is_active` = 1;

-- 2) Aplicaciones de Bancos (8001 a 8013)
INSERT INTO `tblaplicacion`
  (`idAplicacion`, `idModulo`, `nombreAplicacion`, `descripcionAplicacion`, `is_active`)
VALUES
(8001, 8000, 'Moneda',                                  'Mantenimiento de la moneda a utilizar.', 1),
(8002, 8000, 'Bancos',                                  'Mantenimiento del banco a utilizar.', 1),
(8003, 8000, 'Tipo de Cuenta Bancaria',                 'Mantenimiento del tipo de cuenta a utilizar.', 1),
(8004, 8000, 'Tipo de Transacción',                     'Mantenimiento del tipo de transacción a utilizar.', 1),
(8005, 8000, 'Cuenta Bancaria',                         'Mantenimiento de las cuentas a utilizar.', 1),
(8006, 8000, 'Movimiento Bancario',                     'Proceso para el registro de movimientos bancarios.', 1),
(8007, 8000, 'Conciliacion Bancaria',                   'Proceso para conciliar los movimientos con el estado de cuentas.', 1),
(8008, 8000, 'Enlace a Otros Modulos',                  'Proceso que permite la comunicacion con los demas modulos.', 1),
(8009, 8000, 'Disponibilidad Diaria',                   'Proceso que permite visualizar la disponibilidad del cliente.', 1),
(8010, 8000, 'Tipo de Cambio',                          'Proceso que permite visualizar el cambio de la moneda.', 1),
(8011, 8000, 'Proceso autorizacion de Orden de Compra', 'Proceso que autoriza las ordenes de las compras.', 1),
(8012, 8000, 'Proceso Generacion de Pago',              'Proceso que permite generar los pagos requeridos.', 1),
(8013, 8000, 'Cierre Bancario',                         'Proceso que culmina con los movimientos mensuales y anuales.', 1)
ON DUPLICATE KEY UPDATE
  `nombreAplicacion` = VALUES(`nombreAplicacion`),
  `descripcionAplicacion` = VALUES(`descripcionAplicacion`),
  `is_active` = 1;
  
  
-- 1. El módulo 3 pasa a ser el 7000 y recupera el nombre "Ventas"
UPDATE tblModulo
SET idModulo = 7000,
    nombreModulo = 'Ventas',
    descripcionModulo = 'Módulo de ventas: clientes, vendedores, listas de precio, cotizaciones, pedidos, facturación, cuentas por cobrar, recibos, depósitos, comisiones y cierres'
WHERE idModulo = 3;

-- Verifica que ya exista el 7000 antes de seguir
SELECT idModulo, nombreModulo FROM tblModulo;

-- 2. Aplicaciones del módulo 7000
INSERT INTO tblAplicacion (idAplicacion, idModulo, nombreAplicacion, descripcionAplicacion, is_active)
VALUES
(7000, 7000, 'Ventas',                    'Configuración general del módulo: correlativos de venta y cuentas contables de cuentas por cobrar', 1),
(7002, 7000, 'Listas de Precio',          'Gestiona los tipos de cliente, las listas de precio, los precios por tipo de cliente y su historial de modificaciones', 1),
(7003, 7000, 'Movimientos de CxC',        'Registro y consulta de los movimientos de cuentas por cobrar de los clientes', 1),
(7014, 7000, 'Notas de CxC',              'Gestiona las notas de crédito y débito de cuentas por cobrar', 1),
(7005, 7000, 'Recibos de Pago',           'Registra los recibos de pago de los clientes y su aplicación a las facturas', 1),
(7015, 7000, 'Depósitos de Venta',        'Registra los depósitos de las ventas con su detalle', 1),
(7011, 7000, 'Cotizaciones',              'Registra y da seguimiento a las cotizaciones enviadas a los clientes', 1),
(7004, 7000, 'Pedidos',                   'Registra y da seguimiento a los pedidos de los clientes', 1),
(7007, 7000, 'Facturación',               'Emisión y control de facturas de venta y versiones de facturación por cliente', 1),
(7013, 7000, 'Comisiones',                'Define reglas de comisión y calcula las comisiones de los vendedores sobre facturas y abonos', 1),
(7009, 7000, 'Catálogos de Ventas',       'Administra clientes, vendedores, productos de venta y formas de pago', 1),
(7010, 7000, 'Cierres de CxC',            'Cierre de períodos de cuentas por cobrar, saldos por cliente y traslados', 1) AS nuevo
ON DUPLICATE KEY UPDATE
    idModulo              = nuevo.idModulo,
    nombreAplicacion      = nuevo.nombreAplicacion,
    descripcionAplicacion = nuevo.descripcionAplicacion,
    is_active             = 1;
    


INSERT INTO tblmodulo
(idModulo, nombreModulo, descripcionModulo, is_active)
VALUES
(6000, 'Compras y Cuentas por Pagar', 'Gestión de compras y cuentas por pagar', 1);

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
      SELECT 1 FROM tblusuariorol ur WHERE ur.idUsuario = u.idUsuario
  );



INSERT INTO tblrolmoduloaplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion, derEditarRolModuloAplicacion, derEliminarRolModuloAplicacion, derImprimirRolModuloAplicacion)
SELECT ur.idRol, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblusuario u
JOIN tblusuariorol ur ON ur.idUsuario = u.idUsuario
JOIN tblmodulo m ON m.nombreModulo = 'Compras y Cuentas por Pagar'
JOIN tblaplicacion a ON
    (a.idModulo = m.idModulo
     AND a.idAplicacion BETWEEN 6000 AND 6014)
    OR (a.idModulo = 4 AND a.idAplicacion = 12)
WHERE u.nombreUsuario = 'imelendez'
  AND a.is_active = 1
ON DUPLICATE KEY UPDATE
    derInsertarRolModuloAplicacion = 1,
    derEditarRolModuloAplicacion = 1,
    derEliminarRolModuloAplicacion = 1,
    derImprimirRolModuloAplicacion = 1;

INSERT INTO tblusuariomoduloaplicacion
    (idUsuario, idModulo, idAplicacion,
     derInsertarUsuarioModuloAplicacion, derEditarUsuarioModuloAplicacion, derEliminarUsuarioModuloAplicacion, derImprimirUsuarioModuloAplicacion)
SELECT u.idUsuario, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblusuario u
JOIN tblmodulo m ON m.nombreModulo = 'Compras y Cuentas por Pagar'
JOIN tblaplicacion a ON
    (a.idModulo = m.idModulo
     AND a.idAplicacion BETWEEN 6000 AND 6014)
    OR (a.idModulo = 4 AND a.idAplicacion = 12)
WHERE u.nombreUsuario = 'imelendez'
  AND a.is_active = 1
ON DUPLICATE KEY UPDATE
    derInsertarUsuarioModuloAplicacion = 1,
    derEditarUsuarioModuloAplicacion = 1,
    derEliminarUsuarioModuloAplicacion = 1,
    derImprimirUsuarioModuloAplicacion = 1;

COMMIT;

SET SESSION SQL_SAFE_UPDATES = @compras_sql_safe_updates;

  
-- crear los empleado y usaruarioa de cada uno de los modulos 
INSERT INTO tblEmpleado
    (idEmpleado, codigoEmpleado, dpiEmpleado, nitEmpleado, nombresEmpleado, apellidosEmpleado,
     idDepartamento, idJornada, salarioEmpleado, generoEmpleado, fechaNacimientoEmpleado,
     fechaContratacionEmpleado, telefonoEmpleado, correoEmpleado) VALUES
(25, 'EMP-016', '0000000000001', '1234567-8', 'Modulo ', 'RRHH',    3, 5, 0000.0000, 'M', '1996-04-12', '2024-03-01', '55500001', 'recursoshumanos.terminus@gmail.com'),
(26, 'EMP-017', '0000000000002', '4589216-3', 'Modulo ', 'Bancos',  8, 5, 0000.0000, 'M', '1992-08-25', '2023-07-17', '55500002', 'terminus.bancoserp22026@gmail.com'),
(27, 'EMP-018', '0000000000003', '7823451-9', 'Modulo ',  'Ventas',   6, 5, 0000.0000, 'M', '1999-01-30', '2025-02-03', '55500003', 'terminus.ventas22026@gmail.com'),
(28, 'EMP-019', '0000000000004', '6158392-4', 'Modulo',  ' Compras',  9, 5, 0000.0000, 'M', '1994-11-09', '2022-11-14', '55500004', ' terminus.compras2206@gmail.com'),
(29, 'EMP-020', '0000000000005', '2947618-2', 'Modulo ', 'Logistica',   7, 5, 0000.0000, 'M', '2000-06-18', '2025-06-02', '55500005', 'terminus.logistica@gmail.com'),
(30, 'EMP-021', '0000000000006', '9361527-5', 'Modulo ', 'Contabilidad',   4, 5, 0000.0000, 'M', '2000-06-18', '2025-06-02', '55500006', 'terminus.conta22026@gmail.com'),
(31, 'EMP-022', '0000000000007', '9361034-6', 'risko ', 'Auditoria',   5, 5, 0000.0000, 'M', '2000-06-18', '2025-06-02', '55500007', 'risko22026@gmail.com');


INSERT INTO `tblusuario` VALUES 
(15,25,'userRRHH','$2b$10$e9CQKArQRmpf.IUtqpxYmOoO9vyh2./7ped56qEokwfgn7DkcacPC','2026-09-17 08:07:07',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),
(16,26,'userbancos','$2b$10$e9CQKArQRmpf.IUtqpxYmOoO9vyh2./7ped56qEokwfgn7DkcacPC','2026-09-17 08:07:07',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),
(17,27,'userventas','$2b$10$e9CQKArQRmpf.IUtqpxYmOoO9vyh2./7ped56qEokwfgn7DkcacPC','2026-09-17 08:07:07',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),
(18,28,'usercompras','$2b$10$e9CQKArQRmpf.IUtqpxYmOoO9vyh2./7ped56qEokwfgn7DkcacPC','2026-09-17 08:07:07',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),
(19,29,'userlogistica','$2b$10$e9CQKArQRmpf.IUtqpxYmOoO9vyh2./7ped56qEokwfgn7DkcacPC','2026-09-17 08:07:07',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),
(20,30,'usercontabilidad','$2b$10$e9CQKArQRmpf.IUtqpxYmOoO9vyh2./7ped56qEokwfgn7DkcacPC','2026-09-17 08:07:07',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),
(21,31,'userauditoria','$2b$10$e9CQKArQRmpf.IUtqpxYmOoO9vyh2./7ped56qEokwfgn7DkcacPC','2026-09-17 08:07:07',1,'2026-09-17 14:07:07','2026-09-17 14:07:07');

INSERT INTO `tblrol` VALUES 
(15,'Recursos Humanos','Rol con acceso a recursos humanos',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),
(16,'Bancos','Rol con acceso a bancos',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),
(17,'Ventas','Rol con acceso a ventas',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),
(18,'compras','Rol con acceso compras',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),
(19,'Contabilidad','Rol con acceso a contabilidad',1,'2026-09-17 14:07:07','2026-09-17 14:07:07'),
(20,'Logistica','Rol con acceso a logistica',1,'2026-09-17 14:07:07','2026-09-17 14:07:07');

INSERT INTO `tblusuariorol` VALUES 
(15,15,'2026-09-17','2026-09-17 14:07:07','2026-09-17 14:07:07'),
(16,16,'2026-09-17','2026-09-17 14:07:07','2026-09-17 14:07:07'),
(17,17,'2026-09-17','2026-09-17 14:07:07','2026-09-17 14:07:07'),
(18,18,'2026-09-17','2026-09-17 14:07:07','2026-09-17 14:07:07'),
(19,20,'2026-09-17','2026-09-17 14:07:07','2026-09-17 14:07:07'),
(20,19,'2026-09-17','2026-09-17 14:07:07','2026-09-17 14:07:07'),
(21,1,'2026-09-17','2026-09-17 14:07:07','2026-09-17 14:07:07');


  
-- permisos de ventas
-- y le puse permisos para todo con (insertar, editar, eliminar y leer) 1
INSERT INTO tblRolModuloAplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion,
     derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion,
     derImprimirRolModuloAplicacion)
SELECT 17, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblAplicacion a
WHERE a.idModulo = 7000
  AND NOT EXISTS (
        SELECT 1
        FROM tblRolModuloAplicacion rma
        WHERE rma.idRol = 17
          AND rma.idModulo = a.idModulo
          AND rma.idAplicacion = a.idAplicacion
  );

-- y le puse permisos para todo con (insertar, editar, eliminar y leer) 1
INSERT INTO tblRolModuloAplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion,
     derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion,
     derImprimirRolModuloAplicacion)
SELECT 1, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblAplicacion a
WHERE a.idModulo = 7000
  AND NOT EXISTS (
        SELECT 1
        FROM tblRolModuloAplicacion rma
        WHERE rma.idRol = 1
          AND rma.idModulo = a.idModulo
          AND rma.idAplicacion = a.idAplicacion
  );

-- permisos de resusosn humanos 
-- y le puse permisos para todo con (insertar, editar, eliminar y leer) 1
INSERT INTO tblRolModuloAplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion,
     derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion,
     derImprimirRolModuloAplicacion)
SELECT 15, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblAplicacion a
WHERE a.idModulo = 9000
  AND NOT EXISTS (
        SELECT 1
        FROM tblRolModuloAplicacion rma
        WHERE rma.idRol = 15
          AND rma.idModulo = a.idModulo
          AND rma.idAplicacion = a.idAplicacion
  );

-- y le puse permisos para todo con (insertar, editar, eliminar y leer) 1
INSERT INTO tblRolModuloAplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion,
     derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion,
     derImprimirRolModuloAplicacion)
SELECT 1, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblAplicacion a
WHERE a.idModulo = 9000
  AND NOT EXISTS (
        SELECT 1
        FROM tblRolModuloAplicacion rma
        WHERE rma.idRol = 1
          AND rma.idModulo = a.idModulo
          AND rma.idAplicacion = a.idAplicacion
  );
  
  -- permisos de Bancos
-- y le puse permisos para todo con (insertar, editar, eliminar y leer) 1
INSERT INTO tblRolModuloAplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion,
     derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion,
     derImprimirRolModuloAplicacion)
SELECT 16, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblAplicacion a
WHERE a.idModulo = 8000
  AND NOT EXISTS (
        SELECT 1
        FROM tblRolModuloAplicacion rma
        WHERE rma.idRol = 16
          AND rma.idModulo = a.idModulo
          AND rma.idAplicacion = a.idAplicacion
  );

-- y le puse permisos para todo con (insertar, editar, eliminar y leer) 1
INSERT INTO tblRolModuloAplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion,
     derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion,
     derImprimirRolModuloAplicacion)
SELECT 1, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblAplicacion a
WHERE a.idModulo = 8000
  AND NOT EXISTS (
        SELECT 1
        FROM tblRolModuloAplicacion rma
        WHERE rma.idRol = 1
          AND rma.idModulo = a.idModulo
          AND rma.idAplicacion = a.idAplicacion
  );
  
  
-- permisos de contabilidad
-- y le puse permisos para todo con (insertar, editar, eliminar y leer) 1
INSERT INTO tblRolModuloAplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion,
     derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion,
     derImprimirRolModuloAplicacion)
SELECT 19, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblAplicacion a
WHERE a.idModulo = 10000
  AND NOT EXISTS (
        SELECT 1
        FROM tblRolModuloAplicacion rma
        WHERE rma.idRol = 19
          AND rma.idModulo = a.idModulo
          AND rma.idAplicacion = a.idAplicacion
  );

-- y le puse permisos para todo con (insertar, editar, eliminar y leer) 1
INSERT INTO tblRolModuloAplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion,
     derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion,
     derImprimirRolModuloAplicacion)
SELECT 1, a.idModulo, a.idAplicacion, 1, 1, 1, 1
FROM tblAplicacion a
WHERE a.idModulo = 10000
  AND NOT EXISTS (
        SELECT 1
        FROM tblRolModuloAplicacion rma
        WHERE rma.idRol = 1
          AND rma.idModulo = a.idModulo
          AND rma.idAplicacion = a.idAplicacion
  );






