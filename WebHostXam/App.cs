using System;
using System.Collections.Generic;
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
using WebHostXam.Managers;
using WebHostXam.Models;
using Xamarin.Essentials;

namespace WebHostXam
{
    public class App
    {
        public static IWebHost Host { get; set; }
        public static WebHostParameters WebHostParameters { get; set; } = new WebHostParameters();
        private const String DEVICE_IP = "device_ip";
        private const String FILES_DOWNLOADED = "files_sownloaded";
        private const String NONE = "none";
        private const string ServerURLForIp = "http://193.193.222.87:5600/InsertOrUpdateTabletIp"; //prod
       private const string ServerURLForMedia = "http://193.193.222.87:5600/GetMediaFiles"; // server prod
        //private const string ServerURLForMedia = "http://192.168.0.102:5555/GetMediaFiles"; // local
        private const string AccessData = "Hilgrup1289";
        private readonly HttpClient _httpClient;
        private readonly WindowViewManager _viewManager;
        public App()
        {
            _viewManager = WindowViewManager.GetInstance();
            _httpClient = new HttpClient();
           

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
            
            InitServerIp();
            _viewManager.LoadHTMLForView();
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

        public void ComparisonIp()
        {
            try
            {
                var localsavedIp = Preferences.Get(DEVICE_IP, NONE);
                var currentIp = NetworkHelper.GetIpAddress().ToString();
                
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
                var response = await _httpClient.PostAsync(ServerURLForIp, data);

            }
            catch (Exception e)
            {
                
                throw;
            }
        }

        public async Task DownloadMediaContent()
        {
            var ip = NetworkHelper.GetIpAddress(); //need delete
            Preferences.Set(FILES_DOWNLOADED, "false");
            try
            {
                var data = new StringContent($"\"{AccessData}\"", Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ServerURLForMedia, data);
                var str = await response.Content.ReadAsStringAsync();
               
                var files =  JsonConvert.DeserializeObject<List<FIleModel>>(str);
                foreach (var file in files)
                {
                    if (File.Exists($"/storage/emulated/0/Download/{file.FileName}{file.FileExtension}"))
                    {
                        File.Delete($"/storage/emulated/0/Download/{file.FileName}{file.FileExtension}");
                    }
                    Byte[] bytes = file.FileBytes;
                    FileInfo fileInfo = new FileInfo($"/storage/emulated/0/Download/{file.FileName}{file.FileExtension}");
                    using (Stream stream = fileInfo.OpenWrite())
                    {
                        await stream.WriteAsync(bytes,0 ,bytes.Length);
                        stream.Close();
                    }
                }
                Preferences.Set(FILES_DOWNLOADED, "true");
                _viewManager.FilesDownloaded();
                
            }
            catch (Exception e)
            {
               
                throw;
            }
        }
        
    }
}