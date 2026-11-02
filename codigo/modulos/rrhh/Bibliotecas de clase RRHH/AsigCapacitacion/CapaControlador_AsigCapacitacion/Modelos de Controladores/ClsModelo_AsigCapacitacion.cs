using CapaControlador_Seguridad;
using CapaModelo_AsigCapacitacion;
using CapaModelo_AsigCapacitacion.Entidades;
using CapaModelo_AsigCapacitacion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;

/*
 * ==================================================================
 * Área : RRHH
 * Autor : Carlos David Calderón Ramirez
 * Carné : 9959-23-848
 * Fecha : 31/10/2026
 * ==================================================================
 * Propósito :
 *  El formulario del módulo de Asignacion Capacitacion nos 
 *  permite registrar, consultar, modificar y eliminar los 
 *  datos ingresados al sistema, tambien cuenta con Reportes y Ayudas.
 * ===================================================================
*/

namespace CapaControlador_AsigCapacitacion.Modelos_de_Controladores
{
    public class ClsModeloAsigCapacitacion
    {
        private int _IdAsignacionCapacitacion;
        private int _IdEmpleado;
        private string _NombreEmpleado;
        private int _IdCapacitacion;
        private string _NombreCapacitacion;
        private string _NombreInstructor;
        private DateTime _FechaAsignacionCapacitacion;

        private ClsRepositorioAsigCapacitacion _RepositorioAsigCapacitacion;
        private List<ClsModeloAsigCapacitacion> _ListaAsigCapacitacion;

        public EstadoEntidad Estado { private get; set; }

        public int IdAsignacionCapacitacion { get => _IdAsignacionCapacitacion; set => _IdAsignacionCapacitacion = value; }
        public int IdEmpleado { get => _IdEmpleado; set => _IdEmpleado = value; }
        public string NombreEmpleado { get => _NombreEmpleado; set => _NombreEmpleado = value; }
        public int IdCapacitacion { get => _IdCapacitacion; set => _IdCapacitacion = value; }
        public string NombreCapacitacion { get => _NombreCapacitacion; set => _NombreCapacitacion = value; }
        public string NombreInstructor { get => _NombreInstructor; set => _NombreInstructor = value; }
        public DateTime FechaAsignacionCapacitacion { get => _FechaAsignacionCapacitacion; set => _FechaAsignacionCapacitacion = value; }

        public ClsModeloAsigCapacitacion()
        {
            _RepositorioAsigCapacitacion = new ClsRepositorioAsigCapacitacion();
        }

        public string RrhhMetGrabarCambios()
        {
            string Mensaje = null;
            try
            {
                if (Estado != EstadoEntidad.Deleted)
                {
                    if (_IdEmpleado <= 0)
                        return "Debe seleccionar un Empleado.";
                    if (_IdCapacitacion <= 0)
                        return "Debe seleccionar una Capacitación.";
                }

                var ModeloDatos = new ClsAsigCapacitacion();
                ModeloDatos.IdAsignacionCapacitacion = _IdAsignacionCapacitacion;
                ModeloDatos.IdEmpleado = _IdEmpleado;
                ModeloDatos.IdCapacitacion = _IdCapacitacion;
                ModeloDatos.FechaAsignacionCapacitacion = _FechaAsignacionCapacitacion;

                switch (Estado)
                {
                    case EstadoEntidad.Added:
                        _RepositorioAsigCapacitacion.RrhhMetAgregar(ModeloDatos);
                        Mensaje = "Grabacion exitosa";
                        break;

                    case EstadoEntidad.Modified:
                        _RepositorioAsigCapacitacion.RrhhMetEditar(ModeloDatos);
                        Mensaje = "Actualizacion exitosa";
                        break;

                    case EstadoEntidad.Deleted:
                        _RepositorioAsigCapacitacion.RrhhMetRemover(ModeloDatos);
                        Mensaje = "Eliminacion exitosa";
                        break;
                }
            }
            catch (OdbcException OdbcEx)
            {
                if (OdbcEx.Errors.Count > 0 && OdbcEx.Errors[0].NativeError == 1062)
                    Mensaje = "El empleado ya está asignado a esta capacitación.";
                else
                    Mensaje = OdbcEx.ToString();
            }
            catch (Exception Ex)
            {
                Mensaje = Ex.ToString();
            }
            return Mensaje;
        }

        public List<ClsModeloAsigCapacitacion> RrhhMetObtenerTodos()
        {
            var Resultado = _RepositorioAsigCapacitacion.RrhhMetObtenerTodos();
            _ListaAsigCapacitacion = new List<ClsModeloAsigCapacitacion>();
            foreach (ClsAsigCapacitacion Item in Resultado)
            {
                _ListaAsigCapacitacion.Add(new ClsModeloAsigCapacitacion
                {
                    _IdAsignacionCapacitacion = Item.IdAsignacionCapacitacion,
                    _IdEmpleado = Item.IdEmpleado,
                    _NombreEmpleado = Item.NombreEmpleado,
                    _IdCapacitacion = Item.IdCapacitacion,
                    _NombreCapacitacion = Item.NombreCapacitacion,
                    _NombreInstructor = Item.NombreInstructor,
                    _FechaAsignacionCapacitacion = Item.FechaAsignacionCapacitacion
                });
            }
            return _ListaAsigCapacitacion;
        }

        // Un valor 0 significa "sin filtro"
        public List<ClsModeloAsigCapacitacion> RrhhMetBuscarPorFiltro(int IdEmpleado, int IdCapacitacion)
        {
            var Todos = RrhhMetObtenerTodos();
            return Todos.FindAll(e =>
                (IdEmpleado <= 0 || e._IdEmpleado == IdEmpleado) &&
                (IdCapacitacion <= 0 || e._IdCapacitacion == IdCapacitacion));
        }

        public DataTable RrhhMetObtenerEmpleados()
        {
            return _RepositorioAsigCapacitacion.RrhhMetObtenerEmpleados();
        }

        public DataTable RrhhMetObtenerCapacitaciones()
        {
            return _RepositorioAsigCapacitacion.RrhhMetObtenerCapacitaciones();
        }
    }
}