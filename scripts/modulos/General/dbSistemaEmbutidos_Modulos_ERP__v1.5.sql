-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡ Modulos !!!!!!!!!!!!!!!!!!!!!!!!!-------------

USE dbSistemaEmbutidos;
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

-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡  Contabilidad  !!!!!!!!!!!!!!!!!!!!!!!!!-------------
-- =============================================================================
--  1. CATÁLOGOS
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
         
   -- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡ Logistica !!!!!!!!!!!!!!!!!!!!!!!!!-------------     
/* RESPONSABLE BRIAN DE LA CRUZ */ 
-- tblEmpresaTransportista
CREATE TABLE IF NOT EXISTS tblEmpresaTransportista (
Pk_Empresa_Transportista INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 nombreEmpresaTransportista VARCHAR(100) NOT NULL,
 contactoEmpresaTransportista VARCHAR(100) NOT NULL,
 telefonoEmpresaTransportista VARCHAR(30) COLLATE utf8mb4_unicode_ci NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqEmpresaTransportistaNombre UNIQUE (nombreEmpresaTransportista)
) ENGINE=InnoDB;

-- tblTipoServicio
CREATE TABLE IF NOT EXISTS tblTipoServicio (
Pk_Tipo_Servicio INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 nombreTipoServicio VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqTipoServicioNombre UNIQUE (nombreTipoServicio)
) ENGINE=InnoDB;

-- tblTarifaTransporte
CREATE TABLE IF NOT EXISTS tblTarifaTransporte (
Pk_Tarifa_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 nombreTarifaTransporte VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqTarifaTransporteNombre UNIQUE (nombreTarifaTransporte)
) ENGINE=InnoDB;

-- tblEstado
CREATE TABLE IF NOT EXISTS tblEstado (
Pk_Estado INT AUTO_INCREMENT NOT NULL,
    tipoEstado VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Estado PRIMARY KEY (Pk_Estado),
    CONSTRAINT uqTipoEstado UNIQUE (tipoEstado)
) ENGINE=InnoDB;

-- tblTipoTransporte
CREATE TABLE IF NOT EXISTS tblTipoTransporte (
Pk_Tipo_Transporte INT AUTO_INCREMENT NOT NULL,
    nombreTipoTransporte VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Tipo_Transporte PRIMARY KEY (Pk_Tipo_Transporte),
    CONSTRAINT uqNombreTipoTransporte UNIQUE (nombreTipoTransporte)
) ENGINE=InnoDB;

