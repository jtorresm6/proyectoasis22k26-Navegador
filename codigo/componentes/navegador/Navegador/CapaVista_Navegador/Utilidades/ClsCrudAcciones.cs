// ============================================================================
// Desarrollador: Oskar Saul Cermeño Jimenez
// Carnet:        0901-23-15379
// Fecha:         16/09/2026
// Módulo:        CapaVista_Navegador
// Descripción:   Gestor de acciones CRUD (Insertar, Modificar, Eliminar) con
//                validaciones previas de integridad referencial, duplicidad y
//                traducción de excepciones de base de datos a mensajes legibles.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Windows.Forms;
using CapaControlador_Navegador;
using CapaModelo_Navegador;

namespace CapaVista_Navegador
{
    public class ClsCrudAcciones
    {
        private ClsCtrlRegistro _CtrlRegistro = new ClsCtrlRegistro();
        private ClsNavegadorBitacora _Bitacora = new ClsNavegadorBitacora();
        private ClsConexionBD _ConexionBD = new ClsConexionBD();

        // ====================================================================
        // Función:      NavegadorFuncConfirmarAccion
        // Descripción:  Muestra un cuadro de diálogo modal con opciones Sí/No 
        //               para requerir confirmación explícita del usuario antes
        //               de ejecutar una acción crítica (guardar, editar, eliminar).
        // Parámetros:   - Titulo: Encabezado de la ventana de diálogo.
        //               - Mensaje: Contenido descriptivo de la confirmación.
        // Retorna:      True si el usuario hace clic en 'Sí', False en caso contrario.
        // ====================================================================
        public bool NavegadorFuncConfirmarAccion(string Titulo, string Mensaje)
        {
            return MessageBox.Show(Mensaje, Titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        // ====================================================================
        // Función:      NavegadorFuncResumenDatos
        // Descripción:  Recorre un diccionario de pares clave-valor y construye
        //               una cadena formateada con saltos de línea para listar
        //               los campos y sus datos en los diálogos de confirmación.
        // Parámetros:   - Datos: Diccionario con los nombres de campos y valores.
        // Retorna:      Cadena de texto formateada ("Campo: Valor\n...").
        // ====================================================================
        private string NavegadorFuncResumenDatos(Dictionary<string, string> Datos)
        {
            string Resumen = "";

            foreach (KeyValuePair<string, string> Dato in Datos)
                Resumen += Dato.Key + ": " + Dato.Value + "\n";

            return Resumen;
        }

        // ====================================================================
        // Función:      NavegadorFuncGuardar
        // Descripción:  Punto de entrada principal para persistir un registro.
        //               Filtra campos según el esquema (omite autoincrementables
        //               al insertar o llaves primarias al modificar), valida 
        //               nulabilidad obligatoria, reglas de negocio del controlador
        //               y existencia de llaves foráneas antes de delegar la 
        //               inserción o actualización.
        // Parámetros:   - Tabla: Nombre de la tabla destino.
        //               - Esquema: Definición de columnas, tipos y restricciones.
        //               - DatosFormulario: Claves y valores capturados en la vista.
        //               - ModoModificar: True si es actualización, False si es inserción.
        //               - PkOriginal: Diccionario con la clave primaria original.
        //               - Mensaje: Parámetro de salida con advertencias o errores.
        // Retorna:      True si la operación fue exitosa; False si falló una 
        //               validación o el guardado.
        // ====================================================================
        public bool NavegadorFuncGuardar(string Tabla, List<ClsColumnaInfo> Esquema, Dictionary<string, string> DatosFormulario,
            bool ModoModificar, Dictionary<string, string> PkOriginal, out string Mensaje)
        {
            Mensaje = "";

            Dictionary<string, string> Datos = new Dictionary<string, string>();

            foreach (ClsColumnaInfo Columna in Esquema)
            {
                if (!DatosFormulario.ContainsKey(Columna.Nombre))
                    continue;

                if (!ModoModificar && Columna.EsAutoincremento)
                    continue;

                if (ModoModificar && Columna.EsPK)
                    continue;

                string Valor = DatosFormulario[Columna.Nombre];

                if (string.IsNullOrWhiteSpace(Valor))
                {
                    if (!Columna.Nullable)
                    {
                        Mensaje = "El campo '" + Columna.Nombre + "' es obligatorio.";
                        return false;
                    }

                    continue;
                }

                Datos[Columna.Nombre] = Valor;
            }

            List<string> Errores = _CtrlRegistro.NavegadorFuncValidarRegistro(Datos, Tabla);

            Errores.AddRange(NavegadorFuncValidarLlavesForaneas(Tabla, Esquema, Datos));

            if (Errores.Count > 0)
            {
                Mensaje = string.Join("\n", Errores);
                return false;
            }

            if (!ModoModificar)
                return NavegadorFuncInsertar(Tabla, Esquema, Datos, out Mensaje);

            return NavegadorFuncModificar(Tabla, Datos, PkOriginal, out Mensaje);
        }

        // ====================================================================
        // Función:      NavegadorFuncValidarLlavesForaneas
        // Descripción:  Verifica la integridad referencial de los campos FK 
        //               comprobando que el valor ingresado exista previamente en
        //               la tabla padre configurada. Omite validación si los 
        //               metadatos están incompletos, si apuntan a la misma tabla 
        //               o si la consulta remota falla (dejando el control final al motor BD).
        // Parámetros:   - Tabla: Nombre de la tabla que contiene las FK.
        //               - Esquema: Lista de metadatos de las columnas.
        //               - Datos: Valores actuales capturados para la inserción/edición.
        // Retorna:      Lista de cadenas con los errores de FK no encontradas.
        // ====================================================================
        private List<string> NavegadorFuncValidarLlavesForaneas(string Tabla, List<ClsColumnaInfo> Esquema, Dictionary<string, string> Datos)
        {
            List<string> Errores = new List<string>();

            foreach (ClsColumnaInfo Columna in Esquema)
            {
                if (!Columna.EsFK)
                    continue;

                // Metadato incompleto: no hay a donde validar
                if (string.IsNullOrWhiteSpace(Columna.TablaFK) || string.IsNullOrWhiteSpace(Columna.ColumnaFK))
                    continue;

                // Metadato sospechoso: la FK apunta a la propia tabla que se esta editando
                if (string.Equals(Columna.TablaFK, Tabla, StringComparison.OrdinalIgnoreCase))
                    continue;

                string ValorFK;
                if (!Datos.TryGetValue(Columna.Nombre, out ValorFK) || string.IsNullOrWhiteSpace(ValorFK))
                    continue;

                bool Existe;

                try
                {
                    Existe = _CtrlRegistro.NavegadorFuncExisteValorCampo(Columna.TablaFK, Columna.ColumnaFK, ValorFK);
                }
                catch (Exception)
                {
                    // Si no se pudo consultar la tabla padre no se bloquea al Usuario;
                    // la restriccion real la aplica la base de datos al insertar.
                    continue;
                }

                if (!Existe)
                {
                    Errores.Add("La llave foránea '" + Columna.Nombre + "' con valor '" + ValorFK +
                        "' no existe en '" + Columna.TablaFK + "." + Columna.ColumnaFK + "'.");
                }
            }

            return Errores;
        }

        // ====================================================================
        // Inicio cambio - Matthew Juárez - 0901-23-4250
        // Transacción de Base de Datos para Insertar y Bitácora (Commit / Rollback)
        // ====================================================================
        private bool NavegadorFuncInsertar(string Tabla, List<ClsColumnaInfo> Esquema, Dictionary<string, string> Datos, out string Mensaje)
        {
            Mensaje = "";

            List<string> CamposPK = new List<string>();
            List<string> ValoresPK = new List<string>();

            foreach (ClsColumnaInfo Columna in Esquema)
            {
                if (!Columna.EsPK) continue;

                string Valor;

                if (Datos.TryGetValue(Columna.Nombre, out Valor))
                {
                    CamposPK.Add(Columna.Nombre);
                    ValoresPK.Add(Valor);
                }
            }

            if (CamposPK.Count > 0)
            {
                bool Duplicada = false;

                try
                {
                    Duplicada = _CtrlRegistro.NavegadorFuncExisteLlavePrimaria(
                        Tabla, CamposPK.ToArray(), ValoresPK.ToArray());
                }
                catch (Exception Excepcion)
                {
                    Mensaje = "No se pudo verificar la llave primaria: " +
                        NavegadorFuncMensajeAmigable(Excepcion);
                    return false;
                }

                if (Duplicada)
                {
                    Mensaje = "Ya existe un registro con esta llave primaria (" +
                        string.Join(", ", CamposPK.ToArray()) + " = " +
                        string.Join(", ", ValoresPK.ToArray()) + ").";
                    return false;
                }
            }

            if (!NavegadorFuncConfirmarAccion("Confirmar ingreso",
                "¿Desea ingresar el siguiente registro en la tabla '" + Tabla + "'?\n\n" +
                NavegadorFuncResumenDatos(Datos)))
                return false;

            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();
            OdbcTransaction Transaccion = null;

            try
            {
                Transaccion = Conexion.BeginTransaction();

                bool Insertado = _CtrlRegistro.NavegadorFuncInsertarRegistro(Tabla, Datos, Conexion, Transaccion);

                if (Insertado)
                {
                    int IdRegistro = 0;
                    if (ValoresPK.Count > 0)
                        int.TryParse(ValoresPK[0], out IdRegistro);

                    _Bitacora.NavegadorMetRegistrarBitacora(
                        "INSERT", Tabla, IdRegistro,
                        "Se insertó un registro en " + Tabla + ": " + NavegadorFuncResumenDatos(Datos),
                        Conexion, Transaccion);

                    Transaccion.Commit();
                    return true;
                }
                else
                {
                    Transaccion.Rollback();
                    Mensaje = "No se pudo insertar el registro.";
                    return false;
                }
            }
            catch (Exception Excepcion)
            {
                if (Transaccion != null)
                {
                    try { Transaccion.Rollback(); } catch { }
                }
                Mensaje = "Error al ejecutar la transacción: " + NavegadorFuncMensajeAmigable(Excepcion);
                return false;
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }
        }

        // ====================================================================
        // Inicio cambio - Matthew Juárez - 0901-23-4250
        // Transacción de Base de Datos para Modificar y Bitácora (Commit / Rollback)
        // ====================================================================
        private bool NavegadorFuncModificar(string Tabla, Dictionary<string, string> Datos,
            Dictionary<string, string> ClavesPrimarias, out string Mensaje)
        {
            Mensaje = "";

            if (ClavesPrimarias == null || ClavesPrimarias.Count == 0)
            {
                Mensaje = "No se encontró la llave primaria del registro seleccionado.";
                return false;
            }

            if (Datos.Count == 0)
            {
                Mensaje = "No hay campos disponibles para Modificar.";
                return false;
            }

            if (!NavegadorFuncConfirmarAccion("Confirmar modificación",
                "¿Desea guardar los cambios en la tabla '" + Tabla + "'?\n\n" +
                NavegadorFuncResumenDatos(Datos)))
                return false;

            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();
            OdbcTransaction Transaccion = null;

            try
            {
                Transaccion = Conexion.BeginTransaction();

                bool Actualizado = _CtrlRegistro.NavegadorFuncActualizarRegistro(Tabla, Datos, ClavesPrimarias, Conexion, Transaccion);

                if (Actualizado)
                {
                    int IdRegistro = 0;
                    foreach (string ValorPk in ClavesPrimarias.Values) { int.TryParse(ValorPk, out IdRegistro); break; }

                    _Bitacora.NavegadorMetRegistrarBitacora(
                        "UPDATE", Tabla, IdRegistro,
                        "Se actualizó un registro en " + Tabla + ": " + NavegadorFuncResumenDatos(Datos),
                        Conexion, Transaccion);

                    Transaccion.Commit();
                    return true;
                }
                else
                {
                    Transaccion.Rollback();
                    Mensaje = "No se pudo actualizar el registro.";
                    return false;
                }
            }
            catch (Exception Excepcion)
            {
                if (Transaccion != null)
                {
                    try { Transaccion.Rollback(); } catch { }
                }
                Mensaje = "Error al ejecutar la transacción: " + NavegadorFuncMensajeAmigable(Excepcion);
                return false;
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }
        }

        // ====================================================================
        // Inicio cambio - Matthew Juárez - 0901-23-4250
        // Transacción de Base de Datos para Eliminar y Bitácora (Commit / Rollback)
        // ====================================================================
        public bool NavegadorFuncEliminar(string Tabla, Dictionary<string, string> ClavesPrimarias, out string Mensaje)
        {
            Mensaje = "";

            if (ClavesPrimarias == null || ClavesPrimarias.Count == 0)
            {
                Mensaje = "La tabla no tiene una llave primaria detectable.";
                return false;
            }

            foreach (KeyValuePair<string, string> Clave in ClavesPrimarias)
            {
                if (string.IsNullOrWhiteSpace(Clave.Value))
                {
                    Mensaje = "No se pudo obtener el valor de la llave primaria del registro seleccionado.";
                    return false;
                }
            }

            if (!NavegadorFuncConfirmarAccion("Confirmar eliminación",
                "¿Desea eliminar el registro seleccionado de la tabla '" + Tabla + "'?"))
                return false;

            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();
            OdbcTransaction Transaccion = null;

            try
            {
                Transaccion = Conexion.BeginTransaction();

                bool Eliminado = _CtrlRegistro.NavegadorFuncEliminarRegistro(Tabla, ClavesPrimarias, Conexion, Transaccion);

                if (Eliminado)
                {
                    int IdRegistro = 0;
                    foreach (string ValorPk in ClavesPrimarias.Values) { int.TryParse(ValorPk, out IdRegistro); break; }

                    _Bitacora.NavegadorMetRegistrarBitacora(
                        "DELETE", Tabla, IdRegistro,
                        "Se eliminó un registro de " + Tabla + ".",
                        Conexion, Transaccion);

                    Transaccion.Commit();
                    return true;
                }
                else
                {
                    Transaccion.Rollback();
                    Mensaje = "No se pudo eliminar el registro.";
                    return false;
                }
            }
            catch (Exception Excepcion)
            {
                if (Transaccion != null)
                {
                    try { Transaccion.Rollback(); } catch { }
                }
                Mensaje = "Error al ejecutar la transacción: " + NavegadorFuncMensajeAmigable(Excepcion);
                return false;
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }
        }

        public string NavegadorFuncMensajeAmigable(Exception Excepcion)
        {
            string TextoMinusculas = (Excepcion.Message ?? "").ToLowerInvariant();

            if (TextoMinusculas.Contains("doesn't exist") || TextoMinusculas.Contains("does not exist") ||
                TextoMinusculas.Contains("unknown table") || TextoMinusculas.Contains("no existe") ||
                TextoMinusculas.Contains("invalid object name"))
                return "La tabla indicada no existe o el nombre está escrito incorrectamente. Verifique el nombre configurado para el CRUD.";

            if (TextoMinusculas.Contains("foreign key") || TextoMinusculas.Contains("fk_") || TextoMinusculas.Contains("reference constraint"))
                return "El registro no puede guardarse o eliminarse porque existe una relación de llave foránea.";

            if (TextoMinusculas.Contains("duplicate entry") || TextoMinusculas.Contains("duplicate key") ||
                TextoMinusculas.Contains("unique constraint") || TextoMinusculas.Contains("violation of unique") ||
                TextoMinusculas.Contains("violation of primary key"))
                return "Ya existe un registro con el mismo valor en un campo único.";

            if (TextoMinusculas.Contains("cannot be null") || TextoMinusculas.Contains("null value") ||
                TextoMinusculas.Contains("not-null constraint") || TextoMinusculas.Contains("insert the value null"))
                return "Hay un campo obligatorio que no puede quedar vacío.";

            if (TextoMinusculas.Contains("data too long") || TextoMinusculas.Contains("truncat") ||
                TextoMinusculas.Contains("string or binary data would be truncated"))
                return "Uno de los valores ingresados es demasiado largo para el campo correspondiente.";

            if ((TextoMinusculas.Contains("incorrect") && TextoMinusculas.Contains("value")) ||
                TextoMinusculas.Contains("conversion failed") || TextoMinusculas.Contains("invalid input syntax"))
                return "Uno de los valores ingresados tiene un formato incorrecto para su campo.";

            return Excepcion.Message;
        }
    }
}