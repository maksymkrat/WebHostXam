using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace WebHostXam
{
    public class NetworkHelper
    {
        public static IPAddress GetIpAddress()
        {
            // Up, Ethernet and IP4.
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(network => network.OperationalStatus == OperationalStatus.Up &&
                                  (network.NetworkInterfaceType == NetworkInterfaceType.Ethernet || 
                                   network.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) &&
                                  network.GetIPProperties().UnicastAddresses
                                      .Where(ai => ai.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                      .Count() > 0)
                .ToArray();
            if (networkInterfaces.Count() == 0)
                return null;

            var addressInfos = networkInterfaces[0].GetIPProperties().UnicastAddresses
                .Where(ai => ai.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                             !ai.Address.ToString().StartsWith("169"))
                .ToArray();
            if (addressInfos.Count() == 0)
                return null;

            return addressInfos[0].Address;
        }
    }
}