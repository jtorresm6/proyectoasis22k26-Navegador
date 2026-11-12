using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CapaVista_MovimientosClientes;
using CapaVista_Pedidos;

namespace CapaVista_Ventas
{
    internal sealed class ClsMenuProcesosVentas
    {
        private readonly TableLayoutPanel _Menu;
        private readonly List<Button> _Opciones = new List<Button>();
        private readonly HashSet<Button> _Abiertos = new HashSet<Button>();
        private readonly Dictionary<Button, List<Button>> _Grupos = new Dictionary<Button, List<Button>>();
        private readonly Dictionary<Button, int> _AplicacionPorOpcion = new Dictionary<Button, int>();
        private readonly Func<int, bool> _TieneAccesoAplicacion;
        private readonly Action<int, string> _MostrarAccesoDenegado;
        private readonly Action<Button, bool> _AplicarEstadoAcceso;

        internal ClsMenuProcesosVentas(TableLayoutPanel Menu, Button Movimientos, Button Pagos, Button Pedidos,
            Action<Form> Abrir, Func<int> Usuario, Func<int, bool> TieneAccesoAplicacion,
            Action<int, string> MostrarAccesoDenegado, Action<Button, bool> AplicarEstadoAcceso)
        {
            _Menu = Menu;
            _TieneAccesoAplicacion = TieneAccesoAplicacion;
            _MostrarAccesoDenegado = MostrarAccesoDenegado;
            _AplicarEstadoAcceso = AplicarEstadoAcceso;
            // Evita pintar cada fila por separado durante la expansion y el desplazamiento.
            typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(Menu, true, null);
            Menu.BackColor = Color.FromArgb(59, 29, 74);
            Menu.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            var Originales = Menu.Controls.Cast<Control>().OrderBy(Menu.GetRow).ToArray();
            Menu.Controls.Clear();
            Menu.RowStyles.Clear();
            Menu.RowCount = 0;
            foreach (Control Original in Originales)
            {
                VentasMetAgregar(Original);
                if (Original != Movimientos && Original != Pagos && Original != Pedidos) continue;
                var Padre = (Button)Original;
                var Hijos = new List<Button>();
                _Grupos.Add(Padre, Hijos);
                Padre.Paint += (Origen, Evento) =>
                {
                    int X = Padre.Width - 18, Y = Padre.Height / 2;
                    Point[] Flecha = _Abiertos.Contains(Padre)
                        ? new[] { new Point(X - 5, Y - 3), new Point(X + 5, Y - 3), new Point(X, Y + 3) }
                        : new[] { new Point(X - 3, Y - 5), new Point(X - 3, Y + 5), new Point(X + 3, Y) };
                    using (var Pincel = new SolidBrush(Color.FromArgb(201, 162, 74)))
                        Evento.Graphics.FillPolygon(Pincel, Flecha);
                };
                string[] Titulos = Original == Movimientos
                    ? new[] { "Estado de cuenta", "Cargos y ajustes" }
                    : Original == Pedidos
                        ? new[] { "Seguimiento de pedidos", "Nuevo pedido" }
                        : new[] { "Recepción de pagos", "Depósitos a Bancos", "Consulta de recibos" };
                foreach (string Titulo in Titulos)
                {
                    string Opcion = Titulo;
                    int IdAplicacion = VentasFuncIdAplicacion(Opcion);
                    var Boton = new Button
                    {
                        Text = Titulo, Name = "VentasBtnProceso" + _Opciones.Count,
                        Dock = DockStyle.Fill, FlatStyle = FlatStyle.Flat,
                        BackColor = Color.FromArgb(59, 29, 74), ForeColor = Color.White,
                        TextAlign = ContentAlignment.MiddleLeft, Visible = false, Tag = false
                    };
                    _AplicacionPorOpcion.Add(Boton, IdAplicacion);
                    Boton.Click += (Origen, Evento) =>
                    {
                        if (!_TieneAccesoAplicacion(IdAplicacion))
                        {
                            _MostrarAccesoDenegado(IdAplicacion, Opcion);
                            return;
                        }

                        using (var Formulario = VentasFuncCrear(Opcion))
                        {
                            var Movimiento = Formulario as FrmMovimientosClientesVentas;
                            if (Movimiento != null) Movimiento.VentasIdUsuario = Usuario();
                            Abrir(Formulario);
                        }
                    };
                    Hijos.Add(Boton);
                    _Opciones.Add(Boton);
                    VentasMetAgregar(Boton);
                }
            }
            Pagos.Text = "Pagos";
            Menu.AutoSize = false;
            Menu.ColumnStyles[0].SizeType = SizeType.Percent;
            Menu.ColumnStyles[0].Width = 100;
            Menu.Dock = DockStyle.Top;
            var Panel = Menu.Parent as ScrollableControl;
            if (Panel != null) Panel.AutoScroll = true;
            Menu.Parent.SizeChanged += (Origen, Evento) => VentasMetAcomodar();
            VentasMetAcomodar();
        }

