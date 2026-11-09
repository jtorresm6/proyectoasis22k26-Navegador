using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_MovimientosBancarios.Repositorio;

namespace CapaControlador_MovimientosBancarios.Modelo_de_Controladores
{
    public class ClsModeloMetodoPago
    {
        private readonly ClsRepositorioMetodoPago _RepositorioMetodoPago;
        public ClsModeloMetodoPago()
        {
            _RepositorioMetodoPago = new ClsRepositorioMetodoPago();
        }

        public DataTable BancosMetObtenerMetodosTabla()
        {
            var Metodos = _RepositorioMetodoPago.BancosMetObtenerTodos();
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("ID", typeof(int));
            Tabla.Columns.Add("Nombre", typeof(string));
            Tabla.Columns.Add("Descripcion", typeof(string));
            foreach (var Metodo in Metodos)
            {
                Tabla.Rows.Add(Metodo.IdMetodoPago, Metodo.NombreMetodoPago, Metodo.DescripcionMetodoPago);
            }
            return Tabla;
        }
    }
}
