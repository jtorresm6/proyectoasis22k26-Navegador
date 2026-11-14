using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using CapaModelo_ListadoPrecios_Ventas.Contratos;
using CapaModelo_ListadoPrecios_Ventas.Entidades;

namespace CapaModelo_ListadoPrecios_Ventas
{
    // El listado utiliza las tablas reales de dbsistemaembutidos.
    public class ClsModeloListadoPrecios : IRepositorioListadoPrecios
    {
        // Se reutiliza la conexión central del proyecto de Ventas.
        private static string CadenaConexion { get { return CapaModelo_Ventas.ClsConexion.CadenaConexion; } }

        public bool UsaBaseDeDatos { get; private set; }

        // True cuando tblproducto tiene idProveedorProductos (relación con tblproveedor en v1.6).
        public bool ProveedorRelacionado { get; private set; }
        public string MensajeEstado { get; private set; }

        public ClsModeloListadoPrecios()
        {
            MensajeEstado = "Aún no se ha consultado la base de datos.";
        }

        public List<ClsPrecioCliente> VentasFuncObtenerListado()
        {
            var resultado = new List<ClsPrecioCliente>();

            // tbltipocliente contiene el nombre y el descuento del cliente.
            // No se filtra por estadoTipoCliente porque esa columna no existe en la tabla real.
            // Tampoco se relaciona con tbllistaprecioencabezado: el respaldo no contiene una FK entre ambas tablas.
            // En dbSistemaEmbutidos v1.6 las claves de Ventas usan idTipoCliente e idProducto.
            // tblProducto conserva las FK de clasificación Fk_Marca_Codigo/Fk_Linea_Codigo/Fk_Categoria_Codigo.
            const string sqlBase = @"
                SELECT
                    tc.idTipoCliente AS IdTipoCliente,
                    tc.nombreTipoCliente AS TipoCliente,
                    tc.porcentajeDescuentoTipoCliente AS DescuentoCliente,
                    p.idProducto AS IdProducto,
                    p.nombreProducto AS Producto,
                    COALESCE(m.nombreMarca, 'Sin marca') AS Marca,
                    COALESCE(li.nombreLinea, 'Sin línea') AS Linea,
                    COALESCE(c.nombreCategoria, 'Sin categoría') AS Categoria,
                    {PROVEEDOR}
                    p.precioVentaProducto AS PrecioBase,
                    ROUND(
                        p.precioVentaProducto *
                        (1 - (tc.porcentajeDescuentoTipoCliente / 100)),
                        2
                    ) AS PrecioFinal
                FROM tblproducto p
                CROSS JOIN tbltipocliente tc
                LEFT JOIN tblmarca m ON m.idMarca = p.Fk_Marca_Codigo
                LEFT JOIN tbllinea li ON li.idLinea = p.Fk_Linea_Codigo
                LEFT JOIN tblcategoria c ON c.idCategoria = p.Fk_Categoria_Codigo
                {JOIN_PROVEEDOR}
                ORDER BY tc.nombreTipoCliente, p.nombreProducto;";

            try
            {
                using (var conexion = new OdbcConnection(CadenaConexion))
                {
                    conexion.Open();

                    ProveedorRelacionado = ExisteColumnaProveedorProducto(conexion);

                    string sql = sqlBase
                        .Replace("{PROVEEDOR}", ProveedorRelacionado
                            ? "COALESCE(pv.nombreProveedor, 'Sin proveedor') AS Proveedor, COALESCE(pv.idProveedor, 0) AS IdProveedor,"
                            : "'Sin proveedor' AS Proveedor, 0 AS IdProveedor,")
                        .Replace("{JOIN_PROVEEDOR}", ProveedorRelacionado
                            ? "LEFT JOIN tblproveedor pv ON pv.idProveedor = p.idProveedorProductos"
                            : string.Empty);

                    using (var comando = new OdbcCommand(sql, conexion))
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            decimal precioBase = Convert.ToDecimal(
                                lector["PrecioBase"], CultureInfo.InvariantCulture);
                            decimal precioFinal = Convert.ToDecimal(
                                lector["PrecioFinal"], CultureInfo.InvariantCulture);

                            resultado.Add(new ClsPrecioCliente
                            {
                                IdListaPrecio = 0,
                                IdTipoCliente = Convert.ToInt32(
                                    lector["IdTipoCliente"], CultureInfo.InvariantCulture),
                                IdProducto = Convert.ToInt32(
                                    lector["IdProducto"], CultureInfo.InvariantCulture),
                                IdProveedor = Convert.ToInt32(
                                    lector["IdProveedor"], CultureInfo.InvariantCulture),
                                TipoCliente = Convert.ToString(
                                    lector["TipoCliente"], CultureInfo.InvariantCulture),
                                Producto = Convert.ToString(
                                    lector["Producto"], CultureInfo.InvariantCulture),
                                Marca = Convert.ToString(
                                    lector["Marca"], CultureInfo.InvariantCulture),
                                Linea = Convert.ToString(
                                    lector["Linea"], CultureInfo.InvariantCulture),
                                Categoria = Convert.ToString(
                                    lector["Categoria"], CultureInfo.InvariantCulture),
                                Proveedor = Convert.ToString(
                                    lector["Proveedor"], CultureInfo.InvariantCulture),
                                PrecioBase = precioBase,
                                PrecioFinal = precioFinal,
                                PorcentajeDescuento = Convert.ToDecimal(
                                    lector["DescuentoCliente"], CultureInfo.InvariantCulture)
                            });
                        }
                    }
                }

