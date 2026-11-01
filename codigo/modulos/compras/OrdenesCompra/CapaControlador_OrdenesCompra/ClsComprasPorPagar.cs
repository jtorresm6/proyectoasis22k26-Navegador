using System;
using System.Data;
using CapaModelo_Componentes.Repositorios;
using System.Globalization;
using CapaModelo_OrdenesCompra.Contratos;
using CapaControlador_OrdenesCompra.Entidades;
using CapaModelo_OrdenesCompra.Repositorios;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Valida y coordina la creación, consulta, filtrado y anulación de
 *  compras por pagar, así como la actualización de su vencimiento.
 * ===================================================================
*/

namespace CapaControlador_OrdenesCompra
{
    public sealed class ClsComprasPorPagar
    {
        private readonly IRepositorioComprasPorPagar _Repositorio;
        public ClsComprasPorPagar() : this(new ClsRepositorioComprasPorPagar()) { }
        public ClsComprasPorPagar(IRepositorioComprasPorPagar Repositorio)
        {
            _Repositorio = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
        }

        public DataTable ComprasFuncListar() => _Repositorio.ComprasFuncListar();
        public DataTable ComprasFuncListarProveedores() => _Repositorio.ComprasFuncListarProveedores();

        public ClsCompraPorPagar ComprasFuncAnular(int Id)
        {
            if (Id <= 0) throw new ArgumentOutOfRangeException(nameof(Id));
            _Repositorio.ComprasProcAnular(Id);
            var Compra = ComprasFuncObtener(Id);
            if (Compra == null || !string.Equals(Compra.Estado, "Anulada", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("No se pudo anular la orden. Actualiza la lista y vuelve a intentarlo.");
            return Compra;
        }

        public void ComprasProcActualizarVencimiento(int Id, DateTime Emision, DateTime Fecha)
        {
            if (Id <= 0) throw new ArgumentOutOfRangeException(nameof(Id));
            if (Fecha.Date < Emision.Date) throw new ArgumentException("El vencimiento no puede ser anterior a la emisión.");
            _Repositorio.ComprasProcActualizarVencimiento(Id, Fecha.Date);
        }

        public int ComprasFuncCrear(int IdProveedor, string Factura, DateTime Emision, DateTime Vencimiento, decimal Monto)
        {
            Factura = (Factura ?? string.Empty).Trim();
            if (IdProveedor <= 0) throw new ArgumentException("Selecciona un proveedor.");
            if (Factura.Length == 0 || Factura.Length > 30)
                throw new ArgumentException("La factura debe tener entre 1 y 30 caracteres.");
            if (Emision.Date > Vencimiento.Date)
                throw new ArgumentException("El vencimiento no puede ser anterior a la emision.");
            if (Monto <= 0 || Monto > 99999999.9999M || decimal.Round(Monto, 4) != Monto)
                throw new ArgumentException("El importe debe ser positivo, con un maximo de ocho enteros y cuatro decimales.");
            try { return _Repositorio.ComprasFuncCrear(IdProveedor, Factura, Emision.Date, Vencimiento.Date, Monto); }
            catch (ClsExcepcionDatosCompras Ex)
            {
                if (Ex.Codigo == 1062)
                    throw new InvalidOperationException("Ya existe una compra con esa factura para el proveedor seleccionado.", Ex);
                throw;
            }
        }

        public ClsCompraPorPagar ComprasFuncObtener(int Id)
        {
            if (Id <= 0) throw new ArgumentOutOfRangeException(nameof(Id));
            var Compra = _Repositorio.ComprasFuncObtener(Id);
            return Compra == null ? null : new ClsCompraPorPagar(Compra);
        }

        public DataTable ComprasFuncFiltrar(DataTable Datos, string Palabra, string Estado, DateTime? Inicio, DateTime? Fin)
        {
            if (Datos == null) throw new ArgumentNullException(nameof(Datos));
            if (Inicio.HasValue != Fin.HasValue || (Inicio.HasValue && Inicio.Value.Date > Fin.Value.Date))
                throw new ArgumentException("El rango de fechas no es valido.");
            if (!string.IsNullOrEmpty(Estado) && Estado != "Todos" && Estado != "Pendiente" &&
                Estado != "Parcial" && Estado != "Pagada" && Estado != "Anulada")
                throw new ArgumentException("Estado no valido.", nameof(Estado));
            string Buscar = (Palabra ?? string.Empty).Trim();
            DataTable Resultado = Datos.Clone();
            foreach (DataRow Fila in Datos.Rows)
            {
                if (Fila.RowState == DataRowState.Deleted) continue;
                if (!string.IsNullOrEmpty(Estado) && Estado != "Todos" &&
                    !string.Equals(Convert.ToString(Fila["estadoCuentaPorPagar"]), Estado, StringComparison.OrdinalIgnoreCase)) continue;
                DateTime Emision = Convert.ToDateTime(Fila["fechaEmisionCuentaPorPagar"]).Date;
                if (Inicio.HasValue && (Emision < Inicio.Value.Date || Emision > Fin.Value.Date)) continue;
                bool Coincide = Buscar.Length == 0;
                foreach (string Campo in new[] { "numeroFacturaCuentaPorPagar", "nombreProveedor", "nitProveedor" })
                    if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(Convert.ToString(Fila[Campo]), Buscar,
                        CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0) Coincide = true;
                if (Coincide) Resultado.ImportRow(Fila);
            }
            return Resultado;
        }
    }
}
