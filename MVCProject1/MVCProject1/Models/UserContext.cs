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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserInfo>()
				.Property(u => u.amount)
				.HasColumnType("decimal(18,2)");
		}


	}
}