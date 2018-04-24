using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using advertisingAgency.Models;
using Microsoft.EntityFrameworkCore;

namespace advertisingAgency
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
            string connection = Configuration.GetConnectionString("SQLConnection");
            services.AddDbContext<adAgencyContext>(options => options.UseSqlServer(connection));

            services.AddTransient<adAgencyContext>();
            services.AddMemoryCache();
            services.AddResponseCaching();
            services.AddDistributedMemoryCache();
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseLastElementCache();
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseResponseCaching();
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
