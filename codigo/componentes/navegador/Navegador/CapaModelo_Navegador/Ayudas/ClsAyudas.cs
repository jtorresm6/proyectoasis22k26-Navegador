/*
 * ============================================================
 * Área   : NuevoNavegador
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 31/10/2026
 *
 * Propósito:
 * Gestiona el acceso a los datos de las ayudas dinámicas del
 * Navegador. Obtiene información de las aplicaciones y módulos,
 * identifica los archivos de ayuda incluidos en el sistema,
 * administra sus copias locales y permite consultar las páginas
 * contenidas en los archivos CHM.
 *
 * ============================================================
 */


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace CapaModelo_Navegador
{
    
    public class ClsAyudas : CapaModelo_Seguridad.ClsConexion
    {
        
        private const string _PrefijoRecurso = "Ayudas/";

        private const string _CarpetaProducto = "Embutidos de Calidad";
        private const string _CarpetaAyudas = "Ayudas";

       
        private static readonly List<string> _CopiasVerificadas = new List<string>();

        public DataTable NavegadorFuncObtenerDatosAplicacion(int IdAplicacion)
        {
            string ConsultaSQL =
                "SELECT a.nombreAplicacion, m.nombreModulo " +
                "FROM tblaplicacion a " +
                "INNER JOIN tblmodulo m ON m.idModulo = a.idModulo " +
                "WHERE a.idAplicacion = ?";

            OdbcConnection Conexion = SeguridadMetObtenerConexion();
            DataTable TablaDatos = new DataTable();

            try
            {
                using (OdbcDataAdapter AdaptadorDatos = new OdbcDataAdapter(ConsultaSQL, Conexion))
                {
                    AdaptadorDatos.SelectCommand.Parameters.AddWithValue("@IdAplicacion", IdAplicacion);
                    AdaptadorDatos.Fill(TablaDatos);
                }
            }
            finally
            {
                SeguridadMetDesconexion(Conexion);
            }

            return TablaDatos;
        }

        // Devuelve el nombre de los .chm que trae el sistema, por ejemplo "SeguridadAyudas.chm".
        public List<string> NavegadorFuncListarArchivosAyuda()
        {
            List<string> Archivos = new List<string>();

            foreach (string Recurso in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (Recurso.StartsWith(_PrefijoRecurso, StringComparison.Ordinal) &&
                    Recurso.EndsWith(".chm", StringComparison.OrdinalIgnoreCase))
                {
                    Archivos.Add(Recurso.Substring(_PrefijoRecurso.Length));
                }
            }

            Archivos.Sort();

            return Archivos;
        }

        // Devuelve la ruta en el disco del .chm que se pide (el nombre tal como lo entrega
        // NavegadorFuncListarArchivosAyuda). Solo se copia ese archivo, y solo cuando hace falta: si en la
        // carpeta del usuario ya hay una copia igual al recurso se usa tal cual; si falta o es de una
        // versión anterior del sistema se vuelve a copiar.
        public string NavegadorFuncObtenerRutaArchivo(string Archivo)
        {
            string Carpeta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                _CarpetaProducto,
                _CarpetaAyudas);
            string Ruta = Path.Combine(Carpeta, Archivo);

            using (Stream Recurso = Assembly.GetExecutingAssembly().GetManifestResourceStream(_PrefijoRecurso + Archivo))
            {
                if (Recurso == null)
                {
                    throw new FileNotFoundException("El archivo de ayuda '" + Archivo + "' no está incluido en el sistema.");
                }

                if (!NavegadorFuncCopiaVigente(Ruta, Recurso))
                {
                    Directory.CreateDirectory(Carpeta);
                    NavegadorMetCopiarRecurso(Recurso, Ruta);
                }

                if (!_CopiasVerificadas.Contains(Ruta))
                {
                    _CopiasVerificadas.Add(Ruta);
                }
            }

            return Ruta;
        }

       
        private bool NavegadorFuncCopiaVigente(string Ruta, Stream Recurso)
        {
            if (!File.Exists(Ruta) || new FileInfo(Ruta).Length != Recurso.Length)
            {
                return false;
            }

            if (_CopiasVerificadas.Contains(Ruta))
            {
                return true;
            }

            using (FileStream Copia = new FileStream(Ruta, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (SHA256 Algoritmo = SHA256.Create())
            {
                string HuellaCopia = BitConverter.ToString(Algoritmo.ComputeHash(Copia));

                Recurso.Position = 0;
                string HuellaRecurso = BitConverter.ToString(Algoritmo.ComputeHash(Recurso));
                Recurso.Position = 0;

                return HuellaCopia == HuellaRecurso;
            }
        }

        // Se copia a un archivo temporal y solo al terminar se le pone el nombre definitivo
        private void NavegadorMetCopiarRecurso(Stream Recurso, string Ruta)
        {
            string Temporal = Ruta + "." + Guid.NewGuid().ToString("N") + ".tmp";

            try
            {
                using (FileStream Destino = new FileStream(Temporal, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    Recurso.CopyTo(Destino);
                }

                if (new FileInfo(Temporal).Length != Recurso.Length)
                {
                    throw new IOException("La copia de la ayuda '" + Path.GetFileName(Ruta) + "' quedó incompleta.");
                }

                if (File.Exists(Ruta))
                {
                    File.Delete(Ruta);
                }

                File.Move(Temporal, Ruta);
            }
            finally
            {
                if (File.Exists(Temporal))
                {
                    File.Delete(Temporal);
                }
            }
        }

        // Devuelve los nombres de las páginas (.htm y .html) que trae un .chm, tal como están guardadas
        // adentro (por ejemplo "Empleados_Seguridad.html" 
        public List<string> NavegadorFuncListarPaginas(string RutaArchivo)
        {
            List<string> Paginas = new List<string>();

            using (FileStream Archivo = new FileStream(RutaArchivo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BinaryReader Lector = new BinaryReader(Archivo))
            {
                
                if (Encoding.ASCII.GetString(Lector.ReadBytes(4)) != "ITSF")
                {
                    throw new InvalidDataException("El archivo '" + RutaArchivo + "' no es un archivo de ayuda .chm válido.");
                }

                Archivo.Position = 0x48;
                long InicioDirectorio = Lector.ReadInt64();
                long LargoDirectorio = Lector.ReadInt64();

                Archivo.Position = InicioDirectorio;
                byte[] Directorio = Lector.ReadBytes((int)LargoDirectorio);


                if (Encoding.ASCII.GetString(Directorio, 0, 4) != "ITSP")
                {
                    throw new InvalidDataException("No se pudo leer el directorio del archivo de ayuda '" + RutaArchivo + "'.");
                }

                int LargoEncabezado = BitConverter.ToInt32(Directorio, 0x08);
                int TamanoBloque = BitConverter.ToInt32(Directorio, 0x10);
                int CantidadBloques = BitConverter.ToInt32(Directorio, 0x2C);

                for (int Bloque = 0; Bloque < CantidadBloques; Bloque++)
                {
                    int InicioBloque = LargoEncabezado + (Bloque * TamanoBloque);

                    if (InicioBloque + TamanoBloque > Directorio.Length)
                    {
                        break;
                    }

                    // Solo los bloques "PMGL" traen nombres de archivos; los demás son índices.
                    if (Encoding.ASCII.GetString(Directorio, InicioBloque, 4) != "PMGL")
                    {
                        continue;
                    }

                    int EspacioLibre = BitConverter.ToInt32(Directorio, InicioBloque + 4);
                    int Posicion = InicioBloque + 20;
                    int FinBloque = InicioBloque + TamanoBloque - EspacioLibre;

                    while (Posicion < FinBloque)
                    {

                        int LargoNombre = (int)NavegadorFuncLeerEntero(Directorio, ref Posicion);
                        string Nombre = Encoding.UTF8.GetString(Directorio, Posicion, LargoNombre);
                        Posicion += LargoNombre;

                        NavegadorFuncLeerEntero(Directorio, ref Posicion);
                        NavegadorFuncLeerEntero(Directorio, ref Posicion);
                        NavegadorFuncLeerEntero(Directorio, ref Posicion);

                        string Minuscula = Nombre.ToLowerInvariant();

                        if (Nombre.StartsWith("/") && (Minuscula.EndsWith(".htm") || Minuscula.EndsWith(".html")))
                        {
                            Paginas.Add(Nombre.Substring(1));
                        }
                    }
                }
            }

            return Paginas;
        }


        private long NavegadorFuncLeerEntero(byte[] Datos, ref int Posicion)
        {
            long Valor = 0;
            byte Actual;

            do
            {
                Actual = Datos[Posicion];
                Posicion++;
                Valor = (Valor << 7) + (Actual & 0x7F);
            }
            while ((Actual & 0x80) != 0);

            return Valor;
        }
    }
}
//--------------------------------------------------
// Final: Evelyn Sofía Andrade Luna
// Carne: 9959-23-1224
