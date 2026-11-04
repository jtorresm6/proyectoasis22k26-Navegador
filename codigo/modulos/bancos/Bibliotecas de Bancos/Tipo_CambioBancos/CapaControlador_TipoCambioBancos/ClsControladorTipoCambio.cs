using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_TipoCambio.Entidades;
using CapaModelo_TipoCambio.Repositorio;

namespace CapaControlador_TipoCambio
{
    public class ClsControladorTipoCambio
    {
        private readonly ClsRepositorioTipoCambio _repositorio;

        public ClsControladorTipoCambio()
        {
            _repositorio = new ClsRepositorioTipoCambio();
        }

        // ==========================================
        // TASAS ACTUALES
        // ==========================================

        public DataTable BancosMetObtenerTasasActuales(bool incluirSinTasa = true)
        {
            var lista = _repositorio.BancosMetObtenerTasasActuales(incluirSinTasa);
            var tabla = new DataTable();
            tabla.Columns.Add("Código",      typeof(string));
            tabla.Columns.Add("Moneda",      typeof(string));
            tabla.Columns.Add("Tasa en GTQ", typeof(string));
            tabla.Columns.Add("Fecha",       typeof(string));
            tabla.Columns.Add("Estado",      typeof(string));

            foreach (var t in lista)
            {
                string fechaStr = t.Estado == "Sin tasa" ? "—" : t.FechaTasa.ToString("dd/MM/yyyy");
                string estadoSimbolo = t.Estado == "Sin tasa" ? "●  Sin tasa" : "●  Disponible";
                tabla.Rows.Add(
                    t.CodigoMoneda,
                    t.NombreMoneda,
                    t.TasaTexto,
                    fechaStr,
                    estadoSimbolo);
            }
            return tabla;
        }

        public string BancosMetActualizarDesdeBanguat()
        {
            return "Actualización completada en modo demostración.\n" +
                   "Tasas sincronizadas para la fecha de consulta.";
        }

        // ==========================================
        // HISTÓRICO
        // ==========================================

        public DataTable BancosMetObtenerHistorico(
            DateTime desde,
            DateTime hasta,
            string monedaOrigen,
            string monedaDestino)
        {
            var lista = _repositorio.BancosMetObtenerHistorico(
                desde, hasta, monedaOrigen, monedaDestino);

            var tabla = new DataTable();
            tabla.Columns.Add("Fecha",          typeof(string));
            tabla.Columns.Add("Moneda origen",  typeof(string));
            tabla.Columns.Add("Moneda destino", typeof(string));
            tabla.Columns.Add("Tasa",           typeof(string));
            tabla.Columns.Add("Fuente",         typeof(string));
            tabla.Columns.Add("Estado",         typeof(string));

            foreach (var t in lista)
            {
                string estadoSimbolo = t.Estado == "Sin tasa" ? "●  Sin tasa" : "●  Disponible";
                tabla.Rows.Add(
                    t.FechaTasa.ToString("dd/MM/yyyy"),
                    t.MonedaOrigen,
                    t.MonedaDestino,
                    t.Tasa.ToString("F6"),
                    t.Fuente,
                    estadoSimbolo);
            }
            return tabla;
        }

        public (decimal Minima, decimal Maxima, decimal Variacion) BancosMetObtenerResumenHistorico()
        {
            return _repositorio.BancosMetObtenerResumenHistorico();
        }

        // ==========================================
        // CONVERSOR
        // ==========================================

        public (decimal Resultado, decimal TasaAplicada, string Mensaje)
            BancosMetConvertir(
                decimal importe,
                string monedaOrigen,
                string monedaDestino)
        {
            if (monedaOrigen == monedaDestino)
                return (importe, 1.000000m, "Misma moneda — tasa = 1.000000");

            // Si es EUR -> GTQ
            if (monedaOrigen == "EUR" && monedaDestino == "GTQ")
            {
                decimal tasa = 8.421500m;
                decimal res = Math.Round(importe * tasa, 2);
                return (res, tasa, $"1 EUR = {tasa:F6} GTQ");
            }

            // Si es USD -> GTQ
            if (monedaOrigen == "USD" && monedaDestino == "GTQ")
            {
                decimal tasa = 7.753200m;
                decimal res = Math.Round(importe * tasa, 2);
                return (res, tasa, $"1 USD = {tasa:F6} GTQ");
            }

            // Buscar en repositorio
            var tasaObj = _repositorio.BancosMetObtenerTasaActual(monedaOrigen, monedaDestino);
            if (tasaObj != null && tasaObj.Tasa > 0)
            {
                decimal res = Math.Round(importe * tasaObj.Tasa, 2);
                return (res, tasaObj.Tasa, $"1 {monedaOrigen} = {tasaObj.Tasa:F6} {monedaDestino}");
            }

            // Inversa si destino es GTQ
            if (monedaOrigen == "GTQ")
            {
                var directa = _repositorio.BancosMetObtenerTasaActual(monedaDestino, "GTQ");
                if (directa != null && directa.Tasa > 0)
                {
                    decimal tasaInv = Math.Round(1m / directa.Tasa, 6);
                    decimal res = Math.Round(importe * tasaInv, 2);
                    return (res, tasaInv, $"1 GTQ = {tasaInv:F6} {monedaDestino}");
                }
            }

            return (0m, 0m, $"No se encontró tasa demo para {monedaOrigen}/{monedaDestino}.");
        }
    }
}
