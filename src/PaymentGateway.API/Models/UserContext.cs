using Microsoft.EntityFrameworkCore;
using SharedData.Models;

namespace PaymentGateway.API.Models
{
   public class UserContext : DbContext
   {

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// This tells SQL Server: "Use Decimal with 18 digits total, 2 after the dot"
			modelBuilder.Entity<UserInfo>()
				.Property(p => p.amount)
				.HasColumnType("decimal(18,2)");
		}

		public UserContext(DbContextOptions<UserContext> options)
           : base(options) 
       {    
       }
        public DbSet<UserInfo> CreditCardInfo { get; set; }


    }
}