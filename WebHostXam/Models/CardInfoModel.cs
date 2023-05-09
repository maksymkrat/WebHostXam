namespace WebHostXam.Models
{
    public class CardInfoModel
    {
        public string FourLastPhoneDigits { get; set; }
        public string CardNumber { get; set; }
        public float WasUAN { get; set; }
        public float AccruedUAN { get; set; }
        public float WithdrawnUAN { get; set; }
        public float RemainderUAN { get; set; }
    }
}