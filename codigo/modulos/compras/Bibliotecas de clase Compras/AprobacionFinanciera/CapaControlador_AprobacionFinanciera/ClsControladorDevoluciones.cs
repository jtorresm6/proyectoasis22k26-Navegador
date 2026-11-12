using System;
using System.Data;
using CapaModelo_AprobacionFinanciera;

namespace CapaControlador_AprobacionFinanciera
{
    public class ClsControladorDevoluciones
    {
        private ClsRepositorioDevoluciones _Repositorio = new ClsRepositorioDevoluciones();

        public DataTable ComprasFuncListarCompras()
        {
            try
            {
                return _Repositorio.ComprasFuncListarCompras();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en Capa Controlador (Listar Compras): " + Ex.Message);
            }
        }
        public DataTable ComprasFuncObtenerEncabezadoCompra(int IdCompra)
        {
            try
            {
                return _Repositorio.ComprasFuncObtenerEncabezadoCompra(IdCompra);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error Controlador (Encabezado): " + Ex.Message);
            }
        }

        public DataTable ComprasFuncObtenerDetallesCompra(int IdCompra)
        {
            try
            {
                return _Repositorio.ComprasFuncObtenerDetallesCompra(IdCompra);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error Controlador (Detalles): " + Ex.Message);
            }
        }
        public int ComprasFuncRegistrarDevolucion(int IdCompra, DateTime FechaDevolucion, string Motivo, string SerieNota, string NumeroNota, decimal TotalDevolucion, DataTable Detalles)
        {
            if (TotalDevolucion <= 0)
                throw new ArgumentException("El total de la devolución no puede ser cero.");

            if (Detalles == null || Detalles.Rows.Count == 0)
                throw new ArgumentException("Debe agregar al menos un producto a la devolución.");

            try
            {
                return _Repositorio.ComprasFuncRegistrarDevolucion(IdCompra, FechaDevolucion, Motivo, SerieNota, NumeroNota, TotalDevolucion, Detalles);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error Controlador (Registrar Devolución): " + Ex.Message);
            }
        }
    }
}