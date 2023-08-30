namespace CrmWeb.Models
{
    public class Customer
    {
        public string PartnerId { get; set; } = null!;
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string PLZ { get; set; } = null!;

        public ICollection<Orders> Orders { get; set; } = null!;
    }
}
