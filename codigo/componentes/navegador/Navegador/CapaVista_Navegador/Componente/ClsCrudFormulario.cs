using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_Navegador;
using CapaModelo_Navegador;

namespace CapaVista_Navegador
{
    // Diego Alejandro Cheng Peña 0901-22-8091 
    // Fecha actual : 14/09/2026

    // Arma el panel dinamico de un registro: labels, textbox, combo, fecha, checkbox
    public class ClsCrudFormulario
    {
        private readonly Control _Formulario;
        private readonly ClsCtrlTabla _CtrlTabla = new ClsCtrlTabla();
        private Panel NavegadorPnlRegistro;
        private Dictionary<string, Control> _Controles;
        private List<ClsColumnaInfo> _Esquema;
        private string _Tabla;
        private bool _ModoModificar, _PkCompuesta;

        // Representa una opcion de una llave foranea para mostrar valor y descripcion
        private class ClsOpcionForanea
        {
            public string Valor, Descripcion;

            // Muestra la llave junto con su descripcion
            public override string ToString() =>
                string.IsNullOrWhiteSpace(Descripcion) ? Valor : Valor + " - " + Descripcion;
        }

        // Indica si el panel del registro esta visible
        public bool Visible => NavegadorPnlRegistro?.Visible ?? false;
        // Indica si el formulario esta en modo modificar
        public bool ModoModificar => _ModoModificar;
        // Obtiene la posicion inferior del panel del registro
        public int Bottom => NavegadorPnlRegistro?.Bottom ?? 0;

        // Inicializa el formulario CRUD con el formulario principal
        public ClsCrudFormulario(Control Formulario) => _Formulario = Formulario;

        // Abre el formulario dinamico para insertar o modificar un registro
        public void NavegadorMetAbrir(string Tabla, List<ClsColumnaInfo> Esquema,
            bool Modificar, DataGridViewRow Fila, ClsCrudGrid Grid, int PosicionY)
        {

            //Cierra el panel si ya estaba abierto
            NavegadorMetCerrar();
            //Recopila la informacion de la tabla
            _Tabla = Tabla;
            _Esquema = Esquema;
            _ModoModificar = Modificar;
            _PkCompuesta = Esquema.FindAll(Columna => Columna.EsPK).Count > 1;

            //Crea y configura el panel de registro
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

            //Agrega el panel al formulario principal
            _Formulario.Controls.Add(NavegadorPnlRegistro);
            _Controles = new Dictionary<string, Control>();

            //Crea el titulo del formulario dinamico
            NavegadorPnlRegistro.Controls.Add(new Label
            {
                Name = "NavegadorLblTitulo",
                Text = (Modificar ? "Modificar registro - " : "Nuevo registro - ") + Tabla,
                Font = new Font(_Formulario.Font.FontFamily, 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 8)
            });

            int PosicionVertical = 34;

            //Recorre las columnas para crear sus etiquetas y controles
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

                //Resalta visualmente las llaves primarias y foraneas
                if (Columna.EsPK || Columna.EsFK)
                {
                    Etiqueta.Font = new Font(Etiqueta.Font, FontStyle.Bold);
                    Etiqueta.ForeColor = Columna.EsPK ? Color.DarkRed : Color.DarkBlue;
                }

                //Crea el control correspondiente al tipo de columna
                Control Campo = NavegadorMetCrearControl(
                    Columna, Modificar, Fila, Grid, PosicionVertical);

                NavegadorPnlRegistro.Controls.Add(Etiqueta);
                NavegadorPnlRegistro.Controls.Add(Campo);
                _Controles[Columna.Nombre] = Campo;

                PosicionVertical += 42;
            }

            //Muestra el panel de registro al frente del formulario
            NavegadorPnlRegistro.Visible = true;
            NavegadorPnlRegistro.BringToFront();
        }

