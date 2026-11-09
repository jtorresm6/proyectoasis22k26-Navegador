-- =============================================================================
--  TERMINUS ERP  |  Contabilidad
--  Script 2 de 2: datos
-- =============================================================================

USE dbContabilidad;

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