// Dylan Rene Hernandez Recinos 16/09/2026
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Validación de atributos y validación en capa Controlador.
    // Roger Yankhel de Jesús Herrera Alcántara 0901-23-2429.
    public class ClsCtrlRegistro
    {
        private readonly ClsRegistros _Registros = new ClsRegistros();
        private readonly ClsEsquema _Esquema = new ClsEsquema();

        private static readonly string[] _TiposTexto =
        {
            "string", "char", "nchar", "varchar", "nvarchar", "varchar2",
            "nvarchar2", "character", "character varying", "text", "ntext",
            "tinytext", "mediumtext", "longtext", "memo", "clob", "nclob",
            "citext", "json", "jsonb", "xml", "enum", "set"
        };

        private static readonly string[] _TiposEnteros =
        {
            "byte", "sbyte", "int16", "int32", "int64", "uint16", "uint32",
            "uint64", "tinyint", "smallint", "mediumint", "int", "integer",
            "bigint", "int2", "int4", "int8", "serial", "smallserial",
            "bigserial", "counter", "year"
        };

        private static readonly string[] _TiposDecimales =
        {
            "decimal", "numeric", "number", "money", "smallmoney", "currency"
        };

        private static readonly string[] _TiposReales =
        {
            "single", "double", "double precision", "float", "real"
        };

        private static readonly string[] _TiposFecha =
        {
            "date", "dateonly", "datetime", "datetime2", "smalldatetime",
            "timestamp", "timestamptz", "datetimeoffset"
        };

        private static readonly string[] _TiposBinarios =
        {
            "byte[]", "binary", "varbinary", "longvarbinary", "blob", "bytea",
            "image", "rowversion"
        };

        public bool NavegadorFuncExisteLlavePrimaria(
            string NombreTabla,
            string[] CamposPK,
            string[] ValoresPK)
        {
            NavegadorMetValidarNombreTabla(NombreTabla);

            if (CamposPK == null || ValoresPK == null ||
                CamposPK.Length == 0 || CamposPK.Length != ValoresPK.Length)
                throw new ArgumentException("La llave primaria no es válida.");

            return _Registros.NavegadorFuncExisteLlavePrimaria(
                NombreTabla, CamposPK, ValoresPK);
        }

        public bool NavegadorFuncExisteValorCampo(
            string NombreTabla,
            string NombreCampo,
            string Valor)
        {
            NavegadorMetValidarNombreTabla(NombreTabla);

            if (string.IsNullOrWhiteSpace(NombreCampo) || Valor == null)
                throw new ArgumentException("El campo y su valor son obligatorios.");

            return _Registros.NavegadorFuncExisteValorCampo(
                NombreTabla, NombreCampo, Valor);
        }

        public bool NavegadorFuncInsertarRegistro(
            string NombreTabla,
            Dictionary<string, string> Datos)
        {
            NavegadorMetValidarDatos(NombreTabla, Datos, "insertar");
            return _Registros.NavegadorFuncInsertarRegistro(NombreTabla, Datos);
        }

        public bool NavegadorFuncActualizarRegistro(
            string NombreTabla,
            Dictionary<string, string> Valores,
            Dictionary<string, string> ClavesPrimarias)
        {
            NavegadorMetValidarColeccion(ClavesPrimarias, "llave primaria");
            NavegadorMetValidarDatos(
                NombreTabla,
                NavegadorFuncCombinar(Valores, ClavesPrimarias),
                "actualizar");

            return _Registros.NavegadorFuncActualizarRegistro(
                NombreTabla, Valores, ClavesPrimarias);
        }

        public bool NavegadorFuncEliminarRegistro(
            string NombreTabla,
            Dictionary<string, string> ClavesPrimarias)
        {
            NavegadorMetValidarColeccion(ClavesPrimarias, "llave primaria");
            NavegadorMetValidarDatos(NombreTabla, ClavesPrimarias, "eliminar");
            return _Registros.NavegadorFuncEliminarRegistro(NombreTabla, ClavesPrimarias);
        }

        // Valida dinámicamente los atributos existentes en cualquier esquema ODBC.
        public List<string> NavegadorFuncValidarRegistro(
    Dictionary<string, string> Datos,
    string NombreTabla)
{
    NavegadorMetValidarNombreTabla(NombreTabla);
    NavegadorMetValidarColeccion(Datos, "datos");

    List<string> Errores = new List<string>();
    List<ClsColumnaInfo> Columnas =
        _Esquema.NavegadorFuncObtenerEsquemaTabla(NombreTabla);

    if (Columnas == null || Columnas.Count == 0)
        throw new InvalidOperationException("No se encontró el esquema de la tabla.");

    foreach (KeyValuePair<string, string> Dato in Datos)
    {
        ClsColumnaInfo Columna = Columnas.Find(Item =>
            string.Equals(Item.Nombre, Dato.Key, StringComparison.OrdinalIgnoreCase));

        if (Columna == null)
        {
            Errores.Add("El atributo '" + Dato.Key + "' no existe en la tabla.");
            continue;
        }

        string Error = NavegadorFuncValidarAtributo(Dato.Value, Columna);
        if (!string.IsNullOrEmpty(Error)) Errores.Add(Error);

        // Validar Llaves Foráneas directamente desde el controlador
        if (Columna.EsFK && !string.IsNullOrWhiteSpace(Dato.Value) &&
            !string.IsNullOrWhiteSpace(Columna.TablaFK) && !string.IsNullOrWhiteSpace(Columna.ColumnaFK))
        {
            try
            {
                bool ExisteFK = _Registros.NavegadorFuncExisteValorCampo(Columna.TablaFK, Columna.ColumnaFK, Dato.Value);
                if (!ExisteFK)
                {
                    Errores.Add("La llave foránea '" + Columna.Nombre + "' con valor '" + Dato.Value +
                        "' no existe en '" + Columna.TablaFK + "." + Columna.ColumnaFK + "'.");
                }
            }
            catch (Exception)
            {
                // Si falla la consulta a la BD no bloqueamos al controlador
            }
        }
    }

    return Errores;
}

        // Valida nulabilidad, longitud y tipo usando los metadatos reales de la columna.
        public string NavegadorFuncValidarAtributo(string Valor, ClsColumnaInfo Columna)
        {
            if (Columna == null)
                return "No se recibió la información del atributo.";

            if (string.IsNullOrWhiteSpace(Valor))
                return !Columna.Nullable && !Columna.EsAutoincremento
                    ? "El atributo '" + Columna.Nombre + "' es obligatorio."
                    : "";

            if (NavegadorFuncEsBooleano(Columna))
            {
                string Booleano = Valor.Trim().ToLowerInvariant();
                return Regex.IsMatch(Booleano, @"^(0|1|true|false|yes|no|si)$")
                    ? "" : NavegadorFuncError(Columna, "booleano");
            }

            if (NavegadorFuncEsTipo(Columna, _TiposBinarios)) return "";

            if (NavegadorFuncEsTipo(Columna, _TiposEnteros))
                return Regex.IsMatch(Valor, @"^[+-]?\d+$")
                    ? "" : NavegadorFuncError(Columna, "entero");

            if (NavegadorFuncEsTipo(Columna, _TiposDecimales))
            {
                decimal Numero;
                return NavegadorFuncEsDecimal(Valor, out Numero)
                    ? "" : NavegadorFuncError(Columna, "decimal");
            }

            if (NavegadorFuncEsTipo(Columna, _TiposReales))
            {
                double Numero;
                return NavegadorFuncEsReal(Valor, out Numero)
                    ? "" : NavegadorFuncError(Columna, "numérico");
            }

            if (NavegadorFuncEsTipo(Columna, new[] { "guid", "uuid", "uniqueidentifier" }))
            {
                Guid Identificador;
                return Guid.TryParse(Valor, out Identificador)
                    ? "" : NavegadorFuncError(Columna, "identificador único");
            }

            if (NavegadorFuncEsTipo(Columna, _TiposFecha))
            {
                DateTime Fecha;
                return DateTime.TryParse(Valor, out Fecha)
                    ? "" : NavegadorFuncError(Columna, "fecha");
            }

            if (NavegadorFuncEsTipo(Columna, new[] { "time", "timeonly", "timespan", "interval" }))
            {
                TimeSpan Hora;
                return TimeSpan.TryParse(Valor, out Hora)
                    ? "" : NavegadorFuncError(Columna, "hora");
            }

            if (NavegadorFuncEsTipo(Columna, _TiposTexto))
            {
                long Limite = Columna.Longitud > 0
                    ? Columna.Longitud : Columna.TamanoColumna;

                if (Limite > 0 && Valor.Length > Limite)
                    return "El atributo '" + Columna.Nombre +
                        "' admite como máximo " + Limite + " caracteres.";
            }

            // Los tipos propios del motor se delegan al proveedor ODBC.
            return "";
        }

        private void NavegadorMetValidarDatos(
            string NombreTabla,
            Dictionary<string, string> Datos,
            string Accion)
        {
            List<string> Errores = NavegadorFuncValidarRegistro(Datos, NombreTabla);
            if (Errores.Count > 0)
                throw new ArgumentException(
                    "No se puede " + Accion + ":\n" + string.Join("\n", Errores.ToArray()));
        }

        private void NavegadorMetValidarNombreTabla(string NombreTabla)
        {
            if (string.IsNullOrWhiteSpace(NombreTabla))
                throw new ArgumentException("El nombre de la tabla es obligatorio.");
        }

        private void NavegadorMetValidarColeccion(
            Dictionary<string, string> Datos,
            string Nombre)
        {
            if (Datos == null || Datos.Count == 0)
                throw new ArgumentException("La colección de " + Nombre + " está vacía.");
        }

        private Dictionary<string, string> NavegadorFuncCombinar(
            Dictionary<string, string> Valores,
            Dictionary<string, string> Claves)
        {
            NavegadorMetValidarColeccion(Valores, "valores");
            Dictionary<string, string> Resultado =
                new Dictionary<string, string>(Valores, StringComparer.OrdinalIgnoreCase);

            foreach (KeyValuePair<string, string> Clave in Claves)
                Resultado[Clave.Key] = Clave.Value;

            return Resultado;
        }

        private bool NavegadorFuncEsBooleano(ClsColumnaInfo Columna)
        {
            string TipoCompleto = (Columna.TipoColumnaTexto ?? "")
                .ToLowerInvariant().Replace(" ", "");

            return NavegadorFuncEsTipo(Columna, new[] { "bool", "boolean" }) ||
                   TipoCompleto.Contains("tinyint(1)") ||
                   (NavegadorFuncEsTipo(Columna, new[] { "bit" }) &&
                    Columna.TamanoColumna <= 1);
        }

        private bool NavegadorFuncEsTipo(
            ClsColumnaInfo Columna,
            string[] Tipos)
        {
            string[] Fuentes =
            {
                Columna.TipoDato, Columna.TipoNet, Columna.TipoColumnaTexto
            };

            foreach (string FuenteOriginal in Fuentes)
            {
                string Fuente = NavegadorFuncTipoBase(FuenteOriginal);
                foreach (string Tipo in Tipos)
                    if (Fuente == Tipo || Fuente.StartsWith(Tipo + " ")) return true;
            }

            return false;
        }

        private string NavegadorFuncTipoBase(string Tipo)
        {
            if (string.IsNullOrWhiteSpace(Tipo)) return "";
            string Resultado = Tipo.Trim().ToLowerInvariant();
            int Parentesis = Resultado.IndexOf('(');
            if (Parentesis >= 0) Resultado = Resultado.Substring(0, Parentesis);
            return Resultado.Replace(" unsigned", "").Replace(" zerofill", "").Trim();
        }

        private bool NavegadorFuncEsDecimal(string Valor, out decimal Numero)
        {
            return decimal.TryParse(Valor, NumberStyles.Number,
                       CultureInfo.CurrentCulture, out Numero) ||
                   decimal.TryParse(Valor, NumberStyles.Number,
                       CultureInfo.InvariantCulture, out Numero);
        }

        private bool NavegadorFuncEsReal(string Valor, out double Numero)
        {
            bool Valido = double.TryParse(Valor, NumberStyles.Float,
                              CultureInfo.CurrentCulture, out Numero) ||
                          double.TryParse(Valor, NumberStyles.Float,
                              CultureInfo.InvariantCulture, out Numero);

            return Valido && !double.IsNaN(Numero) && !double.IsInfinity(Numero);
        }

        private string NavegadorFuncError(ClsColumnaInfo Columna, string Tipo)
        {
            return "El atributo '" + Columna.Nombre + "' debe ser de tipo " + Tipo + ".";
        }
    }
}
