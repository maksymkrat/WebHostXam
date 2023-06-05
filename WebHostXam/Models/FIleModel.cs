using Microsoft.AspNetCore.Http;

namespace WebHostXam.Models
{
    public class FIleModel
    {
        public byte[] FileBytes { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}