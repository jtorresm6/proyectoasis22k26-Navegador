from pathlib import Path
root = Path(r'C:\proyectoasis22k26-Compras\codigo\modulos\compras\Compras\CapaVista_Compras')
changes = {'FrmMDICompras.cs': ['ComprasBtnAyuda'], 'FrmMDICompras.Designer.cs': ['ComprasTlpRegistroCompras', 'ComprasTlpDevolucionesCompras']}
for name, controls in changes.items():
    path = root / name
    data = path.read_bytes()
    for control in controls:
        before = (control + '.Enabled = false;').encode()
        assert data.count(before) == 1, control
        data = data.replace(before, (control + '.Enabled = true;').encode())
    path.write_bytes(data)
for name in changes:
    data = (root / name).read_text(encoding='utf-8-sig')
    assert '.Enabled = false;' not in data, name
print('Enabled Compras, Devoluciones de compras, and Ayuda. No disabled controls remain in the main navigation form.')
