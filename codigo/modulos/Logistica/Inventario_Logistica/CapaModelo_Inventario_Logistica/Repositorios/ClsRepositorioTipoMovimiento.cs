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
            _SelectAll = "SELECT idTipoMovimiento, nombreTipoMovimiento, descripcionTipoMovimiento, signoTipoMovimiento, created_at, updated_at FROM tblTipoMovimiento";
        }

        public IEnumerable<ClsTipoMovimiento> LogisticaMetObtenerTodos()
        {
            var ListaTipoMovimento = new List<ClsTipoMovimiento>();
            var TablaDatos = LogisticaMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var TipoMovimiento = new ClsTipoMovimiento();
                TipoMovimiento.Pk_TipoMovimiento = Convert.ToInt32(Fila["idTipoMovimiento"]);
                TipoMovimiento.NombreTipoMovimiento = Fila["nombreTipoMovimiento"].ToString();
                TipoMovimiento.DescripcionTipoMovimiento = Fila["descripcionTipoMovimiento"].ToString();
                TipoMovimiento.SignoTipoMovimiento = Fila["signoTipoMovimiento"].ToString();
                TipoMovimiento.CreatedAt = Convert.ToDateTime(Fila["created_at"]);
                TipoMovimiento.UpdatedAt = Convert.ToDateTime(Fila["updated_at"]);
                ListaTipoMovimento.Add(TipoMovimiento);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaTipoMovimento;
        }
    }
}
