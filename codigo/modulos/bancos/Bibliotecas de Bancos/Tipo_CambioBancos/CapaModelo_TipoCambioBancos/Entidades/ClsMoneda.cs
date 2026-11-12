namespace CapaModelo_TipoCambio.Entidades
{
    public class ClsMoneda
    {
        public int IdBanguat { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool DolaresPorUnidad { get; set; }

        public override string ToString()
        {
            return Codigo + " - " + Nombre;
        }
    }
}
