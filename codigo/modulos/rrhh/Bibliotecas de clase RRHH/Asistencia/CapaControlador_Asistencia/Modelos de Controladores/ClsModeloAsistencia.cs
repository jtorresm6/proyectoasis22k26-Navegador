using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaModelo_Asistencia.Entidades;
using CapaModelo_Asistencia.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Controlador de la ventana 9101 de Asistencia. Valida y graba los
 *  cambios en tblAsistenciaEmpleado y le pasa a la vista lo que se
 *  revisa de otras ventanas: empleados con jornada y baja, permisos
 *  aprobados, vacaciones y capacitaciones. Antes de grabar revisa que
 *  el usuario en sesión tenga el permiso de la acción en el módulo
 *  9000, aplicación 9101.
 * ===================================================================
*/

namespace CapaControlador_Asistencia.Modelos_de_Controladores
{
    public class ClsModeloAsistencia
    {
        public const int IdModulo = 9000;
        public const int IdAplicacion = 9101;
        private static readonly string[] EstadosValidos = { "PRESENTE", "TARDANZA", "AUSENTE", "PERMISO" };

        private int _IdAsistenciaEmpleado;
        private int _IdEmpleado;
        private string _CodigoEmpleado;
        private string _NombreEmpleado;
        private string _DepartamentoEmpleado;
        private string _PuestoEmpleado;
        private DateTime _FechaAsistencia;
        private TimeSpan? _HoraEntrada;
        private TimeSpan? _HoraSalida;
        private string _EstadoAsistencia;
        private string _Observaciones;
        private DateTime _CreatedAt;
        private DateTime _UpdatedAt;

        private ClsRepositorioAsistencia _RepositorioAsistencia;
        private List<ClsModeloAsistencia> _ListaAsistencias;
        private ClsPermisoAplicacion _PermisosUsuario;

        public EstadoEntidad Estado { private get; set; }
        public bool Grabado { get; private set; }

        public int IdAsistenciaEmpleado { get => _IdAsistenciaEmpleado; set => _IdAsistenciaEmpleado = value; }
        public int IdEmpleado { get => _IdEmpleado; set => _IdEmpleado = value; }
        public string CodigoEmpleado { get => _CodigoEmpleado; set => _CodigoEmpleado = value; }
        public string NombreEmpleado { get => _NombreEmpleado; set => _NombreEmpleado = value; }
        public string DepartamentoEmpleado { get => _DepartamentoEmpleado; set => _DepartamentoEmpleado = value; }
        public string PuestoEmpleado { get => _PuestoEmpleado; set => _PuestoEmpleado = value; }
        public DateTime FechaAsistencia { get => _FechaAsistencia; set => _FechaAsistencia = value; }
        public TimeSpan? HoraEntrada { get => _HoraEntrada; set => _HoraEntrada = value; }
        public TimeSpan? HoraSalida { get => _HoraSalida; set => _HoraSalida = value; }
        // En la vista va como "Presente", en la BD como "PRESENTE"
        public string EstadoAsistencia { get => _EstadoAsistencia; set => _EstadoAsistencia = value; }
        public string Observaciones { get => _Observaciones; set => _Observaciones = value; }
        public DateTime CreatedAt { get => _CreatedAt; set => _CreatedAt = value; }
        public DateTime UpdatedAt { get => _UpdatedAt; set => _UpdatedAt = value; }

        public ClsModeloAsistencia()
        {
            _RepositorioAsistencia = new ClsRepositorioAsistencia();
        }

