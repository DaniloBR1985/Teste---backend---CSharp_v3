using System;
using System.Collections.Generic;
using System.Text;

namespace MoedaConsole.Model
{
    public class DadosCotacao
    {
        public float vlr_cotacao { get; set; }
        public int cod_cotacao { get; set; }
        public Cod_Cotacao moeda { get; set; }
        public DateTime dat_cotacao { get; set; }


    }
}
