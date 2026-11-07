from pathlib import Path
import subprocess,time,json,os
root=Path('.').resolve();workspace=root/'tmp/mysql_prueba_compras'
base=Path('C:/Program Files/MySQL/MySQL Server 8.0')
socket='COMPRAS_PRUEBA_'+str(os.getpid())
args=[str(base/'bin/mysqld.exe'),'--no-defaults','--basedir='+str(base),'--datadir='+str(workspace/'data'),'--log-error='+str(workspace/'servidor.log'),'--skip-networking','--enable-named-pipe','--socket='+socket,'--mysqlx=0','--innodb-buffer-pool-size=64M']
client=[str(base/'bin/mysql.exe'),'--no-defaults','--protocol=PIPE','--socket='+socket,'--user=root','--batch','--raw','--skip-column-names','--default-character-set=utf8mb4']
checks=0

def execute(sql,ok=True):
    result=subprocess.run(client,input=sql,encoding='utf-8',capture_output=True,timeout=30)
    if ok and result.returncode:
        raise RuntimeError(result.stderr)
    return result

def scalar(sql):return execute('USE dbsistemaembutidos;\n'+sql).stdout.strip()
def verify(condition,message):
    global checks
    if not condition:raise AssertionError(message)
    checks+=1

tables=['tblPlazo','tblConceptoProveedor','tblProveedor','tblOrdenCompra','tblDetalleOrdenCompra','tblCompra','tblDetalleCompra','tblCuentaPorPagar','tblDevolucionCompra','tblDetalleDevolucionCompra','tblPagoProveedor','tblDetallePagoProveedor','tblSolicitudPago']
def snapshot():return {table:scalar('SELECT * FROM '+table+' ORDER BY 1,2;') for table in tables}
prelude='SET @ComprasIdMoneda=5; SET @ComprasIdMetodoPago=7; SET @ComprasIdProducto1=101; SET @ComprasIdProducto2=102;\n'
seed=(root/'03_datos_prueba_compras.sql').read_text(encoding='utf-8-sig')
server=subprocess.Popen(args,stdout=subprocess.DEVNULL,stderr=subprocess.DEVNULL,creationflags=subprocess.CREATE_NO_WINDOW)
try:
    deadline=time.monotonic()+20
    while time.monotonic()<deadline:
        if server.poll() is not None:raise RuntimeError((workspace/'servidor.log').read_text(encoding='utf-8',errors='replace')[-2500:])
        if execute('SELECT 1;',False).returncode==0:break
        time.sleep(.15)
    else:raise TimeoutError('La instancia aislada de MySQL no inicio.')
    execute((root/'01_modulo_compras_cuentas_por_pagar_v2.sql').read_text(encoding='utf-8-sig'))
    execute('USE dbsistemaembutidos; ALTER TABLE tblProveedor AUTO_INCREMENT=1001; ALTER TABLE tblOrdenCompra AUTO_INCREMENT=2001; ALTER TABLE tblCompra AUTO_INCREMENT=3001; ALTER TABLE tblCuentaPorPagar AUTO_INCREMENT=4001; ALTER TABLE tblDevolucionCompra AUTO_INCREMENT=5001; ALTER TABLE tblPagoProveedor AUTO_INCREMENT=6001; ALTER TABLE tblSolicitudPago AUTO_INCREMENT=7001;')
    result=execute(prelude+seed)
    (workspace/'resultado_inicial.txt').write_text(result.stdout,encoding='utf-8')
    counts={'tblProveedor':4,'tblOrdenCompra':12,'tblDetalleOrdenCompra':24,'tblCompra':3,'tblDetalleCompra':6,'tblCuentaPorPagar':4,'tblDevolucionCompra':1,'tblDetalleDevolucionCompra':1,'tblPagoProveedor':5,'tblDetallePagoProveedor':2,'tblSolicitudPago':5}
    for table,count in counts.items():verify(scalar('SELECT COUNT(*) FROM '+table+';')==str(count),table)
    verify(scalar("SELECT COUNT(*) FROM tblOrdenCompra WHERE estadoOrdenCompra='Pendiente';")=='5','Pendientes')
    verify(scalar("SELECT COUNT(*) FROM tblOrdenCompra WHERE estadoOrdenCompra='Autorizada';")=='2','Autorizadas')
    verify(scalar("SELECT COUNT(*) FROM tblOrdenCompra WHERE estadoOrdenCompra='Recibida';")=='3','Recibidas')
    verify(scalar('SELECT MIN(idOrdenCompra) FROM tblOrdenCompra;')=='2001','No depender de PK fijos')
    verify(scalar('SELECT COUNT(*) FROM tblOrdenCompra WHERE idMoneda<>5;')=='0','Moneda configurable')
    verify(scalar('SELECT COUNT(*) FROM tblPagoProveedor WHERE idMetodoPago<>7;')=='0','Metodo configurable')
    verify(scalar('SELECT COUNT(*) FROM tblDetalleOrdenCompra WHERE idProducto NOT IN (101,102);')=='0','Productos configurables')
    verify(scalar('SELECT COUNT(*) FROM tblOrdenCompra o INNER JOIN (SELECT idOrdenCompra,SUM(subtotalDetalleOrdenCompra) subtotal FROM tblDetalleOrdenCompra GROUP BY idOrdenCompra) d ON d.idOrdenCompra=o.idOrdenCompra WHERE ABS(o.subtotalOrdenCompra-d.subtotal)>0.00001 OR ABS(o.totalOrdenCompra-o.subtotalOrdenCompra+o.descuentoOrdenCompra)>0.00001;')=='0','Totales ordenes')
    verify(scalar('SELECT COUNT(*) FROM tblDetalleOrdenCompra WHERE ABS(subtotalDetalleOrdenCompra-ROUND(cantidadDetalleOrdenCompra*costoUnitarioDetalleOrdenCompra*(1-descuentoPorcentajeDetalleOrdenCompra/100),4))>0.00001;')=='0','Descuentos detalle')
    verify(scalar('SELECT COUNT(*) FROM tblCompra c INNER JOIN (SELECT idCompra,SUM(subtotalDetalleCompra) subtotal FROM tblDetalleCompra GROUP BY idCompra) d ON d.idCompra=c.idCompra WHERE ABS(c.subtotalCompra-d.subtotal)>0.00001 OR ABS(c.totalCompra-c.subtotalCompra+c.descuentoCompra)>0.00001;')=='0','Totales compras')
    verify(scalar("SELECT COUNT(*) FROM tblCuentaPorPagar c WHERE ABS(c.saldoActualCuentaPorPagar-(c.montoTotalCuentaPorPagar-COALESCE((SELECT SUM(d.montoAplicadoDetallePago) FROM tblDetallePagoProveedor d INNER JOIN tblPagoProveedor p ON p.idPagoProveedor=d.idPagoProveedor WHERE d.idCuentaPorPagar=c.idCuentaPorPagar AND p.estadoPagoProveedor='Pagado'),0)-COALESCE((SELECT SUM(dev.totalDevolucionCompra) FROM tblDevolucionCompra dev WHERE dev.idCompra=c.idCompra AND dev.estadoDevolucionCompra='Registrada'),0)))>0.00001;")=='0','Saldos cuentas')
    verify(scalar("SELECT COUNT(*) FROM tblProveedor p WHERE ABS(p.saldoActualProveedor-COALESCE((SELECT SUM(c.saldoActualCuentaPorPagar) FROM tblCuentaPorPagar c WHERE c.idProveedor=p.idProveedor AND c.estadoCuentaPorPagar<>'Anulada'),0))>0.00001 OR ABS(p.saldoActualProveedor-p.saldoAnteriorProveedor-p.cargosMesProveedor+p.abonosMesProveedor)>0.00001;")=='0','Resumen proveedores')
    verify(scalar("SELECT totalOrdenCompra FROM tblOrdenCompra WHERE codigoOrdenCompra='PRUEBA-OC-001';")=='950.0000','Descuento global y de linea')
    verify(scalar("SELECT totalOrdenCompra FROM tblOrdenCompra WHERE codigoOrdenCompra='PRUEBA-OC-012';")=='1200.1234','Cuatro decimales')
    first=snapshot();execute(prelude+seed);verify(snapshot()==first,'Reejecucion sin duplicados ni modificaciones')
    execute("USE dbsistemaembutidos; UPDATE tblOrdenCompra SET estadoOrdenCompra='Autorizada' WHERE codigoOrdenCompra='PRUEBA-OC-001';")
    modified=snapshot();execute(prelude+seed);verify(snapshot()==modified,'No restablecer cambios de pruebas')
    invalid=execute('SET @ComprasIdProducto1=101;SET @ComprasIdProducto2=101;\n'+seed,False)
    verify(invalid.returncode!=0,'Rechazar productos repetidos')
    verify(snapshot()==modified,'Parametros invalidos sin escritura persistente')
    report={'motor':'MySQL 8.0.46, instancia aislada sin TCP','comprobaciones':checks,'cantidades':counts,'resultado':'correcto'}
    (workspace/'verificacion.json').write_text(json.dumps(report,ensure_ascii=False,indent=2),encoding='utf-8')
    print(str(checks)+' comprobaciones correctas en MySQL aislado: relaciones, importes, saldos, repeticion y parametros.')
finally:
    if server.poll() is None:
        subprocess.run([str(base/'bin/mysqladmin.exe'),'--no-defaults','--protocol=PIPE','--socket='+socket,'--user=root','shutdown'],capture_output=True,timeout=15)
        try:server.wait(timeout=15)
        except subprocess.TimeoutExpired:
            server.terminate();server.wait(timeout=10)
