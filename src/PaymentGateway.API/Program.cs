using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.API.Models;
using System;
using System.IO;

namespace PaymentGateway.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					Console.WriteLine("!!! DOCKER IS USING THE CORRECT FILE !!!");
					var context = services.GetRequiredService<UserContext>();
					context.Database.EnsureCreated();
					Console.WriteLine("---------------------------------");
					Console.WriteLine("DATABASE CHECK COMPLETE: Tables Ready!");
					Console.WriteLine("---------------------------------");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"DATABASE ERROR: {ex.Message}");
				}
			}

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}