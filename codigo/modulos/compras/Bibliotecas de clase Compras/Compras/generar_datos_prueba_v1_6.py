from datetime import date, timedelta
from decimal import Decimal, ROUND_HALF_UP
from pathlib import Path
import random

rng = random.Random(1606)
sql = ["USE dbsistemaembutidos;", "SET NAMES utf8mb4;", "START TRANSACTION;", "SET @lote = LEFT(REPLACE(UUID(), '-', ''), 12);"]
counts = {}

def value(v):
    if v is None:
        return 'NULL'
    if isinstance(v, str):
        if v.startswith('@') or v.startswith('CONCAT(') or v.startswith('UNHEX('):
            return v
        return "'" + v.replace("'", "''") + "'"
    return str(v)

def insert(table, columns, values, variable=None, reuse=None):
    statement = f"INSERT INTO {table} ({', '.join(columns.split())}) VALUES ({', '.join(map(value, values))})"
    if reuse:
        statement += f" ON DUPLICATE KEY UPDATE {reuse} = LAST_INSERT_ID({reuse})"
    sql.append(statement + ';')
    counts[table] = counts.get(table, 0) + 1
    if variable:
        sql.append(f'SET {variable} = LAST_INSERT_ID();')

def money(v):
    return Decimal(v).quantize(Decimal('0.0001'), rounding=ROUND_HALF_UP)

for i, (name, days) in enumerate([('Contado', 0), ('15 dias', 15), ('30 dias', 30), ('60 dias', 60), ('90 dias', 90)]):
    insert('tblPlazo', 'nombrePlazo diasPlazo', [name, days], f'@pl{i}', 'idPlazo')
for i, name in enumerate(['Transferencia bancaria', 'Cheque', 'Efectivo', 'Tarjeta empresarial']):
    insert('tblMetodoPago', 'nombreMetodoPago descripcionMetodoPago', [name, f'Pago mediante {name.lower()}'], f'@mp{i}', 'idMetodoPago')
for code, name, symbol in [('GTQ', 'Quetzal', 'Q'), ('USD', 'Dolar estadounidense', '$')]:
    insert('tblMoneda', 'codigoMoneda nombreMoneda simboloMoneda', [code, name, symbol], '@' + code, 'idMoneda')
insert('tblConceptoProveedor', 'nombreConceptoProveedor naturalezaConceptoProveedor', ['Factura de compra', 'Cargo'], '@factura', 'idConceptoProveedor')
insert('tblConceptoProveedor', 'nombreConceptoProveedor naturalezaConceptoProveedor', ['Nota de credito', 'Abono'], '@credito', 'idConceptoProveedor')
for i in range(12):
    dt = date(2026, i + 1, 1)
    insert('tblTipoCambio', 'fechaTipoCambio valorTipoCambio fuenteTipoCambio Fk_Moneda_Origen_Codigo Fk_Moneda_Destino_Codigo', [dt.isoformat(), Decimal('7.65') + Decimal(i % 5) / 100, 'Escenario sintetico v1.6', '@USD', '@GTQ'], f'@tc{i}', 'idTipoCambio')
for i, city in enumerate(['Guatemala', 'Mixco', 'Villa Nueva', 'Escuintla', 'Quetzaltenango', 'Zacapa']):
    insert('tblSucursal', 'nombreSucursal descripcionSucursal ubicacionSucursal paisSucursal', [f"CONCAT('Prueba ', '{city} ', @lote)", 'Centro de distribucion y procesamiento', f'Zona industrial, {city}', 'Guatemala'], f'@s{i}')
    for j, name in enumerate(['Camara refrigerada', 'Almacen seco']):
        insert('tblBodega', 'nombreBodega Fk_Sucursal_Codigo paisBodega', [name, f'@s{i}', 'Guatemala'], f'@b{i * 2 + j}')
