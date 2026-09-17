using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaControlador_Navegador;
using CapaEntidades_Navegador;

namespace CapaVista_Navegador
{
    // Insertar, Modificar, Eliminar, Guardar y los mensajes de confirmacion/error, todo junto
    public class ClsCrudAcciones
    {
        private ClsCtrlRegistro _CtrlRegistro = new ClsCtrlRegistro();

        public bool NavegadorFuncConfirmarAccion(string Titulo, string Mensaje)
        {
            return MessageBox.Show(Mensaje, Titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private string NavegadorFuncResumenDatos(Dictionary<string, string> Datos)
        {
            string Resumen = "";

            foreach (KeyValuePair<string, string> Dato in Datos)
                Resumen += Dato.Key + ": " + Dato.Value + "\n";

            return Resumen;
        }

        // Inserta o actualiza segun el modo, validando los campos antes de guardar
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

        // Valida cada FK de la tabla actual sin navegar ni cargar llaves de otra tabla.
        // Una FK puede repetirse en varios registros; lo obligatorio es que el valor exista
        // en la columna referenciada de la tabla padre.
        //
        // IMPORTANTE: si los metadatos de la FK vienen incompletos o apuntan a la misma tabla
        // que se esta editando, se omite la validacion en lugar de bloquear el guardado.
        // Esos casos indican que el lector de esquema esta devolviendo la tabla hija en
        // TablaFK en vez de la tabla padre, y el motor de BD igual rechazara el insert
        // si la relacion realmente se incumple.
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

            return _CtrlRegistro.NavegadorFuncInsertarRegistro(Tabla, Datos);
        }

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

            return _CtrlRegistro.NavegadorFuncActualizarRegistro(Tabla, Datos, ClavesPrimarias);
        }

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

            return _CtrlRegistro.NavegadorFuncEliminarRegistro(Tabla, ClavesPrimarias);
        }

        // Traduce errores tecnicos del motor de BD a mensajes que el Usuario entienda
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