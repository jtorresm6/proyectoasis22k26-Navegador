from datetime import date
from pathlib import Path
import re
import sqlite3

source = (Path(__file__).resolve().parents[1] / 'CapaModelo_Compras' / 'ClsIndicadoresCompras.cs').read_text(encoding='utf-8')
query = re.search(r'@"(.*?)", CommandType.Text', source, re.S)[1]
query = query.replace('CURRENT_DATE()', "'2026-11-05'")
query = query.replace("DATE_ADD('2026-11-05', INTERVAL 7 DAY)", "'2026-11-12'")
db = sqlite3.connect(':memory:')
db.create_function('GREATEST', 2, max)
db.create_function('DATEDIFF', 2, lambda a, b: (date.fromisoformat(a) - date.fromisoformat(b)).days)
db.executescript('''
CREATE TABLE tblConceptoProveedor (idConceptoProveedor INTEGER PRIMARY KEY, naturalezaConceptoProveedor TEXT);
CREATE TABLE tblCuentaPorPagar (idConceptoProveedor INTEGER, fechaEmisionCuentaPorPagar TEXT, fechaVencimientoCuentaPorPagar TEXT, saldoActualCuentaPorPagar NUMERIC, tipoCambioCuentaPorPagar NUMERIC, estadoCuentaPorPagar TEXT);
CREATE TABLE tblEncabezadoOrdenCompra (estadoOrdenCompra TEXT, fechaEmisionOrdenCompra TEXT);
''')
assert db.execute(query).fetchone() == (0, 0, 0, 0, 0, 0)
db.executemany('INSERT INTO tblConceptoProveedor VALUES (?, ?)', [(1, 'Cargo'), (2, 'Abono')])
rows = [
    (1, '2026-10-06', '2026-11-04', 100, 1, 'Pendiente'),
    (1, '2026-10-26', '2026-11-05', 50, 7.7, 'Parcial'),
    (1, '2026-11-05', '2026-11-12', 200, 1, 'Pendiente'),
    (1, '2026-11-05', '2026-11-13', 300, 1, 'Pendiente'),
    (1, '2026-10-06', '2026-11-04', 999, 1, 'Anulada'),
    (1, '2026-10-06', '2026-11-04', 999, 1, 'Pagada'),
    (1, '2026-11-06', '2026-11-07', 999, 1, 'Pendiente'),
    (2, '2026-10-06', '2026-11-04', 999, 1, 'Pendiente'),
    (1, '2026-10-06', '2026-11-04', 0, 1, 'Pendiente'),
    (1, '2026-10-06', '2026-11-04', -999, 1, 'Parcial')
]
db.executemany('INSERT INTO tblCuentaPorPagar VALUES (?, ?, ?, ?, ?, ?)', rows)
db.executemany('INSERT INTO tblEncabezadoOrdenCompra VALUES (?, ?)', [
    ('Autorizada', '2026-11-05'), ('Recibida', '2026-11-04'),
    ('Pendiente', '2026-11-03'), ('Rechazada', '2026-11-02'),
    ('Anulada', '2026-11-01'), ('Autorizada', '2026-11-06')
])
actual = db.execute(query).fetchone()
expected = (10, 50, 1, 985, 100, 585)
assert actual == expected, (actual, expected)
print('PASS: empty database, GTQ/USD conversion, partial balances, exclusions, overdue and seven-day boundaries, order percentage.')
