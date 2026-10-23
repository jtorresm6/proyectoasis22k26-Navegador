using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using CapaModelo_ListadoPrecios.Entidades;
using CapaModelo_ListadoPrecios.Contratos;

namespace CapaModelo_ListadoPrecios
{

    // Acceso a datos. Detecta una tabla existente por los nombres de columnas conocidos;
    // si el DSN o el esquema del equipo aún no está disponible, usa datos de demostración.
    public class ClsModeloListadoPrecios : IRepositorioListadoPrecios
    {
        private static string CadenaConexion { get { return CapaModelo_Ventas.ClsConexion.CadenaConexion; } }
        private static readonly List<ClsPrecioCliente> _DatosDemostracion = new List<ClsPrecioCliente>
        {
            new ClsPrecioCliente { TipoCliente = "Mayorista", TipoProducto = "Producto A", PorcentajeDescuento = 12m, PrecioBase = 100m, PrecioFinal = 88m },
            new ClsPrecioCliente { TipoCliente = "Minorista", TipoProducto = "Producto A", PorcentajeDescuento = 5m, PrecioBase = 100m, PrecioFinal = 95m },
            new ClsPrecioCliente { TipoCliente = "Público", TipoProducto = "Producto B", PorcentajeDescuento = 0m, PrecioBase = 75m, PrecioFinal = 75m }
        };
        private static readonly object _Bloqueo = new object();
        private ClsTablaPrecio _tablaDetectada;
        public bool UsaBaseDeDatos { get; private set; }
        public string MensajeEstado { get; private set; }

        public ClsModeloListadoPrecios()
        {
            MensajeEstado = "Modo demostración: no se ha confirmado una tabla de precios compatible.";
        }

        public List<ClsPrecioCliente> ListadoPreciosMetObtenerListado()
        {
            try
            {
                using (OdbcConnection Conexion = new OdbcConnection(CadenaConexion))
                {
                    Conexion.Open();
                    _tablaDetectada = ListadoPreciosMetDetectarTabla(Conexion);
                    if (_tablaDetectada != null)
                    {
                        List<ClsPrecioCliente> Datos = ListadoPreciosMetConsultarTabla(Conexion, _tablaDetectada);
                        UsaBaseDeDatos = true;
                        MensajeEstado = "Listado cargado desde la base de datos.";
                        return Datos;
                    }
                    MensajeEstado = "Conexión disponible, pero no se encontró una tabla con columnas de listado de precios reconocibles. Se muestran datos de demostración.";
                }
            }
            catch (Exception Excepcion)
            {
                MensajeEstado = "Modo demostración: no fue posible consultar la conexión configurada en Ventas (" + Excepcion.Message + ").";
            }
            UsaBaseDeDatos = false;
            lock (_Bloqueo) return _DatosDemostracion.Select(ListadoPreciosMetClonar).ToList();
        }

