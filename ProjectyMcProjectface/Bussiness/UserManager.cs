using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDataModels;

namespace Bussiness
{
    public class UserManager
    {
        public static void RegisterConnectionString(int UserId, string connStr, string dataSource, string DBName)
        {
            using (InternalDBModel context = new InternalDBModel())
            {
                try
                {
                    context.ConnectionStrings.Add(new ConnectionString()
                    {
                        UserId = UserId,
                        String = connStr,
                        DataSource = dataSource,
                        DatabaseName = DBName
                    });
                    context.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw (ex);
                }
                
            }
        }
        public static bool AuthorizeLogin(string userName, string password)
        {
            string hashedPassword = GetStringSha256Hash(password);
            userName = userName.Trim();
            
            using(InternalDBModel context = new InternalDBModel())
            {
                foreach(RegisteredUser user in context.RegisteredUsers.Where(r => r.UserName == userName))
                {
                    if(user.PassWord == hashedPassword)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public static void RegisterUser(string UserName, string Password, string Email)
        {
            using (InternalDBModel context = new InternalDBModel())
            {
                string encryptedPassword = GetStringSha256Hash(Password);
                context.RegisteredUsers.Add(new RegisteredUser()
                {
                    UserName = UserName,
                    PassWord = encryptedPassword,
                    eMail = Email
                });
                context.SaveChanges();
            }
        }
        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}
