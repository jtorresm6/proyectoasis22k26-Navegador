using CapaModelo_AutorizacionOrdenes.Contratos;
using CapaModelo_AutorizacionOrdenes.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
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
 *  La clase Repositorio de Orden de Compra se encarga de consultar mediante
 *  ODBC las órdenes de compra pendientes de autorización, heredando de
 *  ClsSentencias e implementando la interfaz IRepositorioOrdenCompra.
 * ===================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Repositorios
{
    public class ClsRepositorioOrdenCompra : ClsSentencias, IRepositorioOrdenCompra
    {
        private string _SelectAll;

        public ClsRepositorioOrdenCompra()
        {
            _SelectAll = "SELECT o.Pk_OrdenCompraRecibida, o.referenciaExternaOrdenCompra, o.proveedorOrdenCompra, " +
                         "o.fechaOrdenCompra, o.fechaVencimientoOrdenCompra, o.importeOrdenCompra, " +
                         "o.estadoOrdenCompra, o.Fk_Moneda_Codigo, m.codigoMoneda " +
                         "FROM tblOrdenCompraRecibida o " +
                         "INNER JOIN tblMoneda m ON m.Pk_Moneda = o.Fk_Moneda_Codigo " +
                         "ORDER BY o.fechaVencimientoOrdenCompra";
        }
        public int BancosMetAgregar(ClsOrdenCompra Entidad)
        {
            throw new NotSupportedException("Las órdenes las inserta Compras.");
        }

        public int BancosMetEditar(ClsOrdenCompra Entidad)
        {
            throw new NotSupportedException("Las órdenes las inserta Compras.");
        }

        public int BancosMetRemover(ClsOrdenCompra Entidad)
        {
            throw new NotSupportedException("Las órdenes las administra Compras.");
        }
        public IEnumerable<ClsOrdenCompra> BancosMetObtenerTodos()
        {
            var Lista = new List<ClsOrdenCompra>();
            var TablaDatos = BancosMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Row in TablaDatos.Rows)
            {
                var Orden = new ClsOrdenCompra();
                Orden.IdOrdenCompra = Convert.ToInt64(Row[0]);
                Orden.ReferenciaOrdenCompra = Convert.ToString(Row[1]);
                Orden.ProveedorOrdenCompra = Convert.ToString(Row[2]);
                Orden.FechaOrdenCompra = Convert.ToDateTime(Row[3]);
                Orden.FechaVencimientoOrdenCompra = Convert.ToDateTime(Row[4]);
                Orden.ImporteOrdenCompra = Convert.ToDecimal(Row[5]);
                Orden.EstadoOrdenCompra = Convert.ToString(Row[6]);
                Orden.IdMoneda = Convert.ToInt64(Row[7]);
                Orden.CodigoMoneda = Convert.ToString(Row[8]);
                Lista.Add(Orden);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return Lista;
        }
    }
}
