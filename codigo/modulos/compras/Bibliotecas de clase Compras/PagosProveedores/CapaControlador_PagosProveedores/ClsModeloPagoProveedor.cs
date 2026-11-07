using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using CapaModelo_Componentes.Repositorios;
using CapaModelo_PagosProveedores.Entidades;
using CapaModelo_PagosProveedores.Repositorios;
using CapaModelo_PagosProveedores.Contratos;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 20/10/2026
 * ==================================================================
 * Propósito :
 *  Valida los datos de los pagos a proveedores (encabezado y detalle)
 *  y coordina su registro, modificación, eliminación y consulta
 *  mediante el repositorio.
 * ===================================================================
*/

namespace CapaControlador_PagosProveedores
{
    public class ClsModeloPagoProveedor : IValidatableObject
    {
        private int _IdPagoProveedor;
        private int _IdProveedor;
        private string _NombreProveedor;
        private DateTime _FechaPagoProveedor;
        private decimal _MontoPagoProveedor;
        private int _IdMetodoPago;
        private string _NombreMetodoPago;
        private long _IdMoneda;
        private long? _IdTipoCambio;
        private decimal _TipoCambioPagoProveedor = 1;
        private string _ReferenciaPagoProveedor;
        private string _EstadoPagoProveedor;
        private List<ClsModeloDetallePagoProveedor> _Detalles = new List<ClsModeloDetallePagoProveedor>();
        private readonly IRepositorioPagoProveedor _RepositorioPagoProveedor;

        public EnmEstadoEntidadCompras Estado { private get; set; }
        private List<ClsModeloPagoProveedor> _ListaPago;

        public int IdPagoProveedor { get => _IdPagoProveedor; set => _IdPagoProveedor = value; }

        public string NoPagoProveedor => "PG-" + _IdPagoProveedor.ToString("D4");

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor")]
        public int IdProveedor { get => _IdProveedor; set => _IdProveedor = value; }

        public string NombreProveedor { get => _NombreProveedor; private set => _NombreProveedor = value; }

        [Required(ErrorMessage = "El campo Fecha de pago es requerido")]
        public DateTime FechaPagoProveedor { get => _FechaPagoProveedor; set => _FechaPagoProveedor = value; }

