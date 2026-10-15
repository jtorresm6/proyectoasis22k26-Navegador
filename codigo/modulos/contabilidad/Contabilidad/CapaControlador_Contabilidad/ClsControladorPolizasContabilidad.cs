/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Reglas de la póliza que no dependen de la pantalla: período
 *  contable a partir de la fecha, totales del debe y del haber,
 *  diferencia, y validación antes de guardar o aplicar.
 * Reglas especificas:
 *  Una póliza es válida si tiene descripción, al menos dos líneas,
 *  cada línea tiene importe solo en el debe o solo en el haber, y el
 *  total del debe es igual al del haber y mayor que cero.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using CapaModelo_Contabilidad;

namespace CapaControlador_Contabilidad
{
    public class ClsControladorPolizasContabilidad
    {
        private readonly ClsRepositorioPolizasContabilidad _Repositorio = new ClsRepositorioPolizasContabilidad();

        public List<ClsCuentaPolizaContabilidad> ContabilidadFuncObtenerCuentas()
        {
            List<ClsCuentaPolizaContabilidad> Cuentas = _Repositorio.ContabilidadFuncObtenerCuentas()
                .Select(Cuenta => new ClsCuentaPolizaContabilidad { IdCuenta = Cuenta.IdCuenta, Codigo = Cuenta.Codigo, Nombre = Cuenta.Nombre })
                .ToList();
            if (Cuentas.Any(Cuenta => !ClsCodigoCuentaContabilidad.ContabilidadFuncEsValido(Cuenta.Codigo)) ||
                Cuentas.GroupBy(Cuenta => Cuenta.Codigo).Any(Grupo => Grupo.Count() > 1))
                throw new InvalidOperationException("El catálogo contiene códigos contables inválidos o duplicados.");
            return Cuentas;
        }

        public string[] ContabilidadFuncObtenerEstados()
        {
            return _Repositorio.ContabilidadFuncObtenerEstados();
        }

        public string[] ContabilidadFuncObtenerOrigenes()
        {
            return _Repositorio.ContabilidadFuncObtenerOrigenes();
        }

        // Formato YYYY-MM de poliza_encabezado.periodo_contable
        public string ContabilidadFuncObtenerPeriodo(DateTime Fecha)
        {
            return Fecha.ToString("yyyy-MM");
        }

        public decimal ContabilidadFuncTotalDebe(IEnumerable<ClsLineaPolizaContabilidad> Lineas)
        {
            return Lineas.Sum(Linea => Linea.Debe);
        }

        public decimal ContabilidadFuncTotalHaber(IEnumerable<ClsLineaPolizaContabilidad> Lineas)
        {
            return Lineas.Sum(Linea => Linea.Haber);
        }

        public bool ContabilidadFuncEstaCuadrada(IEnumerable<ClsLineaPolizaContabilidad> Lineas)
        {
            decimal Debe = ContabilidadFuncTotalDebe(Lineas);
            return Debe > 0 && Debe == ContabilidadFuncTotalHaber(Lineas);
        }

        // Devuelve "" si la póliza es válida; si no, el motivo en español.
        public string ContabilidadFuncValidar(string Descripcion, IList<ClsLineaPolizaContabilidad> Lineas)
        {
            if (string.IsNullOrWhiteSpace(Descripcion)) return "Escriba la descripción de la póliza.";
            if (Lineas.Count < 2) return "La póliza necesita al menos dos cuentas.";
            foreach (ClsLineaPolizaContabilidad Linea in Lineas)
            {
                if (Linea.Debe < 0 || Linea.Haber < 0)
                    return "Los importes no pueden ser negativos (" + Linea.Cuenta + ").";
                if ((Linea.Debe > 0) == (Linea.Haber > 0))
                    return "Cada cuenta debe tener importe solo en el debe o solo en el haber (" + Linea.Cuenta + ").";
            }
            if (!ContabilidadFuncEstaCuadrada(Lineas))
                return "La póliza no cuadra: el total del debe debe ser igual al del haber.";
            return "";
        }
    }
}
