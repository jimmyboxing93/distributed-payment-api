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
