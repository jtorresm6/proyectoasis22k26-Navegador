using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_DisponibilidadDiariaBancos.Entidades;
using CapaModelo_DisponibilidadDiariaBancos.Repositorio;

namespace CapaControlador_DisponibilidadDiariaBancos
{
    public class ClsControladorDisponibilidad
    {
        private readonly ClsRepositorioDisponibilidad _repositorio;

        public ClsControladorDisponibilidad()
        {
            _repositorio = new ClsRepositorioDisponibilidad();
        }

        // Catálogos 

        // Devuelve la lista de bancos para poblar el combo.
        public List<string> BancosMetObtenerBancos()
        {
            return _repositorio.BancosMetObtenerBancos();
        }

        // Devuelve las cuentas disponibles para el banco indicado
        public List<string> BancosMetObtenerCuentas(string banco)
        {
            return _repositorio.BancosMetObtenerCuentas(banco);
        }

        //  Consulta principal

        public DataTable BancosMetConsultarDisponibilidad(
            DateTime fechaCorte,
            string banco,
            string numeroCuenta,
            string moneda)
        {
            var lista = _repositorio.BancosMetObtenerDisponibilidad(
                fechaCorte, banco, numeroCuenta, moneda);

            var tabla = new DataTable();
            tabla.Columns.Add("Banco",          typeof(string));
            tabla.Columns.Add("Cuenta",         typeof(string));
            tabla.Columns.Add("Tipo de cuenta", typeof(string));
            tabla.Columns.Add("Moneda",         typeof(string));
            tabla.Columns.Add("Saldo Inicial",  typeof(decimal));
            tabla.Columns.Add("Ingresos",       typeof(decimal));
            tabla.Columns.Add("Egresos",        typeof(decimal));
            tabla.Columns.Add("Comprometido",   typeof(decimal));
            tabla.Columns.Add("Restringido",    typeof(decimal));
            tabla.Columns.Add("Disponible",     typeof(decimal));

            foreach (var d in lista)
            {
                tabla.Rows.Add(
                    d.Banco,
                    d.NumeroCuenta,
                    d.TipoCuenta ?? "Monetaria",
                    d.Moneda,
                    d.SaldoInicial,
                    d.Ingresos,
                    d.Egresos,
                    d.Comprometido,
                    d.Restringido,
                    d.Disponible);
            }

            return tabla;
        }


        // Calcula los indicadores de resumen para las filas de la moneda dada.
        //  Importante: no mezcla GTQ, USD y EUR en un único total.
      
        public (decimal SaldoInicial, decimal Ingresos, decimal Egresos,
                decimal Comprometido, decimal Restringido, decimal Disponible)
            BancosMetCalcularResumen(
                DateTime fechaCorte,
                string banco,
                string numeroCuenta,
                string monedaResumen)
        {
            var lista = _repositorio.BancosMetObtenerDisponibilidad(
                fechaCorte, banco, numeroCuenta, monedaResumen);

            decimal si = 0, ing = 0, eg = 0, comp = 0, rest = 0;
            foreach (var d in lista)
            {
                si   += d.SaldoInicial;
                ing  += d.Ingresos;
                eg   += d.Egresos;
                comp += d.Comprometido;
                rest += d.Restringido;
            }

            decimal disp = si + ing - eg - comp - rest;
            return (si, ing, eg, comp, rest, disp);
        }

        // Compromisos y Resumen de Cuenta 

     
        // Obtiene los compromisos de la cuenta indicada como DataTable.

        public DataTable BancosMetConsultarCompromisos(string numeroCuenta)
        {
            var lista = _repositorio.BancosMetObtenerCompromisos(numeroCuenta);
            var tabla = new DataTable();
            tabla.Columns.Add("Fecha", typeof(string));
            tabla.Columns.Add("Concepto", typeof(string));
            tabla.Columns.Add("Beneficiario", typeof(string));
            tabla.Columns.Add("Monto", typeof(decimal));

            foreach (var c in lista)
            {
                tabla.Rows.Add(c.Fecha.ToString("dd/MM/yyyy"), c.Concepto, c.Beneficiario, c.Monto);
            }

            return tabla;
        }


        // Obtiene el resumen de la cuenta indicada como DataTable.

        public DataTable BancosMetConsultarResumenCuenta(string numeroCuenta)
        {
            var lista = _repositorio.BancosMetObtenerResumenCuenta(numeroCuenta);
            var tabla = new DataTable();
            tabla.Columns.Add("Concepto", typeof(string));
            tabla.Columns.Add("Monto", typeof(decimal));

            foreach (var r in lista)
            {
                tabla.Rows.Add(r.Concepto, r.Monto);
            }

            return tabla;
        }
    }
}
