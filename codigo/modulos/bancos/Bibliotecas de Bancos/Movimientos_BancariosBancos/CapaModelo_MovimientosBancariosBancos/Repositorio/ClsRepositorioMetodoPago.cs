using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_MovimientosBancarios.Contratos;
using CapaModelo_MovimientosBancarios.Entidades;

namespace CapaModelo_MovimientosBancarios.Repositorio
{
    public class ClsRepositorioMetodoPago :
        ClsSentencias, IRepositorioMetodoPago
    {
        private string _SelectAll;

        public ClsRepositorioMetodoPago()
        {
            _SelectAll = @"SELECT * FROM tblMetodoPago WHERE is_active = 1 ORDER BY nombreMetodoPago";
        } 

        public int BancosMetAgregar(ClsMetodoPago Entidad)
        {
            throw new NotSupportedException("El registro de métodos de pago no corresponde a este módulo.");
        }

        public int BancosMetEditar(ClsMetodoPago Entidad)
        {
            throw new NotSupportedException("La edición de métodos de pago no corresponde a este módulo.");
        }

        public int BancosMetRemover(ClsMetodoPago Entidad)
        {
            throw new NotSupportedException("La eliminación de métodos de pago no corresponde a este módulo.");
        }

        public IEnumerable<ClsMetodoPago> BancosMetObtenerTodos()
        {
            var ListaMetodos = new List<ClsMetodoPago>();
            var TablaDatos = BancosMetEjecucionConsulta(
                _SelectAll, CommandType.Text);

            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Metodo = new ClsMetodoPago();

                Metodo.IdMetodoPago = Convert.ToInt32(Fila["idMetodoPago"]);
                Metodo.NombreMetodoPago = Fila["nombreMetodoPago"].ToString();
                Metodo.DescripcionMetodoPago = Fila["descripcionMetodoPago"].ToString();
                Metodo.IsActive = Convert.ToBoolean(Fila["is_active"]);
                Metodo.CreatedAt = Convert.ToDateTime(Fila["created_at"]);
                Metodo.UpdatedAt = Convert.ToDateTime(Fila["updated_at"]);
                ListaMetodos.Add(Metodo);
            }

            TablaDatos.Clear();
            TablaDatos = null;
            return ListaMetodos;
        }
    }
}
