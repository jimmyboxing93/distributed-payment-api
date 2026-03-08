using Microsoft.Extensions.Hosting;
using ViewApi.Data;


namespace ViewApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var host = CreateHostBuilder(args).Build();

			
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var context = services.GetRequiredService<SeniorDbContext>();
					context.Database.EnsureCreated();
					Console.WriteLine("UI IDENTITY DB: Success!");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"UI IDENTITY DB ERROR: {ex.Message}");
				}
			}

			host.Run();
		}
        

        public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					
					config.AddEnvironmentVariables();
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
