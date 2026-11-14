// =====================================================================
// Autor:       Andre De Jesus Gonzalez Camey
// Carné:       9959-23-3117
// Módulo:      Tesorería y Bancos – Enlace con otros módulos
// Descripción: Control de usuario reutilizable que consulta, ordena, pagina y exporta
//              los registros de un módulo.
// Fecha:       12/11/2026
// =====================================================================
using System;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_EnlaceB.Entidades;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class UsrConsultaModulo : UserControl
    {
        private ModuloEnlace _Modulo;
        private ClsControladorEnlaceB _Controlador;
        private readonly ClsPaginadorEnlace _Paginador;
        internal bool Ocupado { get; private set; }

        public UsrConsultaModulo()
        {
            InitializeComponent();
            _Paginador = new ClsPaginadorEnlace(_BancarioDgvTabla, _BancarioBtnAnterior, _BancarioBtnSiguiente, _BancarioLblPagina);
            _BancarioDtpInicio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            _BancarioDtpFin.Value = DateTime.Today;
            _BancarioDtpInicio.ValueChanged += BancarioMetCambioFecha;
            _BancarioDtpFin.ValueChanged += BancarioMetCambioFecha;
        }

        internal void BancarioMetConfigurar(ModuloEnlace Modulo, ClsControladorEnlaceB Controlador)
        {
            _Modulo = Modulo;
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
        }

        private void BancarioMetCambioFecha(object Remitente, EventArgs Argumentos)
        {
            ClsEstiloEnlace.BancarioMetValidacion(_BancarioPnlBordeFin, _BancarioLblErrorFin, "");
        }

        private async void BancarioMetConsultar(object Remitente, EventArgs Argumentos)
        {
            if (Ocupado)
                return;
            DateTime Inicio = _BancarioDtpInicio.Value.Date;
            DateTime Fin = _BancarioDtpFin.Value.Date;
            if (Inicio > Fin || Fin.Year >= 9999)
            {
                _BancarioLblEstado.Text = "Revise el rango de fechas.";
                ClsEstiloEnlace.BancarioMetValidacion(_BancarioPnlBordeFin, _BancarioLblErrorFin, "Revise el rango de fechas.");
                _BancarioDtpFin.Focus();
                return;
            }

            Ocupado = true;
            Enabled = false;
            UseWaitCursor = true;
            _BancarioLblEstado.Text = "Consultando…";
            try
            {
                var Datos = await Task.Run(() => _Controlador.BancarioMetConsultarModulo(_Modulo, Inicio, Fin));
                _Paginador.BancarioMetCargar(Datos);
                _BancarioLblEstado.Text = Datos.Rows.Count == 0 ? "No hay registros en el período seleccionado." : Datos.Rows.Count + " filas.";
            }
            catch (Exception Ex)
            {
                _Paginador.BancarioMetCargar(null);
                _BancarioLblEstado.Text = "No se pudo consultar.";
                ClsEstiloEnlace.BancarioMetError(this, Ex);
            }
            finally
            {
                Ocupado = false;
                Enabled = true;
                UseWaitCursor = false;
            }
        }

        private void BancarioMetVolver(object Remitente, EventArgs Argumentos)
        {
            FindForm().Close();
        }

        private void BancarioMetLimpiar(object Remitente, EventArgs Argumentos)
        {
            _Paginador.BancarioMetCargar(null);
            _BancarioLblEstado.Text = "Sin consultar.";
        }

        private void BancarioMetExportar(object Remitente, EventArgs Argumentos)
        {
            var Datos = _Paginador.Datos;
            if (Datos == null || Datos.Rows.Count == 0)
            {
                MessageBox.Show(this, "Consulte registros antes de exportar.", "Enlace con otros módulos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var BancarioSfdDialogo = new SaveFileDialog
            {
                Filter = "Archivo CSV|*.csv",
                FileName = "Enlace_" + _Modulo + ".csv"
            }

            )
            {
                if (BancarioSfdDialogo.ShowDialog(this) != DialogResult.OK)
                    return;
                try
                {
                    using (var Writer = new StreamWriter(BancarioSfdDialogo.FileName, false, new UTF8Encoding(true)))
                    {
                        var Campos = new string[Datos.Columns.Count];
                        for (int Indice = 0; Indice < Campos.Length; Indice++)
                            Campos[Indice] = BancarioMetCsv(Datos.Columns[Indice].ColumnName);
                        Writer.WriteLine(string.Join(",", Campos));
                        foreach (DataRow Fila in Datos.Rows)
                        {
                            for (int Indice = 0; Indice < Campos.Length; Indice++)
                                Campos[Indice] = BancarioMetCsv(Fila[Indice] is DateTime ? ((DateTime)Fila[Indice]).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToString(Fila[Indice], CultureInfo.InvariantCulture));
                            Writer.WriteLine(string.Join(",", Campos));
                        }
                    }

                    _BancarioLblEstado.Text = "Archivo exportado.";
                    ClsEstiloEnlace.BancarioMetExito(this, "El archivo CSV se exportó correctamente.");
                }
                catch (Exception Ex)
                {
                    ClsEstiloEnlace.BancarioMetError(this, Ex);
                }
            }
        }

        private static string BancarioMetCsv(string Valor)
        {
            if (!string.IsNullOrEmpty(Valor) && "=+-@\t\r".IndexOf(Valor[0]) >= 0)
                Valor = "'" + Valor;
            return "\"" + (Valor ?? "").Replace("\"", "\"\"") + "\"";
        }
    }
}
