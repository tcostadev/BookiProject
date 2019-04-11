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
    public class ControloAcessosController : BaseController
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            
            string sql = $@"SELECT u.id_user, u.nome_completo
                                FROM utilizador u 
                            WHERE 1=1
                                AND u.username = @username
                                AND u.password = @password";

            bool loggedIn = false;

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@username", model.Username);
                command.Parameters.AddWithValue("@password", model.Password);
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        loggedIn = !string.IsNullOrEmpty(reader["id_user"].ToString());
                        model.Nome = reader["nome_completo"].ToString();
                    }
                        
                }

                connection.Close();
            }

            if (loggedIn)
            {
                LogInUser(model);
                ViewBag.LoggedIn = loggedIn;
                ViewBag.UserLogin = model;
            }
            else
            {
                ModelState.AddModelError("Password", "Credenciais erradas");
                return PartialView(model);
            }
                

            return Json(new
            {
                Redirect = true,
                UrlRedirect = Url.Action("Index", "Home")
            });
        }

        public ActionResult Registo()
        {
            var model = new RegistoModel
            {
                ListaLocalizacoes = CustomHelper.GetListaLocalizacoes(ConnectionString)
            };
            
            return View(model);
        }
        [HttpPost]
        public ActionResult Registo(RegistoModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListaLocalizacoes = CustomHelper.GetListaLocalizacoes(ConnectionString);
                return PartialView(model);
            }
                
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
        public ActionResult Logout()
        {
            ViewBag.LoggedIn = false;
            LogOutUser();
            return RedirectToAction("Index", "Home");
        }
    }
}