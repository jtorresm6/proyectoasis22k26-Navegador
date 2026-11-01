using CapaModelo_Liquidacion_Rrhh.Contratos;
using CapaModelo_Liquidacion_Rrhh.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Liquidacion_Rrhh.Repositorios
{
    public class ClsRepositorioMotivoBaja : ClsSentencias, IRepositorioMotivoBaja
    {
        private string _SelectAll;
        public ClsRepositorioMotivoBaja ()
        {
            _SelectAll = @"
    SELECT b.idMotivoBaja,
           b.nombreMotivoBaja,
           b.aplicaIndemnizacionMotivoBaja AS AplicaIndemnizacion,
           b.is_active
    FROM tblMotivoBaja b";
        }
        public IEnumerable<ClsMotivoBaja> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsMotivoBaja>();
            var TablaDatos=RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                Lista.Add(new ClsMotivoBaja
                {
                    IdMotivoBaja = Convert.ToInt32(Fila["idMotivoBaja"]),
                    NombreMotivoBaja = Fila["nombreMotivoBaja"].ToString(),
                    AplicaIndenmizacionMotivoBaja = Convert.ToBoolean(Fila["AplicaIndemnizacion"]),
                    EstadoMotivoBaja = Convert.ToBoolean(Fila["is_active"])
                });
            }
            return Lista;
        }
    }
}
