using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Xamarinme;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using WebHostXam.Managers;

namespace WebHostXam.KestrelWebHost
{
    public class Program
    {
        public static Task Main(WebHostParameters webHostParameters)
        {
            var webHost = new WebHostBuilder()
                
                .ConfigureAppConfiguration((config) =>
                {
                    config.AddEmbeddedResource(
                        new EmbeddedResourceConfigurationOptions
                        {
                            Assembly = Assembly.GetExecutingAssembly(),
                            Prefix = "WebHostXam"
                        });
                    
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IHostLifetime, ConsoleLifetimePatch>();
                    services.AddDirectoryBrowser();

                })
               
                .UseKestrel(options =>
                {
                    options.Listen(webHostParameters.ServerIpEndpoint);
                    options.Limits.MaxRequestBodySize = 9 * 1024 * 1024;
                    options.Limits.MaxResponseBufferSize = 9 * 1024 * 1024;


                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
            

            App.Host = webHost;

            return webHost.RunPatchedAsync();
        }
    }
}