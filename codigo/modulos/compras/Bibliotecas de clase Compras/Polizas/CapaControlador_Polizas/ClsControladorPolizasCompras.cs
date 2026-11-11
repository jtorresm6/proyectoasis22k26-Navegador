using CapaControlador_Contabilidad;
using CapaModelo_Polizas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaControlador_Polizas
{
    public interface IIntegracionContabilidadPolizasCompras
    {
        List<ClsModeloCuentaPolizaContabilidad> ContabilidadFuncCuentasPermitidas(string CodigoModulo);
        bool ContabilidadFuncRecibirPoliza(string CodigoModulo, string DocumentoOrigen, DateTime Fecha, string Descripcion,
            List<ClsModeloLineaIntegracionContabilidad> Lineas, out string CodigoPoliza, out string Estado, out string Mensaje);
    }

    public interface IRepositorioPolizasCompras
    {
        DataTable ComprasFuncConsultarCuentasPorPagar();
    }

    internal sealed class ClsIntegracionContabilidadPolizasCompras : IIntegracionContabilidadPolizasCompras
    {
        private readonly ClsControladorIntegracionContabilidad _Integracion = new ClsControladorIntegracionContabilidad();

        public List<ClsModeloCuentaPolizaContabilidad> ContabilidadFuncCuentasPermitidas(string CodigoModulo)
        {
            return _Integracion.ContabilidadFuncCuentasPermitidas(CodigoModulo);
        }

        public bool ContabilidadFuncRecibirPoliza(string CodigoModulo, string DocumentoOrigen, DateTime Fecha, string Descripcion,
            List<ClsModeloLineaIntegracionContabilidad> Lineas, out string CodigoPoliza, out string Estado, out string Mensaje)
        {
            return _Integracion.ContabilidadFuncRecibirPoliza(CodigoModulo, DocumentoOrigen, Fecha, Descripcion, Lineas,
                out CodigoPoliza, out Estado, out Mensaje);
        }
    }

    internal sealed class ClsRepositorioPolizasComprasAdaptador : IRepositorioPolizasCompras
    {
        private readonly ClsRepositorioPolizasCompras _Repositorio = new ClsRepositorioPolizasCompras();

        public DataTable ComprasFuncConsultarCuentasPorPagar()
        {
            return _Repositorio.ComprasFuncConsultarCuentasPorPagar();
        }
    }

    public sealed class ClsControladorPolizasCompras
    {
        private const string CodigoModulo = "MOD-COMPR";
        private readonly IIntegracionContabilidadPolizasCompras _Integracion;
        private readonly IRepositorioPolizasCompras _Compras;

        public ClsControladorPolizasCompras()
            : this(new ClsIntegracionContabilidadPolizasCompras(), new ClsRepositorioPolizasComprasAdaptador())
        {
        }

        public ClsControladorPolizasCompras(IIntegracionContabilidadPolizasCompras Integracion, IRepositorioPolizasCompras Compras)
        {
            _Integracion = Integracion ?? throw new ArgumentNullException(nameof(Integracion));
            _Compras = Compras ?? throw new ArgumentNullException(nameof(Compras));
        }

        public List<ClsModeloCuentaPolizaCompras> ContabilidadFuncCuentasCompras()
        {
            return _Integracion.ContabilidadFuncCuentasPermitidas(CodigoModulo)
                .Select(Cuenta => new ClsModeloCuentaPolizaCompras { Codigo = Cuenta.Codigo, Nombre = Cuenta.Nombre })
                .ToList();
        }

        public DataTable ComprasFuncConsultarCuentasPorPagar()
        {
            return _Compras.ComprasFuncConsultarCuentasPorPagar();
        }

        public bool ContabilidadFuncEnviarPoliza(ClsModeloPolizaCompras Poliza,
            out string CodigoPoliza, out string Estado, out string Mensaje)
        {
            CodigoPoliza = string.Empty;
            Estado = string.Empty;
            Mensaje = ContabilidadFuncValidar(Poliza);
            if (Mensaje.Length > 0) return false;
            try
            {
                var Lineas = Poliza.Lineas.Select(Linea => new ClsModeloLineaIntegracionContabilidad
                {
                    CodigoCuenta = Linea.CodigoCuenta.Trim(),
                    Debe = Linea.Debe,
                    Haber = Linea.Haber,
                    Descripcion = Linea.Descripcion
                }).ToList();
                return _Integracion.ContabilidadFuncRecibirPoliza(CodigoModulo,
                    Poliza.DocumentoOrigen.Trim(), Poliza.Fecha, Poliza.Descripcion, Lineas,
                    out CodigoPoliza, out Estado, out Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "No se pudo comunicar con Contabilidad: " + Ex.Message;
                return false;
            }
        }

        private static string ContabilidadFuncValidar(ClsModeloPolizaCompras Poliza)
        {
            if (Poliza == null) return "No hay póliza para enviar.";
            if (string.IsNullOrWhiteSpace(Poliza.DocumentoOrigen) || Poliza.DocumentoOrigen.Trim().Length > 30)
                return "El documento de origen debe tener entre 1 y 30 caracteres.";
            if (Poliza.Descripcion != null && Poliza.Descripcion.Trim().Length > 255)
                return "La descripción admite hasta 255 caracteres.";
            if (Poliza.Lineas == null || Poliza.Lineas.Count < 2)
                return "Agregue al menos dos líneas contables.";
            foreach (var Linea in Poliza.Lineas)
            {
                if (string.IsNullOrWhiteSpace(Linea.CodigoCuenta) || Linea.Debe < 0m || Linea.Haber < 0m ||
                    ((Linea.Debe > 0m) == (Linea.Haber > 0m)))
                    return "Cada línea necesita una cuenta y un importe en Debe o Haber.";
            }
            if (Poliza.Lineas.Sum(x => x.Debe) != Poliza.Lineas.Sum(x => x.Haber))
                return "La póliza no está cuadrada.";
            return string.Empty;
        }
    }
}