        private void VentasMetAgregar(Control Control)
        {
            int Fila = _Menu.RowCount++;
            if (Control is Button) Control.Margin = new Padding(3, 5, 3, 5);
            _Menu.RowStyles.Add(new RowStyle(SizeType.Absolute, Control is Button ? 56 : 100));
            _Menu.Controls.Add(Control, 0, Fila);
        }

        internal void VentasMetAlternar(Button Padre)
        {
            if (!_Grupos.TryGetValue(Padre, out List<Button> Opciones))
                return;

            bool Abierto = !_Abiertos.Contains(Padre);

            _Menu.SuspendLayout();
            try
            {
            if (Abierto) _Abiertos.Add(Padre); else _Abiertos.Remove(Padre);
            foreach (var Boton in Opciones)
            {
                bool TieneAcceso = _TieneAccesoAplicacion(_AplicacionPorOpcion[Boton]);
                _AplicarEstadoAcceso(Boton, TieneAcceso);
                Boton.Tag = Abierto;
                Boton.Visible = Abierto;
            }
            Padre.Invalidate();
            VentasMetAcomodar();
            }
            finally
            {
                _Menu.ResumeLayout(true);
                _Menu.Invalidate(true);
                _Menu.Parent.Invalidate(true);
                _Menu.Update();
            }
        }

        internal void VentasMetActualizarPermisos()
        {
            foreach (var Grupo in _Grupos)
            {
                bool TieneAlgunaAplicacion = Grupo.Value.Any(Boton =>
                    _TieneAccesoAplicacion(_AplicacionPorOpcion[Boton]));
                _AplicarEstadoAcceso(Grupo.Key, TieneAlgunaAplicacion);
                Grupo.Key.Enabled = true;
                foreach (var Boton in Grupo.Value)
                {
                    _AplicarEstadoAcceso(Boton,
                        _TieneAccesoAplicacion(_AplicacionPorOpcion[Boton]));
                }
            }
        }

        internal bool VentasMetEsGrupo(Button Boton)
        {
            return _Grupos.ContainsKey(Boton);
        }

        internal bool VentasMetTieneAccesoGrupo(Button Boton)
        {
            if (!_Grupos.TryGetValue(Boton, out List<Button> Opciones))
                return true;

            return Opciones.Any(Opcion =>
                _TieneAccesoAplicacion(_AplicacionPorOpcion[Opcion]));
        }

        private void VentasMetAcomodar()
        {
            foreach (var Boton in _Opciones)
            {
                _Menu.RowStyles[_Menu.GetRow(Boton)].Height = (bool)Boton.Tag ? 50 : 0;
                Boton.Padding = new Padding(_Menu.Parent.Width > 100 ? 20 : 0, 0, 0, 0);
            }
            _Menu.Height = (int)_Menu.RowStyles.Cast<RowStyle>().Sum(Fila => Fila.Height);
        }

        private static Form VentasFuncCrear(string Opcion)
        {
            switch (Opcion)
            {
                case "Estado de cuenta": return new FrmEstadoCuentaVentas();
                case "Cargos y ajustes": return new FrmCargosAjustesVentas();
                case "Recepción de pagos": return new FrmRecepcionPagosVentas();
                case "Depósitos a Bancos": return new FrmDepositosBancosVentas();
                case "Seguimiento de pedidos": return new FrmSeguimientoPedidos();
                case "Nuevo pedido": return new FrmPedidoVentas();
                default: return new FrmConsultaRecibosVentas();
            }
        }

        private static int VentasFuncIdAplicacion(string Opcion)
        {
            switch (Opcion)
            {
                case "Estado de cuenta": return 7003;
                case "Cargos y ajustes": return 7014;
                case "Recepción de pagos": return 7005;
                case "Depósitos a Bancos": return 7015;
                case "Consulta de recibos": return 7005;
                case "Seguimiento de pedidos": return 7004;
                case "Nuevo pedido": return 7004;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Opcion), Opcion,
                        "La opción de Ventas no tiene una aplicación de permisos asignada.");
            }
        }
    }
}
