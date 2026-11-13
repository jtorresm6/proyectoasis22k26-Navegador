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
    public class ClsModeloPercepcionDeduccionRango
    {
        private int _IdPercepcionDeduccionRango;
        private int _IdPercepcionDeduccion;
        private decimal _SalarioDesdePercepcionDeduccionRango;
        private decimal? _SalarioHastaPercepcionDeduccionRango;
        private string _ModoValorPercepcionDeduccionRango;
        private decimal _ValorPercepcionDeduccionRango;
        private decimal _MontoFijoBasePercepcionDeduccionRango;
        private bool _IsActive;
        private DateTime _CreatedAt;
        private DateTime _UpdatedAt;

        
        private ClsRepositorioPercepcionDeduccionRango _RepositorioPercepcionDeduccionRango;

       
        public EstadoEntidad Estado { private get; set; }

        
        private List<ClsModeloPercepcionDeduccionRango> _ListaRangos;

       
        public int IdPercepcionDeduccionRango { get => _IdPercepcionDeduccionRango; set => _IdPercepcionDeduccionRango = value; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una percepción/deducción")]
        public int IdPercepcionDeduccion { get => _IdPercepcionDeduccion; set => _IdPercepcionDeduccion = value; }

        [Range(typeof(decimal), "0", "99999999.9999", ErrorMessage = "El Salario inicial debe estar entre 0 y 99,999,999.9999")]
        public decimal SalarioDesdePercepcionDeduccionRango { get => _SalarioDesdePercepcionDeduccionRango; set => _SalarioDesdePercepcionDeduccionRango = value; }

        
        [Range(typeof(decimal), "0", "99999999.9999", ErrorMessage = "El Salario final debe estar entre 0 y 99,999,999.9999")]
        public decimal? SalarioHastaPercepcionDeduccionRango { get => _SalarioHastaPercepcionDeduccionRango; set => _SalarioHastaPercepcionDeduccionRango = value; }

        [Required(ErrorMessage = "El campo Modo de valor es requerido")]
        [RegularExpression("^(%|Monto)$", ErrorMessage = "El Modo de valor debe ser % o Monto")]
        public string ModoValorPercepcionDeduccionRango { get => _ModoValorPercepcionDeduccionRango; set => _ModoValorPercepcionDeduccionRango = value; }

        [Range(typeof(decimal), "0", "99999999.9999", ErrorMessage = "El Valor debe estar entre 0 y 99,999,999.9999")]
        public decimal ValorPercepcionDeduccionRango { get => _ValorPercepcionDeduccionRango; set => _ValorPercepcionDeduccionRango = value; }

        [Range(typeof(decimal), "0", "99999999.9999", ErrorMessage = "El Monto fijo debe estar entre 0 y 99,999,999.9999")]
        public decimal MontoFijoBasePercepcionDeduccionRango { get => _MontoFijoBasePercepcionDeduccionRango; set => _MontoFijoBasePercepcionDeduccionRango = value; }

        public bool IsActive { get => _IsActive; set => _IsActive = value; }

        public DateTime CreatedAt { get => _CreatedAt; private set => _CreatedAt = value; }
        public DateTime UpdatedAt { get => _UpdatedAt; private set => _UpdatedAt = value; }

        
        public ClsModeloPercepcionDeduccionRango()
        {
            _RepositorioPercepcionDeduccionRango = new ClsRepositorioPercepcionDeduccionRango();
        }

        
        public string RrhhMetGrabarCambios()
        {
            string Mensaje = null;
            try
            {
               
                if (Estado == EstadoEntidad.Modified || Estado == EstadoEntidad.Deleted)
                {
                    if (_IdPercepcionDeduccionRango <= 0)
                        return "Debe indicar un rango válido para esta operación";
                }

                if (Estado == EstadoEntidad.Added || Estado == EstadoEntidad.Modified)
                {
                    if (_IdPercepcionDeduccion <= 0)
                        return "Debe seleccionar una percepción/deducción";

                    if (_SalarioHastaPercepcionDeduccionRango.HasValue &&
                        _SalarioHastaPercepcionDeduccionRango.Value <= _SalarioDesdePercepcionDeduccionRango)
                        return "El salario final debe ser mayor al salario inicial";

                    if (_ModoValorPercepcionDeduccionRango == "%" && _ValorPercepcionDeduccionRango > 100)
                        return "Cuando el modo es %, el valor no puede ser mayor a 100";

                    int IdParaExcluir = (Estado == EstadoEntidad.Added) ? 0 : _IdPercepcionDeduccionRango;
                    int Coincidencias = _RepositorioPercepcionDeduccionRango.RrhhMetContarPorDesde(
                        _IdPercepcionDeduccion, _SalarioDesdePercepcionDeduccionRango, IdParaExcluir);
                    if (Coincidencias > 0)
                        return "Ya existe un rango con ese salario inicial para esta percepción/deducción";
                }

                var ModeloDatosRango = new ClsPercepcionDeduccionRango();
                ModeloDatosRango.IdPercepcionDeduccionRango = _IdPercepcionDeduccionRango;
                ModeloDatosRango.IdPercepcionDeduccion = _IdPercepcionDeduccion;
                ModeloDatosRango.SalarioDesdePercepcionDeduccionRango = _SalarioDesdePercepcionDeduccionRango;
                ModeloDatosRango.SalarioHastaPercepcionDeduccionRango = _SalarioHastaPercepcionDeduccionRango;
                ModeloDatosRango.ModoValorPercepcionDeduccionRango = _ModoValorPercepcionDeduccionRango;
                ModeloDatosRango.ValorPercepcionDeduccionRango = _ValorPercepcionDeduccionRango;
                ModeloDatosRango.MontoFijoBasePercepcionDeduccionRango = _MontoFijoBasePercepcionDeduccionRango;
                ModeloDatosRango.IsActive = _IsActive;

                switch (Estado)
                {
                    case EstadoEntidad.Added:
                        _RepositorioPercepcionDeduccionRango.RrhhMetAgregar(ModeloDatosRango);
                        Mensaje = "Grabación exitosa";
                        break;
                    case EstadoEntidad.Modified:
                        _RepositorioPercepcionDeduccionRango.RrhhMetEditar(ModeloDatosRango);
                        Mensaje = "Actualización exitosa";
                        break;
                    case EstadoEntidad.Deleted:
                        _RepositorioPercepcionDeduccionRango.RrhhMetRemover(ModeloDatosRango);
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

        public List<ClsModeloPercepcionDeduccionRango> RrhhMetObtenerTodos()
        {
            var ModeloDatosRango = _RepositorioPercepcionDeduccionRango.RrhhMetObtenerTodos();
            _ListaRangos = new List<ClsModeloPercepcionDeduccionRango>();
            foreach (ClsPercepcionDeduccionRango Item in ModeloDatosRango)
            {
                _ListaRangos.Add(new ClsModeloPercepcionDeduccionRango
                {
                    _IdPercepcionDeduccionRango = Item.IdPercepcionDeduccionRango,
                    _IdPercepcionDeduccion = Item.IdPercepcionDeduccion,
                    _SalarioDesdePercepcionDeduccionRango = Item.SalarioDesdePercepcionDeduccionRango,
                    _SalarioHastaPercepcionDeduccionRango = Item.SalarioHastaPercepcionDeduccionRango,
                    _ModoValorPercepcionDeduccionRango = Item.ModoValorPercepcionDeduccionRango,
                    _ValorPercepcionDeduccionRango = Item.ValorPercepcionDeduccionRango,
                    _MontoFijoBasePercepcionDeduccionRango = Item.MontoFijoBasePercepcionDeduccionRango,
                    _IsActive = Item.IsActive,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt
                });
            }
            return _ListaRangos;
        }

        public ClsModeloPercepcionDeduccionRango RrhhMetBuscarPorId(int IdPercepcionDeduccionRango)
        {
            if (_ListaRangos == null)
                RrhhMetObtenerTodos();

            return _ListaRangos.FirstOrDefault(e => e._IdPercepcionDeduccionRango == IdPercepcionDeduccionRango);
        }

        public IEnumerable<ClsModeloPercepcionDeduccionRango> RrhhMetBuscarPorPercepcionDeduccion(int IdPercepcionDeduccion)
        {
            if (_ListaRangos == null)
                RrhhMetObtenerTodos();

            return _ListaRangos.FindAll(e => e._IdPercepcionDeduccion == IdPercepcionDeduccion);
        }

        // Convierte errores de BD en mensajes legibles
        private string RrhhMetObtenerMensajeError(Exception Ex)
        {
            string Texto = Ex.Message ?? string.Empty;

            if (Texto.IndexOf("Duplicate entry", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Ya existe un rango con ese salario inicial para esta percepción/deducción.";
            if (Texto.IndexOf("foreign key", StringComparison.OrdinalIgnoreCase) >= 0)
                return "La percepción/deducción indicada no existe, o el registro está relacionado con otros datos.";
            if (Ex is System.Data.Odbc.OdbcException)
                return "Error de base de datos: " + Texto;

            return "Ocurrió un error inesperado: " + Texto;
        }
    }
}
