using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectyMcProjectface.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DatabaseEdit()
        {
            return View();
        }

        public ActionResult DatabaseView()
        {
            return View();
        }
    }
}