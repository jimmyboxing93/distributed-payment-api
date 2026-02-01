using MVCProject1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVCProject1.UserData
{
    public class SqlUserData : IUserInfo
    {
        private UserContext _userContext;
        public SqlUserData(UserContext userContext)
        {
            _userContext = userContext;
        }

        public List<UserInfo> GetCreditCards()
        {
            return _userContext.CreditCardInfo.ToList();
        }

        public UserInfo GetCreditCard(Guid id)
        {
            var user = _userContext.CreditCardInfo.Find(id);
            return user;
        }

        public UserInfo AddCreditCard(UserInfo userInfo)
        {
            userInfo.UserID = Guid.NewGuid();
            _userContext.CreditCardInfo.Add(userInfo);
            _userContext.SaveChanges();
            return userInfo;
        }

        public UserInfo EditCreditCard(UserInfo userInfo)
        {
            var ExistingUser = _userContext.CreditCardInfo.Find(userInfo.UserID);

            if (ExistingUser != null)
            {
                ExistingUser.password = userInfo.password;
                ExistingUser.firstName = userInfo.firstName;
                ExistingUser.lastName = userInfo.lastName;
                ExistingUser.creditCardNumber = userInfo.creditCardNumber;
                ExistingUser.ccv = userInfo.ccv;
                ExistingUser.expirationDate = userInfo.expirationDate;
                ExistingUser.amount = userInfo.amount;
                ExistingUser.Name = userInfo.Name; 
                _userContext.CreditCardInfo.Update(ExistingUser);
                _userContext.SaveChanges();
            }
            return userInfo;
        }

        public void DeleteCreditCard(UserInfo userInfo)
        {
           _userContext.CreditCardInfo.Remove(userInfo);
            _userContext.SaveChanges();
        }
    }
}