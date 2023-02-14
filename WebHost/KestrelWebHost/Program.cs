using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarinme;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using WebHost.Managers;

namespace WebHost.KestrelWebHost;

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
                        Prefix = "DemoApp.WebHost"
                    });
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IHostLifetime, ConsoleLifetimePatch>();
                services.AddSingleton<IReceiptManager, ReceiptManager>();
            })
            .UseKestrel(options =>
            {
                options.Listen(webHostParameters.ServerIpEndpoint);
            })
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .Build();

        App.Host = webHost;

        return webHost.RunPatchedAsync();
    } 
}