using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using CapaModelo_Componentes.Repositorios;
using CapaModelo_Herramientas.Repositorios;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 29/10/2026
 * ==================================================================
 * Propósito :
 *  Reglas de negocio de las herramientas de moneda de Compras: consulta de tipos
 *  de cambio vigentes, conversión de montos entre monedas, historial con sus
 *  estadísticas y formato de montos en la moneda de visualización.
 * Reglas específicas:
 *  - Las monedas y tipos de cambio los administra Bancos; aquí solo se consultan.
 *  - Para convertir se busca, en este orden: tipo de cambio directo, inverso
 *    (1 / valor) y cruzado a través de la moneda local.
 *  - Los tipos de cambio se manejan con 4 decimales.
 * ===================================================================
*/

namespace CapaControlador_Herramientas
{
    public class ClsControladorMoneda
    {
        private const string _CodigoMonedaLocal = "GTQ";
        private const string _CodigoMonedaSugerida = "USD";
        private const int _DecimalesTipoCambio = 4;
        private const int _DecimalesMonto = 2;
        private const decimal _MontoMaximo = 99999999.99m;

        private readonly ClsRepositorioMoneda _Repositorio;

        public ClsControladorMoneda() : this(new ClsRepositorioMoneda())
        {
        }

        public ClsControladorMoneda(ClsRepositorioMoneda Repositorio)
        {
            _Repositorio = Repositorio;
        }

        public List<ClsMonedaCompras> ComprasFuncObtenerMonedas()
        {
            DataTable Datos = ComprasFuncEjecutar(() => _Repositorio.ComprasFuncObtenerMonedasActivas());
            return Datos.AsEnumerable().Select(Fila => new ClsMonedaCompras(
                Convert.ToInt64(Fila["idMoneda"]),
                Convert.ToString(Fila["codigoMoneda"]),
                Convert.ToString(Fila["nombreMoneda"]),
                Convert.ToString(Fila["simboloMoneda"]))).ToList();
        }

        public ClsMonedaCompras ComprasFuncObtenerMonedaLocal(IEnumerable<ClsMonedaCompras> Monedas)
        {
            return Monedas.FirstOrDefault(Moneda =>
                string.Equals(Moneda.Codigo, _CodigoMonedaLocal, StringComparison.OrdinalIgnoreCase));
        }

        public ClsMonedaCompras ComprasFuncObtenerMonedaExtranjeraSugerida(IEnumerable<ClsMonedaCompras> Monedas)
        {
            // Para Compras la moneda extranjera más usada con proveedores es el dólar; si no existe, se toma la primera distinta a la local.
            List<ClsMonedaCompras> Extranjeras = Monedas.Where(Moneda =>
                !string.Equals(Moneda.Codigo, _CodigoMonedaLocal, StringComparison.OrdinalIgnoreCase)).ToList();
            return Extranjeras.FirstOrDefault(Moneda =>
                string.Equals(Moneda.Codigo, _CodigoMonedaSugerida, StringComparison.OrdinalIgnoreCase)) ?? Extranjeras.FirstOrDefault();
        }

        public DataTable ComprasFuncObtenerTiposCambioVigentes(DateTime Fecha)
        {
            return ComprasFuncEjecutar(() => _Repositorio.ComprasFuncObtenerTiposCambioVigentes(Fecha));
        }

        public ClsResultadoConversion ComprasFuncConvertir(string MontoTexto, ClsMonedaCompras Origen,
            ClsMonedaCompras Destino, DateTime Fecha)
        {
            var Resultado = new ClsResultadoConversion { MonedaOrigen = Origen, MonedaDestino = Destino };

            if (!ComprasFuncLeerMonto(MontoTexto, out decimal Monto))
                return ComprasFuncFallo(Resultado, "Ingrese un monto numérico mayor a cero (máximo 99,999,999.99).");
            if (Origen == null || Destino == null)
                return ComprasFuncFallo(Resultado, "Seleccione la moneda de origen y la de destino.");

            return ComprasFuncConvertir(Monto, Origen, Destino, Fecha);
        }

