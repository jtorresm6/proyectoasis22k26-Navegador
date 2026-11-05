/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 * Descripción: Consulta y convierte los registros de tipos de movimiento en entidades del modelo.
 */
using CapaModelo_Inventario_Logistica.Contratos;
using CapaModelo_Inventario_Logistica.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Inventario_Logistica.Repositorios
{
    public class ClsRepositorioTipoMovimiento : ClsRepositorioMaestro, IRepositorioTipoMovimiento
    {
        private string _SelectAll;

        public ClsRepositorioTipoMovimiento()
        {
            _SelectAll = "SELECT * FROM tblTipoMovimiento";
        }

        public IEnumerable<ClsTipoMovimiento> LogisticaMetObtenerTodos()
        {
            var ListaTipoMovimento = new List<ClsTipoMovimiento>();
            var TablaDatos = LogisticaMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var TipoMovimiento = new ClsTipoMovimiento();
                TipoMovimiento.Pk_TipoMovimiento = Convert.ToInt32(Fila[0]);
                TipoMovimiento.NombreTipoMovimiento = Fila[1].ToString();
                TipoMovimiento.DescripcionTipoMovimiento = Fila[2].ToString();
                TipoMovimiento.SignoTipoMovimiento = Fila[3].ToString();
                TipoMovimiento.CreatedAt = Convert.ToDateTime(Fila[13]);
                TipoMovimiento.UpdatedAt = Convert.ToDateTime(Fila[14]);
                ListaTipoMovimento.Add(TipoMovimiento);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaTipoMovimento;
        }
    }
}
