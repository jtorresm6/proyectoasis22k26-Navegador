/*
 * ============================================================
 * Área   : NuevoNavegador
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 31/10/2026
 *
 * Propósito:
 * Almacena la información del destino de una ayuda del
 * Navegador, incluyendo la ruta del archivo CHM, la página
 * que se debe mostrar y el mensaje correspondiente.
 *
 * ============================================================
 */
namespace CapaControlador_Navegador
{
  
    public class ClsDestinoAyuda
    {
        private string _RutaArchivo = "";
        private string _Pagina = "";
        private string _Mensaje = "";

        public string RutaArchivo
        {
            get { return _RutaArchivo; }
            set { _RutaArchivo = value; }
        }

        public string Pagina
        {
            get { return _Pagina; }
            set { _Pagina = value; }
        }

        public string Mensaje
        {
            get { return _Mensaje; }
            set { _Mensaje = value; }
        }
    }
}
//--------------------------------------------------
// - Final: Evelyn Sofía Andrade Luna
// - Carne: 9959-23-1224
