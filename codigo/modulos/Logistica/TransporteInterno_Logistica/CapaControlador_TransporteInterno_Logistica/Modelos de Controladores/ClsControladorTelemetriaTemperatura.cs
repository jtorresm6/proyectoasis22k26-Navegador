using CapaModelo_TransporteInterno_Logistica.Contratos;
using CapaModelo_TransporteInterno_Logistica.Entidades;
using CapaModelo_TransporteInterno_Logistica.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores
{
    public class ClsControladorTelemetriaTemperatura
    {
        // =====================================================
        // REPOSITORIO DE TELEMETRÍA
        // =====================================================

        private readonly IRepositorioTelemetriaTemperatura
            _Repositorio;

        public ClsControladorTelemetriaTemperatura()
        {
            _Repositorio =
                new ClsRepositorioTelemetriaTemperatura();
        }

        // =====================================================
        // CONSULTAR TODOS LOS REGISTROS
        // =====================================================

        public List<ClsDatosTelemetriaTemperatura>
            LogisticaFuncObtenerTodos()
        {
            var Lista =
                new List<ClsDatosTelemetriaTemperatura>();

            var Registros =
                _Repositorio.LogisticaMetObtenerTodos();

            foreach (ClsTelemetriaTemperatura Entidad
                in Registros)
            {
                Lista.Add(
                    new ClsDatosTelemetriaTemperatura
                    {
                        IdTelemetriaTemperatura =
                            Entidad.IdTelemetriaTemperatura,

                        FkEncabezadoViajeCodigo =
                            Entidad.Fk_Encabezado_Viaje_Codigo,

                        FechaHoraTelemetriaTemperatura =
                            Entidad.FechaHoraTelemetriaTemperatura,

                        TemperaturaCelsiusTelemetriaTemperatura =
                            Entidad.TemperaturaCelsiusTelemetriaTemperatura,

                        HumedadRelativaTelemetriaTemperatura =
                            Entidad.HumedadRelativaTelemetriaTemperatura,

                        PuertaAbiertaTelemetriaTemperatura =
                            Entidad.PuertaAbiertaTelemetriaTemperatura,

                        UbicacionGpsTelemetriaTemperatura =
                            Entidad.UbicacionGpsTelemetriaTemperatura,

                        NombreViaje =
                            "Viaje #" +
                            Entidad.Fk_Encabezado_Viaje_Codigo,

                        CreatedAt =
                            Entidad.CreatedAt,

                        UpdatedAt =
                            Entidad.UpdatedAt
                    });
            }

            return Lista;
        }

        // =====================================================
        // CONSULTAR VIAJES PARA EL COMBOBOX
        // =====================================================

        public List<ClsOpcionCatalogoLote>
            LogisticaFuncViajes()
        {
            return _Repositorio
                .LogisticaMetObtenerViajes()
                .Select(V => new ClsOpcionCatalogoLote
                {
                    Id = V.Id,
                    Nombre = V.Nombre
                })
                .ToList();
        }

        // =====================================================
        // AGREGAR REGISTRO
        // =====================================================

        public string LogisticaFuncAgregar(
            ClsDatosTelemetriaTemperatura Datos)
        {
            LogisticaProcValidar(Datos);

            ClsTelemetriaTemperatura Entidad =
                LogisticaFuncConvertirEntidad(Datos);

            int Resultado =
                _Repositorio.LogisticaMetAgregar(Entidad);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo registrar la telemetría.");
            }

            return "Telemetría registrada correctamente.";
        }

        // =====================================================
        // MODIFICAR REGISTRO
        // =====================================================

        public string LogisticaFuncEditar(
            ClsDatosTelemetriaTemperatura Datos)
        {
            if (Datos == null ||
                Datos.IdTelemetriaTemperatura <= 0)
            {
                throw new ValidationException(
                    "Seleccione un registro de telemetría válido.");
            }

            LogisticaProcValidar(Datos);

            ClsTelemetriaTemperatura Entidad =
                LogisticaFuncConvertirEntidad(Datos);

            int Resultado =
                _Repositorio.LogisticaMetEditar(Entidad);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo modificar el registro de telemetría.");
            }

            return "Telemetría modificada correctamente.";
        }

        // =====================================================
        // ELIMINAR REGISTRO
        // =====================================================

        public string LogisticaFuncEliminar(
            long IdTelemetriaTemperatura)
        {
            if (IdTelemetriaTemperatura <= 0)
            {
                throw new ValidationException(
                    "Seleccione un registro válido.");
            }

            int Resultado =
                _Repositorio.LogisticaMetEliminar(
                    IdTelemetriaTemperatura);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo eliminar el registro de telemetría.");
            }

            return "Telemetría eliminada correctamente.";
        }

        // =====================================================
        // CONVERTIR DATOS A ENTIDAD
        // =====================================================

        private ClsTelemetriaTemperatura
            LogisticaFuncConvertirEntidad(
                ClsDatosTelemetriaTemperatura Datos)
        {
            return new ClsTelemetriaTemperatura
            {
                IdTelemetriaTemperatura =
                    Datos.IdTelemetriaTemperatura,

                Fk_Encabezado_Viaje_Codigo =
                    Datos.FkEncabezadoViajeCodigo,

                FechaHoraTelemetriaTemperatura =
                    Datos.FechaHoraTelemetriaTemperatura,

                TemperaturaCelsiusTelemetriaTemperatura =
                    Datos.TemperaturaCelsiusTelemetriaTemperatura,

                HumedadRelativaTelemetriaTemperatura =
                    Datos.HumedadRelativaTelemetriaTemperatura,

                PuertaAbiertaTelemetriaTemperatura =
                    Datos.PuertaAbiertaTelemetriaTemperatura,

                UbicacionGpsTelemetriaTemperatura =
                    Datos.UbicacionGpsTelemetriaTemperatura
            };
        }

        // =====================================================
        // VALIDACIONES
        // =====================================================

        private void LogisticaProcValidar(
            ClsDatosTelemetriaTemperatura Datos)
        {
            if (Datos == null)
            {
                throw new ValidationException(
                    "Complete los datos de telemetría.");
            }

            // Validaciones DataAnnotations.
            var Contexto = new ValidationContext(Datos);

            Validator.ValidateObject(
                Datos,
                Contexto,
                true);

            // Validar viaje seleccionado.
            if (Datos.FkEncabezadoViajeCodigo <= 0)
            {
                throw new ValidationException(
                    "Seleccione un viaje válido.");
            }

            // Validar fecha y hora.
            if (Datos.FechaHoraTelemetriaTemperatura ==
                DateTime.MinValue)
            {
                throw new ValidationException(
                    "Ingrese una fecha y hora válida.");
            }

            // Validar temperatura.
            if (Datos.TemperaturaCelsiusTelemetriaTemperatura <
                    -999.99m ||
                Datos.TemperaturaCelsiusTelemetriaTemperatura >
                    999.99m)
            {
                throw new ValidationException(
                    "La temperatura está fuera del rango permitido.");
            }

            // Validar humedad.
            if (Datos.HumedadRelativaTelemetriaTemperatura < 0 ||
                Datos.HumedadRelativaTelemetriaTemperatura > 1)
            {
                throw new ValidationException(
                    "La humedad debe estar entre 0 y 1.");
            }

            // Validar ubicación.
            if (string.IsNullOrWhiteSpace(
                Datos.UbicacionGpsTelemetriaTemperatura))
            {
                throw new ValidationException(
                    "Ingrese la ubicación GPS.");
            }
        }
    }
}
