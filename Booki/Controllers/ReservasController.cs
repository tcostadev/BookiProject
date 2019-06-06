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

            var sql = $@"SELECT * 
                            FROM reserva_hotel rh
                                WHERE 1=1
                                    AND rh.id_utilizador = @user_id
                                ORDER BY rh.data_inicio DESC";



            return View();
        }
        public ActionResult SearchDestinos(string dataInicio, string dataFim, string localizacao)
        {
            ViewBag.LoggedIn = IsLoggedIn();

            var model = new SearchDestinosModel
            {
                DataInicio = Convert.ToDateTime(dataInicio),
                DataFim = Convert.ToDateTime(dataFim),
                Localizacao = localizacao.ToUpper()
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
					                                        and rh.id_tarifa_hotel = th.id_tarifa
					                                        and rh.data_inicio <= '2019-05-01' 
					                                        and rh.data_fim  >= '2019-05-01' )
					
	                                        ) as NumeroQuartosDisponiveis

                                        from tarifas_hotel th
	
	                                    left join hotel h on h.id_hotel = th.id_hotel
	                                    left join localizacao l on l.id_localizacao = h.id_localizacao
	                                    left join tipo_quarto tp on tp.id_tipo_quarto = th.id_tipo_quarto
	                                    left join capacidade_tipo_quarto cpt on tp.id_tipo_quarto = cpt.id_tipo_quarto  and h.id_hotel = cpt.id_hotel

	                                    where 1=1
		                                    and th.data_inicio <= '{dataInicio}' 
		                                    and th.data_fim  >= '{dataFim}' 
		                                    and (l.localizacao like '%{localizacao}%' or h.nome like '%{localizacao}%')
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
        public ActionResult ReservarDestino(string jsonTarifa)
        {
            var tarifaModel = jsonTarifa.DeserializeFromJson<TarifasModel>();

            string sql = $@"INSERT INTO utilizador (
                                    [username],
                                    [password],
                                    [nome_completo],
                                    [email],
                                    [morada],
                                    [codigo_postal],
                                    [id_localizacao]
                                   ) VALUES (
                                    @username,
                                    @password,
                                    @nome_completo,
                                    @email,
                                    @morada,
                                    @codigo_postal,
                                    @id_localizacao)";

            //using (var connection = new SqlConnection(ConnectionString))
            //using (var command = new SqlCommand(sql, connection))
            //{
            //    connection.Open();

            //    command.Parameters.AddWithValue("@username", model.Username);
            //    command.Parameters.AddWithValue("@password", model.Password);
            //    command.Parameters.AddWithValue("@nome_completo", model.Nome);
            //    command.Parameters.AddWithValue("@email", model.Email);
            //    command.Parameters.AddWithValue("@morada", model.Morada);
            //    command.Parameters.AddWithValue("@codigo_postal", model.CodigoPostal);
            //    command.Parameters.AddWithValue("@id_localizacao", model.IdLocalizacao);

            //    command.ExecuteNonQuery();

            //    connection.Close();
            //}

            return PartialView();
        }
    }
}