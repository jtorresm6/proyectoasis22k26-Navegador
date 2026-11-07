from pathlib import Path
p = Path(r'C:\proyectoasis22k26-Compras\codigo\modulos\compras\Compras\CapaVista_Compras\FrmMDICompras.Designer.cs')
data = p.read_bytes()
for name in ['RegistroCompras','DevolucionesCompras','Proveedores','OrdenesCompra','Mantenimiento2','Mantenimiento3']:
    old = ('this.ComprasBtnIcono' + name + '.Anchor = System.Windows.Forms.AnchorStyles.Right;').encode()
    new = ('this.ComprasBtnIcono' + name + '.Dock = System.Windows.Forms.DockStyle.Fill;').encode()
    assert data.count(old) == 1, name
    data = data.replace(old,new)
p.write_bytes(data)
print('All six dropdown icons now resize with their navigation rows.')
