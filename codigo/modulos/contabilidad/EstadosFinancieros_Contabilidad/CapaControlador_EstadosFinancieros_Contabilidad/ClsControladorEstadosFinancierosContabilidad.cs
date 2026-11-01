/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Reglas del estado de resultados: toma las pólizas validadas y aplicadas del
 *  rango, clasifica cada cuenta por su código y calcula los totales
 *  de ingresos, costos, gastos y el resultado neto.
 * Reglas especificas:
 *  Ingresos = haber menos debe. Costos y gastos = debe menos haber.
 *  Se excluyen borradores, anuladas y cuentas de balance.
 * ===================================================================
*/

using System;
using System.Data;
using System.Linq;
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Repositorios;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaControlador_Contabilidad
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsControladorEstadosFinancierosContabilidad
    {
        private readonly IRepositorioPolizasContabilidad _Repositorio = new ClsRepositorioPolizasContabilidad();

        public string ContabilidadFuncErrorFechas(DateTime Inicio, DateTime Fin)
        {
            return ClsValidacionDatosContabilidad.ContabilidadFuncErrorRangoFechas(Inicio, Fin);
        }

        public DataTable ContabilidadFuncResultados(DateTime Desde, DateTime Hasta)
        {
            if (Hasta.Date < Desde.Date) throw new ArgumentException("La fecha final debe ser igual o posterior a la inicial.");
            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Rubro", typeof(string));
            Resultado.Columns.Add("Codigo", typeof(string));
            Resultado.Columns.Add("Cuenta", typeof(string));
            Resultado.Columns.Add("Importe", typeof(decimal));
            string[] Rubros = { "INGRESOS", "COSTOS", "GASTOS" };

            var Lineas = _Repositorio.ContabilidadFuncObtenerTodos()
                .Where(Poliza => (Poliza.Estado == "VALIDADA" || Poliza.Estado == "APLICADA") && Poliza.Fecha >= Desde.Date && Poliza.Fecha <= Hasta.Date)
                .SelectMany(Poliza => Poliza.Detalle)
                .Where(Linea => Rubros.Contains(ClsCodigoCuentaContabilidad.ContabilidadFuncClase(Linea.Codigo)));
            var Grupos = Lineas.GroupBy(Linea => new { Rubro = ClsCodigoCuentaContabilidad.ContabilidadFuncClase(Linea.Codigo), Linea.Codigo, Linea.Cuenta });
            foreach (var Grupo in Grupos)
            {
                decimal Debe = Grupo.Sum(Linea => Linea.Debe);
                decimal Haber = Grupo.Sum(Linea => Linea.Haber);
                Resultado.Rows.Add(Grupo.Key.Rubro, Grupo.Key.Codigo, Grupo.Key.Cuenta, Grupo.Key.Rubro == "INGRESOS" ? Haber - Debe : Debe - Haber);
            }
            return Resultado;
        }

        public decimal ContabilidadFuncTotalRubro(DataTable Resultado, string Rubro)
        {
            return Resultado.Rows.Cast<DataRow>().Where(Fila => (string)Fila["Rubro"] == Rubro).Sum(Fila => (decimal)Fila["Importe"]);
        }

        public decimal ContabilidadFuncResultadoNeto(DataTable Resultado)
        {
            return ContabilidadFuncTotalRubro(Resultado, "INGRESOS") - ContabilidadFuncTotalRubro(Resultado, "COSTOS") - ContabilidadFuncTotalRubro(Resultado, "GASTOS");
        }
    }
}
