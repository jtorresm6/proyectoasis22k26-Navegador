using System;
using System.Collections.Generic;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 29/10/2026
 * ==================================================================
 * Propósito :
 *  Objetos que transportan monedas, conversiones e historial de tipo de cambio
 *  entre el controlador de herramientas y las pantallas de Compras.
 * ===================================================================
*/

namespace CapaControlador_Herramientas
{
    public class ClsMonedaCompras
    {
        public ClsMonedaCompras(long IdMoneda, string Codigo, string Nombre, string Simbolo)
        {
            this.IdMoneda = IdMoneda;
            this.Codigo = Codigo;
            this.Nombre = Nombre;
            this.Simbolo = Simbolo;
        }

        public long IdMoneda { get; }
        public string Codigo { get; }
        public string Nombre { get; }
        public string Simbolo { get; }
        public string Descripcion => Codigo + " - " + Nombre + " (" + Simbolo + ")";

        public override string ToString() => Descripcion;
    }

    public class ClsResultadoConversion
    {
        public bool Exitoso { get; internal set; }
        public string Mensaje { get; internal set; }
        public decimal MontoOrigen { get; internal set; }
        public decimal MontoConvertido { get; internal set; }
        public decimal TipoCambio { get; internal set; }
        public string Metodo { get; internal set; }
        public DateTime? FechaTipoCambio { get; internal set; }
        public string Fuente { get; internal set; }
        public ClsMonedaCompras MonedaOrigen { get; internal set; }
        public ClsMonedaCompras MonedaDestino { get; internal set; }
    }

    public class ClsPuntoTipoCambio
    {
        public ClsPuntoTipoCambio(DateTime Fecha, decimal Valor)
        {
            this.Fecha = Fecha;
            this.Valor = Valor;
        }

        public DateTime Fecha { get; }
        public decimal Valor { get; }
    }

    public class ClsHistorialTipoCambio
    {
        public List<ClsPuntoTipoCambio> Puntos { get; } = new List<ClsPuntoTipoCambio>();
        public bool EsInverso { get; internal set; }
        public decimal Minimo { get; internal set; }
        public decimal Maximo { get; internal set; }
        public decimal Promedio { get; internal set; }
        public decimal VariacionPorcentual { get; internal set; }
        public bool TieneDatos => Puntos.Count > 0;
    }
}
