// ------------------------------------------------------
// Módulo: Logística - Transporte Externo
// Clase: Controlador de Costos de Transporte
// ------------------------------------------------------

using System;
using System.Data;
using CapaModelo_TransporteExterno_Logistica.Repositorio;

namespace CapaControlador_TransporteExterno_Logistica
{
    public class ClsControladorCostosTransporte
    {
        private ClsRepositorioCostosTransporte
            LogisticaObjRepositorio =
            new ClsRepositorioCostosTransporte();

        // ------------------------------------------------------
        // OBTENER GUÍAS
        // ------------------------------------------------------
        public DataTable LogisticaMetObtenerGuias()
        {
            return LogisticaObjRepositorio.LogisticaMetObtenerGuias();
        }

        // ------------------------------------------------------
        // OBTENER TARIFAS
        // ------------------------------------------------------
        public DataTable LogisticaMetObtenerTarifas()
        {
            return LogisticaObjRepositorio.LogisticaMetObtenerTarifas();
        }

        // ------------------------------------------------------
        // CONSULTAR COSTO
        // ------------------------------------------------------
        public DataTable LogisticaMetObtenerCostoPorGuia(string numeroGuia)
        {
            return LogisticaObjRepositorio
                .LogisticaMetObtenerCostoPorGuia(numeroGuia);
        }

        // ------------------------------------------------------
        // VERIFICAR EXISTENCIA
        // ------------------------------------------------------
        public bool LogisticaMetExisteCostoPorGuia(string numeroGuia)
        {
            return LogisticaObjRepositorio
                .LogisticaMetExisteCostoPorGuia(numeroGuia);
        }

        // ------------------------------------------------------
        // VALIDAR DATOS
        // ------------------------------------------------------
        private void LogisticaMetValidarDatos(
            string numeroGuia,
            int idTarifa,
            decimal cantidad,
            decimal costoUnitario,
            string moneda,
            string tipoPago)
        {
            if (string.IsNullOrWhiteSpace(numeroGuia))
                throw new Exception("Debe seleccionar una guía.");

            if (idTarifa <= 0)
                throw new Exception("Debe seleccionar una tarifa.");

            if (cantidad <= 0)
                throw new Exception("Los kilómetros deben ser mayores a cero.");

            if (costoUnitario <= 0)
                throw new Exception("El costo por kilómetro no es válido.");

            if (moneda != "GTQ" && moneda != "USD")
                throw new Exception("Seleccione una moneda válida.");

            if (tipoPago != "Transferencia" && tipoPago != "Efectivo")
                throw new Exception("Seleccione un tipo de pago válido.");
        }

        // ------------------------------------------------------
        // GUARDAR
        // ------------------------------------------------------
        public bool LogisticaMetGuardarCosto(
            string numeroGuia,
            int idTarifa,
            decimal cantidad,
            decimal costoUnitario,
            string moneda,
            string tipoPago)
        {
            LogisticaMetValidarDatos(
                numeroGuia,
                idTarifa,
                cantidad,
                costoUnitario,
                moneda,
                tipoPago);

            if (LogisticaMetExisteCostoPorGuia(numeroGuia))
                return false;

            return LogisticaObjRepositorio.LogisticaMetGuardarCosto(
                numeroGuia,
                idTarifa,
                cantidad,
                costoUnitario,
                moneda,
                tipoPago);
        }

        // ------------------------------------------------------
        // MODIFICAR
        // ------------------------------------------------------
        public bool LogisticaMetModificarCosto(
            string numeroGuia,
            int idTarifa,
            decimal cantidad,
            decimal costoUnitario,
            string moneda,
            string tipoPago)
        {
            LogisticaMetValidarDatos(
                numeroGuia,
                idTarifa,
                cantidad,
                costoUnitario,
                moneda,
                tipoPago);

            if (!LogisticaMetExisteCostoPorGuia(numeroGuia))
                return false;

            return LogisticaObjRepositorio.LogisticaMetModificarCosto(
                numeroGuia,
                idTarifa,
                cantidad,
                costoUnitario,
                moneda,
                tipoPago);
        }

        // ------------------------------------------------------
        // ELIMINAR
        // ------------------------------------------------------
        public bool LogisticaMetEliminarCosto(string numeroGuia)
        {
            if (string.IsNullOrWhiteSpace(numeroGuia))
                throw new Exception("Debe seleccionar una guía.");

            return LogisticaObjRepositorio
                .LogisticaMetEliminarCosto(numeroGuia);
        }
    }
}