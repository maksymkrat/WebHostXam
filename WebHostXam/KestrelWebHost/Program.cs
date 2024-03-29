﻿using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Xamarinme;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
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
                })
                
                .UseKestrel(options =>
                {
                    options.Listen(webHostParameters.ServerIpEndpoint);
                })

                .UseContentRoot(Directory.GetCurrentDirectory())
                //.UseContentRoot(Process.GetCurrentProcess().MainModule.FileName)
                // .UseContentRoot(Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments))
                //.UseContentRoot(@"/storage/emulated/0/Download/")
                .UseStartup<Startup>()
                
                .Build();

            App.Host = webHost;

            return webHost.RunPatchedAsync();
        }
    }
}