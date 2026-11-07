from pathlib import Path
p=Path('CapaVista_OrdenesCompra/FrmDetalleOrdenCompra.cs')
s=p.read_bytes().decode('utf-8-sig').replace('\r\n','\n')
s=s.replace('            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);','            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);\n            ComprasProcConfigurarConsulta();',1)
s=s.replace('ComprasDtpFechaEmision.Enabled = Nueva;', 'ComprasDtpFechaEmision.Enabled = false;').replace('ComprasDtpFechaVencimiento.Enabled = Nueva;', 'ComprasDtpFechaVencimiento.Enabled = false;').replace('ComprasCboNombreProveedor.Enabled = Nueva;', 'ComprasCboNombreProveedor.Enabled = false;')
s=s.replace('ComprasLblFechaDeEmision.MostrarAsterisco = Nueva;', 'ComprasLblFechaDeEmision.MostrarAsterisco = false;').replace('ComprasLblFechaDeVencimiento.MostrarAsterisco = Nueva;', 'ComprasLblFechaDeVencimiento.MostrarAsterisco = false;')
s=s.replace('ComprasBtnDescartar.AccessibleName = Compra == null ? "Descartar" : "Anular orden";', 'ComprasBtnDescartar.AccessibleName = "Anular orden";')
s=s.replace('_Compra == null || (!_Compra.Bloqueado && _Compra.Estado != "Anulada" && _Compra.Estado != "Recibida")', '_Compra != null && !_Compra.Bloqueado && _Compra.Estado != "Anulada" && _Compra.Estado != "Recibida"')
needle='        private void ComprasProcMostrarEncabezado()'
code='''        private void ComprasProcConfigurarConsulta()
        {
            ComprasGbxEncabezado.Text = "Datos de la orden";
            ComprasTlpEncabezado.Controls.Clear();
            ComprasTlpEncabezado.ColumnStyles.Clear();
            ComprasTlpEncabezado.ColumnCount = 1;
            ComprasTlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            ComprasTlpEncabezado.RowStyles.Clear();
            ComprasTlpEncabezado.RowCount = 3;
            ComprasTlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            ComprasTlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));
            ComprasTlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Absolute, 210F));
            ComprasTlpPrincipal.RowStyles[1].Height = 590F;
            ComprasProcAgregarGrupoConsulta("ComprasGbxMoneda", "1. Moneda", 0,
                ComprasLblEtiquetaMoneda, ComprasTxtMoneda,
                ComprasLblEtiquetaTipoCambio, ComprasTxtTipoCambio);
            ComprasProcAgregarGrupoConsulta("ComprasGbxProveedor", "2. Proveedor", 1,
                ComprasLblNombreProveedor, ComprasCboNombreProveedor,
                ComprasLblNitProveedor, ComprasTxtNitProveedor,
                ComprasLblTelefonoProveedor, ComprasTxtTelefonoProveedor,
                ComprasLblCorreoProveedor, ComprasTxtCorreoProveedor,
                ComprasLblDireccionProveedor, ComprasTxtDireccionProveedor,
                ComprasLblEstadoProveedor, ComprasTxtEstadoProveedor);
            ComprasProcAgregarGrupoConsulta("ComprasGbxPlazo", "3. Orden y plazo", 2,
                ComprasLblCodigoOrden, ComprasTxtCodigoOrden,
                ComprasLblEstado, ComprasLblEstadoValor,
                ComprasLblFechaDeEmision, ComprasDtpFechaEmision,
                ComprasLblFechaDeVencimiento, ComprasDtpFechaVencimiento,
                ComprasLblPlazo, ComprasTxtPlazoValor,
                ComprasLblDiasPlazo, ComprasTxtDiasPlazo);
            ComprasTlpMantenimientoProductos.Visible = false;
            ComprasTlpDetalles.RowStyles[0].Height = 0F;
            ComprasGbxDetalles.Text = "4. Productos de la orden";
            ComprasGbxFinalizacion.Text = "5. Revisión final y comentario";
            ComprasTlpTotales.Controls.Clear();
            ComprasTlpTotales.ColumnStyles.Clear();
            ComprasTlpTotales.ColumnCount = 2;
            ComprasTlpTotales.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            ComprasTlpTotales.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            Control[] Importes = { ComprasLblEtiquetaSubtotal, ComprasTxtSubtotal,
                ComprasLblEtiquetaDescuento, ComprasTxtDescuentoTotal,
                ComprasLblEtiquetaImpuesto, ComprasTxtImpuesto,
                ComprasLblEtiquetaTotal, ComprasTxtTotalDeLaOrden };
            for (int Indice = 0; Indice < Importes.Length; Indice++)
                ComprasTlpTotales.Controls.Add(Importes[Indice], Indice % 2, Indice / 2);
            ComprasBtnGuardar.Visible = false;
            ComprasTlpComentarioYTotales.Controls.Remove(ComprasBtnGuardar);
            ComprasTlpComentarioYTotales.ColumnStyles.Clear();
            ComprasTlpComentarioYTotales.ColumnCount = 2;
            ComprasTlpComentarioYTotales.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            ComprasTlpComentarioYTotales.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            ComprasTlpComentarioYTotales.SetCellPosition(ComprasBtnRegresar, new TableLayoutPanelCellPosition(0, 0));
            ComprasTlpComentarioYTotales.SetCellPosition(ComprasBtnDescartar, new TableLayoutPanelCellPosition(1, 0));
            ComprasBtnRegresar.Text = "Regresar";
            ComprasBtnDescartar.Text = "Anular orden";
            ComprasProcSoloLectura(this);
            ComprasDgvProductos.Tabla.ReadOnly = true;
            ComprasDgvProductos.Tabla.AllowUserToAddRows = false;
            ComprasDgvProductos.Tabla.AllowUserToDeleteRows = false;
        }

        private void ComprasProcAgregarGrupoConsulta(string Nombre, string Titulo, int Fila, params Control[] Campos)
        {
            var Grupo = new ClsGroupBoxCompras
            {
                Name = Nombre, Text = Titulo, Dock = DockStyle.Fill, Margin = new Padding(6)
            };
            var Tabla = new ClsTableLayoutPanelCompras
            {
                Dock = DockStyle.Fill, ColumnCount = 4, RowCount = (Campos.Length + 3) / 4,
                Padding = new Padding(8), Margin = Padding.Empty
            };
            Tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            Tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            Tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            Tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            for (int FilaActual = 0; FilaActual < Tabla.RowCount; FilaActual++)
                Tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / Tabla.RowCount));
            for (int Indice = 0; Indice < Campos.Length; Indice++)
            {
                Campos[Indice].Dock = DockStyle.Fill;
                Campos[Indice].Margin = new Padding(4, 6, 4, 6);
                Tabla.Controls.Add(Campos[Indice], Indice % 4, Indice / 4);
            }
            Grupo.Controls.Add(Tabla);
            ComprasTlpEncabezado.Controls.Add(Grupo, 0, Fila);
        }

        private static void ComprasProcSoloLectura(Control Contenedor)
        {
            foreach (Control Campo in Contenedor.Controls)
            {
                var Texto = Campo as ComprasUsrTextBox;
                if (Texto != null) Texto.ReadOnly = true;
                if (Campo is ComprasUsrComboBox || Campo is DateTimePicker) Campo.Enabled = false;
                ComprasProcSoloLectura(Campo);
            }
        }

'''
assert needle in s
s=s.replace(needle,code+needle,1)
p.write_bytes(s.replace('\n','\r\n').encode('utf-8'))
