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
    public class ClsRepositorioParametroLey : ClsSentencias, IRepositorioParametroLey
    {
        private string _SelectAll;

        public ClsRepositorioParametroLey()
        {
            _SelectAll = @"
    SELECT p.idParametroLey,
           p.idEmpresa,
           p.codigoParametroLey,
           p.descripcionParametroLey,
           p.valorParametroLey,
           p.vigenteDesdeParametroLey,
           p.vigenteHastaParametroLey,
           p.is_active
    FROM tblparametroley p";
        }

        public IEnumerable<ClsParametroLey> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsParametroLey>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);

            foreach (DataRow Fila in TablaDatos.Rows)
            {
                Lista.Add(new ClsParametroLey
                {
                    IdParametroLey = Convert.ToInt32(Fila["idParametroLey"]),
                    IdEmpresa = Convert.ToInt32(Fila["idEmpresa"]),
                    CodigoParametroLey = Fila["codigoParametroLey"].ToString(),
                    DescripcionParametroLey = Fila["descripcionParametroLey"].ToString(),
                    ValorParametroLey = Convert.ToDecimal(Fila["valorParametroLey"]),
                    VigenteDesdeParametroLey = Convert.ToDateTime(Fila["vigenteDesdeParametroLey"]),
                    VigenteHastaParametroLey = Convert.ToDateTime(Fila["vigenteHastaParametroLey"]),
                    IsActive = Convert.ToBoolean(Fila["is_active"])
                });
            }
            return Lista;
        }
    }
}
