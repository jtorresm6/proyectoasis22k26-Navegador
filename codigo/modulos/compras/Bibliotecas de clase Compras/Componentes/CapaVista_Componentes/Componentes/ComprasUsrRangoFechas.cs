using System;
using System.ComponentModel;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Carlos Andres Arriaza Lara
 * Carné : 0901-23-13862
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Administra un rango de fechas válido y notifica sus cambios a los formularios de Compras.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    [DefaultProperty("FechaInicio")]
    [DefaultEvent("RangoCambiado")]
    public partial class ComprasUsrRangoFechas : UserControl
    {
        private bool _ActualizandoRango;
        private DateTime _FechaInicio;
        private DateTime _FechaFin;

        public ComprasUsrRangoFechas()
        {
            InitializeComponent();
            DoubleBuffered = true;
            DateTime Hoy = DateTime.Today;
            ComprasProcEstablecerRango(new DateTime(Hoy.Year, Hoy.Month, 1), Hoy);
            ComprasDtpFechaInicio.ValueChanged += ComprasProcFechaCambiada;
            ComprasDtpFechaFin.ValueChanged += ComprasProcFechaCambiada;
        }

        [Category("Compras")]
        [Description("Tamaño de la fuente de ambas fechas, en puntos.")]
        [DefaultValue(14F)]
        [RefreshProperties(RefreshProperties.All)]
        public float TamanoFuente
        {
            get => ComprasDtpFechaInicio.TamanoFuente;
            set
            {
                ComprasDtpFechaInicio.TamanoFuente = value;
                ComprasDtpFechaFin.TamanoFuente = value;
            }
        }

        [Category("Compras")]
        [Description("Tamano de la fuente de las etiquetas Desde y Hasta, en puntos.")]
        [DefaultValue(13F)]
        [RefreshProperties(RefreshProperties.All)]
        public float TamanoFuenteEtiquetas
        {
            get => ComprasLblFechaInicio.TamanoTipografia;
            set
            {
                ComprasLblFechaInicio.TamanoTipografia = value;
                ComprasLblFechaFin.TamanoTipografia = value;
                ComprasTlpRango.RowStyles[0].Height = Math.Max(28F,
                    Math.Max(ComprasLblFechaInicio.Font.Height, ComprasLblFechaFin.Font.Height) + 4F);
            }
        }

        [Category("Compras")]
        [Description("Fecha inicial sin hora. Si supera la fecha final, ambas quedan en la fecha inicial.")]
        [RefreshProperties(RefreshProperties.All)]
        public DateTime FechaInicio
        {
            get => _FechaInicio;
            set => ComprasProcEstablecerRango(value, value.Date > _FechaFin ? value : _FechaFin);
        }

        [Category("Compras")]
        [Description("Fecha final sin hora. Si precede a la fecha inicial, ambas quedan en la fecha final.")]
        [RefreshProperties(RefreshProperties.All)]
        public DateTime FechaFin
        {
            get => _FechaFin;
            set => ComprasProcEstablecerRango(value.Date < _FechaInicio ? value : _FechaInicio, value);
        }

        [Category("Compras")]
        [Description("Se produce una vez al cambiar el rango, con ambas fechas ya validadas.")]
        public event EventHandler RangoCambiado;

        public void ComprasProcEstablecerRango(DateTime Inicio, DateTime Fin)
        {
            Inicio = Inicio.Date;
            Fin = Fin.Date;
            if (Inicio < DateTimePicker.MinimumDateTime.Date || Inicio > DateTimePicker.MaximumDateTime.Date)
                throw new ArgumentOutOfRangeException(nameof(Inicio));
            if (Fin < DateTimePicker.MinimumDateTime.Date || Fin > DateTimePicker.MaximumDateTime.Date)
                throw new ArgumentOutOfRangeException(nameof(Fin));
            if (Inicio > Fin)
                throw new ArgumentException("La fecha inicial no puede ser posterior a la fecha final.", nameof(Inicio));
            if (_ActualizandoRango || (_FechaInicio == Inicio && _FechaFin == Fin)) return;

            _ActualizandoRango = true;
            try
            {
                ComprasDtpFechaInicio.MaxDate = DateTimePicker.MaximumDateTime;
                ComprasDtpFechaFin.MinDate = DateTimePicker.MinimumDateTime;
                ComprasDtpFechaInicio.Value = Inicio;
                ComprasDtpFechaFin.Value = Fin;
                ComprasDtpFechaInicio.MaxDate = Fin;
                ComprasDtpFechaFin.MinDate = Inicio;
                _FechaInicio = Inicio;
                _FechaFin = Fin;
            }
            finally
            {
                _ActualizandoRango = false;
            }
            RangoCambiado?.Invoke(this, EventArgs.Empty);
        }

        private void ComprasProcFechaCambiada(object Remitente, EventArgs Evento)
        {
            if (_ActualizandoRango) return;
            ComprasProcEstablecerRango(ComprasDtpFechaInicio.Value, ComprasDtpFechaFin.Value);
        }
    }
}
