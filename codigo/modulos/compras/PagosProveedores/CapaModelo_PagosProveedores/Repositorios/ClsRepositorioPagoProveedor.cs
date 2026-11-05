using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_PagosProveedores.Contratos;
using CapaModelo_PagosProveedores.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 20/10/2026
 * ==================================================================
 * Propósito :
 *  Ejecuta los procedimientos almacenados de pagos a proveedores y
 *  convierte sus resultados en entidades y datos para las consultas.
 * ===================================================================
*/

namespace CapaModelo_PagosProveedores.Repositorios
{
   
    public class ClsRepositorioPagoProveedor : ClsSentenciasCompras, IRepositorioPagoProveedor
    {
        private readonly string _Insert;
        private readonly string _Update;
        private readonly string _Delete;
        private readonly string _SelectFiltro;
        private readonly string _SelectUno;
        private readonly string _SelectProveedores;
        private readonly string _SelectCuentas;

        public ClsRepositorioPagoProveedor()
        {
            _Insert = "{call spInsertarPagoProveedor(?,?,?,?,?,?)}";
            _Update = "{call spActualizarPagoProveedor(?,?,?,?,?,?)}";
            _Delete = "{call spEliminarPagoProveedor(?)}";
            _SelectFiltro = "{call spConsultarPagoProveedor(?,?,?,?)}";
            _SelectUno = "{call spObtenerPagoProveedor(?)}";
            _SelectProveedores = "{call spObtenerProveedor()}";
            _SelectCuentas = "{call spObtenerCuentaPorPagarPendiente(?)}";
        }

        
        private static OdbcParameter ComprasFuncInt(string Nombre, int? Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.Int) { Value = (object)Valor ?? DBNull.Value };
        }
        private static OdbcParameter ComprasFuncFecha(string Nombre, DateTime? Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.Date) { Value = Valor.HasValue ? (object)Valor.Value.Date : DBNull.Value };
        }
        private static OdbcParameter ComprasFuncDecimal(string Nombre, decimal Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.Decimal) { Value = Valor };
        }
        private static OdbcParameter ComprasFuncTexto(string Nombre, string Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.VarChar, 30)
            { Value = string.IsNullOrWhiteSpace(Valor) ? (object)DBNull.Value : Valor };
        }

        

        
        public int ComprasMetAgregar(ClsPagoProveedor Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(ComprasFuncInt("p_cuentaPorPagar", Entidad.IdCuentaPorPagar));
            Parametros.Add(ComprasFuncFecha("p_fecha", Entidad.FechaPagoProveedor));
            Parametros.Add(ComprasFuncDecimal("p_monto", Entidad.MontoPagoProveedor));
            Parametros.Add(ComprasFuncTexto("p_metodo", Entidad.MetodoPagoProveedor));
            Parametros.Add(ComprasFuncTexto("p_referencia", Entidad.ReferenciaPagoProveedor));
            Parametros.Add(ComprasFuncTexto("p_estado", Entidad.EstadoPagoProveedor));

            return Convert.ToInt32(ComprasMetEjecucionEscalar(_Insert, Parametros, CommandType.Text));
        }

        public int ComprasMetEditar(ClsPagoProveedor Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(ComprasFuncInt("p_idPago", Entidad.IdPagoProveedor));
            Parametros.Add(ComprasFuncFecha("p_fecha", Entidad.FechaPagoProveedor));
            Parametros.Add(ComprasFuncDecimal("p_monto", Entidad.MontoPagoProveedor));
            Parametros.Add(ComprasFuncTexto("p_metodo", Entidad.MetodoPagoProveedor));
            Parametros.Add(ComprasFuncTexto("p_referencia", Entidad.ReferenciaPagoProveedor));
            Parametros.Add(ComprasFuncTexto("p_estado", Entidad.EstadoPagoProveedor));

            ComprasMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
            return 1; 
        }

        public int ComprasMetRemover(ClsPagoProveedor Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(ComprasFuncInt("p_idPago", Entidad.IdPagoProveedor));

            ComprasMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
            return 1;
        }

        public IEnumerable<ClsPagoProveedor> ComprasMetObtenerTodos()
        {
            return ComprasMetConsultar(null, null, null, null);
        }

        

        public IEnumerable<ClsPagoProveedor> ComprasMetConsultar(int? IdProveedor, string Estado, DateTime? Desde, DateTime? Hasta)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(ComprasFuncInt("p_proveedor", IdProveedor));
            Parametros.Add(ComprasFuncTexto("p_estado", Estado));
            Parametros.Add(ComprasFuncFecha("p_desde", Desde));
            Parametros.Add(ComprasFuncFecha("p_hasta", Hasta));

            var Lista = new List<ClsPagoProveedor>();
            var TablaDatos = ComprasMetEjecucionConsulta(_SelectFiltro, CommandType.Text, Parametros);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Pago = new ClsPagoProveedor();
                Pago.IdPagoProveedor = Convert.ToInt32(Fila["idPago"]);
                Pago.NoPagoProveedor = Fila["noPago"].ToString();
                Pago.NombreProveedor = Fila["proveedor"].ToString();
                Pago.FacturaCuentaPorPagar = Fila["factura"].ToString();
                Pago.FechaPagoProveedor = Convert.ToDateTime(Fila["fecha"]);
                Pago.MontoPagoProveedor = Convert.ToDecimal(Fila["monto"]);
                Pago.EstadoPagoProveedor = Fila["estado"].ToString();
                Lista.Add(Pago);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return Lista;
        }

        public ClsPagoProveedor ComprasMetObtenerPorId(int IdPago)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(ComprasFuncInt("p_idPago", IdPago));

            var TablaDatos = ComprasMetEjecucionConsulta(_SelectUno, CommandType.Text, Parametros);
            if (TablaDatos.Rows.Count == 0) return null;

            DataRow Fila = TablaDatos.Rows[0];
            var Pago = new ClsPagoProveedor();
            Pago.IdPagoProveedor = Convert.ToInt32(Fila["idPago"]);
            Pago.IdProveedor = Convert.ToInt32(Fila["idProveedor"]);
            Pago.IdCuentaPorPagar = Convert.ToInt32(Fila["idCuentaPorPagar"]);
            Pago.FacturaCuentaPorPagar = Fila["factura"].ToString();
            Pago.FechaPagoProveedor = Convert.ToDateTime(Fila["fecha"]);
            Pago.MontoPagoProveedor = Convert.ToDecimal(Fila["monto"]);
            Pago.MetodoPagoProveedor = Fila["metodo"].ToString();
            Pago.ReferenciaPagoProveedor = Fila["referencia"] == DBNull.Value ? null : Fila["referencia"].ToString();
            Pago.EstadoPagoProveedor = Fila["estado"].ToString();
            return Pago;
        }

        
        public DataTable ComprasMetObtenerProveedores()
        {
            return ComprasMetEjecucionConsulta(_SelectProveedores, CommandType.Text);
        }

        
        public DataTable ComprasMetObtenerCuentasPendientes(int IdProveedor)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(ComprasFuncInt("p_proveedor", IdProveedor));
            return ComprasMetEjecucionConsulta(_SelectCuentas, CommandType.Text, Parametros);
        }
    }
}
