# Pruebas de MovimientosProveedores

Proyecto MSTest para .NET Framework 4.8, integrado en MovimientosProveedores.sln.

## Ejecutar

1. Abrir MovimientosProveedores.sln en Visual Studio con las herramientas de .NET Framework 4.8.
2. Restaurar paquetes NuGet (MSTest.TestFramework y MSTest.TestAdapter 2.2.10).
3. Compilar en Debug y ejecutar las pruebas desde Prueba > Explorador de pruebas. La arquitectura del proyecto es x64.

## Cobertura

33 casos con datos fijos y fechas independientes del reloj del equipo:

- Proveedor, concepto y moneda con identificadores cero o negativos.
- Serie y numero nulos, vacios, con espacios y mayores al limite.
- Vencimiento anterior a emision.
- Tipo de cambio e importe cero, negativos, fuera de rango o con demasiados decimales.
- Valores validos, minimos positivos, maximos y longitudes maximas; vencimiento el mismo dia.
- Registro y normalizacion de espacios; rechazo sin guardar para datos invalidos.
- Rechazo de modificaciones, eliminaciones y estados desconocidos.
- Repositorio nulo, consultas de catalogos e historial.

Los casos invalidos deben aparecer como correctos cuando la validacion lanza la excepcion esperada. Se utiliza un repositorio de prueba en memoria: no requiere conexion ODBC y no modifica datos reales.

Los datos se pueden editar en los atributos DataRow y en CrearNota de TestMovimientosProveedores.cs. El proyecto referencia las capas reales de controlador y modelo.
