/*
 * Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: cálculos y validaciones de demostración, sin persistencia.
 */
using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CapaModelo_Contabilidad;

namespace CapaControlador_Contabilidad
{
    public class ClsControladorPrototiposContabilidad
    {
        private readonly ClsRepositorioPrototiposContabilidad _Repositorio = new ClsRepositorioPrototiposContabilidad();

        public string[] ContabilidadFuncModulos() { return _Repositorio.ContabilidadFuncModulos(); }
        public DataTable ContabilidadFuncPresupuesto() { return _Repositorio.ContabilidadFuncPresupuesto(); }
        public DataTable ContabilidadFuncVidaUtil() { return _Repositorio.ContabilidadFuncVidaUtil(); }

        public Dictionary<string, string> ContabilidadFuncErroresActivo(string Descripcion, string Centro, DateTime Adquisicion,
            DateTime Inicio, decimal Compra, decimal Residual, int Meses)
        {
            Dictionary<string, string> Errores = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(Descripcion)) Errores["Descripcion"] = "Ingrese una descripción.";
            if (string.IsNullOrWhiteSpace(Centro)) Errores["Centro"] = "Ingrese el centro de costo.";
            if (Inicio.Date < Adquisicion.Date) Errores["Inicio"] = "No puede ser anterior a la adquisición.";
            if (Compra <= 0m || Compra > 9999999999999.99m) Errores["Compra"] = "Ingrese un valor de compra válido mayor que cero.";
            if (Residual < 0m || Residual > Compra) Errores["Residual"] = "Debe estar entre cero y el valor de compra.";
            if (Meses <= 0) Errores["Meses"] = "La vida útil debe ser mayor que cero.";
            return Errores;
        }

