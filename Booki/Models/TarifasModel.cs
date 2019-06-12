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
        public int? NumeroQuartosDisponiveis { get; set; }
    }
    public class SearchDestinosModel
    {
        public string Localizacao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public List<TarifasModel> ListaTarifas { get; set; }
        public List<ArtigosModel> ListaArtigos { get; set; } = new List<ArtigosModel>();
        public List<ReservasModel> ListaReservas { get; set; }
    }
    public class ReservasModel
    {
        public int IdReserva { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string NrHospedes { get; set; }
        public string Preco { get; set; }
        public string PrecoUnidade { get; set; }
        public string LocalizacaoHotel { get; set; }
        public string TipoQuarto { get; set; }
        public string NomeHotel { get; set; }
        public string MoradaCompleta { get; set; }
        public string Classificacao { get; set; }
        public bool Apagado { get; set; }
        public List<ArtigosModel> Artigos { get; set; } = new List<ArtigosModel>();
    }
    public class ArtigosModel
    {
        public int IdArtigo { get; set; }
        public string Designacao { get; set; }
        public bool PermitePreMarcacao { get; set; }
        public decimal Preco { get; set; }
    }
}