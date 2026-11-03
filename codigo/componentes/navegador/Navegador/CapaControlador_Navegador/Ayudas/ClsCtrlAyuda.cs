/*
 * ============================================================
 * Área   : NuevoNavegador
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 31/10/2026
 *
 * Propósito:
 * Determina el destino de la ayuda dinámica correspondiente
 * a una aplicación del Navegador. Consulta la información de
 * la aplicación y su módulo, identifica el archivo CHM
 * correspondiente y selecciona la página que debe mostrarse.
 * También contempla alternativas de búsqueda mediante el
 * nombre del formulario y la normalización de los nombres.
 *
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using CapaModelo_Navegador;


namespace CapaControlador_Navegador
{
    
    public class ClsCtrlAyuda
    {
        private ClsAyudas _Ayudas = new ClsAyudas();


        private static readonly string[] _Prefijos = { "mantenimientodel", "mantenimientode", "mantenimiento" };

        public ClsDestinoAyuda NavegadorFuncObtenerDestino(int IdAplicacion, string NombreFormulario)
        {
            ClsDestinoAyuda Destino = new ClsDestinoAyuda();

            if (IdAplicacion <= 0)
            {
                Destino.Mensaje = "El Navegador de este formulario no tiene configurada su aplicación, por eso no se puede buscar su ayuda.";
                return Destino;
            }

            DataTable DatosAplicacion = _Ayudas.NavegadorFuncObtenerDatosAplicacion(IdAplicacion);

            if (DatosAplicacion.Rows.Count == 0)
            {
                Destino.Mensaje = "La aplicación " + IdAplicacion + " no está registrada en Seguridad, por eso no se puede buscar su ayuda.";
                return Destino;
            }

            string NombreAplicacion = DatosAplicacion.Rows[0]["nombreAplicacion"].ToString();
            string NombreModulo = DatosAplicacion.Rows[0]["nombreModulo"].ToString();

            string Archivo = NavegadorFuncElegirArchivo(_Ayudas.NavegadorFuncListarArchivosAyuda(), NombreModulo);

            if (Archivo == "")
            {
                Destino.Mensaje =
                    "Todavía no hay ayuda para el módulo '" + NombreModulo + "'.\n\n" +
                    "Para agregarla hay que poner el archivo [Módulo]Ayudas.chm (por ejemplo SeguridadAyudas.chm) en la carpeta ayuda del proyecto y volver a compilar el Navegador.";
                return Destino;
            }

            try
            {
                Destino.RutaArchivo = _Ayudas.NavegadorFuncObtenerRutaArchivo(Archivo);
            }
            catch (Exception Excepcion)
            {
                Destino.Mensaje =
                    "No se pudo preparar la ayuda del módulo '" + NombreModulo + "'.\n\n" + Excepcion.Message + "\n\n" +
                    "Si tiene abierta una ventana de ayuda, ciérrela y vuelva a intentarlo.";
                return Destino;
            }

            // Si no se puede leer la lista de páginas del .chm se abre su inicio
            List<string> Paginas;

            try
            {
                Paginas = _Ayudas.NavegadorFuncListarPaginas(Destino.RutaArchivo);
            }
            catch (Exception)
            {
                Paginas = new List<string>();
            }

            Destino.Pagina = NavegadorFuncElegirPagina(Paginas, NombreAplicacion, NombreFormulario, NombreModulo, Archivo);

            return Destino;
        }

        
        private string NavegadorFuncElegirArchivo(List<string> Archivos, string NombreModulo)
        {
            string Modulo = NavegadorFuncNormalizar(NombreModulo);
            string Parcial = "";

            foreach (string Archivo in Archivos)
            {
                string Nombre = NavegadorFuncModuloDelArchivo(Archivo);

                if (Nombre == Modulo)
                {
                    return Archivo;
                }

                bool SeParecen = Nombre.Length >= 4 && Modulo.Length >= 4 &&
                    (Modulo.StartsWith(Nombre) || Nombre.StartsWith(Modulo));

                if (Parcial == "" && SeParecen)
                {
                    Parcial = Archivo;
                }
            }

            return Parcial;
        }

       
        private string NavegadorFuncElegirPagina(List<string> Paginas, string NombreAplicacion, string NombreFormulario, string NombreModulo, string Archivo)
        {
            List<string> Nombres = NavegadorFuncNombresPosibles(NombreAplicacion, NombreFormulario);
            List<string> Modulos = new List<string>();
            Modulos.Add(NavegadorFuncNormalizar(NombreModulo));

            string ModuloArchivo = NavegadorFuncModuloDelArchivo(Archivo);

            if (!Modulos.Contains(ModuloArchivo))
            {
                Modulos.Add(ModuloArchivo);
            }

            foreach (string Nombre in Nombres)
            {
                foreach (string Modulo in Modulos)
                {
                    foreach (string Pagina in Paginas)
                    {
                        string NombrePagina = NavegadorFuncNormalizar(Path.GetFileNameWithoutExtension(Pagina));

                        if (NombrePagina == Nombre + Modulo)
                        {
                            return Pagina;
                        }
                    }
                }
            }

            foreach (string Nombre in Nombres)
            {
                foreach (string Pagina in Paginas)
                {
                    string NombrePagina = Path.GetFileNameWithoutExtension(Pagina);
                    int Separador = NombrePagina.LastIndexOf('_');

                    if (Separador > 0 && NavegadorFuncNormalizar(NombrePagina.Substring(0, Separador)) == Nombre)
                    {
                        return Pagina;
                    }
                }
            }

            return "";
        }

        private List<string> NavegadorFuncNombresPosibles(string NombreAplicacion, string NombreFormulario)
        {
            List<string> Originales = new List<string>();
            Originales.Add(NavegadorFuncNormalizar(NombreAplicacion));
            Originales.Add(NavegadorFuncNormalizar(NavegadorFuncQuitarConectores(NombreAplicacion)));

            string Formulario = NavegadorFuncNormalizar(NombreFormulario);
            string SinFrm = Formulario.StartsWith("frm") ? Formulario.Substring(3) : Formulario;

            
            if (SinFrm.EndsWith("navegador") && SinFrm.Length > "navegador".Length)
            {
                Originales.Add(SinFrm.Substring(0, SinFrm.Length - "navegador".Length));
            }

            Originales.Add(SinFrm);
            Originales.Add(Formulario);

            List<string> Nombres = new List<string>();

            foreach (string Original in Originales)
            {
                if (Original != "" && !Nombres.Contains(Original))
                {
                    Nombres.Add(Original);
                }

                foreach (string Prefijo in _Prefijos)
                {
                    if (Original.StartsWith(Prefijo) && Original.Length > Prefijo.Length)
                    {
                        string SinPrefijo = Original.Substring(Prefijo.Length);

                        if (!Nombres.Contains(SinPrefijo))
                        {
                            Nombres.Add(SinPrefijo);
                        }
                    }
                }
            }

            return Nombres;
        }

        private string NavegadorFuncQuitarConectores(string Texto)
        {
            List<string> Palabras = new List<string>();

            foreach (string Palabra in Texto.Split(' '))
            {
                string Minuscula = Palabra.ToLowerInvariant();

                if (Minuscula == "de" || Minuscula == "del" || Minuscula == "la" || Minuscula == "las" ||
                    Minuscula == "los" || Minuscula == "el" || Minuscula == "y")
                {
                    continue;
                }

                Palabras.Add(Palabra);
            }

            return string.Join(" ", Palabras.ToArray());
        }

        private string NavegadorFuncModuloDelArchivo(string RutaArchivo)
        {
            string Nombre = NavegadorFuncNormalizar(Path.GetFileNameWithoutExtension(RutaArchivo));

            return Nombre.Replace("ayudas", "").Replace("ayuda", "");
        }

        // Deja solo letras y números en minúscula y sin tildes: "Módulos_Seguridad" -> "modulosseguridad".
        private string NavegadorFuncNormalizar(string Texto)
        {
            if (string.IsNullOrEmpty(Texto))
            {
                return "";
            }

            StringBuilder Resultado = new StringBuilder();

            foreach (char Letra in Texto.Normalize(NormalizationForm.FormD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(Letra) == UnicodeCategory.NonSpacingMark)
                {
                    continue;
                }

                if (char.IsLetterOrDigit(Letra))
                {
                    Resultado.Append(char.ToLowerInvariant(Letra));
                }
            }

            return Resultado.ToString();
        }
    }
}
//--------------------------------------------------
// Final: Evelyn Sofía Andrade Luna
// Carne: 9959-23-1224
