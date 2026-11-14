using System;
using System.Data;
using CapaModelo_CierreInventario_Logistica.Contratos;
using CapaModelo_CierreInventario_Logistica.Entidad;
using CapaModelo_CierreInventario_Logistica.Repositorio;

namespace CapaControlador_CierreInventario_Logistica
{
    public class ClsControladorCierreInventario
    {
        private readonly IContratoCierreInventario modelo;

        public ClsControladorCierreInventario()
        {
            modelo = new ClsRepositorioCierreInventario();
        }

        public DataTable ObtenerSucursales()
        {
            return modelo.ObtenerSucursales();
        }

        public ResultadoCierre ValidarPeriodo(int sucursal, int anio, int mes)
        {
            ValidarEntrada(sucursal, anio, mes);
            return modelo.VistaPrevia(sucursal, anio, mes);
        }

        public int ConfirmarCierre(int sucursal, int anio, int mes)
        {
            ValidarEntrada(sucursal, anio, mes);
            string nombre = "Cierre de Inventario " + mes.ToString("00") + "/" + anio;
            return modelo.EjecutarCierre(sucursal, anio, mes, nombre);
        }

        public DataTable ConsultarCierres(int? sucursal, DateTime inicio, DateTime fin, string nombre)
        {
            if (inicio.Date > fin.Date)
                throw new ArgumentException("La fecha inicial no puede superar la final.");
            return modelo.ConsultarCierres(sucursal, inicio.Date, fin.Date, nombre);
        }

        private void ValidarEntrada(int sucursal, int anio, int mes)
        {
            if (sucursal <= 0 || anio < 2000 || anio > 9998 || mes < 1 || mes > 12)
                throw new ArgumentException("Seleccione sucursal, mes y año válidos.");
        }
    }
}
