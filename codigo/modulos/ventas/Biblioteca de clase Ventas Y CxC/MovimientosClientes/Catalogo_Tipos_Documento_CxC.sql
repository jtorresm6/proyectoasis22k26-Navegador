CREATE TABLE IF NOT EXISTS tblTipoDocumentoCxC (
 codigoTipoDocumentoCxC INT NOT NULL,
 nombreTipoDocumentoCxC VARCHAR(100) NOT NULL,
 efectoTipoDocumentoCxC VARCHAR(10) NOT NULL,
 activoTipoDocumentoCxC BOOLEAN NOT NULL DEFAULT 1,
 CONSTRAINT Pk_TipoDocumentoCxC PRIMARY KEY (codigoTipoDocumentoCxC),
 CONSTRAINT ckTipoDocumentoCxCEfecto CHECK (efectoTipoDocumentoCxC IN ('CARGO','ABONO')),
 CONSTRAINT ckTipoDocumentoCxCCodigo CHECK (codigoTipoDocumentoCxC BETWEEN 0 AND 4)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
INSERT INTO tblTipoDocumentoCxC (codigoTipoDocumentoCxC,nombreTipoDocumentoCxC,efectoTipoDocumentoCxC)
SELECT d.Id,d.Nombre,d.Efecto FROM (
 SELECT 0 Id,'Factura de CxC (sin productos)' Nombre,'CARGO' Efecto
 UNION ALL SELECT 1,'Nota de credito','ABONO'
 UNION ALL SELECT 2,'Nota de debito','CARGO'
 UNION ALL SELECT 3,'Ajuste de cargo','CARGO'
 UNION ALL SELECT 4,'Ajuste de abono','ABONO'
) d WHERE NOT EXISTS(SELECT 1 FROM tblTipoDocumentoCxC t WHERE t.codigoTipoDocumentoCxC=d.Id);
