using CapaControlador_Nomina.ObjetosdeValor;
using CapaModelo_Nomina.Entidades;
using CapaModelo_Nomina.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_Nomina.Modelos_de_controladores
{
    public class ClsModeloPercepcionDeduccion
    {
        private int _IdPercepcionDeduccion;
        private string _NombrePercepcionDeduccion;
        private string _PeriodoPercepcionDeduccion;
        private bool _EfectoPercepcionDeduccion;
        private string _TipoPercepcionDeduccion;
        private decimal _ValorTipoPercepcionDeduccion;
        private string _AplicacionPercepcionDeduccion;
        private bool _IsActive;
        private DateTime _CreatedAt;
        private DateTime _UpdatedAt;

        // 2. Repositorio privado
        private ClsRepositorioPercepcionDeduccion _RepositorioPercepcionDeduccion;

        // 3. Estado
        public EstadoEntidad Estado { private get; set; }

        // 4. Lista privada para busquedas en memoria
        private List<ClsModeloPercepcionDeduccion> _ListaPercepcionesDeducciones;

        // 5. Propiedades publicas
        // Autoincremental: sin validaciones (se exige solo en Modified y Deleted)
        public int IdPercepcionDeduccion { get => _IdPercepcionDeduccion; set => _IdPercepcionDeduccion = value; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\\s.\\-%]+$", ErrorMessage = "El campo Nombre solo admite letras, números, espacios, punto, guion y %")]
        [StringLength(maximumLength: 100, MinimumLength = 3, ErrorMessage = "El campo Nombre debe tener entre 3 y 100 caracteres")]
        public string NombrePercepcionDeduccion { get => _NombrePercepcionDeduccion; set => _NombrePercepcionDeduccion = value; }

        [Required(ErrorMessage = "El campo Período es requerido")]
        [RegularExpression("^(MENSUAL|ANUAL)$", ErrorMessage = "El Período debe ser MENSUAL o ANUAL")]
        public string PeriodoPercepcionDeduccion { get => _PeriodoPercepcionDeduccion; set => _PeriodoPercepcionDeduccion = value; }

        public bool EfectoPercepcionDeduccion { get => _EfectoPercepcionDeduccion; set => _EfectoPercepcionDeduccion = value; }

        [Required(ErrorMessage = "El campo Tipo es requerido")]
        [RegularExpression("^(Tabla|%|Monto)$", ErrorMessage = "El Tipo debe ser Tabla, % o Monto")]
        public string TipoPercepcionDeduccion { get => _TipoPercepcionDeduccion; set => _TipoPercepcionDeduccion = value; }

        [Range(typeof(decimal), "0", "99999999.9999", ErrorMessage = "El Valor debe estar entre 0 y 99,999,999.9999")]
        public decimal ValorTipoPercepcionDeduccion { get => _ValorTipoPercepcionDeduccion; set => _ValorTipoPercepcionDeduccion = value; }

        [Required(ErrorMessage = "El campo Aplicación es requerido")]
        [RegularExpression("^(Todos|Algunos)$", ErrorMessage = "La Aplicación debe ser Todos o Algunos")]
        public string AplicacionPercepcionDeduccion { get => _AplicacionPercepcionDeduccion; set => _AplicacionPercepcionDeduccion = value; }

        public bool IsActive { get => _IsActive; set => _IsActive = value; }

        public DateTime CreatedAt { get => _CreatedAt; private set => _CreatedAt = value; }
        public DateTime UpdatedAt { get => _UpdatedAt; private set => _UpdatedAt = value; }

        // 6. Constructor
        public ClsModeloPercepcionDeduccion()
        {
            _RepositorioPercepcionDeduccion = new ClsRepositorioPercepcionDeduccion();
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
                    if (_IdPercepcionDeduccion <= 0)
                        return "Debe indicar una percepción/deducción válida para esta operación";
                }

                if (Estado == EstadoEntidad.Added || Estado == EstadoEntidad.Modified)
                {
                    int IdParaExcluir = (Estado == EstadoEntidad.Added) ? 0 : _IdPercepcionDeduccion;
                    int Coincidencias = _RepositorioPercepcionDeduccion.RrhhMetContarPorNombre(_NombrePercepcionDeduccion.Trim(), IdParaExcluir);
                    if (Coincidencias > 0)
                        return "Ya existe una percepción/deducción con el nombre: " + _NombrePercepcionDeduccion;

                    if (_TipoPercepcionDeduccion == "%" && _ValorTipoPercepcionDeduccion > 100)
                        return "Cuando el tipo es %, el valor no puede ser mayor a 100";
                }

                var ModeloDatosPercepcionDeduccion = new ClsPercepcionDeduccion();
                ModeloDatosPercepcionDeduccion.IdPercepcionDeduccion = _IdPercepcionDeduccion;
                ModeloDatosPercepcionDeduccion.NombrePercepcionDeduccion = _NombrePercepcionDeduccion?.Trim();
                ModeloDatosPercepcionDeduccion.PeriodoPercepcionDeduccion = _PeriodoPercepcionDeduccion;
                ModeloDatosPercepcionDeduccion.EfectoPercepcionDeduccion = _EfectoPercepcionDeduccion;
                ModeloDatosPercepcionDeduccion.TipoPercepcionDeduccion = _TipoPercepcionDeduccion;
                ModeloDatosPercepcionDeduccion.ValorTipoPercepcionDeduccion = _ValorTipoPercepcionDeduccion;
                ModeloDatosPercepcionDeduccion.AplicacionPercepcionDeduccion = _AplicacionPercepcionDeduccion;
                ModeloDatosPercepcionDeduccion.IsActive = _IsActive;

                switch (Estado)
                {
                    case EstadoEntidad.Added:
                        _RepositorioPercepcionDeduccion.RrhhMetAgregar(ModeloDatosPercepcionDeduccion);
                        Mensaje = "Grabación exitosa";
                        break;
                    case EstadoEntidad.Modified:
                        _RepositorioPercepcionDeduccion.RrhhMetEditar(ModeloDatosPercepcionDeduccion);
                        Mensaje = "Actualización exitosa";
                        break;
                    case EstadoEntidad.Deleted:
                        _RepositorioPercepcionDeduccion.RrhhMetRemover(ModeloDatosPercepcionDeduccion);
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

        public List<ClsModeloPercepcionDeduccion> RrhhMetObtenerTodos()
        {
            var ModeloDatosPercepcionDeduccion = _RepositorioPercepcionDeduccion.RrhhMetObtenerTodos();
            _ListaPercepcionesDeducciones = new List<ClsModeloPercepcionDeduccion>();
            foreach (ClsPercepcionDeduccion Item in ModeloDatosPercepcionDeduccion)
            {
                _ListaPercepcionesDeducciones.Add(new ClsModeloPercepcionDeduccion
                {
                    _IdPercepcionDeduccion = Item.IdPercepcionDeduccion,
                    _NombrePercepcionDeduccion = Item.NombrePercepcionDeduccion,
                    _PeriodoPercepcionDeduccion = Item.PeriodoPercepcionDeduccion,
                    _EfectoPercepcionDeduccion = Item.EfectoPercepcionDeduccion,
                    _TipoPercepcionDeduccion = Item.TipoPercepcionDeduccion,
                    _ValorTipoPercepcionDeduccion = Item.ValorTipoPercepcionDeduccion,
                    _AplicacionPercepcionDeduccion = Item.AplicacionPercepcionDeduccion,
                    _IsActive = Item.IsActive,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt
                });
            }
            return _ListaPercepcionesDeducciones;
        }

        public ClsModeloPercepcionDeduccion RrhhMetBuscarPorId(int IdPercepcionDeduccion)
        {
            if (_ListaPercepcionesDeducciones == null)
                RrhhMetObtenerTodos();

            return _ListaPercepcionesDeducciones.FirstOrDefault(e => e._IdPercepcionDeduccion == IdPercepcionDeduccion);
        }

        public IEnumerable<ClsModeloPercepcionDeduccion> RrhhMetBuscarPorNombre(string NombrePercepcionDeduccion)
        {
            if (_ListaPercepcionesDeducciones == null)
                RrhhMetObtenerTodos();

            return _ListaPercepcionesDeducciones.FindAll(e =>
                e._NombrePercepcionDeduccion.ToUpper().Contains(NombrePercepcionDeduccion.ToUpper()));
        }

        // Convierte errores de BD en mensajes legibles
        private string RrhhMetObtenerMensajeError(Exception Ex)
        {
            string Texto = Ex.Message ?? string.Empty;

            if (Texto.IndexOf("Duplicate entry", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Ya existe una percepción/deducción con ese nombre.";
            if (Texto.IndexOf("foreign key", StringComparison.OrdinalIgnoreCase) >= 0)
                return "No se puede completar la operación porque el registro está relacionado con otros datos.";
            if (Ex is System.Data.Odbc.OdbcException)
                return "Error de base de datos: " + Texto;

            return "Ocurrió un error inesperado: " + Texto;
        }
    }
}
