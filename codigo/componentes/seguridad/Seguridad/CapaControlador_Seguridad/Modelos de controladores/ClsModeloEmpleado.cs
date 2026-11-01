using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_Seguridad.Contratos;
using System.Data.Odbc;
using CapaModelo_Seguridad.Entidades;
using CapaModelo_Seguridad.Repositorios;



/*
 * ==================================================================
 * Área : Seguridad
 * Autor : Carlos David Calderón Ramirez
 * Carné : 9959-23-848
 * Fecha : 22/09/2026
 * ==================================================================
 * Propósito :
 *  La clase de Modelo Controlador del módulo de Mantenimiento
 *  de Empleados en esta clase se encuentran las validaciones necesarias
 *  para un buen funsionamiento del sistema tambien se encunetra los campos
 *  requeridos y en general se encuentra el manejo de errores
 * ===================================================================
*/


namespace CapaControlador_Seguridad
{
    public class ClsModeloEmpleado
    {
        private int _IdEmpleado;
        private string _CodigoEmpleado;
        private string _DpiEmpleado;
        private string _NitEmpleado;
        private string _NombresEmpleado;
        private string _ApellidosEmpleado;
        private string _PuestoEmpleado;
        private string _GeneroEmpleado;
        private DateTime _FechaNacimientoEmpleado;
        private DateTime _FechaContratacionEmpleado;
        private string _TelefonoEmpleado;
        private string _CorreoEmpleado;
        private bool _IsActive;
        private DateTime _CreatedAt;
        private DateTime _UpdatedAt;
        private ClsRepositorioEmpleado _RepositorioEmpleado;

        public EstadoEntidad Estado { private get; set; }
        private List<ClsModeloEmpleado> _ListaEmpleado;

        public int IdEmpleado { get => _IdEmpleado; set => _IdEmpleado = value; }

        [Required(ErrorMessage = "El campo Código Empleado es requerido")]
        public string CodigoEmpleado { get => _CodigoEmpleado; set => _CodigoEmpleado = value; }

        [Required(ErrorMessage = "El campo Dpi es requerido")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "El Dpi debe tener 13 dígitos numéricos")]
        
        public string DpiEmpleado { get => _DpiEmpleado; set => _DpiEmpleado = value; }

        [RegularExpression(@"^\d{7}-[0-9a-zA-Z]$", ErrorMessage = "El Nit debe tener el formato 1234567-8 o 1234567-A")]
        public string NitEmpleado { get => _NitEmpleado; set => _NitEmpleado = value; }

        [Required(ErrorMessage = "El campo Nombres es requerido")]
        [RegularExpression("^[a-zA-Zá-ú ]+$", ErrorMessage = "El campo Nombre debe ser solo letras")]
        public string NombresEmpleado { get => _NombresEmpleado; set => _NombresEmpleado = value; }

        [Required(ErrorMessage = "El campo Apellidos es requerido")]
        [RegularExpression("^[a-zA-Zá-ú ]+$", ErrorMessage = "El campo Apellidos debe ser solo letras")]
        public string ApellidosEmpleado { get => _ApellidosEmpleado; set => _ApellidosEmpleado = value; }

        [Required(ErrorMessage = "El campo Puesto es requerido")]
        [RegularExpression("^[a-zA-Zá-ú ]+$", ErrorMessage = "El campo Puesto debe ser solo letras")]
        public string PuestoEmpleado { get => _PuestoEmpleado; set => _PuestoEmpleado = value; }

        [Required(ErrorMessage = "El campo Género es requerido")]
        public string GeneroEmpleado { get => _GeneroEmpleado; set => _GeneroEmpleado = value; }

        [Required(ErrorMessage = "El campo Fecha de Nacimiento es requerido")]
        public DateTime FechaNacimientoEmpleado { get => _FechaNacimientoEmpleado; set => _FechaNacimientoEmpleado = value; }

        [Required(ErrorMessage = "El campo Fecha de Contratación es requerido")]
        public DateTime FechaContratacionEmpleado { get => _FechaContratacionEmpleado; set => _FechaContratacionEmpleado = value; }

        [RegularExpression(@"^\d{8}$", ErrorMessage = "El Telefono debe tener 8 dígitos numéricos")]
        public string TelefonoEmpleado { get => _TelefonoEmpleado; set => _TelefonoEmpleado = value; }

        [RegularExpression(@"^[^@\s]+@[^@\s]+.[^@\s]+$", ErrorMessage = "El correo no tiene un formato válido")]
        public string CorreoEmpleado { get => _CorreoEmpleado; set => _CorreoEmpleado = value; }

        public bool IsActive { get => _IsActive; set => _IsActive = value; }

        public DateTime CreatedAt { get => _CreatedAt; private set => _CreatedAt = value; }
        public DateTime UpdatedAt { get => _UpdatedAt; private set => _UpdatedAt = value; }

