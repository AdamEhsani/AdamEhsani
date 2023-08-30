namespace CrmWeb.Pages.Clients
{
    public class Orders
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string? Driver { get; set; }
        public DateTime OrderDate { get; set; }
        public string TotalPrice { get; set; }
    }
}
