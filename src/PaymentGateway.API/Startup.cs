using Microsoft.EntityFrameworkCore;
using PaymentGateway.API.Models;
using SharedData.UserData;
using PaymentGateway.API.Middleware;
using SharedData.Interfaces;

namespace PaymentGateway.API
{
    // Responsible for using MVC middleware and responds to all HTTP requests. 
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                // Allows static files default to wwwroot directory
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

			services.AddMvc(options => options.EnableEndpointRouting = false);

			// Dynamic connection from Configuration
			services.AddDbContextPool<UserContext>(opt =>
				opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddScoped<IUserInfo, SqlUserData>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default",
                    template:
                    "{controller=Home}/{action=Payment}/{id?}");
            });
            // Used to access static files such as js. IF omitted, will return 404 error.
            app.UseStaticFiles();
        }
    }
}