        [Required(ErrorMessage = "El campo Monto es requerido")]
        public decimal MontoPagoProveedor { get => _MontoPagoProveedor; set => _MontoPagoProveedor = value; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un método de pago")]
        public int IdMetodoPago { get => _IdMetodoPago; set => _IdMetodoPago = value; }

        public string NombreMetodoPago { get => _NombreMetodoPago; private set => _NombreMetodoPago = value; }

        [Range(1, long.MaxValue, ErrorMessage = "Debe seleccionar una moneda")]
        public long IdMoneda { get => _IdMoneda; set => _IdMoneda = value; }

        // El tipo de cambio y su referencia los resuelve el sistema al grabar (según moneda y fecha)
        public long? IdTipoCambio { get => _IdTipoCambio; private set => _IdTipoCambio = value; }

        public decimal TipoCambioPagoProveedor { get => _TipoCambioPagoProveedor; private set => _TipoCambioPagoProveedor = value; }

        [StringLength(30, ErrorMessage = "La Referencia no puede exceder 30 caracteres")]
        public string ReferenciaPagoProveedor { get => _ReferenciaPagoProveedor; set => _ReferenciaPagoProveedor = value; }

        [Required(ErrorMessage = "El campo Estado es requerido")]
        [RegularExpression("^(Pagado|Programado|Rechazado|Anulado)$", ErrorMessage = "El Estado debe ser Pagado, Programado, Rechazado o Anulado")]
        public string EstadoPagoProveedor { get => _EstadoPagoProveedor; set => _EstadoPagoProveedor = value; }

        public List<ClsModeloDetallePagoProveedor> Detalles { get => _Detalles; set => _Detalles = value ?? new List<ClsModeloDetallePagoProveedor>(); }

        public decimal TotalAplicado => _Detalles.Sum(Detalle => Detalle.MontoAplicado);

        public ClsModeloPagoProveedor() : this(new ClsRepositorioPagoProveedor()) { }

        public ClsModeloPagoProveedor(IRepositorioPagoProveedor Repositorio)
        {
            _RepositorioPagoProveedor = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext ValidationContext)
        {
            // ---- Encabezado ----
            if (_MontoPagoProveedor <= 0)
            {
                yield return new ValidationResult(
                    "El monto debe ser mayor a cero",
                    new[] { nameof(MontoPagoProveedor) });
            }

            if (_MontoPagoProveedor > 99999999.9999m)
            {
                yield return new ValidationResult(
                    "El monto excede el máximo permitido",
                    new[] { nameof(MontoPagoProveedor) });
            }

            if (Estado == EnmEstadoEntidadCompras.Added && _EstadoPagoProveedor != "Pagado" && _EstadoPagoProveedor != "Programado")
            {
                yield return new ValidationResult(
                    "Un pago nuevo solo puede quedar Pagado o Programado",
                    new[] { nameof(EstadoPagoProveedor) });
            }

            if (_TipoCambioPagoProveedor <= 0)
            {
                yield return new ValidationResult(
                    "El tipo de cambio debe ser mayor a cero",
                    new[] { nameof(TipoCambioPagoProveedor) });
            }

            if (!_IdTipoCambio.HasValue && _TipoCambioPagoProveedor != 1)
            {
                yield return new ValidationResult(
                    "Si no hay un tipo de cambio registrado, el valor debe ser 1",
                    new[] { nameof(TipoCambioPagoProveedor) });
            }

            // ---- Detalle ----
            if (_Detalles == null || _Detalles.Count == 0)
            {
                yield return new ValidationResult(
                    "Debe aplicar el pago al menos a un documento por pagar",
                    new[] { nameof(Detalles) });
                yield break;
            }

            foreach (var Detalle in _Detalles)
            {
                var ErroresDetalle = new List<ValidationResult>();
                Validator.TryValidateObject(Detalle, new ValidationContext(Detalle), ErroresDetalle, true);
                foreach (var Error in ErroresDetalle)
                    yield return new ValidationResult(Error.ErrorMessage, new[] { nameof(Detalles) });

                if (Detalle.MontoAplicado <= 0)
                {
                    yield return new ValidationResult(
                        "El monto aplicado al documento " + Detalle.SerieDocumento + "-" + Detalle.NumeroDocumento + " debe ser mayor a cero",
                        new[] { nameof(Detalles) });
                }
            }

            if (_Detalles.Select(Detalle => Detalle.DocumentoClave).Distinct().Count() != _Detalles.Count)
            {
                yield return new ValidationResult(
                    "Un mismo documento no puede aparecer dos veces en el pago",
                    new[] { nameof(Detalles) });
            }

            if (Math.Round(TotalAplicado, 4) != Math.Round(_MontoPagoProveedor, 4))
            {
                yield return new ValidationResult(
                    "El monto del pago debe ser igual a la suma de los montos aplicados a los documentos",
                    new[] { nameof(MontoPagoProveedor) });
            }
        }

        public string ComprasMetGrabarCambios()                
        {
            string Mensaje = null;
            try
            {
                if (Estado != EnmEstadoEntidadCompras.Deleted)
                {
                    // El tipo de cambio vigente a la fecha del pago lo define el sistema
                    if (_IdMoneda > 0)
                    {
                        _RepositorioPagoProveedor.ComprasMetResolverTipoCambio(_IdMoneda, _FechaPagoProveedor, out long? IdTipoCambioResuelto, out decimal ValorTipoCambio);
                        _IdTipoCambio = IdTipoCambioResuelto;
                        _TipoCambioPagoProveedor = ValorTipoCambio;
                    }

                    var Errores = new List<ValidationResult>();
                    if (!Validator.TryValidateObject(this, new ValidationContext(this), Errores, true))
                        return string.Join(Environment.NewLine, Errores.ConvertAll(Error => Error.ErrorMessage));
                }

                var ModeloDatos = new ClsEncabezadoPagoProveedor();
                ModeloDatos.IdPagoProveedor = _IdPagoProveedor;
                ModeloDatos.IdProveedor = _IdProveedor;
                ModeloDatos.FechaPagoProveedor = _FechaPagoProveedor;
                ModeloDatos.MontoPagoProveedor = _MontoPagoProveedor;
                ModeloDatos.IdMetodoPago = _IdMetodoPago;
                ModeloDatos.IdMoneda = _IdMoneda;
                ModeloDatos.IdTipoCambio = _IdTipoCambio;
                ModeloDatos.TipoCambioPagoProveedor = _TipoCambioPagoProveedor;
                ModeloDatos.ReferenciaPagoProveedor = _ReferenciaPagoProveedor;
                ModeloDatos.EstadoPagoProveedor = _EstadoPagoProveedor;
                ModeloDatos.Detalles = _Detalles.ConvertAll(Detalle => new ClsDetallePagoProveedor
                {
                    SerieDocumento = Detalle.SerieDocumento,
                    NumeroDocumento = Detalle.NumeroDocumento,
                    IdConcepto = Detalle.IdConcepto,
                    MontoAplicado = Detalle.MontoAplicado
                });

                switch (Estado)
                {
                    case EnmEstadoEntidadCompras.Added:
                        _IdPagoProveedor = _RepositorioPagoProveedor.ComprasMetAgregar(ModeloDatos);
                        Mensaje = "Grabacion exitosa";
                        break;
                    case EnmEstadoEntidadCompras.Modified:
                        _RepositorioPagoProveedor.ComprasMetEditar(ModeloDatos);
                        Mensaje = "Actualizacion exitosa";
                        break;
                    case EnmEstadoEntidadCompras.Deleted:
                        _RepositorioPagoProveedor.ComprasMetRemover(ModeloDatos);
                        Mensaje = "Eliminacion exitosa";
                        break;
                }
            }
            catch (ClsExcepcionDatosCompras Ex)
            {
                Mensaje = Ex.Message;
            }
            catch (Exception)
            {
                Mensaje = "Ocurrió un error inesperado en el sistema. Intente nuevamente o contacte al administrador.";
            }
            return Mensaje;
        }

        public List<ClsModeloPagoProveedor> ComprasMetObtenerTodos()
        {
            return ComprasMetObtenerTodos(null, null, null, null);
        }

        public List<ClsModeloPagoProveedor> ComprasMetObtenerTodos(int? IdProveedor, string EstadoFiltro, DateTime? Desde, DateTime? Hasta)
        {
            if (IdProveedor.HasValue && IdProveedor.Value <= 0)
                IdProveedor = null;
            if (string.IsNullOrWhiteSpace(EstadoFiltro) || EstadoFiltro == "Todos")
                EstadoFiltro = null;

            var ResultadoConsulta = _RepositorioPagoProveedor.ComprasMetConsultar(IdProveedor, EstadoFiltro, Desde, Hasta);
            _ListaPago = new List<ClsModeloPagoProveedor>();
            foreach (ClsEncabezadoPagoProveedor Item in ResultadoConsulta)
            {
                _ListaPago.Add(ComprasFuncMapearDesdeEntidad(Item));
            }
            return _ListaPago;
        }

        public ClsModeloPagoProveedor ComprasMetObtenerPorId(int IdPago)
        {
            var Item = _RepositorioPagoProveedor.ComprasMetObtenerPorId(IdPago);
            return Item == null ? null : ComprasFuncMapearDesdeEntidad(Item);
        }

        public DataTable ComprasMetObtenerProveedores()
        {
            return _RepositorioPagoProveedor.ComprasMetObtenerProveedores();
        }

        public DataTable ComprasMetObtenerCuentasPendientes(int IdProveedor)
        {
            return _RepositorioPagoProveedor.ComprasMetObtenerCuentasPendientes(IdProveedor);
        }

        public DataTable ComprasMetObtenerMetodosPago()
        {
            return _RepositorioPagoProveedor.ComprasMetObtenerMetodosPago();
        }

        public DataTable ComprasMetObtenerMonedas()
        {
            return _RepositorioPagoProveedor.ComprasMetObtenerMonedas();
        }

        private static ClsModeloPagoProveedor ComprasFuncMapearDesdeEntidad(ClsEncabezadoPagoProveedor Item)
        {
            var Modelo = new ClsModeloPagoProveedor
            {
                _IdPagoProveedor = Item.IdPagoProveedor,
                _IdProveedor = Item.IdProveedor,
                _NombreProveedor = Item.NombreProveedor,
                _FechaPagoProveedor = Item.FechaPagoProveedor,
                _MontoPagoProveedor = Item.MontoPagoProveedor,
                _IdMetodoPago = Item.IdMetodoPago,
                _NombreMetodoPago = Item.NombreMetodoPago,
                _IdMoneda = Item.IdMoneda,
                _IdTipoCambio = Item.IdTipoCambio,
                _TipoCambioPagoProveedor = Item.TipoCambioPagoProveedor,
                _ReferenciaPagoProveedor = Item.ReferenciaPagoProveedor,
                _EstadoPagoProveedor = Item.EstadoPagoProveedor
            };

            if (Item.Detalles != null)
            {
                Modelo._Detalles = Item.Detalles.ConvertAll(Detalle => new ClsModeloDetallePagoProveedor
                {
                    SerieDocumento = Detalle.SerieDocumento,
                    NumeroDocumento = Detalle.NumeroDocumento,
                    IdConcepto = Detalle.IdConcepto,
                    NombreConcepto = Detalle.NombreConcepto,
                    MontoAplicado = Detalle.MontoAplicado
                });
            }
            return Modelo;
        }

    }
}


