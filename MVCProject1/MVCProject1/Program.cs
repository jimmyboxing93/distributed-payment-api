using Microsoft.AspNetCore;
using MVCProject1.Models;
using Serilog;


namespace MVCProject1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/payement-api-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Docker is starting the app");

                var host = CreateWebHostBuilder(args).Build();


                // This block runs BEFORE the app starts listening for web requests
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<UserContext>();
                        // This forces a connection to SQL Server immediately
                        context.Database.EnsureCreated();
                        Log.Information("Database connection verified successfully.");

                    }
                    catch (Exception ex)
                    {

                        Log.Fatal(ex, "Database error: Could not ensure database creation.");

                    }
                }
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The application failed to start correctly.");
            }
            finally 
            {
				// Ensures all logs are written before the app closes

				Log.CloseAndFlush();
            }
            
           

            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
			//This tells the web host to use Serilog
				.ConfigureServices(services => services.AddSerilog())
				.UseStartup<Startup>();
    }
}
