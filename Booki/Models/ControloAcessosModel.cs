using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booki.Models
{
    public class RegistoModel
    {
        public int IdUser { get; set; }
        [Required(ErrorMessage ="Campo obrigatório")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Nome { get; set; }
        public string Morada { get; set; }
        public string CodigoPostal { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public int IdLocalizacao { get; set; }

        public List<LocalizacaoModel> ListaLocalizacoes { get; set; } = new List<LocalizacaoModel>();
    }
    public class LoginModel
    {
        public int IdUser { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Password { get; set; }
        public string Nome { get; set; }
    }
}