using System;
using System.Collections.Generic;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.UserData
{
    public interface IUserInfo
    {
        List<UserInfo> GetCreditCards();

        UserInfo GetCreditCard(Guid id);

        UserInfo AddCreditCard(UserInfo userInfo);

        UserInfo EditCreditCard(UserInfo userInfo);

        void DeleteCreditCard(UserInfo userInfo);
    }
}
