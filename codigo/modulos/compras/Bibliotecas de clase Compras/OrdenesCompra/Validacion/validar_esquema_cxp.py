from pathlib import Path
import subprocess
import re

raiz = Path(__file__).resolve().parent.parent
cliente = Path('C:/Program Files/MySQL/MySQL Server 8.0/bin/mysql.exe')
argumentos = [str(cliente), '--no-defaults', '--protocol=tcp', '--host=127.0.0.1',
              '--port=33317', '--user=root', '--default-character-set=utf8mb4', '--batch', '--skip-column-names']


def ejecutar(sql, error=None):
    resultado = subprocess.run(argumentos, input=sql.encode('utf-8'), capture_output=True)
    salida = resultado.stdout.decode('utf-8', errors='replace').strip()
    mensaje = resultado.stderr.decode('utf-8', errors='replace')
    if error is None and resultado.returncode:
        raise AssertionError(mensaje)
    if error is not None and (resultado.returncode == 0 or f'ERROR {error} ' not in mensaje):
        raise AssertionError(f'Se esperaba ERROR {error}: {mensaje}')
    return salida


directorio = re.sub('/+', '/', ejecutar('SELECT @@datadir;').replace('\\', '/')).rstrip('/').lower()
assert directorio == str(raiz / 'Validacion/mysql-cxp-check').replace('\\', '/').lower()
script = (raiz / 'script_cxp_v1.6.sql').read_text(encoding='utf-8-sig')
ejecutar(script)
ejecutar(script)
base = 'USE dbsistemaembutidos; '
assert ejecutar(base + 'SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE();') == '22'
assert ejecutar(base + 'SELECT COUNT(*) FROM tblPlazo;') == '7'
assert ejecutar(base + 'SELECT COUNT(*) FROM tblConceptoProveedor;') == '5'
assert ejecutar(base + "SELECT COUNT(*) FROM information_schema.key_column_usage WHERE table_schema = DATABASE() AND referenced_table_name = 'tblTipoCambio' AND referenced_column_name = 'Pk_TipoCambio';") == '4'
ejecutar(base + '''
INSERT INTO tblMoneda (Pk_Moneda, codigoMoneda, nombreMoneda, simboloMoneda) VALUES
 (9001, 'GTQ', 'Quetzal', 'Q'), (9002, 'USD', 'Dolar', '$');
INSERT INTO tblTipoCambio (Pk_TipoCambio, fechaTipoCambio, valorTipoCambio, fuenteTipoCambio,
 Fk_Moneda_Origen_Codigo, Fk_Moneda_Destino_Codigo)
 VALUES (9001, '2026-11-07', 7.7, 'Validacion', 9002, 9001);
INSERT INTO tblProveedor (idProveedor, nombreProveedor, nitProveedor, correoProveedor, telefonoProveedor, direccionProveedor)
 VALUES (9001, 'Proveedor validacion', 'VALID9001', 'prueba@example.test', '1234', 'Direccion');
INSERT INTO tblEncabezadoOrdenCompra (idOrdenCompra, idProveedor, idPlazo, fechaEmisionOrdenCompra,
 fechaVencimientoOrdenCompra, idMoneda, idTipoCambio, tipoCambioOrdenCompra,
 subtotalOrdenCompra, descuentoOrdenCompra, impuestoOrdenCompra, totalOrdenCompra)
 SELECT 9001, 9001, idPlazo, '2026-11-07', '2026-12-07', 9002, 9001, 7.7, 100, 10, 12, 102
 FROM tblPlazo WHERE nombrePlazo = '30 dias';
INSERT INTO tblEncabezadoCompra (idCompra, idProveedor, serieFacturaCompra, numeroFacturaCompra,
 fechaCompra, idMetodoPago, idMoneda, idTipoCambio, tipoCambioCompra, subtotalCompra, totalCompra)
 VALUES (9001, 9001, 'V', '9001', '2026-11-07', 9001, 9002, 9001, 7.7, 100, 100);
'''.replace('INSERT INTO tblEncabezadoCompra', "INSERT INTO tblMetodoPago (idMetodoPago, nombreMetodoPago, descripcionMetodoPago) VALUES (9001, 'Validacion', 'Validacion');\nINSERT INTO tblEncabezadoCompra"))
rechazos = [
    ('UPDATE tblEncabezadoOrdenCompra SET idTipoCambio=999999 WHERE idOrdenCompra=9001', 1452),
    ('UPDATE tblEncabezadoOrdenCompra SET idMoneda=999999 WHERE idOrdenCompra=9001', 1452),
    ('UPDATE tblEncabezadoOrdenCompra SET idTipoCambio=NULL WHERE idOrdenCompra=9001', 3819),
    ("UPDATE tblEncabezadoOrdenCompra SET fechaVencimientoOrdenCompra='2026-11-01' WHERE idOrdenCompra=9001", 3819),
    ('UPDATE tblEncabezadoOrdenCompra SET totalOrdenCompra=999 WHERE idOrdenCompra=9001', 3819),
    ("UPDATE tblEncabezadoOrdenCompra SET estadoOrdenCompra='Invalido' WHERE idOrdenCompra=9001", 3819),
    ('UPDATE tblEncabezadoCompra SET idTipoCambio=999999 WHERE idCompra=9001', 1452),
    ('UPDATE tblEncabezadoCompra SET totalCompra=-1 WHERE idCompra=9001', 3819),
    ("UPDATE tblTipoCambio SET valorTipoCambio=0 WHERE Pk_TipoCambio=9001", 3819),
    ('DELETE FROM tblTipoCambio WHERE Pk_TipoCambio=9001', 1451),
]
for sql, error in rechazos:
    ejecutar(base + sql + ';', error)
ejecutar(base + '''
INSERT INTO tblCuentaPorPagar (idProveedor,idConceptoProveedor,serieDocumentoCuentaPorPagar,
 numeroDocumentoCuentaPorPagar,fechaEmisionCuentaPorPagar,fechaVencimientoCuentaPorPagar,
 idMoneda,idTipoCambio,tipoCambioCuentaPorPagar,montoTotalCuentaPorPagar,saldoActualCuentaPorPagar)
 SELECT 9001,idConceptoProveedor,'V','9001','2026-11-07','2026-12-07',9002,9001,7.7,100,100
 FROM tblConceptoProveedor WHERE nombreConceptoProveedor='Factura de compra';
INSERT INTO tblEncabezadoPagoProveedor (idPagoProveedor,idProveedor,fechaPagoProveedor,
 montoPagoProveedor,idMetodoPago,idMoneda,idTipoCambio,tipoCambioPagoProveedor)
 VALUES (9001,9001,'2026-11-07',100,9001,9002,9001,7.7);
''')
for tabla, condicion in [('tblCuentaPorPagar', "numeroDocumentoCuentaPorPagar='9001'"),
                         ('tblEncabezadoPagoProveedor', 'idPagoProveedor=9001')]:
    ejecutar(base + f'UPDATE {tabla} SET idTipoCambio=999999 WHERE {condicion};', 1452)
print('MySQL 8: 22 tablas creadas, script repetible, datos iniciales sin duplicados, cuatro relaciones de cotizacion y 12 rechazos de datos invalidos verificados.')
