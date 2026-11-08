using System;
using System.Data;
using CapaModelo_TransporteExterno_Logistica;

namespace CapaControlador_TransporteExterno_Logistica
{
    public class ClsControladorIngresarProducto
    {
        private ClsRepositorioIngresarProducto modelo;

        public ClsControladorIngresarProducto()
        {
            modelo = new ClsRepositorioIngresarProducto();
        }

        public DataTable LogisticaFuncObtenerGuias()
        {
            return modelo.LogisticaFuncObtenerGuias();
        }

        public DataTable LogisticaFuncObtenerProductosDisponibles(int idServicio)
        {
            if (idServicio <= 0)
            {
                throw new Exception("Seleccione un transporte.");
            }

            return modelo.LogisticaFuncObtenerProductosDisponibles(idServicio);
        }

        public DataTable LogisticaFuncObtenerProductosDelServicio(int idServicio)
        {
            if (idServicio <= 0)
            {
                throw new Exception("Seleccione un transporte.");
            }

            return modelo.LogisticaFuncObtenerProductosDelServicio(idServicio);
        }

        public decimal LogisticaFuncCalcularTotal(
            decimal precioUnitario,
            int cantidad)
        {
            return precioUnitario * cantidad;
        }

        public void LogisticaProcGuardarProductos(
            int idServicio,
            DataTable productos)
        {
            if (idServicio <= 0)
            {
                throw new Exception("Seleccione una guía.");
            }

            if (productos == null)
            {
                throw new Exception("No hay una lista de productos para guardar.");
            }

            modelo.LogisticaProcGuardarProductos(idServicio, productos);
        }
    }
}