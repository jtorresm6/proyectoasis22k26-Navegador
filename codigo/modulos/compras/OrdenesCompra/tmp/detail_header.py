from pathlib import Path
import re
p=Path('CapaVista_OrdenesCompra/FrmDetalleOrdenCompra.Designer.cs')
s=p.read_bytes().decode('utf-8-sig').replace('\r\n','\n')
s=s.replace('this.ComprasUsrEncabezado.Codigo = "6003";', 'this.ComprasUsrEncabezado.Codigo = "6011";')
s=s.replace('this.ComprasUsrEncabezado.Titulo = "ORDENES DE COMPRA";', 'this.ComprasUsrEncabezado.Titulo = "DETALLE DE ORDEN DE COMPRA";')
s=re.sub(r'this\.Text = "6003[^"\n]*";', 'this.Text = "6011 - Detalle de orden de compra";',s)
s=re.sub(r'this\.ComprasUsrEncabezado\.Descripcion = .*?;', 'this.ComprasUsrEncabezado.Descripcion = "Consulta los datos y productos de la orden de compra. Regresa al listado o anula la orden.";',s,count=1,flags=re.S)
s=s.replace('            ComprasUsrEncabezado.Descripcion = "Consulta los datos y productos de la orden de compra. Regresa al listado o anula la orden.";\n','')
p.write_bytes(s.replace('\n','\r\n').encode('utf-8'))
