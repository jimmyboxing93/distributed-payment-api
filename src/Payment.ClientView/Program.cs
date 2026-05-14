using SharedData.Data;

namespace ViewApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());

			// Walk up the tree until we find the .env file or hit the drive root
			while (currentDir != null && !File.Exists(Path.Combine(currentDir.FullName, ".env")))
			{
				currentDir = currentDir.Parent;
			}

			if (currentDir != null)
			{
				// Found it! Load the .env from the solution root
				DotNetEnv.Env.Load(Path.Combine(currentDir.FullName, ".env"));
			}


			var host = CreateHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope()) 
			{
				var service = scope.ServiceProvider;
				try
				{
					var context = service.GetRequiredService<SeniorDbContext>();
					context.Database.EnsureCreated();
				}
				catch (Exception ex) 
				{
					Console.WriteLine($"An error occurred creating the DB: {ex.Message}");
				}
			}

			CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
