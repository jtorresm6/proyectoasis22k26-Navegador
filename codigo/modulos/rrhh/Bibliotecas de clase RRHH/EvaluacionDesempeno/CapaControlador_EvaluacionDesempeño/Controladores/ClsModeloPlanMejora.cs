using CapaControlador_Seguridad;
using CapaModelo_EvaluacionDesempeño.Entidades;
using CapaModelo_EvaluacionDesempeño.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Ventana 9403 - Plan de Mejora. Valida los datos del plan, los
 *  graba según el Estado (Added, Modified, Deleted) y registra la
 *  acción en la bitácora de Seguridad.
 * ===================================================================
*/

namespace CapaControlador_EvaluacionDesempeño.Controladores
{
    public class ClsModeloPlanMejora
    {
        private const string _Tabla = "tblPlanMejora";

        private readonly ClsRepositorioPlanMejora _RepositorioPlanMejora = new ClsRepositorioPlanMejora();

        public EstadoEntidad Estado { private get; set; }

        public int IdPlanMejora { get; set; }
        public string NombrePlanMejora { get; set; }
        public string DescripcionPlanMejora { get; set; }
        public int? IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public int? IdEvaluacionDesempeno { get; set; }
        public string FechaEvaluacion { get; set; }

        public string RrhhMetGrabarCambios()
        {
            string Mensaje = null;
            try
            {
                if (Estado != EstadoEntidad.Deleted)
                {
                    NombrePlanMejora = (NombrePlanMejora ?? "").Trim();
                    DescripcionPlanMejora = (DescripcionPlanMejora ?? "").Trim();

                    if (NombrePlanMejora.Length == 0)
                        return "Debe ingresar el Nombre del Plan.";
                    if (NombrePlanMejora.Length > 100)
                        return "El Nombre del Plan no puede tener más de 100 caracteres.";
                    if (DescripcionPlanMejora.Length == 0)
                        return "Debe ingresar la Descripción.";
                    if (DescripcionPlanMejora.Length > 255)
                        return "La Descripción no puede tener más de 255 caracteres.";
                    if (IdEmpleado.HasValue && !IdEvaluacionDesempeno.HasValue)
                        return "Debe seleccionar la Evaluación del empleado a la que se relaciona el plan.";
                }

                var ModeloDatos = new ClsPlanMejora
                {
                    IdPlanMejora = IdPlanMejora,
                    NombrePlanMejora = NombrePlanMejora,
                    DescripcionPlanMejora = DescripcionPlanMejora,
                    IdEvaluacionDesempeno = IdEvaluacionDesempeno
                };

                switch (Estado)
                {
                    case EstadoEntidad.Added:
                        IdPlanMejora = _RepositorioPlanMejora.RrhhMetAgregar(ModeloDatos);
                        RrhhMetRegistrarBitacora("INSERTAR", "Plan de mejora creado: " + NombrePlanMejora);
                        Mensaje = "Grabacion exitosa";
                        break;

                    case EstadoEntidad.Modified:
                        _RepositorioPlanMejora.RrhhMetEditar(ModeloDatos);
                        RrhhMetRegistrarBitacora("MODIFICAR", "Plan de mejora modificado: " + NombrePlanMejora);
                        Mensaje = "Actualizacion exitosa";
                        break;

                    case EstadoEntidad.Deleted:
                        _RepositorioPlanMejora.RrhhMetRemover(ModeloDatos);
                        RrhhMetRegistrarBitacora("ELIMINAR", "Plan de mejora eliminado: " + NombrePlanMejora);
                        Mensaje = "Eliminacion exitosa";
                        break;
                }
            }
            catch (OdbcException OdbcEx)
            {
                if (OdbcEx.Errors.Count > 0 && OdbcEx.Errors[0].NativeError == 1062)
                    Mensaje = "Ya existe un plan de mejora con ese nombre.";
                else
                    Mensaje = "Error al grabar el plan de mejora: " + OdbcEx.Message;
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al grabar el plan de mejora: " + Ex.Message;
            }
            return Mensaje;
        }

        public List<ClsModeloPlanMejora> RrhhMetObtenerTodos()
        {
            return _RepositorioPlanMejora.RrhhMetObtenerTodos()
                .Select(Item => new ClsModeloPlanMejora
                {
                    IdPlanMejora = Item.IdPlanMejora,
                    NombrePlanMejora = Item.NombrePlanMejora,
                    DescripcionPlanMejora = Item.DescripcionPlanMejora,
                    IdEmpleado = Item.IdEmpleado,
                    NombreEmpleado = Item.NombreEmpleado,
                    IdEvaluacionDesempeno = Item.IdEvaluacionDesempeno,
                    FechaEvaluacion = Item.FechaEvaluacion
                })
                .ToList();
        }

        // Busca sin importar mayúsculas en el nombre, la descripción, el empleado y la fecha de evaluación.
        public List<ClsModeloPlanMejora> RrhhMetBuscar(string Texto)
        {
            var Todos = RrhhMetObtenerTodos();
            if (string.IsNullOrWhiteSpace(Texto)) return Todos;

            string Filtro = Texto.Trim().ToLowerInvariant();
            return Todos.FindAll(Plan =>
                Plan.NombrePlanMejora.ToLowerInvariant().Contains(Filtro) ||
                Plan.DescripcionPlanMejora.ToLowerInvariant().Contains(Filtro) ||
                Plan.NombreEmpleado.ToLowerInvariant().Contains(Filtro) ||
                Plan.FechaEvaluacion.Contains(Filtro));
        }

        // Un valor 0 significa "sin filtro".
        public List<ClsModeloPlanMejora> RrhhMetBuscarPorEmpleado(int IdEmpleado)
        {
            var Todos = RrhhMetObtenerTodos();
            return IdEmpleado <= 0 ? Todos : Todos.FindAll(Plan => Plan.IdEmpleado == IdEmpleado);
        }

        public DataTable RrhhMetObtenerEmpleados()
        {
            return _RepositorioPlanMejora.RrhhMetObtenerEmpleados();
        }

        public DataTable RrhhMetObtenerEvaluaciones(int IdEmpleado, int IdPlanMejora)
        {
            return _RepositorioPlanMejora.RrhhMetObtenerEvaluaciones(IdEmpleado, IdPlanMejora);
        }

        private void RrhhMetRegistrarBitacora(string Accion, string Detalles)
        {
            ClsModeloBitacora.SeguridadMetRegistrarAccion(Accion, _Tabla, IdPlanMejora, Detalles);
        }
    }
}