        public bool ListadoPreciosMetGuardar(ClsPrecioCliente RegistroPrecio, out string Mensaje)
        {
            if (RegistroPrecio == null) { Mensaje = "No se recibió información para guardar."; return false; }
            RegistroPrecio.PrecioFinal = ListadoPreciosMetCalcularPrecioFinal(RegistroPrecio.PrecioBase, RegistroPrecio.PorcentajeDescuento);
            try
            {
                using (OdbcConnection Conexion = new OdbcConnection(CadenaConexion))
                {
                    Conexion.Open();
                    ClsTablaPrecio NombreTablaSql = _tablaDetectada ?? ListadoPreciosMetDetectarTabla(Conexion);
                    if (NombreTablaSql != null)
                    {
                        ListadoPreciosMetGuardarEnTabla(Conexion, NombreTablaSql, RegistroPrecio);
                        _tablaDetectada = NombreTablaSql;
                        UsaBaseDeDatos = true;
                        MensajeEstado = "Cambios guardados en la base de datos.";
                        Mensaje = MensajeEstado;
                        return true;
                    }
                }
            }
            catch (Exception Excepcion)
            {
                // Si nunca se detectó una tabla compatible, permite la prueba de interfaz sin fingir persistencia.
                if (_tablaDetectada != null)
                {
                    Mensaje = "No se pudo guardar en la tabla detectada: " + Excepcion.Message;
                    return false;
                }
                lock (_Bloqueo)
                {
                    ClsPrecioCliente RegistroExistente = _DatosDemostracion.FirstOrDefault(PrecioClienteActual =>
                        ListadoPreciosMetIgual(PrecioClienteActual.TipoCliente, RegistroPrecio.TipoCliente) && ListadoPreciosMetIgual(PrecioClienteActual.TipoProducto, RegistroPrecio.TipoProducto));
                    if (RegistroExistente == null) _DatosDemostracion.Add(ListadoPreciosMetClonar(RegistroPrecio));
                    else
                    {
                        RegistroExistente.PorcentajeDescuento = RegistroPrecio.PorcentajeDescuento;
                        RegistroExistente.PrecioBase = RegistroPrecio.PrecioBase;
                        RegistroExistente.PrecioFinal = RegistroPrecio.PrecioFinal;
                    }
                }
                UsaBaseDeDatos = false;
                MensajeEstado = "Guardado solo en memoria de demostración; la conexión ODBC no está disponible (" + Excepcion.Message + ").";
                Mensaje = MensajeEstado;
                return true;
            }
            // Fallback explícito para permitir probar el formulario mientras se configura el esquema.
            lock (_Bloqueo)
            {
                ClsPrecioCliente RegistroExistente = _DatosDemostracion.FirstOrDefault(PrecioClienteActual =>
                    ListadoPreciosMetIgual(PrecioClienteActual.TipoCliente, RegistroPrecio.TipoCliente) && ListadoPreciosMetIgual(PrecioClienteActual.TipoProducto, RegistroPrecio.TipoProducto));
                if (RegistroExistente == null) _DatosDemostracion.Add(ListadoPreciosMetClonar(RegistroPrecio));
                else
                {
                    RegistroExistente.PorcentajeDescuento = RegistroPrecio.PorcentajeDescuento;
                    RegistroExistente.PrecioBase = RegistroPrecio.PrecioBase;
                    RegistroExistente.PrecioFinal = RegistroPrecio.PrecioFinal;
                }
            }
            UsaBaseDeDatos = false;
            MensajeEstado = "Guardado solo en memoria de demostración; no se persistió en la base de datos.";
            Mensaje = MensajeEstado;
            return true;
        }

        public static decimal ListadoPreciosMetCalcularPrecioFinal(decimal PrecioBase, decimal PorcentajeDescuento)
        {
            return Math.Round(PrecioBase * (1m - PorcentajeDescuento / 100m), 2, MidpointRounding.AwayFromZero);
        }

        private static ClsTablaPrecio ListadoPreciosMetDetectarTabla(OdbcConnection Conexion)
        {
            DataTable Tablas = Conexion.GetSchema("Tables");
            foreach (DataRow Fila in Tablas.Rows)
            {
                string TipoTabla = Convert.ToString(Fila["TABLE_TYPE"], CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(TipoTabla) && !TipoTabla.Equals("TABLE", StringComparison.OrdinalIgnoreCase) && !TipoTabla.Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase)) continue;
                string NombreTabla = Convert.ToString(Fila["TABLE_NAME"], CultureInfo.InvariantCulture);
                string Esquema = Tablas.Columns.Contains("TABLE_SCHEM") ? Convert.ToString(Fila["TABLE_SCHEM"], CultureInfo.InvariantCulture) : null;
                if (string.IsNullOrWhiteSpace(NombreTabla)) continue;
                DataTable Columnas;
                try { Columnas = Conexion.GetSchema("Columns", new string[] { null, Esquema, NombreTabla, null }); }
                catch { continue; }
                Dictionary<string, string> Mapa = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (DataRow Columna in Columnas.Rows)
                {
                    string Campo = Convert.ToString(Columna["COLUMN_NAME"], CultureInfo.InvariantCulture);
                    if (!string.IsNullOrWhiteSpace(Campo)) Mapa[Campo] = Campo;
                }
                string Cliente = ListadoPreciosMetBuscarColumna(Mapa, "tipo_cliente", "tipocliente", "clasificacion_cliente", "categoria_cliente", "cliente_tipo");
                string Producto = ListadoPreciosMetBuscarColumna(Mapa, "tipo_producto", "tipoproducto", "nombre_producto", "producto", "descripcion_producto");
                string PorcentajeDescuento = ListadoPreciosMetBuscarColumna(Mapa, "porcentaje_descuento", "porcentajedescuento", "descuento_porcentaje", "descuento", "porcentaje");
                string Precio = ListadoPreciosMetBuscarColumna(Mapa, "precio_base", "preciobase", "precio_unitario", "precio", "price");
                string PrecioFinal = ListadoPreciosMetBuscarColumna(Mapa, "precio_final", "preciofinal", "precio_con_descuento");
                if (Cliente != null && Producto != null && PorcentajeDescuento != null && Precio != null)
                    return new ClsTablaPrecio { Esquema = Esquema, Nombre = NombreTabla, ColumnaCliente = Cliente, ColumnaProducto = Producto, ColumnaDescuento = PorcentajeDescuento, ColumnaBase = Precio, ColumnaFinal = PrecioFinal };
            }
            return null;
        }

