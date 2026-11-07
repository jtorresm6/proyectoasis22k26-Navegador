using System.Data;
using System;
using CapaModelo_MovimientosProveedores.Contratos;
using CapaModelo_MovimientosProveedores.Entidades;
using CapaModelo_MovimientosProveedores.Repositorios;

namespace CapaControlador_MovimientosProveedores
{
    public sealed class ClsModeloNotaProveedor
    {
        private readonly IRepositorioNotasProveedor _Repositorio;

        public ClsModeloNotaProveedor() : this(new ClsRepositorioNotasProveedor()) { }

        public ClsModeloNotaProveedor(IRepositorioNotasProveedor Repositorio)
        {
            if (Repositorio == null) throw new ArgumentNullException("Repositorio");
            _Repositorio = Repositorio;
        }
        public DataTable ComprasMetObtenerProveedores() { return _Repositorio.ComprasMetObtenerProveedores(); }
        public DataTable ComprasMetObtenerConceptos() { return _Repositorio.ComprasMetObtenerConceptos(); }
        public DataTable ComprasMetObtenerMonedas() { return _Repositorio.ComprasMetObtenerMonedas(); }
        public DataTable ComprasMetConsultarNotas(int IdProveedor) { return _Repositorio.ComprasMetConsultarNotas(IdProveedor); }
        public void ComprasMetAgregar(int Proveedor, int Concepto, string Serie, string Numero,
            DateTime Emision, DateTime Vencimiento, int Moneda, decimal Cambio, decimal Monto)
        {
            IdProveedor = Proveedor;
            IdConceptoProveedor = Concepto;
            this.Serie = Serie;
            this.Numero = Numero;
            FechaEmision = Emision;
            FechaVencimiento = Vencimiento;
            IdMoneda = Moneda;
            TipoCambio = Cambio;
            this.Monto = Monto;
            Estado = EstadoEntidad.Added;
            GrabarCambios();
        }

        public int IdProveedor { get; set; }
        public int IdConceptoProveedor { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int IdMoneda { get; set; }
        public decimal TipoCambio { get; set; }
        public decimal Monto { get; set; }
        public EstadoEntidad Estado { private get; set; }

        public string GrabarCambios()
        {
            // Las notas registradas no se editan ni eliminan desde este proceso contable.
            if (Estado != EstadoEntidad.Added)
                throw new InvalidOperationException("Este proceso solo permite agregar notas de proveedor.");
            ComprasMetValidar();
            var Nota = new ClsNotaProveedor {
                IdProveedor = IdProveedor, IdConceptoProveedor = IdConceptoProveedor,
                Serie = Serie, Numero = Numero, FechaEmision = FechaEmision,
                FechaVencimiento = FechaVencimiento, IdMoneda = IdMoneda,
                TipoCambio = TipoCambio, Monto = Monto
            };
            _Repositorio.ComprasMetAgregar(Nota);
            return "Registro agregado correctamente.";
        }

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
