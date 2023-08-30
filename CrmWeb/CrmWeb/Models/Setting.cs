namespace CrmWeb.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string? Name { get; set; }    
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PLZ { get; set; }
        public string? UstIdNr { get; set; }
        public string? DeliveryPrice { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }

    }
}
