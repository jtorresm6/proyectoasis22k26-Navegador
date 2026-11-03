using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using CapaModelo_ListadoPrecios.Contratos;
using CapaModelo_ListadoPrecios.Entidades;

namespace CapaModelo_ListadoPrecios
{
    // El listado utiliza las tablas reales de dbsistemaembutidos.
    public class ClsModeloListadoPrecios : IRepositorioListadoPrecios
    {
        // Se reutiliza la conexión central del proyecto de Ventas.
        private static string CadenaConexion { get { return CapaModelo_Ventas.ClsConexion.CadenaConexion; } }

        public bool UsaBaseDeDatos { get; private set; }
        public string MensajeEstado { get; private set; }

        public ClsModeloListadoPrecios()
        {
            MensajeEstado = "Aún no se ha consultado la base de datos.";
        }

        public List<ClsPrecioCliente> VentasFuncObtenerListado()
        {
            var resultado = new List<ClsPrecioCliente>();

            // LEFT JOIN con listas vigentes: si aún no hay listas, se muestra el precio de venta del producto.
            const string sql = @"
                SELECT
                    COALESCE(l.Pk_ListaPrecioEncabezado, 0) AS IdLista,
                    CASE WHEN l.Pk_ListaPrecioEncabezado IS NULL
                         THEN 'Precio de venta'
                         ELSE CONCAT(l.codigoListaPrecioEncabezado, ' - ', l.nombreListaPrecioEncabezado)
                    END AS ListaPrecio,
                    p.idProducto,
                    p.nombreProducto,
                    COALESCE(m.nombreMarca, 'Sin marca') AS Marca,
                    COALESCE(li.nombreLinea, 'Sin línea') AS Linea,
                    COALESCE(c.nombreCategoria, 'Sin categoría') AS Categoria,
                    'No disponible en BD' AS Proveedor,
                    p.precioVentaProducto AS PrecioBase,
                    COALESCE(d.precioUnitarioListaPrecioDetalle, p.precioVentaProducto) AS PrecioFinal
                FROM tblproducto p
                LEFT JOIN tblmarca m ON m.idMarca = p.fkMarcaIdMarca
                LEFT JOIN tbllinea li ON li.idLinea = p.fkLineaIdLinea
                LEFT JOIN tblcategoria c ON c.idCategoria = p.fkCategoriaIdCategoria
                LEFT JOIN (
                    SELECT Pk_ListaPrecioEncabezado, codigoListaPrecioEncabezado,
                           nombreListaPrecioEncabezado
                    FROM tbllistaprecioencabezado
                    WHERE activoListaPrecioEncabezado = 1
                      AND vigenteDesdeListaPrecioEncabezado <= CURDATE()
                      AND (vigenteHastaListaPrecioEncabezado IS NULL
                           OR vigenteHastaListaPrecioEncabezado >= CURDATE())
                ) l ON 1 = 1
                LEFT JOIN tbllistapreciodetalle d
                    ON d.Fk_ListaPrecioEncabezado_Codigo = l.Pk_ListaPrecioEncabezado
                   AND d.Fk_Producto_Codigo = p.idProducto
                   AND d.activoListaPrecioDetalle = 1
                WHERE p.estadoProducto = 1
                ORDER BY ListaPrecio, p.nombreProducto;";

            try
            {
                using (var conexion = new OdbcConnection(CadenaConexion))
                {
                    conexion.Open();
                    using (var comando = new OdbcCommand(sql, conexion))
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            decimal precioBase = Convert.ToDecimal(lector["PrecioBase"], CultureInfo.InvariantCulture);
                            decimal precioFinal = Convert.ToDecimal(lector["PrecioFinal"], CultureInfo.InvariantCulture);

                            resultado.Add(new ClsPrecioCliente
                            {
                                IdListaPrecio = Convert.ToInt32(lector["IdLista"], CultureInfo.InvariantCulture),
                                IdProducto = Convert.ToInt32(lector["idProducto"], CultureInfo.InvariantCulture),
                                TipoCliente = Convert.ToString(lector["ListaPrecio"], CultureInfo.InvariantCulture),
                                Producto = Convert.ToString(lector["nombreProducto"], CultureInfo.InvariantCulture),
                                Marca = Convert.ToString(lector["Marca"], CultureInfo.InvariantCulture),
                                Linea = Convert.ToString(lector["Linea"], CultureInfo.InvariantCulture),
                                Categoria = Convert.ToString(lector["Categoria"], CultureInfo.InvariantCulture),
                                Proveedor = Convert.ToString(lector["Proveedor"], CultureInfo.InvariantCulture),
                                PrecioBase = precioBase,
                                PrecioFinal = precioFinal,
                                PorcentajeDescuento = precioBase > 0m
                                    ? Math.Round((precioBase - precioFinal) / precioBase * 100m, 2)
                                    : 0m
                            });
                        }
                    }
                }

