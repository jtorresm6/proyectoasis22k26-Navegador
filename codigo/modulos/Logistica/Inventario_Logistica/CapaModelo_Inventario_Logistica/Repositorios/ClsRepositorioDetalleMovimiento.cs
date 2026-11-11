/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 * Descripción: Consulta los detalles de movimiento y convierte sus registros en entidades del modelo.
 */
using CapaModelo_Inventario_Logistica.Contratos;
using CapaModelo_Inventario_Logistica.Entidades;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaModelo_Inventario_Logistica.Repositorios
{
    public class ClsRepositorioDetalleMovimiento : ClsRepositorioMaestro, IRepositorioDetalleMovimiento
    {
        private readonly string _SelectAll;

        public ClsRepositorioDetalleMovimiento()
        {
            _SelectAll =
                "SELECT d.idDetalleMovimiento AS Pk_DetalleMovimiento, " +
                "d.Fk_Movimiento_Inventario_Codigo AS Fk_MovimientoInventario_Codigo, " +
                "l.codigoLoteProducto AS LoteMovimientoInventario, " +
                "d.Fk_Producto_Codigo AS Fk_Producto_Codigo, " +
                "d.cantidadDetalleMovimiento AS CantidadDetalleMovimiento, " +
                "d.precioCostoDetalleMovimiento AS PrecioCompraDetalleMovimiento, " +
                "d.created_at AS CreatedAt, d.updated_at AS UpdatedAt " +
                "FROM tblDetalleMovimiento d " +
                "LEFT JOIN tblLoteProducto l ON d.Fk_Lote_Producto_Codigo = l.idLoteProducto";
        }

        public IEnumerable<ClsDetalleMovimiento> LogisticaMetObtenerTodos()
        {
            var ListaDetalleMovimiento = new List<ClsDetalleMovimiento>();
            var TablaDatos = LogisticaMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var DetalleMovimiento = new ClsDetalleMovimiento
                {
                    Pk_DetalleMovimiento = Convert.ToInt32(Fila["Pk_DetalleMovimiento"]),
                    Fk_MovimientoInventario_Codigo = Convert.ToInt32(Fila["Fk_MovimientoInventario_Codigo"]),
                    LoteMovimientoInventario = Fila["LoteMovimientoInventario"].ToString(),
                    Fk_Producto_Codigo = Convert.ToInt32(Fila["Fk_Producto_Codigo"]),
                    CantidadDetalleMovimiento = Convert.ToInt32(Fila["CantidadDetalleMovimiento"]),
                    PrecioCompraDetalleMovimiento = Convert.ToDecimal(Fila["PrecioCompraDetalleMovimiento"]),
                    CreatedAt = Convert.ToDateTime(Fila["CreatedAt"]),
                    UpdatedAt = Convert.ToDateTime(Fila["UpdatedAt"])
                };
                ListaDetalleMovimiento.Add(DetalleMovimiento);
            }
            return ListaDetalleMovimiento;
        }
    }
}