-- tblTransporte
CREATE TABLE IF NOT EXISTS tblTransporte (
Pk_Transporte INT AUTO_INCREMENT NOT NULL,
    nroPlacaTransporte VARCHAR(30) NOT NULL,
    marcaTransporte VARCHAR(100) NOT NULL,
    modeloTransporte VARCHAR(100) NOT NULL,
    anioTransporte SMALLINT NOT NULL,
    esRefrigeradoTransporte BOOLEAN NOT NULL DEFAULT FALSE,
    Fk_Estado_Codigo INT NOT NULL,
    Fk_Tipo_Transporte_Codigo INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Transporte PRIMARY KEY (Pk_Transporte),
    CONSTRAINT uqNroPlacaTransporte UNIQUE (nroPlacaTransporte),
    INDEX idxIdEstadoTransporte (Fk_Estado_Codigo),
    INDEX idxIdTipoTransporteTransporte (Fk_Tipo_Transporte_Codigo),
    CONSTRAINT Fk_Transporte_Estado FOREIGN KEY (Fk_Estado_Codigo) REFERENCES tblEstado(Pk_Estado) ON DELETE RESTRICT,
    CONSTRAINT Fk_Transporte_Tipo_Transporte FOREIGN KEY (Fk_Tipo_Transporte_Codigo) REFERENCES tblTipoTransporte(Pk_Tipo_Transporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblCamaraRefrigeracion
CREATE TABLE IF NOT EXISTS tblCamaraRefrigeracion (
Pk_Camara_Refrigeracion INT AUTO_INCREMENT NOT NULL,
    Fk_Transporte_Codigo INT NOT NULL,
    marcaEquipoCamaraRefrigeracion VARCHAR(100) NOT NULL,
    tempMinimaCelsiusCamaraRefrigeracion DECIMAL(5,2) NOT NULL,
    tempMaximaCelsiusCamaraRefrigeracion DECIMAL(5,2) NOT NULL,
    estadoOperativoCamaraRefrigeracion VARCHAR(30) NOT NULL DEFAULT 'OPERATIVO',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Camara_Refrigeracion PRIMARY KEY (Pk_Camara_Refrigeracion),
    CONSTRAINT uqIdTransporteCamaraRefrigeracion UNIQUE (Fk_Transporte_Codigo),
    CONSTRAINT Fk_Camara_Refrigeracion_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(Pk_Transporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblConductor
CREATE TABLE IF NOT EXISTS tblConductor (
Pk_Conductor INT AUTO_INCREMENT NOT NULL,
    codigoConductor VARCHAR(30) NOT NULL,
    nombreCompletoConductor VARCHAR(100) NOT NULL,
    numeroLicenciaConductor VARCHAR(30) NOT NULL,
    fechaVencimientoDocConductor DATE NOT NULL,
    Fk_Estado_Codigo INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Conductor PRIMARY KEY (Pk_Conductor),
    CONSTRAINT uqCodigoConductor UNIQUE (codigoConductor),
    CONSTRAINT uqNumeroLicenciaConductor UNIQUE (numeroLicenciaConductor),
    INDEX idxIdEstadoConductor (Fk_Estado_Codigo),
    CONSTRAINT Fk_Conductor_Estado FOREIGN KEY (Fk_Estado_Codigo) REFERENCES tblEstado(Pk_Estado) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblInspeccionSanitaria
CREATE TABLE IF NOT EXISTS tblInspeccionSanitaria (
Pk_Inspeccion_Sanitaria BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Transporte_Codigo INT NOT NULL,
    fechaInspeccionSanitaria DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    sanitizadoCorrectoInspeccionSanitaria BOOLEAN NOT NULL,
    libreOloresContamInspeccionSanitaria BOOLEAN NOT NULL,
    aprobadoParaCargaInspeccionSanitaria BOOLEAN NOT NULL,
    observacionesInspeccionSanitaria VARCHAR(500) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Inspeccion_Sanitaria PRIMARY KEY (Pk_Inspeccion_Sanitaria),
    INDEX idxIdTransporteInspeccionSanitaria (Fk_Transporte_Codigo),
    CONSTRAINT Fk_Inspeccion_Sanitaria_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(Pk_Transporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblLoteProducto
CREATE TABLE IF NOT EXISTS tblLoteProducto (
Pk_Lote_Producto BIGINT AUTO_INCREMENT NOT NULL,
    codigoLoteProducto VARCHAR(30) NOT NULL,
    descripcionLoteProducto VARCHAR(255) NOT NULL,
    requiereFrioLoteProducto BOOLEAN NOT NULL DEFAULT TRUE,
    fechaFabricacionLoteProducto DATE NOT NULL,
    fechaVencimientoLoteProducto DATE NOT NULL,
    tempRequeridaMinLoteProducto DECIMAL(5,2) NOT NULL,
    tempRequeridaMaxLoteProducto DECIMAL(5,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Lote_Producto PRIMARY KEY (Pk_Lote_Producto),
    CONSTRAINT uqCodigoLoteProducto UNIQUE (codigoLoteProducto)
) ENGINE=InnoDB;

-- tblEncabezadoViaje
CREATE TABLE IF NOT EXISTS tblEncabezadoViaje (
Pk_Encabezado_Viaje BIGINT AUTO_INCREMENT NOT NULL,
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
    CONSTRAINT Pk_Encabezado_Viaje PRIMARY KEY (Pk_Encabezado_Viaje),
    INDEX idxIdTransporteEncabezadoViaje (Fk_Transporte_Codigo),
    INDEX idxIdConductorEncabezadoViaje (Fk_Conductor_Codigo),
    INDEX idxEstadoViajeEncabezadoViaje (estadoViajeEncabezadoViaje),
    CONSTRAINT Fk_Encabezado_Viaje_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(Pk_Transporte) ON DELETE RESTRICT,
    CONSTRAINT Fk_Encabezado_Viaje_Conductor FOREIGN KEY (Fk_Conductor_Codigo) REFERENCES tblConductor(Pk_Conductor) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblDetalleViaje
CREATE TABLE IF NOT EXISTS tblDetalleViaje (
Pk_Detalle_Viaje BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Encabezado_Viaje_Codigo BIGINT NOT NULL,
    ordenDetalleViaje INT NOT NULL,
    descripcionDetalleViaje VARCHAR(255) NOT NULL,
    cantidadDetalleViaje DECIMAL(18,3) NOT NULL,
    pesoDetalleViaje DECIMAL(18,3) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Detalle_Viaje PRIMARY KEY (Pk_Detalle_Viaje),
    CONSTRAINT uqOrdenDetalleViaje UNIQUE (Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje),
    CONSTRAINT Fk_Detalle_Viaje_Encabezado_Viaje FOREIGN KEY (Fk_Encabezado_Viaje_Codigo) REFERENCES tblEncabezadoViaje(Pk_Encabezado_Viaje) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblDetalleViajeLoteProducto
CREATE TABLE IF NOT EXISTS tblDetalleViajeLoteProducto (
Pk_Detalle_Viaje_Lote_Producto BIGINT NOT NULL AUTO_INCREMENT,
    Fk_Detalle_Viaje_Codigo BIGINT NOT NULL,
    Fk_Lote_Producto_Codigo BIGINT NOT NULL,
    cantidadCajasDetalleViajeLoteProducto INT NOT NULL,
    pesoKgDetalleViajeLoteProducto DECIMAL(18,3) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Detalle_Viaje_Lote_Producto PRIMARY KEY (Pk_Detalle_Viaje_Lote_Producto),
    CONSTRAINT uqDetalleViajeLoteProducto UNIQUE (Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo),
    INDEX idxIdLoteProductoDetalleViajeLoteProducto (Fk_Lote_Producto_Codigo),
    CONSTRAINT Fk_Detalle_Viaje_Lote_Producto_Detalle_Viaje FOREIGN KEY (Fk_Detalle_Viaje_Codigo) REFERENCES tblDetalleViaje(Pk_Detalle_Viaje) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Viaje_Lote_Producto_Lote_Producto FOREIGN KEY (Fk_Lote_Producto_Codigo) REFERENCES tblLoteProducto(Pk_Lote_Producto) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblTelemetriaTemperatura
CREATE TABLE IF NOT EXISTS tblTelemetriaTemperatura (
Pk_Telemetria_Temperatura BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Encabezado_Viaje_Codigo BIGINT NOT NULL,
    fechaHoraTelemetriaTemperatura DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    temperaturaCelsiusTelemetriaTemperatura DECIMAL(5,2) NOT NULL,
    /* Porcentaje en formato decimal: 0.6500 = 65 % (DECIMAL(5,4) permite llegar a 100 %) */
    humedadRelativaTelemetriaTemperatura DECIMAL(5,4) NOT NULL,
    puertaAbiertaTelemetriaTemperatura BOOLEAN NOT NULL DEFAULT FALSE,
    ubicacionGpsTelemetriaTemperatura VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Telemetria_Temperatura PRIMARY KEY (Pk_Telemetria_Temperatura),
    INDEX idxCorrelativoFechaTelemetriaTemperatura (Fk_Encabezado_Viaje_Codigo, fechaHoraTelemetriaTemperatura),
    CONSTRAINT Fk_Telemetria_Temperatura_Encabezado_Viaje FOREIGN KEY (Fk_Encabezado_Viaje_Codigo) REFERENCES tblEncabezadoViaje(Pk_Encabezado_Viaje) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblAlertaCadenaFrio
CREATE TABLE IF NOT EXISTS tblAlertaCadenaFrio (
Pk_Alerta_Cadena_Frio BIGINT AUTO_INCREMENT NOT NULL,
    Fk_Encabezado_Viaje_Codigo BIGINT NOT NULL,
    Fk_Telemetria_Temperatura_Codigo BIGINT NOT NULL,
    tipoAlertaCadenaFrio VARCHAR(30) NOT NULL,
    temperaturaRegistradaAlertaCadenaFrio DECIMAL(5,2) NOT NULL,
    atendidaAlertaCadenaFrio BOOLEAN NOT NULL DEFAULT FALSE,
    observacionCalidadAlertaCadenaFrio VARCHAR(500) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Alerta_Cadena_Frio PRIMARY KEY (Pk_Alerta_Cadena_Frio),
    INDEX idxCorrelativoEncabezadoViajeAlertaCadenaFrio (Fk_Encabezado_Viaje_Codigo),
    INDEX idxIdTelemetriaTemperaturaAlertaCadenaFrio (Fk_Telemetria_Temperatura_Codigo),
    INDEX idxAtendidaAlertaCadenaFrio (atendidaAlertaCadenaFrio),
    CONSTRAINT Fk_Alerta_Cadena_Frio_Encabezado_Viaje FOREIGN KEY (Fk_Encabezado_Viaje_Codigo) REFERENCES tblEncabezadoViaje(Pk_Encabezado_Viaje) ON DELETE CASCADE,
    CONSTRAINT Fk_Alerta_Cadena_Frio_Telemetria_Temperatura FOREIGN KEY (Fk_Telemetria_Temperatura_Codigo) REFERENCES tblTelemetriaTemperatura(Pk_Telemetria_Temperatura) ON DELETE CASCADE
) ENGINE=InnoDB;


/* RESPONSABLE VELVETH SARAI CHAVEZ MEJIA*/ 
-- tblSucursal
CREATE TABLE IF NOT EXISTS tblSucursal (
Pk_Sucursal INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreSucursal VARCHAR(100) NOT NULL,
    descripcionSucursal VARCHAR(255) NOT NULL,
    ubicacionSucursal VARCHAR(500) NOT NULL,
    paisSucursal VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- tblBodega
CREATE TABLE IF NOT EXISTS tblBodega (
Pk_Bodega INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreBodega VARCHAR(100) NOT NULL,
    Fk_Sucursal_Codigo INT NOT NULL,
    paisBodega VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	
    UNIQUE KEY uqBodegaSucursal (Pk_Bodega,Fk_Sucursal_Codigo),
    CONSTRAINT Fk_Bodega_Sucursal FOREIGN KEY (Fk_Sucursal_Codigo) REFERENCES tblSucursal(Pk_Sucursal) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblCategoria
CREATE TABLE IF NOT EXISTS tblCategoria (
Pk_Categoria INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreCategoria VARCHAR(100) NOT NULL,
    porcentajeComisionCategoria DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqCategoriaNombre UNIQUE (nombreCategoria),
    CONSTRAINT ckPorcentajeComisionCategoriaRango CHECK (porcentajeComisionCategoria >= 0.00 AND porcentajeComisionCategoria <= 0.99)
) ENGINE=InnoDB;

-- tblLinea
CREATE TABLE IF NOT EXISTS tblLinea (
Pk_Linea INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreLinea VARCHAR(100) NOT NULL,
    porcentajeComisionLinea DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqLineaNombre UNIQUE (nombreLinea),
    CONSTRAINT ckPorcentajeComisionLineaRango CHECK (porcentajeComisionLinea >= 0.00 AND porcentajeComisionLinea <= 0.99)
) ENGINE=InnoDB;

-- tblMarca
CREATE TABLE IF NOT EXISTS tblMarca (
Pk_Marca INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreMarca VARCHAR(100) NOT NULL,
    porcentajeComisionMarca DECIMAL(3,2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqMarcaNombre UNIQUE (nombreMarca),
    CONSTRAINT ckPorcentajeComisionMarcaRango CHECK (porcentajeComisionMarca >= 0.00 AND porcentajeComisionMarca <= 0.99)
) ENGINE=InnoDB;

-- tblProducto
CREATE TABLE IF NOT EXISTS tblProducto (
Pk_Producto INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    skuProducto VARCHAR(30) NOT NULL,
    clasificacionProducto CHAR(5) NOT NULL DEFAULT '?????',
    nombreProducto VARCHAR(100) NOT NULL,
    Fk_Marca_Codigo INT NOT NULL,
    Fk_Categoria_Codigo INT NOT NULL,
    Fk_Linea_Codigo INT NOT NULL,
    descripcionProducto VARCHAR(255) NOT NULL,
    imagenProducto LONGBLOB NOT NULL, -- Campo BLOB para almacenar la imagen (Método de conversión de Img a binario en aplicación)
    paisProducto VARCHAR(100) NOT NULL,
    precioCompraProducto DECIMAL(12,4) NOT NULL,
    precioVentaProducto DECIMAL(12,4) NOT NULL,
    
    porcentajeComisionProducto DECIMAL(3,2) NOT NULL,
    stockAnteriorProducto INT NOT NULL DEFAULT 0,
    stockActualProducto INT NOT NULL DEFAULT 0,
    cargosMesProducto INT NOT NULL DEFAULT 0,
    abonosMesProducto INT NOT NULL DEFAULT 0,
    cargosAcumuladosProducto BIGINT NOT NULL DEFAULT 0,
    abonosAcumuladosProducto BIGINT NOT NULL DEFAULT 0,
    stockMinimoProducto INT NOT NULL ,
    stockMaximoProducto INT NOT NULL ,
    pesoProducto DECIMAL(4,2) NOT NULL,

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT uqProductoSku UNIQUE (skuProducto),
    CONSTRAINT Fk_Producto_Marca FOREIGN KEY (Fk_Marca_Codigo) REFERENCES tblMarca(Pk_Marca) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Categoria FOREIGN KEY (Fk_Categoria_Codigo) REFERENCES tblCategoria(Pk_Categoria) ON DELETE RESTRICT,
    CONSTRAINT Fk_Producto_Linea FOREIGN KEY (Fk_Linea_Codigo) REFERENCES tblLinea(Pk_Linea) ON DELETE RESTRICT,
    CONSTRAINT ckPorcentajeComisionProductoRango CHECK (porcentajeComisionProducto >= 0.00 AND porcentajeComisionProducto <= 0.99)
) ENGINE=InnoDB;

-- tblStock
CREATE TABLE IF NOT EXISTS tblStock (
Pk_Stock INT AUTO_INCREMENT NOT NULL,
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

    estadoVentaStock BOOLEAN NOT NULL,
    estadoCompraStock BOOLEAN NOT NULL,

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_Stock PRIMARY KEY (Pk_Stock),
    CONSTRAINT uqStockSucursalBodegaProducto UNIQUE (Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo),

    CONSTRAINT Fk_Stock_Bodega FOREIGN KEY (Fk_Bodega_Codigo, Fk_Sucursal_Codigo)
    REFERENCES tblBodega(Pk_Bodega, Fk_Sucursal_Codigo) ON DELETE RESTRICT,

    CONSTRAINT Fk_Stock_Producto FOREIGN KEY (Fk_Producto_Codigo)
    REFERENCES tblProducto(Pk_Producto) ON DELETE RESTRICT,
    CONSTRAINT ckEstadoVentaStock CHECK (estadoVentaStock IN (0, 1)),
    CONSTRAINT ckEstadoCompraStock CHECK (estadoCompraStock IN (0, 1))
) ENGINE=InnoDB;

-- tblTipoMovimiento
CREATE TABLE IF NOT EXISTS tblTipoMovimiento (
Pk_Tipo_Movimiento INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreTipoMovimiento VARCHAR(100) NOT NULL,
    descripcionTipoMovimiento VARCHAR(255) NOT NULL,
	signoTipoMovimiento CHAR(1) NOT NULL,

	created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqTipoMovimientoNombre UNIQUE (nombreTipoMovimiento),
    CONSTRAINT ckSignoTipoMovimiento CHECK (signoTipoMovimiento IN ('+', '-'))
) ENGINE=InnoDB;

-- tblMovimientoInventario
CREATE TABLE IF NOT EXISTS tblMovimientoInventario (
Pk_Movimiento_Inventario INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Fk_Tipo_Movimiento_Codigo INT NOT NULL,

    Fk_Bodega_Origen_Codigo INT NULL,
    Fk_Bodega_Destino_Codigo INT NULL,
    Fk_Transporte_Codigo INT NULL,
    fechaMovimientoInventario DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    observacionesMovimientoInventario VARCHAR(500) NOT NULL,
    estadoMovimientoInventario ENUM('PENDIENTE', 'CUADRADO', 'CERRADO', 'ANULADO') NOT NULL DEFAULT 'PENDIENTE',
    Fk_Cierre_Movimiento_Codigo INT NULL COMMENT 'FK al cierre; permite NULL mientras el movimiento no se haya cerrado', 
    
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Fk_Movimiento_Inventario_Tipo_Movimiento FOREIGN KEY (Fk_Tipo_Movimiento_Codigo)
	REFERENCES tblTipoMovimiento(Pk_Tipo_Movimiento) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Bodega_Origen FOREIGN KEY (Fk_Bodega_Origen_Codigo)
    REFERENCES tblBodega(Pk_Bodega) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Bodega_Destino FOREIGN KEY (Fk_Bodega_Destino_Codigo)
    REFERENCES tblBodega(Pk_Bodega) ON DELETE RESTRICT,

    CONSTRAINT Fk_Movimiento_Inventario_Transporte FOREIGN KEY (Fk_Transporte_Codigo)
    REFERENCES tblTransporte(Pk_Transporte) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- tblDetalleMovimiento
CREATE TABLE IF NOT EXISTS tblDetalleMovimiento (
Pk_Detalle_Movimiento INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Fk_Movimiento_Inventario_Codigo INT NOT NULL,
	loteDetalleMovimiento VARCHAR(30) NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    cantidadDetalleMovimiento INT NOT NULL,
    precioCompraDetalleMovimiento DECIMAL(12,4) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Fk_Detalle_Movimiento_Movimiento_Inventario FOREIGN KEY (Fk_Movimiento_Inventario_Codigo)
    REFERENCES tblMovimientoInventario(Pk_Movimiento_Inventario) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Movimiento_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(Pk_Producto) ON DELETE RESTRICT
) ENGINE=InnoDB;

/* RESPONSABLE JOSE XAVIER BOLAÑOS TENAS */ 
-- tblTraslado
CREATE TABLE IF NOT EXISTS tblTraslado (
Pk_Traslado INT NOT NULL AUTO_INCREMENT,
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

    PRIMARY KEY (Pk_Traslado),
    UNIQUE KEY uqCodigoTraslado (codigoTraslado),
    CONSTRAINT Fk_Traslado_Bodega_Origen FOREIGN KEY (Fk_Bodega_Origen_Codigo) REFERENCES tblBodega(Pk_Bodega) ON DELETE RESTRICT,
    CONSTRAINT Fk_Traslado_Bodega_Destino FOREIGN KEY (Fk_Bodega_Destino_Codigo) REFERENCES tblBodega(Pk_Bodega) ON DELETE RESTRICT,
    CONSTRAINT Fk_Traslado_Transporte FOREIGN KEY (Fk_Transporte_Codigo) REFERENCES tblTransporte(Pk_Transporte) ON DELETE RESTRICT,
    CONSTRAINT ckTrasladoBodegasDistintas CHECK (Fk_Bodega_Origen_Codigo <> Fk_Bodega_Destino_Codigo) -- Origen y destino no pueden ser la misma bodega
) ENGINE=InnoDB;

-- tblDetalleTraslado
CREATE TABLE IF NOT EXISTS tblDetalleTraslado (
Pk_Detalle_Traslado INT NOT NULL AUTO_INCREMENT,
    Fk_Traslado_Codigo INT NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    cantidadDetalleTraslado INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_Detalle_Traslado PRIMARY KEY (Pk_Detalle_Traslado),
    CONSTRAINT uqDetalleTrasladoProducto UNIQUE (Fk_Traslado_Codigo, Fk_Producto_Codigo),
    CONSTRAINT Fk_Detalle_Traslado_Traslado FOREIGN KEY (Fk_Traslado_Codigo) REFERENCES tblTraslado(Pk_Traslado) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Traslado_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(Pk_Producto) ON DELETE RESTRICT,
    CONSTRAINT ckDetalleTrasladoCantidad CHECK (cantidadDetalleTraslado > 0)
) ENGINE=InnoDB;

/* RESPONSABLE CARLOS EDUARDO CHIN CAAL */ 
-- tblAuditoriaInventario
CREATE TABLE IF NOT EXISTS tblAuditoriaInventario (
Pk_Auditoria_Inventario INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Fk_Movimiento_Inventario_Codigo INT NOT NULL,
    fechaAuditoriaInventario DATETIME NOT NULL,
    resultadoAuditoriaInventario VARCHAR(100) NOT NULL,
    observacionesCalidadAuditoriaInventario TEXT NOT NULL, -- Observaciones extensas, pueden superar 255 caracteres
    detallesAuditoriaInventario TEXT NOT NULL, -- Detalles extensos de auditoria, sin limite corto predefinido
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Fk_Auditoria_Inventario_Movimiento_Inventario FOREIGN KEY (Fk_Movimiento_Inventario_Codigo) REFERENCES tblMovimientoInventario(Pk_Movimiento_Inventario) ON DELETE RESTRICT
) ENGINE=InnoDB;

/* RESPONSABLE NILLS BERDUCIDO GOMEZ */ 

-- tblCierreMovimiento
CREATE TABLE IF NOT EXISTS tblCierreMovimiento (
Pk_Cierre_Movimiento INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombreCierreMovimiento VARCHAR(100) NOT NULL,
    fechaInicioCierreMovimiento DATE NOT NULL,
    fechaFinCierreMovimiento DATE NOT NULL,
    estadoCierreMovimiento VARCHAR(30) NOT NULL DEFAULT 'ABIERTO',
    esCierreAnualCierreMovimiento BOOLEAN NOT NULL DEFAULT FALSE,
    toleranciaCierreMovimiento INT NOT NULL DEFAULT 0,
    diferenciaUnidadesCierreMovimiento INT NOT NULL DEFAULT 0,
    diferenciaMontoCierreMovimiento DECIMAL(12,4) NOT NULL DEFAULT 0,
    motivoAceptacionCierreMovimiento VARCHAR(500) NOT NULL,
    motivoAnulacionCierreMovimiento VARCHAR(500) NOT NULL,
    observacionesCierreMovimiento VARCHAR(500) NOT NULL,
    fechaEjecucionCierreMovimiento DATETIME NULL,
    CONSTRAINT uqCierreMovimientoFin UNIQUE (fechaFinCierreMovimiento),
    CONSTRAINT ckCierreMovimientoFechas CHECK (fechaInicioCierreMovimiento <= fechaFinCierreMovimiento),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- tblDetalleCierre
CREATE TABLE IF NOT EXISTS tblDetalleCierre (
Pk_Detalle_Cierre INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
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
    CONSTRAINT Fk_Detalle_Cierre_Cierre_Movimiento FOREIGN KEY (Fk_Cierre_Movimiento_Codigo) REFERENCES tblCierreMovimiento(Pk_Cierre_Movimiento) ON DELETE CASCADE,
    CONSTRAINT Fk_Detalle_Cierre_Stock FOREIGN KEY (Fk_Stock_Codigo) REFERENCES tblStock(Pk_Stock) ON DELETE RESTRICT,
    CONSTRAINT uqDetalleCierreStock UNIQUE (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo)
) ENGINE=InnoDB;


-- DOCUMENTOS Y CIERRES: vinculo y fotografia del inventario antes del cierre.
-- La tabla MovimientoInventario se crea antes que CierreMovimiento.
-- Comprobar la existencia del vinculo para poder ejecutar de nuevo el script.
SET @sqlFkCierre = (
    SELECT IF(
        EXISTS (
            SELECT 1
            FROM information_schema.KEY_COLUMN_USAGE
            WHERE TABLE_SCHEMA = DATABASE()
              AND TABLE_NAME = 'tblMovimientoInventario'
              AND COLUMN_NAME = 'Fk_Cierre_Movimiento_Codigo'
              AND REFERENCED_TABLE_NAME = 'tblCierreMovimiento'
              AND REFERENCED_COLUMN_NAME = 'Pk_Cierre_Movimiento'
        ),
        'SELECT ''La FK del cierre ya existe'' AS mensaje',
        'ALTER TABLE tblMovimientoInventario ADD CONSTRAINT Fk_Movimiento_Inventario_Cierre_Movimiento FOREIGN KEY (Fk_Cierre_Movimiento_Codigo) REFERENCES tblCierreMovimiento(Pk_Cierre_Movimiento) ON DELETE RESTRICT'
    )
);
PREPARE stmFkCierre FROM @sqlFkCierre;
EXECUTE stmFkCierre;
DEALLOCATE PREPARE stmFkCierre;

-- tblHistoricoInventario: copia permanente del final de cada periodo, por producto y ubicación.
CREATE TABLE IF NOT EXISTS tblHistoricoInventario (
    Pk_Historico_Inventario BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Fk_Cierre_Movimiento_Codigo INT NOT NULL,
    Fk_Stock_Codigo INT NOT NULL,
    Fk_Producto_Codigo INT NOT NULL,
    Fk_Sucursal_Codigo INT NOT NULL,
    Fk_Bodega_Codigo INT NOT NULL,
    fechaCorteHistoricoInventario DATE NOT NULL,
    stockAnteriorHistoricoInventario INT NOT NULL,
    stockActualHistoricoInventario INT NOT NULL,
    cargosMesHistoricoInventario INT NOT NULL,
    abonosMesHistoricoInventario INT NOT NULL,
    cargosAcumuladosHistoricoInventario BIGINT NOT NULL,
    abonosAcumuladosHistoricoInventario BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT uqHistoricoInventarioCierreStock UNIQUE (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo),
    CONSTRAINT Fk_Historico_Inventario_Cierre_Movimiento FOREIGN KEY (Fk_Cierre_Movimiento_Codigo)
        REFERENCES tblCierreMovimiento(Pk_Cierre_Movimiento) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Stock FOREIGN KEY (Fk_Stock_Codigo)
        REFERENCES tblStock(Pk_Stock) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Producto FOREIGN KEY (Fk_Producto_Codigo)
        REFERENCES tblProducto(Pk_Producto) ON DELETE RESTRICT,
    CONSTRAINT Fk_Historico_Inventario_Bodega FOREIGN KEY (Fk_Bodega_Codigo, Fk_Sucursal_Codigo)
        REFERENCES tblBodega(Pk_Bodega, Fk_Sucursal_Codigo) ON DELETE RESTRICT
) ENGINE=InnoDB;

/* RESPONSABLE GABRIELA PINTO GARCIA  */ 
-- tblServicioTransporteExterno
CREATE TABLE IF NOT EXISTS tblServicioTransporteExterno (
Pk_Servicio_Transporte_Externo INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
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
 CONSTRAINT Fk_Servicio_Transporte_Externo_Empresa_Transportista FOREIGN KEY (Fk_Empresa_Transportista_Codigo) REFERENCES tblEmpresaTransportista(Pk_Empresa_Transportista) ON DELETE RESTRICT,
 CONSTRAINT Fk_Servicio_Transporte_Externo_Tipo_Servicio FOREIGN KEY (Fk_Tipo_Servicio_Codigo) REFERENCES tblTipoServicio(Pk_Tipo_Servicio) ON DELETE RESTRICT,
 CONSTRAINT Fk_Servicio_Transporte_Externo_Traslado FOREIGN KEY (Fk_Traslado_Codigo) REFERENCES tblTraslado(Pk_Traslado) ON DELETE RESTRICT,
    CONSTRAINT ckServicioEstado CHECK (estadoServicioTransporteExterno IN ('Programado','En tránsito','Entregado','Cancelado')),
    CONSTRAINT ckServicioFechas CHECK (fechaEstimadaEntregaServicioTransporteExterno >= fechaContratacionServicioTransporteExterno)
) ENGINE=InnoDB;

-- tblCostoTransporte
CREATE TABLE IF NOT EXISTS tblCostoTransporte (
Pk_Costo_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 Fk_Tarifa_Transporte_Codigo INT NOT NULL,
 cantidadCostoTransporte DECIMAL(12,4) NOT NULL,
 costoUnitarioCostoTransporte DECIMAL(12,4) NOT NULL,
 costoTotalCostoTransporte DECIMAL(12,4) NOT NULL,
 monedaCostoTransporte CHAR(3) NOT NULL,
 tipoPagoCostoTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqCostoServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Costo_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE,
 CONSTRAINT Fk_Costo_Transporte_Tarifa_Transporte FOREIGN KEY (Fk_Tarifa_Transporte_Codigo) REFERENCES tblTarifaTransporte(Pk_Tarifa_Transporte) ON DELETE RESTRICT,
    CONSTRAINT ckCostoCantidad CHECK (cantidadCostoTransporte > 0),
    CONSTRAINT ckCostoUnitario CHECK (costoUnitarioCostoTransporte >= 0),
    CONSTRAINT ckCostoMoneda CHECK (monedaCostoTransporte IN ('GTQ','USD')),
    CONSTRAINT ckCostoTipoPago CHECK (tipoPagoCostoTransporte IN ('Transferencia','Efectivo'))
) ENGINE=InnoDB;

-- tblVehiculoTransporte
CREATE TABLE IF NOT EXISTS tblVehiculoTransporte (
Pk_Vehiculo_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 placaVehiculoTransporte VARCHAR(30) NOT NULL,
 tipoVehiculoTransporte VARCHAR(100) NOT NULL,
 capacidadVehiculoTransporte VARCHAR(100) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqVehiculoServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Vehiculo_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblConductorTransporte
CREATE TABLE IF NOT EXISTS tblConductorTransporte (
Pk_Conductor_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 nombreConductorTransporte VARCHAR(100) NOT NULL,
 telefonoConductorTransporte VARCHAR(30) COLLATE utf8mb4_unicode_ci NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqConductorServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT Fk_Conductor_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblProductoTransporte
CREATE TABLE IF NOT EXISTS tblProductoTransporte (
Pk_Producto_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 Fk_Producto_Codigo INT NOT NULL,
 cantidadProductoTransporte DECIMAL(12,4) NOT NULL,
 unidadMedidaProductoTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Producto_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE,
 CONSTRAINT Fk_Producto_Transporte_Producto FOREIGN KEY (Fk_Producto_Codigo) REFERENCES tblProducto(Pk_Producto) ON DELETE RESTRICT,
    CONSTRAINT ckProductoCantidad CHECK (cantidadProductoTransporte > 0)
) ENGINE=InnoDB;

-- tblUbicacionTransporte
CREATE TABLE IF NOT EXISTS tblUbicacionTransporte (
Pk_Ubicacion_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 ubicacionActualUbicacionTransporte VARCHAR(500) NOT NULL,
 fechaActualizacionUbicacionTransporte DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Ubicacion_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE,
 INDEX idxUbicacionServicioFecha (Fk_Servicio_Transporte_Externo_Codigo, fechaActualizacionUbicacionTransporte)
) ENGINE=InnoDB;

-- tblIncidenciaTransporte
CREATE TABLE IF NOT EXISTS tblIncidenciaTransporte (
Pk_Incidencia_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
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
 CONSTRAINT Fk_Incidencia_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblHistorialTransporte
CREATE TABLE IF NOT EXISTS tblHistorialTransporte (
Pk_Historial_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 fechaMovimientoHistorialTransporte DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
 tipoMovimientoHistorialTransporte VARCHAR(100) NOT NULL,
 descripcionMovimientoHistorialTransporte VARCHAR(255) NOT NULL,
 estadoTransporteHistorialTransporte VARCHAR(30) NOT NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT Fk_Historial_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo) REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE
) ENGINE=InnoDB;

-- tblSolicitudPagoTransporte
CREATE TABLE IF NOT EXISTS tblSolicitudPagoTransporte (
Pk_Solicitud_Pago_Transporte INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Fk_Servicio_Transporte_Externo_Codigo INT NOT NULL,
 codigoSolicitudPagoTransporte VARCHAR(30) NOT NULL,
 estadoSolicitudPagoTransporte VARCHAR(30) NOT NULL DEFAULT 'Solicitado',
 referenciaBancariaSolicitudPagoTransporte VARCHAR(100) NULL,
 fechaPagoSolicitudPagoTransporte DATE NULL,
 created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 CONSTRAINT uqSolicitudServicio UNIQUE (Fk_Servicio_Transporte_Externo_Codigo),
 CONSTRAINT uqSolicitudCodigo UNIQUE (codigoSolicitudPagoTransporte),
 CONSTRAINT Fk_Solicitud_Pago_Transporte_Servicio_Transporte_Externo FOREIGN KEY (Fk_Servicio_Transporte_Externo_Codigo)
  REFERENCES tblServicioTransporteExterno(Pk_Servicio_Transporte_Externo) ON DELETE CASCADE
) ENGINE=InnoDB;

-- VISTA DE SOLICITUDES DE PAGO
CREATE OR REPLACE VIEW vwSolicitudesPagoTransporte AS
SELECT s.codigoSolicitudPagoTransporte AS solicitud,
       t.numeroGuiaServicioTransporteExterno AS numeroGuia,
       e.nombreEmpresaTransportista AS beneficiario,
       CASE ts.nombreTipoServicio
           WHEN 'Refrigerado' THEN 'Servicio de transporte externo refrigerado'
           WHEN 'Congelado' THEN 'Servicio de transporte externo congelado'
           WHEN 'Carga seca' THEN 'Servicio de transporte externo de carga seca'
           WHEN 'Carga especial' THEN 'Servicio de transporte externo de carga especial'
           ELSE CONCAT('Servicio de transporte externo ', LOWER(ts.nombreTipoServicio))
       END AS concepto,
       c.costoTotalCostoTransporte AS montoTotal,
       s.fechaPagoSolicitudPagoTransporte AS fechaPago,
       s.estadoSolicitudPagoTransporte AS estadoSolicitud,
       s.referenciaBancariaSolicitudPagoTransporte AS referenciaBancaria
FROM tblServicioTransporteExterno t
JOIN tblEmpresaTransportista e ON e.Pk_Empresa_Transportista = t.Fk_Empresa_Transportista_Codigo
JOIN tblTipoServicio ts ON ts.Pk_Tipo_Servicio = t.Fk_Tipo_Servicio_Codigo
JOIN tblCostoTransporte c ON c.Fk_Servicio_Transporte_Externo_Codigo = t.Pk_Servicio_Transporte_Externo
JOIN tblSolicitudPagoTransporte s ON s.Fk_Servicio_Transporte_Externo_Codigo = t.Pk_Servicio_Transporte_Externo;


DROP PROCEDURE IF EXISTS spGenerarSolicitudPagoTransporte;
DELIMITER $$
-- La referencia bancaria y la fecha de pago pueden quedar NULL mientras el pago esté pendiente.
CREATE PROCEDURE spGenerarSolicitudPagoTransporte(
    IN pIdServicio INT,
    IN pReferenciaBancaria VARCHAR(100),
    IN pFechaPago DATE
)
BEGIN
    DECLARE vId INT;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;
    START TRANSACTION;
    IF NOT EXISTS (SELECT 1 FROM tblCostoTransporte
                   WHERE Fk_Servicio_Transporte_Externo_Codigo = pIdServicio) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El servicio no existe o no tiene costo registrado';
    END IF;
    INSERT INTO tblSolicitudPagoTransporte (
        Fk_Servicio_Transporte_Externo_Codigo,
        codigoSolicitudPagoTransporte,
        referenciaBancariaSolicitudPagoTransporte,
        fechaPagoSolicitudPagoTransporte
    ) VALUES (
        pIdServicio,
        CONCAT('TMP-', LEFT(REPLACE(UUID(), '-', ''), 26)),
        pReferenciaBancaria,
        pFechaPago
    );
    SET vId = LAST_INSERT_ID();
    UPDATE tblSolicitudPagoTransporte
    SET codigoSolicitudPagoTransporte = CONCAT('SP-', LPAD(vId, 6, '0'))
    WHERE Pk_Solicitud_Pago_Transporte = vId;
    COMMIT;
    SELECT vId AS idSolicitud, CONCAT('SP-', LPAD(vId, 6, '0')) AS codigoSolicitudPago;
END$$
DELIMITER ;

DROP PROCEDURE IF EXISTS spRegistrarServicioTransporteExterno;
DELIMITER $$
CREATE PROCEDURE spRegistrarServicioTransporteExterno(
    IN pEmpresa INT, IN pTipoServicio INT, IN pTraslado INT,
    IN pOrigen VARCHAR(100), IN pDireccionOrigen VARCHAR(500),
    IN pDestino VARCHAR(100), IN pDireccionDestino VARCHAR(500),
    IN pFechaContratacion DATE, IN pFechaEstimada DATE, IN pFechaReal DATE,
    IN pTarifa INT, IN pCantidad DECIMAL(12,4), IN pCosto DECIMAL(12,4),
    IN pMoneda CHAR(3), IN pTipoPago VARCHAR(30)
)
BEGIN
    DECLARE vId INT;
    DECLARE vEmpresa VARCHAR(100);
    DECLARE vPrefijo VARCHAR(10);
    DECLARE vGuia VARCHAR(30);
    DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;
    START TRANSACTION;
    IF pTraslado IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El traslado es obligatorio';
    END IF;
    SELECT nombreEmpresaTransportista INTO vEmpresa
    FROM tblEmpresaTransportista WHERE Pk_Empresa_Transportista = pEmpresa;
    IF vEmpresa IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Empresa transportista no válida';
    END IF;
    SET vPrefijo = CASE UPPER(TRIM(vEmpresa))
        WHEN 'FORZA' THEN 'FZ' WHEN 'CARGO EXPRESO' THEN 'CE'
        WHEN 'GUATEX' THEN 'GX' WHEN 'DHL EXPRESS' THEN 'DHL'
        WHEN 'FEDEX' THEN 'FDX' WHEN 'UPS' THEN 'UPS' ELSE 'TR' END;
    INSERT INTO tblServicioTransporteExterno (
       Fk_Empresa_Transportista_Codigo, Fk_Tipo_Servicio_Codigo, Fk_Traslado_Codigo,
       origenServicioTransporteExterno, direccionOrigenServicioTransporteExterno,
       destinoServicioTransporteExterno, direccionDestinoServicioTransporteExterno,
       fechaContratacionServicioTransporteExterno, fechaEstimadaEntregaServicioTransporteExterno,
       fechaEntregaRealServicioTransporteExterno,
       numeroGuiaServicioTransporteExterno
    ) VALUES (pEmpresa, pTipoServicio, pTraslado, pOrigen, pDireccionOrigen,
              pDestino, pDireccionDestino, pFechaContratacion, pFechaEstimada, pFechaReal,
              CONCAT('TMP-', LEFT(REPLACE(UUID(), '-', ''), 26)));
    SET vId = LAST_INSERT_ID();
    SET vGuia = CONCAT(vPrefijo, '-', LPAD(vId, 6, '0'));
    UPDATE tblServicioTransporteExterno
    SET numeroGuiaServicioTransporteExterno = vGuia
    WHERE Pk_Servicio_Transporte_Externo = vId;
    INSERT INTO tblCostoTransporte (
       Fk_Servicio_Transporte_Externo_Codigo, Fk_Tarifa_Transporte_Codigo,
       cantidadCostoTransporte, costoUnitarioCostoTransporte, costoTotalCostoTransporte,
       monedaCostoTransporte, tipoPagoCostoTransporte
    ) VALUES (vId, pTarifa, pCantidad, pCosto, ROUND(pCantidad * pCosto, 4), pMoneda, pTipoPago);
    COMMIT;
    SELECT vId AS idServicio, vGuia AS numeroGuia;
END$$
DELIMITER ;


-- ============================================================================
-- CIERRE MENSUAL DE INVENTARIO (compatible con MySQL 8.0+)
-- Los contadores mensuales se actualizan AL CONFIRMAR CADA MOVIMIENTO en la
-- aplicación/servicio transaccional. Este procedimiento NO los vuelve a sumar.
-- stockActualStock debe representar stockAnteriorStock + cargosMes - abonosMes
-- antes del cierre. El cierre guarda la fotografía, acumula, y reinicia el mes.
-- ============================================================================

DROP PROCEDURE IF EXISTS spEjecutarCierreInventario;
DELIMITER $$
CREATE PROCEDURE spEjecutarCierreInventario(IN pIdCierre INT)
BEGIN
    DECLARE vInicio DATE;
    DECLARE vFin DATE;
    DECLARE vEstado VARCHAR(30);
    DECLARE vCantidad INT DEFAULT 0;
    DECLARE vUltimoFin DATE;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;
    SELECT fechaInicioCierreMovimiento, fechaFinCierreMovimiento, estadoCierreMovimiento
      INTO vInicio, vFin, vEstado
      FROM tblCierreMovimiento
     WHERE Pk_Cierre_Movimiento = pIdCierre FOR UPDATE;

    IF vInicio IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Cierre no encontrado';
    END IF;
    IF vEstado <> 'ABIERTO' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Solo se puede ejecutar un cierre ABIERTO';
    END IF;
    IF vInicio <> DATE_FORMAT(vInicio, '%Y-%m-01')
       OR vFin <> LAST_DAY(vInicio) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El cierre debe abarcar un mes calendario completo';
    END IF;
    IF vFin >= CURRENT_DATE() THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Solo puede cerrarse un mes ya terminado';
    END IF;

    SELECT MAX(fechaFinCierreMovimiento) INTO vUltimoFin
      FROM tblCierreMovimiento
     WHERE estadoCierreMovimiento = 'CERRADO';
    IF vUltimoFin IS NOT NULL AND vInicio <> DATE_ADD(vUltimoFin, INTERVAL 1 DAY) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El cierre debe seguir consecutivamente al ultimo cierre';
    END IF;

    -- Bloquear existencias durante todo el corte; ejecutar sin movimientos concurrentes.
    UPDATE tblStock SET stockActualStock = stockActualStock;

    -- No puede haber documentos sin revisar ni datos de detalle defectuosos.
    SELECT COUNT(*) INTO vCantidad
      FROM tblMovimientoInventario m
     WHERE m.fechaMovimientoInventario >= vInicio
       AND m.fechaMovimientoInventario < DATE_ADD(vFin, INTERVAL 1 DAY)
       AND m.estadoMovimientoInventario NOT IN ('CUADRADO', 'ANULADO');
    IF vCantidad > 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Hay documentos pendientes de cuadre';
    END IF;

    SELECT COUNT(*) INTO vCantidad
      FROM tblMovimientoInventario m
     WHERE m.fechaMovimientoInventario >= vInicio
       AND m.fechaMovimientoInventario < DATE_ADD(vFin, INTERVAL 1 DAY)
       AND m.estadoMovimientoInventario = 'CUADRADO'
       AND NOT EXISTS (SELECT 1 FROM tblDetalleMovimiento d
                        WHERE d.Fk_Movimiento_Inventario_Codigo = m.Pk_Movimiento_Inventario
                          AND d.cantidadDetalleMovimiento > 0);
    IF vCantidad > 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Hay movimientos cuadrados sin detalle valido';
    END IF;

    SELECT COUNT(*) INTO vCantidad FROM tblStock
     WHERE stockAnteriorStock < 0 OR stockActualStock < 0
        OR cargosMesStock < 0 OR abonosMesStock < 0
        OR stockActualStock <> stockAnteriorStock + cargosMesStock - abonosMesStock;
    IF vCantidad > 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Existen saldos sin cuadrar en stock';
    END IF;

    -- Fotografía del periodo ANTES de cualquier actualización (incluye ceros).
    INSERT INTO tblHistoricoInventario
        (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo, Fk_Producto_Codigo,
         Fk_Sucursal_Codigo, Fk_Bodega_Codigo, fechaCorteHistoricoInventario,
         stockAnteriorHistoricoInventario, stockActualHistoricoInventario,
         cargosMesHistoricoInventario, abonosMesHistoricoInventario,
         cargosAcumuladosHistoricoInventario, abonosAcumuladosHistoricoInventario)
    SELECT pIdCierre, Pk_Stock, Fk_Producto_Codigo, Fk_Sucursal_Codigo,
           Fk_Bodega_Codigo, vFin, stockAnteriorStock, stockActualStock,
           cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock
      FROM tblStock;

    -- La tabla DetalleCierre conserva el resumen del corte.
    INSERT INTO tblDetalleCierre
        (Fk_Cierre_Movimiento_Codigo, Fk_Stock_Codigo,
         stockAnteriorDetalleCierre, stockActualDetalleCierre,
         cargosMesDetalleCierre, abonosMesDetalleCierre,
         cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
    SELECT pIdCierre, Pk_Stock, stockAnteriorStock, stockActualStock,
           cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock
      FROM tblStock;

    -- Inmovilizar documentos del mes antes de marcar cerrado el corte.
    UPDATE tblMovimientoInventario
       SET estadoMovimientoInventario = 'CERRADO', Fk_Cierre_Movimiento_Codigo = pIdCierre
     WHERE fechaMovimientoInventario >= vInicio
       AND fechaMovimientoInventario < DATE_ADD(vFin, INTERVAL 1 DAY)
       AND estadoMovimientoInventario = 'CUADRADO';

    -- El stock actual permanece; el anterior pasa a ser el stock actual.
    UPDATE tblStock
       SET stockAnteriorStock = stockActualStock,
           cargosAcumuladosStock = cargosAcumuladosStock + cargosMesStock,
           abonosAcumuladosStock = abonosAcumuladosStock + abonosMesStock,
           cargosMesStock = 0,
           abonosMesStock = 0;

    -- Los totales por producto son la suma de sus ubicaciones.
    UPDATE tblProducto p
    LEFT JOIN (
        SELECT Fk_Producto_Codigo AS idProducto,
               SUM(stockAnteriorStock) AS anterior, SUM(stockActualStock) AS actual,
               SUM(cargosMesStock) AS cargos, SUM(abonosMesStock) AS abonos,
               SUM(cargosAcumuladosStock) AS cargosAc,
               SUM(abonosAcumuladosStock) AS abonosAc
          FROM tblStock GROUP BY Fk_Producto_Codigo
    ) a ON a.idProducto = p.Pk_Producto
       SET p.stockAnteriorProducto = COALESCE(a.anterior, 0),
           p.stockActualProducto = COALESCE(a.actual, 0),
           p.cargosMesProducto = COALESCE(a.cargos, 0),
           p.abonosMesProducto = COALESCE(a.abonos, 0),
           p.cargosAcumuladosProducto = COALESCE(a.cargosAc, 0),
           p.abonosAcumuladosProducto = COALESCE(a.abonosAc, 0);

    UPDATE tblCierreMovimiento
       SET estadoCierreMovimiento = 'CERRADO',
           fechaEjecucionCierreMovimiento = NOW()
     WHERE Pk_Cierre_Movimiento = pIdCierre;
    COMMIT;
END$$
DELIMITER ;

-- Evitar modificaciones o borrados de movimientos de periodos cerrados.
-- Eliminar nombres anteriores y actuales para permitir ejecutar de nuevo el script.
DROP TRIGGER IF EXISTS trMovimientoInventarioNoEditarCerrado;
DROP TRIGGER IF EXISTS trMovimientoInventarioBeforeUpdateCerrado;
DELIMITER $$
CREATE TRIGGER trMovimientoInventarioBeforeUpdateCerrado
BEFORE UPDATE ON tblMovimientoInventario FOR EACH ROW
BEGIN
    IF OLD.estadoMovimientoInventario = 'CERRADO' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Documento cerrado: no se puede editar';
    END IF;
END$$
DELIMITER ;

DROP TRIGGER IF EXISTS trMovimientoInventarioNoBorrarCerrado;
DROP TRIGGER IF EXISTS trMovimientoInventarioBeforeDeleteCerrado;
DELIMITER $$
CREATE TRIGGER trMovimientoInventarioBeforeDeleteCerrado
BEFORE DELETE ON tblMovimientoInventario FOR EACH ROW
BEGIN
    IF OLD.estadoMovimientoInventario = 'CERRADO' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Documento cerrado: no se puede borrar';
    END IF;
END$$
DELIMITER ;

DROP TRIGGER IF EXISTS trDetalleMovimientoNoEditarCerrado;
DROP TRIGGER IF EXISTS trDetalleMovimientoBeforeUpdateCerrado;
DELIMITER $$
CREATE TRIGGER trDetalleMovimientoBeforeUpdateCerrado
BEFORE UPDATE ON tblDetalleMovimiento FOR EACH ROW
BEGIN
    IF EXISTS (SELECT 1 FROM tblMovimientoInventario m
               WHERE m.Pk_Movimiento_Inventario = OLD.Fk_Movimiento_Inventario_Codigo
                 AND m.estadoMovimientoInventario = 'CERRADO')
       OR EXISTS (SELECT 1 FROM tblMovimientoInventario m
               WHERE m.Pk_Movimiento_Inventario = NEW.Fk_Movimiento_Inventario_Codigo
                 AND m.estadoMovimientoInventario = 'CERRADO') THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Detalle de documento cerrado: no se puede editar';
    END IF;
END$$
DELIMITER ;

DROP TRIGGER IF EXISTS trDetalleMovimientoNoBorrarCerrado;
DROP TRIGGER IF EXISTS trDetalleMovimientoBeforeDeleteCerrado;
DELIMITER $$
CREATE TRIGGER trDetalleMovimientoBeforeDeleteCerrado
BEFORE DELETE ON tblDetalleMovimiento FOR EACH ROW
BEGIN
    IF EXISTS (SELECT 1 FROM tblMovimientoInventario m
               WHERE m.Pk_Movimiento_Inventario = OLD.Fk_Movimiento_Inventario_Codigo
                 AND m.estadoMovimientoInventario = 'CERRADO') THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Detalle de documento cerrado: no se puede borrar';
    END IF;
END$$
DELIMITER ;

DROP TRIGGER IF EXISTS trDetalleMovimientoNoAgregarCerrado;
DROP TRIGGER IF EXISTS trDetalleMovimientoBeforeInsertCerrado;
DELIMITER $$
CREATE TRIGGER trDetalleMovimientoBeforeInsertCerrado
BEFORE INSERT ON tblDetalleMovimiento FOR EACH ROW
BEGIN
    IF EXISTS (SELECT 1 FROM tblMovimientoInventario m
               WHERE m.Pk_Movimiento_Inventario = NEW.Fk_Movimiento_Inventario_Codigo
                 AND m.estadoMovimientoInventario = 'CERRADO') THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Detalle de documento cerrado: no se puede agregar';
    END IF;
END$$
DELIMITER ;


-- Verificar los 3 procedimientos y 5 triggers:
SELECT ROUTINE_NAME, ROUTINE_TYPE
FROM information_schema.ROUTINES
WHERE ROUTINE_SCHEMA = DATABASE()
  AND ROUTINE_NAME IN (
    'spGenerarSolicitudPagoTransporte',
    'spRegistrarServicioTransporteExterno',
    'spEjecutarCierreInventario'
  );

SELECT TRIGGER_NAME, EVENT_MANIPULATION, EVENT_OBJECT_TABLE
FROM information_schema.TRIGGERS
WHERE TRIGGER_SCHEMA = DATABASE()
  AND TRIGGER_NAME IN (
    'trMovimientoInventarioBeforeUpdateCerrado',
    'trMovimientoInventarioBeforeDeleteCerrado',
    'trDetalleMovimientoBeforeUpdateCerrado',
    'trDetalleMovimientoBeforeDeleteCerrado',
    'trDetalleMovimientoBeforeInsertCerrado'
  );

         
-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡ Ventas y Cuentas por Cobrar !!!!!!!!!!!!!!!!!!!!!!!!!-------------
CREATE TABLE `tblCliente` (
  `Pk_Cliente` INT NOT NULL AUTO_INCREMENT,
  `codigoCliente` VARCHAR(30) NOT NULL,
  `tipoIdentificacionCliente` ENUM('NIT','CUI','EXTRANJERO','CF') NOT NULL,
  `nombreCliente` VARCHAR(100) NOT NULL,
  `direccionCliente` VARCHAR(500) NULL,
  `nombreContactoCliente` VARCHAR(100) NULL,
  `telefonoCliente` VARCHAR(30) NULL,
  `correoCliente` VARCHAR(150) NULL,
  `diasCreditoCliente` INT NOT NULL DEFAULT 0,
  `limiteCreditoCliente` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `Fk_Moneda_Credito` BIGINT NOT NULL,
  `activoCliente` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `nitCliente` CHAR(10) NULL,
  `dpiCliente` CHAR(13) NULL,
  `identificacionExtranjeraCliente` VARCHAR(30) NULL,
  CONSTRAINT `Pk_Cliente` PRIMARY KEY (`Pk_Cliente`),
  CONSTRAINT `uqClientecodigo` UNIQUE (`codigoCliente`),
  CONSTRAINT `uqClientenit` UNIQUE (`nitCliente`),
  CONSTRAINT `uqClientedpi` UNIQUE (`dpiCliente`),
  CONSTRAINT `uqClienteidentificacionExtranjera` UNIQUE (`identificacionExtranjeraCliente`),
  CONSTRAINT `Fk_Cliente_Moneda_Credito` FOREIGN KEY (`Fk_Moneda_Credito`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxClienteMonedaCredito` (`Fk_Moneda_Credito`),
  CONSTRAINT `ckClienteactivoClienteBooleano` CHECK (activoCliente IN (0,1)),
  CONSTRAINT `ckClienteIdentificacionCoherente` CHECK ((tipoIdentificacionCliente = 'CF' AND nitCliente IS NULL AND dpiCliente IS NULL AND identificacionExtranjeraCliente IS NULL) OR (tipoIdentificacionCliente = 'NIT' AND nitCliente IS NOT NULL AND dpiCliente IS NULL AND identificacionExtranjeraCliente IS NULL) OR (tipoIdentificacionCliente = 'CUI' AND dpiCliente IS NOT NULL AND nitCliente IS NULL AND identificacionExtranjeraCliente IS NULL) OR (tipoIdentificacionCliente = 'EXTRANJERO' AND identificacionExtranjeraCliente IS NOT NULL AND nitCliente IS NULL AND dpiCliente IS NULL)),
  CONSTRAINT `ckClienteCreditoNoNegativo` CHECK (diasCreditoCliente >= 0 AND limiteCreditoCliente >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblVendedor` (
  `Pk_Vendedor` INT NOT NULL AUTO_INCREMENT,
  `codigoVendedor` VARCHAR(30) NOT NULL,
  `Fk_Empleado_Codigo` INT NOT NULL,
  `activoVendedor` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_Vendedor` PRIMARY KEY (`Pk_Vendedor`),
  CONSTRAINT `uqVendedorcodigo` UNIQUE (`codigoVendedor`),
  CONSTRAINT `uqVendedorEmpleadoCodigo` UNIQUE (`Fk_Empleado_Codigo`),
  CONSTRAINT `Fk_Vendedor_Empleado` FOREIGN KEY (`Fk_Empleado_Codigo`)
    REFERENCES `tblEmpleado` (`idEmpleado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `ckVendedoractivoVendedorBooleano` CHECK (activoVendedor IN (0,1))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblProductoVenta` (
  `Pk_ProductoVenta` INT NOT NULL,
  `tipoItemProductoVenta` CHAR(1) NOT NULL,
  `unidadMedidaProductoVenta` VARCHAR(30) NOT NULL,
  `tasaImpuestoProductoVenta` DECIMAL(3,2) NOT NULL DEFAULT 0,
  `activoProductoVenta` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ProductoVenta` PRIMARY KEY (`Pk_ProductoVenta`),
  CONSTRAINT `Fk_ProductoVenta_Producto` FOREIGN KEY (`Pk_ProductoVenta`)
    REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `ckProductoVentatasaImpuestoProductoVentaRango` CHECK (tasaImpuestoProductoVenta BETWEEN 0 AND 0.99),
  CONSTRAINT `ckProductoVentaactivoProductoVentaBooleano` CHECK (activoProductoVenta IN (0,1)),
  CONSTRAINT `ckProductoVentaTipoItemValido` CHECK (tipoItemProductoVenta IN ('B','S'))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblListaPrecioEncabezado` (
  `Pk_ListaPrecioEncabezado` INT NOT NULL AUTO_INCREMENT,
  `codigoListaPrecioEncabezado` VARCHAR(30) NOT NULL,
  `nombreListaPrecioEncabezado` VARCHAR(100) NOT NULL,
  `Fk_Moneda_Codigo` BIGINT NOT NULL,
  `vigenteDesdeListaPrecioEncabezado` DATE NOT NULL,
  `vigenteHastaListaPrecioEncabezado` DATE NULL,
  `activoListaPrecioEncabezado` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ListaPrecioEncabezado` PRIMARY KEY (`Pk_ListaPrecioEncabezado`),
  CONSTRAINT `uqListaPrecioEncabezadocodigo` UNIQUE (`codigoListaPrecioEncabezado`),
  CONSTRAINT `Fk_ListaPrecioEncabezado_Moneda` FOREIGN KEY (`Fk_Moneda_Codigo`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxListaPrecioEncabezadoMonedaCodigo` (`Fk_Moneda_Codigo`),
  CONSTRAINT `ckListaPrecioEncabezadoactivoListaPrecioEncabezadoBooleano` CHECK (activoListaPrecioEncabezado IN (0,1)),
  CONSTRAINT `ckListaPrecioEncabezadoFechasCoherentes` CHECK (vigenteHastaListaPrecioEncabezado >= vigenteDesdeListaPrecioEncabezado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblCierreMovimientoCxCEncabezado` (
  `Pk_CierreMovimientoCxCEncabezado` INT NOT NULL AUTO_INCREMENT,
  `numeroCierreMovimientoCxCEncabezado` VARCHAR(30) NOT NULL,
  `fechaDesdeCierreMovimientoCxCEncabezado` DATE NOT NULL,
  `fechaHastaCierreMovimientoCxCEncabezado` DATE NOT NULL,
  `fechaCorteCierreMovimientoCxCEncabezado` DATE NOT NULL,
  `Fk_Moneda_Codigo` BIGINT NOT NULL,
  `tipoCierreMovimientoCxCEncabezado` VARCHAR(20) NOT NULL,
  `estadoCierreMovimientoCxCEncabezado` VARCHAR(20) NOT NULL,
  `documentosRevisadosCierreMovimientoCxCEncabezado` BOOLEAN NOT NULL DEFAULT 0,
  `pagosConciliadosCierreMovimientoCxCEncabezado` BOOLEAN NOT NULL DEFAULT 0,
  `observacionesCierreMovimientoCxCEncabezado` TEXT NULL,
  `Fk_Usuario_Responsable` INT NOT NULL,
  `fechaConfirmacionCierreMovimientoCxCEncabezado` DATETIME NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CierreMovimientoCxCEncabezado` PRIMARY KEY (`Pk_CierreMovimientoCxCEncabezado`),
  CONSTRAINT `uqCierreMovimientoCxCEncabezadonumero` UNIQUE (`numeroCierreMovimientoCxCEncabezado`),
  CONSTRAINT `Fk_CierreMovimientoCxCEncabezado_Usuario_Responsable` FOREIGN KEY (`Fk_Usuario_Responsable`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_CierreMovimientoCxCEncabezado_Moneda` FOREIGN KEY (`Fk_Moneda_Codigo`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxCierreMovimientoCxCEncabezadoMonedaCodigofechaDesdefechaHasta` (`Fk_Moneda_Codigo`, `fechaDesdeCierreMovimientoCxCEncabezado`, `fechaHastaCierreMovimientoCxCEncabezado`),
  KEY `idxCierreMovimientoCxCEncabezadoUsuarioResponsable` (`Fk_Usuario_Responsable`),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadodocumentosRevisadosCierra3aaae2a` CHECK (documentosRevisadosCierreMovimientoCxCEncabezado IN (0,1)),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadopagosConciliadosCierreMob8223bf3` CHECK (pagosConciliadosCierreMovimientoCxCEncabezado IN (0,1)),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadoValoresTipoCierre` CHECK (tipoCierreMovimientoCxCEncabezado IN ('MENSUAL','ANUAL')),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadoValoresEstadoCierre` CHECK (estadoCierreMovimientoCxCEncabezado IN ('BORRADOR','CONFIRMADO','ANULADO')),
  CONSTRAINT `ckCierreMovimientoCxCEncabezadoFechasCoherentes` CHECK (fechaHastaCierreMovimientoCxCEncabezado >= fechaDesdeCierreMovimientoCxCEncabezado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblReglaComision` (
  `Pk_ReglaComision` INT NOT NULL AUTO_INCREMENT,
  `codigoReglaComision` VARCHAR(30) NOT NULL,
  `nombreReglaComision` VARCHAR(100) NOT NULL,
  `baseCalculoReglaComision` VARCHAR(30) NOT NULL,
  `tasaComisionReglaComision` DECIMAL(3,2) NOT NULL,
  `vigenteDesdeReglaComision` DATE NOT NULL,
  `vigenteHastaReglaComision` DATE NULL,
  `requiereAutorizacionReglaComision` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ReglaComision` PRIMARY KEY (`Pk_ReglaComision`),
  CONSTRAINT `uqReglaComisioncodigo` UNIQUE (`codigoReglaComision`),
  CONSTRAINT `ckReglaComisiontasaComisionReglaComisionRango` CHECK (tasaComisionReglaComision BETWEEN 0 AND 0.99),
  CONSTRAINT `ckReglaComisionrequiereAutorizacionReglaComisionBooleano` CHECK (requiereAutorizacionReglaComision IN (0,1)),
  CONSTRAINT `ckReglaComisionValoresBaseCalculo` CHECK (baseCalculoReglaComision IN ('VENTA_NETA','COBRO_NETO')),
  CONSTRAINT `ckReglaComisionFechasCoherentes` CHECK (vigenteHastaReglaComision >= vigenteDesdeReglaComision)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblControlPeriodoCxC` (
  `Fk_Moneda_Codigo` BIGINT NOT NULL,
  `fechaUltimoCierreControlPeriodoCxC` DATE NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ControlPeriodoCxC` PRIMARY KEY (`Fk_Moneda_Codigo`),
  CONSTRAINT `Fk_ControlPeriodoCxC_Moneda` FOREIGN KEY (`Fk_Moneda_Codigo`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblCorrelativoVenta` (
  `prefijoCorrelativoVenta` VARCHAR(30) NOT NULL,
  `ultimoNumeroCorrelativoVenta` BIGINT NOT NULL DEFAULT 0,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CorrelativoVenta` PRIMARY KEY (`prefijoCorrelativoVenta`),
  CONSTRAINT `ckCorrelativoVentaNumeroNoNegativo` CHECK (ultimoNumeroCorrelativoVenta >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblDepositoVentaEncabezado` (
  `Pk_DepositoVentaEncabezado` INT NOT NULL AUTO_INCREMENT,
  `codigoDepositoVentaEncabezado` VARCHAR(30) NOT NULL,
  `fechaDepositoVentaEncabezado` DATE NOT NULL,
  `referenciaDepositoVentaEncabezado` VARCHAR(100) NULL,
  `Fk_Moneda_Codigo` BIGINT NOT NULL,
  `totalDepositoVentaEncabezado` DECIMAL(12,4) NOT NULL,
  `estadoDepositoVentaEncabezado` VARCHAR(25) NOT NULL DEFAULT 'PREPARADO',
  `Fk_Usuario_Registro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Fk_CuentaBancaria_Codigo` BIGINT NOT NULL,
  `Fk_TipoTransaccion_Codigo` BIGINT NOT NULL,
  `Fk_MovimientoBancario_Codigo` BIGINT NULL,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_DepositoVentaEncabezado` PRIMARY KEY (`Pk_DepositoVentaEncabezado`),
  CONSTRAINT `uqDepositoVentaEncabezadocodigo` UNIQUE (`codigoDepositoVentaEncabezado`),
  CONSTRAINT `uqDepositoVentaEncabezadoMovimientoBancarioCodigo` UNIQUE (`Fk_MovimientoBancario_Codigo`),
  CONSTRAINT `Fk_DepositoVentaEncabezado_Usuario_Registro` FOREIGN KEY (`Fk_Usuario_Registro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_DepositoVentaEncabezado_Moneda` FOREIGN KEY (`Fk_Moneda_Codigo`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_DepositoVentaEncabezado_CuentaBancaria` FOREIGN KEY (`Fk_CuentaBancaria_Codigo`)
    REFERENCES `tblCuentaBancaria` (`Pk_CuentaBancaria`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_DepositoVentaEncabezado_TipoTransaccion` FOREIGN KEY (`Fk_TipoTransaccion_Codigo`)
    REFERENCES `tblTipoTransaccion` (`Pk_TipoTransaccion`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_DepositoVentaEncabezado_MovimientoBancario` FOREIGN KEY (`Fk_MovimientoBancario_Codigo`)
    REFERENCES `tblMovimientoBancario` (`Pk_MovimientoBancario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxDepositoVentaEncabezadoUsuarioRegistro` (`Fk_Usuario_Registro`),
  KEY `idxDepositoVentaEncabezadoMonedaCodigo` (`Fk_Moneda_Codigo`),
  KEY `idxDepositoVentaEncabezadoCuentaBancariaCodigo` (`Fk_CuentaBancaria_Codigo`),
  KEY `idxDepositoVentaEncabezadoTipoTransaccionCodigo` (`Fk_TipoTransaccion_Codigo`),
  CONSTRAINT `ckDepositoVentaEncabezadoTotalPositivo` CHECK (totalDepositoVentaEncabezado > 0),
  CONSTRAINT `ckDepositoVentaEncabezadoValoresEstadoDeposito` CHECK (estadoDepositoVentaEncabezado IN ('PREPARADO','ENVIADO','CONFIRMADO','ANULADO'))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblCuentaContableCxC` (
  `Fk_Moneda_Codigo` BIGINT NOT NULL,
  `tipoCuentaContableCxC` VARCHAR(30) NOT NULL,
  `cuentaClientesCuentaContableCxC` VARCHAR(30) NOT NULL,
  `cuentaContrapartidaCuentaContableCxC` VARCHAR(30) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CuentaContableCxC` PRIMARY KEY (`Fk_Moneda_Codigo`, `tipoCuentaContableCxC`),
  CONSTRAINT `Fk_CuentaContableCxC_Moneda` FOREIGN KEY (`Fk_Moneda_Codigo`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblFormaPagoVenta` (
  `Pk_FormaPagoVenta` INT NOT NULL AUTO_INCREMENT,
  `codigoFormaPagoVenta` VARCHAR(30) NOT NULL,
  `nombreFormaPagoVenta` VARCHAR(100) NOT NULL,
  `requiereReferenciaFormaPagoVenta` BOOLEAN NOT NULL DEFAULT FALSE,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_FormaPagoVenta` PRIMARY KEY (`Pk_FormaPagoVenta`),
  CONSTRAINT `uqFormaPagoVentacodigo` UNIQUE (`codigoFormaPagoVenta`),
  CONSTRAINT `ckFormaPagoVentarequiereReferenciaFormaPagoVentaBooleano` CHECK (requiereReferenciaFormaPagoVenta IN (0,1))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblListaPrecioDetalle` (
  `Pk_ListaPrecioDetalle` INT NOT NULL AUTO_INCREMENT,
  `Fk_ListaPrecioEncabezado_Codigo` INT NOT NULL,
  `Fk_Producto_Codigo` INT NOT NULL,
  `precioUnitarioListaPrecioDetalle` DECIMAL(12,4) NOT NULL,
  `activoListaPrecioDetalle` BOOLEAN NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ListaPrecioDetalle` PRIMARY KEY (`Pk_ListaPrecioDetalle`),
  CONSTRAINT `uqListaPrecioDetalleListaPrecioEncabezadoCodigoProductoCodigo` UNIQUE (`Fk_ListaPrecioEncabezado_Codigo`, `Fk_Producto_Codigo`),
  CONSTRAINT `Fk_ListaPrecioDetalle_ListaPrecioEncabezado` FOREIGN KEY (`Fk_ListaPrecioEncabezado_Codigo`)
    REFERENCES `tblListaPrecioEncabezado` (`Pk_ListaPrecioEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ListaPrecioDetalle_Producto` FOREIGN KEY (`Fk_Producto_Codigo`)
    REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxListaPrecioDetalleProductoCodigo` (`Fk_Producto_Codigo`),
  CONSTRAINT `ckListaPrecioDetalleactivoListaPrecioDetalleBooleano` CHECK (activoListaPrecioDetalle IN (0,1)),
  CONSTRAINT `ckListaPrecioDetallePrecioNoNegativo` CHECK (precioUnitarioListaPrecioDetalle >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblCotizacionEncabezado` (
  `Pk_CotizacionEncabezado` INT NOT NULL AUTO_INCREMENT,
  `numeroCotizacionEncabezado` VARCHAR(30) NOT NULL,
  `Fk_Cliente_Codigo` INT NOT NULL,
  `Fk_Vendedor_Codigo` INT NOT NULL,
  `Fk_Moneda_Codigo` BIGINT NOT NULL,
  `fechaEmisionCotizacionEncabezado` DATE NOT NULL,
  `vigenteHastaCotizacionEncabezado` DATE NOT NULL,
  `estadoCotizacionEncabezado` VARCHAR(25) NOT NULL,
  `observacionesCotizacionEncabezado` TEXT NULL,
  `Fk_Usuario_Registro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Fk_ListaPrecioEncabezado_Codigo` INT NOT NULL,
  CONSTRAINT `Pk_CotizacionEncabezado` PRIMARY KEY (`Pk_CotizacionEncabezado`),
  CONSTRAINT `uqCotizacionEncabezadonumero` UNIQUE (`numeroCotizacionEncabezado`),
  CONSTRAINT `Fk_CotizacionEncabezado_Cliente` FOREIGN KEY (`Fk_Cliente_Codigo`)
    REFERENCES `tblCliente` (`Pk_Cliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_CotizacionEncabezado_Vendedor` FOREIGN KEY (`Fk_Vendedor_Codigo`)
    REFERENCES `tblVendedor` (`Pk_Vendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_CotizacionEncabezado_Usuario_Registro` FOREIGN KEY (`Fk_Usuario_Registro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_CotizacionEncabezado_Moneda` FOREIGN KEY (`Fk_Moneda_Codigo`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_CotizacionEncabezado_ListaPrecioEncabezado` FOREIGN KEY (`Fk_ListaPrecioEncabezado_Codigo`)
    REFERENCES `tblListaPrecioEncabezado` (`Pk_ListaPrecioEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxCotizacionEncabezadoClienteCodigo` (`Fk_Cliente_Codigo`),
  KEY `idxCotizacionEncabezadoVendedorCodigo` (`Fk_Vendedor_Codigo`),
  KEY `idxCotizacionEncabezadoMonedaCodigo` (`Fk_Moneda_Codigo`),
  KEY `idxCotizacionEncabezadoUsuarioRegistro` (`Fk_Usuario_Registro`),
  KEY `idxCotizacionEncabezadoListaPrecioEncabezadoCodigo` (`Fk_ListaPrecioEncabezado_Codigo`),
  CONSTRAINT `ckCotizacionEncabezadoValoresEstadoCotizacion` CHECK (estadoCotizacionEncabezado IN ('BORRADOR','EMITIDA','ACEPTADA','RECHAZADA','VENCIDA','ANULADA')),
  CONSTRAINT `ckCotizacionEncabezadoFechasCoherentes` CHECK (vigenteHastaCotizacionEncabezado >= fechaEmisionCotizacionEncabezado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblReciboPago` (
  `Pk_ReciboPago` INT NOT NULL AUTO_INCREMENT,
  `numeroReciboPago` VARCHAR(30) NOT NULL,
  `Fk_Cliente_Codigo` INT NOT NULL,
  `Fk_Moneda_Codigo` BIGINT NOT NULL,
  `fechaReciboPago` DATE NOT NULL,
  `montoReciboPago` DECIMAL(12,4) NOT NULL,
  `referenciaReciboPago` VARCHAR(255) NULL,
  `estadoReciboPago` VARCHAR(20) NOT NULL,
  `observacionesReciboPago` TEXT NULL,
  `Fk_Usuario_Registro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Fk_FormaPagoVenta_Codigo` INT NOT NULL,
  CONSTRAINT `Pk_ReciboPago` PRIMARY KEY (`Pk_ReciboPago`),
  CONSTRAINT `uqReciboPagonumero` UNIQUE (`numeroReciboPago`),
  CONSTRAINT `Fk_ReciboPago_Cliente` FOREIGN KEY (`Fk_Cliente_Codigo`)
    REFERENCES `tblCliente` (`Pk_Cliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ReciboPago_Usuario_Registro` FOREIGN KEY (`Fk_Usuario_Registro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ReciboPago_Moneda` FOREIGN KEY (`Fk_Moneda_Codigo`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ReciboPago_FormaPagoVenta` FOREIGN KEY (`Fk_FormaPagoVenta_Codigo`)
    REFERENCES `tblFormaPagoVenta` (`Pk_FormaPagoVenta`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxReciboPagoClienteCodigofecha` (`Fk_Cliente_Codigo`, `fechaReciboPago`),
  KEY `idxReciboPagoMonedaCodigo` (`Fk_Moneda_Codigo`),
  KEY `idxReciboPagoUsuarioRegistro` (`Fk_Usuario_Registro`),
  KEY `idxReciboPagoFormaPagoVentaCodigo` (`Fk_FormaPagoVenta_Codigo`),
  CONSTRAINT `ckReciboPagoMontoPositivo` CHECK (montoReciboPago > 0),
  CONSTRAINT `ckReciboPagoValoresEstadoPago` CHECK (estadoReciboPago IN ('BORRADOR','CONFIRMADO','ANULADO'))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblComisionEncabezado` (
  `Pk_ComisionEncabezado` INT NOT NULL AUTO_INCREMENT,
  `numeroComisionEncabezado` VARCHAR(30) NOT NULL,
  `Fk_Vendedor_Codigo` INT NOT NULL,
  `Fk_ReglaComision_Codigo` INT NOT NULL,
  `Fk_Moneda_Codigo` BIGINT NOT NULL,
  `fechaDesdeComisionEncabezado` DATE NOT NULL,
  `fechaHastaComisionEncabezado` DATE NOT NULL,
  `estadoComisionEncabezado` VARCHAR(25) NOT NULL,
  `Fk_Usuario_Solicita` INT NULL,
  `fechaSolicitudComisionEncabezado` DATETIME NULL,
  `Fk_Usuario_Autoriza` INT NULL,
  `fechaAutorizacionComisionEncabezado` DATETIME NULL,
  `comentarioAutorizacionComisionEncabezado` TEXT NULL,
  `referenciaContableComisionEncabezado` VARCHAR(100) NULL,
  `fechaEnvioComisionEncabezado` DATETIME NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionEncabezado` PRIMARY KEY (`Pk_ComisionEncabezado`),
  CONSTRAINT `uqComisionEncabezadonumero` UNIQUE (`numeroComisionEncabezado`),
  CONSTRAINT `Fk_ComisionEncabezado_Vendedor` FOREIGN KEY (`Fk_Vendedor_Codigo`)
    REFERENCES `tblVendedor` (`Pk_Vendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ComisionEncabezado_ReglaComision` FOREIGN KEY (`Fk_ReglaComision_Codigo`)
    REFERENCES `tblReglaComision` (`Pk_ReglaComision`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ComisionEncabezado_Usuario_Solicita` FOREIGN KEY (`Fk_Usuario_Solicita`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ComisionEncabezado_Usuario_Autoriza` FOREIGN KEY (`Fk_Usuario_Autoriza`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ComisionEncabezado_Moneda` FOREIGN KEY (`Fk_Moneda_Codigo`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxComisionEncabezadoVendedorCodigofechaDesdefechaHasta` (`Fk_Vendedor_Codigo`, `fechaDesdeComisionEncabezado`, `fechaHastaComisionEncabezado`),
  KEY `idxComisionEncabezadoReglaComisionCodigo` (`Fk_ReglaComision_Codigo`),
  KEY `idxComisionEncabezadoMonedaCodigo` (`Fk_Moneda_Codigo`),
  KEY `idxComisionEncabezadoUsuarioSolicita` (`Fk_Usuario_Solicita`),
  KEY `idxComisionEncabezadoUsuarioAutoriza` (`Fk_Usuario_Autoriza`),
  CONSTRAINT `ckComisionEncabezadoValoresEstadoComision` CHECK (estadoComisionEncabezado IN ('BORRADOR','SOLICITADA','APROBADA','RECHAZADA','ENVIADA')),
  CONSTRAINT `ckComisionEncabezadoFechasCoherentes` CHECK (fechaHastaComisionEncabezado >= fechaDesdeComisionEncabezado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblClienteListaPrecio` (
  `Fk_Cliente_Codigo` INT NOT NULL,
  `Fk_ListaPrecioEncabezado_Codigo` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ClienteListaPrecio` PRIMARY KEY (`Fk_Cliente_Codigo`),
  CONSTRAINT `Fk_ClienteListaPrecio_Cliente` FOREIGN KEY (`Fk_Cliente_Codigo`)
    REFERENCES `tblCliente` (`Pk_Cliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ClienteListaPrecio_ListaPrecioEncabezado` FOREIGN KEY (`Fk_ListaPrecioEncabezado_Codigo`)
    REFERENCES `tblListaPrecioEncabezado` (`Pk_ListaPrecioEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxClienteListaPrecioListaPrecioEncabezadoCodigo` (`Fk_ListaPrecioEncabezado_Codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblCierreSaldoCliente` (
  `Fk_CierreMovimientoCxCEncabezado_Codigo` INT NOT NULL,
  `Fk_Cliente_Codigo` INT NOT NULL,
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
  CONSTRAINT `Pk_CierreSaldoCliente` PRIMARY KEY (`Fk_CierreMovimientoCxCEncabezado_Codigo`, `Fk_Cliente_Codigo`),
  CONSTRAINT `Fk_CierreSaldoCliente_CierreMovimientoCxCEncabezado` FOREIGN KEY (`Fk_CierreMovimientoCxCEncabezado_Codigo`)
    REFERENCES `tblCierreMovimientoCxCEncabezado` (`Pk_CierreMovimientoCxCEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_CierreSaldoCliente_Cliente` FOREIGN KEY (`Fk_Cliente_Codigo`)
    REFERENCES `tblCliente` (`Pk_Cliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxCierreSaldoClienteClienteCodigo` (`Fk_Cliente_Codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblTrasladoCxCEncabezado` (
  `Fk_CierreMovimientoCxCEncabezado_Codigo` INT NOT NULL,
  `claveIntegracionTrasladoCxCEncabezado` VARCHAR(80) NOT NULL,
  `estadoTrasladoCxCEncabezado` VARCHAR(20) NOT NULL DEFAULT 'PENDIENTE',
  `referenciaContableTrasladoCxCEncabezado` VARCHAR(100) NULL,
  `Fk_Usuario_Codigo` INT NOT NULL,
  `fechaConfirmacionTrasladoCxCEncabezado` DATETIME NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_TrasladoCxCEncabezado` PRIMARY KEY (`Fk_CierreMovimientoCxCEncabezado_Codigo`),
  CONSTRAINT `uqTrasladoCxCEncabezadoclaveIntegracion` UNIQUE (`claveIntegracionTrasladoCxCEncabezado`),
  CONSTRAINT `Fk_TrasladoCxCEncabezado_CierreMovimientoCxCEncabezado` FOREIGN KEY (`Fk_CierreMovimientoCxCEncabezado_Codigo`)
    REFERENCES `tblCierreMovimientoCxCEncabezado` (`Pk_CierreMovimientoCxCEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_TrasladoCxCEncabezado_Usuario` FOREIGN KEY (`Fk_Usuario_Codigo`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxTrasladoCxCEncabezadoUsuarioCodigo` (`Fk_Usuario_Codigo`),
  CONSTRAINT `ckTrasladoCxCEncabezadoValoresEstado` CHECK (estadoTrasladoCxCEncabezado IN ('PENDIENTE','PREPARADO','ENVIADO','CONFIRMADO','RECHAZADO'))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblClienteFacturaVersion` (
  `Pk_ClienteFacturaVersion` INT NOT NULL AUTO_INCREMENT,
  `Fk_Cliente_Codigo` INT NOT NULL,
  `numeroVersionClienteFacturaVersion` INT NOT NULL,
  `tipoIdentificacionClienteFacturaVersion` ENUM('NIT','CUI','EXTRANJERO','CF') NOT NULL,
  `nitClienteFacturaVersion` CHAR(10) NULL,
  `dpiClienteFacturaVersion` CHAR(13) NULL,
  `identificacionExtranjeraClienteFacturaVersion` VARCHAR(30) NULL,
  `nombreClienteFacturaVersion` VARCHAR(100) NOT NULL,
  `direccionClienteFacturaVersion` VARCHAR(500) NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ClienteFacturaVersion` PRIMARY KEY (`Pk_ClienteFacturaVersion`),
  CONSTRAINT `uqClienteFacturaVersionClienteCodigonumeroVersion` UNIQUE (`Fk_Cliente_Codigo`, `numeroVersionClienteFacturaVersion`),
  CONSTRAINT `Fk_ClienteFacturaVersion_Cliente` FOREIGN KEY (`Fk_Cliente_Codigo`)
    REFERENCES `tblCliente` (`Pk_Cliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `ckClienteFacturaVersionIdentificacionCoherente` CHECK ((tipoIdentificacionClienteFacturaVersion = 'CF' AND nitClienteFacturaVersion IS NULL AND dpiClienteFacturaVersion IS NULL AND identificacionExtranjeraClienteFacturaVersion IS NULL) OR (tipoIdentificacionClienteFacturaVersion = 'NIT' AND nitClienteFacturaVersion IS NOT NULL AND dpiClienteFacturaVersion IS NULL AND identificacionExtranjeraClienteFacturaVersion IS NULL) OR (tipoIdentificacionClienteFacturaVersion = 'CUI' AND dpiClienteFacturaVersion IS NOT NULL AND nitClienteFacturaVersion IS NULL AND identificacionExtranjeraClienteFacturaVersion IS NULL) OR (tipoIdentificacionClienteFacturaVersion = 'EXTRANJERO' AND identificacionExtranjeraClienteFacturaVersion IS NOT NULL AND nitClienteFacturaVersion IS NULL AND dpiClienteFacturaVersion IS NULL)),
  CONSTRAINT `ckClienteFacturaVersionVersionPositiva` CHECK (numeroVersionClienteFacturaVersion > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblCotizacionDetalle` (
  `Pk_CotizacionDetalle` INT NOT NULL AUTO_INCREMENT,
  `Fk_CotizacionEncabezado_Codigo` INT NOT NULL,
  `numeroLineaCotizacionDetalle` INT NOT NULL,
  `Fk_Producto_Codigo` INT NOT NULL,
  `descripcionItemCotizacionDetalle` VARCHAR(255) NOT NULL,
  `cantidadCotizacionDetalle` DECIMAL(12,4) NOT NULL,
  `precioUnitarioCotizacionDetalle` DECIMAL(12,4) NOT NULL,
  `descuentoMontoCotizacionDetalle` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `tasaImpuestoCotizacionDetalle` DECIMAL(3,2) NOT NULL DEFAULT 0,
  `impuestoMontoCotizacionDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND((cantidadCotizacionDetalle * precioUnitarioCotizacionDetalle - descuentoMontoCotizacionDetalle) * tasaImpuestoCotizacionDetalle, 4)) STORED,
  `totalLineaCotizacionDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(cantidadCotizacionDetalle * precioUnitarioCotizacionDetalle - descuentoMontoCotizacionDetalle + impuestoMontoCotizacionDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CotizacionDetalle` PRIMARY KEY (`Pk_CotizacionDetalle`),
  CONSTRAINT `uqCotizacionDetalleCotizacionEncabezadoCodigonumeroLinea` UNIQUE (`Fk_CotizacionEncabezado_Codigo`, `numeroLineaCotizacionDetalle`),
  CONSTRAINT `Fk_CotizacionDetalle_CotizacionEncabezado` FOREIGN KEY (`Fk_CotizacionEncabezado_Codigo`)
    REFERENCES `tblCotizacionEncabezado` (`Pk_CotizacionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_CotizacionDetalle_Producto` FOREIGN KEY (`Fk_Producto_Codigo`)
    REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxCotizacionDetalleProductoCodigo` (`Fk_Producto_Codigo`),
  CONSTRAINT `ckCotizacionDetalletasaImpuestoCotizacionDetalleRango` CHECK (tasaImpuestoCotizacionDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckCotizacionDetalleCantidadPrecioDescuentoValidos` CHECK (cantidadCotizacionDetalle > 0 AND precioUnitarioCotizacionDetalle >= 0 AND descuentoMontoCotizacionDetalle >= 0 AND descuentoMontoCotizacionDetalle <= cantidadCotizacionDetalle * precioUnitarioCotizacionDetalle),
  CONSTRAINT `ckCotizacionDetalleNumeroLineaPositivo` CHECK (numeroLineaCotizacionDetalle > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblPedidoEncabezado` (
  `Pk_PedidoEncabezado` INT NOT NULL AUTO_INCREMENT,
  `numeroPedidoEncabezado` VARCHAR(30) NOT NULL,
  `Fk_CotizacionEncabezado_Codigo` INT NULL,
  `referenciaIndependientePedidoEncabezado` VARCHAR(100) NULL,
  `Fk_Cliente_Codigo` INT NOT NULL,
  `Fk_Vendedor_Codigo` INT NOT NULL,
  `Fk_Moneda_Codigo` BIGINT NOT NULL,
  `fechaPedidoEncabezado` DATE NOT NULL,
  `fechaEntregaPedidoEncabezado` DATE NULL,
  `fechaVencimientoPedidoEncabezado` DATE NULL,
  `estadoPedidoEncabezado` VARCHAR(25) NOT NULL DEFAULT 'PENDIENTE',
  `observacionesPedidoEncabezado` TEXT NULL,
  `Fk_Usuario_Registro` INT NULL DEFAULT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_PedidoEncabezado` PRIMARY KEY (`Pk_PedidoEncabezado`),
  CONSTRAINT `uqPedidoEncabezadonumero` UNIQUE (`numeroPedidoEncabezado`),
  CONSTRAINT `uqPedidoEncabezadoCotizacionEncabezadoCodigo` UNIQUE (`Fk_CotizacionEncabezado_Codigo`),
  CONSTRAINT `Fk_PedidoEncabezado_CotizacionEncabezado` FOREIGN KEY (`Fk_CotizacionEncabezado_Codigo`)
    REFERENCES `tblCotizacionEncabezado` (`Pk_CotizacionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_PedidoEncabezado_Cliente` FOREIGN KEY (`Fk_Cliente_Codigo`)
    REFERENCES `tblCliente` (`Pk_Cliente`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_PedidoEncabezado_Vendedor` FOREIGN KEY (`Fk_Vendedor_Codigo`)
    REFERENCES `tblVendedor` (`Pk_Vendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_PedidoEncabezado_Usuario_Registro` FOREIGN KEY (`Fk_Usuario_Registro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_PedidoEncabezado_Moneda` FOREIGN KEY (`Fk_Moneda_Codigo`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxPedidoEncabezadoClienteCodigo` (`Fk_Cliente_Codigo`),
  KEY `idxPedidoEncabezadoVendedorCodigo` (`Fk_Vendedor_Codigo`),
  KEY `idxPedidoEncabezadoMonedaCodigo` (`Fk_Moneda_Codigo`),
  KEY `idxPedidoEncabezadoUsuarioRegistro` (`Fk_Usuario_Registro`),
  CONSTRAINT `ckPedidoEncabezadoValoresEstadoPedido` CHECK (estadoPedidoEncabezado IN ('PENDIENTE','CONFIRMADO','PARCIAL','COMPLETADO','ANULADO')),
  CONSTRAINT `ckPedidoEncabezadoFechasCoherentes` CHECK (fechaVencimientoPedidoEncabezado >= fechaPedidoEncabezado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblDepositoVentaDetalle` (
  `Fk_DepositoVentaEncabezado_Codigo` INT NOT NULL,
  `Fk_ReciboPago_Codigo` INT NOT NULL,
  `importeDepositoVentaDetalle` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_DepositoVentaDetalle` PRIMARY KEY (`Fk_DepositoVentaEncabezado_Codigo`, `Fk_ReciboPago_Codigo`),
  CONSTRAINT `Fk_DepositoVentaDetalle_DepositoVentaEncabezado` FOREIGN KEY (`Fk_DepositoVentaEncabezado_Codigo`)
    REFERENCES `tblDepositoVentaEncabezado` (`Pk_DepositoVentaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_DepositoVentaDetalle_ReciboPago` FOREIGN KEY (`Fk_ReciboPago_Codigo`)
    REFERENCES `tblReciboPago` (`Pk_ReciboPago`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxDepositoVentaDetalleReciboPagoCodigo` (`Fk_ReciboPago_Codigo`),
  CONSTRAINT `ckDepositoVentaDetalleImportePositivo` CHECK (importeDepositoVentaDetalle > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblTrasladoCxCDetalle` (
  `Fk_TrasladoCxCEncabezado_Codigo` INT NOT NULL,
  `numeroLineaTrasladoCxCDetalle` INT NOT NULL,
  `cuentaTrasladoCxCDetalle` VARCHAR(30) NOT NULL,
  `descripcionTrasladoCxCDetalle` VARCHAR(100) NOT NULL,
  `debeTrasladoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `haberTrasladoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_TrasladoCxCDetalle` PRIMARY KEY (`Fk_TrasladoCxCEncabezado_Codigo`, `numeroLineaTrasladoCxCDetalle`),
  CONSTRAINT `Fk_TrasladoCxCDetalle_TrasladoCxCEncabezado` FOREIGN KEY (`Fk_TrasladoCxCEncabezado_Codigo`)
    REFERENCES `tblTrasladoCxCEncabezado` (`Fk_CierreMovimientoCxCEncabezado_Codigo`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `ckTrasladoCxCDetalleDebeHaberNoNegativos` CHECK (debeTrasladoCxCDetalle >= 0 AND haberTrasladoCxCDetalle >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblPedidoDetalle` (
  `Pk_PedidoDetalle` INT NOT NULL AUTO_INCREMENT,
  `Fk_PedidoEncabezado_Codigo` INT NOT NULL,
  `Fk_CotizacionDetalle_Codigo` INT NULL,
  `numeroLineaPedidoDetalle` INT NOT NULL,
  `Fk_Producto_Codigo` INT NOT NULL,
  `descripcionItemPedidoDetalle` VARCHAR(255) NOT NULL,
  `cantidadPedidoDetalle` DECIMAL(12,4) NOT NULL,
  `precioUnitarioPedidoDetalle` DECIMAL(12,4) NOT NULL,
  `descuentoMontoPedidoDetalle` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `tasaImpuestoPedidoDetalle` DECIMAL(3,2) NOT NULL DEFAULT 0,
  `impuestoMontoPedidoDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND((cantidadPedidoDetalle * precioUnitarioPedidoDetalle - descuentoMontoPedidoDetalle) * tasaImpuestoPedidoDetalle, 4)) STORED,
  `totalLineaPedidoDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(cantidadPedidoDetalle * precioUnitarioPedidoDetalle - descuentoMontoPedidoDetalle + impuestoMontoPedidoDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_PedidoDetalle` PRIMARY KEY (`Pk_PedidoDetalle`),
  CONSTRAINT `uqPedidoDetallePedidoEncabezadoCodigonumeroLinea` UNIQUE (`Fk_PedidoEncabezado_Codigo`, `numeroLineaPedidoDetalle`),
  CONSTRAINT `Fk_PedidoDetalle_PedidoEncabezado` FOREIGN KEY (`Fk_PedidoEncabezado_Codigo`)
    REFERENCES `tblPedidoEncabezado` (`Pk_PedidoEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_PedidoDetalle_CotizacionDetalle` FOREIGN KEY (`Fk_CotizacionDetalle_Codigo`)
    REFERENCES `tblCotizacionDetalle` (`Pk_CotizacionDetalle`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_PedidoDetalle_Producto` FOREIGN KEY (`Fk_Producto_Codigo`)
    REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxPedidoDetalleCotizacionDetalleCodigo` (`Fk_CotizacionDetalle_Codigo`),
  KEY `idxPedidoDetalleProductoCodigo` (`Fk_Producto_Codigo`),
  CONSTRAINT `ckPedidoDetalletasaImpuestoPedidoDetalleRango` CHECK (tasaImpuestoPedidoDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckPedidoDetalleCantidadPrecioDescuentoValidos` CHECK (cantidadPedidoDetalle > 0 AND precioUnitarioPedidoDetalle >= 0 AND descuentoMontoPedidoDetalle >= 0 AND descuentoMontoPedidoDetalle <= cantidadPedidoDetalle * precioUnitarioPedidoDetalle),
  CONSTRAINT `ckPedidoDetalleNumeroLineaPositivo` CHECK (numeroLineaPedidoDetalle > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblFacturaEncabezado` (
  `Pk_FacturaEncabezado` INT NOT NULL AUTO_INCREMENT,
  `serieFacturaEncabezado` VARCHAR(30) NOT NULL,
  `numeroFacturaEncabezado` VARCHAR(30) NOT NULL,
  `Fk_Sucursal_Codigo` INT NOT NULL,
  `Fk_PedidoEncabezado_Codigo` INT NULL,
  `Fk_Vendedor_Codigo` INT NULL,
  `Fk_Moneda_Codigo` BIGINT NOT NULL,
  `fechaEmisionFacturaEncabezado` DATE NOT NULL,
  `fechaVencimientoFacturaEncabezado` DATE NOT NULL,
  `tipoDteFacturaEncabezado` VARCHAR(30) NOT NULL,
  `condicionPagoFacturaEncabezado` VARCHAR(15) NOT NULL,
  `estadoFacturaEncabezado` VARCHAR(25) NOT NULL,
  `uuidDteFacturaEncabezado` CHAR(36) NULL,
  `fechaCertificacionFacturaEncabezado` DATETIME NULL,
  `observacionesFacturaEncabezado` TEXT NULL,
  `Fk_Usuario_Registro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Fk_ClienteFacturaVersion_Codigo` INT NOT NULL,
  `Fk_CotizacionEncabezado_Codigo` INT NULL,
  CONSTRAINT `Pk_FacturaEncabezado` PRIMARY KEY (`Pk_FacturaEncabezado`),
  CONSTRAINT `uqFacturaEncabezadouuidDte` UNIQUE (`uuidDteFacturaEncabezado`),
  CONSTRAINT `uqFacturaEncabezadoSucursalCodigoserienumero` UNIQUE (`Fk_Sucursal_Codigo`, `serieFacturaEncabezado`, `numeroFacturaEncabezado`),
  CONSTRAINT `uqFacturaEncabezadoCotizacionEncabezadoCodigo` UNIQUE (`Fk_CotizacionEncabezado_Codigo`),
  CONSTRAINT `Fk_FacturaEncabezado_Sucursal` FOREIGN KEY (`Fk_Sucursal_Codigo`)
    REFERENCES `tblSucursal` (`Pk_Sucursal`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_FacturaEncabezado_PedidoEncabezado` FOREIGN KEY (`Fk_PedidoEncabezado_Codigo`)
    REFERENCES `tblPedidoEncabezado` (`Pk_PedidoEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_FacturaEncabezado_Vendedor` FOREIGN KEY (`Fk_Vendedor_Codigo`)
    REFERENCES `tblVendedor` (`Pk_Vendedor`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_FacturaEncabezado_Usuario_Registro` FOREIGN KEY (`Fk_Usuario_Registro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_FacturaEncabezado_Moneda` FOREIGN KEY (`Fk_Moneda_Codigo`)
    REFERENCES `tblMoneda` (`Pk_Moneda`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_FacturaEncabezado_ClienteFacturaVersion` FOREIGN KEY (`Fk_ClienteFacturaVersion_Codigo`)
    REFERENCES `tblClienteFacturaVersion` (`Pk_ClienteFacturaVersion`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_FacturaEncabezado_CotizacionEncabezado` FOREIGN KEY (`Fk_CotizacionEncabezado_Codigo`)
    REFERENCES `tblCotizacionEncabezado` (`Pk_CotizacionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxFacturaEncabezadoPedidoEncabezadoCodigo` (`Fk_PedidoEncabezado_Codigo`),
  KEY `idxFacturaEncabezadoVendedorCodigo` (`Fk_Vendedor_Codigo`),
  KEY `idxFacturaEncabezadoMonedaCodigo` (`Fk_Moneda_Codigo`),
  KEY `idxFacturaEncabezadoUsuarioRegistro` (`Fk_Usuario_Registro`),
  KEY `idxFacturaEncabezadoClienteFacturaVersionCodigo` (`Fk_ClienteFacturaVersion_Codigo`),
  CONSTRAINT `ckFacturaEncabezadoCondicionPagoValida` CHECK (condicionPagoFacturaEncabezado IN ('CONTADO','CREDITO')),
  CONSTRAINT `ckFacturaEncabezadoValoresEstadoFactura` CHECK (estadoFacturaEncabezado IN ('BORRADOR','EMITIDA','ANULADA')),
  CONSTRAINT `ckFacturaEncabezadoFechasCoherentes` CHECK (fechaVencimientoFacturaEncabezado >= fechaEmisionFacturaEncabezado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblFacturaDetalle` (
  `Pk_FacturaDetalle` INT NOT NULL AUTO_INCREMENT,
  `Fk_FacturaEncabezado_Codigo` INT NOT NULL,
  `Fk_PedidoDetalle_Codigo` INT NULL,
  `numeroLineaFacturaDetalle` INT NOT NULL,
  `Fk_Producto_Codigo` INT NOT NULL,
  `descripcionItemFacturaDetalle` VARCHAR(255) NOT NULL,
  `cantidadFacturaDetalle` DECIMAL(12,4) NOT NULL,
  `precioUnitarioFacturaDetalle` DECIMAL(12,4) NOT NULL,
  `descuentoMontoFacturaDetalle` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `tasaImpuestoFacturaDetalle` DECIMAL(3,2) NOT NULL DEFAULT 0,
  `impuestoMontoFacturaDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND((cantidadFacturaDetalle * precioUnitarioFacturaDetalle - descuentoMontoFacturaDetalle) * tasaImpuestoFacturaDetalle, 4)) STORED,
  `totalLineaFacturaDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(cantidadFacturaDetalle * precioUnitarioFacturaDetalle - descuentoMontoFacturaDetalle + impuestoMontoFacturaDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_FacturaDetalle` PRIMARY KEY (`Pk_FacturaDetalle`),
  CONSTRAINT `uqFacturaDetalleFacturaEncabezadoCodigonumeroLinea` UNIQUE (`Fk_FacturaEncabezado_Codigo`, `numeroLineaFacturaDetalle`),
  CONSTRAINT `Fk_FacturaDetalle_FacturaEncabezado` FOREIGN KEY (`Fk_FacturaEncabezado_Codigo`)
    REFERENCES `tblFacturaEncabezado` (`Pk_FacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_FacturaDetalle_PedidoDetalle` FOREIGN KEY (`Fk_PedidoDetalle_Codigo`)
    REFERENCES `tblPedidoDetalle` (`Pk_PedidoDetalle`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_FacturaDetalle_Producto` FOREIGN KEY (`Fk_Producto_Codigo`)
    REFERENCES `tblProducto` (`Pk_Producto`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxFacturaDetallePedidoDetalleCodigo` (`Fk_PedidoDetalle_Codigo`),
  KEY `idxFacturaDetalleProductoCodigo` (`Fk_Producto_Codigo`),
  CONSTRAINT `ckFacturaDetalletasaImpuestoFacturaDetalleRango` CHECK (tasaImpuestoFacturaDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckFacturaDetalleCantidadPrecioDescuentoValidos` CHECK (cantidadFacturaDetalle > 0 AND precioUnitarioFacturaDetalle >= 0 AND descuentoMontoFacturaDetalle >= 0 AND descuentoMontoFacturaDetalle <= cantidadFacturaDetalle * precioUnitarioFacturaDetalle),
  CONSTRAINT `ckFacturaDetalleNumeroLineaPositivo` CHECK (numeroLineaFacturaDetalle > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblNotaCxC` (
  `Pk_NotaCxC` INT NOT NULL AUTO_INCREMENT,
  `numeroNotaCxC` VARCHAR(30) NOT NULL,
  `Fk_FacturaEncabezado_Codigo` INT NOT NULL,
  `tipoNotaCxC` VARCHAR(10) NOT NULL,
  `fechaNotaCxC` DATE NOT NULL,
  `montoNotaCxC` DECIMAL(12,4) NOT NULL,
  `motivoNotaCxC` TEXT NOT NULL,
  `estadoNotaCxC` VARCHAR(20) NOT NULL,
  `Fk_Usuario_Registro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_NotaCxC` PRIMARY KEY (`Pk_NotaCxC`),
  CONSTRAINT `uqNotaCxCnumero` UNIQUE (`numeroNotaCxC`),
  CONSTRAINT `Fk_NotaCxC_FacturaEncabezado` FOREIGN KEY (`Fk_FacturaEncabezado_Codigo`)
    REFERENCES `tblFacturaEncabezado` (`Pk_FacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_NotaCxC_Usuario_Registro` FOREIGN KEY (`Fk_Usuario_Registro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxNotaCxCFacturaEncabezadoCodigo` (`Fk_FacturaEncabezado_Codigo`),
  KEY `idxNotaCxCUsuarioRegistro` (`Fk_Usuario_Registro`),
  CONSTRAINT `ckNotaCxCMontoPositivo` CHECK (montoNotaCxC > 0),
  CONSTRAINT `ckNotaCxCValoresTipoNota` CHECK (tipoNotaCxC IN ('CREDITO','DEBITO')),
  CONSTRAINT `ckNotaCxCValoresEstadoNota` CHECK (estadoNotaCxC IN ('BORRADOR','APLICADA','ANULADA'))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblAplicacionPago` (
  `Pk_AplicacionPago` INT NOT NULL AUTO_INCREMENT,
  `Fk_ReciboPago_Codigo` INT NOT NULL,
  `Fk_FacturaEncabezado_Codigo` INT NOT NULL,
  `fechaAplicacionPago` DATE NOT NULL,
  `montoAplicadoAplicacionPago` DECIMAL(12,4) NOT NULL,
  `estadoAplicacionPago` VARCHAR(20) NOT NULL,
  `Fk_Usuario_Registro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_AplicacionPago` PRIMARY KEY (`Pk_AplicacionPago`),
  CONSTRAINT `Fk_AplicacionPago_ReciboPago` FOREIGN KEY (`Fk_ReciboPago_Codigo`)
    REFERENCES `tblReciboPago` (`Pk_ReciboPago`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_AplicacionPago_FacturaEncabezado` FOREIGN KEY (`Fk_FacturaEncabezado_Codigo`)
    REFERENCES `tblFacturaEncabezado` (`Pk_FacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_AplicacionPago_Usuario_Registro` FOREIGN KEY (`Fk_Usuario_Registro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxAplicacionPagoFacturaEncabezadoCodigofecha` (`Fk_FacturaEncabezado_Codigo`, `fechaAplicacionPago`),
  KEY `idxAplicacionPagoReciboPagoCodigo` (`Fk_ReciboPago_Codigo`),
  KEY `idxAplicacionPagoUsuarioRegistro` (`Fk_Usuario_Registro`),
  CONSTRAINT `ckAplicacionPagoMontoPositivo` CHECK (montoAplicadoAplicacionPago > 0),
  CONSTRAINT `ckAplicacionPagoValoresEstadoAplicacion` CHECK (estadoAplicacionPago IN ('APLICADA','REVERTIDA'))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblCierreMovimientoCxCDetalle` (
  `Pk_CierreMovimientoCxCDetalle` INT NOT NULL AUTO_INCREMENT,
  `Fk_CierreMovimientoCxCEncabezado_Codigo` INT NOT NULL,
  `Fk_FacturaEncabezado_Codigo` INT NOT NULL,
  `saldoInicialCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `cargosPeriodoCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `abonosPeriodoCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `saldoFinalCierreMovimientoCxCDetalle` DECIMAL(12,4) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_CierreMovimientoCxCDetalle` PRIMARY KEY (`Pk_CierreMovimientoCxCDetalle`),
  CONSTRAINT `uqCierreMovimientoCxCDetalleCierreMovimientoCxCEncabeza4f1c6502` UNIQUE (`Fk_CierreMovimientoCxCEncabezado_Codigo`, `Fk_FacturaEncabezado_Codigo`),
  CONSTRAINT `Fk_CierreMovimientoCxCDetalle_CierreMovimientoCxCEncabezado` FOREIGN KEY (`Fk_CierreMovimientoCxCEncabezado_Codigo`)
    REFERENCES `tblCierreMovimientoCxCEncabezado` (`Pk_CierreMovimientoCxCEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_CierreMovimientoCxCDetalle_FacturaEncabezado` FOREIGN KEY (`Fk_FacturaEncabezado_Codigo`)
    REFERENCES `tblFacturaEncabezado` (`Pk_FacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxCierreMovimientoCxCDetalleFacturaEncabezadoCodigo` (`Fk_FacturaEncabezado_Codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblComisionFacturaEncabezado` (
  `Pk_ComisionFacturaEncabezado` INT NOT NULL AUTO_INCREMENT,
  `Fk_FacturaEncabezado_Codigo` INT NOT NULL,
  `metodoComisionFacturaEncabezado` ENUM('LINEA','PRODUCTO','CATEGORIA','MARCA','MIXTA') NOT NULL,
  `estadoComisionFacturaEncabezado` ENUM('CALCULADA','ANULADA') NOT NULL DEFAULT 'CALCULADA',
  `observacionesComisionFacturaEncabezado` TEXT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionFacturaEncabezado` PRIMARY KEY (`Pk_ComisionFacturaEncabezado`),
  CONSTRAINT `uqComisionFacturaEncabezadoFacturaEncabezadoCodigo` UNIQUE (`Fk_FacturaEncabezado_Codigo`),
  CONSTRAINT `Fk_ComisionFacturaEncabezado_FacturaEncabezado` FOREIGN KEY (`Fk_FacturaEncabezado_Codigo`)
    REFERENCES `tblFacturaEncabezado` (`Pk_FacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblMovimientoCxC` (
  `Pk_MovimientoCxC` INT NOT NULL AUTO_INCREMENT,
  `Fk_FacturaEncabezado_Codigo` INT NOT NULL,
  `Fk_AplicacionPago_Codigo` INT NULL,
  `Fk_NotaCxC_Codigo` INT NULL,
  `Fk_MovimientoCxC_Revertido` INT NULL,
  `fechaMovimientoCxC` DATE NOT NULL,
  `tipoMovimientoCxC` VARCHAR(30) NOT NULL,
  `descripcionMovimientoCxC` VARCHAR(255) NOT NULL,
  `cargoMovimientoCxC` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `abonoMovimientoCxC` DECIMAL(12,4) NOT NULL DEFAULT 0,
  `Fk_Usuario_Registro` INT NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_MovimientoCxC` PRIMARY KEY (`Pk_MovimientoCxC`),
  CONSTRAINT `uqMovimientoCxCAplicacionPagoCodigo` UNIQUE (`Fk_AplicacionPago_Codigo`),
  CONSTRAINT `uqMovimientoCxCNotaCxCCodigo` UNIQUE (`Fk_NotaCxC_Codigo`),
  CONSTRAINT `uqMovimientoCxCRevertido` UNIQUE (`Fk_MovimientoCxC_Revertido`),
  CONSTRAINT `Fk_MovimientoCxC_FacturaEncabezado` FOREIGN KEY (`Fk_FacturaEncabezado_Codigo`)
    REFERENCES `tblFacturaEncabezado` (`Pk_FacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_MovimientoCxC_AplicacionPago` FOREIGN KEY (`Fk_AplicacionPago_Codigo`)
    REFERENCES `tblAplicacionPago` (`Pk_AplicacionPago`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_MovimientoCxC_NotaCxC` FOREIGN KEY (`Fk_NotaCxC_Codigo`)
    REFERENCES `tblNotaCxC` (`Pk_NotaCxC`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_MovimientoCxC_MovimientoCxC_Revertido` FOREIGN KEY (`Fk_MovimientoCxC_Revertido`)
    REFERENCES `tblMovimientoCxC` (`Pk_MovimientoCxC`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_MovimientoCxC_Usuario_Registro` FOREIGN KEY (`Fk_Usuario_Registro`)
    REFERENCES `tblUsuario` (`idUsuario`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxMovimientoCxCFacturaEncabezadoCodigofecha` (`Fk_FacturaEncabezado_Codigo`, `fechaMovimientoCxC`),
  KEY `idxMovimientoCxCfecha` (`fechaMovimientoCxC`),
  KEY `idxMovimientoCxCUsuarioRegistro` (`Fk_Usuario_Registro`),
  CONSTRAINT `ckMovimientoCxCCargoAbonoExcluyentes` CHECK (cargoMovimientoCxC >= 0 AND abonoMovimientoCxC >= 0 AND ((cargoMovimientoCxC > 0 AND abonoMovimientoCxC = 0) OR (abonoMovimientoCxC > 0 AND cargoMovimientoCxC = 0))),
  CONSTRAINT `ckMovimientoCxCReversionIdentificada` CHECK ((tipoMovimientoCxC = 'REVERSO' AND Fk_MovimientoCxC_Revertido IS NOT NULL) OR (tipoMovimientoCxC <> 'REVERSO' AND Fk_MovimientoCxC_Revertido IS NULL)),
  CONSTRAINT `ckMovimientoCxCValoresTipoMovimiento` CHECK (tipoMovimientoCxC IN ('FACTURA','PAGO','NOTA_CREDITO','NOTA_DEBITO','AJUSTE_POSITIVO','AJUSTE_NEGATIVO','REVERSO'))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblComisionDetalle` (
  `Pk_ComisionDetalle` INT NOT NULL AUTO_INCREMENT,
  `Fk_ComisionEncabezado_Codigo` INT NOT NULL,
  `Fk_FacturaEncabezado_Codigo` INT NOT NULL,
  `Fk_AplicacionPago_Codigo` INT NULL,
  `baseComisionDetalle` DECIMAL(12,4) NOT NULL,
  `tasaAplicadaComisionDetalle` DECIMAL(3,2) NOT NULL,
  `montoComisionDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(baseComisionDetalle * tasaAplicadaComisionDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionDetalle` PRIMARY KEY (`Pk_ComisionDetalle`),
  CONSTRAINT `Fk_ComisionDetalle_ComisionEncabezado` FOREIGN KEY (`Fk_ComisionEncabezado_Codigo`)
    REFERENCES `tblComisionEncabezado` (`Pk_ComisionEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ComisionDetalle_FacturaEncabezado` FOREIGN KEY (`Fk_FacturaEncabezado_Codigo`)
    REFERENCES `tblFacturaEncabezado` (`Pk_FacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ComisionDetalle_AplicacionPago` FOREIGN KEY (`Fk_AplicacionPago_Codigo`)
    REFERENCES `tblAplicacionPago` (`Pk_AplicacionPago`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxComisionDetalleComisionEncabezadoCodigo` (`Fk_ComisionEncabezado_Codigo`),
  KEY `idxComisionDetalleFacturaEncabezadoCodigo` (`Fk_FacturaEncabezado_Codigo`),
  KEY `idxComisionDetalleAplicacionPagoCodigo` (`Fk_AplicacionPago_Codigo`),
  CONSTRAINT `ckComisionDetalletasaAplicadaComisionDetalleRango` CHECK (tasaAplicadaComisionDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckComisionDetalleBaseNoNegativa` CHECK (baseComisionDetalle >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblComisionAbono` (
  `Pk_ComisionAbono` INT NOT NULL AUTO_INCREMENT,
  `Fk_ComisionFacturaEncabezado_Codigo` INT NOT NULL,
  `Fk_AplicacionPago_Codigo` INT NOT NULL,
  `tasaFacturaPagadaAcumuladaComisionAbono` DECIMAL(5,4) NOT NULL,
  `montoComisionDevengadoComisionAbono` DECIMAL(12,4) NOT NULL,
  `estadoPagoVendedorComisionAbono` ENUM('PENDIENTE','PAGADO','REVERSADO') NOT NULL DEFAULT 'PENDIENTE',
  `fechaPagoVendedorComisionAbono` DATE NULL,
  `referenciaNominaComisionAbono` VARCHAR(100) NULL,
  `observacionesComisionAbono` TEXT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionAbono` PRIMARY KEY (`Pk_ComisionAbono`),
  CONSTRAINT `uqComisionAbonoAplicacionPagoCodigo` UNIQUE (`Fk_AplicacionPago_Codigo`),
  CONSTRAINT `Fk_ComisionAbono_ComisionFacturaEncabezado` FOREIGN KEY (`Fk_ComisionFacturaEncabezado_Codigo`)
    REFERENCES `tblComisionFacturaEncabezado` (`Pk_ComisionFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ComisionAbono_AplicacionPago` FOREIGN KEY (`Fk_AplicacionPago_Codigo`)
    REFERENCES `tblAplicacionPago` (`Pk_AplicacionPago`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxComisionAbonoComisionFacturaEncabezadoCodigoestadoPa19c1f12f` (`Fk_ComisionFacturaEncabezado_Codigo`, `estadoPagoVendedorComisionAbono`),
  CONSTRAINT `ckComisionAbonotasaFacturaPagadaAcumuladaComisionAbonoRango` CHECK (tasaFacturaPagadaAcumuladaComisionAbono BETWEEN 0 AND 1),
  CONSTRAINT `ckComisionAbonoDevengoNoNegativo` CHECK (montoComisionDevengadoComisionAbono >= 0),
  CONSTRAINT `ckComisionAbonoFechaPagoVendedor` CHECK (estadoPagoVendedorComisionAbono <> 'PAGADO' OR fechaPagoVendedorComisionAbono IS NOT NULL)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `tblComisionFacturaDetalle` (
  `Pk_ComisionFacturaDetalle` INT NOT NULL AUTO_INCREMENT,
  `Fk_ComisionFacturaEncabezado_Codigo` INT NOT NULL,
  `Fk_FacturaDetalle_Codigo` INT NOT NULL,
  `metodoComisionFacturaDetalle` ENUM('LINEA','PRODUCTO','CATEGORIA','MARCA') NOT NULL,
  `nombreClasificacionHistoricaComisionFacturaDetalle` VARCHAR(100) NOT NULL,
  `tasaAplicadaComisionFacturaDetalle` DECIMAL(3,2) NOT NULL,
  `baseComisionFacturaDetalle` DECIMAL(12,4) NOT NULL,
  `montoComisionLineaComisionFacturaDetalle` DECIMAL(12,4) GENERATED ALWAYS AS (ROUND(baseComisionFacturaDetalle * tasaAplicadaComisionFacturaDetalle, 4)) STORED,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Fk_Linea_Codigo` INT NULL,
  `Fk_Categoria_Codigo` INT NULL,
  `Fk_Marca_Codigo` INT NULL,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT `Pk_ComisionFacturaDetalle` PRIMARY KEY (`Pk_ComisionFacturaDetalle`),
  CONSTRAINT `uqComisionFacturaDetalleFacturaDetalleCodigo` UNIQUE (`Fk_FacturaDetalle_Codigo`),
  CONSTRAINT `Fk_ComisionFacturaDetalle_ComisionFacturaEncabezado` FOREIGN KEY (`Fk_ComisionFacturaEncabezado_Codigo`)
    REFERENCES `tblComisionFacturaEncabezado` (`Pk_ComisionFacturaEncabezado`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ComisionFacturaDetalle_FacturaDetalle` FOREIGN KEY (`Fk_FacturaDetalle_Codigo`)
    REFERENCES `tblFacturaDetalle` (`Pk_FacturaDetalle`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ComisionFacturaDetalle_Linea` FOREIGN KEY (`Fk_Linea_Codigo`)
    REFERENCES `tblLinea` (`Pk_Linea`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ComisionFacturaDetalle_Categoria` FOREIGN KEY (`Fk_Categoria_Codigo`)
    REFERENCES `tblCategoria` (`Pk_Categoria`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `Fk_ComisionFacturaDetalle_Marca` FOREIGN KEY (`Fk_Marca_Codigo`)
    REFERENCES `tblMarca` (`Pk_Marca`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  KEY `idxComisionFacturaDetalleComisionFacturaEncabezadoCodigo` (`Fk_ComisionFacturaEncabezado_Codigo`),
  KEY `idxComisionFacturaDetalleLineaCodigo` (`Fk_Linea_Codigo`),
  KEY `idxComisionFacturaDetalleCategoriaCodigo` (`Fk_Categoria_Codigo`),
  KEY `idxComisionFacturaDetalleMarcaCodigo` (`Fk_Marca_Codigo`),
  CONSTRAINT `ckComisionFacturaDetalletasaAplicadaComisionFacturaDetalleRango` CHECK (tasaAplicadaComisionFacturaDetalle BETWEEN 0 AND 0.99),
  CONSTRAINT `ckComisionFacturaDetalleBaseNoNegativa` CHECK (baseComisionFacturaDetalle >= 0),
  CONSTRAINT `ckComisionFacturaDetalleClasificacionTipada` CHECK ((metodoComisionFacturaDetalle = 'PRODUCTO' AND Fk_Linea_Codigo IS NULL AND Fk_Categoria_Codigo IS NULL AND Fk_Marca_Codigo IS NULL) OR (metodoComisionFacturaDetalle = 'LINEA' AND Fk_Linea_Codigo IS NOT NULL AND Fk_Categoria_Codigo IS NULL AND Fk_Marca_Codigo IS NULL) OR (metodoComisionFacturaDetalle = 'CATEGORIA' AND Fk_Categoria_Codigo IS NOT NULL AND Fk_Linea_Codigo IS NULL AND Fk_Marca_Codigo IS NULL) OR (metodoComisionFacturaDetalle = 'MARCA' AND Fk_Marca_Codigo IS NOT NULL AND Fk_Linea_Codigo IS NULL AND Fk_Categoria_Codigo IS NULL))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

DELIMITER $$

CREATE TRIGGER `trClienteBeforeInsert` BEFORE INSERT ON `tblCliente` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Credito` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trClienteBeforeUpdate` BEFORE UPDATE ON `tblCliente` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Credito` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trListaPrecioEncabezadoBeforeInsert` BEFORE INSERT ON `tblListaPrecioEncabezado` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trListaPrecioEncabezadoBeforeUpdate` BEFORE UPDATE ON `tblListaPrecioEncabezado` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trCotizacionEncabezadoBeforeInsert` BEFORE INSERT ON `tblCotizacionEncabezado` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trCotizacionEncabezadoBeforeUpdate` BEFORE UPDATE ON `tblCotizacionEncabezado` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trPedidoEncabezadoBeforeInsert` BEFORE INSERT ON `tblPedidoEncabezado` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trPedidoEncabezadoBeforeUpdate` BEFORE UPDATE ON `tblPedidoEncabezado` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trFacturaEncabezadoBeforeInsert` BEFORE INSERT ON `tblFacturaEncabezado` FOR EACH ROW
BEGIN
  DECLARE fechaUltimoCierre DATE DEFAULT NULL;
  DECLARE fechaCierreAnterior DATE DEFAULT NULL;
  INSERT INTO `tblControlPeriodoCxC` (`Fk_Moneda_Codigo`)
    VALUES (NEW.`Fk_Moneda_Codigo`) ON DUPLICATE KEY UPDATE `Fk_Moneda_Codigo`=NEW.`Fk_Moneda_Codigo`;
  SELECT `fechaUltimoCierreControlPeriodoCxC` INTO fechaUltimoCierre
    FROM `tblControlPeriodoCxC` WHERE `Fk_Moneda_Codigo`=NEW.`Fk_Moneda_Codigo` FOR UPDATE;
  IF fechaUltimoCierre IS NOT NULL AND NEW.`fechaEmisionFacturaEncabezado`<=fechaUltimoCierre THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='La fecha pertenece a un periodo de CxC cerrado';
  END IF;
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trFacturaEncabezadoBeforeUpdate` BEFORE UPDATE ON `tblFacturaEncabezado` FOR EACH ROW
BEGIN
  DECLARE fechaUltimoCierre DATE DEFAULT NULL;
  DECLARE fechaCierreAnterior DATE DEFAULT NULL;
  SELECT `fechaUltimoCierreControlPeriodoCxC` INTO fechaCierreAnterior FROM `tblControlPeriodoCxC`
    WHERE `Fk_Moneda_Codigo`=OLD.`Fk_Moneda_Codigo` FOR UPDATE;
  IF fechaCierreAnterior IS NOT NULL AND OLD.`fechaEmisionFacturaEncabezado`<=fechaCierreAnterior THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='No se modifica una factura de un periodo cerrado';
  END IF;
  INSERT INTO `tblControlPeriodoCxC` (`Fk_Moneda_Codigo`)
    VALUES (NEW.`Fk_Moneda_Codigo`) ON DUPLICATE KEY UPDATE `Fk_Moneda_Codigo`=NEW.`Fk_Moneda_Codigo`;
  SELECT `fechaUltimoCierreControlPeriodoCxC` INTO fechaUltimoCierre
    FROM `tblControlPeriodoCxC` WHERE `Fk_Moneda_Codigo`=NEW.`Fk_Moneda_Codigo` FOR UPDATE;
  IF fechaUltimoCierre IS NOT NULL AND NEW.`fechaEmisionFacturaEncabezado`<=fechaUltimoCierre THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='La fecha pertenece a un periodo de CxC cerrado';
  END IF;
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trReciboPagoBeforeInsert` BEFORE INSERT ON `tblReciboPago` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trReciboPagoBeforeUpdate` BEFORE UPDATE ON `tblReciboPago` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trAplicacionPagoBeforeInsert` BEFORE INSERT ON `tblAplicacionPago` FOR EACH ROW
BEGIN
  DECLARE monedaRecibo BIGINT;
  DECLARE clienteRecibo INT;
  SELECT `Fk_Moneda_Codigo`,`Fk_Cliente_Codigo` INTO monedaRecibo,clienteRecibo
    FROM `tblReciboPago` WHERE `Pk_ReciboPago`=NEW.`Fk_ReciboPago_Codigo`;
  IF NOT EXISTS(SELECT 1 FROM `tblFacturaEncabezado` f JOIN `tblClienteFacturaVersion` v
      ON v.`Pk_ClienteFacturaVersion`=f.`Fk_ClienteFacturaVersion_Codigo`
    WHERE f.`Pk_FacturaEncabezado`=NEW.`Fk_FacturaEncabezado_Codigo` AND f.`Fk_Moneda_Codigo`=monedaRecibo
      AND v.`Fk_Cliente_Codigo`=clienteRecibo) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Recibo y factura deben corresponder al mismo cliente y moneda';
  END IF;
END$$

CREATE TRIGGER `trAplicacionPagoBeforeUpdate` BEFORE UPDATE ON `tblAplicacionPago` FOR EACH ROW
BEGIN
  DECLARE monedaRecibo BIGINT;
  DECLARE clienteRecibo INT;
  SELECT `Fk_Moneda_Codigo`,`Fk_Cliente_Codigo` INTO monedaRecibo,clienteRecibo
    FROM `tblReciboPago` WHERE `Pk_ReciboPago`=NEW.`Fk_ReciboPago_Codigo`;
  IF NOT EXISTS(SELECT 1 FROM `tblFacturaEncabezado` f JOIN `tblClienteFacturaVersion` v
      ON v.`Pk_ClienteFacturaVersion`=f.`Fk_ClienteFacturaVersion_Codigo`
    WHERE f.`Pk_FacturaEncabezado`=NEW.`Fk_FacturaEncabezado_Codigo` AND f.`Fk_Moneda_Codigo`=monedaRecibo
      AND v.`Fk_Cliente_Codigo`=clienteRecibo) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Recibo y factura deben corresponder al mismo cliente y moneda';
  END IF;
END$$

CREATE TRIGGER `trMovimientoCxCBeforeInsert` BEFORE INSERT ON `tblMovimientoCxC` FOR EACH ROW
BEGIN
  DECLARE monedaFactura BIGINT;
  DECLARE fechaUltimoCierre DATE DEFAULT NULL;
  DECLARE cantidadOriginal INT DEFAULT 0;
  SELECT `Fk_Moneda_Codigo` INTO monedaFactura FROM `tblFacturaEncabezado`
    WHERE `Pk_FacturaEncabezado`=NEW.`Fk_FacturaEncabezado_Codigo`;
  INSERT INTO `tblControlPeriodoCxC` (`Fk_Moneda_Codigo`) VALUES (monedaFactura)
    ON DUPLICATE KEY UPDATE `Fk_Moneda_Codigo`=monedaFactura;
  SELECT `fechaUltimoCierreControlPeriodoCxC` INTO fechaUltimoCierre FROM `tblControlPeriodoCxC`
    WHERE `Fk_Moneda_Codigo`=monedaFactura FOR UPDATE;
  IF fechaUltimoCierre IS NOT NULL AND NEW.`fechaMovimientoCxC`<=fechaUltimoCierre THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='No se registran movimientos en un periodo cerrado';
  END IF;
  IF NEW.`Fk_MovimientoCxC_Revertido` IS NOT NULL THEN
    SELECT COUNT(*) INTO cantidadOriginal FROM `tblMovimientoCxC`
    WHERE `Pk_MovimientoCxC`=NEW.`Fk_MovimientoCxC_Revertido`
      AND `Fk_FacturaEncabezado_Codigo`=NEW.`Fk_FacturaEncabezado_Codigo`
      AND `tipoMovimientoCxC`<>'REVERSO'
      AND `cargoMovimientoCxC`=NEW.`abonoMovimientoCxC` AND `abonoMovimientoCxC`=NEW.`cargoMovimientoCxC`;
    IF cantidadOriginal<>1 THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='La reversion debe invertir el cargo o abono original de la misma factura'; END IF;
  END IF;
END$$

CREATE TRIGGER `trMovimientoCxCBeforeUpdate` BEFORE UPDATE ON `tblMovimientoCxC` FOR EACH ROW
BEGIN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El libro CxC es inmutable; registre una reversion en periodo abierto';
END$$

CREATE TRIGGER `trCierreMovimientoCxCEncabezadoBeforeInsert` BEFORE INSERT ON `tblCierreMovimientoCxCEncabezado` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trCierreMovimientoCxCEncabezadoBeforeUpdate` BEFORE UPDATE ON `tblCierreMovimientoCxCEncabezado` FOR EACH ROW
BEGIN
  IF OLD.`estadoCierreMovimientoCxCEncabezado`='CONFIRMADO' THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El cierre confirmado no puede modificarse'; END IF;
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trCierreMovimientoCxCDetalleBeforeInsert` BEFORE INSERT ON `tblCierreMovimientoCxCDetalle` FOR EACH ROW
BEGIN
  IF (SELECT `estadoCierreMovimientoCxCEncabezado` FROM `tblCierreMovimientoCxCEncabezado`
      WHERE `Pk_CierreMovimientoCxCEncabezado`=NEW.`Fk_CierreMovimientoCxCEncabezado_Codigo`)='CONFIRMADO' THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='No se agregan detalles a un cierre confirmado';
  END IF;
END$$

CREATE TRIGGER `trCierreMovimientoCxCDetalleBeforeUpdate` BEFORE UPDATE ON `tblCierreMovimientoCxCDetalle` FOR EACH ROW
BEGIN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El detalle historico del cierre es inmutable';
END$$

CREATE TRIGGER `trComisionEncabezadoBeforeInsert` BEFORE INSERT ON `tblComisionEncabezado` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trComisionEncabezadoBeforeUpdate` BEFORE UPDATE ON `tblComisionEncabezado` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trControlPeriodoCxCBeforeInsert` BEFORE INSERT ON `tblControlPeriodoCxC` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trControlPeriodoCxCBeforeUpdate` BEFORE UPDATE ON `tblControlPeriodoCxC` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trCierreSaldoClienteBeforeInsert` BEFORE INSERT ON `tblCierreSaldoCliente` FOR EACH ROW
BEGIN
  IF (SELECT `estadoCierreMovimientoCxCEncabezado` FROM `tblCierreMovimientoCxCEncabezado`
      WHERE `Pk_CierreMovimientoCxCEncabezado`=NEW.`Fk_CierreMovimientoCxCEncabezado_Codigo`)='CONFIRMADO' THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='No se agregan detalles a un cierre confirmado';
  END IF;
END$$

CREATE TRIGGER `trCierreSaldoClienteBeforeUpdate` BEFORE UPDATE ON `tblCierreSaldoCliente` FOR EACH ROW
BEGIN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El detalle historico del cierre es inmutable';
END$$

CREATE TRIGGER `trDepositoVentaEncabezadoBeforeInsert` BEFORE INSERT ON `tblDepositoVentaEncabezado` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
  IF NOT EXISTS(SELECT 1 FROM tblCuentaBancaria WHERE Pk_CuentaBancaria=NEW.`Fk_CuentaBancaria_Codigo`
      AND Fk_Moneda_Codigo=NEW.`Fk_Moneda_Codigo`) THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='La cuenta bancaria debe usar la moneda del deposito'; END IF;
  IF NOT EXISTS(SELECT 1 FROM tblTipoTransaccion WHERE Pk_TipoTransaccion=NEW.`Fk_TipoTransaccion_Codigo`
      AND naturalezaTipoTransaccion IN ('INGRESO','AMBOS')) THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El deposito requiere un concepto bancario de ingreso'; END IF;
  IF NEW.`Fk_MovimientoBancario_Codigo` IS NOT NULL AND NOT EXISTS(SELECT 1 FROM tblMovimientoBancario
      WHERE Pk_MovimientoBancario=NEW.`Fk_MovimientoBancario_Codigo` AND Fk_CuentaBancaria_Codigo=NEW.`Fk_CuentaBancaria_Codigo`) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El movimiento bancario no pertenece a la cuenta del deposito';
  END IF;
END$$

CREATE TRIGGER `trDepositoVentaEncabezadoBeforeUpdate` BEFORE UPDATE ON `tblDepositoVentaEncabezado` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
  IF NOT EXISTS(SELECT 1 FROM tblCuentaBancaria WHERE Pk_CuentaBancaria=NEW.`Fk_CuentaBancaria_Codigo`
      AND Fk_Moneda_Codigo=NEW.`Fk_Moneda_Codigo`) THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='La cuenta bancaria debe usar la moneda del deposito'; END IF;
  IF NOT EXISTS(SELECT 1 FROM tblTipoTransaccion WHERE Pk_TipoTransaccion=NEW.`Fk_TipoTransaccion_Codigo`
      AND naturalezaTipoTransaccion IN ('INGRESO','AMBOS')) THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El deposito requiere un concepto bancario de ingreso'; END IF;
  IF NEW.`Fk_MovimientoBancario_Codigo` IS NOT NULL AND NOT EXISTS(SELECT 1 FROM tblMovimientoBancario
      WHERE Pk_MovimientoBancario=NEW.`Fk_MovimientoBancario_Codigo` AND Fk_CuentaBancaria_Codigo=NEW.`Fk_CuentaBancaria_Codigo`) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El movimiento bancario no pertenece a la cuenta del deposito';
  END IF;
END$$

CREATE TRIGGER `trDepositoVentaDetalleBeforeInsert` BEFORE INSERT ON `tblDepositoVentaDetalle` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM `tblDepositoVentaEncabezado` d JOIN `tblReciboPago` p
      ON p.`Fk_Moneda_Codigo`=d.`Fk_Moneda_Codigo`
    WHERE d.`Pk_DepositoVentaEncabezado`=NEW.`Fk_DepositoVentaEncabezado_Codigo` AND p.`Pk_ReciboPago`=NEW.`Fk_ReciboPago_Codigo`) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El recibo y el deposito deben usar la misma moneda';
  END IF;
END$$

CREATE TRIGGER `trDepositoVentaDetalleBeforeUpdate` BEFORE UPDATE ON `tblDepositoVentaDetalle` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM `tblDepositoVentaEncabezado` d JOIN `tblReciboPago` p
      ON p.`Fk_Moneda_Codigo`=d.`Fk_Moneda_Codigo`
    WHERE d.`Pk_DepositoVentaEncabezado`=NEW.`Fk_DepositoVentaEncabezado_Codigo` AND p.`Pk_ReciboPago`=NEW.`Fk_ReciboPago_Codigo`) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El recibo y el deposito deben usar la misma moneda';
  END IF;
END$$

CREATE TRIGGER `trCuentaContableCxCBeforeInsert` BEFORE INSERT ON `tblCuentaContableCxC` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trCuentaContableCxCBeforeUpdate` BEFORE UPDATE ON `tblCuentaContableCxC` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM tblMoneda WHERE Pk_Moneda=NEW.`Fk_Moneda_Codigo` AND codigoMoneda='GTQ') THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='Ventas opera en quetzales; seleccione GTQ del catalogo de Bancos';
  END IF;
END$$

CREATE TRIGGER `trComisionAbonoBeforeInsert` BEFORE INSERT ON `tblComisionAbono` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM `tblComisionFacturaEncabezado` ce JOIN `tblAplicacionPago` a
      ON a.`Fk_FacturaEncabezado_Codigo`=ce.`Fk_FacturaEncabezado_Codigo`
    WHERE ce.`Pk_ComisionFacturaEncabezado`=NEW.`Fk_ComisionFacturaEncabezado_Codigo`
      AND a.`Pk_AplicacionPago`=NEW.`Fk_AplicacionPago_Codigo`) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El abono de comision debe corresponder a una aplicacion de la misma factura';
  END IF;
END$$

CREATE TRIGGER `trComisionAbonoBeforeUpdate` BEFORE UPDATE ON `tblComisionAbono` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM `tblComisionFacturaEncabezado` ce JOIN `tblAplicacionPago` a
      ON a.`Fk_FacturaEncabezado_Codigo`=ce.`Fk_FacturaEncabezado_Codigo`
    WHERE ce.`Pk_ComisionFacturaEncabezado`=NEW.`Fk_ComisionFacturaEncabezado_Codigo`
      AND a.`Pk_AplicacionPago`=NEW.`Fk_AplicacionPago_Codigo`) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El abono de comision debe corresponder a una aplicacion de la misma factura';
  END IF;
END$$

CREATE TRIGGER `trComisionFacturaDetalleBeforeInsert` BEFORE INSERT ON `tblComisionFacturaDetalle` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM `tblComisionFacturaEncabezado` ce JOIN `tblFacturaDetalle` fd
      ON fd.`Fk_FacturaEncabezado_Codigo`=ce.`Fk_FacturaEncabezado_Codigo`
    JOIN tblProducto pr ON pr.Pk_Producto=fd.`Fk_Producto_Codigo`
    WHERE ce.`Pk_ComisionFacturaEncabezado`=NEW.`Fk_ComisionFacturaEncabezado_Codigo`
      AND fd.`Pk_FacturaDetalle`=NEW.`Fk_FacturaDetalle_Codigo`
      AND (NEW.`Fk_Linea_Codigo` IS NULL OR NEW.`Fk_Linea_Codigo`=pr.Fk_Linea_Codigo)
      AND (NEW.`Fk_Categoria_Codigo` IS NULL OR NEW.`Fk_Categoria_Codigo`=pr.Fk_Categoria_Codigo)
      AND (NEW.`Fk_Marca_Codigo` IS NULL OR NEW.`Fk_Marca_Codigo`=pr.Fk_Marca_Codigo)) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El detalle y la clasificacion deben pertenecer a la factura y producto comisionados';
  END IF;
END$$

CREATE TRIGGER `trComisionFacturaDetalleBeforeUpdate` BEFORE UPDATE ON `tblComisionFacturaDetalle` FOR EACH ROW
BEGIN
  IF NOT EXISTS(SELECT 1 FROM `tblComisionFacturaEncabezado` ce JOIN `tblFacturaDetalle` fd
      ON fd.`Fk_FacturaEncabezado_Codigo`=ce.`Fk_FacturaEncabezado_Codigo`
    JOIN tblProducto pr ON pr.Pk_Producto=fd.`Fk_Producto_Codigo`
    WHERE ce.`Pk_ComisionFacturaEncabezado`=NEW.`Fk_ComisionFacturaEncabezado_Codigo`
      AND fd.`Pk_FacturaDetalle`=NEW.`Fk_FacturaDetalle_Codigo`
      AND (NEW.`Fk_Linea_Codigo` IS NULL OR NEW.`Fk_Linea_Codigo`=pr.Fk_Linea_Codigo)
      AND (NEW.`Fk_Categoria_Codigo` IS NULL OR NEW.`Fk_Categoria_Codigo`=pr.Fk_Categoria_Codigo)
      AND (NEW.`Fk_Marca_Codigo` IS NULL OR NEW.`Fk_Marca_Codigo`=pr.Fk_Marca_Codigo)) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El detalle y la clasificacion deben pertenecer a la factura y producto comisionados';
  END IF;
END$$

CREATE TRIGGER `trClienteFacturaVersionBeforeUpdate` BEFORE UPDATE ON `tblClienteFacturaVersion` FOR EACH ROW
BEGIN
  IF EXISTS(SELECT 1 FROM `tblFacturaEncabezado` WHERE `Fk_ClienteFacturaVersion_Codigo`=OLD.`Pk_ClienteFacturaVersion`) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='La identidad ya facturada es historica; registre una nueva version';
  END IF;
END$$

CREATE TRIGGER `trMovimientoCxCBeforeDelete` BEFORE DELETE ON `tblMovimientoCxC` FOR EACH ROW
BEGIN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El historico contable no puede eliminarse';
END$$

CREATE TRIGGER `trCierreSaldoClienteBeforeDelete` BEFORE DELETE ON `tblCierreSaldoCliente` FOR EACH ROW
BEGIN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El historico contable no puede eliminarse';
END$$

CREATE TRIGGER `trCierreMovimientoCxCDetalleBeforeDelete` BEFORE DELETE ON `tblCierreMovimientoCxCDetalle` FOR EACH ROW
BEGIN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='El historico contable no puede eliminarse';
END$$

CREATE TRIGGER `trCierreMovimientoCxCEncabezadoBeforeDelete` BEFORE DELETE ON `tblCierreMovimientoCxCEncabezado` FOR EACH ROW
BEGIN
  IF OLD.`estadoCierreMovimientoCxCEncabezado`='CONFIRMADO' THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='No se elimina un cierre confirmado'; END IF;
END$$

DELIMITER ;

-- ----------------------------- ¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡  Modulo de compras !!!!!!!!!!!!!!!!!!!!!!!!!-------------

CREATE TABLE IF NOT EXISTS tblProveedor(
	idProveedor INT AUTO_INCREMENT,
    nombreProveedor VARCHAR(100) NOT NULL,
    nitProveedor char(10) not null,
    correoProveedor VARCHAR(150) NOT NULL,
    telefonoProveedor varchar(30) NOT NULL,
    direccionProveedor VARCHAR(500) NOT NULL,
    estadoProveedor BOOLEAN NOT NULL DEFAULT 1,
	created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT Pk_Proveedor PRIMARY KEY(idProveedor),
   CONSTRAINT uqProveedorNit UNIQUE (nitProveedor)

);
CREATE TABLE tblCuentaPorPagar (
    idCuentaPorPagar               INT            NOT NULL AUTO_INCREMENT,
    idProveedor                    INT            NOT NULL,
    numeroFacturaCuentaPorPagar     VARCHAR(30)    NOT NULL,
    fechaEmisionCuentaPorPagar      DATE           NOT NULL,
    fechaVencimientoCuentaPorPagar  DATE           NOT NULL,
    montoTotalCuentaPorPagar        DECIMAL(12,4)  NOT NULL,
    saldoCuentaPorPagar             DECIMAL(12,4)  NOT NULL,
    estadoCuentaPorPagar            VARCHAR(30)    NOT NULL DEFAULT 'Pendiente',
    created_at                      TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                      TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

   CONSTRAINT Pk_CuentaPorPagar PRIMARY KEY (idCuentaPorPagar),
	CONSTRAINT Fk_CuentaPorPagar_Proveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor (idProveedor),
    INDEX idxCuentaPorPagarProveedor (idProveedor),
    CONSTRAINT uqFacturaProveedorCuentaPorPagar UNIQUE (idProveedor, numeroFacturaCuentaPorPagar),
    CONSTRAINT ckMontoTotalCuentaPorPagar CHECK (montoTotalCuentaPorPagar > 0),
    CONSTRAINT ckSaldoCuentaPorPagar CHECK (saldoCuentaPorPagar >= 0 AND saldoCuentaPorPagar <= montoTotalCuentaPorPagar),
    CONSTRAINT ckEstadoCuentaPorPagar CHECK (estadoCuentaPorPagar IN ('Pendiente','Parcial','Pagada','Anulada'))
) ENGINE=InnoDB;

CREATE TABLE tblPagoProveedor (
    idPagoProveedor             INT            NOT NULL AUTO_INCREMENT,
    idCuentaPorPagar            INT            NOT NULL,
    idProveedor                 INT            NOT NULL,
    fechaPagoProveedor          DATE           NOT NULL,
    montoPagoProveedor          DECIMAL(12,4)  NOT NULL,
    metodoPagoProveedor         VARCHAR(30)    NOT NULL,
    referenciaPagoProveedor     VARCHAR(30)    NULL,
    estadoPagoProveedor         VARCHAR(30)    NOT NULL DEFAULT 'Programado',
    created_at                  TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                  TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT Pk_PagoProveedor PRIMARY KEY (idPagoProveedor),
	CONSTRAINT Fk_PagoProveedor_CuentaPorPagar FOREIGN KEY (idCuentaPorPagar) REFERENCES tblCuentaPorPagar (idCuentaPorPagar),
    CONSTRAINT Fk_PagoProveedor_Proveedor FOREIGN KEY (idProveedor) REFERENCES tblProveedor (idProveedor),
    INDEX idxPagoProveedorCuenta (idCuentaPorPagar),
    INDEX idxPagoProveedorProveedor (idProveedor),
    INDEX idxPagoProveedorFecha (fechaPagoProveedor),
    CONSTRAINT ckMontoPagoProveedor CHECK (montoPagoProveedor > 0),
    CONSTRAINT ckEstadoPagoProveedor CHECK (estadoPagoProveedor IN ('Pagado','Programado','Rechazado','Anulado')),
    CONSTRAINT ckMetodoPagoProveedor CHECK (metodoPagoProveedor IN ('Transferencia','Cheque','Efectivo','Deposito'))
) ENGINE=InnoDB;

CREATE TABLE tblSolicitudPago (
    idSolicitudPago                INT            NOT NULL AUTO_INCREMENT,
    moduloOrigenSolicitudPago       VARCHAR(30)    NOT NULL,
    idOrigenSolicitudPago           INT            NOT NULL,
    beneficiarioSolicitudPago       VARCHAR(100)   NOT NULL,
    montoSolicitudPago              DECIMAL(12,4)  NOT NULL,
    metodoSolicitudPago             VARCHAR(30)    NOT NULL,
    estadoSolicitudPago             VARCHAR(30)    NOT NULL DEFAULT 'Pendiente',
    created_at                      TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at                      TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

	CONSTRAINT Pk_SolicitudPago PRIMARY KEY (idSolicitudPago),
    INDEX idxSolicitudPagoOrigen (moduloOrigenSolicitudPago, idOrigenSolicitudPago),
    CONSTRAINT ckMontoSolicitudPago CHECK (montoSolicitudPago > 0),
    CONSTRAINT ckEstadoSolicitudPago CHECK (estadoSolicitudPago IN ('Pendiente','Procesada','Rechazada'))
) ENGINE=InnoDB;
