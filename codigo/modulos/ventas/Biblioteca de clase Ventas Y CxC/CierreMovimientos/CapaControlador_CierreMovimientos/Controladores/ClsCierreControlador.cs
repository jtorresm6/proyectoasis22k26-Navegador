// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System.Data;
using System;

namespace CapaControlador_CierreMovimientos
{
    public sealed class ClsCierreControlador
    {
        private readonly Servicios.ClsCierreServicio _Servicio = new Servicios.ClsCierreServicio();
        public DataTable VentasFuncListar(int Moneda)
        {
            return _Servicio.VentasFuncListar(Moneda);
        }

        public string VentasFuncGuardarBorrador(int Id, int Moneda, int Usuario, string Tipo, DateTime Desde, DateTime Hasta, string Observaciones)
        {
            return _Servicio.VentasFuncGuardarBorrador(Id, Moneda, Usuario, Tipo, Desde, Hasta, Observaciones);
        }

        public void VentasMetEliminarBorrador(int Id, int Usuario, string Motivo)
        {
            _Servicio.VentasMetEliminarBorrador(Id, Usuario, Motivo);
        }

        public DataTable VentasFuncCatalogo(string Nombre)
        {
            return _Servicio.VentasFuncCatalogo(Nombre);
        }

        public DataTable VentasFuncRevisar(int Moneda, string Tipo, DateTime Desde, DateTime Hasta)
        {
            return _Servicio.VentasFuncRevisar(Moneda, Tipo, Desde, Hasta);
        }

        public DataTable VentasFuncEstadoPeriodo(int Moneda, string Tipo, DateTime Desde, DateTime Hasta)
        {
            return _Servicio.VentasFuncEstadoPeriodo(Moneda, Tipo, Desde, Hasta);
        }

        public string VentasFuncConfirmar(int Moneda, int Usuario, string Tipo, DateTime Desde, DateTime Hasta, bool Revisado)
        {
            return _Servicio.VentasFuncConfirmar(Moneda, Usuario, Tipo, Desde, Hasta, Revisado);
        }

        public DataTable VentasFuncHistorial(int Moneda, int Cliente, string Periodo)
        {
            return _Servicio.VentasFuncHistorial(Moneda, Cliente, Periodo);
        }

        public DataTable VentasFuncPartida(int IdCierre)
        {
            return _Servicio.VentasFuncPartida(IdCierre);
        }

        public DataTable VentasFuncEstadoTraslado(int IdCierre)
        {
            return _Servicio.VentasFuncEstadoTraslado(IdCierre);
        }

        public string VentasFuncTrasladar(int IdCierre, int Usuario, Func<string, int, DateTime, DataTable, string> Registrar)
        {
            if (Registrar == null)
                throw new InvalidOperationException("Configure el enlace de Contabilidad.");
            DataSet Datos = _Servicio.VentasFuncPrepararTraslado(IdCierre, Usuario);
            DataRow Encabezado = Datos.Tables[0].Rows[0];
            if ((string)Encabezado["estado"] == "CONFIRMADO")
                return (string)Encabezado["referenciaContable"];
            // Sin transaccion abierta durante la llamada externa. El receptor debe deduplicar por clave.
            string Referencia = Registrar((string)Encabezado["claveIntegracion"], Convert.ToInt32(Encabezado["idMoneda"]), Convert.ToDateTime(Encabezado["fechaHasta"]), Datos.Tables[1]);
            if (string.IsNullOrWhiteSpace(Referencia))
                throw new InvalidOperationException("Contabilidad no devolvio una confirmacion. La partida sigue pendiente con la misma clave para reintentar.");
            _Servicio.VentasMetConfirmarTraslado(IdCierre, Usuario, Referencia);
            return Referencia;
        }

        public void VentasMetValidarCaptura(bool Condicion, string Mensaje)
        {
            VentasValidaciones.ClsValidacionesVentas.VentasMetExigir(Condicion, Mensaje);
        }

        public decimal VentasFuncImporteCelda(object Valor)
        {
            return VentasValidaciones.ClsValidacionesVentas.VentasFuncImporteCelda(Valor);
        }

        public DateTime[] VentasFuncPeriodo(int Anio, int Mes, bool Anual)
        {
            return Validaciones.ClsReglasCierre.VentasFuncPeriodo(Anio, Mes, Anual);
        }
    }
}
