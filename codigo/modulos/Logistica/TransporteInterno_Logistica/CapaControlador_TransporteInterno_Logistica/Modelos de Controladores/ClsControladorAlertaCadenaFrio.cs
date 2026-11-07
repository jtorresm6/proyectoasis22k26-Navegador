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
    public class ClsControladorAlertaCadenaFrio
    {
        // =====================================================
        // REPOSITORIO
        // =====================================================

        private readonly IRepositorioAlertaCadenaFrio _Repositorio;

        public ClsControladorAlertaCadenaFrio()
        {
            _Repositorio = new ClsRepositorioAlertaCadenaFrio();
        }

        // =====================================================
        // CONSULTAR TODAS LAS ALERTAS
        // =====================================================

        public List<ClsDatosAlertaCadenaFrio>
            LogisticaFuncObtenerTodos()
        {
            var Lista = new List<ClsDatosAlertaCadenaFrio>();

            var Registros = _Repositorio.LogisticaMetObtenerTodos();

            foreach (ClsAlertaCadenaFrio Entidad in Registros)
            {
                Lista.Add(new ClsDatosAlertaCadenaFrio
                {
                    IdAlertaCadenaFrio =
                        Entidad.IdAlertaCadenaFrio,

                    FkTelemetriaTemperaturaCodigo =
                        Entidad.Fk_Telemetria_Temperatura_Codigo,

                    TipoAlertaCadenaFrio =
                        Entidad.TipoAlertaCadenaFrio,

                    AtendidaAlertaCadenaFrio =
                        Entidad.AtendidaAlertaCadenaFrio,

                    ObservacionCalidadAlertaCadenaFrio =
                        Entidad.ObservacionCalidadAlertaCadenaFrio,

                    NombreTelemetria =
                        "Telemetría #" +
                        Entidad.Fk_Telemetria_Temperatura_Codigo,

                    CreatedAt = Entidad.CreatedAt,

                    UpdatedAt = Entidad.UpdatedAt
                });
            }

            return Lista;
        }

        // =====================================================
        // CONSULTAR TELEMETRÍAS PARA EL COMBOBOX
        // =====================================================

        public List<ClsOpcionCatalogoLote>
            LogisticaFuncTelemetrias()
        {
            return _Repositorio
                .LogisticaMetObtenerTelemetrias()
                .Select(T => new ClsOpcionCatalogoLote
                {
                    Id = T.Id,
                    Nombre = T.Nombre
                })
                .ToList();
        }

        // =====================================================
        // AGREGAR ALERTA
        // =====================================================

        public string LogisticaFuncAgregar(
            ClsDatosAlertaCadenaFrio Datos)
        {
            LogisticaProcValidar(Datos);

            ClsAlertaCadenaFrio Entidad =
                LogisticaFuncConvertirEntidad(Datos);

            int Resultado = _Repositorio.LogisticaMetAgregar(Entidad);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo registrar la alerta.");
            }

            return "Alerta de cadena de frío registrada correctamente.";
        }

        // =====================================================
        // MODIFICAR ALERTA
        // =====================================================

        public string LogisticaFuncEditar(
            ClsDatosAlertaCadenaFrio Datos)
        {
            if (Datos == null || Datos.IdAlertaCadenaFrio <= 0)
            {
                throw new ValidationException(
                    "Seleccione una alerta válida.");
            }

            LogisticaProcValidar(Datos);

            ClsAlertaCadenaFrio Entidad =
                LogisticaFuncConvertirEntidad(Datos);

            int Resultado = _Repositorio.LogisticaMetEditar(Entidad);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo modificar la alerta.");
            }

            return "Alerta modificada correctamente.";
        }

        // =====================================================
        // ELIMINAR ALERTA
        // =====================================================

        public string LogisticaFuncEliminar(long IdAlertaCadenaFrio)
        {
            if (IdAlertaCadenaFrio <= 0)
            {
                throw new ValidationException(
                    "Seleccione una alerta válida.");
            }

            int Resultado =
                _Repositorio.LogisticaMetEliminar(IdAlertaCadenaFrio);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo eliminar la alerta.");
            }

            return "Alerta eliminada correctamente.";
        }

        // =====================================================
        // CONVERTIR DATOS A ENTIDAD
        // =====================================================

        private ClsAlertaCadenaFrio LogisticaFuncConvertirEntidad(
            ClsDatosAlertaCadenaFrio Datos)
        {
            return new ClsAlertaCadenaFrio
            {
                IdAlertaCadenaFrio =
                    Datos.IdAlertaCadenaFrio,

                Fk_Telemetria_Temperatura_Codigo =
                    Datos.FkTelemetriaTemperaturaCodigo,

                TipoAlertaCadenaFrio =
                    Datos.TipoAlertaCadenaFrio,

                AtendidaAlertaCadenaFrio =
                    Datos.AtendidaAlertaCadenaFrio,

                ObservacionCalidadAlertaCadenaFrio =
                    Datos.ObservacionCalidadAlertaCadenaFrio
            };
        }

        // =====================================================
        // VALIDACIONES
        // =====================================================

        private void LogisticaProcValidar(
            ClsDatosAlertaCadenaFrio Datos)
        {
            if (Datos == null)
            {
                throw new ValidationException(
                    "Complete los datos de la alerta.");
            }

            var Contexto = new ValidationContext(Datos);

            Validator.ValidateObject(
                Datos,
                Contexto,
                true);

            if (Datos.FkTelemetriaTemperaturaCodigo <= 0)
            {
                throw new ValidationException(
                    "Seleccione una telemetría válida.");
            }

            if (string.IsNullOrWhiteSpace(
                Datos.TipoAlertaCadenaFrio))
            {
                throw new ValidationException(
                    "Seleccione un tipo de alerta.");
            }

            if (string.IsNullOrWhiteSpace(
                Datos.ObservacionCalidadAlertaCadenaFrio))
            {
                throw new ValidationException(
                    "Ingrese una observación de calidad.");
            }
        }
    }
}
