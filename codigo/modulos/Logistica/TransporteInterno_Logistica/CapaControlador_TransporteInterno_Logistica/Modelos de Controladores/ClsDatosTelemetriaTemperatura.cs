using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores
{
    public class ClsDatosTelemetriaTemperatura
    {
        // =====================================================
        // IDENTIFICADOR
        // =====================================================

        public long IdTelemetriaTemperatura { get; set; }

        // =====================================================
        // VIAJE ASOCIADO
        // =====================================================

        [Range(
            1,
            long.MaxValue,
            ErrorMessage = "Seleccione un viaje válido.")]
        public long FkEncabezadoViajeCodigo { get; set; }

        // =====================================================
        // FECHA Y HORA
        // =====================================================

        [Required(
            ErrorMessage = "La fecha y hora son obligatorias.")]
        public DateTime FechaHoraTelemetriaTemperatura
        {
            get;
            set;
        }

        // =====================================================
        // TEMPERATURA
        // =====================================================

        [Range(
            typeof(decimal),
            "-999.99",
            "999.99",
            ErrorMessage = "La temperatura debe estar entre " +
                           "-999.99 y 999.99 grados Celsius.")]
        public decimal TemperaturaCelsiusTelemetriaTemperatura
        {
            get;
            set;
        }

        // =====================================================
        // HUMEDAD RELATIVA
        // =====================================================

        [Range(
            typeof(decimal),
            "0",
            "1",
            ErrorMessage = "La humedad relativa debe estar " +
                           "entre 0.0000 y 1.0000.")]
        public decimal HumedadRelativaTelemetriaTemperatura
        {
            get;
            set;
        }

        // =====================================================
        // PUERTA ABIERTA
        // =====================================================

        public bool PuertaAbiertaTelemetriaTemperatura
        {
            get;
            set;
        }

        // =====================================================
        // UBICACIÓN GPS
        // =====================================================

        [Required(
            ErrorMessage = "La ubicación GPS es obligatoria.")]
        [StringLength(
            100,
            ErrorMessage = "La ubicación GPS admite " +
                           "hasta 100 caracteres.")]
        public string UbicacionGpsTelemetriaTemperatura
        {
            get;
            set;
        }

        // =====================================================
        // CAMPOS PARA CONSULTAS
        // =====================================================

        public string NombreViaje { get; set; }

        [Browsable(false)]
        public DateTime CreatedAt { get; set; }

        [Browsable(false)]
        public DateTime UpdatedAt { get; set; }
    }
}