        public ClsModeloEmpleado()
        {
            _RepositorioEmpleado = new ClsRepositorioEmpleado();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaNacimientoEmpleado > FechaContratacionEmpleado)
            {
                yield return new ValidationResult(
                    "La fecha de nacimiento no puede ser mayor a la fecha de contratación",
                    new[] { nameof(FechaNacimientoEmpleado) });
            }
        }



        public string SeguridadMetGrabarCambios()
        {
            string Mensaje = null;
            try
            {
                var ModeloDatos = new ClsEmpleado();
                ModeloDatos.IdEmpleado = _IdEmpleado;
                ModeloDatos.CodigoEmpleado = _CodigoEmpleado;
                ModeloDatos.DpiEmpleado = _DpiEmpleado;
                ModeloDatos.NitEmpleado = _NitEmpleado;
                ModeloDatos.NombresEmpleado = _NombresEmpleado;
                ModeloDatos.ApellidosEmpleado = _ApellidosEmpleado;
                ModeloDatos.PuestoEmpleado = _PuestoEmpleado;
                ModeloDatos.GeneroEmpleado = _GeneroEmpleado;
                ModeloDatos.FechaNacimientoEmpleado = _FechaNacimientoEmpleado;
                ModeloDatos.FechaContratacionEmpleado = _FechaContratacionEmpleado;
                ModeloDatos.TelefonoEmpleado = _TelefonoEmpleado;
                ModeloDatos.CorreoEmpleado = _CorreoEmpleado;

                switch (Estado)
                {
                    case EstadoEntidad.Added:
                        _RepositorioEmpleado.SeguridadMetAgregar(ModeloDatos);
                        ClsModeloBitacora.SeguridadMetRegistrarAccion("INSERT", "tblEmpleado", ModeloDatos.IdEmpleado, "Se agregó el empleado: " + _NombresEmpleado + " " + _ApellidosEmpleado);
                        Mensaje = "Grabacion exitosa";
                        break;
                    case EstadoEntidad.Modified:
                        _RepositorioEmpleado.SeguridadMetEditar(ModeloDatos);
                        ClsModeloBitacora.SeguridadMetRegistrarAccion("UPDATE", "tblEmpleado", ModeloDatos.IdEmpleado, "Se actualizó el empleado: " + _NombresEmpleado + " " + _ApellidosEmpleado);
                        Mensaje = "Actualizacion exitosa";
                        break;
                    case EstadoEntidad.Deleted:
                        _RepositorioEmpleado.SeguridadMetRemover(ModeloDatos);
                        ClsModeloBitacora.SeguridadMetRegistrarAccion("DELETE", "tblEmpleado", ModeloDatos.IdEmpleado, "Se eliminó el empleado ID: " + ModeloDatos.IdEmpleado);
                        Mensaje = "Eliminacion exitosa";
                        break;
                }
            }
            catch (OdbcException Ex)
            {
                bool esErrorLlaveForanea = false;
                bool esValorDuplicado = false;
                bool esDatoDemasiadoLargo = false;

                foreach (OdbcError error in Ex.Errors)
                {
                    if (error.NativeError == 1451 || error.NativeError == 1452)
                        esErrorLlaveForanea = true;
                    else if (error.NativeError == 1062)
                        esValorDuplicado = true;
                    else if (error.NativeError == 1406)
                        esDatoDemasiadoLargo = true;
                }

                if (esErrorLlaveForanea)
                    Mensaje = "No se puede eliminar este empleado porque tiene una relacion en otro módulo del sistema.";
                else if (esValorDuplicado)
                    Mensaje = "Ya existe un registro con ese código, DPI, NIT o correo. Verifique los datos ingresados.";
                else if (esDatoDemasiadoLargo)
                    Mensaje = "Uno de los campos ingresados excede la longitud permitida.";
                else
                    Mensaje = "Ocurrió un problema al procesar la solicitud. Verifique los datos e intente nuevamente.";
            }
            catch (Exception)
            {
                Mensaje = "Ocurrió un error inesperado en el sistema. Intente nuevamente o contacte al administrador.";
            }
            return Mensaje;
        }


        public List<ClsModeloEmpleado> SeguridadMetObtenerTodos()
        {
            var ResultadoConsulta = _RepositorioEmpleado.SeguridadMetObtenerTodos();
            _ListaEmpleado = new List<ClsModeloEmpleado>();
            foreach (ClsEmpleado Item in ResultadoConsulta)
            {
                _ListaEmpleado.Add(new ClsModeloEmpleado
                {
                    _IdEmpleado = Item.IdEmpleado,
                    _CodigoEmpleado = Item.CodigoEmpleado,
                    _DpiEmpleado = Item.DpiEmpleado,
                    _NitEmpleado = Item.NitEmpleado,
                    _NombresEmpleado = Item.NombresEmpleado,
                    _ApellidosEmpleado = Item.ApellidosEmpleado,
                    _PuestoEmpleado = Item.PuestoEmpleado,
                    _GeneroEmpleado = Item.GeneroEmpleado,
                    _FechaNacimientoEmpleado = Item.FechaNacimientoEmpleado,
                    _FechaContratacionEmpleado = Item.FechaContratacionEmpleado,
                    _TelefonoEmpleado = Item.TelefonoEmpleado,
                    _CorreoEmpleado = Item.CorreoEmpleado,
                    _IsActive = Item.IsActive,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt
                });
            }
            return _ListaEmpleado;
        }

        public IEnumerable<ClsModeloEmpleado> SeguridadMetBuscarPorId(string CodigoEmpleado)
        {
            return _ListaEmpleado.FindAll(e => e._CodigoEmpleado == CodigoEmpleado);
        }
    }
}