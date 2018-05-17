using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AdvertisingAgency.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvertisingAgency.DbCacheMiddleware;
using Microsoft.AspNetCore.Identity;
using AdvertisingAgency.Models;

namespace AdvertisingAgency
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //string connection = Configuration.GetConnectionString("SQLConnection");
            //services.AddDbContext<adAgencyContext>(options => options.UseSqlServer(connection));
            services.AddTransient<adAgencyContext>();

            string connection = Configuration.GetConnectionString("SQLUserConnection");
            services.AddDbContext<AccountContext>(options => options.UseSqlServer(connection));
            services.AddIdentity < User, IdentityRole >(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<AccountContext>()
               .AddDefaultTokenProviders();

        services.AddSession();
            services.AddMvc().AddSessionStateTempDataProvider();

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Caching",
                   new CacheProfile()
                   {
                       Duration = 2 * 16 + 240,
                       Location = ResponseCacheLocation.Any
                   });
                options.CacheProfiles.Add("NoCaching",
                   new CacheProfile()
                   {
                       Location = ResponseCacheLocation.None,
                       NoStore = true
                   });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession();
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
