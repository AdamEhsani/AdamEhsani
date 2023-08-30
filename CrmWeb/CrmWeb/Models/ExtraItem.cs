namespace CrmWeb.Models
{
    public class ExtraItem
    {
        public string Name { get; set; } = null!;
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string? PriceS { get; set; }
        public string? PriceM { get; set; }
        public string? PriceL { get; set; }
        public string? PriceXL { get; set; }
        public string? PriceXXL { get; set; }
    }
}
