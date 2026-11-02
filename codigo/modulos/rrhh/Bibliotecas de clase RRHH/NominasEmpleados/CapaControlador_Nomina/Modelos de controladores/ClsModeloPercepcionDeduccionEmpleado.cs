using CapaControlador_Nomina.ObjetosdeValor;
using CapaModelo_Nomina.Entidades;
using CapaModelo_Nomina.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_Nomina.Modelos_de_controladores
{
    public class ClsModeloPercepcionDeduccionEmpleado
    {
        private int _IdPercepcionDeduccionEmpleado;
        private int _IdPercepcionDeduccion;
        private int? _IdEmpleado;
        private int? _IdDepartamento;
        private bool _IsActive;
        private DateTime _CreatedAt;
        private DateTime _UpdatedAt;

        // 2. Repositorio privado
        private ClsRepositorioPercepcionDeduccionEmpleado _RepositorioPercepcionDeduccionEmpleado;

        // 3. Estado
        public EstadoEntidad Estado { private get; set; }

        // 4. Lista privada para busquedas en memoria
        private List<ClsModeloPercepcionDeduccionEmpleado> _ListaAsignaciones;

        // 5. Propiedades publicas
        // Autoincremental: sin validaciones (se exige solo en Modified y Deleted)
        public int IdPercepcionDeduccionEmpleado { get => _IdPercepcionDeduccionEmpleado; set => _IdPercepcionDeduccionEmpleado = value; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una percepción/deducción")]
        public int IdPercepcionDeduccion { get => _IdPercepcionDeduccion; set => _IdPercepcionDeduccion = value; }

        // Opcional: se valida en conjunto con IdDepartamento (al menos uno es obligatorio)
        public int? IdEmpleado { get => _IdEmpleado; set => _IdEmpleado = value; }

        // Opcional: se valida en conjunto con IdEmpleado (al menos uno es obligatorio)
        public int? IdDepartamento { get => _IdDepartamento; set => _IdDepartamento = value; }

        public bool IsActive { get => _IsActive; set => _IsActive = value; }

        public DateTime CreatedAt { get => _CreatedAt; private set => _CreatedAt = value; }
        public DateTime UpdatedAt { get => _UpdatedAt; private set => _UpdatedAt = value; }

        // 6. Constructor
        public ClsModeloPercepcionDeduccionEmpleado()
        {
            _RepositorioPercepcionDeduccionEmpleado = new ClsRepositorioPercepcionDeduccionEmpleado();
        }

        // 7. Metodos publicos
        public string RrhhMetGrabarCambios()
        {
            string Mensaje = null;
            try
            {
                // Reglas de negocio (antes del switch)
                if (Estado == EstadoEntidad.Modified || Estado == EstadoEntidad.Deleted)
                {
                    if (_IdPercepcionDeduccionEmpleado <= 0)
                        return "Debe indicar una asignación válida para esta operación";
                }

                if (Estado == EstadoEntidad.Added || Estado == EstadoEntidad.Modified)
                {
                    // Un valor 0 o negativo (ej. combo sin seleccion) se trata como "sin asignar"
                    if (_IdEmpleado.HasValue && _IdEmpleado.Value <= 0)
                        _IdEmpleado = null;
                    if (_IdDepartamento.HasValue && _IdDepartamento.Value <= 0)
                        _IdDepartamento = null;

                    if (_IdPercepcionDeduccion <= 0)
                        return "Debe seleccionar una percepción/deducción";

                    // Regla principal: se asigna a un empleado o a un departamento, nunca a ambos ni a ninguno
                    if (!_IdEmpleado.HasValue && !_IdDepartamento.HasValue)
                        return "Debe asignar la percepción/deducción a un empleado o a un departamento";
                    if (_IdEmpleado.HasValue && _IdDepartamento.HasValue)
                        return "La percepción/deducción se asigna a un empleado o a un departamento, no a ambos";

                    int IdParaExcluir = (Estado == EstadoEntidad.Added) ? 0 : _IdPercepcionDeduccionEmpleado;

                    if (_IdEmpleado.HasValue)
                    {
                        int CoincidenciasEmpleado = _RepositorioPercepcionDeduccionEmpleado
                            .RrhhMetContarPorEmpleado(_IdPercepcionDeduccion, _IdEmpleado.Value, IdParaExcluir);
                        if (CoincidenciasEmpleado > 0)
                            return "El empleado ya tiene asignada esta percepción/deducción";
                    }

                    if (_IdDepartamento.HasValue)
                    {
                        int CoincidenciasDepartamento = _RepositorioPercepcionDeduccionEmpleado
                            .RrhhMetContarPorDepartamento(_IdPercepcionDeduccion, _IdDepartamento.Value, IdParaExcluir);
                        if (CoincidenciasDepartamento > 0)
                            return "El departamento ya tiene asignada esta percepción/deducción";
                    }
                }

                var ModeloDatosAsignacion = new ClsPercepcionDeduccionEmpleado();
                ModeloDatosAsignacion.IdPercepcionDeduccionEmpleado = _IdPercepcionDeduccionEmpleado;
                ModeloDatosAsignacion.IdPercepcionDeduccion = _IdPercepcionDeduccion;
                ModeloDatosAsignacion.IdEmpleado = _IdEmpleado;
                ModeloDatosAsignacion.IdDepartamento = _IdDepartamento;
                ModeloDatosAsignacion.IsActive = _IsActive;

                switch (Estado)
                {
                    case EstadoEntidad.Added:
                        _RepositorioPercepcionDeduccionEmpleado.RrhhMetAgregar(ModeloDatosAsignacion);
                        Mensaje = "Grabación exitosa";
                        break;
                    case EstadoEntidad.Modified:
                        _RepositorioPercepcionDeduccionEmpleado.RrhhMetEditar(ModeloDatosAsignacion);
                        Mensaje = "Actualización exitosa";
                        break;
                    case EstadoEntidad.Deleted:
                        _RepositorioPercepcionDeduccionEmpleado.RrhhMetRemover(ModeloDatosAsignacion);
                        Mensaje = "Eliminación exitosa";
                        break;
                }
            }
            catch (Exception Ex)
            {
                Mensaje = RrhhMetObtenerMensajeError(Ex);
            }
            return Mensaje;
        }

        public List<ClsModeloPercepcionDeduccionEmpleado> RrhhMetObtenerTodos()
        {
            var ModeloDatosAsignacion = _RepositorioPercepcionDeduccionEmpleado.RrhhMetObtenerTodos();
            _ListaAsignaciones = new List<ClsModeloPercepcionDeduccionEmpleado>();
            foreach (ClsPercepcionDeduccionEmpleado Item in ModeloDatosAsignacion)
            {
                _ListaAsignaciones.Add(new ClsModeloPercepcionDeduccionEmpleado
                {
                    _IdPercepcionDeduccionEmpleado = Item.IdPercepcionDeduccionEmpleado,
                    _IdPercepcionDeduccion = Item.IdPercepcionDeduccion,
                    _IdEmpleado = Item.IdEmpleado,
                    _IdDepartamento = Item.IdDepartamento,
                    _IsActive = Item.IsActive,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt
                });
            }
            return _ListaAsignaciones;
        }

        public ClsModeloPercepcionDeduccionEmpleado RrhhMetBuscarPorId(int IdPercepcionDeduccionEmpleado)
        {
            if (_ListaAsignaciones == null)
                RrhhMetObtenerTodos();

            return _ListaAsignaciones.FirstOrDefault(e => e._IdPercepcionDeduccionEmpleado == IdPercepcionDeduccionEmpleado);
        }

        public IEnumerable<ClsModeloPercepcionDeduccionEmpleado> RrhhMetBuscarPorPercepcionDeduccion(int IdPercepcionDeduccion)
        {
            if (_ListaAsignaciones == null)
                RrhhMetObtenerTodos();

            return _ListaAsignaciones.FindAll(e => e._IdPercepcionDeduccion == IdPercepcionDeduccion);
        }

        public IEnumerable<ClsModeloPercepcionDeduccionEmpleado> RrhhMetBuscarPorEmpleado(int IdEmpleado)
        {
            if (_ListaAsignaciones == null)
                RrhhMetObtenerTodos();

            return _ListaAsignaciones.FindAll(e => e._IdEmpleado == IdEmpleado);
        }

        public IEnumerable<ClsModeloPercepcionDeduccionEmpleado> RrhhMetBuscarPorDepartamento(int IdDepartamento)
        {
            if (_ListaAsignaciones == null)
                RrhhMetObtenerTodos();

            return _ListaAsignaciones.FindAll(e => e._IdDepartamento == IdDepartamento);
        }

        // Convierte errores de BD en mensajes legibles
        private string RrhhMetObtenerMensajeError(Exception Ex)
        {
            string Texto = Ex.Message ?? string.Empty;

            if (Texto.IndexOf("Duplicate entry", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (Texto.IndexOf("uqPercepcionDeduccionEmpleadoDepartamento", StringComparison.OrdinalIgnoreCase) >= 0)
                    return "El departamento ya tiene asignada esta percepción/deducción.";
                if (Texto.IndexOf("uqPercepcionDeduccionEmpleadoEmpleado", StringComparison.OrdinalIgnoreCase) >= 0)
                    return "El empleado ya tiene asignada esta percepción/deducción.";
                return "Ya existe una asignación con esos datos.";
            }
            if (Texto.IndexOf("foreign key", StringComparison.OrdinalIgnoreCase) >= 0)
                return "El empleado, departamento o percepción/deducción indicado no existe, o el registro está relacionado con otros datos.";
            if (Ex is System.Data.Odbc.OdbcException)
                return "Error de base de datos: " + Texto;

            return "Ocurrió un error inesperado: " + Texto;
        }
    }
}
