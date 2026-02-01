using Microsoft.EntityFrameworkCore;

namespace MVCProject1.Models
{
   public class UserContext : DbContext
   {

       public UserContext(DbContextOptions<UserContext> options)
           : base(options) 
       {    
       }
        public DbSet<UserInfo> CreditCardInfo { get; set; }


    }
}