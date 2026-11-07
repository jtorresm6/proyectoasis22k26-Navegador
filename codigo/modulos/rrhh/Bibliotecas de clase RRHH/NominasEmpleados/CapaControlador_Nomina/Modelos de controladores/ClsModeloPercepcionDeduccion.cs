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
        private ClsRepositorioPercepcionDeduccion _RepositorioPercepcionDeduccion;

        public EstadoEntidad Estado { private get; set; }
        private List<ClsModeloPercepcionDeduccion> _ListaPercepcionDeduccion;

        // Autoincrement
        public int IdPercepcionDeduccion { get => _IdPercepcionDeduccion; set => _IdPercepcionDeduccion = value; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [RegularExpression("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ\\s\\.\\-/%()]+$", ErrorMessage = "El campo Nombre contiene caracteres no permitidos")]
        [StringLength(maximumLength: 100, MinimumLength = 3, ErrorMessage = "El campo Nombre debe tener entre 3 y 100 caracteres")]
        public string NombrePercepcionDeduccion { get => _NombrePercepcionDeduccion; set => _NombrePercepcionDeduccion = value; }

        [Required(ErrorMessage = "Debe seleccionar un Período")]
        [RegularExpression("^(MENSUAL|ANUAL)$", ErrorMessage = "El Período debe ser MENSUAL o ANUAL")]
        public string PeriodoPercepcionDeduccion { get => _PeriodoPercepcionDeduccion; set => _PeriodoPercepcionDeduccion = value; }

        public bool EfectoPercepcionDeduccion { get => _EfectoPercepcionDeduccion; set => _EfectoPercepcionDeduccion = value; }

        [Required(ErrorMessage = "Debe seleccionar un Tipo")]
        [RegularExpression("^(Tabla|%|Monto)$", ErrorMessage = "El Tipo debe ser Tabla, % o Monto")]
        public string TipoPercepcionDeduccion { get => _TipoPercepcionDeduccion; set => _TipoPercepcionDeduccion = value; }

        [Range(typeof(decimal), "0", "99999999.9999", ErrorMessage = "El Valor debe estar entre 0 y 99,999,999.9999")]
        public decimal ValorTipoPercepcionDeduccion { get => _ValorTipoPercepcionDeduccion; set => _ValorTipoPercepcionDeduccion = value; }

        [Required(ErrorMessage = "Debe seleccionar una Aplicación")]
        [RegularExpression("^(Todos|Algunos)$", ErrorMessage = "La Aplicación debe ser Todos o Algunos")]
        public string AplicacionPercepcionDeduccion { get => _AplicacionPercepcionDeduccion; set => _AplicacionPercepcionDeduccion = value; }

        public bool IsActive { get => _IsActive; set => _IsActive = value; }
        public DateTime CreatedAt { get => _CreatedAt; private set => _CreatedAt = value; }
        public DateTime UpdatedAt { get => _UpdatedAt; private set => _UpdatedAt = value; }

        public ClsModeloPercepcionDeduccion()
        {
            _RepositorioPercepcionDeduccion = new ClsRepositorioPercepcionDeduccion();
        }

        public string RrhhMetGrabarCambios()
        {
            string Mensaje = null;
            try
            {
                if (Estado == EstadoEntidad.Modified || Estado == EstadoEntidad.Deleted)
                {
                    if (_IdPercepcionDeduccion <= 0)
                        return "Debe indicar un registro válido para esta operación";
                }

                // Regla de negocio: un porcentaje no puede pasar de 100
                if (Estado != EstadoEntidad.Deleted &&
                    _TipoPercepcionDeduccion == "%" && _ValorTipoPercepcionDeduccion > 100)
                    return "Cuando el tipo es %, el valor no puede ser mayor a 100";

                var ModeloDatos = new ClsPercepcionDeduccion();
                ModeloDatos.IdPercepcionDeduccion = _IdPercepcionDeduccion;
                ModeloDatos.NombrePercepcionDeduccion = _NombrePercepcionDeduccion;
                ModeloDatos.PeriodoPercepcionDeduccion = _PeriodoPercepcionDeduccion;
                ModeloDatos.EfectoPercepcionDeduccion = _EfectoPercepcionDeduccion;
                ModeloDatos.TipoPercepcionDeduccion = _TipoPercepcionDeduccion;
                ModeloDatos.ValorTipoPercepcionDeduccion = _ValorTipoPercepcionDeduccion;
                ModeloDatos.AplicacionPercepcionDeduccion = _AplicacionPercepcionDeduccion;
                ModeloDatos.IsActive = _IsActive;

                switch (Estado)
                {
                    case EstadoEntidad.Added:
                        _RepositorioPercepcionDeduccion.RrhhMetAgregar(ModeloDatos);
                        Mensaje = "Grabacion exitosa";
                        break;
                    case EstadoEntidad.Modified:
                        _RepositorioPercepcionDeduccion.RrhhMetEditar(ModeloDatos);
                        Mensaje = "Actualizacion exitosa";
                        break;
                    case EstadoEntidad.Deleted:
                        _RepositorioPercepcionDeduccion.RrhhMetRemover(ModeloDatos);
                        Mensaje = "Eliminacion exitosa";
                        break;
                }
            }
            catch (OdbcException OdbcEx)
            {
                // 1062 = violación de UNIQUE (nombre duplicado)
                if (OdbcEx.Errors.Count > 0 && OdbcEx.Errors[0].NativeError == 1062)
                    Mensaje = "Ya existe un registro con el nombre: " + _NombrePercepcionDeduccion;
                else
                    Mensaje = OdbcEx.ToString();
            }
            catch (Exception Ex)
            {
                Mensaje = Ex.ToString();
            }
            return Mensaje;
        }

        public List<ClsModeloPercepcionDeduccion> RrhhMetObtenerTodos()
        {
            var Resultado = _RepositorioPercepcionDeduccion.RrhhMetObtenerTodos();
            _ListaPercepcionDeduccion = new List<ClsModeloPercepcionDeduccion>();
            foreach (ClsPercepcionDeduccion Item in Resultado)
            {
                _ListaPercepcionDeduccion.Add(new ClsModeloPercepcionDeduccion
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
            return _ListaPercepcionDeduccion;
        }

        public IEnumerable<ClsModeloPercepcionDeduccion> RrhhMetBuscarPorId(int Id)
        {
            if (_ListaPercepcionDeduccion == null) RrhhMetObtenerTodos();
            return _ListaPercepcionDeduccion.FindAll(e => e._IdPercepcionDeduccion == Id);
        }

        public IEnumerable<ClsModeloPercepcionDeduccion> RrhhMetBuscarPorNombre(string Nombre)
        {
            if (_ListaPercepcionDeduccion == null) RrhhMetObtenerTodos();
            return _ListaPercepcionDeduccion.FindAll(e =>
                e._NombrePercepcionDeduccion.ToUpper().Contains(Nombre.ToUpper()));
        }
    }
}
