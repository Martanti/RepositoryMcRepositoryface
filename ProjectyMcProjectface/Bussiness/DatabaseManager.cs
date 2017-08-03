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
        public DatabaseManager(IInternalDBModel internalDBModel)
        {
            _internalDBContext = internalDBModel;
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
            internalBuilder.InitialCatalog = userId + "_" + originalConn.Database;
            stringObj.InternalConnString = internalBuilder.ConnectionString;

            if (String.IsNullOrWhiteSpace(databaseName))
            {
                stringObj.DatabaseName = originalConn.Database;
            }
            else
            {
                stringObj.DatabaseName = databaseName;
            }
            IDatabaseCopy DBCpy = InjectionKernel.Instance.Get<IDatabaseCopy>();
            DBCpy.CopyDatabaseSMO(stringObj.String, stringObj.InternalConnString, userId.ToString());

            _internalDBContext.ConnectionStrings.Add(stringObj);
            _internalDBContext.SaveChanges();
        }
        public List<Database> GetDatabasesByEmail(string email)
        {
            List<Database> returnValues = new List<Database>();
            IUserManager userManager = InjectionKernel.Instance.Get<IUserManager>();
            foreach (var db in _internalDBContext.ConnectionStrings.Where(x => x.UserId == int.Parse(userManager.GetIdByEmail(email))).AsQueryable())
            {
                Database dbModel = new Database();
                dbModel.InternalConnectionString = db.String;
                dbModel.Name = db.DatabaseName;
                dbModel.OriginalConnectionString = db.InternalConnString;
                returnValues.Add(dbModel);
            }

            return returnValues;
        }
        public bool CheckDatabaseExistance(string id, string connString)
        {
            SqlConnection connection = new SqlConnection(connString);
            if(_internalDBContext.ConnectionStrings.Any
                (x => new SqlConnectionStringBuilder(x.InternalConnString).DataSource == id
                +"_"+new SqlConnectionStringBuilder(x.String).DataSource))
            {
                return true;
            }
            return false;
        }
    }
}
