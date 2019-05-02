using Booki.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Booki.Helper
{
    public class CustomHelper
    {
        public static class Constantes
        {
            public static class TiposNotificacao
            {
                public static string Info = "alert-primary";
                public static string Success = "alert-success";
                public static string Erro = "alert-danger";
                public static string Aviso = "alert-warning";
            }
        }

        public static List<LocalizacaoModel> GetListaLocalizacoes(string connectionString)
        {
            var listaFinal = new List<LocalizacaoModel>();

            var sql = "SELECT * from localizacao";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var newItem = new LocalizacaoModel
                        {
                            IdLocalizacao = Convert.ToInt16(reader["id_localizacao"]),
                            Localizacao = reader["localizacao"].ToString()
                        };

                        listaFinal.Add(newItem);
                    }
                }

                connection.Close();
            }
            return listaFinal;
        }
    }
}