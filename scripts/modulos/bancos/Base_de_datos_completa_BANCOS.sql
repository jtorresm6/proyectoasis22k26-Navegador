-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: dbsistemaembutidos
-- ------------------------------------------------------
-- Server version	8.0.46



CREATE DATABASE IF NOT EXISTS dbsistemaembutidos
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE dbsistemaembutidos;

INSERT INTO tblrolmoduloaplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion, derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion, derImprimirRolModuloAplicacion)
VALUES
    (12, 8000, 8001, 1, 1, 1, 1),
    (14, 8000, 8001, 1, 1, 1, 1),
    (15, 8000, 8001, 1, 1, 1, 1)
ON DUPLICATE KEY UPDATE
    derInsertarRolModuloAplicacion = 1,
    derEditarRolModuloAplicacion = 1,
    derEliminarRolModuloAplicacion = 1,
    derImprimirRolModuloAplicacion = 1;





/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--


--
-- Table structure for table `tblaplicacion`
--

DROP TABLE IF EXISTS `tblaplicacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tblaplicacion` (
  `idAplicacion` int NOT NULL AUTO_INCREMENT,
  `idModulo` int NOT NULL,
  `nombreAplicacion` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcionAplicacion` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idAplicacion`),
  KEY `idxAplicacionIdModulo` (`idModulo`),
  CONSTRAINT `Fk_Aplicacion_Modulo` FOREIGN KEY (`idModulo`) REFERENCES `tblmodulo` (`idModulo`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8014 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblaplicacion`
--

LOCK TABLES `tblaplicacion` WRITE;
/*!40000 ALTER TABLE `tblaplicacion` DISABLE KEYS */;
INSERT INTO `tblaplicacion` VALUES (3,3,'Gestión de Pedidos','Registro y seguimiento de pedidos de clientes',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(4,4,'Empleados','Gestión de empleados del sistema',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(5,4,'Usuarios','Gestión de usuarios del sistema',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(6,4,'Módulos','Administración de módulos del sistema',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(7,4,'Aplicaciones','Administración de aplicaciones del sistema',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(8,4,'Perfiles','Administración de roles/perfiles',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(9,4,'Asignación de Perfiles','Asignación de roles a usuarios',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(10,4,'Asignación Aplicación Perfiles','Asignación de permisos por rol/módulo/aplicación',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(11,4,'Asignación Aplicación Usuario','Asignación de permisos por usuario/módulo/aplicación',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(12,4,'Bitácora','Consulta de bitácora del sistema',1,'2026-09-17 20:07:07','2026-09-20 08:48:38'),(15,4,'Prueba','Esto es una prueba controlada por parte de Seguridad.',1,'2026-09-24 05:46:12','2026-09-24 05:46:12'),(16,4,'Peliculas','SEGUNDO EXAMEN PARCIAL',1,'2026-09-29 15:32:08','2026-09-29 15:32:08'),(8001,8000,'Moneda','Mantenimiento de la moneda a utilizar.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8002,8000,'Bancos','Mantenimiento del banco a utilizar.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8003,8000,'Tipo de Cuenta Bancaria','Mantenimiento del tipo de cuenta a utilizar.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8004,8000,'Tipo de Transacción','Mantenimiento del tipo de transacción a utilizar.',0,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8005,8000,'Cuenta Bancaria','Mantenimiento de las cuentas a utilizar.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8006,8000,'Movimiento Bancario','Proceso para el registro de movimientos bancarios.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8007,8000,'Conciliacion Bancaria','Proceso para conciliar los movimientos con el estado de cuentas.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8008,8000,'Enlace a Otros Modulos','Proceso que permite la comunicacion con los demas modulos.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8009,8000,'Disponibilidad Diaria','Proceso que permite visualizar la disponibilidad del cliente.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8010,8000,'Tipo de Cambio','Proceso que permite visualizar el cambio de la moneda.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8011,8000,'Proceso autorizacion de Orden de Compra','Proceso que autoriza las ordenes de las compras.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8012,8000,'Proceso Generacion de Pago','Proceso que permite generar los pagos requeridos.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(8013,8000,'Cierre Bancario','Proceso que culmina con los movimientos mensuales y anuales.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00');
/*!40000 ALTER TABLE `tblaplicacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tblbitacora`
--

DROP TABLE IF EXISTS `tblbitacora`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tblbitacora` (
  `idBitacora` int NOT NULL AUTO_INCREMENT,
  `idUsuario` int DEFAULT NULL,
  `accionBitacora` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `tablaBitacora` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `idRegistroBitacora` int NOT NULL,
  `detallesBitacora` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ipBitacora` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `fechaHoraBitacora` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`idBitacora`),
  KEY `idxBitacoraIdUsuario` (`idUsuario`),
  KEY `idxBitacoraFechaHora` (`fechaHoraBitacora`),
  KEY `idxBitacoraTabla` (`tablaBitacora`),
  CONSTRAINT `Fk_Bitacora_Usuario` FOREIGN KEY (`idUsuario`) REFERENCES `tblusuario` (`idUsuario`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=412 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblbitacora`
--

LOCK TABLES `tblbitacora` WRITE;
/*!40000 ALTER TABLE `tblbitacora` DISABLE KEYS */;

INSERT INTO `tblbitacora` VALUES (1,1,'INSERT','tblEmpleado',1,'Alta de empleado de prueba','192.168.1.10','2026-09-17 08:07:07','2026-09-17 20:07:07'),(2,2,'UPDATE','tblAplicacion',2,'Actualización de datos de aplicación','192.168.1.11','2026-09-17 08:07:07','2026-09-17 20:07:07'),(3,3,'DELETE','tblUsuarioRol',3,'Eliminación de asignación de rol','192.168.1.12','2026-09-17 08:07:07','2026-09-17 20:07:07'),(4,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-17 08:07:53','2026-09-17 20:07:53'),(5,1,'INSERT','tblUsuarioRol',3,'Se asignó el perfil 2 al usuario 3','192.168.167.48','2026-09-17 08:09:00','2026-09-17 20:09:00'),(6,1,'INSERT','tblEmpleado',0,'Se agregó el empleado: Oscar Moralesl','192.168.167.48','2026-09-17 08:13:37','2026-09-17 20:13:37'),(7,1,'INSERT','tblUsuario',0,'Se agregó el usuario: SofiGoty','192.168.167.48','2026-09-17 08:14:24','2026-09-17 20:14:24'),(8,1,'INSERT','tblUsuarioRol',4,'Se asignó el perfil 1 al usuario 4','192.168.167.48','2026-09-17 08:14:33','2026-09-17 20:14:33'),(9,1,'LOGIN','tblUsuario',4,'Inicio de sesión exitoso del usuario: SofiGoty','192.168.167.48','2026-09-17 08:14:55','2026-09-17 20:14:55'),(10,1,'UPDATE','tblAsigAppPerf',1,'Se actualizaron permisos de aplicación 4 al rol 1','192.168.167.48','2026-09-17 08:16:33','2026-09-17 20:16:33'),(11,1,'LOGIN','tblUsuario',4,'Inicio de sesión exitoso del usuario: SofiGoty','192.168.167.48','2026-09-17 08:17:20','2026-09-17 20:17:20'),(12,1,'UPDATE','tblAsigAppPerf',1,'Se actualizaron permisos de aplicación 4 al rol 1','192.168.167.48','2026-09-17 08:19:43','2026-09-17 20:19:43'),(13,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-17 08:22:18','2026-09-17 20:22:18'),(14,1,'DELETE','tblEmpleado',4,'Se eliminó el empleado ID: 4','192.168.167.48','2026-09-17 08:23:26','2026-09-17 20:23:26'),(15,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-17 08:27:14','2026-09-17 20:27:14'),(16,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-17 13:53:54','2026-09-18 01:53:54'),(17,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-17 14:04:42','2026-09-18 02:04:42'),(18,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-17 19:21:21','2026-09-18 07:21:21'),(19,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-17 19:51:48','2026-09-18 07:51:48'),(20,1,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.11','2026-09-17 19:53:15','2026-09-18 07:53:16'),(21,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-17 19:53:42','2026-09-18 07:53:42'),(22,1,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.11','2026-09-18 08:24:51','2026-09-18 20:24:51'),(23,NULL,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.11','2026-09-18 08:44:12','2026-09-18 20:44:12'),(24,NULL,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 08:44:48','2026-09-18 20:44:48'),(25,NULL,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.11','2026-09-18 08:58:45','2026-09-18 20:58:45'),(26,NULL,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 08:58:59','2026-09-18 20:58:59'),(27,NULL,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 09:04:20','2026-09-18 21:04:20'),(28,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 09:07:48','2026-09-18 21:07:48'),(29,2,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.11','2026-09-18 09:08:15','2026-09-18 21:08:15'),(30,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 09:08:38','2026-09-18 21:08:38'),(31,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 09:17:02','2026-09-18 21:17:02'),(32,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 11:41:10','2026-09-18 23:41:10'),(33,1,'INSERT','tblEmpleado',0,'Se agregó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-18 11:48:58','2026-09-18 23:48:58'),(34,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-18 11:49:13','2026-09-18 23:49:13'),(35,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-18 11:49:54','2026-09-18 23:49:55'),(36,1,'INSERT','tblEmpleado',0,'Se agregó el empleado: dfdsf dsfdsf','192.168.1.11','2026-09-18 11:58:46','2026-09-18 23:58:46'),(37,1,'UPDATE','tblEmpleado',8,'Se actualizó el empleado: dfdsf dsfdsf','192.168.1.11','2026-09-18 11:59:21','2026-09-18 23:59:21'),(38,1,'DELETE','tblEmpleado',8,'Se eliminó el empleado ID: 8','192.168.1.11','2026-09-18 11:59:25','2026-09-18 23:59:25'),(39,1,'INSERT','tblUsuario',0,'Se agregó el usuario: Omorales','192.168.1.11','2026-09-18 12:02:42','2026-09-19 00:02:42'),(40,1,'INSERT','tblModulo',0,'Se agregó el módulo: Compras','192.168.1.11','2026-09-18 12:10:26','2026-09-19 00:10:26'),(41,1,'UPDATE','tblModulo',5,'Se actualizó el módulo: Compras','192.168.1.11','2026-09-18 12:10:33','2026-09-19 00:10:33'),(42,1,'DELETE','tblModulo',5,'Se eliminó el módulo ID: 5','192.168.1.11','2026-09-18 12:10:38','2026-09-19 00:10:38'),(43,1,'INSERT','tblModulo',5,'Se agregó el módulo: Comprasdfd','192.168.1.11','2026-09-18 12:10:52','2026-09-19 00:10:53'),(44,1,'UPDATE','tblModulo',6,'Se actualizó el módulo: Comprasdfd','192.168.1.11','2026-09-18 12:10:56','2026-09-19 00:10:56'),(45,1,'UPDATE','tblModulo',4,'Se actualizó el módulo: Seguridad','192.168.1.11','2026-09-18 12:11:19','2026-09-19 00:11:19'),(46,1,'DELETE','tblModulo',6,'Se eliminó el módulo ID: 6','192.168.1.11','2026-09-18 12:13:28','2026-09-19 00:13:28'),(47,1,'INSERT','tblAplicacion',0,'Se agregó la aplicación: fdsfsdfdsf','192.168.1.11','2026-09-18 12:24:05','2026-09-19 00:24:05'),(48,1,'UPDATE','tblAplicacion',13,'Se actualizó la aplicación: fdsfsdfds','192.168.1.11','2026-09-18 12:24:20','2026-09-19 00:24:20'),(49,1,'DELETE','tblAplicacion',13,'Se eliminó la aplicación ID: 13','192.168.1.11','2026-09-18 12:24:25','2026-09-19 00:24:25'),(50,1,'INSERT','tblAplicacion',13,'Se agregó la aplicación: Perfiles','192.168.1.11','2026-09-18 12:24:41','2026-09-19 00:24:41'),(51,1,'INSERT','tblModulo',0,'Se agregó el módulo: dsolpkojdfkdsf','192.168.1.11','2026-09-18 12:26:47','2026-09-19 00:26:47'),(52,1,'DELETE','tblModulo',8,'Se eliminó el módulo ID: 8','192.168.1.11','2026-09-18 12:26:51','2026-09-19 00:26:51'),(53,1,'DELETE','tblAplicacion',14,'Se eliminó la aplicación ID: 14','192.168.1.11','2026-09-18 12:27:35','2026-09-19 00:27:35'),(54,1,'INSERT','tblRol',0,'Se agregó el perfil: Probador','192.168.1.11','2026-09-18 12:30:24','2026-09-19 00:30:24'),(55,1,'UPDATE','tblRol',5,'Se actualizó el perfil: Probador','192.168.1.11','2026-09-18 12:33:51','2026-09-19 00:33:51'),(56,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 12:42:44','2026-09-19 00:42:44'),(57,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 12:43:19','2026-09-19 00:43:19'),(58,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 12:43:56','2026-09-19 00:43:56'),(59,1,'DELETE','tblAsigAppPerf',3,'Se eliminó asignación de aplicación 3 al rol 3','192.168.1.11','2026-09-18 12:44:12','2026-09-19 00:44:12'),(60,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 12:45:34','2026-09-19 00:45:34'),(61,1,'UPDATE','tblAsigAppPerf',2,'Se actualizaron permisos de aplicación 2 al rol 2','192.168.1.11','2026-09-18 12:46:57','2026-09-19 00:46:57'),(62,1,'DELETE','tblAsigAppPerf',2,'Se eliminó asignación de aplicación 5 al rol 2','192.168.1.11','2026-09-18 12:47:07','2026-09-19 00:47:07'),(63,1,'INSERT','tblAsigAppUsuario',1,'Se asignó aplicación 1 al módulo 1','192.168.1.11','2026-09-18 12:48:09','2026-09-19 00:48:09'),(64,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 15:40:45','2026-09-19 03:40:45'),(65,1,'UPDATE','tblAsigAppPerf',1,'Se actualizaron permisos de aplicación 1 al rol 1','192.168.1.11','2026-09-18 15:41:54','2026-09-19 03:41:54'),(66,1,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 15:44:36','2026-09-19 03:44:36'),(67,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 15:44:58','2026-09-19 03:44:59'),(68,1,'INSERT','tblRol',0,'Se agregó el perfil: Pruebas','192.168.1.11','2026-09-18 15:45:38','2026-09-19 03:45:38'),(69,1,'DELETE','tblRol',5,'Se eliminó el rol perfil: 5','192.168.1.11','2026-09-18 15:45:45','2026-09-19 03:45:45'),(70,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 15:46:11','2026-09-19 03:46:11'),(71,1,'INSERT','tblUsuarioRol',5,'Se asignó el perfil 3 al usuario 5','192.168.1.11','2026-09-18 15:56:26','2026-09-19 03:56:27'),(72,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 11 al rol 3','192.168.1.11','2026-09-18 15:57:29','2026-09-19 03:57:29'),(73,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 10 al rol 3','192.168.1.11','2026-09-18 15:57:37','2026-09-19 03:57:37'),(74,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 8 al rol 3','192.168.1.11','2026-09-18 15:57:43','2026-09-19 03:57:43'),(75,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 7 al rol 3','192.168.1.11','2026-09-18 15:57:51','2026-09-19 03:57:51'),(76,1,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 15:58:13','2026-09-19 03:58:13'),(77,1,'INSERT','tblAsigAppPerf',1,'Se asignó aplicación 1 al rol 1','192.168.1.11','2026-09-18 15:58:39','2026-09-19 03:58:39'),(78,1,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 16:13:17','2026-09-19 04:13:17'),(79,1,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 17:43:22','2026-09-19 05:43:22'),(80,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 5 al rol 3','192.168.1.11','2026-09-18 17:44:16','2026-09-19 05:44:16'),(81,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 6 al rol 3','192.168.1.11','2026-09-18 17:44:33','2026-09-19 05:44:33'),(82,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 4 al rol 3','192.168.1.11','2026-09-18 17:44:53','2026-09-19 05:44:53'),(83,NULL,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 18:19:05','2026-09-19 06:19:05'),(84,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 18:19:24','2026-09-19 06:19:24'),(85,NULL,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 18:20:11','2026-09-19 06:20:11'),(86,NULL,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-18 18:20:32','2026-09-19 06:20:32'),(87,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 18:20:48','2026-09-19 06:20:48'),(88,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 19:58:29','2026-09-19 07:58:29'),(89,NULL,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 20:25:30','2026-09-19 08:25:30'),(90,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 20:38:01','2026-09-19 08:38:01'),(91,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-18 22:15:35','2026-09-19 10:15:35'),(92,1,'INSERT','tblAsigAppPerf',3,'Se asignó aplicación 12 al rol 3','192.168.1.11','2026-09-18 22:16:49','2026-09-19 10:16:49'),(93,NULL,'LOGIN','tblUsuario',5,'Inicio de sesión exitoso del usuario: Omorales','192.168.1.11','2026-09-18 22:17:09','2026-09-19 10:17:09'),(94,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 09:19:13','2026-09-19 21:19:13'),(95,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 16:24:37','2026-09-20 04:24:37'),(96,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 16:26:02','2026-09-20 04:26:02'),(97,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 16:45:59','2026-09-20 04:45:59'),(98,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-19 16:46:27','2026-09-20 04:46:27'),(99,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-19 16:46:42','2026-09-20 04:46:42'),(100,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-19 16:47:25','2026-09-20 04:47:25'),(101,1,'INSERT','tblEmpleado',5,'Se agregó el empleado: Pepito Muralles','192.168.1.11','2026-09-19 16:49:06','2026-09-20 04:49:06'),(102,1,'UPDATE','tblEmpleado',12,'Se actualizó el empleado: Pepito Muralles','192.168.1.11','2026-09-19 16:49:29','2026-09-20 04:49:29'),(103,1,'UPDATE','tblEmpleado',5,'Se actualizó el empleado: Oscar Emilio Morales Lemus','192.168.1.11','2026-09-19 16:50:24','2026-09-20 04:50:24'),(104,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 20:46:49','2026-09-20 08:46:49'),(105,1,'UPDATE','tblEmpleado',12,'Se actualizó el empleado: Pepito Muralles','192.168.1.11','2026-09-19 20:47:08','2026-09-20 08:47:08'),(106,1,'UPDATE','tblAplicacion',12,'Se actualizó la aplicación: Bitácora','192.168.1.11','2026-09-19 20:48:28','2026-09-20 08:48:28'),(107,1,'UPDATE','tblAplicacion',12,'Se actualizó la aplicación: Bitácora','192.168.1.11','2026-09-19 20:48:38','2026-09-20 08:48:38'),(108,1,'DELETE','tblAplicacion',1,'Se eliminó la aplicación ID: 1','192.168.1.11','2026-09-19 20:48:52','2026-09-20 08:48:52'),(109,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 20:49:17','2026-09-20 08:49:17'),(110,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 22:01:50','2026-09-20 10:01:50'),(111,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 22:33:27','2026-09-20 10:33:27'),(112,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 22:35:23','2026-09-20 10:35:23'),(113,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 22:36:26','2026-09-20 10:36:26'),(114,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 22:55:23','2026-09-20 10:55:23'),(115,1,'DELETE','tblRol',3,'Se eliminó el rol perfil: 3','192.168.1.11','2026-09-19 22:56:55','2026-09-20 10:56:55'),(116,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:25:33','2026-09-20 11:25:33'),(117,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:27:56','2026-09-20 11:27:56'),(118,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:28:33','2026-09-20 11:28:33'),(119,1,'UPDATE','tblModulo',2,'Se actualizó el módulo: Inventario','192.168.1.11','2026-09-19 23:28:56','2026-09-20 11:28:56'),(120,1,'UPDATE','tblModulo',2,'Se actualizó el módulo: Inventario','192.168.1.11','2026-09-19 23:29:01','2026-09-20 11:29:01'),(121,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:30:10','2026-09-20 11:30:10'),(122,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:43:23','2026-09-20 11:43:23'),(123,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:48:49','2026-09-20 11:48:49'),(124,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:50:17','2026-09-20 11:50:17'),(125,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:52:57','2026-09-20 11:52:58'),(126,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-19 23:54:44','2026-09-20 11:54:44'),(127,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:05:44','2026-09-20 12:05:44'),(128,1,'DELETE','tblAplicacion',2,'Se eliminó la aplicación ID: 2','192.168.1.11','2026-09-20 00:06:15','2026-09-20 12:06:15'),(129,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:11:20','2026-09-20 12:11:20'),(130,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:14:04','2026-09-20 12:14:04'),(131,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:17:19','2026-09-20 12:17:19'),(132,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:22:42','2026-09-20 12:22:42'),(133,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.11','2026-09-20 00:26:44','2026-09-20 12:26:44'),(134,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','172.20.10.4','2026-09-20 00:37:21','2026-09-20 12:37:21'),(135,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','172.20.10.4','2026-09-20 00:39:27','2026-09-20 12:39:27'),(136,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:40:49','2026-09-20 20:40:49'),(137,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:43:23','2026-09-20 20:43:23'),(138,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:44:51','2026-09-20 20:44:51'),(139,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:46:20','2026-09-20 20:46:20'),(140,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:47:12','2026-09-20 20:47:12'),(141,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:55:47','2026-09-20 20:55:48'),(142,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:57:12','2026-09-20 20:57:12'),(143,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:58:00','2026-09-20 20:58:00'),(144,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:58:46','2026-09-20 20:58:46'),(145,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 08:59:38','2026-09-20 20:59:38'),(146,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:14:45','2026-09-20 21:14:45'),(147,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:16:11','2026-09-20 21:16:11'),(148,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:26:36','2026-09-20 21:26:36'),(149,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:29:58','2026-09-20 21:29:58'),(150,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:31:54','2026-09-20 21:31:54'),(151,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:34:18','2026-09-20 21:34:18'),(152,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:35:27','2026-09-20 21:35:27'),(153,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:39:39','2026-09-20 21:39:39'),(154,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:42:23','2026-09-20 21:42:23'),(155,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:54:49','2026-09-20 21:54:49'),(156,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:57:32','2026-09-20 21:57:32'),(157,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 09:59:16','2026-09-20 21:59:16'),(158,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 10:04:06','2026-09-20 22:04:06'),(159,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 11:47:28','2026-09-20 23:47:28'),(160,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 12:23:34','2026-09-21 00:23:34'),(161,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 15:35:11','2026-09-21 03:35:11'),(162,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 15:41:55','2026-09-21 03:41:55'),(163,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 18:11:00','2026-09-21 06:11:00'),(164,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 18:14:17','2026-09-21 06:14:17'),(165,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 19:52:31','2026-09-21 07:52:31'),(166,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 20:24:57','2026-09-21 08:24:58'),(167,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-20 21:39:02','2026-09-21 09:39:02'),(168,1,'INSERT','tblUsuario',0,'Se agregó el usuario: Ppito','192.168.1.2','2026-09-20 21:39:53','2026-09-21 09:39:53'),(169,1,'UPDATE','tblUsuario',9,'Se actualizó el usuario: Ppito','192.168.1.2','2026-09-20 21:40:30','2026-09-21 09:40:30'),(170,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 15:21:33','2026-09-22 03:21:33'),(171,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 22:04:15','2026-09-22 10:04:15'),(172,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 22:09:15','2026-09-22 10:09:15'),(173,1,'UPDATE','tblUsuario',9,'Se actualizó el usuario: Ppitote','192.168.1.2','2026-09-21 22:09:53','2026-09-22 10:09:53'),(174,1,'DELETE','tblUsuario',9,'Se eliminó el usuario ID: 9','192.168.1.2','2026-09-21 22:09:59','2026-09-22 10:10:00'),(175,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 22:11:32','2026-09-22 10:11:32'),(176,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 22:24:51','2026-09-22 10:24:51'),(177,1,'DELETE','tblUsuario',5,'Se eliminó el usuario ID: 5','192.168.1.2','2026-09-21 22:25:15','2026-09-22 10:25:15'),(178,1,'DELETE','tblEmpleado',5,'Se eliminó el empleado ID: 5','192.168.1.2','2026-09-21 22:25:25','2026-09-22 10:25:25'),(179,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-21 22:29:12','2026-09-22 10:29:12'),(180,1,'INSERT','tblEmpleado',0,'Se agregó el empleado: Oscar Emilio Morales Lemus','192.168.1.2','2026-09-21 22:30:17','2026-09-22 10:30:17'),(181,1,'INSERT','tblUsuario',0,'Se agregó el usuario: Omorales','192.168.1.2','2026-09-21 22:30:38','2026-09-22 10:30:38'),(182,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-22 06:47:05','2026-09-22 18:47:05'),(183,1,'DELETE','tblUsuario',11,'Se eliminó el usuario ID: 11','192.168.167.48','2026-09-22 06:48:42','2026-09-22 18:48:42'),(184,1,'DELETE','tblEmpleado',12,'Se eliminó el empleado ID: 12','192.168.167.48','2026-09-22 06:50:50','2026-09-22 18:50:51'),(185,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-22 07:08:29','2026-09-22 19:08:29'),(186,1,'INSERT','tblEmpleado',0,'Se agregó el empleado: Empleado Examen','192.168.167.48','2026-09-22 07:11:30','2026-09-22 19:11:31'),(187,1,'INSERT','tblUsuario',0,'Se agregó el usuario: UsuarioExamen','192.168.167.48','2026-09-22 07:12:30','2026-09-22 19:12:30'),(188,1,'INSERT','tblRol',0,'Se agregó el perfil: PerfilExamen','192.168.167.48','2026-09-22 07:16:29','2026-09-22 19:16:29'),(189,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.167.48','2026-09-22 07:17:23','2026-09-22 19:17:23'),(190,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-22 07:19:39','2026-09-22 19:19:39'),(191,1,'INSERT','tblAsigAppPerf',12,'Se asignó aplicación 8 al rol 12','192.168.167.48','2026-09-22 07:20:38','2026-09-22 19:20:39'),(192,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.167.48','2026-09-22 07:21:11','2026-09-22 19:21:11'),(193,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-22 07:21:45','2026-09-22 19:21:45'),(194,1,'INSERT','tblUsuarioRol',12,'Se asignó el perfil 12 al usuario 12','192.168.167.48','2026-09-22 07:22:32','2026-09-22 19:22:32'),(195,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.167.48','2026-09-22 07:22:48','2026-09-22 19:22:48'),(196,12,'INSERT','tblRol',0,'Se agregó el perfil: Prueba','192.168.167.48','2026-09-22 07:25:13','2026-09-22 19:25:13'),(197,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-22 07:26:54','2026-09-22 19:26:54'),(198,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 16:22:37','2026-09-23 04:22:37'),(199,1,'INSERT','tblModulo',0,'Se agregó el módulo: Contabilidad','192.168.1.2','2026-09-22 16:24:31','2026-09-23 04:24:31'),(200,1,'UPDATE','tblModulo',12,'Se actualizó el módulo: Contabilidad','192.168.1.2','2026-09-22 16:26:01','2026-09-23 04:26:01'),(201,1,'DELETE','tblModulo',12,'Se eliminó el módulo ID: 12','192.168.1.2','2026-09-22 16:28:47','2026-09-23 04:28:47'),(202,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 16:57:01','2026-09-23 04:57:01'),(203,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.1.2','2026-09-22 16:57:27','2026-09-23 04:57:27'),(204,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 17:48:41','2026-09-23 05:48:41'),(205,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 17:49:18','2026-09-23 05:49:18'),(206,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 17:52:57','2026-09-23 05:52:57'),(207,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 18:07:57','2026-09-23 06:07:57'),(208,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 18:09:35','2026-09-23 06:09:35'),(209,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.1.2','2026-09-22 18:10:11','2026-09-23 06:10:11'),(210,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 19:16:51','2026-09-23 07:16:51'),(211,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 20:21:32','2026-09-23 08:21:32'),(212,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 20:28:21','2026-09-23 08:28:21'),(213,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 20:39:51','2026-09-23 08:39:51'),(214,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 21:38:32','2026-09-23 09:38:32'),(215,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:32:12','2026-09-23 11:32:12'),(216,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.1.2','2026-09-22 23:33:18','2026-09-23 11:33:18'),(217,2,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.2','2026-09-22 23:33:42','2026-09-23 11:33:42'),(218,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:34:17','2026-09-23 11:34:17'),(219,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.1.2','2026-09-22 23:38:03','2026-09-23 11:38:03'),(220,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:42:18','2026-09-23 11:42:18'),(221,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:43:12','2026-09-23 11:43:12'),(222,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:44:58','2026-09-23 11:44:58'),(223,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:47:01','2026-09-23 11:47:01'),(224,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:48:54','2026-09-23 11:48:54'),(225,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:51:43','2026-09-23 11:51:43'),(226,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:52:55','2026-09-23 11:52:55'),(227,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:53:56','2026-09-23 11:53:56'),(228,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:56:37','2026-09-23 11:56:37'),(229,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:57:34','2026-09-23 11:57:34'),(230,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-22 23:58:23','2026-09-23 11:58:23'),(231,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 00:00:00','2026-09-23 12:00:00'),(232,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 00:01:45','2026-09-23 12:01:45'),(233,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 00:03:49','2026-09-23 12:03:49'),(234,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 00:15:17','2026-09-23 12:15:17'),(235,12,'LOGIN','tblUsuario',12,'Inicio de sesión exitoso del usuario: UsuarioExamen','192.168.1.2','2026-09-23 00:15:49','2026-09-23 12:15:49'),(236,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 17:32:40','2026-09-24 05:32:40'),(237,1,'UPDATE','tblEmpleado',1,'Se actualizó el empleado: Isabel Meléndez','192.168.1.2','2026-09-23 17:40:07','2026-09-24 05:40:07'),(238,1,'INSERT','tblEmpleado',1,'Se agregó el empleado: Pruebas Capacitacion','192.168.1.2','2026-09-23 17:42:57','2026-09-24 05:42:57'),(239,1,'INSERT','tblUsuario',0,'Se agregó el usuario: Pcapacitacion','192.168.1.2','2026-09-23 17:43:51','2026-09-24 05:43:51'),(240,1,'INSERT','tblModulo',0,'Se agregó el módulo: Pruebas','192.168.1.2','2026-09-23 17:44:26','2026-09-24 05:44:26'),(241,1,'INSERT','tblAplicacion',0,'Se agregó la aplicación: Prueba','192.168.1.2','2026-09-23 17:46:12','2026-09-24 05:46:12'),(242,1,'DELETE','tblRol',13,'Se eliminó el rol perfil: 13','192.168.1.2','2026-09-23 17:46:19','2026-09-24 05:46:19'),(243,1,'UPDATE','tblRol',1,'Se actualizó el perfil: Administrador','192.168.1.2','2026-09-23 17:46:22','2026-09-24 05:46:22'),(244,1,'INSERT','tblRol',1,'Se agregó el perfil: Prueba','192.168.1.2','2026-09-23 17:47:34','2026-09-24 05:47:34'),(245,1,'INSERT','tblUsuarioRol',13,'Se asignó el perfil 14 al usuario 13','192.168.1.2','2026-09-23 17:47:47','2026-09-24 05:47:47'),(246,1,'INSERT','tblAsigAppPerf',14,'Se asignó aplicación 15 al rol 14','192.168.1.2','2026-09-23 17:48:40','2026-09-24 05:48:40'),(247,1,'INSERT','tblAsigAppUsuario',13,'Se asignó aplicación 15 al módulo 13','192.168.1.2','2026-09-23 17:49:15','2026-09-24 05:49:15'),(248,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.2','2026-09-23 19:44:06','2026-09-24 07:44:07'),(249,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-29 08:49:29','2026-09-29 14:49:29'),(250,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-29 08:53:11','2026-09-29 14:53:11'),(251,1,'INSERT','tblempleado',0,'Se insertó un registro en tblempleado: codigoEmpleado: EMP-011\ndpiEmpleado: 3142534231425\nnitEmpleado: 66554433-6\nnombresEmpleado: examenpkd\napellidosEmpleado: SegundoParcial\npuestoEmpleado: Examen\ngeneroEmpleado: M\nfechaNacimientoEmpleado: 2005-05-28\nfechaContratacionEmpleado: 2026-09-29\ntelefonoEmpleado: 77559933\ncorreoEmpleado: examensegundo@gmail.com\nis_active: 1\ncreated_at: 2026-09-29\nupdated_at: 2026-09-29\n','192.168.167.48','2026-09-29 08:54:47','2026-09-29 14:54:47'),(252,1,'INSERT','tblUsuario',0,'Se agregó el usuario: examenpkd','192.168.167.48','2026-09-29 08:55:29','2026-09-29 14:55:29'),(253,1,'INSERT','tblRol',0,'Se agregó el perfil: perfilpkd','192.168.167.48','2026-09-29 08:56:56','2026-09-29 14:56:56'),(254,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-29 09:31:31','2026-09-29 15:31:31'),(255,1,'INSERT','tblAplicacion',0,'Se agregó la aplicación: Peliculas','192.168.167.48','2026-09-29 09:32:09','2026-09-29 15:32:09'),(256,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-29 09:35:41','2026-09-29 15:35:41'),(257,1,'INSERT','tblAsigAppPerf',15,'Se asignó aplicación 16 al rol 15','192.168.167.48','2026-09-29 09:36:13','2026-09-29 15:36:14'),(258,1,'INSERT','tblAsigAppPerf',15,'Se asignó aplicación 12 al rol 15','192.168.167.48','2026-09-29 09:36:22','2026-09-29 15:36:22'),(259,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-29 09:36:28','2026-09-29 15:36:28'),(260,14,'LOGIN','tblUsuario',14,'Inicio de sesión exitoso del usuario: examenpkd','192.168.167.48','2026-09-29 09:36:50','2026-09-29 15:36:50'),(261,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-09-29 09:37:17','2026-09-29 15:37:17'),(262,1,'UPDATE','tblRol',15,'Se actualizó el perfil: perfilpkd','192.168.167.48','2026-09-29 09:37:47','2026-09-29 15:37:47'),(263,1,'UPDATE','tblRol',15,'Se actualizó el perfil: perfilpkd','192.168.167.48','2026-09-29 09:37:56','2026-09-29 15:37:56'),(264,1,'INSERT','tblUsuarioRol',14,'Se asignó el perfil 15 al usuario 14','192.168.167.48','2026-09-29 09:38:04','2026-09-29 15:38:04'),(265,14,'LOGIN','tblUsuario',14,'Inicio de sesión exitoso del usuario: examenpkd','192.168.167.48','2026-09-29 09:38:18','2026-09-29 15:38:18'),(266,14,'LOGIN','tblUsuario',14,'Inicio de sesión exitoso del usuario: examenpkd','192.168.167.48','2026-09-29 09:44:09','2026-09-29 15:44:10'),(267,14,'INSERT','Peliculas',1,'Se insertó un registro en Peliculas: idPeliculas: 1\nNombre: Nemo\nClasificacion: Todas Edades\nGenero: Aventura\nSubtitulado: Si\nIdioma: Ingles\nPrecio: 25\n','192.168.167.48','2026-09-29 09:45:12','2026-09-29 15:45:12'),(268,14,'LOGIN','tblUsuario',14,'Inicio de sesión exitoso del usuario: examenpkd','192.168.167.48','2026-09-29 09:54:22','2026-09-29 15:54:22'),(269,14,'INSERT','Peliculas',2,'Se insertó un registro en Peliculas: idPeliculas: 2\nNombre: IronMan\nClasificacion: PR\nGenero: Accion\nSubtitulado: Si\nIdioma: Portugues\nPrecio: 30\n','192.168.167.48','2026-09-29 09:55:16','2026-09-29 15:55:16'),(270,14,'INSERT','Peliculas',3,'Se insertó un registro en Peliculas: idPeliculas: 3\nNombre: Terminator\nClasificacion: PR\nGenero: M\nSubtitulado: Si\nIdioma: Español\nPrecio: 25\n','192.168.167.48','2026-09-29 09:55:40','2026-09-29 15:55:40'),(271,14,'INSERT','Peliculas',4,'Se insertó un registro en Peliculas: idPeliculas: 4\nNombre: RapidosyFuriosos\nClasificacion: PR\nGenero: Accion\nSubtitulado: Si\nIdioma: Ingles\nPrecio: 35\n','192.168.167.48','2026-09-29 09:56:09','2026-09-29 15:56:09'),(272,14,'DELETE','Peliculas',4,'Se eliminó un registro de Peliculas.','192.168.167.48','2026-09-29 09:56:18','2026-09-29 15:56:19'),(273,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-10 23:05:16','2026-10-11 05:05:17'),(274,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-10 23:49:23','2026-10-11 05:49:23'),(275,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-10 23:49:35','2026-10-11 05:49:35'),(276,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-10 23:50:22','2026-10-11 05:50:22'),(277,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-10 23:50:35','2026-10-11 05:50:35'),(278,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-10 23:54:43','2026-10-11 05:54:43'),(279,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-10 23:55:34','2026-10-11 05:55:34'),(280,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-10 23:55:48','2026-10-11 05:55:48'),(281,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-10 23:57:22','2026-10-11 05:57:22'),(282,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-10 23:57:34','2026-10-11 05:57:35'),(283,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:03:32','2026-10-11 07:03:32'),(284,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:03:52','2026-10-11 07:03:52'),(285,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:05:33','2026-10-11 07:05:33'),(286,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:05:47','2026-10-11 07:05:47'),(287,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:06:01','2026-10-11 07:06:01'),(288,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:07:32','2026-10-11 07:07:32'),(289,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:08:59','2026-10-11 07:08:59'),(290,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:10:11','2026-10-11 07:10:11'),(291,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:12:52','2026-10-11 07:12:52'),(292,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:23:03','2026-10-11 07:23:03'),(293,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:23:25','2026-10-11 07:23:25'),(294,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:23:58','2026-10-11 07:23:58'),(295,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:29:44','2026-10-11 07:29:44'),(296,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 01:30:23','2026-10-11 07:30:23'),(297,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-11 12:55:04','2026-10-11 18:55:04'),(298,1,'INSERT','tblAsigAppPerf',2,'Se asignó aplicación 7 al rol 2','192.168.1.4','2026-10-11 12:55:49','2026-10-11 18:55:49'),(299,1,'INSERT','tblAsigAppPerf',2,'Se asignó aplicación 10 al rol 2','192.168.1.4','2026-10-11 12:56:30','2026-10-11 18:56:30'),(300,1,'INSERT','tblAsigAppPerf',2,'Se asignó aplicación 6 al rol 2','192.168.1.4','2026-10-11 12:56:50','2026-10-11 18:56:50'),(301,2,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.4','2026-10-11 12:56:59','2026-10-11 18:57:00'),(302,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-12 09:14:47','2026-10-12 15:14:47'),(303,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-12 09:15:00','2026-10-12 15:15:00'),(304,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-12 09:15:12','2026-10-12 15:15:12'),(305,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-13 06:29:34','2026-10-13 12:29:34'),(306,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-13 06:29:53','2026-10-13 12:29:53'),(307,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-13 16:20:38','2026-10-13 22:20:38'),(308,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-13 16:34:05','2026-10-13 22:34:05'),(309,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-13 17:35:43','2026-10-13 23:35:43'),(310,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.4','2026-10-13 19:56:14','2026-10-14 01:56:14'),(311,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-14 07:22:58','2026-10-14 13:22:58'),(312,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-14 08:44:45','2026-10-14 14:44:45'),(313,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-14 08:46:49','2026-10-14 14:46:50'),(314,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','172.20.10.3','2026-10-15 07:54:38','2026-10-15 13:54:38'),(315,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','172.20.10.3','2026-10-15 07:55:50','2026-10-15 13:55:50'),(316,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','172.20.10.3','2026-10-15 07:59:42','2026-10-15 13:59:42'),(317,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','172.20.10.3','2026-10-15 08:02:13','2026-10-15 14:02:14'),(318,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','172.20.10.3','2026-10-15 08:05:30','2026-10-15 14:05:30'),(319,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','172.20.10.3','2026-10-15 08:07:25','2026-10-15 14:07:25'),(320,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','172.20.10.3','2026-10-15 08:11:41','2026-10-15 14:11:41'),(321,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-16 10:05:49','2026-10-16 16:05:49'),(322,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-16 10:06:00','2026-10-16 16:06:00'),(323,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-16 10:08:07','2026-10-16 16:08:07'),(324,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-16 10:08:28','2026-10-16 16:08:28'),(325,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-16 10:17:29','2026-10-16 16:17:29'),(326,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-16 10:17:56','2026-10-16 16:17:56'),(327,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-16 10:24:45','2026-10-16 16:24:45'),(328,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.167.48','2026-10-16 10:24:58','2026-10-16 16:24:58'),(329,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-16 15:57:54','2026-10-16 21:57:54'),(330,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 14:48:38','2026-10-17 20:48:38'),(331,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 15:35:15','2026-10-17 21:35:15'),(332,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 16:03:39','2026-10-17 22:03:39'),(333,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 16:19:06','2026-10-17 22:19:06'),(334,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 16:20:59','2026-10-17 22:20:59'),(335,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 16:21:59','2026-10-17 22:21:59'),(336,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 16:53:07','2026-10-17 22:53:07'),(337,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 17:40:28','2026-10-17 23:40:29'),(338,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 17:42:56','2026-10-17 23:42:56'),(339,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 17:43:47','2026-10-17 23:43:47'),(340,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 17:44:45','2026-10-17 23:44:45'),(341,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-17 18:20:51','2026-10-18 00:20:51'),(342,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 16:22:10','2026-10-19 22:22:10'),(343,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 18:50:10','2026-10-20 00:50:10'),(344,1,'INSERT','tblmodulo',0,'Se insertó un registro en tblmodulo: nombreModulo: Bancos\ndescripcionModulo: Módulo bancario del sistema de Enbutidos S.A.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 18:51:31','2026-10-20 00:51:31'),(345,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Monedas\ndescripcionAplicacion: Mantenimiento de la moneda a utilizar.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 18:55:23','2026-10-20 00:55:23'),(346,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Bancos\ndescripcionAplicacion: Mantenimiento del banco a utilizar.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 18:55:56','2026-10-20 00:55:56'),(347,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Tipo de Cuenta Bancaria\ndescripcionAplicacion: Mantenimiento del banco a utilizar.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 18:56:19','2026-10-20 00:56:19'),(348,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Tipo de Transacción\ndescripcionAplicacion: Mantenimiento de la transacción a utilizar.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 18:56:59','2026-10-20 00:56:59'),(349,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Cuenta Bancaria\ndescripcionAplicacion: Mantenimiento de las cuentas a utilizar.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 18:57:25','2026-10-20 00:57:26'),(350,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Movimiento Bancario\ndescripcionAplicacion: Proceso para el registro de movimientos bancarios.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 18:58:06','2026-10-20 00:58:06'),(351,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Conciliación Bancaria\ndescripcionAplicacion: Proceso para conciliar los movimientos con el estado de cuentas.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 18:58:39','2026-10-20 00:58:39'),(352,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Enlace a Otros Módulos\ndescripcionAplicacion: Proceso que permite la comunicación con los demás módulos.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 18:59:09','2026-10-20 00:59:09'),(353,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Disponibilidad Diaria\ndescripcionAplicacion: Proceso que permite visualizar la disponibilidad del cliente.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 18:59:44','2026-10-20 00:59:44'),(354,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Tipo de Cambio\ndescripcionAplicacion: Proceso que permite visualizar el cambio de la moneda.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 19:00:16','2026-10-20 01:00:16'),(355,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Proceso autorización de Orden de Compra\ndescripcionAplicacion: Proceso que autoriza las ordenes de las compras.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 19:01:48','2026-10-20 01:01:48'),(356,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Proceso Generación de Pago\ndescripcionAplicacion: Proceso que permite generar los pagos requeridos.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 19:02:23','2026-10-20 01:02:23'),(357,1,'INSERT','tblAplicacion',0,'Se insertó un registro en tblAplicacion: idModulo: 14\nnombreAplicacion: Cierre Bancario\ndescripcionAplicacion: Proceso que culmina con los movimientos mensuales y anuales.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 19:02:54','2026-10-20 01:02:54'),(358,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 19:06:05','2026-10-20 01:06:05'),(359,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 19:06:30','2026-10-20 01:06:30'),(360,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 19:06:42','2026-10-20 01:06:42'),(361,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 19:24:26','2026-10-20 01:24:26'),(362,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 19:24:53','2026-10-20 01:24:53'),(363,1,'INSERT','tblAsigAppPerf',1,'Se asignó aplicación 20 al rol 1','192.168.1.24','2026-10-19 19:25:21','2026-10-20 01:25:22'),(364,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 19:25:28','2026-10-20 01:25:28'),(365,1,'INSERT','tblTipoTransaccion',0,'Se insertó un registro en tblTipoTransaccion: codigoTipoTransaccion: TRANS-001\nnombreTipoTransaccion: Banca en Línea\nnaturalezaTipoTransaccion: Ambos\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 19:28:12','2026-10-20 01:28:12'),(366,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 19:29:18','2026-10-20 01:29:18'),(367,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 19:44:57','2026-10-20 01:44:57'),(368,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 19:49:11','2026-10-20 01:49:11'),(369,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 19:50:54','2026-10-20 01:50:54'),(370,1,'DELETE','tblAplicacion',17,'Se eliminó un registro de tblAplicacion.','192.168.1.24','2026-10-19 19:55:54','2026-10-20 01:55:54'),(371,1,'DELETE','tblmodulo',14,'Se eliminó un registro de tblmodulo.','192.168.1.24','2026-10-19 20:02:50','2026-10-20 02:02:50'),(372,1,'INSERT','tblmodulo',8000,'Se insertó un registro en tblmodulo: idModulo: 8000\nnombreModulo: Bancos\ndescripcionModulo: Modulo bancario del sistema de Enbutidos S.A.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:03:12','2026-10-20 02:03:13'),(373,1,'INSERT','tblAplicacion',8001,'Se insertó un registro en tblAplicacion: idAplicacion: 8001\nidModulo: 8000\nnombreAplicacion: Moneda\ndescripcionAplicacion: Mantenimiento de la moneda a utilizar.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:04:34','2026-10-20 02:04:34'),(374,1,'INSERT','tblAplicacion',8002,'Se insertó un registro en tblAplicacion: idAplicacion: 8002\nidModulo: 8000\nnombreAplicacion: Bancos\ndescripcionAplicacion: Mantenimiento del banco a utilizar.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:05:28','2026-10-20 02:05:28'),(375,1,'INSERT','tblAplicacion',8003,'Se insertó un registro en tblAplicacion: idAplicacion: 8003\nidModulo: 8000\nnombreAplicacion: Tipo de Cuenta Bancaria\ndescripcionAplicacion: Mantenimiento del tipo de cuenta a utilizar.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:06:10','2026-10-20 02:06:10'),(376,1,'INSERT','tblAplicacion',8004,'Se insertó un registro en tblAplicacion: idAplicacion: 8004\nidModulo: 8000\nnombreAplicacion: Tipo de Transacción\ndescripcionAplicacion: Mantenimiento del tipo de transacción a utilizar.\nis_active: 0\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:07:07','2026-10-20 02:07:07'),(377,1,'INSERT','tblAplicacion',8005,'Se insertó un registro en tblAplicacion: idAplicacion: 8005\nidModulo: 8000\nnombreAplicacion: Cuenta Bancaria\ndescripcionAplicacion: Mantenimiento de las cuentas a utilizar.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:07:53','2026-10-20 02:07:53'),(378,1,'INSERT','tblAplicacion',8006,'Se insertó un registro en tblAplicacion: idAplicacion: 8006\nidModulo: 8000\nnombreAplicacion: Movimiento Bancario\ndescripcionAplicacion: Proceso para el registro de movimientos bancarios.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:08:17','2026-10-20 02:08:17'),(379,1,'INSERT','tblAplicacion',8007,'Se insertó un registro en tblAplicacion: idAplicacion: 8007\nidModulo: 8000\nnombreAplicacion: Conciliacion Bancaria\ndescripcionAplicacion: Proceso para conciliar los movimientos con el estado de cuentas.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:08:44','2026-10-20 02:08:44'),(380,1,'INSERT','tblAplicacion',8008,'Se insertó un registro en tblAplicacion: idAplicacion: 8008\nidModulo: 8000\nnombreAplicacion: Enlace a Otros Modulos\ndescripcionAplicacion: Proceso que permite la comunicacion con los demas modulos.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:09:10','2026-10-20 02:09:10'),(381,1,'INSERT','tblAplicacion',8009,'Se insertó un registro en tblAplicacion: idAplicacion: 8009\nidModulo: 8000\nnombreAplicacion: Disponibilidad Diaria\ndescripcionAplicacion: Proceso que permite visualizar la disponibilidad del cliente.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:09:30','2026-10-20 02:09:30'),(382,1,'INSERT','tblAplicacion',8010,'Se insertó un registro en tblAplicacion: idAplicacion: 8010\nidModulo: 8000\nnombreAplicacion: Tipo de Cambio\ndescripcionAplicacion: Proceso que permite visualizar el cambio de la moneda.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:09:50','2026-10-20 02:09:50'),(383,1,'INSERT','tblAplicacion',8011,'Se insertó un registro en tblAplicacion: idAplicacion: 8011\nidModulo: 8000\nnombreAplicacion: Proceso autorizacion de Orden de Compra\ndescripcionAplicacion: Proceso que autoriza las ordenes de las compras.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:10:09','2026-10-20 02:10:09'),(384,1,'INSERT','tblAplicacion',8012,'Se insertó un registro en tblAplicacion: idAplicacion: 8012\nidModulo: 8000\nnombreAplicacion: Proceso Generacion de Pago\ndescripcionAplicacion: Proceso que permite generar los pagos requeridos.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:10:32','2026-10-20 02:10:32'),(385,1,'INSERT','tblAplicacion',8013,'Se insertó un registro en tblAplicacion: idAplicacion: 8013\nidModulo: 8000\nnombreAplicacion: Cierre Bancario\ndescripcionAplicacion: Proceso que culmina con los movimientos mensuales y anuales.\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:11:07','2026-10-20 02:11:07'),(386,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:12:52','2026-10-20 02:12:52'),(387,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:13:13','2026-10-20 02:13:13'),(388,1,'INSERT','tblAsigAppPerf',1,'Se asignó aplicación 8004 al rol 1','192.168.1.24','2026-10-19 20:14:15','2026-10-20 02:14:15'),(389,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:14:44','2026-10-20 02:14:44'),(390,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:15:26','2026-10-20 02:15:26'),(391,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:18:18','2026-10-20 02:18:18'),(392,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:19:48','2026-10-20 02:19:48'),(393,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:21:36','2026-10-20 02:21:36'),(394,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:22:07','2026-10-20 02:22:07'),(395,2,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.24','2026-10-19 20:23:04','2026-10-20 02:23:04'),(396,2,'DELETE','tblAsigAppPerf',1,'Se eliminó asignación de aplicación 8004 al rol 1','192.168.1.24','2026-10-19 20:23:17','2026-10-20 02:23:18'),(397,2,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.24','2026-10-19 20:23:24','2026-10-20 02:23:24'),(398,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:23:41','2026-10-20 02:23:41'),(399,1,'INSERT','tblAsigAppPerf',1,'Se asignó aplicación 8004 al rol 1','192.168.1.24','2026-10-19 20:23:56','2026-10-20 02:23:56'),(400,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:24:07','2026-10-20 02:24:07'),(401,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:25:57','2026-10-20 02:25:57'),(402,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:27:21','2026-10-20 02:27:21'),(403,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:28:28','2026-10-20 02:28:28'),(404,1,'INSERT','tblAsigAppPerf',2,'Se asignó aplicación 8004 al rol 2','192.168.1.24','2026-10-19 20:29:40','2026-10-20 02:29:40'),(405,2,'LOGIN','tblUsuario',2,'Inicio de sesión exitoso del usuario: cramirez','192.168.1.24','2026-10-19 20:29:53','2026-10-20 02:29:53'),(406,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:37:28','2026-10-20 02:37:28'),(407,1,'INSERT','tblTipoTransaccion',0,'Se insertó un registro en tblTipoTransaccion: codigoTipoTransaccion: TRANS-002\nnombreTipoTransaccion: Nota de Débito\nnaturalezaTipoTransaccion: Egreso\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:39:01','2026-10-20 02:39:01'),(408,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:41:00','2026-10-20 02:41:01'),(409,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:41:13','2026-10-20 02:41:13'),(410,1,'LOGIN','tblUsuario',1,'Inicio de sesión exitoso del usuario: imelendez','192.168.1.24','2026-10-19 20:41:29','2026-10-20 02:41:29'),(411,1,'INSERT','tblTipoTransaccion',0,'Se insertó un registro en tblTipoTransaccion: codigoTipoTransaccion: TRANS-003\nnombreTipoTransaccion: Interes Ganado\nnaturalezaTipoTransaccion: Ingreso\nis_active: 1\ncreated_at: 2026-10-19\nupdated_at: 2026-10-19\n','192.168.1.24','2026-10-19 20:42:12','2026-10-20 02:42:12');

/*!40000 ALTER TABLE `tblbitacora` ENABLE KEYS */;

UNLOCK TABLES;

--
-- Table structure for table `tblempleado`
--

DROP TABLE IF EXISTS `tblempleado`;

/*!40101 SET @saved_cs_client     = @@character_set_client */;

/*!50503 SET character_set_client = utf8mb4 */;

CREATE TABLE `tblempleado` (
  `idEmpleado` int NOT NULL AUTO_INCREMENT,
  `codigoEmpleado` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `dpiEmpleado` char(13) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `nitEmpleado` char(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `nombresEmpleado` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `apellidosEmpleado` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `puestoEmpleado` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `generoEmpleado` char(1) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'M = Masculino, F = Femenino',
  `fechaNacimientoEmpleado` date NOT NULL,
  `fechaContratacionEmpleado` date NOT NULL,
  `telefonoEmpleado` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `correoEmpleado` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idEmpleado`),
  UNIQUE KEY `uqCodigoEmpleado` (`codigoEmpleado`),
  UNIQUE KEY `uqDpiEmpleado` (`dpiEmpleado`),
  UNIQUE KEY `uqNitEmpleado` (`nitEmpleado`),
  UNIQUE KEY `uqCorreoEmpleado` (`correoEmpleado`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblempleado`
--

LOCK TABLES `tblempleado` WRITE;

/*!40000 ALTER TABLE `tblempleado` DISABLE KEYS */;

INSERT INTO `tblempleado` VALUES (1,'EMP-001','1234567890123','1234567-K','Isabel','Meléndez','Coordinadora de Implementación','F','2000-05-14','2026-01-10','55512345','isabel@gmail.com',1,'2026-09-17 20:07:07','2026-09-24 05:40:07'),(2,'EMP-002','2345678901234','2345678K','Carlos','Ramírez','Analista de Sistemas','M','1998-03-22','2025-11-05','55523456','carlos.ramirez@terminus.com',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(3,'EMP-003','3456789012345','3456789K','María','López','Administradora de Base de Datos','F','1995-09-30','2025-08-19','55534567','maria.lopez@terminus.com',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(13,'EMP-006','8463778648763','3727836-7','Oscar Emilio','Morales Lemus','Gerente','M','2005-05-28','2026-09-21','32323232','oscaremilio@gmail.com',1,'2026-09-22 10:30:17','2026-09-22 10:30:17'),(14,'EMP-007','5463212315453','5454542-1','Empleado','Examen','Pruebas','F','1990-09-22','2026-09-22','87897654','prueba@gmail.com',1,'2026-09-22 19:11:30','2026-09-22 19:11:30'),(19,'EMP-010','9876543219876','9876543-2','Pruebas','Capacitacion','Probador','M','1990-01-01','2026-01-10','12345678','terminus.seguridad22026@gmail.com',1,'2026-09-24 05:42:57','2026-09-24 05:42:57'),(20,'EMP-011','3142534231425','66554433-6','examenpkd','SegundoParcial','Examen','M','2005-05-28','2026-09-29','77559933','examensegundo@gmail.com',1,'2026-09-29 06:00:00','2026-09-29 06:00:00');
/*!40000 ALTER TABLE `tblempleado` ENABLE KEYS */;

UNLOCK TABLES;

--
-- Table structure for table `tblmodulo`
--

DROP TABLE IF EXISTS `tblmodulo`;

/*!40101 SET @saved_cs_client     = @@character_set_client */;

/*!50503 SET character_set_client = utf8mb4 */;

CREATE TABLE `tblmodulo` (
  `idModulo` int NOT NULL AUTO_INCREMENT,
  `nombreModulo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcionModulo` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idModulo`),
  UNIQUE KEY `uqNombreModulo` (`nombreModulo`)
) ENGINE=InnoDB AUTO_INCREMENT=8001 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblmodulo`
--

LOCK TABLES `tblmodulo` WRITE;
/*!40000 ALTER TABLE `tblmodulo` DISABLE KEYS */;
INSERT INTO `tblmodulo` VALUES (1,'Producción','Módulo de control de producción de embutidos',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(2,'Inventario','Módulo de control de materia prima y producto terminado',1,'2026-09-17 20:07:07','2026-09-20 11:29:01'),(3,'Ventas','Módulo de gestión de pedidos y clientes',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(4,'Seguridad','Módulo de administración y seguridad del sistema',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(13,'Pruebas','Esto es una prueba controlada por parte de Seguridad.',1,'2026-09-24 05:44:26','2026-09-24 05:44:26'),(8000,'Bancos','Modulo bancario del sistema de Enbutidos S.A.',1,'2026-10-19 06:00:00','2026-10-19 06:00:00');
/*!40000 ALTER TABLE `tblmodulo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tblrecuperacioncontrasena`
--

DROP TABLE IF EXISTS `tblrecuperacioncontrasena`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tblrecuperacioncontrasena` (
  `idRecuperacionContrasena` int NOT NULL AUTO_INCREMENT,
  `idUsuario` int NOT NULL,
  `tokenRecuperacionContrasena` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `fechaExpiracionRecuperacionContrasena` datetime NOT NULL,
  `usadoRecuperacionContrasena` tinyint(1) NOT NULL DEFAULT '0',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idRecuperacionContrasena`),
  UNIQUE KEY `uqTokenRecuperacionContrasena` (`tokenRecuperacionContrasena`),
  KEY `idxRecuperacionContrasenaIdUsuario` (`idUsuario`),
  CONSTRAINT `Fk_RecuperacionContrasena_Usuario` FOREIGN KEY (`idUsuario`) REFERENCES `tblusuario` (`idUsuario`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblrecuperacioncontrasena`
--

LOCK TABLES `tblrecuperacioncontrasena` WRITE;
/*!40000 ALTER TABLE `tblrecuperacioncontrasena` DISABLE KEYS */;
INSERT INTO `tblrecuperacioncontrasena` VALUES (1,1,'TOKEN-A1B2C3D4','2026-09-18 08:07:07',0,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(2,2,'TOKEN-E5F6G7H8','2026-09-18 08:07:07',0,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(3,3,'TOKEN-I9J0K1L2','2026-09-18 08:07:07',1,'2026-09-17 20:07:07','2026-09-17 20:07:07');
/*!40000 ALTER TABLE `tblrecuperacioncontrasena` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tblrol`
--

DROP TABLE IF EXISTS `tblrol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tblrol` (
  `idRol` int NOT NULL AUTO_INCREMENT,
  `nombreRol` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcionRol` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idRol`),
  UNIQUE KEY `uqNombreRol` (`nombreRol`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblrol`
--

LOCK TABLES `tblrol` WRITE;
/*!40000 ALTER TABLE `tblrol` DISABLE KEYS */;
INSERT INTO `tblrol` VALUES (1,'Administrador','Rol con acceso total al sistema',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(2,'Supervisor','Rol con acceso a reportes y aprobaciones',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(11,'Pruebas','pruebas del sistema',0,'2026-09-19 03:45:38','2026-09-19 03:45:38'),(12,'PerfilExamen','Permisos personalizados para el examen',1,'2026-09-22 19:16:29','2026-09-22 19:16:29'),(14,'Prueba','Esta es una prueba controlada por parte de Seguridad',1,'2026-09-24 05:47:34','2026-09-24 05:47:34'),(15,'perfilpkd','Perfil para el Segundo Examen Parcial',1,'2026-09-29 14:56:56','2026-09-29 15:37:56');
/*!40000 ALTER TABLE `tblrol` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tblrolmoduloaplicacion`
--

DROP TABLE IF EXISTS `tblrolmoduloaplicacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblrolmoduloaplicacion`
--

LOCK TABLES `tblrolmoduloaplicacion` WRITE;
/*!40000 ALTER TABLE `tblrolmoduloaplicacion` DISABLE KEYS */;
INSERT INTO `tblrolmoduloaplicacion` VALUES (1,4,4,1,1,1,1,'2026-09-17 20:07:07','2026-09-17 20:19:43'),(1,4,5,1,1,1,1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(1,4,6,1,1,1,1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(1,4,7,1,1,1,1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(1,4,8,1,1,1,1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(1,4,9,1,1,1,1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(1,4,10,1,1,1,1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(1,4,11,1,1,1,1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(1,4,12,1,1,1,1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(1,8000,8004,1,1,1,1,'2026-10-20 02:23:56','2026-10-20 02:23:56'),(2,4,4,1,1,0,0,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(2,4,6,0,0,1,0,'2026-10-11 18:56:50','2026-10-11 18:56:50'),(2,4,7,1,1,1,0,'2026-10-11 18:55:49','2026-10-11 18:55:49'),(2,4,10,0,1,1,1,'2026-10-11 18:56:30','2026-10-11 18:56:30'),(2,8000,8004,1,1,1,1,'2026-10-20 02:29:40','2026-10-20 02:29:40'),(12,4,8,1,1,0,0,'2026-09-22 19:20:38','2026-09-22 19:20:38'),(14,4,15,1,1,1,1,'2026-09-24 05:48:40','2026-09-24 05:48:40'),(15,4,12,0,0,0,1,'2026-09-29 15:36:22','2026-09-29 15:36:22'),(15,4,16,1,1,1,1,'2026-09-29 15:36:13','2026-09-29 15:36:13');
/*!40000 ALTER TABLE `tblrolmoduloaplicacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tbltipotransaccion`
--

DROP TABLE IF EXISTS `tbltipotransaccion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbltipotransaccion` (
  `Pk_TipoTransaccion` bigint NOT NULL AUTO_INCREMENT,
  `codigoTipoTransaccion` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `nombreTipoTransaccion` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `naturalezaTipoTransaccion` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Pk_TipoTransaccion`),
  UNIQUE KEY `uqTipoTransaccion1` (`codigoTipoTransaccion`),
  CONSTRAINT `ckTipoTransaccion1` CHECK ((`naturalezaTipoTransaccion` in (_utf8mb4'INGRESO',_utf8mb4'EGRESO',_utf8mb4'AMBOS'))),
  CONSTRAINT `ckTipoTransaccion2` CHECK ((`is_active` in (0,1)))
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbltipotransaccion`
--

LOCK TABLES `tbltipotransaccion` WRITE;
/*!40000 ALTER TABLE `tbltipotransaccion` DISABLE KEYS */;
INSERT INTO `tbltipotransaccion` VALUES (1,'TRANS-001','Banca en Línea','Ambos',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(2,'TRANS-002','Nota de Débito','Egreso',1,'2026-10-19 06:00:00','2026-10-19 06:00:00'),(3,'TRANS-003','Interes Ganado','Ingreso',1,'2026-10-19 06:00:00','2026-10-19 06:00:00');
/*!40000 ALTER TABLE `tbltipotransaccion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tblusuario`
--

DROP TABLE IF EXISTS `tblusuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tblusuario` (
  `idUsuario` int NOT NULL AUTO_INCREMENT,
  `idEmpleado` int NOT NULL,
  `nombreUsuario` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `contrasenaUsuario` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ultimoAccesoUsuario` datetime NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idUsuario`),
  UNIQUE KEY `uqIdEmpleadoUsuario` (`idEmpleado`),
  UNIQUE KEY `uqUsuarioUsuario` (`nombreUsuario`),
  KEY `idxUsuarioIdEmpleado` (`idEmpleado`),
  CONSTRAINT `Fk_Usuario_Empleado` FOREIGN KEY (`idEmpleado`) REFERENCES `tblempleado` (`idEmpleado`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblusuario`
--

LOCK TABLES `tblusuario` WRITE;
/*!40000 ALTER TABLE `tblusuario` DISABLE KEYS */;
INSERT INTO `tblusuario` VALUES (1,1,'imelendez','$2b$10$e9CQKArQRmpf.IUtqpxYmOoO9vyh2./7ped56qEokwfgn7DkcacPC','2026-09-17 08:07:07',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(2,2,'cramirez','$2b$10$ojN1MyPkUaylOuupBEGmhemdx5tQi4BdbbJrpSF2ZJjD0ZWM/lbe2','2026-09-17 08:07:07',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(3,3,'mlopez','$2b$10$Bo9RDmJRGKKLxm1tSmRO1ODP0CiGmuqy/wcnJ6bs2MkOVe1OQml5i','2026-09-17 08:07:07',1,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(12,14,'UsuarioExamen','$2a$11$m6Fi2WikM6YW5QbVZikQBuxMCXRfLjnxPleDRY75FLmVjYncKxEc6','2026-09-22 07:12:30',1,'2026-09-22 19:12:30','2026-09-22 19:12:30'),(13,19,'Pcapacitacion','$2a$11$Vee.0P876yc/Mnwy2ePINeGVsQoOLofGZT30VkeJz/B1y3ND7tSAq','2026-09-23 17:43:51',1,'2026-09-24 05:43:51','2026-09-24 05:43:51'),(14,20,'examenpkd','$2a$11$TokHSP3FuZc1oemM3ZNgOOqsX9oVfVCcC1ivS.DZyzqbfbnsJawH6','2026-09-29 08:55:29',1,'2026-09-29 14:55:29','2026-09-29 14:55:29');
/*!40000 ALTER TABLE `tblusuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tblusuariomoduloaplicacion`
--

DROP TABLE IF EXISTS `tblusuariomoduloaplicacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblusuariomoduloaplicacion`
--

LOCK TABLES `tblusuariomoduloaplicacion` WRITE;

/*!40000 ALTER TABLE `tblusuariomoduloaplicacion` DISABLE KEYS */;
INSERT INTO `tblusuariomoduloaplicacion` VALUES (3,3,3,1,0,0,0,'2026-09-17 20:07:07','2026-09-17 20:07:07'),(13,13,15,0,0,0,0,'2026-09-24 05:49:15','2026-09-24 05:49:15');
/*!40000 ALTER TABLE `tblusuariomoduloaplicacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tblusuariorol`
--

DROP TABLE IF EXISTS `tblusuariorol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblusuariorol`
--

LOCK TABLES `tblusuariorol` WRITE;
/*!40000 ALTER TABLE `tblusuariorol` DISABLE KEYS */;
INSERT INTO `tblusuariorol` VALUES (1,1,'2026-09-17','2026-09-17 20:07:07','2026-09-17 20:07:07'),(2,2,'2026-09-17','2026-09-17 20:07:07','2026-09-17 20:07:07'),(3,2,'2026-09-17','2026-09-17 20:09:00','2026-09-17 20:09:00'),(12,12,'2026-09-22','2026-09-22 19:22:32','2026-09-22 19:22:32'),(13,14,'2026-09-23','2026-09-24 05:47:47','2026-09-24 05:47:47'),(14,15,'2026-09-29','2026-09-29 15:38:04','2026-09-29 15:38:04');
/*!40000 ALTER TABLE `tblusuariorol` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-10-19 20:54:50



USE dbsistemaembutidos;

SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS tblCuentaBancaria;
DROP TABLE IF EXISTS tblTipoCuentaBancaria;
DROP TABLE IF EXISTS tblBanco;
DROP TABLE IF EXISTS tblMoneda;
SET FOREIGN_KEY_CHECKS = 1;

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






-- inserts

USE dbsistemaembutidos;

-- Monedas
INSERT INTO tblMoneda (
    codigoMoneda,
    nombreMoneda,
    simboloMoneda
) VALUES
('GTQ', 'Quetzal guatemalteco', 'Q'),
('USD', 'Dólar estadounidense', 'US$'),
('EUR', 'Euro', '€'),
('MXN', 'Peso mexicano', 'MX$'),
('GBP', 'Libra esterlina', '£'),
('CAD', 'Dólar canadiense', 'C$');

-- Bancos
INSERT INTO tblBanco (
    codigoBanco,
    nombreBanco
) VALUES
('BI', 'Banco Industrial'),
('BANRURAL', 'Banco de Desarrollo Rural'),
('GYT', 'Banco G&T Continental'),
('BAC', 'BAC Credomatic');

-- Tipos de cuenta bancaria
INSERT INTO tblTipoCuentaBancaria (
    codigoTipoCuentaBancaria,
    nombreTipoCuentaBancaria
) VALUES
('MONETARIA', 'Cuenta monetaria'),
('AHORRO', 'Cuenta de ahorro'),
('PLAZO_FIJO', 'Depósito a plazo fijo');

-- Obtener los IDs de los bancos
SET @bancoBI = (
    SELECT Pk_Banco FROM tblBanco
    WHERE codigoBanco = 'BI'
);

SET @bancoBanrural = (
    SELECT Pk_Banco FROM tblBanco
    WHERE codigoBanco = 'BANRURAL'
);

SET @bancoGYT = (
    SELECT Pk_Banco FROM tblBanco
    WHERE codigoBanco = 'GYT'
);

SET @bancoBAC = (
    SELECT Pk_Banco FROM tblBanco
    WHERE codigoBanco = 'BAC'
);

-- Obtener los IDs de las monedas
SET @monedaGTQ = (
    SELECT Pk_Moneda FROM tblMoneda
    WHERE codigoMoneda = 'GTQ'
);

SET @monedaUSD = (
    SELECT Pk_Moneda FROM tblMoneda
    WHERE codigoMoneda = 'USD'
);

SET @monedaEUR = (
    SELECT Pk_Moneda FROM tblMoneda
    WHERE codigoMoneda = 'EUR'
);

-- Obtener los IDs de los tipos de cuenta
SET @tipoMonetaria = (
    SELECT Pk_TipoCuentaBancaria FROM tblTipoCuentaBancaria
    WHERE codigoTipoCuentaBancaria = 'MONETARIA'
);

SET @tipoAhorro = (
    SELECT Pk_TipoCuentaBancaria FROM tblTipoCuentaBancaria
    WHERE codigoTipoCuentaBancaria = 'AHORRO'
);

SET @tipoPlazoFijo = (
    SELECT Pk_TipoCuentaBancaria FROM tblTipoCuentaBancaria
    WHERE codigoTipoCuentaBancaria = 'PLAZO_FIJO'
);

-- Cuentas bancarias
INSERT INTO tblCuentaBancaria (
    numeroCuentaBancaria,
    nombreCuentaBancaria,
    codigoCuentaContableCuentaBancaria,
    limiteSobregiroCuentaBancaria,
    Fk_Banco_Codigo,
    Fk_Moneda_Codigo,
    Fk_TipoCuentaBancaria_Codigo
) VALUES
(
    'PRUEBA-BI-001',
    'Cuenta operativa principal en quetzales',
    '1.1.02.001',
    15000.0000,
    @bancoBI,
    @monedaGTQ,
    @tipoMonetaria
),
(
    'PRUEBA-BI-002',
    'Cuenta de pagos en dólares',
    '1.1.02.002',
    0.0000,
    @bancoBI,
    @monedaUSD,
    @tipoMonetaria
),
(
    'PRUEBA-BANRURAL-001',
    'Cuenta para pago de nómina',
    '1.1.02.003',
    0.0000,
    @bancoBanrural,
    @monedaGTQ,
    @tipoMonetaria
),
(
    'PRUEBA-BANRURAL-002',
    'Cuenta de ahorro para contingencias',
    '1.1.02.004',
    0.0000,
    @bancoBanrural,
    @monedaGTQ,
    @tipoAhorro
),
(
    'PRUEBA-GYT-001',
    'Cuenta para pago a proveedores',
    '1.1.02.005',
    10000.0000,
    @bancoGYT,
    @monedaGTQ,
    @tipoMonetaria
),
(
    'PRUEBA-GYT-002',
    'Cuenta de operaciones en euros',
    '1.1.02.006',
    0.0000,
    @bancoGYT,
    @monedaEUR,
    @tipoMonetaria
),
(
    'PRUEBA-BAC-001',
    'Cuenta de cobros y operaciones',
    '1.1.02.007',
    5000.0000,
    @bancoBAC,
    @monedaGTQ,
    @tipoMonetaria
),
(
    'PRUEBA-BI-003',
    'Depósito de inversión a plazo fijo',
    '1.1.02.008',
    0.0000,
    @bancoBI,
    @monedaGTQ,
    @tipoPlazoFijo
);


-- Permisos de la aplicación 8001 (Moneda) del módulo 8000 (Bancos)
INSERT INTO tblrolmoduloaplicacion
    (idRol, idModulo, idAplicacion,
     derInsertarRolModuloAplicacion, derEditarRolModuloAplicacion,
     derEliminarRolModuloAplicacion, derImprimirRolModuloAplicacion)
VALUES
    (1, 8000, 8001, 1, 1, 1, 1),
    (2, 8000, 8001, 1, 1, 1, 1)
ON DUPLICATE KEY UPDATE
    derInsertarRolModuloAplicacion = VALUES(derInsertarRolModuloAplicacion),
    derEditarRolModuloAplicacion   = VALUES(derEditarRolModuloAplicacion),
    derEliminarRolModuloAplicacion = VALUES(derEliminarRolModuloAplicacion),
    derImprimirRolModuloAplicacion = VALUES(derImprimirRolModuloAplicacion);
