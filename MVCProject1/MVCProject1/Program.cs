using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MVCProject1.Models;
using System;

namespace MVCProject1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("!!! DOCKER IS STARTING THE APP !!!");
            
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
                    
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("SUCCESS: Database connection established!");
                    Console.WriteLine("---------------------------------");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"DATABASE ERROR: {ex.Message}");
                    if (ex.InnerException != null) 
                        Console.WriteLine($"DETAILS: {ex.InnerException.Message}");
                    Console.WriteLine("---------------------------------");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
