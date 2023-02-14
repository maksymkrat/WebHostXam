using System.Net;
using Microsoft.AspNetCore.Hosting;
using WebHost.KestrelWebHost;

namespace WebHost;

public class App
{
    public static IWebHost Host { get; set; }
    public static WebHostParameters WebHostParameters { get; set; } = new WebHostParameters();

    public App()
    {
        WebHostParameters.ServerIpEndpoint = new IPEndPoint(NetworkHelper.GetIpAddress(), 5000);
        

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