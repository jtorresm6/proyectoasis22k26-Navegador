from pathlib import Path
p=Path('CapaVista_OrdenesCompra/FrmDetalleOrdenCompra.Designer.cs')
s=p.read_bytes().decode().replace('\r\n','\n')
needle='            this.ComprasTlpConsultaMoneda.RowCount = 1;'
s=s.replace(needle,needle+'''
            this.ComprasTlpConsultaMoneda.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpConsultaMoneda.Controls.Add(this.ComprasLblEtiquetaMoneda, 0, 0);
            this.ComprasTlpConsultaMoneda.Controls.Add(this.ComprasTxtMoneda, 1, 0);
            this.ComprasTlpConsultaMoneda.Controls.Add(this.ComprasLblEtiquetaTipoCambio, 2, 0);
            this.ComprasTlpConsultaMoneda.Controls.Add(this.ComprasTxtTipoCambio, 3, 0);''',1)
p.write_bytes(s.replace('\n','\r\n').encode())