        public ClsResultadoConversion ComprasFuncConvertir(decimal Monto, ClsMonedaCompras Origen,
            ClsMonedaCompras Destino, DateTime Fecha)
        {
            var Resultado = new ClsResultadoConversion { MonedaOrigen = Origen, MonedaDestino = Destino, MontoOrigen = Monto };

            ClsTasa Tasa = ComprasFuncBuscarTasa(Origen, Destino, Fecha.Date);
            if (Tasa == null)
                return ComprasFuncFallo(Resultado, "Bancos no tiene registrado un tipo de cambio entre " + Origen.Codigo +
                    " y " + Destino.Codigo + " al " + Fecha.ToString("dd/MM/yyyy") + ".");

            Resultado.Exitoso = true;
            Resultado.TipoCambio = Math.Round(Tasa.Valor, _DecimalesTipoCambio);
            Resultado.MontoConvertido = Math.Round(Monto * Tasa.Valor, _DecimalesMonto, MidpointRounding.AwayFromZero);
            Resultado.Metodo = Tasa.Metodo;
            Resultado.FechaTipoCambio = Tasa.Fecha;
            Resultado.Fuente = Tasa.Fuente;
            Resultado.Mensaje = "1 " + Origen.Codigo + " = " + ComprasFuncFormatearTasa(Resultado.TipoCambio) + " " + Destino.Codigo;
            return Resultado;
        }

        public ClsHistorialTipoCambio ComprasFuncObtenerHistorial(ClsMonedaCompras Origen, ClsMonedaCompras Destino,
            DateTime Desde, DateTime Hasta)
        {
            if (Origen == null || Destino == null || Origen.IdMoneda == Destino.IdMoneda)
                throw new InvalidOperationException("Seleccione dos monedas distintas para ver el historial.");
            if (Desde.Date > Hasta.Date)
                throw new InvalidOperationException("La fecha inicial no puede ser mayor que la fecha final.");

            var Historial = new ClsHistorialTipoCambio();
            DataTable Datos = ComprasFuncEjecutar(() =>
                _Repositorio.ComprasFuncObtenerHistorial(Origen.IdMoneda, Destino.IdMoneda, Desde.Date, Hasta.Date));

            if (Datos.Rows.Count == 0)
            {
                Datos = ComprasFuncEjecutar(() =>
                    _Repositorio.ComprasFuncObtenerHistorial(Destino.IdMoneda, Origen.IdMoneda, Desde.Date, Hasta.Date));
                Historial.EsInverso = Datos.Rows.Count > 0;
            }

            // La consulta viene ordenada por fecha y versión: la última fila de cada día es la versión vigente.
            var PorDia = new SortedDictionary<DateTime, decimal>();
            foreach (DataRow Fila in Datos.Rows)
            {
                decimal Valor = Convert.ToDecimal(Fila["valorTipoCambio"]);
                PorDia[Convert.ToDateTime(Fila["fechaTipoCambio"]).Date] =
                    Math.Round(Historial.EsInverso ? 1m / Valor : Valor, _DecimalesTipoCambio);
            }

            foreach (KeyValuePair<DateTime, decimal> Punto in PorDia)
                Historial.Puntos.Add(new ClsPuntoTipoCambio(Punto.Key, Punto.Value));

            if (Historial.TieneDatos)
            {
                Historial.Minimo = Historial.Puntos.Min(Punto => Punto.Valor);
                Historial.Maximo = Historial.Puntos.Max(Punto => Punto.Valor);
                Historial.Promedio = Math.Round(Historial.Puntos.Average(Punto => Punto.Valor), _DecimalesTipoCambio);
                decimal Inicial = Historial.Puntos.First().Valor;
                decimal Final = Historial.Puntos.Last().Valor;
                Historial.VariacionPorcentual = Inicial == 0 ? 0 : Math.Round((Final - Inicial) / Inicial * 100m, 2);
            }

            return Historial;
        }

        public static string ComprasFuncFormatearMonto(decimal Monto, ClsMonedaCompras Moneda)
        {
            string Simbolo = Moneda == null ? string.Empty : Moneda.Simbolo + " ";
            return Simbolo + Monto.ToString("N2", CultureInfo.InvariantCulture);
        }

