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
    public class ClsControladorDetalleViaje
    {
        // =====================================================
        // REPOSITORIO DE DETALLE DE VIAJE
        // =====================================================

        private readonly IRepositorioDetalleViaje _Repositorio;

        public ClsControladorDetalleViaje()
        {
            _Repositorio = new ClsRepositorioDetalleViaje();
        }

        // =====================================================
        // CONSULTAR TODOS LOS DETALLES
        // =====================================================

        public List<ClsDatosDetalleViaje> LogisticaFuncObtenerTodos()
        {
            var Lista = new List<ClsDatosDetalleViaje>();

            var Registros = _Repositorio.LogisticaMetObtenerTodos();

            foreach (ClsDetalleViaje Entidad in Registros)
            {
                Lista.Add(new ClsDatosDetalleViaje
                {
                    IdDetalleViaje =
                        Entidad.IdDetalleViaje,

                    FkEncabezadoViajeCodigo =
                        Entidad.Fk_Encabezado_Viaje_Codigo,

                    OrdenDetalleViaje =
                        Entidad.OrdenDetalleViaje,

                    DescripcionDetalleViaje =
                        Entidad.DescripcionDetalleViaje,

                    CantidadDetalleViaje =
                        Entidad.CantidadDetalleViaje,

                    PesoDetalleViaje =
                        Entidad.PesoDetalleViaje,

                    // Se obtiene el identificador de viaje
                    // como representación inicial.
                    DescripcionViaje =
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
        // OBTENER VIAJES PARA EL COMBOBOX
        // =====================================================

        public List<ClsOpcionCatalogo> LogisticaFuncViajes()
        {
            return _Repositorio
                .LogisticaMetObtenerViajes()
                .Select(V => new ClsOpcionCatalogo
                {
                    Id = V.Id,
                    Nombre = V.Nombre
                })
                .ToList();
        }

        // =====================================================
        // AGREGAR DETALLE
        // =====================================================

        public string LogisticaFuncAgregar(
            ClsDatosDetalleViaje Datos)
        {
            LogisticaProcValidar(Datos);

            ClsDetalleViaje Entidad =
                LogisticaFuncConvertirEntidad(Datos);

            int Resultado =
                _Repositorio.LogisticaMetAgregar(Entidad);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo registrar el detalle del viaje.");
            }

            return "Detalle de viaje registrado correctamente.";
        }

        // =====================================================
        // MODIFICAR DETALLE
        // =====================================================

        public string LogisticaFuncEditar(
            ClsDatosDetalleViaje Datos)
        {
            if (Datos == null || Datos.IdDetalleViaje <= 0)
            {
                throw new ValidationException(
                    "Seleccione un detalle de viaje válido.");
            }

            LogisticaProcValidar(Datos);

            ClsDetalleViaje Entidad =
                LogisticaFuncConvertirEntidad(Datos);

            int Resultado =
                _Repositorio.LogisticaMetEditar(Entidad);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo modificar el detalle del viaje.");
            }

            return "Detalle de viaje modificado correctamente.";
        }

        // =====================================================
        // ELIMINAR DETALLE
        // =====================================================

        public string LogisticaFuncEliminar(long IdDetalleViaje)
        {
            if (IdDetalleViaje <= 0)
            {
                throw new ValidationException(
                    "Seleccione un detalle de viaje válido.");
            }

            int Resultado =
                _Repositorio.LogisticaMetEliminar(IdDetalleViaje);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo eliminar el detalle del viaje.");
            }

            return "Detalle de viaje eliminado correctamente.";
        }

        // =====================================================
        // CONVERTIR DATOS DEL CONTROLADOR A ENTIDAD
        // =====================================================

        private ClsDetalleViaje LogisticaFuncConvertirEntidad(
            ClsDatosDetalleViaje Datos)
        {
            return new ClsDetalleViaje
            {
                IdDetalleViaje =
                    Datos.IdDetalleViaje,

                Fk_Encabezado_Viaje_Codigo =
                    Datos.FkEncabezadoViajeCodigo,

                OrdenDetalleViaje =
                    Datos.OrdenDetalleViaje,

                DescripcionDetalleViaje =
                    Datos.DescripcionDetalleViaje,

                CantidadDetalleViaje =
                    Datos.CantidadDetalleViaje,

                PesoDetalleViaje =
                    Datos.PesoDetalleViaje
            };
        }

        // =====================================================
        // VALIDACIONES
        // =====================================================

        private void LogisticaProcValidar(
            ClsDatosDetalleViaje Datos)
        {
            if (Datos == null)
            {
                throw new ValidationException(
                    "Complete todos los campos obligatorios.");
            }

            var Contexto = new ValidationContext(Datos);

            Validator.ValidateObject(
                Datos,
                Contexto,
                true);

            if (Datos.FkEncabezadoViajeCodigo <= 0 ||
                Datos.OrdenDetalleViaje <= 0 ||
                string.IsNullOrWhiteSpace(
                    Datos.DescripcionDetalleViaje))
            {
                throw new ValidationException(
                    "Complete todos los campos obligatorios.");
            }

            if (Datos.CantidadDetalleViaje < 0 ||
                Datos.PesoDetalleViaje < 0)
            {
                throw new ValidationException(
                    "La cantidad y el peso no pueden ser negativos.");
            }

            // La BD tiene UNIQUE por viaje y orden.
            // Esta validación anticipa el error de duplicado.
            bool ExisteOrden = _Repositorio
                .LogisticaMetObtenerTodos()
                .Any(D =>
                    D.Fk_Encabezado_Viaje_Codigo ==
                        Datos.FkEncabezadoViajeCodigo &&
                    D.OrdenDetalleViaje ==
                        Datos.OrdenDetalleViaje &&
                    D.IdDetalleViaje !=
                        Datos.IdDetalleViaje);

            if (ExisteOrden)
            {
                throw new ValidationException(
                    "Ya existe un detalle con ese orden " +
                    "para el viaje seleccionado.");
            }
        }
    }
}