        private static string ListadoPreciosMetBuscarColumna(Dictionary<string, string> Mapa, params string[] Nombres)
        {
            foreach (string Candidato in Nombres)
                foreach (KeyValuePair<string, string> Campo in Mapa)
                    if (ListadoPreciosMetNormalizar(Campo.Key) == ListadoPreciosMetNormalizar(Candidato)) return Campo.Value;
            return null;
        }
        private static string ListadoPreciosMetNormalizar(string Valor) { return new string((Valor ?? "").Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray()); }
        private static string ListadoPreciosMetCitarIdentificador(string Identificador) { return "\"" + (Identificador ?? "").Replace("\"", "\"\"") + "\""; }
        private static string ListadoPreciosMetConstruirTablaSql(ClsTablaPrecio Tabla) { return string.IsNullOrWhiteSpace(Tabla.Esquema) ? ListadoPreciosMetCitarIdentificador(Tabla.Nombre) : ListadoPreciosMetCitarIdentificador(Tabla.Esquema) + "." + ListadoPreciosMetCitarIdentificador(Tabla.Nombre); }

        private static List<ClsPrecioCliente> ListadoPreciosMetConsultarTabla(OdbcConnection Conexion, ClsTablaPrecio Tabla)
        {
            string Sql = "SELECT " + ListadoPreciosMetCitarIdentificador(Tabla.ColumnaCliente) + "," + ListadoPreciosMetCitarIdentificador(Tabla.ColumnaProducto) + "," + ListadoPreciosMetCitarIdentificador(Tabla.ColumnaDescuento) + "," + ListadoPreciosMetCitarIdentificador(Tabla.ColumnaBase) +
                (Tabla.ColumnaFinal == null ? "" : "," + ListadoPreciosMetCitarIdentificador(Tabla.ColumnaFinal)) + " FROM " + ListadoPreciosMetConstruirTablaSql(Tabla);
            List<ClsPrecioCliente> Resultado = new List<ClsPrecioCliente>();
            using (OdbcCommand Comando = new OdbcCommand(Sql, Conexion))
            using (OdbcDataReader Lector = Comando.ExecuteReader())
            {
                while (Lector.Read())
                {
                    decimal PorcentajeDescuento = ListadoPreciosMetDecimalSeguro(Lector[2]);
                    decimal PrecioBase = ListadoPreciosMetDecimalSeguro(Lector[3]);
                    decimal PrecioFinal = Tabla.ColumnaFinal == null || Lector.IsDBNull(4) ? ListadoPreciosMetCalcularPrecioFinal(PrecioBase, PorcentajeDescuento) : ListadoPreciosMetDecimalSeguro(Lector[4]);
                    Resultado.Add(new ClsPrecioCliente { TipoCliente = Convert.ToString(Lector[0]), TipoProducto = Convert.ToString(Lector[1]), PorcentajeDescuento = PorcentajeDescuento, PrecioBase = PrecioBase, PrecioFinal = PrecioFinal });
                }
            }
            return Resultado;
        }

