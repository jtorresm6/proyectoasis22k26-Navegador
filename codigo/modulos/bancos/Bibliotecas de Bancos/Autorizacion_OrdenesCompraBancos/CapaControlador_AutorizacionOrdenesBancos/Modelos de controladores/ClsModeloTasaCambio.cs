using System;

/*
 * ==========================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 13/11/2026
 * ==========================================================================
 * Propósito :
 *  La clase Modelo de Tasa de Cambio pertenece a la capa controlador. Guarda la
 *  tasa consultada (valor, fecha, fuente y método) y su identificador en
 *  tblTipoCambio, y normaliza el método al formato que exige la base de datos.
 * ==========================================================================
*/

namespace CapaControlador_AutorizacionOrdenes.Modelos_de_controladores
{
    public class ClsModeloTasaCambio
    {
        private decimal _Tasa;
        private DateTime _Fecha;
        private string _Fuente;
        private string _Metodo;
        private long? _IdTipoCambio;

        public decimal Tasa { get => _Tasa; set => _Tasa = value; }
        public DateTime Fecha { get => _Fecha; set => _Fecha = value; }
        public string Fuente { get => _Fuente; set => _Fuente = value; }
        public string Metodo { get => _Metodo; set => _Metodo = value; }
        public long? IdTipoCambio { get => _IdTipoCambio; set => _IdTipoCambio = value; }

        // La tabla solo acepta DIRECTA, INVERSA o CRUZADA; el módulo de Tipo de Cambio
        // puede devolver textos como "INVERSA (DIRECTA)" o "CRUZADA (promedio)".
        public static string BancosMetNormalizarMetodo(string Metodo)
        {
            string Texto = (Metodo ?? string.Empty).Trim().ToUpperInvariant();
            if (Texto.StartsWith("DIRECTA"))
            {
                return "DIRECTA";
            }
            if (Texto.StartsWith("INVERSA"))
            {
                return "INVERSA";
            }
            return "CRUZADA";
        }
    }
}
