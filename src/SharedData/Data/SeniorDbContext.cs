using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedData.Models;


namespace SharedData.Data;

public class SeniorDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public SeniorDbContext(DbContextOptions<SeniorDbContext> options) : base(options)
    {
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// CRITICAL: Call base first so Identity roles are configured
		base.OnModelCreating(modelBuilder);

		// Force ChatMessages to use Guid/UniqueIdentifier
		modelBuilder.Entity<ChatMessageRecord>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Id)
				  .ValueGeneratedNever(); // We generate Guids in C#, not SQL

			entity.Property(e => e.ChatSessionId)
				  .IsRequired();
		});

		// Force ChatSessions to use Guid/UniqueIdentifier
		modelBuilder.Entity<ChatSession>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Id)
				  .ValueGeneratedNever();
		});
	}

	public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatMessageRecord> ChatMessages { get; set; }

 }

