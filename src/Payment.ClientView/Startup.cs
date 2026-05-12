using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Payment.ClientView.Services;
using ViewApi.Data;

namespace ViewApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Combine your two calls into one. 
			// This is the modern place to add global filters if you remember them later!
			services.AddControllersWithViews();

            services.AddScoped<ITokenService, TokenService>();

			var connectionString = Configuration.GetConnectionString("DefaultConnection");

			// Ensure 'using Microsoft.EntityFrameworkCore;' is at the top
			services.AddDbContext<SeniorDbContext>(options =>
				options.UseSqlServer(connectionString));

			services.AddIdentity<IdentityUser, IdentityRole>()
					.AddEntityFrameworkStores<SeniorDbContext>();

            services.AddAuthentication(options =>
            {

                // Calling bearer token
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWT_KEY"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
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

				endpoints.MapControllers();
			});
        }
    }
}