        public string RrhhMetGrabarCambios()
        {
            string Mensaje = null;
            Grabado = false;
            try
            {
                // Primero los datos, luego el permiso y al final lo que se revisa en la BD
                if (Estado != EstadoEntidad.Deleted)
                {
                    string Error = RrhhMetValidarDatos();
                    if (Error != null) return Error;
                }

                string SinPermiso = RrhhMetRevisarPermiso();
                if (SinPermiso != null) return SinPermiso;

                if (Estado != EstadoEntidad.Deleted)
                {
                    DateTime? FechaBaja = _RepositorioAsistencia.RrhhMetObtenerFechaBaja(_IdEmpleado);
                    if (FechaBaja.HasValue && _FechaAsistencia.Date >= FechaBaja.Value.Date)
                        return $"El empleado fue dado de baja el {FechaBaja.Value:dd/MM/yyyy}, no se le puede registrar asistencia desde esa fecha.";
                }

                var ModeloDatos = new ClsAsistenciaEmpleado();
                ModeloDatos.IdAsistenciaEmpleado = _IdAsistenciaEmpleado;
                ModeloDatos.IdEmpleado = _IdEmpleado;
                ModeloDatos.FechaAsistenciaEmpleado = _FechaAsistencia.Date;
                ModeloDatos.HoraEntradaAsistenciaEmpleado = _HoraEntrada;
                ModeloDatos.HoraSalidaAsistenciaEmpleado = _HoraSalida;
                ModeloDatos.EstadoAsistenciaEmpleado = (_EstadoAsistencia ?? "").Trim().ToUpperInvariant();
                ModeloDatos.ObservacionAsistenciaEmpleado = (_Observaciones ?? "").Trim();

                switch (Estado)
                {
                    case EstadoEntidad.Added:
                        _IdAsistenciaEmpleado = _RepositorioAsistencia.RrhhMetAgregar(ModeloDatos);
                        Mensaje = "La asistencia se guardó correctamente.";
                        break;

                    case EstadoEntidad.Modified:
                        _RepositorioAsistencia.RrhhMetEditar(ModeloDatos);
                        Mensaje = "La asistencia se modificó correctamente.";
                        break;

                    case EstadoEntidad.Deleted:
                        _RepositorioAsistencia.RrhhMetRemover(ModeloDatos);
                        Mensaje = "La asistencia se eliminó correctamente.";
                        break;
                }
                Grabado = true;
            }
            catch (OdbcException OdbcEx)
            {
                int Codigo = OdbcEx.Errors.Count > 0 ? OdbcEx.Errors[0].NativeError : 0;
                if (Codigo == 1062)
                    Mensaje = "Este empleado ya tiene asistencia registrada en esa fecha.";
                else if (Codigo == 1451)
                    Mensaje = "No se puede eliminar, la asistencia ya tiene horas extra registradas.";
                else if (Codigo == 3819 || Codigo == 4025)
                    Mensaje = "La hora de salida debe ser después de la hora de entrada.";
                else
                    Mensaje = RrhhMetMensajeError(OdbcEx);
            }
            catch (Exception Ex)
            {
                Mensaje = RrhhMetMensajeError(Ex);
            }
            return Mensaje;
        }

        // Permisos combinados de los roles del usuario que inició sesión
        public ClsPermisoAplicacion RrhhMetPermisosUsuario()
        {
            if (_PermisosUsuario != null) return _PermisosUsuario;
            try
            {
                var ModeloPermisos = new ClsModeloAsigAppPerf();
                ModeloPermisos.SeguridadMetObtenerTodos();
                _PermisosUsuario = ModeloPermisos.SeguridadMetObtenerPermisosSesion(IdModulo, IdAplicacion);
            }
            catch (Exception)
            {
                _PermisosUsuario = new ClsPermisoAplicacion();
            }
            return _PermisosUsuario;
        }

        private string RrhhMetRevisarPermiso()
        {
            ClsPermisoAplicacion Permisos = RrhhMetPermisosUsuario();
            if (!Permisos.TieneAcceso)
                return "No tiene acceso a la ventana de Asistencia.";
            if (Estado == EstadoEntidad.Added && !Permisos.PuedeInsertar)
                return "No tiene permiso para registrar asistencias.";
            if (Estado == EstadoEntidad.Modified && !Permisos.PuedeEditar)
                return "No tiene permiso para modificar asistencias.";
            if (Estado == EstadoEntidad.Deleted && !Permisos.PuedeEliminar)
                return "No tiene permiso para eliminar asistencias.";
            return null;
        }

