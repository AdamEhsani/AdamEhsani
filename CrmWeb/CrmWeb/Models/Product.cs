namespace CrmWeb.Models
{
    public class Product
    {
        public string PartnerId { get; set; } = null!;
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int PriceS { get; set; } = 0;
        public int PriceM { get; set; } = 0;
        public int PriceL { get; set; } = 0;
        public int PriceXL { get; set; } = 0;
        public int PriceXXL { get; set; } = 0;
        public string Category { get; set; } = null!;
    }
}
