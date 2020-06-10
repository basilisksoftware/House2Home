using Microsoft.AspNetCore.Http;

namespace API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string FireLabels { get; set; }
        public string PictureUrls { get; set; }
        public bool Collect { get; set; }
    }
}