        public static string RrhhMetMensajeError(Exception Ex)
        {
            if (Ex is OdbcException OdbcEx && OdbcEx.Errors.Count > 0)
            {
                string Estado = OdbcEx.Errors[0].SQLState ?? "";
                int Codigo = OdbcEx.Errors[0].NativeError;
                // IM002: no existe el DSN, 08xxx y 2002/2003/2013: no hay conexión con el servidor
                if (Estado == "IM002" || Estado.StartsWith("08") || Codigo == 2002 || Codigo == 2003 || Codigo == 2013)
                    return "No se pudo conectar con la base de datos. Revise la conexión ODBC (EmbutidosS.A) e intente de nuevo.";
                return "Ocurrió un error en la base de datos: " + OdbcEx.Errors[0].Message;
            }
            return "Ocurrió un error inesperado: " + Ex.Message;
        }

        // Reglas que no necesitan la BD; devuelve null si todo está bien
        public string RrhhMetValidarDatos()
        {
            if (_IdEmpleado <= 0)
                return "Debe seleccionar un empleado.";
            if (_FechaAsistencia.Date > DateTime.Today)
                return "La fecha no puede ser futura.";
            if (_HoraSalida.HasValue && !_HoraEntrada.HasValue)
                return "Marque primero la hora de entrada.";
            if (_HoraSalida.HasValue && _HoraSalida.Value <= _HoraEntrada.Value)
                return "La hora de salida debe ser después de la hora de entrada.";
            string EstadoBD = (_EstadoAsistencia ?? "").Trim().ToUpperInvariant();
            if (Array.IndexOf(EstadosValidos, EstadoBD) < 0)
                return "Seleccione un estado válido.";
            if (EstadoBD == "TARDANZA" && !_HoraEntrada.HasValue)
                return "Para marcar tardanza se necesita la hora de entrada.";
            if (EstadoBD == "AUSENTE" && _HoraEntrada.HasValue)
                return "Si el empleado está ausente no lleva hora de entrada.";
            if ((_Observaciones ?? "").Trim().Length > 255)
                return "Las observaciones no pueden pasar de 255 caracteres.";
            return null;
        }

        public List<ClsModeloAsistencia> RrhhMetObtenerTodos()
        {
            var Resultado = _RepositorioAsistencia.RrhhMetObtenerTodos();
            _ListaAsistencias = new List<ClsModeloAsistencia>();
            foreach (ClsAsistenciaEmpleado Item in Resultado)
            {
                _ListaAsistencias.Add(new ClsModeloAsistencia
                {
                    _IdAsistenciaEmpleado = Item.IdAsistenciaEmpleado,
                    _IdEmpleado = Item.IdEmpleado,
                    _CodigoEmpleado = Item.CodigoEmpleado,
                    _NombreEmpleado = Item.NombreEmpleado,
                    _DepartamentoEmpleado = Item.DepartamentoEmpleado,
                    _PuestoEmpleado = Item.PuestoEmpleado,
                    _FechaAsistencia = Item.FechaAsistenciaEmpleado,
                    _HoraEntrada = Item.HoraEntradaAsistenciaEmpleado,
                    _HoraSalida = Item.HoraSalidaAsistenciaEmpleado,
                    _EstadoAsistencia = RrhhMetEstadoVista(Item.EstadoAsistenciaEmpleado),
                    _Observaciones = Item.ObservacionAsistenciaEmpleado,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt
                });
            }
            return _ListaAsistencias;
        }

