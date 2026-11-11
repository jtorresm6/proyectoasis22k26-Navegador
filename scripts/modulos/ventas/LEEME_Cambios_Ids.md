# Cambios de llaves de Ventas - 04/11/2026

Se mantienen las 38 tablas, su orden, autores, tipos, cálculos, índices y relaciones, incluidas las 11 llaves primarias compuestas.

- `id` pasa a `idNombreTabla`: `idCliente`, `idCotizacionEncabezado`, `idCotizacionDetalle`, etc.
- Las columnas foráneas usan `id` y la entidad relacionada: `idCliente`, `idProducto`, `idUsuarioRegistro`, etc.
- Se eliminan `idmodulo`, `idaplicacion` y sus CHECK.
- Se conservan los nombres de restricciones `Pk_...` y `Fk_...`: no son nombres de columnas.
- No se renombran tablas o llaves de otros módulos. Sus columnas referenciadas conservan los nombres originales (`Pk_Producto`, `Pk_Moneda`, etc.).
- FacturaDetalle conserva la relación por UUID: `idFacturaEncabezadoUuidDte` sigue siendo CHAR(36) y apunta a `uuidDteFacturaEncabezado`.
- CotizacionDetalle conserva PRIMARY KEY (`idCotizacionEncabezado`, `idCotizacionDetalle`).

## Ejecución

`Ventasbd v1.2.sql` crea las tablas en `dbventas`. No es una migración de una base con datos. No se ejecutó sobre la base de datos del usuario.

En la base compartida de Seguridad `dbsistemaembutidos`, ejecutar:

1. `Insert_Modulo_Aplicaciones_VENTAS.sql`: módulo 7000 y aplicaciones 7001 a 7016.
2. `Insert_Pruebas_Permisos_VENTAS.sql`: permisos completos al rol 1 y al usuario existente `imelendez`, igual que el ejemplo de Bancos. No crea usuarios ni roles.

Los inserts se pueden repetir sin duplicar registros. Se incluye 7016 porque Consulta de recibos utiliza ese código en el formulario actual. 7013 corresponde a reglas/liquidación de comisiones. 7000 identifica el módulo, no una aplicación.

Las consultas MVC que usen los nombres anteriores deben actualizarse antes de conectar la aplicación a esta estructura. No se modificó código C#.

La nomenclatura `idNombre` sigue la instrucción del equipo; difiere de los prefijos Pk_/Fk_ para columnas del PDF EST-01 V1.4. Se conserva el resto de la estructura sin aplicar cambios adicionales de tipos o precisiones.
