using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamersShop.WebUI.Controllers
{
    public class NavController : Controller
    {
        // GET: Nav
        public string Menu()
        {
            return "Тестируем контроллер Nav";
        }
    }
}