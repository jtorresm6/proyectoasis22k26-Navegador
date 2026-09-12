using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaControlador_Navegador;
using CapaEntidades_Navegador;

namespace CapaVista_Navegador
{
    // Insertar, Modificar, Eliminar, Guardar y los mensajes de confirmacion/error, todo junto
    public class crudAcciones
    {
        private ctrlRegistro ctrlRegistro = new ctrlRegistro();

        public bool ConfirmarAccion(string titulo, string mensaje)
        {
            return MessageBox.Show(mensaje, titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private string ResumenDatos(Dictionary<string, string> datos)
        {
            string resumen = "";

            foreach (KeyValuePair<string, string> dato in datos)
                resumen += dato.Key + ": " + dato.Value + "\n";

            return resumen;
        }

        // Inserta o actualiza segun el modo, validando los campos antes de guardar
        public bool Guardar(string tabla, List<columnaInfo> esquema, Dictionary<string, string> datosFormulario,
            bool modoModificar, Dictionary<string, string> pkOriginal, out string mensaje)
        {
            mensaje = "";

            Dictionary<string, string> datos = new Dictionary<string, string>();

            foreach (columnaInfo col in esquema)
            {
                if (!datosFormulario.ContainsKey(col.Nombre))
                    continue;

                if (!modoModificar && col.EsAutoincremento)
                    continue;

                if (modoModificar && col.EsPK)
                    continue;

                string valor = datosFormulario[col.Nombre];

                if (string.IsNullOrWhiteSpace(valor))
                {
                    if (!col.Nullable)
                    {
                        mensaje = "El campo '" + col.Nombre + "' es obligatorio.";
                        return false;
                    }

                    continue;
                }

                datos[col.Nombre] = valor;
            }

            List<string> errores = ctrlRegistro.ValidarRegistro(datos, tabla);

            if (errores.Count > 0)
            {
                mensaje = string.Join("\n", errores);
                return false;
            }

            if (!modoModificar)
                return Insertar(tabla, esquema, datos, out mensaje);

            return Modificar(tabla, datos, pkOriginal, out mensaje);
        }

        private bool Insertar(string tabla, List<columnaInfo> esquema, Dictionary<string, string> datos, out string mensaje)
        {
            mensaje = "";

            List<string> pkCampos = new List<string>();
            List<string> pkValores = new List<string>();

            foreach (columnaInfo col in esquema)
            {
                if (!col.EsPK) continue;

                string valor;

                if (datos.TryGetValue(col.Nombre, out valor))
                {
                    pkCampos.Add(col.Nombre);
                    pkValores.Add(valor);
                }
            }

            if (pkCampos.Count > 0 && ctrlRegistro.ExisteLlavePrimaria(tabla, pkCampos.ToArray(), pkValores.ToArray()))
            {
                mensaje = "Ya existe un registro con esta llave primaria (" +
                    string.Join(", ", pkCampos) + " = " + string.Join(", ", pkValores) + ").";
                return false;
            }

            if (!ConfirmarAccion("Confirmar ingreso",
                "¿Desea ingresar el siguiente registro en la tabla '" + tabla + "'?\n\n" + ResumenDatos(datos)))
                return false;

            return ctrlRegistro.InsertarRegistro(tabla, datos);
        }

        private bool Modificar(string tabla, Dictionary<string, string> datos, Dictionary<string, string> pk, out string mensaje)
        {
            mensaje = "";

            if (pk == null || pk.Count == 0)
            {
                mensaje = "No se encontró la llave primaria del registro seleccionado.";
                return false;
            }

            if (datos.Count == 0)
            {
                mensaje = "No hay campos disponibles para modificar.";
                return false;
            }

            if (!ConfirmarAccion("Confirmar modificación",
                "¿Desea guardar los cambios en la tabla '" + tabla + "'?\n\n" + ResumenDatos(datos)))
                return false;

            return ctrlRegistro.ActualizarRegistro(tabla, datos, pk);
        }

        public bool Eliminar(string tabla, Dictionary<string, string> pk, out string mensaje)
        {
            mensaje = "";

            if (pk == null || pk.Count == 0)
            {
                mensaje = "La tabla no tiene una llave primaria detectable.";
                return false;
            }

            foreach (KeyValuePair<string, string> clave in pk)
            {
                if (string.IsNullOrWhiteSpace(clave.Value))
                {
                    mensaje = "No se pudo obtener el valor de la llave primaria del registro seleccionado.";
                    return false;
                }
            }

            if (!ConfirmarAccion("Confirmar eliminación",
                "¿Desea eliminar el registro seleccionado de la tabla '" + tabla + "'?"))
                return false;

            return ctrlRegistro.EliminarRegistro(tabla, pk);
        }

        // Traduce errores tecnicos del motor de BD a mensajes que el usuario entienda
        public string MensajeAmigable(Exception ex)
        {
            string lower = (ex.Message ?? "").ToLowerInvariant();

            if (lower.Contains("foreign key") || lower.Contains("fk_") || lower.Contains("reference constraint"))
                return "El registro no puede guardarse o eliminarse porque existe una relación de llave foránea.";

            if (lower.Contains("duplicate entry") || lower.Contains("duplicate key") ||
                lower.Contains("unique constraint") || lower.Contains("violation of unique") ||
                lower.Contains("violation of primary key"))
                return "Ya existe un registro con el mismo valor en un campo único.";

            if (lower.Contains("cannot be null") || lower.Contains("null value") ||
                lower.Contains("not-null constraint") || lower.Contains("insert the value null"))
                return "Hay un campo obligatorio que no puede quedar vacío.";

            if (lower.Contains("data too long") || lower.Contains("truncat") ||
                lower.Contains("string or binary data would be truncated"))
                return "Uno de los valores ingresados es demasiado largo para el campo correspondiente.";

            if ((lower.Contains("incorrect") && lower.Contains("value")) ||
                lower.Contains("conversion failed") || lower.Contains("invalid input syntax"))
                return "Uno de los valores ingresados tiene un formato incorrecto para su campo.";

            return ex.Message;
        }
    }
}