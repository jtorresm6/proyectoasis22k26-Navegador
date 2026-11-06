using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CapaModelo_DisponibilidadDiariaBancos.Entidades;
using CapaModelo_DisponibilidadDiariaBancos.Repositorio;
using CapaModelo_DisponibilidadDiariaBancos.Servicios;

namespace CapaControlador_DisponibilidadDiariaBancos
{
    public class ClsControladorDisponibilidad
    {
        private readonly IRepositorioDisponibilidad _repositorio;
        private readonly IProveedorTipoCambioDisponibilidad _tiposCambio;
        private int? _usuarioRegistroId;
        private string _usuarioRegistro;

        public ClsControladorDisponibilidad() : this(new ClsRepositorioDisponibilidad()) { }

        public ClsControladorDisponibilidad(IRepositorioDisponibilidad repositorio)
            : this(repositorio, new ClsProveedorTipoCambioDisponibilidad()) { }

        public ClsControladorDisponibilidad(
            IRepositorioDisponibilidad repositorio,
            IProveedorTipoCambioDisponibilidad tiposCambio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            _tiposCambio = tiposCambio ?? throw new ArgumentNullException(nameof(tiposCambio));
        }

        public Task<ClsCotizacionDisponibilidad> BancosMetObtenerTipoCambioAsync(
            string moneda, DateTime fecha, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(moneda) ||
                string.Equals(moneda.Trim(), "Todas", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Selecciona la moneda de la cuenta.");
            ValidarConsulta(fecha, null, null, moneda);
            return _tiposCambio.BancosMetObtenerTasaAsync(
                moneda.Trim().ToUpperInvariant(), fecha.Date, token);
        }

        // La Vista obtiene estos datos de la sesion real de Seguridad. En una
        // ejecucion sin sesion se conserva NULL; nunca se atribuye a un usuario ficticio.
        public void BancosMetEstablecerUsuarioRegistro(int? usuarioId, string nombreUsuario)
        {
            if (usuarioId.HasValue && usuarioId.Value <= 0)
                throw new ArgumentException("El identificador de usuario debe ser válido.");
            string nombre = string.IsNullOrWhiteSpace(nombreUsuario) ? null : nombreUsuario.Trim();
            if (nombre != null && nombre.Length > 100)
                throw new ArgumentException("El nombre del usuario no puede superar 100 caracteres.");
            _usuarioRegistroId = usuarioId;
            _usuarioRegistro = usuarioId.HasValue ? nombre : null;
        }

        public Task<ClsCatalogosDisponibilidad> BancosMetObtenerCatalogosAsync(CancellationToken token)
        {
            return Task.Run(() => _repositorio.BancosMetObtenerCatalogos(token), token);
        }

        public Task<ClsConsultaDisponibilidad> BancosMetConsultarAsync(
            DateTime fecha, long? bancoId, long? cuentaId, string moneda, CancellationToken token)
        {
            ValidarConsulta(fecha, bancoId, cuentaId, moneda);
            return Task.Run(() =>
            {
                ClsConsultaDisponibilidad consulta = _repositorio.BancosMetConsultar(fecha.Date, bancoId, cuentaId, moneda, token);
                token.ThrowIfCancellationRequested();
                if (consulta == null || consulta.Detalle == null || consulta.Compromisos == null)
                    throw new InvalidOperationException("La consulta no devolvió una estructura válida.");
                consulta.Resumen = CalcularResumen(consulta.Detalle, moneda, consulta.TieneTabla);
                consulta.MonedaResumen = consulta.Resumen.Moneda;
                if (consulta.Resumen.CuentasSinTasa > 0)
                    consulta.Mensaje = (consulta.Mensaje ?? "") + " Faltan tasas válidas para " +
                        consulta.Resumen.CuentasSinTasa + " cuenta(s). El consolidado en GTQ no está disponible.";
                return consulta;
            }, token);
        }

        public Task BancosMetGuardarAsync(ClsDisponibilidad registro, bool reemplazar, CancellationToken token)
        {
            ClsRepositorioDisponibilidad.BancosMetValidarRegistro(registro);
            var copia = new ClsDisponibilidad
            {
                CuentaId = registro.CuentaId, BancoId = registro.BancoId, Banco = registro.Banco,
                NumeroCuenta = registro.NumeroCuenta, TipoCuenta = registro.TipoCuenta,
                Moneda = registro.Moneda.Trim().ToUpperInvariant(), FechaCorte = registro.FechaCorte.Date,
                SaldoInicial = registro.SaldoInicial, Ingresos = registro.Ingresos, Egresos = registro.Egresos,
                Comprometido = registro.Comprometido, Restringido = registro.Restringido,
                TipoCambioAplicado = registro.TipoCambioAplicado, DisponibleGtq = registro.DisponibleGtq,
                Fuente = registro.Fuente.Trim(), FechaGeneracion = registro.FechaGeneracion,
                VersionRegistro = registro.VersionRegistro,
                UsuarioRegistroId = _usuarioRegistroId, UsuarioRegistro = _usuarioRegistro
            };
            return Task.Run(() => _repositorio.BancosMetGuardar(copia, reemplazar, token), token);
        }

        private static ClsResumenDisponibilidad CalcularResumen(
            List<ClsDisponibilidad> registros, string moneda, bool tieneTabla)
        {
            bool todas = string.IsNullOrWhiteSpace(moneda) ||
                string.Equals(moneda.Trim(), "Todas", StringComparison.OrdinalIgnoreCase);
            string codigo = todas ? "GTQ" : moneda.Trim().ToUpperInvariant();
            var resumen = new ClsResumenDisponibilidad
            {
                EsConsolidado = todas, Moneda = codigo, ConversionCompleta = false
            };
            if (!tieneTabla || registros.Count == 0) return resumen;

            foreach (ClsDisponibilidad registro in registros)
            {
                if (registro == null || string.IsNullOrWhiteSpace(registro.Moneda))
                    throw new InvalidOperationException("El resultado contiene una cuenta sin moneda.");
                if (!todas && !string.Equals(registro.Moneda.Trim(), codigo, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("La consulta contiene una moneda ajena al filtro seleccionado.");
                if (registro.SaldoInicial < 0m || registro.Ingresos < 0m || registro.Egresos < 0m ||
                    registro.Comprometido < 0m || registro.Restringido < 0m)
                    throw new InvalidOperationException("El resultado contiene componentes negativos no válidos.");
            }
            if (todas)
            {
                resumen.CuentasSinTasa = registros.Count(r =>
                    !string.Equals(r.Moneda.Trim(), "GTQ", StringComparison.OrdinalIgnoreCase) &&
                    (!r.TipoCambioAplicado.HasValue || r.TipoCambioAplicado.Value <= 0m));
                if (resumen.CuentasSinTasa > 0) return resumen;
            }
            decimal saldo = 0m, ingresos = 0m, egresos = 0m, comprometido = 0m, restringido = 0m;
            foreach (ClsDisponibilidad registro in registros)
            {
                decimal factor = !todas || string.Equals(registro.Moneda.Trim(), "GTQ", StringComparison.OrdinalIgnoreCase)
                    ? 1m : registro.TipoCambioAplicado.Value;
                saldo += registro.SaldoInicial * factor;
                ingresos += registro.Ingresos * factor;
                egresos += registro.Egresos * factor;
                comprometido += registro.Comprometido * factor;
                restringido += registro.Restringido * factor;
            }
            resumen.SaldoInicial = saldo;
            resumen.Ingresos = ingresos;
            resumen.Egresos = egresos;
            resumen.Comprometido = comprometido;
            resumen.Restringido = restringido;
            resumen.Disponible = saldo + ingresos - egresos - comprometido - restringido;
            resumen.ConversionCompleta = true;
            return resumen;
        }

        private static void ValidarConsulta(DateTime fecha, long? bancoId, long? cuentaId, string moneda)
        {
            if (fecha.Date > DateTime.Today) throw new ArgumentException("No se admiten cortes futuros.");
            if (fecha.Date < new DateTime(1000, 1, 1)) throw new ArgumentException("La fecha está fuera del rango de MySQL.");
            if (bancoId.HasValue && bancoId.Value <= 0) throw new ArgumentException("Selecciona un banco válido.");
            if (cuentaId.HasValue && cuentaId.Value <= 0) throw new ArgumentException("Selecciona una cuenta válida.");
            if (string.IsNullOrWhiteSpace(moneda) || string.Equals(moneda.Trim(), "Todas", StringComparison.OrdinalIgnoreCase)) return;
            string codigo = moneda.Trim().ToUpperInvariant();
            if (codigo.Length != 3 || codigo.Any(c => c < 'A' || c > 'Z'))
                throw new ArgumentException("Selecciona un código de moneda válido.");
        }
    }
}
