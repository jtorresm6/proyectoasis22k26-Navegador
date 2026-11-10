/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using CapaModelo_MovimientosBancarios.Contratos;
using CapaModelo_MovimientosBancarios.Entidades;
using CapaModelo_MovimientosBancarios.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_MovimientosBancarios.Modelo_de_Controladores
{
    public class ClsModeloMovimientoBancario
    {
        private readonly IRepositorioMovimientoBancario _Repositorio;

        public ClsModeloMovimientoBancario()
        {
            _Repositorio = new ClsRepositorioMovimientoBancario();
        }

        public long BancosMetRegistrarMovimiento(
            ClsRegistroMovimientoBancario Movimiento)
        {
            if (Movimiento == null)
            {
                throw new ArgumentNullException(nameof(Movimiento));
            }

            if (Movimiento.IdConceptoBancario <= 0)
            {
                throw new ArgumentException("Debe seleccionar un concepto bancario.");
            }

            if (Movimiento.IdCuentaEmpresa <= 0)
            {
                throw new ArgumentException("Debe seleccionar una cuenta empresarial.");
            }

            if (Movimiento.IdTipoTransaccion <= 0)
            {
                throw new ArgumentException("Debe especificar un tipo de transacción.");
            }

            if (Movimiento.Importe <= 0)
            {
                throw new ArgumentException("El importe debe ser mayor que cero.");
            }

            if (Movimiento.ImporteGTQ != Movimiento.Importe ||
                Movimiento.TipoCambioAplicado != 1.0000m)
            {
                throw new ArgumentException("Por ahora solo se permiten movimientos en GTQ.");
            }

            return _Repositorio.BancosMetRegistrarMovimiento(Movimiento);
        }

        public long BancosMetRegistrarMovimientoDesdeVista(string Referencia, long IdConcepto, long IdMetodo, long IdTipoTransaccion, long IdCuentaEmpresa, long? IdCuentaTercero, long? IdBeneficiario, string TipoBeneficiario, string Concepto, string Naturaleza, decimal Monto, DateTime FechaOperacion, DateTime FechaAplicacion)
        {
            ClsRegistroMovimientoBancario Movimiento =
                new ClsRegistroMovimientoBancario
                {
                    ReferenciaMovimiento = Referencia,
                    IdConceptoBancario = IdConcepto,
                    IdMetodoPago = IdMetodo,
                    IdTipoTransaccion = IdTipoTransaccion,
                    IdCuentaEmpresa = IdCuentaEmpresa,
                    IdCuentaTercero = IdCuentaTercero,
                    IdBeneficiario = IdBeneficiario,
                    TipoBeneficiario = TipoBeneficiario,
                    Concepto = Concepto,
                    Naturaleza = Naturaleza,
                    Importe = Monto,
                    ImporteGTQ = Monto,
                    TipoCambioAplicado = 1.0000m,
                    FechaOperacion = FechaOperacion,
                    FechaAplicacion = FechaAplicacion
                };

            return BancosMetRegistrarMovimiento(Movimiento);
        }
        public DataTable BancosMetObtenerMovimientosTabla()
        {
            return _Repositorio.BancosMetObtenerMovimientosTabla();
        }

    }
}
