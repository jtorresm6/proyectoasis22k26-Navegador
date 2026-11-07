from pathlib import Path
p=Path('CapaVista_OrdenesCompra/FrmDetalleOrdenCompra.cs')
s=p.read_bytes().decode().replace('\r\n','\n')
s=s.replace('            ComprasGbxEncabezado.Text = "Datos de la orden";', '            ComprasUsrEncabezado.Descripcion = "Consulta los datos y productos de la orden de compra. Regresa al listado o anula la orden.";\n            ComprasGbxEncabezado.Text = "Datos de la orden";',1)
s=s.replace('            for (int Indice = 0; Indice < Importes.Length; Indice++)\n                ComprasTlpTotales.Controls.Add(Importes[Indice], Indice % 2, Indice / 2);', '''            for (int Indice = 0; Indice < Importes.Length; Indice++)
            {
                ComprasTlpTotales.SetColumnSpan(Importes[Indice], 1);
                ComprasTlpTotales.SetRowSpan(Importes[Indice], 1);
                Importes[Indice].Dock = DockStyle.Fill;
                ComprasTlpTotales.Controls.Add(Importes[Indice], Indice % 2, Indice / 2);
            }''')
s=s.replace('            ComprasBtnRegresar.Text = "Regresar";', '''            foreach (var Boton in new[] { ComprasBtnRegresar, ComprasBtnDescartar })
            {
                Boton.TipoBoton = ComprasTipoBoton.Libre;
                Boton.BackgroundImage = null;
                Boton.Size = new System.Drawing.Size(150, 44);
                Boton.Anchor = AnchorStyles.None;
            }
            ComprasBtnRegresar.Text = "Regresar";''',1)
s=s.replace('                Tabla.Controls.Add(Campos[Indice], Indice % 4, Indice / 4);','''                Tabla.SetColumnSpan(Campos[Indice], 1);
                Tabla.SetRowSpan(Campos[Indice], 1);
                var Etiqueta = Campos[Indice] as ComprasUsrLabel;
                if (Etiqueta != null) Etiqueta.MostrarAsterisco = false;
                Tabla.Controls.Add(Campos[Indice], Indice % 4, Indice / 4);''',1)
p.write_bytes(s.replace('\n','\r\n').encode())
