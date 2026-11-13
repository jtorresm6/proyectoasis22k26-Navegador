using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;
namespace CapaVista_OrdenesCompra
{
    internal static class ClsProductosOrdenesCompraPestanas
    {
        internal static void Ajustar(FlowLayoutPanel Panel, int? BodegaVisible)
        {
            int Activo = -1;
            for (int Indice = 0; Indice < Panel.Controls.Count; Indice++)
                if (Convert.ToInt32(Panel.Controls[Indice].Tag) == BodegaVisible) Activo = Indice;
            if (Activo < 0) return;
            int Disponible = Panel.ClientSize.Width;
            Func<int, int> Ancho = Indice => Math.Max(120,
                Panel.Controls[Indice].GetPreferredSize(Size.Empty).Width)
                + Panel.Controls[Indice].Margin.Horizontal;
            int Inicio = Activo, Usado = Ancho(Activo);
            while (Inicio > 0 && Usado + Ancho(Inicio - 1) <= Disponible) { Inicio--; Usado += Ancho(Inicio); }
            Usado = 0;
            Panel.SuspendLayout();
            try
            {
                for (int Indice = 0; Indice < Panel.Controls.Count; Indice++)
                {
                    bool Mostrar = Indice >= Inicio && (Usado + Ancho(Indice) <= Disponible || Indice == Activo);
                    Panel.Controls[Indice].Visible = Mostrar;
                    if (Mostrar) Usado += Ancho(Indice);
                }
            }
            finally { Panel.ResumeLayout(true); }
        }

        internal static int? Actualizar(FlowLayoutPanel Panel, Control Anterior, Control Siguiente, DataTable Detalles, int? BodegaVisible, Action<int> Seleccionar)
        {
            while (Panel.Controls.Count > 0)
            {
                Control PestanaAnterior = Panel.Controls[0];
                Panel.Controls.Remove(PestanaAnterior);
                PestanaAnterior.Dispose();
            }
            var Bodegas = new System.Collections.Generic.Dictionary<int, string>();
            foreach (DataRow Fila in Detalles.Rows)
                Bodegas[Convert.ToInt32(Fila["idBodega"])] = (string.IsNullOrWhiteSpace(Convert.ToString(Fila["sucursal"]))
                    ? "Sucursal sin nombre" : Convert.ToString(Fila["sucursal"]))
                    + Environment.NewLine + Convert.ToString(Fila["bodega"]);
            if (!BodegaVisible.HasValue || !Bodegas.ContainsKey(BodegaVisible.Value))
            {
                BodegaVisible = null;
                foreach (int Id in Bodegas.Keys) { BodegaVisible = Id; break; }
            }
            foreach (var Bodega in Bodegas)
            {
                int Id = Bodega.Key;
                var Boton = new ClsButtonCompras
                {
                    TipoBoton = ComprasTipoBoton.Libre, EsActivo = BodegaVisible == Id,
                    Name = "ComprasTabBodega" + Id, Text = Bodega.Value, Tag = Id, AutoSize = true,
                    Height = 80, AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    MinimumSize = new Size(120, 80), MaximumSize = new Size(0, 80),
                    TextAlign = ContentAlignment.MiddleCenter, Padding = new Padding(8, 0, 8, 0),
                    FlatStyle = FlatStyle.Flat, Margin = new Padding(2, 0, 2, 0),
                    TabIndex = Panel.Controls.Count,
                    UseVisualStyleBackColor = false,
                    BackColor = BodegaVisible == Id ? ClsTemaCompras.PrincipalNavegador : ClsTemaCompras.FondoBotones,
                    ForeColor = BodegaVisible == Id ? Color.White : ClsTemaCompras.PrincipalNavegador
                };
                Boton.FlatAppearance.MouseOverBackColor = Boton.BackColor;
                Boton.FlatAppearance.MouseDownBackColor = Boton.BackColor;
                Boton.Click += (Remitente, Evento) =>
                {
                    Seleccionar(Id);
                };
                Panel.Controls.Add(Boton);
            }
            int IndiceActivo = -1;
            for (int Indice = 0; Indice < Panel.Controls.Count; Indice++)
                if ((int)Panel.Controls[Indice].Tag == BodegaVisible)
                    IndiceActivo = Indice;
            Anterior.Enabled = IndiceActivo > 0;
            Siguiente.Enabled = IndiceActivo >= 0 && IndiceActivo < Panel.Controls.Count - 1;
            Ajustar(Panel, BodegaVisible);
            return BodegaVisible;
        }
    }
}
