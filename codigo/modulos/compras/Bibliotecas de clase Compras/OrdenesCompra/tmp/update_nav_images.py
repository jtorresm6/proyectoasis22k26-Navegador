from pathlib import Path
p = Path(r'C:\proyectoasis22k26-Compras\codigo\modulos\compras\Compras\CapaVista_Compras')
f = p / 'FrmMDICompras.Designer.cs'
s = f.read_bytes().decode('utf-8-sig')
mapping = {'OrdenesCompra':'ComprasBtnOrdenesCompra','RegistroCompras':'ComprasBtnCompras','DevolucionesCompras':'ComprasBtnDevolucionesCompra','Proveedores':'ComprasBtnProveedores','Mantenimiento2':'ComprasBtnPlazos','Mantenimiento3':'ComprasBtnConceptos'}
import re
for tab, resource in mapping.items():
    pattern = r'(this\.ComprasBtnIcono' + tab + r'\.BackgroundImage = .*?resources.GetObject\(")[^"]+("\)\)\);)'
    s, count = re.subn(pattern, lambda m: m[1] + resource + m[2], s)
    assert count == 1, tab
f.write_bytes(s.encode('utf-8'))
f = p / 'FrmMDICompras.cs'
s = f.read_bytes().decode('utf-8-sig')
newline = '\r\n' if '\r\n' in s else '\n'
s = s.replace('\r\n','\n')
s = s.replace('        private Form _FormularioSubproceso;', '        private ToolTip _AyudaNavegador;\n        private Form _FormularioSubproceso;', 1)
s = s.replace('            InitializeComponent();', '            InitializeComponent();\n            if (_Componentes == null) _Componentes = new Container();\n            _AyudaNavegador = new ToolTip(_Componentes) { ShowAlways = true };', 1)
needle = '            Elemento.MouseEnter += (Origen, Argumentos) => Panel.ComprasProcActualizarHover(true);'
replacement = '''            foreach (Control ControlActual in Panel.Controls)
            {
                var Etiqueta = ControlActual as CapaVista_Componentes.Componentes.ComprasUsrLabel;
                if (Etiqueta != null)
                {
                    _AyudaNavegador.SetToolTip(Elemento, Etiqueta.Texto);
                    break;
                }
            }
''' + needle
assert s.count(needle) == 1
s = s.replace(needle, replacement, 1)
f.write_bytes(s.replace('\n',newline).encode('utf-8'))
print('Updated six navigation images and recursive full-name tooltips in FrmMDICompras.')