for table, field, names, prefix in [
    ('Categoria', 'Categoria', ['Carnes', 'Condimentos', 'Empaques', 'Limpieza', 'Repuestos'], 'cat'),
    ('Linea', 'Linea', ['Materia prima', 'Insumos de produccion', 'Material auxiliar', 'Mantenimiento'], 'lin'),
    ('Marca', 'Marca', ['Cumbre Test', 'Valle Test', 'AgroNorte Test', 'Empaca Test', 'Higiene Test'], 'mar')]:
    for i, name in enumerate(names):
        insert('tbl' + table, f'nombre{field} porcentajeComision{field}', [name, Decimal('0.03')], f'@{prefix}{i}', 'id' + table)

suppliers = ['Carnes del Valle', 'Porcicola La Cumbre', 'Avicola Los Pinos', 'Frigorificos del Norte', 'Agropecuaria Santa Elena', 'Proteinas de Oriente', 'Carnes Selectas del Sur', 'Distribuidora Pecuaria Central', 'Especias del Altiplano', 'Condimentos La Huerta', 'Ingredientes del Pacifico', 'Suministros Alimentarios Maya', 'Empaques del Valle', 'Plasticos Industriales Centro', 'Cartones y Cajas del Sur', 'Etiquetas del Istmo', 'Higiene Industrial del Norte', 'Soluciones Sanitarias del Sur', 'Repuestos Frio Industrial', 'Mecanica Alimentaria Central']
for i, name in enumerate(suppliers):
    insert('tblProveedor', 'nombreProveedor nitProveedor correoProveedor telefonoProveedor direccionProveedor estadoProveedor idPlazo', [name + ' (Prueba)', str(9901600000 + i), f'compras{i + 1:02}@proveedor.example', f'+502 22{i + 10:02}-{1100 + i}', f'{i + 1} avenida {10 + i}-25, zona {i % 12 + 1}, Guatemala', int(i != 19), f'@pl{i % 5}'], f'@p{i}', 'idProveedor')

families = [
    [('Pierna de cerdo deshuesada', '24.50'), ('Recorte de cerdo 80/20', '19.75'), ('Grasa dorsal de cerdo', '12.25'), ('Carne de res industrial', '34.80'), ('Pechuga de pollo', '23.40'), ('Muslo de pollo deshuesado', '18.60'), ('Panceta fresca', '28.90'), ('Carne de pavo', '39.50')],
    [('Sal refinada', '3.20'), ('Pimienta negra molida', '68.00'), ('Ajo en polvo', '42.50'), ('Paprika dulce', '49.80'), ('Mezcla para chorizo', '36.25'), ('Proteina de soya', '18.75'), ('Almidon de maiz', '8.50'), ('Humo liquido', '52.00')],
    [('Bolsa al vacio', '0.85'), ('Tripa de colageno', '3.40'), ('Bandeja alimentaria', '1.25'), ('Caja corrugada', '6.80'), ('Etiqueta termica', '0.18'), ('Pelicula stretch', '24.50'), ('Bolsa grado alimenticio', '0.65'), ('Cinta de empaque', '9.25')],
    [('Detergente alcalino', '22.50'), ('Sanitizante alimentario', '31.75'), ('Desengrasante industrial', '26.40'), ('Jabon para manos', '18.50'), ('Guantes de nitrilo', '38.00'), ('Cofia desechable', '0.45'), ('Cepillo sanitario', '45.00'), ('Papel industrial', '32.50')],
    [('Sello para empacadora', '185.00'), ('Cuchilla de molino', '320.00'), ('Rodamiento inoxidable', '145.00'), ('Banda transportadora', '475.00'), ('Sensor de temperatura', '225.00'), ('Empaque de silicona', '78.50'), ('Filtro de refrigeracion', '165.00'), ('Lubricante alimentario', '95.00')]]
