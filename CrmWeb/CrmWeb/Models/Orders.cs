namespace CrmWeb.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string? Driver { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        public Customer Customer { get; set; } = null!;
        public ICollection<OrderDetail> OrderDetails { get; set; } = null!;
    }
}