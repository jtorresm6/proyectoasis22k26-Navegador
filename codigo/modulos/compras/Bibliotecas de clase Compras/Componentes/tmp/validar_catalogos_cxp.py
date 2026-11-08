from pathlib import Path
import re, sqlite3
script=Path('script_cxp_v1.6.sql').read_text(encoding='utf-8-sig')
repo=Path('CapaModelo_Componentes/Repositorios/ClsRepositorioCombo.cs').read_text(encoding='utf-8-sig')
view=Path('CapaVista_Componentes/Componentes/ComprasUsrComboBox.cs').read_text(encoding='utf-8-sig')
db=sqlite3.connect(':memory:')
db.row_factory=sqlite3.Row
db.execute('PRAGMA foreign_keys = ON')
db.create_function('CONCAT',-1,lambda *v: ''.join(map(str,v)))
tables={}
for name,body in re.findall(r'CREATE TABLE IF NOT EXISTS\s+(\w+)\s*\((.*?)\)\s*ENGINE=',script,re.S|re.I):
 body=body.replace('AUTO_INCREMENT','').replace(' ON UPDATE CURRENT_TIMESTAMP','')
 body=re.sub(r'^\s*INDEX\s+[^\n]+\n','\n',body,flags=re.M)
 body=re.sub(r'UNIQUE KEY \w+', 'UNIQUE',body)
 db.execute('CREATE TABLE '+name+' ('+body+')')
 tables[name]={r[1] for r in db.execute('PRAGMA table_info('+name+')')}
refs=re.findall(r'REFERENCES\s+(\w+)\s*\(([^)]+)\)',script,re.I)
for name,cols in refs:
 assert name in tables,(name,'table missing')
 for col in cols.split(','): assert col.strip() in tables[name],(name,col,'FK target missing')
def special(table):
 m=re.search(r'if \(TablaPermitida == "'+table+r'"\)\s*return ComprasFuncEjecucionConsulta\((.*?), CommandType.Text\);',repo,re.S)
 return ''.join(re.findall(r'"([^"\n]*)"',m.group(1))) if m else None
configs=re.findall(r'case EnmTipoDatosCompras\.(\w+):\s*Tabla = "(\w+)";\s*Valor = "(\w+)";\s*Mostrar = "(\w+)";',view)
assert len(configs)==13,len(configs)
for tipo,tabla,valor,mostrar in configs:
 query=special(tabla) or f'SELECT `{valor}`, `{mostrar}` FROM `{tabla}`'
 cols={c[0] for c in db.execute(query).description}
 assert {valor,mostrar}<=cols,(tipo,cols)
db.executemany('INSERT INTO tblMoneda (Pk_Moneda,codigoMoneda,nombreMoneda,simboloMoneda,is_active) VALUES (?,?,?,?,?)',[(5000000001,'GTQ','Quetzal','Q',1),(2,'USD','Dolar','$',1),(3,'EUR','Euro','EUR',0)])
monedas=db.execute(special('tblMoneda')).fetchall()
assert [r['codigoMoneda'] for r in monedas]==['USD','GTQ']
assert monedas[1]['Pk_Moneda']==monedas[1]['idMoneda']==5000000001
db.executemany('INSERT INTO tblMetodoPago (idMetodoPago,nombreMetodoPago,descripcionMetodoPago,is_active) VALUES (?,?,?,?)',[(8,'Transferencia','Banco',1),(9,'Desactivado','Otro',0)])
metodos=db.execute(special('tblMetodoPago')).fetchall()
assert len(metodos)==1 and metodos[0]['idMetodoPago']==8
db.execute("INSERT INTO tblTipoCambio (Pk_TipoCambio,fechaTipoCambio,valorTipoCambio,fuenteTipoCambio,Fk_Moneda_Origen_Codigo,Fk_Moneda_Destino_Codigo) VALUES (6000000001,'2026-11-07',7.65,'BANGUAT',2,5000000001)")
cambio=db.execute(special('tblTipoCambio')).fetchone()
assert cambio['Pk_TipoCambio']==cambio['idTipoCambio']==6000000001
assert cambio['descripcionTipoCambio'].startswith('USD / GTQ - ')
assert not db.execute('PRAGMA foreign_key_check').fetchall()
print(f'PASS: {len(tables)} tables from the current script, {len(refs)} FK targets, all {len(configs)} selector mappings, active payment methods/currencies, BIGINT identifiers, aliases and exchange joins.')
