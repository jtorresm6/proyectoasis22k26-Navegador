using System;
using System.Data;
using CapaModelo_CierreInventario_Logistica.Entidad;

namespace CapaModelo_CierreInventario_Logistica.Contratos
{
    public interface IContratoCierreInventario
    {
        DataTable ObtenerSucursales();
        ResultadoCierre VistaPrevia(int sucursal, int anio, int mes);
        int EjecutarCierre(int sucursal, int anio, int mes, string nombre);
        DataTable ConsultarCierres(int? sucursal, DateTime inicio, DateTime fin, string nombre);
    }
}
