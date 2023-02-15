namespace WebHostXam.Models
{
    public class ReceiptItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "from web host";
        public string Description { get; set; }
        public float Price { get; set; }
    }
}