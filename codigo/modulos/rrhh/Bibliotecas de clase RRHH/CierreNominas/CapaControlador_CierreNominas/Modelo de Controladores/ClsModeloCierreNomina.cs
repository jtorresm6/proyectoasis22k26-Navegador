using CapaControlador_CierreNominas.Objetos_de_valor;
using CapaControlador_Contabilidad;
using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaModelo_CierreNominas.Entidades;
using CapaModelo_CierreNominas.Repositorios;
using CapaModelo_CierreNominas.Contratos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Odbc;
using System.Linq;

/*
 * ==================================================================
 * Área: Recursos Humanos
 * Autores: Lourdes Isabel Melendez Pineda
 * Fecha o ultima edicion: 31/10/2026
 * ==================================================================
 * Propósito : Clase del controlador que verifica el período de nómina,
 * arma la póliza, la envía a Contabilidad y, si la acepta,
 * guarda el cierre y deja registro en bitácora. El cierre es
 * irreversible, por eso no tiene editar ni eliminar.
 * ===================================================================
 */

namespace CapaControlador_CierreNominas
{
    public class ClsModeloCierreNomina
    {
        private int _IdCierreNomina;
        private string _PeriodoCierreNomina;
        private DateTime _FechaInicioCierreNomina;
        private DateTime _FechaFinalCierreNomina;
        private int _CantidadEmpleado;
        private decimal _TotalPercepcion;
        private decimal _TotalDeduccion;
        private decimal _TotalNeto;
        private decimal _TotalPatronal;
        private decimal _TotalDebe;
        private decimal _TotalHaber;
        private string _DocumentoOrigen;
        private string _CodigoPoliza;
        private DateTime _FechaPoliza;
        private string _EstadoPoliza;
        private int _IdUsuarioCierra;
        private DateTime _FechaEjecucion;
        private DateTime _CreatedAt;
        private DateTime _UpdatedAt;
        private ClsRepositorioCierreNomina _RepositorioCierreNomina;

        private List<ClsModeloCierreNomina> _ListaCierreNomina;

        public int IdCierreNomina { get => _IdCierreNomina; private set => _IdCierreNomina = value; }

        [Required(ErrorMessage = "El campo Mes y Año de la nómina es requerido")]
        public string PeriodoCierreNomina { get => _PeriodoCierreNomina; set => _PeriodoCierreNomina = value; }

        [Required(ErrorMessage = "La fecha de inicio del período es requerida")]
        public DateTime FechaInicioCierreNomina { get => _FechaInicioCierreNomina; set => _FechaInicioCierreNomina = value; }

        [Required(ErrorMessage = "La fecha final del período es requerida")]
        public DateTime FechaFinalCierreNomina { get => _FechaFinalCierreNomina; set => _FechaFinalCierreNomina = value; }

        public int CantidadEmpleadoCierreNomina { get => _CantidadEmpleado; private set => _CantidadEmpleado = value; }
        public decimal TotalPercepcionCierreNomina { get => _TotalPercepcion; private set => _TotalPercepcion = value; }
        public decimal TotalDeduccionCierreNomina { get => _TotalDeduccion; private set => _TotalDeduccion = value; }
        public decimal TotalNetoCierreNomina { get => _TotalNeto; private set => _TotalNeto = value; }
        public decimal TotalPatronalCierreNomina { get => _TotalPatronal; private set => _TotalPatronal = value; }
        public decimal TotalDebeCierreNomina { get => _TotalDebe; private set => _TotalDebe = value; }
        public decimal TotalHaberCierreNomina { get => _TotalHaber; private set => _TotalHaber = value; }

        public string DocumentoOrigenCierreNomina { get => _DocumentoOrigen; private set => _DocumentoOrigen = value; }
        public string CodigoPolizaCierreNomina { get => _CodigoPoliza; private set => _CodigoPoliza = value; }
        public DateTime FechaPolizaCierreNomina { get => _FechaPoliza; private set => _FechaPoliza = value; }
        public string EstadoPolizaCierreNomina { get => _EstadoPoliza; private set => _EstadoPoliza = value; }

        public int IdUsuarioCierra { get => _IdUsuarioCierra; private set => _IdUsuarioCierra = value; }
        public DateTime FechaEjecucionCierreNomina { get => _FechaEjecucion; private set => _FechaEjecucion = value; }

        public DateTime CreatedAt { get => _CreatedAt; private set => _CreatedAt = value; }
        public DateTime UpdatedAt { get => _UpdatedAt; private set => _UpdatedAt = value; }

