using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;

namespace CapaVista_OrdenesCompra.Presentacion
{
    internal sealed class ClsOrdenesCompraErrores
    {
        private sealed class Posicion
        {
            internal Control Control;
            internal int Fila;
            internal int Columna;
            internal int Filas;
            internal int Altura;
            internal AnchorStyles Ancla;
        }

        private sealed class TablaError
        {
            internal TableLayoutPanel Tabla;
            internal readonly List<Posicion> Posiciones = new List<Posicion>();
            internal readonly List<RowStyle> Filas = new List<RowStyle>();
            internal int Cantidad;
            internal GroupBox Grupo;
        }

        private readonly Dictionary<TableLayoutPanel, TablaError> _Tablas = new Dictionary<TableLayoutPanel, TablaError>();
        private readonly Dictionary<Control, bool> _Errores = new Dictionary<Control, bool>();
        private readonly Dictionary<GroupBox, int> _Alturas = new Dictionary<GroupBox, int>();
        private readonly Action _Reorganizar;
        private bool _Actualizando;

        internal ClsOrdenesCompraErrores(Control Contenedor, Action Reorganizar)
        {
            _Reorganizar = Reorganizar;
            Registrar(Contenedor);
            foreach (var Grupo in _Alturas) Grupo.Key.Height = Grupo.Value + 22;
            foreach (var Tabla in _Tablas.Values) Actualizar(Tabla);
        }

        private void Registrar(Control Contenedor)
        {
            var Grupo = Contenedor as GroupBox;
            if (Grupo != null) _Alturas[Grupo] = Grupo.Height;
            var Tabla = Contenedor as TableLayoutPanel;
            GroupBox GrupoTabla = null;
            for (Control Padre = Contenedor.Parent; Padre != null; Padre = Padre.Parent)
                if (Padre is GroupBox) { GrupoTabla = (GroupBox)Padre; break; }
            if (Tabla != null && GrupoTabla != null)
            {
                var Datos = new TablaError { Tabla = Tabla, Cantidad = Tabla.RowCount, Grupo = GrupoTabla };
                foreach (RowStyle Fila in Tabla.RowStyles)
                    Datos.Filas.Add(new RowStyle(Fila.SizeType, Fila.Height));
                while (Datos.Filas.Count < Datos.Cantidad) Datos.Filas.Add(new RowStyle(SizeType.AutoSize));
                foreach (Control Campo in Tabla.Controls)
                {
                    Datos.Posiciones.Add(new Posicion { Control = Campo, Fila = Tabla.GetRow(Campo),
                        Columna = Tabla.GetColumn(Campo), Filas = Tabla.GetRowSpan(Campo),
                        Altura = Campo.Height, Ancla = Campo.Anchor });
                    if (!(Campo is ComprasUsrTextBox) && !(Campo is ComprasUsrComboBox)) continue;
                    _Errores[Campo] = false;
                    var Etiquetas = Campo.Controls.Find("ComprasLblError", true);
                    if (Etiquetas.Length == 0) continue;
                    Control Etiqueta = Etiquetas[0];
                    Etiqueta.Margin = new Padding(Etiqueta.Margin.Left, 0, Etiqueta.Margin.Right, 0);
                    Etiqueta.TextChanged += (Remitente, Evento) => Establecer(Campo, !string.IsNullOrWhiteSpace(Etiqueta.Text));
                }
                bool TieneCampos = false;
                foreach (var Posicion in Datos.Posiciones)
                    if (_Errores.ContainsKey(Posicion.Control)) TieneCampos = true;
                if (TieneCampos) _Tablas[Tabla] = Datos;
            }
            foreach (Control Hijo in Contenedor.Controls) Registrar(Hijo);
        }

        internal void Establecer(Control Campo, bool Mostrar)
        {
            if (_Actualizando || !_Errores.ContainsKey(Campo) || _Errores[Campo] == Mostrar) return;
            ScrollableControl Desplazamiento = null;
            for (Control Padre = Campo.Parent; Padre != null; Padre = Padre.Parent)
                if (Padre is ClsOrdenesCompraComunPanel) { Desplazamiento = (ScrollableControl)Padre; break; }
            Point Posicion = Desplazamiento == null ? Point.Empty : Desplazamiento.AutoScrollPosition;
            try
            {
                _Errores[Campo] = Mostrar;
                Actualizar(_Tablas[(TableLayoutPanel)Campo.Parent]);
                _Reorganizar();
            }
            finally
            {
                if (Desplazamiento != null && !Desplazamiento.IsDisposed)
                    Desplazamiento.AutoScrollPosition = new Point(-Posicion.X, -Posicion.Y);
            }
        }

