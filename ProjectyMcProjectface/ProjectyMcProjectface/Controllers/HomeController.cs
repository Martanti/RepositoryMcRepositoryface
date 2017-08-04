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
        public ActionResult Index(bool isPartial = false)
        {
            var baseModel = new BaseModel() {IsPartial = isPartial };
            return View("Index", baseModel);
        }
        public ActionResult ViewCurrentDatabase(bool isPartial = false)
        {
            var baseModel = new BaseModel() { IsPartial = isPartial };
            return PartialView("ViewCurrentDatabase", baseModel);
        }
        public ActionResult DatabaseEdit(bool isPartial = false)
        {
            var baseModel = new BaseModel() { IsPartial = isPartial };
            return View(baseModel);
        }

        public ActionResult DatabaseView(bool isPartial = false)
        {
            return View(new BaseModel() { IsPartial=isPartial});
        }

        [HttpGet]
        public ActionResult AddDatabase(bool isPartial = false)
        {
            DatabaseRegisterModel model = new DatabaseRegisterModel();
            model.ConnectionString = "";
            model.Name = "";
            model.IsConnectionSuccessfull = false;
            model.IsHttpGet = true;
            model.ErrorMessage = "";

            model.IsPartial = isPartial;

            return View("AddDatabase", model);
        }
        [HttpPost]
        public ActionResult AddDatabase(DatabaseRegisterModel model, bool isPartial=false)
        {
            model.IsPartial = isPartial;
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
            if (!model.IsConnectionSuccessfull)
            {
                model.ErrorMessage = Resources.MainPageAddDatabaseResources.ErrorConnStringInvalid;
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
            if (!model.IsConnectionSuccessfull)
            {
                model.ErrorMessage = Resources.MainPageAddDatabaseResources.ErrorConnStringInvalid;
            }

            if (model.IsConnectionSuccessfull)
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                string email = claims.Single(c => c.Type == ClaimTypes.Email).Value.ToString();
                IUserManager userManager = InjectionKernel.Instance.Get<IUserManager>();
                string id = userManager.GetIdByEmail(email);

                if(DBManager.CheckDatabaseExistance(id, model.ConnectionString))
                {
                    model.ErrorMessage = Resources.MainPageAddDatabaseResources.ErrorConnStringAlreadyRegistered;
                }
                else
                {
                    DBManager.RegisterDatabase(model.ConnectionString, ConfigurationManager.AppSettings["InternalDBConnectionString"].ToString(), int.Parse(id), model.Name);
                    return View("DatabaseRegisterSuccessful", new BaseModel() {IsPartial = model.IsPartial });
                }
                
            }
            return View("AddDatabase", model);
        }

        public ActionResult SignOut()
        {
            var owinContext = Request.GetOwinContext();
            var authManager = owinContext.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Login");
        }
        public ActionResult AddDatabaseToCookies(string internalDbName)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}