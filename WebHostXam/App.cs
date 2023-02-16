using System;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using WebHostXam.KestrelWebHost;

namespace WebHostXam
{
    public class App
    {
        public static IWebHost Host { get; set; }
        public static WebHostParameters WebHostParameters { get; set; } = new WebHostParameters();

        public App()
        {
            WebHostParameters.ServerIpEndpoint = new IPEndPoint(NetworkHelper.GetIpAddress(), 3555);
        

            new Thread(async () =>
            {
                try
                {
                    await Program.Main(WebHostParameters);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"######## EXCEPTION: {ex.Message}");
                }
            }).Start();
        }
    }
}