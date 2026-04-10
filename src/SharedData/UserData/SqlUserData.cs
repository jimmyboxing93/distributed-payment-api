using SharedData.Models;
using SharedData.Data;
using SharedData.Interfaces;
using SharedData.UserData.Interfaces;

namespace SharedData.UserData
{
    public class SqlUserData : IUserInfo, IBankingReadService
    {
		private readonly SeniorDbContext _userContext;
		public SqlUserData(SeniorDbContext userContext)
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
                ExistingUser.PasswordHash = userInfo.PasswordHash;
                ExistingUser.FirstName = userInfo.FirstName;
                ExistingUser.LastName = userInfo.LastName;
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