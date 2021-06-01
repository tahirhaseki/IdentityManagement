using IdentityManagement.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Services;
using IdentityManagement.Infrastructure.Services;

namespace IdentityManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityServiceConfig(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppIdentityDbContext>()
              .AddDefaultTokenProviders();

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                        options.EnableTokenCleanup = true;
                    })
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                    })
                    .AddAspNetIdentity<AppUser>();

            return services;
        }

        public static IServiceCollection AddServices<TUser>(this IServiceCollection services) where TUser : IdentityUser<int>
        {
            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
            return services;
        }

        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<AppPersistedGrantDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<AppConfigurationDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
