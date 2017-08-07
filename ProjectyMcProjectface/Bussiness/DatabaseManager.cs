using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EFDataModels;
using Dto;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bussiness
{
    public class DatabaseManager : IDatabaseManager
    {
        public readonly string SelectedDatabaseCookieName;
        public readonly string EmailCookieName;
        public readonly int cookieExpirationTimeInYears = 15;

        private IInternalDBModel _internalDBContext;
        private IUserManager _userManager;
        private IDatabaseCopy _databaseCopy;
        private ISmoManager _smoManager;
        public DatabaseManager(IInternalDBModel internalDBModel, IUserManager userManager,
            IDatabaseCopy databaseCopy, ISmoManager smoManager)
        {
            SelectedDatabaseCookieName = "SelectedDatabase";
            EmailCookieName = "UserEmail";
            _internalDBContext = internalDBModel;
            _userManager = userManager;
            _databaseCopy = databaseCopy;
            _smoManager = smoManager;
        }
        public bool IsDatabaseAvailable(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }   
        }
        public void RegisterDatabase(string originalConnectionString,string internalConnectionString, int userId, string databaseName = "")
        {
            ConnectionString stringObj = new ConnectionString();

            stringObj.UserId = userId;
            stringObj.String = originalConnectionString;
            SqlConnection originalConn = new SqlConnection(originalConnectionString);
            SqlConnectionStringBuilder internalBuilder = new SqlConnectionStringBuilder(internalConnectionString);
            internalBuilder.InitialCatalog = stringObj.InternalDatabaseName = userId + "_" + originalConn.Database;
            stringObj.InternalConnString = internalBuilder.ConnectionString;

            if (String.IsNullOrWhiteSpace(databaseName))
            {
                stringObj.DatabaseName = originalConn.Database;
            }
            else
            {
                stringObj.DatabaseName = databaseName;
            }
            
            _databaseCopy.CopyDatabaseSMO(stringObj.String, stringObj.InternalConnString, userId.ToString());

            _internalDBContext.ConnectionStrings.Add(stringObj);
            _internalDBContext.SaveChanges();
        }
        public List<Database> GetDatabasesByEmail(string email)
        {
            int id = int.Parse(_userManager.GetIdByEmail(email));
            List<Database> returnValues = new List<Database>();
            foreach (var db in _internalDBContext.ConnectionStrings.Where(x => x.UserId == id).AsQueryable())
            {
                Database dbModel = new Database();
                dbModel.InternalConnectionString = db.InternalConnString;
                dbModel.Name = db.DatabaseName;
                dbModel.TableCount = _smoManager.GetTableCountByConnectionString(db.InternalConnString);
                dbModel.OriginalConnectionString = db.String;
                dbModel.InternalName = new SqlConnectionStringBuilder(db.InternalConnString).InitialCatalog;
                returnValues.Add(dbModel);
            }

            return returnValues;
        }
        public bool CheckDatabaseExistance(string id, string connString)
        {
            SqlConnection connection = new SqlConnection(connString);
            string dataSource = new SqlConnectionStringBuilder(connString).InitialCatalog;
            if (_internalDBContext.ConnectionStrings.Any
                (x => x.InternalDatabaseName == (id
                +"_"+dataSource)))
            {
                return true;
            }
            return false;
        }
        public void SaveDatabaseInCookies(string internalName, string email)
        {
            if (HttpContext.Current.Request.Cookies[SelectedDatabaseCookieName] != null)
            {
                HttpContext.Current.Response.Cookies[SelectedDatabaseCookieName].Value = internalName;
            }
            else
            {
                HttpCookie cookie = new HttpCookie(SelectedDatabaseCookieName);
                cookie.Value = internalName;
                //cookie.Expires = cookie.Expires = DateTime.UtcNow.AddYears(cookieExpirationTimeInYears);
                cookie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            if (HttpContext.Current.Request.Cookies[EmailCookieName] != null)
            {
                HttpContext.Current.Response.Cookies[EmailCookieName].Value = email;
            }
            else
            {
                HttpCookie cookie = new HttpCookie(EmailCookieName);
                cookie.Value = email ;
                //cookie.Expires = cookie.Expires = DateTime.UtcNow.AddYears(cookieExpirationTimeInYears);
                cookie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public Database GetDatabaseFromCookies()
        {
            Database dbModel = new Database();
            if (HttpContext.Current.Request.Cookies[SelectedDatabaseCookieName] != null && HttpContext.Current.Request.Cookies[EmailCookieName] != null)
            {
                string internalDbName = HttpContext.Current.Request.Cookies[SelectedDatabaseCookieName].Value.ToString();
                string email = HttpContext.Current.Request.Cookies[EmailCookieName].Value.ToString();
                int id = int.Parse(_userManager.GetIdByEmail(email));

                try {
                    dbModel = _smoManager.GetDatabaseByInternalConnString(_internalDBContext.ConnectionStrings.Single
                        (x => x.InternalDatabaseName == internalDbName && x.UserId == id).InternalConnString,
                        _internalDBContext.ConnectionStrings.Single
                        (x => x.InternalDatabaseName == internalDbName && x.UserId == id).DatabaseName);
                    return dbModel;
                }
                catch
                {
                    return null;
                }
                
            }
            else
            {
                return null;
            }
        }
    }
}
