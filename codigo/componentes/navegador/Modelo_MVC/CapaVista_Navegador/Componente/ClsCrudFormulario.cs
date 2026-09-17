using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_Navegador;
using CapaEntidades_Navegador;

namespace CapaVista_Navegador
{
    public class ClsCrudFormulario
    {
        private readonly Form _Formulario;
        private readonly ClsCtrlTabla _CtrlTabla = new ClsCtrlTabla();
        private Panel NavegadorPnlRegistro;
        private Dictionary<string, Control> _Controles;
        private List<ClsColumnaInfo> _Esquema;
        private string _Tabla;
        private bool _ModoModificar, _PkCompuesta;

        private class ClsOpcionForanea
        {
            public string Valor, Descripcion;

            public override string ToString() =>
                string.IsNullOrWhiteSpace(Descripcion) ? Valor : Valor + " - " + Descripcion;
        }

        public bool Visible => NavegadorPnlRegistro?.Visible ?? false;
        public bool ModoModificar => _ModoModificar;
        public int Bottom => NavegadorPnlRegistro?.Bottom ?? 0;

        public ClsCrudFormulario(Form Formulario) => _Formulario = Formulario;

        public void NavegadorMetAbrir(string Tabla, List<ClsColumnaInfo> Esquema,
            bool Modificar, DataGridViewRow Fila, ClsCrudGrid Grid, int PosicionY)
        {
            NavegadorMetCerrar();
            _Tabla = Tabla;
            _Esquema = Esquema;
            _ModoModificar = Modificar;
            _PkCompuesta = Esquema.FindAll(Columna => Columna.EsPK).Count > 1;

            NavegadorPnlRegistro = new Panel
            {
                Name = "NavegadorPnlRegistro",
                Location = new Point(10, PosicionY),
                Width = _Formulario.ClientSize.Width - 20,
                Height = Math.Max(150, Math.Min(400, 50 + Esquema.Count * 42)),
                BackColor = Color.FromArgb(242, 233, 217),
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            _Formulario.Controls.Add(NavegadorPnlRegistro);
            _Controles = new Dictionary<string, Control>();

            NavegadorPnlRegistro.Controls.Add(new Label
            {
                Name = "NavegadorLblTitulo",
                Text = (Modificar ? "Modificar registro - " : "Nuevo registro - ") + Tabla,
                Font = new Font(_Formulario.Font.FontFamily, 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 8)
            });

            int PosicionVertical = 34;

            foreach (ClsColumnaInfo Columna in Esquema)
            {
                Label Etiqueta = new Label
                {
                    Name = "NavegadorLbl" + Columna.Nombre,
                    Text = Columna.Nombre + (Columna.EsPK ? " [PK]" : "") +
                           (Columna.EsFK ? " [FK]" : ""),
                    Location = new Point(15, PosicionVertical + 4),
                    AutoSize = true
                };

                if (Columna.EsPK || Columna.EsFK)
                {
                    Etiqueta.Font = new Font(Etiqueta.Font, FontStyle.Bold);
                    Etiqueta.ForeColor = Columna.EsPK ? Color.DarkRed : Color.DarkBlue;
                }

                Control Campo = NavegadorMetCrearControl(
                    Columna, Modificar, Fila, Grid, PosicionVertical);

                NavegadorPnlRegistro.Controls.Add(Etiqueta);
                NavegadorPnlRegistro.Controls.Add(Campo);
                _Controles[Columna.Nombre] = Campo;

                PosicionVertical += 42;
            }

            NavegadorPnlRegistro.Visible = true;
            NavegadorPnlRegistro.BringToFront();
        }

        private Control NavegadorMetCrearControl(ClsColumnaInfo Columna,
            bool Modificar, DataGridViewRow Fila, ClsCrudGrid Grid, int PosicionVertical)
        {
            if (Columna.EsFK &&
                !string.IsNullOrWhiteSpace(Columna.TablaFK) &&
                !string.IsNullOrWhiteSpace(Columna.ColumnaFK) &&
                !(Columna.EsPK && _PkCompuesta))
            {
                return NavegadorMetCrearCombo(
                    Columna, Modificar, Fila, Grid, PosicionVertical);
            }

            if (ClsTipoColumna.NavegadorFuncEsFecha(Columna))
            {
                return new DateTimePicker
                {
                    Name = "NavegadorDtp" + Columna.Nombre,
                    Location = new Point(190, PosicionVertical),
                    Width = 250,
                    Format = DateTimePickerFormat.Short,
                    Value = NavegadorFuncFecha(Fila, Columna.Nombre, Grid),
                    Enabled = !(Columna.EsPK && Modificar)
                };
            }

            if (ClsTipoColumna.NavegadorFuncEsBooleano(Columna))
            {
                return new CheckBox
                {
                    Name = "NavegadorChk" + Columna.Nombre,
                    Text = "Sí (marcado) / No (desmarcado)",
                    Location = new Point(190, PosicionVertical + 3),
                    AutoSize = true,
                    Checked = Fila != null &&
                        NavegadorFuncEsVerdadero(
                            Grid.NavegadorFuncObtenerValor(Fila, Columna.Nombre)),
                    Enabled = !(Columna.EsPK && Modificar)
                };
            }

            TextBox CampoTexto = new TextBox
            {
                Name = "NavegadorTxt" + Columna.Nombre,
                Location = new Point(190, PosicionVertical),
                Width = 250,
                Text = Fila == null
                    ? ""
                    : Grid.NavegadorFuncObtenerValor(Fila, Columna.Nombre)
            };

            if ((!Modificar && Columna.EsAutoincremento) ||
                (Modificar && Columna.EsPK))
            {
                if (!Modificar && Columna.EsAutoincremento)
                    CampoTexto.Text = "(automático)";

                CampoTexto.ReadOnly = true;
                CampoTexto.BackColor = Color.LightGray;
            }

            return CampoTexto;
        }

        private Control NavegadorMetCrearCombo(ClsColumnaInfo Columna,
            bool Modificar, DataGridViewRow Fila, ClsCrudGrid Grid, int PosicionVertical)
        {
            ComboBox Combo = new ComboBox
            {
                Name = "NavegadorCbo" + Columna.Nombre,
                Location = new Point(190, PosicionVertical),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = !(Columna.EsPK && Modificar)
            };

            try
            {
                DataTable TablaDatos =
                    _CtrlTabla.NavegadorFuncLlenarDgv(Columna.TablaFK);

                if (TablaDatos == null ||
                    !TablaDatos.Columns.Contains(Columna.ColumnaFK))
                    return Combo;

                string ValorActual = Fila == null
                    ? ""
                    : Grid.NavegadorFuncObtenerValor(Fila, Columna.Nombre);

                string ColumnaDescripcion =
                    NavegadorFuncDescripcion(TablaDatos, Columna.ColumnaFK);

                if (Columna.Nullable)
                {
                    Combo.Items.Add(new ClsOpcionForanea
                    {
                        Valor = "",
                        Descripcion = "(ninguno)"
                    });
                }

                foreach (DataRow FilaDatos in TablaDatos.Rows)
                {
                    ClsOpcionForanea Opcion = new ClsOpcionForanea
                    {
                        Valor = Convert.ToString(
                            FilaDatos[Columna.ColumnaFK]),

                        Descripcion = ColumnaDescripcion == null
                            ? ""
                            : Convert.ToString(
                                FilaDatos[ColumnaDescripcion])
                    };

                    Combo.Items.Add(Opcion);

                    if (string.Equals(
                        Opcion.Valor,
                        ValorActual,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        Combo.SelectedItem = Opcion;
                    }
                }
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show(
                    "No se pudieron cargar los valores de '" +
                    Columna.TablaFK + "' para '" +
                    Columna.Nombre + "': " +
                    Excepcion.Message,
                    "Llave foránea",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            return Combo;
        }

        private string NavegadorFuncDescripcion(
            DataTable Tabla, string Llave)
        {
            string[] Palabras =
            {
                "nombre", "descripcion", "titulo", "usuario", "codigo"
            };

            foreach (string Palabra in Palabras)
            {
                foreach (DataColumn Columna in Tabla.Columns)
                {
                    if (Columna.DataType == typeof(string) &&
                        !Columna.ColumnName.Equals(
                            Llave, StringComparison.OrdinalIgnoreCase) &&
                        Columna.ColumnName.IndexOf(
                            Palabra, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return Columna.ColumnName;
                    }
                }
            }

            foreach (DataColumn Columna in Tabla.Columns)
            {
                if (Columna.DataType == typeof(string) &&
                    !Columna.ColumnName.Equals(
                        Llave, StringComparison.OrdinalIgnoreCase))
                {
                    return Columna.ColumnName;
                }
            }

            return null;
        }

        private DateTime NavegadorFuncFecha(
            DataGridViewRow Fila, string Campo, ClsCrudGrid Grid)
        {
            DateTime Fecha;

            return Fila != null &&
                DateTime.TryParse(
                    Grid.NavegadorFuncObtenerValor(Fila, Campo),
                    out Fecha)
                ? Fecha
                : DateTime.Today;
        }

        private bool NavegadorFuncEsVerdadero(string Valor)
        {
            Valor = Valor.ToLowerInvariant();

            return Valor == "1" ||
                   Valor == "true" ||
                   Valor == "yes" ||
                   Valor == "si";
        }

        public Dictionary<string, string> NavegadorFuncObtenerDatos()
        {
            Dictionary<string, string> Datos =
                new Dictionary<string, string>();

            if (_Controles == null)
                return Datos;

            foreach (KeyValuePair<string, Control> Control in _Controles)
            {
                if (Control.Value is DateTimePicker Fecha)
                {
                    Datos[Control.Key] =
                        Fecha.Value.ToString("yyyy-MM-dd");
                }
                else if (Control.Value is CheckBox Casilla)
                {
                    Datos[Control.Key] =
                        Casilla.Checked ? "1" : "0";
                }
                else if (Control.Value is ComboBox Combo)
                {
                    Datos[Control.Key] =
                        (Combo.SelectedItem as ClsOpcionForanea)?.Valor ?? "";
                }
                else
                {
                    Datos[Control.Key] =
                        Control.Value.Text.Trim();
                }
            }

            return Datos;
        }

        public bool NavegadorFuncLlaveInvalida(
            DataGridView Grid, ClsCrudGrid GridControl)
        {
            if (_ModoModificar || _Controles == null)
                return false;

            List<ClsColumnaInfo> Llaves =
                _Esquema.FindAll(Columna => Columna.EsPK);

            Dictionary<string, string> Datos =
                NavegadorFuncObtenerDatos();

            foreach (ClsColumnaInfo Columna in Llaves)
            {
                if (!Columna.EsAutoincremento &&
                    (!Datos.ContainsKey(Columna.Nombre) ||
                     string.IsNullOrEmpty(Datos[Columna.Nombre])))
                {
                    MessageBox.Show(
                        "Debe ingresar un valor para la llave primaria '" +
                        Columna.Nombre + "'.",
                        "Llave primaria",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    NavegadorMetEnfocar(Columna.Nombre);
                    return true;
                }
            }

            if (Grid == null || GridControl == null)
                return false;

            foreach (DataGridViewRow Fila in Grid.Rows)
            {
                if (Fila.IsNewRow)
                    continue;

                bool Coincide = true;

                foreach (ClsColumnaInfo Columna in Llaves)
                {
                    if (!Columna.EsAutoincremento &&
                        !string.Equals(
                            GridControl.NavegadorFuncObtenerValor(
                                Fila, Columna.Nombre).Trim(),
                            Datos[Columna.Nombre],
                            StringComparison.OrdinalIgnoreCase))
                    {
                        Coincide = false;
                        break;
                    }
                }

                if (Coincide &&
                    Llaves.Exists(Columna => !Columna.EsAutoincremento))
                {
                    MessageBox.Show(
                        "Ya existe un registro con esa llave primaria.",
                        "Llave duplicada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    NavegadorMetEnfocar(Llaves[0].Nombre);
                    return true;
                }
            }

            return false;
        }

        private void NavegadorMetEnfocar(string Campo)
        {
            if (_Controles != null &&
                _Controles.ContainsKey(Campo))
            {
                _Controles[Campo].Focus();
            }
        }

        public void NavegadorMetCerrar()
        {
            if (NavegadorPnlRegistro != null)
            {
                _Formulario.Controls.Remove(NavegadorPnlRegistro);
                NavegadorPnlRegistro.Dispose();
                NavegadorPnlRegistro = null;
            }

            _Controles = null;
        }
    }
}