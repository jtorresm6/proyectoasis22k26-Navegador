using CapaModelo_Liquidacion_Rrhh.Entidades;
using CapaModelo_Liquidacion_Rrhh.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_Liquidacion_Rrhh.Modelos_de_Controladores
{
    public class ClsModeloMotivoBaja
    {
       
            private int _IdMotivoBaja;
            private string _NombreMotivoBaja;
            private bool _AplicaIndenmizacionMotivoBaja;
            private bool _EstadoMotivoBaja;

            public int IdMotivoBaja { get => _IdMotivoBaja; set => _IdMotivoBaja = value; }
            public string NombreMotivoBaja { get => _NombreMotivoBaja; set => _NombreMotivoBaja = value; }
            public bool AplicaIndenmizacionMotivoBaja { get => _AplicaIndenmizacionMotivoBaja; set => _AplicaIndenmizacionMotivoBaja = value; }
            public bool EstadoMotivoBaja { get => _EstadoMotivoBaja; set => _EstadoMotivoBaja = value; }
           

            // Texto listo para mostrar en el reporte
            public string EstadoTexto { get => _EstadoMotivoBaja ? "Activa" : "Inactiva"; }

            private ClsRepositorioMotivoBaja _RepositorioMotivoBaja = new ClsRepositorioMotivoBaja();

            public List<ClsModeloMotivoBaja> MotivoBajaMetObtenerParaReporte()
            {
                var Resultado = _RepositorioMotivoBaja.RrhhMetObtenerTodos();
                var Lista = new List<ClsModeloMotivoBaja>();

                foreach (ClsMotivoBaja Item in Resultado)
                {
                    Lista.Add(new ClsModeloMotivoBaja()
                    {
                        _IdMotivoBaja = Item.IdMotivoBaja,
                        _NombreMotivoBaja = Item.NombreMotivoBaja,
                        _AplicaIndenmizacionMotivoBaja = Item.AplicaIndenmizacionMotivoBaja,
                        _EstadoMotivoBaja = Item.EstadoMotivoBaja,
                        
                    });
                }

                return Lista;
            }
        }
    }

