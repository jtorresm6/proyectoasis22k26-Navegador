using System;

namespace CapaModelo_TipoCambio.Entidades
{
    public class ClsMonedaCatalogoLocal
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Simbolo { get; set; }
        public bool Activa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public long? IdBanguat { get { return ClsCatalogoMonedasBanguat.BancosMetIdDeCodigo(Codigo); } }
        public bool RequiereMigrarId { get { return !IdBanguat.HasValue || Id != IdBanguat.Value; } }
        public string Clase { get { return ClsCatalogoMonedasBanguat.BancosMetClase(Codigo); } }
    }

    public class ClsResultadoSincronizacionMonedas
    {
        public int TotalApi { get; set; }
        public int Agregadas { get; set; }
        public int Actualizadas { get; set; }
        public int IdsMigrados { get; set; }
        public long ReferenciasMigradas { get; set; }
        public string Mensaje { get; set; }
    }
}
