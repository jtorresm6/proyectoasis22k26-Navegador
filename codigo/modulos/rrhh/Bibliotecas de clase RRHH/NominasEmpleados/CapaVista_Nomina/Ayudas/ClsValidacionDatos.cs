/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Transporta el resultado del cálculo de una percepción o
 *  deducción, incluyendo identificador, nombre, efecto y monto.
 *  Define el nombre del concepto de comisión por ventas utilizado
 *  para integrar las comisiones al cálculo y detalle de nómina.
 * ==================================================================
 */

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