        // Las columnas van con los mismos nombres que usa la vista
        public DataTable RrhhMetObtenerEmpleados()
        {
            var Empleados = new DataTable();
            Empleados.Columns.Add("idEmpleado", typeof(int));
            Empleados.Columns.Add("codigoEmpleado", typeof(string));
            Empleados.Columns.Add("nombreEmpleado", typeof(string));
            Empleados.Columns.Add("departamentoEmpleado", typeof(string));
            Empleados.Columns.Add("puestoEmpleado", typeof(string));
            Empleados.Columns.Add("tipoPuesto", typeof(string));
            Empleados.Columns.Add("jornada", typeof(string));
            Empleados.Columns.Add("horaInicioJornada", typeof(TimeSpan));
            Empleados.Columns.Add("horaFinJornada", typeof(TimeSpan));
            Empleados.Columns.Add("minutosTolerancia", typeof(int));
            Empleados.Columns.Add("fechaContratacion", typeof(DateTime));
            Empleados.Columns.Add("fechaBaja", typeof(DateTime));
            Empleados.Columns.Add("salario", typeof(decimal));

            foreach (ClsEmpleadoAsistencia Item in _RepositorioAsistencia.RrhhMetObtenerEmpleados())
            {
                string Jornada = $"{Item.NombreJornada} ({Item.HoraInicioJornada:hh\\:mm} - {Item.HoraFinJornada:hh\\:mm})";
                Empleados.Rows.Add(Item.IdEmpleado, Item.CodigoEmpleado, Item.NombreEmpleado, Item.DepartamentoEmpleado,
                                   Item.PuestoEmpleado, Item.TipoPuesto, Jornada, Item.HoraInicioJornada,
                                   Item.HoraFinJornada, Item.MinutosToleranciaJornada, Item.FechaContratacionEmpleado.Date,
                                   Item.FechaBajaEmpleado.HasValue ? (object)Item.FechaBajaEmpleado.Value.Date : DBNull.Value,
                                   Item.SalarioEmpleado);
            }
            return Empleados;
        }

        public DataTable RrhhMetObtenerPermisos()
        {
            var Permisos = new DataTable();
            Permisos.Columns.Add("idPermiso", typeof(int));
            Permisos.Columns.Add("idEmpleado", typeof(int));
            Permisos.Columns.Add("tipoPermiso", typeof(string));
            Permisos.Columns.Add("fechaInicioPermiso", typeof(DateTime));
            Permisos.Columns.Add("fechaFinPermiso", typeof(DateTime));
            Permisos.Columns.Add("motivoPermiso", typeof(string));
            Permisos.Columns.Add("estadoPermiso", typeof(string));
            Permisos.Columns.Add("unidadPermiso", typeof(string));
            Permisos.Columns.Add("horaInicioPermiso", typeof(TimeSpan));
            Permisos.Columns.Add("horaFinPermiso", typeof(TimeSpan));

            foreach (ClsPermisoAsistencia Item in _RepositorioAsistencia.RrhhMetObtenerPermisosAprobados())
            {
                Permisos.Rows.Add(Item.IdPermisoEmpleado, Item.IdEmpleado, Item.NombreTipoPermiso,
                                  Item.FechaInicioPermisoEmpleado.Date, Item.FechaFinPermisoEmpleado.Date,
                                  Item.MotivoPermisoEmpleado, Item.EstadoPermisoEmpleado, Item.UnidadPermisoEmpleado,
                                  Item.HoraInicioPermisoEmpleado.HasValue ? (object)Item.HoraInicioPermisoEmpleado.Value : DBNull.Value,
                                  Item.HoraFinPermisoEmpleado.HasValue ? (object)Item.HoraFinPermisoEmpleado.Value : DBNull.Value);
            }
            return Permisos;
        }

        public DataTable RrhhMetObtenerCapacitaciones()
        {
            var Capacitaciones = new DataTable();
            Capacitaciones.Columns.Add("idEmpleado", typeof(int));
            Capacitaciones.Columns.Add("nombreCapacitacion", typeof(string));
            Capacitaciones.Columns.Add("fechaInicio", typeof(DateTime));
            Capacitaciones.Columns.Add("fechaFin", typeof(DateTime));

            foreach (ClsCapacitacionAsistencia Item in _RepositorioAsistencia.RrhhMetObtenerCapacitaciones())
            {
                Capacitaciones.Rows.Add(Item.IdEmpleado, Item.NombreCapacitacion,
                                        Item.FechaInicioCapacitacion.Date, Item.FechaFinCapacitacion.Date);
            }
            return Capacitaciones;
        }

        private static string RrhhMetEstadoVista(string EstadoBD)
        {
            if (string.IsNullOrEmpty(EstadoBD)) return "";
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(EstadoBD.ToLowerInvariant());
        }
    }
}
