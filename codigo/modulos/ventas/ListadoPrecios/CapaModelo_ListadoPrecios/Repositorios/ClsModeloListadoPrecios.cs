using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using CapaModelo_ListadoPrecios.Contratos;
using CapaModelo_ListadoPrecios.Entidades;

namespace CapaModelo_ListadoPrecios
{
    // El listado usa las listas y productos del esquema de Embutidos.
    public class ClsModeloListadoPrecios : IRepositorioListadoPrecios
    {
        private static string CadenaConexion { get { return CapaModelo_Ventas.ClsConexion.CadenaConexion; } }

        public bool UsaBaseDeDatos { get; private set; }
        public string MensajeEstado { get; private set; }

        public ClsModeloListadoPrecios()
        {
            MensajeEstado = "Aún no se ha consultado la base de datos.";
        }

        public List<ClsPrecioCliente> VentasFuncObtenerListado()
        {
            var Resultado = new List<ClsPrecioCliente>();
            const string Sql = @"SELECT CONCAT(l.codigoListaPrecioEncabezado,' - ',l.nombreListaPrecioEncabezado) Lista,
    CONCAT(p.skuProducto,' - ',p.nombreProducto) Producto,
    p.precioVentaProducto BasePrecio,COALESCE(d.precioUnitarioListaPrecioDetalle,p.precioVentaProducto) PrecioFinal
    FROM tblListaPrecioEncabezado l
    CROSS JOIN tblProductoVenta pv
    JOIN tblProducto p ON p.Pk_Producto=pv.Pk_ProductoVenta
    LEFT JOIN tblListaPrecioDetalle d ON d.Fk_ListaPrecioEncabezado_Codigo=l.Pk_ListaPrecioEncabezado
      AND d.Fk_Producto_Codigo=p.Pk_Producto AND d.activoListaPrecioDetalle=1
    WHERE l.activoListaPrecioEncabezado=1 AND pv.activoProductoVenta=1
    ORDER BY l.codigoListaPrecioEncabezado,p.skuProducto";
            try
            {
                using (var Conexion = new OdbcConnection(CadenaConexion))
                {
                    Conexion.Open();
                    using (var Comando = new OdbcCommand(Sql, Conexion))
                    using (var Lector = Comando.ExecuteReader())
                        while (Lector.Read())
                        {
                            decimal Base = Convert.ToDecimal(Lector[2], CultureInfo.InvariantCulture);
                            decimal Final = Convert.ToDecimal(Lector[3], CultureInfo.InvariantCulture);
                            Resultado.Add(new ClsPrecioCliente
                            {
                                TipoCliente = Convert.ToString(Lector[0], CultureInfo.InvariantCulture),
                                TipoProducto = Convert.ToString(Lector[1], CultureInfo.InvariantCulture),
                                PrecioBase = Base,
                                PrecioFinal = Final,
                                PorcentajeDescuento = Base > 0 ? Math.Round((1m - Final / Base) * 100m, 2) : 0m
                            });
                        }
                }
                UsaBaseDeDatos = true;
                MensajeEstado = "Listado cargado desde Embutidos.";
            }
            catch (OdbcException Error)
            {
                UsaBaseDeDatos = false;
                MensajeEstado = "No se pudo consultar Embutidos: " + Error.Message;
                throw;
            }
            return Resultado;
        }

        public bool VentasFuncGuardar(ClsPrecioCliente Registro, out string Mensaje)
        {
            if (Registro == null || string.IsNullOrWhiteSpace(Registro.TipoCliente) ||
                string.IsNullOrWhiteSpace(Registro.TipoProducto) || Registro.PorcentajeDescuento < 0 ||
                Registro.PorcentajeDescuento > 100)
            {
                Mensaje = "Seleccione una lista y un producto válidos.";
                return false;
            }

            const string Buscar = @"SELECT l.Pk_ListaPrecioEncabezado,p.Pk_Producto,p.precioVentaProducto
    FROM tblListaPrecioEncabezado l CROSS JOIN tblProducto p
    WHERE CONCAT(l.codigoListaPrecioEncabezado,' - ',l.nombreListaPrecioEncabezado)=?
      AND CONCAT(p.skuProducto,' - ',p.nombreProducto)=?
      AND l.activoListaPrecioEncabezado=1";
            const string Guardar = @"INSERT INTO tblListaPrecioDetalle(Fk_ListaPrecioEncabezado_Codigo,Fk_Producto_Codigo,precioUnitarioListaPrecioDetalle,activoListaPrecioDetalle)
    VALUES (?,?,?,1) ON DUPLICATE KEY UPDATE precioUnitarioListaPrecioDetalle=VALUES(precioUnitarioListaPrecioDetalle),activoListaPrecioDetalle=1";
            try
            {
                using (var Conexion = new OdbcConnection(CadenaConexion))
                {
                    Conexion.Open();
                    int Lista, Producto;
                    decimal Base;
                    using (var Comando = new OdbcCommand(Buscar, Conexion))
                    {
                        Comando.Parameters.Add("lista", OdbcType.VarChar, 200).Value = Registro.TipoCliente;
                        Comando.Parameters.Add("producto", OdbcType.VarChar, 200).Value = Registro.TipoProducto;
                        using (var Lector = Comando.ExecuteReader())
                        {
                            if (!Lector.Read())
                            {
                                Mensaje = "La lista o el producto no existe en Embutidos.";
                                return false;
                            }
                            Lista = Convert.ToInt32(Lector[0], CultureInfo.InvariantCulture);
                            Producto = Convert.ToInt32(Lector[1], CultureInfo.InvariantCulture);
                            Base = Convert.ToDecimal(Lector[2], CultureInfo.InvariantCulture);
                        }
                    }
                    decimal Precio = VentasFuncCalcularPrecioFinal(Base, Registro.PorcentajeDescuento);
                    using (var Comando = new OdbcCommand(Guardar, Conexion))
                    {
                        Comando.Parameters.Add("lista", OdbcType.Int).Value = Lista;
                        Comando.Parameters.Add("producto", OdbcType.Int).Value = Producto;
                        Comando.Parameters.Add("precio", OdbcType.VarChar, 64).Value = Precio.ToString(CultureInfo.InvariantCulture);
                        Comando.ExecuteNonQuery();
                    }
                }
                UsaBaseDeDatos = true;
                MensajeEstado = Mensaje = "Precio guardado en Embutidos.";
                return true;
            }
            catch (OdbcException Error)
            {
                UsaBaseDeDatos = false;
                MensajeEstado = Mensaje = "No se pudo guardar en Embutidos: " + Error.Message;
                return false;
            }
        }

        public static decimal VentasFuncCalcularPrecioFinal(decimal PrecioBase, decimal Descuento)
        {
            return Math.Round(PrecioBase * (1m - Descuento / 100m), 2, MidpointRounding.AwayFromZero);
        }
    }
}
