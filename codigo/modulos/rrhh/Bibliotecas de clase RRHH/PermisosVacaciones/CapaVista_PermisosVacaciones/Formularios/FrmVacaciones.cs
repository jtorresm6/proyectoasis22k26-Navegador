/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 23/10/2026
 * Modificado por : Daniella Jurado 0901-23-3679 (02/11/2026)
 * ==================================================================
 * Propósito :
 *  Ventana 9203 - Vacaciones. Mantenimiento de tblVacacionEmpleado con el Navegador.
 * Reglas especificas:
 *  Se ingresa el empleado, la fecha inicio y fin y una observación
 *  opcional. Los días (diasTomarVacacionEmpleado) son los días hábiles
 *  del rango y se calculan solos.
 *  Al elegir el empleado se muestra su fecha de contratación
 *  (tblEmpleado), su antigüedad a la fecha actual y si puede optar a
 *  vacaciones. Mientras no se cumplan las reglas del Código de Trabajo
 *  de Guatemala (ver ClsControladorPermisosVacaciones) el botón Guardar
 *  del Navegador queda deshabilitado.
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using CapaControlador_PermisosVacaciones.Clases;

namespace CapaVista_PermisosVacaciones.Formularios
{
    public partial class FrmVacaciones : Form
    {
        private readonly ClsControladorPermisosVacaciones _Controlador = new ClsControladorPermisosVacaciones();
        private readonly string _TextoInicial;
        private Control _BtnGuardar;

        public FrmVacaciones()
        {
            InitializeComponent();
            _TextoInicial = RrhhLblAntiguedad.Text;
            RrhhNavVacaciones.NavegadorMetConfigurar(
                _Controlador.RrhhFuncTablaVacaciones(),
                _Controlador.RrhhFuncIdModulo(),
                _Controlador.RrhhFuncIdAplicacionVacaciones());

            Control[] Guardar = RrhhNavVacaciones.Controls.Find("NavegadorBtnGuardar", true);
            _BtnGuardar = Guardar.Length > 0 ? Guardar[0] : null;

            RrhhNavVacaciones.NavegadorAccionSolicitada += Accion =>
            {
                // El panel lo arma el Navegador después de este evento, por eso se espera con BeginInvoke.
                if (Accion == "INGRESAR" || Accion == "MODIFICAR")
                    BeginInvoke((Action)(() => RrhhMetConectar(Accion == "MODIFICAR")));
                else if (Accion == "CANCELAR" || Accion == "CONSULTAR" || Accion == "REFRESCAR" || Accion == "SALIR")
                    RrhhMetPermitirGuardar(true, _TextoInicial);
            };
        }

        private void RrhhMetConectar(bool Modificar)
        {
            string[] Fechas = _Controlador.RrhhFuncFechasVacaciones();
            var Inicio = RrhhFuncBuscar("NavegadorDtp" + Fechas[0]) as DateTimePicker;
            var Fin = RrhhFuncBuscar("NavegadorDtp" + Fechas[1]) as DateTimePicker;
            // El empleado es un combo (llave foránea); si el Navegador lo dibujara como texto también sirve.
            Control Empleado = RrhhFuncBuscar("NavegadorCboidEmpleado") ?? RrhhFuncBuscar("NavegadorTxtidEmpleado");
            var Dias = RrhhFuncBuscar("NavegadorTxtdiasTomarVacacionEmpleado") as TextBox;
            var Id = RrhhFuncBuscar("NavegadorTxtidVacacionEmpleado") as TextBox;
            if (Inicio == null || Fin == null || Empleado == null)
            {
                RrhhMetPermitirGuardar(false, "No se encontraron en el Navegador los campos de empleado y fechas de " +
                    _Controlador.RrhhFuncTablaVacaciones() + ". Verifique que se corrió el script de la base de datos.");
                return;
            }

            // Al ingresar, las reglas de días y partes se revisan hasta que el usuario elige fechas.
            bool FechasElegidas = Modificar;

            // Revisa las reglas, actualiza el resumen y habilita o no Guardar.
            Action<bool> Revisar = Avisar =>
            {
                if (Dias != null) Dias.Text = _Controlador.RrhhFuncDiasHabiles(Inicio.Value, Fin.Value).ToString();

                int IdEmpleado = RrhhFuncIdOpcion(Empleado is ComboBox ? ((ComboBox)Empleado).SelectedItem : Empleado.Text);
                if (IdEmpleado == 0)
                {
                    RrhhMetPermitirGuardar(false, "Elija un empleado para ver su fecha de contratación y si puede optar a vacaciones.");
                    return;
                }

                int IdActual;
                int.TryParse(Id == null ? "" : Id.Text, out IdActual);

                string Mensaje;
                try
                {
                    RrhhLblAntiguedad.Text = _Controlador.RrhhFuncResumenEmpleado(IdEmpleado, IdActual);
                    Mensaje = FechasElegidas
                        ? _Controlador.RrhhFuncValidarVacacion(IdEmpleado, Inicio.Value, Fin.Value, IdActual)
                        : _Controlador.RrhhFuncPuedeOptar(IdEmpleado);
                }
                catch (Exception Ex)
                {
                    Mensaje = "No se pudieron validar las vacaciones del empleado: " + Ex.Message;
                }

                RrhhMetPermitirGuardar(Mensaje == null, RrhhLblAntiguedad.Text);
                if (Avisar && Mensaje != null)
                    MessageBox.Show(Mensaje, "Vacaciones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };

            if (Dias != null) Dias.ReadOnly = true;
            if (Empleado is ComboBox)
                ((ComboBox)Empleado).SelectionChangeCommitted += (Remitente, Evento) => Revisar(true);
            else
                Empleado.Leave += (Remitente, Evento) => Revisar(true);
            EventHandler FechaCambiada = (Remitente, Evento) => { FechasElegidas = true; Revisar(false); };
            Inicio.ValueChanged += FechaCambiada;
            Fin.ValueChanged += FechaCambiada;
            Inicio.Leave += (Remitente, Evento) => Revisar(true);
            Fin.Leave += (Remitente, Evento) => Revisar(true);
            Revisar(false);
        }

        private void RrhhMetPermitirGuardar(bool Permitir, string Resumen)
        {
            if (_BtnGuardar != null) _BtnGuardar.Enabled = Permitir;
            RrhhLblAntiguedad.Text = Resumen;
        }

        private Control RrhhFuncBuscar(string Nombre)
        {
            Control[] Encontrados = RrhhNavVacaciones.Controls.Find(Nombre, true);
            return Encontrados.Length > 0 ? Encontrados[0] : null;
        }

        // El combo del Navegador muestra "id - nombre"; se toma el id.
        private static int RrhhFuncIdOpcion(object Opcion)
        {
            string Texto = Opcion == null ? "" : Opcion.ToString();
            int Separador = Texto.IndexOf(" - ", StringComparison.Ordinal);
            int Id;
            int.TryParse(Separador < 0 ? Texto : Texto.Substring(0, Separador), out Id);
            return Id;
        }
    }
}
