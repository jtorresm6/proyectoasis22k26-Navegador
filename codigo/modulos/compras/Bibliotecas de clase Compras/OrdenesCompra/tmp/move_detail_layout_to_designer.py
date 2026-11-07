from pathlib import Path
import re
p=Path('CapaVista_OrdenesCompra')
f=p/'FrmDetalleOrdenCompra.cs'
s=f.read_bytes().decode('utf-8-sig').replace('\r\n','\n')
a=s.index('        private void ComprasProcConfigurarConsulta()')
b=s.index('        private static void ComprasProcSoloLectura',a)
block=s[a:b]
layout=block[block.index('            ComprasUsrEncabezado.Descripcion'):block.index('            ComprasProcSoloLectura(this);')]
fields=[]
def group(m):
    name,title,row,raw=m.groups()
    controls=[x.strip() for x in raw.replace('\n','').split(',')]
    table=name.replace('Gbx','TlpConsulta')
    fields.extend([('ClsGroupBoxCompras',name),('ClsTableLayoutPanelCompras',table)])
    lines=[f'{name} = new ClsGroupBoxCompras();',f'{name}.Name = "{name}";',f'{name}.Text = "{title}";',f'{name}.Dock = DockStyle.Fill;',f'{name}.Margin = new Padding(6);',f'{table} = new ClsTableLayoutPanelCompras();',f'{table}.Dock = DockStyle.Fill;',f'{table}.ColumnCount = 4;',f'{table}.RowCount = {(len(controls)+3)//4};',f'{table}.Padding = new Padding(8);']
    for width in [18,32,18,32]: lines.append(f'{table}.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, {width}F));')
    for _ in range((len(controls)+3)//4): lines.append(f'{table}.RowStyles.Add(new RowStyle(SizeType.Percent, {100/((len(controls)+3)//4):.6f}F));')
    for i,c in enumerate(controls):
        lines.extend([f'{c}.Dock = DockStyle.Fill;',f'{c}.Margin = new Padding(4, 6, 4, 6);',f'{table}.SetColumnSpan({c}, 1);',f'{table}.SetRowSpan({c}, 1);',f'{table}.Controls.Add({c}, {i%4}, {i//4});'])
        if 'Lbl' in c: lines.append(f'{c}.MostrarAsterisco = false;')
    lines.extend([f'{name}.Controls.Add({table});',f'ComprasTlpEncabezado.Controls.Add({name}, 0, {row});'])
    return '\n'.join('            '+l for l in lines)
layout=re.sub(r'            ComprasProcAgregarGrupoConsulta\("([^"]+)", "([^"]+)", (\d+),\s*(.*?)\);',group,layout,flags=re.S)
start=layout.index('            Control[] Importes')
end=layout.index('            ComprasBtnGuardar.Visible',start)
controls=['ComprasLblEtiquetaSubtotal','ComprasTxtSubtotal','ComprasLblEtiquetaDescuento','ComprasTxtDescuentoTotal','ComprasLblEtiquetaImpuesto','ComprasTxtImpuesto','ComprasLblEtiquetaTotal','ComprasTxtTotalDeLaOrden']
lines=[]
for i,c in enumerate(controls):
    lines.extend([f'            ComprasTlpTotales.SetColumnSpan({c}, 1);',f'            ComprasTlpTotales.SetRowSpan({c}, 1);',f'            {c}.Dock = DockStyle.Fill;',f'            ComprasTlpTotales.Controls.Add({c}, {i%2}, {i//2});'])
layout=layout[:start]+'\n'.join(lines)+'\n'+layout[end:]
start=layout.index('            foreach (var Boton')
end=layout.index('            ComprasBtnRegresar.Text',start)
lines=[]
for c in ['ComprasBtnRegresar','ComprasBtnDescartar']:
    lines.extend([f'            {c}.TipoBoton = ComprasTipoBoton.Libre;',f'            {c}.BackgroundImage = null;',f'            {c}.Size = new System.Drawing.Size(150, 44);',f'            {c}.Anchor = AnchorStyles.None;'])
layout=layout[:start]+'\n'.join(lines)+'\n'+layout[end:]
s=s[:a]+s[b:]
s=s.replace('            ComprasProcConfigurarConsulta();','''            ComprasProcSoloLectura(this);
            ComprasDgvProductos.Tabla.ReadOnly = true;
            ComprasDgvProductos.Tabla.AllowUserToAddRows = false;
            ComprasDgvProductos.Tabla.AllowUserToDeleteRows = false;''',1)
f.write_bytes(s.replace('\n','\r\n').encode('utf-8'))
f=p/'FrmDetalleOrdenCompra.Designer.cs'
s=f.read_bytes().decode('utf-8-sig').replace('\r\n','\n')
s='using System.Windows.Forms;\nusing CapaVista_Componentes.Componentes;\n\n'+s
needle='            this.ComprasPnlDesplazamiento.ResumeLayout(false);'
assert s.count(needle)==1
s=s.replace(needle,layout+needle,1)
needle='        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPrincipal;'
s=s.replace(needle,'\n'.join(f'        private {kind} {name};' for kind,name in fields)+'\n'+needle,1)
f.write_bytes(s.replace('\n','\r\n').encode('utf-8'))
print('Layout moved to FrmDetalleOrdenCompra.Designer.cs; runtime layout construction removed.')
