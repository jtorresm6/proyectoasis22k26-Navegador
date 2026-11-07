
USE dbsistemaembutidos;

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

-- ---------- Datos iniciales de catalogos ----------
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


-- ---------- Empleados ----------
-- ---------- Datos de RRHH en el empleado ----------

-- 1) Datos base para que existan los padres
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

-- 2) Agregar las columnas (sin FK todavía) permitiendo NULL
ALTER TABLE tblEmpleado
    ADD COLUMN idPuesto INT NULL AFTER apellidosEmpleado,
    ADD COLUMN idDepartamento INT NULL AFTER idPuesto,
    ADD COLUMN idJornada INT NULL AFTER idDepartamento,
    ADD COLUMN salarioEmpleado DECIMAL(12,4) NULL AFTER idJornada;

-- 3) Rellenar los empleados existentes (aprovechando puestoEmpleado)
UPDATE tblEmpleado e
JOIN tblPuesto p ON p.nombrePuesto = e.puestoEmpleado
SET e.idPuesto = p.idPuesto
WHERE e.idEmpleado > 0;

UPDATE tblEmpleado
SET idPuesto = (SELECT idPuesto FROM tblPuesto LIMIT 1)
WHERE idEmpleado > 0 AND idPuesto IS NULL;

UPDATE tblEmpleado SET
    idDepartamento  = (SELECT idDepartamento FROM tblDepartamento LIMIT 1),
    idJornada       = (SELECT idJornada FROM tblJornada LIMIT 1),
    salarioEmpleado = 0
WHERE idEmpleado > 0;

-- Los que no coincidan por nombre, asígnales uno por defecto
UPDATE tblEmpleado
SET idPuesto = (SELECT idPuesto FROM tblPuesto LIMIT 1)
WHERE idEmpleado > 0 AND idPuesto IS NULL;

UPDATE tblEmpleado
SET idDepartamento  = (SELECT idDepartamento FROM tblDepartamento LIMIT 1),
    idJornada       = (SELECT idJornada FROM tblJornada LIMIT 1),
    salarioEmpleado = 0
WHERE idEmpleado > 0;

-- 4) Ahora sí: NOT NULL, quitar columna vieja y crear las FK
ALTER TABLE tblEmpleado
    MODIFY idDepartamento INT NOT NULL,
    MODIFY idJornada INT NOT NULL,
    MODIFY salarioEmpleado DECIMAL(12,4) NOT NULL,
    DROP COLUMN puestoEmpleado,
    ADD CONSTRAINT Fk_Empleado_Departamento FOREIGN KEY (idDepartamento)
        REFERENCES tblDepartamento (idDepartamento) ON DELETE RESTRICT ON UPDATE CASCADE,
    ADD CONSTRAINT Fk_Empleado_Jornada FOREIGN KEY (idJornada)
        REFERENCES tblJornada (idJornada) ON DELETE RESTRICT ON UPDATE CASCADE;
 


-- ---------- Historial de puestos ----------
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