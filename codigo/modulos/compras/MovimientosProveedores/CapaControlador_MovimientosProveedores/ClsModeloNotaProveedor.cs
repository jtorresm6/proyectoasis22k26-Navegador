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
            var Nota = new ClsNotaProveedor {
                IdProveedor = Proveedor, IdConceptoProveedor = Concepto, Serie = Serie, Numero = Numero,
                FechaEmision = Emision, FechaVencimiento = Vencimiento, IdMoneda = Moneda,
                TipoCambio = Cambio, Monto = Monto
            };
            Nota.ComprasMetValidar();
            _Repositorio.ComprasMetAgregar(Nota);
        }
    }
}


