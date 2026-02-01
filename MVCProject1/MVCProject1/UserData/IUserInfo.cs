using System;
using System.Collections.Generic;
using MVCProject1.Models;

namespace MVCProject1.UserData
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
