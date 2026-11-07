using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores
{
    public class ClsDatosAlertaCadenaFrio
    {
        // =====================================================
        // IDENTIFICADOR
        // =====================================================

        public long IdAlertaCadenaFrio { get; set; }

        // =====================================================
        // TELEMETRÍA ASOCIADA
        // =====================================================

        [Range(
            1,
            long.MaxValue,
            ErrorMessage = "Seleccione un registro de telemetría válido.")]
        public long FkTelemetriaTemperaturaCodigo
        {
            get;
            set;
        }

        // =====================================================
        // TIPO DE ALERTA
        // =====================================================

        [Required(
            ErrorMessage = "El tipo de alerta es obligatorio.")]
        [StringLength(
            30,
            ErrorMessage = "El tipo de alerta admite hasta 30 caracteres.")]
        public string TipoAlertaCadenaFrio
        {
            get;
            set;
        }

        // =====================================================
        // ESTADO DE ATENCIÓN
        // =====================================================

        public bool AtendidaAlertaCadenaFrio
        {
            get;
            set;
        }

        // =====================================================
        // OBSERVACIÓN DE CALIDAD
        // =====================================================

        [Required(
            ErrorMessage = "La observación es obligatoria.")]
        [StringLength(
            500,
            ErrorMessage = "La observación admite hasta 500 caracteres.")]
        public string ObservacionCalidadAlertaCadenaFrio
        {
            get;
            set;
        }

        // =====================================================
        // CAMPOS PARA CONSULTAS
        // =====================================================

        public string NombreTelemetria
        {
            get;
            set;
        }

        [Browsable(false)]
        public DateTime CreatedAt { get; set; }

        [Browsable(false)]
        public DateTime UpdatedAt { get; set; }

    }
}
