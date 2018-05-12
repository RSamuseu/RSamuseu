using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AdAgencyWebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace AdAgencyWebAPI
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
            services.AddDbContext<adAgencyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SQLConnection")));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, adAgencyContext context)
        {
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseMvc();
        }
    }
}
