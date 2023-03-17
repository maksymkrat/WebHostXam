namespace WebHostXam.Models
{
    public class TabletInfoModel
    {
        public string TabletIP { get; set; }
        public string TabletMAC { get; set; }

        public TabletInfoModel(string tabletIp, string tabletMac)
        {
            TabletIP = tabletIp;
            TabletMAC = tabletMac;
        }
    }
}