using Booki.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var dataInicioDt = Convert.ToDateTime(dataInicio);
            var dataFimDt = Convert.ToDateTime(dataInicio);
            var listaTarifas = new List<TarifasModel>();

            var getTarifasSql = $@"select 
                                            th.id_tarifa,
                                            th.preco_unidade, 
                                            tp.designacao, 
                                            tp.capacidade, 
                                            h.nome, 
                                            l.localizacao 
                                        from tarifas_hotel th
	
	                                    left join hotel h on h.id_hotel = th.id_hotel
	                                    left join localizacao l on l.id_localizacao = h.id_localizacao
	                                    left join tipo_quarto tp on tp.id_tipo_quarto = th.id_tipo_quarto
	
	                                    where 1=1
		                                    and th.data_inicio >= {dataInicio}
		                                    and th.data_fim <=  {dataFim}
		                                    and l.localizacao like '%{localizacao}%'
                                    ;";

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(getTarifasSql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        listaTarifas.Add(new TarifasModel
                        {
                            IdTarifa = Convert.ToInt32(reader["id_tarifa"]),
                            
                        });
                    }

                }

                connection.Close();
            }

            return View();
        }
    }
}