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


-- ---------- 7. Permisos y vacaciones (simplificado) ----------
ALTER TABLE tblPermisoEmpleado
    DROP FOREIGN KEY Fk_PermisoEmpleado_TipoPermiso,
    DROP COLUMN idTipoPermiso,
    DROP COLUMN horaInicioPermisoEmpleado,
    DROP COLUMN horaFinPermisoEmpleado,
    DROP COLUMN unidadPermisoEmpleado,
    DROP COLUMN estadoPermisoEmpleado,
    ADD COLUMN conGoceSueldoPermisoEmpleado BOOLEAN NOT NULL DEFAULT TRUE AFTER motivoPermisoEmpleado,
    MODIFY COLUMN totalPermisoEmpleado DECIMAL(6,2) NULL DEFAULT 0;

ALTER TABLE tblVacacionEmpleado
    DROP FOREIGN KEY Fk_VacacionEmpleado_PeriodoVacacion,
    DROP COLUMN idPeriodoVacacion,
    DROP COLUMN estadoVacacionEmpleado;


DROP TABLE tblPeriodoVacacion, tblTipoPermiso;

-- fin de cambios Daniella Jurado 0901-23-3679


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
    valorImpuesto                 DECIMAL(5,4) NOT NULL,
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

