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
    public class ClsControladorDetalleViajeLoteProducto
    {
        // =====================================================
        // REPOSITORIO
        // =====================================================

        private readonly IRepositorioDetalleViajeLoteProducto
            _Repositorio;

        public ClsControladorDetalleViajeLoteProducto()
        {
            _Repositorio =
                new ClsRepositorioDetalleViajeLoteProducto();
        }

        // =====================================================
        // CONSULTAR ASIGNACIONES
        // =====================================================

        public List<ClsDatosDetalleViajeLoteProducto>
            LogisticaFuncObtenerTodos()
        {
            var Lista =
                new List<ClsDatosDetalleViajeLoteProducto>();

            var Registros =
                _Repositorio.LogisticaMetObtenerTodos();

            foreach (ClsDetalleViajeLoteProducto Entidad
                in Registros)
            {
                Lista.Add(new ClsDatosDetalleViajeLoteProducto
                {
                    IdDetalleViajeLoteProducto =
                        Entidad.IdDetalleViajeLoteProducto,

                    FkDetalleViajeCodigo =
                        Entidad.Fk_Detalle_Viaje_Codigo,

                    FkLoteProductoCodigo =
                        Entidad.Fk_Lote_Producto_Codigo,

                    CantidadCajasDetalleViajeLoteProducto =
                        Entidad.CantidadCajasDetalleViajeLoteProducto,

                    PesoKgDetalleViajeLoteProducto =
                        Entidad.PesoKgDetalleViajeLoteProducto,

                    NombreDetalleViaje =
                        "Detalle #" +
                        Entidad.Fk_Detalle_Viaje_Codigo,

                    NombreLoteProducto =
                        "Lote #" +
                        Entidad.Fk_Lote_Producto_Codigo,

                    CreatedAt = Entidad.CreatedAt,

                    UpdatedAt = Entidad.UpdatedAt
                });
            }

            return Lista;
        }

        // =====================================================
        // CATÁLOGO DE DETALLES DE VIAJE
        // =====================================================

        public List<ClsOpcionCatalogoLote>
            LogisticaFuncDetallesViaje()
        {
            return _Repositorio
                .LogisticaMetObtenerDetallesViaje()
                .Select(D => new ClsOpcionCatalogoLote
                {
                    Id = D.Id,
                    Nombre = D.Nombre
                })
                .ToList();
        }

        // =====================================================
        // CATÁLOGO DE LOTES DE PRODUCTO
        // =====================================================

        public List<ClsOpcionCatalogoLote>
            LogisticaFuncLotesProducto()
        {
            return _Repositorio
                .LogisticaMetObtenerLotesProducto()
                .Select(L => new ClsOpcionCatalogoLote
                {
                    Id = L.Id,
                    Nombre = L.Nombre
                })
                .ToList();
        }

        // =====================================================
        // AGREGAR ASIGNACIÓN
        // =====================================================

        public string LogisticaFuncAgregar(
            ClsDatosDetalleViajeLoteProducto Datos)
        {
            LogisticaProcValidar(Datos);

            ClsDetalleViajeLoteProducto Entidad =
                LogisticaFuncConvertirEntidad(Datos);

            int Resultado =
                _Repositorio.LogisticaMetAgregar(Entidad);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo registrar la asignación.");
            }

            return "Lote asignado correctamente.";
        }

        // =====================================================
        // MODIFICAR ASIGNACIÓN
        // =====================================================

        public string LogisticaFuncEditar(
            ClsDatosDetalleViajeLoteProducto Datos)
        {
            if (Datos == null ||
                Datos.IdDetalleViajeLoteProducto <= 0)
            {
                throw new ValidationException(
                    "Seleccione una asignación válida.");
            }

            LogisticaProcValidar(Datos);

            ClsDetalleViajeLoteProducto Entidad =
                LogisticaFuncConvertirEntidad(Datos);

            int Resultado =
                _Repositorio.LogisticaMetEditar(Entidad);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo modificar la asignación.");
            }

            return "Asignación modificada correctamente.";
        }

        // =====================================================
        // ELIMINAR ASIGNACIÓN
        // =====================================================

        public string LogisticaFuncEliminar(
            long IdDetalleViajeLoteProducto)
        {
            if (IdDetalleViajeLoteProducto <= 0)
            {
                throw new ValidationException(
                    "Seleccione una asignación válida.");
            }

            int Resultado =
                _Repositorio.LogisticaMetEliminar(
                    IdDetalleViajeLoteProducto);

            if (Resultado <= 0)
            {
                throw new Exception(
                    "No se pudo eliminar la asignación.");
            }

            return "Asignación eliminada correctamente.";
        }

        // =====================================================
        // CONVERTIR DATOS A ENTIDAD
        // =====================================================

        private ClsDetalleViajeLoteProducto
            LogisticaFuncConvertirEntidad(
                ClsDatosDetalleViajeLoteProducto Datos)
        {
            return new ClsDetalleViajeLoteProducto
            {
                IdDetalleViajeLoteProducto =
                    Datos.IdDetalleViajeLoteProducto,

                Fk_Detalle_Viaje_Codigo =
                    Datos.FkDetalleViajeCodigo,

                Fk_Lote_Producto_Codigo =
                    Datos.FkLoteProductoCodigo,

                CantidadCajasDetalleViajeLoteProducto =
                    Datos.CantidadCajasDetalleViajeLoteProducto,

                PesoKgDetalleViajeLoteProducto =
                    Datos.PesoKgDetalleViajeLoteProducto
            };
        }

        // =====================================================
        // VALIDACIONES
        // =====================================================

        private void LogisticaProcValidar(
            ClsDatosDetalleViajeLoteProducto Datos)
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

            if (Datos.FkDetalleViajeCodigo <= 0 ||
                Datos.FkLoteProductoCodigo <= 0 ||
                Datos.CantidadCajasDetalleViajeLoteProducto <= 0)
            {
                throw new ValidationException(
                    "Complete todos los campos obligatorios.");
            }

            if (Datos.PesoKgDetalleViajeLoteProducto < 0)
            {
                throw new ValidationException(
                    "El peso no puede ser negativo.");
            }

            // No permitir repetir un lote en el mismo detalle.
            bool Existe = _Repositorio
                .LogisticaMetObtenerTodos()
                .Any(A =>
                    A.Fk_Detalle_Viaje_Codigo ==
                        Datos.FkDetalleViajeCodigo &&

                    A.Fk_Lote_Producto_Codigo ==
                        Datos.FkLoteProductoCodigo &&

                    A.IdDetalleViajeLoteProducto !=
                        Datos.IdDetalleViajeLoteProducto);

            if (Existe)
            {
                throw new ValidationException(
                    "Este lote ya está asignado " +
                    "al detalle de viaje seleccionado.");
            }
        }
    }
}
