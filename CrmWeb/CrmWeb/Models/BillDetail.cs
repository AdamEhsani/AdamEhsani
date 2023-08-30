namespace CrmWeb.Models
{
    public class BillDetail
    {
        public int Id { get; set; } 
        public string? Product { get; set; }
        public int Price { get; set; }
        public string? ExtraItems { get; set; }
        public int OrderId { get; set; }

        public Orders Order { get; set; } = null!;
        public Product Products { get; set; } = null!;

    }
}
