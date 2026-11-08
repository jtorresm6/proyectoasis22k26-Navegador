from pathlib import Path
import re
import sqlite3

raiz = Path(__file__).resolve().parent.parent
script = (raiz / 'script_cxp_v1.6.sql').read_text(encoding='utf-8-sig')
fuente = (raiz / 'CapaModelo_OrdenesCompra/Repositorios/ClsConsultasOrdenesCompra.cs').read_text(encoding='utf-8-sig')


def consulta(metodo, anterior=False, estado='is_active'):
    cuerpo = fuente.split('internal DataTable ' + metodo + '(', 1)[1].split('\n        }', 1)[0]
    cuerpo = cuerpo.split('return ', 1)[1]
    sql = ''.join(re.findall(r'"([^"\n]*)"', cuerpo.split('CommandType.Text', 1)[0]))
    moneda = 'idMoneda' if anterior else 'Pk_Moneda'
    cambio = 'idTipoCambio' if anterior else 'Pk_TipoCambio'
    return sql.format(moneda, estado) if metodo == 'ComprasFuncListarMonedas' else sql.format(cambio, moneda, estado)


with sqlite3.connect(':memory:') as conexion:
    for tabla in ('tblMoneda', 'tblTipoCambio'):
        definicion = re.search(r'CREATE TABLE IF NOT EXISTS ' + tabla + r'\s*\((.*?)\) ENGINE', script, re.S).group(1)
        columnas = re.findall(r'^\s*(\w+)\s+(BIGINT|INT|CHAR|VARCHAR|BOOLEAN|TIMESTAMP|DATE|DECIMAL|TEXT)\b', definicion, re.M)
        conexion.execute('CREATE TABLE ' + tabla + ' (' + ', '.join(nombre + ' ' + ('INTEGER' if tipo in ('BIGINT', 'INT', 'BOOLEAN') else 'TEXT') for nombre, tipo in columnas) + ')')
    conexion.executemany('INSERT INTO tblMoneda (Pk_Moneda, codigoMoneda, nombreMoneda, simboloMoneda, is_active) VALUES (?, ?, ?, ?, ?)', [
        (10, 'GTQ', 'Quetzal', 'Q', 1), (20, 'USD', 'Dolar', '$', 1), (30, 'EUR', 'Euro', 'E', 0)])
    resultado = conexion.execute(consulta('ComprasFuncListarMonedas'))
    assert resultado.description[0][0] == 'idMoneda'
    assert [fila[0] for fila in resultado] == [10, 20]
    conexion.executemany('INSERT INTO tblTipoCambio (Pk_TipoCambio, fechaTipoCambio, valorTipoCambio, versionTipoCambio, Fk_Moneda_Origen_Codigo, Fk_Moneda_Destino_Codigo) VALUES (?, ?, ?, ?, ?, ?)', [
        (1, '2026-11-01', '7.60', 1, 20, 10), (2, '2026-11-07', '7.70', 1, 20, 10),
        (3, '2026-11-07', '7.75', 2, 20, 10), (4, '2026-11-08', '7.80', 1, 20, 10)])
    cotizacion = consulta('ComprasFuncObtenerTipoCambio')
    assert conexion.execute(cotizacion, (20, '2026-11-07')).fetchone()[0] == 3
    assert conexion.execute(cotizacion, (20, '2026-10-31')).fetchone() is None
    conexion.execute('UPDATE tblMoneda SET is_active = 0 WHERE Pk_Moneda = 10')
    assert conexion.execute(cotizacion, (20, '2026-11-07')).fetchone() is None
    referencias = re.findall(r'REFERENCES tblMoneda\s*\((\w+)\)', script)
    assert len(referencias) == 6 and set(referencias) == {'Pk_Moneda'}
    conexion.execute('ALTER TABLE tblMoneda RENAME COLUMN Pk_Moneda TO idMoneda')
    conexion.execute('ALTER TABLE tblTipoCambio RENAME COLUMN Pk_TipoCambio TO idTipoCambio')
    conexion.execute('UPDATE tblMoneda SET is_active=1 WHERE idMoneda=10')
    assert [fila[0] for fila in conexion.execute(consulta('ComprasFuncListarMonedas', True))] == [10, 20]
    assert conexion.execute(consulta('ComprasFuncObtenerTipoCambio', True), (20, '2026-11-07')).fetchone()[0] == 3
    conexion.execute('ALTER TABLE tblMoneda RENAME COLUMN is_active TO activoMoneda')
    assert [fila[0] for fila in conexion.execute(consulta('ComprasFuncListarMonedas', True, 'activoMoneda'))] == [10, 20]
    assert conexion.execute(consulta('ComprasFuncObtenerTipoCambio', True, 'activoMoneda'), (20, '2026-11-07')).fetchone()[0] == 3
print('10 validaciones correctas: consultas y alias compatibles con ambos esquemas y ambos nombres del estado de moneda.')
