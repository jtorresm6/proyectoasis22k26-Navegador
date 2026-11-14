/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Entrega al MDI la ventana del reporte de activos fijos.
 * Reglas especificas:
 *  Implementa el método parcial de ClsReporteActivosFijosContabilidad
 *  sin tocar ese archivo.
 * ===================================================================
*/

using System.Windows.Forms;
using CapaControlador_Contabilidad;

namespace CapaVista_Contabilidad
{
    // Enlace del reporte con el MDI
    public static partial class ClsReporteActivosFijosContabilidad
    {
        // Crea la ventana del reporte
        static partial void ContabilidadMetCrearReporte(ClsModeloPermisosPantallaContabilidad Permisos, ref Form Formulario)
        {
            Formulario = new FrmReporteActivosFijos(Permisos);
        }
    }
}
