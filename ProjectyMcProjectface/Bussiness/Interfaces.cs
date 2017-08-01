using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public interface IDatabaseCopy
    {
        void CopyDatabaseSMO(string sourceConnectionString, string destinationConnectionString, string userID);
        void CopyDatabaseSMO(string originalServerName, string originalDBName,
            string destinationServerName, string userID);
    }
    public interface IUserManager
    {
        int MaxUserNameLength { get; }
        int MinUserNameLength { get; }
        int MaxPassWordLength { get; }
        int MinPassWordLength { get; }

        void RegisterConnectionString(int UserId, string connStr, string dataSource, string DBName);
        bool VerifyLogin(string userName, string password);
        string[] ValidateRegisterData(string userName, string password, string repeatedPassword, string Email);
        void RegisterUser(string userName, string password, string email);
        bool ValidateEmail(string email);
    }
    public interface IEncryptionManager {
        string GetStringSha256Hash(string text);
    }
    public interface IDatabaseManager
    {
        bool IsDatabaseAvailable(string connectionString);
    }
}
