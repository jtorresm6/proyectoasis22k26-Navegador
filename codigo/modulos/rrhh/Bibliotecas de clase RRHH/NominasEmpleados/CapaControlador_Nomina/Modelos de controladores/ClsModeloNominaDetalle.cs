using CapaControlador_Nomina.ObjetosdeValor;
using CapaModelo_Nomina.Entidades;
using CapaModelo_Nomina.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_Nomina.Modelos_de_controladores
{
    public class ClsModeloNominaDetalle
    {
        private int _IdNominaDetalle;
        private int _IdNominaEncabezado;
        private int _IdPercepcionDeduccion;
        private decimal _MontoNominaDetalle;
        private string _NombreConcepto;
        private string _TipoConcepto;
        private DateTime _CreatedAt;

        private ClsRepositorioNominaDetalle _RepositorioNominaDetalle;
        private ClsRepositorioPercepcionDeduccion _RepositorioConceptos;

        private List<ClsModeloNominaDetalle> _ListaDetalle;

        public int IdNominaDetalle { get => _IdNominaDetalle; private set => _IdNominaDetalle = value; }
        public int IdNominaEncabezado { get => _IdNominaEncabezado; private set => _IdNominaEncabezado = value; }
        public int IdPercepcionDeduccion { get => _IdPercepcionDeduccion; private set => _IdPercepcionDeduccion = value; }
        public string NombreConcepto { get => _NombreConcepto; private set => _NombreConcepto = value; }
        public string TipoConcepto { get => _TipoConcepto; private set => _TipoConcepto = value; }   // Percepción / Deducción
        public decimal MontoNominaDetalle { get => _MontoNominaDetalle; private set => _MontoNominaDetalle = value; }
        public DateTime CreatedAt { get => _CreatedAt; private set => _CreatedAt = value; }

        public ClsModeloNominaDetalle()
        {
            _RepositorioNominaDetalle = new ClsRepositorioNominaDetalle();
            _RepositorioConceptos = new ClsRepositorioPercepcionDeduccion();
        }

        public string RrhhMetGenerarDetalle(int IdNominaEncabezado)
        {
            try
            {
                if (IdNominaEncabezado <= 0)
                    return "Debe seleccionar una nómina";

                var Encabezados = new ClsModeloNominaEncabezado();
                ClsModeloNominaEncabezado Nomina = Encabezados.RrhhMetBuscarPorId(IdNominaEncabezado);
                if (Nomina == null)
                    return "No se encontró la nómina seleccionada";

                if (Nomina.EstadoNminaEncabezado != "Aprobada")
                    return "Solo se puede generar el detalle de nóminas en estado Aprobada";

                if (_RepositorioNominaDetalle.RrhhMetContarPorNomina(IdNominaEncabezado) > 0)
                    return "El detalle de esta nómina ya fue generado";

                var Calculo = new ClsModeloNominaEncabezado();
                Calculo.IdEmpleado = Nomina.IdEmpleado;
                Calculo.FechaInicioNominaEncabezado = Nomina.FechaInicioNominaEncabezado;
                Calculo.FechaFinalNominaEncabezado = Nomina.FechaFinalNominaEncabezado;

                string ErrorCalculo = Calculo.RrhhMetCalcularConceptos();
                if (ErrorCalculo != null)
                    return ErrorCalculo;

                if (Math.Round(Calculo.TotalPercepcionNominaEncabezado, 4) != Math.Round(Nomina.TotalPercepcionNominaEncabezado, 4) ||
                    Math.Round(Calculo.TotalDeduccionNominaEncabezado, 4) != Math.Round(Nomina.TotalDeduccionNominaEncabezado, 4))
                {
                    return "Los conceptos del empleado cambiaron desde que se generó la nómina " +
                           "(percepciones " + Calculo.TotalPercepcionNominaEncabezado.ToString("N2") + " contra " +
                           Nomina.TotalPercepcionNominaEncabezado.ToString("N2") + ", deducciones " +
                           Calculo.TotalDeduccionNominaEncabezado.ToString("N2") + " contra " +
                           Nomina.TotalDeduccionNominaEncabezado.ToString("N2") + "). " +
                           "Rechace la nómina y genérela de nuevo.";
                }

                var Lineas = new List<ClsNominaDetalle>();
                int? IdConceptoComision = null;

                foreach (ClsConceptoCalculado Concepto in Calculo.ConceptosCalculados)
                {
                    int IdConcepto = Concepto.IdPercepcionDeduccion;

                    if (IdConcepto <= 0) // es la comision de ventas: usa su propio concepto
                    {
                        if (IdConceptoComision == null)
                            IdConceptoComision = _RepositorioNominaDetalle.RrhhMetObtenerIdConceptoPorNombre(ClsConceptoCalculado.NombreConceptoComision);

                        if (IdConceptoComision == null)
                            return "Falta crear el concepto '" + ClsConceptoCalculado.NombreConceptoComision +
                                   "' en Percepciones y Deducciones para guardar las comisiones en el detalle";

                        IdConcepto = IdConceptoComision.Value;
                    }

                    Lineas.Add(new ClsNominaDetalle
                    {
                        IdNominaEncabezado = IdNominaEncabezado,
                        IdPercepcionDeduccion = IdConcepto,
                        MontoNominaDetalle = Concepto.Monto
                    });
                }

                if (Lineas.Count == 0)
                    return "La nómina no tiene percepciones ni deducciones que registrar";

                int Insertadas = _RepositorioNominaDetalle.RrhhMetAgregarLote(Lineas);
                return "Detalle generado exitosamente: " + Insertadas + " concepto(s)";
            }
            catch (Exception Ex)
            {
                return RrhhMetObtenerMensajeError(Ex);
            }
        }

        public List<ClsModeloNominaDetalle> RrhhMetObtenerTodos()
        {
            _ListaDetalle = RrhhMetConstruirLista(_RepositorioNominaDetalle.RrhhMetObtenerTodos());
            return _ListaDetalle;
        }

        public List<ClsModeloNominaDetalle> RrhhMetObtenerPorNomina(int IdNominaEncabezado)
        {
            _ListaDetalle = RrhhMetConstruirLista(_RepositorioNominaDetalle.RrhhMetObtenerPorNomina(IdNominaEncabezado));
            return _ListaDetalle;
        }

        public List<int> RrhhMetObtenerNominasConDetalle()
        {
            return _RepositorioNominaDetalle.RrhhMetObtenerNominasConDetalle();
        }

        public IEnumerable<ClsModeloNominaDetalle> RrhhMetBuscarPorNombre(string NombreConcepto)
        {
            if (_ListaDetalle == null)
                RrhhMetObtenerTodos();

            return _ListaDetalle.FindAll(e =>
                e._NombreConcepto.ToUpper().Contains(NombreConcepto.ToUpper()));
        }

        private List<ClsModeloNominaDetalle> RrhhMetConstruirLista(IEnumerable<ClsNominaDetalle> Datos)
        {
            var Conceptos = _RepositorioConceptos.RrhhMetObtenerTodos()
                .ToDictionary(c => c.IdPercepcionDeduccion);

            var Lista = new List<ClsModeloNominaDetalle>();
            foreach (ClsNominaDetalle Item in Datos)
            {
                ClsPercepcionDeduccion Concepto;
                bool Existe = Conceptos.TryGetValue(Item.IdPercepcionDeduccion, out Concepto);

                Lista.Add(new ClsModeloNominaDetalle
                {
                    _IdNominaDetalle = Item.IdNominaDetalle,
                    _IdNominaEncabezado = Item.IdNominaEncabezado,
                    _IdPercepcionDeduccion = Item.IdPercepcionDeduccion,
                    _NombreConcepto = Existe ? Concepto.NombrePercepcionDeduccion : "(concepto no disponible)",
                    _TipoConcepto = Existe ? (Concepto.EfectoPercepcionDeduccion ? "Percepción" : "Deducción") : string.Empty,
                    _MontoNominaDetalle = Item.MontoNominaDetalle,
                    _CreatedAt = Item.CreatedAt
                });
            }
            return Lista;
        }

        private string RrhhMetObtenerMensajeError(Exception Ex)
        {
            string Texto = Ex.Message ?? string.Empty;

            if (Texto.IndexOf("Unknown column", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Error de base de datos: falta una columna. Verifique que ejecutó los scripts de Nómina. Detalle: " + Texto;
            if (Texto.IndexOf("Duplicate entry", StringComparison.OrdinalIgnoreCase) >= 0)
                return "El detalle de esta nómina ya contiene alguno de esos conceptos.";
            if (Texto.IndexOf("foreign key", StringComparison.OrdinalIgnoreCase) >= 0)
                return "No se pudo guardar el detalle porque un concepto o la nómina ya no existe.";
            if (Ex is System.Data.Odbc.OdbcException)
                return "Error de base de datos: " + Texto;

            return "Ocurrió un error inesperado: " + Texto;
        }

    }
}
