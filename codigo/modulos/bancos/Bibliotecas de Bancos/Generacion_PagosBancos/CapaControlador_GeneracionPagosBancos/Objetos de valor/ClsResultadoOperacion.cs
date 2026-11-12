using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 03/11/2026
 * ================================================================
 * Propósito :
 *  Objeto de valor con el que el controlador le responde a la vista
 *  el resultado de un proceso: si salió bien, el mensaje a mostrar
 *  y un valor de apoyo (id o número generado).
 * ================================================================
 */

namespace CapaControlador_GeneracionPagos
{
    public class ClsResultadoOperacion
    {
        public bool Exito { get; private set; }
        public string Mensaje { get; private set; }
        public string Valor { get; private set; }

        public ClsResultadoOperacion(bool Exito, string Mensaje)
        {
            this.Exito = Exito;
            this.Mensaje = Mensaje;
        }

        public ClsResultadoOperacion(bool Exito, string Mensaje, string Valor)
        {
            this.Exito = Exito;
            this.Mensaje = Mensaje;
            this.Valor = Valor;
        }
    }
}
