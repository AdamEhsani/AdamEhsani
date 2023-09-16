using CrmWeb.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CrmWeb.Pages.Clients
{
    public class CustomersModel : PageModel
    {
        public List<CustomerInfo> Customers = new List<CustomerInfo>();
        public void OnGet()
        {
            DbAddress Db = new DbAddress();
            var partnerId = Request.Cookies["PartnerId"];

            using (SqlConnection connection = new SqlConnection(Db.DB()))
            {
                connection.Open();
                String sql = "SELECT * FROM Customer WHERE PartnerId = @partnerId;";

                using (SqlCommand Command = new SqlCommand(sql, connection))
                {
                    Command.Parameters.AddWithValue("@partnerId", partnerId);

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            CustomerInfo customer = new CustomerInfo();
                            customer.Id = Reader.GetInt32(0);
                            customer.Name = Reader.GetString(1);
                            customer.Phone = Reader.GetString(2);
                            customer.Address = Reader.GetString(3);
                            Customers.Add(customer);
                        }
                    }
                }
            }
        }
    }
}
