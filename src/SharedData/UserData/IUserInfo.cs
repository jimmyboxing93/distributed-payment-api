using SharedData.Models;

namespace SharedData.Interfaces
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
