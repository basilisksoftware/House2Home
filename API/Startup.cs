using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        internal static IConfiguration Configuration { get; private set; }


        // Use MySQL in prod
        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            ConfigureServices(services);
        }

        // // Use SQLite in dev
        // public void ConfigureDevelopmentServices(IServiceCollection services)
        // {
        //     services.AddDbContext<DataContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
        //     ConfigureServices(services);
        // }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddControllers();

            // Add CORS support
            services.AddCors();

            // Add the Authorisation class as a scoped service so it can be injected into other classes
            services.AddScoped<Authorisation>();

            // Add the authentication scheme (JWT). This checks that the token is actually valid
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            // }

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            // Authentication must be before authorisation!
            app.UseAuthentication();

            // Use authorisation 
            app.UseAuthorization();

            // Set the application to use the default files and filepaths for static files (e.g. index.html)
            app.UseDefaultFiles();

            // Add the functionality to actually serve static files from the above filepaths
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Fall back to Angular routing when an endpoint is non-API
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
