using System;
using System.IO;
using System.Text;

namespace CapaVista_Componentes.Componentes
{
    internal static class ClsContenidoAyudaCompras
    {
        internal static bool ContienePagina(string Ruta, string Pagina)
        {
            using (var Archivo = File.Open(Ruta, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var Lector = new BinaryReader(Archivo, Encoding.UTF8))
            {
                if (Archivo.Length < 0x58 || Encoding.ASCII.GetString(Lector.ReadBytes(4)) != "ITSF")
                    throw new InvalidDataException("El archivo de ayuda no es un CHM valido.");
                int Version = Lector.ReadInt32();
                if (Version != 2 && Version != 3)
                    throw new InvalidDataException("La version del archivo de ayuda no es compatible.");
                Archivo.Position = 0x48;
                long Directorio = Lector.ReadInt64();
                long Longitud = Lector.ReadInt64();
                if (Directorio < 0 || Longitud < 0x54 || Directorio > Archivo.Length - Longitud)
                    throw new InvalidDataException("El directorio del CHM no es valido.");
                Archivo.Position = Directorio;
                if (Encoding.ASCII.GetString(Lector.ReadBytes(4)) != "ITSP")
                    throw new InvalidDataException("No se encontro el directorio del CHM.");
                Archivo.Position = Directorio + 8;
                int Cabecera = Lector.ReadInt32();
                Lector.ReadInt32();
                int TamanoBloque = Lector.ReadInt32();
                if (Cabecera < 20 || Cabecera > Longitud || TamanoBloque < 20 || TamanoBloque > 1048576)
                    throw new InvalidDataException("La estructura del directorio CHM no es valida.");
                string Buscada = "/" + Uri.UnescapeDataString(Pagina).TrimStart('/');
                for (long Posicion = Directorio + Cabecera;
                    Posicion <= Directorio + Longitud - TamanoBloque; Posicion += TamanoBloque)
                {
                    Archivo.Position = Posicion;
                    byte[] Bloque = Lector.ReadBytes(TamanoBloque);
                    if (Encoding.ASCII.GetString(Bloque, 0, 4) != "PMGL") continue;
                    uint EspacioLibre = BitConverter.ToUInt32(Bloque, 4);
                    if (EspacioLibre > TamanoBloque - 20)
                        throw new InvalidDataException("El bloque del directorio CHM no es valido.");
                    int Limite = TamanoBloque - (int)EspacioLibre;
                    int Indice = 20;
                    while (Indice < Limite)
                    {
                        ulong TamanoNombre = LeerEntero(Bloque, ref Indice, Limite);
                        if (TamanoNombre > (ulong)(Limite - Indice))
                            throw new InvalidDataException("El nombre de una pagina CHM no es valido.");
                        string Nombre = Encoding.UTF8.GetString(Bloque, Indice, (int)TamanoNombre);
                        Indice += (int)TamanoNombre;
                        LeerEntero(Bloque, ref Indice, Limite);
                        LeerEntero(Bloque, ref Indice, Limite);
                        LeerEntero(Bloque, ref Indice, Limite);
                        if (string.Equals(Nombre, Buscada, StringComparison.OrdinalIgnoreCase)) return true;
                    }
                }
                return false;
            }
        }

        private static ulong LeerEntero(byte[] Datos, ref int Indice, int Limite)
        {
            ulong Valor = 0;
            for (int Cantidad = 0; Cantidad < 10 && Indice < Limite; Cantidad++)
            {
                byte Actual = Datos[Indice++];
                if (Valor > (ulong.MaxValue >> 7)) break;
                Valor = (Valor << 7) | (uint)(Actual & 0x7F);
                if ((Actual & 0x80) == 0) return Valor;
            }
            throw new InvalidDataException("Una entrada del directorio CHM no es valida.");
        }
    }
}
