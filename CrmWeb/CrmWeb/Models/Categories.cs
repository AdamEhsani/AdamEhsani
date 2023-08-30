namespace CrmWeb.Models
{
    public class Categories
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string Category { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
