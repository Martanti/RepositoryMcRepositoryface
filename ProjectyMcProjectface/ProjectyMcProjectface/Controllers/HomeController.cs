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

        [AjaxOnly]
        public ActionResult ViewCurrentDatabase(bool isPartial = true, string internalDbName = null)
        {
            Database database;
            if(internalDbName == null) {
                database = _databaseManager.GetDatabaseFromCookies();
            }
            else
            {
                database = _databaseManager.GetDatabaseByInternalDbName(internalDbName);
            }
            if(database == null)
            {
                return DatabaseView();
            }
            database.IsPartial = isPartial;
            return View("ViewCurrentDatabase", database);
        }
        
        [AjaxOnly]
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

        [AjaxOnly]
        public ActionResult DatabaseEdit(bool isPartial = false)
        {
            var baseModel = new BaseModel() { IsPartial = isPartial };
            return View(baseModel);
        }
        
        [AjaxOnly]
        public ActionResult DatabaseView(bool isPartial = true)
        {
            var model = new BaseModel() { IsPartial = isPartial };
            return View("DatabaseView", model);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult AddDatabase(bool isPartial = true)
        {
            DatabaseRegisterModel model = new DatabaseRegisterModel();
            model.ConnectionString = "";
            model.Name = "";
            model.IsConnectionSuccessful = false;
            model.IsHttpGet = true;
            model.ErrorMessage = "";
            model.ConnectionTestSuccess = "";
            model.DatabaseAddedSuccessfuly = "";

            model.IsPartial = isPartial;

            return View("AddDatabase", model);
        }

        [HttpPost]
        public ActionResult AddDatabase(DatabaseRegisterModel model, bool isPartial=true)
        {
            model.IsPartial = isPartial;
            model.IsHttpGet = false;
            if (!String.IsNullOrWhiteSpace(model.ConnectionString))
            {
                model.ConnectionString = model.ConnectionString.Trim();
                model.IsConnectionSuccessful = _databaseManager.IsDatabaseAvailable(model.ConnectionString);
                
                
            }
            if (!String.IsNullOrWhiteSpace(model.Name))
            {
                model.Name = model.Name.Trim();
            }
            if (!model.IsConnectionSuccessful)
            {
                model.ErrorMessage = Resources.MainPageAddDatabaseResources.ErrorConnStringInvalid;
            }
            else
            {
                model.ConnectionTestSuccess = Resources.MainPageAddDatabaseResources.TestSuccess;
            }

            return View("AddDatabase", model);
        }

        [HttpPost]
        public ActionResult RegisterDatabase(DatabaseRegisterModel model, bool isPartial = true)
        {
            model.IsHttpGet = false;
            model.IsPartial = isPartial;

            if (!String.IsNullOrWhiteSpace(model.ConnectionString))
            {
                model.ConnectionString = model.ConnectionString.Trim();
                
                model.IsConnectionSuccessful = _databaseManager.IsDatabaseAvailable(model.ConnectionString);
            }
            if (!String.IsNullOrWhiteSpace(model.Name))
            {
                model.Name = model.Name.Trim();
            }
            if (!model.IsConnectionSuccessful)
            {
                model.ErrorMessage = Resources.MainPageAddDatabaseResources.ErrorConnStringInvalid;
            }

            if (model.IsConnectionSuccessful)
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
                    model.DatabaseAddedSuccessfuly = Resources.MainPageAddDatabaseResources.RegistrationWasASuccess;
                    _databaseManager.RegisterDatabase(model.ConnectionString, ConfigurationManager.AppSettings["InternalDBConnectionString"].ToString(), int.Parse(id), model.Name);
                    return View("AddDatabase", model);
                } 
            }
            return View("AddDatabase", model);
        }

        public ActionResult SignOut()
        {
            if (HttpContext.Request.Cookies[SelectedDatabaseCookieName] != null)
            {
                HttpCookie currentUserCookie = HttpContext.Request.Cookies[SelectedDatabaseCookieName];
                HttpContext.Response.Cookies.Remove(SelectedDatabaseCookieName);
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                HttpContext.Response.SetCookie(currentUserCookie);
            }
            if (HttpContext.Request.Cookies[EmailCookieName] != null)
            {
                HttpCookie currentUserCookie = HttpContext.Request.Cookies[EmailCookieName];
                HttpContext.Response.Cookies.Remove(EmailCookieName);
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                HttpContext.Response.SetCookie(currentUserCookie);
            }
            if (HttpContext.Request.Cookies[ReturnUrlCookieName] != null)
            {
                HttpCookie currentUserCookie = HttpContext.Request.Cookies[ReturnUrlCookieName];
                HttpContext.Response.Cookies.Remove(ReturnUrlCookieName);
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                HttpContext.Response.SetCookie(currentUserCookie);
            }
            

            var owinContext = Request.GetOwinContext();
            var authManager = owinContext.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Login");
        }

        [AjaxOnly]
        public ActionResult AddDatabaseToCookies(string internalDbName)
        {
            var identity = (ClaimsIdentity)User.Identity;
            _databaseManager.SaveDatabaseInCookies(internalDbName, identity.Claims.Single(x => x.Type == ClaimTypes.Email).Value.ToString());
            var db = _databaseManager.GetDatabaseFromCookies();
            ActionResult result = ViewCurrentDatabase(true, internalDbName);
            return result;
        }
    }
}