                UsaBaseDeDatos = true;
                MensajeEstado = resultado.Count == 0
                    ? "La conexión funciona, pero no hay productos o tipos de cliente registrados."
                    : (ProveedorRelacionado
                        ? "Datos cargados desde tblproducto, tbltipocliente y tblproveedor."
                        : "Datos cargados desde tblproducto y tbltipocliente. tblproducto aún no tiene la columna idProveedorProductos para asociar proveedores.");
                return resultado;
            }
            catch (Exception error)
            {
                UsaBaseDeDatos = false;
                MensajeEstado = "Error al consultar dbsistemaembutidos: " + error.Message;
                throw;
            }
        }

        private static bool ExisteColumnaProveedorProducto(OdbcConnection conexion)
        {
            const string sqlColumna = @"
                SELECT COUNT(*)
                FROM information_schema.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE()
                  AND TABLE_NAME = 'tblproducto'
                  AND COLUMN_NAME = 'idProveedorProductos';";

            using (var comando = new OdbcCommand(sqlColumna, conexion))
                return Convert.ToInt32(comando.ExecuteScalar(), CultureInfo.InvariantCulture) > 0;
        }

        // Obtiene los proveedores activos de tblproveedor para llenar el filtro del formulario.
        public List<string> VentasFuncObtenerProveedores()
        {
            var proveedores = new List<string>();
            const string sql = "SELECT nombreProveedor FROM tblproveedor WHERE estadoProveedor = 1 ORDER BY nombreProveedor;";

            using (var conexion = new OdbcConnection(CadenaConexion))
            {
                conexion.Open();
                using (var comando = new OdbcCommand(sql, conexion))
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                        proveedores.Add(Convert.ToString(lector[0], CultureInfo.InvariantCulture));
                }
            }

            return proveedores;
        }

        public bool VentasFuncGuardar(ClsPrecioCliente registro, out string mensaje)
        {
            var registros = new List<ClsPrecioCliente>();
            if (registro != null) registros.Add(registro);
            return VentasFuncGuardarVarios(registros, out mensaje);
        }

        // Guarda los cambios según las columnas que realmente existen en la base de datos.
        // El descuento pertenece al tipo de cliente y afecta a todos sus productos.
        // Los aumentos modifican precioVentaProducto y, por tanto, afectan a todos los tipos de cliente.
        public bool VentasFuncGuardarVarios(IList<ClsPrecioCliente> registros, out string mensaje)
        {
            if (registros == null || registros.Count == 0)
            {
                mensaje = "No hay registros para guardar.";
                return false;
            }

            foreach (ClsPrecioCliente registro in registros)
            {
                if (registro == null || registro.IdProducto <= 0 || registro.IdTipoCliente <= 0)
                {
                    mensaje = "Hay un registro sin identificador de producto o tipo de cliente válido.";
                    return false;
                }
                if (registro.PrecioBase < 0m || registro.PrecioFinal < 0m)
                {
                    mensaje = "El precio base y el precio final no pueden ser negativos.";
                    return false;
                }
                if (registro.PorcentajeDescuento < 0m || registro.PorcentajeDescuento > 100m)
                {
                    mensaje = "El descuento debe estar entre 0 y 100 por ciento.";
                    return false;
                }
            }

            string tipoModificacion = registros[0].TipoModificacion ?? string.Empty;
            if (registros.Any(x => !string.Equals(
                x.TipoModificacion, tipoModificacion, StringComparison.OrdinalIgnoreCase)))
            {
                mensaje = "No se pueden mezclar tipos de modificación en una misma operación.";
                return false;
            }

            const string actualizarDescuentoCliente = @"
                UPDATE tbltipocliente
                SET porcentajeDescuentoTipoCliente = ?
                WHERE idTipoCliente = ?;";

            const string actualizarPrecioProducto = @"
                UPDATE tblproducto
                SET precioVentaProducto = ?
                WHERE idProducto = ?;";

            try
            {
                using (var conexion = new OdbcConnection(CadenaConexion))
                {
                    conexion.Open();
                    using (OdbcTransaction transaccion = conexion.BeginTransaction())
                    {
                        try
                        {
                            if (string.Equals(tipoModificacion, "Descuento", StringComparison.OrdinalIgnoreCase))
                            {
                                // Un tipo de cliente tiene un único porcentajeDescuento en la base de datos.
                                foreach (ClsPrecioCliente registro in registros
                                    .GroupBy(x => x.IdTipoCliente)
                                    .Select(g => g.First()))
                                {
                                    using (var comando = new OdbcCommand(
                                        actualizarDescuentoCliente, conexion, transaccion))
                                    {
                                        comando.Parameters.Add("porcentajeDescuento", OdbcType.Decimal).Value =
                                            registro.PorcentajeDescuento;
                                        comando.Parameters.Add("idTipoCliente", OdbcType.Int).Value =
                                            registro.IdTipoCliente;

                                        comando.ExecuteNonQuery();
                                    }
                                }
                            }
                            else if (string.Equals(tipoModificacion, "AumentoBase", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(tipoModificacion, "AumentoPorcentaje", StringComparison.OrdinalIgnoreCase))
                            {
                                // El precio de venta pertenece al producto, no al tipo de cliente.
                                foreach (ClsPrecioCliente registro in registros
                                    .GroupBy(x => x.IdProducto)
                                    .Select(g => g.First()))
                                {
                                    using (var comando = new OdbcCommand(
                                        actualizarPrecioProducto, conexion, transaccion))
                                    {
                                        comando.Parameters.Add("precioVentaProducto", OdbcType.Decimal).Value =
                                            registro.PrecioBase;
                                        comando.Parameters.Add("idProducto", OdbcType.Int).Value =
                                            registro.IdProducto;

                                        comando.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException("Tipo de modificación no reconocido.");
                            }

                            transaccion.Commit();
                        }
                        catch
                        {
                            transaccion.Rollback();
                            throw;
                        }
                    }
                }

                UsaBaseDeDatos = true;
                mensaje = string.Equals(tipoModificacion, "Descuento", StringComparison.OrdinalIgnoreCase)
                    ? "Se actualizó el descuento de " + registros.Select(x => x.IdTipoCliente).Distinct().Count() +
                      " tipo(s) de cliente. El porcentaje aplica a todos los productos de cada tipo."
                    : "Se actualizó el precio base de " + registros.Select(x => x.IdProducto).Distinct().Count() +
                      " producto(s). El cambio aplica a todos los tipos de cliente.";
                return true;
            }
            catch (Exception error)
            {
                UsaBaseDeDatos = false;
                mensaje = "No se guardaron los cambios. La transacción fue cancelada: " + error.Message;
                return false;
            }
        }

        public static decimal VentasFuncCalcularPrecioFinal(
            decimal precioBase, decimal valorModificacion, string tipoModificacion)
        {
            if (precioBase < 0m)
                throw new ArgumentOutOfRangeException("precioBase", "El precio base no puede ser negativo.");
            if (valorModificacion < 0m)
                throw new ArgumentOutOfRangeException("valorModificacion", "El valor no puede ser negativo.");

            decimal precioFinal;
            if (tipoModificacion == "Descuento" || tipoModificacion == "Descuento (%)")
                precioFinal = precioBase - (precioBase * (valorModificacion / 100m));
            else if (tipoModificacion == "AumentoPorcentaje" || tipoModificacion == "Aumento por porcentaje (%)")
                precioFinal = precioBase + (precioBase * (valorModificacion / 100m));
            else if (tipoModificacion == "AumentoBase" || tipoModificacion == "Aumento por base")
                precioFinal = precioBase + valorModificacion;
            else
                throw new ArgumentException("Tipo de modificación no reconocido.", "tipoModificacion");

            return Math.Round(precioFinal, 2, MidpointRounding.AwayFromZero);
        }
    }
}
