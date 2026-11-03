/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 23/10/2026
 * Modificado por : Daniella Jurado 0901-23-3679 (02/11/2026)
 * ==================================================================
 * Propósito :
 *  Ventana 9402 - RealizarEvaluacion. Mantenimiento de
 *  tblEvaluacionDesempeno con el Navegador.
 * Reglas especificas:
 *  Se ingresa el empleado evaluado, el evaluador, la fecha y la nota
 *  (1 a 5) de cada aspecto. El puntaje lo calcula la BD (trigger),
 *  por eso ese campo se deja vacío. El plan de mejora no se asigna
 *  aquí, sino en la ventana Plan de Mejora (9403).
 *  Cada nota se valida al salir del campo: si no está entre 1 y 5 se
 *  avisa y no se deja guardar.
 * ===================================================================
*/

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace CapaVista_EvaluacionDesempeño.Formularios
{
    public partial class FrmRealizarEvaluacion : Form
    {
        private static readonly string[] _ColumnasNotas =
        {
            "calidadEvaluacionDesempeno",
            "productividadEvaluacionDesempeno",
            "trabajoEquipoEvaluacionDesempeno",
            "puntualidadEvaluacionDesempeno",
            "iniciativaEvaluacionDesempeno"
        };

        public FrmRealizarEvaluacion()
        {
            InitializeComponent();
            RrhhNavRealizarEvaluacion.NavegadorMetConfigurar("tblEvaluacionDesempeno", 9000, 9402);

            // El panel de Ingresar/Modificar lo arma el Navegador después de este evento,
            // por eso las validaciones se conectan cuando termina (BeginInvoke).
            RrhhNavRealizarEvaluacion.NavegadorAccionSolicitada += Accion =>
            {
                if (Accion == "INGRESAR" || Accion == "MODIFICAR")
                    BeginInvoke((Action)RrhhMetConectarValidacionNotas);
            };

            // Cancelar, Salir y Ayuda deben funcionar aunque haya una nota inválida.
            foreach (string Boton in new[] { "NavegadorBtnCancelar", "NavegadorBtnSalir", "NavegadorBtnAyuda" })
                foreach (Control Control in RrhhNavRealizarEvaluacion.Controls.Find(Boton, true))
                    Control.CausesValidation = false;
        }

        private void RrhhMetConectarValidacionNotas()
        {
            foreach (string Columna in _ColumnasNotas)
                foreach (Control Campo in RrhhNavRealizarEvaluacion.Controls.Find("NavegadorTxt" + Columna, true))
                    Campo.Validating += RrhhMetValidarNota;
        }

        // Si la nota no está entre 1 y 5, el foco se queda en el campo y no se llega a Guardar.
        private void RrhhMetValidarNota(object sender, CancelEventArgs e)
        {
            var Campo = (TextBox)sender;
            string Texto = Campo.Text.Trim();
            if (Texto.Length == 0) return;

            int Nota;
            if (int.TryParse(Texto, out Nota) && Nota >= 1 && Nota <= 5) return;

            MessageBox.Show(
                "La evaluación únicamente acepta notas en el rango de 1 a 5,\n" +
                "siendo 1 la más baja y 5 la más alta.",
                "Nota fuera de rango", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Campo.SelectAll();
            e.Cancel = true;
        }
    }
}
