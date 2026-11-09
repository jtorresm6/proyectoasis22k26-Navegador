/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 * Descripción: Obtiene los detalles de movimiento del repositorio y adapta sus datos al modelo del controlador.
 */
using System;
using System.Collections.Generic;
using CapaModelo_Inventario_Logistica.Entidades;
using CapaModelo_Inventario_Logistica.Repositorios;

namespace CapaControlador_Inventario_Logistica.Modelos_de_Controladores
{
    public class ClsModelosDetalleMovimiento
    {
        private readonly ClsRepositorioDetalleMovimiento _RepositorioDetalleMovimiento;

        public int _PkDetalleMovimiento { get; set; }
        public int _FkMovimientoInventarioCodigo { get; set; }
        public string _LoteMovimientoInventario { get; set; }
        public int _FkProductoCodigo { get; set; }
        public int _CantidadDetalleMovimiento { get; set; }
        public decimal _PrecioCompraDetalleMovimiento { get; set; }
        public DateTime _CreatedAt { get; set; }
        public DateTime _UpdatedAt { get; set; }

        public ClsModelosDetalleMovimiento()
        {
            _RepositorioDetalleMovimiento = new ClsRepositorioDetalleMovimiento();
        }

        public List<ClsModelosDetalleMovimiento> LogisticaMetObtenerTodos()
        {
            var ResultadoConsulta = _RepositorioDetalleMovimiento.LogisticaMetObtenerTodos();
            var ListaDetalleMovimiento = new List<ClsModelosDetalleMovimiento>();
            foreach (ClsDetalleMovimiento Item in ResultadoConsulta)
            {
                ListaDetalleMovimiento.Add(new ClsModelosDetalleMovimiento
                {
                    _PkDetalleMovimiento = Item.Pk_DetalleMovimiento,
                    _FkMovimientoInventarioCodigo = Item.Fk_MovimientoInventario_Codigo,
                    _LoteMovimientoInventario = Item.LoteMovimientoInventario,
                    _FkProductoCodigo = Item.Fk_Producto_Codigo,
                    _CantidadDetalleMovimiento = Item.CantidadDetalleMovimiento,
                    _PrecioCompraDetalleMovimiento = Item.PrecioCompraDetalleMovimiento,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt
                });
            }
            return ListaDetalleMovimiento;
        }
    }
}
