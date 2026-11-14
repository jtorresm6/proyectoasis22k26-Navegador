using System;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;

class ValidarFilasErrores
{
    static T Campo<T>(object Objeto, string Nombre) where T : class
    {
        return (T)Objeto.GetType().GetField(Nombre, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Objeto);
    }
    static void Verificar(bool Condicion, string Mensaje)
    {
        if (!Condicion) throw new Exception(Mensaje);
    }
    [STAThread]
    static void Main()
    {
        using (var Formulario = new FrmOrdenesCompra())
        {
            Formulario.CreateControl();
            var Tabla = Campo<TableLayoutPanel>(Formulario, "ComprasTlpProducto");
            var Importe = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescuentoProducto");
            var Porcentaje = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescuentoPorcentajeProducto");
            var Sku = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSkuProducto");
            var Nombre = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboNombreProducto");
            var Grupo = Campo<GroupBox>(Formulario, "ComprasGbxProducto");
            int Filas = Tabla.RowCount, Fila = Tabla.GetRow(Importe), Altura = Importe.Height, AltoGrupo = Grupo.Height;
            Action<Control, string> Error = (Control, Mensaje) => Formulario.GetType().GetMethod("ComprasProcEstablecerError", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Formulario, new object[] { Control, Mensaje });
            Error(Importe, "Error de importe");
            Verificar(Tabla.RowCount == Filas + 1, "Falta fila auxiliar");
            Verificar(Tabla.GetRowSpan(Importe) == 2, "RowSpan debe ser dos");
            Verificar(Tabla.RowStyles[Fila + 1].SizeType == SizeType.Absolute && Tabla.RowStyles[Fila + 1].Height == 22F, "Fila debe medir 22 px");
            Verificar(Importe.Height == Altura + 22, "Altura de campo incorrecta");
            Error(Porcentaje, "Error de porcentaje");
            Error(Sku, "Error de SKU");
            Error(Nombre, "Error de nombre");
            Verificar(Tabla.RowCount == Filas + 4, "No soporta errores simultaneos");
            Verificar(Grupo.Height >= AltoGrupo + 66, "Grupo no crece con errores simultaneos");
            Error(Importe, "");
            Verificar(Tabla.GetRowSpan(Importe) == 1 && Tabla.RowCount == Filas + 3, "No restaura fila intermedia");
            Error(Importe, "Otro error");
            Error(Importe, "Otro mensaje");
            Verificar(Tabla.RowCount == Filas + 4, "Fila duplicada");
            Error(Importe, ""); Error(Porcentaje, ""); Error(Sku, ""); Error(Nombre, "");
            Verificar(Tabla.RowCount == Filas && Tabla.GetRow(Importe) == Fila, "No restaura las posiciones");
            Verificar(Importe.Height == Altura && Grupo.Height == AltoGrupo, "No restaura alturas");
            Sku.ComprasMetMostrarError("Error interno");
            Verificar(Tabla.RowCount == Filas + 1, "Error interno no observado");
            Sku.ComprasMetLimpiarError();
            Verificar(Tabla.RowCount == Filas, "Error interno no restaurado");
            foreach (string NombreTabla in new[] { "ComprasTlpMoneda", "ComprasTlpProveedor", "ComprasTlpPlazo", "ComprasTlpBodega" })
            {
                var Otra = Campo<TableLayoutPanel>(Formulario, NombreTabla);
                int Cantidad = Otra.RowCount;
                foreach (Control Control in Otra.Controls)
                    if (Control is ComprasUsrTextBox || Control is ComprasUsrComboBox)
                    {
                        int Original = Otra.GetRowSpan(Control);
                        Error(Control, "Error de validación");
                        Verificar(Otra.RowCount == Cantidad + 1 && Otra.GetRowSpan(Control) == Original + 1, "Error de filas en " + NombreTabla);
                        Error(Control, "");
                        Verificar(Otra.RowCount == Cantidad && Otra.GetRowSpan(Control) == Original, "Restauracion en " + NombreTabla);
                    }
            }
            var Proveedor = Campo<TableLayoutPanel>(Formulario, "ComprasTlpProveedor");
            var ProveedorNombre = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboNombreProveedor");
            var Correo = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtCorreoProveedor");
            int FilasProveedor = Proveedor.RowCount;
            Error(ProveedorNombre, "Selecciona un proveedor"); Error(Correo, "Correo inválido");
            Verificar(Proveedor.RowCount == FilasProveedor + 1, "Los campos de una misma fila deben compartir la auxiliar");
            Error(ProveedorNombre, "");
            Verificar(Proveedor.RowCount == FilasProveedor + 1 && Proveedor.GetRowSpan(Correo) == 2, "Se quitó la fila de un error vigente");
            Error(Correo, "");
            Verificar(Proveedor.RowCount == FilasProveedor, "No restauró la fila compartida");
            var Comentario = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtComentario");
            var TablaComentario = (TableLayoutPanel)Comentario.Parent;
            int FilasComentario = TablaComentario.RowCount;
            Error(Comentario, "Comentario inválido");
            Verificar(TablaComentario.RowCount == FilasComentario + 1 && TablaComentario.GetRowSpan(Comentario) == 2, "No maneja tablas anidadas");
            Error(Comentario, "");
            Verificar(TablaComentario.RowCount == FilasComentario && TablaComentario.GetRowSpan(Comentario) == 1, "No restauró la tabla anidada");
            for (int Ciclo = 0; Ciclo < 10; Ciclo++)
            {
                Error(Sku, "Error"); Error(Importe, "Error"); Error(Sku, ""); Error(Importe, "");
                Verificar(Tabla.RowCount == Filas && Grupo.Height == AltoGrupo && Importe.Height == Altura, "Las alturas acumulan cambios");
            }
            Error(Importe, "El descuento supera el precio unitario.");
            Error(Sku, "Selecciona una opción del catálogo.");
            var Contenido = Campo<Panel>(Formulario, "ComprasPnlContenido");
            Contenido.Parent = null;
            Contenido.CreateControl();
            using (var Captura = new Bitmap(Contenido.Width, Contenido.Height))
            {
                Contenido.DrawToBitmap(Captura, new Rectangle(Point.Empty, Captura.Size));
                Captura.Save("Validacion/FilasErrores.png");
            }
        }
        Console.WriteLine("Filas de errores: insercion, 22 px, RowSpan, errores simultaneos, restauracion y alturas correctos.");
    }
}
