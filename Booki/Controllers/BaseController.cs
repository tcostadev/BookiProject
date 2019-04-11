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
        public string ConnectionString = "Data Source=LEGION-Y530;Initial Catalog=Booki;User ID=sa;Password=tiagoIsmai2019";
        public string SessionUtilizador = "SessionUtilizador";

        public void LogInUser(LoginModel userLoggedIn)
        {
            Session[SessionUtilizador] = userLoggedIn;
        }
        public void LogOutUser()
        {
            Session[SessionUtilizador] = null;
        }
    }
}