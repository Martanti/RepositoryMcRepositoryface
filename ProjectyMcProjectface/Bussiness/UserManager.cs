﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDataModels;

namespace Bussiness
{
    
    public class UserManager : IUserManager
    {
        public int MaxUserNameLength { get; set; }
        public int MinUserNameLength { get; set; }
        public int MaxPassWordLength { get; set; }
        public int MinPassWordLength { get; set; }

        public UserManager(int maxUserNameLength = 36, int minUserNameLength = 4, int maxPassWordLength = 100, int minPassWordLength = 4)
        {
            MaxUserNameLength = maxUserNameLength;
            MinUserNameLength = minUserNameLength;
            MaxPassWordLength = maxPassWordLength;
            MinPassWordLength = minPassWordLength;

        }

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
            IEncryptionManager encryptionManager = new EncryptionManager(); //Karolis_to do - dependency managers
            string hashedPassword = encryptionManager.GetStringSha256Hash(password);
            userName = userName.Trim();
            
            using(InternalDBModel context = new InternalDBModel())
            {
                try
                {
                    var user = context.RegisteredUsers.Single(u => u.UserName == userName && u.PassWord == hashedPassword);
                    return true;
                }
                catch (System.InvalidOperationException)
                {
                    return false;
                }
            }
        }
        public string[] ValidateRegisterData(string userName, string password, string repeatedPassword, string Email)
        {
            userName = userName.Trim();
            password = password.Trim();
            repeatedPassword = repeatedPassword.Trim();
            Email = Email.Trim();
            string[] returnValues = { "", "", "", "" };

            using (InternalDBModel context = new InternalDBModel())
            {
                if (String.IsNullOrEmpty(userName))
                {
                    returnValues[0] = Resources.Properties.Resources.ValidationUserNameEmpty;
                }
                else if (userName.Length > MaxUserNameLength)
                {
                    returnValues[0] = Resources.Properties.Resources.ValidationUserNameTooLong;
                }
                else if (userName.Length < MinUserNameLength)
                {
                    returnValues[0] = Resources.Properties.Resources.ValidationUserNameTooShort;
                }
                else if (context.RegisteredUsers.Where(u => u.UserName == userName) != null)
                {
                    returnValues[0] = Resources.Properties.Resources.ValidationUserNameTaken;
                }

                if (String.IsNullOrEmpty(password))
                {
                    returnValues[1] = Resources.Properties.Resources.ValidationPasswordEmpty;
                }
                else if (password.Length < MinPassWordLength)
                {
                    returnValues[1] = Resources.Properties.Resources.ValidationPasswordTooShort;
                }
                else if (password.Length > MaxPassWordLength)
                {
                    returnValues[1] = Resources.Properties.Resources.ValidationPasswordTooLong;
                }

                if (repeatedPassword != password)
                {
                    returnValues[2] = Resources.Properties.Resources.ValidationPasswordsDoNotMatch;
                }

                if (String.IsNullOrEmpty(Email))
                {
                    returnValues[3] = Resources.Properties.Resources.ValidationEmailEmpty;
                }
                else if (!ValidateEmail(Email))
                {
                    returnValues[3] = Resources.Properties.Resources.ValidationEmailInvalid;
                }
            }
            
            return returnValues;
        }
        public bool ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public void RegisterUser(string userName, string password, string email)
        {
            using (InternalDBModel context = new InternalDBModel())
            {
                IEncryptionManager encryptionManager = new EncryptionManager(); //Karolis_to do - dependency managers
                string hashedPassword = encryptionManager.GetStringSha256Hash(password);
                context.RegisteredUsers.Add(new RegisteredUser()
                {
                    UserName = userName,
                    PassWord = hashedPassword,
                    Email = email
                });
                context.SaveChanges();
            }
        }
        
    }
}