        public ClsModeloCierreNomina()
        {
            _RepositorioCierreNomina = new ClsRepositorioCierreNomina();
        }

        // Cierra la nómina del período: verifica, arma la póliza, la envía a Contabilidad y guarda el cierre
        public string RrhhMetCerrarNomina()
        {
            string Mensaje = null;
            try
            {
                if (_FechaFinalCierreNomina.Date < _FechaInicioCierreNomina.Date)
                    return "La fecha final no puede ser menor a la fecha inicial.";

                if (_PeriodoCierreNomina != _FechaFinalCierreNomina.ToString("yyyy-MM"))
                    return "El mes y año seleccionado no coincide con las fechas del período.";

                // Se vuelve a verificar contra la base de datos: no se confía en lo que muestra la pantalla
                List<ClsModeloLineaIntegracionContabilidad> Lineas;
                var Resumen = RrhhMetPrepararCierre(_FechaInicioCierreNomina, _FechaFinalCierreNomina, out Lineas);
                if (!Resumen.PuedeCerrar)
                    return Resumen.MensajeBloqueo;

                string DocumentoOrigen = ClsConfiguracionCierreNomina.PrefijoDocumentoOrigen + _PeriodoCierreNomina;
                string CodigoPoliza;
                string EstadoPoliza;
                string MensajeContabilidad;

                var Contabilidad = new ClsControladorIntegracionContabilidad();
                bool PolizaAceptada = Contabilidad.ContabilidadFuncRecibirPoliza(
                    ClsConfiguracionCierreNomina.CodigoModuloContable,
                    ClsConfiguracionCierreNomina.IdPais,
                    DocumentoOrigen,
                    _FechaFinalCierreNomina.Date,
                    "Cierre de nómina " + _PeriodoCierreNomina,
                    Lineas,
                    out CodigoPoliza,
                    out EstadoPoliza,
                    out MensajeContabilidad);

                if (!PolizaAceptada)
                    return "Contabilidad no aceptó la póliza: " + MensajeContabilidad;

                var ModeloDatos = new ClsCierreNomina();
                ModeloDatos.PeriodoCierreNomina = _PeriodoCierreNomina;
                ModeloDatos.FechaInicioCierreNomina = _FechaInicioCierreNomina.Date;
                ModeloDatos.FechaFinalCierreNomina = _FechaFinalCierreNomina.Date;
                ModeloDatos.CantidadEmpleadoCierreNomina = Resumen.EmpleadosEnNomina;
                ModeloDatos.TotalPercepcionCierreNomina = Resumen.TotalPercepciones;
                ModeloDatos.TotalDeduccionCierreNomina = Resumen.TotalDeducciones;
                ModeloDatos.TotalNetoCierreNomina = Resumen.SalarioLiquido;
                ModeloDatos.TotalPatronalCierreNomina = 0m;
                ModeloDatos.TotalDebeCierreNomina = Resumen.TotalDebe;
                ModeloDatos.TotalHaberCierreNomina = Resumen.TotalHaber;
                ModeloDatos.DocumentoOrigenCierreNomina = DocumentoOrigen;
                ModeloDatos.CodigoPolizaCierreNomina = CodigoPoliza;
                ModeloDatos.FechaPolizaCierreNomina = _FechaFinalCierreNomina.Date;
                ModeloDatos.EstadoPolizaCierreNomina = EstadoPoliza;
                ModeloDatos.IdUsuarioCierra = ClsSesionSeguridad.IdUsuario;

                int IdCierre;
                try
                {
                    IdCierre = _RepositorioCierreNomina.RrhhMetAgregar(ModeloDatos);
                }
                catch (Exception ExGuardado)
                {
                    string Detalle = ExGuardado.GetType().Name + ": " + ExGuardado.Message;
                    if (ExGuardado.InnerException != null)
                        Detalle += " | " + ExGuardado.InnerException.Message;

                    // La póliza ya existe en Contabilidad: repetir el cierre la duplicaría
                    return "La póliza " + CodigoPoliza + " fue aceptada por Contabilidad, pero el cierre no se pudo guardar en Recursos Humanos. " +
                           "No repita el cierre y avise al administrador. Detalle: " + Detalle;
                }

                // La bitácora no debe ocultar que el cierre sí se realizó
                try
                {
                    ClsModeloBitacora.SeguridadMetRegistrarAccion("INSERT", "tblCierreNomina", IdCierre,
                        "Se cerró la nómina " + _PeriodoCierreNomina + " con la póliza " + CodigoPoliza);
                }
                catch (Exception)
                {
                }

                Mensaje = "Cierre exitoso. Póliza " + CodigoPoliza + " (" + EstadoPoliza + ")";
            }
            catch (OdbcException OdbcEx)
            {
                Mensaje = OdbcEx.Message;
            }
            catch (Exception Ex)
            {
                Mensaje = Ex.Message;
            }
            return Mensaje;
        }

        
        // Períodos de nómina abiertos para el combo Mes y Año
        public List<ClsPeriodoNominaValor> RrhhMetObtenerPeriodosAbiertos()
        {
            var ListaPeriodos = new List<ClsPeriodoNominaValor>();
            foreach (ClsNominaAbierta Item in _RepositorioCierreNomina.RrhhMetObtenerNominasAbiertas())
            {
                ListaPeriodos.Add(new ClsPeriodoNominaValor
                {
                    PeriodoNomina = Item.PeriodoNomina,
                    FechaInicioNomina = Item.FechaInicioNomina,
                    FechaFinalNomina = Item.FechaFinalNomina,
                    CantidadEmpleadoNomina = Item.CantidadEmpleadoNomina
                });
            }
            return ListaPeriodos;
        }

