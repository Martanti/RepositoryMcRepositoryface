using System;
using System.Collections.Generic;
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
        IDatabaseManager _databaseManager;
        IUserManager _userManager;

        public HomeController()
        {
            _databaseManager = InjectionKernel.Instance.Get<IDatabaseManager>();
            _userManager = InjectionKernel.Instance.Get<IUserManager>();
        }

        public ActionResult Index(bool isPartial = false)
        {
            var baseModel = new BaseModel() {IsPartial = isPartial };
            return View("Index", baseModel);
        }

        public ActionResult ViewCurrentDatabase(bool isPartial = false)
        {
            var Database = _databaseManager.GetDatabaseFromCookies();
            Database.IsPartial = isPartial;
            return View("ViewCurrentDatabase", Database);
        }

        public ActionResult ViewTable(string internalDbName, string schema, string name, bool isPartial = true)
        {
            try
            {
                Table table = _databaseManager.GetTableByInternalName(internalDbName, schema, name);
                table.IsPartial = isPartial;
                return View("ViewTable", table);
            }
            catch(Exception ex) {
                throw (ex);
            }
        }

        public ActionResult DatabaseEdit(bool isPartial = false)
        {
            var baseModel = new BaseModel() { IsPartial = isPartial };
            return View(baseModel);
        }

        public ActionResult DatabaseView(bool isPartial = false)
        {
            var model = new BaseModel() { IsPartial = isPartial };
            return View("DatabaseView", model);
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
                model.IsConnectionSuccessfull = _databaseManager.IsDatabaseAvailable(model.ConnectionString);
                
                
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

            if (!String.IsNullOrWhiteSpace(model.ConnectionString))
            {
                model.ConnectionString = model.ConnectionString.Trim();
                
                model.IsConnectionSuccessfull = _databaseManager.IsDatabaseAvailable(model.ConnectionString);
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

                string id = _userManager.GetIdByEmail(email);

                if(_databaseManager.CheckDatabaseExistance(id, model.ConnectionString))
                {
                    model.ErrorMessage = Resources.MainPageAddDatabaseResources.ErrorConnStringAlreadyRegistered;
                }
                else
                {
                    _databaseManager.RegisterDatabase(model.ConnectionString, ConfigurationManager.AppSettings["InternalDBConnectionString"].ToString(), int.Parse(id), model.Name);
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
            var identity = (ClaimsIdentity)User.Identity;
            _databaseManager.SaveDatabaseInCookies(internalDbName, identity.Claims.Single(x => x.Type == ClaimTypes.Email).Value.ToString());
            return RedirectToAction("ViewCurrentDatabase", "Home");
        }
    }
}