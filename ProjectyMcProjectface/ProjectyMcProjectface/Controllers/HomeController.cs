using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bussiness;
using System.Web;
using System.Web.Mvc;
using Dto;
using System.Security.Claims;
using System.Configuration;

namespace ProjectyMcProjectface.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            string userName = claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();

            ViewBag.MainPageLayoutUsername = userName;

            return View("Index");
        }

        public ActionResult DatabaseEdit()
        {
            return View();
        }

        public ActionResult DatabaseView()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddDatabase()
        {
            DatabaseRegisterModel model = new DatabaseRegisterModel();
            model.ConnectionString = "";
            model.Name = "";
            model.IsConnectionSuccessfull = false;
            model.IsHttpGet = true;
            return View("AddDatabase", model);

        }
        [HttpPost]
        public ActionResult AddDatabase(DatabaseRegisterModel model)
        {
            model.IsHttpGet = false;
            if (!String.IsNullOrWhiteSpace(model.ConnectionString))
            {
                model.ConnectionString = model.ConnectionString.Trim();
                IDatabaseManager DBManager = InjectionKernel.Instance.Get<IDatabaseManager>();
                model.IsConnectionSuccessfull = DBManager.IsDatabaseAvailable(model.ConnectionString);
            }
            if (!String.IsNullOrWhiteSpace(model.Name))
            {
                model.Name = model.Name.Trim();
            }
            return View("AddDatabase", model);
        }
        [HttpPost]
        public ActionResult RegisterDatabase(DatabaseRegisterModel model)
        {
            model.IsHttpGet = false;
            IDatabaseManager DBManager = InjectionKernel.Instance.Get<IDatabaseManager>();

            if (!String.IsNullOrWhiteSpace(model.ConnectionString))
            {
                model.ConnectionString = model.ConnectionString.Trim();
                
                model.IsConnectionSuccessfull = DBManager.IsDatabaseAvailable(model.ConnectionString);
            }
            if (!String.IsNullOrWhiteSpace(model.Name))
            {
                model.Name = model.Name.Trim();
            }

            if (model.IsConnectionSuccessfull)
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                string email = claims.Single(c => c.Type == ClaimTypes.Email).Value.ToString();
                IUserManager userManager = InjectionKernel.Instance.Get<IUserManager>();
                string id = userManager.GetIdByEmail(email);

                DBManager.RegisterDatabase(model.ConnectionString, ConfigurationManager.AppSettings["InternalDBConnectionString"].ToString(), int.Parse(id), model.Name);
                return RedirectToAction("DatabaseRegisterSuccessful", "Home");
            }
            else
            {
                return View("AddDatabase", model);
            }
        }
        public ActionResult DatabaseRegisterSuccessful()
        {
            return View("DatabaseRegisterSuccessful");
        }

        public ActionResult SignOut()
        {
            var owinContext = Request.GetOwinContext();
            var authManager = owinContext.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Login");
        }
    }
}