        private static void ListadoPreciosMetGuardarEnTabla(OdbcConnection Conexion, ClsTablaPrecio Tabla, ClsPrecioCliente RegistroPrecio)
        {
            string NombreTablaSql = ListadoPreciosMetConstruirTablaSql(Tabla);
            string SqlBuscar = "SELECT COUNT(*) FROM " + NombreTablaSql + " WHERE " + ListadoPreciosMetCitarIdentificador(Tabla.ColumnaCliente) + "=? AND " + ListadoPreciosMetCitarIdentificador(Tabla.ColumnaProducto) + "=?";
            bool Existe;
            using (OdbcCommand Comando = new OdbcCommand(SqlBuscar, Conexion))
            {
                Comando.Parameters.Add("p1", OdbcType.VarChar).Value = RegistroPrecio.TipoCliente;
                Comando.Parameters.Add("p2", OdbcType.VarChar).Value = RegistroPrecio.TipoProducto;
                Existe = Convert.ToInt32(Comando.ExecuteScalar(), CultureInfo.InvariantCulture) > 0;
            }
            List<string> Campos = new List<string> { Tabla.ColumnaDescuento, Tabla.ColumnaBase };
            List<object> Valores = new List<object> { RegistroPrecio.PorcentajeDescuento, RegistroPrecio.PrecioBase };
            if (Tabla.ColumnaFinal != null) { Campos.Add(Tabla.ColumnaFinal); Valores.Add(RegistroPrecio.PrecioFinal); }
            string Sql;
            if (Existe)
                Sql = "UPDATE " + NombreTablaSql + " SET " + string.Join(",", Campos.Select(Columna => ListadoPreciosMetCitarIdentificador(Columna) + "=?")) + " WHERE " + ListadoPreciosMetCitarIdentificador(Tabla.ColumnaCliente) + "=? AND " + ListadoPreciosMetCitarIdentificador(Tabla.ColumnaProducto) + "=?";
            else
            {
                Campos.Insert(0, Tabla.ColumnaProducto); Campos.Insert(0, Tabla.ColumnaCliente);
                Valores.Insert(0, RegistroPrecio.TipoProducto); Valores.Insert(0, RegistroPrecio.TipoCliente);
                Sql = "INSERT INTO " + NombreTablaSql + " (" + string.Join(",", Campos.Select(ListadoPreciosMetCitarIdentificador)) + ") VALUES (" + string.Join(",", Campos.Select(Columna => "?")) + ")";
            }
            using (OdbcCommand Comando = new OdbcCommand(Sql, Conexion))
            {
                foreach (object Valor in Valores) Comando.Parameters.Add("p", Valor is decimal ? OdbcType.Decimal : OdbcType.VarChar).Value = Valor;
                if (Existe)
                {
                    Comando.Parameters.Add("pCliente", OdbcType.VarChar).Value = RegistroPrecio.TipoCliente;
                    Comando.Parameters.Add("pProducto", OdbcType.VarChar).Value = RegistroPrecio.TipoProducto;
                }
                Comando.ExecuteNonQuery();
            }
        }

        private static decimal ListadoPreciosMetDecimalSeguro(object Valor)
        {
            if (Valor == null || Valor == DBNull.Value) return 0m;
            decimal ValorDecimal;
            if (decimal.TryParse(Convert.ToString(Valor, CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out ValorDecimal)) return ValorDecimal;
            if (decimal.TryParse(Convert.ToString(Valor), NumberStyles.Any, CultureInfo.GetCultureInfo("es-GT"), out ValorDecimal)) return ValorDecimal;
            return 0m;
        }
        private static bool ListadoPreciosMetIgual(string ValorA, string ValorB) { return string.Equals(ValorA, ValorB, StringComparison.OrdinalIgnoreCase); }
        private static ClsPrecioCliente ListadoPreciosMetClonar(ClsPrecioCliente PrecioClienteActual)
        { return new ClsPrecioCliente { TipoCliente = PrecioClienteActual.TipoCliente, TipoProducto = PrecioClienteActual.TipoProducto, PorcentajeDescuento = PrecioClienteActual.PorcentajeDescuento, PrecioBase = PrecioClienteActual.PrecioBase, PrecioFinal = PrecioClienteActual.PrecioFinal }; }

        private sealed class ClsTablaPrecio
        {
            public string Esquema, Nombre, ColumnaCliente, ColumnaProducto, ColumnaDescuento, ColumnaBase, ColumnaFinal;
        }
    }
}
