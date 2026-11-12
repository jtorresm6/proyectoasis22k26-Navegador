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
 *  La clase Modelo de Orden de Compra pertenece a la capa controlador. Se
 *  encarga de obtener, desde el repositorio, las órdenes de compra pendientes
 *  de autorización y de entregarlas a la vista.
 * ===================================================================
*/

namespace CapaControlador_AutorizacionOrdenes.Modelos_de_controladores
{
    public class ClsModeloOrdenCompra
    {
        private long _IdOrdenCompra;
        private string _ReferenciaOrdenCompra;
        private string _ProveedorOrdenCompra;
        private DateTime _FechaOrdenCompra;
        private DateTime _FechaVencimientoOrdenCompra;
        private decimal _ImporteOrdenCompra;
        private string _EstadoOrdenCompra;
        private long _IdMoneda;
        private string _CodigoMoneda;
        private ClsRepositorioOrdenCompra _RepositorioOrdenCompra;

        public long IdOrdenCompra { get => _IdOrdenCompra; private set => _IdOrdenCompra = value; }

        public string ReferenciaOrdenCompra { get => _ReferenciaOrdenCompra; private set => _ReferenciaOrdenCompra = value; }

        public string ProveedorOrdenCompra { get => _ProveedorOrdenCompra; private set => _ProveedorOrdenCompra = value; }

        public DateTime FechaOrdenCompra { get => _FechaOrdenCompra; private set => _FechaOrdenCompra = value; }

        public DateTime FechaVencimientoOrdenCompra { get => _FechaVencimientoOrdenCompra; private set => _FechaVencimientoOrdenCompra = value; }

        public decimal ImporteOrdenCompra { get => _ImporteOrdenCompra; private set => _ImporteOrdenCompra = value; }

        public string EstadoOrdenCompra { get => _EstadoOrdenCompra; private set => _EstadoOrdenCompra = value; }

        public long IdMoneda { get => _IdMoneda; private set => _IdMoneda = value; }

        public string CodigoMoneda { get => _CodigoMoneda; private set => _CodigoMoneda = value; }

        private List<ClsModeloOrdenCompra> _ListaOrdenCompra;
        public ClsModeloOrdenCompra()
        {
            _RepositorioOrdenCompra = new ClsRepositorioOrdenCompra();
        }

        public List<ClsModeloOrdenCompra> BancosMetObtenerTodos()
        {
            var DatosOrdenes = _RepositorioOrdenCompra.BancosMetObtenerTodos();
            _ListaOrdenCompra = new List<ClsModeloOrdenCompra>();
            foreach (ClsOrdenCompra Item in DatosOrdenes)
            {
                _ListaOrdenCompra.Add(new ClsModeloOrdenCompra
                {
                    _IdOrdenCompra = Item.IdOrdenCompra,
                    _ReferenciaOrdenCompra = Item.ReferenciaOrdenCompra,
                    _ProveedorOrdenCompra = Item.ProveedorOrdenCompra,
                    _FechaOrdenCompra = Item.FechaOrdenCompra,
                    _FechaVencimientoOrdenCompra = Item.FechaVencimientoOrdenCompra,
                    _ImporteOrdenCompra = Item.ImporteOrdenCompra,
                    _EstadoOrdenCompra = Item.EstadoOrdenCompra,
                    _IdMoneda = Item.IdMoneda,
                    _CodigoMoneda = Item.CodigoMoneda
                });
            }
            return _ListaOrdenCompra;
        }

        public List<ClsModeloOrdenCompra> BancosMetObtenerPendientes()
        {
            BancosMetObtenerTodos();
            return _ListaOrdenCompra.FindAll(e => e._EstadoOrdenCompra == "PENDIENTE");
        }
        public ClsModeloOrdenCompra BancosMetBuscarPorId(long IdOrdenCompra)
        {
            return _ListaOrdenCompra.Find(e => e._IdOrdenCompra == IdOrdenCompra);
        }
    }
}
