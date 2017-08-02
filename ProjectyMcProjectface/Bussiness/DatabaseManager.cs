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
        public List<DatabaseModel> GetDatabasesByEmail(string email)
        {
            var context = InjectionKernel.Instance.Get<IInternalDBModel>();
            List<DatabaseModel> returnValues = new List<DatabaseModel>();
            IUserManager userManager = InjectionKernel.Instance.Get<IUserManager>();
            foreach (var db in context.ConnectionStrings.Where(x => x.UserId == int.Parse(userManager.GetIdByEmail(email))).AsQueryable())
            {
                DatabaseModel dbModel = new DatabaseModel();
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
            IInternalDBModel context = InjectionKernel.Instance.Get<IInternalDBModel>();
            if(context.ConnectionStrings.Any
                (x => new SqlConnectionStringBuilder(x.InternalConnString).DataSource == id
                +"_"+new SqlConnectionStringBuilder(x.String).DataSource))
            {
                return true;
            }
            return false;
        }
    }
}
