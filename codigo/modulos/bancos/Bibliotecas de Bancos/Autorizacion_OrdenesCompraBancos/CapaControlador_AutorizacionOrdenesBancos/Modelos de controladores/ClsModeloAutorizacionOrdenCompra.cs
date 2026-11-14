using CapaModelo_AutorizacionOrdenes.Contratos;
using CapaModelo_AutorizacionOrdenes.Entidades;
using CapaModelo_AutorizacionOrdenes.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  La clase Modelo de Autorización de Orden de Compra pertenece a la capa
 *  controlador. Se encarga de recibir los datos de la decisión desde la vista,
 *  armar la evidencia de fondos y el detalle por cuenta, y enviarlos al
 *  repositorio para registrar la autorización o el rechazo de una orden de
 *  compra, retornando un mensaje con el resultado de la operación.
 * ===================================================================
*/

namespace CapaControlador_AutorizacionOrdenes.Modelos_de_controladores
{
    public class ClsModeloAutorizacionOrdenCompra
    {
        private long _IdAutorizacionOrden;
        private long _IdOrdenCompra;
        private int _IdEmpresaAutorizacionOrden;
        private string _EstadoAutorizacionOrden;
        private int? _IdUsuarioAutoriza;
        private DateTime? _FechaDecisionAutorizacionOrden;
        private string _ComentarioAutorizacionOrden;
        private string _MotivoRechazoAutorizacionOrden;
        private DateTime _CreatedAt;
        private DateTime _UpdatedAt;
        private string _NombreUsuarioAutoriza;
        private string _NombreEmpresaAutorizacionOrden;
        private string _Ip;

        private List<ClsEvidenciaFondos> _Evidencias;
        private List<ClsAutorizacionDetalle> _Detalles;
        private List<ClsModeloAutorizacionOrdenCompra> _ListaAutorizacionOrdenCompra;
        private ClsRepositorioAutorizacionOrdenCompra _RepositorioAutorizacionOrdenCompra;

