using System.Collections.Generic;
using System.Web.Mvc;
using Booki.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Booki.UnitTests
{
    [TestClass]
    public class BookiTests
    {
        [TestMethod]
        public void LoginTest()
        {
            var control = new ControloAcessosController();

            var result = control.Login(new Models.LoginModel()
            {
                IdUser = 0,
                Nome = "",
                Username = "carlos",
                Password = "password"
            }) as JsonResult;

            IDictionary<string, object> data = (IDictionary<string, object>)new System.Web.Routing.RouteValueDictionary(result.Data);

            Assert.AreEqual(true, data["Redirect"]);
        }
    }
}