        // Resumen y grilla del período seleccionado, con la verificación de cuadre
        public ClsResumenCierreNominaValor RrhhMetObtenerPrevio(DateTime FechaInicio, DateTime FechaFinal)
        {
            List<ClsModeloLineaIntegracionContabilidad> Lineas;
            return RrhhMetPrepararCierre(FechaInicio, FechaFinal, out Lineas);
        }

        public List<ClsModeloCierreNomina> RrhhMetObtenerTodos()
        {
            var ResultadoConsulta = _RepositorioCierreNomina.RrhhMetObtenerTodos();
            _ListaCierreNomina = new List<ClsModeloCierreNomina>();
            foreach (ClsCierreNomina Item in ResultadoConsulta)
            {
                _ListaCierreNomina.Add(new ClsModeloCierreNomina
                {
                    _IdCierreNomina = Item.IdCierreNomina,
                    _PeriodoCierreNomina = Item.PeriodoCierreNomina,
                    _FechaInicioCierreNomina = Item.FechaInicioCierreNomina,
                    _FechaFinalCierreNomina = Item.FechaFinalCierreNomina,
                    _CantidadEmpleado = Item.CantidadEmpleadoCierreNomina,
                    _TotalPercepcion = Item.TotalPercepcionCierreNomina,
                    _TotalDeduccion = Item.TotalDeduccionCierreNomina,
                    _TotalNeto = Item.TotalNetoCierreNomina,
                    _TotalPatronal = Item.TotalPatronalCierreNomina,
                    _TotalDebe = Item.TotalDebeCierreNomina,
                    _TotalHaber = Item.TotalHaberCierreNomina,
                    _DocumentoOrigen = Item.DocumentoOrigenCierreNomina,
                    _CodigoPoliza = Item.CodigoPolizaCierreNomina,
                    _FechaPoliza = Item.FechaPolizaCierreNomina,
                    _EstadoPoliza = Item.EstadoPolizaCierreNomina,
                    _IdUsuarioCierra = Item.IdUsuarioCierra,
                    _FechaEjecucion = Item.FechaEjecucionCierreNomina,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt
                });
            }
            return _ListaCierreNomina;
        }

