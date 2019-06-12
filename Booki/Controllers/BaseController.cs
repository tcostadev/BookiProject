using Booki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booki.Controllers
{
    public abstract partial class BaseController : Controller
    {
        public string ConnectionString = "Data Source=DESKTOP-DIVT7IR;Initial Catalog=Booki;User ID=sa;Password=vascocrespo1997";
        public string SessionUtilizador = "SessionUtilizador";

        public void LogInUser(LoginModel userLoggedIn)
        {
            Session[SessionUtilizador] = userLoggedIn;
        }
        public bool IsLoggedIn()
        {
            return Session[SessionUtilizador] != null;
        }
        public void LogOutUser()
        {
            Session[SessionUtilizador] = null;
        }
        public LoginModel GetUserLogged()
        {
            return (LoginModel)Session[SessionUtilizador];
        }
    }
}