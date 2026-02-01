using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MVCProject1.Models;
using MVCProject1.UserData;
using Microsoft.Extensions.Hosting;
using MVCProject1.Middleware;

namespace MVCProject1
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

            services.AddMvc();
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddDbContextPool<UserContext>(opt => opt.UseSqlServer("server=S128105\\SQLEXPRESS;database=UserData;Trusted_Connection=true"));


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