        // Determina y crea el control adecuado para cada columna
        private Control NavegadorMetCrearControl(ClsColumnaInfo Columna,
            bool Modificar, DataGridViewRow Fila, ClsCrudGrid Grid, int PosicionVertical)
        {
            //Crea un combo para las columnas que son llaves foraneas
            if (Columna.EsFK &&
                !string.IsNullOrWhiteSpace(Columna.TablaFK) &&
                !string.IsNullOrWhiteSpace(Columna.ColumnaFK) &&
                !(Columna.EsPK && _PkCompuesta))
            {
                return NavegadorMetCrearCombo(
                    Columna, Modificar, Fila, Grid, PosicionVertical);
            }

            //Crea un selector de fecha para las columnas de tipo fecha
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

            //Crea una casilla para las columnas de tipo booleano
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

            //Crea un cuadro de texto para las columnas restantes
            TextBox CampoTexto = new TextBox
            {
                Name = "NavegadorTxt" + Columna.Nombre,
                Location = new Point(190, PosicionVertical),
                Width = 250,
                Text = Fila == null
                    ? ""
                    : Grid.NavegadorFuncObtenerValor(Fila, Columna.Nombre)
            };

            //Bloquea las llaves autoincrementales y las llaves en modificacion
            if ((!Modificar && Columna.EsAutoincremento) ||
                (Modificar && Columna.EsPK))
            {
                if (!Modificar && Columna.EsAutoincremento)
                    CampoTexto.Text = NavegadorFuncSiguienteLlave(Columna);

                CampoTexto.ReadOnly = true;
                CampoTexto.BackColor = Color.LightGray;
            }

            return CampoTexto;
        }
        //Genera actomaticamente la siguiente llave primaria
        private string NavegadorFuncSiguienteLlave(ClsColumnaInfo Columna)
        {
            try
            {
                //Consigue los registros de la tabla
                DataTable TablaDatos = _CtrlTabla.NavegadorFuncLlenarDgv(_Tabla);
                long UltimaLlave = 0;

                //Verifica que la tabla y la columna existan
                if (TablaDatos != null && TablaDatos.Columns.Contains(Columna.Nombre))
                {
                    //Busca la llave primaria mas alta en la tabla
                    foreach (DataRow Fila in TablaDatos.Rows)
                    {
                        //La valida como llave primaria
                        if (Fila[Columna.Nombre] != DBNull.Value &&
                        long.TryParse(Fila[Columna.Nombre].ToString(), out long Valor) &&
                        Valor > UltimaLlave)
                        UltimaLlave = Valor;
                    }
                }
                //Da la siguiente llave primaria
                return (UltimaLlave + 1).ToString();
            }
            catch (Exception Excepcion)
            {
                // Muerta error si no la genera
                MessageBox.Show("No se pudo generar la llave automática: " + Excepcion.Message,
                "Llave primaria", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "1";
            }
        }
        //Crea el combo con los valores disponibles de una llave foranea
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

            //Carga los registros de la tabla relacionada
            try
            {
                DataTable TablaDatos =
                    _CtrlTabla.NavegadorFuncLlenarDgv(Columna.TablaFK);

                //Verifica que exista la columna de la llave foranea
                if (TablaDatos == null ||
                    !TablaDatos.Columns.Contains(Columna.ColumnaFK))
                    return Combo;

                string ValorActual = Fila == null
                    ? ""
                    : Grid.NavegadorFuncObtenerValor(Fila, Columna.Nombre);

                //Busca una columna descriptiva para mostrar en el combo
                string ColumnaDescripcion =
                    NavegadorFuncDescripcion(TablaDatos, Columna.ColumnaFK);

                //Agrega la opcion vacia cuando la llave permite valores nulos
                if (Columna.Nullable)
                {
                    Combo.Items.Add(new ClsOpcionForanea
                    {
                        Valor = "",
                        Descripcion = "(ninguno)"
                    });
                }

                //Agrega los valores de la tabla relacionada al combo
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

                    //Selecciona automaticamente el valor actual del registro
                    if (string.Equals(
                        Opcion.Valor,
                        ValorActual,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        Combo.SelectedItem = Opcion;
                    }
                }
            }
            //Muestra una advertencia si ocurre un error al cargar la llave foranea
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

        //Busca una columna de texto adecuada para usar como descripcion
        private string NavegadorFuncDescripcion(
            DataTable Tabla, string Llave)
        {
            string[] Palabras =
            {
                "nombre", "descripcion", "titulo", "usuario", "codigo"
            };

            //Busca primero nombres de columnas descriptivas conocidas
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

            //Si no encuentra una descripcion conocida utiliza otra columna de texto
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

        //Obtiene la fecha actual o la fecha existente del registro
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

        //Convierte diferentes representaciones de verdadero a un valor booleano
        private bool NavegadorFuncEsVerdadero(string Valor)
        {
            Valor = Valor.ToLowerInvariant();

            return Valor == "1" ||
                   Valor == "true" ||
                   Valor == "yes" ||
                   Valor == "si";
        }

        //Obtiene los valores introducidos en todos los controles del formulario
        public Dictionary<string, string> NavegadorFuncObtenerDatos()
        {
            Dictionary<string, string> Datos =
                new Dictionary<string, string>();

            //Devuelve un diccionario vacio si no existen controles
            if (_Controles == null)
                return Datos;

            //Recorre los controles y obtiene su valor segun el tipo
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

        //Valida que las llaves primarias no esten vacias ni duplicadas
        public bool NavegadorFuncLlaveInvalida(DataGridView Grid, ClsCrudGrid GridControl)
        {
            // La validación de unicidad de llaves primarias se delegó al Controlador
            return false;
        }

        //Enfoca el control correspondiente al campo indicado
        private void NavegadorMetEnfocar(string Campo)
        {
            if (_Controles != null &&
                _Controles.ContainsKey(Campo))
            {
                _Controles[Campo].Focus();
            }
        }

        //Cierra y elimina el panel de registro del formulario para seguir con la navegacion normal del formulario
        public void NavegadorMetCerrar()
        {
            //Elimina el panel y libera sus recursos
            if (NavegadorPnlRegistro != null)
            {
                _Formulario.Controls.Remove(NavegadorPnlRegistro);
                NavegadorPnlRegistro.Dispose();
                NavegadorPnlRegistro = null;
            }

            //Limpia la referencia de los controles del formulario
            _Controles = null;
        }
    }
}

// Diego Alejandro Cheng Peña 0901-22-8091 
// Fecha actual : 14/09/2026
