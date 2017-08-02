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
            using (var context = new InternalDBContext())
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

                context.ConnectionStrings.Add(stringObj);
                context.SaveChanges();
            }
        }
        public List<DatabaseModel> GetDatabases(string email)
        {
            return new List<DatabaseModel>();
        }
    }
}
