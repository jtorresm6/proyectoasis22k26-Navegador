using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_Facturas;
using Microsoft.Reporting.WinForms;

namespace CapaVista_Facturas
{
    public partial class FrmFacturas : Form
    {
        private readonly ClsFacturaOrigenControlador _Servicio = new ClsFacturaOrigenControlador();
        public int VentasIdUsuario { get; set; }
        private DataTable _Detalle = ClsReglasFactura.VentasFuncCrearDetalle();
        private DataTable _Productos;
        private int _ClienteId, _OrigenId;
        private bool _Cargando, _Emitiendo;
        private ClsFacturaRegistrada _UltimaFactura;
        private readonly Label _VentasLblDescuentoResumen = new Label();
        private readonly Timer _Refresco = new Timer { Interval = 15000 };
        public FrmFacturas()
        {
            InitializeComponent();
            _VentasPnlResumen.Height=122;
            _VentasLblDescuentoResumen.Name="VentasLblDescuentoResumen";
            _VentasLblDescuentoResumen.SetBounds(15,8,320,22);
            _VentasLblDescuentoResumen.Font=_VentasLblSubtotalResumen.Font;
            _VentasPnlResumen.Controls.Add(_VentasLblDescuentoResumen);
            _VentasLblSubtotalResumen.Top=36; _VentasLblImpuestosResumen.Top=62; _VentasLblTotalGeneralResumen.Top=90;
            var Ayuda=new Button {Name="VentasBtnAyuda",Left=490,Top=640,Width=64,Height=64,
                BackgroundImage=(System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("AyudaVentas"),
                BackgroundImageLayout=ImageLayout.Stretch,FlatStyle=FlatStyle.Flat,Cursor=Cursors.Hand,
                AccessibleName="Ayuda",AccessibleDescription="Guía de Facturas (F1)",TabIndex=200};
            Ayuda.FlatAppearance.BorderSize=0;
            Ayuda.Click+=(Solicitud,Evento)=>VentasMetMostrarAyudaFacturas();
            _VentasTtpAccionesOriginales.SetToolTip(Ayuda,"Ayuda de Facturas (F1)");Controls.Add(Ayuda);
            _VentasCboTipoDte.Items.Clear(); _VentasCboTipoDte.Items.Add("Factura local"); _VentasCboTipoDte.SelectedIndex=0; _VentasCboTipoDte.Enabled=false;
            _VentasTxtEstablecimiento.Text="Embutidos de Calidad, S.A.";
            _VentasLblSubtitulo.Text="Facturación con inventario y cuentas por cobrar";
            _VentasLblIvaLinea.Text="IVA";
            _VentasTxtNitReceptor.CharacterCasing=CharacterCasing.Upper;
            foreach(var Campo in new[]{_VentasTxtPrecioUnitario,_VentasTxtSubtotalLinea,_VentasTxtIvaLinea,_VentasTxtTotal}) Campo.ReadOnly=true;
            _VentasRdoNit.CheckedChanged+=(Solicitud,Evento)=>{if(!_Cargando && _OrigenId==0)_VentasTxtNitReceptor.Clear();};
            _VentasRdoCodigoCliente.CheckedChanged+=(Solicitud,Evento)=>{if(!_Cargando && _OrigenId==0)_VentasTxtNitReceptor.Clear();};
            _VentasDgvItems.ReadOnly=true; _VentasDgvItems.AllowUserToAddRows=false; _VentasDgvItems.AllowUserToDeleteRows=false;
            _VentasDgvItems.SelectionMode=DataGridViewSelectionMode.FullRowSelect; _VentasDgvItems.MultiSelect=false;
            _VentasDgvItems.DataSource=_Detalle; _VentasDgvItems.Columns["Tasa"].Visible=false;
            _VentasDgvItems.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
            _VentasDgvItems.Columns["Producto"].FillWeight=220;
            _VentasDgvItems.Columns["PrecioUnitario"].HeaderText="Precio unitario";
            foreach(var Cbo in new[]{_VentasCboCodigoProducto,_VentasCboNombreProducto,_VentasCboTipoOrigen,_VentasCboDocumentoOrigen,_VentasCboMoneda,_VentasCboCondicionPago}) Cbo.DropDownStyle=ComboBoxStyle.DropDownList;
            _VentasCboTipoOrigen.SelectedIndexChanged+=(Solicitud,Evento)=>VentasMetEjecutar(VentasMetCambiarOrigen);
            _VentasCboDocumentoOrigen.SelectedIndexChanged+=(Solicitud,Evento)=>{if(!_Cargando && _OrigenId>0) VentasMetLimpiarDatos();};
            _VentasBtnCargarDocumento.Click+=(Solicitud,Evento)=>VentasMetEjecutar(VentasMetCargarOrigen);
            _VentasTxtNitReceptor.Leave+=(Solicitud,Evento)=>{if(!_Cargando && _OrigenId==0) VentasMetEjecutar(VentasMetBuscarCliente);};
            _VentasTxtNitReceptor.KeyDown+=(Solicitud,Evento)=>{if(Evento.KeyCode==Keys.Enter){Evento.SuppressKeyPress=true;VentasMetEjecutar(VentasMetBuscarCliente);}};
            _VentasTxtNitReceptor.TextChanged+=(Solicitud,Evento)=>{if(!_Cargando && _OrigenId==0){_ClienteId=0;_Detalle.Clear();_VentasTxtNombreComprador.Clear();_VentasTxtDireccionComprador.Clear();VentasMetVincularProductos(null);VentasMetTotales();}};
            _VentasCboCodigoProducto.SelectedIndexChanged+=(Solicitud,Evento)=>VentasMetSincronizar(_VentasCboCodigoProducto,_VentasCboNombreProducto);
            _VentasCboNombreProducto.SelectedIndexChanged+=(Solicitud,Evento)=>VentasMetSincronizar(_VentasCboNombreProducto,_VentasCboCodigoProducto);
            _VentasTxtCantidad.TextChanged+=(Solicitud,Evento)=>VentasMetPrevisualizarLinea(); _VentasTxtDescuentos.TextChanged+=(Solicitud,Evento)=>VentasMetPrevisualizarLinea();
            _VentasDgvItems.KeyDown+=(Solicitud,Evento)=>{if((Evento.KeyCode==Keys.Delete || Evento.KeyCode==Keys.Back) && _VentasDgvItems.CurrentRow!=null && !_Emitiendo){Evento.SuppressKeyPress=true;_Detalle.Rows.RemoveAt(_VentasDgvItems.CurrentRow.Index);VentasMetTotales();}};
            var Actualizar=new Button {Text="Actualizar precios",Left=130,Top=645,Width=170,Height=40};
            Actualizar.Click+=(Solicitud,Evento)=>VentasMetEjecutar(()=>VentasMetActualizarPrecios(true)); Controls.Add(Actualizar);
            var Reimprimir=new Button {Text="Última factura",Left=310,Top=645,Width=160,Height=40};
            Reimprimir.Click+=(Solicitud,Evento)=>{if(_UltimaFactura!=null) VentasMetEjecutar(()=>VentasMetVistaPrevia(_UltimaFactura));};Controls.Add(Reimprimir);
            Shown+=(Solicitud,Evento)=>VentasMetEjecutar(()=>{VentasMetVincular(_VentasCboMoneda,_Servicio.VentasFuncMonedas());_VentasCboMoneda.Enabled=false;_VentasCboTipoOrigen.SelectedIndex=0;VentasMetCambiarOrigen();_Refresco.Start();});
            _Refresco.Tick+=(Solicitud,Evento)=>{if(!_Emitiendo && _ClienteId>0 && _OrigenId==0) VentasMetEjecutar(()=>VentasMetActualizarPrecios(false));};
            FormClosing+=(Solicitud,Evento)=>{if(_Emitiendo)Evento.Cancel=true;else if(_Detalle.Rows.Count>0)Evento.Cancel=MessageBox.Show("Hay una factura sin guardar. ¿Desea salir?","Facturas",MessageBoxButtons.YesNo)==DialogResult.No;};
            FormClosed+=(Solicitud,Evento)=>_Refresco.Dispose();
            VentasMetLimpiarDatos();
        }
        private void VentasMetEjecutar(Action Accion)
        {
            try{Accion();}catch(Exception Ex){MessageBox.Show(this,Ex.Message,"Facturas",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
        }
        private static void VentasMetVincular(ComboBox Combo,DataTable Tabla)
        {
            Combo.DataSource=null;Combo.DisplayMember="Nombre";Combo.ValueMember="Id";Combo.DataSource=Tabla;Combo.SelectedIndex=-1;
        }
        private void VentasMetLimpiarDatos()
        {
            _Cargando=true;_ClienteId=0;_OrigenId=0;_Detalle.Clear();_VentasTxtNitReceptor.Clear();_VentasTxtNombreComprador.Clear();_VentasTxtDireccionComprador.Clear();
            _VentasTxtNitReceptor.Enabled=_VentasCboTipoOrigen.SelectedIndex<=0;
            _VentasCboMoneda.SelectedIndex=-1;_VentasCboCondicionPago.SelectedIndex=-1;VentasMetVincularProductos(null);_VentasDtpFechaEmision.Value=DateTime.Now;_Cargando=false;VentasMetTotales();
        }
        private void VentasMetCambiarOrigen()
        {
            if(_Cargando)return;VentasMetLimpiarDatos();bool Externo=_VentasCboTipoOrigen.SelectedIndex>0;
            _VentasCboDocumentoOrigen.Enabled=_VentasBtnCargarDocumento.Enabled=Externo;
            _VentasBtnAgregarItem.Enabled=!Externo;_VentasCboCodigoProducto.Enabled=_VentasCboNombreProducto.Enabled=!Externo;
            _VentasCboDocumentoOrigen.DataSource=null;
            if(Externo) VentasMetVincular(_VentasCboDocumentoOrigen,_Servicio.VentasFuncOrigenes(_VentasCboTipoOrigen.SelectedIndex==2));
        }
        private void VentasMetBuscarCliente()
        {
            if(_OrigenId>0)return;
            string Valor=ClsReglasFactura.VentasFuncNormalizarIdentificacion(_VentasTxtNitReceptor.Text);
            if(Valor.Length==0)return;
            if(!_VentasRdoCodigoCliente.Checked && !ClsReglasFactura.VentasFuncIdentificacionValida(Valor)) throw new ArgumentException("Ingrese CF, un NIT o CUI válido.");
            var Tabla=_Servicio.VentasFuncBuscarCliente(Valor,_VentasRdoCodigoCliente.Checked);
            if(Tabla.Rows.Count!=1)throw new InvalidOperationException("No se encontró un cliente activo único. Revise su identificación o código.");
            var Cliente=Tabla.Rows[0]; int Id=Convert.ToInt32(Cliente["Id"]);
            if(_ClienteId!=Id)_Detalle.Clear();_ClienteId=Id;
            _VentasTxtNombreComprador.Text=Convert.ToString(Cliente["Nombre"]);_VentasTxtDireccionComprador.Text=Convert.ToString(Cliente["Direccion"]);
            if(Cliente["Lista"]==DBNull.Value)throw new InvalidOperationException("El cliente no tiene una lista de precios vigente asignada.");
            _VentasCboMoneda.SelectedValue=Convert.ToInt64(Cliente["Moneda"]);
            if(_VentasCboCondicionPago.SelectedIndex<0)_VentasCboCondicionPago.SelectedItem=Convert.ToInt32(Cliente["Dias"])>0?"CREDITO":"CONTADO";
            VentasMetActualizarPrecios(false);
        }
        private void VentasMetCargarOrigen()
        {
            if(_VentasCboDocumentoOrigen.SelectedIndex<0)throw new ArgumentException("Seleccione un documento.");
            bool Pedido=_VentasCboTipoOrigen.SelectedIndex==2;int Id=Convert.ToInt32(_VentasCboDocumentoOrigen.SelectedValue);
            var Documento=_Servicio.VentasFuncCargar(Pedido,Id);var Nuevo=_Servicio.VentasFuncDetalleOrigen(Pedido,Id);var Encabezado=Documento.Tables[0].Rows[0];
            _Cargando=true;
            try{
                _ClienteId=Convert.ToInt32(Encabezado["ClienteId"]);_OrigenId=Id;_VentasTxtNitReceptor.Text=Convert.ToString(Encabezado["Identificacion"]);
                bool Cf=_VentasTxtNitReceptor.Text=="CF";_VentasTxtNombreComprador.Text=Cf?"Consumidor Final":Convert.ToString(Encabezado["Cliente"]);_VentasTxtDireccionComprador.Text=Cf?"Ciudad":Convert.ToString(Encabezado["Direccion"]);
                _VentasCboMoneda.SelectedValue=Convert.ToInt64(Encabezado["MonedaId"]);_VentasCboCondicionPago.SelectedItem=Convert.ToInt32(Encabezado["DiasCredito"])>0?"CREDITO":"CONTADO";
                _Detalle.Clear();foreach(DataRow Fila in Nuevo.Rows)_Detalle.ImportRow(Fila);
            }finally{_Cargando=false;}VentasMetTotales();
        }
        private void VentasMetVincularProductos(DataTable Tabla)
        {
            bool Anterior=_Cargando;_Cargando=true;_Productos=Tabla;
            _VentasCboCodigoProducto.DataSource=null;_VentasCboNombreProducto.DataSource=null;
            if(Tabla!=null){_VentasCboCodigoProducto.DisplayMember="Codigo";_VentasCboCodigoProducto.ValueMember="Codigo";_VentasCboCodigoProducto.DataSource=Tabla.Copy();_VentasCboNombreProducto.DisplayMember="Descripcion";_VentasCboNombreProducto.ValueMember="Codigo";_VentasCboNombreProducto.DataSource=Tabla;}
            _VentasCboCodigoProducto.SelectedIndex=_VentasCboNombreProducto.SelectedIndex=-1;_Cargando=Anterior;
        }
        private void VentasMetActualizarPrecios(bool Informar)
        {
            if(_ClienteId<=0 || _OrigenId>0)return;
            var Tabla=_Servicio.VentasFuncProductos(_ClienteId);Tabla.Columns.Add("Descripcion",typeof(string));foreach(DataRow Fila in Tabla.Rows)Fila["Descripcion"]=Fila["Producto"]+" | Disponible: "+Fila["Disponible"];
            if(Tabla.Rows.Count>0) _VentasCboMoneda.SelectedValue=Convert.ToInt64(Tabla.Rows[0]["Moneda"]);
            bool Cambio=false;
            foreach(DataRow Fila in _Detalle.Rows){var Producto=Tabla.AsEnumerable().SingleOrDefault(X=>Convert.ToInt32(X["Codigo"])==Convert.ToInt32(Fila["Codigo"]));if(Producto==null)throw new InvalidOperationException("Un producto dejó de estar disponible en la lista. Elimínelo antes de emitir.");if(!Equals(Fila["PrecioUnitario"],Producto["Precio"]) || !Equals(Fila["Tasa"],Producto["Tasa"]))Cambio=true;Fila["PrecioUnitario"]=Producto["Precio"];Fila["Tasa"]=Producto["Tasa"];}
            if(_Detalle.Rows.Count>0)ClsReglasFactura.VentasMetRecalcular(_Detalle);
            int Seleccionado=_VentasCboCodigoProducto.SelectedIndex>=0?Convert.ToInt32(_VentasCboCodigoProducto.SelectedValue):0;
            VentasMetVincularProductos(Tabla);if(Seleccionado>0)_VentasCboCodigoProducto.SelectedValue=Seleccionado;VentasMetTotales();
            if(Informar || Cambio)MessageBox.Show(this,Cambio?"Los precios o impuestos cambiaron. Se actualizaron los importes; revíselos antes de emitir.":"Lista de precios y existencias actualizadas.","Facturas");
        }
        private void VentasMetSincronizar(ComboBox Origen,ComboBox Destino)
        {
            if(_Cargando || Origen.SelectedIndex<0)return;_Cargando=true;Destino.SelectedValue=Origen.SelectedValue;_Cargando=false;
            var Fila=((DataRowView)Origen.SelectedItem).Row;_VentasTxtPrecioUnitario.Text=Convert.ToDecimal(Fila["Precio"]).ToString("F4");
            if(string.IsNullOrWhiteSpace(_VentasTxtCantidad.Text))_VentasTxtCantidad.Text="1";if(string.IsNullOrWhiteSpace(_VentasTxtDescuentos.Text))_VentasTxtDescuentos.Text="0";VentasMetPrevisualizarLinea();
        }
        private void VentasMetPrevisualizarLinea()
        {
            if(_Cargando || _VentasCboCodigoProducto.SelectedIndex<0)return;
            try{decimal Cantidad,Descuento;if(!decimal.TryParse(_VentasTxtCantidad.Text,out Cantidad)||!decimal.TryParse(_VentasTxtDescuentos.Text,out Descuento))return;
                var Fila=((DataRowView)_VentasCboCodigoProducto.SelectedItem).Row;var Importes=ClsReglasFactura.VentasFuncCalcular(Cantidad,Convert.ToDecimal(Fila["Precio"]),Descuento,Convert.ToDecimal(Fila["Tasa"]));_VentasTxtSubtotalLinea.Text=Importes[0].ToString("F4");_VentasTxtIvaLinea.Text=Importes[1].ToString("F4");_VentasTxtTotal.Text=Importes[2].ToString("F4");
            }catch(ArgumentException){_VentasTxtSubtotalLinea.Clear();_VentasTxtIvaLinea.Clear();_VentasTxtTotal.Clear();}
        }
        private void VentasMetBtnAgregarItemClick(object Emisor,EventArgs Evento)
        {
            VentasMetEjecutar(()=>{
                if(_ClienteId<=0 || _OrigenId>0 || _VentasCboCodigoProducto.SelectedIndex<0)throw new ArgumentException("Busque un cliente y seleccione un producto.");
                int Codigo=Convert.ToInt32(_VentasCboCodigoProducto.SelectedValue);VentasMetActualizarPrecios(false);
                var Producto=_Productos.AsEnumerable().Single(Fila=>Convert.ToInt32(Fila["Codigo"])==Codigo);
                decimal Cantidad,Descuento;if(!decimal.TryParse(_VentasTxtCantidad.Text,out Cantidad)||!decimal.TryParse(_VentasTxtDescuentos.Text,out Descuento))throw new ArgumentException("Cantidad o descuento inválido.");
                var Actual=_Detalle.AsEnumerable().SingleOrDefault(Fila=>Convert.ToInt32(Fila["Codigo"])==Codigo);
                if(Actual!=null){Cantidad+=Convert.ToDecimal(Actual["Cantidad"]);Descuento+=Convert.ToDecimal(Actual["Descuento"]);}
                var Valores=ClsReglasFactura.VentasFuncCalcular(Cantidad,Convert.ToDecimal(Producto["Precio"]),Descuento,Convert.ToDecimal(Producto["Tasa"]));
                if(Cantidad>Convert.ToDecimal(Producto["Disponible"]))throw new ArgumentException("La cantidad supera las existencias disponibles.");
                if(Actual!=null)_Detalle.Rows.Remove(Actual);
                _Detalle.Rows.Add(Codigo,Producto["Producto"],Cantidad,Producto["Precio"],Descuento,Producto["Tasa"],Valores[0],Valores[1],Valores[2]);VentasMetTotales();
                _VentasTxtCantidad.Text="1";_VentasTxtDescuentos.Text="0";
            });
        }
        private void VentasMetTotales()
        {
            string Moneda=_VentasCboMoneda.SelectedItem is DataRowView ? Convert.ToString(((DataRowView)_VentasCboMoneda.SelectedItem)["Codigo"]):"";
            decimal Sub=0,Impuesto=0,Total=0;foreach(DataRow Fila in _Detalle.Rows){Sub+=Convert.ToDecimal(Fila["Subtotal"]);Impuesto+=Convert.ToDecimal(Fila["Impuesto"]);Total+=Convert.ToDecimal(Fila["Total"]);}
            _VentasLblDescuentoResumen.Text="Descuento incluido: "+Moneda+" "+_Detalle.AsEnumerable().Sum(Fila=>Convert.ToDecimal(Fila["Descuento"])).ToString("N4");
            _VentasLblSubtotalResumen.Text="Subtotal: "+Moneda+" "+Sub.ToString("N4");_VentasLblImpuestosResumen.Text="IVA: "+Moneda+" "+Impuesto.ToString("N4");_VentasLblTotalGeneralResumen.Text="TOTAL: "+Moneda+" "+Total.ToString("N4");
        }
        private void VentasMetTxtNitReceptorKeyPress(object Emisor,KeyPressEventArgs Evento)
        {
            if(!char.IsControl(Evento.KeyChar) && !char.IsLetterOrDigit(Evento.KeyChar) && Evento.KeyChar!='-')Evento.Handled=true;
        }
        private void VentasMetTxtCantidadKeyPress(object Emisor,KeyPressEventArgs Evento)
        {
            if(!char.IsControl(Evento.KeyChar) && !char.IsDigit(Evento.KeyChar))Evento.Handled=true;
        }
        private ClsSolicitudFactura VentasFuncConfigurarEmision()
        {
            var Solicitud=new ClsSolicitudFactura {Cliente=_ClienteId,Usuario=VentasIdUsuario,OrigenId=_OrigenId,Pedido=_VentasCboTipoOrigen.SelectedIndex==2,Moneda=Convert.ToInt64(_VentasCboMoneda.SelectedValue),Condicion=_VentasCboCondicionPago.Text,Detalle=_Detalle.Copy()};
            var Empresas=_Servicio.VentasFuncEmpresas().AsEnumerable().Where(Fila=>string.Equals(Convert.ToString(Fila["Nombre"]).Trim(),"Embutidos de Calidad, S.A.",StringComparison.OrdinalIgnoreCase)).ToArray();
            if(Empresas.Length!=1)throw new InvalidOperationException("Configure en tblEmpresa los datos fiscales de Embutidos de Calidad, S.A. antes de emitir.");
            Solicitud.Empresa=Convert.ToInt32(Empresas[0]["Id"]);
            var Sucursales=_Servicio.VentasFuncSucursales();
            if(Sucursales.Rows.Count==0)throw new InvalidOperationException("No hay sucursales configuradas.");
            Solicitud.Sucursal=Convert.ToInt32(Sucursales.Rows[0]["Id"]);
            var Efectivo=_Servicio.VentasFuncFormasPago().AsEnumerable().FirstOrDefault(Fila=>!Convert.ToBoolean(Fila["RequiereReferencia"]) && Convert.ToString(Fila["Nombre"]).IndexOf("Efectivo",StringComparison.OrdinalIgnoreCase)>=0);
            if(Solicitud.Condicion=="CONTADO" && Efectivo==null)throw new InvalidOperationException("Configure una forma de pago Efectivo sin referencia.");
            Solicitud.FormaPago=Efectivo==null?0:Convert.ToInt32(Efectivo["Id"]);Solicitud.ReferenciaPago=null;            Solicitud.Distribucion=new DataTable();Solicitud.Distribucion.Columns.Add("ProductoId",typeof(int));Solicitud.Distribucion.Columns.Add("StockId",typeof(int));Solicitud.Distribucion.Columns.Add("Cantidad",typeof(decimal));
            foreach(var Grupo in _Detalle.AsEnumerable().GroupBy(Fila=>Convert.ToInt32(Fila["Codigo"])))
            {
                decimal Requerido=Grupo.Sum(Fila=>Convert.ToDecimal(Fila["Cantidad"]));var Tabla=_Servicio.VentasFuncExistencias(Grupo.Key);Tabla.Columns.Add("Retirar",typeof(int));decimal Restante=Requerido;
                foreach(DataRow Fila in Tabla.Rows){int Retirar=(int)Math.Min(Restante,Convert.ToDecimal(Fila["Disponible"]));Fila["Retirar"]=Retirar;Restante-=Retirar;}
                if(Restante>0)throw new InvalidOperationException("Stock insuficiente para "+Grupo.First()["Producto"]);
                foreach(DataRow Fila in Tabla.Rows)if(Convert.ToInt32(Fila["Retirar"])>0)Solicitud.Distribucion.Rows.Add(Grupo.Key,Fila["StockId"],Fila["Retirar"]);
            }
            return Solicitud;
        }
        private void VentasMetBtnEmitirFacturaClick(object Emisor,EventArgs Evento)
        {
            if(_Emitiendo)return;VentasMetEjecutar(()=>{
                if(_ClienteId<=0||_VentasCboMoneda.SelectedIndex<0||_VentasCboCondicionPago.SelectedIndex<0)throw new ArgumentException("Complete cliente, moneda y condición de pago.");
                if(_VentasCboTipoOrigen.SelectedIndex>0 && _OrigenId==0)throw new ArgumentException("Cargue el documento de origen.");
                ClsReglasFactura.VentasMetRecalcular(_Detalle);_Emitiendo=true;_VentasBtnEmitirFactura.Enabled=false;
                try{
                    VentasMetActualizarPrecios(false);
                    bool Imprimir;
                    using(var Opciones=new Form {Text="Salida de factura",ClientSize=new Size(410,145),StartPosition=FormStartPosition.CenterParent,FormBorderStyle=FormBorderStyle.FixedDialog,MaximizeBox=false,MinimizeBox=false}){
                        Opciones.Controls.Add(new Label{Text="Guardar PDF y registrar factura",Left=20,Top=20,Width=370});
                        Opciones.Controls.Add(new Button{Text="PDF",Left=20,Top=70,Width=160,Height=40,DialogResult=DialogResult.No});Opciones.Controls.Add(new Button{Text="PDF e Imprimir",Left=210,Top=70,Width=180,Height=40,DialogResult=DialogResult.Yes});var Resultado=Opciones.ShowDialog(this);if(Resultado!=DialogResult.No && Resultado!=DialogResult.Yes)return;Imprimir=Resultado==DialogResult.Yes;
                    }
                    var Solicitud=VentasFuncConfigurarEmision();if(Solicitud==null)return;
                    string Ruta;
                    using(var Carpeta=new FolderBrowserDialog {Description="Seleccione la carpeta para guardar el PDF con el UUID de autorización local."}){if(Carpeta.ShowDialog(this)!=DialogResult.OK)return;Ruta=Path.Combine(Carpeta.SelectedPath,Solicitud.Uuid+".pdf");}
                    bool PdfEscrito=false;
                    try{
                        _UltimaFactura=_Servicio.VentasFuncRegistrar(Solicitud,Factura=>{using(var Report=new LocalReport()){ClsReporteFactura.VentasMetConfigurar(Report,Factura);byte[] ContenidoPdf=Report.Render("PDF");using(var Archivo=new FileStream(Ruta,FileMode.CreateNew,FileAccess.Write,FileShare.None)){Archivo.Write(ContenidoPdf,0,ContenidoPdf.Length);Archivo.Flush(true);}PdfEscrito=true;}});
                    }catch(Exception Ex){throw new InvalidOperationException(Ex.Message+(PdfEscrito?"\nEl PDF se escribió, pero no se confirmó el registro. Conserve el UUID "+Solicitud.Uuid+" para verificarlo antes de repetir la emisión.":"\nLa operación no fue confirmada."),Ex);}
                    VentasMetLimpiarDatos();VentasMetCambiarOrigen();MessageBox.Show(this,"Factura registrada y PDF guardado:\n"+Ruta,"Emisión exitosa");if(Imprimir)VentasMetVistaPrevia(_UltimaFactura);
                }finally{_Emitiendo=false;_VentasBtnEmitirFactura.Enabled=true;}
            });
        }
        private void VentasMetVistaPrevia(ClsFacturaRegistrada Factura)
        {
            using (var Ventana = new FrmReporteFactura(Factura)) Ventana.ShowDialog(this);
        }
    }
    public static class ClsReporteFactura
    {
        public static void VentasMetConfigurar(LocalReport Reporte,ClsFacturaRegistrada Factura)
        {
            using(var Stream=typeof(FrmFacturas).Assembly.GetManifestResourceStream("CapaVista_Facturas.Reportes.RpReporteFactura.rdlc"))Reporte.LoadReportDefinition(Stream);
            var Tabla=Factura.Detalle.Copy();foreach(string Columna in new[]{"NitCliente","NombreCliente","DireccionCliente"})Tabla.Columns.Add(Columna,typeof(string));
            foreach(DataRow Fila in Tabla.Rows){Fila["NitCliente"]=Factura.Identificacion;Fila["NombreCliente"]=Factura.Cliente;Fila["DireccionCliente"]=Factura.Direccion;}
            Reporte.DataSources.Clear();Reporte.DataSources.Add(new ReportDataSource("dsDetalle",Tabla));
            Reporte.SetParameters(new[]{new ReportParameter("Autorizacion",Factura.Uuid),new ReportParameter("Serie",Factura.Serie),new ReportParameter("Numero",Factura.Numero),new ReportParameter("Moneda",Factura.Moneda),new ReportParameter("Fecha",Factura.Fecha.ToString("dd/MM/yyyy HH:mm:ss")),new ReportParameter("FechaVencimiento",Factura.FechaVencimiento.ToString("dd/MM/yyyy")),new ReportParameter("Empresa",Factura.Empresa),new ReportParameter("NitEmisor",Factura.NitEmisor),new ReportParameter("DireccionEmisor",Factura.DireccionEmisor),new ReportParameter("Condicion",Factura.Condicion)});
        }
    }
}
