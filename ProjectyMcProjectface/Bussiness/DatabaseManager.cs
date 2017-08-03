using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EFDataModels;
using Dto;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public class DatabaseManager : IDatabaseManager
    {
        private IInternalDBModel _internalDBContext;
        private IUserManager _userManager;
        private IDatabaseCopy _databaseCopy;
        public DatabaseManager(IInternalDBModel internalDBModel, IUserManager userManager,
            IDatabaseCopy databaseCopy)
        {
            _internalDBContext = internalDBModel;
            _userManager = userManager;
            _databaseCopy = databaseCopy;
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
    }
}
