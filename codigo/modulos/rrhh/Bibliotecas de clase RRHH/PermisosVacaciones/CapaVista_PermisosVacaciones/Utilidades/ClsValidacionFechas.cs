using System;
using System.ComponentModel;
using System.Windows.Forms;
using CapaVista_Navegador;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Valida en el panel de Ingresar/Modificar del Navegador que la fecha
 *  fin no sea anterior a la fecha inicio. Si lo es, avisa en español y
 *  no deja salir del campo, así el Navegador no llega a Guardar.
 * ===================================================================
*/

namespace CapaVista_PermisosVacaciones.Utilidades
{
    public static class ClsValidacionFechas
    {
        // Columnas: [0] = fecha inicio, [1] = fecha fin
        public static void RrhhMetConfigurar(Form Ventana, Navegador Nav, string[] Columnas)
        {
            // El panel lo arma el Navegador después de este evento, por eso se espera con BeginInvoke.
            Nav.NavegadorAccionSolicitada += Accion =>
            {
                if (Accion == "INGRESAR" || Accion == "MODIFICAR")
                    Ventana.BeginInvoke((Action)(() => RrhhMetConectar(Nav, Columnas)));
            };

            // Cancelar, Salir y Ayuda deben funcionar aunque las fechas estén mal.
            foreach (string Boton in new[] { "NavegadorBtnCancelar", "NavegadorBtnSalir", "NavegadorBtnAyuda" })
                foreach (Control Control in Nav.Controls.Find(Boton, true))
                    Control.CausesValidation = false;
        }

        private static void RrhhMetConectar(Navegador Nav, string[] Columnas)
        {
            DateTimePicker Inicio = RrhhFuncBuscar(Nav, Columnas[0]);
            DateTimePicker Fin = RrhhFuncBuscar(Nav, Columnas[1]);
            if (Inicio == null || Fin == null) return;

            CancelEventHandler Validar = (Remitente, Evento) =>
            {
                if (Fin.Value.Date >= Inicio.Value.Date) return;
                MessageBox.Show("La fecha fin no puede ser anterior a la fecha inicio.",
                    "Fechas incorrectas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Evento.Cancel = true;
            };
            Inicio.Validating += Validar;
            Fin.Validating += Validar;
        }

        private static DateTimePicker RrhhFuncBuscar(Navegador Nav, string Columna)
        {
            Control[] Encontrados = Nav.Controls.Find("NavegadorDtp" + Columna, true);
            return Encontrados.Length > 0 ? Encontrados[0] as DateTimePicker : null;
        }
    }
}
