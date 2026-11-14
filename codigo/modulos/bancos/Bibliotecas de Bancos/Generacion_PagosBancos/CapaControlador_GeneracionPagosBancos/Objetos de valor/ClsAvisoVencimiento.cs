using System;

/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 05/11/2026
 * ================================================================
 * Propósito :
 *  Objeto de valor con los datos de una obligación que ya venció o
 *  está por vencer, para que la vista la muestre en el aviso al
 *  tesorero. Dias es negativo si ya venció, 0 si vence hoy y
 *  positivo con los días que faltan.
 * ================================================================
 */

namespace CapaControlador_GeneracionPagos
{
    public class ClsAvisoVencimiento
    {
        public string Origen { get; private set; }
        public string Documento { get; private set; }
        public string Beneficiario { get; private set; }
        public string Importe { get; private set; }
        public DateTime Vencimiento { get; private set; }
        public int Dias { get; private set; }

        public ClsAvisoVencimiento(string Origen, string Documento, string Beneficiario, string Importe, DateTime Vencimiento, int Dias)
        {
            this.Origen = Origen;
            this.Documento = Documento;
            this.Beneficiario = Beneficiario;
            this.Importe = Importe;
            this.Vencimiento = Vencimiento;
            this.Dias = Dias;
        }
    }
}