        public static string ComprasFuncFormatearTasa(decimal Tasa)
        {
            return Tasa.ToString("N" + _DecimalesTipoCambio, CultureInfo.InvariantCulture);
        }

        // Búsqueda del tipo de cambio

        private sealed class ClsTasa
        {
            public decimal Valor;
            public string Metodo;
            public DateTime? Fecha;
            public string Fuente;
        }

        private ClsTasa ComprasFuncBuscarTasa(ClsMonedaCompras Origen, ClsMonedaCompras Destino, DateTime Fecha)
        {
            if (Origen.IdMoneda == Destino.IdMoneda)
                return new ClsTasa { Valor = 1m, Metodo = "Misma moneda", Fuente = "—" };

            ClsTasa Tasa = ComprasFuncBuscarTasaSimple(Origen.IdMoneda, Destino.IdMoneda, Fecha);
            if (Tasa != null) return Tasa;

            List<ClsMonedaCompras> Monedas = ComprasFuncObtenerMonedas();
            ClsMonedaCompras Local = ComprasFuncObtenerMonedaLocal(Monedas);
            if (Local == null || Local.IdMoneda == Origen.IdMoneda || Local.IdMoneda == Destino.IdMoneda)
                return null;

            ClsTasa HaciaLocal = ComprasFuncBuscarTasaSimple(Origen.IdMoneda, Local.IdMoneda, Fecha);
            ClsTasa DesdeLocal = ComprasFuncBuscarTasaSimple(Local.IdMoneda, Destino.IdMoneda, Fecha);
            if (HaciaLocal == null || DesdeLocal == null) return null;

            return new ClsTasa
            {
                Valor = HaciaLocal.Valor * DesdeLocal.Valor,
                Metodo = "Cruzado a través de " + Local.Codigo,
                Fecha = HaciaLocal.Fecha < DesdeLocal.Fecha ? HaciaLocal.Fecha : DesdeLocal.Fecha,
                Fuente = HaciaLocal.Fuente == DesdeLocal.Fuente ? HaciaLocal.Fuente : HaciaLocal.Fuente + " / " + DesdeLocal.Fuente
            };
        }

        private ClsTasa ComprasFuncBuscarTasaSimple(long IdOrigen, long IdDestino, DateTime Fecha)
        {
            DataTable Directo = ComprasFuncEjecutar(() => _Repositorio.ComprasFuncObtenerTipoCambio(IdOrigen, IdDestino, Fecha));
            if (Directo.Rows.Count > 0)
                return ComprasFuncCrearTasa(Directo.Rows[0], false);

            DataTable Inverso = ComprasFuncEjecutar(() => _Repositorio.ComprasFuncObtenerTipoCambio(IdDestino, IdOrigen, Fecha));
            return Inverso.Rows.Count > 0 ? ComprasFuncCrearTasa(Inverso.Rows[0], true) : null;
        }

        private static ClsTasa ComprasFuncCrearTasa(DataRow Fila, bool EsInverso)
        {
            decimal Valor = Convert.ToDecimal(Fila["valorTipoCambio"]);
            return new ClsTasa
            {
                Valor = EsInverso ? 1m / Valor : Valor,
                Metodo = EsInverso ? "Inverso" : "Directo",
                Fecha = Convert.ToDateTime(Fila["fechaTipoCambio"]),
                Fuente = Convert.ToString(Fila["fuenteTipoCambio"])
            };
        }

        // Utilidades

        private static bool ComprasFuncLeerMonto(string Texto, out decimal Monto)
        {
            Monto = 0;
            if (string.IsNullOrWhiteSpace(Texto)) return false;
            string Limpio = Texto.Trim().Replace(",", string.Empty);
            return decimal.TryParse(Limpio, NumberStyles.Number, CultureInfo.InvariantCulture, out Monto)
                   && Monto > 0 && Monto <= _MontoMaximo;
        }

        private static ClsResultadoConversion ComprasFuncFallo(ClsResultadoConversion Resultado, string Mensaje)
        {
            Resultado.Exitoso = false;
            Resultado.Mensaje = Mensaje;
            return Resultado;
        }

        private static Resultado ComprasFuncEjecutar<Resultado>(Func<Resultado> Operacion)
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
