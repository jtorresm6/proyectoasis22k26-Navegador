using System;
using System.Data;
using CapaModelo_AprobacionFinanciera;

namespace CapaControlador_AprobacionFinanciera
{
    public class ClsAprobacionFinanciera
    {
        private ClsRepositorioAprobacion _Repositorio = new ClsRepositorioAprobacion();

        public DataTable ComprasFuncListarPendientes()
        {
            try
            {
                return _Repositorio.ComprasFuncListarPendientes();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en Capa Controlador: " + Ex.Message);
            }
        }

        // grid del detalle
        public DataTable ComprasFuncObtenerDetalleOrden(int IdOrden)
        {
            try
            {
                return _Repositorio.ComprasFuncObtenerDetalleOrden(IdOrden);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error Controlador (Detalles): " + Ex.Message);
            }
        }

        // Valida los datos
        public int ComprasFuncRegistrarComprobacion(int IdOrdenCompra, string Serie, string Numero, DateTime Recepcion, DateTime Emision, decimal MontoOrden, decimal MontoFactura, string EstadoCompra, string Observaciones, DataTable Detalles)
        {
            Serie = (Serie ?? string.Empty).Trim();
            Numero = (Numero ?? string.Empty).Trim();
            Observaciones = (Observaciones ?? string.Empty).Trim();

            // Validaciones de negocio
            if (IdOrdenCompra <= 0)
                throw new ArgumentException("ID de orden no válido.");

            if (Serie.Length == 0 || Numero.Length == 0)
                throw new ArgumentException("La serie y el número de factura son obligatorios.");

            if (MontoFactura <= 0)
                throw new ArgumentException("El monto de la factura física debe ser mayor a cero.");

            try
            {
                return _Repositorio.ComprasFuncRegistrarComprobacion(IdOrdenCompra, Serie, Numero, Recepcion, Emision, MontoOrden, MontoFactura, EstadoCompra, Observaciones, Detalles);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error Controlador (Guardar): " + Ex.Message);
            }
        }
    }
}