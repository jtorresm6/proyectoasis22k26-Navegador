/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 22/10/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Abrir la conexión de datos de pedidos.
 */
using System;
using System.Data;
using System.Data.Odbc;
using System.Globalization;

namespace CapaModelo_Pedidos
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Gestiona la conexión ODBC y las transacciones de Pedidos.
    internal sealed class ClsConexion : IDisposable
    {
        private readonly OdbcConnection _Conexion;
        private OdbcTransaction _Transaccion;

        internal ClsConexion()
        {
            _Conexion = new OdbcConnection(CapaModelo_Ventas.ClsConexion.CadenaConexion);
            try { _Conexion.Open(); }
            catch { _Conexion.Dispose(); throw; }
        }

        internal void VentasMetIniciar()
        {
            _Transaccion = _Conexion.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        internal void VentasMetConfirmar()
        {
            _Transaccion.Commit();
            _Transaccion.Dispose();
            _Transaccion = null;
        }

        private OdbcCommand VentasFuncComando(string Sql, object[] Valores)
        {
            if (Sql.Split('?').Length - 1 != Valores.Length)
                throw new ArgumentException("Revise los parámetros SQL.");

            var Comando = new OdbcCommand(Sql, _Conexion, _Transaccion) { CommandTimeout = 20 };
            foreach (object Valor in Valores)
            {
                var Parametro = new OdbcParameter();
                if (Valor is decimal)
                {
                    Parametro.OdbcType = OdbcType.VarChar;
                    Parametro.Value = ((decimal)Valor).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    if (Valor is DateTime) Parametro.OdbcType = OdbcType.DateTime;
                    Parametro.Value = Valor ?? DBNull.Value;
                }
                Comando.Parameters.Add(Parametro);
            }
            return Comando;
        }

        internal DataTable VentasFuncTabla(string Sql, params object[] Valores)
        {
            using (var Comando = VentasFuncComando(Sql, Valores))
            using (var Adaptador = new OdbcDataAdapter(Comando))
            {
                var Tabla = new DataTable();
                Adaptador.Fill(Tabla);
                return Tabla;
            }
        }

        internal object VentasFuncEscalar(string Sql, params object[] Valores)
        {
            using (var Comando = VentasFuncComando(Sql, Valores))
                return Comando.ExecuteScalar();
        }

        internal int VentasFuncEjecutar(string Sql, params object[] Valores)
        {
            using (var Comando = VentasFuncComando(Sql, Valores))
                return Comando.ExecuteNonQuery();
        }

        internal int VentasFuncObtenerId()
        {
            return Convert.ToInt32(VentasFuncEscalar("SELECT LAST_INSERT_ID()"));
        }

        public void Dispose()
        {
            if (_Transaccion != null)
            {
                try { _Transaccion.Rollback(); }
                catch (OdbcException) { }
                _Transaccion.Dispose();
            }
            _Conexion.Dispose();
        }
    }
}