                UsaBaseDeDatos = true;
                MensajeEstado = resultado.Count == 0
                    ? "La conexión funciona, pero no hay productos activos en tblproducto."
                    : "Datos cargados desde dbsistemaembutidos.";
                return resultado;
            }
            catch (Exception error)
            {
                UsaBaseDeDatos = false;
                MensajeEstado = "Error al consultar dbsistemaembutidos: " + error.Message;
                throw;
            }
        }

        public bool VentasFuncGuardar(ClsPrecioCliente registro, out string mensaje)
        {
            var registros = new List<ClsPrecioCliente>();
            if (registro != null) registros.Add(registro);
            return VentasFuncGuardarVarios(registros, out mensaje);
        }

        // Guarda el conjunto completo en una sola transacción: todos se aplican o ninguno.
        public bool VentasFuncGuardarVarios(IList<ClsPrecioCliente> registros, out string mensaje)
        {
            if (registros == null || registros.Count == 0)
            {
                mensaje = "No hay registros para guardar.";
                return false;
            }

            foreach (ClsPrecioCliente registro in registros)
            {
                if (registro == null || registro.IdProducto <= 0 || registro.PrecioFinal < 0m)
                {
                    mensaje = "Hay un registro sin identificador de producto válido o con precio negativo.";
                    return false;
                }
                if (registro.IdListaPrecio < 0)
                {
                    mensaje = "El identificador de lista de precios no es válido.";
                    return false;
                }
            }

            const string guardarDetalle = @"
                INSERT INTO tbllistapreciodetalle
                    (Fk_ListaPrecioEncabezado_Codigo, Fk_Producto_Codigo,
                     precioUnitarioListaPrecioDetalle, activoListaPrecioDetalle)
                VALUES (?, ?, ?, 1)
                ON DUPLICATE KEY UPDATE
                    precioUnitarioListaPrecioDetalle = VALUES(precioUnitarioListaPrecioDetalle),
                    activoListaPrecioDetalle = 1;";
            const string actualizarProducto = @"
                UPDATE tblproducto
                SET precioVentaProducto = ?
                WHERE idProducto = ? AND estadoProducto = 1;";

            try
            {
                using (var conexion = new OdbcConnection(CadenaConexion))
                {
                    conexion.Open();
                    using (OdbcTransaction transaccion = conexion.BeginTransaction())
                    {
                        try
                        {
                            foreach (ClsPrecioCliente registro in registros)
                            {
                                bool esAumentoBase = string.Equals(registro.TipoModificacion, "AumentoBase", StringComparison.OrdinalIgnoreCase);

                                // El aumento por base modifica el precio de venta del producto, no el porcentaje.
                                if (esAumentoBase)
                                {
                                    using (var comando = new OdbcCommand(actualizarProducto, conexion, transaccion))
                                    {
                                        comando.Parameters.Add("precio", OdbcType.Decimal).Value = registro.PrecioBase;
                                        comando.Parameters.Add("producto", OdbcType.Int).Value = registro.IdProducto;
                                        if (comando.ExecuteNonQuery() == 0)
                                            throw new InvalidOperationException("No se pudo actualizar el precio base del producto " + registro.Producto + ". Puede que ya no esté activo.");
                                    }
                                }

                                if (registro.IdListaPrecio > 0)
                                {
                                    using (var comando = new OdbcCommand(guardarDetalle, conexion, transaccion))
                                    {
                                        comando.Parameters.Add("lista", OdbcType.Int).Value = registro.IdListaPrecio;
                                        comando.Parameters.Add("producto", OdbcType.Int).Value = registro.IdProducto;
                                        comando.Parameters.Add("precio", OdbcType.Decimal).Value = registro.PrecioFinal;
                                        comando.ExecuteNonQuery();
                                    }
                                }
                                else if (!esAumentoBase)
                                {
                                    throw new InvalidOperationException("No hay una lista de precios activa para guardar el descuento o aumento porcentual. El precio base no se modificó.");
                                }
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
                mensaje = "Se guardaron correctamente " + registros.Count + " registro(s) en dbsistemaembutidos.";
                return true;
            }
            catch (Exception error)
            {
                UsaBaseDeDatos = false;
                mensaje = "No se guardaron los cambios. La transacción fue cancelada: " + error.Message;
                return false;
            }
        }

        public static decimal VentasFuncCalcularPrecioFinal(decimal precioBase, decimal valorModificacion, string tipoModificacion)
        {
            if (precioBase < 0m) throw new ArgumentOutOfRangeException("precioBase", "El precio base no puede ser negativo.");
            if (valorModificacion < 0m) throw new ArgumentOutOfRangeException("valorModificacion", "El valor no puede ser negativo.");

            decimal precioFinal = precioBase;
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
