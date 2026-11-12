using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_Facturas;
using CapaModelo_Facturas;
using Microsoft.Reporting.WinForms;

namespace CapaVista_Facturas
{
    public partial class FrmFacturas : Form
    {
        private readonly ClsFacturaOrigenControlador servicio = new ClsFacturaOrigenControlador();
        public int VentasIdUsuario { get; set; }
        private DataTable detalle = ReglasFactura.CrearDetalle();
        private DataTable productos;
        private int clienteId, origenId;
        private bool cargando, emitiendo;
        private FacturaRegistrada ultimaFactura;
        private readonly Timer refresco = new Timer { Interval = 15000 };
        public FrmFacturas()
        {
            InitializeComponent();
            var ayuda=new Button {Name="VentasBtnAyuda",Left=490,Top=640,Width=64,Height=64,
                BackgroundImage=(System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("AyudaVentas"),
                BackgroundImageLayout=ImageLayout.Stretch,FlatStyle=FlatStyle.Flat,Cursor=Cursors.Hand,
                AccessibleName="Ayuda",AccessibleDescription="Guía de Facturas (F1)",TabIndex=200};
            ayuda.FlatAppearance.BorderSize=0;
            ayuda.Click+=(s,e)=>MostrarAyudaFacturas();
            VentasTtpAccionesOriginales.SetToolTip(ayuda,"Ayuda de Facturas (F1)");Controls.Add(ayuda);
            VentasCboTipoDte.Items.Clear(); VentasCboTipoDte.Items.Add("Factura local"); VentasCboTipoDte.SelectedIndex=0; VentasCboTipoDte.Enabled=false;
            VentasTxtEstablecimiento.Text="Embutidos de Calidad, S.A.";
            VentasLblSubtitulo.Text="Facturación con inventario y cuentas por cobrar";
            VentasLblIvaLinea.Text="IVA";
            VentasTxtNitReceptor.CharacterCasing=CharacterCasing.Upper;
            foreach(var campo in new[]{VentasTxtPrecioUnitario,VentasTxtSubtotalLinea,VentasTxtIvaLinea,VentasTxtTotal}) campo.ReadOnly=true;
            VentasRdoNit.CheckedChanged+=(s,e)=>{if(!cargando && origenId==0)VentasTxtNitReceptor.Clear();};
            VentasRdoCodigoCliente.CheckedChanged+=(s,e)=>{if(!cargando && origenId==0)VentasTxtNitReceptor.Clear();};
            VentasDgvItems.ReadOnly=true; VentasDgvItems.AllowUserToAddRows=false; VentasDgvItems.AllowUserToDeleteRows=false;
            VentasDgvItems.SelectionMode=DataGridViewSelectionMode.FullRowSelect; VentasDgvItems.MultiSelect=false;
            VentasDgvItems.DataSource=detalle; VentasDgvItems.Columns["Tasa"].Visible=false;
            VentasDgvItems.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
            VentasDgvItems.Columns["Producto"].FillWeight=220;
            VentasDgvItems.Columns["PrecioUnitario"].HeaderText="Precio unitario";
            foreach(var cbo in new[]{VentasCboCodigoProducto,VentasCboNombreProducto,VentasCboTipoOrigen,VentasCboDocumentoOrigen,VentasCboMoneda,VentasCboCondicionPago}) cbo.DropDownStyle=ComboBoxStyle.DropDownList;
            VentasCboTipoOrigen.SelectedIndexChanged+=(s,e)=>Ejecutar(CambiarOrigen);
            VentasCboDocumentoOrigen.SelectedIndexChanged+=(s,e)=>{if(!cargando && origenId>0) LimpiarDatos();};
            VentasBtnCargarDocumento.Click+=(s,e)=>Ejecutar(CargarOrigen);
            VentasTxtNitReceptor.Leave+=(s,e)=>{if(!cargando && origenId==0) Ejecutar(BuscarCliente);};
            VentasTxtNitReceptor.KeyDown+=(s,e)=>{if(e.KeyCode==Keys.Enter){e.SuppressKeyPress=true;Ejecutar(BuscarCliente);}};
            VentasTxtNitReceptor.TextChanged+=(s,e)=>{if(!cargando && origenId==0){clienteId=0;detalle.Clear();VentasTxtNombreComprador.Clear();VentasTxtDireccionComprador.Clear();VincularProductos(null);Totales();}};
            VentasCboCodigoProducto.SelectedIndexChanged+=(s,e)=>Sincronizar(VentasCboCodigoProducto,VentasCboNombreProducto);
            VentasCboNombreProducto.SelectedIndexChanged+=(s,e)=>Sincronizar(VentasCboNombreProducto,VentasCboCodigoProducto);
            VentasTxtCantidad.TextChanged+=(s,e)=>PrevisualizarLinea(); VentasTxtDescuentos.TextChanged+=(s,e)=>PrevisualizarLinea();
            VentasDgvItems.KeyDown+=(s,e)=>{if((e.KeyCode==Keys.Delete || e.KeyCode==Keys.Back) && VentasDgvItems.CurrentRow!=null && !emitiendo){e.SuppressKeyPress=true;detalle.Rows.RemoveAt(VentasDgvItems.CurrentRow.Index);Totales();}};
            var actualizar=new Button {Text="Actualizar precios",Left=130,Top=645,Width=170,Height=40};
            actualizar.Click+=(s,e)=>Ejecutar(()=>ActualizarPrecios(true)); Controls.Add(actualizar);
            var reimprimir=new Button {Text="Última factura",Left=310,Top=645,Width=160,Height=40};
            reimprimir.Click+=(s,e)=>{if(ultimaFactura!=null) Ejecutar(()=>VistaPrevia(ultimaFactura));};Controls.Add(reimprimir);
            Shown+=(s,e)=>Ejecutar(()=>{Vincular(VentasCboMoneda,servicio.Monedas());VentasCboMoneda.Enabled=false;VentasCboTipoOrigen.SelectedIndex=0;CambiarOrigen();refresco.Start();});
            refresco.Tick+=(s,e)=>{if(!emitiendo && clienteId>0 && origenId==0) Ejecutar(()=>ActualizarPrecios(false));};
            FormClosing+=(s,e)=>{if(emitiendo)e.Cancel=true;else if(detalle.Rows.Count>0)e.Cancel=MessageBox.Show("Hay una factura sin guardar. ¿Desea salir?","Facturas",MessageBoxButtons.YesNo)==DialogResult.No;};
            FormClosed+=(s,e)=>refresco.Dispose();
            LimpiarDatos();
        }
        private void Ejecutar(Action accion)
        {
            try{accion();}catch(Exception ex){MessageBox.Show(this,ex.Message,"Facturas",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
        }
        private static void Vincular(ComboBox combo,DataTable tabla)
        {
            combo.DataSource=null;combo.DisplayMember="Nombre";combo.ValueMember="Id";combo.DataSource=tabla;combo.SelectedIndex=-1;
        }
        private void LimpiarDatos()
        {
            cargando=true;clienteId=0;origenId=0;detalle.Clear();VentasTxtNitReceptor.Clear();VentasTxtNombreComprador.Clear();VentasTxtDireccionComprador.Clear();
            VentasTxtNitReceptor.Enabled=VentasCboTipoOrigen.SelectedIndex<=0;
            VentasCboMoneda.SelectedIndex=-1;VentasCboCondicionPago.SelectedIndex=-1;VincularProductos(null);VentasDtpFechaEmision.Value=DateTime.Now;cargando=false;Totales();
        }
        private void CambiarOrigen()
        {
            if(cargando)return;LimpiarDatos();bool externo=VentasCboTipoOrigen.SelectedIndex>0;
            VentasCboDocumentoOrigen.Enabled=VentasBtnCargarDocumento.Enabled=externo;
            VentasBtnAgregarItem.Enabled=!externo;VentasCboCodigoProducto.Enabled=VentasCboNombreProducto.Enabled=!externo;
            VentasCboDocumentoOrigen.DataSource=null;
            if(externo) Vincular(VentasCboDocumentoOrigen,servicio.VentasFuncOrigenes(VentasCboTipoOrigen.SelectedIndex==2));
        }
        private void BuscarCliente()
        {
            if(origenId>0)return;
            string valor=ReglasFactura.NormalizarIdentificacion(VentasTxtNitReceptor.Text);
            if(valor.Length==0)return;
            if(!VentasRdoCodigoCliente.Checked && !ReglasFactura.IdentificacionValida(valor)) throw new ArgumentException("Ingrese CF, un NIT o CUI válido.");
            var t=servicio.BuscarCliente(valor,VentasRdoCodigoCliente.Checked);
            if(t.Rows.Count!=1)throw new InvalidOperationException("No se encontró un cliente activo único. Revise su identificación o código.");
            var c=t.Rows[0]; int id=Convert.ToInt32(c["Id"]);
            if(clienteId!=id)detalle.Clear();clienteId=id;
            VentasTxtNombreComprador.Text=Convert.ToString(c["Nombre"]);VentasTxtDireccionComprador.Text=Convert.ToString(c["Direccion"]);
            if(c["Lista"]==DBNull.Value)throw new InvalidOperationException("El cliente no tiene una lista de precios vigente asignada.");
            VentasCboMoneda.SelectedValue=Convert.ToInt64(c["Moneda"]);
            if(VentasCboCondicionPago.SelectedIndex<0)VentasCboCondicionPago.SelectedItem=Convert.ToInt32(c["Dias"])>0?"CREDITO":"CONTADO";
            ActualizarPrecios(false);
        }
        private void CargarOrigen()
        {
            if(VentasCboDocumentoOrigen.SelectedIndex<0)throw new ArgumentException("Seleccione un documento.");
            bool pedido=VentasCboTipoOrigen.SelectedIndex==2;int id=Convert.ToInt32(VentasCboDocumentoOrigen.SelectedValue);
            var documento=servicio.VentasFuncCargar(pedido,id);var nuevo=servicio.DetalleOrigen(pedido,id);var h=documento.Tables[0].Rows[0];
            cargando=true;
            try{
                clienteId=Convert.ToInt32(h["ClienteId"]);origenId=id;VentasTxtNitReceptor.Text=Convert.ToString(h["Identificacion"]);
                bool cf=VentasTxtNitReceptor.Text=="CF";VentasTxtNombreComprador.Text=cf?"Consumidor Final":Convert.ToString(h["Cliente"]);VentasTxtDireccionComprador.Text=cf?"Ciudad":Convert.ToString(h["Direccion"]);
                VentasCboMoneda.SelectedValue=Convert.ToInt64(h["MonedaId"]);VentasCboCondicionPago.SelectedItem=Convert.ToInt32(h["DiasCredito"])>0?"CREDITO":"CONTADO";
                detalle.Clear();foreach(DataRow r in nuevo.Rows)detalle.ImportRow(r);
            }finally{cargando=false;}Totales();
        }
        private void VincularProductos(DataTable tabla)
        {
            bool anterior=cargando;cargando=true;productos=tabla;
            VentasCboCodigoProducto.DataSource=null;VentasCboNombreProducto.DataSource=null;
            if(tabla!=null){VentasCboCodigoProducto.DisplayMember="Codigo";VentasCboCodigoProducto.ValueMember="Codigo";VentasCboCodigoProducto.DataSource=tabla.Copy();VentasCboNombreProducto.DisplayMember="Descripcion";VentasCboNombreProducto.ValueMember="Codigo";VentasCboNombreProducto.DataSource=tabla;}
            VentasCboCodigoProducto.SelectedIndex=VentasCboNombreProducto.SelectedIndex=-1;cargando=anterior;
        }
        private void ActualizarPrecios(bool informar)
        {
            if(clienteId<=0 || origenId>0)return;
            var t=servicio.Productos(clienteId);t.Columns.Add("Descripcion",typeof(string));foreach(DataRow r in t.Rows)r["Descripcion"]=r["Producto"]+" | Disponible: "+r["Disponible"];
            if(t.Rows.Count>0) VentasCboMoneda.SelectedValue=Convert.ToInt64(t.Rows[0]["Moneda"]);
            bool cambio=false;
            foreach(DataRow r in detalle.Rows){var p=t.AsEnumerable().SingleOrDefault(x=>Convert.ToInt32(x["Codigo"])==Convert.ToInt32(r["Codigo"]));if(p==null)throw new InvalidOperationException("Un producto dejó de estar disponible en la lista. Elimínelo antes de emitir.");if(!Equals(r["PrecioUnitario"],p["Precio"]) || !Equals(r["Tasa"],p["Tasa"]))cambio=true;r["PrecioUnitario"]=p["Precio"];r["Tasa"]=p["Tasa"];}
            if(detalle.Rows.Count>0)ReglasFactura.Recalcular(detalle);
            int seleccionado=VentasCboCodigoProducto.SelectedIndex>=0?Convert.ToInt32(VentasCboCodigoProducto.SelectedValue):0;
            VincularProductos(t);if(seleccionado>0)VentasCboCodigoProducto.SelectedValue=seleccionado;Totales();
            if(informar || cambio)MessageBox.Show(this,cambio?"Los precios o impuestos cambiaron. Se actualizaron los importes; revíselos antes de emitir.":"Lista de precios y existencias actualizadas.","Facturas");
        }
        private void Sincronizar(ComboBox origen,ComboBox destino)
        {
            if(cargando || origen.SelectedIndex<0)return;cargando=true;destino.SelectedValue=origen.SelectedValue;cargando=false;
            var r=((DataRowView)origen.SelectedItem).Row;VentasTxtPrecioUnitario.Text=Convert.ToDecimal(r["Precio"]).ToString("F4");
            if(string.IsNullOrWhiteSpace(VentasTxtCantidad.Text))VentasTxtCantidad.Text="1";if(string.IsNullOrWhiteSpace(VentasTxtDescuentos.Text))VentasTxtDescuentos.Text="0";PrevisualizarLinea();
        }
        private void PrevisualizarLinea()
        {
            if(cargando || VentasCboCodigoProducto.SelectedIndex<0)return;
            try{decimal cantidad,descuento;if(!decimal.TryParse(VentasTxtCantidad.Text,out cantidad)||!decimal.TryParse(VentasTxtDescuentos.Text,out descuento))return;
                var r=((DataRowView)VentasCboCodigoProducto.SelectedItem).Row;var v=ReglasFactura.Calcular(cantidad,Convert.ToDecimal(r["Precio"]),descuento,Convert.ToDecimal(r["Tasa"]));VentasTxtSubtotalLinea.Text=v[0].ToString("F4");VentasTxtIvaLinea.Text=v[1].ToString("F4");VentasTxtTotal.Text=v[2].ToString("F4");
            }catch(ArgumentException){VentasTxtSubtotalLinea.Clear();VentasTxtIvaLinea.Clear();VentasTxtTotal.Clear();}
        }
        private void VentasBtnAgregarItem_Click(object sender,EventArgs e)
        {
            Ejecutar(()=>{
                if(clienteId<=0 || origenId>0 || VentasCboCodigoProducto.SelectedIndex<0)throw new ArgumentException("Busque un cliente y seleccione un producto.");
                int codigo=Convert.ToInt32(VentasCboCodigoProducto.SelectedValue);ActualizarPrecios(false);
                var p=productos.AsEnumerable().Single(r=>Convert.ToInt32(r["Codigo"])==codigo);
                decimal cantidad,descuento;if(!decimal.TryParse(VentasTxtCantidad.Text,out cantidad)||!decimal.TryParse(VentasTxtDescuentos.Text,out descuento))throw new ArgumentException("Cantidad o descuento inválido.");
                var actual=detalle.AsEnumerable().SingleOrDefault(r=>Convert.ToInt32(r["Codigo"])==codigo);
                if(actual!=null){cantidad+=Convert.ToDecimal(actual["Cantidad"]);descuento+=Convert.ToDecimal(actual["Descuento"]);}
                var valores=ReglasFactura.Calcular(cantidad,Convert.ToDecimal(p["Precio"]),descuento,Convert.ToDecimal(p["Tasa"]));
                if(cantidad>Convert.ToDecimal(p["Disponible"]))throw new ArgumentException("La cantidad supera las existencias disponibles.");
                if(actual!=null)detalle.Rows.Remove(actual);
                detalle.Rows.Add(codigo,p["Producto"],cantidad,p["Precio"],descuento,p["Tasa"],valores[0],valores[1],valores[2]);Totales();
                VentasTxtCantidad.Text="1";VentasTxtDescuentos.Text="0";
            });
        }
        private void Totales()
        {
            string moneda=VentasCboMoneda.SelectedItem is DataRowView ? Convert.ToString(((DataRowView)VentasCboMoneda.SelectedItem)["Codigo"]):"";
            decimal sub=0,iva=0,total=0;foreach(DataRow r in detalle.Rows){sub+=Convert.ToDecimal(r["Subtotal"]);iva+=Convert.ToDecimal(r["Impuesto"]);total+=Convert.ToDecimal(r["Total"]);}
            VentasLblSubtotalResumen.Text="Subtotal: "+moneda+" "+sub.ToString("N4");VentasLblImpuestosResumen.Text="IVA: "+moneda+" "+iva.ToString("N4");VentasLblTotalGeneralResumen.Text="TOTAL: "+moneda+" "+total.ToString("N4");
        }
        private void VentasTxtNitReceptor_KeyPress(object sender,KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && e.KeyChar!='-')e.Handled=true;
        }
        private void VentasTxtCantidad_KeyPress(object sender,KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))e.Handled=true;
        }
        private SolicitudFactura ConfigurarEmision()
        {
            var s=new SolicitudFactura {Cliente=clienteId,Usuario=VentasIdUsuario,OrigenId=origenId,Pedido=VentasCboTipoOrigen.SelectedIndex==2,Moneda=Convert.ToInt64(VentasCboMoneda.SelectedValue),Condicion=VentasCboCondicionPago.Text,Detalle=detalle.Copy()};
            var empresas=servicio.Empresas().AsEnumerable().Where(r=>string.Equals(Convert.ToString(r["Nombre"]).Trim(),"Embutidos de Calidad, S.A.",StringComparison.OrdinalIgnoreCase)).ToArray();
            if(empresas.Length!=1)throw new InvalidOperationException("Configure en tblEmpresa los datos fiscales de Embutidos de Calidad, S.A. antes de emitir.");
            s.Empresa=Convert.ToInt32(empresas[0]["Id"]);
            var sucursales=servicio.VentasFuncSucursales();
            if(sucursales.Rows.Count==0)throw new InvalidOperationException("No hay sucursales configuradas.");
            s.Sucursal=Convert.ToInt32(sucursales.Rows[0]["Id"]);
            var efectivo=servicio.FormasPago().AsEnumerable().FirstOrDefault(r=>!Convert.ToBoolean(r["RequiereReferencia"]) && Convert.ToString(r["Nombre"]).IndexOf("Efectivo",StringComparison.OrdinalIgnoreCase)>=0);
            if(s.Condicion=="CONTADO" && efectivo==null)throw new InvalidOperationException("Configure una forma de pago Efectivo sin referencia.");
            s.FormaPago=efectivo==null?0:Convert.ToInt32(efectivo["Id"]);s.ReferenciaPago=null;            s.Distribucion=new DataTable();s.Distribucion.Columns.Add("ProductoId",typeof(int));s.Distribucion.Columns.Add("StockId",typeof(int));s.Distribucion.Columns.Add("Cantidad",typeof(decimal));
            foreach(var grupo in detalle.AsEnumerable().GroupBy(r=>Convert.ToInt32(r["Codigo"])))
            {
                decimal requerido=grupo.Sum(r=>Convert.ToDecimal(r["Cantidad"]));var t=servicio.Existencias(grupo.Key);t.Columns.Add("Retirar",typeof(int));decimal restante=requerido;
                foreach(DataRow r in t.Rows){int retirar=(int)Math.Min(restante,Convert.ToDecimal(r["Disponible"]));r["Retirar"]=retirar;restante-=retirar;}
                if(restante>0)throw new InvalidOperationException("Stock insuficiente para "+grupo.First()["Producto"]);
                foreach(DataRow r in t.Rows)if(Convert.ToInt32(r["Retirar"])>0)s.Distribucion.Rows.Add(grupo.Key,r["StockId"],r["Retirar"]);
            }
            return s;
        }
        private void VentasBtnEmitirFactura_Click(object sender,EventArgs e)
        {
            if(emitiendo)return;Ejecutar(()=>{
                if(clienteId<=0||VentasCboMoneda.SelectedIndex<0||VentasCboCondicionPago.SelectedIndex<0)throw new ArgumentException("Complete cliente, moneda y condición de pago.");
                if(VentasCboTipoOrigen.SelectedIndex>0 && origenId==0)throw new ArgumentException("Cargue el documento de origen.");
                ReglasFactura.Recalcular(detalle);emitiendo=true;VentasBtnEmitirFactura.Enabled=false;
                try{
                    ActualizarPrecios(false);
                    bool imprimir;
                    using(var opciones=new Form {Text="Salida de factura",ClientSize=new Size(410,145),StartPosition=FormStartPosition.CenterParent,FormBorderStyle=FormBorderStyle.FixedDialog,MaximizeBox=false,MinimizeBox=false}){
                        opciones.Controls.Add(new Label{Text="Guardar PDF y registrar factura",Left=20,Top=20,Width=370});
                        opciones.Controls.Add(new Button{Text="PDF",Left=20,Top=70,Width=160,Height=40,DialogResult=DialogResult.No});opciones.Controls.Add(new Button{Text="PDF e Imprimir",Left=210,Top=70,Width=180,Height=40,DialogResult=DialogResult.Yes});var resultado=opciones.ShowDialog(this);if(resultado!=DialogResult.No && resultado!=DialogResult.Yes)return;imprimir=resultado==DialogResult.Yes;
                    }
                    var solicitud=ConfigurarEmision();if(solicitud==null)return;
                    string ruta;
                    using(var carpeta=new FolderBrowserDialog {Description="Seleccione la carpeta para guardar el PDF con el UUID de autorización local."}){if(carpeta.ShowDialog(this)!=DialogResult.OK)return;ruta=Path.Combine(carpeta.SelectedPath,solicitud.Uuid+".pdf");}
                    bool pdfEscrito=false;
                    try{
                        ultimaFactura=servicio.Registrar(solicitud,f=>{using(var report=new LocalReport()){ReporteFactura.Configurar(report,f);byte[] bytes=report.Render("PDF");using(var archivo=new FileStream(ruta,FileMode.CreateNew,FileAccess.Write,FileShare.None)){archivo.Write(bytes,0,bytes.Length);archivo.Flush(true);}pdfEscrito=true;}});
                    }catch(Exception ex){throw new InvalidOperationException(ex.Message+(pdfEscrito?"\nEl PDF se escribió, pero no se confirmó el registro. Conserve el UUID "+solicitud.Uuid+" para verificarlo antes de repetir la emisión.":"\nLa operación no fue confirmada."),ex);}
                    LimpiarDatos();CambiarOrigen();MessageBox.Show(this,"Factura registrada y PDF guardado:\n"+ruta,"Emisión exitosa");if(imprimir)VistaPrevia(ultimaFactura);
                }finally{emitiendo=false;VentasBtnEmitirFactura.Enabled=true;}
            });
        }
        private void VistaPrevia(FacturaRegistrada factura)
        {
            using(var ventana=new Form {Text="Factura "+factura.Numero,Width=1100,Height=800,StartPosition=FormStartPosition.CenterParent})
            using(var viewer=new ReportViewer {Dock=DockStyle.Fill,ProcessingMode=ProcessingMode.Local,ShowPrintButton=true,ShowPageNavigationControls=true,ShowZoomControl=true})
            {ventana.Controls.Add(viewer);ReporteFactura.Configurar(viewer.LocalReport,factura);viewer.SetDisplayMode(DisplayMode.PrintLayout);viewer.ZoomMode=ZoomMode.PageWidth;viewer.RefreshReport();ventana.ShowDialog(this);}
        }
    }
    public static class ReporteFactura
    {
        public static void Configurar(LocalReport reporte,FacturaRegistrada f)
        {
            using(var stream=typeof(FrmFacturas).Assembly.GetManifestResourceStream("CapaVista_Facturas.rptFactura.rdlc"))reporte.LoadReportDefinition(stream);
            var t=f.Detalle.Copy();foreach(string columna in new[]{"NitCliente","NombreCliente","DireccionCliente"})t.Columns.Add(columna,typeof(string));
            foreach(DataRow r in t.Rows){r["NitCliente"]=f.Identificacion;r["NombreCliente"]=f.Cliente;r["DireccionCliente"]=f.Direccion;}
            reporte.DataSources.Clear();reporte.DataSources.Add(new ReportDataSource("dsDetalle",t));
            reporte.SetParameters(new[]{new ReportParameter("Autorizacion",f.Uuid),new ReportParameter("Serie",f.Serie),new ReportParameter("Numero",f.Numero),new ReportParameter("Moneda",f.Moneda),new ReportParameter("Fecha",f.Fecha.ToString("dd/MM/yyyy HH:mm:ss")),new ReportParameter("Empresa",f.Empresa),new ReportParameter("NitEmisor",f.NitEmisor),new ReportParameter("DireccionEmisor",f.DireccionEmisor),new ReportParameter("Condicion",f.Condicion)});
        }
    }
}
