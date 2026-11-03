# Estructura MVC de movimientos de proveedores

La organización sigue Practica-MVC-P-II:

- `Entidades`: datos de las notas y filas del balance, sin lógica de presentación.
- `Contratos`: operaciones de notas y balance, utilizadas por los controladores mediante interfaces.
- `Repositorios/Repositorio`: acceso a la conexión ODBC compartida de Compras.
- `Repositorios/RepositorioMaestro`: ejecución SQL parametrizada y liberación de recursos.
- Repositorios concretos: consultas, conversión de filas y registro transaccional de notas.
- `ClsModeloNotaProveedor`: propiedades del modelo, validación, construcción de la entidad y `GrabarCambios`, coordinado mediante `EstadoEntidad`.
- `ClsControladorBalanceAntiguedadProveedor`: consulta del balance mediante su contrato de repositorio.

Los métodos `ComprasMet...` usados por los formularios se conservan como entrada compatible. Las bases `ClsConexionCompras` y `ClsSentenciasCompras` delegan a la nueva jerarquía.

A diferencia del mantenimiento de películas de la referencia, este proceso solo registra notas y consulta movimientos: los estados Modified y Deleted se rechazan. No se agregan operaciones para alterar o eliminar documentos contables existentes. El guardado propaga las excepciones para que el formulario conserve su manejo de errores.

Se mantienen el DSN de Compras, los parámetros posicionales ODBC, la precisión decimal y la transacción que verifica proveedor, concepto, moneda y duplicados antes de insertar. El balance clasifica saldos actuales por fecha de vencimiento y moneda; no reconstruye saldos históricos.
