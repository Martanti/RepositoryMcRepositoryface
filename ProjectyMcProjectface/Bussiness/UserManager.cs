using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDataModels;
using System.Security.Claims;
using Dto;

namespace Bussiness
{
    
    public class UserManager : IUserManager
    {
        public int MaxUserNameLength { get; }
        public int MinUserNameLength { get; }
        public int MaxPassWordLength { get; }
        public int MinPassWordLength { get; }
        private IEncryptionManager _encryptionManager;
        private IInternalDBModel _internalDBContext;
        public UserManager(IEncryptionManager encryptionManager, IInternalDBModel internalDBModel)
        {
            MaxUserNameLength = 36;
            MinUserNameLength = 4;
            MaxPassWordLength = 100;
            MinPassWordLength = 4;

            _encryptionManager = encryptionManager;
            _internalDBContext = internalDBModel;
        }
        public bool VerifyLogin(string Email, string password)
        {
            if(String.IsNullOrWhiteSpace(Email) || String.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            string hashedPassword = _encryptionManager.GetStringSha256Hash(password);
            Email = Email.Trim();

            try
            {
                var user = _internalDBContext.RegisteredUsers.Single(u => u.Email == Email && u.PassWord == hashedPassword);
                return true;
            }
            catch (System.InvalidOperationException)
            {
                return false;
            }
        }
        public string[] ValidateRegisterData(string userName, string password, string repeatedPassword, string Email)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                userName = userName.Trim();
            }
            if (!String.IsNullOrEmpty(password))
            {
                password = password.Trim();
            }
            if (!String.IsNullOrEmpty(Email))
            {
                Email = Email.Trim();
            }
            
            string[] returnValues = { "", "", "", "" };

            if (String.IsNullOrEmpty(userName))
            {
                returnValues[0] = Resources.ValidationResources.ValidationUserNameEmpty;
            }
            else if (userName.Length > MaxUserNameLength)
            {
                returnValues[0] = Resources.ValidationResources.ValidationUserNameTooLong;
            }
            else if (userName.Length < MinUserNameLength)
            {
                returnValues[0] = Resources.ValidationResources.ValidationUserNameTooShort;
            }

            if (String.IsNullOrEmpty(password))
            {
                returnValues[1] = Resources.ValidationResources.ValidationPasswordEmpty;
            }
            else if (password.Length < MinPassWordLength)
            {
                returnValues[1] = Resources.ValidationResources.ValidationPasswordTooShort;
            }
            else if (password.Length > MaxPassWordLength)
            {
                returnValues[1] = Resources.ValidationResources.ValidationPasswordTooLong;
            }

            if (repeatedPassword != password)
            {
                returnValues[2] = Resources.ValidationResources.ValidationPasswordsDoNotMatch;
            }

            if (String.IsNullOrEmpty(Email))
            {
                returnValues[3] = Resources.ValidationResources.ValidationEmailEmpty;
            }
            else if (!ValidateEmail(Email))
            {
                returnValues[3] = Resources.ValidationResources.ValidationEmailInvalid;
            }
            else
            {
                try
                {
                    var temp = _internalDBContext.RegisteredUsers.Single(u => u.Email == Email);
                    returnValues[3] = Resources.ValidationResources.ValidationEmailTaken;
                }
                catch
                {

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
            string hashedPassword = _encryptionManager.GetStringSha256Hash(password);
            _internalDBContext.RegisteredUsers.Add(new RegisteredUser()
            {
                UserName = userName,
                PassWord = hashedPassword,
                Email = email
            });
            _internalDBContext.SaveChanges();
        }

        public string GetUsernameByEmail(string email)
        {
            return _internalDBContext.RegisteredUsers.Single(x => x.Email == email).UserName;
        }
        public string GetIdByEmail(string email)
        {
            return _internalDBContext.RegisteredUsers.Single(x => x.Email == email).Id.ToString();
        }
        public string GetUsernameFromIdentity(ClaimsIdentity identity)
        {
            IEnumerable<Claim> claims = identity.Claims;
            return claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();
        }

    }
}
