using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDataModels;

namespace Bussiness
{
    public interface IUserManager
    {
        bool VerifyLogin(string userName, string password);
    }
    public class UserManager : IUserManager
    {
        public void RegisterConnectionString(int UserId, string connStr, string dataSource, string DBName)
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
        public bool VerifyLogin(string userName, string password)
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
        public string[] ValidateRegisterData(string userName, string password, string repeatedPassword, string Email)
        {
            string[] returnValues = { "", "", "", "" };

            using (InternalDBModel context = new InternalDBModel())
            {
                if (userName.Length > 36)
                {
                    returnValues[0] = "The user name is too long";
                }
                else if (userName.Length < 4)
                {
                    returnValues[0] = "The user name is too short";
                }
                else if (context.RegisteredUsers.Where(u => u.UserName == userName) != null)
                {
                    returnValues[0] = "The user name is already taken";
                }

                if (password.Length < 4)
                {
                    returnValues[1] = "The password is too short";
                }
                else if (password.Length > 100)
                {
                    returnValues[1] = "The password is too long";
                }

                if (repeatedPassword != password)
                {
                    returnValues[2] = "Passwords don't match";
                }

                if (String.IsNullOrEmpty(Email))
                {
                    returnValues[3] = "The E-mail address cannot be empty";
                }
                else if (!Email.Contains('@'))
                {
                    returnValues[3] = "The E-mail addres is invalid";
                }
                else if(context.RegisteredUsers.Where(m => m.Email == Email) != null)
                {
                    returnValues[3] = "An account associated with given E-mail already exists";
                }
            }
            
            return returnValues;
        }
        public void RegisterUser(string userName, string password, string email)
        {
            using (InternalDBModel context = new InternalDBModel())
            {
                string hashedPassword = GetStringSha256Hash(password);
                context.RegisteredUsers.Add(new RegisteredUser()
                {
                    UserName = userName,
                    PassWord = hashedPassword,
                    Email = email
                });
                context.SaveChanges();
            }
        }
        internal string GetStringSha256Hash(string text)
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
