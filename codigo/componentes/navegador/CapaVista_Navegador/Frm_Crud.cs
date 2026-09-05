using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_Navegador;
using System.Text.RegularExpressions;

namespace CapaVista_Navegador
{
    public partial class Frm_Crud : Form
    {
        // CAMBIO PARA LA FUTURA BASE DE DATOS:
        // modificar los datos de esta sección

        string nombreTabla = "tbl_empleados";

        string[] campos =
        {
            "id_empleado",
            "dpi_emp",
            "nit_emp",
            "nombre_emp",
            "apellido_emp",
            "fecha_nacimiento",
            "direccion_emp",
            "fecha_contratacion",
            "estado_emp",
            "id_puesto"
        };

        string[] camposPK =
        {
            "id_empleado"
        };

        string[] camposUnicos =
        {
            "dpi_emp",
            "nit_emp"
        };

        string[] camposCorreo =
        {
        };
        // FIN DE LA SECCIÓN A MODIFICAR
        Controlador controlador =
            new Controlador();

        public Frm_Crud()
        {
            InitializeComponent();

            Btn_Consultar.Click += Btn_Consultar_Click;
            Btn_ingresar.Click += Btn_ingresar_Click;
            Btn_guardar.Click += Btn_guardar_Click;
            Btn_cancelar.Click += Btn_cancelar_Click;
            Btn_refrescar.Click += Btn_refrescar_Click;
        }

