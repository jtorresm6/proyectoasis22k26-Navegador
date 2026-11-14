
using System;
using System.Data;
using CapaModelo_TransporteExterno_Logistica;

namespace CapaControlador_TransporteExterno_Logistica
{
    public class ClsControladorIncidenciasEHistorial
    {
        private ClsRepositorioIncidenciasEHistorial modelo;

        public ClsControladorIncidenciasEHistorial()
        {
            modelo = new ClsRepositorioIncidenciasEHistorial();
        }

        // ==========================================
        // OBTENER GUIAS
        // ==========================================
        public DataTable LogisticaFuncObtenerGuias()
        {
            return modelo.LogisticaFuncObtenerGuias();
        }

        // ==========================================
        // OBTENER INCIDENCIAS
        // ==========================================
        public DataTable LogisticaFuncObtenerIncidencias(int idServicio)
        {
            if (idServicio <= 0)
                throw new Exception("Seleccione un transporte.");

            return modelo.LogisticaFuncObtenerIncidencias(idServicio);
        }

        // ==========================================
        // GUARDAR INCIDENCIA
        // ==========================================
        public void LogisticaProcGuardarIncidencia(
            int idServicio,
            string codigo,
            DateTime fecha,
            string tipo,
            string descripcion,
            string responsable,
            string estado)
        {
            LogisticaMetValidarIncidencia(
                idServicio, codigo, tipo, descripcion, responsable, estado);

            modelo.LogisticaProcGuardarIncidencia(
                idServicio,
                codigo.Trim(),
                fecha,
                tipo.Trim(),
                descripcion.Trim(),
                responsable.Trim(),
                estado.Trim());
        }

        // ==========================================
        // MODIFICAR INCIDENCIA
        // ==========================================
        public void LogisticaProcModificarIncidencia(
            int idIncidencia,
            string codigo,
            DateTime fecha,
            string tipo,
            string descripcion,
            string responsable,
            string estado)
        {
            if (idIncidencia <= 0)
                throw new Exception("Seleccione una incidencia.");

            LogisticaMetValidarIncidencia(
                1, codigo, tipo, descripcion, responsable, estado);

            modelo.LogisticaProcModificarIncidencia(
                idIncidencia,
                codigo.Trim(),
                fecha,
                tipo.Trim(),
                descripcion.Trim(),
                responsable.Trim(),
                estado.Trim());
        }

        // ==========================================
        // ELIMINAR INCIDENCIA
        // ==========================================
        public void LogisticaProcEliminarIncidencia(int idIncidencia)
        {
            if (idIncidencia <= 0)
                throw new Exception("Seleccione una incidencia.");

            modelo.LogisticaProcEliminarIncidencia(idIncidencia);
        }

        // ==========================================
        // VALIDAR INCIDENCIA
        // ==========================================
        private void LogisticaMetValidarIncidencia(
            int idServicio,
            string codigo,
            string tipo,
            string descripcion,
            string responsable,
            string estado)
        {
            if (idServicio <= 0)
                throw new Exception("Seleccione un transporte.");

            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("Ingrese el código de la incidencia.");

            if (string.IsNullOrWhiteSpace(tipo))
                throw new Exception("Seleccione el tipo de incidencia.");

            if (string.IsNullOrWhiteSpace(descripcion))
                throw new Exception("Ingrese la descripción.");

            if (string.IsNullOrWhiteSpace(responsable))
                throw new Exception("Ingrese el responsable.");

            if (string.IsNullOrWhiteSpace(estado))
                throw new Exception("Seleccione el estado.");
        }

        // ==========================================
        // CONSULTAR HISTORIAL
        // ==========================================
        public DataTable LogisticaFuncObtenerHistorial(
            int idServicio,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            string estado)
        {
            if (idServicio <= 0)
                throw new Exception("Seleccione un transporte.");

            if (fechaDesde.HasValue && fechaHasta.HasValue &&
                fechaDesde.Value.Date > fechaHasta.Value.Date)
            {
                throw new Exception(
                    "La fecha inicial no puede ser mayor que la fecha final.");
            }

            return modelo.LogisticaFuncObtenerHistorial(
                idServicio,
                fechaDesde,
                fechaHasta,
                estado);
        }

        // ==========================================
        // REGISTRAR MOVIMIENTO EN HISTORIAL
        // ==========================================
        public void LogisticaProcRegistrarMovimiento(
            int idServicio,
            string tipoMovimiento,
            string descripcion,
            string estado)
        {
            if (idServicio <= 0)
                throw new Exception("Seleccione un transporte válido.");

            if (string.IsNullOrWhiteSpace(tipoMovimiento))
                throw new Exception("Ingrese el tipo de movimiento.");

            if (string.IsNullOrWhiteSpace(descripcion))
                throw new Exception("Ingrese la descripción.");

            if (string.IsNullOrWhiteSpace(estado))
                throw new Exception("Ingrese el estado.");

            modelo.LogisticaProcRegistrarMovimiento(
                idServicio,
                tipoMovimiento.Trim(),
                descripcion.Trim(),
                estado.Trim());
        }
    }
}
