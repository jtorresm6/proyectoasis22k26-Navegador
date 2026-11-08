using CapaModelo_MovimientosBancarios.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_MovimientosBancarios.Contratos
{
    public interface IRepositorioBeneficiario : IRepositorioGenerico<ClsBeneficiario>
    {
        IEnumerable<ClsBeneficiario> BancosMetObtenerPorTipo(string TipoBeneficiario);
        IEnumerable<ClsCuentaBancariaTercero>BancosMetObtenerCuentasPorBeneficiario(string TipoBeneficiario, long IdBeneficiario);
    }
}
