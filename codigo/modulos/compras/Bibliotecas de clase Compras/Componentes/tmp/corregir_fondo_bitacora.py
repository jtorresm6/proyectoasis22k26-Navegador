from pathlib import Path
p = Path(__file__).resolve().parents[2] / 'Compras' / 'CapaVista_Compras' / 'FrmBitacoraCompras.cs'
s = p.read_text(encoding='utf-8-sig')
old = '            ComprasMetEncabezadoSinImagen(ComprasEncabezado, true);\n'
if s.count(old) != 1:
    raise RuntimeError('No se encontro la llamada esperada en el constructor de Bitacora.')
s = s.replace(old, '', 1)
p.write_text(s, encoding='utf-8-sig')
