from pathlib import Path
import re
p=Path('CapaVista_OrdenesCompra/FrmDetalleOrdenCompra.Designer.cs')
s=p.read_bytes().decode('utf-8-sig').replace('\r\n','\n')
s=s.replace('this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 254F));','this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 540F));')
s=s.replace('this.ComprasTlpEncabezado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 230F));','this.ComprasTlpEncabezado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));')
s=s.replace('this.ComprasTlpEncabezado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 210F));','this.ComprasTlpEncabezado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 190F));')
s=s.replace('this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));','this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));')
s=s.replace('            this.ComprasTlpDetalles.Controls.Add(this.ComprasTlpMantenimientoProductos, 0, 0);','            this.ComprasTlpMantenimientoProductos.Visible = false;')
s=s.replace('            this.ComprasTlpComentarioYTotales.Controls.Add(this.ComprasBtnGuardar);','')
s=s.replace('this.ComprasGbxFinalizacion.Text = "5. Revisión final y comentario";', 'this.ComprasGbxFinalizacion.Text = "5. Totales y comentario";')
needle='            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasTxtPlazoValor, 1, 2);'
s=s.replace(needle,needle+'\n            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasLblDiasPlazo, 2, 2);\n            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasTxtDiasPlazo, 3, 2);')
for name in ['ComprasBtnRegresar','ComprasBtnDescartar']:
    s=re.sub(r'(this\.'+name+r'\.Size = )new System.Drawing.Size\([^;]+;',r'\1new System.Drawing.Size(150, 44);',s)
    s=re.sub(r'(this\.'+name+r'\.Font = )[^;]+;',r'\1new System.Drawing.Font("Tahoma", 10F);',s)
    s=re.sub(r'(this\.'+name+r'\.MinimumSize = )[^;]+;',r'\1new System.Drawing.Size(150, 44);',s)
    s=re.sub(r'(this\.'+name+r'\.MaximumSize = )[^;]+;',r'\1new System.Drawing.Size(180, 44);',s)
p.write_bytes(s.replace('\n','\r\n').encode('utf-8'))
p=Path('CapaVista_OrdenesCompra/FrmDetalleOrdenCompra.cs')
s=p.read_bytes().decode('utf-8-sig').replace('\r\n','\n')
s=s.replace('if (Texto != null) Texto.ReadOnly = true;', '''if (Texto != null)
                {
                    Texto.Enabled = true;
                    Texto.ReadOnly = true;
                    Texto.MostrarEtiquetaError = false;
                    Texto.TabStop = false;
                }''')
needle='''                ComprasTxtEstadoProveedor.Text = _Compra.ProveedorActivo ? "Activo" : "Inactivo";
            }
'''
assert needle in s
s=s.replace(needle,needle+'            ComprasProcSoloLectura(this);\n',1)
p.write_bytes(s.replace('\n','\r\n').encode('utf-8'))
