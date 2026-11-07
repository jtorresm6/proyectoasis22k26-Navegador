using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores
{
    public class ClsDatosEncabezadoViaje
    {
        // Identificador del viaje.
        public long IdEncabezadoViaje { get; set; }

        // Fecha del documento.
        [Required(
            ErrorMessage = "La fecha del documento es obligatoria.")]
        public DateTime FechaDocumentoEncabezadoViaje
        {
            get;
            set;
        }

        // Fecha y hora de salida.
        [Required(
            ErrorMessage = "La fecha y hora de salida son obligatorias.")]
        public DateTime HoraSalidaEncabezadoViaje
        {
            get;
            set;
        }

        // La llegada puede ser nula.
        public DateTime? HoraLlegadaEncabezadoViaje
        {
            get;
            set;
        }

        // Indica si requiere cadena de frío.
        public bool RequiereCadenaFrioEncabezadoViaje
        {
            get;
            set;
        }

        // Estado del viaje.
        [Required(
            ErrorMessage = "El estado del viaje es obligatorio.")]
        [StringLength(
            30,
            ErrorMessage = "El estado admite hasta 30 caracteres.")]
        public string EstadoViajeEncabezadoViaje
        {
            get;
            set;
        }

        // Peso total del viaje.
        [Range(
            typeof(decimal),
            "0",
            "999999999999999.999",
            ErrorMessage = "El peso total debe ser mayor o igual a cero.")]
        public decimal PesoTotalEncabezadoViaje
        {
            get;
            set;
        }

        // Vehículo asociado.
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Seleccione un vehículo válido.")]
        public int Fk_Transporte_Codigo
        {
            get;
            set;
        }

        // Conductor asociado.
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Seleccione un conductor válido.")]
        public int Fk_Conductor_Codigo
        {
            get;
            set;
        }

        // Campos adicionales obtenidos mediante JOIN.

        [Browsable(false)]
        public DateTime CreatedAt
        {
            get;
            set;
        }

        [Browsable(false)]
        public DateTime UpdatedAt
        {
            get;
            set;
        }

        // Información legible para el DataGridView.
        public string PlacaTransporte
        {
            get;
            set;
        }

        public string NombreConductor
        {
            get;
            set;
        }
    }
}
