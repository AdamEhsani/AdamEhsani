using com.sun.xml.@internal.bind.v2.runtime.unmarshaller;

namespace CrmWeb.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        public Orders Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}