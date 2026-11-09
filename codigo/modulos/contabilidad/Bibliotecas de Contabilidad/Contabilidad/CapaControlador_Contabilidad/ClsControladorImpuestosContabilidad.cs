/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 30/10/2026
 * ==================================================================
 * Propósito :
 *  Consulta de impuestos para los demás módulos: la lista de
 *  impuestos, los de una operación, el valor de uno y el cálculo
 *  sobre un monto.
 * Reglas especificas:
 *  El impuesto se busca por su nombre. La operación COMPRA o VENTA
 *  también trae los impuestos de AMBAS. Sin tramos, el impuesto es el
 *  monto por el valor; con tramos, es el importe fijo del tramo más su
 *  tasa sobre lo que excede el inicio del tramo. Se redondea a dos
 *  decimales.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using CapaModelo_Contabilidad.Entidades;
using CapaModelo_Contabilidad.Repositorios;

namespace CapaControlador_Contabilidad
{
    public class ClsControladorImpuestosContabilidad
    {
        private readonly ClsRepositorioImpuestosContabilidad _Repositorio = new ClsRepositorioImpuestosContabilidad();

        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 02/11/2026
        public List<ClsModeloImpuestoContabilidad> ContabilidadFuncImpuestos()
        {
            return _Repositorio.ContabilidadFuncObtenerImpuestos().Select(ContabilidadFuncModelo).ToList();
        }

        public List<ClsModeloImpuestoContabilidad> ContabilidadFuncImpuestos(string Operacion)
        {
            string Buscada = (Operacion ?? string.Empty).Trim().ToUpperInvariant();
            return _Repositorio.ContabilidadFuncObtenerImpuestos()
                .Where(Impuesto => Impuesto.Operacion == Buscada || (Impuesto.Operacion == "AMBAS" && (Buscada == "COMPRA" || Buscada == "VENTA")))
                .Select(ContabilidadFuncModelo).ToList();
        }

        public decimal ContabilidadFuncValor(string Nombre)
        {
            return ContabilidadFuncBuscar(Nombre).Valor;
        }

        public decimal ContabilidadFuncCalcular(string Nombre, decimal Monto)
        {
            return ContabilidadFuncCalcular(ContabilidadFuncBuscar(Nombre), Monto);
        }

        public static decimal ContabilidadFuncCalcular(ClsImpuestoContabilidad Impuesto, decimal Monto)
        {
            if (Monto <= 0m) return 0m;
            if (Impuesto.Tramos.Count == 0) return Math.Round(Monto * Impuesto.Valor, 2, MidpointRounding.AwayFromZero);
            ClsImpuestoTramoContabilidad Tramo = Impuesto.Tramos.OrderBy(Fila => Fila.MontoDesde)
                .Last(Fila => Fila.MontoDesde == 0m || Monto > Fila.MontoDesde);
            return Math.Round(Tramo.ImporteFijo + (Monto - Tramo.MontoDesde) * Tramo.Tasa, 2, MidpointRounding.AwayFromZero);
        }

        private ClsImpuestoContabilidad ContabilidadFuncBuscar(string Nombre)
        {
            string Buscado = (Nombre ?? string.Empty).Trim();
            ClsImpuestoContabilidad Impuesto = _Repositorio.ContabilidadFuncObtenerImpuestos()
                .FirstOrDefault(Fila => string.Equals(Fila.Nombre, Buscado, StringComparison.OrdinalIgnoreCase));
            if (Impuesto == null) throw new InvalidOperationException("No hay un impuesto activo llamado " + Buscado + ".");
            return Impuesto;
        }

        private static ClsModeloImpuestoContabilidad ContabilidadFuncModelo(ClsImpuestoContabilidad Impuesto)
        {
            return new ClsModeloImpuestoContabilidad
            {
                Nombre = Impuesto.Nombre, Descripcion = Impuesto.Descripcion, Tipo = Impuesto.Tipo, Operacion = Impuesto.Operacion,
                Valor = Impuesto.Valor, Periodicidad = Impuesto.Periodicidad, CuentaAcreditable = Impuesto.CuentaAcreditable,
                CuentaPorPagar = Impuesto.CuentaPorPagar,
                Tramos = Impuesto.Tramos.Select(Tramo => new ClsModeloTramoImpuestoContabilidad
                    { MontoDesde = Tramo.MontoDesde, MontoHasta = Tramo.MontoHasta, Tasa = Tramo.Tasa, ImporteFijo = Tramo.ImporteFijo }).ToList()
            };
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 02/11/2026
    }
}
