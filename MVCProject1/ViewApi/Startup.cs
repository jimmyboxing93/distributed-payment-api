using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ViewApi.Data;
using ViewApi.Helper;

namespace ViewApi
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
			var builder = new ConfigurationBuilder()
		    .SetBasePath(env.ContentRootPath)
		    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
		    .AddEnvironmentVariables(); 
			Configuration = builder.Build();
		}

        public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			// 2. Register PaymentAPI helper as a Singleton
			services.AddSingleton<PaymentAPI>();

			var connectionString = Configuration.GetConnectionString("DefaultConnection");

			// Ensure 'using Microsoft.EntityFrameworkCore;' is at the top
			services.AddDbContext<SeniorDbContext>(options =>
				options.UseSqlServer(connectionString));

			services.AddIdentity<IdentityUser, IdentityRole>()
					.AddEntityFrameworkStores<SeniorDbContext>();

			services.AddDataProtection()
		            .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/root/.aspnet/DataProtection-Keys"));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Login}/{id?}");
            });
        }
    }
}
