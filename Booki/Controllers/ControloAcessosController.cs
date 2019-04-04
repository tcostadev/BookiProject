using Booki.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booki.Controllers
{
    public class ControloAcessosController : BaseController
    {
    
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registo(ControloAcessosModel model)
        {
            string sql = $@"INSERT INTO utilizador (
                                    [username],
                                    [password],
                                    [nome_completo],
                                    [morada],
                                    [codigo_postal],
                                    [id_localizacao]
                                   ) VALUES (
                                    @username,
                                    @password,
                                    @nome_completo,
                                    @morada,
                                    @codigo_postal,
                                    @id_localizacao)";

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@username", model.Username);
                command.Parameters.AddWithValue("@password", model.Password);
                command.Parameters.AddWithValue("@nome_completo", model.Nome);
                command.Parameters.AddWithValue("@morada", model.Morada);
                command.Parameters.AddWithValue("@codigo_postal", model.CodigoPostal);
                command.Parameters.AddWithValue("@id_localizacao", model.IdLocalizacao);
                
                command.ExecuteNonQuery();

                connection.Close();
            }
            
            return Json(new { });
        }
    }
}