        public void actualizarDataGridView()
        {
            try
            {
                DataTable dtVista =
                    controlador.llenarDgv(nombreTabla);

                Dgv_datos.DataSource = dtVista;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar los datos:\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void Btn_Consultar_Click(
            object sender,
            EventArgs e)
        {
            actualizarDataGridView();
        }

        private void Btn_ingresar_Click(
            object sender,
            EventArgs e)
        {
            CrearFormularioIngreso();
        }

        private void CrearFormularioIngreso()
        {
            Form formulario = new Form();

            formulario.Text = "Ingresar empleado";
            formulario.StartPosition =
                FormStartPosition.CenterParent;
            formulario.Size =
                new Size(520, 680);
            formulario.FormBorderStyle =
                FormBorderStyle.FixedDialog;
            formulario.MaximizeBox = false;
            formulario.MinimizeBox = false;

            Panel panel = new Panel();

            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;

            formulario.Controls.Add(panel);

            Dictionary<string, Control> controles =
                new Dictionary<string, Control>();

            int posicionY = 20;

            foreach (string campo in campos)
            {
                Label etiqueta = new Label();

                etiqueta.Text = campo;
                etiqueta.Location =
                    new Point(25, posicionY);
                etiqueta.AutoSize = true;

                Control control;

                if (campo == "fecha_nacimiento" ||
                    campo == "fecha_contratacion")
                {
                    DateTimePicker calendario =
                        new DateTimePicker();

                    calendario.Name =
                        "dtp_" + campo;

                    calendario.Location =
                        new Point(180, posicionY - 3);

                    calendario.Width = 260;

                    calendario.Format =
                        DateTimePickerFormat.Short;

                    calendario.Value =
                        DateTime.Today;

                    control = calendario;
                }
                else
                {
                    TextBox caja =
                        new TextBox();

                    caja.Name =
                        "txt_" + campo;

                    caja.Location =
                        new Point(180, posicionY - 3);

                    caja.Width = 260;

                    control = caja;
                }

                panel.Controls.Add(etiqueta);
                panel.Controls.Add(control);

                controles.Add(campo, control);

                posicionY += 45;
            }

            Button botonGuardar =
                new Button();

            botonGuardar.Text = "Guardar";
            botonGuardar.Width = 100;
            botonGuardar.Height = 35;

            botonGuardar.Location =
                new Point(180, posicionY + 10);

            panel.Controls.Add(botonGuardar);

            botonGuardar.Click += (sender, e) =>
            {
                GuardarRegistro(
                    formulario,
                    controles
                );
            };

            formulario.ShowDialog();
        }

        private void GuardarRegistro(
            Form formulario,
            Dictionary<string, Control> controles)
        {
            try
            {
                Dictionary<string, string> datos =
                    new Dictionary<string, string>();

                foreach (string campo in campos)
                {
                    string valor;

                    if (campo == "fecha_nacimiento" ||
                        campo == "fecha_contratacion")
                    {
                        DateTimePicker calendario =
                            (DateTimePicker)controles[campo];

                        valor =
                            calendario.Value.ToString(
                                "yyyy-MM-dd"
                            );
                    }
                    else
                    {
                        TextBox caja =
                            (TextBox)controles[campo];

                        valor =
                            caja.Text.Trim();
                    }

                    if (string.IsNullOrWhiteSpace(valor))
                    {
                        MessageBox.Show(
                            "El campo '" +
                            campo +
                            "' es obligatorio.",
                            "Validación",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        controles[campo].Focus();

                        return;
                    }

                    datos.Add(campo, valor);
                }

                foreach (string campoCorreo
                    in camposCorreo)
                {
                    if (datos.ContainsKey(campoCorreo))
                    {
                        string correo =
                            datos[campoCorreo];

                        if (!ValidarCorreo(correo))
                        {
                            MessageBox.Show(
                                "El correo ingresado en '" +
                                campoCorreo +
                                "' no tiene un formato válido.",
                                "Correo inválido",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );

                            controles[campoCorreo].Focus();

                            return;
                        }
                    }
                }

                string[] valoresPK =
                    new string[camposPK.Length];

                for (int i = 0;
                    i < camposPK.Length;
                    i++)
                {
                    if (!datos.ContainsKey(camposPK[i]))
                    {
                        MessageBox.Show(
                            "El campo de llave primaria '" +
                            camposPK[i] +
                            "' no existe.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );

                        return;
                    }

                    valoresPK[i] =
                        datos[camposPK[i]];
                }

                bool existePK =
                    controlador.ExisteLlavePrimaria(
                        nombreTabla,
                        camposPK,
                        valoresPK
                    );

                if (existePK)
                {
                    MessageBox.Show(
                        "La llave primaria ya existe.\n\n" +
                        "No se puede insertar un empleado " +
                        "con el mismo ID.",
                        "Registro duplicado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    controles[camposPK[0]].Focus();

                    return;
                }

                foreach (string campoUnico
                    in camposUnicos)
                {
                    if (!datos.ContainsKey(campoUnico))
                    {
                        continue;
                    }

                    string valor =
                        datos[campoUnico];

                    bool existe =
                        controlador.ExisteValorCampo(
                            nombreTabla,
                            campoUnico,
                            valor
                        );

                    if (existe)
                    {
                        MessageBox.Show(
                            "El valor '" +
                            valor +
                            "' ya está registrado " +
                            "en el campo '" +
                            campoUnico +
                            "'.",
                            "Dato duplicado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        controles[campoUnico].Focus();

                        return;
                    }
                }

                bool insertado =
                    controlador.InsertarRegistro(
                        nombreTabla,
                        datos
                    );

                if (insertado)
                {
                    MessageBox.Show(
                        "Empleado ingresado correctamente.",
                        "Registro guardado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    formulario.Close();

                    actualizarDataGridView();
                }
                else
                {
                    MessageBox.Show(
                        "No se pudo insertar el empleado.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error:\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private bool ValidarCorreo(string correo)
        {
            string patron =
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(
                correo,
                patron
            );
        }

        private void Btn_guardar_Click(
            object sender,
            EventArgs e)
        {
            MessageBox.Show(
                "Para ingresar un nuevo empleado " +
                "utilice el botón Ingresar.",
                "Información",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void Btn_cancelar_Click(
            object sender,
            EventArgs e)
        {
            MessageBox.Show(
                "Operación cancelada.",
                "Información",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void Btn_refrescar_Click(
            object sender,
            EventArgs e)
        {
            actualizarDataGridView();
        }
    }
}