using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Navegador
{
    public class Sentencias
    {
        conexionBD conn = new conexionBD();
        public OdbcDataAdapter llenarTbl(string nombreTabla)
        {
            string sSQL = "SELECT * FROM " + nombreTabla + " ;";
            OdbcDataAdapter daSentencias = new OdbcDataAdapter(sSQL, conn.conexion());
            return daSentencias;
        }
    }
}