        public Dictionary<string, string> ContabilidadFuncErroresPartida(string Nombre, string Modulo, decimal Cantidad, DataTable Detalle)
        {
            Dictionary<string, string> Errores = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(Nombre)) Errores["Nombre"] = "Ingrese el nombre de la partida.";
            if (!ContabilidadFuncModulos().Contains(Modulo)) Errores["Modulo"] = "Seleccione un módulo válido.";
            if (Cantidad <= 0m || Cantidad > 9999999999999.99m) Errores["Cantidad"] = "La asignación debe ser mayor que cero.";
            else if (ContabilidadFuncTotalPresupuesto(Detalle) + Cantidad > 9999999999999.99m)
                Errores["Cantidad"] = "El total excede DECIMAL(15,2).";
            return Errores;
        }

        public string ContabilidadFuncErrorFechas(DateTime Inicio, DateTime Fin)
        {
            return Fin.Date < Inicio.Date ? "La fecha final no puede ser anterior a la inicial." : string.Empty;
        }

        public decimal ContabilidadFuncRestante(DataTable Detalle, decimal Gastado)
        {
            return ContabilidadFuncTotalPresupuesto(Detalle) - Gastado;
        }

        public decimal ContabilidadFuncBaseDepreciable(decimal Compra, decimal Residual)
        {
            return Compra - Residual;
        }

        public decimal ContabilidadFuncTotalPresupuesto(DataTable Detalle)
        {
            decimal Total = 0m;
            foreach (DataRow Fila in Detalle.Rows) Total += Convert.ToDecimal(Fila["Cantidad"]);
            return Total;
        }

        public string ContabilidadFuncValidarPresupuesto(DateTime Inicio, DateTime? Cierre, DataTable Detalle)
        {
            if (Cierre.HasValue && Cierre.Value.Date < Inicio.Date) return "La fecha de cierre debe ser igual o posterior al inicio.";
            if (Detalle.Rows.Count == 0) return "Agregue al menos una partida al presupuesto.";
            foreach (DataRow Fila in Detalle.Rows)
            {
                if (string.IsNullOrWhiteSpace(Convert.ToString(Fila["Nombre"]))) return "Cada partida necesita un nombre.";
                decimal Cantidad = Convert.ToDecimal(Fila["Cantidad"]);
                if (Cantidad <= 0m || Cantidad > 9999999999999.99m) return "Las partidas deben tener importes positivos dentro de DECIMAL(15,2).";
            }
            if (ContabilidadFuncTotalPresupuesto(Detalle) > 9999999999999.99m) return "El total excede DECIMAL(15,2).";
            return string.Empty;
        }

        public string ContabilidadFuncValidarActivo(string Descripcion, string Centro, DateTime Adquisicion,
            DateTime Inicio, decimal Compra, decimal Residual, int Meses)
        {
            return ContabilidadFuncErroresActivo(Descripcion, Centro, Adquisicion, Inicio, Compra, Residual, Meses).Values.FirstOrDefault() ?? string.Empty;
        }

        public decimal ContabilidadFuncDepreciacion(decimal Compra, decimal Residual, int Meses)
        {
            if (Compra < 0m || Residual < 0m || Residual > Compra || Meses <= 0) return 0m;
            return decimal.Round((Compra - Residual) / Meses, 2, MidpointRounding.AwayFromZero);
        }

        public DataTable ContabilidadFuncPolizas(DateTime Periodo)
        {
            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Poliza", typeof(int));
            Resultado.Columns.Add("Fecha", typeof(DateTime));
            Resultado.Columns.Add("Estado", typeof(string));
            Resultado.Columns.Add("Debe", typeof(decimal));
            Resultado.Columns.Add("Haber", typeof(decimal));
            Resultado.Columns.Add("Diferencia", typeof(decimal));
            var Grupos = _Repositorio.ContabilidadFuncMovimientos().Rows.Cast<DataRow>()
                .Where(Fila => ((DateTime)Fila["Fecha"]).ToString("yyyy-MM", CultureInfo.InvariantCulture) == Periodo.ToString("yyyy-MM", CultureInfo.InvariantCulture))
                .GroupBy(Fila => (int)Fila["Poliza"]);
            foreach (var Grupo in Grupos)
            {
                DataRow Primera = Grupo.First();
                decimal Debe = Grupo.Sum(Fila => (decimal)Fila["Debe"]);
                decimal Haber = Grupo.Sum(Fila => (decimal)Fila["Haber"]);
                Resultado.Rows.Add(Grupo.Key, Primera["Fecha"], Primera["Estado"], Debe, Haber, Debe - Haber);
            }
            return Resultado;
        }

        public string ContabilidadFuncValidarCierre(DataTable Polizas)
        {
            if (Polizas.Rows.Count == 0) return "El período no tiene pólizas para revisar.";
            foreach (DataRow Fila in Polizas.Rows)
            {
                if ((string)Fila["Estado"] == "BORRADOR") return "Hay pólizas en borrador. Revise y aplique o anule antes del cierre.";
                if ((string)Fila["Estado"] == "APLICADA" && (decimal)Fila["Diferencia"] != 0m) return "Hay pólizas aplicadas descuadradas.";
            }
            if (!Polizas.Rows.Cast<DataRow>().Any(Fila => (string)Fila["Estado"] == "APLICADA")) return "El período no tiene pólizas aplicadas.";
            return string.Empty;
        }

        public DataTable ContabilidadFuncResultados(DateTime Desde, DateTime Hasta)
        {
            if (Hasta.Date < Desde.Date) throw new ArgumentException("La fecha final debe ser igual o posterior a la inicial.");
            DataTable Resultado = new DataTable();
            Resultado.Columns.Add("Rubro", typeof(string));
            Resultado.Columns.Add("Codigo", typeof(string));
            Resultado.Columns.Add("Cuenta", typeof(string));
            Resultado.Columns.Add("Importe", typeof(decimal));
            var Grupos = _Repositorio.ContabilidadFuncMovimientos().Rows.Cast<DataRow>()
                .Where(Fila => (string)Fila["Estado"] == "APLICADA" && (DateTime)Fila["Fecha"] >= Desde.Date
                    && (DateTime)Fila["Fecha"] <= Hasta.Date && new[] { "INGRESOS", "COSTOS", "GASTOS" }.Contains(ClsCodigoCuentaContabilidad.ContabilidadFuncClase((string)Fila["Codigo"])))
                .GroupBy(Fila => new { Rubro = ClsCodigoCuentaContabilidad.ContabilidadFuncClase((string)Fila["Codigo"]), Codigo = (string)Fila["Codigo"], Cuenta = (string)Fila["Cuenta"] });
            foreach (var Grupo in Grupos)
            {
                decimal Debe = Grupo.Sum(Fila => (decimal)Fila["Debe"]);
                decimal Haber = Grupo.Sum(Fila => (decimal)Fila["Haber"]);
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

        public ClsResumenCierreContabilidad ContabilidadFuncResumenCierre(DataTable Polizas)
        {
            var Filas = Polizas.Rows.Cast<DataRow>();
            var Aplicadas = Filas.Where(Fila => (string)Fila["Estado"] == "APLICADA").ToList();
            return new ClsResumenCierreContabilidad
            {
                Aplicadas = Aplicadas.Count, Borradores = Filas.Count(Fila => (string)Fila["Estado"] == "BORRADOR"),
                Debe = Aplicadas.Sum(Fila => (decimal)Fila["Debe"]), Haber = Aplicadas.Sum(Fila => (decimal)Fila["Haber"])
            };
        }
    }

    public sealed class ClsResumenCierreContabilidad
    {
        public int Aplicadas { get; set; }
        public int Borradores { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
    }
}
