using System;

namespace CapaModelo_MovimientosProveedores.Entidades
{
    public sealed class ClsNotaProveedor
    {
        public int IdProveedor { get; set; }
        public int IdConceptoProveedor { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int IdMoneda { get; set; }
        public decimal TipoCambio { get; set; }
        public decimal Monto { get; set; }

        public void ComprasMetValidar()
        {
            if (IdProveedor <= 0) throw new ArgumentException("Seleccione un proveedor.");
            if (IdConceptoProveedor <= 0) throw new ArgumentException("Seleccione un tipo de nota.");
            Serie = (Serie ?? "").Trim();
            Numero = (Numero ?? "").Trim();
            if (Serie.Length == 0 || Serie.Length > 20) throw new ArgumentException("La serie es obligatoria y admite hasta 20 caracteres.");
            if (Numero.Length == 0 || Numero.Length > 30) throw new ArgumentException("El número es obligatorio y admite hasta 30 caracteres.");
            if (FechaVencimiento.Date < FechaEmision.Date) throw new ArgumentException("El vencimiento no puede ser anterior a la emisión.");
            if (IdMoneda <= 0) throw new ArgumentException("Seleccione una moneda.");
            if (TipoCambio <= 0 || TipoCambio > 999999.999999m || decimal.Round(TipoCambio, 6) != TipoCambio)
                throw new ArgumentException("Ingrese un tipo de cambio válido, con hasta seis decimales.");
            if (Monto <= 0 || Monto > 99999999.9999m || decimal.Round(Monto, 4) != Monto)
                throw new ArgumentException("Ingrese un importe mayor que cero, con hasta cuatro decimales.");
        }
    }
}


