using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booki.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.LoggedIn = IsLoggedIn();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.LoggedIn = IsLoggedIn();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.LoggedIn = IsLoggedIn();

            return View();
        }
    }
}