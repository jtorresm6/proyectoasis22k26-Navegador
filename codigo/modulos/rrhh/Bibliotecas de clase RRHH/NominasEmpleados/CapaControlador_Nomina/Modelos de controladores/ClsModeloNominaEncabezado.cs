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
    public class ClsModeloNominaEncabezado
    {
        private int _IdNominaEncabezado;
        private int _IdEmpleado;
        private string _NombreEmpleado;
        private DateTime _FechaInicioNominaEncabezado;
        private DateTime _FechaFinalNominaEncabezado;
        private decimal _SalarioBaseNominaEncabezado;
        private int _DuracionHoraExtraNominaEncabezado;
        private decimal _TotalHorasExtraNominaEncabezado;
        private decimal _TotalPercepcionNominaEncabezado;
        private decimal _TotalDeduccionNominaEncabezado;
        private decimal _TotalNetoNominaEncabezado;
        private string _EstadoNminaEncabezado;
        private DateTime _CreatedAt;
        private DateTime _UpdatedAt;

        private ClsRepositorioNominaEncabezado _RepositorioNominaEncabezado;
        private ClsModeloComisiones _ModeloComisiones;

        public EstadoEntidad Estado { private get; set; }

        private List<ClsModeloNominaEncabezado> _ListaNominas;

        public int IdNominaEncabezado { get => _IdNominaEncabezado; set => _IdNominaEncabezado = value; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un empleado")]
        public int IdEmpleado { get => _IdEmpleado; set => _IdEmpleado = value; }

        public string NombreEmpleado { get => _NombreEmpleado; private set => _NombreEmpleado = value; }

        public DateTime FechaInicioNominaEncabezado { get => _FechaInicioNominaEncabezado; set => _FechaInicioNominaEncabezado = value; }
        public DateTime FechaFinalNominaEncabezado { get => _FechaFinalNominaEncabezado; set => _FechaFinalNominaEncabezado = value; }

        [Range(typeof(decimal), "0", "99999999.9999", ErrorMessage = "El Salario base está fuera de rango")]
        public decimal SalarioBaseNominaEncabezado { get => _SalarioBaseNominaEncabezado; private set => _SalarioBaseNominaEncabezado = value; }

        [Range(0, int.MaxValue, ErrorMessage = "La duración de horas extra no puede ser negativa")]
        public int DuracionHoraExtraNominaEncabezado { get => _DuracionHoraExtraNominaEncabezado; private set => _DuracionHoraExtraNominaEncabezado = value; }

        [Range(typeof(decimal), "0", "99999999.9999", ErrorMessage = "El Total de horas extra está fuera de rango")]
        public decimal TotalHorasExtraNominaEncabezado { get => _TotalHorasExtraNominaEncabezado; private set => _TotalHorasExtraNominaEncabezado = value; }

        [Range(typeof(decimal), "0", "99999999.9999", ErrorMessage = "El Total de percepciones está fuera de rango")]
        public decimal TotalPercepcionNominaEncabezado { get => _TotalPercepcionNominaEncabezado; private set => _TotalPercepcionNominaEncabezado = value; }

        [Range(typeof(decimal), "0", "99999999.9999", ErrorMessage = "El Total de deducciones está fuera de rango")]
        public decimal TotalDeduccionNominaEncabezado { get => _TotalDeduccionNominaEncabezado; private set => _TotalDeduccionNominaEncabezado = value; }

        public decimal TotalNetoNominaEncabezado { get => _TotalNetoNominaEncabezado; private set => _TotalNetoNominaEncabezado = value; }

        [Required(ErrorMessage = "El campo Estado es requerido")]
        [RegularExpression("^(Aprobada|Rechazada|Pagada)$", ErrorMessage = "El Estado debe ser Aprobada, Rechazada o Pagada")]
        public string EstadoNminaEncabezado { get => _EstadoNminaEncabezado; set => _EstadoNminaEncabezado = value; }

        public DateTime CreatedAt { get => _CreatedAt; private set => _CreatedAt = value; }
        public DateTime UpdatedAt { get => _UpdatedAt; private set => _UpdatedAt = value; }

        public List<ClsConceptoCalculado> ConceptosCalculados { get; private set; }

       
        public ClsModeloNominaEncabezado()
        {
            _RepositorioNominaEncabezado = new ClsRepositorioNominaEncabezado();
            _ModeloComisiones = new ClsModeloComisiones();
            _EstadoNminaEncabezado = "Aprobada";
            _FechaInicioNominaEncabezado = DateTime.Today;
            _FechaFinalNominaEncabezado = DateTime.Today;
            ConceptosCalculados = new List<ClsConceptoCalculado>();
        }

        public string RrhhMetCalcularConceptos()
        {
            try
            {
                if (_IdEmpleado <= 0)
                    return "Debe seleccionar un empleado";
                if (_FechaFinalNominaEncabezado.Date < _FechaInicioNominaEncabezado.Date)
                    return "La fecha final no puede ser menor a la fecha inicial";

                
                decimal? Salario = _RepositorioNominaEncabezado.RrhhMetObtenerSalarioEmpleado(_IdEmpleado);
                if (Salario == null)
                    return "El empleado seleccionado no tiene un salario registrado";
                if (Salario.Value <= 0)
                    return "El empleado no tiene un salario válido";

                var Conceptos = new List<ClsConceptoCalculado>();
                decimal TotalPercepcion = 0;
                decimal TotalDeduccion = 0;

                foreach (ClsPercepcionDeduccion Concepto in _RepositorioNominaEncabezado.RrhhMetObtenerConceptosAplicables(_IdEmpleado))
                {
                    if (string.Equals(Concepto.NombrePercepcionDeduccion, ClsConceptoCalculado.NombreConceptoComision,
                            StringComparison.OrdinalIgnoreCase))
                        continue;

                    decimal Monto;

                    if (Concepto.TipoPercepcionDeduccion == "%")
                    {
                        Monto = Salario.Value * Concepto.ValorTipoPercepcionDeduccion / 100m;
                    }
                    else if (Concepto.TipoPercepcionDeduccion == "Monto")
                    {
                        Monto = Concepto.ValorTipoPercepcionDeduccion;
                    }
                    else 
                    {
                        string Modo;
                        decimal ValorRango;
                        decimal MontoFijoBase;
                        bool Encontrado = _RepositorioNominaEncabezado.RrhhMetObtenerRangoSalario(
                            Concepto.IdPercepcionDeduccion, Salario.Value, out Modo, out ValorRango, out MontoFijoBase);

                        if (!Encontrado)
                            continue; 

                        Monto = MontoFijoBase + (Modo == "%" ? Salario.Value * ValorRango / 100m : ValorRango);
                    }

                    Monto = Math.Round(Monto, 4, MidpointRounding.AwayFromZero);

                    Conceptos.Add(new ClsConceptoCalculado
                    {
                        IdPercepcionDeduccion = Concepto.IdPercepcionDeduccion,
                        NombreConcepto = Concepto.NombrePercepcionDeduccion,
                        EsPercepcion = Concepto.EfectoPercepcionDeduccion,
                        Monto = Monto
                    });

                    if (Concepto.EfectoPercepcionDeduccion)
                        TotalPercepcion += Monto;
                    else
                        TotalDeduccion += Monto;
                }

                decimal TotalComisiones = Math.Round(
                    _ModeloComisiones.RrhhMetObtenerTotalComisiones(_IdEmpleado, _FechaInicioNominaEncabezado, _FechaFinalNominaEncabezado),
                    4, MidpointRounding.AwayFromZero);
                if (TotalComisiones > 0)
                {
                    Conceptos.Add(new ClsConceptoCalculado
                    {
                        IdPercepcionDeduccion = 0, 
                        NombreConcepto = ClsConceptoCalculado.NombreConceptoComision,
                        EsPercepcion = true,
                        Monto = TotalComisiones
                    });
                    TotalPercepcion += TotalComisiones;
                }

                _SalarioBaseNominaEncabezado = Salario.Value;
                _TotalPercepcionNominaEncabezado = TotalPercepcion;
                _TotalDeduccionNominaEncabezado = TotalDeduccion;
                ConceptosCalculados = Conceptos;
                return null;
            }
            catch (Exception Ex)
            {
                return RrhhMetObtenerMensajeError(Ex);
            }
        }

        public string RrhhMetCalcularNomina()
        {
            try
            {
                if (_IdEmpleado <= 0)
                    return "Debe seleccionar un empleado";
                if (_FechaFinalNominaEncabezado.Date < _FechaInicioNominaEncabezado.Date)
                    return "La fecha final no puede ser menor a la fecha inicial";

                if (!_RepositorioNominaEncabezado.RrhhMetEsEmpleadoActivo(_IdEmpleado))
                    return "El empleado seleccionado no está activo";

                string ErrorConceptos = RrhhMetCalcularConceptos();
                if (ErrorConceptos != null)
                    return ErrorConceptos;

                int Duracion;
                decimal TotalHorasExtra;
                _RepositorioNominaEncabezado.RrhhMetObtenerResumenHorasExtra(
                    _IdEmpleado, _FechaInicioNominaEncabezado, _FechaFinalNominaEncabezado,
                    out Duracion, out TotalHorasExtra);

                _DuracionHoraExtraNominaEncabezado = Duracion;
                _TotalHorasExtraNominaEncabezado = TotalHorasExtra;
                _TotalNetoNominaEncabezado = _SalarioBaseNominaEncabezado + _TotalPercepcionNominaEncabezado
                                             + TotalHorasExtra - _TotalDeduccionNominaEncabezado;

                if (_TotalNetoNominaEncabezado < 0)
                    return "Las deducciones superan los ingresos del empleado; revise los conceptos asignados";

                return null;
            }
            catch (Exception Ex)
            {
                return RrhhMetObtenerMensajeError(Ex);
            }
        }

        public string RrhhMetGrabarCambios()
        {
            string Mensaje = null;
            try
            {
                if (Estado == EstadoEntidad.Modified || Estado == EstadoEntidad.Deleted)
                {
                    if (_IdNominaEncabezado <= 0)
                        return "Debe indicar una nómina válida para esta operación";
                }

                if (Estado == EstadoEntidad.Deleted)
                    return "La eliminación de nóminas se habilitará cuando se defina el manejo del detalle";

                if (Estado == EstadoEntidad.Modified)
                {
                    var NominaActual = RrhhMetObtenerTodos().FirstOrDefault(e => e._IdNominaEncabezado == _IdNominaEncabezado);
                    if (NominaActual == null)
                        return "No se encontró la nómina seleccionada";
                    if (NominaActual._EstadoNminaEncabezado == "Rechazada" && _EstadoNminaEncabezado != "Rechazada")
                        return "Una nómina rechazada no puede reactivarse; genere una nueva";
                }

                if (Estado == EstadoEntidad.Added)
                {
                    if (_IdEmpleado <= 0)
                        return "Debe seleccionar un empleado";
                    if (_FechaFinalNominaEncabezado.Date < _FechaInicioNominaEncabezado.Date)
                        return "La fecha final no puede ser menor a la fecha inicial";

                    int Traslapes = _RepositorioNominaEncabezado.RrhhMetContarTraslapes(
                        _IdEmpleado, _FechaInicioNominaEncabezado, _FechaFinalNominaEncabezado, 0);
                    if (Traslapes > 0)
                        return "El empleado ya tiene una nómina que se traslapa con ese período";

                    // Se recalcula aqui: no se confia en los valores que traiga el formulario
                    string ErrorCalculo = RrhhMetCalcularNomina();
                    if (ErrorCalculo != null)
                        return ErrorCalculo;

                    _EstadoNminaEncabezado = "Aprobada";
                }

                var ModeloDatosNomina = new ClsNominaEncabezado();
                ModeloDatosNomina.IdNominaEncabezado = _IdNominaEncabezado;
                ModeloDatosNomina.IdEmpleado = _IdEmpleado;
                ModeloDatosNomina.FechaInicioNominaEncabezado = _FechaInicioNominaEncabezado.Date;
                ModeloDatosNomina.FechaFinalNominaEncabezado = _FechaFinalNominaEncabezado.Date;
                ModeloDatosNomina.SalarioBaseNominaEncabezado = _SalarioBaseNominaEncabezado;
                ModeloDatosNomina.DuracionHoraExtraNominaEncabezado = _DuracionHoraExtraNominaEncabezado;
                ModeloDatosNomina.TotalHorasExtraNominaEncabezado = _TotalHorasExtraNominaEncabezado;
                ModeloDatosNomina.TotalPercepcionNominaEncabezado = _TotalPercepcionNominaEncabezado;
                ModeloDatosNomina.TotalDeduccionNominaEncabezado = _TotalDeduccionNominaEncabezado;
                ModeloDatosNomina.TotalNetoNominaEncabezado = _TotalNetoNominaEncabezado;
                ModeloDatosNomina.EstadoNminaEncabezado = _EstadoNminaEncabezado;

                switch (Estado)
                {
                    case EstadoEntidad.Added:
                        _RepositorioNominaEncabezado.RrhhMetAgregar(ModeloDatosNomina);
                        _IdNominaEncabezado = ModeloDatosNomina.IdNominaEncabezado;
                        Mensaje = "Grabación exitosa";
                        break;
                    case EstadoEntidad.Modified:
                        // Por ahora solo se cambia el estado de la nomina
                        _RepositorioNominaEncabezado.RrhhMetEditar(ModeloDatosNomina);
                        Mensaje = "Actualización exitosa";
                        break;
                }
            }
            catch (Exception Ex)
            {
                Mensaje = RrhhMetObtenerMensajeError(Ex);
            }
            return Mensaje;
        }

        public List<ClsModeloNominaEncabezado> RrhhMetObtenerTodos()
        {
            var Empleados = new ClsModeloCatalogos().RrhhMetObtenerEmpleados()
                .ToDictionary(e => e.Key, e => e.Value);

            var ModeloDatosNomina = _RepositorioNominaEncabezado.RrhhMetObtenerTodos();
            _ListaNominas = new List<ClsModeloNominaEncabezado>();
            foreach (ClsNominaEncabezado Item in ModeloDatosNomina)
            {
                string Nombre;
                _ListaNominas.Add(new ClsModeloNominaEncabezado
                {
                    _IdNominaEncabezado = Item.IdNominaEncabezado,
                    _IdEmpleado = Item.IdEmpleado,
                    _NombreEmpleado = Empleados.TryGetValue(Item.IdEmpleado, out Nombre) ? Nombre : "(sin nombre)",
                    _FechaInicioNominaEncabezado = Item.FechaInicioNominaEncabezado,
                    _FechaFinalNominaEncabezado = Item.FechaFinalNominaEncabezado,
                    _SalarioBaseNominaEncabezado = Item.SalarioBaseNominaEncabezado,
                    _DuracionHoraExtraNominaEncabezado = Item.DuracionHoraExtraNominaEncabezado,
                    _TotalHorasExtraNominaEncabezado = Item.TotalHorasExtraNominaEncabezado,
                    _TotalPercepcionNominaEncabezado = Item.TotalPercepcionNominaEncabezado,
                    _TotalDeduccionNominaEncabezado = Item.TotalDeduccionNominaEncabezado,
                    _TotalNetoNominaEncabezado = Item.TotalNetoNominaEncabezado,
                    _EstadoNminaEncabezado = Item.EstadoNminaEncabezado,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt
                });
            }
            return _ListaNominas;
        }

        public ClsModeloNominaEncabezado RrhhMetBuscarPorId(int IdNominaEncabezado)
        {
            if (_ListaNominas == null)
                RrhhMetObtenerTodos();

            return _ListaNominas.FirstOrDefault(e => e._IdNominaEncabezado == IdNominaEncabezado);
        }

        // Busca por nombre del empleado
        public IEnumerable<ClsModeloNominaEncabezado> RrhhMetBuscarPorNombre(string NombreEmpleado)
        {
            if (_ListaNominas == null)
                RrhhMetObtenerTodos();

            return _ListaNominas.FindAll(e =>
                e._NombreEmpleado.ToUpper().Contains(NombreEmpleado.ToUpper()));
        }

        // Convierte errores de BD en mensajes legibles
        private string RrhhMetObtenerMensajeError(Exception Ex)
        {
            string Texto = Ex.Message ?? string.Empty;

            if (Texto.IndexOf("Unknown column", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Error de base de datos: una columna no existe o está mal escrita. Detalle: " + Texto;
            if (Texto.IndexOf("foreign key", StringComparison.OrdinalIgnoreCase) >= 0)
                return "No se puede completar la operación porque el registro está relacionado con otros datos.";
            if (Texto.IndexOf("Duplicate entry", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Ya existe un registro con esos datos.";
            if (Ex is System.Data.Odbc.OdbcException)
                return "Error de base de datos: " + Texto;

            return "Ocurrió un error inesperado: " + Texto;
        }

    }
}
