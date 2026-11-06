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
            _SelectAll = "SELECT * FROM tblDetalleMovimiento";
        }

        public IEnumerable<ClsDetalleMovimiento> LogisticaMetObtenerTodos()
        {
            var ListaDetalleMovimiento = new List<ClsDetalleMovimiento>();
            var TablaDatos = LogisticaMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var DetalleMovimiento = new ClsDetalleMovimiento
                {
                    Pk_DetalleMovimiento = Convert.ToInt32(Fila[0]),
                    Fk_MovimientoInventario_Codigo = Convert.ToInt32(Fila[1]),
                    LoteMovimientoInventario = Fila[2].ToString(),
                    Fk_Producto_Codigo = Convert.ToInt32(Fila[3]),
                    CantidadDetalleMovimiento = Convert.ToInt32(Fila[4]),
                    PrecioCompraDetalleMovimiento = Convert.ToDecimal(Fila[5]),
                    CreatedAt = Convert.ToDateTime(Fila[6]),
                    UpdatedAt = Convert.ToDateTime(Fila[7])
                };
                ListaDetalleMovimiento.Add(DetalleMovimiento);
            }
            return ListaDetalleMovimiento;
        }
    }
}
