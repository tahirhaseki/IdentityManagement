using IdentityManagement.Infrastructure.Persistance;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IdentityManagement.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var secret = new Secret("secret".Sha256());
            Console.WriteLine(secret.Value);
            var host = CreateHostBuilder(args).Build();
            using(var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                SeedData.EnsureSeedData(scope.ServiceProvider);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://0.0.0.0:5001");
                });
    }
}
