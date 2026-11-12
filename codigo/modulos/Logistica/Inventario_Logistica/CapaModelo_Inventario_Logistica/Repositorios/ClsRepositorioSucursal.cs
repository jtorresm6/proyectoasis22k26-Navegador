
/*
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsRepositorioSucursal.Informe.cs
 * RESPONSABLE: VELVETH SARAÍ CHÁVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 *
 * DESCRIPCIÓN:
 * Consulta todos los campos de tblSucursal
 * para generar el informe de sucursales.
 */

using System.Data;

namespace CapaModelo_Inventario_Logistica.Repositorios
{
    public partial class ClsRepositorioSucursal
    {
        public DataTable LogisticaMetObtenerDatosInforme()
        {
            string Consulta =
                "SELECT " +
                "idSucursal, " +
                "nombreSucursal, " +
                "descripcionSucursal, " +
                "ubicacionSucursal, " +
                "paisSucursal, " +
                "created_at, " +
                "updated_at " +
                "FROM tblSucursal " +
                "ORDER BY idSucursal";

            var Repositorio = new ClsConsultaSucursalInforme();

            return Repositorio.LogisticaMetEjecucionConsulta(
                Consulta,
                CommandType.Text
            );
        }

        private sealed class ClsConsultaSucursalInforme
            : ClsRepositorioMaestro
        {
        }
    }
}
