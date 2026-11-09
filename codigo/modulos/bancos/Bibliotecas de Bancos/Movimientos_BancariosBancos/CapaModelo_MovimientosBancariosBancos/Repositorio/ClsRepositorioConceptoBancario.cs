/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaModelo_MovimientosBancarios.Contratos;
using CapaModelo_MovimientosBancarios.Entidades;

namespace CapaModelo_MovimientosBancarios.Repositorio
{
    public class ClsRepositorioConceptoBancario : ClsSentencias, IRepositorioConceptoBancario
    {
        private string _SelectAll;

        public ClsRepositorioConceptoBancario()
        {
            _SelectAll = @"SELECT idConceptoBancario, codigoConceptoBancario, nombreConceptoBancario, descripcionConceptoBancario, naturalezaConceptoBancario,
            claseOperacionConceptoBancario, tipoTerceroConceptoBancario, is_active, created_at, updated_at FROM tblConceptoBancario WHERE is_active = 1 ORDER BY codigoConceptoBancario";
        }

        public int BancosMetAgregar(ClsConceptoBancario Entidad)
        {
            throw new NotSupportedException("El registro de conceptos bancarios aún no está implementado.");
        }

        public int BancosMetEditar(ClsConceptoBancario Entidad)
        {
            throw new NotSupportedException("La edición de conceptos bancarios aún no está implementada.");
        }

        public int BancosMetRemover(ClsConceptoBancario Entidad)
        {
            throw new NotSupportedException("La eliminación de conceptos bancarios aún no está implementada.");
        }

        public IEnumerable<ClsConceptoBancario> BancosMetObtenerTodos()
        {
            var ListaConceptos = new List<ClsConceptoBancario>();

            var TablaDatos = BancosMetEjecucionConsulta(
                _SelectAll,
                CommandType.Text
            );

            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Concepto = new ClsConceptoBancario();
                Concepto.idConceptoBancario = Convert.ToInt64(Fila["idConceptoBancario"]);
                Concepto.CodigoConceptoBancario = Fila["codigoConceptoBancario"].ToString();
                Concepto.NombreConceptoBancario = Fila["nombreConceptoBancario"].ToString();
                Concepto.DescripcionConceptoBancario = Fila["descripcionConceptoBancario"] == DBNull.Value ? "" : Fila["descripcionConceptoBancario"].ToString();
                Concepto.NaturalezaConceptoBancario = Fila["naturalezaConceptoBancario"].ToString();
                Concepto.ClaseOperacionConceptoBancario = Fila["claseOperacionConceptoBancario"].ToString();
                Concepto.TipoTerceroConceptoBancario = Fila["tipoTerceroConceptoBancario"].ToString();
                Concepto.IsActive = Convert.ToBoolean(Fila["is_active"]);
                Concepto.CreatedAt = Convert.ToDateTime(Fila["created_at"]);
                Concepto.UpdatedAt = Convert.ToDateTime(Fila["updated_at"]);
                ListaConceptos.Add(Concepto);
            }

            TablaDatos.Clear();
            TablaDatos = null;
            return ListaConceptos;
        }
    }
}
