USE dbsistemaembutidos;

-- ---------- Módulo 9000 ----------
INSERT INTO tblModulo (idModulo, nombreModulo, descripcionModulo, is_active)
VALUES (9000, 'Recursos Humanos',
        'Módulo de administración del personal: empleados, asistencia, permisos, capacitaciones, desempeño, nómina y liquidaciones', 1) AS nuevo
ON DUPLICATE KEY UPDATE
    nombreModulo      = nuevo.nombreModulo,
    descripcionModulo = nuevo.descripcionModulo,
    is_active         = 1;

-- ---------- Aplicaciones del módulo 9000 ----------
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
    
    
    -- Permisos totales para el perfil Administrador (idRol = 1)
-- en todas las aplicaciones del módulo 9000 que aún no tienen asignación
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
  
  
  -- ---------- Verificación ----------
SELECT idModulo, idAplicacion, nombreAplicacion, is_active
FROM tblAplicacion
WHERE idModulo = 9000
ORDER BY idAplicacion;