        private void Actualizar(TablaError Datos)
        {
            _Actualizando = true;
            var Tabla = Datos.Tabla;
            Tabla.SuspendLayout();
            try
            {
                var Auxiliares = new SortedSet<int>();
                foreach (var Posicion in Datos.Posiciones)
                    if (_Errores.ContainsKey(Posicion.Control) && _Errores[Posicion.Control])
                        Auxiliares.Add(Posicion.Fila + Posicion.Filas - 1);
                foreach (var Posicion in Datos.Posiciones) Tabla.SetRowSpan(Posicion.Control, 1);
                Tabla.RowCount = Math.Max(Datos.Cantidad, Datos.Filas.Count) + Auxiliares.Count;
                Tabla.RowStyles.Clear();
                for (int Fila = 0; Fila < Datos.Filas.Count; Fila++)
                {
                    RowStyle Original = Datos.Filas[Fila];
                    Tabla.RowStyles.Add(new RowStyle(Original.SizeType, Original.Height));
                    if (Auxiliares.Contains(Fila)) Tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
                }
                foreach (var Posicion in Datos.Posiciones)
                {
                    int Desplazamiento = 0, Expansion = 0;
                    bool Mostrar = _Errores.ContainsKey(Posicion.Control) && _Errores[Posicion.Control];
                    foreach (int Auxiliar in Auxiliares)
                    {
                        if (Auxiliar < Posicion.Fila) Desplazamiento++;
                        else if (Auxiliar < Posicion.Fila + Posicion.Filas - 1
                            || Mostrar && Auxiliar == Posicion.Fila + Posicion.Filas - 1) Expansion++;
                    }
                    Tabla.SetCellPosition(Posicion.Control, new TableLayoutPanelCellPosition(Posicion.Columna, Posicion.Fila + Desplazamiento));
                    Tabla.SetRowSpan(Posicion.Control, Posicion.Filas + Expansion);
                    var Texto = Posicion.Control as ComprasUsrTextBox;
                    var Combo = Posicion.Control as ComprasUsrComboBox;
                    if (Texto != null) { Texto.AlturaEtiquetaError = 22; Texto.MostrarEtiquetaError = Mostrar; }
                    if (Combo != null) { Combo.AlturaEtiquetaError = 22; Combo.MostrarEtiquetaError = Mostrar; }
                    if (Texto != null || Combo != null)
                    {
                        Posicion.Control.Anchor = Mostrar ? AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right : Posicion.Ancla;
                        Posicion.Control.Height = Posicion.Altura + (Mostrar ? 22 : 0);
                    }
                }
                var Grupo = Datos.Grupo;
                int Minimo = _Alturas[Grupo], Extras = 0;
                foreach (var Otra in _Tablas.Values)
                {
                    if (Otra.Grupo != Grupo) continue;
                    var FilasConError = new HashSet<int>();
                    foreach (var Posicion in Otra.Posiciones)
                        if (_Errores.ContainsKey(Posicion.Control) && _Errores[Posicion.Control])
                            FilasConError.Add(Posicion.Fila + Posicion.Filas - 1);
                    Extras += FilasConError.Count * 22;
                    if (Otra.Tabla.Parent != Grupo) continue;
                    int Necesario = Otra.Tabla.Padding.Vertical + Grupo.Padding.Vertical + Grupo.Font.Height + 10;
                    foreach (RowStyle Fila in Otra.Filas)
                        if (Fila.SizeType == SizeType.Absolute) Necesario += (int)Math.Ceiling(Fila.Height);
                    Minimo = Math.Max(Minimo, Necesario);
                }
                Grupo.Height = Minimo + Math.Max(22, Extras);
            }
            finally
            {
                Tabla.ResumeLayout(true);
                _Actualizando = false;
            }
        }
    }
}
