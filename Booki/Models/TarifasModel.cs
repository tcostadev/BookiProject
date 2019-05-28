using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Booki.Models
{
    public class TarifasModel
    {
        public int IdTarifa { get; set; }
        public int IdHotel { get; set; }

        public decimal PrecoUnidade { get; set; }
        public string DesignacaoTipoQuarto { get; set; }
        public string Capacidade { get; set; }
        public string Classificacao { get; set; }
        public string NomeHotel { get; set; }
        public string LocalizacaoHotel { get; set; }
        public string MoradaHotel { get; set; }
    }
    public class SearchDestinosModel
    {
        public List<TarifasModel> ListaTarifas { get; set; }
    }
}