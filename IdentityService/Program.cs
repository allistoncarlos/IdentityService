using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace IdentityService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseHealthChecks("/hc")
                .ConfigureAppConfiguration((builderContext, configurationBuilder) =>
                {
                    var configuration = configurationBuilder.Build();

                    if (builderContext.HostingEnvironment.IsDevelopment() && configuration["DOTNET_RUNNING_IN_CONTAINER"] != null && Convert.ToBoolean(configuration["DOTNET_RUNNING_IN_CONTAINER"]))
                        configurationBuilder.AddJsonFile(@"/app/UserSecrets/secrets.json"); ;
                })
                .UseStartup<Startup>()
                .Build();
    }
}
