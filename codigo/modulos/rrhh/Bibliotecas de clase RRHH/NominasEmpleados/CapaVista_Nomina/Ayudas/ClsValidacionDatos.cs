using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;


namespace CapaVista_Nomina.Ayudas
{
    public class ClsValidacionDatos
    {
        private readonly object _Modelo;

        public ClsValidacionDatos(object Modelo)
        {
            _Modelo = Modelo;
        }

        public bool RrhhMetValidar()
        {
            var Resultados = new List<ValidationResult>();
            var Contexto = new ValidationContext(_Modelo);
            bool Valido = Validator.TryValidateObject(_Modelo, Contexto, Resultados, true);

            if (!Valido)
            {
                var Mensaje = new StringBuilder();
                foreach (var Resultado in Resultados)
                    Mensaje.AppendLine("• " + Resultado.ErrorMessage);
                MessageBox.Show(Mensaje.ToString(), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return Valido;
        }
    }
}