        public long IdAutorizacionOrden { get => _IdAutorizacionOrden; set => _IdAutorizacionOrden = value; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una orden de compra")]
        public long IdOrdenCompra { get => _IdOrdenCompra; set => _IdOrdenCompra = value; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe indicar la empresa")]
        public int IdEmpresaAutorizacionOrden { get => _IdEmpresaAutorizacionOrden; set => _IdEmpresaAutorizacionOrden = value; }

        [Required(ErrorMessage = "El campo Estado es requerido")]
        public string EstadoAutorizacionOrden { get => _EstadoAutorizacionOrden; set => _EstadoAutorizacionOrden = value; }

        public int? IdUsuarioAutoriza { get => _IdUsuarioAutoriza; set => _IdUsuarioAutoriza = value; }

        public DateTime? FechaDecisionAutorizacionOrden { get => _FechaDecisionAutorizacionOrden; set => _FechaDecisionAutorizacionOrden = value; }

        public string ComentarioAutorizacionOrden { get => _ComentarioAutorizacionOrden; set => _ComentarioAutorizacionOrden = value; }

        public string MotivoRechazoAutorizacionOrden { get => _MotivoRechazoAutorizacionOrden; set => _MotivoRechazoAutorizacionOrden = value; }

        public DateTime CreatedAt { get => _CreatedAt; private set => _CreatedAt = value; }

        public DateTime UpdatedAt { get => _UpdatedAt; private set => _UpdatedAt = value; }

        public string NombreUsuarioAutoriza { get => _NombreUsuarioAutoriza; set => _NombreUsuarioAutoriza = value; }

        public string NombreEmpresaAutorizacionOrden { get => _NombreEmpresaAutorizacionOrden; set => _NombreEmpresaAutorizacionOrden = value; }

        public List<ClsEvidenciaFondos> Evidencias { get => _Evidencias; set => _Evidencias = value; }

        public List<ClsAutorizacionDetalle> Detalles { get => _Detalles; set => _Detalles = value; }

        public string Ip { get => _Ip; set => _Ip = value; }

        public void BancosMetLimpiarCuentas()
        {
            _Evidencias = new List<ClsEvidenciaFondos>();
            _Detalles = new List<ClsAutorizacionDetalle>();
        }
        public List<ClsModeloAutorizacionOrdenCompra> BancosMetObtenerTodos()
        {
            var DatosAutorizacionOrdenCompra = _RepositorioAutorizacionOrdenCompra.BancosMetObtenerTodos();
            _ListaAutorizacionOrdenCompra = new List<ClsModeloAutorizacionOrdenCompra>();
            foreach (ClsAutorizacionOrdenCompra Item in DatosAutorizacionOrdenCompra)
            {
                _ListaAutorizacionOrdenCompra.Add(new ClsModeloAutorizacionOrdenCompra
                {
                    _IdAutorizacionOrden = Item.IdAutorizacionOrden,
                    _IdOrdenCompra = Item.IdOrdenCompra,
                    _IdEmpresaAutorizacionOrden = Item.IdEmpresaAutorizacionOrden,
                    _EstadoAutorizacionOrden = Item.EstadoAutorizacionOrden,
                    _IdUsuarioAutoriza = Item.IdUsuarioAutoriza,
                    _FechaDecisionAutorizacionOrden = Item.FechaDecisionAutorizacionOrden,
                    _ComentarioAutorizacionOrden = Item.ComentarioAutorizacionOrden,
                    _MotivoRechazoAutorizacionOrden = Item.MotivoRechazoAutorizacionOrden,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt,
                    _NombreUsuarioAutoriza = Item.NombreUsuarioAutoriza,
                    _NombreEmpresaAutorizacionOrden = Item.NombreEmpresaAutorizacionOrden
                });
            }
            return _ListaAutorizacionOrdenCompra;
        }

        public void BancosMetAgregarCuenta(ClsModeloCuentaDisponible Cuenta, long IdMonedaOrden, decimal Monto)
        {
            BancosMetAgregarCuenta(Cuenta, IdMonedaOrden, Monto, Cuenta.TipoCambio, Cuenta.IdTipoCambio);
        }

        public void BancosMetAgregarCuenta(ClsModeloCuentaDisponible Cuenta, long IdMonedaOrden, decimal Monto, decimal TipoCambio, long? IdTipoCambio)
        {
            bool MismaMoneda = Cuenta.IdMoneda == IdMonedaOrden;

            _Evidencias.Add(new ClsEvidenciaFondos
            {
                IdCuenta = Cuenta.IdCuenta,
                FechaCorteDisponibilidadFondos = DateTime.Today,
                SaldoDisponibleDisponibilidadFondos = Cuenta.SaldoDisponible,
                IdMonedaOrigen = Cuenta.IdMoneda,
                IdMonedaDestino = IdMonedaOrden,
                TipoCambioDisponibilidadFondos = MismaMoneda ? 1m : TipoCambio,
                IdTipoCambio = MismaMoneda ? (long?)null : IdTipoCambio,
                FuenteDisponibilidadFondos = "tblDisponibilidadDiaria"
            });

            _Detalles.Add(new ClsAutorizacionDetalle
            {
                IdCuenta = Cuenta.IdCuenta,
                MontoAsignadoPagoOrden = Monto
            });
        }

        public int BancosMetObtenerIdEmpresa()
        {
            return _RepositorioAutorizacionOrdenCompra.BancosMetObtenerIdEmpresa();
        }

        public string BancosMetGrabarCambios()
        {
            string Mensaje = null;
            try
            {
                var DatosAutorizacionOrdenCompra = new ClsAutorizacionOrdenCompra();
                DatosAutorizacionOrdenCompra.IdAutorizacionOrden = _IdAutorizacionOrden;
                DatosAutorizacionOrdenCompra.IdOrdenCompra = _IdOrdenCompra;
                DatosAutorizacionOrdenCompra.IdEmpresaAutorizacionOrden = _IdEmpresaAutorizacionOrden;
                DatosAutorizacionOrdenCompra.EstadoAutorizacionOrden = _EstadoAutorizacionOrden;
                DatosAutorizacionOrdenCompra.IdUsuarioAutoriza = _IdUsuarioAutoriza;
                DatosAutorizacionOrdenCompra.FechaDecisionAutorizacionOrden = _FechaDecisionAutorizacionOrden;
                DatosAutorizacionOrdenCompra.ComentarioAutorizacionOrden = _ComentarioAutorizacionOrden;
                DatosAutorizacionOrdenCompra.MotivoRechazoAutorizacionOrden = _MotivoRechazoAutorizacionOrden;
                DatosAutorizacionOrdenCompra.CreatedAt = _CreatedAt;
                DatosAutorizacionOrdenCompra.UpdatedAt = _UpdatedAt;
                DatosAutorizacionOrdenCompra.Ip = _Ip;
                DatosAutorizacionOrdenCompra.Evidencias = _Evidencias;
                DatosAutorizacionOrdenCompra.Detalles = _Detalles;

                _RepositorioAutorizacionOrdenCompra.BancosMetAgregar(DatosAutorizacionOrdenCompra);
                if (_EstadoAutorizacionOrden == "AUTORIZADA")
                {
                    Mensaje = "Autorizacion Exitosa";
                }
                else
                {
                    Mensaje = "Rechazo Exitoso";
                }
            }
            catch (Exception Ex)
            {
                Mensaje = "No se pudo grabar: " + Ex.Message;
            }

            return Mensaje;
        }
        public ClsModeloAutorizacionOrdenCompra()
        {
            _RepositorioAutorizacionOrdenCompra = new ClsRepositorioAutorizacionOrdenCompra();
            _Evidencias = new List<ClsEvidenciaFondos>();
            _Detalles = new List<ClsAutorizacionDetalle>();
        }
    }
}
