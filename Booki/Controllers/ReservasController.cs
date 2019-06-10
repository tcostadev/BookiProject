using Booki.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Booki.Helper;

namespace Booki.Controllers
{
    public class ReservasController : BaseController
    {
        // GET: Reservas
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult VerReservasUtilizador()
        {
            ViewBag.LoggedIn = IsLoggedIn();

            var utilizador = (LoginModel)Session[SessionUtilizador];

            var listaReservas = new List<ReservasModel>();

            var getReservasUser = $@"SELECT 
	                                    rh.data_inicio,
	                                    rh.data_fim,
	                                    rh.nr_hospedes,
	                                    th.preco_unidade,
	                                    tp.designacao,
	                                    h.nome,
	                                    h.classificacao,
	                                    rh.preco_total,
                                        
	                                    CONCAT(h.morada, ', ', h.codigo_postal, ' - ', l.localizacao) as morada,
	
	                                    rh.id_reserva_hotel,
                                        rh.apagado

                                        FROM reserva_hotel rh

		                                    left join tarifas_hotel th on th.id_tarifa = rh.id_tarifa_hotel
		                                    left join hotel h on h.id_hotel = th.id_hotel
		                                    left join localizacao l on l.id_localizacao = h.id_localizacao
		                                    left join tipo_quarto tp on tp.id_tipo_quarto = th.id_tipo_quarto

                                            WHERE 1=1
                                                AND rh.id_utilizador = {utilizador.IdUser}
                                            ORDER BY rh.data_inicio DESC";

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(getReservasUser, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var newTarifa = new ReservasModel
                        {
                            IdReserva = Convert.ToInt32(reader["id_reserva_hotel"]),
                            DataInicio = Convert.ToDateTime(reader["data_inicio"]),
                            DataFim = Convert.ToDateTime(reader["data_fim"]),

                            NomeHotel = reader["nome"].ToString(),
                            MoradaCompleta = reader["morada"].ToString(),
                            Classificacao = reader["classificacao"].ToString(),

                            NrHospedes = reader["nr_hospedes"].ToString(),
                            TipoQuarto = reader["designacao"].ToString(),
                            Preco = reader["preco_total"].ToString(),
                            PrecoUnidade = reader["preco_unidade"].ToString(),
                            Apagado = Convert.ToBoolean(reader["apagado"])
                        };
                        
                        listaReservas.Add(newTarifa);
                    }
                }

                connection.Close();
            }