products = []
for cat, items in enumerate(families):
    for k, (name, cost) in enumerate(items):
        for size in range(2):
            n = len(products)
            supplier = k % 8 if cat == 0 else 8 + (cat - 1) * 4 + k % (3 if cat == 4 else 4)
            supplier = min(supplier, 18)
            cost = money(Decimal(cost) * (1 + Decimal(size) / 5))
            products.append((supplier, cost, cat))
            insert('tblProducto', 'skuProducto clasificacionProducto nombreProducto idProveedorProductos Fk_Marca_Codigo Fk_Categoria_Codigo Fk_Linea_Codigo descripcionProducto imagenProducto paisProducto precioCostoProducto precioVentaProducto porcentajeComisionProducto stockMinimoProducto stockMaximoProducto pesoProducto', [f"CONCAT('T16-', @lote, '-{n:03}')", ['MP001', 'IN001', 'EM001', 'LI001', 'RE001'][cat], f'{name} - presentacion {size + 1}', f'@p{supplier}', f'@mar{cat}', f'@cat{cat}', f'@lin{min(cat, 3)}', 'Producto sintetico para pruebas de compras', "UNHEX('89504E470D0A1A0A0000000D4948445200000001000000010804000000B51C0C020000000B4944415478DA6364F80F00010501012718E3660000000049454E44AE426082')", 'Guatemala', cost, money(cost * Decimal('1.30')), Decimal('0.04'), 20 if cat < 4 else 2, 2000 if cat < 4 else 40, Decimal('1.00')], f'@pr{n}')

