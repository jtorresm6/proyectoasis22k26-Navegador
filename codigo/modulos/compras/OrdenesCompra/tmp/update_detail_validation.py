from pathlib import Path
p=Path('Validacion/ValidarVistaOrdenesCompra.cs')
b=p.read_bytes(); b=b.replace(b'Codigo.Text == "1" && !Codigo.Enabled && Codigo.MaxLength == 30',b'Codigo.Text == "1" && Codigo.Enabled && Codigo.ReadOnly && Codigo.MaxLength == 30'); p.write_bytes(b)
