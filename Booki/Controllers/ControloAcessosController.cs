﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booki.Controllers
{
    public class ControloAcessosController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registo()
        {
            return View();
        }
    }
}