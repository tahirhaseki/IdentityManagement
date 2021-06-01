using IdentityManagement.Infrastructure.Extensions;
using IdentityManagement.Infrastructure.Persistance;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagement.Server
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
            services.AddDatabaseConfiguration(Configuration.GetConnectionString("DefaultConnection"))
                    .AddIdentityServiceConfig(Configuration)
                    .AddServices<AppUser>();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseIdentityServer();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name:"default",
                    template:"{controller}/{action}"
                );
            });

        }
    }
}
