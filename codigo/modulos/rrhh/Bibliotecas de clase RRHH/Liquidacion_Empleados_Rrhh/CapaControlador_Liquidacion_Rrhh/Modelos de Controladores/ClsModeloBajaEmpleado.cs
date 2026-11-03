using CapaControlador_Liquidacion_Rrhh.Objetos_de_valor;
using CapaModelo_Liquidacion_Rrhh.Contratos;
using CapaModelo_Liquidacion_Rrhh.Entidades;
using CapaModelo_Liquidacion_Rrhh.Repositorios;
  
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Odbc;

namespace CapaControlador_Liquidacion_Rrhh
{
    public class ClsModeloBajaEmpleado
    {
        private int _IdBajaEmpleado;
        private int _IdEmpleado;
        private int _IdMotivoBaja;
        private DateTime _FechaBajaEmpleado;
        private string _ObservacionBajaEmpleado;
        private int _IdUsuarioRegistra;
        private int _IsActive;

        // Nuevos campos privados
        private string _NombresEmpleado;
        private string _NombreMotivoBaja;

        private ClsRepositorioBajaEmpleado _RepositorioBajaEmpleado;
        private List<ClsModeloBajaEmpleado> _ListaBajaEmpleado;

        public EstadoEntidadLiquidacion Estado { private get; set; }

        public int IdBajaEmpleado { get => _IdBajaEmpleado; set => _IdBajaEmpleado = value; }

        [Required(ErrorMessage = "Debe seleccionar un empleado")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un empleado válido")]
        public int IdEmpleado { get => _IdEmpleado; set => _IdEmpleado = value; }

        [Required(ErrorMessage = "Debe seleccionar un motivo de baja")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un motivo válido")]
        public int IdMotivoBaja { get => _IdMotivoBaja; set => _IdMotivoBaja = value; }

        [Required(ErrorMessage = "La fecha de baja es obligatoria")]
        public DateTime FechaBajaEmpleado { get => _FechaBajaEmpleado; set => _FechaBajaEmpleado = value; }

        [StringLength(255, ErrorMessage = "Las observaciones no pueden superar los 255 caracteres")]
        public string ObservacionBajaEmpleado { get => _ObservacionBajaEmpleado; set => _ObservacionBajaEmpleado = value; }

        [Required]
        public int IdUsuarioRegistra { get => _IdUsuarioRegistra; set => _IdUsuarioRegistra = value; }

        public int IsActive { get => _IsActive; set => _IsActive = value; }

        // Nuevas propiedades públicas para la Vista/DataGridView
        public string NombresEmpleado { get => _NombresEmpleado; set => _NombresEmpleado = value; }
        public string NombreMotivoBaja { get => _NombreMotivoBaja; set => _NombreMotivoBaja = value; }

        public ClsModeloBajaEmpleado()
        {
            _RepositorioBajaEmpleado = new ClsRepositorioBajaEmpleado();
        }

        public string LiquidacionMetGrabarCambios()
        {
            string Mensaje = null;
            try
            {
                var EntidadBaja = new ClsBajaEmpleado();
                {
                    EntidadBaja.IdBajaEmpleado = _IdBajaEmpleado;
                    EntidadBaja.IdEmpleado = _IdEmpleado;
                    EntidadBaja.IdMotivoBaja = _IdMotivoBaja;
                    EntidadBaja.FechaBajaEmpleado = _FechaBajaEmpleado;
                    EntidadBaja.ObservacionBajaEmpleado = _ObservacionBajaEmpleado;
                    EntidadBaja.IdUsuarioRegistra = _IdUsuarioRegistra;
                    EntidadBaja.IsActive = _IsActive;
                }
                switch (Estado)
                {
                    case EstadoEntidadLiquidacion.Added:
                        _RepositorioBajaEmpleado.LiquidacionMetAgregar (EntidadBaja);
                      
                        Mensaje = "Grabacion exitosa";
                        break;

                    case EstadoEntidadLiquidacion.Modified:
                        _RepositorioBajaEmpleado.LiquidacionMetEditar(EntidadBaja);
                        
                        Mensaje = "Actualizacion exitosa";
                        break;

                    case EstadoEntidadLiquidacion.Deleted:
                        _RepositorioBajaEmpleado.LiquidacionMetRemover(EntidadBaja);
                        Mensaje = "Eliminacion exitosa";
                        break;
                }
            }
            catch (OdbcException exOdbc)
            {
                if (exOdbc.Errors[0].NativeError == 1062)
                {
                    Mensaje = "Este registro de baja ya existe o infringe una llave duplicada.";
                }
                else
                {
                    Mensaje = exOdbc.Message;
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.ToString();
            }

            return Mensaje;
        }

        public List<ClsModeloBajaEmpleado> LiquidacionMetObtenerTodos()
        {
            var Obtenidos = _RepositorioBajaEmpleado.LiquidacionMetObtenerTodos();
            _ListaBajaEmpleado = new List<ClsModeloBajaEmpleado>();

            foreach (var Item in Obtenidos)
            {
                _ListaBajaEmpleado.Add(new ClsModeloBajaEmpleado
                {
                    _IdBajaEmpleado = Item.IdBajaEmpleado,
                    _IdEmpleado = Item.IdEmpleado,
                    _IdMotivoBaja = Item.IdMotivoBaja,
                    _FechaBajaEmpleado = Item.FechaBajaEmpleado,
                    _ObservacionBajaEmpleado = Item.ObservacionBajaEmpleado,
                    _IdUsuarioRegistra = Item.IdUsuarioRegistra,
                    _IsActive = Item.IsActive,

                    // Asignación de los nombres/descripciones
                    _NombresEmpleado = Item.NombresEmpleado,
                    _NombreMotivoBaja = Item.NombreMotivoBaja
                });
            }

            return _ListaBajaEmpleado;
        }

        public DataTable LiquidacionMetObtenerEmpleados()
        {
            return _RepositorioBajaEmpleado.LiquidacionMetObtenerEmpleados();
        }

        public DataTable LiquidacionMetObtenerMotivosBaja()
        {
            return _RepositorioBajaEmpleado.LiquidacionMetObtenerMotivosBaja();
        }
    }
}