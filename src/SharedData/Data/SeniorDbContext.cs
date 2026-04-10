using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedData.Models;

namespace SharedData.Data;

public class SeniorDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
	public SeniorDbContext(DbContextOptions<SeniorDbContext> options) : base(options) { }

	// 1. ADD THE DBSET FOR YOUR FINANCIAL DATA
	public DbSet<UserInfo> CreditCardInfo { get; set; }
	public DbSet<ChatSession> ChatSessions { get; set; }
	public DbSet<ChatMessageRecord> ChatMessages { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// 2. CONFIGURE YOUR FINANCIAL DATA
		modelBuilder.Entity<UserInfo>(entity =>
		{
			// Set the precision for money
			entity.Property(p => p.amount)
				  .HasColumnType("decimal(18,2)");

			// Map the table name explicitly if needed
			entity.ToTable("UserInfo");
		});

		// 3. EXISTING CHAT CONFIG (Keep this exactly as is)
		modelBuilder.Entity<ChatMessageRecord>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Id).ValueGeneratedNever();
			entity.Property(e => e.ChatSessionId).IsRequired();
		});

		modelBuilder.Entity<ChatSession>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Id).ValueGeneratedNever();
		});
	}
}