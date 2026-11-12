using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaModelo_CierreMovimientos.Entidades;
using CapaModelo_CierreMovimientos.Servicios;
using CapaModelo_Componentes.Repositorios;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 24/10/2026 (prototipo) - 04/11/2026 (datos reales y cierre)
 * ==================================================================
 * Propósito :
 *  Controlador del cierre de movimientos. Obtiene los períodos abiertos, arma
 *  la vista previa del cierre (validaciones, saldos por proveedor, antigüedad
 *  de saldos y totales, que también alimentan el reporte) y ejecuta el cierre
 *  con el usuario y la IP de la sesión de Seguridad.
 *  La lógica de negocio está en CapaModelo_CierreMovimientos.
 * ===================================================================
*/

namespace CapaControlador_CierreMovimientos
{
    public class ClsResumenCierre
    {
        public DataTable Validaciones { get; set; }
        public DataTable SaldosProveedor { get; set; }
        public DataTable Antiguedad { get; set; }
        public DateTime FechaCorte { get; set; }
        public decimal TotalCargos { get; set; }
        public decimal TotalAbonos { get; set; }
        public decimal SaldoFinal { get; set; }
        public bool PuedeCerrar { get; set; }
        public string Periodo { get; set; }
    }

    public class ClsCierreMovimientos
    {
        private readonly ClsServicioCierreMovimientos _Servicio;

        public ClsCierreMovimientos() : this(new ClsServicioCierreMovimientos()) { }

        public ClsCierreMovimientos(ClsServicioCierreMovimientos Servicio)
        {
            _Servicio = Servicio ?? throw new ArgumentNullException(nameof(Servicio));
        }

        public DataTable ComprasFuncObtenerPeriodosCerrables()
        {
            List<ClsPeriodoCompras> Periodos = ComprasFuncEjecutar(() => _Servicio.ComprasFuncObtenerPeriodosAbiertos());
            var Tabla = new DataTable();
            Tabla.Columns.Add("cmp_id_periodo", typeof(int));
            Tabla.Columns.Add("cmp_descripcion", typeof(string));
            foreach (ClsPeriodoCompras Periodo in Periodos) Tabla.Rows.Add(Periodo.IdPeriodo, Periodo.Descripcion);
            return Tabla;
        }

        public ClsResumenCierre ComprasFuncPrepararCierre(int IdPeriodo)
        {
            ClsPreparacionCierre Preparacion = ComprasFuncEjecutar(() =>
                _Servicio.ComprasFuncPrepararCierre(IdPeriodo, ClsSesionSeguridad.HaySesionActiva, DateTime.Today));

            var Validaciones = new DataTable();
            Validaciones.Columns.Add("cmp_estado", typeof(string));
            Validaciones.Columns.Add("cmp_validacion", typeof(string));
            Validaciones.Columns.Add("cmp_origen", typeof(string));
            foreach (ClsValidacionCierre Validacion in Preparacion.Validaciones.OrderByDescending(Fila => Fila.Estado))
                Validaciones.Rows.Add(Validacion.Estado.ToString(), Validacion.Validacion, Validacion.Origen);

            var Saldos = new DataTable();
            Saldos.Columns.Add("cmp_proveedor", typeof(string));
            Saldos.Columns.Add("cmp_saldo_inicial", typeof(decimal));
            Saldos.Columns.Add("cmp_total_cargos", typeof(decimal));
            Saldos.Columns.Add("cmp_total_abonos", typeof(decimal));
            Saldos.Columns.Add("cmp_saldo_final", typeof(decimal));
            foreach (ClsSaldoProveedorCierre Saldo in Preparacion.Saldos)
                Saldos.Rows.Add(Saldo.NombreProveedor, Saldo.SaldoInicial, Saldo.Cargos, Saldo.Abonos, Saldo.SaldoFinal);

            var Antiguedad = new DataTable();
            Antiguedad.Columns.Add("cmp_proveedor", typeof(string));
            Antiguedad.Columns.Add("cmp_rango", typeof(string));
            Antiguedad.Columns.Add("cmp_documentos", typeof(int));
            Antiguedad.Columns.Add("cmp_monto", typeof(decimal));
            foreach (ClsAntiguedadCierre Fila in Preparacion.Antiguedad)
                Antiguedad.Rows.Add(Fila.NombreProveedor, Fila.NombreRango, Fila.CantidadDocumentos, Fila.Monto);

            return new ClsResumenCierre
            {
                Validaciones = Validaciones,
                SaldosProveedor = Saldos,
                Antiguedad = Antiguedad,
                FechaCorte = Preparacion.Periodo.FechaFin,
                TotalCargos = Preparacion.Saldos.Sum(Saldo => Saldo.Cargos),
                TotalAbonos = Preparacion.Saldos.Sum(Saldo => Saldo.Abonos),
                SaldoFinal = Preparacion.Saldos.Sum(Saldo => Saldo.SaldoFinal),
                PuedeCerrar = Preparacion.PuedeCerrar,
                Periodo = Preparacion.Periodo.Descripcion
            };
        }

        public string ComprasFuncEjecutarCierre(int IdPeriodo)
        {
            int? IdUsuario = ClsSesionSeguridad.HaySesionActiva ? ClsSesionSeguridad.IdUsuario : (int?)null;
            string Ip = ClsSesionSeguridad.SeguridadMetObtenerIPLocal();

            ClsResultadoCierre Resultado = ComprasFuncEjecutar(() =>
                _Servicio.ComprasFuncEjecutarCierre(IdPeriodo, IdUsuario, Ip, DateTime.Today));

            return "Se cerró " + Resultado.Periodo.Descripcion + "." + Environment.NewLine + Environment.NewLine +
                "Proveedores: " + Resultado.Proveedores + Environment.NewLine +
                "Cargos: Q " + Resultado.TotalCargos.ToString("N2") + Environment.NewLine +
                "Abonos: Q " + Resultado.TotalAbonos.ToString("N2") + Environment.NewLine +
                "Saldo final: Q " + Resultado.SaldoFinal.ToString("N2") + Environment.NewLine +
                "Documentos bloqueados: " + Resultado.DocumentosBloqueados + Environment.NewLine + Environment.NewLine +
                "Período abierto: " + Resultado.SiguientePeriodo + ".";
        }

        private static T ComprasFuncEjecutar<T>(Func<T> Operacion)
        {
            try
            {
                return Operacion();
            }
            catch (ClsExcepcionDatosCompras Error)
            {
                throw new InvalidOperationException(Error.Message, Error);
            }
        }
    }
}
