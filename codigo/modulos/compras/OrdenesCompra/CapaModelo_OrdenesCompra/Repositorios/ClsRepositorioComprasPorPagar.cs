using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_Componentes.Repositorios;
using CapaModelo_OrdenesCompra.Contratos;
using CapaModelo_OrdenesCompra.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Ejecuta las consultas y operaciones de compras por pagar mediante
 *  acceso a datos parametrizado y transacciones cuando corresponde.
 * ===================================================================
*/

namespace CapaModelo_OrdenesCompra.Repositorios
{
    public sealed class ClsRepositorioComprasPorPagar : ClsRepositorioMaestroCompras, IRepositorioComprasPorPagar
    {
        private const string _Consulta = "SELECT c.idCuentaPorPagar, c.idProveedor, c.numeroFacturaCuentaPorPagar, "
            + "c.fechaEmisionCuentaPorPagar, c.fechaVencimientoCuentaPorPagar, c.montoTotalCuentaPorPagar, "
            + "c.saldoCuentaPorPagar, c.estadoCuentaPorPagar, p.nombreProveedor, p.nitProveedor, "
            + "p.telefonoProveedor, p.correoProveedor, p.direccionProveedor, p.estadoProveedor "
            + "FROM tblCuentaPorPagar c LEFT JOIN tblProveedor p ON p.idProveedor = c.idProveedor ";

        public DataTable ComprasFuncListar()
        {
            return ComprasFuncConsultar(_Consulta + "ORDER BY c.fechaEmisionCuentaPorPagar DESC, c.idCuentaPorPagar DESC", null);
        }

        public ClsCompraPorPagar ComprasFuncObtener(int Id)
        {
            if (Id <= 0) throw new ArgumentOutOfRangeException(nameof(Id));
            using (DataTable Datos = ComprasFuncConsultar(_Consulta + "WHERE c.idCuentaPorPagar = ?", Id))
            {
                return Datos.Rows.Count == 0 ? null : new ClsCompraPorPagar(Datos.Rows[0]);
            }
        }

        public DataTable ComprasFuncListarProveedores()
        {
            return ComprasFuncConsultar("SELECT * FROM tblProveedor WHERE estadoProveedor = 1 ORDER BY nombreProveedor", null);
        }

        public void ComprasProcAnular(int Id)
        {
            if (Id <= 0) throw new ArgumentOutOfRangeException(nameof(Id));
            ComprasFuncEjecucionNonQuery("UPDATE tblCuentaPorPagar SET estadoCuentaPorPagar = 'Anulada' WHERE idCuentaPorPagar = ? AND estadoCuentaPorPagar <> 'Anulada'",
                new List<OdbcParameter> { new OdbcParameter("Id", OdbcType.Int) { Value = Id } }, CommandType.Text);
        }

        public void ComprasProcActualizarVencimiento(int Id, DateTime Fecha)
        {
            if (Id <= 0) throw new ArgumentOutOfRangeException(nameof(Id));
            ComprasFuncEjecucionNonQuery("UPDATE tblCuentaPorPagar SET fechaVencimientoCuentaPorPagar = ? WHERE idCuentaPorPagar = ? AND fechaEmisionCuentaPorPagar <= ?",
                new List<OdbcParameter>
                {
                    new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date },
                    new OdbcParameter("Id", OdbcType.Int) { Value = Id },
                    new OdbcParameter("Emision", OdbcType.Date) { Value = Fecha.Date }
                }, CommandType.Text);
        }

        public int ComprasFuncCrear(int IdProveedor, string Factura, DateTime Emision, DateTime Vencimiento, decimal Monto)
        {
            const string Sql = "INSERT INTO tblCuentaPorPagar (idProveedor, numeroFacturaCuentaPorPagar, "
                + "fechaEmisionCuentaPorPagar, fechaVencimientoCuentaPorPagar, montoTotalCuentaPorPagar, "
                + "saldoCuentaPorPagar, estadoCuentaPorPagar) "
                + "SELECT idProveedor, ?, ?, ?, ?, ?, 'Pendiente' FROM tblProveedor "
                + "WHERE idProveedor = ? AND estadoProveedor = 1";
            return ComprasFuncEjecutar((Conexion, Transaccion) =>
            {
                var Parametros = new List<OdbcParameter>
                {
                    new OdbcParameter("Factura", OdbcType.VarChar, 30) { Value = Factura },
                    new OdbcParameter("Emision", OdbcType.Date) { Value = Emision.Date },
                    new OdbcParameter("Vencimiento", OdbcType.Date) { Value = Vencimiento.Date }
                };
                foreach (string Campo in new[] { "Monto", "Saldo" })
                {
                    Parametros.Add(new OdbcParameter(Campo, OdbcType.Decimal)
                    {
                        Precision = 12, Scale = 4, Value = Monto
                    });
                }
                Parametros.Add(new OdbcParameter("IdProveedor", OdbcType.Int) { Value = IdProveedor });
                using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, Sql, CommandType.Text, Parametros))
                {
                    if (Comando.ExecuteNonQuery() != 1)
                        throw new InvalidOperationException("El proveedor no está disponible o está inactivo.");
                    Comando.Parameters.Clear();
                    Comando.CommandText = "SELECT LAST_INSERT_ID()";
                    return Convert.ToInt32(Comando.ExecuteScalar());
                }
            }, true);
        }

        private DataTable ComprasFuncConsultar(string Sql, int? Id)
        {
            var Parametros = new List<OdbcParameter>();
            if (Id.HasValue) Parametros.Add(new OdbcParameter("IdCuentaPorPagar", OdbcType.Int) { Value = Id.Value });
            return ComprasFuncEjecucionConsulta(Sql, CommandType.Text, Parametros);
        }
    }
}
