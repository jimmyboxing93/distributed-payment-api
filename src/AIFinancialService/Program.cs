using AIFinancialService.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.SemanticKernel;
using SharedData.Data;

try
{
	// 1. DYNAMIC ENVIRONMENT LOADING (The Walker)
	var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
	string? envPath = null;

	while (currentDir != null)
	{
		var potentialPath = Path.Combine(currentDir.FullName, ".env");
		if (File.Exists(potentialPath))
		{
			envPath = potentialPath;
			break;
		}
		currentDir = currentDir.Parent;
	}

	if (envPath != null)
	{
		DotNetEnv.Env.Load(envPath);
		Console.WriteLine($"SUCCESS: Environment loaded from: {envPath}");
	}
	else
	{
		Console.WriteLine("WARNING: No .env file found in the directory tree.");
	}

	Console.WriteLine("Starting AIFinancialService...");
	var builder = WebApplication.CreateBuilder(args);

	// 2. CONFIGURATION & SECRETS
	// This allows variables to be pulled from .env OR System Environment Variables
	builder.Configuration.AddEnvironmentVariables();

	var connString = builder.Configuration.GetConnectionString("DefaultConnection");
	var apiKey = System.Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? builder.Configuration["GEMINI_API_KEY"];
	var modelId = builder.Configuration["GEMINI_MODEL_ID"] ?? "gemini-2.5-flash";
	Console.WriteLine($"DEBUG: Raw Model ID from Config: {builder.Configuration["GEMINI_MODEL_ID"]}");
	Console.WriteLine($"DEBUG: Final Model ID being used: {modelId}");

	Console.WriteLine($"Connection String found: {!string.IsNullOrEmpty(connString)}");
	Console.WriteLine($"API Key loaded: {!string.IsNullOrEmpty(apiKey)}");

	// 3. SERVICE REGISTRATION
	// Prevent crash if API Key is missing
	if (!string.IsNullOrEmpty(apiKey))
	{
		builder.Services.AddGoogleAIGeminiChatCompletion(modelId, apiKey);
		builder.Services.AddScoped<IFinanceAgentService, FinanceAgentSerivce>();
	}
	else
	{
		Console.WriteLine("!!! CRITICAL: API Key missing. Semantic Kernel not registered.");
	}

	builder.Services.AddDbContext<SeniorDbContext>(options =>
		options.UseSqlServer(connString));

	builder.Services.AddTransient(sp => new Kernel(sp));
	builder.Services.AddScoped<IChatHistoryService, ChatHistoryService>();
	builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.PropertyNamingPolicy = null;
		options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
	});


	// 4. BUILD & RUN
	var app = builder.Build();

	Console.WriteLine("App Build Successful. Mapping routes...");
	app.MapControllers();

	Console.WriteLine("Everything ready. Running app...");

	using (var scope = app.Services.CreateScope()) 
	{
		var context = scope.ServiceProvider.GetRequiredService<SeniorDbContext>();
		var databaseCreator = context.Database.GetService<IRelationalDatabaseCreator>();

		try
		{

			databaseCreator.CreateTables();
			Console.WriteLine("Tables created successfully!");
		}
		catch (Exception) 
		{
			Console.WriteLine("Tables already exist or were already handled.");
		}
	}

	app.Run();
}
catch (Exception ex)
{
	Console.WriteLine("!!! CRITICAL STARTUP ERROR !!!");
	Console.WriteLine(ex.Message);
	Console.WriteLine(ex.StackTrace);
}