for n in range(300):
    supplier = n % 19
    eligible = [i for i, p in enumerate(products) if p[0] == supplier]
    if not eligible:
        continue
    dt = date(2026, 1, 5) + timedelta(days=n % 300)
    terms = [0, 15, 30, 60, 90][supplier % 5]
    usd = supplier in (10, 18)
    rate = Decimal('7.65') + Decimal((dt.month - 1) % 5) / 100 if usd else Decimal(1)
    currency = '@USD' if usd else '@GTQ'
    tc = f'@tc{dt.month - 1}' if usd else None
    chosen = rng.sample(eligible, min(len(eligible), rng.randint(2, 5)))
    lines = []
    for index in chosen:
        qty = rng.randint(4, 30) if products[index][2] == 4 else rng.randint(30, 500)
        cost = money(products[index][1] / rate)
        discount = Decimal(rng.choice(['0', '0', '0.03', '0.05']))
        lines.append((index, qty, cost, discount))
    subtotal = sum((money(q * c) for _, q, c, d in lines), Decimal(0))
    discount = sum((money(q * c * d) for _, q, c, d in lines), Decimal(0))
    tax = money((subtotal - discount) * Decimal('0.12'))
    total = subtotal - discount + tax
    status = ['Recibida'] * 8 + ['Pendiente', 'Autorizada', 'Rechazada', 'Anulada']
    state = status[n % 12]
    bodega = (n % 6) * 2 + int(products[chosen[0]][2] != 0)
    insert('tblEncabezadoOrdenCompra', 'idProveedor idPlazo fechaEmisionOrdenCompra fechaVencimientoOrdenCompra idMoneda idTipoCambio tipoCambioOrdenCompra subtotalOrdenCompra descuentoOrdenCompra impuestoOrdenCompra totalOrdenCompra comentarioOrdenCompra estadoOrdenCompra bloqueadoOrdenCompra', [f'@p{supplier}', f'@pl{supplier % 5}', dt.isoformat(), (dt + timedelta(days=terms)).isoformat(), currency, tc, rate, subtotal, discount, tax, total, 'Abastecimiento programado de planta; datos sinteticos', state, int(state in ('Recibida', 'Rechazada', 'Anulada'))], '@oc')
    for j, (index, qty, cost, disc) in enumerate(lines, 1):
        insert('tblDetalleOrdenCompra', 'idOrdenCompra ordenDetalleOrdenCompra idProducto idBodega cantidadDetalleOrdenCompra costoUnitarioDetalleOrdenCompra descuentoPorcentajeDetalleOrdenCompra', ['@oc', j, f'@pr{index}', f'@b{bodega}', qty, cost, disc * 100])
    if state != 'Recibida':
        continue
    purchase_date = dt + timedelta(days=2)
    doc = f"CONCAT(@lote, '-{n:04}')"
    annulled = n % 37 == 0
    insert('tblEncabezadoCompra', 'idOrdenCompra idProveedor serieFacturaCompra numeroFacturaCompra fechaCompra idMetodoPago idMoneda idTipoCambio tipoCambioCompra subtotalCompra descuentoCompra impuestoCompra totalCompra estadoCompra bloqueadoCompra', ['@oc', f'@p{supplier}', 'T16-FAC', doc, purchase_date.isoformat(), '@mp0', currency, tc, rate, subtotal, discount, tax, total, 'Anulada' if annulled else 'Registrada', int(annulled)], '@compra')
    for j, (index, qty, cost, disc) in enumerate(lines, 1):
        insert('tblDetalleCompra', 'idCompra ordenDetalleCompra idProducto idBodega cantidadDetalleCompra costoUnitarioDetalleCompra descuentoPorcentualDetalleCompra', ['@compra', j, f'@pr{index}', f'@b{bodega}', qty, cost, disc])
    returned = Decimal(0)
    if n % 7 == 0 and not annulled:
        index, qty, cost, disc = lines[0]
        returned_qty = max(1, qty // 10)
        net = money(returned_qty * cost * (1 - disc))
        returned = net + money(net * Decimal('0.12'))
        insert('tblDevolucionCompra', 'idCompra fechaDevolucionCompra motivoDevolucionCompra serieNotaCreditoDevolucionCompra numeroNotaCreditoDevolucionCompra totalDevolucionCompra', ['@compra', (purchase_date + timedelta(days=2)).isoformat(), rng.choice(['Empaque danado durante el transporte', 'Producto fuera de especificacion', 'Diferencia de calidad en inspeccion de recepcion']), 'T16-NC', doc, returned], '@dev')
        insert('tblDetalleDevolucionCompra', 'idDevolucionCompra ordenDetalleDevolucionCompra idProducto idBodega cantidadDetalleDevolucionCompra costoUnitarioDetalleDevolucionCompra descuentoPorcentajeDetalleDevolucionCompra', ['@dev', 1, f'@pr{index}', f'@b{bodega}', returned_qty, cost, disc])
        insert('tblCuentaPorPagar', 'idProveedor idConceptoProveedor serieDocumentoCuentaPorPagar numeroDocumentoCuentaPorPagar fechaEmisionCuentaPorPagar fechaVencimientoCuentaPorPagar idMoneda idTipoCambio tipoCambioCuentaPorPagar montoTotalCuentaPorPagar saldoActualCuentaPorPagar estadoCuentaPorPagar', [f'@p{supplier}', '@credito', 'T16-NC', doc, (purchase_date + timedelta(days=2)).isoformat(), (purchase_date + timedelta(days=2)).isoformat(), currency, tc, rate, -returned, 0, 'Pagada'])
    payment_state = ['Pagado', 'Pagado', 'Programado', 'Rechazado', 'Anulado'][n % 5]
    amount = money((total - returned) * (Decimal('0.5') if n % 2 else 1))
    paid = amount if payment_state == 'Pagado' and not annulled else Decimal(0)
    balance = Decimal(0) if annulled else total - returned - paid
    account_state = 'Anulada' if annulled else 'Pagada' if balance == 0 else 'Parcial' if paid or returned else 'Pendiente'
    insert('tblCuentaPorPagar', 'idProveedor idConceptoProveedor serieDocumentoCuentaPorPagar numeroDocumentoCuentaPorPagar fechaEmisionCuentaPorPagar fechaVencimientoCuentaPorPagar idMoneda idTipoCambio tipoCambioCuentaPorPagar montoTotalCuentaPorPagar saldoActualCuentaPorPagar estadoCuentaPorPagar bloqueadoCuentaPorPagar', [f'@p{supplier}', '@factura', 'T16-FAC', doc, purchase_date.isoformat(), (purchase_date + timedelta(days=terms)).isoformat(), currency, tc, rate, total, balance, account_state, int(annulled)])
    if annulled:
        continue
    insert('tblEncabezadoPagoProveedor', 'idProveedor fechaPagoProveedor montoPagoProveedor idMetodoPago idMoneda idTipoCambio tipoCambioPagoProveedor referenciaPagoProveedor estadoPagoProveedor bloqueadoPagoProveedor', [f'@p{supplier}', (purchase_date + timedelta(days=max(terms, 3))).isoformat(), amount, f'@mp{n % 4}', currency, tc, rate, doc, payment_state, int(payment_state != 'Programado')], '@pago')
    insert('tblDetallePagoProveedor', 'idPagoProveedor serieDocumentoCuentaPorPagar numeroDocumentoCuentaPorPagar idConceptoProveedor montoAplicadoDetallePago', ['@pago', 'T16-FAC', doc, '@factura', amount])
    insert('tblSolicitudPago', 'moduloOrigenSolicitudPago idOrigenSolicitudPago beneficiarioSolicitudPago montoSolicitudPago metodoSolicitudPago estadoSolicitudPago', ['Compras', '@pago', suppliers[supplier] + ' (Prueba)', amount, ['Transferencia bancaria', 'Cheque', 'Efectivo', 'Tarjeta empresarial'][n % 4], 'Procesada' if payment_state == 'Pagado' else 'Pendiente' if payment_state == 'Programado' else 'Rechazada'])

sql.append('SET @fecha_corte = CURRENT_DATE();')
for i in range(20):
    sql.append(f"UPDATE tblProveedor p SET saldoActualProveedor = (SELECT COALESCE(SUM(c.saldoActualCuentaPorPagar * c.tipoCambioCuentaPorPagar), 0) FROM tblCuentaPorPagar c WHERE c.idProveedor = p.idProveedor AND c.estadoCuentaPorPagar <> 'Anulada'), cargosAcumuladosProveedor = (SELECT COALESCE(SUM(c.montoTotalCuentaPorPagar * c.tipoCambioCuentaPorPagar), 0) FROM tblCuentaPorPagar c WHERE c.idProveedor = p.idProveedor AND c.idConceptoProveedor = @factura AND c.estadoCuentaPorPagar <> 'Anulada'), abonosAcumuladosProveedor = (SELECT COALESCE(SUM((c.montoTotalCuentaPorPagar - c.saldoActualCuentaPorPagar) * c.tipoCambioCuentaPorPagar), 0) FROM tblCuentaPorPagar c WHERE c.idProveedor = p.idProveedor AND c.idConceptoProveedor = @factura AND c.estadoCuentaPorPagar <> 'Anulada'), cargosMesProveedor = (SELECT COALESCE(SUM(c.montoTotalCuentaPorPagar * c.tipoCambioCuentaPorPagar), 0) FROM tblCuentaPorPagar c WHERE c.idProveedor = p.idProveedor AND c.idConceptoProveedor = @factura AND c.estadoCuentaPorPagar <> 'Anulada' AND DATE_FORMAT(c.fechaEmisionCuentaPorPagar, '%Y-%m') = DATE_FORMAT(@fecha_corte, '%Y-%m')), abonosMesProveedor = (SELECT COALESCE(SUM(h.montoPagoProveedor * h.tipoCambioPagoProveedor), 0) FROM tblEncabezadoPagoProveedor h WHERE h.idProveedor = p.idProveedor AND h.estadoPagoProveedor = 'Pagado' AND DATE_FORMAT(h.fechaPagoProveedor, '%Y-%m') = DATE_FORMAT(@fecha_corte, '%Y-%m')) + (SELECT COALESCE(SUM(-c.montoTotalCuentaPorPagar * c.tipoCambioCuentaPorPagar), 0) FROM tblCuentaPorPagar c WHERE c.idProveedor = p.idProveedor AND c.idConceptoProveedor = @credito AND c.estadoCuentaPorPagar <> 'Anulada' AND DATE_FORMAT(c.fechaEmisionCuentaPorPagar, '%Y-%m') = DATE_FORMAT(@fecha_corte, '%Y-%m')) WHERE p.idProveedor = @p{i};")
    sql.append(f'UPDATE tblProveedor SET saldoAnteriorProveedor = saldoActualProveedor - cargosMesProveedor + abonosMesProveedor WHERE idProveedor = @p{i};')
sql.append('COMMIT;')
sql.append("SELECT @lote AS loteDatosPrueba, 'Carga completada' AS resultado;")
path = Path(__file__).with_name('datos_prueba_cxp_v1.6.sql')
path.write_text('\n\n'.join(sql) + '\n', encoding='utf-8')
print(path.name)
for table, count in counts.items():
    print(f'{table}: {count}')
print(f'Total: {sum(counts.values())}')
