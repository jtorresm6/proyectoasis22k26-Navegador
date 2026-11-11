// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System.Data;
using System;

namespace CapaControlador_MovimientosClientes
{
    public sealed class ClsMovimientosControlador
    {
        private readonly Servicios.ClsMovimientosServicio _Servicio = new Servicios.ClsMovimientosServicio();
        public DataTable VentasFuncListarDocumentos(int Cliente, int Moneda)
        {
            return _Servicio.VentasFuncListarDocumentos(Cliente, Moneda);
        }

        public DataTable VentasFuncListarRecibos(int Cliente, int Moneda)
        {
            return _Servicio.VentasFuncListarRecibos(Cliente, Moneda);
        }

        public void VentasMetModificarDocumento(int Id, string Clase, int Usuario, string Descripcion, DateTime Vencimiento)
        {
            _Servicio.VentasMetModificarDocumento(Id, Clase, Usuario, Descripcion, Vencimiento);
        }

        public void VentasMetAnularDocumento(int Id, string Clase, int Usuario, string Motivo)
        {
            _Servicio.VentasMetAnularDocumento(Id, Clase, Usuario, Motivo);
        }

        public void VentasMetModificarRecibo(int Id, int Usuario, string Observaciones)
        {
            _Servicio.VentasMetModificarRecibo(Id, Usuario, Observaciones);
        }

        public DataTable VentasFuncCatalogo(string Nombre)
        {
            return _Servicio.VentasFuncCatalogo(Nombre);
        }

        public DataSet VentasFuncEstadoCuenta(int Cliente, int Moneda, DateTime Desde, DateTime Hasta)
        {
            return _Servicio.VentasFuncEstadoCuenta(Cliente, Moneda, Desde, Hasta);
        }

        public DataTable VentasFuncPendientes(int Cliente, int Moneda)
        {
            return _Servicio.VentasFuncPendientes(Cliente, Moneda);
        }

        public string VentasFuncRegistrarDocumento(int Cliente, int Moneda, int Usuario, int Establecimiento, string Numero, int Tipo, DateTime Fecha, DateTime Vencimiento, decimal Importe, string Referencia, string Motivo)
        {
            return _Servicio.VentasFuncRegistrarDocumento(Cliente, Moneda, Usuario, Establecimiento, Numero, Tipo, Fecha, Vencimiento, Importe, Referencia, Motivo);
        }

        public string VentasFuncRegistrarRecibo(int Cliente, int Moneda, int Usuario, string Numero, DateTime Fecha, decimal Monto, decimal TipoCambio, string Forma, string Referencia, string Observaciones, DataTable Aplicaciones)
        {
            return _Servicio.VentasFuncRegistrarRecibo(Cliente, Moneda, Usuario, Numero, Fecha, Monto, TipoCambio, Forma, Referencia, Observaciones, Aplicaciones);
        }

        public DataTable VentasFuncConsultarRecibos(string Recibo, string Deposito)
        {
            return _Servicio.VentasFuncConsultarRecibos(Recibo, Deposito);
        }

        public DataTable VentasFuncAplicaciones(int Pago)
        {
            return _Servicio.VentasFuncAplicaciones(Pago);
        }

        public DataTable VentasFuncDocumento(int Cliente, int Moneda, int Establecimiento, string Numero, bool Factura)
        {
            return _Servicio.VentasFuncDocumento(Cliente, Moneda, Establecimiento, Numero, Factura);
        }

        public DataSet VentasFuncDeposito(string Numero)
        {
            return _Servicio.VentasFuncDeposito(Numero);
        }

        public DataTable VentasFuncRecibosDisponibles(int Moneda)
        {
            return _Servicio.VentasFuncRecibosDisponibles(Moneda);
        }

        public string VentasFuncPrepararDeposito(int Moneda, int Usuario, DateTime Fecha, string Numero, string Empresa, string Banco, string Cuenta, string Concepto, string Referencia, DataTable Recibos)
        {
            return _Servicio.VentasFuncPrepararDeposito(Moneda, Usuario, Fecha, Numero, Empresa, Banco, Cuenta, Concepto, Referencia, Recibos);
        }

        public void VentasMetAnularRecibo(int Pago, DateTime Fecha, int Usuario, string Motivo)
        {
            _Servicio.VentasMetAnularRecibo(Pago, Fecha, Usuario, Motivo);
        }

        public void VentasMetValidarCaptura(bool Condicion, string Mensaje)
        {
            VentasValidaciones.ClsValidacionesVentas.VentasMetExigir(Condicion, Mensaje);
        }

        public decimal VentasFuncImporteCelda(object Valor)
        {
            return VentasValidaciones.ClsValidacionesVentas.VentasFuncImporteCelda(Valor);
        }

        public DataRow VentasFuncLeerDocumento(int Id, string Clase)
        {
            return _Servicio.VentasFuncLeerDocumento(Id, Clase);
        }

        public DataSet VentasFuncLeerRecibo(int Id)
        {
            return _Servicio.VentasFuncLeerRecibo(Id);
        }

        public DataRow VentasFuncLeerMovimiento(int Id)
        {
            return _Servicio.VentasFuncLeerMovimiento(Id);
        }
    }
}
