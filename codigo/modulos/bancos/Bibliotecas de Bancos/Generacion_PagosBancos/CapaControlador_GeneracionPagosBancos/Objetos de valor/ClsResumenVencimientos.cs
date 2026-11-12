using System.Collections.Generic;

/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 05/11/2026
 * ================================================================
 * Propósito :
 *  Objeto de valor con el resumen de vencimientos que el
 *  controlador le entrega a la vista: cuántas obligaciones ya
 *  vencieron, cuántas vencen hoy y cuántas vencen dentro de los
 *  días de aviso, junto con el detalle de cada una.
 * ================================================================
 */

namespace CapaControlador_GeneracionPagos
{
    public class ClsResumenVencimientos
    {
        public int Vencidas { get; private set; }
        public int VencenHoy { get; private set; }
        public int PorVencer { get; private set; }
        public List<ClsAvisoVencimiento> Avisos { get; private set; }

        public bool HayAvisos
        {
            get { return Avisos.Count > 0; }
        }

        // Hay algo que ya venció o que vence hoy
        public bool HayUrgentes
        {
            get { return Vencidas + VencenHoy > 0; }
        }

        public ClsResumenVencimientos(List<ClsAvisoVencimiento> Avisos)
        {
            this.Avisos = Avisos;
            foreach (ClsAvisoVencimiento Aviso in Avisos)
            {
                if (Aviso.Dias < 0)
                    Vencidas++;
                else if (Aviso.Dias == 0)
                    VencenHoy++;
                else
                    PorVencer++;
            }
        }
    }
}
