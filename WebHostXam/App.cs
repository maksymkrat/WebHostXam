using System;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using WebHostXam.KestrelWebHost;
using Xamarin.Essentials;

namespace WebHostXam
{
    public class App
    {
        public static IWebHost Host { get; set; }
        public static WebHostParameters WebHostParameters { get; set; } = new WebHostParameters();
        private const String DEVICE_IP = "device_ip";
        private const String NONE = "none";

        public App()
        {
            WebHostParameters.ServerIpEndpoint = new IPEndPoint(NetworkHelper.GetIpAddress(), 3555);
            ComparisonIp();

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

        public void ComparisonIp()
        {
            var localIp = Preferences.Get(DEVICE_IP, NONE);
            var currentIp = WebHostParameters.ServerIpEndpoint.Address.ToString();

            if (localIp.Equals(NONE))
            {
                Preferences.Set(DEVICE_IP, currentIp);
                //maybe send also
            }
            else
            {
                if (!localIp.Equals(currentIp))
                {
                    // send ip on server
                }
            }
        }
    }
}