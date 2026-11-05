using CapaModelo_Componentes.Repositorios;


using System.Data;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Miguel David Contreras Jacinto
 * Carné : 0901-21-3878
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Coordina la consulta de datos que utilizan los selectores de Compras.
 * ===================================================================
*/

namespace CapaControlador_Componentes
{
    public class ClsComboCompras
    {
        private readonly ClsRepositorioCombo _Sentencias = new ClsRepositorioCombo();


        public DataTable ComprasFuncObtenerProveedores() => _Sentencias.ComprasFuncObtenerProveedores();

        public ClsProveedorCompras ComprasFuncObtenerProveedor(int IdProveedor)
        {
            DataTable Datos = _Sentencias.ComprasFuncObtenerProveedor(IdProveedor);
            return Datos.Rows.Count == 0 ? null : new ClsProveedorCompras(Datos.Rows[0]);
        }

        public DataTable ComprasFuncEnviarDatos(string Tabla, string Campo1, string Campo2)
        {
            var DtTabla = _Sentencias.ComprasFuncObtenerDatos(Tabla, Campo1, Campo2);

            return DtTabla;
        }

    }
}