            return View("ViewReservas", listaReservas);
        }
        public ActionResult SearchDestinos(DateTime dataInicio, DateTime dataFim, string localizacao)
        {
            ViewBag.LoggedIn = IsLoggedIn();

            var model = new SearchDestinosModel
            {
                DataInicio = Convert.ToDateTime(dataInicio),
                DataFim = Convert.ToDateTime(dataFim),
                Localizacao = string.IsNullOrEmpty(localizacao) ? "": localizacao.ToUpper()
            };

            var dataInicioDt = Convert.ToDateTime(dataInicio);
            var dataFimDt = Convert.ToDateTime(dataInicio);
            var listaTarifas = new List<TarifasModel>();

            var getTarifasSql = $@"select 
                                    th.id_tarifa,
                                    th.preco_unidade, 
                                    tp.designacao, 
                                    tp.capacidade, 
                                    h.nome, 
                                    l.localizacao,
                                    h.codigo_postal + ' - '+ h.morada as morada,
                                    h.id_hotel,
                                    h.classificacao,

                                    -- numero quartos disponiveis em cada hotel
	                                (cpt.n_quartos_disponiveis -
	
	                                -- numero quartos disponiveis na data especifica
	                                (select COUNT(rh.id_reserva_hotel) 
		                                from reserva_hotel rh
				                                where 1=1 
                                                    and rh.apagado = 0 or rh.apagado is null
					                                and rh.id_tarifa_hotel = th.id_tarifa
                                                    and (rh.data_inicio <= '{dataInicio.ToString("yyyy-MM-dd")}' and rh.data_fim  >= '{dataFim.ToString("yyyy-MM-dd")}') 
                                                    or (rh.data_inicio >= '{dataInicio.ToString("yyyy-MM-dd")}' and rh.data_fim <= '{dataFim.ToString("yyyy-MM-dd")}')
                                    )
					
	                                ) as NumeroQuartosDisponiveis

                                from tarifas_hotel th
	
	                            left join hotel h on h.id_hotel = th.id_hotel
	                            left join localizacao l on l.id_localizacao = h.id_localizacao
	                            left join tipo_quarto tp on tp.id_tipo_quarto = th.id_tipo_quarto
	                            left join capacidade_tipo_quarto cpt on tp.id_tipo_quarto = cpt.id_tipo_quarto  and h.id_hotel = cpt.id_hotel

	                            where 1=1
		                            and th.data_inicio <= '{dataInicio.ToString("yyyy-MM-dd")}' 
		                            and th.data_fim  >= '{dataFim.ToString("yyyy-MM-dd")}' 
		                            {(string.IsNullOrEmpty(localizacao) ? "": $"and (l.localizacao like '%{localizacao}%' or h.nome like '%{localizacao}%')")}
                                    ;";

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(getTarifasSql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var newTarifa = new TarifasModel
                        {
                            IdTarifa = Convert.ToInt32(reader["id_tarifa"]),
                            IdHotel = Convert.ToInt32(reader["id_hotel"]),

                            NomeHotel = reader["nome"].ToString(),
                            LocalizacaoHotel = reader["localizacao"].ToString(),
                            MoradaHotel = reader["morada"].ToString(),
                            Classificacao = reader["classificacao"].ToString(),
                            
                            Capacidade = reader["capacidade"].ToString(),
                            DesignacaoTipoQuarto = reader["designacao"].ToString(),
                            PrecoUnidade = Convert.ToDecimal(reader["preco_unidade"]),
                        };

                        if (!Convert.IsDBNull(reader["NumeroQuartosDisponiveis"]))
                            newTarifa.NumeroQuartosDisponiveis = Convert.ToInt32(reader["NumeroQuartosDisponiveis"]);

                        listaTarifas.Add(newTarifa);
                    }
                }

                connection.Close();
            }
            
            model.ListaTarifas = listaTarifas;

            return View(model);
        }
        public ActionResult ReservarDestino(string jsonTarifa, string dataInicio, string dataFim, string nHospedes, string precoTotal)
        {
            ViewBag.LoggedIn = IsLoggedIn();

            var tarifaModel = jsonTarifa.DeserializeFromJson<TarifasModel>();
            var utilizador = GetUserLogged();

            string sql = $@"INSERT INTO reserva_hotel(
                                    [data_inicio],
                                    [data_fim],
                                    [nr_hospedes],
                                    [id_tarifa_hotel],
                                    [id_utilizador],
                                    [preco_total],
                                    [apagado]
                                    ) VALUES (
                                    @data_inicio,
                                    @data_fim,
                                    @nr_hospedes,
                                    @id_tarifa_hotel,
                                    @id_utilizador,
                                    @preco,
                                    @apagado)";

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@data_inicio", Convert.ToDateTime(dataInicio));
                command.Parameters.AddWithValue("@data_fim", Convert.ToDateTime(dataFim));
                command.Parameters.AddWithValue("@nr_hospedes", Convert.ToInt16(nHospedes));
                command.Parameters.AddWithValue("@id_tarifa_hotel", tarifaModel.IdTarifa);
                command.Parameters.AddWithValue("@preco", Convert.ToDecimal(precoTotal));
                command.Parameters.AddWithValue("@id_utilizador", utilizador.IdUser);
                command.Parameters.AddWithValue("@apagado", false);

                command.ExecuteNonQuery();

                connection.Close();
            }

            return Json(new { });
        }
        public ActionResult CancelarReserva(string idReserva)
        {
            string sql = $@"UPDATE reserva_hotel SET apagado = 1 WHERE id_reserva_hotel = {Convert.ToInt16(idReserva)}";

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();

               
                command.ExecuteNonQuery();

                connection.Close();
            }
            return Json(new { });
        }
    }
}