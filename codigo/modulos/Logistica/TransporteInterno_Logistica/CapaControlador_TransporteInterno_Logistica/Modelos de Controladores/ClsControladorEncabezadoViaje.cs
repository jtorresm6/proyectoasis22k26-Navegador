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
    public class ClsControladorEncabezadoViaje
    {
        // =====================================================
        // REPOSITORIO DE ENCABEZADO DE VIAJE
        // =====================================================

        private readonly IRepositorioEncabezadoViaje _Repositorio;

        public ClsControladorEncabezadoViaje()
        {
            _Repositorio = new ClsRepositorioEncabezadoViaje();
        }

        // =====================================================
        // CONSULTAR TODOS LOS VIAJES
        // =====================================================

        public List<ClsDatosEncabezadoViaje> LogisticaFuncObtenerTodos()
        {
            var Lista = new List<ClsDatosEncabezadoViaje>();

            var Registros = _Repositorio.LogisticaMetObtenerTodos();

            foreach (ClsEncabezadoViaje Entidad in Registros)
            {
                Lista.Add(new ClsDatosEncabezadoViaje
                {
                    IdEncabezadoViaje =
                        Entidad.IdEncabezadoViaje,

                    FechaDocumentoEncabezadoViaje =
                        Entidad.FechaDocumentoEncabezadoViaje,

                    HoraSalidaEncabezadoViaje =
                        Entidad.HoraSalidaEncabezadoViaje,

                    HoraLlegadaEncabezadoViaje =
                        Entidad.HoraLlegadaEncabezadoViaje,

                    RequiereCadenaFrioEncabezadoViaje =
                        Entidad.RequiereCadenaFrioEncabezadoViaje,

                    EstadoViajeEncabezadoViaje =
                        Entidad.EstadoViajeEncabezadoViaje,

                    PesoTotalEncabezadoViaje =
                        Entidad.PesoTotalEncabezadoViaje,

                    Fk_Transporte_Codigo =
                        Entidad.Fk_Transporte_Codigo,

                    Fk_Conductor_Codigo =
                        Entidad.Fk_Conductor_Codigo,

                    PlacaTransporte =
                        Entidad.PlacaTransporte,

                    NombreConductor =
                        Entidad.NombreConductor,

                    CreatedAt =
                        Entidad.CreatedAt,

                    UpdatedAt =
                        Entidad.UpdatedAt
                });
            }

            return Lista;
        }

        // =====================================================
        // CONSULTAR CATÁLOGO DE TRANSPORTES
        // =====================================================

        public List<ClsOpcionCatalogo> LogisticaFuncTransportes()
        {
            return _Repositorio
                .LogisticaMetObtenerTransportes()
                .Select(T => new ClsOpcionCatalogo
                {
                    Id = T.Id,
                    Nombre = T.Nombre
                })
                .ToList();
        }

        // =====================================================
        // CONSULTAR CATÁLOGO DE CONDUCTORES
        // =====================================================

        public List<ClsOpcionCatalogo> LogisticaFuncConductores()
        {
            return _Repositorio
                .LogisticaMetObtenerConductores()
                .Select(C => new ClsOpcionCatalogo
                {
                    Id = C.Id,
                    Nombre = C.Nombre
                })
                .ToList();
        }

        // =====================================================
        // AGREGAR VIAJE
        // =====================================================

        public string LogisticaFuncAgregar(
            ClsDatosEncabezadoViaje Datos)
        {
            LogisticaProcValidar(Datos);

            ClsEncabezadoViaje Entidad =
                LogisticaFuncConvertirEntidad(Datos);

            int Resultado =
                _Repositorio.LogisticaMetAgregar(Entidad);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo registrar el viaje.");
            }

            return "Viaje registrado correctamente.";
        }

        // =====================================================
        // MODIFICAR VIAJE
        // =====================================================

        public string LogisticaFuncEditar(
            ClsDatosEncabezadoViaje Datos)
        {
            if (Datos == null || Datos.IdEncabezadoViaje <= 0)
            {
                throw new ValidationException(
                    "Seleccione un viaje válido.");
            }

            LogisticaProcValidar(Datos);

            ClsEncabezadoViaje Entidad =
                LogisticaFuncConvertirEntidad(Datos);

            int Resultado =
                _Repositorio.LogisticaMetEditar(Entidad);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo modificar el viaje.");
            }

            return "Viaje modificado correctamente.";
        }

        // =====================================================
        // ELIMINAR VIAJE
        // =====================================================

        public string LogisticaFuncEliminar(
            long IdEncabezadoViaje)
        {
            if (IdEncabezadoViaje <= 0)
            {
                throw new ValidationException(
                    "Seleccione un viaje válido.");
            }

            int Resultado =
                _Repositorio.LogisticaMetEliminar(
                    IdEncabezadoViaje);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo eliminar el viaje.");
            }

            return "Viaje eliminado correctamente.";
        }

        // =====================================================
        // CONVERTIR DATOS DEL CONTROLADOR A ENTIDAD
        // =====================================================

        private ClsEncabezadoViaje LogisticaFuncConvertirEntidad(
            ClsDatosEncabezadoViaje Datos)
        {
            return new ClsEncabezadoViaje
            {
                IdEncabezadoViaje =
                    Datos.IdEncabezadoViaje,

                FechaDocumentoEncabezadoViaje =
                    Datos.FechaDocumentoEncabezadoViaje,

                HoraSalidaEncabezadoViaje =
                    Datos.HoraSalidaEncabezadoViaje,

                HoraLlegadaEncabezadoViaje =
                    Datos.HoraLlegadaEncabezadoViaje,

                RequiereCadenaFrioEncabezadoViaje =
                    Datos.RequiereCadenaFrioEncabezadoViaje,

                EstadoViajeEncabezadoViaje =
                    Datos.EstadoViajeEncabezadoViaje,

                PesoTotalEncabezadoViaje =
                    Datos.PesoTotalEncabezadoViaje,

                Fk_Transporte_Codigo =
                    Datos.Fk_Transporte_Codigo,

                Fk_Conductor_Codigo =
                    Datos.Fk_Conductor_Codigo
            };
        }

        // =====================================================
        // VALIDAR DATOS DEL VIAJE
        // =====================================================

        private void LogisticaProcValidar(
            ClsDatosEncabezadoViaje Datos)
        {
            if (Datos == null)
            {
                ClsValidacionesEncabezadoViaje
        .LogisticaProcValidar(Datos);
            }

            // Validaciones DataAnnotations.
            var Contexto = new ValidationContext(Datos);

            Validator.ValidateObject(
                Datos,
                Contexto,
                true);

            // Validar fecha de documento y salida.
            if (Datos.FechaDocumentoEncabezadoViaje ==
                    DateTime.MinValue ||
                Datos.HoraSalidaEncabezadoViaje ==
                    DateTime.MinValue)
            {
                throw new ValidationException(
                    "Complete todos los campos obligatorios.");
            }

            // Validar referencias obligatorias.
            if (Datos.Fk_Transporte_Codigo <= 0 ||
                Datos.Fk_Conductor_Codigo <= 0 ||
                string.IsNullOrWhiteSpace(
                    Datos.EstadoViajeEncabezadoViaje))
            {
                throw new ValidationException(
                    "Complete todos los campos obligatorios.");
            }

            // Validar el peso.
            if (Datos.PesoTotalEncabezadoViaje < 0)
            {
                throw new ValidationException(
                    "El peso total no puede ser negativo.");
            }

            // Validar salida respecto al documento.
            if (Datos.HoraSalidaEncabezadoViaje <
                Datos.FechaDocumentoEncabezadoViaje.Date)
            {
                throw new ValidationException(
                    "La salida no puede ser anterior " +
                    "a la fecha del documento.");
            }

            // Validar hora de llegada.
            if (Datos.HoraLlegadaEncabezadoViaje.HasValue &&
                Datos.HoraLlegadaEncabezadoViaje.Value <
                Datos.HoraSalidaEncabezadoViaje)
            {
                throw new ValidationException(
                    "La llegada no puede ser anterior " +
                    "a la salida.");
            }
        }
    }
}
