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
            InitServerIp();
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

        public void InitServerIp()
        {
            try
            {
                WebHostParameters.ServerIpEndpoint = new IPEndPoint(NetworkHelper.GetIpAddress(), 3555);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
        }

        public void ComparisonIp()
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}