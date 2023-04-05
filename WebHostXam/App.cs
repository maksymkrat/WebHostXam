using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using WebHostXam.KestrelWebHost;
using WebHostXam.Models;
using Xamarin.Essentials;

namespace WebHostXam
{
    public class App
    {
        public static IWebHost Host { get; set; }
        public static WebHostParameters WebHostParameters { get; set; } = new WebHostParameters();
        private const String DEVICE_IP = "device_ip";
        private const String NONE = "none";
        
        private readonly string ServerURL = "http://172.19.100.133:5555/InsertOrUpdateTabletIp";
        private readonly   HttpClient _httpClient= new HttpClient();

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

        private void InitServerIp()
        {
            try
            {
                //WebHostParameters.ServerIpEndpoint = new IPEndPoint(NetworkHelper.GetIpAddress(), 3555);
                WebHostParameters.ServerIpEndpoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 3555);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void ComparisonIp()
        {
            try
            {
                var localsavedIp = Preferences.Get(DEVICE_IP, NONE);
                var currentIp = WebHostParameters.ServerIpEndpoint.Address.ToString();
                
                if (localsavedIp.Equals(NONE))
                {
                    Preferences.Set(DEVICE_IP, currentIp);
                    SendIp(currentIp);

                }
                else
                {
                    if (!localsavedIp.Equals(currentIp))
                    {
                        Preferences.Set(DEVICE_IP, currentIp);
                        SendIp(currentIp);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        private async Task SendIp(string ip)
        {
            try
            {
                var mac = "73"; //hardcode
                var tabletInfo = new TabletInfoModel(ip, mac);
                var json = JsonConvert.SerializeObject(tabletInfo);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ServerURL, data);

            }
            catch (Exception e)
            {
                
                throw;
            }
        }
    }
}