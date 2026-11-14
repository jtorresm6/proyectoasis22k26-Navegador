# Órdenes de compra

Los nombres siguen `Cls` + pantalla o función + responsabilidad. Los formularios y controles conservan sus archivos `.Designer.cs` y `.resx` junto al archivo principal.

| Capa | Ubicación | Responsabilidad |
| --- | --- | --- |
| Vista | Raíz | `FrmEncabezados` y `ClsEncabezadosOrdenesCompraTabla` |
| Vista | Raíz | `FrmOrdenesCompra`, `ClsOrdenesCompraCatalogos`, `ClsOrdenesCompraFlujo` y `ClsOrdenesCompraErrores` |
| Vista | Raíz | `FrmDetalleOrdenCompra`, `ClsDetallesOrdenesCompraDiseno`, `ClsDetallesOrdenesCompraTabla` y `ClsDetallesOrdenesCompraReporte` |
| Vista | Raíz | `UsrProductosOrdenesCompra`, `ClsProductosOrdenesCompraTabla`, `ClsProductosOrdenesCompraPestanas` y `ClsProductosOrdenesCompraPaginacion` |
| Vista | Raíz | `ClsOrdenesCompraComunEdicion` y `ClsOrdenesCompraComunPanel` |
| Controlador | Raíz | `ClsOrdenesCompraControlador`, `ClsEncabezadosOrdenesCompraControlador` y `ClsDetallesOrdenesCompraControlador` |
| Controlador | `Servicios` | `ClsOrdenesCompraValidar`, `ClsDetallesOrdenesCompraValidar`, `ClsOrdenesCompraCalcular`, `ClsEncabezadosOrdenesCompraFiltrar` y `ClsOrdenesCompraEstados` |
| Controlador | Raíz | `ClsOrdenesCompraDatos`, datos entregados a la vista |
| Modelo | `Entidades` | `ClsOrdenesCompraEntidad`, representación de la orden persistida |
| Modelo | `Contratos` | `IOrdenesCompraRepositorio` |
| Modelo | `Repositorios` | `ClsOrdenesCompraRepositorio`, `ClsOrdenesCompraRepositorioBase`, `ClsEncabezadosOrdenesCompraConsultar`, `ClsDetallesOrdenesCompraConsultar`, `ClsOrdenesCompraConsultarCatalogos`, `ClsOrdenesCompraComandos` y `ClsOrdenesCompraErroresDatos` |

`ClsProductosOrdenesCompraTabla.cs` mantiene las líneas y sus importes; su archivo parcial `.Columnas.cs` configura la presentación del grid. Las pruebas unitarias enlazan únicamente el archivo de datos.

Los formularios conservan sus nombres originales: `FrmEncabezados`, `FrmOrdenesCompra` y `FrmDetalleOrdenCompra`. No se necesitan adaptadores de compatibilidad. Los archivos de Vista están en la raíz; únicamente los reportes están en `Reportes`. Los archivos generados de compilación permanecen en `bin` y `obj`.

`ClsOrdenesCompraDatos` está directamente en Controlador y utiliza su namespace principal. La entidad de persistencia permanece en Modelo.

`ClsOrdenesCompraControlador` conserva el punto de entrada y delega consultas de encabezados, operaciones del detalle, cálculos y validaciones a sus clases específicas. La persistencia continúa usando el esquema existente.

La suite unitaria está en `PruebaUnitaria_OrdenesCompra`; las verificaciones de interfaz, navegación y reportes están en `Validacion`.