        // Verifica cada empleado, arma las líneas de la póliza y comprueba que Debe y Haber cuadren
        private ClsResumenCierreNominaValor RrhhMetPrepararCierre(DateTime FechaInicio, DateTime FechaFinal,
            out List<ClsModeloLineaIntegracionContabilidad> Lineas)
        {
            var Resumen = new ClsResumenCierreNominaValor();
            Lineas = new List<ClsModeloLineaIntegracionContabilidad>();

            var Previo = _RepositorioCierreNomina.RrhhMetObtenerPrevio(FechaInicio, FechaFinal).ToList();
            foreach (ClsPrevioCierreNomina Item in Previo)
            {
                string Problema = RrhhMetVerificarEmpleado(Item);
                Resumen.Filas.Add(new ClsFilaCierreNominaValor
                {
                    IdNominaEncabezado = Item.IdNominaEncabezado,
                    Codigo = Item.CodigoEmpleado,
                    Empleado = Item.NombreEmpleado,
                    Percepciones = Item.TotalPercepcionNominaEncabezado,
                    Deducciones = Item.TotalDeduccionNominaEncabezado,
                    Verificacion = Problema ?? "Correcto",
                    Verificado = Problema == null
                });
                Resumen.TotalPercepciones += Item.TotalPercepcionNominaEncabezado;
                Resumen.TotalDeducciones += Item.TotalDeduccionNominaEncabezado;
                Resumen.SalarioLiquido += Item.TotalNetoNominaEncabezado;
            }
            Resumen.EmpleadosEnNomina = Previo.Count;

            if (Previo.Count == 0)
            {
                Resumen.TextoCuadre = "Sin nóminas";
                Resumen.MensajeBloqueo = "No hay nóminas abiertas para el período seleccionado.";
                return Resumen;
            }

            int Incorrectos = Resumen.Filas.Count(f => !f.Verificado);
            if (Incorrectos > 0)
            {
                Resumen.TextoCuadre = "Verificación incorrecta";
                Resumen.MensajeBloqueo = "Hay " + Incorrectos + " empleado(s) con verificación incorrecta. Corrija su nómina antes de cerrar.";
                return Resumen;
            }

            string Periodo = FechaFinal.ToString("yyyy-MM");
            Lineas = RrhhMetArmarLineasPoliza(_RepositorioCierreNomina.RrhhMetObtenerLineasPoliza(FechaInicio, FechaFinal),
                Resumen.SalarioLiquido, Periodo);
            Resumen.TotalDebe = Lineas.Sum(l => l.Debe);
            Resumen.TotalHaber = Lineas.Sum(l => l.Haber);

            if (Resumen.TotalDebe != Resumen.TotalHaber)
            {
                Resumen.TextoCuadre = "Póliza descuadrada";
                Resumen.MensajeBloqueo = "La póliza no cuadra: Debe Q " + Resumen.TotalDebe.ToString("N2") +
                                         " y Haber Q " + Resumen.TotalHaber.ToString("N2") + ".";
                return Resumen;
            }

            Resumen.CuadreCorrecto = true;
            Resumen.PuedeCerrar = true;
            Resumen.TextoCuadre = "Cuadre correcto: Debe y Haber Q " + Resumen.TotalDebe.ToString("N2");
            return Resumen;
        }

        // Devuelve null si el empleado está correcto, o el motivo del problema
        private string RrhhMetVerificarEmpleado(ClsPrevioCierreNomina Item)
        {
            if (Item.ConceptosSinCuenta > 0)
                return "Concepto sin cuenta contable";

            if (Item.TotalPercepcionNominaEncabezado - Item.TotalDeduccionNominaEncabezado != Item.TotalNetoNominaEncabezado)
                return "Neto no cuadra";

            if (Item.DetallePercepcion != Item.TotalPercepcionNominaEncabezado ||
                Item.DetalleDeduccion != Item.TotalDeduccionNominaEncabezado)
                return "Detalle no coincide";

            return null;
        }

        // Percepciones al Debe, deducciones al Haber y el salario líquido al Haber de Sueldos por Pagar
        private List<ClsModeloLineaIntegracionContabilidad> RrhhMetArmarLineasPoliza(
            IEnumerable<ClsLineaPolizaNomina> Cuentas, decimal TotalNeto, string Periodo)
        {
            var Lineas = new List<ClsModeloLineaIntegracionContabilidad>();
            foreach (ClsLineaPolizaNomina Cuenta in Cuentas)
            {
                if (Cuenta.MontoTotal <= 0)
                    continue;

                var Linea = new ClsModeloLineaIntegracionContabilidad();
                Linea.CodigoCuenta = Cuenta.CodigoCuenta;
                Linea.Descripcion = Cuenta.NombreCuenta;
                if (Cuenta.EsPercepcion)
                    Linea.Debe = Cuenta.MontoTotal;
                else
                    Linea.Haber = Cuenta.MontoTotal;
                Lineas.Add(Linea);
            }

            if (TotalNeto > 0)
            {
                var LineaLiquido = new ClsModeloLineaIntegracionContabilidad();
                LineaLiquido.CodigoCuenta = ClsConfiguracionCierreNomina.CodigoCuentaSueldosPorPagar;
                LineaLiquido.Haber = TotalNeto;
                LineaLiquido.Descripcion = "Sueldos por pagar nómina " + Periodo;
                Lineas.Add(LineaLiquido);
            }
            return Lineas;
        }
    }
} 