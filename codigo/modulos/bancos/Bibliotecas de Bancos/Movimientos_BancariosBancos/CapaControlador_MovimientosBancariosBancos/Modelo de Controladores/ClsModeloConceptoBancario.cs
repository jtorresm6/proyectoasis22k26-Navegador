/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_MovimientosBancarios.Entidades;
using CapaModelo_MovimientosBancarios.Repositorio;

namespace CapaControlador_MovimientosBancarios.Modelo_de_Controladores
{
    public class ClsModeloConceptoBancario
    {
        private readonly ClsRepositorioConceptoBancario _RepositorioConceptoBancario;

        public ClsModeloConceptoBancario()
        {
            _RepositorioConceptoBancario = new ClsRepositorioConceptoBancario();
        }

        public List<ClsConceptoBancario> BancosMetObtenerTodos()
        {
            return _RepositorioConceptoBancario.BancosMetObtenerTodos().ToList();
        }

        public IEnumerable<ClsConceptoBancario> BancosMetBuscarPorCodigo(
            string CodigoConceptoBancario)
        {
            var ListaConceptos = BancosMetObtenerTodos();

            return ListaConceptos.Where(
                Concepto => Concepto.CodigoConceptoBancario
                    .Equals(
                        CodigoConceptoBancario,
                        StringComparison.OrdinalIgnoreCase
                    )
            );
        }
        public DataTable BancosMetObtenerConceptosTabla()
        {
            var Conceptos = _RepositorioConceptoBancario.BancosMetObtenerTodos();

            DataTable Tabla = new DataTable();

            Tabla.Columns.Add("ID", typeof(long));
            Tabla.Columns.Add("Codigo", typeof(string));
            Tabla.Columns.Add("Concepto", typeof(string));
            Tabla.Columns.Add("Descripcion", typeof(string));
            Tabla.Columns.Add("Naturaleza", typeof(string));
            Tabla.Columns.Add("ClaseOperacion", typeof(string));
            Tabla.Columns.Add("TipoTercero", typeof(string));

            foreach (var Concepto in Conceptos)
            {
                Tabla.Rows.Add(
                    Concepto.idConceptoBancario,
                    Concepto.CodigoConceptoBancario,
                    Concepto.NombreConceptoBancario,
                    Concepto.DescripcionConceptoBancario,
                    Concepto.NaturalezaConceptoBancario,
                    Concepto.ClaseOperacionConceptoBancario,
                    Concepto.TipoTerceroConceptoBancario
                );
            }

            return Tabla;
        }


    }
}
